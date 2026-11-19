using System.Collections.Generic;
using System.IO;
using com.oddlabs.tt.model;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The New Tides building kit: the Extended buildings modelled offline (PortTools/buildings: kit.py, designs.py,
    /// textures.py) and stored as Resources/TTExtended/bk_&lt;model&gt;.bytes with their own tiling textures
    /// (bk_planks, bk_thatch, bk_stone, bk_hide ... and team masks). One model per race: the Vikings' Mead Hall, Rune
    /// Forge, Rune Stone and the Natives' Spirit Lodge, Crystal Forge, War Totem; a dock, a market and a stone circle
    /// for each. Parts named "wall" are rebuilt in stone when the building is upgraded. Missing files fall back to the
    /// old procedural models.
    /// </summary>
    public static partial class TTExtendedArt
    {
        public sealed class KitPart
        {
            public string name, tex, mask;
            public float gloss;
            public Mesh mesh;
        }

        static readonly Dictionary<string, KitPart[]> s_kit = new Dictionary<string, KitPart[]>();
        static readonly Dictionary<string, float> s_kitScale = new Dictionary<string, float>();
        static readonly Dictionary<string, bool> s_texExists = new Dictionary<string, bool>();

        /// <summary>The kit model (e.g. "hall_v"), or null when its file isn't there.</summary>
        public static KitPart[] Kit(string model)
        {
            if (s_kit.TryGetValue(model, out var have)) return have;
            KitPart[] parts = null;
            try
            {
                var ta = Resources.Load<TextAsset>("TTExtended/bk_" + model);
                if (ta != null) parts = Parse(ta.bytes, model);
            }
            catch (System.Exception e) { Debug.LogWarning("[Tribal Trouble] building kit " + model + ": " + e.Message); parts = null; }
            s_kit[model] = parts;
            return parts;
        }

        static KitPart[] Parse(byte[] data, string model)
        {
            using var br = new BinaryReader(new MemoryStream(data));
            if (br.ReadByte() != 'T' || br.ReadByte() != 'T' || br.ReadByte() != 'B' || br.ReadByte() != 'K') throw new IOException("not a kit file");
            int version = br.ReadInt32();
            s_kitScale[model] = version >= 2 ? br.ReadSingle() : 1f;   // the model's scale (its anchors scale with it)
            int n = br.ReadInt32();
            var parts = new KitPart[n];
            for (int i = 0; i < n; i++)
            {
                var p = new KitPart { name = Str(br), tex = Str(br), mask = Str(br), gloss = br.ReadSingle() };
                int nv = br.ReadInt32();
                var v = new Vector3[nv]; var nn = new Vector3[nv]; var uv = new Vector2[nv];
                for (int k = 0; k < nv; k++)
                {
                    v[k] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    nn[k] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                    uv[k] = new Vector2(br.ReadSingle(), br.ReadSingle());
                }
                int ni = br.ReadInt32();
                var t = new int[ni];
                for (int k = 0; k < ni; k++) t[k] = br.ReadInt32();
                var m = new Mesh { name = "Kit " + model + " " + p.name };
                m.indexFormat = nv > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
                m.vertices = v; m.normals = nn; m.uv = uv; m.triangles = t;
                m.RecalculateBounds();
                p.mesh = m;
                parts[i] = p;
            }
            return parts;
        }

        static string Str(BinaryReader br)
        {
            int len = br.ReadInt32();
            return len > 0 ? System.Text.Encoding.UTF8.GetString(br.ReadBytes(len)) : "";
        }

        /// <summary>The kit model's name for an Extended building of this race.</summary>
        static string KitName(int templateId, bool vik)
        {
            string r = vik ? "_v" : "_n";
            switch (templateId)
            {
                case Race.BUILDING_HALL: return "hall" + r;
                case Race.BUILDING_FORGE: return "forge" + r;
                case Race.BUILDING_TOTEM: return "totem" + r;
                case Race.BUILDING_DOCK: return "dock" + r;
                case Race.BUILDING_MARKET: return "market" + r;
                case Race.BUILDING_SHRINE: return "shrine" + r;
                default: return null;
            }
        }

        /// <summary>Draws the building from the kit; false when there's no kit model for it (the old one is drawn).
        /// An Elemental tribe's building wears the textures' Elemental versions (basalt, ochre, moss, ember cracks).</summary>
        static bool DrawKit(LandBuilding b, Matrix4x4 grow, Color team, Material bm)
        {
            var parts = Kit(KitName(((BuildingTemplate)b.getTemplate()).getTemplateID(), Vikings(b)) ?? "");
            if (parts == null) return false;
            bool elemental = b.getOwner().isElemental();
            foreach (var p in parts)
            {
                string tex = p.tex;   // (an upgraded building keeps its own textures: the tier skin goes over them - SkinOf)
                if (elemental) tex = ElementalTex(tex);
                Emit(p.mesh, Mat(tex, string.IsNullOrEmpty(p.mask) ? null : p.mask, team, bm, p.gloss), grow, true);
            }
            return true;
        }

        /// <summary>A texture's Elemental version (TTExtended/&lt;name&gt;_e) when there is one.</summary>
        internal static string ElementalTex(string tex)
        {
            string e = tex + "_e";
            if (!s_texExists.TryGetValue(e, out bool ok))
                s_texExists[e] = ok = Resources.Load<Texture2D>("TTExtended/" + e) != null;
            return ok ? e : tex;
        }

        /// <summary>How much bigger the kit model of this building is than the old one (its glows and rings scale with
        /// it); 1 when the old model is drawn.</summary>
        static float KitScale(LandBuilding b)
        {
            var name = KitName(((BuildingTemplate)b.getTemplate()).getTemplateID(), Vikings(b));
            if (name == null || Kit(name) == null) return 1f;
            return s_kitScale.TryGetValue(name, out float k) ? k : 1f;
        }

        /// <summary>The placing ghost's meshes for an Extended building of this race, or null.</summary>
        public static Mesh[] KitGhost(int templateId, bool vik)
        {
            var name = KitName(templateId, vik);
            var parts = name != null ? Kit(name) : null;
            if (parts == null) return null;
            var r = new Mesh[parts.Length];
            for (int i = 0; i < parts.Length; i++) r[i] = parts[i].mesh;
            return r;
        }

        /// <summary>Is this the Vikings' template (for ghosts, which have no owner yet)?</summary>
        public static bool VikingTemplate(BuildingTemplate t, com.oddlabs.tt.landscape.World world)
        {
            var rr = world.getRacesResources();
            return t == rr.getRace(RacesResources.RACE_VIKINGS).getBuildingTemplate(t.getTemplateID());
        }
    }
}
