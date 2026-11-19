using System.Collections.Generic;
using com.oddlabs.tt.model;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Looks for the Extended-rules content (WorldParameters.extended). The simulation only knows sizes and rules; the
    /// Java client shows stand-in sprites. Here they get their own models, built from TT-style shapes with the HD
    /// textures from your other projects (Resources/TTExtended, made from the MeleeWeaponsPack atlas and TTReal rock):
    /// <list type="bullet">
    /// <item>Palisade: a ring of sharpened logs with a team-coloured lashing band, rising as it is built.</item>
    /// <item>Shrine: standing stones around a carved totem with a glowing orb; a ring of light sweeps out with each
    /// healing pulse (every 2 s, the sim's ShrineAura period).</item>
    /// <item>Champion: the warrior model 1.2x with a round shield on his back (wood + iron for Vikings, painted hide
    /// for Natives, team-coloured). Scout: the warrior model 0.88x.</item>
    /// </list>
    /// </summary>
    public static partial class TTExtendedArt
    {
        public const float ChampionScale = 1.2f, ScoutScale = 0.88f, ShamanScale = 1.05f, SlingerScale = 0.92f;
        public const float MeleeLightScale = 1.05f, MeleeHeavyScale = 1.15f;

        public const float Unique1Scale = 0.95f, Unique2Scale = 1.12f;
        public const float CrystalScale = 1.05f, DiamondScale = 1.22f, Unique3Scale = 1.06f;   // the arsenal

        public static float KindScale(int kind) => kind == Race.UNIT_CHAMPION ? ChampionScale : kind == Race.UNIT_SCOUT ? ScoutScale
                                                  : kind == Race.UNIT_SHAMAN ? ShamanScale : kind == Race.UNIT_SLINGER ? SlingerScale
                                                  : kind == Race.UNIT_MELEE_LIGHT ? MeleeLightScale : kind == Race.UNIT_MELEE_HEAVY ? MeleeHeavyScale
                                                  : kind == Race.UNIT_UNIQUE_1 ? Unique1Scale : kind == Race.UNIT_UNIQUE_2 ? Unique2Scale
                                                  : kind == Race.UNIT_CRYSTAL ? CrystalScale : kind == Race.UNIT_DIAMOND ? DiamondScale
                                                  : kind == Race.UNIT_UNIQUE_3 ? Unique3Scale
                                                  : kind == Race.UNIT_MINOR_GOD ? MinorGodScale : kind == Race.UNIT_MAJOR_GOD ? MajorGodScale
                                                  : kind == Race.UNIT_ZOMBIE ? 0.95f : kind == Race.UNIT_ZOMBIE_BRUTE ? 1.12f
                                                  : kind == Race.UNIT_ZOMBIE_BOSS ? 1.4f   // the Risen Chieftain: the chief's body, grown
                                                  : kind == Race.UNIT_ELITE_1 ? 1.16f : kind == Race.UNIT_ELITE_2 ? 1.02f : kind == Race.UNIT_ELITE_3 ? 1.04f   // the elites
                                                  : kind == Race.UNIT_SPINNER ? 1.12f : 1f;

        public const float MinorGodScale = 1.5f, MajorGodScale = 2.15f;   // the gods: bigger than any warrior / the chieftain (they start a little bigger now)

        /// <summary>A unit's drawn size: its kind's, and a god grows with every kill (GodPowers.growth).</summary>
        public static float UnitScale(Unit u, int kind) => KindScale(kind) * (kind == Race.UNIT_MINOR_GOD || kind == Race.UNIT_MAJOR_GOD
            ? com.oddlabs.tt.model.GodPowers.growth(u)
            : kind >= Race.UNIT_ELITE_1 && kind <= Race.UNIT_ELITE_3 ? 1f + 0.035f * com.oddlabs.tt.model.ElitePowers.rank(u) : 1f);   // an elite stands taller with each rank

        /// <summary>The Shaman's / Skald's magic: a glowing spirit orb floating over the shoulder.</summary>
        /// <summary>(unit object, attachment) pairs already made, so each frame needs no lookup by name.</summary>
        static readonly HashSet<(int, string)> s_attached = new HashSet<(int, string)>();

        static bool FirstAttach(GameObject unitRoot, string what)
        {
            var k = (unitRoot.GetHashCode(), what);
            if (s_attached.Contains(k)) return false;
            if (s_attached.Count > 20000) s_attached.Clear(); // dead units' entries; the name check below still guards
            s_attached.Add(k);
            return true;
        }

        public static void AttachCasterOrb(GameObject unitRoot, Unit u)
        {
            if (!FirstAttach(unitRoot, "Caster orb") || unitRoot.transform.Find("Caster orb") != null) return;
            bool vik = Vikings(u);
            var go = new GameObject("Caster orb");
            go.transform.SetParent(unitRoot.transform, false);
            go.transform.localPosition = new Vector3(0.1f, 2.15f, 0.35f);
            go.transform.localScale = Vector3.one * 0.32f;
            AddOrbParts(go, vik, 0.5f);
            go.AddComponent<TTFloatBob>();
        }

        /// <summary>A glowing spirit orb on go: the core (a hot centre, a coloured rim, swirling wisps), a soft halo
        /// round it facing the camera, and (motes &gt; 0) a few sparks circling it.</summary>
        static void AddOrbParts(GameObject go, bool vik, float motes)
        {
            go.AddComponent<MeshFilter>().sharedMesh = Orb();
            var r = go.AddComponent<MeshRenderer>();
            r.sharedMaterial = OrbMat(vik, false);
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;
            var halo = new GameObject("Halo");
            halo.transform.SetParent(go.transform, false);
            halo.AddComponent<MeshFilter>().sharedMesh = HaloQuad();
            var hr = halo.AddComponent<MeshRenderer>();
            hr.sharedMaterial = OrbMat(vik, true);
            hr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            hr.receiveShadows = false;
            if (motes <= 0f) return;
            var ring = new GameObject("Motes");
            ring.transform.SetParent(go.transform, false);
            ring.AddComponent<TTOrbit>().speed = vik ? -140f : 120f;
            for (int i = 0; i < 3; i++)
            {
                var m = new GameObject("Mote");
                m.transform.SetParent(ring.transform, false);
                float a = i * Mathf.PI * 2f / 3f;
                m.transform.localPosition = new Vector3(Mathf.Cos(a) * 1.6f, (i - 1) * 0.35f, Mathf.Sin(a) * 1.6f);
                m.transform.localScale = Vector3.one * 0.22f;
                m.AddComponent<MeshFilter>().sharedMesh = Orb();
                var mr = m.AddComponent<MeshRenderer>();
                mr.sharedMaterial = OrbMat(vik, false);
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                mr.receiveShadows = false;
            }
        }

        static readonly Material[] s_orbMats = new Material[4];
        static Mesh s_haloQuad;

        /// <summary>The orbs' material (TribalTrouble/SpiritOrb): green-white for the natives, ember for the vikings.</summary>
        static Material OrbMat(bool vik, bool halo)
        {
            int k = (vik ? 1 : 0) + (halo ? 2 : 0);
            if (s_orbMats[k] != null) return s_orbMats[k];
            var sh = Shader.Find("TribalTrouble/SpiritOrb");
            Material m;
            if (sh != null)
            {
                m = new Material(sh) { name = "Spirit orb" + (halo ? " halo" : ""), enableInstancing = true };
                m.SetColor("_BaseColor", vik ? new Color(1f, 0.5f, 0.16f, 1f) : new Color(0.4f, 1f, 0.72f, 1f));
                m.SetFloat("_Halo", halo ? 1f : 0f);
                m.SetFloat("_Intensity", halo ? 1.1f : 1.6f);
            }
            else
            {
                m = new Material(Glow());   // (no orb shader: the plain glow)
                m.SetColor("_BaseColor", halo ? Color.clear : (vik ? new Color(1f, 0.55f, 0.2f, 1f) : new Color(0.45f, 1f, 0.75f, 1f)));
            }
            return s_orbMats[k] = m;
        }

        static Mesh HaloQuad()
        {
            if (s_haloQuad != null) return s_haloQuad;
            s_haloQuad = new Mesh
            {
                name = "Orb halo",
                vertices = new[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(1, 1, 0), new Vector3(-1, 1, 0) },
                uv = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) },
                normals = new[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back },
                triangles = new[] { 0, 2, 1, 0, 3, 2 },
            };
            s_haloQuad.bounds = new Bounds(Vector3.zero, Vector3.one * 8f);   // (billboarded in the shader)
            return s_haloQuad;
        }

        /// <summary>A spirit bolt in flight: the orb and its halo, drawn this frame only.</summary>
        public static void DrawOrb(Vector3 p, float size, bool vik)
        {
            var mm = Matrix4x4.TRS(p, Quaternion.identity, Vector3.one * size);
            var rp = new RenderParams(OrbMat(vik, false)) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off, receiveShadows = false };
            Graphics.RenderMesh(rp, Orb(), 0, mm);
            var hp = new RenderParams(OrbMat(vik, true)) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off, receiveShadows = false };
            Graphics.RenderMesh(hp, HaloQuad(), 0, mm);
        }

        static Mesh s_palisadeSpan, s_palisadeSpanDiag, s_palisadePost;
        static Mesh s_palisade, s_stones, s_totem, s_orb, s_ring, s_shield;
        static readonly Dictionary<(string, int), Material> s_mats = new Dictionary<(string, int), Material>();
        static Material s_glow, s_pulse;
        static readonly MaterialPropertyBlock s_block = new MaterialPropertyBlock();
        static readonly MaterialPropertyBlock s_ringBlock = new MaterialPropertyBlock();

        public static bool IsExtendedBuilding(Building b) => ((BuildingTemplate)b.getTemplate()).getTemplateID() >= Race.BUILDING_PALISADE;

        public static int UnitKind(Unit u)
        {
            var race = u.getOwner().getRace();
            var t = u.getTemplate();
            if (t == race.getUnitTemplate(Race.UNIT_CHAMPION)) return Race.UNIT_CHAMPION;
            if (t == race.getUnitTemplate(Race.UNIT_SCOUT)) return Race.UNIT_SCOUT;
            if (t == race.getUnitTemplate(Race.UNIT_SHAMAN)) return Race.UNIT_SHAMAN;
            if (t == race.getUnitTemplate(Race.UNIT_SLINGER)) return Race.UNIT_SLINGER;
            if (t == race.getUnitTemplate(Race.UNIT_MELEE_LIGHT)) return Race.UNIT_MELEE_LIGHT;
            if (t == race.getUnitTemplate(Race.UNIT_MELEE_HEAVY)) return Race.UNIT_MELEE_HEAVY;
            if (t == race.getUnitTemplate(Race.UNIT_UNIQUE_1)) return Race.UNIT_UNIQUE_1;
            if (t == race.getUnitTemplate(Race.UNIT_UNIQUE_2)) return Race.UNIT_UNIQUE_2;
            if (t == race.getUnitTemplate(Race.UNIT_CRYSTAL)) return Race.UNIT_CRYSTAL;   // the arsenal
            if (t == race.getUnitTemplate(Race.UNIT_DIAMOND)) return Race.UNIT_DIAMOND;
            if (t == race.getUnitTemplate(Race.UNIT_UNIQUE_3)) return Race.UNIT_UNIQUE_3;
            if (t == race.getUnitTemplate(Race.UNIT_MINOR_GOD)) return Race.UNIT_MINOR_GOD;   // the gods
            if (t == race.getUnitTemplate(Race.UNIT_ELITE_1)) return Race.UNIT_ELITE_1;   // the elites
            if (t == race.getUnitTemplate(Race.UNIT_ELITE_2)) return Race.UNIT_ELITE_2;
            if (t == race.getUnitTemplate(Race.UNIT_ELITE_3)) return Race.UNIT_ELITE_3;
            if (t == race.getUnitTemplate(Race.UNIT_MAJOR_GOD)) return Race.UNIT_MAJOR_GOD;
            if (t == race.getUnitTemplate(Race.UNIT_SPINNER)) return Race.UNIT_SPINNER;   // the chicken whirlers
            // the Zombies mode's dead: their template is the race of the one who fell, their owner the player of the dead
            var rr = u.getOwner().getWorld().getRacesResources();
            for (int r = 0; r < 2; r++)
            {
                var rc = rr.getRace(r);
                if (t == rc.getUnitTemplate(Race.UNIT_ZOMBIE)) return Race.UNIT_ZOMBIE;
                if (t == rc.getUnitTemplate(Race.UNIT_ZOMBIE_BRUTE)) return Race.UNIT_ZOMBIE_BRUTE;
                if (t == rc.getUnitTemplate(Race.UNIT_ZOMBIE_BOSS)) return Race.UNIT_ZOMBIE_BOSS;   // the Risen Chieftain
            }
            return -1;
        }

        public static bool Vikings(Selectable s) => s.getOwner().getRace() == s.getOwner().getWorld().getRacesResources().getRace(RacesResources.RACE_VIKINGS);

        // ------------------------------------------------------------------ materials

        static Material Mat(string tex, string mask, Color team, Material baseMaterial, float gloss = 0.4f)
        {
            Color32 c32 = team;
            var key = (tex, (c32.r << 16) | (c32.g << 8) | c32.b);
            if (s_mats.TryGetValue(key, out var m)) return m;
            m = baseMaterial != null ? new Material(baseMaterial) : new Material(Shader.Find("TribalTrouble/Sprite"));
            m.name = "Extended " + tex;
            m.enableInstancing = true;
            var t = Resources.Load<Texture2D>("TTExtended/" + tex);
            if (t != null && t.anisoLevel < 8) t.anisoLevel = 8;
            m.SetTexture("_BaseMap", t != null ? t : Texture2D.grayTexture);
            var mk = mask != null ? Resources.Load<Texture2D>("TTExtended/" + mask) : null;
            m.SetTexture("_TeamMask", mk != null ? mk : Texture2D.blackTexture);
            m.SetFloat("_UseTeamMask", mk != null ? 1f : 0f);
            m.SetColor("_TeamColor", team);
            m.SetColor("_Tint", Color.white);
            m.SetFloat("_Wind", 0f);
            m.SetFloat("_Relief", 0.8f);
            m.SetFloat("_Gloss", gloss);
            m.SetFloat("_Cull", 2f);
            s_mats[key] = m;
            return m;
        }

        static Material Glow()
        {
            if (s_glow != null) return s_glow;
            var sh = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            s_glow = new Material(sh) { name = "Shrine orb", enableInstancing = true };
            return s_glow;
        }

        static Material Pulse()
        {
            if (s_pulse != null) return s_pulse;
            var sh = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            s_pulse = new Material(sh) { name = "Shrine pulse" };
            if (s_pulse.HasProperty("_Surface")) { s_pulse.SetFloat("_Surface", 1f); s_pulse.SetFloat("_Blend", 2f); }   // additive
            s_pulse.SetOverrideTag("RenderType", "Transparent");
            s_pulse.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            s_pulse.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
            s_pulse.SetInt("_ZWrite", 0);
            s_pulse.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            s_pulse.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            return s_pulse;
        }

        // ------------------------------------------------------------------ meshes

        sealed class MeshBuilder
        {
            public readonly List<Vector3> v = new List<Vector3>();
            public readonly List<Vector3> n = new List<Vector3>();
            public readonly List<Vector2> uv = new List<Vector2>();
            public readonly List<int> t = new List<int>();

            public void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector2 ua, Vector2 ub, Vector2 uc, Vector2 ud)
            {
                var nn = Vector3.Cross(b - a, d - a).normalized;
                int i = v.Count;
                v.Add(a); v.Add(b); v.Add(c); v.Add(d);
                n.Add(nn); n.Add(nn); n.Add(nn); n.Add(nn);
                uv.Add(ua); uv.Add(ub); uv.Add(uc); uv.Add(ud);
                t.Add(i); t.Add(i + 3); t.Add(i + 1); t.Add(i + 1); t.Add(i + 3); t.Add(i + 2);
            }

            public void Tri(Vector3 a, Vector3 b, Vector3 c, Vector2 ua, Vector2 ub, Vector2 uc)
            {
                var nn = Vector3.Cross(b - a, c - a).normalized;
                int i = v.Count;
                v.Add(a); v.Add(b); v.Add(c);
                n.Add(nn); n.Add(nn); n.Add(nn);
                uv.Add(ua); uv.Add(ub); uv.Add(uc);
                t.Add(i); t.Add(i + 2); t.Add(i + 1);
            }

            /// <summary>A sharpened log: radius r, shaft height h, point height tip, uOffset into the bark texture.</summary>
            public void Log(Vector3 basePos, float r, float h, float tip, float uOffset, Vector3 lean)
            {
                const int sides = 8;
                for (int i = 0; i < sides; i++)
                {
                    float a0 = i * Mathf.PI * 2f / sides, a1 = (i + 1) * Mathf.PI * 2f / sides;
                    var d0 = new Vector3(Mathf.Cos(a0), 0, Mathf.Sin(a0)) * r;
                    var d1 = new Vector3(Mathf.Cos(a1), 0, Mathf.Sin(a1)) * r;
                    var top = lean * h + Vector3.up * h;
                    float u0 = uOffset + i / (float)sides * 0.3f, u1 = uOffset + (i + 1) / (float)sides * 0.3f;
                    float vt = h / 3f;
                    // shaft (outward facing: counter-clockwise seen from outside)
                    Quad(basePos + d1, basePos + d0, basePos + top + d0, basePos + top + d1,
                         new Vector2(u1, 0), new Vector2(u0, 0), new Vector2(u0, vt), new Vector2(u1, vt));
                    // point
                    var apex = basePos + top * (1f + tip / h);
                    Tri(basePos + top + d1, basePos + top + d0, apex, new Vector2(u1, vt), new Vector2(u0, vt), new Vector2((u0 + u1) * .5f, vt + tip / 3f));
                }
            }

            public Mesh Build(string name)
            {
                var m = new Mesh { name = name };
                m.indexFormat = v.Count > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
                m.SetVertices(v); m.SetNormals(n); m.SetUVs(0, uv); m.SetTriangles(t, 0);
                m.RecalculateBounds();
                return m;
            }
        }

        /// <summary>
        /// A palisade is one cell (2 m): a post of three logs, and a row of logs from it half-way to each palisade cell
        /// next to it (straight or diagonal), so neighbouring posts make one closed wall.
        /// </summary>
        internal static Mesh PalisadePost()
        {
            if (s_palisadePost != null) return s_palisadePost;
            var mb = new MeshBuilder();
            var rnd = new System.Random(77);
            for (int i = 0; i < 3; i++)
            {
                float a = i * Mathf.PI * 2f / 3f + 0.4f;
                mb.Log(new Vector3(Mathf.Cos(a) * 0.2f, -0.3f, Mathf.Sin(a) * 0.2f), 0.22f, 2.9f + (float)rnd.NextDouble() * 0.3f, 0.5f, (float)rnd.NextDouble(), Vector3.zero);
            }
            return s_palisadePost = mb.Build("PalisadePost");
        }

        internal static Mesh PalisadeSpan(bool diagonal)
        {
            if (!diagonal && s_palisadeSpan != null) return s_palisadeSpan;
            if (diagonal && s_palisadeSpanDiag != null) return s_palisadeSpanDiag;
            var mb = new MeshBuilder();
            var rnd = new System.Random(diagonal ? 913 : 911);
            float len = diagonal ? 1.4142f : 1f;   // half the way to the next cell's centre (2 m cells)
            const float step = 0.34f;
            for (float x = 0.3f; x <= len * 1.02f + 0.05f; x += step)
            {
                float r = 0.17f + (float)rnd.NextDouble() * 0.05f;
                float h = 2.35f + (float)rnd.NextDouble() * 0.45f;
                var lean = new Vector3(0, 0, ((float)rnd.NextDouble() - .5f) * 0.05f);
                mb.Log(new Vector3(x, -0.3f, ((float)rnd.NextDouble() - .5f) * 0.06f), r, h, 0.45f + (float)rnd.NextDouble() * 0.15f, (float)rnd.NextDouble(), lean);
            }
            var m = mb.Build(diagonal ? "PalisadeSpanDiag" : "PalisadeSpan");
            if (diagonal) s_palisadeSpanDiag = m; else s_palisadeSpan = m;
            return m;
        }

        /// <summary>The cells round a palisade that hold a palisade of the same owner (8 directions, bit i = dir i).</summary>
        static readonly int[,] s_dirs = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 } };

        static LandBuilding PalisadeAt(LandBuilding b, int gx, int gy)
        {
            var grid = b.getOwner().getWorld().getUnitGrid();
            if (gx < 0 || gy < 0 || gx >= grid.getGridSize() || gy >= grid.getGridSize()) return null;
            var occ = grid.getOccupant(gx, gy);
            return occ is LandBuilding o && o != b && !o.isDead() && o.getOwner() == b.getOwner()
                && ((BuildingTemplate)o.getTemplate()).getTemplateID() == Race.BUILDING_PALISADE ? o : null;
        }

        static readonly bool[] s_palN = new bool[8];
        static readonly LandBuilding[] s_palNb = new LandBuilding[8];

        static void DrawPalisade(LandBuilding b, Matrix4x4 m, float rise, Material mat)
        {
            var pos = new Vector3(m.m03, m.m13, m.m23);
            var up = Matrix4x4.Scale(new Vector3(1f, rise, 1f));
            Emit(PalisadePost(), mat, Matrix4x4.Translate(pos) * up, true);
            int gx = b.getGridX(), gy = b.getGridY();
            bool[] n = s_palN;
            var nb = s_palNb;
            for (int i = 0; i < 8; i++) { nb[i] = PalisadeAt(b, gx + s_dirs[i, 0], gy + s_dirs[i, 1]); n[i] = nb[i] != null; }
            for (int i = 0; i < 8; i++)
            {
                if (!n[i]) continue;
                int dx = s_dirs[i, 0], dy = s_dirs[i, 1];
                bool diag = dx != 0 && dy != 0;
                // a diagonal joins only where the corner isn't already closed by the two straight neighbours
                if (diag && (PalisadeAt(b, gx + dx, gy) != null || PalisadeAt(b, gx, gy + dy) != null)) continue;
                var other = TTSpace.ToUnity(nb[i].getPositionX(), nb[i].getPositionY(), nb[i].getPositionZ());
                var dir = other - pos;
                var flat = new Vector3(dir.x, 0, dir.z);
                if (flat.sqrMagnitude < 1e-4f) continue;
                var rot = Quaternion.FromToRotation(Vector3.right, flat.normalized);
                // the slope between the two posts: the row of logs follows the ground (logs stay upright)
                float slope = dir.y / flat.magnitude;
                var shear = Matrix4x4.identity; shear.m10 = slope;
                Emit(PalisadeSpan(diag), mat, Matrix4x4.Translate(pos) * Matrix4x4.Rotate(rot) * shear * up, true);
            }
        }

        internal static Mesh Palisade()
        {
            if (s_palisade != null) return s_palisade;
            var mb = new MeshBuilder();
            var rnd = new System.Random(4242);
            const int logs = 15;
            const float R = 2.15f;
            for (int i = 0; i < logs; i++)
            {
                float a = i * Mathf.PI * 2f / logs;
                var p = new Vector3(Mathf.Cos(a) * R, -0.3f, Mathf.Sin(a) * R);
                float r = 0.19f + (float)rnd.NextDouble() * 0.07f;
                float h = 2.5f + (float)rnd.NextDouble() * 0.6f;
                var lean = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * 0.04f + new Vector3((float)rnd.NextDouble() - .5f, 0, (float)rnd.NextDouble() - .5f) * 0.04f;
                mb.Log(p, r, h, 0.45f + (float)rnd.NextDouble() * 0.15f, (float)rnd.NextDouble(), lean);
            }
            // a few inner logs so it reads as solid
            for (int i = 0; i < 6; i++)
            {
                float a = (i + .5f) * Mathf.PI * 2f / 6;
                var p = new Vector3(Mathf.Cos(a) * 1.2f, -0.3f, Mathf.Sin(a) * 1.2f);
                mb.Log(p, 0.22f, 2.2f + (float)rnd.NextDouble() * 0.4f, 0.4f, (float)rnd.NextDouble(), Vector3.zero);
            }
            return s_palisade = mb.Build("Palisade");
        }

        internal static Mesh Stones()
        {
            if (s_stones != null) return s_stones;
            var mb = new MeshBuilder();
            var rnd = new System.Random(77);
            const int count = 8;
            for (int i = 0; i < count; i++)
            {
                float a = i * Mathf.PI * 2f / count;
                var c = new Vector3(Mathf.Cos(a) * 3.1f, -0.2f, Mathf.Sin(a) * 3.1f);
                var outward = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
                var side = Vector3.Cross(Vector3.up, outward);
                float w = 0.35f + (float)rnd.NextDouble() * 0.1f, d = 0.24f, h = 1.9f + (float)rnd.NextDouble() * 0.8f;
                float taper = 0.7f;
                Vector3 B(float sx, float sz) => c + side * (sx * w) + outward * (sz * d);
                Vector3 T(float sx, float sz) => c + Vector3.up * h + side * (sx * w * taper) + outward * (sz * d * taper);
                float u = (float)rnd.NextDouble();
                float vh = h / 2.5f;
                mb.Quad(B(-1, 1), B(1, 1), T(1, 1), T(-1, 1), new Vector2(u, 0), new Vector2(u + .3f, 0), new Vector2(u + .3f, vh), new Vector2(u, vh));
                mb.Quad(B(1, -1), B(-1, -1), T(-1, -1), T(1, -1), new Vector2(u, 0), new Vector2(u + .3f, 0), new Vector2(u + .3f, vh), new Vector2(u, vh));
                mb.Quad(B(1, 1), B(1, -1), T(1, -1), T(1, 1), new Vector2(u, 0), new Vector2(u + .1f, 0), new Vector2(u + .1f, vh), new Vector2(u, vh));
                mb.Quad(B(-1, -1), B(-1, 1), T(-1, 1), T(-1, -1), new Vector2(u, 0), new Vector2(u + .1f, 0), new Vector2(u + .1f, vh), new Vector2(u, vh));
                mb.Quad(T(-1, 1), T(1, 1), T(1, -1), T(-1, -1), new Vector2(u, .9f), new Vector2(u + .2f, .9f), new Vector2(u + .2f, 1f), new Vector2(u, 1f));
            }
            // low plinth in the middle
            {
                float s = 1.0f, h = 0.45f;
                var o = new Vector3(0, -0.2f, 0);
                Vector3 P(float x, float y, float z) => o + new Vector3(x * s, y * h, z * s);
                mb.Quad(P(-1, 0, 1), P(1, 0, 1), P(1, 1, 1), P(-1, 1, 1), new Vector2(0, 0), new Vector2(.8f, 0), new Vector2(.8f, .2f), new Vector2(0, .2f));
                mb.Quad(P(1, 0, -1), P(-1, 0, -1), P(-1, 1, -1), P(1, 1, -1), new Vector2(0, 0), new Vector2(.8f, 0), new Vector2(.8f, .2f), new Vector2(0, .2f));
                mb.Quad(P(1, 0, 1), P(1, 0, -1), P(1, 1, -1), P(1, 1, 1), new Vector2(0, 0), new Vector2(.8f, 0), new Vector2(.8f, .2f), new Vector2(0, .2f));
                mb.Quad(P(-1, 0, -1), P(-1, 0, 1), P(-1, 1, 1), P(-1, 1, -1), new Vector2(0, 0), new Vector2(.8f, 0), new Vector2(.8f, .2f), new Vector2(0, .2f));
                mb.Quad(P(-1, 1, 1), P(1, 1, 1), P(1, 1, -1), P(-1, 1, -1), new Vector2(0, .3f), new Vector2(.8f, .3f), new Vector2(.8f, 1.1f), new Vector2(0, 1.1f));
            }
            return s_stones = mb.Build("Shrine stones");
        }

        internal static Mesh Totem()
        {
            if (s_totem != null) return s_totem;
            var mb = new MeshBuilder();
            mb.Log(new Vector3(0, 0.2f, 0), 0.36f, 3.4f, 0.3f, 0.1f, Vector3.zero);
            return s_totem = mb.Build("Shrine totem");
        }

        static Mesh Orb()
        {
            if (s_orb != null) return s_orb;
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s_orb = go.GetComponent<MeshFilter>().sharedMesh;
            Object.Destroy(go);
            return s_orb;
        }

        static Mesh Ring()
        {
            if (s_ring != null) return s_ring;
            const int seg = 64;
            var v = new Vector3[seg * 2];
            var t = new int[seg * 6];
            for (int i = 0; i < seg; i++)
            {
                float a = i * Mathf.PI * 2f / seg;
                var d = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
                v[i * 2] = d * 0.88f; v[i * 2 + 1] = d;
                int nx = (i + 1) % seg, k = i * 6;
                t[k] = i * 2; t[k + 1] = nx * 2; t[k + 2] = i * 2 + 1;
                t[k + 3] = i * 2 + 1; t[k + 4] = nx * 2; t[k + 5] = nx * 2 + 1;
            }
            s_ring = new Mesh { name = "Shrine pulse ring", vertices = v, triangles = t };
            s_ring.RecalculateBounds();
            return s_ring;
        }

        public static Mesh Shield()
        {
            if (s_shield != null) return s_shield;
            const int seg = 32, rings = 6;
            const float R = 0.42f, dome = 0.06f, thick = 0.03f;
            var v = new List<Vector3>(); var n = new List<Vector3>(); var uv = new List<Vector2>(); var t = new List<int>();
            // front: a shallow dome facing +z
            v.Add(new Vector3(0, 0, dome + thick)); n.Add(Vector3.forward); uv.Add(new Vector2(.5f, .5f));
            for (int r = 1; r <= rings; r++)
            {
                float rr = R * r / rings, z = dome * (1 - (rr / R) * (rr / R)) + thick;
                for (int i = 0; i < seg; i++)
                {
                    float a = i * Mathf.PI * 2f / seg;
                    var p = new Vector3(Mathf.Cos(a) * rr, Mathf.Sin(a) * rr, z);
                    v.Add(p); n.Add(new Vector3(p.x * 0.3f, p.y * 0.3f, 1f).normalized);
                    uv.Add(new Vector2(.5f + Mathf.Cos(a) * .5f * r / rings, .5f + Mathf.Sin(a) * .5f * r / rings));
                }
            }
            for (int i = 0; i < seg; i++) { t.Add(0); t.Add(1 + (i + 1) % seg); t.Add(1 + i); }
            for (int r = 1; r < rings; r++)
                for (int i = 0; i < seg; i++)
                {
                    int a = 1 + (r - 1) * seg + i, b = 1 + (r - 1) * seg + (i + 1) % seg, c = a + seg, d = b + seg;
                    t.Add(a); t.Add(d); t.Add(c); t.Add(a); t.Add(b); t.Add(d);
                }
            // back: flat disc facing -z (uses the rim colour)
            int back = v.Count;
            v.Add(Vector3.zero); n.Add(Vector3.back); uv.Add(new Vector2(.5f, .98f));
            for (int i = 0; i < seg; i++)
            {
                float a = i * Mathf.PI * 2f / seg;
                v.Add(new Vector3(Mathf.Cos(a) * R, Mathf.Sin(a) * R, 0)); n.Add(Vector3.back); uv.Add(new Vector2(.5f, .98f));
            }
            for (int i = 0; i < seg; i++) { t.Add(back); t.Add(back + 1 + i); t.Add(back + 1 + (i + 1) % seg); }
            // the rim's edge between the face and the back (it was open: the shield looked paper-thin from the side)
            int side = v.Count;
            for (int i = 0; i <= seg; i++)
            {
                float a = i * Mathf.PI * 2f / seg;
                var d = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f);
                v.Add(d * R); n.Add(d); uv.Add(new Vector2(.5f + .49f * d.x, .5f + .49f * d.y));
                v.Add(d * R + new Vector3(0, 0, thick)); n.Add(d); uv.Add(new Vector2(.5f + .49f * d.x, .5f + .49f * d.y));
            }
            for (int i = 0; i < seg; i++)
            {
                int a0 = side + i * 2, a1 = a0 + 2;
                t.Add(a0); t.Add(a0 + 1); t.Add(a1); t.Add(a1); t.Add(a0 + 1); t.Add(a1 + 1);
                t.Add(a0); t.Add(a1); t.Add(a0 + 1); t.Add(a1); t.Add(a1 + 1); t.Add(a0 + 1);   // (both windings: always seen)
            }
            var m = new Mesh { name = "Champion shield" };
            m.SetVertices(v); m.SetNormals(n); m.SetUVs(0, uv); m.SetTriangles(t, 0);
            m.RecalculateBounds();
            return s_shield = m;
        }

        // ------------------------------------------------------------------ drawing

        /// <summary>Draws an Extended building (m: the building's TT transform in Unity space), in its tier skin once
        /// it's finished.</summary>
        public static void DrawBuilding(LandBuilding b, Matrix4x4 m, Color team, Material baseMaterial)
        {
            s_skin = b.isComplete() ? SkinOf(b) : Vector4.zero;
            s_skinVik = Vikings(b);
            try { DrawBuildingModel(b, m, team, baseMaterial); }
            finally { s_skin = Vector4.zero; }
        }

        static void DrawBuildingModel(LandBuilding b, Matrix4x4 m, Color team, Material baseMaterial)
        {
            int id = ((BuildingTemplate)b.getTemplate()).getTemplateID();
            float progress = b.getEffectiveMaxHitPoints() > 0 ? Mathf.Clamp01(b.getBuildPoints() / (float)b.getEffectiveMaxHitPoints()) : 1f;
            float rise = Mathf.Lerp(0.12f, 1f, progress);
            var grow = m * Matrix4x4.Scale(new Vector3(1f, rise, 1f));
            var rp = new RenderParams(null) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows = true };
            // the kit models are bigger than the old ones: their glows, sparks and footings scale with them (ma), the
            // models themselves are drawn at their own size (grow)
            float ks = KitScale(b);
            var ma = m * Matrix4x4.Scale(Vector3.one * ks);
            if (id == Race.BUILDING_BRIDGE) { DrawBridge(b, m, progress, team, baseMaterial); return; }   // a span of a bridge
            if (id == Race.BUILDING_FORGE) { DrawForge(b, ma, grow, team, baseMaterial); DrawTierExtras(b, m, team, baseMaterial, 3.2f * ks); return; }   // the arsenal
            if (id == Race.BUILDING_TOTEM) { DrawWarTotem(b, ma, grow, team, baseMaterial); return; }
            if (id == Race.BUILDING_DOCK) { DrawDock(b, ma, grow, team, baseMaterial); DrawTierExtras(b, m, team, baseMaterial, 3.2f * ks); return; }   // the dock
            if (id == Race.BUILDING_MARKET) { DrawMarket(b, ma, grow, team, baseMaterial); DrawTierExtras(b, m, team, baseMaterial, 3.2f * ks); return; }   // the market
            if (id == Race.BUILDING_HALL)
            {
                DrawHall(b, ma, grow, team, baseMaterial);
                DrawTierExtras(b, m, team, baseMaterial, 3.2f * ks);
                return;
            }
            if (id == Race.BUILDING_PALISADE)
            {
                rp.material = Mat(TierTex(b, "palisade_bark"), "palisade_team", team, baseMaterial, 0.3f);
                DrawPalisade(b, m, rise, rp.material);
                DrawTierExtras(b, m, team, baseMaterial, 1.1f);
                return;
            }
            // shrine
            if (!DrawKit(b, grow, team, baseMaterial))
            {
                rp.material = Mat("shrine_stone", "shrine_team", team, baseMaterial, 0.5f);
                Emit(Stones(), rp.material, grow, true);
                rp.material = Mat("palisade_bark", "palisade_team", team, baseMaterial, 0.3f);
                Emit(Totem(), rp.material, grow, true);
            }
            if (!b.isComplete()) return;
            DrawTierExtras(b, m, team, baseMaterial, 3.4f * ks);
            float t = Time.time;
            // glowing orb on the totem
            var orbPos = ma.MultiplyPoint3x4(new Vector3(0, 4.15f, 0));
            float pulse = 0.5f + 0.5f * Mathf.Sin(t * 3.1f);
            var glowColor = Color.Lerp(new Color(1f, 0.85f, 0.45f), team, 0.25f) * (2.2f + pulse * 1.5f);
            s_block.Clear();
            s_block.SetColor("_BaseColor", glowColor);
            Graphics.RenderMesh(new RenderParams(Glow()) { matProps = s_block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off },
                Orb(), 0, Matrix4x4.TRS(orbPos, Quaternion.identity, Vector3.one * (0.45f + pulse * 0.08f)));
            // the healing pulse: a ring of light sweeping out to the aura radius every 2 s
            float phase = (t % 2f) / 2f;
            float radius = Mathf.Lerp(1.5f, 14f, phase);
            var ringBlock = s_ringBlock;
            ringBlock.Clear();
            ringBlock.SetColor("_BaseColor", new Color(1f, 0.9f, 0.55f, (1f - phase) * 0.55f));
            var center = m.MultiplyPoint3x4(new Vector3(0, 0.25f, 0));
            Graphics.RenderMesh(new RenderParams(Pulse()) { matProps = ringBlock, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off },
                Ring(), 0, Matrix4x4.TRS(center, Quaternion.identity, new Vector3(radius, 1, radius)));
        }

        /// <summary>Adds the round shield to a champion's GameObject (once): on the back; a melee Huscarl / Spear Guard
        /// carries it on the other side, held up in front.</summary>
        static readonly Dictionary<int, GameObject> s_shields = new Dictionary<int, GameObject>();

        /// <summary>A shield for champions (on the back) and heavy melee warriors (on the left forearm). It rides a
        /// bone, so it moves with the body instead of standing still while the unit animates through it.</summary>
        public static void AttachShield(GameObject unitRoot, Unit u, Color team, Material baseMaterial, bool front = false)
        {
            int id = unitRoot.GetHashCode();
            if (s_shields.TryGetValue(id, out var existing) && existing != null) return;   // (gone with a LOD switch: again)
            bool vik = Vikings(u);
            var go = new GameObject("Shield");
            go.AddComponent<MeshFilter>().sharedMesh = Shield();
            // the tribe's own painted shield (the Viking warrior's iron-rimmed boards, a Native hide shield in the
            // warriors' colours), team-coloured like theirs; the Elemental tribe's in its colours
            string stex = vik ? "bk_shield_viking" : "bk_shield_native";
            if (u.getOwner().isElemental()) stex = ElementalTex(stex);
            go.AddComponent<MeshRenderer>().sharedMaterial = Mat(stex, vik ? "bk_shield_viking_team" : "bk_shield_native_team", team, baseMaterial, vik ? 0.6f : 0.25f);
            // where it sits on the unit in its rest pose: TT units face +x in their local frame (Java yaw 0 = +x)
            var inRoot = Matrix4x4.TRS(front ? new Vector3(0.32f, 1.05f, 0.18f) : new Vector3(-0.2f, 1.15f, 0f),
                front ? Quaternion.Euler(0f, 90f, -6f) : Quaternion.Euler(0f, -90f, 8f), Vector3.one);
            Transform bone = null;
            int bi = -1;
            var smr = unitRoot.GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr != null && smr.sharedMesh != null)
            {
                string want = front ? "L Forearm" : "Spine1";
                var bones = smr.bones;
                for (int i = 0; i < bones.Length; i++)
                    if (bones[i] != null && bones[i].name.EndsWith(want)) { bone = bones[i]; bi = i; break; }
                if (bone != null && bi >= smr.sharedMesh.bindposes.Length) bone = null;
            }
            if (bone != null)
            {
                // the same matrix a vertex skinned only to that bone gets: rest pose (mesh space) -> bone space
                var rootToMesh = (unitRoot.transform.worldToLocalMatrix * smr.transform.localToWorldMatrix).inverse;
                var local = smr.sharedMesh.bindposes[bi] * (ShieldFromSkeleton(smr, front, vik, out var inMesh) ? inMesh : rootToMesh * inRoot);
                go.transform.SetParent(bone, false);
                go.transform.localPosition = local.GetColumn(3);
                go.transform.localRotation = local.rotation;
                go.transform.localScale = local.lossyScale;
            }
            else
            {
                go.transform.SetParent(unitRoot.transform, false);
                go.transform.localPosition = inRoot.GetColumn(3);
                go.transform.localRotation = inRoot.rotation;
            }
            s_shields[id] = go;
        }

        static int Bone(Transform[] bones, string suffix)
        {
            for (int i = 0; i < bones.Length; i++) if (bones[i] != null && bones[i].name.EndsWith(suffix)) return i;
            return -1;
        }

        /// <summary>
        /// Where the shield goes, worked out from the unit's own skeleton in its rest pose (mesh space) rather than
        /// fixed numbers: on the back, flat against the shoulder blades, between the spine and the neck; or on the left
        /// forearm, strapped on its outer side and facing out and a little forward. Sized to the torso.
        /// </summary>
        static bool ShieldFromSkeleton(SkinnedMeshRenderer smr, bool front, bool vik, out Matrix4x4 inMesh)
        {
            inMesh = Matrix4x4.identity;
            var mesh = smr.sharedMesh; var bones = smr.bones;
            int neck = Bone(bones, "Neck"), pelvis = Bone(bones, "Pelvis"), lu = Bone(bones, "L UpperArm"), ru = Bone(bones, "R UpperArm"),
                toe = Bone(bones, "L Toe0"), foot = Bone(bones, "L Foot"), fore = Bone(bones, "L Forearm"), hand = Bone(bones, "L Hand");
            int n = mesh.bindposes.Length;
            foreach (int k in new[] { neck, pelvis, lu, ru }) if (k < 0 || k >= n) return false;
            Vector3 P(int b) => mesh.bindposes[b].inverse.MultiplyPoint3x4(Vector3.zero);
            Vector3 pN = P(neck), pP = P(pelvis), pL = P(lu), pR = P(ru);
            var up = (pN - pP).normalized;
            float torso = (pN - pP).magnitude;
            if (torso < 1e-3f) return false;
            var side = Vector3.ProjectOnPlane(pL - pR, up).normalized;   // towards the left
            Vector3 fwd = Vector3.zero;
            if (toe >= 0 && foot >= 0 && toe < n && foot < n) fwd = Vector3.ProjectOnPlane(P(toe) - P(foot), up).normalized;
            if (fwd.sqrMagnitude < 0.5f) fwd = Vector3.Cross(side, up).normalized;
            float scale = torso * (vik ? 1.2f : 1.05f);   // a rim about half the torso long (the mesh is 0.42 across)
            Vector3 center, normal, upAxis;
            if (!front)
            {
                // over the champion's cape (which hangs clear of the back), leaning with it: the top towards the shoulders
                center = Vector3.Lerp(pP, pN, 0.62f) - fwd * ((vik ? 0.44f : 0.33f) * torso);
                upAxis = (up + 0.22f * fwd).normalized;
                normal = Vector3.ProjectOnPlane(-fwd, upAxis).normalized;
            }
            else
            {
                if (fore < 0 || hand < 0 || fore >= n || hand >= n) return false;
                Vector3 pF = P(fore), pH = P(hand);
                var arm = pH - pF;
                if (arm.sqrMagnitude < 1e-6f) return false;
                var axis = arm.normalized;
                var mid = Vector3.Lerp(pF, pH, 0.45f);
                // out from the body's middle, square to the forearm, then a little forward
                var spineLine = pP + up * Vector3.Dot(mid - pP, up);
                var outward = Vector3.ProjectOnPlane(mid - spineLine, axis);
                if (outward.sqrMagnitude < 1e-6f) outward = Vector3.ProjectOnPlane(side, axis);
                normal = (outward.normalized + 0.45f * Vector3.ProjectOnPlane(fwd, axis)).normalized;
                center = mid + normal * (0.07f * torso);
                upAxis = Vector3.ProjectOnPlane(-axis, normal).normalized;   // the shield's top towards the elbow
                if (upAxis.sqrMagnitude < 0.5f) upAxis = Vector3.ProjectOnPlane(up, normal).normalized;
            }
            var rot = Quaternion.LookRotation(normal, upAxis);   // the shield mesh faces +z, its top is +y
            inMesh = Matrix4x4.TRS(center, rot, Vector3.one * scale);
            return true;
        }

        // ================================================================== second batch: resources, tiers, halls, race units

        /// <summary>0 gold, 1 crystal, 2 herbs, 3 coal (ExtendedResources.RARE order), or -1.</summary>
        public static int RareKind(object s) => s is GoldSupply ? 0 : s is CrystalSupply ? 1 : s is HerbSupply ? 2 : s is CoalSupply ? 3 : s is DiamondSupply ? 4 : -1;

        /// <summary>The rock model recoloured: gold ore and coal (texture ids 100 + kind in FaithfulView's batches).</summary>
        public static readonly Color[] RareTints = { new Color(1.45f, 1.1f, 0.35f, 1f), new Color(0.6f, 0.95f, 1.2f, 1f), new Color(0.5f, 0.95f, 0.4f, 1f), new Color(0.2f, 0.2f, 0.23f, 1f), new Color(0.78f, 0.8f, 0.86f, 1f) };
        public static float RareGloss(int k) => k == 0 ? 1.8f : k == 3 ? 0.7f : k == 4 ? 1.1f : 0.4f;

        static Material s_diamondMat;
        static Mesh s_crystal, s_leaves, s_flowers, s_stoneRing, s_ironBand;

        /// <summary>Set by FaithfulView each frame: adds one instance to an instanced batch (mesh, material, matrix,
        /// casts shadows). Without it (no view) the models are drawn one by one.</summary>
        public static System.Action<Mesh, Material, Matrix4x4, bool> Instanced;

        static void Emit(Mesh mesh, Material material, Matrix4x4 m, bool shadows)
        {
            TTFit.Add(mesh, m);   // (measuring a New Tides building's hitbox)
            if (s_skin != Vector4.zero) material = SkinMat(material);
            if (Instanced != null) { Instanced(mesh, material, m, shadows); return; }
            Graphics.RenderMesh(new RenderParams(material)
            {
                shadowCastingMode = shadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off,
                receiveShadows = true
            }, mesh, 0, m);
        }

        static Material s_crystalGlow;
        static Material CrystalGlow()
        {
            if (s_crystalGlow != null) return s_crystalGlow;
            var sh = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            s_crystalGlow = new Material(sh) { name = "Crystal glow", enableInstancing = true };
            s_crystalGlow.SetColor("_BaseColor", new Color(0.55f, 1.6f, 2f, 1f));
            return s_crystalGlow;
        }
        static Mesh s_hallWalls, s_hallRoof, s_hallGables, s_lodgePoles, s_lodgeCover;
        static Material s_crystalMat, s_leafMat, s_flowerMat;
        static readonly Dictionary<int, float> s_radius = new Dictionary<int, float>();

        static Material Plain(Material baseMaterial, Color c, float gloss, bool twoSided, string name)
        {
            var m = baseMaterial != null ? new Material(baseMaterial) : new Material(Shader.Find("TribalTrouble/Sprite"));
            m.name = name;
            m.enableInstancing = true;
            m.SetTexture("_BaseMap", Texture2D.whiteTexture);
            m.SetTexture("_TeamMask", Texture2D.blackTexture);
            m.SetFloat("_UseTeamMask", 0f);
            m.SetColor("_Tint", c);
            m.SetFloat("_Gloss", gloss);
            m.SetFloat("_Wind", 0f);
            m.SetFloat("_Cull", twoSided ? 0f : 2f);
            return m;
        }

        /// <summary>A cluster of pointed six-sided crystals, about 1.4 m tall.</summary>
        internal static Mesh Crystal()
        {
            if (s_crystal != null) return s_crystal;
            var mb = new MeshBuilder();
            var rnd = new System.Random(11);
            for (int k = 0; k < 6; k++)
            {
                float a = k * 1.05f + (float)rnd.NextDouble() * 0.4f;
                float off = k == 0 ? 0f : 0.35f + (float)rnd.NextDouble() * 0.25f;
                var basePos = new Vector3(Mathf.Cos(a) * off, -0.15f, Mathf.Sin(a) * off);
                var axis = (Vector3.up + new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * (k == 0 ? 0.05f : 0.45f)).normalized;
                float h = k == 0 ? 1.4f : 0.6f + (float)rnd.NextDouble() * 0.5f, r = k == 0 ? 0.22f : 0.1f + (float)rnd.NextDouble() * 0.06f;
                var side = Vector3.Cross(axis, Vector3.forward).normalized; var side2 = Vector3.Cross(axis, side);
                var ring = new Vector3[6];
                for (int i = 0; i < 6; i++) ring[i] = (side * Mathf.Cos(i * Mathf.PI / 3) + side2 * Mathf.Sin(i * Mathf.PI / 3)) * r;
                var top = basePos + axis * h; var tip = top + axis * r * 2.2f;
                for (int i = 0; i < 6; i++)
                {
                    int j = (i + 1) % 6;
                    mb.Quad(basePos + ring[j], basePos + ring[i], top + ring[i], top + ring[j], new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1));
                    mb.Tri(top + ring[j], top + ring[i], tip, new Vector2(0, 0), new Vector2(1, 0), new Vector2(.5f, 1));
                }
            }
            return s_crystal = mb.Build("Crystal");
        }

        /// <summary>A herb bush: crossed leaf blades and a few flowers.</summary>
        internal static void Herbs(out Mesh leaves, out Mesh flowers)
        {
            if (s_leaves == null)
            {
                var mb = new MeshBuilder(); var fb = new MeshBuilder();
                var rnd = new System.Random(5);
                for (int i = 0; i < 14; i++)
                {
                    float a = i * 2.4f, len = 0.55f + (float)rnd.NextDouble() * 0.4f, w = 0.11f;
                    var dir = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
                    var up = Vector3.up * (0.5f + (float)rnd.NextDouble() * 0.6f);
                    var b0 = dir * 0.05f; var tipP = dir * len * 0.7f + up;
                    var sideV = Vector3.Cross(dir, Vector3.up) * w;
                    var mid = (b0 + tipP) * 0.5f + Vector3.up * 0.1f;
                    mb.Quad(b0 - sideV * .3f, b0 + sideV * .3f, mid + sideV, mid - sideV, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, .5f), new Vector2(0, .5f));
                    mb.Quad(mid - sideV, mid + sideV, tipP + sideV * .1f, tipP - sideV * .1f, new Vector2(0, .5f), new Vector2(1, .5f), new Vector2(1, 1), new Vector2(0, 1));
                    if (i % 4 == 0)
                    {
                        var c = tipP + Vector3.up * 0.06f; float fr = 0.09f;
                        for (int p = 0; p < 5; p++)
                        {
                            float pa = p * Mathf.PI * 2 / 5;
                            var o = new Vector3(Mathf.Cos(pa), 0.2f, Mathf.Sin(pa)) * fr;
                            fb.Tri(c, c + o + Vector3.Cross(o, Vector3.up) * .5f, c + o - Vector3.Cross(o, Vector3.up) * .5f, new Vector2(.5f, .5f), new Vector2(0, 1), new Vector2(1, 1));
                        }
                    }
                }
                s_leaves = mb.Build("Herb leaves"); s_flowers = fb.Build("Herb flowers");
            }
            leaves = s_leaves; flowers = s_flowers;
        }

        /// <summary>Crystal and herb deposits (gold and coal are the recoloured rocks).</summary>
        public static void DrawRare(int k, Matrix4x4 m, Material baseMaterial, bool shadows)
        {
            if (k == 1)
            {
                s_crystalMat ??= Plain(baseMaterial, new Color(0.55f, 0.92f, 1.25f, 1f), 2f, false, "Crystal");
                Emit(Crystal(), s_crystalMat, m, shadows);
                // a soft glow in the heart of it
                Emit(Orb(), CrystalGlow(), m * Matrix4x4.TRS(new Vector3(0, 0.45f, 0), Quaternion.identity, Vector3.one * 0.28f), false);
            }
            else if (k == 4)   // diamonds: white-blue shards bursting out of a pale rock (the rock is the instanced model)
            {
                s_diamondMat ??= Plain(baseMaterial, new Color(1.25f, 1.3f, 1.45f, 1f), 3f, false, "Diamond");
                Emit(Crystal(), s_diamondMat, m * Matrix4x4.TRS(new Vector3(0.15f, 0.35f, 0.1f), Quaternion.Euler(0, 35, 12), Vector3.one * 0.55f), shadows);
                Emit(Crystal(), s_diamondMat, m * Matrix4x4.TRS(new Vector3(-0.25f, 0.3f, -0.15f), Quaternion.Euler(0, 140, -18), Vector3.one * 0.4f), shadows);
            }
            else if (k == 2)
            {
                Herbs(out var leaves, out var flowers);
                s_leafMat ??= Plain(baseMaterial, new Color(0.35f, 0.75f, 0.25f, 1f), 0.3f, true, "Herb leaves");
                s_flowerMat ??= Plain(baseMaterial, new Color(1f, 0.45f, 0.85f, 1f), 0.3f, true, "Herb flowers");
                Emit(leaves, s_leafMat, m, shadows);
                Emit(flowers, s_flowerMat, m, false);
            }
        }

        // ------------------------------------------------------------------ building tiers

        static string TierTex(LandBuilding b, string wood) => wood;   // (the tier skin shows the upgrade now)

        /// <summary>Unit-radius ring of squared stone blocks hugging the ground (scaled to the building).</summary>
        static Mesh StoneRing()
        {
            if (s_stoneRing != null) return s_stoneRing;
            var mb = new MeshBuilder();
            const int n = 22;
            for (int i = 0; i < n; i++)
            {
                float a0 = i * Mathf.PI * 2 / n + 0.02f, a1 = (i + 1) * Mathf.PI * 2 / n - 0.02f;
                float h = 0.55f + ((i * 7) % 3) * 0.08f;
                Vector3 P(float a, float rr, float y) => new Vector3(Mathf.Cos(a) * rr, y, Mathf.Sin(a) * rr);
                float ri = 0.9f, ro = 1.02f;
                mb.Quad(P(a1, ro, -0.2f), P(a0, ro, -0.2f), P(a0, ro, h), P(a1, ro, h), new Vector2(0, 0), new Vector2(.3f, 0), new Vector2(.3f, .25f), new Vector2(0, .25f));
                mb.Quad(P(a0, ri, -0.2f), P(a1, ri, -0.2f), P(a1, ri, h), P(a0, ri, h), new Vector2(0, 0), new Vector2(.3f, 0), new Vector2(.3f, .25f), new Vector2(0, .25f));
                mb.Quad(P(a0, ro, h), P(a0, ri, h), P(a1, ri, h), P(a1, ro, h), new Vector2(0, .3f), new Vector2(.1f, .3f), new Vector2(.1f, .4f), new Vector2(0, .4f));
                mb.Quad(P(a0, ri, -0.2f), P(a0, ri, h), P(a0, ro, h), P(a0, ro, -0.2f), new Vector2(0, 0), new Vector2(.1f, 0), new Vector2(.1f, .1f), new Vector2(0, .1f));
                mb.Quad(P(a1, ro, -0.2f), P(a1, ro, h), P(a1, ri, h), P(a1, ri, -0.2f), new Vector2(0, 0), new Vector2(.1f, 0), new Vector2(.1f, .1f), new Vector2(0, .1f));
            }
            return s_stoneRing = mb.Build("Stone tier ring");
        }

        /// <summary>An iron band round the stone footing (no spikes: the iron is in the building's skin).</summary>
        static Mesh IronBand()
        {
            if (s_ironBand != null) return s_ironBand;
            var mb = new MeshBuilder();
            const int n = 40;
            for (int i = 0; i < n; i++)
            {
                float a0 = i * Mathf.PI * 2 / n, a1 = (i + 1) * Mathf.PI * 2 / n;
                Vector3 P(float a, float rr, float y) => new Vector3(Mathf.Cos(a) * rr, y, Mathf.Sin(a) * rr);
                mb.Quad(P(a1, 1.04f, 0.3f), P(a0, 1.04f, 0.3f), P(a0, 1.04f, 0.45f), P(a1, 1.04f, 0.45f), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1));
            }
            return s_ironBand = mb.Build("Iron tier band");
        }

        /// <summary>Was the stone footing and its iron band round an upgraded building: gone - the upgrade is the building's
        /// own skin (SkinOf: stone, iron, crystal and diamond, chicken), with no ring of blocks round it.</summary>
        public static void DrawTierExtras(LandBuilding b, Matrix4x4 m, Color team, Material baseMaterial, float radius)
        {
            return;
#pragma warning disable CS0162
            if (b.getTier() == 0) return;
            var keep = s_skin;
            s_skin = Vector4.zero;
            var pos = new Vector3(m.m03, m.m13, m.m23);
            var ring = Matrix4x4.TRS(pos, m.rotation, new Vector3(radius, 1f, radius));
            Emit(StoneRing(), Mat("shrine_stone", null, Color.white, baseMaterial, 0.4f), ring, true);
            if (b.getTier() >= 2)
                Emit(IronBand(), IronMat(baseMaterial), ring, true);
            s_skin = keep;
#pragma warning restore CS0162
        }

        // ------------------------------------------------------------------ tier skins

        static Vector4 s_skin;   // the skin of the building being drawn (Emit puts it on), zero for none
        static bool s_skinVik;
        static readonly Dictionary<(Material, int, bool), Material> s_skinMats = new Dictionary<(Material, int, bool), Material>();
        static readonly int SkinId = Shader.PropertyToID("_Skin"), SkinGlowId = Shader.PropertyToID("_SkinGlow");

        /// <summary>A building's tier skin for the sprite shader's _Skin: x stone, y iron, z crystal and diamond (the
        /// third tier: the Natives' crystal-bound, the Vikings' rune-carved), w chicken. Upgrades add up.</summary>
        public static Vector4 SkinOf(LandBuilding b)
        {
            int t = b.getTier();
            return new Vector4(t >= 1 ? 1f : 0f, t >= 2 ? 1f : 0f, t >= 3 ? 1f : 0f, b.hasChickenBarbs() ? 1f : 0f);
        }

        /// <summary>The crystal's glow: cyan (Natives), icier (Vikings).</summary>
        public static Color SkinGlow(bool vik) => vik ? new Color(0.55f, 0.85f, 1.35f, 1f) : new Color(0.25f, 0.95f, 1.3f, 1f);

        /// <summary>The material in the skin being drawn (one copy per material and skin, so it still batches).</summary>
        static Material SkinMat(Material material)
        {
            if (material == null || !material.HasProperty(SkinId)) return material;
            int bits = (s_skin.x > 0 ? 1 : 0) | (s_skin.y > 0 ? 2 : 0) | (s_skin.z > 0 ? 4 : 0) | (s_skin.w > 0 ? 8 : 0);
            var key = (material, bits, s_skinVik);
            if (s_skinMats.TryGetValue(key, out var sm)) return sm;
            sm = new Material(material) { name = material.name + " skin " + bits };
            sm.SetVector(SkinId, s_skin);
            sm.SetColor(SkinGlowId, SkinGlow(s_skinVik));
            s_skinMats[key] = sm;
            return sm;
        }

        static Material s_iron;
        static Material IronMat(Material bm) => s_iron ??= Plain(bm, new Color(0.32f, 0.35f, 0.4f, 1f), 1.8f, false, "Iron tier");

        static readonly MaterialPropertyBlock s_tintBlock = new MaterialPropertyBlock();
        static readonly int TintId = Shader.PropertyToID("_Tint");

        sealed class TintState { public Renderer[] renderers; public Color color; public Vector4 skin; }

        static readonly int TrimId = Shader.PropertyToID("_Trim");
        static readonly int JitterId = Shader.PropertyToID("_Jitter");
        sealed class LookState { public Renderer[] renderers; public Vector4 trim, jitter; }
        static readonly Dictionary<int, LookState> s_looks = new Dictionary<int, LookState>();

        /// <summary>
        /// A warrior's look (New Tides): the metal of his gear in his tribe's armour - iron at the first two levels of
        /// the Armour research, gold at the third, crystal at the fourth, diamond at the fifth (crystal and diamond
        /// warriors always in their own stone). The same for a whole tribe, so the units still batch as before. Called
        /// every frame for the warriors drawn as models.
        /// </summary>
        // ------------------------------------------------------------------ melee blows (presentation only)

        /// <summary>
        /// A melee warrior's blow: the original models have only the throw, so a fighter who keeps his weapon steps
        /// into the swing (a lunge forward and a lean over his weapon at the moment it lands) - and the chicken
        /// whirlers spin right round through it. The chieftain's blows lunge too. Called after the unit's object is
        /// placed for the frame; nothing here touches the game.
        /// </summary>
        public static void MeleeMotion(GameObject go, Unit u, int kind, int anim, float ticks)
        {
            if (go == null || u == null || anim != com.oddlabs.tt.model.Unit.Animation.THROWING || u.isDead()) return;
            var f = u.getWeaponFactory();
            bool chief = u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC);
            if (!(f is com.oddlabs.tt.model.weapon.MeleeFactory) && !chief) return;
            if (!(u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour)) return;
            float r = f.getReleaseRatio(), p = ticks - Mathf.Floor(ticks);
            var t = go.transform;
            var fwd = TTSpace.ToUnity(u.getDirectionX(), u.getDirectionY(), 0f);
            if (fwd.sqrMagnitude < 1e-6f) return;
            fwd.Normalize();
            // a bell round the moment of the blow
            float bell = Mathf.Exp(-((p - r) * (p - r)) / (2f * 0.11f * 0.11f));
            t.position += fwd * (0.32f * bell * t.lossyScale.y);
            t.rotation = Quaternion.AngleAxis(14f * bell, Vector3.Cross(Vector3.up, fwd)) * t.rotation;
            if (kind == Race.UNIT_SPINNER)
            {
                // right round, from just before the blow to a little after it
                float s = Mathf.Clamp01((p - (r - 0.22f)) / 0.5f);
                s = s * s * (3f - 2f * s);
                t.rotation = Quaternion.AngleAxis(360f * s, Vector3.up) * t.rotation;
            }
        }

        public static void UnitLook(GameObject go, Unit u, int kind)
        {
            if (go == null || u == null || !u.getOwner().getWorld().isExtended()) return;
            int armour = u.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ARMOR);
            Vector4 trim = Vector4.zero;
            if (kind == Race.UNIT_DIAMOND || armour >= 5) trim = new Vector4(0.95f, 0.98f, 1.12f, 4f);
            else if (kind == Race.UNIT_CRYSTAL || armour == 4) { var g = SkinGlow(Vikings(u)); trim = new Vector4(g.r * 0.75f, g.g * 0.8f, g.b * 0.85f, 3f); }
            else if (armour == 3) trim = new Vector4(1.18f, 0.86f, 0.34f, 2f);
            else if (armour >= 1) trim = new Vector4(0.62f, 0.67f, 0.76f, 1f);
            // (a shade of his own per warrior was tried: a property block of its own per unit broke the batching of
            // every unit near the camera - the frame rate halved in the big battle - so the clothes stay as painted)
            Vector4 jitter = Vector4.zero;
            int id = go.GetHashCode();
            if (trim.w == 0f && !s_looks.ContainsKey(id)) return;
            if (s_looks.TryGetValue(id, out var st))
            {
                bool valid = true;
                foreach (var rr in st.renderers) if (rr == null) { valid = false; break; }
                if (valid && st.trim == trim && st.jitter == jitter) return;
                if (!valid) st = null;
            }
            if (st == null)
            {
                if (s_looks.Count > 4096) s_looks.Clear();
                s_tintScratch.Clear();
                go.GetComponentsInChildren(s_tintScratch);
                for (int i = s_tintScratch.Count - 1; i >= 0; i--)
                {
                    var rr = s_tintScratch[i];
                    string n = rr != null ? rr.gameObject.name : null;
                    if (n == null || n.StartsWith("Glow") || n == "Caster orb") s_tintScratch.RemoveAt(i);
                }
                st = new LookState { renderers = s_tintScratch.ToArray() };
                s_looks[id] = st;
            }
            st.trim = trim;
            st.jitter = jitter;
            foreach (var rr in st.renderers)
            {
                rr.GetPropertyBlock(s_tintBlock);
                s_tintBlock.SetVector(TrimId, trim);
                s_tintBlock.SetVector(JitterId, jitter);
                rr.SetPropertyBlock(s_tintBlock);
            }
        }
        static readonly Dictionary<int, TintState> s_tints = new Dictionary<int, TintState>();
        static readonly List<Renderer> s_tintScratch = new List<Renderer>();

        /// <summary>Tints a model's renderers (merged into whatever property block they carry). Called every frame:
        /// the renderers are found once per model and the blocks touched only when the colour changes or the model
        /// was rebuilt.</summary>
        public static void Tint(GameObject go, Color c, Vector4 skin = default, bool vik = false)
        {
            int id = go.GetHashCode();
            if (s_tints.TryGetValue(id, out var st))
            {
                bool valid = true;
                foreach (var rr in st.renderers) if (rr == null) { valid = false; break; }
                if (valid && st.color == c && st.skin == skin) return;
                if (!valid) st = null;
            }
            if (st == null)
            {
                if (s_tints.Count > 4096) s_tints.Clear(); // models come and go: drop stale entries now and then
                s_tintScratch.Clear();
                go.GetComponentsInChildren(s_tintScratch);
                for (int i = s_tintScratch.Count - 1; i >= 0; i--)
                {
                    var rr = s_tintScratch[i];
                    string n = rr != null ? rr.gameObject.name : null;
                    if (n == null || n == "Shield" || n.StartsWith("Glow") || n.StartsWith("Unique") || n == "Cape") s_tintScratch.RemoveAt(i);
                }
                st = new TintState { renderers = s_tintScratch.ToArray() };
                s_tints[id] = st;
            }
            st.color = c;
            st.skin = skin;
            foreach (var rr in st.renderers)
            {
                rr.GetPropertyBlock(s_tintBlock);
                s_tintBlock.SetColor(TintId, c);
                s_tintBlock.SetVector(SkinId, skin);
                s_tintBlock.SetColor(SkinGlowId, SkinGlow(vik));
                rr.SetPropertyBlock(s_tintBlock);
            }
        }

        /// <summary>A classic building (quarters, armory, tower) rebuilt in stone or iron (crystal and diamond), or barbed:
        /// its model in the tier skin, and the footing drawn round it at its own size.</summary>
        public static void DrawTier(LandBuilding b, Matrix4x4 m, Color team, Material baseMaterial, GameObject go)
        {
            if (go == null) return;
            int id = go.GetHashCode();
            if (!s_radius.TryGetValue(id, out float radius))
            {
                var rs = go.GetComponentsInChildren<Renderer>();
                if (rs.Length == 0) return;
                var bb = rs[0].bounds;
                foreach (var r in rs) bb.Encapsulate(r.bounds);
                radius = Mathf.Clamp(Mathf.Min(bb.extents.x, bb.extents.z) * 0.92f, 1.2f, 9f);
                s_radius[id] = radius;
            }
            Tint(go, Color.white, b.isComplete() ? SkinOf(b) : Vector4.zero, Vikings(b));   // the tier skin on the model itself
            DrawTierExtras(b, m, team, baseMaterial, radius);
        }

        // ------------------------------------------------------------------ race halls

        /// <summary>Mead Hall: a longhouse of upright logs under a steep bark roof. Spirit Lodge: a lodge of poles under
        /// painted hides with a glowing spirit orb above.</summary>
        static void DrawHall(LandBuilding b, Matrix4x4 m, Matrix4x4 grow, Color team, Material baseMaterial)
        {
            bool vik = Vikings(b);
            var rp = new RenderParams(null) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows = true };
            if (DrawKit(b, grow, team, baseMaterial)) { }
            else if (vik)
            {
                Longhouse(out var walls, out var roof, out var gables);
                rp.material = Mat(TierTex(b, "palisade_bark"), "palisade_team", team, baseMaterial, 0.3f);
                Emit(walls, rp.material, grow, true);
                rp.material = Mat("palisade_bark", null, team, baseMaterial, 0.2f);
                Emit(gables, rp.material, grow, true);
                rp.material = Mat("shield_viking", "shield_viking_team", team, baseMaterial, 0.4f);
                Emit(roof, rp.material, grow, true);
            }
            else
            {
                Lodge(out var poles, out var cover);
                rp.material = Mat("palisade_bark", null, team, baseMaterial, 0.3f);
                Emit(poles, rp.material, grow, true);
                rp.material = Mat("shield_native", "shield_native_team", team, baseMaterial, 0.3f);
                Emit(cover, rp.material, grow, true);
            }
            if (!b.isComplete()) return;
            float t = Time.time, pulse = 0.5f + 0.5f * Mathf.Sin(t * 2.4f);
            s_block.Clear();
            s_block.SetColor("_BaseColor", (vik ? new Color(1f, 0.6f, 0.25f) : new Color(0.4f, 1f, 0.7f)) * (2f + pulse));
            var at = vik ? new Vector3(0, 0.9f, 3.1f) : new Vector3(0, 6.2f, 0);
            Graphics.RenderMesh(new RenderParams(Glow()) { matProps = s_block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off },
                Orb(), 0, m * Matrix4x4.TRS(at, Quaternion.identity, Vector3.one * (vik ? 0.35f : 0.55f + pulse * 0.08f)));
            // the aura's reach, sweeping out every 2 s (HallAura.PERIOD)
            float phase = (t % 2f) / 2f;
            var ringBlock = s_ringBlock;
            ringBlock.Clear();
            ringBlock.SetColor("_BaseColor", (vik ? new Color(1f, 0.6f, 0.3f) : new Color(0.4f, 1f, 0.6f)) * new Color(1, 1, 1, (1f - phase) * 0.4f));
            var center = m.MultiplyPoint3x4(new Vector3(0, 0.25f, 0));
            float radius = Mathf.Lerp(2f, 16f, phase);
            Graphics.RenderMesh(new RenderParams(Pulse()) { matProps = ringBlock, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off },
                Ring(), 0, Matrix4x4.TRS(center, Quaternion.identity, new Vector3(radius, 1, radius)));
        }

        static void Longhouse(out Mesh walls, out Mesh roof, out Mesh gables)
        {
            if (s_hallWalls == null)
            {
                var wb = new MeshBuilder(); var rb = new MeshBuilder(); var gb = new MeshBuilder();
                var rnd = new System.Random(31);
                const float L = 3.4f, W = 2.3f, H = 2.2f, roofH = 2.4f;
                // long walls: upright logs (bowed a little like a real longhouse)
                for (int side = -1; side <= 1; side += 2)
                    for (int i = 0; i <= 14; i++)
                    {
                        float z = -L + i * (2 * L / 14f);
                        float bow = 1f - 0.12f * (z / L) * (z / L);
                        wb.Log(new Vector3(side * W * bow, -0.3f, z), 0.2f, H, 0.0f, (float)rnd.NextDouble(), Vector3.zero);
                    }
                // end walls
                for (int end = -1; end <= 1; end += 2)
                    for (int i = 1; i < 9; i++)
                    {
                        float x = -W + i * (2 * W / 9f);
                        if (end == 1 && Mathf.Abs(x) < 0.6f) continue; // the door
                        gb.Log(new Vector3(x, -0.3f, end * L * 1.0f), 0.19f, H + 0.3f, 0.0f, (float)rnd.NextDouble(), Vector3.zero);
                    }
                // steep roof, two slopes, overhanging, with the ridge running the hall's length
                for (int side = -1; side <= 1; side += 2)
                {
                    var eaveA = new Vector3(side * (W + 0.55f), H - 0.25f, -L - 0.6f);
                    var eaveB = new Vector3(side * (W + 0.55f), H - 0.25f, L + 0.6f);
                    var ridgeA = new Vector3(0, H + roofH, -L - 0.8f);
                    var ridgeB = new Vector3(0, H + roofH, L + 0.8f);
                    if (side > 0) rb.Quad(eaveA, eaveB, ridgeB, ridgeA, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1));
                    else rb.Quad(eaveB, eaveA, ridgeA, ridgeB, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1));
                    // underside so it isn't see-through from below
                    if (side > 0) rb.Quad(ridgeA, ridgeB, eaveB, eaveA, new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0));
                    else rb.Quad(ridgeB, ridgeA, eaveA, eaveB, new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0));
                }
                // gable triangles
                for (int end = -1; end <= 1; end += 2)
                {
                    var a = new Vector3(-W, H, end * L); var bq = new Vector3(W, H, end * L); var c = new Vector3(0, H + roofH - 0.1f, end * L);
                    if (end > 0) gb.Tri(a, bq, c, new Vector2(0, 0), new Vector2(1, 0), new Vector2(.5f, 1));
                    else gb.Tri(bq, a, c, new Vector2(0, 0), new Vector2(1, 0), new Vector2(.5f, 1));
                }
                s_hallWalls = wb.Build("Mead hall walls"); s_hallRoof = rb.Build("Mead hall roof"); s_hallGables = gb.Build("Mead hall gables");
            }
            walls = s_hallWalls; roof = s_hallRoof; gables = s_hallGables;
        }

        static void Lodge(out Mesh poles, out Mesh cover)
        {
            if (s_lodgePoles == null)
            {
                var pb = new MeshBuilder(); var cb = new MeshBuilder();
                var rnd = new System.Random(17);
                const int n = 12; const float R = 3.1f, H = 5.4f;
                var apex = new Vector3(0, H, 0);
                for (int i = 0; i < n; i++)
                {
                    float a = i * Mathf.PI * 2 / n;
                    var basePos = new Vector3(Mathf.Cos(a) * R, -0.2f, Mathf.Sin(a) * R);
                    var lean = (apex - basePos) / H - Vector3.up;   // log leaning to the apex (and a bit beyond)
                    pb.Log(basePos, 0.12f, H * 1.12f, 0.3f, (float)rnd.NextDouble(), lean);
                }
                // the hide cover: a cone from 0.1 m up to 85 % of the height, open at the door
                const int seg = 24;
                for (int i = 0; i < seg; i++)
                {
                    float a0 = i * Mathf.PI * 2 / seg, a1 = (i + 1) * Mathf.PI * 2 / seg;
                    if (Mathf.Abs(Mathf.DeltaAngle(a0 * Mathf.Rad2Deg, 0f)) < 20f) continue; // the door flap is open
                    float r0 = R * 0.98f, r1 = R * 0.2f, y1 = H * 0.82f;
                    var A = new Vector3(Mathf.Cos(a0) * r0, 0.05f, Mathf.Sin(a0) * r0);
                    var B = new Vector3(Mathf.Cos(a1) * r0, 0.05f, Mathf.Sin(a1) * r0);
                    var C = new Vector3(Mathf.Cos(a1) * r1, y1, Mathf.Sin(a1) * r1);
                    var D = new Vector3(Mathf.Cos(a0) * r1, y1, Mathf.Sin(a0) * r1);
                    float u0 = i / (float)seg, u1 = (i + 1) / (float)seg;
                    cb.Quad(B, A, D, C, new Vector2(u1, 0.05f), new Vector2(u0, 0.05f), new Vector2(u0, 0.95f), new Vector2(u1, 0.95f));
                    cb.Quad(A, B, C, D, new Vector2(u0, 0.05f), new Vector2(u1, 0.05f), new Vector2(u1, 0.95f), new Vector2(u0, 0.95f));
                }
                s_lodgePoles = pb.Build("Spirit lodge poles"); s_lodgeCover = cb.Build("Spirit lodge cover");
            }
            poles = s_lodgePoles; cover = s_lodgeCover;
        }

        // ------------------------------------------------------------------ race units

        /// <summary>A small glowing light carried by a race unit: poison (Blowgunner), flame (Fire Thrower).</summary>
        public static void AttachGlow(GameObject unitRoot, string name, Color color, Vector3 at, float size)
        {
            if (!FirstAttach(unitRoot, name) || unitRoot.transform.Find("Glow " + name) != null) return;
            var go = new GameObject("Glow " + name);
            go.transform.SetParent(unitRoot.transform, false);
            go.transform.localPosition = at;
            go.transform.localScale = Vector3.one * size;
            go.AddComponent<MeshFilter>().sharedMesh = Orb();
            var r = go.AddComponent<MeshRenderer>();
            r.sharedMaterial = Glow();
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            var b = new MaterialPropertyBlock();
            b.SetColor("_BaseColor", color);
            r.SetPropertyBlock(b);
            go.AddComponent<TTFloatBob>();
        }

        /// <summary>Dresses a race unit: Blowgunner (poison light), Jaguar Warrior (golden, shield up front), Fire Thrower
        /// (a flame in hand), Berserker (blood-red).</summary>
        public static void DressRaceUnit(GameObject go, Unit u, int kind, Color team, Material baseMaterial)
        {
            bool vik = Vikings(u);
            if (kind == Race.UNIT_UNIQUE_1)
            {
                if (vik) AttachGlow(go, "Flame", new Color(2.4f, 1.1f, 0.25f, 1f), new Vector3(0.55f, 1.35f, 0.3f), 0.22f);
                else AttachGlow(go, "Poison", new Color(0.6f, 2.2f, 0.4f, 1f), new Vector3(0.35f, 1.5f, 0.3f), 0.16f);
            }
            else
            {
                // the Berserker's woad and the Jaguar Warrior's coat are their skins now (TTUniqueArt.SkinFor)
                if (!vik) AttachShield(go, u, team, baseMaterial, true);
            }
        }
    }
}
