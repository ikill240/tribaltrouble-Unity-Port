// Translated from MapAnalysis.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 24
    public class MapAnalysis : java.lang.Object
    {
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public com.oddlabs.tt.landscape.HeightMap heightmap;
        public bool[][] access;
        public int size;
        public const int TREE_BUCKET = 8;
        public const int ORE_BUCKET = 32;
        public int tree_buckets_side;
        public com.oddlabs.tt.player.ai.MapAnalysis.Scan scan_f;
        // Java: 43
        public class Scan : java.lang.Object
        {
            public com.oddlabs.tt.player.ai.MapAnalysis.SupplyList iron;
            public com.oddlabs.tt.player.ai.MapAnalysis.SupplyList rocks;
            public com.oddlabs.tt.player.ai.MapAnalysis.SupplyList trees;
            public int[] tree_sums;
            public Scan(com.oddlabs.tt.player.ai.MapAnalysis.SupplyList iron, com.oddlabs.tt.player.ai.MapAnalysis.SupplyList rocks, com.oddlabs.tt.player.ai.MapAnalysis.SupplyList trees, int[] tree_sums) : base()
            {
                this.iron = iron;
                this.rocks = rocks;
                this.trees = trees;
                this.tree_sums = tree_sums;
            }
        }
        // Java: 62
        public class SupplyList : java.util.ArrayList
        {
            public int cell_f;
            public int side_f;
            public java.util.List buckets;
            // Java: 76
            public void put(com.oddlabs.tt.model.Supply supply)
            {
                add(supply);
                ((java.util.List)buckets.get((supply.getGridY() / cell_f) * side_f + supply.getGridX() / cell_f)).add(supply);
            }
            // Java: 81
            public int side()
            {
                return side_f;
            }
            // Java: 85
            public int cell()
            {
                return cell_f;
            }
            // Java: 90
            public int ringDistance(int ring)
            {
                return java.lang.Math.max(0, ring * cell_f - (cell_f - 1));
            }
            // Java: 94
            public java.util.List bucket(int bx, int by)
            {
                return ((java.util.List)buckets.get(by * side_f + bx));
            }
            public SupplyList(int cell_v, int grid_size) : base()
            {
                this.cell_f = cell_v;
                this.side_f = (grid_size + cell_v - 1) / cell_v;
                this.buckets = new java.util.ArrayList(side_f * side_f);
                for (int i = 0; i < side_f * side_f; i++)
                {
                    buckets.add(new java.util.ArrayList());
                }
            }
        }
        // Java: 105
        public static java.util.List near(java.util.List supplies, int x, int y, int radius)
        {
            if (!((supplies is com.oddlabs.tt.player.ai.MapAnalysis.SupplyList list)))
            {
                return supplies;
            }
            int side = list.side();
            int cell = list.cell();
            int bx0 = java.lang.Math.max(0, (x - radius) / cell);
            int by0 = java.lang.Math.max(0, (y - radius) / cell);
            int bx1 = java.lang.Math.min(side - 1, (x + radius) / cell);
            int by1 = java.lang.Math.min(side - 1, (y + radius) / cell);
            java.util.List @out = new java.util.ArrayList();
            for (int by = by0; by <= by1; by++)
            {
                for (int bx = bx0; bx <= bx1; bx++)
                {
                    @out.addAll(list.bucket(bx, by));
                }
            }
            return @out;
        }
        public static java.lang.@ref.WeakReference last_world;
        public static int last_tick;
        public static java.lang.@ref.WeakReference last_scan;
        // Java: 136
        public int getSize()
        {
            return size;
        }
        // Java: 140
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 145
        public com.oddlabs.tt.pathfinder.UnitGrid getGrid()
        {
            return grid;
        }
        // Java: 150
        public java.util.List getIron()
        {
            return scan_f.iron;
        }
        // Java: 154
        public java.util.List getRocks()
        {
            return scan_f.rocks;
        }
        // Java: 158
        public java.util.List getTrees()
        {
            return scan_f.trees;
        }
        // Java: 162
        public bool inside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < size && y < size;
        }
        // Java: 166
        public float height(int x, int y)
        {
            return heightmap.getHeight(java.lang.Math.clamp(x, 0, size - 1), java.lang.Math.clamp(y, 0, size - 1));
        }
        // Java: 173
        public void scanSupplies()
        {
            scan_f = scan();
        }
        // Java: 182
        public com.oddlabs.tt.player.ai.MapAnalysis.Scan scan()
        {
            int tick = world.getTick();
            com.oddlabs.tt.player.ai.MapAnalysis.Scan last = ((com.oddlabs.tt.player.ai.MapAnalysis.Scan)com.oddlabs.tt.player.ai.MapAnalysis.last_scan.get());
            if (last == null || ((object)((com.oddlabs.tt.landscape.World)com.oddlabs.tt.player.ai.MapAnalysis.last_world.get()) != (object)world) || com.oddlabs.tt.player.ai.MapAnalysis.last_tick != tick)
            {
                last = walkGrid();
            }
            else if (com.oddlabs.tt.player.ai.MapAnalysis.anyEmptied(last))
            {
                last = withoutEmptied(last);
            }
            else
            {
                return last;
            }
            com.oddlabs.tt.player.ai.MapAnalysis.last_world = new java.lang.@ref.WeakReference(world);
            com.oddlabs.tt.player.ai.MapAnalysis.last_tick = tick;
            com.oddlabs.tt.player.ai.MapAnalysis.last_scan = new java.lang.@ref.WeakReference(last);
            return last;
        }
        // Java: 198
        public com.oddlabs.tt.player.ai.MapAnalysis.Scan walkGrid()
        {
            com.oddlabs.tt.player.ai.MapAnalysis.SupplyList iron = new com.oddlabs.tt.player.ai.MapAnalysis.SupplyList(com.oddlabs.tt.player.ai.MapAnalysis.ORE_BUCKET, size);
            com.oddlabs.tt.player.ai.MapAnalysis.SupplyList rocks = new com.oddlabs.tt.player.ai.MapAnalysis.SupplyList(com.oddlabs.tt.player.ai.MapAnalysis.ORE_BUCKET, size);
            com.oddlabs.tt.player.ai.MapAnalysis.SupplyList trees = new com.oddlabs.tt.player.ai.MapAnalysis.SupplyList(com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET, size);
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
                            trees.put(tree);
                            row++;
                        }
                    }
                    else if ((occ is com.oddlabs.tt.model.IronSupply supply))
                    {
                        if (!supply.isEmpty() && supply.getGridX() == x && supply.getGridY() == y)
                        {
                            iron.put(supply);
                        }
                    }
                    else if ((occ is com.oddlabs.tt.model.RockSupply supply_2))
                    {
                        if (!supply_2.isEmpty() && supply_2.getGridX() == x && supply_2.getGridY() == y)
                        {
                            rocks.put(supply_2);
                        }
                    }
                    sums[(y + 1) * stride + x + 1] = sums[y * stride + x + 1] + row;
                }
            }
            return new com.oddlabs.tt.player.ai.MapAnalysis.Scan(iron, rocks, trees, sums);
        }
        // Java: 226
        public static bool anyEmptied(com.oddlabs.tt.player.ai.MapAnalysis.Scan scan_v)
        {
            foreach (com.oddlabs.tt.landscape.TreeSupply t in java.lang.JIter.of(scan_v.trees))
            {
                if (t.isEmpty())
                {
                    return true;
                }
            }
            foreach (com.oddlabs.tt.model.IronSupply s in java.lang.JIter.of(scan_v.iron))
            {
                if (s.isEmpty())
                {
                    return true;
                }
            }
            foreach (com.oddlabs.tt.model.RockSupply s_2 in java.lang.JIter.of(scan_v.rocks))
            {
                if (s_2.isEmpty())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 239
        public com.oddlabs.tt.player.ai.MapAnalysis.Scan withoutEmptied(com.oddlabs.tt.player.ai.MapAnalysis.Scan from)
        {
            com.oddlabs.tt.player.ai.MapAnalysis.SupplyList iron = new com.oddlabs.tt.player.ai.MapAnalysis.SupplyList(com.oddlabs.tt.player.ai.MapAnalysis.ORE_BUCKET, size);
            foreach (com.oddlabs.tt.model.IronSupply s in java.lang.JIter.of(from.iron))
            {
                if (!s.isEmpty())
                {
                    iron.put(s);
                }
            }
            com.oddlabs.tt.player.ai.MapAnalysis.SupplyList rocks = new com.oddlabs.tt.player.ai.MapAnalysis.SupplyList(com.oddlabs.tt.player.ai.MapAnalysis.ORE_BUCKET, size);
            foreach (com.oddlabs.tt.model.RockSupply s_2 in java.lang.JIter.of(from.rocks))
            {
                if (!s_2.isEmpty())
                {
                    rocks.put(s_2);
                }
            }
            com.oddlabs.tt.player.ai.MapAnalysis.SupplyList trees = new com.oddlabs.tt.player.ai.MapAnalysis.SupplyList(com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET, size);
            foreach (com.oddlabs.tt.landscape.TreeSupply t in java.lang.JIter.of(from.trees))
            {
                if (!t.isEmpty())
                {
                    trees.put(t);
                }
            }
            int stride = size + 1;
            int[] sums = new int[stride * stride];
            int next = 0;
            for (int y = 0; y < size; y++)
            {
                int row = 0;
                for (int x = 0; x < size; x++)
                {
                    while (next < trees.size() && ((com.oddlabs.tt.landscape.TreeSupply)trees.get(next)).getGridY() == y && ((com.oddlabs.tt.landscape.TreeSupply)trees.get(next)).getGridX() == x)
                    {
                        row++;
                        next++;
                    }
                    sums[(y + 1) * stride + x + 1] = sums[y * stride + x + 1] + row;
                }
            }
            return new com.oddlabs.tt.player.ai.MapAnalysis.Scan(iron, rocks, trees, sums);
        }
        // Java: 273
        public float averageTreeDistance(int x, int y, int count, int radius, float missing)
        {
            return averageTreeDistance(x, y, count, radius, missing, null);
        }
        // Java: 282
        public float averageTreeDistance(int x, int y, int count, int radius, float missing, com.oddlabs.tt.player.ai.DistanceField field)
        {
            int site = (field != null ? field.getAround(x, y, 1) : com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE);
            if (site == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE)
            {
                field = null;
            }
            float[] best = new float[count];
            java.util.Arrays.fill(best, java.lang.Float.MAX_VALUE);
            int bx0 = java.lang.Math.max(0, (x - radius) / com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET);
            int by0 = java.lang.Math.max(0, (y - radius) / com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET);
            int bx1 = java.lang.Math.min(tree_buckets_side - 1, (x + radius) / com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET);
            int by1 = java.lang.Math.min(tree_buckets_side - 1, (y + radius) / com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET);
            int r2 = radius * radius;
            if (!inside(x, y))
            {
                for (int by = by0; by <= by1; by++)
                {
                    for (int bx = bx0; bx <= bx1; bx++)
                    {
                        com.oddlabs.tt.player.ai.MapAnalysis.nearestTrees(scan_f.trees.bucket(bx, by), x, y, r2, field, site, best);
                    }
                }
            }
            else
            {
                int cbx = x / com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET;
                int cby = y / com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET;
                int rings = java.lang.Math.max(java.lang.Math.max(cbx - bx0, bx1 - cbx), java.lang.Math.max(cby - by0, by1 - cby));
                java.util.List ring = new java.util.ArrayList();
                for (int r = 0; r <= rings; r++)
                {
                    int l = scan_f.trees.ringDistance(r);
                    if ((float)(((float)(java.lang.Math.sqrt(l * l))) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID) >= best[count - 1])
                    {
                        break;
                    }
                    com.oddlabs.tt.player.ai.MapAnalysis.ringBuckets(scan_f.trees, cbx, cby, r, bx0, by0, bx1, by1, ring);
                    foreach (java.util.List bucket in java.lang.JIter.of(ring))
                    {
                        com.oddlabs.tt.player.ai.MapAnalysis.nearestTrees(bucket, x, y, r2, field, site, best);
                    }
                }
            }
            float sum = 0.0f;
            foreach (float d in best)
            {
                sum = (float)(sum + (d == java.lang.Float.MAX_VALUE ? missing : d));
            }
            return (float)(sum / count);
        }
        // Java: 319
        public static void nearestTrees(java.util.List bucket, int x, int y, int r2, com.oddlabs.tt.player.ai.DistanceField field, int site, float[] best)
        {
            int count = best.Length;
            foreach (com.oddlabs.tt.landscape.TreeSupply t in java.lang.JIter.of(bucket))
            {
                if (t.isEmpty())
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                if (d2 > r2)
                {
                    continue;
                }
                float d = (float)(((float)(java.lang.Math.sqrt(d2))) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
                if (field != null && d < best[count - 1])
                {
                    int walk = field.getAround(t.getGridX(), t.getGridY(), 1);
                    d = (walk == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE ? java.lang.Float.MAX_VALUE : java.lang.Math.max(d, java.lang.Math.abs(walk - site)));
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
        // Java: 345
        public static void ringBuckets(com.oddlabs.tt.player.ai.MapAnalysis.SupplyList list, int cbx, int cby, int r, int bx0, int by0, int bx1, int by1, java.util.List @out)
        {
            @out.clear();
            if (r == 0)
            {
                if (cbx >= bx0 && cbx <= bx1 && cby >= by0 && cby <= by1)
                {
                    @out.add(list.bucket(cbx, cby));
                }
                return;
            }
            int x0 = java.lang.Math.max(bx0, cbx - r);
            int x1 = java.lang.Math.min(bx1, cbx + r);
            for (int side = (-1); side <= 1; side += 2)
            {
                int by = cby + side * r;
                if (by < by0 || by > by1)
                {
                    continue;
                }
                for (int bx = x0; bx <= x1; bx++)
                {
                    @out.add(list.bucket(bx, by));
                }
            }
            int y0 = java.lang.Math.max(by0, cby - r + 1);
            int y1 = java.lang.Math.min(by1, cby + r - 1);
            for (int side_2 = (-1); side_2 <= 1; side_2 += 2)
            {
                int bx_2 = cbx + side_2 * r;
                if (bx_2 < bx0 || bx_2 > bx1)
                {
                    continue;
                }
                for (int by_2 = y0; by_2 <= y1; by_2++)
                {
                    @out.add(list.bucket(bx_2, by_2));
                }
            }
        }
        // Java: 376
        public int treesAround(int x, int y, int radius)
        {
            int stride = size + 1;
            int x0 = java.lang.Math.clamp(x - radius, 0, size);
            int y0 = java.lang.Math.clamp(y - radius, 0, size);
            int x1 = java.lang.Math.clamp(x + radius + 1, 0, size);
            int y1 = java.lang.Math.clamp(y + radius + 1, 0, size);
            int[] tree_sums = scan_f.tree_sums;
            return tree_sums[y1 * stride + x1] - tree_sums[y0 * stride + x1] - tree_sums[y1 * stride + x0] + tree_sums[y0 * stride + x0];
        }
        // Java: 386
        public static int countStanding(java.util.List supplies)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (!s.isEmpty())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 397
        public bool passable(int x, int y)
        {
            return passable(x, y, null);
        }
        // Java: 402
        public bool passable(int x, int y, com.oddlabs.tt.pathfinder.Occupant ignore)
        {
            if (!inside(x, y) || !access[y][x])
            {
                return false;
            }
            com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
            return occ == null || ((object)occ == (object)ignore) || (occ is com.oddlabs.tt.pathfinder.Movable) || occ.getPenalty() != com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 410
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
        // Java: 429
        public bool canPlace(com.oddlabs.tt.model.BuildingTemplate template, int x, int y)
        {
            return inside(x, y) && template.isPlacingLegal(grid, x, y);
        }
        // Java: 437
        public com.oddlabs.tt.player.ai.DistanceField computeField(int sx, int sy, int max_cost)
        {
            return computeField(new int[] { sx }, new int[] { sy }, max_cost);
        }
        // Java: 446
        public com.oddlabs.tt.player.ai.DistanceField computeField(int[] xs, int[] ys, int max_cost)
        {
            int reach = (max_cost >= 4 * size ? size : (max_cost + 3) / 2 + 1);
            int wx0 = size;
            int wy0 = size;
            int wx1 = (-1);
            int wy1 = (-1);
            for (int i = 0; i < xs.Length; i++)
            {
                if (!inside(xs[i], ys[i]))
                {
                    continue;
                }
                wx0 = java.lang.Math.min(wx0, xs[i]);
                wy0 = java.lang.Math.min(wy0, ys[i]);
                wx1 = java.lang.Math.max(wx1, xs[i]);
                wy1 = java.lang.Math.max(wy1, ys[i]);
            }
            if (wx1 < 0)
            {
                return new com.oddlabs.tt.player.ai.DistanceField(size, xs[0], ys[0], 0, 0, 0, 0);
            }
            wx0 = java.lang.Math.max(0, wx0 - reach);
            wy0 = java.lang.Math.max(0, wy0 - reach);
            wx1 = java.lang.Math.min(size - 1, wx1 + reach);
            wy1 = java.lang.Math.min(size - 1, wy1 + reach);
            int w = wx1 - wx0 + 1;
            int h = wy1 - wy0 + 1;
            com.oddlabs.tt.player.ai.DistanceField field = new com.oddlabs.tt.player.ai.DistanceField(size, xs[0], ys[0], wx0, wy0, w, h);
            int[] cost = field.raw();
            int[][] buckets = new int[4][];
            int[] counts = new int[4];
            for (int i_2 = 0; i_2 < 4; i_2++)
            {
                buckets[i_2] = new int[java.lang.Math.max(256, xs.Length)];
            }
            com.oddlabs.tt.pathfinder.Occupant source_occupant = (xs.Length == 1 && inside(xs[0], ys[0]) ? grid.getOccupant(xs[0], ys[0]) : (com.oddlabs.tt.pathfinder.Occupant)(null));
            if (source_occupant != null && source_occupant.getPenalty() != com.oddlabs.tt.pathfinder.Occupant.STATIC)
            {
                source_occupant = null;
            }
            bool[] open = new bool[w * h];
            for (int ly = 0; ly < h; ly++)
            {
                for (int lx = 0; lx < w; lx++)
                {
                    open[ly * w + lx] = passable(wx0 + lx, wy0 + ly, source_occupant);
                }
            }
            for (int i_3 = 0; i_3 < xs.Length; i_3++)
            {
                if (!inside(xs[i_3], ys[i_3]))
                {
                    continue;
                }
                int index = (ys[i_3] - wy0) * w + xs[i_3] - wx0;
                if (cost[index] == 0)
                {
                    continue;
                }
                cost[index] = 0;
                buckets[0][counts[0]++] = index;
            }
            if (counts[0] == 0)
            {
                return field;
            }
            int current = 0;
            int empty_rounds = 0;
            int[] work = new int[256];
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
                if (work.Length < n)
                {
                    work = new int[java.lang.Math.max(n, work.Length * 2)];
                }
                java.lang.JSystem.arraycopy(bucket, 0, work, 0, n);
                for (int i_4 = 0; i_4 < n; i_4++)
                {
                    int index_2 = work[i_4];
                    if (cost[index_2] != current)
                    {
                        continue;
                    }
                    int lx_2 = index_2 % w;
                    int ly_2 = index_2 / w;
                    for (int dy = (-1); dy <= 1; dy++)
                    {
                        for (int dx = (-1); dx <= 1; dx++)
                        {
                            if (dx == 0 && dy == 0)
                            {
                                continue;
                            }
                            int nx = lx_2 + dx;
                            int ny = ly_2 + dy;
                            if (nx < 0 || ny < 0 || nx >= w || ny >= h || !open[ny * w + nx])
                            {
                                continue;
                            }
                            if (dx != 0 && dy != 0 && (!open[ly_2 * w + nx] || !open[ny * w + lx_2]))
                            {
                                continue;
                            }
                            int step = (dx != 0 && dy != 0 ? 3 : 2);
                            int next = current + step;
                            int nindex = ny * w + nx;
                            if (next < cost[nindex])
                            {
                                cost[nindex] = next;
                                int nb = next & 3;
                                if (counts[nb] == buckets[nb].Length)
                                {
                                    int[] grown = new int[buckets[nb].Length * 2];
                                    java.lang.JSystem.arraycopy(buckets[nb], 0, grown, 0, counts[nb]);
                                    buckets[nb] = grown;
                                }
                                buckets[nb][counts[nb]++] = nindex;
                            }
                        }
                    }
                }
                current++;
            }
            if (reach == size)
            {
                field.compact();
            }
            return field;
        }
        // Java: 558
        public static int dist2(int x0, int y0, int x1, int y1)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            return dx * dx + dy * dy;
        }
        // Java: 565
        public static float meters(int x0, int y0, int x1, int y1)
        {
            return (float)(((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ai.MapAnalysis.dist2(x0, y0, x1, y1)))) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
        }
        static MapAnalysis()
        {
            MapAnalysis.last_world = new java.lang.@ref.WeakReference(null);
            MapAnalysis.last_tick = (-1);
            MapAnalysis.last_scan = new java.lang.@ref.WeakReference(null);
        }
        public MapAnalysis(com.oddlabs.tt.landscape.World world) : base()
        {
            this.world = world;
            this.grid = world.getUnitGrid();
            this.heightmap = world.getHeightMap();
            this.access = heightmap.getAccessGrid();
            this.size = grid.getGridSize();
            this.tree_buckets_side = (size + com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET - 1) / com.oddlabs.tt.player.ai.MapAnalysis.TREE_BUCKET;
            this.scan_f = scan();
        }
    }
}
