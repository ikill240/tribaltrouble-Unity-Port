using System.Collections.Generic;
using TribalTrouble.Presentation;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Realistic thrown weapons for the Modern look: the Viking axe and the Native spear are drawn with the
    /// photo-textured MeleeWeaponsPack models from your Stick City project, fitted onto the original model (same
    /// length, head at the same end, blade on the same side) and textured per tier with HD atlases made to match the
    /// original look: a stone head for rock, steel for iron, the bright toy colours for rubber.
    /// The simulation is untouched.
    /// </summary>
    public static class TTRealWeapons
    {
        static TTRealWeaponSet s_set;
        static bool s_loaded;
        static readonly Dictionary<(TTSpriteDefinition, int), TTRealTrees.Part> s_parts = new Dictionary<(TTSpriteDefinition, int), TTRealTrees.Part>();
        static readonly Dictionary<string, Material> s_mats = new Dictionary<string, Material>();

        static TTRealWeaponSet Set
        {
            get
            {
                if (!s_loaded) { s_loaded = true; s_set = Resources.Load<TTRealWeaponSet>("TTRealWeapons"); }
                return s_set;
            }
        }

        /// <summary>The warriors' held-weapon data (null before Build Real Weapons has run).</summary>
        public static TTRealWeaponSet.HeldWeapon[] Held => Set != null && Set.version >= 5 ? Set.held : null;

        /// <summary>The replacement for a weapon sprite (null: draw the original).</summary>
        public static TTRealTrees.Part Get(TTSpriteDefinition def, int texture, Material baseMaterial)
        {
            if (def == null || def.model == null) return null;
            var key = (def, texture);
            if (s_parts.TryGetValue(key, out var part)) return part;
            part = null;
            try { part = Build(def, texture, baseMaterial); }
            catch (System.Exception e) { Debug.LogWarning("[Tribal Trouble] Real weapon for " + def.Key + " failed: " + e.Message); }
            s_parts[key] = part;
            return part;
        }

        static TTRealTrees.Part Build(TTSpriteDefinition def, int texture, Material baseMaterial)
        {
            var set = Set;
            if (set == null) return null;
            Mesh mesh;
            Matrix4x4 local;
            // the fit is made in the editor (Build Real Weapons) and stored: players can't read mesh vertices
            if (def.spriteName == "axe" && set.axeFit != null) { mesh = set.axeFit; local = set.axeLocal; }
            else if (def.spriteName == "spear" && set.spearFit != null) { mesh = set.spearFit; local = set.spearLocal; }
            else if (!Application.isEditor || !Fit(def, def.spriteName == "axe" ? set.axes : def.spriteName == "spear" ? set.spears : null, out mesh, out local)) return null;
            string race = def.group != null && def.group.Contains("native") ? "native" : "viking";
            string tier = texture == 0 ? race + "_rock" : texture == 2 ? race + "_rubber" : "weapons_iron";
            return new TTRealTrees.Part { mesh = mesh, submesh = 0, material = MaterialFor(tier, baseMaterial), local = local };
        }

        /// <summary>
        /// Picks the candidate closest in proportions to the original weapon and fits it onto it (same length, head at
        /// the same end, blade on the same side). Reads mesh vertices, so it runs in the editor.
        /// </summary>
        public static bool Fit(TTSpriteDefinition def, Mesh[] candidates, out Mesh best, out Matrix4x4 local)
        {
            best = null;
            local = Matrix4x4.identity;
            if (def == null || def.model == null || candidates == null || candidates.Length == 0) return false;

            // the original weapon's vertices in model-root space
            var orig = new List<Vector3>();
            var root = def.model.transform;
            foreach (var f in def.model.GetComponentsInChildren<MeshFilter>(true))
                if (f.sharedMesh != null) Collect(f.sharedMesh, root.worldToLocalMatrix * f.transform.localToWorldMatrix, orig);
            foreach (var s in def.model.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                if (s.sharedMesh != null) Collect(s.sharedMesh, root.worldToLocalMatrix * s.transform.localToWorldMatrix, orig);
            return FitPoints(orig, candidates, out best, out local);
        }

        /// <summary>Fits the best candidate onto a weapon given by its vertices (any space).</summary>
        public static bool FitPoints(List<Vector3> orig, Mesh[] candidates, out Mesh best, out Matrix4x4 local)
        {
            best = null;
            local = Matrix4x4.identity;
            if (orig == null || orig.Count < 4 || candidates == null) return false;
            var fo = Frame.Of(orig);

            Frame fb = default;
            float bestScore = float.MaxValue;
            foreach (var c in candidates)
            {
                if (c == null) continue;
                var v = new List<Vector3>(c.vertices);
                if (v.Count < 4) continue;
                var fr = Frame.Of(v);
                float score = Mathf.Abs(Mathf.Log(fr.ratio2 / fo.ratio2)) + 0.5f * Mathf.Abs(Mathf.Log(fr.ratio3 / fo.ratio3));
                if (score < bestScore) { bestScore = score; best = c; fb = fr; }
            }
            if (best == null) return false;

            // real frame -> original frame: long axis (head end to head end), blade side to blade side, same length
            var R = Matrix4x4.identity;
            R.SetColumn(0, fo.a1); R.SetColumn(1, fo.a2); R.SetColumn(2, fo.a3);
            var Rr = Matrix4x4.identity;
            Rr.SetColumn(0, fb.a1); Rr.SetColumn(1, fb.a2); Rr.SetColumn(2, fb.a3);
            float scale = fo.length / Mathf.Max(1e-4f, fb.length);
            // shaft onto shaft (not box centre onto box centre: the heads differ, the grip must not)
            local = Matrix4x4.Translate(fo.shaft) * R * Matrix4x4.Scale(Vector3.one * scale) * Rr.transpose * Matrix4x4.Translate(-fb.shaft);
            return true;
        }

        /// <summary>Tier material for a warrior's texture variant (0 rock, 1 iron, 2 rubber).</summary>
        public static Material TierMaterial(bool natives, int variant, Material baseMaterial)
        {
            string race = natives ? "native" : "viking";
            return MaterialFor(variant == 0 ? race + "_rock" : variant == 2 ? race + "_rubber" : "weapons_iron", baseMaterial);
        }

        /// <summary>
        /// Swaps a warrior instance's painted weapon for the real one: the skinned mesh without the weapon triangles,
        /// plus the real weapon parented to the weapon bone. Returns true if anything was replaced.
        /// </summary>
        /// <summary>How much bigger the real axes and spears are drawn than the painted ones they replace.</summary>
        public const float HeldScale = 1.18f;

        public static bool ApplyHeld(GameObject root, string spriteKey, int variant, Material baseMaterial)
        {
            var set = Set;
            if (set == null || set.held == null || set.held.Length == 0) return false;
            bool any = false;
            foreach (var smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                foreach (var h in set.held)
                {
                    if (h == null || h.sprite != spriteKey || h.original != smr.sharedMesh || h.stripped == null) continue;
                    smr.sharedMesh = h.stripped;
                    Transform bone = null;
                    foreach (var b in smr.bones) if (b != null && b.name == h.bone) { bone = b; break; }
                    if (bone != null && h.weapon != null && bone.Find("Real weapon") == null)
                    {
                        var go = new GameObject("Real weapon");
                        go.transform.SetParent(bone, false);
                        // a little bigger than the painted original (read better at game distance), grown about the hand
                        go.transform.localPosition = (Vector3)h.local.GetColumn(3) * HeldScale;
                        go.transform.localRotation = h.local.rotation;
                        go.transform.localScale = h.local.lossyScale * HeldScale;
                        go.AddComponent<MeshFilter>().sharedMesh = h.weapon;
                        go.AddComponent<MeshRenderer>().sharedMaterial = TierMaterial(h.spear, variant, baseMaterial);
                        if (!string.IsNullOrEmpty(h.bone2) && h.anchorInBone.sqrMagnitude > 1e-6f)
                        {
                            Transform b2 = null;
                            foreach (var b in smr.bones) if (b != null && b.name == h.bone2) { b2 = b; break; }
                            if (b2 != null)
                            {
                                var al = go.AddComponent<TTHeldWeaponAligner>();
                                al.bone = bone; al.bone2 = b2; al.anchorInBone = h.anchorInBone; al.anchorInBone2 = h.anchorInBone2;
                                al.basePos = go.transform.localPosition; al.baseRot = go.transform.localRotation;
                                foreach (var b in smr.bones) if (b != null && b.name.EndsWith("R Hand")) { al.hand = b; break; }
                            }
                        }
                    }
                    any = true;
                }
            }
            return any;
        }

        static void Collect(Mesh mesh, Matrix4x4 m, List<Vector3> into)
        {
            foreach (var p in mesh.vertices) into.Add(m.MultiplyPoint3x4(p));
        }

        public static Material MaterialFor(string tier, Material baseMaterial)
        {
            if (s_mats.TryGetValue(tier, out var m)) return m;
            var tex = Resources.Load<Texture2D>("TTRealWeapons/" + tier);
            if (tex != null && tex.anisoLevel < 8) tex.anisoLevel = 8;
            m = baseMaterial != null ? new Material(baseMaterial) : new Material(Shader.Find("TribalTrouble/Sprite"));
            m.name = "Real weapon " + tier;
            m.enableInstancing = true;
            m.SetTexture("_BaseMap", tex != null ? tex : Texture2D.grayTexture);
            m.SetColor("_Tint", Color.white);
            m.SetFloat("_UseTeamMask", 0f);
            m.SetFloat("_Wind", 0f);
            m.SetFloat("_Gloss", tier.EndsWith("rock") ? 0.2f : 1f);
            m.SetFloat("_Relief", 0.35f);
            m.SetFloat("_Cull", 0f);
            s_mats[tier] = m;
            return m;
        }

        /// <summary>
        /// A weapon's frame from its vertices: a1 = long axis pointing to the head (the end with the wider cross
        /// section), a2 = second axis pointing to the side the head sticks out to (the axe blade), a3 = a1 x a2.
        /// </summary>
        public struct Frame
        {
            public Vector3 a1, a2, a3, center;
            /// <summary>The point on the shaft's line level with the centre: the bounding-box centre sits off to the
            /// blade side by half the head's width, the shaft (where the hand is) does not.</summary>
            public Vector3 shaft;
            public float length, ratio2, ratio3;

            public static Frame Of(List<Vector3> v)
            {
                var b = new Bounds(v[0], Vector3.zero);
                foreach (var p in v) b.Encapsulate(p);
                var size = b.size;
                int i1 = 0;
                for (int i = 1; i < 3; i++) if (size[i] > size[i1]) i1 = i;
                int i2 = (i1 + 1) % 3, i3 = (i1 + 2) % 3;
                if (size[i3] > size[i2]) { int t = i2; i2 = i3; i3 = t; }
                var f = new Frame { center = b.center, length = size[i1] };
                f.ratio2 = Mathf.Max(1e-3f, size[i2]) / Mathf.Max(1e-3f, size[i1]);
                f.ratio3 = Mathf.Max(1e-3f, size[i3]) / Mathf.Max(1e-3f, size[i1]);
                // the ends: the 25% of the length at each end
                float lo = b.min[i1], hi = b.max[i1], cut = size[i1] * 0.25f;
                float wLo = 0, wHi = 0, sLo = 0, sHi = 0;
                int nLo = 0, nHi = 0;
                float c2 = b.center[i2], c3 = b.center[i3];
                foreach (var p in v)
                {
                    float w = Mathf.Abs(p[i2] - c2) + Mathf.Abs(p[i3] - c3);
                    if (p[i1] < lo + cut) { wLo = Mathf.Max(wLo, w); sLo += p[i2]; nLo++; }
                    else if (p[i1] > hi - cut) { wHi = Mathf.Max(wHi, w); sHi += p[i2]; nHi++; }
                }
                bool headHigh = wHi >= wLo;
                f.a1 = Axis(i1) * (headHigh ? 1f : -1f);
                // the head's offset from the shaft along the second axis
                float headMean = headHigh ? (nHi > 0 ? sHi / nHi : c2) : (nLo > 0 ? sLo / nLo : c2);
                float shaftMean = headHigh ? (nLo > 0 ? sLo / nLo : c2) : (nHi > 0 ? sHi / nHi : c2);
                f.a2 = Axis(i2) * (headMean >= shaftMean ? 1f : -1f);
                f.a3 = Vector3.Cross(f.a1, f.a2);
                // the shaft: the middle of the cross section over the butt half (away from the head)
                float m2 = 0f, m3 = 0f; int nb = 0;
                float buttLo = headHigh ? lo : hi - size[i1] * 0.5f, buttHi = headHigh ? lo + size[i1] * 0.5f : hi;
                float b2lo = float.MaxValue, b2hi = float.MinValue, b3lo = float.MaxValue, b3hi = float.MinValue;
                foreach (var p in v)
                    if (p[i1] >= buttLo && p[i1] <= buttHi)
                    {
                        b2lo = Mathf.Min(b2lo, p[i2]); b2hi = Mathf.Max(b2hi, p[i2]);
                        b3lo = Mathf.Min(b3lo, p[i3]); b3hi = Mathf.Max(b3hi, p[i3]);
                        nb++;
                    }
                f.shaft = f.center;
                if (nb >= 3)
                {
                    m2 = (b2lo + b2hi) * 0.5f; m3 = (b3lo + b3hi) * 0.5f;
                    f.shaft[i2] = m2; f.shaft[i3] = m3;
                }
                return f;
            }

            static Vector3 Axis(int i) => i == 0 ? Vector3.right : i == 1 ? Vector3.up : Vector3.forward;
        }
    }
}

