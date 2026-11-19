// How the Steam release draws each sprite (its SpriteFile registrations: lighting, cullface, modulate_color), and the
// alpha test every sprite gets there (InstancedSpriteRenderer: ALPHA_TEST_VALUE 0.1 - see-through texels, such as the
// feathers of a headdress or the cut-out edges of a low-detail building, are dropped, not drawn as a dark quad):
//  - buildings (built, half built, started; ships) and their props: lit, both faces (cullface false)
//  - scenery (the registry's decorations): lit, both faces
//  - tree crowns: unlit (daylight is in the texture: texture x scene tint), both faces; trunks lit
//  - plants: modulated, unlit, both faces
//  - everything else (units, weapons, attachments, supplies, fragments, the rally flag): lit, back faces culled
using System.Collections.Generic;
using UnityEngine;

namespace TribalTroubleSteam.Presentation
{
    public static class SteamSpriteLook
    {
        public const int NoCull = 1, Unlit = 2;
        /// <summary>Java's InstancedSpriteRenderer alpha test is 0.1 with alpha to coverage under 4x MSAA, which shows a
        /// texel by how opaque it is; a plain test matches that look at the coverage's midpoint (at 0.1 the half
        /// transparent edge texels were drawn solid: the dark rims round feathers and weapons).</summary>
        public const float AlphaTest = 0.5f;

        static readonly Dictionary<TTSpriteDefinition, int> s_flags = new Dictionary<TTSpriteDefinition, int>();

        /// <summary>The first time a sprite is drawn, what it is drawn as (the scene's kind of it).</summary>
        public static void Note(TTSpriteDefinition def, TribalTroubleSteam.RKind kind)
        {
            if (def == null || s_flags.ContainsKey(def)) return;
            int f = ByName(def);
            if (kind == TribalTroubleSteam.RKind.Building || kind == TribalTroubleSteam.RKind.Prop || kind == TribalTroubleSteam.RKind.Scenery) f |= NoCull;
            if (kind == TribalTroubleSteam.RKind.Plants) f |= NoCull | Unlit;
            s_flags[def] = f;
        }

        public static int Flags(TTSpriteDefinition def)
        {
            if (def == null) return 0;
            return s_flags.TryGetValue(def, out int f) ? f : ByName(def);
        }

        static int ByName(TTSpriteDefinition def)
        {
            string n = (def.spriteName ?? "").ToLowerInvariant();
            if (n.Contains("crown")) return NoCull | Unlit;
            if (n.StartsWith("plant_") || n.StartsWith("viking_plant_")) return NoCull | Unlit;
            if (n.StartsWith("quarters") || n.StartsWith("armory") || n.StartsWith("tower") || n.StartsWith("ship")) return NoCull;
            return 0;
        }

        static readonly int CullId = Shader.PropertyToID("_Cull");
        static readonly int UnlitId = Shader.PropertyToID("_Unlit");
        static readonly int CutoffId = Shader.PropertyToID("_Cutoff");
        static readonly int AlphaClipId = Shader.PropertyToID("_AlphaClip");

        /// <summary>Sets a sprite material up the Steam release's way (only on the Steam shader, which has _Unlit).</summary>
        public static void Apply(Material mat, TTSpriteDefinition def, bool foliage)
        {
            if (mat == null || !mat.HasProperty(UnlitId)) return;
            int f = Flags(def);
            mat.SetFloat(UnlitId, (f & Unlit) != 0 ? 1f : 0f);
            mat.SetFloat(CullId, (f & NoCull) != 0 || foliage ? 0f : 2f);
            mat.EnableKeyword("_ALPHATEST_ON");
            mat.SetFloat(AlphaClipId, 1f);
            // (foliage keeps the port's own cut, tuned for Unity's mipmaps without alpha to coverage)
            if (!foliage) mat.SetFloat(CutoffId, AlphaTest);
        }
    }
}
