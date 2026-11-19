using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.Presentation
{
    /// <summary>
    /// Far units, drawn in bulk: the low-detail model of a sprite baked once per animation frame (posed from
    /// TTPoseCache, then SkinnedMeshRenderer.BakeMesh) into an ordinary mesh, so every unit far from the camera is one
    /// instance of "this model, this frame" in a single instanced draw instead of a skinned GameObject of its own.
    /// Frames snap to the nearest key (no tween) - at that distance nobody sees the difference. A few new frames are
    /// baked per frame; until a frame is ready the nearest baked one of the same animation is used.
    /// </summary>
    public static class TTPoseBaker
    {
        sealed class Model
        {
            public GameObject go;
            public SkinnedMeshRenderer smr;
            public Transform[] bones;
            public TTPoseCache poses;
            public Matrix4x4 bakedToRoot;
            public int lod;
            public readonly Dictionary<int, Mesh> meshes = new Dictionary<int, Mesh>();
        }

        static readonly Dictionary<(TTSpriteDefinition, int), Model> s_models = new Dictionary<(TTSpriteDefinition, int), Model>();
        static readonly HashSet<(TTSpriteDefinition, int)> s_failed = new HashSet<(TTSpriteDefinition, int)>();
        static int s_budget;
        static Transform s_root;

        /// <summary>How many new frames may be baked this frame.</summary>
        public static void BeginFrame(int budget = 24) { s_budget = budget; s_vatBudget = 16; }

        /// <summary>The LOD the far model uses (the lowest detail one).</summary>
        public static int FarLod(TTSpriteDefinition def) => def.lods.Length > 1 ? def.lods.Length - 1 : 0;

        static Model Make(TTSpriteDefinition def, int lod)
        {
            var mk = (def, lod);
            if (s_models.TryGetValue(mk, out var md)) return md;
            if (s_failed.Contains(mk)) return null;
            try
            {
                var poses = TTPoseCache.For(def);
                if (poses == null || lod >= def.lods.Length) { s_failed.Add(mk); return null; }
                if (s_root == null) { var r = new GameObject("TT far-unit bakers") { hideFlags = HideFlags.HideAndDontSave }; r.SetActive(true); s_root = r.transform; }
                var go = Object.Instantiate(def.model, s_root, false);
                go.hideFlags = HideFlags.HideAndDontSave;
                foreach (var r in go.GetComponentsInChildren<Renderer>(true)) r.enabled = false;
                var animRoot = TTSpriteView.FindAnimationRoot(go);
                var node = TTSpriteView.FindChildRecursive(go.transform, def.lods[lod].nodeName);
                var smr = node != null ? node.GetComponent<SkinnedMeshRenderer>() : null;
                var bones = poses.Resolve(animRoot);
                if (smr == null || smr.sharedMesh == null || bones == null) { Object.Destroy(go); s_failed.Add(mk); return null; }
                // baked vertices are in the renderer's space without its scale: back to the model's root
                var t = smr.transform;
                var bakedToRoot = go.transform.worldToLocalMatrix * Matrix4x4.TRS(t.position, t.rotation, Vector3.one);
                md = new Model { go = go, smr = smr, bones = bones, poses = poses, bakedToRoot = bakedToRoot, lod = lod };
                s_models[mk] = md;
                return md;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("[Tribal Trouble] far model for " + def.Key + ": " + e.Message);
                s_failed.Add(mk);
                return null;
            }
        }

        /// <summary>The frame index (0..frames-1) nearest a unit's animation state (TTSpriteView.CycleTime's mapping).</summary>
        static int Frame(TTSpriteDefinition.AnimInfo a, float ticks)
        {
            int n = Mathf.Max(1, a.frames);
            float pos = ticks / Mathf.Max(a.wpc, 1e-6f);
            if (a.loop)
            {
                float t = pos - Mathf.Floor(pos);
                int f = Mathf.RoundToInt(t * n);
                return f >= n ? 0 : f;
            }
            float last = (n - 1f) / n;
            return Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp(pos, 0f, last) * n), 0, n - 1);
        }

        /// <summary>The baked mesh for a unit's animation state and the matrix from it to the unit's root; false if it
        /// can't be drawn this way (no animation, no skinned model, nothing baked yet).</summary>
        public static bool Get(TTSpriteDefinition def, int wantLod, int anim, float ticks, out Mesh mesh, out Matrix4x4 bakedToRoot, out int lod, int maxFrames = 0)
        {
            mesh = null; bakedToRoot = Matrix4x4.identity; lod = 0;
            if (def == null || def.animations.Length == 0 || def.lods.Length == 0) return false;
            var md = Make(def, Mathf.Clamp(wantLod, 0, def.lods.Length - 1));
            if (md == null) return false;
            anim = Mathf.Clamp(anim, 0, def.animations.Length - 1);
            var a = def.animations[anim];
            if (a == null || a.frames <= 0) return false;
            int f = Frame(a, ticks);
            // fewer distinct poses the further away (each pose is a draw of its own): every q-th frame only
            if (maxFrames > 0 && a.frames > maxFrames)
            {
                int q = (a.frames + maxFrames - 1) / maxFrames;
                f = f / q * q;
            }
            int key = anim * 4096 + f;
            if (!md.meshes.TryGetValue(key, out mesh))
            {
                if (s_budget > 0)
                {
                    s_budget--;
                    mesh = Bake(md, anim, f, a.frames);
                    md.meshes[key] = mesh;
                }
                else
                {
                    // not baked yet: the nearest baked frame of this animation, or nothing
                    for (int d = 1; d < a.frames && mesh == null; d++)
                    {
                        if (md.meshes.TryGetValue(anim * 4096 + (f + d) % a.frames, out mesh)) break;
                        if (md.meshes.TryGetValue(anim * 4096 + (f - d + a.frames) % a.frames, out mesh)) break;
                    }
                }
            }
            if (mesh == null) return false;
            bakedToRoot = md.bakedToRoot;
            lod = md.lod;
            return true;
        }

        // ---- every pose of a model in a texture (TTSprite's _TT_VAT): the GPU blends each unit's two poses itself, so
        // units drawn in bulk animate as smoothly as skinned ones, and one instanced draw takes every pose and team

        public sealed class Vat
        {
            public Texture2D tex;
            public Mesh mesh;
            public Mesh shadowMesh;   // mesh without the painted weapon (the shadow it casts), or null
            public Matrix4x4 bakedToRoot;
            public int lod, oversample;
            public int[] rowStart;   // first pose row of each animation (-1: none)
            public bool ready;
            internal int nextAnim, nextSample, totalRows, vertexCount;
            internal ushort[] data;
            internal Vector3 bmin, bmax;
        }

        static readonly Dictionary<(TTSpriteDefinition, int), Vat> s_vats = new Dictionary<(TTSpriteDefinition, int), Vat>();
        static int s_vatBudget;
        /// <summary>Off: units in bulk use the baked meshes of their nearest pose (no shader support found).</summary>
        public static bool VatEnabled = true;
        const int VatOversample = 2;   // poses per animation frame (the GPU blends between them; more = closer to the bones' tween)

        /// <summary>The pose texture of a model, or null while it is still being worked out (a few poses a frame) or
        /// when it can't be made.</summary>
        public static Vat GetVat(TTSpriteDefinition def, int wantLod)
        {
            if (!VatEnabled || def == null || def.animations.Length == 0 || def.lods.Length == 0) return null;
            int lod = Mathf.Clamp(wantLod, 0, def.lods.Length - 1);
            var key = (def, lod);
            if (s_vats.TryGetValue(key, out var v))
            {
                if (v == null || v.ready) return v;
                if (v.tex != null) Continue(def, v);
                return v.ready ? v : null;
            }
            var md = Make(def, lod);
            if (md == null) { s_vats[key] = null; return null; }
            v = StartVat(def, md);
            s_vats[key] = v;
            if (v != null) Continue(def, v);
            return v != null && v.ready ? v : null;
        }

        static Vat StartVat(TTSpriteDefinition def, Model md)
        {
            int vc = md.smr.sharedMesh.vertexCount;
            if (vc <= 0 || vc > 16384) return null;
            int os = VatOversample, rows = 0;
            var starts = new int[def.animations.Length];
            for (int c = 0; c < def.animations.Length; c++)
            {
                var a = def.animations[c];
                if (a == null || a.frames <= 0) { starts[c] = -1; continue; }
                starts[c] = rows;
                rows += a.frames * os;
            }
            if (rows == 0) return null;
            if (rows * 2 > 16384) { os = 1; rows = 0; for (int c = 0; c < def.animations.Length; c++) { var a = def.animations[c]; if (a == null || a.frames <= 0) continue; starts[c] = rows; rows += a.frames; } }
            if (rows * 2 > 16384) return null;
            var tex = new Texture2D(vc, rows * 2, TextureFormat.RGBAHalf, false, true)
            {
                name = "TT poses " + def.Key + " lod" + md.lod, filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp, hideFlags = HideFlags.HideAndDontSave,
            };
            return new Vat
            {
                tex = tex, bakedToRoot = md.bakedToRoot, lod = md.lod, oversample = os, rowStart = starts, totalRows = rows, vertexCount = vc,
                data = new ushort[vc * rows * 2 * 4], bmin = Vector3.one * float.MaxValue, bmax = Vector3.one * float.MinValue,
            };
        }

        static Mesh s_tmp;
        static readonly List<Vector3> s_pos = new List<Vector3>(), s_nrm = new List<Vector3>();

        /// <summary>A few more poses into the texture (within this frame's budget); the texture goes up once complete.</summary>
        static void Continue(TTSpriteDefinition def, Vat v)
        {
            var md = s_models.TryGetValue((def, v.lod), out var m) ? m : null;
            if (md == null) { v.tex = null; return; }
            s_tmp ??= new Mesh { hideFlags = HideFlags.HideAndDontSave };
            while (s_vatBudget > 0 && v.nextAnim < def.animations.Length)
            {
                int c = v.nextAnim;
                var a = def.animations[c];
                if (v.rowStart[c] < 0 || a == null || a.frames <= 0) { v.nextAnim++; v.nextSample = 0; continue; }
                int samples = a.frames * v.oversample;
                s_vatBudget--;
                int sm = v.nextSample;
                if (!md.poses.Apply(c, (float)sm / samples, md.bones)) { v.tex = null; return; }
                md.smr.BakeMesh(s_tmp, false);
                s_tmp.GetVertices(s_pos);
                s_tmp.GetNormals(s_nrm);
                if (s_pos.Count != v.vertexCount) { v.tex = null; return; }
                int row = (v.rowStart[c] + sm) * 2, w = v.vertexCount;
                bool normals = s_nrm.Count == w;
                for (int i = 0; i < w; i++)
                {
                    var p = s_pos[i];
                    v.bmin = Vector3.Min(v.bmin, p); v.bmax = Vector3.Max(v.bmax, p);
                    int o = (row * w + i) * 4;
                    v.data[o] = Mathf.FloatToHalf(p.x); v.data[o + 1] = Mathf.FloatToHalf(p.y); v.data[o + 2] = Mathf.FloatToHalf(p.z); v.data[o + 3] = Mathf.FloatToHalf(1f);
                    var n = normals ? s_nrm[i] : Vector3.up;
                    int q = ((row + 1) * w + i) * 4;
                    v.data[q] = Mathf.FloatToHalf(n.x); v.data[q + 1] = Mathf.FloatToHalf(n.y); v.data[q + 2] = Mathf.FloatToHalf(n.z); v.data[q + 3] = 0;
                }
                if (sm == 0 && c == FirstAnim(v))
                {
                    // the mesh that is drawn: the model in its first pose (the shader replaces positions and normals)
                    v.mesh = Object.Instantiate(s_tmp);
                    v.mesh.name = "TT pose mesh " + def.Key;
                    v.mesh.hideFlags = HideFlags.HideAndDontSave;
                }
                if (++v.nextSample >= samples) { v.nextAnim++; v.nextSample = 0; }
            }
            if (v.nextAnim < def.animations.Length || v.mesh == null) return;
            v.tex.SetPixelData(v.data, 0);
            v.tex.Apply(false, true);
            v.data = null;
            // every pose fits in the bounds (instances are culled with them)
            var b = new Bounds((v.bmin + v.bmax) * 0.5f, v.bmax - v.bmin);
            b.Expand(0.5f);
            v.mesh.bounds = b;
            v.shadowMesh = WeaponlessCopy(def, v.mesh);
            v.mesh.UploadMeshData(true);
            v.ready = true;
        }

        /// <summary>A warrior's pose mesh without the painted weapon's triangles (the real weapons' stripped mesh has
        /// the same vertices), for the shadow it casts: the crowd's painted weapons threw dark blotches that weren't
        /// their shape. Null when the model has none.</summary>
        static Mesh WeaponlessCopy(TTSpriteDefinition def, Mesh mesh)
        {
            var held = TribalTrouble.FaithfulGame.TTRealWeapons.Held;
            if (held == null || mesh == null) return null;
            foreach (var h in held)
            {
                if (h == null || h.sprite != def.Key || h.stripped == null || !h.stripped.isReadable) continue;
                if (h.stripped.vertexCount != mesh.vertexCount || h.stripped.subMeshCount != mesh.subMeshCount) continue;
                try
                {
                    var sm = Object.Instantiate(mesh);
                    sm.name = mesh.name + " (shadow, no weapon)";
                    sm.hideFlags = HideFlags.HideAndDontSave;
                    for (int sub = 0; sub < mesh.subMeshCount; sub++) sm.SetTriangles(h.stripped.GetTriangles(sub), sub, false);
                    sm.bounds = mesh.bounds;
                    sm.UploadMeshData(true);
                    return sm;
                }
                catch (System.Exception ex) { Debug.LogWarning("[TT] weaponless shadow mesh for " + def.Key + ": " + ex.Message); return null; }
            }
            return null;
        }

        static int FirstAnim(Vat v) { for (int c = 0; c < v.rowStart.Length; c++) if (v.rowStart[c] >= 0) return c; return 0; }

        /// <summary>A unit's two poses (rows) and the blend between them - TTPoseCache.Apply's frame and tween.</summary>
        public static Vector4 VatPose(Vat v, TTSpriteDefinition def, int anim, float ticks)
        {
            anim = Mathf.Clamp(anim, 0, def.animations.Length - 1);
            var a = def.animations[anim];
            int start = v.rowStart[anim];
            if (a == null || start < 0) { int f = FirstAnim(v); start = v.rowStart[f]; return new Vector4(start, start, 0f, 0f); }
            int n = a.frames * v.oversample;
            float pos = ticks / Mathf.Max(a.wpc, 1e-6f), time;
            if (a.loop) time = pos - Mathf.Floor(pos);
            else time = Mathf.Clamp(pos, 0f, a.frames > 0 ? (a.frames - 1f) / a.frames : 1f);
            float x = time * n;
            int s1 = x > 0f ? (int)x : 0;
            float tw = x - s1;
            int s2;
            if (a.loop) { s1 %= n; s2 = s1 + 1 == n ? 0 : s1 + 1; }
            else { if (s1 >= n - 1) { s1 = n - 1; tw = 0f; } s2 = Mathf.Min(s1 + 1, n - 1); }
            return new Vector4(start + s1, start + s2, Mathf.Clamp01(tw), 0f);
        }

        static Mesh Bake(Model md, int anim, int frame, int frames)
        {
            if (!md.poses.Apply(anim, (float)frame / frames, md.bones)) return null;
            var mesh = new Mesh { name = "far " + md.smr.sharedMesh.name + " " + anim + ":" + frame, hideFlags = HideFlags.HideAndDontSave };
            md.smr.BakeMesh(mesh, false);
            mesh.RecalculateBounds();
            mesh.UploadMeshData(true);
            return mesh;
        }
    }
}
