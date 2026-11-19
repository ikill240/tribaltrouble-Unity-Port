using System.Collections.Generic;
using TribalTrouble.Game;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TerrainType = com.oddlabs.tt.procedural.Landscape.TerrainType;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Everything around the island, as your Java renderer draws it: the sky dome with its two drifting cloud layers
    /// (scenery/Sky.java, GeneratorClouds), the sea textures and Gerstner waves (scenery/Water.java, GeneratorOcean),
    /// the planar reflection the water samples, the generator's distance fog (EXP2, thinning with camera height) and
    /// the sun (DefaultRenderer.sunDirection). The "Modern" graphics style adds real-time shadows, hemispheric light,
    /// SSAO, bloom, tone mapping and colour grading on top; "Classic" keeps the original look.
    /// </summary>
    public sealed class TTEnvironment : MonoBehaviour
    {
        // ---- Java constants
        static readonly Vector3 JavaSun = new Vector3(-0.9f, 0.7f, 0.7f);   // (x, y, z up)
        static readonly Color[] SkyInit = { new Color(0.90f, 0.95f, 1f), new Color(1.50f, 0.90f, 0.65f), new Color(1.35f, 0.85f, 0.6f) };   // (the Rift: a hot, hazy sky)
        static readonly Color[] SkyGradient = { new Color(0.75f, 0.825f, 0.95f), new Color(0.6f, 0.6f, 0.85f), new Color(0.85f, 0.7f, 0.6f) };
        static readonly Color[] SkyTexEnv = { new Color(0.95f, 0.975f, 1f), new Color(1f, 0.95f, 0.8f), new Color(1f, 0.85f, 0.7f) };
        const float SkyRadius = 6000f;
        const int SkySubdiv = 20, SkyGradientLength = 20, SkyDefaultColorPow = 8;
        static readonly float[] DepthScale = { 1f, 2f, 2.5f }, MinAlpha = { 0.05f, 0.05f, 0.1f }, MaxAlpha = { 0.6f, 0.6f, 0.75f };
        static readonly Vector2[] WaveDirs = { new Vector2(1f, 0f), new Vector2(0.707f, 0.707f), new Vector2(-0.5f, 0.866f) };

        public Camera cam;
        public Light sun;
        FaithfulTerrain _terrain;
        int _type;               // 0 native, 1 viking
        float _meters;
        Color _fogColor;
        float _fogDensity, _fogHeight;
        bool _fogOn;
        Material _skyMat, _waterMat;
        GameObject _sky;
        float _waveTime, _waveSpeed;
        Vector2 _scroll0, _scroll1;
        float _flowDir = Mathf.Deg2Rad * 45f, _flowSpeed = 0.001f, _targetFlowDir, _targetFlowSpeed = 0.001f, _flowTimer;
        Vector2 _innerOff, _outerOff;
        float _innerDir, _outerDir, _innerSpeed = 0.004f, _outerSpeed = 0.002f, _tInnerDir, _tOuterDir, _tInnerSpeed = 0.004f, _tOuterSpeed = 0.002f;
        float _innerTimer, _outerTimer, _innerInterval = 30f, _outerInterval = 40f;
        readonly System.Random _rng = new System.Random();
        // modern atmosphere (TTAtmosphere.cs / TTAtmosphere.hlsl)
        TTAtmosphere.Lighting _atm;
        // time of day (TTDayNight): Day is the original sun, Night and Cycle move the light and grade the picture
        Vector3 _toSun0;
        TTDayNight.State _tod;
        float _hour = 9f, _todTimer;
        int _todMode = -1;
        Volume _todVol;
        VolumeProfile _todProfile;
        /// <summary>The clock shown on screen while the day / night cycle runs (null otherwise).</summary>
        public static string ClockText;
        Vector2 _cloudOff;
        float _seaLevel;
        static Texture2D s_cloudNoise;

        // reflection
        Camera _reflCam;
        RenderTexture _reflRT;
        // post-processing
        Volume _volume;
        VolumeProfile _profile;
        bool _texturesSet;

        // ---- generated textures (once per terrain type, built off the main thread)
        sealed class Gen { public com.oddlabs.tt.render.Texture[] ocean, clouds; public Texture2D[] tex; public bool failed; }
        static readonly Gen[] s_gen = new Gen[3];   // native, viking, the Elemental Rift
        static readonly object s_lock = new object();

        /// <summary>Starts generating the sky and sea textures for a terrain type (call early; ~1 s in the background).</summary>
        public static void Prewarm(TerrainType type)
        {
            int t = type.ordinal();
            lock (s_lock)
            {
                if (s_gen[t] != null) return;
                s_gen[t] = new Gen();
            }
            var g = s_gen[t];
            var th = new System.Threading.Thread(() =>
            {
                try
                {
                    var ocean = new com.oddlabs.tt.procedural.GeneratorOcean(type).generate();
                    var clouds = new com.oddlabs.tt.procedural.GeneratorClouds(type).generate();
                    lock (s_lock) { g.ocean = ocean; g.clouds = clouds; }
                }
                catch (System.Exception e) { Debug.LogException(e); lock (s_lock) g.failed = true; }
            }, 4 * 1024 * 1024) { IsBackground = true, Name = "TT sky/sea textures" };
            th.Start();
        }

        static Texture2D[] Textures(int t)
        {
            var g = s_gen[t];
            if (g == null) return null;
            lock (s_lock)
            {
                if (g.tex != null) return g.tex;
                if (g.ocean == null || g.clouds == null) return null;
                g.tex = new[] { Upload(g.ocean[0], 4), Upload(g.ocean[1], 4), Upload(g.clouds[0], 1), Upload(g.clouds[1], 1) };
                g.ocean = null; g.clouds = null;
                return g.tex;
            }
        }

        static Texture2D Upload(com.oddlabs.tt.render.Texture t, int expected)
        {
            var img = t.images[0];
            int w = img.getWidth(), h = img.getHeight(), bpp = img.getPixelSize();
            var data = img.pixel_data.array();
            var px = new Color32[w * h];
            for (int i = 0; i < px.Length; i++)
            {
                if (bpp >= 4) px[i] = new Color32((byte)data[i * 4], (byte)data[i * 4 + 1], (byte)data[i * 4 + 2], (byte)data[i * 4 + 3]);
                else { byte v = (byte)data[i * bpp]; px[i] = new Color32(v, v, v, v); }
            }
            // linear = raw values: the shaders do the Java (gamma space) maths and convert at the end
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, true, true) { wrapMode = TextureWrapMode.Repeat, filterMode = FilterMode.Trilinear, anisoLevel = 4 };
            tex.SetPixels32(px);
            tex.Apply(true, true);
            return tex;
        }

        // ================================================================== setup

        public static TTEnvironment Create(Transform parent, Camera cam, Light sun, FaithfulTerrain terrain, TerrainType type, com.oddlabs.tt.resource.FogInfo fog)
        {
            var go = new GameObject("Environment");
            go.transform.SetParent(parent, false);
            var env = go.AddComponent<TTEnvironment>();
            env.Init(cam, sun, terrain, type, fog);
            return env;
        }

        void Init(Camera c, Light s, FaithfulTerrain terrain, TerrainType type, com.oddlabs.tt.resource.FogInfo fog)
        {
            cam = c; sun = s; _terrain = terrain;
            _type = type.ordinal();
            _meters = terrain.meters;
            Prewarm(type);

            // fog (Landscape.getFogInfo: EXP2, thinning as the camera rises to height_factor)
            var fc = fog?.getColor();
            _fogColor = fc != null ? new Color(fc.x(), fc.y(), fc.z()) : new Color(0.65f, 0.75f, 1f);
            _fogDensity = fog?.getDensity() ?? 0.0012f;
            _fogHeight = fog is com.oddlabs.tt.resource.DistanceFogInfo d ? d.getHeightFactor() : 0f;
            _fogOn = fog == null || fog.getMode() != com.oddlabs.tt.resource.FogInfo.Mode.NONE;

            // sun
            Vector3 toSun = new Vector3(JavaSun.x, JavaSun.z, JavaSun.y).normalized;
            if (sun != null)
            {
                sun.type = LightType.Directional;
                sun.transform.rotation = Quaternion.LookRotation(-toSun, Vector3.up);
                sun.color = Color.white;
            }
            Shader.SetGlobalVector("_TT_SunDir", toSun);
            _atm = TTAtmosphere.Compute(toSun);
            _toSun0 = toSun;
            _hour = GameSettings.cycleStartHour;
            _seaLevel = terrain.seaLevel;
            TTRealTextures.ApplyTerrain(terrain.terrainMaterial, _type);
            TTRealTextures.ApplyWater(terrain.waterMaterial);
            _cloudOff = new Vector2((float)_rng.NextDouble() * 9000f, (float)_rng.NextDouble() * 9000f);

            BuildSky();
            _waterMat = terrain.waterMaterial;
            _waveSpeed = _type == 1 ? 0.8f : 0.4f;
            if (_waterMat != null)
            {
                float amp = 0.15f * (_type == 1 ? 1.5f : 1f), steep = 0.5f * (_type == 1 ? 1.2f : 1f);
                float[] len = _type == 1 ? new[] { 50f, 28f, 14f } : new[] { 60f, 35f, 18f };
                float[] ampScale = { 1f, 0.53f, 0.27f };
                for (int i = 0; i < 3; i++)
                {
                    _waterMat.SetVector("_WaveDirLen" + i, new Vector4(WaveDirs[i].x, WaveDirs[i].y, len[i], 0));
                    _waterMat.SetVector("_WaveAmpSteep" + i, new Vector4(amp * ampScale[i], steep, 0, 0));
                }
                _waterMat.SetFloat("_DepthScale", DepthScale[_type]);
                _waterMat.SetFloat("_MinAlpha", MinAlpha[_type]);
                _waterMat.SetFloat("_MaxAlpha", MaxAlpha[_type]);
                _waterMat.SetFloat("_RepeatRate", 0.001f);
                // Landscape.NATIVE_SEA_BOTTOM_COLOR 0x374278 / VIKING_SEA_BOTTOM_COLOR 0x1A333D
                _waterMat.SetColor("_SeaBottom", _type == 2 ? new Color32(0x2A, 0x30, 0x26, 255) : _type == 1 ? new Color32(0x1A, 0x33, 0x3D, 255) : new Color32(0x37, 0x42, 0x78, 255));
                // modern: clear tropical water vs darker, greener northern sea (absorption per metre, scattering colour)
                // the Rift: murky, green-brown water
                _waterMat.SetVector("_Absorb", _type == 2 ? new Vector4(0.3f, 0.16f, 0.2f, 0) : _type == 1 ? new Vector4(0.36f, 0.11f, 0.095f, 0) : new Vector4(0.42f, 0.075f, 0.055f, 0));
                _waterMat.SetColor("_Scatter", _type == 2 ? new Color(0.05f, 0.06f, 0.03f) : _type == 1 ? new Color(0.012f, 0.075f, 0.08f) : new Color(0.0f, 0.11f, 0.15f));
            }
            _tInnerDir = _innerDir = (float)(_rng.NextDouble() * Mathf.PI * 2);
            _tOuterDir = _outerDir = _innerDir + 0.3f;
            Shader.SetGlobalVector("_TT_Wind", new Vector4(Mathf.Cos(_innerDir), Mathf.Sin(_innerDir), 1f, 0f));   // foliage sways with the clouds
            _targetFlowDir = _flowDir;

            if (cam != null) cam.farClipPlane = Mathf.Max(cam.farClipPlane, 9000f);
            ApplySettings();
            GameSettings.Changed += ApplySettings;
            RenderPipelineManager.beginCameraRendering += BeginCamera;
            RenderPipelineManager.endCameraRendering += EndCamera;
        }

        void OnDestroy()
        {
            GameSettings.Changed -= ApplySettings;
            RenderPipelineManager.beginCameraRendering -= BeginCamera;
            RenderPipelineManager.endCameraRendering -= EndCamera;
            if (_reflRT != null) { _reflRT.Release(); Destroy(_reflRT); }
            if (_profile != null) Destroy(_profile);
            if (_a11yProfile != null) Destroy(_a11yProfile);
            if (_todProfile != null) Destroy(_todProfile);
            ClockText = null;
            if (_skyMat != null) Destroy(_skyMat);
        }

        void BuildSky()
        {
            _sky = new GameObject("Sky dome");
            _sky.transform.SetParent(transform, false);
            var mesh = new Mesh { name = "SkyDome" };
            var verts = new List<Vector3>();
            var cols = new List<Color>();
            var uv0 = new List<Vector2>();
            var uv1 = new List<Vector2>();
            var tris = new List<int>();
            // Sky.makeSkyVertices
            Color g = SkyGradient[_type];
            Color def = new Color(Mathf.Pow(g.r, SkyDefaultColorPow), Mathf.Pow(g.g, SkyDefaultColorPow), Mathf.Pow(g.b, SkyDefaultColorPow));
            var grad = new Color[SkyGradientLength];
            grad[0] = SkyInit[_type];
            for (int i = 1; i < SkyGradientLength; i++)
            {
                float a = (float)i / (SkyGradientLength - 1);
                grad[i] = new Color(a * def.r + (1 - a) * grad[i - 1].r * g.r, a * def.g + (1 - a) * grad[i - 1].g * g.g, a * def.b + (1 - a) * grad[i - 1].b * g.b);
            }
            float hInc = Mathf.PI / 2 / (SkySubdiv - 1), aInc = Mathf.PI * 2 / SkySubdiv, off = aInc / 2, start = -Mathf.PI / 4;
            float c = _meters / 2f;
            for (int i = 0; i < SkySubdiv - 1; i++)
            {
                float z = Mathf.Sin(hInc * i) * SkyRadius, r = Mathf.Cos(hInc * i) * SkyRadius;
                float hc = Mathf.Abs(z) < 250f ? SkyRadius / 250f : SkyRadius / z;
                for (int j = 0; j < SkySubdiv; j++)
                {
                    float x = Mathf.Cos(start + aInc * j + off * i) * r, y = Mathf.Sin(start + aInc * j + off * i) * r;
                    verts.Add(new Vector3(x + c, z, y + c));
                    uv0.Add(new Vector2(x * hc / (SkyRadius * 8f) + 0.5f, y * hc / (SkyRadius * 8f) + 0.5f));
                    uv1.Add(new Vector2(x * hc / (SkyRadius * 8f) + 0.5f, y * hc / (SkyRadius * 8f) + 0.5f));
                    cols.Add(i < SkyGradientLength ? grad[i] : def);
                }
            }
            int top = verts.Count;
            verts.Add(new Vector3(c, SkyRadius, c)); uv0.Add(new Vector2(0.5f, 0.5f)); uv1.Add(new Vector2(0.5f, 0.5f));
            cols.Add(SkySubdiv - 1 < SkyGradientLength ? grad[SkySubdiv - 1] : def);
            for (int i = 0; i < SkySubdiv - 2; i++)
                for (int j = 0; j < SkySubdiv; j++)
                {
                    int a0 = i * SkySubdiv + j, a1 = i * SkySubdiv + (j + 1) % SkySubdiv;
                    int b0 = (i + 1) * SkySubdiv + j, b1 = (i + 1) * SkySubdiv + (j + 1) % SkySubdiv;
                    tris.Add(a0); tris.Add(b0); tris.Add(a1);
                    tris.Add(a1); tris.Add(b0); tris.Add(b1);
                }
            int last = (SkySubdiv - 2) * SkySubdiv;
            for (int j = 0; j < SkySubdiv; j++) { tris.Add(last + j); tris.Add(top); tris.Add(last + (j + 1) % SkySubdiv); }
            // a skirt below the horizon in the fog colour, so a low camera never sees the background
            int skirt = verts.Count;
            for (int j = 0; j < SkySubdiv; j++)
            {
                float a = start + aInc * j;
                verts.Add(new Vector3(Mathf.Cos(a) * SkyRadius + c, -SkyRadius * 0.3f, Mathf.Sin(a) * SkyRadius + c));
                uv0.Add(Vector2.zero); uv1.Add(Vector2.zero); cols.Add(_fogColor);
            }
            for (int j = 0; j < SkySubdiv; j++)
            {
                int s0 = skirt + j, s1 = skirt + (j + 1) % SkySubdiv, r0 = j, r1 = (j + 1) % SkySubdiv;
                tris.Add(s0); tris.Add(r0); tris.Add(s1);
                tris.Add(s1); tris.Add(r0); tris.Add(r1);
            }
            mesh.SetVertices(verts); mesh.SetColors(cols); mesh.SetUVs(0, uv0); mesh.SetUVs(1, uv1); mesh.SetTriangles(tris, 0);
            mesh.bounds = new Bounds(new Vector3(c, 0, c), Vector3.one * SkyRadius * 2.5f);
            _sky.AddComponent<MeshFilter>().sharedMesh = mesh;
            var mr = _sky.AddComponent<MeshRenderer>();
            mr.shadowCastingMode = ShadowCastingMode.Off;
            mr.receiveShadows = false;
            var shader = Shader.Find("TribalTrouble/Sky");
            var res = Resources.Load<Material>("TTSky");
            _skyMat = res != null ? new Material(res) : new Material(shader);
            _skyMat.SetColor("_SkyColor", SkyTexEnv[_type]);
            mr.sharedMaterial = _skyMat;
        }

        // ================================================================== settings

        public void ApplySettings()
        {
            bool modern = GameSettings.graphicsStyle == 1;
            Shader.SetGlobalFloat("_TT_Modern", modern ? 1f : 0f);
            Shader.SetGlobalFloat("_TT_ShadowStrength", 0.45f);
            Shader.SetGlobalFloat("_TT_GroundExposure", modern ? 0.9f : 1.0f);   // keeps bright sand from clipping under the tone curve
            Shader.SetGlobalFloat("_TT_SkyExposure", TTAtmosphere.SkyExposure);
            // height fog: denser and lower over the northern seas
            Shader.SetGlobalVector("_TT_FogParams", _type == 2 ? new Vector4(0.0007f, 0.003f, _seaLevel, 0.95f) : _type == 1 ? new Vector4(0.00055f, 0.0035f, _seaLevel, 0.95f) : new Vector4(0.0003f, 0.003f, _seaLevel, 0.92f));
            if (s_cloudNoise == null) s_cloudNoise = Resources.Load<Texture2D>("TTReal/cloud_noise");
            if (s_cloudNoise != null) Shader.SetGlobalTexture("_TT_CloudNoise", s_cloudNoise);
            Shader.SetGlobalVector("_TT_CloudParams", new Vector4(_type == 1 ? 0.7f : 0.38f, _cloudOff.x, _cloudOff.y, modern && s_cloudNoise != null ? (_type == 1 ? 0.6f : 0.45f) : 0f));   // the north: heavy storm cover
            if (sun != null)
            {
                sun.shadows = modern && GameSettings.shadows ? LightShadows.Soft : LightShadows.None;
                sun.shadowStrength = 0.8f;
                sun.shadowBias = 0.05f;
                sun.shadowNormalBias = 0.4f;
                // the softest filtering: moving shadows (units, swaying trees) glide instead of hopping texel to texel
                var ld = sun.GetUniversalAdditionalLightData();
                if (ld != null) ld.softShadowQuality = SoftShadowQuality.High;
            }
            if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset urpS)
            {
                // four cascades fitted round what the camera looks at (UpdateShadowDistance), tight bounding spheres,
                // and (with the memory for it) a 8192 map: about twice the detail where the units are
                urpS.shadowCascadeCount = 4;
                urpS.conservativeEnclosingSphere = true;
                urpS.mainLightShadowmapResolution = SystemInfo.graphicsMemorySize >= 7000 ? 8192 : 4096;
            }
            _shadowDist = -1f; _focus = -1f;   // re-evaluated in LateUpdate (follows the camera, the setting is the minimum)
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.62f, 0.66f, 0.76f);
            RenderSettings.ambientEquatorColor = new Color(0.5f, 0.5f, 0.48f);
            RenderSettings.ambientGroundColor = new Color(0.34f, 0.30f, 0.26f);

            // modern shaders do their own height fog with sun in-scattering (TTApplyFog); Unity's fog is the classic one
            RenderSettings.fog = _fogOn && !modern;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            if (cam != null)
            {
                cam.clearFlags = CameraClearFlags.SolidColor;
                var data = cam.GetUniversalAdditionalCameraData();
                data.renderPostProcessing = (modern && GameSettings.postEffects) || AccessibilityOn || GameSettings.TimeOfDayNow != TTDayNight.Day;
                data.antialiasing = GameSettings.antiAliasing ? AntialiasingMode.SubpixelMorphologicalAntiAliasing : AntialiasingMode.None;
                data.antialiasingQuality = AntialiasingQuality.High;
                data.dithering = true;
                // the modern sea refracts the scene below it (opaque colour + depth copies)
                data.requiresColorOption = modern ? CameraOverrideOption.On : CameraOverrideOption.UsePipelineSettings;
                data.requiresDepthOption = CameraOverrideOption.On;
                cam.allowHDR = true;
            }
            SetupVolume(modern && GameSettings.postEffects);
            SetupAccessibility();
            SetupReflection(GameSettings.reflections);
            UpdateTimeOfDay(0f, true);
        }

        // ================================================================== time of day

        /// <summary>Advances the cycle and re-lights the scene when the hour moved enough (or on force).</summary>
        void UpdateTimeOfDay(float dt, bool force)
        {
            int mode = GameSettings.TimeOfDayNow;
            if (mode != _todMode)
            {
                if (mode == TTDayNight.Cycle && _todMode != -1) _hour = GameSettings.cycleStartHour;
                _todMode = mode; force = true;
            }
            if (mode == TTDayNight.Cycle) _hour = Mathf.Repeat(_hour + dt / TTDayNight.SecondsPerHour, 24f);
            ClockText = mode == TTDayNight.Cycle ? TTDayNight.Clock(_hour) : null;
            if (mode == TTDayNight.Day) { if (force) ApplyLighting(); return; }
            // the light's direction every frame: the shadows glide round with the sun instead of stepping twice a second
            // (the sky's colours are worked out every half second below - too slow a change to see steps in)
            if (mode == TTDayNight.Cycle && !force)
            {
                var dir = TTDayNight.LightDir(_hour);
                Shader.SetGlobalVector("_TT_SunDir", dir);
                if (sun != null) sun.transform.rotation = Quaternion.LookRotation(-dir, Vector3.up);
            }
            _todTimer -= dt;
            if (!force && _todTimer > 0f) return;
            _todTimer = 0.5f;   // the sun moves a quarter of a degree in that time
            _tod = TTDayNight.Evaluate(mode == TTDayNight.Night ? TTDayNight.NightHour : _hour);
            ApplyLighting();
        }

        void ApplyLighting()
        {
            bool modern = GameSettings.graphicsStyle == 1;
            bool tod = GameSettings.TimeOfDayNow != TTDayNight.Day;
            Vector3 toSun = tod ? _tod.lightDir : _toSun0;
            var atm = tod ? _tod.atm : _atm;
            Shader.SetGlobalVector("_TT_SunDir", toSun);
            // modern: sun, sky and haze from the physical sky model (TTAtmosphere), the same numbers the shaders use
            Shader.SetGlobalColor("_TT_AmbientSky", modern ? atm.ambientSky : new Color(0.62f, 0.66f, 0.76f));
            Shader.SetGlobalColor("_TT_AmbientGround", modern ? atm.ambientGround : new Color(0.34f, 0.30f, 0.26f));
            Shader.SetGlobalColor("_TT_SunColor", modern ? atm.sun : Color.white);
            Shader.SetGlobalColor("_TT_SkyZenith", atm.zenith);
            Shader.SetGlobalColor("_TT_SkyHorizon", atm.horizon);
            if (sun != null)
            {
                sun.transform.rotation = Quaternion.LookRotation(-toSun, Vector3.up);
                float sunMax = modern ? Mathf.Max(0.01f, atm.sun.maxColorComponent) : 1f;
                sun.color = modern ? atm.sun / sunMax : Color.white;
                sun.intensity = modern ? (atm.sun.maxColorComponent < 0.01f ? 0f : sunMax) : 1f;
            }
            Color fog = tod && modern ? _fogColor * _tod.fogScale : _fogColor;
            RenderSettings.fogColor = fog;
            Shader.SetGlobalColor("_TT_FogColor", fog);
            if (cam != null) cam.backgroundColor = modern ? atm.horizon : fog;
            SetupTimeVolume(tod, modern);
        }

        void SetupTimeVolume(bool on, bool modern)
        {
            if (!on) { if (_todVol != null) _todVol.enabled = false; return; }
            if (_todVol == null)
            {
                _todProfile = ScriptableObject.CreateInstance<VolumeProfile>();
                _todProfile.Add<ColorAdjustments>(true);
                var go = new GameObject("Time of day");
                go.transform.SetParent(transform, false);
                _todVol = go.AddComponent<Volume>();
                _todVol.isGlobal = true;
                _todVol.priority = 15;   // over the modern grading (its exposure is folded in below), under accessibility
                _todVol.sharedProfile = _todProfile;
            }
            _todVol.enabled = true;
            _todProfile.TryGet(out ColorAdjustments ca);
            if (modern)
            {
                ca.colorFilter.Override(Color.white);
                ca.postExposure.Override((GameSettings.postEffects ? -0.2f : 0f) + _tod.modernExposure);
                ca.saturation.Override(GameSettings.postEffects ? 8f - 22f * (1f - _tod.dayWeight) : -22f * (1f - _tod.dayWeight));
            }
            else
            {
                ca.colorFilter.Override(_tod.classicTint);
                ca.postExposure.Override(_tod.classicExposure);
                ca.saturation.Override(-30f * (1f - _tod.dayWeight));
            }
        }

        void SetupVolume(bool on)
        {
            if (!on) { if (_volume != null) _volume.enabled = false; return; }
            if (_volume == null)
            {
                _profile = ScriptableObject.CreateInstance<VolumeProfile>();
                var tone = _profile.Add<Tonemapping>(true);
                tone.mode.Override(TonemappingMode.ACES);   // filmic response for the HDR sky and sun
                var bloom = _profile.Add<Bloom>(true);
                bloom.threshold.Override(1.6f);
                bloom.intensity.Override(0.28f);
                bloom.scatter.Override(0.65f);
                var ca = _profile.Add<ColorAdjustments>(true);
                ca.contrast.Override(6f);
                ca.saturation.Override(8f);
                ca.postExposure.Override(-0.2f);   // keeps snow and sand from clipping under ACES
                var wb = _profile.Add<WhiteBalance>(true);
                wb.temperature.Override(0f);
                var vig = _profile.Add<Vignette>(true);
                vig.intensity.Override(0.22f);
                vig.smoothness.Override(0.45f);
                var go = new GameObject("Post-processing");
                go.transform.SetParent(transform, false);
                _volume = go.AddComponent<Volume>();
                _volume.isGlobal = true;
                _volume.priority = 10;
                _volume.sharedProfile = _profile;
            }
            _volume.enabled = true;
        }

        // ---- accessibility (Java PostProcessShader: high contrast, colour-vision correction)
        Volume _a11y;
        VolumeProfile _a11yProfile;
        static bool AccessibilityOn => GameSettings.highContrast || GameSettings.cvdMode > 0;

        void SetupAccessibility()
        {
            if (!AccessibilityOn) { if (_a11y != null) _a11y.enabled = false; return; }
            if (_a11y == null)
            {
                _a11yProfile = ScriptableObject.CreateInstance<VolumeProfile>();
                _a11yProfile.Add<ChannelMixer>(true);
                _a11yProfile.Add<ColorAdjustments>(true);
                var go = new GameObject("Accessibility");
                go.transform.SetParent(transform, false);
                _a11y = go.AddComponent<Volume>();
                _a11y.isGlobal = true;
                _a11y.priority = 20;
                _a11y.sharedProfile = _a11yProfile;
            }
            _a11y.enabled = true;
            _a11yProfile.TryGet(out ChannelMixer mix);
            _a11yProfile.TryGet(out ColorAdjustments ca);
            // high contrast: (c - .5) * (1 + k) + .5 blended by k, plus a little saturation
            float k = GameSettings.highContrast ? Mathf.Clamp01(GameSettings.contrastIntensity) : 0f;
            ca.contrast.Override(k * 70f);
            ca.saturation.Override(k * 25f);
            // daltonize is linear: out = M * c with M = I + C (I - LMS2RGB * Sim * RGB2LMS) * intensity
            var M = CvdMatrix(GameSettings.cvdMode, GameSettings.cvdIntensity);
            mix.redOutRedIn.Override(M[0, 0] * 100f); mix.redOutGreenIn.Override(M[0, 1] * 100f); mix.redOutBlueIn.Override(M[0, 2] * 100f);
            mix.greenOutRedIn.Override(M[1, 0] * 100f); mix.greenOutGreenIn.Override(M[1, 1] * 100f); mix.greenOutBlueIn.Override(M[1, 2] * 100f);
            mix.blueOutRedIn.Override(M[2, 0] * 100f); mix.blueOutGreenIn.Override(M[2, 1] * 100f); mix.blueOutBlueIn.Override(M[2, 2] * 100f);
        }

        static float[,] Mul(float[,] a, float[,] b)
        {
            var r = new float[3, 3];
            for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) for (int k = 0; k < 3; k++) r[i, j] += a[i, k] * b[k, j];
            return r;
        }

        static float[,] CvdMatrix(int mode, float intensity)
        {
            var I = new float[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            if (mode <= 0) return I;
            // the GLSL matrices are column-major: these are their row-major forms
            var rgb2lms = new float[,] { { 17.8824f, 43.5161f, 4.11935f }, { 3.45565f, 27.1554f, 3.86714f }, { 0.0299566f, 0.184309f, 1.46709f } };
            var lms2rgb = new float[,] { { 0.0809f, -0.1305f, 0.1167f }, { -0.0102f, 0.0540f, -0.1136f }, { -0.0003f, -0.0041f, 0.6935f } };
            float[,] sim = mode == 1 ? new float[,] { { 0, 2.02344f, -2.52581f }, { 0, 1, 0 }, { 0, 0, 1 } }
                         : mode == 2 ? new float[,] { { 1, 0, 0 }, { 0.494207f, 0, 1.24827f }, { 0, 0, 1 } }
                         : new float[,] { { 1, 0, 0 }, { 0, 1, 0 }, { -0.395913f, 0.801109f, 0 } };
            var S = Mul(lms2rgb, Mul(sim, rgb2lms));
            var E = new float[3, 3];
            for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) E[i, j] = I[i, j] - S[i, j];
            // error shifted to the channels the viewer can see
            var C = mode <= 2 ? new float[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0.7f, 0.7f, 0 } } : new float[,] { { 0, 0, 0.7f }, { 0, 0, 0.7f }, { 0, 0, 0 } };
            var CE = Mul(C, E);
            var M = new float[3, 3];
            for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) M[i, j] = I[i, j] + CE[i, j] * Mathf.Clamp01(intensity);
            return M;
        }

        void SetupReflection(bool on)
        {
            if (!on || cam == null)
            {
                if (_reflCam != null) _reflCam.enabled = false;
                if (_waterMat != null) _waterMat.SetFloat("_HasReflection", 0f);
                return;
            }
            if (_reflCam == null)
            {
                var go = new GameObject("Water reflection");
                go.transform.SetParent(transform, false);
                _reflCam = go.AddComponent<Camera>();
                _reflCam.cullingMask = ~((1 << 4) | (1 << 5));   // no water, no UI
                _reflCam.clearFlags = CameraClearFlags.SolidColor;
                var data = _reflCam.GetUniversalAdditionalCameraData();
                data.renderPostProcessing = false;
                data.renderShadows = false;
                data.requiresDepthOption = CameraOverrideOption.Off;
                data.requiresColorOption = CameraOverrideOption.Off;
            }
            _reflCam.enabled = true;
            if (_waterMat != null) _waterMat.SetFloat("_HasReflection", 1f);
        }

        void EnsureRT()
        {
            int w = Mathf.Max(64, cam.pixelWidth / 2), h = Mathf.Max(64, cam.pixelHeight / 2);
            if (_reflRT != null && _reflRT.width == w && _reflRT.height == h) return;
            if (_reflRT != null) { _reflRT.Release(); Destroy(_reflRT); }
            _reflRT = new RenderTexture(w, h, 24, RenderTextureFormat.DefaultHDR) { name = "TT water reflection" };
            _reflCam.targetTexture = _reflRT;
            if (_waterMat != null) _waterMat.SetTexture("_Reflection", _reflRT);
        }

        void BeginCamera(ScriptableRenderContext ctx, Camera c)
        {
            if (c == _reflCam) GL.invertCulling = true;
        }

        void EndCamera(ScriptableRenderContext ctx, Camera c)
        {
            if (c == _reflCam) GL.invertCulling = false;
        }

        void UpdateReflection()
        {
            if (_reflCam == null || !_reflCam.enabled || cam == null) return;
            EnsureRT();
            float sea = _terrain != null ? _terrain.seaLevel : 0f;
            _reflCam.fieldOfView = cam.fieldOfView;
            _reflCam.aspect = cam.aspect;
            _reflCam.nearClipPlane = cam.nearClipPlane;
            _reflCam.farClipPlane = cam.farClipPlane;
            _reflCam.backgroundColor = _fogColor;
            _reflCam.depth = cam.depth - 1;
            var plane = new Vector4(0, 1, 0, -sea);
            var reflect = ReflectionMatrix(plane);
            _reflCam.transform.position = reflect.MultiplyPoint(cam.transform.position);
            _reflCam.worldToCameraMatrix = cam.worldToCameraMatrix * reflect;
            // oblique near plane at the sea surface, so nothing under water shows in the reflection
            Vector4 clip = CameraSpacePlane(_reflCam, new Vector3(0, sea, 0), Vector3.up, 0.05f);
            _reflCam.projectionMatrix = cam.CalculateObliqueMatrix(clip);
        }

        static Matrix4x4 ReflectionMatrix(Vector4 p)
        {
            var m = Matrix4x4.identity;
            m.m00 = 1 - 2 * p.x * p.x; m.m01 = -2 * p.x * p.y; m.m02 = -2 * p.x * p.z; m.m03 = -2 * p.w * p.x;
            m.m10 = -2 * p.y * p.x; m.m11 = 1 - 2 * p.y * p.y; m.m12 = -2 * p.y * p.z; m.m13 = -2 * p.w * p.y;
            m.m20 = -2 * p.z * p.x; m.m21 = -2 * p.z * p.y; m.m22 = 1 - 2 * p.z * p.z; m.m23 = -2 * p.w * p.z;
            return m;
        }

        static Vector4 CameraSpacePlane(Camera c, Vector3 pos, Vector3 normal, float offset)
        {
            var m = c.worldToCameraMatrix;
            Vector3 cp = m.MultiplyPoint(pos + normal * offset);
            Vector3 cn = m.MultiplyVector(normal).normalized;
            return new Vector4(cn.x, cn.y, cn.z, -Vector3.Dot(cp, cn));
        }

        // ================================================================== per frame

        float _shadowDist = -1f, _focus = -1f;

        /// <summary>
        /// The RTS camera looks down from far away; shadows must reach the ground it looks at, so the shadow distance
        /// follows the distance to the focus point (the setting is the minimum).
        /// </summary>
        void UpdateShadowDistance()
        {
            if (cam == null || !(GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset urp)) return;
            var f = cam.transform.forward;
            float h = cam.transform.position.y - _seaLevelGuess;
            float focus = f.y < -0.05f ? h / -f.y : 400f;
            float want = Mathf.Clamp(focus * 1.7f + 60f, GameSettings.shadowDistance, Mathf.Max(GameSettings.shadowDistance, 600f));   // was up to 1500 m: on the biggest islands that put the whole forest into every shadow cascade
            // glides to it (it used to jump in 8 % steps: every jump re-sized the shadow texels - shadows popped)
            float next = _shadowDist < 0 ? want : Mathf.Lerp(_shadowDist, want, 1f - Mathf.Exp(-Time.unscaledDeltaTime * 3f));
            float f0 = _focus < 0 ? focus : Mathf.Lerp(_focus, focus, 1f - Mathf.Exp(-Time.unscaledDeltaTime * 3f));
            if (_shadowDist < 0 || Mathf.Abs(next - _shadowDist) > _shadowDist * 0.002f || Mathf.Abs(f0 - _focus) > _focus * 0.002f)
            {
                _shadowDist = next;
                _focus = f0;
                urp.shadowDistance = next;
                // the first cascade just covers the ground in view, the next ones step out from it
                float c0 = Mathf.Clamp((f0 * 1.15f + 15f) / next, 0.04f, 0.5f);
                float c1 = Mathf.Clamp((f0 * 1.8f + 45f) / next, c0 + 0.04f, 0.75f);
                float c2 = Mathf.Clamp((f0 * 2.8f + 90f) / next, c1 + 0.04f, 0.94f);
                urp.cascade4Split = new Vector3(c0, c1, c2);
            }
        }
        const float _seaLevelGuess = 0f;

        void LateUpdate()
        {
            float dt = Mathf.Min(Time.deltaTime, 0.1f);
            UpdateShadowDistance();
            TTBackdrop.Draw(cam, _type, _seaLevel);   // far-off land round the horizon (modern look)
            UpdateTimeOfDay(Time.deltaTime, false);
            if (!_texturesSet)
            {
                var tex = Textures(_type);
                if (tex != null)
                {
                    _texturesSet = true;
                    if (_waterMat != null) { _waterMat.SetTexture("_Ocean0", tex[0]); _waterMat.SetTexture("_Ocean1", tex[1]); }
                    _skyMat.SetTexture("_Inner", tex[2]);
                    _skyMat.SetTexture("_Outer", tex[3]);
                }
            }

            // fog thins as the camera rises (FogShader: density * (1 - camH / heightFactor))
            if (cam != null)
            {
                float h = Mathf.Max(0f, cam.transform.position.y);
                float dens = _fogDensity * (_fogHeight > 0 ? 1f - Mathf.Clamp01(h / _fogHeight) : 1f);
                RenderSettings.fogDensity = dens;
            }

            // Sky.updateAnimation: cloud layers drift and slowly change direction and speed
            _innerTimer += dt;
            if (_innerTimer > _innerInterval)
            {
                _innerTimer = 0; _innerInterval = 30f + Gauss() * 10f;
                _tInnerDir += Mathf.Deg2Rad * Gauss() * 10f;
                _tInnerSpeed = Mathf.Clamp(_tInnerSpeed + _innerSpeed * Gauss() * 0.1f, 0.002f, 0.008f);
            }
            _innerDir += (_tInnerDir - _innerDir) * dt * 0.2f;
            _innerSpeed += (_tInnerSpeed - _innerSpeed) * dt * 0.2f;
            _innerOff += new Vector2(Mathf.Cos(_innerDir), Mathf.Sin(_innerDir)) * _innerSpeed * dt;
            _outerTimer += dt;
            if (_outerTimer > _outerInterval)
            {
                _outerTimer = 0; _outerInterval = 40f + Gauss() * 15f;
                _tOuterDir += Mathf.Deg2Rad * Gauss() * 8f;
                _tOuterSpeed = Mathf.Clamp(_tOuterSpeed + _outerSpeed * Gauss() * 0.1f, 0.001f, 0.004f);
            }
            _outerDir += (_tOuterDir - _outerDir) * dt * 0.1f;
            _outerSpeed += (_tOuterSpeed - _outerSpeed) * dt * 0.1f;
            _outerOff += new Vector2(Mathf.Cos(_outerDir), Mathf.Sin(_outerDir)) * _outerSpeed * dt;
            _skyMat.SetVector("_InnerOffset", _innerOff);
            _skyMat.SetVector("_OuterOffset", _outerOff);

            // Water.updateAnimation: waves, and the two texture layers flowing with a slowly wandering current
            _waveTime += dt;
            // clouds drift with the wind (and their shadows with them)
            var wd = new Vector2(Mathf.Cos(_innerDir), Mathf.Sin(_innerDir));
            _cloudOff += wd * (9f * dt);
            if (_cloudOff.x > 9000f) _cloudOff.x -= 9000f; else if (_cloudOff.x < -9000f) _cloudOff.x += 9000f;
            if (_cloudOff.y > 9000f) _cloudOff.y -= 9000f; else if (_cloudOff.y < -9000f) _cloudOff.y += 9000f;
            bool modernNow = GameSettings.graphicsStyle == 1;
            Shader.SetGlobalVector("_TT_CloudParams", new Vector4(_type == 1 ? 0.7f : 0.38f, _cloudOff.x, _cloudOff.y, modernNow && s_cloudNoise != null ? (_type == 1 ? 0.6f : 0.45f) : 0f));
            _flowTimer += dt;
            if (_flowTimer > 20f)
            {
                _flowTimer = 0;
                _targetFlowDir = _flowDir + Mathf.Deg2Rad * Gauss() * 30f;
                _targetFlowSpeed = Mathf.Clamp(0.001f + Gauss() * 0.0003f, 0.0005f, 0.002f);
            }
            _flowDir += (_targetFlowDir - _flowDir) * dt * 0.05f;
            _flowSpeed += (_targetFlowSpeed - _flowSpeed) * dt * 0.05f;
            var flow = new Vector2(Mathf.Cos(_flowDir), Mathf.Sin(_flowDir)) * _flowSpeed * dt;
            _scroll0 += flow;
            _scroll1 += new Vector2(-flow.y, flow.x) * 0.7f;
            if (_waterMat != null)
            {
                _waterMat.SetFloat("_WaveTime", _waveTime * _waveSpeed);
                _waterMat.SetVector("_Scroll", new Vector4(_scroll0.x, _scroll0.y, _scroll1.x, _scroll1.y));
            }

            UpdateReflection();
        }

        float Gauss()
        {
            double u1 = 1.0 - _rng.NextDouble(), u2 = _rng.NextDouble();
            return (float)(System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2));
        }
    }
}
