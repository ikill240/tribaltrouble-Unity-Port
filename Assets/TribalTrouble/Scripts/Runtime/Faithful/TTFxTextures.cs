using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The effects' particle sprites remastered with ComfyUI (SDXL on black, alpha from the brightness:
    /// PortTools/backdrops/gen_fx.py): Resources/TTReal/fx_smoke, fx_fire, fx_dust, fx_spark. The procedural soft
    /// blobs stay as the fallback (and in the classic look).
    /// </summary>
    public static class TTFxTextures
    {
        static readonly Dictionary<string, Texture2D> s_tex = new Dictionary<string, Texture2D>();

        public static Texture2D Get(string name, Texture2D fallback)
        {
            if (TribalTrouble.Game.GameSettings.graphicsStyle != 1) return fallback;
            if (!s_tex.TryGetValue(name, out var t))
            {
                t = Resources.Load<Texture2D>("TTReal/fx_" + name);
                if (t != null) t.wrapMode = TextureWrapMode.Clamp;
                s_tex[name] = t;
            }
            return t != null ? t : fallback;
        }
    }
}
