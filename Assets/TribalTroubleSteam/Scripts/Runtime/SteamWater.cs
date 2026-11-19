// The Steam release's sea, per frame (scenery/Water.java, DefaultRenderer.render):
//  - the island's terrain type picks the waves (Native: 0.15 / 0.0795 / 0.0405 m, steepness 0.5, 60 / 35 / 18 m,
//    speed 0.4; Viking: x 1.5, steepness 0.6, 50 / 28 / 14 m, speed 0.8) and the depth scale (1 m / 2 m)
//  - the GeneratorOcean textures for that terrain (made once, off the main thread)
//  - the flow of the two texture layers drifts: a Gaussian change of direction and speed every ~17.5 s, eased in
//  - the planar reflection: the scene drawn again from the camera mirrored in the sea level (not the sea itself),
//    full size, unless the graphic detail is low (the fog colour is reflected then)
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TribalTroubleSteam
{
    public sealed class SteamWater
    {
        const int WaterLayer = 4, UiLayer = 5;
        static readonly float[] DirX = { 1f, 0.707f, -0.5f }, DirY = { 0f, 0.707f, 0.866f };

        readonly Material _mat;
        readonly float _seaLevel;
        readonly float _waveSpeed;
        readonly com.oddlabs.tt.procedural.Landscape.TerrainType _terrain;
        Task<com.oddlabs.tt.render.Texture[]> _gen;
        bool _texturesSet;

        float _waveTime, _lastTime = -1f;
        Vector2 _scroll0, _scroll1;
        float _flowDir = 45f * Mathf.Deg2Rad, _flowSpeed = 0.001f, _targetDir, _targetSpeed, _sinceChange, _interval = 20f;
        readonly System.Random _rng = new System.Random();

        Camera _reflCam;
        RenderTexture _reflRT;
        public Camera ReflectionCamera => _reflCam != null && _reflCam.enabled ? _reflCam : null;

        static readonly System.Collections.Generic.Dictionary<com.oddlabs.tt.procedural.Landscape.TerrainType, Texture2D[]> s_ocean =
            new System.Collections.Generic.Dictionary<com.oddlabs.tt.procedural.Landscape.TerrainType, Texture2D[]>();

        static readonly int WaveTimeId = Shader.PropertyToID("_WaveTime");
        static readonly int ScrollId = Shader.PropertyToID("_Scroll");
        static readonly int DetailId = Shader.PropertyToID("_Detail");
        static readonly int HasReflId = Shader.PropertyToID("_HasReflection");
        static readonly int ReflId = Shader.PropertyToID("_Reflection");

        public SteamWater(Material mat, float seaLevel, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, Transform parent)
        {
            _mat = mat;
            _seaLevel = seaLevel;
            _terrain = terrain ?? com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE;
            bool viking = (object)_terrain == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING;
            _targetDir = _flowDir; _targetSpeed = _flowSpeed;
            _waveSpeed = viking ? 0.8f : 0.4f;
            if (_mat != null)
            {
                float amp = 0.15f * (viking ? 1.5f : 1f), steep = 0.5f * (viking ? 1.2f : 1f);
                float[] amps = { amp, amp * 0.53f, amp * 0.27f };
                float[] lens = viking ? new[] { 50f, 28f, 14f } : new[] { 60f, 35f, 18f };
                for (int i = 0; i < 3; i++)
                {
                    _mat.SetVector("_WaveDirLen" + i, new Vector4(DirX[i], DirY[i], lens[i], 0f));
                    _mat.SetVector("_WaveAmpSteep" + i, new Vector4(amps[i], steep, 0f, 0f));
                }
                _mat.SetFloat("_DepthScale", viking ? 2f : 1f);
                _mat.SetFloat("_MinAlpha", 0.05f);
                _mat.SetFloat("_MaxAlpha", 0.6f);
                _mat.SetFloat("_RepeatRate", com.oddlabs.tt.global.Globals.WATER_REPEAT_RATE);
                _mat.SetFloat(HasReflId, 0f);
                Apply();
            }
            if (s_ocean.TryGetValue(_terrain, out var cached)) SetTextures(cached);
            else
            {
                var t = _terrain;
                _gen = Task.Run(() => new com.oddlabs.tt.procedural.GeneratorOcean(t).generate());
            }
            MakeReflectionCamera(parent);
        }

        void SetTextures(Texture2D[] tex)
        {
            if (_mat == null || tex == null) return;
            if (tex.Length > 0 && tex[0] != null) _mat.SetTexture("_Ocean0", tex[0]);
            if (tex.Length > 1 && tex[1] != null) _mat.SetTexture("_Ocean1", tex[1]);
            _texturesSet = true;
        }

        void MakeReflectionCamera(Transform parent)
        {
            var go = new GameObject("Water reflection (Steam)");
            go.transform.SetParent(parent, false);
            _reflCam = go.AddComponent<Camera>();
            _reflCam.cullingMask = ~((1 << WaterLayer) | (1 << UiLayer));
            _reflCam.clearFlags = CameraClearFlags.SolidColor;
            _reflCam.allowMSAA = false;
            _reflCam.allowHDR = true;
            var data = _reflCam.GetUniversalAdditionalCameraData();
            data.renderPostProcessing = false;
            data.renderShadows = false;
            data.requiresDepthOption = CameraOverrideOption.Off;
            data.requiresColorOption = CameraOverrideOption.Off;
            _reflCam.enabled = false;
        }

        /// <summary>Once a frame, after the main camera has its Java view.</summary>
        public void Update(Camera cam)
        {
            if (!_texturesSet && _gen != null && _gen.IsCompleted)
            {
                Texture2D[] tex = null;
                if (_gen.Status == TaskStatus.RanToCompletion && _gen.Result != null)
                {
                    var r = _gen.Result;
                    tex = new Texture2D[r.Length];
                    for (int i = 0; i < r.Length; i++)
                    {
                        // raw values (the Java samples the bytes as they are): a linear texture
                        tex[i] = SteamTerrain.ToTexture(r[i], false, TextureWrapMode.Repeat);
                        if (tex[i] != null) { tex[i].name = "Steam ocean " + i; tex[i].hideFlags = HideFlags.DontSave; }
                    }
                    s_ocean[_terrain] = tex;
                }
                else if (_gen.Exception != null) Debug.LogWarning("[TT Steam] ocean texture: " + _gen.Exception.GetBaseException().Message);
                _gen = null;
                SetTextures(tex);
                _texturesSet = true;
            }
            Animate();
            Apply();
            Reflection(cam);
        }

        void Animate()
        {
            float now = Time.unscaledTime;
            float dt = now - _lastTime;
            if (_lastTime < 0f || dt < 0f || dt > 1f) dt = 0.016f;
            _lastTime = now;
            _waveTime += dt;
            _sinceChange += dt;
            if (_sinceChange > _interval)
            {
                _sinceChange = 0f;
                _interval = 17.5f + Gauss() * 5f;
                _targetDir += Gauss() * 7.5f * Mathf.Deg2Rad;
                float change = _flowSpeed * Gauss() * 0.05f;
                _targetSpeed = Mathf.Clamp(_targetSpeed + change, 0.0005f, 0.002f);
            }
            _flowDir += (_targetDir - _flowDir) * dt * 0.5f;
            _flowSpeed += (_targetSpeed - _flowSpeed) * dt * 0.5f;
            _scroll0 += new Vector2(Mathf.Cos(_flowDir), Mathf.Sin(_flowDir)) * _flowSpeed * dt;
            float d2 = _flowDir + 90f * Mathf.Deg2Rad;
            _scroll1 += new Vector2(Mathf.Cos(d2), Mathf.Sin(d2)) * _flowSpeed * 0.7f * dt;
        }

        void Apply()
        {
            if (_mat == null) return;
            _mat.SetFloat(WaveTimeId, _waveTime * _waveSpeed);
            _mat.SetVector(ScrollId, new Vector4(_scroll0.x, _scroll0.y, _scroll1.x, _scroll1.y));
            _mat.SetFloat(DetailId, com.oddlabs.tt.global.Globals.draw_detail ? 1f : 0f);
        }

        float Gauss()
        {
            double u1 = 1.0 - _rng.NextDouble(), u2 = _rng.NextDouble();
            return (float)(System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2));
        }

        void Reflection(Camera cam)
        {
            bool on = cam != null && _mat != null && _reflCam != null && SteamGlobals.Detail != com.oddlabs.tt.global.Globals.DETAIL_LOW;
            _reflCam.enabled = on;
            _mat?.SetFloat(HasReflId, on ? 1f : 0f);
            if (!on) return;
            int w = Mathf.Max(64, cam.pixelWidth), h = Mathf.Max(64, cam.pixelHeight);
            if (_reflRT == null || _reflRT.width != w || _reflRT.height != h)
            {
                if (_reflRT != null) { _reflRT.Release(); Object.Destroy(_reflRT); }
                _reflRT = new RenderTexture(w, h, 24, RenderTextureFormat.DefaultHDR) { name = "Steam water reflection", hideFlags = HideFlags.DontSave };
                _reflCam.targetTexture = _reflRT;
                _mat.SetTexture(ReflId, _reflRT);
            }
            _reflCam.depth = cam.depth - 1f;
            _reflCam.backgroundColor = RenderSettings.fogColor;
            _reflCam.nearClipPlane = cam.nearClipPlane;
            _reflCam.farClipPlane = cam.farClipPlane;
            _reflCam.fieldOfView = cam.fieldOfView;
            _reflCam.aspect = cam.aspect;
            var plane = new Vector4(0f, 1f, 0f, -_seaLevel);
            var reflect = ReflectionMatrix(plane);
            _reflCam.transform.SetPositionAndRotation(reflect.MultiplyPoint(cam.transform.position), cam.transform.rotation);
            _reflCam.worldToCameraMatrix = cam.worldToCameraMatrix * reflect;
            // the oblique near plane at the sea level: nothing under the water is in the reflection (the Java draws
            // the patches above the sea and clips the models at the sea level)
            var clip = CameraSpacePlane(_reflCam.worldToCameraMatrix, new Vector3(0f, _seaLevel, 0f), Vector3.up, 0.05f);
            var proj = cam.projectionMatrix;
            _reflCam.projectionMatrix = proj;
            _reflCam.projectionMatrix = _reflCam.CalculateObliqueMatrix(clip);
        }

        static Matrix4x4 ReflectionMatrix(Vector4 p)
        {
            var m = Matrix4x4.identity;
            m.m00 = 1 - 2 * p.x * p.x; m.m01 = -2 * p.x * p.y; m.m02 = -2 * p.x * p.z; m.m03 = -2 * p.w * p.x;
            m.m10 = -2 * p.y * p.x; m.m11 = 1 - 2 * p.y * p.y; m.m12 = -2 * p.y * p.z; m.m13 = -2 * p.w * p.y;
            m.m20 = -2 * p.z * p.x; m.m21 = -2 * p.z * p.y; m.m22 = 1 - 2 * p.z * p.z; m.m23 = -2 * p.w * p.z;
            return m;
        }

        static Vector4 CameraSpacePlane(Matrix4x4 worldToCamera, Vector3 pos, Vector3 normal, float offset)
        {
            Vector3 cp = worldToCamera.MultiplyPoint(pos + normal * offset);
            Vector3 cn = worldToCamera.MultiplyVector(normal).normalized;
            return new Vector4(cn.x, cn.y, cn.z, -Vector3.Dot(cp, cn));
        }

        public void Dispose()
        {
            if (_reflCam != null) { _reflCam.targetTexture = null; Object.Destroy(_reflCam.gameObject); }
            if (_reflRT != null) { _reflRT.Release(); Object.Destroy(_reflRT); }
            _reflCam = null; _reflRT = null;
        }
    }
}
