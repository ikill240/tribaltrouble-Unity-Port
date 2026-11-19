using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Model textures your Java game generates at run time instead of loading ("Generator:com.oddlabs.tt.procedural.X"
    /// in the sprite files): the rock and iron supply textures (GeneratorRock, GeneratorIron), made by the translated
    /// generators and uploaded once.
    /// </summary>
    public static class TTGeneratedTextures
    {
        static readonly Dictionary<string, Texture2D> s_cache = new Dictionary<string, Texture2D>();
        const string Prefix = "Generator:";

        public static bool IsGenerated(string name) => name != null && name.StartsWith(Prefix);

        public static Texture2D Get(string name)
        {
            if (!IsGenerated(name)) return null;
            if (s_cache.TryGetValue(name, out var t)) return t;
            t = null;
            try
            {
                Faithful.FaithfulMatch.InitGlobals();
                com.oddlabs.tt.render.Texture[] tex = null;
                switch (name.Substring(Prefix.Length))
                {
                    case "com.oddlabs.tt.procedural.GeneratorRock": tex = new com.oddlabs.tt.procedural.GeneratorRock().generate(); break;
                    case "com.oddlabs.tt.procedural.GeneratorIron": tex = new com.oddlabs.tt.procedural.GeneratorIron().generate(); break;
                }
                if (tex != null && tex.Length > 0) t = Upload(tex[0]);
            }
            catch (System.Exception e) { Debug.LogWarning("[TT] generated texture " + name + ": " + e.Message); }
            s_cache[name] = t;
            return t;
        }

        static Texture2D Upload(com.oddlabs.tt.render.Texture t)
        {
            var img = t.images[0];
            int w = img.getWidth(), h = img.getHeight(), bpp = img.getPixelSize();
            var data = img.pixel_data.array();
            var px = new Color32[w * h];
            for (int i = 0; i < px.Length; i++)
            {
                if (bpp >= 3) px[i] = new Color32((byte)data[i * bpp], (byte)data[i * bpp + 1], (byte)data[i * bpp + 2], 255);
                else { byte v = (byte)data[i * bpp]; px[i] = new Color32(v, v, v, 255); }
            }
            // a colour texture like the models' own textures (sRGB)
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, true, false) { wrapMode = TextureWrapMode.Repeat, filterMode = FilterMode.Trilinear, anisoLevel = 4, name = "Generated rock" };
            tex.SetPixels32(px);
            tex.Apply(true, true);
            return tex;
        }
    }
}
