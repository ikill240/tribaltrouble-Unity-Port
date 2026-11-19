// Translated from MapAnalysis.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 24
    public class MapAnalysis : java.lang.Object
    {
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public com.oddlabs.tt.landscape.HeightMap heightmap;
        public bool[][] access;
        public int size;
        public java.util.List iron;
        public java.util.List rocks;
        public java.util.List trees;
        public const int TREE_BUCKET = 8;
        public int tree_buckets_side;
        public java.util.List tree_buckets;
        public int[] tree_sums;
        // Java: 55
        public int getSize()
        {
            return size;
        }
        // Java: 59
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 64
        public com.oddlabs.tt.pathfinder.UnitGrid getGrid()
        {
            return grid;
        }
        // Java: 69
        public java.util.List getIron()
        {
            return iron;
        }
        // Java: 73
        public java.util.List getRocks()
        {
            return rocks;
        }
        // Java: 77
        public java.util.List getTrees()
        {
            return trees;
        }
        // Java: 81
        public bool inside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < size && y < size;
        }
        // Java: 85
        public float height(int x, int y)
        {
            return heightmap.getHeight(java.lang.Math.clamp(x, 0, size - 1), java.lang.Math.clamp(y, 0, size - 1));
        }
        // Java: 92
        public void scanSupplies()
        {
            iron.clear();
            rocks.clear();
            trees.clear();
            foreach (java.util.List bucket in java.lang.JIter.of(tree_buckets))
            {
                bucket.clear();
            }
            int stride = size + 1;
            int[] sums = new int[stride * stride];
            for (int y = 0; y < size; y++)
            {
                int row = 0;
                for (int x = 0; x < size; x++)
                {
                    com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
                    if ((occ is com.oddlabs.tt.landscape.TreeSupply tree))
                    {
                        if (!tree.isEmpty() && tree.getGridX() == x && tree.getGridY() == y)
                        {
                            trees.add(tree);
                            ((java.util.List)tree_buckets.get((y / com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET) * tree_buckets_side + x / com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET)).add(tree);
                            row++;
                        }
                    }
                    else if ((occ is com.oddlabs.tt.model.IronSupply supply))
                    {
                        if (!supply.isEmpty() && supply.getGridX() == x && supply.getGridY() == y)
                        {
                            iron.add(supply);
                        }
                    }
                    else if ((occ is com.oddlabs.tt.model.RockSupply supply_2))
                    {
                        if (!supply_2.isEmpty() && supply_2.getGridX() == x && supply_2.getGridY() == y)
                        {
                            rocks.add(supply_2);
                        }
                    }
                    sums[(y + 1) * stride + x + 1] = sums[y * stride + x + 1] + row;
                }
            }
            tree_sums = sums;
        }
        // Java: 127
        public float averageTreeDistance(int x, int y, int count, int radius, float missing)
        {
            return averageTreeDistance(x, y, count, radius, missing, null);
        }
        // Java: 136
        public float averageTreeDistance(int x, int y, int count, int radius, float missing, com.oddlabs.tt.player.gauntlet.DistanceField field)
        {
            int site = (field != null ? field.getAround(x, y, 1) : com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE);
            if (site == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
            {
                field = null;
            }
            float[] best = new float[count];
            java.util.Arrays.fill(best, java.lang.Float.MAX_VALUE);
            int bx0 = java.lang.Math.max(0, (x - radius) / com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET);
            int by0 = java.lang.Math.max(0, (y - radius) / com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET);
            int bx1 = java.lang.Math.min(tree_buckets_side - 1, (x + radius) / com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET);
            int by1 = java.lang.Math.min(tree_buckets_side - 1, (y + radius) / com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET);
            int r2 = radius * radius;
            for (int by = by0; by <= by1; by++)
            {
                for (int bx = bx0; bx <= bx1; bx++)
                {
                    java.util.List bucket = ((java.util.List)tree_buckets.get(by * tree_buckets_side + bx));
                    foreach (com.oddlabs.tt.landscape.TreeSupply t in java.lang.JIter.of(bucket))
                    {
                        if (t.isEmpty())
                        {
                            continue;
                        }
                        int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                        if (d2 > r2)
                        {
                            continue;
                        }
                        float d = (float)(((float)(java.lang.Math.sqrt(d2))) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
                        if (field != null && d < best[count - 1])
                        {
                            int walk = field.getAround(t.getGridX(), t.getGridY(), 1);
                            d = (walk == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE ? java.lang.Float.MAX_VALUE : java.lang.Math.max(d, java.lang.Math.abs(walk - site)));
                        }
                        if (d < best[count - 1])
                        {
                            int i = count - 1;
                            while (i > 0 && best[i - 1] > d)
                            {
                                best[i] = best[i - 1];
                                i--;
                            }
                            best[i] = d;
                        }
                    }
                }
            }
            float sum = 0.0f;
            foreach (float d_2 in best)
            {
                sum = (float)(sum + (d_2 == java.lang.Float.MAX_VALUE ? missing : d_2));
            }
            return (float)(sum / count);
        }
        // Java: 181
        public int treesAround(int x, int y, int radius)
        {
            int stride = size + 1;
            int x0 = java.lang.Math.clamp(x - radius, 0, size);
            int y0 = java.lang.Math.clamp(y - radius, 0, size);
            int x1 = java.lang.Math.clamp(x + radius + 1, 0, size);
            int y1 = java.lang.Math.clamp(y + radius + 1, 0, size);
            return tree_sums[y1 * stride + x1] - tree_sums[y0 * stride + x1] - tree_sums[y1 * stride + x0] + tree_sums[y0 * stride + x0];
        }
        // Java: 193
        public bool passable(int x, int y)
        {
            return passable(x, y, null);
        }
        // Java: 198
        public bool walkable(int x, int y)
        {
            return inside(x, y) && access[y][x];
        }
        // Java: 203
        public bool passable(int x, int y, com.oddlabs.tt.pathfinder.Occupant ignore)
        {
            if (!inside(x, y) || !access[y][x])
            {
                return false;
            }
            com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
            return occ == null || ((object)occ == (object)ignore) || (occ is com.oddlabs.tt.pathfinder.Movable) || occ.getPenalty() != com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 211
        public int[] highGround(int x, int y, int radius)
        {
            int bx = x;
            int by = y;
            float best = (float)(height(x, y) + 1.0f);
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    if (dx * dx + dy * dy > radius * radius || !passable(x + dx, y + dy))
                    {
                        continue;
                    }
                    float h = height(x + dx, y + dy);
                    if (h > best)
                    {
                        best = h;
                        bx = x + dx;
                        by = y + dy;
                    }
                }
            }
            return new int[] { bx, by };
        }
        // Java: 230
        public bool canPlace(com.oddlabs.tt.model.BuildingTemplate template, int x, int y)
        {
            return inside(x, y) && template.isPlacingLegal(grid, x, y);
        }
        // Java: 238
        public com.oddlabs.tt.player.gauntlet.DistanceField computeField(int sx, int sy, int max_cost)
        {
            return computeField(new int[] { sx }, new int[] { sy }, max_cost);
        }
        // Java: 247
        public com.oddlabs.tt.player.gauntlet.DistanceField computeField(int[] xs, int[] ys, int max_cost)
        {
            com.oddlabs.tt.player.gauntlet.DistanceField field = new com.oddlabs.tt.player.gauntlet.DistanceField(size, xs[0], ys[0]);
            int[] cost = field.raw();
            int[][] buckets = new int[4][];
            int[] counts = new int[4];
            for (int i = 0; i < 4; i++)
            {
                buckets[i] = new int[java.lang.Math.max(256, xs.Length)];
            }
            com.oddlabs.tt.pathfinder.Occupant source_occupant = (xs.Length == 1 && inside(xs[0], ys[0]) ? grid.getOccupant(xs[0], ys[0]) : (com.oddlabs.tt.pathfinder.Occupant)(null));
            if (source_occupant != null && source_occupant.getPenalty() != com.oddlabs.tt.pathfinder.Occupant.STATIC)
            {
                source_occupant = null;
            }
            for (int i_2 = 0; i_2 < xs.Length; i_2++)
            {
                if (!inside(xs[i_2], ys[i_2]) || cost[ys[i_2] * size + xs[i_2]] == 0)
                {
                    continue;
                }
                cost[ys[i_2] * size + xs[i_2]] = 0;
                buckets[0][counts[0]++] = ys[i_2] * size + xs[i_2];
            }
            if (counts[0] == 0)
            {
                return field;
            }
            int call = ++pass_call;
            int current = 0;
            int empty_rounds = 0;
            while (empty_rounds < 4)
            {
                int b = current & 3;
                if (counts[b] == 0)
                {
                    empty_rounds++;
                    current++;
                    continue;
                }
                empty_rounds = 0;
                if (current > max_cost)
                {
                    break;
                }
                int[] bucket = buckets[b];
                int n = counts[b];
                counts[b] = 0;
                if (field_work.Length < n)
                {
                    field_work = new int[java.lang.Math.max(n, 2 * field_work.Length)];
                }
                int[] work = field_work;
                java.lang.JSystem.arraycopy(bucket, 0, work, 0, n);
                int straight = current + 2;
                int diagonal = current + 3;
                for (int i_3 = 0; i_3 < n; i_3++)
                {
                    int index = work[i_3];
                    if (cost[index] != current)
                    {
                        continue;
                    }
                    int x = index % size;
                    int y = index / size;
                    bool west = passableIn(x - 1, y, source_occupant, call);
                    bool east = passableIn(x + 1, y, source_occupant, call);
                    bool north = passableIn(x, y - 1, source_occupant, call);
                    bool south = passableIn(x, y + 1, source_occupant, call);
                    if (west)
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index - 1, straight);
                    }
                    if (east)
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index + 1, straight);
                    }
                    if (north)
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index - size, straight);
                    }
                    if (south)
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index + size, straight);
                    }
                    if (north && west && passableIn(x - 1, y - 1, source_occupant, call))
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index - size - 1, diagonal);
                    }
                    if (north && east && passableIn(x + 1, y - 1, source_occupant, call))
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index - size + 1, diagonal);
                    }
                    if (south && west && passableIn(x - 1, y + 1, source_occupant, call))
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index + size - 1, diagonal);
                    }
                    if (south && east && passableIn(x + 1, y + 1, source_occupant, call))
                    {
                        com.oddlabs.tt.player.gauntlet.MapAnalysis.relax(cost, buckets, counts, index + size + 1, diagonal);
                    }
                }
                current++;
            }
            return field;
        }
        public int[] pass_gen;
        public bool[] pass_ok;
        public int pass_call;
        public int[] field_work;
        // Java: 338
        public static void relax(int[] cost, int[][] buckets, int[] counts, int index, int next)
        {
            if (next >= cost[index])
            {
                return;
            }
            cost[index] = next;
            int b = next & 3;
            if (counts[b] == buckets[b].Length)
            {
                buckets[b] = java.util.Arrays.copyOf(buckets[b], counts[b] * 2);
            }
            buckets[b][counts[b]++] = index;
        }
        // Java: 349
        public bool passableIn(int x, int y, com.oddlabs.tt.pathfinder.Occupant ignore, int call)
        {
            if (!inside(x, y))
            {
                return false;
            }
            int i = y * size + x;
            if (pass_gen[i] == call)
            {
                return pass_ok[i];
            }
            bool ok = passable(x, y, ignore);
            pass_gen[i] = call;
            pass_ok[i] = ok;
            return ok;
        }
        // Java: 361
        public static int dist2(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            return dx * dx + dy * dy;
        }
        // Java: 368
        public static int[] towards(int x, int y, int to_x, int to_y, int cells)
        {
            float dx = to_x - x;
            float dy = to_y - y;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len <= cells)
            {
                return new int[] { to_x, to_y };
            }
            return new int[] { x + java.lang.JC.f2i(((float)((float)(dx / len) * cells))), y + java.lang.JC.f2i(((float)((float)(dy / len) * cells))) };
        }
        // Java: 378
        public static float meters(int x0, int y0, int x1, int y1)
        {
            return (float)(((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x0, y0, x1, y1)))) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
        }
        // Java: 383
        public static int[] centroid(java.util.List units)
        {
            long sx = 0;
            long sy = 0;
            foreach (com.oddlabs.tt.model.Selectable u in java.lang.JIter.of(units))
            {
                sx += u.getGridX();
                sy += u.getGridY();
            }
            int n = java.lang.Math.max(1, units.size());
            return new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))) };
        }
        // Java: 395
        public static com.oddlabs.tt.model.Building nearest(java.util.List buildings, int x, int y)
        {
            com.oddlabs.tt.model.Building best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(buildings))
            {
                if (b.isDead())
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, b.getGridX(), b.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = b;
                }
            }
            return best;
        }
        private void init_MapAnalysis()
        {
            this.iron = new java.util.ArrayList();
            this.rocks = new java.util.ArrayList();
            this.trees = new java.util.ArrayList();
            this.tree_buckets = new java.util.ArrayList();
            this.field_work = new int[256];
        }
        public MapAnalysis(com.oddlabs.tt.landscape.World world) : base()
        {
            init_MapAnalysis();
            this.world = world;
            this.grid = world.getUnitGrid();
            this.heightmap = world.getHeightMap();
            this.access = heightmap.getAccessGrid();
            this.size = grid.getGridSize();
            this.tree_sums = new int[(size + 1) * (size + 1)];
            this.pass_gen = new int[size * size];
            this.pass_ok = new bool[size * size];
            this.tree_buckets_side = (size + com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET - 1) / com.oddlabs.tt.player.gauntlet.MapAnalysis.TREE_BUCKET;
            for (int i = 0; i < tree_buckets_side * tree_buckets_side; i++)
            {
                tree_buckets.add(new java.util.ArrayList());
            }
            scanSupplies();
        }
    }
}
