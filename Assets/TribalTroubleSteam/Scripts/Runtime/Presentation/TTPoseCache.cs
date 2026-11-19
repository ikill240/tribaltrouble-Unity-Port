using System.Collections.Generic;
using UnityEngine;

namespace TribalTroubleSteam.Presentation
{
    /// <summary>
    /// The pose of every frame of every animation of a model, worked out once: each clip is sampled at its own keys
    /// (frame i of N at clip time i / N, as the converter wrote them) and the bones' local positions, rotations and
    /// scales kept. A unit is then posed by blending the two frames around its clip time - what the Java renderer
    /// does (frame = cycle position x frames, tweened, LOOP wraps, PLAIN holds the last frame) - instead of sampling
    /// the clip on its own bones every frame, which was most of a unit's cost. Only bones some frame moves are kept.
    /// </summary>
    public sealed class TTPoseCache
    {
        sealed class Clip
        {
            public int frames;
            public bool loop, scaled;
            public Vector3[] pos, scl;
            public Quaternion[] rot;
        }

        static readonly Dictionary<TTSpriteDefinition, TTPoseCache> s_all = new Dictionary<TTSpriteDefinition, TTPoseCache>();
        static readonly HashSet<TTSpriteDefinition> s_failed = new HashSet<TTSpriteDefinition>();

        int[] _index;     // the kept bones, as indices into the animation root's GetComponentsInChildren<Transform>(true)
        int _total;       // how many transforms that is on a fresh copy of the model
        Clip[] _clips;

        /// <summary>The cache of a model (built the first time), or null if it can't be built (sampling is used).</summary>
        public static TTPoseCache For(TTSpriteDefinition def)
        {
            if (def == null || def.model == null) return null;
            if (s_all.TryGetValue(def, out var c)) return c;
            if (s_failed.Contains(def)) return null;
            try { c = Build(def); }
            catch (System.Exception e) { Debug.LogWarning("[Tribal Trouble] pose cache for " + def.Key + ": " + e.Message); c = null; }
            if (c == null) { s_failed.Add(def); return null; }
            s_all[def] = c;
            return c;
        }

        static TTPoseCache Build(TTSpriteDefinition def)
        {
            var go = Object.Instantiate(def.model);
            go.hideFlags = HideFlags.HideAndDontSave;
            foreach (var r in go.GetComponentsInChildren<Renderer>(true)) r.enabled = false;
            try
            {
                var root = TTSpriteView.FindAnimationRoot(go);
                var all = root.GetComponentsInChildren<Transform>(true);
                int n = all.Length;
                var p0 = new Vector3[n]; var r0 = new Quaternion[n]; var s0 = new Vector3[n];
                for (int i = 0; i < n; i++) { p0[i] = all[i].localPosition; r0[i] = all[i].localRotation; s0[i] = all[i].localScale; }
                int clips = def.animations.Length;
                var tp = new Vector3[clips][]; var tr = new Quaternion[clips][]; var ts = new Vector3[clips][];
                var changed = new bool[n];
                var scaled = new bool[clips];
                bool any = false;
                for (int c = 0; c < clips; c++)
                {
                    var a = def.animations[c];
                    if (a == null || a.clip == null || a.frames <= 0) continue;
                    int frames = a.frames;
                    for (int i = 0; i < n; i++) all[i].SetLocalPositionAndRotation(p0[i], r0[i]);
                    for (int i = 0; i < n; i++) all[i].localScale = s0[i];
                    tp[c] = new Vector3[frames * n]; tr[c] = new Quaternion[frames * n]; ts[c] = new Vector3[frames * n];
                    for (int f = 0; f < frames; f++)
                    {
                        a.clip.SampleAnimation(root, (float)f / frames);
                        for (int i = 0; i < n; i++)
                        {
                            var t = all[i];
                            Vector3 lp = t.localPosition, ls = t.localScale; Quaternion lr = t.localRotation;
                            tp[c][f * n + i] = lp; tr[c][f * n + i] = lr; ts[c][f * n + i] = ls;
                            if (!changed[i] && ((lp - p0[i]).sqrMagnitude > 1e-12f || Quaternion.Dot(lr, r0[i]) < 0.9999999f && Quaternion.Dot(lr, r0[i]) > -0.9999999f || (ls - s0[i]).sqrMagnitude > 1e-12f))
                                changed[i] = true;
                            if ((ls - s0[i]).sqrMagnitude > 1e-12f) scaled[c] = true;
                        }
                    }
                    any = true;
                }
                if (!any) return null;
                var keep = new List<int>();
                for (int i = 0; i < n; i++) if (changed[i]) keep.Add(i);
                var cache = new TTPoseCache { _index = keep.ToArray(), _total = n, _clips = new Clip[clips] };
                int k = keep.Count;
                for (int c = 0; c < clips; c++)
                {
                    if (tp[c] == null) continue;
                    var a = def.animations[c];
                    int frames = a.frames;
                    var cl = new Clip { frames = frames, loop = a.loop, scaled = scaled[c], pos = new Vector3[frames * k], rot = new Quaternion[frames * k], scl = scaled[c] ? new Vector3[frames * k] : null };
                    for (int f = 0; f < frames; f++)
                        for (int j = 0; j < k; j++)
                        {
                            int src = f * n + keep[j], dst = f * k + j;
                            cl.pos[dst] = tp[c][src];
                            // rotations kept on the same side as the frame before, so blending takes the short way
                            var q = tr[c][src];
                            if (f > 0 && Quaternion.Dot(cl.rot[dst - k], q) < 0f) q = new Quaternion(-q.x, -q.y, -q.z, -q.w);
                            cl.rot[dst] = q;
                            if (cl.scl != null) cl.scl[dst] = ts[c][src];
                        }
                    cache._clips[c] = cl;
                }
                return cache;
            }
            finally
            {
                if (Application.isPlaying) Object.Destroy(go); else Object.DestroyImmediate(go);
            }
        }

        /// <summary>The kept bones of an instance of the model (call before anything is attached to it), or null if
        /// its hierarchy doesn't match the model's.</summary>
        public Transform[] Resolve(GameObject animationRoot)
        {
            if (animationRoot == null) return null;
            var all = animationRoot.GetComponentsInChildren<Transform>(true);
            if (all.Length != _total) return null;
            var bones = new Transform[_index.Length];
            for (int j = 0; j < bones.Length; j++) bones[j] = all[_index[j]];
            return bones;
        }

        /// <summary>Poses the bones at a clip time (0..1 = one cycle); false if the clip isn't cached.</summary>
        public bool Apply(int anim, float time, Transform[] bones)
        {
            if (anim < 0 || anim >= _clips.Length) return false;
            var c = _clips[anim];
            if (c == null) return false;
            int frames = c.frames;
            float x = time * frames;
            int f1 = x > 0f ? (int)x : 0;
            float tw = x - f1;
            int f2;
            if (c.loop)
            {
                f1 %= frames;
                f2 = f1 + 1 == frames ? 0 : f1 + 1;
            }
            else
            {
                if (f1 >= frames - 1) { f1 = frames - 1; tw = 0f; }
                f2 = Mathf.Min(f1 + 1, frames - 1);
            }
            if (tw < 0f) tw = 0f; else if (tw > 1f) tw = 1f;
            int k = bones.Length, o1 = f1 * k, o2 = f2 * k;
            var pos = c.pos; var rot = c.rot;
            for (int j = 0; j < k; j++)
            {
                var b = bones[j];
                if (b == null) continue;
                Vector3 pa = pos[o1 + j], pb = pos[o2 + j];
                Quaternion qa = rot[o1 + j], qb = rot[o2 + j];
                if (qa.x * qb.x + qa.y * qb.y + qa.z * qb.z + qa.w * qb.w < 0f) qb = new Quaternion(-qb.x, -qb.y, -qb.z, -qb.w);
                float u = 1f - tw;
                float qx = qa.x * u + qb.x * tw, qy = qa.y * u + qb.y * tw, qz = qa.z * u + qb.z * tw, qw = qa.w * u + qb.w * tw;
                float inv = 1f / Mathf.Sqrt(qx * qx + qy * qy + qz * qz + qw * qw);
                b.SetLocalPositionAndRotation(new Vector3(pa.x * u + pb.x * tw, pa.y * u + pb.y * tw, pa.z * u + pb.z * tw),
                    new Quaternion(qx * inv, qy * inv, qz * inv, qw * inv));
                if (c.scl != null) b.localScale = Vector3.LerpUnclamped(c.scl[o1 + j], c.scl[o2 + j], tw);
            }
            return true;
        }
    }
}
