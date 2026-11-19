// The Steam release's own shading (Resources/TTSteam/Shaders, so they are in every build): its classic sprite
// lighting, its landscape, its decals, the placing ghost, particles and the sea, and the renderer's global state
// they read (DefaultRenderer.render: the light direction, ambient, sun colour and scene tint, day or night).
// The catalog's materials (TTSteamSprite / TTSteamTerrain / TTSteamWater) keep their textures and settings; the
// game draws with copies of them on these shaders.
using UnityEngine;

namespace TribalTroubleSteam
{
    public static class SteamShading
    {
        const string Dir = "TTSteam/Shaders/";

        static Shader Load(string file, string name)
        {
            var s = Resources.Load<Shader>(Dir + file);
            if (s == null) s = Shader.Find(name);
            if (s != null && !s.isSupported) { Debug.LogWarning("[TT Steam] shader not supported here: " + name); s = null; }
            return s;
        }

        static Shader s_sprite, s_terrain, s_water, s_decal, s_ghost, s_particle, s_blast, s_point;
        static bool s_loaded;

        static void EnsureLoaded()
        {
            if (s_loaded) return;
            s_loaded = true;
            s_sprite = Load("TTSteamSprite", "TribalTroubleSteam/Sprite");
            s_terrain = Load("TTSteamTerrain", "TribalTroubleSteam/Terrain");
            s_water = Load("TTSteamWater", "TribalTroubleSteam/Water");
            s_decal = Load("TTSteamDecal", "TribalTroubleSteam/Decal");
            s_ghost = Load("TTSteamGhost", "TribalTroubleSteam/Ghost");
            s_particle = Load("TTSteamParticle", "TribalTroubleSteam/Particle");
            s_blast = Load("TTSteamSonicBlast", "TribalTroubleSteam/SonicBlast");
            s_point = Load("TTSteamMapPoint", "TribalTroubleSteam/MapPoint");
        }

        public static Shader Sprite { get { EnsureLoaded(); return s_sprite; } }
        public static Shader Terrain { get { EnsureLoaded(); return s_terrain; } }
        public static Shader Water { get { EnsureLoaded(); return s_water; } }
        public static Shader Decal { get { EnsureLoaded(); return s_decal; } }
        public static Shader Ghost { get { EnsureLoaded(); return s_ghost; } }
        public static Shader Particle { get { EnsureLoaded(); return s_particle; } }
        public static Shader SonicBlast { get { EnsureLoaded(); return s_blast; } }
        public static Shader MapPoint { get { EnsureLoaded(); return s_point; } }

        /// <summary>A copy of a catalog material on the Steam shader (its textures and settings kept); the material
        /// itself when the shader is missing.</summary>
        public static Material OnShader(Material template, Shader shader, string name)
        {
            if (template == null || shader == null) return template;
            if (template.shader == shader) return template;
            var m = new Material(template) { name = name, hideFlags = HideFlags.DontSave };
            m.shader = shader;
            return m;
        }

        // ---- DefaultRenderer's lighting (classic: ambient 0.65, white sun; night: its moonlight and tint)
        static readonly Vector3 AmbientDay = new Vector3(0.65f, 0.65f, 0.65f);
        static readonly Vector3 SunDay = new Vector3(1f, 1f, 1f);
        static readonly Vector3 TintDay = new Vector3(1f, 1f, 1f);
        static readonly Vector3 AmbientNight = new Vector3(0.22f, 0.25f, 0.36f);
        static readonly Vector3 SunNight = new Vector3(0.34f, 0.40f, 0.58f);
        public static readonly Vector3 TintNight = new Vector3(0.30f, 0.35f, 0.52f);

        static readonly int SunDirId = Shader.PropertyToID("_TTS_SunDir");
        static readonly int SunColorId = Shader.PropertyToID("_TTS_SunColor");
        static readonly int AmbientId = Shader.PropertyToID("_TTS_Ambient");
        static readonly int TintId = Shader.PropertyToID("_TTS_SceneTint");
        static readonly int DynamicId = Shader.PropertyToID("_TTS_Dynamic");
        static readonly int ReadyId = Shader.PropertyToID("_TTS_Ready");

        public static Color SceneTint(bool night) { var t = night ? TintNight : TintDay; return new Color(t.x, t.y, t.z, 1f); }

        /// <summary>The global state for this frame (gamma values, as the Java shaders take them).</summary>
        public static void SetGlobals(bool night, bool dynamicShadows)
        {
            var dir = com.oddlabs.tt.global.Globals.REAL_SKY_BODY_DIRECTION;
            var toSun = TTSpace.ToUnity(dir.x(), dir.y(), dir.z()).normalized;
            Vector3 a = night ? AmbientNight : AmbientDay, s = night ? SunNight : SunDay, t = night ? TintNight : TintDay;
            Shader.SetGlobalVector(SunDirId, new Vector4(toSun.x, toSun.y, toSun.z, 0f));
            Shader.SetGlobalVector(SunColorId, new Vector4(s.x, s.y, s.z, 1f));
            Shader.SetGlobalVector(AmbientId, new Vector4(a.x, a.y, a.z, 1f));
            Shader.SetGlobalVector(TintId, new Vector4(t.x, t.y, t.z, 1f));
            Shader.SetGlobalFloat(DynamicId, dynamicShadows ? 1f : 0f);
            Shader.SetGlobalFloat(ReadyId, 1f);
        }
    }
}
