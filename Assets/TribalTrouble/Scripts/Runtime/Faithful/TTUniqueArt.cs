using System.Collections.Generic;
using com.oddlabs.tt.model;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Extended rules: every new warrior gets weapons and a look of his own (the simulation is untouched).
    /// <list type="bullet">
    /// <item>Natives: Blowgunner (a long blowgun), Jaguar Warrior (an obsidian-edged macuahuitl and a jaguar pelt),
    /// Club Warrior (a spiked war club), Spear Guard (a long obsidian spear), Shaman (a feathered staff and a feather
    /// cloak), Scout (a light javelin), Slinger (a sling).</item>
    /// <item>Vikings: Fire Thrower (a glowing ember axe), Berserker (two bearded axes and a wolf pelt), Axeman (a bearded
    /// axe), Huscarl (a long Dane axe and a red cloak), Skald (a rune staff with a horn and a blue cloak), Scout (a
    /// hatchet), Slinger (a sling); the champions of both races wear a cape in their team colour.</item>
    /// </list>
    /// The painted weapon is taken out of the warrior mesh (the stripped mesh made by Build Real Weapons) and the new one
    /// is fitted onto it in the weapon bone's space (TTRealWeaponSet.HeldWeapon.frame*), so it follows every animation.
    /// Capes ride the upper spine. Projectiles change with the thrower: darts, sling stones, spirit bolts.
    /// </summary>
    public static partial class TTExtendedArt
    {
        enum Slot { Wood, Stone, Obsidian, Iron, Gold, Bone, Leather, Team, FeatherRed, FeatherGreen, FeatherBlue, Ember, Rune, Crystal, Diamond,
            FeatherWhite, Fur, Spirit, Count }   // (the last three: the gods' regalia)

        enum Weapon { None, Blowgun, Macuahuitl, Club, LongSpear, ShamanStaff, Javelin, Sling, FireAxe, BeardedAxe, DaneAxe, SkaldStaff, Hatchet,
            CrystalSpear, CrystalAxe, DiamondSpear, DiamondAxe, DartBlowgun, Horn, HandDrum }

        enum Cape { None, Team, Jaguar, Wolf, Feathers, Skald, Huscarl }

        // ------------------------------------------------------------------ which unit gets what

        static Weapon WeaponFor(int kind, bool vik)
        {
            switch (kind)
            {
                case Race.UNIT_UNIQUE_1: return vik ? Weapon.FireAxe : Weapon.Blowgun;
                case Race.UNIT_UNIQUE_2: return vik ? Weapon.BeardedAxe : Weapon.Macuahuitl;
                case Race.UNIT_MELEE_LIGHT: return vik ? Weapon.BeardedAxe : Weapon.Club;
                case Race.UNIT_MELEE_HEAVY: return vik ? Weapon.DaneAxe : Weapon.LongSpear;
                case Race.UNIT_SHAMAN: return vik ? Weapon.SkaldStaff : Weapon.ShamanStaff;
                case Race.UNIT_SCOUT: return vik ? Weapon.Hatchet : Weapon.Javelin;
                case Race.UNIT_SLINGER: return Weapon.Sling;
                case Race.UNIT_CRYSTAL: return vik ? Weapon.CrystalAxe : Weapon.CrystalSpear;   // the arsenal
                case Race.UNIT_DIAMOND: return vik ? Weapon.DiamondAxe : Weapon.DiamondSpear;
                case Race.UNIT_UNIQUE_3: return vik ? Weapon.BeardedAxe : Weapon.DartBlowgun;
                case Race.UNIT_MINOR_GOD: return vik ? Weapon.CrystalAxe : Weapon.Macuahuitl;   // the gods
                case Race.UNIT_MAJOR_GOD: return vik ? Weapon.DiamondAxe : Weapon.DiamondSpear;
                case Race.UNIT_ZOMBIE_BRUTE: return Weapon.Club;   // the risen
                case Race.UNIT_ELITE_1: return vik ? Weapon.DaneAxe : Weapon.Macuahuitl;   // the elites
                case Race.UNIT_ELITE_2: return vik ? Weapon.Hatchet : Weapon.Javelin;
                case Race.UNIT_ELITE_3: return vik ? Weapon.Horn : Weapon.HandDrum;
                case Race.UNIT_SPINNER: return vik ? Weapon.BeardedAxe : Weapon.Macuahuitl;   // the chicken whirlers: one in each hand
                default: return Weapon.None;
            }
        }

        static Cape CapeFor(int kind, bool vik)
        {
            switch (kind)
            {
                case Race.UNIT_CHAMPION: return Cape.Team;
                case Race.UNIT_UNIQUE_2: return vik ? Cape.Wolf : Cape.Jaguar;
                case Race.UNIT_SHAMAN: return vik ? Cape.Skald : Cape.Feathers;
                case Race.UNIT_MELEE_HEAVY: return vik ? Cape.Huscarl : Cape.None;
                case Race.UNIT_DIAMOND: return Cape.Team;
                case Race.UNIT_UNIQUE_3: return vik ? Cape.Wolf : Cape.Feathers;
                case Race.UNIT_MINOR_GOD: return vik ? Cape.None : Cape.Jaguar;   // (the Valkyrie has wings)
                case Race.UNIT_MAJOR_GOD: return vik ? Cape.Huscarl : Cape.Feathers;
                case Race.UNIT_ELITE_1: return vik ? Cape.Team : Cape.Feathers;   // the elites
                case Race.UNIT_ELITE_2: return vik ? Cape.Wolf : Cape.Team;
                case Race.UNIT_ELITE_3: return vik ? Cape.Skald : Cape.Jaguar;
                case Race.UNIT_SPINNER: return vik ? Cape.Wolf : Cape.Feathers;
                default: return Cape.None;
            }
        }

        /// <summary>Length of the new weapon against the painted one, and where on it (0 butt .. 1 head) the hand holds it.</summary>
        static void Proportions(Weapon w, out float k, out float grip)
        {
            switch (w)
            {
                case Weapon.Blowgun: k = 0.86f; grip = 0.3f; break;
                case Weapon.Macuahuitl: k = 0.74f; grip = 0.14f; break;
                case Weapon.Club: k = 0.62f; grip = 0.16f; break;
                case Weapon.LongSpear: k = 1.12f; grip = -1f; break;      // -1: the painted weapon's own grip
                case Weapon.ShamanStaff: k = 0.85f; grip = 0.45f; break;
                case Weapon.Javelin: k = 0.8f; grip = 0.42f; break;
                case Weapon.Sling: k = 0.62f; grip = 0.03f; break;   // (a full arm-length sling: it read as a thread at 0.32)
                case Weapon.FireAxe: k = 1f; grip = -1f; break;
                case Weapon.BeardedAxe: k = 1.08f; grip = -1f; break;
                case Weapon.DaneAxe: k = 1.6f; grip = 0.3f; break;
                case Weapon.SkaldStaff: k = 1.3f; grip = 0.42f; break;
                case Weapon.Hatchet: k = 0.86f; grip = -1f; break;
                case Weapon.CrystalSpear: k = 1f; grip = -1f; break;
                case Weapon.CrystalAxe: k = 1f; grip = -1f; break;
                case Weapon.DiamondSpear: k = 1.15f; grip = -1f; break;
                case Weapon.DiamondAxe: k = 1.45f; grip = 0.3f; break;
                case Weapon.DartBlowgun: k = 0.8f; grip = 0.3f; break;
                case Weapon.Horn: k = 0.66f; grip = 0.2f; break;   // the Horn Bearer's war horn
                case Weapon.HandDrum: k = 0.7f; grip = 0.15f; break;   // the War Drummer's drum on its handle
                default: k = 1f; grip = -1f; break;
            }
        }

        // ------------------------------------------------------------------ the weapon meshes (x blade side, y butt 0 .. head 1, z thickness)

        sealed class WeaponBuilder
        {
            public readonly List<Vector3> v = new List<Vector3>();
            public readonly List<Vector3> n = new List<Vector3>();
            public readonly List<Vector2> uv = new List<Vector2>();
            public readonly List<int>[] t = new List<int>[(int)Slot.Count];

            public WeaponBuilder() { for (int i = 0; i < t.Length; i++) t[i] = new List<int>(); }

            int Add(Vector3 p, Vector3 nn) { v.Add(p); n.Add(nn.normalized); uv.Add(new Vector2(p.x, p.y)); return v.Count - 1; }

            void Tri(Slot s, int a, int b, int c) { t[(int)s].Add(a); t[(int)s].Add(b); t[(int)s].Add(c); }

            /// <summary>A tapered cylinder from (x, y0) to (x, y1) along y.</summary>
            public void Cyl(Slot s, float y0, float y1, float r0, float r1, int sides = 7, float x = 0f, float z = 0f)
            {
                Tube(s, new Vector3(x, y0, z), new Vector3(x, y1, z), r0, r1, sides);
            }

            /// <summary>A tapered tube between two points, capped.</summary>
            public void Tube(Slot s, Vector3 a, Vector3 b, float r0, float r1, int sides = 7)
            {
                var axis = (b - a).normalized;
                var u = Vector3.Cross(axis, Mathf.Abs(axis.z) < 0.9f ? Vector3.forward : Vector3.right).normalized;
                var w = Vector3.Cross(axis, u);
                int b0 = v.Count;
                for (int i = 0; i < sides; i++)
                {
                    float an = i * Mathf.PI * 2f / sides;
                    var d = u * Mathf.Cos(an) + w * Mathf.Sin(an);
                    Add(a + d * r0, d); Add(b + d * r1, d);
                }
                for (int i = 0; i < sides; i++)
                {
                    int i0 = b0 + i * 2, i1 = b0 + ((i + 1) % sides) * 2;
                    Tri(s, i0, i1, i0 + 1); Tri(s, i1, i1 + 1, i0 + 1);
                }
                int ca = Add(a, -axis), cb = Add(b, axis);
                int s0 = v.Count;
                for (int i = 0; i < sides; i++)
                {
                    float an = i * Mathf.PI * 2f / sides;
                    var d = u * Mathf.Cos(an) + w * Mathf.Sin(an);
                    Add(a + d * r0, -axis); Add(b + d * r1, axis);
                }
                for (int i = 0; i < sides; i++)
                {
                    int i0 = s0 + i * 2, i1 = s0 + ((i + 1) % sides) * 2;
                    Tri(s, ca, i0, i1); Tri(s, cb, i1 + 1, i0 + 1);
                }
            }

            public void Cone(Slot s, Vector3 a, Vector3 tip, float r, int sides = 5)
            {
                var axis = (tip - a).normalized;
                var u = Vector3.Cross(axis, Mathf.Abs(axis.z) < 0.9f ? Vector3.forward : Vector3.right).normalized;
                var w = Vector3.Cross(axis, u);
                for (int i = 0; i < sides; i++)
                {
                    float a0 = i * Mathf.PI * 2f / sides, a1 = (i + 1) * Mathf.PI * 2f / sides;
                    var d0 = u * Mathf.Cos(a0) + w * Mathf.Sin(a0);
                    var d1 = u * Mathf.Cos(a1) + w * Mathf.Sin(a1);
                    var nn = Vector3.Cross(d1 * r - d0 * r, tip - (a + d0 * r));
                    int i0 = Add(a + d0 * r, d0 + axis * 0.3f), i1 = Add(a + d1 * r, d1 + axis * 0.3f), i2 = Add(tip, (d0 + d1) * 0.5f + axis * 0.3f);
                    if (Vector3.Dot(nn, d0 + d1) > 0) Tri(s, i0, i2, i1); else Tri(s, i0, i1, i2);
                }
            }

            /// <summary>A squashed sphere.</summary>
            public void Ball(Slot s, Vector3 c, Vector3 r, int rings = 4, int seg = 7)
            {
                int b0 = v.Count;
                for (int i = 0; i <= rings; i++)
                {
                    float th = i * Mathf.PI / rings;
                    for (int j = 0; j <= seg; j++)
                    {
                        float ph = j * Mathf.PI * 2f / seg;
                        var d = new Vector3(Mathf.Sin(th) * Mathf.Cos(ph), Mathf.Cos(th), Mathf.Sin(th) * Mathf.Sin(ph));
                        Add(c + Vector3.Scale(d, r), new Vector3(d.x / r.x, d.y / r.y, d.z / r.z));
                    }
                }
                for (int i = 0; i < rings; i++)
                    for (int j = 0; j < seg; j++)
                    {
                        int a = b0 + i * (seg + 1) + j, b = a + seg + 1;
                        Tri(s, a, a + 1, b); Tri(s, a + 1, b + 1, b);
                    }
            }

            /// <summary>The top part of a squashed sphere (a helmet, a cowl): from the pole down to `reach` (0.5 = half).</summary>
            public void Dome(Slot s, Vector3 c, Vector3 r, float reach = 0.5f, int rings = 5, int seg = 12)
            {
                int b0 = v.Count;
                for (int i = 0; i <= rings; i++)
                {
                    float th = i * Mathf.PI * reach / rings;
                    for (int j = 0; j <= seg; j++)
                    {
                        float ph = j * Mathf.PI * 2f / seg;
                        var d = new Vector3(Mathf.Sin(th) * Mathf.Cos(ph), Mathf.Cos(th), Mathf.Sin(th) * Mathf.Sin(ph));
                        Add(c + Vector3.Scale(d, r), new Vector3(d.x / r.x, d.y / r.y, d.z / r.z));
                    }
                }
                for (int i = 0; i < rings; i++)
                    for (int j = 0; j < seg; j++)
                    {
                        int a = b0 + i * (seg + 1) + j, b = a + seg + 1;
                        Tri(s, a, a + 1, b); Tri(s, a + 1, b + 1, b);
                    }
            }

            /// <summary>A flat polygon (x, y) extruded ±hz in z, ear-clipped.</summary>
            public void Prism(Slot s, Vector2[] poly, float hz, float zOff = 0f)
            {
                int nn = poly.Length;
                var idx = EarClip(poly);
                int f = v.Count;
                for (int i = 0; i < nn; i++) Add(new Vector3(poly[i].x, poly[i].y, zOff + hz), Vector3.forward);
                int bk = v.Count;
                for (int i = 0; i < nn; i++) Add(new Vector3(poly[i].x, poly[i].y, zOff - hz), Vector3.back);
                for (int i = 0; i < idx.Count; i += 3)
                {
                    Tri(s, f + idx[i], f + idx[i + 2], f + idx[i + 1]);
                    Tri(s, bk + idx[i], bk + idx[i + 1], bk + idx[i + 2]);
                }
                float area = 0;
                for (int i = 0; i < nn; i++) { var p = poly[i]; var q = poly[(i + 1) % nn]; area += p.x * q.y - q.x * p.y; }
                for (int i = 0; i < nn; i++)
                {
                    var p = poly[i]; var q = poly[(i + 1) % nn];
                    var e = q - p;
                    var outward = area > 0 ? new Vector3(e.y, -e.x, 0) : new Vector3(-e.y, e.x, 0);
                    int a = Add(new Vector3(p.x, p.y, zOff + hz), outward), b = Add(new Vector3(q.x, q.y, zOff + hz), outward);
                    int c = Add(new Vector3(q.x, q.y, zOff - hz), outward), d = Add(new Vector3(p.x, p.y, zOff - hz), outward);
                    if (area > 0) { Tri(s, a, b, c); Tri(s, a, c, d); } else { Tri(s, a, c, b); Tri(s, a, d, c); }
                }
            }

            /// <summary>A two-sided feather: a thin diamond from base along dir.</summary>
            public void Feather(Slot s, Vector3 a, Vector3 dir, float len, float width, Vector3 flat)
            {
                var d = dir.normalized;
                var side = Vector3.Cross(d, flat).normalized * width;
                var nn = Vector3.Cross(side, d);
                var p0 = a; var p1 = a + d * len * 0.35f + side; var p2 = a + d * len; var p3 = a + d * len * 0.35f - side;
                int i0 = Add(p0, nn), i1 = Add(p1, nn), i2 = Add(p2, nn), i3 = Add(p3, nn);
                Tri(s, i0, i1, i2); Tri(s, i0, i2, i3);
                int j0 = Add(p0, -nn), j1 = Add(p1, -nn), j2 = Add(p2, -nn), j3 = Add(p3, -nn);
                Tri(s, j0, j2, j1); Tri(s, j0, j3, j2);
            }

            public Mesh Build(string name, out Slot[] slots)
            {
                var m = new Mesh { name = name };
                m.SetVertices(v); m.SetNormals(n); m.SetUVs(0, uv);
                var used = new List<Slot>();
                for (int i = 0; i < t.Length; i++) if (t[i].Count > 0) used.Add((Slot)i);
                m.subMeshCount = used.Count;
                for (int i = 0; i < used.Count; i++) m.SetTriangles(t[(int)used[i]], i);
                m.RecalculateBounds();
                slots = used.ToArray();
                return m;
            }

            static List<int> EarClip(Vector2[] p)
            {
                var res = new List<int>();
                var idx = new List<int>();
                for (int i = 0; i < p.Length; i++) idx.Add(i);
                float area = 0;
                for (int i = 0; i < p.Length; i++) { var a = p[i]; var b = p[(i + 1) % p.Length]; area += a.x * b.y - b.x * a.y; }
                if (area < 0) idx.Reverse();
                int guard = 0;
                while (idx.Count > 3 && guard++ < 1000)
                {
                    bool clipped = false;
                    for (int i = 0; i < idx.Count; i++)
                    {
                        int ia = idx[(i + idx.Count - 1) % idx.Count], ib = idx[i], ic = idx[(i + 1) % idx.Count];
                        Vector2 a = p[ia], b = p[ib], c = p[ic];
                        if ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x) <= 0) continue;   // reflex
                        bool inside = false;
                        foreach (int j in idx)
                        {
                            if (j == ia || j == ib || j == ic) continue;
                            if (InTri(p[j], a, b, c)) { inside = true; break; }
                        }
                        if (inside) continue;
                        res.Add(ia); res.Add(ib); res.Add(ic);
                        idx.RemoveAt(i);
                        clipped = true;
                        break;
                    }
                    if (!clipped) break;
                }
                if (idx.Count == 3) { res.Add(idx[0]); res.Add(idx[1]); res.Add(idx[2]); }
                return res;
            }

            static bool InTri(Vector2 q, Vector2 a, Vector2 b, Vector2 c)
            {
                float d1 = (q.x - b.x) * (a.y - b.y) - (a.x - b.x) * (q.y - b.y);
                float d2 = (q.x - c.x) * (b.y - c.y) - (b.x - c.x) * (q.y - c.y);
                float d3 = (q.x - a.x) * (c.y - a.y) - (c.x - a.x) * (q.y - a.y);
                bool neg = d1 < 0 || d2 < 0 || d3 < 0, pos = d1 > 0 || d2 > 0 || d3 > 0;
                return !(neg && pos);
            }
        }

        static Vector2[] P(params float[] xy)
        {
            var r = new Vector2[xy.Length / 2];
            for (int i = 0; i < r.Length; i++) r[i] = new Vector2(xy[i * 2], xy[i * 2 + 1]);
            return r;
        }

        static void BeardedHead(WeaponBuilder b, Slot head, float scale, bool ember)
        {
            // a Norse bearded axe head: the blade sweeps forward and its "beard" hooks down along the haft
            var poly = P(-0.03f, 0.93f, 0.0f, 0.97f, 0.08f, 0.955f, 0.2f, 0.995f, 0.31f, 1.03f, 0.36f, 0.92f, 0.355f, 0.78f,
                         0.3f, 0.64f, 0.24f, 0.67f, 0.17f, 0.78f, 0.08f, 0.835f, 0.0f, 0.84f, -0.03f, 0.86f);
            for (int i = 0; i < poly.Length; i++) poly[i] = new Vector2(poly[i].x * scale, 0.97f - (0.97f - poly[i].y) * scale);
            b.Prism(head, poly, 0.012f);
            // the cutting edge, a brighter strip (or glowing embers)
            var edge = P(0.31f, 1.03f, 0.36f, 0.92f, 0.355f, 0.78f, 0.3f, 0.64f, 0.285f, 0.66f, 0.335f, 0.78f, 0.34f, 0.92f, 0.295f, 1.01f);
            for (int i = 0; i < edge.Length; i++) edge[i] = new Vector2(edge[i].x * scale, 0.97f - (0.97f - edge[i].y) * scale);
            b.Prism(ember ? Slot.Ember : Slot.Iron, edge, ember ? 0.016f : 0.0135f);
        }

        static readonly Dictionary<Weapon, (Mesh mesh, Slot[] slots)> s_weapons = new Dictionary<Weapon, (Mesh, Slot[])>();

        static (Mesh mesh, Slot[] slots) WeaponMesh(Weapon w)
        {
            if (s_weapons.TryGetValue(w, out var have)) return have;
            var b = new WeaponBuilder();
            switch (w)
            {
                case Weapon.Blowgun:
                    b.Cyl(Slot.Wood, 0f, 1f, 0.013f, 0.011f, 8);
                    b.Cyl(Slot.Bone, -0.005f, 0.035f, 0.018f, 0.017f, 8);
                    b.Cyl(Slot.Team, 0.22f, 0.26f, 0.0155f, 0.0155f, 8);
                    b.Cyl(Slot.Team, 0.55f, 0.59f, 0.0145f, 0.0145f, 8);
                    b.Cyl(Slot.Gold, 0.96f, 0.98f, 0.0145f, 0.0145f, 8);
                    b.Feather(Slot.FeatherRed, new Vector3(0.012f, 0.95f, 0f), new Vector3(0.25f, -1f, 0f), 0.09f, 0.012f, Vector3.forward);
                    b.Feather(Slot.FeatherGreen, new Vector3(-0.012f, 0.95f, 0f), new Vector3(-0.2f, -1f, 0.1f), 0.08f, 0.011f, Vector3.forward);
                    break;
                case Weapon.Macuahuitl:
                    b.Cyl(Slot.Wood, 0f, 0.34f, 0.018f, 0.021f);
                    b.Cyl(Slot.Leather, 0.04f, 0.26f, 0.023f, 0.023f);
                    b.Ball(Slot.Wood, new Vector3(0, 0f, 0), new Vector3(0.03f, 0.02f, 0.03f));
                    b.Prism(Slot.Wood, P(-0.045f, 0.3f, 0.045f, 0.3f, 0.052f, 0.95f, 0f, 1.0f, -0.052f, 0.95f), 0.012f);
                    for (float y = 0.36f; y < 0.94f; y += 0.064f)
                    {
                        float hw = 0.045f + (y - 0.3f) / 0.65f * 0.007f;
                        b.Prism(Slot.Obsidian, P(hw - 0.004f, y - 0.026f, hw + 0.032f, y, hw - 0.004f, y + 0.026f), 0.006f);
                        b.Prism(Slot.Obsidian, P(-hw + 0.004f, y + 0.026f, -hw - 0.032f, y, -hw + 0.004f, y - 0.026f), 0.006f);
                    }
                    b.Cyl(Slot.Team, 0.3f, 0.33f, 0.024f, 0.024f);
                    break;
                case Weapon.Club:
                    b.Cyl(Slot.Wood, 0f, 0.72f, 0.022f, 0.032f);
                    b.Cyl(Slot.Leather, 0.03f, 0.24f, 0.027f, 0.028f);
                    b.Ball(Slot.Wood, new Vector3(0, 0.83f, 0), new Vector3(0.078f, 0.165f, 0.078f), 5, 8);
                    for (int i = 0; i < 10; i++)
                    {
                        float a = i * Mathf.PI * 2f / 5f + (i >= 5 ? 0.6f : 0f), y = i < 5 ? 0.78f : 0.9f;
                        var d = new Vector3(Mathf.Cos(a), 0.15f, Mathf.Sin(a));
                        var at = new Vector3(0, y, 0) + new Vector3(d.x * 0.07f, 0, d.z * 0.07f);
                        b.Cone(Slot.Stone, at, at + d.normalized * 0.065f, 0.016f, 4);
                    }
                    b.Cyl(Slot.Team, 0.66f, 0.7f, 0.035f, 0.035f);
                    break;
                case Weapon.LongSpear:
                    b.Cyl(Slot.Wood, 0f, 0.9f, 0.0095f, 0.0085f);
                    b.Prism(Slot.Obsidian, P(0f, 0.86f, 0.024f, 0.905f, 0.017f, 0.965f, 0f, 1.0f, -0.017f, 0.965f, -0.024f, 0.905f), 0.0045f);
                    b.Cyl(Slot.Leather, 0.85f, 0.875f, 0.012f, 0.012f);
                    b.Cyl(Slot.Team, 0.45f, 0.5f, 0.0115f, 0.0115f);
                    b.Feather(Slot.FeatherRed, new Vector3(0.01f, 0.85f, 0f), new Vector3(0.3f, -1f, 0f), 0.06f, 0.009f, Vector3.forward);
                    b.Feather(Slot.FeatherBlue, new Vector3(-0.01f, 0.85f, 0f), new Vector3(-0.3f, -1f, 0f), 0.06f, 0.009f, Vector3.forward);
                    b.Feather(Slot.FeatherGreen, new Vector3(0f, 0.85f, 0.01f), new Vector3(0f, -1f, 0.3f), 0.055f, 0.009f, Vector3.right);
                    b.Cyl(Slot.Bone, -0.01f, 0.02f, 0.011f, 0.011f);
                    break;
                case Weapon.ShamanStaff:
                    b.Cyl(Slot.Wood, 0f, 0.9f, 0.012f, 0.016f);
                    for (int i = 0; i < 10; i++)   // a hoop at the top
                    {
                        float a0 = i * Mathf.PI * 2f / 10f, a1 = (i + 1) * Mathf.PI * 2f / 10f;
                        var c = new Vector3(0, 0.95f, 0);
                        b.Tube(Slot.Wood, c + new Vector3(Mathf.Cos(a0), Mathf.Sin(a0), 0) * 0.05f, c + new Vector3(Mathf.Cos(a1), Mathf.Sin(a1), 0) * 0.05f, 0.007f, 0.007f, 5);
                    }
                    b.Ball(Slot.Bone, new Vector3(0, 0.95f, 0), new Vector3(0.024f, 0.028f, 0.024f));
                    b.Cyl(Slot.Team, 0.86f, 0.89f, 0.019f, 0.019f);
                    b.Feather(Slot.FeatherGreen, new Vector3(0.05f, 0.95f, 0f), new Vector3(0.2f, -1f, 0f), 0.11f, 0.014f, Vector3.forward);
                    b.Feather(Slot.FeatherRed, new Vector3(-0.05f, 0.95f, 0f), new Vector3(-0.2f, -1f, 0f), 0.11f, 0.014f, Vector3.forward);
                    b.Feather(Slot.FeatherBlue, new Vector3(0f, 0.9f, 0.02f), new Vector3(0.05f, -1f, 0.25f), 0.09f, 0.012f, Vector3.right);
                    break;
                case Weapon.Javelin:
                    b.Cyl(Slot.Wood, 0f, 0.92f, 0.0105f, 0.0095f);
                    b.Cone(Slot.Stone, new Vector3(0, 0.9f, 0), new Vector3(0, 1f, 0), 0.02f, 4);
                    b.Cyl(Slot.Leather, 0.88f, 0.905f, 0.013f, 0.013f);
                    for (int i = 0; i < 3; i++)
                    {
                        float a = i * Mathf.PI * 2f / 3f;
                        var d = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
                        b.Feather(i == 0 ? Slot.FeatherRed : Slot.FeatherGreen, new Vector3(0, 0.03f, 0), Vector3.up, 0.14f, 0.012f, d);
                    }
                    break;
                case Weapon.Horn:
                    // a curved war horn: a bone horn swept from the mouthpiece to a wide bell, banded in iron
                    {
                        Vector3 prev = new Vector3(0f, 0f, 0f);
                        for (int i = 1; i <= 6; i++)
                        {
                            float tt = i / 6f;
                            var p = new Vector3(Mathf.Sin(tt * 1.4f) * 0.22f, tt * 0.9f, 0f);
                            b.Tube(Slot.Bone, prev, p, 0.018f + 0.05f * (tt - 1f / 6f), 0.018f + 0.05f * tt, 7);
                            prev = p;
                        }
                        b.Cyl(Slot.Iron, 0.3f, 0.34f, 0.04f, 0.04f, 8);
                        b.Cyl(Slot.Iron, 0.62f, 0.66f, 0.055f, 0.055f, 8);
                        b.Ball(Slot.Leather, new Vector3(Mathf.Sin(1.4f) * 0.22f, 0.92f, 0f), new Vector3(0.07f, 0.02f, 0.07f));
                        b.Cyl(Slot.Team, -0.03f, 0.04f, 0.022f, 0.022f, 6);
                    }
                    break;
                case Weapon.HandDrum:
                    // a wooden handle with a hide-headed drum at its top, painted bands
                    b.Cyl(Slot.Wood, 0f, 0.62f, 0.016f, 0.016f, 6);
                    b.Cyl(Slot.Wood, 0.62f, 0.92f, 0.13f, 0.12f, 12);
                    b.Cyl(Slot.Leather, 0.92f, 0.94f, 0.125f, 0.125f, 12);
                    b.Cyl(Slot.FeatherRed, 0.7f, 0.74f, 0.132f, 0.132f, 12);
                    b.Cyl(Slot.Team, 0.84f, 0.87f, 0.128f, 0.128f, 12);
                    b.Feather(Slot.FeatherGreen, new Vector3(0.13f, 0.78f, 0f), new Vector3(1f, -0.6f, 0f), 0.16f, 0.02f, Vector3.forward);
                    break;
                case Weapon.Sling:
                    // two braided cords from the fist to a wide leather cradle holding a fist-sized stone
                    b.Tube(Slot.Leather, new Vector3(-0.014f, 0.02f, 0f), new Vector3(-0.07f, 0.8f, 0f), 0.013f, 0.011f, 5);
                    b.Tube(Slot.Leather, new Vector3(0.014f, 0.02f, 0f), new Vector3(0.07f, 0.8f, 0f), 0.013f, 0.011f, 5);
                    b.Ball(Slot.Leather, new Vector3(0, 0.86f, 0), new Vector3(0.1f, 0.075f, 0.055f));
                    b.Ball(Slot.Stone, new Vector3(0, 0.885f, 0.012f), new Vector3(0.068f, 0.062f, 0.068f));
                    b.Cyl(Slot.Team, -0.03f, 0.07f, 0.028f, 0.028f, 6);   // the wrist loop
                    break;
                case Weapon.FireAxe:
                case Weapon.BeardedAxe:
                case Weapon.Hatchet:
                    b.Cyl(Slot.Wood, 0f, 1.0f, 0.022f, 0.018f);
                    b.Cyl(Slot.Leather, 0.05f, 0.27f, 0.026f, 0.026f);
                    b.Cyl(Slot.Iron, -0.01f, 0.025f, 0.025f, 0.025f);
                    b.Cyl(Slot.Team, 0.29f, 0.32f, 0.024f, 0.024f);
                    BeardedHead(b, w == Weapon.FireAxe ? Slot.Obsidian : Slot.Iron, w == Weapon.Hatchet ? 0.8f : 1f, w == Weapon.FireAxe);
                    break;
                case Weapon.DaneAxe:
                    b.Cyl(Slot.Wood, 0f, 1.0f, 0.0145f, 0.0125f);
                    b.Cyl(Slot.Leather, 0.04f, 0.2f, 0.017f, 0.017f);
                    b.Cyl(Slot.Leather, 0.5f, 0.6f, 0.016f, 0.016f);
                    b.Cyl(Slot.Iron, 0.78f, 0.86f, 0.0165f, 0.0165f);
                    b.Prism(Slot.Iron, P(-0.02f, 0.93f, 0f, 0.965f, 0.06f, 0.955f, 0.15f, 1.005f, 0.215f, 0.995f, 0.235f, 0.885f,
                                         0.215f, 0.765f, 0.145f, 0.755f, 0.06f, 0.855f, 0f, 0.865f, -0.02f, 0.89f), 0.008f);
                    b.Prism(Slot.Gold, P(0.205f, 0.995f, 0.235f, 0.885f, 0.215f, 0.765f, 0.2f, 0.775f, 0.218f, 0.885f, 0.192f, 0.985f), 0.0095f);
                    b.Cyl(Slot.Team, 0.62f, 0.66f, 0.017f, 0.017f);
                    break;
                case Weapon.SkaldStaff:
                    b.Cyl(Slot.Wood, 0f, 0.93f, 0.014f, 0.018f);
                    b.Ball(Slot.Wood, new Vector3(0, 0.95f, 0), new Vector3(0.036f, 0.05f, 0.036f), 4, 8);
                    b.Cyl(Slot.Iron, 0.89f, 0.91f, 0.021f, 0.021f);
                    b.Cyl(Slot.Rune, 0.8f, 0.83f, 0.0195f, 0.0195f, 8);
                    b.Cyl(Slot.Rune, 0.7f, 0.72f, 0.019f, 0.019f, 8);
                    // a drinking horn hanging from the top
                    var h0 = new Vector3(0.02f, 0.88f, 0f);
                    var pts = new[] { h0, h0 + new Vector3(0.035f, -0.04f, 0f), h0 + new Vector3(0.06f, -0.1f, 0f), h0 + new Vector3(0.065f, -0.16f, 0f), h0 + new Vector3(0.05f, -0.2f, 0f) };
                    float[] rr = { 0.026f, 0.022f, 0.017f, 0.011f, 0.004f };
                    for (int i = 0; i < pts.Length - 1; i++) b.Tube(Slot.Bone, pts[i], pts[i + 1], rr[i], rr[i + 1], 6);
                    b.Cyl(Slot.Gold, 0.875f, 0.885f, 0.027f, 0.027f, 6, 0.02f);
                    b.Cyl(Slot.Team, 0.6f, 0.64f, 0.0185f, 0.0185f);
                    break;
                case Weapon.CrystalSpear:   // a spear with a long crystal point that catches the light
                    b.Cyl(Slot.Wood, 0f, 0.86f, 0.0105f, 0.0095f);
                    b.Cyl(Slot.Leather, 0.8f, 0.84f, 0.013f, 0.013f);
                    b.Prism(Slot.Crystal, P(0f, 0.82f, 0.03f, 0.88f, 0.02f, 0.96f, 0f, 1.04f, -0.02f, 0.96f, -0.03f, 0.88f), 0.012f);
                    b.Cyl(Slot.Team, 0.45f, 0.49f, 0.0115f, 0.0115f);
                    break;
                case Weapon.CrystalAxe:   // a throwing axe with a crystal blade
                    b.Cyl(Slot.Wood, 0f, 1.0f, 0.02f, 0.017f);
                    b.Cyl(Slot.Leather, 0.05f, 0.25f, 0.024f, 0.024f);
                    b.Cyl(Slot.Team, 0.29f, 0.32f, 0.023f, 0.023f);
                    BeardedHead(b, Slot.Crystal, 0.95f, false);
                    break;
                case Weapon.DiamondSpear:   // a heavy spear: gold collar, diamond head
                    b.Cyl(Slot.Wood, 0f, 0.86f, 0.013f, 0.011f);
                    b.Cyl(Slot.Gold, 0.82f, 0.86f, 0.016f, 0.016f);
                    b.Prism(Slot.Diamond, P(0f, 0.84f, 0.04f, 0.9f, 0.026f, 0.97f, 0f, 1.03f, -0.026f, 0.97f, -0.04f, 0.9f), 0.016f);
                    b.Cyl(Slot.Team, 0.4f, 0.45f, 0.014f, 0.014f);
                    b.Cyl(Slot.Iron, -0.01f, 0.03f, 0.014f, 0.014f);
                    break;
                case Weapon.DiamondAxe:   // a long two-handed axe with a diamond-edged head
                    b.Cyl(Slot.Wood, 0f, 1.0f, 0.0145f, 0.0125f);
                    b.Cyl(Slot.Leather, 0.04f, 0.2f, 0.017f, 0.017f);
                    b.Cyl(Slot.Gold, 0.78f, 0.86f, 0.0165f, 0.0165f);
                    b.Prism(Slot.Iron, P(-0.02f, 0.93f, 0f, 0.965f, 0.06f, 0.955f, 0.15f, 1.005f, 0.215f, 0.995f, 0.235f, 0.885f,
                                         0.215f, 0.765f, 0.145f, 0.755f, 0.06f, 0.855f, 0f, 0.865f, -0.02f, 0.89f), 0.008f);
                    b.Prism(Slot.Diamond, P(0.205f, 0.995f, 0.24f, 0.885f, 0.215f, 0.765f, 0.195f, 0.775f, 0.218f, 0.885f, 0.19f, 0.985f), 0.011f);
                    b.Cyl(Slot.Team, 0.62f, 0.66f, 0.017f, 0.017f);
                    break;
                case Weapon.DartBlowgun:   // the Dart Master's long blowgun, bound with crystal rings
                    b.Cyl(Slot.Wood, 0f, 1f, 0.012f, 0.0105f, 8);
                    b.Cyl(Slot.Bone, -0.005f, 0.035f, 0.017f, 0.016f, 8);
                    b.Cyl(Slot.Crystal, 0.3f, 0.33f, 0.0155f, 0.0155f, 8);
                    b.Cyl(Slot.Crystal, 0.62f, 0.65f, 0.0145f, 0.0145f, 8);
                    b.Cyl(Slot.Team, 0.45f, 0.49f, 0.015f, 0.015f, 8);
                    b.Feather(Slot.FeatherBlue, new Vector3(0.012f, 0.95f, 0f), new Vector3(0.25f, -1f, 0f), 0.1f, 0.012f, Vector3.forward);
                    b.Feather(Slot.FeatherRed, new Vector3(-0.012f, 0.95f, 0f), new Vector3(-0.2f, -1f, 0.1f), 0.09f, 0.011f, Vector3.forward);
                    break;
            }
            var mesh = b.Build("Unique " + w, out var slots);
            s_weapons[w] = (mesh, slots);
            return (mesh, slots);
        }

        // ------------------------------------------------------------------ materials

        static readonly Dictionary<string, Material> s_slotMats = new Dictionary<string, Material>();

        static Material SlotMat(Slot s, Color team, Material bm)
        {
            string key = s == Slot.Team ? "team" + ColorUtility.ToHtmlStringRGB(team) : s.ToString();
            if (s_slotMats.TryGetValue(key, out var m) && m != null) return m;
            switch (s)
            {
                case Slot.Wood: m = Plain(bm, new Color(0.45f, 0.29f, 0.16f, 1f), 0.25f, false, "Unique wood"); break;
                case Slot.Stone: m = Plain(bm, new Color(0.55f, 0.53f, 0.5f, 1f), 0.3f, false, "Unique stone"); break;
                case Slot.Obsidian: m = Plain(bm, new Color(0.08f, 0.07f, 0.1f, 1f), 2.2f, false, "Unique obsidian"); break;
                case Slot.Iron: m = Plain(bm, new Color(0.62f, 0.65f, 0.7f, 1f), 1.8f, false, "Unique iron"); break;
                case Slot.Gold: m = Plain(bm, new Color(1.25f, 0.9f, 0.3f, 1f), 2f, false, "Unique gold"); break;
                case Slot.Bone: m = Plain(bm, new Color(0.93f, 0.88f, 0.76f, 1f), 0.6f, false, "Unique bone"); break;
                case Slot.Leather: m = Plain(bm, new Color(0.33f, 0.2f, 0.12f, 1f), 0.4f, false, "Unique leather"); break;
                case Slot.Team: m = Plain(bm, team, 0.5f, false, "Unique team"); break;
                case Slot.FeatherRed: m = Plain(bm, new Color(0.9f, 0.18f, 0.12f, 1f), 0.3f, true, "Unique feather red"); break;
                case Slot.FeatherGreen: m = Plain(bm, new Color(0.15f, 0.7f, 0.35f, 1f), 0.3f, true, "Unique feather green"); break;
                case Slot.FeatherBlue: m = Plain(bm, new Color(0.15f, 0.4f, 0.95f, 1f), 0.3f, true, "Unique feather blue"); break;
                case Slot.Crystal: m = Plain(bm, new Color(0.45f, 1.1f, 1.45f, 1f), 2.6f, false, "Unique crystal"); break;   // the arsenal
                case Slot.Diamond: m = Plain(bm, new Color(1.5f, 1.55f, 1.7f, 1f), 3f, false, "Unique diamond"); break;
                case Slot.FeatherWhite: m = Plain(bm, new Color(1.05f, 1.05f, 1.08f, 1f), 0.5f, true, "Unique feather white"); break;   // the gods' regalia
                case Slot.Fur: m = Plain(bm, new Color(0.92f, 0.6f, 0.2f, 1f), 0.2f, false, "Unique fur"); break;
                case Slot.Ember:
                case Slot.Rune:
                case Slot.Spirit:
                {
                    var sh = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
                    m = new Material(sh) { name = "Unique " + s, enableInstancing = true };
                    m.SetColor("_BaseColor", s == Slot.Ember ? new Color(3.2f, 1.1f, 0.25f, 1f) : s == Slot.Spirit ? new Color(0.5f, 2.6f, 1.1f, 1f) : new Color(0.4f, 1.6f, 2.6f, 1f));
                    break;
                }
            }
            s_slotMats[key] = m;
            return m;
        }

        // ------------------------------------------------------------------ fitting onto the warrior

        /// <summary>Dressed models: unit root -> the model instance that was dressed (its weapon / cape are children of
        /// its bones, so they go when the model is rebuilt, and the instance reads as null then).</summary>
        static readonly Dictionary<int, GameObject> s_dressed = new Dictionary<int, GameObject>();

        static int BoneIndex(Transform[] bones, string suffix)
        {
            for (int i = 0; i < bones.Length; i++) if (bones[i] != null && bones[i].name.EndsWith(suffix)) return i;
            return -1;
        }

        static Vector3 RestPos(Mesh mesh, int bone) => mesh.bindposes[bone].inverse.MultiplyPoint3x4(Vector3.zero);

        /// <summary>
        /// Arms and dresses an Extended warrior once per model instance: its own weapon instead of the painted one (when
        /// Build Real Weapons has stored the painted weapon's frame), a second axe for the Berserker, and a cape.
        /// Safe to call every frame.
        /// </summary>
        public static void DressUnique(GameObject root, Unit u, int kind, Color team, Material baseMaterial)
        {
            int id = root.GetHashCode();
            if (s_dressed.TryGetValue(id, out var have) && have != null) return;
            var smr = root.GetComponentInChildren<SkinnedMeshRenderer>(true);
            if (smr == null) return;   // not built yet
            s_dressed[id] = smr.gameObject;
            if (s_dressed.Count > 4096) Prune();
            bool vik = Vikings(u);
            try
            {
                var w = WeaponFor(kind, vik);
                // (melee warriors keep their weapon through the blow - the throw's release no longer takes it out of the hand)
                bool melee = u.getWeaponFactory() is com.oddlabs.tt.model.weapon.MeleeFactory;
                if (w != Weapon.None) Arm(root, w, (kind == Race.UNIT_UNIQUE_2 && vik) || kind == Race.UNIT_SPINNER, team, baseMaterial, melee);
                var c = CapeFor(kind, vik);
                if (c != Cape.None) AttachCape(root, c, vik, team, baseMaterial);
                if (TribalTrouble.Game.GameSettings.graphicsStyle == 1) PutHat(root, kind, vik, team, baseMaterial);   // helms, hoods, crests (modern look)
                ApplySkin(root, SkinFor(u, kind));
            }
            catch (System.Exception e) { Debug.LogWarning("[Tribal Trouble] dressing " + kind + ": " + e.Message); }
        }

        // ------------------------------------------------------------------ body skins

        static readonly Dictionary<string, Texture2D> s_skins = new Dictionary<string, Texture2D>();
        static readonly MaterialPropertyBlock s_skinBlock = new MaterialPropertyBlock();
        static readonly int SkinBaseMapId = Shader.PropertyToID("_BaseMap");

        static string KindName(int kind)
        {
            switch (kind)
            {
                case Race.UNIT_CHAMPION: return "champion";
                case Race.UNIT_SCOUT: return "scout";
                case Race.UNIT_SHAMAN: return "shaman";
                case Race.UNIT_SLINGER: return "slinger";
                case Race.UNIT_MELEE_LIGHT: return "melee_light";
                case Race.UNIT_MELEE_HEAVY: return "melee_heavy";
                case Race.UNIT_UNIQUE_1: return "unique1";
                case Race.UNIT_UNIQUE_2: return "unique2";
                case Race.UNIT_CRYSTAL: return "crystal";
                case Race.UNIT_DIAMOND: return "diamond";
                case Race.UNIT_UNIQUE_3: return "unique3";
                case Race.UNIT_MINOR_GOD: return "minor_god";
                case Race.UNIT_MAJOR_GOD: return "major_god";
                case Race.UNIT_ZOMBIE: return "zombie";
                case Race.UNIT_ZOMBIE_BRUTE: return "zombie_brute";
                case Race.UNIT_ZOMBIE_BOSS: return "zombie_boss";
                case Race.UNIT_ELITE_1: return "elite1";
                case Race.UNIT_ELITE_2: return "elite2";
                case Race.UNIT_ELITE_3: return "elite3";
                case Race.UNIT_SPINNER: return "spinner";
                default: return null;
            }
        }

        /// <summary>The unit kind's own body texture (Resources/TTSkins, made by PortTools/gen_skins.py from the warrior
        /// texture its template uses: war paint, pelts, quilted armour, mail, soot, woad, rot), or null.</summary>
        static Texture2D SkinFor(Unit u, int kind)
        {
            string k = KindName(kind);
            if (k == null) return null;
            bool vik;
            if (kind == Race.UNIT_ZOMBIE || kind == Race.UNIT_ZOMBIE_BRUTE || kind == Race.UNIT_ZOMBIE_BOSS)
            {
                // the dead wear the body of the race they fell from (their template's), not their owner's
                var rr = u.getOwner().getWorld().getRacesResources();
                vik = u.getTemplate() == rr.getRace(RacesResources.RACE_VIKINGS).getUnitTemplate(kind);
            }
            else vik = Vikings(u);
            string name = (vik ? "viking_" : "native_") + k;
            if (kind == Race.UNIT_MINOR_GOD || kind == Race.UNIT_MAJOR_GOD || kind == Race.UNIT_ZOMBIE_BOSS) name += "_chief";   // the gods / the Risen Chieftain: a body of their own on the chieftain model (gen_god_skins.py)
            if (s_skins.TryGetValue(name, out var tex)) return tex;
            tex = Resources.Load<Texture2D>("TTSkins/" + name);
            if (tex != null && tex.anisoLevel < 8) tex.anisoLevel = 8;
            s_skins[name] = tex;
            return tex;
        }

        /// <summary>The body skin of a unit of a New Tides kind (for units drawn in bulk too), or null.</summary>
        public static Texture2D BodySkin(Unit u)
        {
            int kind = TTExtendedArt.UnitKind(u);
            return kind >= 0 ? SkinFor(u, kind) : null;
        }

        // ------------------------------------------------------------------ the Elemental tribe (chapter 3)

        sealed class ElementalState { public Renderer[] renderers; }
        static readonly Dictionary<int, ElementalState> s_elemental = new Dictionary<int, ElementalState>();
        static readonly Dictionary<string, Texture2D> s_elTex = new Dictionary<string, Texture2D>();
        static readonly List<Renderer> s_elScratch = new List<Renderer>();

        /// <summary>
        /// An Elemental tribe's unit or building: its model wears the Elemental version of its own texture
        /// (Resources/TTSkins/el_&lt;texture&gt;, made by PortTools/skins/gen_elemental.py: basalt, ochre and moss, ember
        /// cracks). Called every frame; the renderers are found once per model instance.
        /// </summary>
        public static void ApplyElemental(GameObject root)
        {
            int id = root.GetHashCode();
            if (s_elemental.TryGetValue(id, out var st))
            {
                bool valid = st.renderers.Length > 0;
                foreach (var r in st.renderers) if (r == null) { valid = false; break; }
                if (valid) return;
            }
            if (s_elemental.Count > 4096) s_elemental.Clear();
            s_elScratch.Clear();
            root.GetComponentsInChildren(true, s_elScratch);
            var list = new List<Renderer>();
            foreach (var r in s_elScratch)
            {
                if (r == null) continue;
                string n = r.gameObject.name;
                if (n == "Cape" || n == "Shield" || n.StartsWith("Glow") || n.StartsWith("Unique") || n == "Caster orb") continue;
                var mat = r.sharedMaterial;
                var bt = mat != null && mat.HasProperty(SkinBaseMapId) ? mat.GetTexture(SkinBaseMapId) : null;
                if (bt == null) continue;
                string key = bt.name.StartsWith("el_") ? null : bt.name;
                if (key == null) continue;
                if (!s_elTex.TryGetValue(key, out var el))
                {
                    el = Resources.Load<Texture2D>("TTSkins/el_" + key);
                    if (el != null && el.anisoLevel < 8) el.anisoLevel = 8;
                    s_elTex[key] = el;
                }
                if (el == null) continue;
                r.GetPropertyBlock(s_skinBlock);
                s_skinBlock.SetTexture(SkinBaseMapId, el);
                r.SetPropertyBlock(s_skinBlock);
                list.Add(r);
            }
            s_elemental[id] = new ElementalState { renderers = list.ToArray() };
        }

        /// <summary>Puts the skin on the body (both LODs; the property block keeps the team colour / tint set on it).</summary>
        static void ApplySkin(GameObject root, Texture2D skin)
        {
            if (skin == null) return;
            foreach (var smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if (smr == null || smr.gameObject.name == "Cape") continue;
                smr.GetPropertyBlock(s_skinBlock);
                s_skinBlock.SetTexture(SkinBaseMapId, skin);
                smr.SetPropertyBlock(s_skinBlock);
            }
        }

        static void Prune()
        {
            var dead = new List<int>();
            foreach (var kv in s_dressed) if (kv.Value == null) dead.Add(kv.Key);
            foreach (var k in dead) s_dressed.Remove(k);
        }

        static void Arm(GameObject root, Weapon w, bool dual, Color team, Material bm, bool melee)
        {
            var held = TTRealWeapons.Held;
            if (held == null) return;
            Proportions(w, out float k, out float gripCanon);
            foreach (var smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                foreach (var h in held)
                {
                    if (h == null || h.stripped == null || h.frameLength <= 0f || (smr.sharedMesh != h.original && smr.sharedMesh != h.stripped)) continue;
                    smr.sharedMesh = h.stripped;   // the painted weapon goes
                    var bones = smr.bones;
                    int bi = -1;
                    for (int i = 0; i < bones.Length; i++) if (bones[i] != null && bones[i].name == h.bone) { bi = i; break; }
                    if (bi < 0) continue;
                    var bone = bones[bi];
                    var real = bone.Find("Real weapon");
                    if (real != null) Object.Destroy(real.gameObject);
                    if (bone.Find("Unique weapon") != null) continue;   // the other LOD shares the skeleton

                    // the painted weapon's frame in bone space; the grip is where the right hand is
                    var mesh = h.stripped;
                    // on the shaft's line (the box centre is off to the blade side), so the new weapon sits in the fist
                    Vector3 c = ShaftOf(h, bi), a1 = h.frameAxis.normalized, a2 = h.frameSide.normalized;
                    var a3 = Vector3.Cross(a2, a1);
                    float L = h.frameLength;
                    float gripPainted;
                    int rh = BoneIndex(bones, "R Hand");
                    if (rh >= 0 && rh < mesh.bindposes.Length)
                    {
                        var handInBone = mesh.bindposes[bi].MultiplyPoint3x4(RestPos(mesh, rh));
                        gripPainted = Mathf.Clamp(Vector3.Dot(handInBone - c, a1) / L + 0.5f, 0.05f, 0.95f);
                    }
                    else gripPainted = 0.25f;
                    if (gripCanon < 0f) gripCanon = gripPainted;
                    float size = L * k;
                    var gripPoint = c + a1 * ((gripPainted - 0.5f) * L);
                    var origin = gripPoint - a1 * (gripCanon * size);
                    var m = Matrix4x4.identity;
                    m.SetColumn(0, a2 * size); m.SetColumn(1, a1 * size); m.SetColumn(2, a3 * size);
                    m.SetColumn(3, new Vector4(origin.x, origin.y, origin.z, 1f));

                    var go = MakeWeapon("Unique weapon", w, team, bm);
                    go.transform.SetParent(bone, false);
                    SetLocal(go.transform, m);
                    if (!string.IsNullOrEmpty(h.bone2) && h.anchorInBone.sqrMagnitude > 1e-6f)
                    {
                        Transform b2 = null;
                        foreach (var bb in bones) if (bb != null && bb.name == h.bone2) { b2 = bb; break; }
                        if (b2 != null)
                        {
                            var al = go.AddComponent<TTHeldWeaponAligner>();
                            al.bone = bone; al.bone2 = b2; al.anchorInBone = h.anchorInBone; al.anchorInBone2 = h.anchorInBone2;
                            al.basePos = go.transform.localPosition; al.baseRot = go.transform.localRotation;
                            al.hand = rh >= 0 ? bones[rh] : null; al.keepInHand = melee || w == Weapon.Blowgun || w == Weapon.DartBlowgun;   // (a blowgun isn't thrown)
                        }
                    }
                    // the Berserker's second axe: the first one mirrored across the body into the left hand
                    int lh = BoneIndex(bones, "L Hand");
                    if (dual && lh >= 0 && rh >= 0 && lh < mesh.bindposes.Length)
                    {
                        var lHand = bones[lh];
                        Vector3 pl = RestPos(mesh, lh), pr = RestPos(mesh, rh);
                        var nrm = (pl - pr).normalized;
                        var mid = (pl + pr) * 0.5f;
                        var refl = Matrix4x4.identity;
                        refl.m00 = 1 - 2 * nrm.x * nrm.x; refl.m01 = -2 * nrm.x * nrm.y; refl.m02 = -2 * nrm.x * nrm.z;
                        refl.m10 = -2 * nrm.y * nrm.x; refl.m11 = 1 - 2 * nrm.y * nrm.y; refl.m12 = -2 * nrm.y * nrm.z;
                        refl.m20 = -2 * nrm.z * nrm.x; refl.m21 = -2 * nrm.z * nrm.y; refl.m22 = 1 - 2 * nrm.z * nrm.z;
                        var mirror = Matrix4x4.Translate(mid) * refl * Matrix4x4.Translate(-mid);
                        var inMesh = mesh.bindposes[bi].inverse * m;
                        var left = mesh.bindposes[lh] * mirror * inMesh * Matrix4x4.Scale(new Vector3(1f, 1f, -1f));
                        if (lHand.Find("Unique weapon L") == null)
                        {
                            var g2 = MakeWeapon("Unique weapon L", w, team, bm);
                            g2.transform.SetParent(lHand, false);
                            SetLocal(g2.transform, left);
                        }
                    }
                }
            }
        }

        static readonly Dictionary<TTRealWeaponSet.HeldWeapon, Vector3> s_shafts = new Dictionary<TTRealWeaponSet.HeldWeapon, Vector3>();

        /// <summary>The painted weapon's shaft point (bone space): baked by Build Real Weapons, else read off the
        /// original mesh when it is readable, else the box centre.</summary>
        static Vector3 ShaftOf(TTRealWeaponSet.HeldWeapon h, int bi)
        {
            if (h.frameShaft != Vector3.zero) return h.frameShaft;
            if (s_shafts.TryGetValue(h, out var got)) return got;
            var result = h.frameCenter;
            try
            {
                var mesh = h.original;
                if (mesh != null && mesh.isReadable && bi < mesh.bindposes.Length)
                {
                    var verts = mesh.vertices; var weights = mesh.boneWeights;
                    var pts = new List<Vector3>();
                    for (int i = 0; i < verts.Length && i < weights.Length; i++)
                    {
                        var w = weights[i];
                        int dom = w.weight0 >= w.weight1 && w.weight0 >= w.weight2 && w.weight0 >= w.weight3 ? w.boneIndex0
                                : w.weight1 >= w.weight2 && w.weight1 >= w.weight3 ? w.boneIndex1 : w.weight2 >= w.weight3 ? w.boneIndex2 : w.boneIndex3;
                        if (dom == bi) pts.Add(mesh.bindposes[bi].MultiplyPoint3x4(verts[i]));
                    }
                    if (pts.Count >= 4)
                    {
                        var fr = TTRealWeapons.Frame.Of(pts);
                        // the same weapon as the baked frame? (then its shaft applies)
                        if ((fr.center - h.frameCenter).sqrMagnitude < 0.01f * h.frameLength * h.frameLength) result = fr.shaft;
                    }
                }
            }
            catch (System.Exception) { }
            s_shafts[h] = result;
            return result;
        }

        static GameObject MakeWeapon(string name, Weapon w, Color team, Material bm)
        {
            var (mesh, slots) = WeaponMesh(w);
            var go = new GameObject(name);
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            var r = go.AddComponent<MeshRenderer>();
            var mats = new Material[slots.Length];
            for (int i = 0; i < slots.Length; i++) mats[i] = SlotMat(slots[i], team, bm);
            r.sharedMaterials = mats;
            return go;
        }

        /// <summary>Sets a transform from an orthogonal matrix with uniform scale (a reflection is turned into a flip of z).</summary>
        static void SetLocal(Transform t, Matrix4x4 m)
        {
            Vector3 x = m.GetColumn(0), y = m.GetColumn(1), z = m.GetColumn(2);
            float s = y.magnitude;
            if (Vector3.Dot(Vector3.Cross(x, y), z) < 0f) z = -z;   // keep it a rotation (the meshes are symmetric in z)
            t.localPosition = m.GetColumn(3);
            t.localRotation = Quaternion.LookRotation(z, y);
            t.localScale = Vector3.one * s;
        }

        // ------------------------------------------------------------------ capes and pelts

        static readonly Dictionary<(Mesh, Cape), Mesh> s_capes = new Dictionary<(Mesh, Cape), Mesh>();
        static readonly Dictionary<Cape, Texture2D> s_capeTex = new Dictionary<Cape, Texture2D>();

        static void AttachCape(GameObject root, Cape cape, bool vik, Color team, Material bm)
        {
            var smr = root.GetComponentInChildren<SkinnedMeshRenderer>(true);
            if (smr == null || smr.sharedMesh == null) return;
            var mesh = smr.sharedMesh;
            var bones = smr.bones;
            int neck = BoneIndex(bones, "Neck"), pelvis = BoneIndex(bones, "Pelvis"), lu = BoneIndex(bones, "L UpperArm"),
                ru = BoneIndex(bones, "R UpperArm"), toe = BoneIndex(bones, "L Toe0"), foot = BoneIndex(bones, "L Foot");
            int spine = BoneIndex(bones, "Spine2");
            if (spine < 0) spine = BoneIndex(bones, "Spine1");
            if (neck < 0 || pelvis < 0 || lu < 0 || ru < 0 || toe < 0 || foot < 0 || spine < 0) return;
            int n = mesh.bindposes.Length;
            if (neck >= n || pelvis >= n || lu >= n || ru >= n || toe >= n || foot >= n || spine >= n) return;
            var holder = smr.transform.parent != null ? smr.transform.parent : root.transform;
            if (holder.Find("Cape") != null) return;
            int spine1 = BoneIndex(bones, "Spine1");
            if (spine1 < 0 || spine1 >= n) spine1 = spine;
            if (!s_capes.TryGetValue((mesh, cape), out var cm))
            {
                Vector3 pN = RestPos(mesh, neck), pP = RestPos(mesh, pelvis), pL = RestPos(mesh, lu), pR = RestPos(mesh, ru);
                var up = (pN - pP).normalized;
                var side = (pL - pR); float shoulders = side.magnitude; side /= Mathf.Max(1e-4f, shoulders);
                var fwd = Vector3.ProjectOnPlane(RestPos(mesh, toe) - RestPos(mesh, foot), up).normalized;
                if (fwd.sqrMagnitude < 0.5f) fwd = Vector3.Cross(side, up);
                float torso = (pN - pP).magnitude;
                float len = torso * (cape == Cape.Jaguar || cape == Cape.Wolf ? 0.8f : cape == Cape.Feathers ? 1.05f : 1.25f);
                float back = (vik ? 0.33f : 0.22f) * torso;   // clear of the back (measured off both warrior models: the back is 0.2-0.3 / 0.1-0.17 torso behind the spine)
                float w0 = shoulders * (vik ? 0.95f : 0.85f), w1 = w0 * (cape == Cape.Wolf || cape == Cape.Jaguar ? 1.15f : 1.4f);
                var top = pN - up * (0.04f * torso) - fwd * back;
                const int cols = 6, rows = 6;
                var v = new List<Vector3>(); var nn = new List<Vector3>(); var uv = new List<Vector2>(); var t = new List<int>();
                var weights = new List<BoneWeight>();
                for (int j = 0; j <= rows; j++)
                    for (int i = 0; i <= cols; i++)
                    {
                        float s = i / (float)cols - 0.5f, tt = j / (float)rows;
                        float width = Mathf.Lerp(w0, w1, tt);
                        float depth = back * 0.2f + tt * tt * 0.22f * torso + (0.25f - s * s) * 0.25f * torso * (1f - 0.4f * tt);
                        var p = top + side * (s * width) - up * (tt * len) - fwd * depth;
                        v.Add(p);   // mesh space: the cape is skinned to the same skeleton
                        nn.Add(-fwd);
                        uv.Add(new Vector2(s + 0.5f, 1f - tt));
                        // shoulders ride the upper spine, the middle the lower spine, the hem the hips - so it sways
                        // with the body instead of standing stiff while the legs swing through it
                        var bw = new BoneWeight();
                        if (tt < 0.5f) { bw.boneIndex0 = spine; bw.weight0 = 1f - tt; bw.boneIndex1 = spine1; bw.weight1 = tt; }
                        else { float h = (tt - 0.5f) * 2f; bw.boneIndex0 = spine1; bw.weight0 = 0.6f - 0.3f * h; bw.boneIndex1 = pelvis; bw.weight1 = 0.4f + 0.3f * h; }
                        if (bw.boneIndex1 == bw.boneIndex0) { bw.weight0 += bw.weight1; bw.weight1 = 0f; }
                        weights.Add(bw);
                    }
                for (int j = 0; j < rows; j++)
                    for (int i = 0; i < cols; i++)
                    {
                        int a = j * (cols + 1) + i, b = a + 1, c = a + cols + 1, d = c + 1;
                        t.Add(a); t.Add(c); t.Add(b); t.Add(b); t.Add(c); t.Add(d);
                    }
                cm = new Mesh { name = "Cape " + cape };
                cm.SetVertices(v); cm.SetNormals(nn); cm.SetUVs(0, uv); cm.SetTriangles(t, 0);
                cm.boneWeights = weights.ToArray();
                cm.bindposes = mesh.bindposes;
                cm.RecalculateBounds();
                s_capes[(mesh, cape)] = cm;
            }
            var go = new GameObject("Cape");
            go.transform.SetParent(holder, false);
            go.transform.localPosition = smr.transform.localPosition;
            go.transform.localRotation = smr.transform.localRotation;
            go.transform.localScale = smr.transform.localScale;
            var cr = go.AddComponent<SkinnedMeshRenderer>();
            cr.sharedMesh = cm;
            cr.bones = bones;
            cr.rootBone = smr.rootBone;
            cr.localBounds = smr.localBounds;
            cr.updateWhenOffscreen = false;
            cr.sharedMaterial = CapeMat(cape, team, bm);
        }

        static Material CapeMat(Cape cape, Color team, Material bm)
        {
            string key = "cape" + cape + (cape == Cape.Team ? ColorUtility.ToHtmlStringRGB(team) : "");
            if (s_slotMats.TryGetValue(key, out var m) && m != null) return m;
            m = Plain(bm, cape == Cape.Team ? team : Color.white, cape == Cape.Wolf || cape == Cape.Jaguar ? 0.2f : 0.35f, true, "Cape " + cape);
            m.SetTexture("_BaseMap", CapeTexture(cape));
            s_slotMats[key] = m;
            return m;
        }

        static Texture2D CapeTexture(Cape cape)
        {
            if (s_capeTex.TryGetValue(cape, out var tex) && tex != null) return tex;
            const int W = 128, H = 128;
            tex = new Texture2D(W, H, TextureFormat.RGBA32, true) { name = "Cape " + cape, wrapMode = TextureWrapMode.Clamp, anisoLevel = 4 };
            var px = new Color32[W * H];
            var rng = new System.Random(17 + (int)cape);
            // jaguar rosettes
            var spots = new List<Vector3>();
            if (cape == Cape.Jaguar) for (int i = 0; i < 60; i++) spots.Add(new Vector3(rng.Next(W), rng.Next(H), 4 + rng.Next(5)));
            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    float u = x / (float)W, v = y / (float)H;
                    float noise = Mathf.PerlinNoise(x * 0.15f, y * 0.15f), fine = Mathf.PerlinNoise(x * 0.6f + 7f, y * 0.9f);
                    Color c;
                    switch (cape)
                    {
                        case Cape.Team:
                        {
                            float trim = v < 0.06f || u < 0.04f || u > 0.96f ? 0.55f : 1f;
                            c = Color.white * (0.85f + 0.15f * noise) * trim;
                            if (v > 0.06f && v < 0.1f) c = new Color(1f, 0.82f, 0.35f);
                            break;
                        }
                        case Cape.Jaguar:
                        {
                            c = Color.Lerp(new Color(0.95f, 0.65f, 0.22f), new Color(1f, 0.85f, 0.5f), noise);
                            foreach (var s in spots)
                            {
                                float d = Mathf.Sqrt((x - s.x) * (x - s.x) + (y - s.y) * (y - s.y));
                                if (d < s.z && d > s.z * 0.45f) { c = new Color(0.12f, 0.08f, 0.04f); break; }
                                if (d <= s.z * 0.45f) { c = Color.Lerp(c, new Color(0.75f, 0.45f, 0.15f), 0.6f); break; }
                            }
                            break;
                        }
                        case Cape.Wolf:
                        {
                            float stripe = Mathf.Abs(Mathf.Sin(x * 0.4f + noise * 6f));
                            c = Color.Lerp(new Color(0.32f, 0.31f, 0.3f), new Color(0.72f, 0.7f, 0.66f), fine * 0.7f + stripe * 0.3f);
                            if (v < 0.12f) c *= 0.7f;   // ragged, darker hem
                            break;
                        }
                        case Cape.Feathers:
                        {
                            int row = (int)(v * 9f);
                            float fu = (u * 10f + (row % 2) * 0.5f) % 1f, fv = (v * 9f) % 1f;
                            bool inFeather = (fu - 0.5f) * (fu - 0.5f) * 4f + (1f - fv) * (1f - fv) * 0.8f < 1f;
                            Color[] cols = { new Color(0.1f, 0.65f, 0.3f), new Color(0.9f, 0.2f, 0.12f), new Color(0.15f, 0.4f, 0.95f), new Color(1f, 0.8f, 0.2f) };
                            c = inFeather ? cols[row % cols.Length] * (0.75f + 0.25f * fv) : new Color(0.2f, 0.15f, 0.1f);
                            break;
                        }
                        case Cape.Skald:
                        {
                            c = Color.Lerp(new Color(0.08f, 0.16f, 0.42f), new Color(0.14f, 0.26f, 0.6f), noise);
                            if (v < 0.08f || (v > 0.11f && v < 0.13f)) c = new Color(0.85f, 0.75f, 0.4f);
                            break;
                        }
                        default: // Huscarl
                        {
                            c = Color.Lerp(new Color(0.35f, 0.04f, 0.04f), new Color(0.55f, 0.08f, 0.06f), noise);
                            if (v < 0.07f || u < 0.035f || u > 0.965f) c = new Color(0.15f, 0.12f, 0.1f);
                            break;
                        }
                    }
                    px[y * W + x] = c;
                }
            tex.SetPixels32(px);
            tex.Apply(true, true);
            s_capeTex[cape] = tex;
            return tex;
        }

        // ------------------------------------------------------------------ projectiles

        static Mesh s_stone;
        static Material s_stoneMat;

        /// <summary>
        /// A race unit's projectile in place of the plain axe / spear: Blowgunner darts (a small spear), sling stones,
        /// the Shaman's / Skald's spirit bolt, the scouts' lighter weapons. Returns false to draw the original.
        /// </summary>
        static com.oddlabs.tt.model.weapon.ThrowingWeapon s_flying;

        public static bool DrawProjectile(com.oddlabs.tt.model.weapon.ThrowingWeapon weapon, Unit thrower, ref Matrix4x4 m, Material bm)
        {
            s_flying = weapon;
            try { return DrawProjectile(thrower, ref m, bm); }
            finally { s_flying = null; }
        }

        public static bool DrawProjectile(Unit thrower, ref Matrix4x4 m, Material bm)
        {
            int kind = UnitKind(thrower);
            bool vik = Vikings(thrower);
            switch (kind)
            {
                case Race.UNIT_UNIQUE_1:   // the Blowgunner
                case Race.UNIT_UNIQUE_3:   // the Dart Master
                    if (vik) return false;
                    DrawDart(s_flying, m, bm, kind == Race.UNIT_UNIQUE_3);
                    return true;
                case Race.UNIT_SCOUT:
                    m = m * Matrix4x4.Scale(Vector3.one * 0.72f);
                    return false;
                case Race.UNIT_SLINGER:
                {
                    if (s_stone == null)
                    {
                        var b = new WeaponBuilder();
                        b.Ball(Slot.Stone, Vector3.zero, new Vector3(0.17f, 0.14f, 0.15f), 4, 6);
                        s_stone = b.Build("Sling stone", out _);
                        s_stoneMat = SlotMat(Slot.Stone, Color.white, bm);
                    }
                    var p = new Vector3(m.m03, m.m13, m.m23);
                    Emit(s_stone, s_stoneMat, Matrix4x4.TRS(p, Quaternion.Euler(Time.time * 400f, Time.time * 250f, 0f), Vector3.one), false);
                    return true;
                }
                case Race.UNIT_SHAMAN:
                {
                    var p = new Vector3(m.m03, m.m13, m.m23);
                    DrawOrb(p, 0.32f + 0.05f * Mathf.Sin(Time.time * 30f), vik);
                    return true;
                }
            }
            return false;
        }

        // ------------------------------------------------------------------ the blowgunners' shot (presentation only)

        sealed class BlowRig
        {
            public Transform head, rUp, rLo, rHand, lUp, lLo, lHand;
            public TTHeldWeaponAligner gun;
            public float lastP = -1f;
        }
        /// <summary>Shots blown this frame (muzzle, direction, Dart Master): TTCombatFx puffs them.</summary>
        public static readonly List<(Vector3 muzzle, Vector3 dir, bool master)> BlowPuffs = new List<(Vector3, Vector3, bool)>();
        /// <summary>Frames a blowgunner was drawn at the mouth (the showcase checks it).</summary>
        public static int BlowPosed;
        static readonly Dictionary<int, BlowRig> s_blowRigs = new Dictionary<int, BlowRig>();

        /// <summary>
        /// The Blowgunner and the Dart Master shoot: the original models only have the spear throw, so through their
        /// attack the arms are bent (two-bone IK on the posed skeleton) to hold the blowgun level at the mouth, pointing
        /// where the warrior faces, both hands on it - and at the release the head gives a short puff and the gun kicks
        /// back a little. Called after the unit is posed for the frame; nothing here touches the game.
        /// </summary>
        public static void BlowMotion(GameObject go, TribalTrouble.Presentation.TTSpriteView view, Unit u, int kind, int anim, float ticks)
        {
            if (go == null || u == null || u.isDead() || Vikings(u) || (kind != Race.UNIT_UNIQUE_1 && kind != Race.UNIT_UNIQUE_3)) return;
            if (anim != com.oddlabs.tt.model.Unit.Animation.THROWING) return;
            var rig = BlowRigOf(go);
            if (rig == null) return;
            view?.RePose();   // from the clip's own pose, every frame
            var fwd = TTSpace.ToUnity(u.getDirectionX(), u.getDirectionY(), 0f);
            if (fwd.sqrMagnitude < 1e-6f) return;
            fwd.Normalize();
            float r = u.getWeaponFactory() != null ? u.getWeaponFactory().getReleaseRatio() : 0.5f;
            float p = ticks - Mathf.Floor(ticks);
            // the release this frame: a puff at the muzzle (once per shot)
            bool released = rig.lastP >= 0f && rig.lastP < r && p >= r && p - rig.lastP < 0.5f;
            rig.lastP = p;
            // up to the mouth at the start of the shot, down again after it
            float wUp = Mathf.SmoothStep(0f, 1f, p / 0.16f);
            float wDown = 1f - Mathf.SmoothStep(0f, 1f, (p - Mathf.Max(r + 0.18f, 0.82f)) / 0.16f);
            float w = Mathf.Clamp01(Mathf.Min(wUp, wDown));
            if (w <= 0.001f) return;
            if (w > 0.5f) BlowPosed++;
            float puff = Mathf.Exp(-((p - r) * (p - r)) / (2f * 0.035f * 0.035f));
            var up = Vector3.up;
            var right = Vector3.Cross(up, fwd).normalized;
            float armLen = Vector3.Distance(rig.rUp.position, rig.rLo.position) + Vector3.Distance(rig.rLo.position, rig.rHand.position);
            if (armLen < 1e-4f) return;
            // a little lean into the shot and the puff: the head nods forward as he blows
            rig.head.rotation = Quaternion.AngleAxis(9f * puff * w, right) * rig.head.rotation;
            var aim = (fwd + up * 0.06f).normalized;
            var mouth = rig.head.position + fwd * (0.16f * armLen) - up * (0.05f * armLen);
            float gunLen = rig.gun != null ? rig.gun.transform.lossyScale.y : armLen * 1.7f;
            var kick = -aim * (0.05f * armLen * puff);
            var rTarget = mouth + aim * (0.17f * gunLen) + kick - up * (0.03f * armLen);
            var lTarget = mouth + aim * (0.42f * gunLen) + kick - up * (0.04f * armLen);
            TwoBone(rig.rUp, rig.rLo, rig.rHand, Vector3.Lerp(rig.rHand.position, rTarget, w), rig.rUp.position + (right * 0.7f - up) * armLen);
            TwoBone(rig.lUp, rig.lLo, rig.lHand, Vector3.Lerp(rig.lHand.position, lTarget, w), rig.lUp.position + (-right * 0.5f - up) * armLen);
            if (rig.gun != null)
                rig.gun.Override(mouth + kick - aim * (0.02f * gunLen), Quaternion.FromToRotation(Vector3.up, aim), w);
            if (released && BlowPuffs.Count < 64) BlowPuffs.Add((mouth + aim * gunLen, aim, kind == Race.UNIT_UNIQUE_3));
        }

        static BlowRig BlowRigOf(GameObject go)
        {
            int id = go.GetHashCode();
            if (s_blowRigs.TryGetValue(id, out var have) && have != null && have.head != null && have.rHand != null) return have;
            if (s_blowRigs.Count > 2048) s_blowRigs.Clear();
            Transform[] bones = null;
            foreach (var smr in go.GetComponentsInChildren<SkinnedMeshRenderer>(false))
                if (smr.bones != null && smr.bones.Length > 0) { bones = smr.bones; break; }
            if (bones == null) { s_blowRigs[id] = null; return null; }
            Transform B(string suffix) { int i = BoneIndex(bones, suffix); return i >= 0 ? bones[i] : null; }
            var rig = new BlowRig { head = B("Head"), rHand = B("R Hand"), lHand = B("L Hand") };
            if (rig.head == null || rig.rHand == null || rig.lHand == null) { s_blowRigs[id] = null; return null; }
            rig.rLo = B("R Forearm") ?? rig.rHand.parent;
            rig.rUp = B("R UpperArm") ?? rig.rLo?.parent;
            rig.lLo = B("L Forearm") ?? rig.lHand.parent;
            rig.lUp = B("L UpperArm") ?? rig.lLo?.parent;
            if (rig.rLo == null || rig.rUp == null || rig.lLo == null || rig.lUp == null) { s_blowRigs[id] = null; return null; }
            foreach (var al in go.GetComponentsInChildren<TTHeldWeaponAligner>(true))
                if (al.gameObject.name.StartsWith("Unique weapon")) { rig.gun = al; break; }
            s_blowRigs[id] = rig;
            return rig;
        }

        /// <summary>Bends a limb (upper, lower, end) so its end reaches the target, the middle joint towards the pole.</summary>
        static void TwoBone(Transform upper, Transform lower, Transform end, Vector3 target, Vector3 pole)
        {
            Vector3 a = upper.position, b = lower.position, c = end.position;
            float la = (b - a).magnitude, lb = (c - b).magnitude;
            if (la < 1e-5f || lb < 1e-5f) return;
            var toT = target - a;
            float lt = Mathf.Clamp(toT.magnitude, Mathf.Abs(la - lb) + 1e-4f, (la + lb) * 0.999f);
            var dir = toT.sqrMagnitude > 1e-8f ? toT.normalized : (c - a).normalized;
            var side = pole - a;
            side -= Vector3.Dot(side, dir) * dir;
            if (side.sqrMagnitude < 1e-8f) side = Vector3.Cross(dir, Vector3.right);
            side.Normalize();
            float cosA = Mathf.Clamp((la * la + lt * lt - lb * lb) / (2f * la * lt), -1f, 1f);
            var elbow = a + dir * (la * cosA) + side * (la * Mathf.Sqrt(1f - cosA * cosA));
            upper.rotation = Quaternion.FromToRotation(b - a, elbow - a) * upper.rotation;
            b = lower.position; c = end.position;
            var reach = a + dir * lt;
            lower.rotation = Quaternion.FromToRotation(c - b, reach - b) * lower.rotation;
        }

        static Mesh s_dartShaft, s_dartTip, s_dartFletch;
        static Material s_dartShaftMat, s_dartTipMat, s_dartFletchMat, s_dartCrystalMat;

        /// <summary>A blown dart (not the warrior's spear): a thin reed shaft, a black thorn tip, a tuft of red down at the
        /// back - the Dart Master's tipped with crystal. It flies point first along its flight.</summary>
        static void DrawDart(com.oddlabs.tt.model.weapon.ThrowingWeapon w, Matrix4x4 m, Material bm, bool master)
        {
            if (s_dartShaft == null)
            {
                var b = new WeaponBuilder();
                b.Cyl(Slot.Wood, 0f, 0.62f, 0.022f, 0.018f, 5);
                s_dartShaft = b.Build("Dart shaft", out _);
                b = new WeaponBuilder();
                b.Cone(Slot.Obsidian, new Vector3(0f, 0.6f, 0f), new Vector3(0f, 0.8f, 0f), 0.03f, 5);
                s_dartTip = b.Build("Dart tip", out _);
                b = new WeaponBuilder();
                for (int i = 0; i < 3; i++)
                {
                    float a = i * Mathf.PI * 2f / 3f;
                    var d = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
                    b.Feather(Slot.FeatherRed, new Vector3(0f, 0.02f, 0f), d * 0.35f + Vector3.up * 0.2f, 0.16f, 0.05f, new Vector3(-d.z, 0f, d.x));
                }
                s_dartFletch = b.Build("Dart fletch", out _);
                s_dartShaftMat = SlotMat(Slot.Wood, Color.white, bm);
                s_dartTipMat = SlotMat(Slot.Obsidian, Color.white, bm);
                s_dartFletchMat = SlotMat(Slot.FeatherRed, Color.white, bm);
                s_dartCrystalMat = SlotMat(Slot.Crystal, Color.white, bm);
            }
            var p = new Vector3(m.m03, m.m13, m.m23);
            // the flight: the weapon's heading at its speed, and its climb or fall now
            Vector3 dir = Vector3.forward;
            if (w != null)
            {
                float v = w.getMetersPerSecond();
                dir = TTSpace.ToUnity(w.getDirectionX() * v, w.getDirectionY() * v, w.getZSpeed());
            }
            if (dir.sqrMagnitude < 1e-6f) dir = Vector3.forward;
            dir.Normalize();
            var rot = Quaternion.FromToRotation(Vector3.up, dir);
            var mm = Matrix4x4.TRS(p, rot, Vector3.one * (master ? 1.15f : 1f));
            Emit(s_dartShaft, s_dartShaftMat, mm, false);
            Emit(s_dartTip, master ? s_dartCrystalMat : s_dartTipMat, mm, false);
            Emit(s_dartFletch, s_dartFletchMat, mm, false);
        }

        static Material s_spiritV, s_spiritN;

        static Material SpiritMat(bool vik)
        {
            var m = vik ? s_spiritV : s_spiritN;
            if (m != null) return m;
            var sh = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            m = new Material(sh) { name = vik ? "Skald bolt" : "Shaman bolt" };
            m.SetColor("_BaseColor", vik ? new Color(3f, 1.5f, 0.5f, 1f) : new Color(0.9f, 2.8f, 1.8f, 1f));
            if (vik) s_spiritV = m; else s_spiritN = m;
            return m;
        }
    }
}
