// Translated from BridgeSpan.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 17
    public class BridgeSpan : java.lang.Object
    {
        public const int HALF = 1;
        public const float SIZE = 5.0f;
        // Java: 24
        public static bool isBridge(com.oddlabs.tt.model.Building b)
        {
            return (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) is com.oddlabs.tt.model.BuildingTemplate t) && t.getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_BRIDGE;
        }
        // Java: 29
        public static bool isPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid grid, int gx, int gy)
        {
            if (!grid.getHeightMap().getWorld().isExtended())
            {
                return false;
            }
            int size = grid.getGridSize();
            com.oddlabs.tt.pathfinder.Occupant unreachable = com.oddlabs.tt.pathfinder.RegionBuilder.unreachable();
            int water = 0;
            for (int dy = -com.oddlabs.tt.model.BridgeSpan.HALF; dy <= com.oddlabs.tt.model.BridgeSpan.HALF; dy++)
            {
                for (int dx = -com.oddlabs.tt.model.BridgeSpan.HALF; dx <= com.oddlabs.tt.model.BridgeSpan.HALF; dx++)
                {
                    int x = gx + dx;
                    int y = gy + dy;
                    if (x < 1 || y < 1 || x >= size - 1 || y >= size - 1)
                    {
                        return false;
                    }
                    com.oddlabs.tt.pathfinder.Occupant land = grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    com.oddlabs.tt.pathfinder.Occupant sea = grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                    if (grid.getRegion(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND) != null)
                    {
                        if (land != null && !((land is com.oddlabs.tt.model.Unit)))
                        {
                            return false;
                        }
                        continue;
                    }
                    if (grid.isBridgeDeck(x, y) || !grid.isWater(x, y))
                    {
                        return false;
                    }
                    if (land != null && ((object)land != (object)unreachable) || sea != null && ((object)sea != (object)unreachable))
                    {
                        return false;
                    }
                    water++;
                }
            }
            return water > 0;
        }
        // Java: 56
        public static bool isShore(com.oddlabs.tt.pathfinder.UnitGrid grid, int gx, int gy)
        {
            return gx >= 0 && gy >= 0 && gx < grid.getGridSize() && gy < grid.getGridSize() && !grid.isWater(gx, gy) && grid.getRegion(gx, gy) != null;
        }
        // Java: 67
        public static int[][] layout(com.oddlabs.tt.pathfinder.UnitGrid grid, int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int n = java.lang.Math.max(java.lang.Math.abs(dx), java.lang.Math.abs(dy));
            if (n == 0)
            {
                return new int[0][];
            }
            int size = grid.getGridSize();
            int[] cx = new int[n + 1];
            int[] cy = new int[n + 1];
            int first = (-1);
            int last = (-1);
            bool ok = com.oddlabs.tt.model.BridgeSpan.isShore(grid, x0, y0) && com.oddlabs.tt.model.BridgeSpan.isShore(grid, x1, y1);
            for (int k = 0; k <= n; k++)
            {
                cx[k] = java.lang.Math.round((float)(x0 + (float)((float)(dx * ((float)(k))) / n)));
                cy[k] = java.lang.Math.round((float)(y0 + (float)((float)(dy * ((float)(k))) / n)));
                if (cx[k] < 0 || cy[k] < 0 || cx[k] >= size || cy[k] >= size)
                {
                    ok = false;
                    continue;
                }
                if (!grid.isWater(cx[k], cy[k]) || grid.getRegion(cx[k], cy[k]) != null)
                {
                    continue;
                }
                if (last >= 0 && last != k - 1)
                {
                    ok = false;
                }
                if (first < 0)
                {
                    first = k;
                }
                last = k;
            }
            if (first < 0)
            {
                return new int[0][];
            }
            int len = last - first + 1;
            int m = java.lang.Math.max(1, (len + 2) / 4);
            int slack = len - 3 * m;
            int[][] spans = new int[m][];
            bool all = ok;
            for (int i = 0; i < m; i++)
            {
                int k_2 = java.lang.Math.max(0, java.lang.Math.min(n, first + 1 + 3 * i + java.lang.Math.floorDiv((i + 1) * slack, m + 1)));
                bool legal = com.oddlabs.tt.model.BridgeSpan.isPlacingLegal(grid, cx[k_2], cy[k_2]);
                all &= legal;
                spans[i] = new int[] { cx[k_2], cy[k_2], (legal ? 1 : 0) };
            }
            if (!all)
            {
                foreach (int[] sp in spans)
                {
                    sp[2] = 0;
                }
            }
            return spans;
        }
        // Java: 114
        public static com.oddlabs.tt.model.Building enemySpanInReach(com.oddlabs.tt.model.Unit unit)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = unit.getUnitGrid();
            int size = grid.getGridSize();
            int ux = unit.getGridX();
            int uy = unit.getGridY();
            com.oddlabs.tt.model.Building best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            for (int dy = -com.oddlabs.tt.model.BridgeSpan.SCAN; dy <= com.oddlabs.tt.model.BridgeSpan.SCAN; dy++)
            {
                for (int dx = -com.oddlabs.tt.model.BridgeSpan.SCAN; dx <= com.oddlabs.tt.model.BridgeSpan.SCAN; dx++)
                {
                    int x = ux + dx;
                    int y = uy + dy;
                    int d = dx * dx + dy * dy;
                    if (x < 0 || y < 0 || x >= size || y >= size || d >= best_d)
                    {
                        continue;
                    }
                    if ((grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.SEA) is com.oddlabs.tt.model.LandBuilding b) && com.oddlabs.tt.model.BridgeSpan.isBridge(b) && !b.isDead() && unit.getOwner().isEnemy(b.getOwner()) && unit.isCloseEnough(unit.getRange(b), b, com.oddlabs.tt.pathfinder.UnitGrid.SEA))
                    {
                        best = b;
                        best_d = d;
                    }
                }
            }
            return best;
        }
        public const int SCAN = 9;
        // Java: 136
        public static void occupy(com.oddlabs.tt.model.LandBuilding span)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = span.getUnitGrid();
            com.oddlabs.tt.pathfinder.Occupant unreachable = com.oddlabs.tt.pathfinder.RegionBuilder.unreachable();
            int mask = 0;
            int i = 0;
            for (int dy = -com.oddlabs.tt.model.BridgeSpan.HALF; dy <= com.oddlabs.tt.model.BridgeSpan.HALF; dy++)
            {
                for (int dx = -com.oddlabs.tt.model.BridgeSpan.HALF; dx <= com.oddlabs.tt.model.BridgeSpan.HALF; dx++, i++)
                {
                    int x = span.getGridX() + dx;
                    int y = span.getGridY() + dy;
                    com.oddlabs.tt.pathfinder.Occupant sea = grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                    if (((object)sea == (object)unreachable))
                    {
                        mask |= 1 << i;
                    }
                    if (sea == null || ((object)sea == (object)unreachable))
                    {
                        grid.forceOccupant(x, y, span, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                    }
                    if (grid.isWater(x, y))
                    {
                        grid.setSpanBlocked(x, y, true);
                    }
                }
            }
            span.setBridgeSeaMask(mask);
        }
        // Java: 155
        public static void free(com.oddlabs.tt.model.LandBuilding span)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = span.getUnitGrid();
            com.oddlabs.tt.pathfinder.Occupant unreachable = com.oddlabs.tt.pathfinder.RegionBuilder.unreachable();
            int mask = span.bridgeSeaMask();
            int i = 0;
            for (int dy = -com.oddlabs.tt.model.BridgeSpan.HALF; dy <= com.oddlabs.tt.model.BridgeSpan.HALF; dy++)
            {
                for (int dx = -com.oddlabs.tt.model.BridgeSpan.HALF; dx <= com.oddlabs.tt.model.BridgeSpan.HALF; dx++, i++)
                {
                    int x = span.getGridX() + dx;
                    int y = span.getGridY() + dy;
                    if (((object)grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.SEA) == (object)span))
                    {
                        grid.forceOccupant(x, y, ((mask & 1 << i) != 0 ? unreachable : (com.oddlabs.tt.pathfinder.Occupant)(null)), com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                    }
                    grid.setSpanBlocked(x, y, false);
                }
            }
        }
        // Java: 169
        public static void open(com.oddlabs.tt.model.LandBuilding span)
        {
            if (span.bridgeRegion() != null)
            {
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid grid = span.getUnitGrid();
            com.oddlabs.tt.pathfinder.Occupant unreachable = com.oddlabs.tt.pathfinder.RegionBuilder.unreachable();
            com.oddlabs.tt.pathfinder.Region region = new com.oddlabs.tt.pathfinder.Region();
            region.setPosition(span.getGridX(), span.getGridY());
            for (int dy = -com.oddlabs.tt.model.BridgeSpan.HALF; dy <= com.oddlabs.tt.model.BridgeSpan.HALF; dy++)
            {
                for (int dx = -com.oddlabs.tt.model.BridgeSpan.HALF; dx <= com.oddlabs.tt.model.BridgeSpan.HALF; dx++)
                {
                    int x = span.getGridX() + dx;
                    int y = span.getGridY() + dy;
                    if (grid.getRegion(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND) != null || !grid.isWater(x, y))
                    {
                        continue;
                    }
                    if (((object)grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND) == (object)unreachable))
                    {
                        grid.forceOccupant(x, y, null, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    }
                    grid.forceRegion(x, y, region, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    grid.setBridgeDeck(x, y, true);
                }
            }
            int size = grid.getGridSize();
            for (int dy_2 = -com.oddlabs.tt.model.BridgeSpan.HALF - 1; dy_2 <= com.oddlabs.tt.model.BridgeSpan.HALF + 1; dy_2++)
            {
                for (int dx_2 = -com.oddlabs.tt.model.BridgeSpan.HALF - 1; dx_2 <= com.oddlabs.tt.model.BridgeSpan.HALF + 1; dx_2++)
                {
                    if (java.lang.Math.max(java.lang.Math.abs(dx_2), java.lang.Math.abs(dy_2)) != com.oddlabs.tt.model.BridgeSpan.HALF + 1)
                    {
                        continue;
                    }
                    int x_2 = span.getGridX() + dx_2;
                    int y_2 = span.getGridY() + dy_2;
                    int bx = x_2 + java.lang.Integer.signum((java.lang.Math.abs(dx_2) == com.oddlabs.tt.model.BridgeSpan.HALF + 1 ? dx_2 : 0));
                    int by = y_2 + java.lang.Integer.signum((java.lang.Math.abs(dy_2) == com.oddlabs.tt.model.BridgeSpan.HALF + 1 ? dy_2 : 0));
                    if (x_2 < 1 || y_2 < 1 || x_2 >= size - 1 || y_2 >= size - 1 || bx < 0 || by < 0 || bx >= size || by >= size)
                    {
                        continue;
                    }
                    if (!grid.isWater(x_2, y_2) || grid.getRegion(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.LAND) != null || grid.isBridgeDeck(x_2, y_2))
                    {
                        continue;
                    }
                    com.oddlabs.tt.pathfinder.Occupant land = grid.getOccupant(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    com.oddlabs.tt.pathfinder.Occupant sea = grid.getOccupant(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                    if (land != null && ((object)land != (object)unreachable) || sea != null && ((object)sea != (object)unreachable))
                    {
                        continue;
                    }
                    com.oddlabs.tt.pathfinder.Region beyond = grid.getRegion(bx, by, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    if (beyond == null || ((object)beyond == (object)region))
                    {
                        continue;
                    }
                    if (((object)land == (object)unreachable))
                    {
                        grid.forceOccupant(x_2, y_2, null, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    }
                    grid.forceRegion(x_2, y_2, region, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    grid.setBridgeDeck(x_2, y_2, true);
                    grid.setSpanBlocked(x_2, y_2, true);
                }
            }
            for (int dy_3 = -com.oddlabs.tt.model.BridgeSpan.HALF - 2; dy_3 <= com.oddlabs.tt.model.BridgeSpan.HALF + 2; dy_3++)
            {
                for (int dx_3 = -com.oddlabs.tt.model.BridgeSpan.HALF - 2; dx_3 <= com.oddlabs.tt.model.BridgeSpan.HALF + 2; dx_3++)
                {
                    int x_3 = span.getGridX() + dx_3;
                    int y_3 = span.getGridY() + dy_3;
                    if (x_3 < 0 || y_3 < 0 || x_3 >= size || y_3 >= size)
                    {
                        continue;
                    }
                    com.oddlabs.tt.pathfinder.Region r = grid.getRegion(x_3, y_3, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    if (((object)r == (object)region))
                    {
                        continue;
                    }
                    if (com.oddlabs.tt.model.BridgeSpan.touchesDeck(grid, x_3, y_3, region))
                    {
                        com.oddlabs.tt.pathfinder.Region.link(region, r);
                    }
                }
            }
            span.setBridgeRegion(region);
        }
        // Java: 225
        public static bool touchesDeck(com.oddlabs.tt.pathfinder.UnitGrid grid, int x, int y, com.oddlabs.tt.pathfinder.Region region)
        {
            int size = grid.getGridSize();
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
                for (int nx = x - 1; nx <= x + 1; nx++)
                {
                    if (nx >= 0 && ny >= 0 && nx < size && ny < size && ((object)grid.getRegion(nx, ny, com.oddlabs.tt.pathfinder.UnitGrid.LAND) == (object)region))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 235
        public static void close(com.oddlabs.tt.model.LandBuilding span)
        {
            com.oddlabs.tt.pathfinder.Region region = span.bridgeRegion();
            if (region == null)
            {
                return;
            }
            span.setBridgeRegion(null);
            com.oddlabs.tt.pathfinder.UnitGrid grid = span.getUnitGrid();
            com.oddlabs.tt.pathfinder.Occupant unreachable = com.oddlabs.tt.pathfinder.RegionBuilder.unreachable();
            java.util.List drowned = new java.util.ArrayList();
            for (int dy = -com.oddlabs.tt.model.BridgeSpan.HALF - 1; dy <= com.oddlabs.tt.model.BridgeSpan.HALF + 1; dy++)
            {
                for (int dx = -com.oddlabs.tt.model.BridgeSpan.HALF - 1; dx <= com.oddlabs.tt.model.BridgeSpan.HALF + 1; dx++)
                {
                    int x = span.getGridX() + dx;
                    int y = span.getGridY() + dy;
                    if (!grid.isBridgeDeck(x, y) || ((object)grid.getRegion(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND) != (object)region))
                    {
                        continue;
                    }
                    com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    if ((occ is com.oddlabs.tt.model.Selectable s) && !s.isDead() && !drowned.contains(s))
                    {
                        drowned.add(s);
                    }
                }
            }
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(drowned))
            {
                if (!s_2.isDead())
                {
                    s_2.hit(100000, 0.0f, 1.0f, span.getOwner());
                }
            }
            for (int dy_2 = -com.oddlabs.tt.model.BridgeSpan.HALF - 1; dy_2 <= com.oddlabs.tt.model.BridgeSpan.HALF + 1; dy_2++)
            {
                for (int dx_2 = -com.oddlabs.tt.model.BridgeSpan.HALF - 1; dx_2 <= com.oddlabs.tt.model.BridgeSpan.HALF + 1; dx_2++)
                {
                    int x_2 = span.getGridX() + dx_2;
                    int y_2 = span.getGridY() + dy_2;
                    if (!grid.isBridgeDeck(x_2, y_2) || ((object)grid.getRegion(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.LAND) != (object)region))
                    {
                        continue;
                    }
                    if (grid.getOccupant(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.LAND) == null)
                    {
                        grid.forceOccupant(x_2, y_2, unreachable, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    }
                    if (((object)grid.getRegion(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.LAND) == (object)region))
                    {
                        grid.forceRegion(x_2, y_2, null, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                    }
                    grid.setBridgeDeck(x_2, y_2, false);
                    if (((object)grid.getOccupant(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.SEA) != (object)span))
                    {
                        grid.setSpanBlocked(x_2, y_2, false);
                    }
                }
            }
            region.unlinkAll();
        }
        public BridgeSpan() : base()
        {
        }
    }
}
