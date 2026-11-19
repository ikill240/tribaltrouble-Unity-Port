using System.Collections.Generic;
using UnityEngine;
using com.oddlabs.tt.model;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Headgear for the New Tides warriors (modern look): the elites and the heavier race units each wear something of
    /// their own on the warrior's head bone, so they can be told apart up close - the Jarl's horned helm, the
    /// Huntress's hood, the Horn Bearer's fur cap, the Eagle Knight's beaked helm, the Hawk Eye's feather crest, the
    /// War Drummer's feather crown, the Huscarl's nasal helm, the Spear Guard's crest, the Champions' plumed helms and
    /// the Diamond Guards' crystal-spiked helms. Built in head radii (about half a torso length) from the weapons'
    /// primitives.
    /// </summary>
    public static partial class TTExtendedArt
    {
        enum Hat { None, HornedHelm, Hood, FurCap, EagleHelm, FeatherCrest, FeatherCrown, NasalHelm, Mohawk, PlumedHelm, DiamondHelm }

        static Hat HatFor(int kind, bool vik)
        {
            switch (kind)
            {
                case Race.UNIT_ELITE_1: return vik ? Hat.HornedHelm : Hat.EagleHelm;
                case Race.UNIT_ELITE_2: return vik ? Hat.Hood : Hat.FeatherCrest;
                case Race.UNIT_ELITE_3: return vik ? Hat.FurCap : Hat.FeatherCrown;
                case Race.UNIT_MELEE_HEAVY: return vik ? Hat.NasalHelm : Hat.Mohawk;
                case Race.UNIT_CHAMPION: return Hat.PlumedHelm;
                case Race.UNIT_DIAMOND: return Hat.DiamondHelm;
                default: return Hat.None;
            }
        }

        static readonly Dictionary<Hat, (Mesh mesh, Slot[] slots)> s_hats = new Dictionary<Hat, (Mesh, Slot[])>();

        /// <summary>Puts the kind's headgear on (once per model instance; DressUnique calls it).</summary>
        static void PutHat(GameObject root, int kind, bool vik, Color team, Material bm)
        {
            var hat = HatFor(kind, vik);
            if (hat == Hat.None) return;
            var smr = root.GetComponentInChildren<SkinnedMeshRenderer>(true);
            if (smr == null || smr.sharedMesh == null) return;
            var mesh = smr.sharedMesh;
            var bones = smr.bones;
            int n = mesh.bindposes.Length;
            int head = BoneIndex(bones, "Head"), neck = BoneIndex(bones, "Neck"), pelvis = BoneIndex(bones, "Pelvis"),
                lu = BoneIndex(bones, "L UpperArm"), ru = BoneIndex(bones, "R UpperArm"), foot = BoneIndex(bones, "L Foot"), toe = BoneIndex(bones, "L Toe0");
            foreach (int b in new[] { head, neck, pelvis, lu, ru, foot, toe })
                if (b < 0 || b >= n || bones[b] == null) return;
            if (bones[head].Find("Headgear") != null) return;   // (the other LOD shares the skeleton)
            Vector3 pN = RestPos(mesh, neck), pP = RestPos(mesh, pelvis), pH = RestPos(mesh, head);
            float T = (pN - pP).magnitude;
            if (T < 1e-4f) return;
            var up = (pN - pP) / T;
            var fwd = Vector3.ProjectOnPlane(RestPos(mesh, toe) - RestPos(mesh, foot), up).normalized;
            var left = Vector3.ProjectOnPlane(RestPos(mesh, lu) - RestPos(mesh, ru), up);
            if (fwd.sqrMagnitude < 0.5f) fwd = Vector3.Cross(left.normalized, up);
            left = Vector3.ProjectOnPlane(left, fwd).normalized;
            // the head's radius (0.52 of a torso length sat on the head like a bucket: the warriors' heads are smaller
            // than their body suggests), and the middle of the head the same share of it above the head bone
            float R = 0.4f * T;
            var c = pH + up * (1.19f * R);
            var m = Matrix4x4.identity;
            m.SetColumn(0, left * R); m.SetColumn(1, up * R); m.SetColumn(2, fwd * R);
            m.SetColumn(3, new Vector4(c.x, c.y, c.z, 1f));
            var (hm, slots) = HatMesh(hat);
            var go = new GameObject("Headgear");
            go.AddComponent<MeshFilter>().sharedMesh = hm;
            var r = go.AddComponent<MeshRenderer>();
            var mats = new Material[slots.Length];
            for (int i = 0; i < slots.Length; i++) mats[i] = SlotMat(slots[i], team, bm);
            r.sharedMaterials = mats;
            go.transform.SetParent(bones[head], false);
            SetLocal(go.transform, mesh.bindposes[head] * m);
        }

        static void Ring(WeaponBuilder b, Slot s, float y, float rx, float rz, float thick, int seg = 14)
        {
            for (int i = 0; i < seg; i++)
            {
                float a0 = i * Mathf.PI * 2f / seg, a1 = (i + 1) * Mathf.PI * 2f / seg;
                b.Tube(s, new Vector3(Mathf.Cos(a0) * rx, y, Mathf.Sin(a0) * rz), new Vector3(Mathf.Cos(a1) * rx, y, Mathf.Sin(a1) * rz), thick, thick, 5);
            }
        }

        static void Horn(WeaponBuilder b, Slot s, float side, float scale)
        {
            var pts = new[] { new Vector3(side * 0.92f, 0.3f, 0f), new Vector3(side * 1.35f, 0.55f, 0.08f), new Vector3(side * 1.6f, 0.95f, 0.15f), new Vector3(side * 1.55f, 1.35f, 0.18f) };
            float[] rad = { 0.22f, 0.17f, 0.12f, 0.08f };
            for (int i = 0; i < 3; i++)
                b.Tube(s, pts[i] * scale + new Vector3(side * 0.92f * (1 - scale), 0f, 0f), pts[i + 1] * scale + new Vector3(side * 0.92f * (1 - scale), 0f, 0f), rad[i] * scale, rad[i + 1] * scale, 7);
            var tip = pts[3] * scale + new Vector3(side * 0.92f * (1 - scale), 0f, 0f);
            b.Cone(s, tip, tip + new Vector3(-side * 0.08f, 0.32f, 0.02f) * scale, 0.08f * scale, 6);
        }

        static (Mesh mesh, Slot[] slots) HatMesh(Hat hat)
        {
            if (s_hats.TryGetValue(hat, out var have)) return have;
            var b = new WeaponBuilder();
            var fs = new[] { Slot.FeatherRed, Slot.FeatherGreen, Slot.FeatherBlue };
            switch (hat)
            {
                case Hat.HornedHelm:   // the Jarl
                    b.Dome(Slot.Iron, new Vector3(0f, 0.02f, 0f), new Vector3(1.05f, 0.98f, 1.08f), 0.5f);
                    Ring(b, Slot.Gold, 0.04f, 1.06f, 1.09f, 0.07f);
                    b.Tube(Slot.Gold, new Vector3(0f, 0.98f, 0f), new Vector3(0f, 0.3f, 1.06f), 0.06f, 0.06f, 5);   // the crest band
                    b.Tube(Slot.Iron, new Vector3(0f, 0.12f, 1.07f), new Vector3(0f, -0.38f, 1.1f), 0.08f, 0.06f, 5);   // the nasal
                    Horn(b, Slot.Bone, -1f, 1f); Horn(b, Slot.Bone, 1f, 1f);
                    break;
                case Hat.Hood:         // the Huntress
                    b.Dome(Slot.Team, new Vector3(0f, -0.05f, -0.06f), new Vector3(1.1f, 1.08f, 1.12f), 0.6f);
                    b.Cone(Slot.Team, new Vector3(0f, 0.55f, -0.8f), new Vector3(0f, -0.2f, -1.6f), 0.35f, 6);   // the hood's tail
                    b.Feather(Slot.FeatherWhite, new Vector3(0.6f, 0.5f, -0.6f), new Vector3(0.4f, 1f, -0.7f), 1.2f, 0.16f, new Vector3(1f, 0f, 0f));
                    b.Feather(Slot.FeatherBlue, new Vector3(0.62f, 0.45f, -0.55f), new Vector3(0.6f, 1f, -0.4f), 0.95f, 0.14f, new Vector3(1f, 0f, 0f));
                    break;
                case Hat.FurCap:       // the Horn Bearer
                    b.Dome(Slot.Leather, new Vector3(0f, 0.08f, 0f), new Vector3(1.12f, 0.85f, 1.14f), 0.55f);
                    Ring(b, Slot.Fur, 0.12f, 1.12f, 1.15f, 0.16f);
                    Horn(b, Slot.Bone, -1f, 0.6f); Horn(b, Slot.Bone, 1f, 0.6f);
                    break;
                case Hat.EagleHelm:    // the Eagle Knight
                    b.Dome(Slot.Leather, new Vector3(0f, 0.05f, -0.04f), new Vector3(1.08f, 1.0f, 1.12f), 0.52f);
                    b.Ball(Slot.Gold, new Vector3(0f, 0.55f, 0.9f), new Vector3(0.38f, 0.26f, 0.35f), 4, 8);           // the upper beak
                    b.Cone(Slot.Gold, new Vector3(0f, 0.45f, 1.1f), new Vector3(0f, 0.0f, 1.6f), 0.24f, 6);
                    b.Ball(Slot.Obsidian, new Vector3(0.42f, 0.72f, 0.82f), new Vector3(0.1f, 0.09f, 0.06f), 3, 6);
                    b.Ball(Slot.Obsidian, new Vector3(-0.42f, 0.72f, 0.82f), new Vector3(0.1f, 0.09f, 0.06f), 3, 6);
                    for (int i = 0; i < 7; i++)   // a fan of feathers down the back of the head
                    {
                        float t = i / 6f - 0.5f;
                        b.Feather(i % 2 == 0 ? Slot.FeatherWhite : Slot.FeatherRed, new Vector3(t * 0.9f, 0.7f, -0.7f), new Vector3(t * 1.6f, 1f, -0.9f), 1.25f - Mathf.Abs(t) * 0.6f, 0.18f, new Vector3(0f, 0f, 1f));
                    }
                    break;
                case Hat.FeatherCrest: // the Hawk Eye
                    Ring(b, Slot.Team, 0.2f, 1.04f, 1.07f, 0.09f);
                    for (int i = 0; i < 5; i++)
                    {
                        float t = i / 4f - 0.5f;
                        b.Feather(fs[i % 3], new Vector3(t * 0.6f, 0.4f, -0.85f), new Vector3(t * 0.7f, 1f, -0.35f), 1.7f - Mathf.Abs(t) * 0.8f, 0.17f, new Vector3(0f, 0f, 1f));
                    }
                    break;
                case Hat.FeatherCrown: // the War Drummer
                    Ring(b, Slot.Gold, 0.25f, 1.05f, 1.08f, 0.1f);
                    for (int i = 0; i < 12; i++)
                    {
                        float a = i * Mathf.PI * 2f / 12f;
                        var d = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
                        b.Feather(fs[i % 3], new Vector3(d.x * 1.0f, 0.3f, d.z * 1.03f), d * 0.45f + Vector3.up, 1.1f, 0.18f, new Vector3(-d.z, 0f, d.x));
                    }
                    break;
                case Hat.NasalHelm:    // the Huscarl
                    b.Dome(Slot.Iron, new Vector3(0f, 0.02f, 0f), new Vector3(1.05f, 1.05f, 1.08f), 0.5f);
                    Ring(b, Slot.Iron, 0.04f, 1.07f, 1.1f, 0.08f);
                    b.Tube(Slot.Iron, new Vector3(0f, 0.12f, 1.08f), new Vector3(0f, -0.42f, 1.12f), 0.1f, 0.07f, 5);
                    b.Tube(Slot.Team, new Vector3(0f, 1.05f, 0.5f), new Vector3(0f, 1.05f, -0.5f), 0.1f, 0.1f, 5);   // a crest in the tribe's colour
                    b.Cone(Slot.Iron, new Vector3(0f, 0.95f, 0f), new Vector3(0f, 1.35f, 0f), 0.14f, 6);
                    break;
                case Hat.Mohawk:       // the Spear Guard
                    b.Dome(Slot.Bone, new Vector3(0f, 0.08f, 0f), new Vector3(1.04f, 0.9f, 1.06f), 0.42f);
                    for (int i = 0; i < 9; i++)
                    {
                        float zz = 0.7f - i * 0.17f;
                        b.Feather(i % 2 == 0 ? Slot.FeatherRed : Slot.Team, new Vector3(0f, 0.85f, zz), new Vector3(0f, 1f, zz * 0.3f - 0.2f), 0.75f + 0.25f * Mathf.Sin(i * 0.4f), 0.16f, new Vector3(1f, 0f, 0f));
                    }
                    break;
                case Hat.PlumedHelm:   // the Champions
                    b.Dome(Slot.Iron, new Vector3(0f, 0.02f, 0f), new Vector3(1.05f, 1.0f, 1.08f), 0.5f);
                    Ring(b, Slot.Gold, 0.05f, 1.07f, 1.1f, 0.07f);
                    b.Tube(Slot.Gold, new Vector3(0f, 0.95f, 0f), new Vector3(0f, 1.25f, 0f), 0.07f, 0.06f, 6);
                    for (int i = 0; i < 6; i++)   // a plume in the tribe's colour
                        b.Feather(Slot.Team, new Vector3(0f, 1.2f, 0.1f - i * 0.05f), new Vector3(0f, 0.6f - i * 0.25f, -1f), 1.0f, 0.22f, new Vector3(1f, 0f, 0f));
                    break;
                case Hat.DiamondHelm:  // the Diamond Guards
                    b.Dome(Slot.Iron, new Vector3(0f, 0.02f, 0f), new Vector3(1.06f, 1.02f, 1.1f), 0.52f);
                    Ring(b, Slot.Diamond, 0.06f, 1.08f, 1.11f, 0.08f);
                    for (int i = 0; i < 5; i++)
                    {
                        float a = Mathf.PI * (0.2f + 0.6f * i / 4f);
                        var at = new Vector3(Mathf.Cos(a) * 0.75f, 0.7f, Mathf.Sin(a) * 0.78f);
                        b.Cone(Slot.Crystal, at, at + new Vector3(Mathf.Cos(a) * 0.12f, i == 2 ? 0.9f : 0.6f, Mathf.Sin(a) * 0.12f), 0.13f, 6);
                    }
                    break;
            }
            var mesh = b.Build("Headgear " + hat, out var slots);
            s_hats[hat] = (mesh, slots);
            return (mesh, slots);
        }
    }
}
