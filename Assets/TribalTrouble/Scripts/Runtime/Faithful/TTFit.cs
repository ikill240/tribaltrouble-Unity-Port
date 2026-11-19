using System.Collections.Generic;
using TribalTrouble.Faithful;
using UnityEngine;
using com.oddlabs.tt.model;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// TT2 (Extended rules): fits each unit's and building's clickable cylinder - and the ring drawn under it - to the
    /// model as it is actually drawn, measured from its mesh bounds the first time one is seen (per template, and per
    /// build stage for buildings). New Tides' own models are bigger or smaller than the placeholder sizes the rules
    /// were written with. Only picking and the selection / placement rings use it; the simulation never does, so
    /// replays and multiplayer stay in step. Classic games keep the original sizes.
    /// </summary>
    public static class TTFit
    {
        static com.oddlabs.tt.landscape.World s_world;
        static bool s_on;
        static bool s_capturing;
        static Bounds s_capture;
        static bool s_captured;
        static readonly Dictionary<BuildingTemplate, float> s_ring = new Dictionary<BuildingTemplate, float>(RefEqT<BuildingTemplate>.Instance);

        sealed class RefEqT<T> : IEqualityComparer<T> where T : class
        {
            public static readonly RefEqT<T> Instance = new RefEqT<T>();
            public bool Equals(T a, T b) => ReferenceEquals(a, b);
            public int GetHashCode(T o) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(o);
        }

        /// <summary>A new world: forget the last one's fits (its templates may be shared), and fit only in TT2.</summary>
        public static void OnWorld(com.oddlabs.tt.landscape.World world)
        {
            if (ReferenceEquals(world, s_world)) return;
            s_world = world;
            s_on = world != null && world.isExtended();
            s_ring.Clear();
            if (world == null) return;
            try { Reset(world); } catch (System.Exception e) { Debug.LogWarning("[TT] hitbox fit reset: " + e.Message); }
        }

        static void Reset(com.oddlabs.tt.landscape.World world)
        {
            var rr = world.getRacesResources();
            if (rr == null) return;
            for (int r = 0; r < 2; r++)
            {
                com.oddlabs.tt.model.Race race = null;
                try { race = rr.getRace(r); } catch (System.Exception) { }
                if (race == null) continue;
                for (int u = 0; u < Race.NUM_UNITS; u++)
                {
                    var t = race.getUnitTemplate(u);
                    if (t != null) t.fitSelection(-1f, -1f);
                }
                for (int b = 0; b < Race.NUM_BUILDINGS; b++)
                {
                    var t = race.getBuildingTemplate(b);
                    if (t != null) t.clearFit();
                }
            }
        }

        static int Level(Building b) => b.getRenderLevel().ordinal();

        /// <summary>A unit or building drawn as a GameObject (the original sprite models): measure its meshes.</summary>
        public static void Measure(object model, GameObject go)
        {
            if (!s_on || go == null) return;
            if (model is Unit u)
            {
                var t = (UnitTemplate)u.getTemplate();
                if (t.isSelectionFitted()) return;
                if (!Measure(go, out float r, out float top)) return;
                float scale = Mathf.Max(0.2f, go.transform.lossyScale.x);
                float orig = t.getSelectionRadius(), origH = t.getSelectionHeight();
                var p = TTSpace.ToUnity(u.getPositionX(), u.getPositionY(), u.getPositionZ());
                // never far from the original circle (scaled like the model): the mesh only nudges it
                r = Mathf.Clamp(r, 0.6f * orig * scale, 1.3f * orig * scale);
                float h = Mathf.Clamp(top - p.y, 0.7f * origH * scale, 1.5f * origH * scale);
                t.fitSelection(r, h);
            }
            else if (model is Building b && !(b is Ship))
            {
                var t = (BuildingTemplate)b.getTemplate();
                int lv = Level(b);
                if (t.isSelectionFitted(lv)) return;
                if (!Measure(go, out float r, out float top)) return;
                Fit(t, lv, r, top - TTSpace.ToUnity(b.getPositionX(), b.getPositionY(), b.getPositionZ()).y);
            }
        }

        /// <summary>
        /// The model's footprint radius (the mean of its meshes' half width and half depth, in their own axes, so a
        /// building turned 45 degrees isn't measured by its diagonal) and its top. Bind-pose mesh bounds - a skinned
        /// renderer's own bounds are deliberately padded far out (TTSpriteView) and would make every circle huge.
        /// </summary>
        static bool Measure(GameObject go, out float radius, out float top)
        {
            radius = 0f; top = float.MinValue;
            bool any = false;
            foreach (var rend in go.GetComponentsInChildren<Renderer>(false))
            {
                if (rend == null || !rend.enabled) continue;
                Mesh mesh = rend is SkinnedMeshRenderer smr ? smr.sharedMesh
                    : rend is MeshRenderer ? rend.GetComponent<MeshFilter>()?.sharedMesh : null;
                if (mesh == null) continue;
                var b = mesh.bounds;
                var sc = rend.transform.lossyScale;
                float r = 0.25f * (Mathf.Abs(b.size.x * sc.x) + Mathf.Abs(b.size.z * sc.z));
                if (r > radius) radius = r;
                var m = rend.transform.localToWorldMatrix;
                for (int i = 0; i < 8; i++)
                {
                    var c = m.MultiplyPoint3x4(new Vector3((i & 1) == 0 ? b.min.x : b.max.x, (i & 2) == 0 ? b.min.y : b.max.y, (i & 4) == 0 ? b.min.z : b.max.z));
                    if (c.y > top) top = c.y;
                }
                any = true;
            }
            return any && radius > 0.05f;
        }

        static void Fit(BuildingTemplate t, int level, float r, float h)
        {
            // no bigger than the ground the building really takes (its footprint: 2 * size - 1 cells of 2 m, halved)
            float footprint = Mathf.Max(1f, 2f * t.getPlacingSize() - 1f);
            r = Mathf.Clamp(r, 0.8f, footprint);
            t.fitSelection(level, r, Mathf.Clamp(h, 0.8f, 30f));
            if (level == 2 || !s_ring.ContainsKey(t)) s_ring[t] = r;
        }

        // ---- the New Tides buildings, drawn straight from meshes (TTExtendedArt.DrawBuilding): captured as they draw

        /// <summary>Whether this building still needs measuring (then wrap its drawing in Begin / End).</summary>
        public static bool NeedsCapture(Building b) =>
            s_on && !((BuildingTemplate)b.getTemplate()).isSelectionFitted(Level(b));

        public static void BeginCapture() { s_capturing = true; s_captured = false; }

        /// <summary>Called for every mesh drawn while capturing (TTExtendedArt.Emit).</summary>
        public static void Add(Mesh mesh, Matrix4x4 m)
        {
            if (!s_capturing || mesh == null) return;
            var b = mesh.bounds;
            for (int i = 0; i < 8; i++)
            {
                var c = m.MultiplyPoint3x4(new Vector3((i & 1) == 0 ? b.min.x : b.max.x, (i & 2) == 0 ? b.min.y : b.max.y, (i & 4) == 0 ? b.min.z : b.max.z));
                if (!s_captured) { s_capture = new Bounds(c, Vector3.zero); s_captured = true; }
                else s_capture.Encapsulate(c);
            }
        }

        public static void EndCapture(Building b)
        {
            s_capturing = false;
            if (!s_captured || s_capture.size.sqrMagnitude < 1e-4f) return;
            // a building only half way up measures low: keep the start / half built stages their own heights
            var p = TTSpace.ToUnity(b.getPositionX(), b.getPositionY(), b.getPositionZ());
            Fit((BuildingTemplate)b.getTemplate(), Level(b), 0.25f * (s_capture.size.x + s_capture.size.z), s_capture.max.y - p.y);
        }

        /// <summary>The ring under a building: its fitted footprint in TT2, the original 5 m otherwise.</summary>
        public static float RingRadius(Building b)
        {
            if (!s_on) return 5f;
            var t = (BuildingTemplate)b.getTemplate();
            return s_ring.TryGetValue(t, out float r) ? r + 0.3f : 5f;
        }
    }
}
