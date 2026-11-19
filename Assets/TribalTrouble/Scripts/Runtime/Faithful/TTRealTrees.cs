using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Realistic trees for the Modern look: the Java tree types (jungle, palm, oak, pine) are drawn with photo-
    /// textured tree models from your other projects (Mobile_FV), picked per tree by its position, scaled to the
    /// original tree's height and shaded with TribalTrouble/Sprite (sun, shadows, wind, light through leaves, fog).
    /// The simulation is untouched: same tree positions, felling and wood.
    /// </summary>
    public static class TTRealTrees
    {
        public sealed class Part
        {
            public Mesh mesh;
            public int submesh;
            public Material material;
            public Matrix4x4 local;    // part -> prefab root, already scaled to the Tribal Trouble tree size
        }

        public sealed class Variant
        {
            public readonly List<Part> parts = new List<Part>();
            public float height;
            public bool odd;                       // painted for a seasonal / fantasy shader: teal and orange leaves
            public Variant tintOf;                 // a shade of another variant (the same meshes, its own tint)
            public Color tintMul = Color.white;
        }

        /// <summary>The shades every tree model also comes in (picked by position), so a forest isn't one green:
        /// as painted, a deeper cooler green, a sunnier warmer one.</summary>
        static readonly Color[] Shades = { Color.white, new Color(0.86f, 0.93f, 0.88f, 1f), new Color(1.08f, 1.05f, 0.88f, 1f) };

        static TTRealTreeSet s_set;
        static bool s_loaded;
        static readonly Dictionary<GameObject, Variant> s_variants = new Dictionary<GameObject, Variant>();
        static readonly Dictionary<Material, Material> s_mats = new Dictionary<Material, Material>();

        static TTRealTreeSet Set
        {
            get
            {
                if (!s_loaded) { s_loaded = true; s_set = Resources.Load<TTRealTreeSet>("TTRealTrees"); }
                return s_set;
            }
        }

        static GameObject[] List(string treeName)
        {
            var s = Set;
            if (s == null) return null;
            switch (treeName)
            {
                case "jungle_tree": return s.jungle;
                case "palm": return s.palm;
                case "oak_tree": return s.oak;
                case "pine_tree": return s.pine;
            }
            return null;
        }

        public static bool Has(string treeName) { var l = List(treeName); return l != null && l.Length > 0 && (!s_usable.TryGetValue(treeName, out var vs) || vs.Length > 0); }

        /// <summary>The model for one tree (stable choice from its position).</summary>
        public static Variant Pick(string treeName, float x, float y, float targetHeight, Material baseMaterial)
        {
            if (!s_usable.TryGetValue(treeName, out var vs))
            {
                // every model of the kind built once; those that can't be drawn cheaply are left out (no parts, or
                // hundreds of them that couldn't be merged - each part is a draw of its own in every pass)
                var l = List(treeName);
                var ok = new List<Variant>();
                if (l != null)
                    foreach (var prefab in l)
                    {
                        if (prefab == null) continue;
                        if (!s_variants.TryGetValue(prefab, out var bv)) { bv = Build(prefab, targetHeight, baseMaterial); s_variants[prefab] = bv; }
                        if (bv.parts.Count > 0 && bv.parts.Count <= MaxParts) ok.Add(bv);
                        else Debug.Log($"[TT] real tree {prefab.name} left out ({bv.parts.Count} parts)");
                    }
                // the models painted for Mobile_FV's seasonal / fantasy shaders (teal, orange and pink leaf cards that
                // shader recolours) came out as speckled rainbow trees here: left out while any natural one remains
                var natural = ok.FindAll(x => !x.odd);
                if (natural.Count > 0) ok = natural;
                var shaded = new List<Variant>();
                foreach (var bv in ok)
                    for (int k = 0; k < Shades.Length; k++) shaded.Add(k == 0 ? bv : Shade(bv, k));
                s_usable[treeName] = vs = shaded.ToArray();
            }
            if (vs.Length == 0) return null;
            int h = (int)(x * 73.13f) * 31 + (int)(y * 19.7f) * 17;
            return vs[(h & 0x7fffffff) % vs.Length];
        }

        static readonly Dictionary<(Material, int), Material> s_shadeMats = new Dictionary<(Material, int), Material>();

        /// <summary>A copy of the variant in shade k: the same meshes, each material's tint multiplied.</summary>
        static Variant Shade(Variant v, int k)
        {
            var c = new Variant { height = v.height, odd = v.odd, tintOf = v, tintMul = Shades[k] };
            foreach (var p in v.parts)
            {
                if (!s_shadeMats.TryGetValue((p.material, k), out var m))
                {
                    m = new Material(p.material) { name = p.material.name + " shade " + k };
                    if (m.HasProperty("_Tint")) m.SetColor("_Tint", p.material.GetColor("_Tint") * Shades[k]);
                    s_shadeMats[(p.material, k)] = m;
                }
                c.parts.Add(new Part { mesh = p.mesh, submesh = p.submesh, material = m, local = p.local });
            }
            return c;
        }

        const int MaxParts = 8;
        static readonly Dictionary<string, Variant[]> s_usable = new Dictionary<string, Variant[]>();

        static Variant Build(GameObject prefab, float targetHeight, Material baseMaterial)
        {
            var v = new Variant();
            var root = prefab.transform;
            var bounds = new Bounds();
            bool any = false;
            var filters = prefab.GetComponentsInChildren<MeshFilter>(true);
            foreach (var f in filters)
            {
                if (f.sharedMesh == null) continue;
                var m = root.worldToLocalMatrix * f.transform.localToWorldMatrix;
                var b = f.sharedMesh.bounds;
                foreach (var c in Corners(b))
                {
                    var p = m.MultiplyPoint3x4(c);
                    if (!any) { bounds = new Bounds(p, Vector3.zero); any = true; } else bounds.Encapsulate(p);
                }
            }
            v.height = any ? Mathf.Max(0.5f, bounds.max.y - Mathf.Min(0f, bounds.min.y)) : 1f;
            float s = targetHeight > 0 ? targetHeight / v.height : 1f;
            var scale = Matrix4x4.Scale(Vector3.one * s);
            foreach (var f in filters)
            {
                var r = f.GetComponent<MeshRenderer>();
                if (f.sharedMesh == null || r == null) continue;
                var mats = r.sharedMaterials;
                var m = scale * (root.worldToLocalMatrix * f.transform.localToWorldMatrix);
                for (int i = 0; i < f.sharedMesh.subMeshCount && i < mats.Length; i++)
                {
                    if (mats[i] == null) continue;
                    string mn = mats[i].name;
                    if (mn.Contains("Unique") || mn.Contains("Combined")) v.odd = true;
                    v.parts.Add(new Part { mesh = f.sharedMesh, submesh = i, material = Convert(mats[i], baseMaterial), local = m });
                }
            }
            Merge(v, prefab.name);
            return v;
        }

        /// <summary>The parts sharing a material merged into one mesh (in the tree's own space): a jungle tree is ~21
        /// parts, each its own instanced draw in every pass and shadow cascade - merged it is one or two. Parts whose
        /// mesh isn't readable (or that are mirrored, which would turn their faces inside out) stay as they are.</summary>
        static void Merge(Variant v, string name)
        {
            var groups = new Dictionary<Material, List<Part>>();
            var order = new List<Material>();
            foreach (var p in v.parts)
            {
                if (!groups.TryGetValue(p.material, out var l)) { groups[p.material] = l = new List<Part>(); order.Add(p.material); }
                l.Add(p);
            }
            int before = v.parts.Count, unreadable = 0, mirrored = 0;
            foreach (var p in v.parts) { if (p.mesh != null && !p.mesh.isReadable) unreadable++; if (p.local.determinant <= 0f) mirrored++; }
            if (order.Count == v.parts.Count) { Debug.Log($"[TT] real tree {name}: {before} parts, nothing to merge"); return; }
            var result = new List<Part>();
            foreach (var mat in order)
            {
                var l = groups[mat];
                var ok = new List<Part>();
                foreach (var p in l)
                {
                    if (p.mesh != null && p.mesh.isReadable && p.local.determinant > 0f && p.submesh < p.mesh.subMeshCount) ok.Add(p);
                    else result.Add(p);
                }
                if (ok.Count < 2) { result.AddRange(ok); continue; }
                try
                {
                    var ci = new CombineInstance[ok.Count];
                    long verts = 0;
                    for (int i = 0; i < ok.Count; i++)
                    {
                        ci[i] = new CombineInstance { mesh = ok[i].mesh, subMeshIndex = ok[i].submesh, transform = ok[i].local };
                        verts += ok[i].mesh.vertexCount;
                    }
                    var mesh = new Mesh { name = name + " " + mat.name + " (merged)" };
                    if (verts > 65000) mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                    mesh.CombineMeshes(ci, true, true, false);
                    mesh.RecalculateBounds();
                    mesh.UploadMeshData(true);
                    result.Add(new Part { mesh = mesh, submesh = 0, material = mat, local = Matrix4x4.identity });
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("[Tribal Trouble] couldn't merge the parts of " + name + ": " + e.Message);
                    result.AddRange(ok);
                }
            }
            v.parts.Clear();
            v.parts.AddRange(result);
            Debug.Log($"[TT] real tree {name}: {before} parts -> {result.Count} ({order.Count} materials, {unreadable} not readable, {mirrored} mirrored)");
        }

        static IEnumerable<Vector3> Corners(Bounds b)
        {
            for (int i = 0; i < 8; i++)
                yield return new Vector3((i & 1) == 0 ? b.min.x : b.max.x, (i & 2) == 0 ? b.min.y : b.max.y, (i & 4) == 0 ? b.min.z : b.max.z);
        }

        /// <summary>URP Lit (or similar) tree material -> TribalTrouble/Sprite with the same texture, cutout and tint.</summary>
        static Material Convert(Material src, Material baseMaterial)
        {
            if (s_mats.TryGetValue(src, out var m)) return m;
            m = baseMaterial != null ? new Material(baseMaterial) : new Material(Shader.Find("TribalTrouble/Sprite"));
            m.name = "Real tree " + src.name;
            m.enableInstancing = true;
            Texture tex = src.HasProperty("_BaseMap") ? src.GetTexture("_BaseMap") : null;
            if (tex == null && src.HasProperty("_MainTex")) tex = src.GetTexture("_MainTex");
            if (tex is Texture2D t2 && t2.anisoLevel < 8) t2.anisoLevel = 8;
            m.SetTexture("_BaseMap", tex != null ? tex : Texture2D.whiteTexture);
            var tint = src.HasProperty("_BaseColor") ? src.GetColor("_BaseColor") : src.HasProperty("_Color") ? src.GetColor("_Color") : Color.white;
            tint.a = 1f;
            m.SetColor("_Tint", tint);
            m.SetFloat("_UseTeamMask", 0f);
            bool cutout = src.IsKeywordEnabled("_ALPHATEST_ON") || (src.HasProperty("_AlphaClip") && src.GetFloat("_AlphaClip") > 0.5f)
                          || src.renderQueue >= 2450 || (tex != null && (src.name.ToLowerInvariant().Contains("atlas") || tex.name.ToLowerInvariant().Contains("atlas")))
                          // leaf cards painted on an atlas with transparency (Mobile_FV ships them as "opaque" URP Lit)
                          || (tex is Texture2D ta && UnityEngine.Experimental.Rendering.GraphicsFormatUtility.HasAlphaChannel(ta.graphicsFormat));
            if (cutout)
            {
                m.EnableKeyword("_ALPHATEST_ON");
                m.SetFloat("_AlphaClip", 1f);
                m.SetFloat("_Cutoff", src.HasProperty("_Cutoff") ? Mathf.Clamp(src.GetFloat("_Cutoff"), 0.2f, 0.7f) : 0.45f);
                m.SetFloat("_Cull", 0f);
            }
            m.SetFloat("_Wind", 1f);     // all parts bend together, so leaves stay on their branches
            m.SetFloat("_Gloss", cutout ? 0.12f : 0.25f);   // leaves: soft, not plastic
            m.SetFloat("_Relief", 0f);   // real geometry: no painted-relief bump (it turns leaf cards into noise)
            s_mats[src] = m;
            return m;
        }
    }
}
