using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Far-off land round the horizon (modern look): a ring of distant islands for tropical maps, a wall of stormy
    /// snow-capped mountains for northern ones, smoking volcanic isles for the Rift. One panorama per climate
    /// (Resources/TTReal/backdrop_tropic / _north / _rift, made by PortTools/backdrops/gen_backdrops.py from SDXL
    /// textures), drawn on a ring far beyond the island that moves with the camera, so it always lies on the horizon.
    /// The panorama carries its own haze; here it is only lit with the time of day.
    /// </summary>
    public static class TTBackdrop
    {
        const float Radius = 6500f;     // inside the camera's far plane (9 km), beyond any island
        const float Height = 900f;      // the top of the panorama: about 8 degrees over the horizon
        const int Segments = 160;

        static Mesh s_ring;
        static readonly Material[] s_mats = new Material[3];
        static readonly bool[] s_tried = new bool[3];
        static readonly MaterialPropertyBlock s_block = new MaterialPropertyBlock();
        static readonly int ColorId = Shader.PropertyToID("_Color");

        static Mesh Ring()
        {
            if (s_ring != null) return s_ring;
            var v = new Vector3[(Segments + 1) * 2];
            var uv = new Vector2[v.Length];
            var t = new int[Segments * 6];
            for (int i = 0; i <= Segments; i++)
            {
                float a = i * Mathf.PI * 2f / Segments;
                var d = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
                v[i * 2] = d * Radius + Vector3.down * 40f;   // a little below sea level: no gap at the waterline
                v[i * 2 + 1] = d * Radius + Vector3.up * Height;
                float u = i / (float)Segments * 2f;            // the panorama twice round
                uv[i * 2] = new Vector2(u, 0f);
                uv[i * 2 + 1] = new Vector2(u, 1f);
            }
            for (int i = 0; i < Segments; i++)
            {
                int a = i * 2, b = a + 2;
                t[i * 6] = a; t[i * 6 + 1] = a + 1; t[i * 6 + 2] = b + 1;
                t[i * 6 + 3] = a; t[i * 6 + 4] = b + 1; t[i * 6 + 5] = b;
            }
            var col = new Color[v.Length];
            for (int i = 0; i < col.Length; i++) col[i] = Color.white;
            s_ring = new Mesh { name = "Backdrop ring", vertices = v, uv = uv, colors = col, triangles = t };
            s_ring.bounds = new Bounds(Vector3.zero, Vector3.one * (Radius * 2f + Height * 2f));
            return s_ring;
        }

        static Material Mat(int type)
        {
            type = Mathf.Clamp(type, 0, 2);
            if (s_tried[type]) return s_mats[type];
            s_tried[type] = true;
            var tex = Resources.Load<Texture2D>("TTReal/backdrop_" + (type == 1 ? "north" : type == 2 ? "rift" : "tropic"));
            var sh = Shader.Find("TribalTrouble/WorldMarker");
            if (tex == null || sh == null) return null;
            tex.wrapModeU = TextureWrapMode.Repeat;
            tex.wrapModeV = TextureWrapMode.Clamp;
            if (tex.anisoLevel < 4) tex.anisoLevel = 4;
            var m = new Material(sh) { name = "Backdrop " + type, mainTexture = tex, hideFlags = HideFlags.DontSave };
            if (m.HasProperty("_ZTest")) m.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
            m.renderQueue = 2950;   // after the land, before the sea (the sea laps over its foot)
            s_mats[type] = m;
            return m;
        }

        /// <summary>Draws the backdrop round the camera (modern look only).</summary>
        public static void Draw(Camera cam, int type, float seaLevel)
        {
            if (cam == null || TribalTrouble.Game.GameSettings.graphicsStyle != 1) return;
            var mat = Mat(type);
            if (mat == null) return;
            var p = cam.transform.position;
            // lit like the sky near the horizon: full by day, dim and blue at night
            Color horizon = Shader.GetGlobalColor("_TT_SkyHorizon");
            float lum = Mathf.Clamp01(horizon.maxColorComponent * 1.6f);
            var c = Color.Lerp(new Color(0.16f, 0.18f, 0.26f, 1f), Color.white, lum);
            c = Color.Lerp(c, new Color(horizon.r, horizon.g, horizon.b, 1f) * 1.4f, 0.18f);
            c.a = 1f;
            s_block.Clear();
            s_block.SetColor(ColorId, c);
            Graphics.RenderMesh(new RenderParams(mat)
            {
                matProps = s_block,
                shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off,
                receiveShadows = false,
                camera = cam,
            }, Ring(), 0, Matrix4x4.Translate(new Vector3(p.x, seaLevel, p.z)));
        }
    }
}
