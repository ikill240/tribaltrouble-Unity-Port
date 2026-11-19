using TribalTrouble.Faithful;
using com.oddlabs.tt.model;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The arsenal's buildings (TT2 Extended), drawn like the other Extended buildings from simple procedural meshes:
    /// the Forge (a stone smithy with a chimney, an anvil and a glowing crystal - blue for the Natives' Crystal Forge,
    /// ember-orange for the Vikings' Rune Forge) and the War Totem (Natives: a carved pole with outspread wings) /
    /// Rune Stone (Vikings: a standing stone whose runes glow), with the aura's reach pulsing out from it.
    /// </summary>
    public static partial class TTExtendedArt
    {
        static Mesh s_forgeWalls, s_forgeRoof, s_forgeIron, s_warTotem, s_runeStone, s_totemWings;
        static Mesh s_dockWood, s_dockCrates, s_marketWood, s_marketCloth, s_marketGoods;

        /// <summary>The Dock: a plank pier on posts, a crane with a hanging load, crates (also the ghost).</summary>
        internal static Mesh[] DockParts()
        {
            if (s_dockWood == null)
            {
                var wb = new MeshBuilder();
                Box(wb, new Vector3(0.6f, 0.55f, 0f), new Vector3(3.0f, 0.12f, 2.2f));            // the deck
                for (int i = 0; i < 6; i++)
                    wb.Log(new Vector3(-2.0f + (i % 3) * 2.2f, -0.6f, i < 3 ? -1.9f : 1.9f), 0.16f, 1.25f, 0f, i * 0.1f, Vector3.zero);
                wb.Log(new Vector3(-1.8f, 0.6f, -1.4f), 0.14f, 3.6f, 0f, 0.2f, Vector3.zero);     // the crane's mast
                Box(wb, new Vector3(-0.6f, 4.1f, -1.4f), new Vector3(1.3f, 0.09f, 0.09f));         // its arm
                s_dockWood = wb.Build("Dock wood");
                var cb = new MeshBuilder();
                Box(cb, new Vector3(1.6f, 0.95f, 1.2f), new Vector3(0.35f, 0.3f, 0.35f));
                Box(cb, new Vector3(2.3f, 0.95f, 1.0f), new Vector3(0.3f, 0.3f, 0.3f));
                Box(cb, new Vector3(1.9f, 1.5f, 1.1f), new Vector3(0.28f, 0.25f, 0.28f));
                Box(cb, new Vector3(0.6f, 3.0f, -1.4f), new Vector3(0.3f, 0.3f, 0.3f));            // the hanging load
                s_dockCrates = cb.Build("Dock crates");
            }
            return new[] { s_dockWood, s_dockCrates };
        }

        static void DrawDock(LandBuilding b, Matrix4x4 m, Matrix4x4 grow, Color team, Material bm)
        {
            bool vik = Vikings(b);
            if (!DrawKit(b, grow, team, bm))
            {
                var parts = DockParts();
                Emit(parts[0], Mat(TierTex(b, "palisade_bark"), "palisade_team", team, bm, 0.3f), grow, true);
                Emit(parts[1], Mat(vik ? "shield_viking" : "shield_native", vik ? "shield_viking_team" : "shield_native_team", team, bm, 0.4f), grow, true);
            }
            if (!b.isComplete()) return;
            // the repair reach (DockAura: 14 m), a slow blue ring every 2 s
            float phase = (Time.time % 2f) / 2f;
            s_ringBlock.Clear();
            s_ringBlock.SetColor("_BaseColor", new Color(0.5f, 0.8f, 1f, (1f - phase) * 0.3f));
            Graphics.RenderMesh(new RenderParams(Pulse()) { matProps = s_ringBlock, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off },
                Ring(), 0, Matrix4x4.TRS(m.MultiplyPoint3x4(new Vector3(0, 0.25f, 0)), Quaternion.identity, new Vector3(1, 1, 1) * Mathf.Lerp(2f, 14f, phase)));
        }

        /// <summary>The Market: a counter under striped awnings, baskets of goods (also the ghost).</summary>
        internal static Mesh[] MarketParts()
        {
            if (s_marketWood == null)
            {
                var wb = new MeshBuilder();
                Box(wb, new Vector3(0f, 0.5f, -1.2f), new Vector3(2.2f, 0.5f, 0.45f));            // the counters
                Box(wb, new Vector3(0f, 0.5f, 1.2f), new Vector3(2.2f, 0.5f, 0.45f));
                foreach (var x in new[] { -2.1f, 2.1f })
                    foreach (var z in new[] { -1.5f, 1.5f })
                        wb.Log(new Vector3(x, 0f, z), 0.1f, 2.6f, 0f, 0.3f, Vector3.zero);           // the awning poles
                s_marketWood = wb.Build("Market wood");
                var cl = new MeshBuilder();
                for (int side = -1; side <= 1; side += 2)
                    for (int i = 0; i < 6; i++)
                    {
                        float x0 = -2.3f + i * 0.766f, x1 = x0 + 0.766f;
                        var a = new Vector3(x0, 2.6f, 0f); var bq = new Vector3(x1, 2.6f, 0f);
                        var c = new Vector3(x1, 2.0f, side * 2.0f); var d = new Vector3(x0, 2.0f, side * 2.0f);
                        float u = i % 2 == 0 ? 0.1f : 0.6f;   // alternate stripes: the team mask's two halves
                        if (side > 0) { cl.Quad(a, bq, c, d, new Vector2(u, 0), new Vector2(u + .3f, 0), new Vector2(u + .3f, 1), new Vector2(u, 1)); cl.Quad(d, c, bq, a, new Vector2(u, 1), new Vector2(u + .3f, 1), new Vector2(u + .3f, 0), new Vector2(u, 0)); }
                        else { cl.Quad(bq, a, d, c, new Vector2(u + .3f, 0), new Vector2(u, 0), new Vector2(u, 1), new Vector2(u + .3f, 1)); cl.Quad(c, d, a, bq, new Vector2(u + .3f, 1), new Vector2(u, 1), new Vector2(u, 0), new Vector2(u + .3f, 0)); }
                    }
                s_marketCloth = cl.Build("Market awnings");
                var gb = new MeshBuilder();
                for (int i = 0; i < 4; i++)
                {
                    Box(gb, new Vector3(-1.5f + i, 1.15f, -1.2f), new Vector3(0.25f, 0.15f, 0.25f), 1.2f);
                    Box(gb, new Vector3(-1.5f + i, 1.15f, 1.2f), new Vector3(0.25f, 0.15f, 0.25f), 1.2f);
                }
                s_marketGoods = gb.Build("Market goods");
            }
            return new[] { s_marketWood, s_marketCloth, s_marketGoods };
        }

        static void DrawMarket(LandBuilding b, Matrix4x4 m, Matrix4x4 grow, Color team, Material bm)
        {
            bool vik = Vikings(b);
            bool kit = DrawKit(b, grow, team, bm);
            if (!kit)
            {
                var parts = MarketParts();
                Emit(parts[0], Mat(TierTex(b, "palisade_bark"), "palisade_team", team, bm, 0.3f), grow, true);
                Emit(parts[1], Mat(vik ? "shield_viking" : "shield_native", vik ? "shield_viking_team" : "shield_native_team", team, bm, 0.2f), grow, true);
                Emit(parts[2], Mat("shrine_stone", null, team, bm, 0.6f), grow, true);
            }
            if (!b.isComplete()) return;
            float gy = kit ? 1.1f : 1.38f;   // on the gold heaps
            // a little gold glinting on the counters
            float t = Time.time;
            s_block.Clear();
            s_block.SetColor("_BaseColor", new Color(1.4f, 1.05f, 0.35f) * (1.2f + 0.4f * Mathf.Sin(t * 2f)));
            var rp = new RenderParams(Glow()) { matProps = s_block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off };
            Graphics.RenderMesh(rp, Orb(), 0, m * Matrix4x4.TRS(new Vector3(0.5f, gy, -1.2f), Quaternion.identity, new Vector3(0.18f, 0.08f, 0.18f)));
            Graphics.RenderMesh(rp, Orb(), 0, m * Matrix4x4.TRS(new Vector3(-0.5f, gy, 1.2f), Quaternion.identity, new Vector3(0.18f, 0.08f, 0.18f)));
        }

        static void Box(MeshBuilder mb, Vector3 c, Vector3 half, float taper = 1f)
        {
            Vector3 B(float x, float z) => c + new Vector3(x * half.x, -half.y, z * half.z);
            Vector3 T(float x, float z) => c + new Vector3(x * half.x * taper, half.y, z * half.z * taper);
            float vh = half.y * 2f / 2.5f;
            mb.Quad(B(-1, 1), B(1, 1), T(1, 1), T(-1, 1), new Vector2(0, 0), new Vector2(.5f, 0), new Vector2(.5f, vh), new Vector2(0, vh));
            mb.Quad(B(1, -1), B(-1, -1), T(-1, -1), T(1, -1), new Vector2(0, 0), new Vector2(.5f, 0), new Vector2(.5f, vh), new Vector2(0, vh));
            mb.Quad(B(1, 1), B(1, -1), T(1, -1), T(1, 1), new Vector2(0, 0), new Vector2(.5f, 0), new Vector2(.5f, vh), new Vector2(0, vh));
            mb.Quad(B(-1, -1), B(-1, 1), T(-1, 1), T(-1, -1), new Vector2(0, 0), new Vector2(.5f, 0), new Vector2(.5f, vh), new Vector2(0, vh));
            mb.Quad(T(-1, 1), T(1, 1), T(1, -1), T(-1, -1), new Vector2(0, .8f), new Vector2(.5f, .8f), new Vector2(.5f, 1f), new Vector2(0, 1f));
        }

        /// <summary>The Forge's parts in its local frame: stone walls and chimney, the roof, the anvil.</summary>
        internal static Mesh[] ForgeParts()
        {
            if (s_forgeWalls == null)
            {
                var wb = new MeshBuilder();
                // three walls of a smithy, open at the front (+x) where the anvil stands
                Box(wb, new Vector3(-0.6f, 0.9f, 0f), new Vector3(0.3f, 1.1f, 2.2f));
                Box(wb, new Vector3(0.6f, 0.9f, -2.0f), new Vector3(1.5f, 1.1f, 0.3f));
                Box(wb, new Vector3(0.6f, 0.9f, 2.0f), new Vector3(1.5f, 1.1f, 0.3f));
                // the hearth and its chimney at the back
                Box(wb, new Vector3(-1.6f, 1.6f, 0f), new Vector3(0.7f, 1.8f, 0.8f), 0.8f);
                Box(wb, new Vector3(-1.6f, 4.0f, 0f), new Vector3(0.45f, 0.8f, 0.5f), 0.85f);
                s_forgeWalls = wb.Build("Forge walls");
                var rb = new MeshBuilder();
                // a lean-to roof of planks over the walls
                var a = new Vector3(-0.9f, 2.5f, -2.5f); var b = new Vector3(-0.9f, 2.5f, 2.5f);
                var c = new Vector3(2.4f, 1.9f, 2.5f); var d = new Vector3(2.4f, 1.9f, -2.5f);
                rb.Quad(a, b, c, d, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1));
                rb.Quad(d, c, b, a, new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0));
                s_forgeRoof = rb.Build("Forge roof");
                var ib = new MeshBuilder();
                // the anvil on a stump, out front
                Box(ib, new Vector3(1.4f, 0.3f, 0f), new Vector3(0.35f, 0.5f, 0.35f));
                Box(ib, new Vector3(1.4f, 0.95f, 0f), new Vector3(0.28f, 0.15f, 0.55f), 0.9f);
                Box(ib, new Vector3(1.4f, 0.95f, 0.75f), new Vector3(0.12f, 0.1f, 0.25f), 0.4f);
                s_forgeIron = ib.Build("Forge anvil");
            }
            return new[] { s_forgeWalls, s_forgeRoof, s_forgeIron };
        }

        static void DrawForge(LandBuilding b, Matrix4x4 m, Matrix4x4 grow, Color team, Material bm)
        {
            bool vik = Vikings(b);
            bool kit = DrawKit(b, grow, team, bm);
            if (!kit)
            {
                var parts = ForgeParts();
                Emit(parts[0], Mat(TierTex(b, "shrine_stone"), "shrine_team", team, bm, 0.5f), grow, true);
                Emit(parts[1], Mat(vik ? "shield_viking" : "shield_native", vik ? "shield_viking_team" : "shield_native_team", team, bm, 0.3f), grow, true);
                Emit(parts[2], Mat("palisade_bark", null, team, bm, 0.8f), grow, true);
            }
            if (!b.isComplete()) return;
            // where the fire, the worked crystal and the chimney are on this model
            Vector3 mouth, mouthSize, crystal, chimney;
            if (!kit) { mouth = new Vector3(-1.0f, 0.9f, 0f); mouthSize = new Vector3(0.25f, 0.45f, 0.5f); crystal = new Vector3(1.4f, 1.3f, 0f); chimney = new Vector3(-1.6f, 4.8f, 0f); }
            else if (vik) { mouth = new Vector3(-1.0f, 0.86f, 0f); mouthSize = new Vector3(0.42f, 0.09f, 0.6f); crystal = new Vector3(0.75f, 1.3f, 0.35f); chimney = new Vector3(-1.55f, 4.45f, 0f); }   // (the coal bed under the hood)
            else { mouth = new Vector3(-0.47f, 0.55f, 0f); mouthSize = new Vector3(0.12f, 0.36f, 0.34f); crystal = new Vector3(1.2f, 0.98f, 0.3f); chimney = new Vector3(-1.45f, 3.35f, 0f); }
            float t = Time.time, pulse = 0.5f + 0.5f * Mathf.Sin(t * 3.7f) * Mathf.Sin(t * 1.3f);
            var glow = vik ? new Color(1f, 0.55f, 0.18f) : new Color(0.35f, 0.9f, 1.2f);
            s_block.Clear();
            s_block.SetColor("_BaseColor", glow * (2.2f + pulse * 1.6f));
            var rp = new RenderParams(Glow()) { matProps = s_block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off };
            // the hearth's mouth and the crystal on the anvil
            Graphics.RenderMesh(rp, Orb(), 0, m * Matrix4x4.TRS(mouth, Quaternion.identity, mouthSize * (1f + pulse * 0.1f)));
            Graphics.RenderMesh(rp, Orb(), 0, m * Matrix4x4.TRS(crystal, Quaternion.Euler(0, t * 40f, 0), new Vector3(0.14f, 0.3f, 0.14f)));
            // sparks from the chimney: a few embers rising
            for (int i = 0; i < 4; i++)
            {
                float ph = (t * 0.6f + i * 0.25f) % 1f;
                var at = chimney + new Vector3(Mathf.Sin(i * 2.1f + t) * 0.2f, ph * 2.4f, Mathf.Cos(i * 1.7f + t) * 0.2f);
                s_block.SetColor("_BaseColor", new Color(1f, 0.55f, 0.15f) * (2.5f * (1f - ph)));
                Graphics.RenderMesh(new RenderParams(Glow()) { matProps = s_block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off },
                    Orb(), 0, m * Matrix4x4.TRS(at, Quaternion.identity, Vector3.one * 0.07f));
            }
        }

        /// <summary>The War Totem's pole (Natives), also the ghost while placing.</summary>
        internal static Mesh WarTotemPost()
        {
            if (s_warTotem != null) return s_warTotem;
            var mb = new MeshBuilder();
            // three carved drums stacked on a short post
            mb.Log(new Vector3(0, -0.2f, 0), 0.32f, 1.0f, 0f, 0.0f, Vector3.zero);
            mb.Log(new Vector3(0, 0.8f, 0), 0.38f, 1.0f, 0f, 0.35f, Vector3.zero);
            mb.Log(new Vector3(0, 1.8f, 0), 0.34f, 1.0f, 0f, 0.7f, Vector3.zero);
            mb.Log(new Vector3(0, 2.8f, 0), 0.4f, 0.55f, 0.25f, 0.2f, Vector3.zero);
            return s_warTotem = mb.Build("War totem");
        }

        static Mesh TotemWings()
        {
            if (s_totemWings != null) return s_totemWings;
            var mb = new MeshBuilder();
            for (int side = -1; side <= 1; side += 2)
            {
                var root0 = new Vector3(side * 0.35f, 2.55f, 0f);
                var root1 = new Vector3(side * 0.35f, 3.1f, 0f);
                var tip0 = new Vector3(side * 1.6f, 3.4f, 0f);
                var tip1 = new Vector3(side * 1.3f, 2.7f, 0f);
                var off = new Vector3(0, 0, 0.05f);
                if (side > 0)
                {
                    mb.Quad(root0 + off, tip1 + off, tip0 + off, root1 + off, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1));
                    mb.Quad(root1 - off, tip0 - off, tip1 - off, root0 - off, new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0));
                }
                else
                {
                    mb.Quad(tip1 + off, root0 + off, root1 + off, tip0 + off, new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1));
                    mb.Quad(tip0 - off, root1 - off, root0 - off, tip1 - off, new Vector2(1, 1), new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0));
                }
            }
            return s_totemWings = mb.Build("War totem wings");
        }

        static Mesh RuneStone()
        {
            if (s_runeStone != null) return s_runeStone;
            var mb = new MeshBuilder();
            Box(mb, new Vector3(0, 1.4f, 0), new Vector3(0.55f, 1.6f, 0.3f), 0.7f);
            Box(mb, new Vector3(0, -0.05f, 0), new Vector3(0.8f, 0.2f, 0.55f), 0.9f);
            return s_runeStone = mb.Build("Rune stone");
        }

        static void DrawWarTotem(LandBuilding b, Matrix4x4 m, Matrix4x4 grow, Color team, Material bm)
        {
            bool vik = Vikings(b);
            bool kit = DrawKit(b, grow, team, bm);
            if (kit) { }
            else if (vik)
                Emit(RuneStone(), Mat(TierTex(b, "shrine_stone"), "shrine_team", team, bm, 0.5f), grow, true);
            else
            {
                Emit(WarTotemPost(), Mat(TierTex(b, "palisade_bark"), "palisade_team", team, bm, 0.3f), grow, true);
                Emit(TotemWings(), Mat("shield_native", "shield_native_team", team, bm, 0.3f), grow, true);
            }
            if (!b.isComplete()) return;
            float t = Time.time, pulse = 0.5f + 0.5f * Mathf.Sin(t * 2.6f);
            var glow = vik ? new Color(0.4f, 0.9f, 1.4f) : new Color(1.2f, 0.45f, 0.25f);
            s_block.Clear();
            s_block.SetColor("_BaseColor", glow * (1.8f + pulse * 1.4f));
            var rp = new RenderParams(Glow()) { matProps = s_block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off };
            if (vik)
            {
                // the runes: a column of small glowing marks on the stone's face
                for (int i = 0; i < 4; i++)
                    {
                        float ry = 0.7f + i * 0.55f;
                        // on the kit stone the runes run up its painted serpent (both faces)
                        float rz = kit ? 0.36f * Mathf.Sin((ry + 0.2f) / 3.35f * Mathf.PI * 3f) : 0f;
                        Graphics.RenderMesh(rp, Orb(), 0, m * Matrix4x4.TRS(new Vector3(kit ? 0.25f - 0.02f * ry : 0.3f, ry, rz), Quaternion.identity, new Vector3(0.05f, 0.16f, 0.1f)));
                        if (kit) Graphics.RenderMesh(rp, Orb(), 0, m * Matrix4x4.TRS(new Vector3(-0.25f + 0.02f * ry, ry, rz), Quaternion.identity, new Vector3(0.05f, 0.16f, 0.1f)));
                    }
            }
            else
                Graphics.RenderMesh(rp, Orb(), 0, m * Matrix4x4.TRS(new Vector3(0, 3.55f, 0), Quaternion.identity, Vector3.one * (0.3f + pulse * 0.05f)));
            // the aura's reach (TotemAura: 7 cells), pulsing every 2 s
            float phase = (t % 2f) / 2f;
            s_ringBlock.Clear();
            s_ringBlock.SetColor("_BaseColor", glow * new Color(1, 1, 1, (1f - phase) * 0.35f));
            var center = m.MultiplyPoint3x4(new Vector3(0, 0.25f, 0));
            float radius = Mathf.Lerp(1.5f, 14f, phase);
            Graphics.RenderMesh(new RenderParams(Pulse()) { matProps = s_ringBlock, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off },
                Ring(), 0, Matrix4x4.TRS(center, Quaternion.identity, new Vector3(radius, 1, radius)));
        }
    }
}
