// Translated from MapAnalysis.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 24
    public class MapAnalysis : java.lang.Object
    {
        public const int UNREACHABLE = 2147483647;
        public int n;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public com.oddlabs.tt.landscape.HeightMap heightmap;
        public bool[][] access;
        public java.util.List regions;
        public int home_x;
        public int home_y;
        public int enemy_x;
        public int enemy_y;
        public int[] dist_home;
        public int[] dist_enemy;
        public int[] tree_sat;
        public float tree_sat_time;
        // Java: 73
        public static com.oddlabs.tt.player.Player nearestEnemy(com.oddlabs.tt.player.Player owner)
        {
            com.oddlabs.tt.player.Player best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (!owner.isEnemy(p))
                {
                    continue;
                }
                float dx = (float)(p.getStartX() - owner.getStartX());
                float dy = (float)(p.getStartY() - owner.getStartY());
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < best_d)
                {
                    best_d = d;
                    best = p;
                }
            }
            return best;
        }
        // Java: 90
        public int clamp(int g)
        {
            return java.lang.Math.max(1, java.lang.Math.min(n - 2, g));
        }
        // Java: 94
        public bool inside(int x, int y)
        {
            return x >= 0 && y >= 0 && x < n && y < n;
        }
        // Java: 98
        public bool isAccessible(int x, int y)
        {
            return inside(x, y) && access[y][x];
        }
        // Java: 102
        public int homeDist(int x, int y)
        {
            return (inside(x, y) ? dist_home[y * n + x] : com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE);
        }
        // Java: 106
        public int enemyDist(int x, int y)
        {
            return (inside(x, y) ? dist_enemy[y * n + x] : com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE);
        }
        // Java: 114
        public float frontness(int x, int y)
        {
            int h = homeDist(x, y);
            int e = enemyDist(x, y);
            if (h == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE || e == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE || h + e == 0)
            {
                return 0.5f;
            }
            return (float)(h / ((float)((h + e))));
        }
        // Java: 123
        public int[] distanceField(int src_x, int src_y)
        {
            return distanceField(src_x, src_y, java.lang.Integer.MAX_VALUE, null);
        }
        // Java: 131
        public int[] distanceField(int src_x, int src_y, int max_cost, int[] reuse)
        {
            int[] dist = (reuse != null && reuse.Length == n * n ? reuse : new int[n * n]);
            java.util.Arrays.fill(dist, com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE);
            int[] start = nearestAccessible(src_x, src_y);
            if (start == null)
            {
                return dist;
            }
            com.oddlabs.tt.player.ultra.MapAnalysis.IntBuckets buckets = new com.oddlabs.tt.player.ultra.MapAnalysis.IntBuckets(4);
            int s = start[1] * n + start[0];
            dist[s] = 0;
            buckets.add(0, s);
            int cur = 0;
            int pending = 1;
            while (pending > 0)
            {
                int idx = buckets.poll(cur);
                if (idx < 0)
                {
                    cur++;
                    continue;
                }
                pending--;
                int d = dist[idx];
                if (d != cur)
                {
                    continue;
                }
                if (d > max_cost)
                {
                    break;
                }
                int x = idx % n;
                int y = idx / n;
                for (int dy = (-1); dy <= 1; dy++)
                {
                    for (int dx = (-1); dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0)
                        {
                            continue;
                        }
                        int nx = x + dx;
                        int ny = y + dy;
                        if (!isAccessible(nx, ny))
                        {
                            continue;
                        }
                        int nd = d + ((dx != 0 && dy != 0 ? 3 : 2));
                        int nidx = ny * n + nx;
                        if (nd < dist[nidx])
                        {
                            dist[nidx] = nd;
                            buckets.add(nd, nidx);
                            pending++;
                        }
                    }
                }
            }
            return dist;
        }
        // Java: 178
        public int[] nearestAccessible(int x, int y)
        {
            for (int r = 0; r < n; r++)
            {
                for (int dy = -r; dy <= r; dy++)
                {
                    for (int dx = -r; dx <= r; dx++)
                    {
                        if (java.lang.Math.max(java.lang.Math.abs(dx), java.lang.Math.abs(dy)) != r)
                        {
                            continue;
                        }
                        if (isAccessible(x + dx, y + dy))
                        {
                            return new int[] { x + dx, y + dy };
                        }
                    }
                }
            }
            return null;
        }
        // Java: 196
        public void refreshTrees(float now, float max_age)
        {
            if ((float)(now - tree_sat_time) < max_age)
            {
                return;
            }
            tree_sat_time = now;
            int w = n + 1;
            for (int y = 0; y < n; y++)
            {
                int row = 0;
                for (int x = 0; x < n; x++)
                {
                    if ((grid.getOccupant(x, y) is com.oddlabs.tt.landscape.TreeSupply))
                    {
                        row++;
                    }
                    tree_sat[(y + 1) * w + (x + 1)] = tree_sat[y * w + (x + 1)] + row;
                }
            }
        }
        // Java: 212
        public int treesAround(int x, int y, int r)
        {
            int x0 = java.lang.Math.max(0, x - r);
            int y0 = java.lang.Math.max(0, y - r);
            int x1 = java.lang.Math.min(n - 1, x + r);
            int y1 = java.lang.Math.min(n - 1, y + r);
            if (x0 > x1 || y0 > y1)
            {
                return 0;
            }
            int w = n + 1;
            return tree_sat[(y1 + 1) * w + (x1 + 1)] - tree_sat[y0 * w + (x1 + 1)] - tree_sat[(y1 + 1) * w + x0] + tree_sat[y0 * w + x0];
        }
        // Java: 226
        public java.util.List ironSupplies()
        {
            return collect(typeof(com.oddlabs.tt.model.IronSupply));
        }
        // Java: 231
        public java.util.List rockSupplies()
        {
            return collect(typeof(com.oddlabs.tt.model.RockSupply));
        }
        // Java: 237
        public java.util.List freeChickens()
        {
            java.util.List all = collect(typeof(com.oddlabs.tt.model.RubberSupply));
            java.util.List result = new java.util.ArrayList(all.size());
            foreach (com.oddlabs.tt.model.RubberSupply c in java.lang.JIter.of(all))
            {
                if (!c.isDead() && !c.isHit())
                {
                    result.add(c);
                }
            }
            return result;
        }
        // Java: 248
        public java.util.List collect(System.Type type)
        {
            java.util.List result = new java.util.ArrayList();
            foreach (com.oddlabs.tt.pathfinder.Region r in java.lang.JIter.of(regions))
            {
                foreach (object o in java.lang.JIter.of(r.getObjects(type)))
                {
                    if (type.isInstance(o) && !(((com.oddlabs.tt.pathfinder.Occupant)o)).isDead())
                    {
                        result.add(type.cast(o));
                    }
                }
            }
            return result;
        }
        // Java: 260
        public float height(int x, int y)
        {
            if (!inside(x, y))
            {
                return 0.0f;
            }
            return heightmap.getHeight(x, y);
        }
        // Java: 266
        public static int octile(int dx, int dy)
        {
            int ax = java.lang.Math.abs(dx);
            int ay = java.lang.Math.abs(dy);
            return 2 * java.lang.Math.max(ax, ay) + java.lang.Math.min(ax, ay);
        }
        // Java: 272
        public static int dist2(int ax, int ay, int bx, int by)
        {
            int dx = ax - bx;
            int dy = ay - by;
            return dx * dx + dy * dy;
        }
        // Java: 278
        public static int chebyshev(int ax, int ay, int bx, int by)
        {
            return java.lang.Math.max(java.lang.Math.abs(ax - bx), java.lang.Math.abs(ay - by));
        }
        // Java: 283
        public class IntBuckets : java.lang.Object
        {
            public int[][] data;
            public int[] size;
            // Java: 292
            public void add(int cost, int @value)
            {
                int b = cost % data.Length;
                if (size[b] == data[b].Length)
                {
                    data[b] = java.util.Arrays.copyOf(data[b], size[b] * 2);
                }
                data[b][size[b]++] = @value;
            }
            // Java: 299
            public int poll(int cost)
            {
                int b = cost % data.Length;
                if (size[b] == 0)
                {
                    return (-1);
                }
                return data[b][--size[b]];
            }
            public IntBuckets(int count) : base()
            {
                data = java.lang.JA.newArray2<int>(count, 64);
                size = new int[count];
            }
        }
        private void init_MapAnalysis()
        {
            this.tree_sat_time = java.lang.Float.NEGATIVE_INFINITY;
        }
        public MapAnalysis(com.oddlabs.tt.player.Player owner) : base()
        {
            init_MapAnalysis();
            this.grid = owner.getWorld().getUnitGrid();
            this.heightmap = grid.getHeightMap();
            this.n = grid.getGridSize();
            this.access = heightmap.getAccessGrid();
            this.home_x = clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartX()));
            this.home_y = clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartY()));
            com.oddlabs.tt.player.Player enemy = com.oddlabs.tt.player.ultra.MapAnalysis.nearestEnemy(owner);
            if (enemy != null)
            {
                this.enemy_x = clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartX()));
                this.enemy_y = clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartY()));
            }
            else
            {
                this.enemy_x = n - 1 - home_x;
                this.enemy_y = n - 1 - home_y;
            }
            this.dist_home = distanceField(home_x, home_y);
            this.dist_enemy = distanceField(enemy_x, enemy_y);
            this.tree_sat = new int[(n + 1) * (n + 1)];
            java.util.LinkedHashSet region_set = new java.util.LinkedHashSet();
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    com.oddlabs.tt.pathfinder.Region r = grid.getRegion(x, y);
                    if (r != null)
                    {
                        region_set.add(r);
                    }
                }
            }
            this.regions = new java.util.ArrayList(region_set);
        }
    }
}
