using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The resource rocks in the Modern look: seamless 1024 px ore textures (Resources/TTExtended/ore_*, colour - not TTReal, which imports as linear data - made by
    /// PortTools/ores/gen_ore_textures.py) in place of the Java game's 128 px generated rock and iron textures, and of
    /// the rock merely tinted for gold, coal and diamonds - grey stone with lichen, rust-red iron ore with lumps of
    /// metal, gold veins in quartz rock, black coal seams, pale rock studded with diamonds.
    /// </summary>
    public static class TTOres
    {
        static readonly Dictionary<string, Texture2D> s_tex = new Dictionary<string, Texture2D>();

        static string Name(string variant, int rare, bool ironMissing)
        {
            if (rare == 0) return "ore_gold";
            if (rare == 3) return "ore_coal";
            if (rare == 4) return "ore_diamond";
            if (rare >= 0) return null;
            if (ironMissing || (variant != null && variant.EndsWith("GeneratorIron"))) return "ore_iron";
            if (variant != null && variant.EndsWith("GeneratorRock")) return "ore_rock";
            return null;
        }

        /// <summary>The HD texture for a resource rock's variant (or null: the classic look, or not a resource rock).</summary>
        public static Texture2D For(string variant, int rare, bool ironMissing)
        {
            if (TribalTrouble.Game.GameSettings.graphicsStyle != 1) return null;
            string n = Name(variant, rare, ironMissing);
            if (n == null) return null;
            if (s_tex.TryGetValue(n, out var t)) return t;
            t = Resources.Load<Texture2D>("TTExtended/" + n);
            if (t != null) { t.wrapMode = TextureWrapMode.Repeat; if (t.anisoLevel < 8) t.anisoLevel = 8; }
            s_tex[n] = t;
            return t;
        }

        /// <summary>How much the ore shines: gold most, then diamonds and the metal in iron ore; plain stone a little.</summary>
        public static float Gloss(string variant, int rare, bool ironMissing)
        {
            switch (Name(variant, rare, ironMissing))
            {
                case "ore_gold": return 1.5f;
                case "ore_diamond": return 1.2f;
                case "ore_iron": return 0.8f;
                case "ore_coal": return 0.9f;
                default: return 0.35f;
            }
        }
    }
}
