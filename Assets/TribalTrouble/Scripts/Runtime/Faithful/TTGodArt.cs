using System.Collections.Generic;
using UnityEngine;
using com.oddlabs.tt.model;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The gods' regalia (New Tides). A god is drawn on its tribe's chieftain model, grown; with a body skin of its own
    /// (gen_god_skins.py) and these pieces, built from the warriors' weapon primitives and riding the model's bones, it
    /// no longer reads as a big chief:
    ///   Valkyrie      a winged helm, great white wings on her back, a gold-headed spear
    ///   Thunder Lord  a horned crown of glowing spikes, iron pauldrons, a storm hammer
    ///   Jaguar Spirit a jaguar cowl (ears, snout, fangs, glowing eyes), a spotted tail, claws
    ///   Great Tiki    a towering carved mask with a feather crown, fire floating over the shoulders, a war club
    /// Sizes are in torso lengths (pelvis to neck), so the pieces fit either chieftain at any scale.
    /// </summary>
    public static partial class TTExtendedArt
    {
        enum GodPart { ValkyrieHelm, ValkyrieWings, ValkyrieSpear, ThunderCrown, ThunderPauldron, ThunderHammer,
            JaguarCowl, JaguarTail, JaguarClaws, TikiMask, TikiFire, TikiClub }

        static readonly Dictionary<GodPart, (Mesh mesh, Slot[] slots)> s_godParts = new Dictionary<GodPart, (Mesh, Slot[])>();
        static readonly Dictionary<int, GameObject> s_godDressed = new Dictionary<int, GameObject>();

        /// <summary>Puts a god's regalia on its model, once per model instance. Safe to call every frame.</summary>
        public static void DressGod(GameObject root, Unit u, int kind, Color team, Material bm)
        {
            int id = root.GetHashCode();
            if (s_godDressed.TryGetValue(id, out var have) && have != null) return;
            var smr = root.GetComponentInChildren<SkinnedMeshRenderer>(true);
            if (smr == null || smr.sharedMesh == null) return;   // not built yet
            s_godDressed[id] = smr.gameObject;
            if (s_godDressed.Count > 512)
            {
                var dead = new List<int>();
                foreach (var kv in s_godDressed) if (kv.Value == null) dead.Add(kv.Key);
                foreach (var k in dead) s_godDressed.Remove(k);
            }
            try { Regalia(smr, Vikings(u), kind == Race.UNIT_MAJOR_GOD, team, bm); }
            catch (System.Exception e) { Debug.LogWarning("[Tribal Trouble] the god's regalia: " + e.Message); }
        }

        static void Regalia(SkinnedMeshRenderer smr, bool vik, bool major, Color team, Material bm)
        {
            var mesh = smr.sharedMesh;
            var bones = smr.bones;
            int n = mesh.bindposes.Length;
            int head = BoneIndex(bones, "Head"), neck = BoneIndex(bones, "Neck"), pelvis = BoneIndex(bones, "Pelvis"),
                lu = BoneIndex(bones, "L UpperArm"), ru = BoneIndex(bones, "R UpperArm"), lh = BoneIndex(bones, "L Hand"),
                rh = BoneIndex(bones, "R Hand"), foot = BoneIndex(bones, "L Foot"), toe = BoneIndex(bones, "L Toe0");
            int spine = BoneIndex(bones, "Spine2");
            if (spine < 0) spine = BoneIndex(bones, "Spine1");
            foreach (int b in new[] { head, neck, pelvis, lu, ru, lh, rh, foot, toe, spine })
                if (b < 0 || b >= n || bones[b] == null) return;
            if (bones[head].Find("God regalia") != null) return;   // (the other LOD shares the skeleton)

            Vector3 pN = RestPos(mesh, neck), pP = RestPos(mesh, pelvis), pH = RestPos(mesh, head);
            float T = (pN - pP).magnitude;
            if (T < 1e-4f) return;
            var up = (pN - pP) / T;
            var fwd = Vector3.ProjectOnPlane(RestPos(mesh, toe) - RestPos(mesh, foot), up).normalized;
            var left = Vector3.ProjectOnPlane(RestPos(mesh, lu) - RestPos(mesh, ru), up);
            if (fwd.sqrMagnitude < 0.5f) fwd = Vector3.Cross(left.normalized, up);
            left = Vector3.ProjectOnPlane(left, fwd).normalized;

            // puts a part: its x along `ax`, y along `ay`, z along `az` (torso lengths), its origin at `o`, on bone `b`
            void Put(GodPart part, int b, Vector3 o, Vector3 ax, Vector3 ay, Vector3 az, string name)
            {
                var m = Matrix4x4.identity;
                m.SetColumn(0, ax * T); m.SetColumn(1, ay * T); m.SetColumn(2, az * T);
                m.SetColumn(3, new Vector4(o.x, o.y, o.z, 1f));
                var (pm, slots) = GodMesh(part);
                var go = new GameObject(name);
                go.AddComponent<MeshFilter>().sharedMesh = pm;
                var r = go.AddComponent<MeshRenderer>();
                var mats = new Material[slots.Length];
                for (int i = 0; i < slots.Length; i++) mats[i] = SlotMat(slots[i], team, bm);
                r.sharedMaterials = mats;
                go.transform.SetParent(bones[b], false);
                SetLocal(go.transform, mesh.bindposes[b] * m);
            }

            var headC = pH + up * (0.42f * T);                    // the middle of the head
            var back = RestPos(mesh, spine) - fwd * (0.3f * T) + up * (0.15f * T);
            // a weapon in the fist: its shaft forward and up from the hand (the arms are out to the sides at rest)
            var grip = (fwd * 0.75f + up * 0.66f).normalized;
            Vector3 GripSide(Vector3 g) => Vector3.Cross(g, left).normalized;

            if (vik && !major)   // the Valkyrie
            {
                Put(GodPart.ValkyrieHelm, head, headC, left, up, fwd, "God regalia");
                Put(GodPart.ValkyrieWings, spine, back, left, up, fwd, "God wings");
                Put(GodPart.ValkyrieSpear, rh, RestPos(mesh, rh), GripSide(grip), grip, left, "God weapon");
            }
            else if (vik)        // the Thunder Lord
            {
                Put(GodPart.ThunderCrown, head, headC, left, up, fwd, "God regalia");
                Put(GodPart.ThunderPauldron, lu, RestPos(mesh, lu) + up * (0.06f * T), left, up, fwd, "God pauldron L");
                Put(GodPart.ThunderPauldron, ru, RestPos(mesh, ru) + up * (0.06f * T), -left, up, fwd, "God pauldron R");
                Put(GodPart.ThunderHammer, rh, RestPos(mesh, rh), GripSide(grip), grip, left, "God weapon");
            }
            else if (!major)     // the Jaguar Spirit
            {
                Put(GodPart.JaguarCowl, head, headC, left, up, fwd, "God regalia");
                Put(GodPart.JaguarTail, pelvis, pP - fwd * (0.22f * T), left, up, fwd, "God tail");
                Put(GodPart.JaguarClaws, lh, RestPos(mesh, lh), left, up, fwd, "God claws L");
                Put(GodPart.JaguarClaws, rh, RestPos(mesh, rh), -left, up, fwd, "God claws R");
            }
            else                 // the Great Tiki
            {
                Put(GodPart.TikiMask, head, headC, left, up, fwd, "God regalia");
                Put(GodPart.TikiFire, lu, RestPos(mesh, lu), left, up, fwd, "God fire L");
                Put(GodPart.TikiFire, ru, RestPos(mesh, ru), -left, up, fwd, "God fire R");
                Put(GodPart.TikiClub, lh, RestPos(mesh, lh), GripSide(grip), grip, -left, "God weapon");
            }
        }

        // ------------------------------------------------------------------ the pieces (x side, y up, z forward; torso lengths)

        static (Mesh mesh, Slot[] slots) GodMesh(GodPart part)
        {
            if (s_godParts.TryGetValue(part, out var have)) return have;
            var b = new WeaponBuilder();
            switch (part)
            {
                case GodPart.ValkyrieHelm:
                {
                    b.Dome(Slot.Iron, new Vector3(0f, 0.02f, 0f), new Vector3(0.4f, 0.36f, 0.42f), 0.52f);
                    b.Tube(Slot.Gold, new Vector3(0f, 0.02f, 0.4f), new Vector3(0f, 0.36f, 0.32f), 0.035f, 0.025f, 6);   // the nasal and crest
                    b.Tube(Slot.Gold, new Vector3(0f, 0.36f, 0.32f), new Vector3(0f, 0.38f, -0.3f), 0.03f, 0.03f, 6);
                    for (int s = -1; s <= 1; s += 2)
                        for (int i = 0; i < 5; i++)   // a white wing on each side of the helm
                            b.Feather(Slot.FeatherWhite, new Vector3(s * 0.37f, 0.08f + i * 0.03f, -0.06f - i * 0.02f),
                                new Vector3(s * (0.45f - i * 0.05f), 1f, -0.55f - i * 0.18f), 0.5f - i * 0.05f, 0.075f, new Vector3(1f, 0f, 0f));
                    break;
                }
                case GodPart.ValkyrieWings:
                {
                    for (int s = -1; s <= 1; s += 2)
                    {
                        Vector3 p0 = new Vector3(s * 0.12f, 0.05f, 0f), p1 = new Vector3(s * 0.6f, 0.75f, -0.25f), p2 = new Vector3(s * 1.15f, 0.62f, -0.42f);
                        Vector3 Bez(float t) => (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
                        var prev = Bez(0f);
                        for (int i = 1; i <= 6; i++)
                        {
                            var q = Bez(i / 6f);
                            b.Tube(Slot.Gold, prev, q, 0.05f - i * 0.004f, 0.046f - i * 0.004f, 6);   // the wing's leading edge
                            prev = q;
                        }
                        for (int i = 0; i <= 18; i++)   // the long flight feathers hanging from it, broad and overlapping
                        {
                            float t = i / 18f;
                            var a = Bez(t);
                            var dir = new Vector3(s * (0.1f + 0.75f * t), -1f + 0.35f * t, -0.2f - 0.15f * t);
                            b.Feather(Slot.FeatherWhite, a, dir, 0.6f + 0.75f * t * (1.4f - t), 0.2f, new Vector3(0f, 0f, 1f));
                        }
                        for (int i = 0; i <= 12; i++)   // a row of middle feathers over them
                        {
                            float t = i / 12f;
                            var a = Bez(t) + new Vector3(0f, -0.02f, 0.02f);
                            b.Feather(Slot.FeatherWhite, a, new Vector3(s * (0.15f + 0.5f * t), -1f, -0.15f), 0.42f + 0.2f * t, 0.17f, new Vector3(0f, 0f, 1f));
                        }
                        for (int i = 0; i <= 8; i++)    // and short gold coverts over their roots
                        {
                            float t = i / 8f;
                            var a = Bez(t) + new Vector3(0f, -0.01f, 0.04f);
                            b.Feather(i % 2 == 0 ? Slot.Gold : Slot.FeatherWhite, a, new Vector3(s * 0.3f, -1f, -0.1f), 0.24f, 0.12f, new Vector3(0f, 0f, 1f));
                        }
                    }
                    break;
                }
                case GodPart.ValkyrieSpear:
                    b.Tube(Slot.Wood, new Vector3(0f, -0.55f, 0f), new Vector3(0f, 1.15f, 0f), 0.035f, 0.03f, 7);
                    b.Tube(Slot.Gold, new Vector3(0f, 1.1f, 0f), new Vector3(0f, 1.18f, 0f), 0.045f, 0.045f, 7);
                    b.Prism(Slot.Gold, P(0f, 1.16f, 0.09f, 1.32f, 0.07f, 1.48f, 0f, 1.62f, -0.07f, 1.48f, -0.09f, 1.32f), 0.018f);
                    b.Prism(Slot.Team, P(0.03f, 1.0f, 0.3f, 0.96f, 0.22f, 0.9f, 0.3f, 0.84f, 0.03f, 0.84f), 0.006f);   // a pennant in the tribe's colour
                    break;
                case GodPart.ThunderCrown:
                {
                    const int seg = 14;
                    for (int i = 0; i < seg; i++)   // the band
                    {
                        float a0 = i * Mathf.PI * 2f / seg, a1 = (i + 1) * Mathf.PI * 2f / seg;
                        b.Tube(Slot.Gold, new Vector3(Mathf.Cos(a0) * 0.4f, 0.12f, Mathf.Sin(a0) * 0.42f), new Vector3(Mathf.Cos(a1) * 0.4f, 0.12f, Mathf.Sin(a1) * 0.42f), 0.045f, 0.045f, 5);
                    }
                    for (int i = 0; i < 7; i++)     // spikes of crackling light round the front
                    {
                        float a = Mathf.PI * (0.15f + 0.7f * i / 6f);
                        var c = new Vector3(Mathf.Cos(a) * 0.4f, 0.14f, Mathf.Sin(a) * 0.42f);
                        b.Cone(Slot.Rune, c, c + new Vector3(Mathf.Cos(a) * 0.05f, i == 3 ? 0.42f : 0.28f, Mathf.Sin(a) * 0.05f), 0.055f, 5);
                    }
                    for (int s = -1; s <= 1; s += 2)   // great curling horns
                    {
                        var pts = new[] { new Vector3(s * 0.36f, 0.14f, -0.02f), new Vector3(s * 0.62f, 0.26f, 0.02f), new Vector3(s * 0.8f, 0.52f, 0.06f), new Vector3(s * 0.8f, 0.82f, 0.08f) };
                        float[] rad = { 0.1f, 0.08f, 0.06f, 0.045f };
                        for (int i = 0; i < 3; i++) b.Tube(Slot.Bone, pts[i], pts[i + 1], rad[i], rad[i + 1], 7);
                        b.Cone(Slot.Bone, pts[3], new Vector3(s * 0.72f, 1.02f, 0.06f), 0.045f, 6);
                    }
                    break;
                }
                case GodPart.ThunderPauldron:
                    b.Dome(Slot.Iron, Vector3.zero, new Vector3(0.28f, 0.22f, 0.3f), 0.55f);
                    b.Dome(Slot.Gold, new Vector3(0f, -0.04f, 0f), new Vector3(0.3f, 0.12f, 0.32f), 0.16f);
                    for (int i = -1; i <= 1; i++)
                        b.Cone(Slot.Rune, new Vector3(0.06f, 0.16f, i * 0.12f), new Vector3(0.12f, 0.42f, i * 0.16f), 0.04f, 5);
                    break;
                case GodPart.ThunderHammer:
                    b.Tube(Slot.Wood, new Vector3(0f, -0.35f, 0f), new Vector3(0f, 0.8f, 0f), 0.04f, 0.04f, 7);
                    b.Tube(Slot.Leather, new Vector3(0f, -0.12f, 0f), new Vector3(0f, 0.12f, 0f), 0.05f, 0.05f, 7);
                    b.Tube(Slot.Iron, new Vector3(-0.26f, 0.82f, 0f), new Vector3(0.26f, 0.82f, 0f), 0.16f, 0.16f, 6);
                    b.Tube(Slot.Rune, new Vector3(-0.16f, 0.82f, 0f), new Vector3(-0.1f, 0.82f, 0f), 0.168f, 0.168f, 6);
                    b.Tube(Slot.Rune, new Vector3(0.1f, 0.82f, 0f), new Vector3(0.16f, 0.82f, 0f), 0.168f, 0.168f, 6);
                    b.Cone(Slot.Gold, new Vector3(0f, 0.97f, 0f), new Vector3(0f, 1.1f, 0f), 0.05f, 6);
                    break;
                case GodPart.JaguarCowl:
                {
                    // a great jaguar-head helm, tall enough to take in the chieftain's headdress
                    var c = new Vector3(0f, -0.08f, -0.02f);
                    var r = new Vector3(0.52f, 1.02f, 0.55f);
                    b.Dome(Slot.Fur, c, r, 0.58f, 7, 14);
                    for (int s = -1; s <= 1; s += 2)
                    {
                        b.Cone(Slot.Fur, new Vector3(s * 0.3f, 0.76f, -0.05f), new Vector3(s * 0.44f, 1.1f, -0.1f), 0.14f, 6);   // the ears
                        b.Cone(Slot.Obsidian, new Vector3(s * 0.31f, 0.8f, -0.02f), new Vector3(s * 0.42f, 1.04f, -0.06f), 0.07f, 5);
                        b.Cone(Slot.Bone, new Vector3(s * 0.11f, 0.2f, 0.64f), new Vector3(s * 0.09f, -0.04f, 0.66f), 0.035f, 5);   // fangs
                        b.Ball(Slot.Spirit, new Vector3(s * 0.18f, 0.52f, 0.5f), new Vector3(0.07f, 0.045f, 0.035f), 3, 7);   // glowing eyes
                        b.Ball(Slot.Obsidian, new Vector3(s * 0.18f, 0.6f, 0.47f), new Vector3(0.1f, 0.025f, 0.05f), 2, 6);   // brows
                    }
                    b.Ball(Slot.Fur, new Vector3(0f, 0.32f, 0.5f), new Vector3(0.27f, 0.18f, 0.23f), 4, 10);   // the muzzle
                    b.Ball(Slot.Obsidian, new Vector3(0f, 0.4f, 0.72f), new Vector3(0.07f, 0.05f, 0.04f), 3, 6);   // the nose
                    var rnd = new System.Random(5);
                    for (int i = 0; i < 34; i++)   // rosettes
                    {
                        float th = (float)(0.15 + rnd.NextDouble() * 1.45), ph = (float)(rnd.NextDouble() * Mathf.PI * 2);
                        var d = new Vector3(Mathf.Sin(th) * Mathf.Cos(ph), Mathf.Cos(th), Mathf.Sin(th) * Mathf.Sin(ph));
                        if (d.z > 0.6f && d.y < 0.75f) continue;   // (not over the face)
                        b.Ball(Slot.Obsidian, Vector3.Scale(d, r) * 1.01f + c, new Vector3(0.05f, 0.05f, 0.05f), 2, 5);
                    }
                    break;
                }
                case GodPart.JaguarTail:
                {
                    var prev = Vector3.zero;
                    for (int i = 1; i <= 10; i++)
                    {
                        float t = i / 10f;
                        var q = new Vector3(0.12f * Mathf.Sin(t * 5f), -0.45f * Mathf.Sin(t * 2.3f) + 0.55f * t * t, -(0.15f + 0.95f * t));
                        float r0 = 0.075f - (i - 1) * 0.004f, r1 = 0.075f - i * 0.004f;
                        b.Tube(i % 3 == 0 ? Slot.Obsidian : Slot.Fur, prev, q, r0, r1, 6);
                        prev = q;
                    }
                    b.Cone(Slot.Obsidian, prev, prev + new Vector3(0f, 0.08f, -0.1f), 0.035f, 5);
                    break;
                }
                case GodPart.JaguarClaws:
                    for (int i = 0; i < 4; i++)
                    {
                        float x = -0.09f + i * 0.06f;
                        b.Cone(Slot.Bone, new Vector3(x, 0f, 0.08f), new Vector3(x * 1.2f, -0.08f, 0.34f), 0.026f, 5);
                    }
                    b.Ball(Slot.Spirit, new Vector3(0f, 0f, 0f), new Vector3(0.09f, 0.09f, 0.09f), 3, 7);   // spirit light round the paw
                    break;
                case GodPart.TikiMask:
                {
                    const float z = 0.4f;
                    b.Prism(Slot.Wood, P(-0.46f, -0.42f, 0.46f, -0.42f, 0.52f, 0.9f, -0.52f, 0.9f), 0.07f, z);   // the board
                    b.Prism(Slot.Gold, P(-0.44f, 0.5f, 0.44f, 0.5f, 0.4f, 0.58f, -0.4f, 0.58f), 0.02f, z + 0.08f);   // brow
                    for (int s = -1; s <= 1; s += 2)
                    {
                        b.Prism(Slot.Bone, P(s * 0.08f, 0.22f, s * 0.34f, 0.22f, s * 0.36f, 0.45f, s * 0.1f, 0.45f), 0.012f, z + 0.075f);   // eye whites
                        b.Prism(Slot.Obsidian, P(s * 0.16f, 0.27f, s * 0.28f, 0.27f, s * 0.28f, 0.4f, s * 0.16f, 0.4f), 0.012f, z + 0.09f);   // pupils
                    }
                    b.Prism(Slot.Wood, P(-0.07f, 0.05f, 0.07f, 0.05f, 0f, 0.28f), 0.05f, z + 0.1f);   // the nose
                    b.Prism(Slot.Ember, P(-0.3f, -0.3f, 0.3f, -0.3f, 0.26f, -0.12f, -0.26f, -0.12f), 0.012f, z + 0.075f);   // a glowing mouth
                    for (int i = 0; i < 6; i++)
                    {
                        float x = -0.25f + i * 0.1f;
                        b.Prism(Slot.Bone, P(x - 0.03f, -0.12f, x + 0.03f, -0.12f, x, -0.2f), 0.01f, z + 0.09f);   // teeth
                    }
                    var fs = new[] { Slot.FeatherRed, Slot.FeatherGreen, Slot.FeatherBlue };
                    for (int i = 0; i <= 12; i++)   // a fan of feathers over the crown
                    {
                        float t = i / 12f - 0.5f;
                        b.Feather(fs[i % 3], new Vector3(t * 0.9f, 0.86f, z - 0.03f), new Vector3(t * 1.6f, 1f, 0f), 0.7f - Mathf.Abs(t) * 0.5f, 0.1f, new Vector3(0f, 0f, 1f));
                    }
                    break;
                }
                case GodPart.TikiFire:
                    b.Ball(Slot.Ember, new Vector3(0f, 0.42f, 0f), new Vector3(0.14f, 0.18f, 0.14f), 4, 8);
                    b.Cone(Slot.Ember, new Vector3(0f, 0.5f, 0f), new Vector3(0.02f, 0.78f, 0f), 0.1f, 6);
                    b.Ball(Slot.Gold, new Vector3(0f, 0.25f, 0f), new Vector3(0.1f, 0.03f, 0.1f), 2, 8);   // the brazier ring it rises from
                    break;
                case GodPart.TikiClub:
                    b.Tube(Slot.Wood, new Vector3(0f, -0.3f, 0f), new Vector3(0f, 0.9f, 0f), 0.04f, 0.075f, 7);
                    for (int i = 0; i < 5; i++)
                        for (int s = -1; s <= 1; s += 2)
                        {
                            float y = 0.4f + i * 0.11f;
                            b.Prism(Slot.Obsidian, P(s * 0.06f, y, s * 0.16f, y + 0.04f, s * 0.06f, y + 0.08f), 0.012f);
                        }
                    b.Ball(Slot.Ember, new Vector3(0f, 0.95f, 0f), new Vector3(0.09f, 0.09f, 0.09f), 3, 7);
                    break;
            }
            var mesh = b.Build("God " + part, out var slots);
            s_godParts[part] = (mesh, slots);
            return (mesh, slots);
        }
    }
}
