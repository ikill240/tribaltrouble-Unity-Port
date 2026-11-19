using System.Collections.Generic;
using TribalTrouble.Faithful;
using TribalTrouble.Game;
using TribalTrouble.GuiPort;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// New Tides Weather option: draws what the simulation's Weather does. Rain that slants with the wind and a
    /// darker, greyer, cloud-covered picture in a storm (with the sky flashing as bolts strike); the camera shaking
    /// in an earthquake, harder near the epicentre; flames, smoke and firelight in burning trees; the sea rising over
    /// the beaches behind a breaking wave front; and a tornado funnel of dust and debris with a gale behind it.
    /// Weather 2 adds hail (white pellets driving down), a blizzard (snow drifting on the wind, a cold white grade),
    /// a heatwave (a hot, bleached, shimmering grade), fog (banks of mist over the ground) and a meteor shower
    /// (glowing rocks streaking down with trails, a flash and a burst of fire and dust where they land).
    /// A banner names each event as it starts. Purely visual: everything it shows comes from the Weather object, so
    /// it can't affect the game. Added to the world when the match has the Weather option.
    /// </summary>
    [DefaultExecutionOrder(200)]   // after TTEnvironment and FaithfulGame (the camera matrix, the cloud parameters)
    public sealed class TTWeatherFx : MonoBehaviour
    {
        Camera _cam;
        FaithfulTerrain _terrain;
        SfxPlayer _sfx;
        Material _rainMat, _flameMat, _smokeMat, _dustMat, _foamMat;
        Texture2D _blob, _streak;
        ParticleSystem _rain, _flakes, _burst;
        Mesh _fogMesh, _meteorMesh;
        readonly Dictionary<float[], Vector3> _meteorsSeen = new Dictionary<float[], Vector3>();
        readonly List<float[]> _meteorsGone = new List<float[]>();
        float _impactFlash;
        Volume _vol;
        VolumeProfile _profile;
        AudioSource _rainAudio, _windAudio;
        int _lastEvent, _lastCount;
        float _bannerTime = -100f;
        string _banner, _bannerSub;
        readonly Dictionary<object, FireFx> _fires = new Dictionary<object, FireFx>(RefEq.Instance);
        readonly List<object> _gone = new List<object>();
        // tornado
        const int FunnelParts = 720, DebrisParts = 90;
        readonly Vector4[] _funnel = new Vector4[FunnelParts];     // height 0..1, angle, radius jitter, size
        readonly Vector4[] _debris = new Vector4[DebrisParts];
        Mesh _funnelMesh, _foamMesh;
        readonly List<Vector3> _v = new List<Vector3>(4096);
        readonly List<Color> _c = new List<Color>(4096);
        readonly List<Vector2> _uv = new List<Vector2>(4096);
        readonly List<int> _i = new List<int>(6144);
        float _seaY0 = float.NaN;

        sealed class FireFx
        {
            public GameObject go;
            public ParticleSystem flames, smoke;
            public Light light;
            public float born;
        }

        public static TTWeatherFx Create(Transform parent, Camera cam, FaithfulTerrain terrain, SfxPlayer sfx)
        {
            var go = new GameObject("Weather");
            go.transform.SetParent(parent, false);
            var fx = go.AddComponent<TTWeatherFx>();
            fx._cam = cam; fx._terrain = terrain; fx._sfx = sfx;
            fx.Init();
            return fx;
        }

        static com.oddlabs.tt.landscape.Weather CurrentWeather(out com.oddlabs.tt.landscape.World world)
        {
            var dr = Host.CurrentWorld;
            world = dr != null && !dr.closed ? dr.world : null;
            return world?.getWeather();
        }

        // ---- set up ---------------------------------------------------------------------------------------------

        void Init()
        {
            var shader = Shader.Find("TribalTrouble/Particle") ?? Shader.Find("Universal Render Pipeline/Particles/Unlit");
            _blob = Blob(64, 2.2f);
            _streak = Streak();
            _rainMat = Mat(shader, "Rain", _streak, false);
            _flameMat = Mat(shader, "Flames", TTFxTextures.Get("fire", _blob), true);   // (ComfyUI sprites in the modern look)
            _smokeMat = Mat(shader, "Smoke", TTFxTextures.Get("smoke", _blob), false);
            _dustMat = Mat(shader, "Funnel dust", TTFxTextures.Get("dust", _blob), false);
            _foamMat = Mat(shader, "Wave foam", _blob, false);
            _funnelMesh = new Mesh { name = "Tornado" }; _funnelMesh.MarkDynamic();
            _foamMesh = new Mesh { name = "Wave" }; _foamMesh.MarkDynamic();
            _fogMesh = new Mesh { name = "Fog" }; _fogMesh.MarkDynamic();
            _meteorMesh = new Mesh { name = "Meteors" }; _meteorMesh.MarkDynamic();
            var rng = new System.Random(1234);
            for (int k = 0; k < FunnelParts; k++)
                _funnel[k] = new Vector4((float)rng.NextDouble(), (float)(rng.NextDouble() * Mathf.PI * 2), (float)rng.NextDouble(), 0.6f + (float)rng.NextDouble());
            for (int k = 0; k < DebrisParts; k++)
                _debris[k] = new Vector4((float)rng.NextDouble(), (float)(rng.NextDouble() * Mathf.PI * 2), (float)rng.NextDouble(), (float)rng.NextDouble());

            // rain: a box of streaks above the spot the camera looks at
            var rgo = new GameObject("Rain");
            rgo.transform.SetParent(transform, false);
            _rain = rgo.AddComponent<ParticleSystem>();
            _rain.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = _rain.main;
            main.loop = true; main.playOnAwake = false;
            main.startLifetime = 1.5f; main.startSpeed = 0f; main.startSize = 0.06f;
            main.maxParticles = 9000;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.startColor = new Color(0.78f, 0.84f, 0.95f, 0.42f);
            var em = _rain.emission; em.rateOverTime = 0f;
            var sh = _rain.shape; sh.shapeType = ParticleSystemShapeType.Box; sh.scale = new Vector3(150f, 1f, 150f);
            var vel = _rain.velocityOverLifetime; vel.enabled = true; vel.space = ParticleSystemSimulationSpace.World;
            vel.x = new ParticleSystem.MinMaxCurve(0f); vel.y = new ParticleSystem.MinMaxCurve(-26f); vel.z = new ParticleSystem.MinMaxCurve(0f);
            var pr = rgo.GetComponent<ParticleSystemRenderer>();
            pr.renderMode = ParticleSystemRenderMode.Stretch;
            pr.velocityScale = 0.045f; pr.lengthScale = 1f;
            pr.sharedMaterial = _rainMat;
            pr.shadowCastingMode = ShadowCastingMode.Off; pr.receiveShadows = false;
            _rain.Play();

            // storm grade: darker, greyer, a little blue
            _profile = ScriptableObject.CreateInstance<VolumeProfile>();
            _profile.Add<ColorAdjustments>(true);
            var vgo = new GameObject("Storm grade");
            vgo.transform.SetParent(transform, false);
            _vol = vgo.AddComponent<Volume>();
            _vol.isGlobal = true; _vol.priority = 16; _vol.weight = 0f;
            _vol.sharedProfile = _profile;

            // hail and snow: round flakes (a blob), set up per event in Flakes()
            var fgo = new GameObject("Hail and snow");
            fgo.transform.SetParent(transform, false);
            _flakes = fgo.AddComponent<ParticleSystem>();
            _flakes.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var fm = _flakes.main;
            fm.loop = true; fm.playOnAwake = false; fm.startSpeed = 0f; fm.maxParticles = 8000;
            fm.simulationSpace = ParticleSystemSimulationSpace.World;
            var fe = _flakes.emission; fe.rateOverTime = 0f;
            var fs = _flakes.shape; fs.shapeType = ParticleSystemShapeType.Box; fs.scale = new Vector3(150f, 1f, 150f);
            var fv = _flakes.velocityOverLifetime; fv.enabled = true; fv.space = ParticleSystemSimulationSpace.World;
            fv.x = new ParticleSystem.MinMaxCurve(0f); fv.y = new ParticleSystem.MinMaxCurve(-3f); fv.z = new ParticleSystem.MinMaxCurve(0f);
            var fn = _flakes.noise; fn.enabled = true; fn.strength = 0.8f; fn.frequency = 0.3f;
            var fr = fgo.GetComponent<ParticleSystemRenderer>();
            fr.renderMode = ParticleSystemRenderMode.Billboard;
            fr.sharedMaterial = _smokeMat;
            fr.shadowCastingMode = ShadowCastingMode.Off; fr.receiveShadows = false;
            _flakes.Play();

            // meteor impacts: a burst of fire and dust, emitted by hand
            var bgo = new GameObject("Meteor impacts");
            bgo.transform.SetParent(transform, false);
            _burst = bgo.AddComponent<ParticleSystem>();
            _burst.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var bm = _burst.main;
            bm.loop = false; bm.playOnAwake = false; bm.maxParticles = 3000;
            bm.simulationSpace = ParticleSystemSimulationSpace.World;
            bm.gravityModifier = 0.6f;
            var be = _burst.emission; be.enabled = false;
            var bsz = _burst.sizeOverLifetime; bsz.enabled = true;
            bsz.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 0.6f, 1f, 1.8f));
            var bcol = _burst.colorOverLifetime; bcol.enabled = true;
            var g = new Gradient();
            g.SetKeys(new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.6f, 0.4f), new GradientAlphaKey(0f, 1f) });
            bcol.color = g;
            var br = bgo.GetComponent<ParticleSystemRenderer>();
            br.renderMode = ParticleSystemRenderMode.Billboard;
            br.sharedMaterial = _smokeMat;
            br.shadowCastingMode = ShadowCastingMode.Off; br.receiveShadows = false;
            _burst.Play();

            _rainAudio = Loop("ambient_wind", 0.9f);
            _windAudio = Loop("ambient_wind", 0.7f);
        }

        AudioSource Loop(string clip, float pitch)
        {
            var c = _sfx?.Get(clip);
            if (c == null) return null;
            var a = gameObject.AddComponent<AudioSource>();
            a.clip = c; a.loop = true; a.volume = 0f; a.pitch = pitch; a.spatialBlend = 0f; a.playOnAwake = false;
            a.Play();
            return a;
        }

        static Material Mat(Shader shader, string name, Texture tex, bool additive)
        {
            var m = new Material(shader) { name = name, mainTexture = tex };
            m.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            m.SetFloat("_DstBlend", (float)(additive ? BlendMode.One : BlendMode.OneMinusSrcAlpha));
            m.renderQueue = 3050;
            return m;
        }

        static Texture2D Blob(int n, float pow)
        {
            var tex = new Texture2D(n, n, TextureFormat.RGBA32, true) { name = "WeatherBlob", wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float dx = (x + .5f) / n * 2 - 1, dy = (y + .5f) / n * 2 - 1;
                    float a = Mathf.Pow(Mathf.Clamp01(1f - Mathf.Sqrt(dx * dx + dy * dy)), pow);
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            tex.SetPixels32(px); tex.Apply(true);
            return tex;
        }

        static Texture2D Streak()
        {
            const int w = 8, h = 64;
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, true) { name = "RainStreak", wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[w * h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    float u = Mathf.Abs((x + .5f) / w * 2 - 1), v = (y + .5f) / h;
                    float a = (1f - u * u) * Mathf.Sin(v * Mathf.PI);
                    px[y * w + x] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(a) * 255));
                }
            tex.SetPixels32(px); tex.Apply(true);
            return tex;
        }

        // ---- every frame ----------------------------------------------------------------------------------------

        void LateUpdate()
        {
            var weather = CurrentWeather(out var world);
            if (weather == null || _cam == null) { Idle(); return; }
            float dt = Mathf.Min(Time.deltaTime, 0.1f);
            var hm = world.getHeightMap();
            Vector3 focus = Focus(hm);
            int ev = weather.getEvent();
            if (weather.getEventCount() != _lastCount && ev != com.oddlabs.tt.landscape.Weather.CALM) { _lastCount = weather.getEventCount(); Banner(ev); }
            _lastEvent = ev;

            float rain = weather.getRain(), dark = weather.getDarkness(), wind = weather.getWind();
            Vector3 windDir = new Vector3(weather.getWindX(), 0f, weather.getWindY());

            // rain
            var em = _rain.emission; em.rateOverTime = 6000f * rain;
            _rain.transform.position = focus + Vector3.up * 34f - windDir * (wind * 12f);
            var vel = _rain.velocityOverLifetime;
            vel.x = new ParticleSystem.MinMaxCurve(windDir.x * (3f + 10f * wind));
            vel.y = new ParticleSystem.MinMaxCurve(-26f);
            vel.z = new ParticleSystem.MinMaxCurve(windDir.z * (3f + 10f * wind));

            // the grade, the clouds and the sky flashing with the bolts
            float flash = Mathf.Max(TTRealLightning.FlashLevel * Mathf.Clamp01(dark * 1.5f), _impactFlash);
            _impactFlash = Mathf.Max(0f, _impactFlash - dt * 3f);
            float k = weather.getIntensity();
            // each event's own look: a cold white blizzard, a hot bleached heatwave, a pale washed-out fog, a red sky of meteors
            Color tint = new Color(0.80f, 0.86f, 0.97f);
            float sat = -38f * dark, exposure = -1.15f * dark;
            float heat = 0f, snow = 0f, fog = 0f;
            switch (ev)
            {
                case com.oddlabs.tt.landscape.Weather.BLIZZARD: snow = k; tint = new Color(0.86f, 0.92f, 1f); exposure = -0.55f * k; sat = -45f * k; break;
                case com.oddlabs.tt.landscape.Weather.HEATWAVE: heat = k; tint = new Color(1f, 0.86f, 0.66f); exposure = 0.25f * k; sat = -12f * k; break;
                case com.oddlabs.tt.landscape.Weather.FOG: fog = k; tint = new Color(0.9f, 0.93f, 0.95f); exposure = 0.1f * k; sat = -50f * k; break;
                case com.oddlabs.tt.landscape.Weather.METEORS: tint = new Color(1f, 0.74f, 0.6f); break;
            }
            float tintK = Mathf.Max(dark, Mathf.Max(heat, Mathf.Max(snow, fog)));
            _vol.weight = Mathf.Clamp01(Mathf.Max(tintK, flash));
            if (_profile.TryGet(out ColorAdjustments ca))
            {
                ca.postExposure.Override(exposure + 1.6f * flash);
                ca.saturation.Override(sat);
                ca.contrast.Override(-30f * fog + 8f * heat);
                ca.colorFilter.Override(Color.Lerp(Color.white, tint, tintK));
            }
            Flakes(weather, ev, k, focus, windDir, wind);
            var cp = Shader.GetGlobalVector("_TT_CloudParams");
            if (dark > 0.01f) { cp.x = Mathf.Lerp(cp.x, 0.92f, dark); Shader.SetGlobalVector("_TT_CloudParams", cp); }
            if (_rainAudio != null) _rainAudio.volume = 0.55f * rain * GameSettings.sfx * GameSettings.master;
            if (_windAudio != null)
            {
                float near = weather.hasTornado() ? Mathf.Clamp01(1f - Vector2.Distance(new Vector2(focus.x, focus.z), new Vector2(weather.getTornadoX(), weather.getTornadoY())) / 160f) : 0f;
                _windAudio.volume = (0.25f * wind + 0.6f * near * weather.getTornadoStrength()) * GameSettings.sfx * GameSettings.master;
                _windAudio.pitch = 0.75f + 0.35f * wind + 0.3f * near;
            }

            Quake(weather, focus);
            Fires(weather, hm, windDir, wind);
            Flood(weather, hm);
            Tornado(weather, hm, dt);
            Fog(fog, focus, hm);
            Meteors(weather);
            Heat(heat);
        }

        void Idle()
        {
            if (_rain != null) { var em = _rain.emission; em.rateOverTime = 0f; }
            if (_flakes != null) { var fe = _flakes.emission; fe.rateOverTime = 0f; }
            Heat(0f);
            if (_vol != null) _vol.weight = 0f;
            if (_rainAudio != null) _rainAudio.volume = 0f;
            if (_windAudio != null) _windAudio.volume = 0f;
        }

        /// <summary>The ground point the camera looks at (or under it, looking flat).</summary>
        Vector3 Focus(com.oddlabs.tt.landscape.HeightMap hm)
        {
            var t = _cam.transform;
            Vector3 p = t.position, f = t.forward;
            float ground = hm.getNearestHeight(p.x, p.z);
            float dist = f.y < -0.08f ? (p.y - ground) / -f.y : 60f;
            Vector3 at = p + f * Mathf.Min(dist, 220f);
            at.y = hm.getNearestHeight(at.x, at.z);
            return at;
        }

        // ---- earthquake: the camera shakes, harder near the epicentre ------------------------------------------

        void Quake(com.oddlabs.tt.landscape.Weather weather, Vector3 focus)
        {
            float s = weather.getQuakeStrength();
            if (s <= 0.01f) return;
            float d = Vector2.Distance(new Vector2(focus.x, focus.z), new Vector2(weather.getQuakeX(), weather.getQuakeY()));
            float k = s * Mathf.Clamp01(1.15f - d / 160f);
            if (k <= 0.01f) return;
            float t = Time.time;
            var off = new Vector3(Mathf.PerlinNoise(t * 14f, 0.3f) - 0.5f, Mathf.PerlinNoise(0.7f, t * 17f) - 0.5f, Mathf.PerlinNoise(t * 11f, 5.1f) - 0.5f) * (1.1f * k);
            var rot = Quaternion.Euler((Mathf.PerlinNoise(t * 9f, 2f) - 0.5f) * 1.6f * k, 0f, (Mathf.PerlinNoise(3f, t * 10f) - 0.5f) * 1.4f * k);
            // the camera's matrix is the Java camera's, set every frame: shake on top of it
            _cam.worldToCameraMatrix = Matrix4x4.TRS(off, rot, Vector3.one) * _cam.worldToCameraMatrix;
        }

        // ---- wildfire: flames, smoke and light in each burning tree --------------------------------------------

        readonly HashSet<object> _seenFires = new HashSet<object>(RefEq.Instance);
        readonly List<(float d, FireFx f)> _nearFires = new List<(float d, FireFx f)>();
        static readonly System.Comparison<(float d, FireFx f)> s_byDistance = (a, b) => a.d.CompareTo(b.d);
        Vector3 _fireWind = new Vector3(float.NaN, 0f, 0f);

        void Fires(com.oddlabs.tt.landscape.Weather weather, com.oddlabs.tt.landscape.HeightMap hm, Vector3 windDir, float wind)
        {
            var list = weather.getFires();
            int n = list.size();
            if (n == 0 && _fires.Count == 0) return;
            var seen = _seenFires;
            seen.Clear();
            Vector3 camPos = _cam.transform.position;
            var near = _nearFires;
            near.Clear();
            var w = new Vector3(windDir.x * (1f + 5f * wind), 0f, windDir.z * (1f + 5f * wind));
            bool windChanged = (w - _fireWind).sqrMagnitude > 1e-6f || float.IsNaN(_fireWind.x);
            if (windChanged) _fireWind = w;
            for (int k = 0; k < n; k++)
            {
                var tree = (com.oddlabs.tt.landscape.TreeSupply)list.get(k);
                seen.Add(tree);
                bool fresh = false;
                if (!_fires.TryGetValue(tree, out var fx))
                {
                    float x = tree.getPositionX(), y = tree.getPositionY();
                    fx = MakeFire(new Vector3(x, hm.getNearestHeight(x, y), y));
                    _fires[tree] = fx;
                    fresh = true;
                }
                if (fresh || windChanged) // the smoke follows the wind; set only when it changes
                {
                    var vel = fx.smoke.velocityOverLifetime;
                    vel.x = new ParticleSystem.MinMaxCurve(w.x);
                    vel.z = new ParticleSystem.MinMaxCurve(w.z);
                }
                near.Add(((fx.go.transform.position - camPos).sqrMagnitude, fx));
            }
            near.Sort(s_byDistance);
            float t = Time.time;
            for (int k = 0; k < near.Count; k++)
            {
                var fx = near[k].f;
                fx.light.enabled = k < 8;
                if (k < 8) fx.light.intensity = (2.6f + 1.2f * Mathf.PerlinNoise(t * 7f, k * 3.1f)) * Mathf.Clamp01((t - fx.born) * 0.8f);
            }
            _gone.Clear();
            foreach (var kv in _fires) if (!seen.Contains(kv.Key)) _gone.Add(kv.Key);
            foreach (var key in _gone)
            {
                var fx = _fires[key];
                fx.flames.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                fx.smoke.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                fx.light.enabled = false;
                Destroy(fx.go, 6f);
                _fires.Remove(key);
            }
        }

        FireFx MakeFire(Vector3 ground)
        {
            var fx = new FireFx { born = Time.time };
            fx.go = new GameObject("Fire");
            fx.go.transform.SetParent(transform, false);
            fx.go.transform.position = ground;
            fx.flames = Particles(fx.go, "Flames", _flameMat, new Vector3(0, 2.5f, 0), 1.1f, 0.9f, 2.4f, 3.5f, 60f,
                new Gradient
                {
                    colorKeys = new[] { new GradientColorKey(new Color(1f, 0.85f, 0.45f), 0f), new GradientColorKey(new Color(1f, 0.45f, 0.1f), 0.45f), new GradientColorKey(new Color(0.6f, 0.12f, 0.02f), 1f) },
                    alphaKeys = new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0.9f, 0.12f), new GradientAlphaKey(0.5f, 0.6f), new GradientAlphaKey(0f, 1f) }
                }, 1.4f, 0.4f);
            fx.smoke = Particles(fx.go, "Smoke", _smokeMat, new Vector3(0, 5f, 0), 1.6f, 4.5f, 2.5f, 6f, 9f,
                new Gradient
                {
                    colorKeys = new[] { new GradientColorKey(new Color(0.25f, 0.23f, 0.22f), 0f), new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 1f) },
                    alphaKeys = new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(0.45f, 0.15f), new GradientAlphaKey(0f, 1f) }
                }, 2.4f, 2.2f);
            var lgo = new GameObject("Firelight");
            lgo.transform.SetParent(fx.go.transform, false);
            lgo.transform.localPosition = new Vector3(0, 3.5f, 0);
            fx.light = lgo.AddComponent<Light>();
            fx.light.type = LightType.Point; fx.light.range = 16f; fx.light.color = new Color(1f, 0.55f, 0.22f); fx.light.shadows = LightShadows.None;
            fx.light.intensity = 0f;
            return fx;
        }

        static ParticleSystem Particles(GameObject parent, string name, Material mat, Vector3 at, float radius, float life, float speed, float size, float rate, Gradient colour, float grow, float lifeJitter)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = at;
            var ps = go.AddComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = ps.main;
            main.loop = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(life - lifeJitter * .5f, life + lifeJitter * .5f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed * .6f, speed);
            main.startSize = new ParticleSystem.MinMaxCurve(size * .6f, size);
            main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
            main.maxParticles = 400;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            var em = ps.emission; em.rateOverTime = rate;
            var sh = ps.shape; sh.shapeType = ParticleSystemShapeType.Cone; sh.angle = 12f; sh.radius = radius;
            sh.rotation = new Vector3(-90f, 0f, 0f);
            var col = ps.colorOverLifetime; col.enabled = true; col.color = new ParticleSystem.MinMaxGradient(colour);
            var sz = ps.sizeOverLifetime; sz.enabled = true; sz.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, grow));
            var vel = ps.velocityOverLifetime; vel.enabled = true; vel.space = ParticleSystemSimulationSpace.World;
            vel.x = new ParticleSystem.MinMaxCurve(0f); vel.y = new ParticleSystem.MinMaxCurve(0f); vel.z = new ParticleSystem.MinMaxCurve(0f);
            var r = go.GetComponent<ParticleSystemRenderer>();
            r.sharedMaterial = mat; r.shadowCastingMode = ShadowCastingMode.Off; r.receiveShadows = false;
            ps.Play();
            return ps;
        }

        // ---- tsunami: the sea rises behind a breaking wave -----------------------------------------------------

        void Flood(com.oddlabs.tt.landscape.Weather weather, com.oddlabs.tt.landscape.HeightMap hm)
        {
            var water = _terrain?.water;
            if (water == null) return;
            if (float.IsNaN(_seaY0)) _seaY0 = water.transform.localPosition.y;
            float level = weather.getFloodLevel();
            var lp = water.transform.localPosition; lp.y = _seaY0 + level; water.transform.localPosition = lp;
            if (weather.getEvent() != com.oddlabs.tt.landscape.Weather.FLOOD) return;
            // the wave front: a foaming crest crossing the island during the rise
            float time = weather.getEventTime(), rise = 7f;
            if (time > rise + 2f) return;
            float meters = hm.getMetersPerWorld(), sea = hm.getSeaLevelMeters();
            var dir = new Vector3(weather.getFloodDirX(), 0, weather.getFloodDirY());
            var side = new Vector3(-dir.z, 0, dir.x);
            var centre = new Vector3(meters * .5f, 0, meters * .5f);
            float p = Mathf.Clamp01(time / rise);
            Vector3 front = centre - dir * (meters * .75f) + dir * (meters * 1.0f * p);
            float crest = 1.2f + 2.6f * Mathf.Sin(Mathf.Clamp01(time / (rise + 2f)) * Mathf.PI) * 1.2f;
            _v.Clear(); _c.Clear(); _uv.Clear(); _i.Clear();
            int segs = 48;
            float width = meters * 1.3f;
            for (int k = 0; k <= segs; k++)
            {
                float s = (k / (float)segs - .5f) * width;
                float wob = Mathf.Sin(k * 0.9f + Time.time * 1.7f) * 2.5f;
                Vector3 b = front + side * s + dir * wob;
                float ground = hm.getNearestHeight(Mathf.Clamp(b.x, 0, meters), Mathf.Clamp(b.z, 0, meters));
                float top = Mathf.Max(sea + weather.getFloodLevel() + crest, ground + 0.6f);
                Vector3 bottom = new Vector3(b.x, Mathf.Max(sea + weather.getFloodLevel() - 0.2f, ground - 0.3f), b.z);
                Vector3 lip = new Vector3(b.x, top, b.z) + dir * 1.5f;
                _v.Add(bottom - dir * 3.5f); _v.Add(lip);
                float alpha = ground > top + 1f ? 0f : 0.85f;
                _c.Add(new Color(0.55f, 0.72f, 0.78f, alpha * 0.7f)); _c.Add(new Color(0.96f, 0.98f, 1f, alpha));
                _uv.Add(new Vector2(k / (float)segs, 0f)); _uv.Add(new Vector2(k / (float)segs, 0.5f));
            }
            for (int k = 0; k < segs; k++)
            {
                int a = k * 2;
                _i.Add(a); _i.Add(a + 1); _i.Add(a + 3); _i.Add(a); _i.Add(a + 3); _i.Add(a + 2);
            }
            Draw(_foamMesh, _foamMat);
        }

        // ---- tornado: a funnel of dust spinning over a debris cloud ---------------------------------------------

        void Tornado(com.oddlabs.tt.landscape.Weather weather, com.oddlabs.tt.landscape.HeightMap hm, float dt)
        {
            float strength = weather.getTornadoStrength();
            if (!weather.hasTornado() || strength <= 0.01f) return;
            float x = weather.getTornadoX(), y = weather.getTornadoY();
            Vector3 basePos = new Vector3(x, hm.getNearestHeight(x, y), y);
            const float H = 48f;
            Vector3 camPos = _cam.transform.position;
            Vector3 right = _cam.transform.right, up = _cam.transform.up;
            _v.Clear(); _c.Clear(); _uv.Clear(); _i.Clear();
            float t = Time.time;
            for (int k = 0; k < FunnelParts; k++)
            {
                var p = _funnel[k];
                float h = Mathf.Repeat(p.x + t * 0.06f, 1f);   // dust rises up the funnel
                float r = (0.9f + 13f * Mathf.Pow(h, 1.7f)) * (0.85f + 0.3f * p.z) * (0.4f + 0.6f * strength);
                float spin = 2.6f / Mathf.Max(0.6f, r * 0.35f);
                float a = p.y + t * spin;
                // the funnel leans and snakes a little
                float lean = Mathf.Sin(t * 0.4f + h * 3f) * 3f * h;
                Vector3 pos = basePos + new Vector3(Mathf.Cos(a) * r + lean, h * H * (0.5f + 0.5f * strength), Mathf.Sin(a) * r + lean * 0.5f);
                float size = (1.4f + 4.2f * h) * p.w;
                float fade = Mathf.Clamp01(h * 8f) * Mathf.Clamp01((1f - h) * 5f);
                float shade = 0.36f + 0.25f * h + 0.12f * p.z;
                Quad(pos, right * size, up * size, new Color(shade, shade * 0.95f, shade * 0.9f, 0.32f * fade * strength));
            }
            for (int k = 0; k < DebrisParts; k++)
            {
                var p = _debris[k];
                float a = p.y + t * (1.6f + p.z);
                float r = 3f + 9f * p.x;
                float h = 0.5f + 7f * Mathf.Repeat(p.w + t * (0.3f + p.z * 0.4f), 1f);
                Vector3 pos = basePos + new Vector3(Mathf.Cos(a) * r, h, Mathf.Sin(a) * r);
                float size = 0.25f + 0.5f * p.z;
                Quad(pos, right * size, up * size, new Color(0.22f, 0.18f, 0.14f, 0.85f * strength));
            }
            // the dust cloud around the foot
            for (int k = 0; k < 14; k++)
            {
                float a = k / 14f * Mathf.PI * 2f + t * 0.8f;
                Vector3 pos = basePos + new Vector3(Mathf.Cos(a) * 6f, 1.5f, Mathf.Sin(a) * 6f);
                Quad(pos, right * 7f, up * 4f, new Color(0.45f, 0.4f, 0.33f, 0.25f * strength));
            }
            Draw(_funnelMesh, _dustMat);
        }

        // ---- hail and snow ---------------------------------------------------------------------------------------

        void Flakes(com.oddlabs.tt.landscape.Weather weather, int ev, float k, Vector3 focus, Vector3 windDir, float wind)
        {
            bool hail = ev == com.oddlabs.tt.landscape.Weather.HAIL, snow = ev == com.oddlabs.tt.landscape.Weather.BLIZZARD;
            var em = _flakes.emission;
            em.rateOverTime = hail ? 2600f * k : snow ? 4200f * k : 0f;
            if (!hail && !snow) return;
            var main = _flakes.main;
            main.startLifetime = hail ? 1.3f : 9f;
            main.startSize = hail ? new ParticleSystem.MinMaxCurve(0.09f, 0.16f) : new ParticleSystem.MinMaxCurve(0.12f, 0.26f);
            main.startColor = hail ? new Color(0.95f, 0.97f, 1f, 0.85f) : new Color(1f, 1f, 1f, 0.8f);
            float fall = hail ? -30f : -3.2f, drift = hail ? 3f + 6f * wind : 4f + 14f * wind;
            var vel = _flakes.velocityOverLifetime;
            vel.x = new ParticleSystem.MinMaxCurve(windDir.x * drift);
            vel.y = new ParticleSystem.MinMaxCurve(fall);
            vel.z = new ParticleSystem.MinMaxCurve(windDir.z * drift);
            var noise = _flakes.noise; noise.strength = hail ? 0.2f : 1.4f;
            _flakes.transform.position = focus + Vector3.up * (hail ? 34f : 26f) - windDir * (drift * (hail ? 1.1f : 6f));
        }

        // ---- heatwave: the air shimmers (a slow wobble of the picture) -------------------------------------------

        float _heatShown;

        void Heat(float k)
        {
            if (_cam == null) return;
            if (k <= 0.01f) { _heatShown = 0f; return; }
            _heatShown = k;
            float t = Time.time;
            // a tiny, slow vertical wobble reads as heat haze without a full-screen distortion pass
            var off = new Vector3(0f, (Mathf.PerlinNoise(t * 1.3f, 0.5f) - 0.5f) * 0.05f * k, 0f);
            _cam.worldToCameraMatrix = Matrix4x4.Translate(off) * _cam.worldToCameraMatrix;
        }

        // ---- fog: banks of mist drifting over the ground around the view ----------------------------------------

        void Fog(float k, Vector3 focus, com.oddlabs.tt.landscape.HeightMap hm)
        {
            if (k <= 0.01f) return;
            Vector3 right = _cam.transform.right, up = Vector3.up;
            _v.Clear(); _c.Clear(); _uv.Clear(); _i.Clear();
            float t = Time.time;
            const int N = 90;
            for (int n = 0; n < N; n++)
            {
                // a fixed scatter around the focus that slides slowly with time, wrapping in a 200 m square
                float hx = Mathf.Repeat(n * 37.31f + t * 0.9f, 200f) - 100f;
                float hz = Mathf.Repeat(n * 71.17f + t * 0.4f, 200f) - 100f;
                float x = focus.x + hx, z = focus.z + hz;
                float ground = hm.getNearestHeight(x, z);
                float edge = Mathf.Clamp01(1.4f - Mathf.Sqrt(hx * hx + hz * hz) / 100f);
                float size = 14f + (n % 5) * 3f;
                Quad(new Vector3(x, ground + 3f + (n % 3), z), right * size, up * size * 0.45f, new Color(0.88f, 0.9f, 0.92f, 0.16f * k * edge));
            }
            Draw(_fogMesh, _smokeMat);
        }

        // ---- meteors: a glowing head and a fiery trail; a flash and a burst where each one lands ----------------

        void Meteors(com.oddlabs.tt.landscape.Weather weather)
        {
            var list = weather.getMeteors();
            _meteorsGone.Clear();
            foreach (var kv in _meteorsSeen) _meteorsGone.Add(kv.Key);
            Vector3 right = _cam.transform.right, up = _cam.transform.up;
            _v.Clear(); _c.Clear(); _uv.Clear(); _i.Clear();
            int count = list.size();
            for (int n = 0; n < count; n++)
            {
                var m = (float[])list.get(n);
                _meteorsGone.Remove(m);
                float u = 1f - Mathf.Clamp01(m[0] / 2.2f);   // 0 = high in the sky, 1 = the ground
                Vector3 to = new Vector3(m[1], m[3], m[2]);
                Vector3 from = new Vector3(m[4], m[3] + 110f, m[5]);
                Vector3 head = Vector3.Lerp(from, to, u * u);   // speeding up as it falls
                _meteorsSeen[m] = to;
                Vector3 dir = (to - from).normalized;
                for (int s = 0; s < 14; s++)
                {
                    Vector3 p = head - dir * (s * 2.2f);
                    float a = 1f - s / 14f;
                    float size = (2.6f - s * 0.12f) * (0.6f + 0.4f * a);
                    Color col = s == 0 ? new Color(1f, 0.95f, 0.8f, 1f) : new Color(1f, 0.55f + 0.3f * a, 0.2f, 0.75f * a * a);
                    Quad(p, right * size, up * size, col);
                }
            }
            Draw(_meteorMesh, _flameMat);
            foreach (var m in _meteorsGone)
            {
                var at = _meteorsSeen[m];
                _meteorsSeen.Remove(m);
                Impact(at);
            }
        }

        void Impact(Vector3 at)
        {
            float d = Vector3.Distance(at, _cam.transform.position);
            _impactFlash = Mathf.Max(_impactFlash, Mathf.Clamp01(1.2f - d / 220f) * 0.8f);
            var ep = new ParticleSystem.EmitParams();
            var rng = new System.Random((int)(at.x * 31 + at.z * 17));
            for (int n = 0; n < 70; n++)
            {
                bool fire = n < 30;
                float a = (float)(rng.NextDouble() * Mathf.PI * 2), sp = 3f + (float)rng.NextDouble() * (fire ? 9f : 5f);
                ep.position = at + Vector3.up * 0.5f;
                ep.velocity = new Vector3(Mathf.Cos(a) * sp, 2f + (float)rng.NextDouble() * (fire ? 10f : 4f), Mathf.Sin(a) * sp);
                ep.startSize = fire ? 0.8f + (float)rng.NextDouble() : 2.5f + (float)rng.NextDouble() * 2.5f;
                ep.startLifetime = fire ? 0.7f + (float)rng.NextDouble() * 0.6f : 2f + (float)rng.NextDouble() * 1.5f;
                ep.startColor = fire ? new Color(1f, 0.6f + 0.3f * (float)rng.NextDouble(), 0.2f, 1f) : new Color(0.32f, 0.28f, 0.24f, 0.6f);
                _burst.Emit(ep, 1);
            }
            if (_sfx != null && d < 260f) _sfx.Play("building_crash", at);
        }

        void Quad(Vector3 c, Vector3 r, Vector3 u, Color col)
        {
            int i = _v.Count;
            _v.Add(c - r - u); _v.Add(c + r - u); _v.Add(c + r + u); _v.Add(c - r + u);
            _c.Add(col); _c.Add(col); _c.Add(col); _c.Add(col);
            _uv.Add(new Vector2(0, 0)); _uv.Add(new Vector2(1, 0)); _uv.Add(new Vector2(1, 1)); _uv.Add(new Vector2(0, 1));
            _i.Add(i); _i.Add(i + 1); _i.Add(i + 2); _i.Add(i); _i.Add(i + 2); _i.Add(i + 3);
        }

        void Draw(Mesh mesh, Material mat)
        {
            if (_v.Count == 0) return;
            mesh.Clear();
            mesh.indexFormat = _v.Count > 65000 ? IndexFormat.UInt32 : IndexFormat.UInt16;
            mesh.SetVertices(_v); mesh.SetColors(_c); mesh.SetUVs(0, _uv); mesh.SetTriangles(_i, 0, false);
            mesh.bounds = new Bounds(_cam.transform.position, Vector3.one * 100000f);
            Graphics.RenderMesh(new RenderParams(mat) { shadowCastingMode = ShadowCastingMode.Off }, mesh, 0, Matrix4x4.identity);
        }

        // ---- the banner ----------------------------------------------------------------------------------------

        static string S(string key, string fallback)
        {
            var v = TTStrings.Get("TTWeather", key);
            return string.IsNullOrEmpty(v) || v == key ? fallback : v;
        }

        void Banner(int ev)
        {
            _bannerTime = Time.unscaledTime;
            switch (ev)
            {
                case 1: _banner = S("storm", "Thunderstorm"); _bannerSub = S("storm_sub", "Lightning strikes the tallest things - towers, huts and trees"); break;
                case 2: _banner = S("quake", "Earthquake"); _bannerSub = S("quake_sub", "Buildings near the epicentre are cracking"); break;
                case 3: _banner = S("fire", "Wildfire"); _bannerSub = S("fire_sub", "The forest is burning - and the wind is spreading it"); break;
                case 4: _banner = S("flood", "Tsunami"); _bannerSub = S("flood_sub", "The sea is rising over the beaches - get to high ground"); break;
                case 5: _banner = S("tornado", "Tornado"); _bannerSub = S("tornado_sub", "A twister is crossing the island - and the gale throws weapons off course"); break;
                case 6: _banner = S("hail", "Hailstorm"); _bannerSub = S("hail_sub", "Hailstones are battering everyone caught in the open"); break;
                case 7: _banner = S("blizzard", "Blizzard"); _bannerSub = S("blizzard_sub", "Snow and bitter cold - everyone moves and works slower"); break;
                case 8: _banner = S("heatwave", "Heatwave"); _bannerSub = S("heatwave_sub", "Tired workers and tinder-dry trees that burst into flame"); break;
                case 9: _banner = S("fog", "Thick Fog"); _bannerSub = S("fog_sub", "Thrown weapons go astray in the mist"); break;
                case 10: _banner = S("meteors", "Meteor Shower"); _bannerSub = S("meteors_sub", "Burning rocks are falling from the sky"); break;
                default: _banner = null; break;
            }
        }

        GUIStyle _big, _small;

        void OnGUI()
        {
            if (_banner == null) return;
            float age = Time.unscaledTime - _bannerTime;
            if (age > 7f) return;
            float a = Mathf.Clamp01(age * 3f) * Mathf.Clamp01((7f - age) * 1.2f);
            float scale = Screen.height / 1080f;
            _big ??= new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            _small ??= new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            _big.fontSize = Mathf.RoundToInt(46 * scale);
            _small.fontSize = Mathf.RoundToInt(21 * scale);
            float y = Screen.height * 0.18f;
            var r1 = new Rect(0, y, Screen.width, 60 * scale);
            var r2 = new Rect(0, y + 56 * scale, Screen.width, 32 * scale);
            var shadow = new Color(0, 0, 0, 0.75f * a);
            Shadowed(r1, _banner, _big, new Color(1f, 0.93f, 0.78f, a), shadow, 2f * scale);
            Shadowed(r2, _bannerSub, _small, new Color(1f, 1f, 1f, a * 0.95f), shadow, 1.5f * scale);
        }

        static void Shadowed(Rect r, string text, GUIStyle style, Color col, Color shadow, float off)
        {
            var old = GUI.color;
            GUI.color = shadow;
            GUI.Label(new Rect(r.x + off, r.y + off, r.width, r.height), text, style);
            GUI.color = col;
            GUI.Label(r, text, style);
            GUI.color = old;
        }

        void OnDestroy()
        {
            foreach (var m in new[] { _rainMat, _flameMat, _smokeMat, _dustMat, _foamMat }) if (m != null) Destroy(m);
            if (_profile != null) Destroy(_profile);
            if (_funnelMesh != null) Destroy(_funnelMesh);
            if (_foamMesh != null) Destroy(_foamMesh);
            if (_fogMesh != null) Destroy(_fogMesh);
            if (_meteorMesh != null) Destroy(_meteorMesh);
        }
    }
}
