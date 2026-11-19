using UnityEngine;
using com.oddlabs.tt.model;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// TT2 bridges (BridgeSpan): each span is 3 x 3 cells (6 m) of plank deck on four piles, its top 0.45 m above the
    /// water (Model.BRIDGE_DECK, where the walkers stand). Spans side by side make one bridge. While it is built the
    /// piles stand first, then the deck goes down plank by plank.
    /// </summary>
    public static partial class TTExtendedArt
    {
        static Mesh s_bridgeDeck, s_bridgePiles;
        const float BridgeHalf = 3f;   // metres: half a span (1.5 cells either side of the centre cell)

        internal static Mesh BridgeDeck()
        {
            if (s_bridgeDeck != null) return s_bridgeDeck;
            var mb = new MeshBuilder();
            const float top = 0.45f, bottom = 0.2f;
            const int planks = 8;
            for (int i = 0; i < planks; i++)
            {
                float z0 = -BridgeHalf + i * (2f * BridgeHalf / planks) + 0.03f, z1 = z0 + 2f * BridgeHalf / planks - 0.06f;
                float h = top + ((i * 7) % 3) * 0.015f;   // a little uneven
                var a = new Vector3(-BridgeHalf, h, z0); var b = new Vector3(BridgeHalf, h, z0);
                var c = new Vector3(BridgeHalf, h, z1); var d = new Vector3(-BridgeHalf, h, z1);
                float u = i * 0.13f;
                mb.Quad(a, d, c, b, new Vector2(u, 0), new Vector2(u, 0.25f), new Vector2(u + 2f, 0.25f), new Vector2(u + 2f, 0));
                // the plank's edges, so it has some thickness
                mb.Quad(new Vector3(-BridgeHalf, bottom, z0), a, b, new Vector3(BridgeHalf, bottom, z0),
                    new Vector2(u, 0), new Vector2(u, 0.05f), new Vector2(u + 2f, 0.05f), new Vector2(u + 2f, 0));
                mb.Quad(new Vector3(BridgeHalf, bottom, z1), c, d, new Vector3(-BridgeHalf, bottom, z1),
                    new Vector2(u, 0), new Vector2(u, 0.05f), new Vector2(u + 2f, 0.05f), new Vector2(u + 2f, 0));
            }
            // two beams under the planks along the other way
            foreach (float x in new[] { -BridgeHalf + 0.4f, BridgeHalf - 0.4f })
            {
                mb.Quad(new Vector3(x - 0.15f, bottom - 0.25f, -BridgeHalf), new Vector3(x - 0.15f, bottom - 0.25f, BridgeHalf),
                    new Vector3(x - 0.15f, bottom, BridgeHalf), new Vector3(x - 0.15f, bottom, -BridgeHalf),
                    new Vector2(0, 0), new Vector2(2, 0), new Vector2(2, 0.1f), new Vector2(0, 0.1f));
                mb.Quad(new Vector3(x + 0.15f, bottom - 0.25f, BridgeHalf), new Vector3(x + 0.15f, bottom - 0.25f, -BridgeHalf),
                    new Vector3(x + 0.15f, bottom, -BridgeHalf), new Vector3(x + 0.15f, bottom, BridgeHalf),
                    new Vector2(0, 0), new Vector2(2, 0), new Vector2(2, 0.1f), new Vector2(0, 0.1f));
            }
            return s_bridgeDeck = mb.Build("Bridge deck");
        }

        internal static Mesh BridgePiles()
        {
            if (s_bridgePiles != null) return s_bridgePiles;
            var mb = new MeshBuilder();
            float p = BridgeHalf - 0.45f;
            int k = 0;
            foreach (float x in new[] { -p, p })
                foreach (float z in new[] { -p, p })
                    mb.Log(new Vector3(x, -3f, z), 0.2f, 3.6f, 0.15f, (k++) * 0.27f, Vector3.zero);
            return s_bridgePiles = mb.Build("Bridge piles");
        }

        static void DrawBridge(LandBuilding b, Matrix4x4 m, float progress, Color team, Material baseMaterial)
        {
            var wood = Mat(Vikings(b) ? "bk_planks" : "bk_darkwood", null, team, baseMaterial, 0.25f);
            var bark = Mat("palisade_bark", null, team, baseMaterial, 0.2f);
            if (b.getOwner().isElemental()) { wood = Mat(ElementalTex("bk_planks"), null, team, baseMaterial, 0.25f); }
            var mw = Matrix4x4.Translate((Vector3)m.GetColumn(3));   // along the grid, whichever way the site faces
            Emit(BridgePiles(), bark, mw, true);
            if (progress <= 0.15f) return;
            // the deck goes down from one side to the other as it is built
            float k = Mathf.Clamp01((progress - 0.15f) / 0.85f);
            var deck = mw * Matrix4x4.Translate(new Vector3(0, 0, -BridgeHalf * (1f - k))) * Matrix4x4.Scale(new Vector3(1f, 1f, Mathf.Max(0.05f, k)));
            Emit(BridgeDeck(), wood, deck, true);
            if (progress < 1f) return;
            // a cell of water the span bridged beyond its own (BridgeSpan.open): planks out to the shore, or halfway
            // to the next span
            var g = b.getUnitGrid();
            int cx = b.getGridX(), cy = b.getGridY(), size = g.getGridSize();
            for (int d = 0; d < 4; d++)
            {
                int sx = d == 0 ? 1 : d == 1 ? -1 : 0, sy = d == 2 ? 1 : d == 3 ? -1 : 0;
                int gx = cx + 2 * sx, gy = cy + 2 * sy, nx = cx + 3 * sx, ny = cy + 3 * sy;
                if (gx < 0 || gy < 0 || gx >= size || gy >= size || !g.isBridgeDeck(gx, gy)) continue;
                if (g.getOccupant(gx, gy, com.oddlabs.tt.pathfinder.UnitGrid.SEA) is LandBuilding) continue;   // the next span's own cell
                bool toSpan = nx >= 0 && ny >= 0 && nx < size && ny < size && g.getOccupant(nx, ny, com.oddlabs.tt.pathfinder.UnitGrid.SEA) is LandBuilding;
                float len = toSpan ? 1f : 2.2f, mid = BridgeHalf + len * 0.5f;
                var ext = mw * Matrix4x4.Translate(new Vector3(sx * mid, 0, sy * mid))
                    * Matrix4x4.Scale(sx != 0 ? new Vector3(len / (2f * BridgeHalf), 1f, 1f) : new Vector3(1f, 1f, len / (2f * BridgeHalf)));
                Emit(BridgeDeck(), wood, ext, true);
            }
        }
    }
}
