using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Weapon shadows shaped like the weapon: a weapon's own mesh flattened onto the ground (the slope under it) along
    /// the sun and drawn as a light shade (TribalTrouble/PlanarShadow). Weapons are too thin for the shadow map (a
    /// spear's shadow came out as a dark blotch that crawled from texel to texel), so they don't cast into it: the
    /// warriors' held real weapons and race weapons, and every thrown axe and spear. FaithfulView adds them each frame;
    /// Flush draws them, one instanced draw per mesh.
    /// </summary>
    public static class TTPlanarShadows
    {
        static Material s_mat;
        static bool s_failed;
        static readonly Dictionary<Mesh, List<Matrix4x4>> s_lists = new Dictionary<Mesh, List<Matrix4x4>>();
        static readonly List<Mesh> s_used = new List<Mesh>();
        static Vector3 s_toSun = Vector3.up;
        static com.oddlabs.tt.landscape.HeightMap s_hm;
        public static bool Active { get; private set; }
        public static int statCount;

        const float Lift = 0.035f;   // above the ground (the pass also pulls itself forward in depth)

        /// <summary>Starts a frame: on in the modern look with shadows, while the sun (or moon) stands high enough.</summary>
        public static void Begin(com.oddlabs.tt.landscape.World world)
        {
            statCount = 0;
            s_hm = world != null ? world.getHeightMap() : null;
            var l = (Vector3)Shader.GetGlobalVector("_TT_SunDir");
            s_toSun = l.sqrMagnitude > 1e-6f ? l.normalized : Vector3.up;
            Active = !s_failed && s_hm != null && s_toSun.y > 0.1f
                     && TribalTrouble.Game.GameSettings.graphicsStyle == 1 && TribalTrouble.Game.GameSettings.shadows;
        }

        static float H(float x, float z) => s_hm.getNearestHeight(x, z);

        /// <summary>The ground under a point (Unity space) and its slope.</summary>
        static void Ground(Vector3 p, out Vector3 at, out Vector3 normal)
        {
            float h = H(p.x, p.z);
            at = new Vector3(p.x, h, p.z);
            float hx = H(p.x + 1f, p.z) - H(p.x - 1f, p.z), hz = H(p.x, p.z + 1f) - H(p.x, p.z - 1f);
            normal = new Vector3(-hx * 0.5f, 1f, -hz * 0.5f).normalized;
        }

        /// <summary>A weapon held by a unit standing at foot: its shadow on the ground there.</summary>
        public static void AddHeld(Mesh mesh, in Matrix4x4 world, Vector3 foot)
        {
            if (!Active || mesh == null) return;
            Ground(foot, out var at, out var n);
            Add(mesh, world, at, n);
        }

        /// <summary>A thrown weapon in flight: its shadow where the sun's line through it meets the ground.</summary>
        public static void AddFlying(Mesh mesh, in Matrix4x4 world)
        {
            if (!Active || mesh == null) return;
            var p = new Vector3(world.m03, world.m13, world.m23);
            // follow the sun's line down to the ground (twice: the ground under the first guess is near enough)
            var q = p;
            for (int k = 0; k < 2; k++)
            {
                float g = H(q.x, q.z);
                q = p - s_toSun * ((p.y - g) / s_toSun.y);
            }
            Ground(q, out var at, out var n);
            Add(mesh, world, at, n);
        }

        static void Add(Mesh mesh, in Matrix4x4 world, Vector3 at, Vector3 n)
        {
            var L = s_toSun;
            float nl = Vector3.Dot(n, L);
            if (nl < 0.08f) return;
            float d = Vector3.Dot(n, at) + Lift;
            // P' = P - L (n.P - d) / (n.L): onto the ground plane along the sun
            var proj = Matrix4x4.identity;
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++) proj[r, c] = (r == c ? 1f : 0f) - L[r] * n[c] / nl;
                proj[r, 3] = L[r] * d / nl;
            }
            if (!s_lists.TryGetValue(mesh, out var list)) s_lists[mesh] = list = new List<Matrix4x4>(64);
            if (list.Count == 0) s_used.Add(mesh);
            list.Add(proj * world);
            statCount++;
        }

        static Material Mat()
        {
            if (s_mat != null || s_failed) return s_mat;
            var sh = Shader.Find("TribalTrouble/PlanarShadow");
            if (sh == null || !sh.isSupported) { s_failed = true; Active = false; Debug.LogWarning("[Tribal Trouble] PlanarShadow shader missing: weapons cast no shadow"); return null; }
            s_mat = new Material(sh) { name = "Weapon shadows", enableInstancing = true };
            return s_mat;
        }

        /// <summary>Draws this frame's weapon shadows.</summary>
        public static void Flush()
        {
            var mat = s_used.Count > 0 ? Mat() : null;
            foreach (var mesh in s_used)
            {
                var list = s_lists[mesh];
                if (mat != null && mesh != null)
                {
                    var rp = new RenderParams(mat) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off, receiveShadows = false };
                    for (int sub = 0; sub < mesh.subMeshCount; sub++)
                        for (int start = 0; start < list.Count; start += 1023)
                            Graphics.RenderMeshInstanced(rp, mesh, sub, list, Mathf.Min(1023, list.Count - start), start);
                }
                list.Clear();
            }
            s_used.Clear();
        }
    }
}
