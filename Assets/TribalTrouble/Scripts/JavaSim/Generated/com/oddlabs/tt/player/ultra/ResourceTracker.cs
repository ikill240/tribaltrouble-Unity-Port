// Translated from ResourceTracker.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 23
    public class ResourceTracker : java.lang.Object
    {
        public int node_cap;
        public const int TREE_CAP = 2;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public java.util.List iron_f;
        public java.util.List rock_f;
        public java.util.Map crowd;
        public float last_scan;
        public bool path_aware;
        // Java: 43
        public void update()
        {
            ctx.map.refreshTrees(ctx.now, 15.0f);
            if ((float)(ctx.now - last_scan) >= 10.0f)
            {
                last_scan = ctx.now;
                iron_f = ctx.map.ironSupplies();
                rock_f = ctx.map.rockSupplies();
            }
            crowd.clear();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if ((com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController gc))
                {
                    com.oddlabs.tt.model.Supply s = gc.getSupply();
                    if (s != null && !s.isDead())
                    {
                        crowd.merge(s, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                    }
                }
            }
        }
        // Java: 60
        public int crowdOf(com.oddlabs.tt.model.Supply s)
        {
            int? c = ((int?)crowd.get(s));
            return (c != null ? ((int)c) : 0);
        }
        // Java: 65
        public void noteAssigned(com.oddlabs.tt.model.Supply s)
        {
            crowd.merge(s, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
        }
        // Java: 69
        public java.util.List iron()
        {
            return iron_f;
        }
        // Java: 74
        public java.util.List rock()
        {
            return rock_f;
        }
        // Java: 83
        public static int remaining(com.oddlabs.tt.model.Supply s)
        {
            if (s.isEmpty())
            {
                return 0;
            }
            return ((s is com.oddlabs.tt.landscape.TreeSupply) || (s is com.oddlabs.tt.model.SupplyModel) ? 10 : 1);
        }
        // Java: 90
        public static int dropDistance(int bx, int by, int nx, int ny)
        {
            int dx = java.lang.Math.max(0, java.lang.Math.abs(nx - bx) - 5);
            int dy = java.lang.Math.max(0, java.lang.Math.abs(ny - by) - 5);
            return com.oddlabs.tt.player.ultra.MapAnalysis.octile(dx, dy);
        }
        public const int FIELD_MAX = 320;
        public int[] drop_field;
        public com.oddlabs.tt.model.Building field_building;
        public int[] tree_walks;
        public float tree_walks_time;
        // Java: 106
        public int[] fieldFor(com.oddlabs.tt.model.Building drop)
        {
            int[] f = drop_field;
            if (((object)drop != (object)field_building) || f == null)
            {
                field_building = drop;
                f = ctx.map.distanceField(drop.getGridX(), drop.getGridY(), com.oddlabs.tt.player.ultra.ResourceTracker.FIELD_MAX, drop_field);
                drop_field = f;
                tree_walks_time = java.lang.Float.NEGATIVE_INFINITY;
            }
            return f;
        }
        // Java: 118
        public int fieldAt(int[] field, int x, int y)
        {
            int best = com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE;
            for (int dy = (-1); dy <= 1; dy++)
            {
                for (int dx = (-1); dx <= 1; dx++)
                {
                    if (ctx.map.inside(x + dx, y + dy))
                    {
                        best = java.lang.Math.min(best, field[(y + dy) * ctx.map.n + x + dx]);
                    }
                }
            }
            return best;
        }
        // Java: 130
        public int walkFrom(int[] field, int bx, int by, int x, int y)
        {
            return walkFrom(field, com.oddlabs.tt.player.ultra.ResourceTracker.FIELD_MAX, bx, by, x, y);
        }
        // Java: 135
        public int walkFrom(int[] field, int max_cost, int bx, int by, int x, int y)
        {
            int straight = com.oddlabs.tt.player.ultra.ResourceTracker.dropDistance(bx, by, x, y);
            if (!path_aware)
            {
                return straight;
            }
            int f = fieldAt(field, x, y);
            if (f == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
            {
                return java.lang.Math.max(straight, max_cost);
            }
            return java.lang.Math.max(straight, f - 10);
        }
        // Java: 146
        public int walkDistance(com.oddlabs.tt.model.Building drop, int x, int y)
        {
            return walkFrom(fieldFor(drop), drop.getGridX(), drop.getGridY(), x, y);
        }
        // Java: 154
        public float treeWalkPath(int[] field, int cx, int cy, int k, int max_r)
        {
            int[] walks = treeWalks(field, cx, cy, max_r);
            return com.oddlabs.tt.player.ultra.ResourceTracker.meanOfFirst(walks, walks.Length, k);
        }
        // Java: 159
        public static float meanOfFirst(int[] sorted, int n, int k)
        {
            float sum = 0.0f;
            int got = java.lang.Math.min(n, k);
            for (int i = 0; i < got; i++)
            {
                sum = (float)(sum + sorted[i]);
            }
            sum = (float)(sum + (float)((k - got) * 80.0f));
            return (float)(sum / k);
        }
        // Java: 168
        public int[] treeWalks(int[] field, int cx, int cy, int max_r)
        {
            int[] buf = new int[64];
            int n = 0;
            for (int y = java.lang.Math.max(0, cy - max_r); y <= java.lang.Math.min(ctx.map.n - 1, cy + max_r); y++)
            {
                for (int x = java.lang.Math.max(0, cx - max_r); x <= java.lang.Math.min(ctx.map.n - 1, cx + max_r); x++)
                {
                    if (ctx.map.treesAround(x, y, 0) == 0)
                    {
                        continue;
                    }
                    if (n == buf.Length)
                    {
                        buf = java.util.Arrays.copyOf(buf, n * 2);
                    }
                    buf[n++] = java.lang.Math.min(80, walkFrom(field, cx, cy, x, y));
                }
            }
            int[] @out = java.util.Arrays.copyOf(buf, n);
            java.util.Arrays.sort(@out);
            return @out;
        }
        // Java: 186
        public int[] armoryTreeWalks(com.oddlabs.tt.model.Building drop)
        {
            int[] f = fieldFor(drop);
            if ((float)(ctx.now - tree_walks_time) >= 15.0f)
            {
                tree_walks_time = ctx.now;
                tree_walks = treeWalks(f, drop.getGridX(), drop.getGridY(), 45);
            }
            return tree_walks;
        }
        public java.util.List avoid_drops;
        // Java: 198
        public void setAvoidDrops(java.util.List drops)
        {
            avoid_drops = drops;
        }
        // Java: 203
        public bool nearerToAvoided(int bx, int by, int x, int y)
        {
            if (avoid_drops.isEmpty())
            {
                return false;
            }
            int own = com.oddlabs.tt.player.ultra.ResourceTracker.dropDistance(bx, by, x, y);
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(avoid_drops))
            {
                if (!b.isDead() && com.oddlabs.tt.player.ultra.ResourceTracker.dropDistance(b.getGridX(), b.getGridY(), x, y) <= own)
                {
                    return true;
                }
            }
            return false;
        }
        public const int BAD_RADIUS = 5;
        public const float BAD_TIME = 180.0f;
        public java.util.List bad_spots;
        // Java: 221
        public void markBad(int x, int y)
        {
            bad_spots.removeIf(new java.util.function.Predicate_L((object b_l) =>
            {
                float[] b = (float[])b_l;
                return ctx.now >= b[2];
            }));
            foreach (float[] b_2 in java.lang.JIter.of(bad_spots))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(java.lang.JC.f2i(b_2[0]), java.lang.JC.f2i(b_2[1]), x, y) <= 1)
                {
                    b_2[2] = (float)(ctx.now + com.oddlabs.tt.player.ultra.ResourceTracker.BAD_TIME);
                    return;
                }
            }
            bad_spots.add(new float[] { x, y, (float)(ctx.now + com.oddlabs.tt.player.ultra.ResourceTracker.BAD_TIME) });
        }
        // Java: 232
        public bool isBad(int x, int y)
        {
            foreach (float[] b in java.lang.JIter.of(bad_spots))
            {
                if (ctx.now < b[2] && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(java.lang.JC.f2i(b[0]), java.lang.JC.f2i(b[1]), x, y) <= com.oddlabs.tt.player.ultra.ResourceTracker.BAD_RADIUS)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 241
        public bool threatened(int x, int y, int r)
        {
            int r2 = r * r;
            com.oddlabs.tt.model.Unit chief = ctx.model.enemy.chieftain;
            if (chief != null && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), x, y) <= r2)
            {
                return true;
            }
            java.util.List ws = ctx.model.enemy.warriors;
            if (threat_version != ctx.model.version || ((object)threat_list != (object)ws) || threat_size != ws.size())
            {
                buildThreatGrid(ws);
            }
            int pad = 2;
            int cx0 = java.lang.Math.max(0, (x - r - pad) >> com.oddlabs.tt.player.ultra.ResourceTracker.THREAT_SHIFT);
            int cy0 = java.lang.Math.max(0, (y - r - pad) >> com.oddlabs.tt.player.ultra.ResourceTracker.THREAT_SHIFT);
            int cx1 = java.lang.Math.min(threat_dim - 1, (x + r + pad) >> com.oddlabs.tt.player.ultra.ResourceTracker.THREAT_SHIFT);
            int cy1 = java.lang.Math.min(threat_dim - 1, (y + r + pad) >> com.oddlabs.tt.player.ultra.ResourceTracker.THREAT_SHIFT);
            for (int cy = cy0; cy <= cy1; cy++)
            {
                for (int cx = cx0; cx <= cx1; cx++)
                {
                    for (int i = threat_head[cy * threat_dim + cx]; i >= 0; i = threat_next[i])
                    {
                        com.oddlabs.tt.model.Unit e = ((com.oddlabs.tt.model.Unit)ws.get(i));
                        if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y) <= r2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public const int THREAT_SHIFT = 4;
        public int threat_dim;
        public int[] threat_head;
        public int[] threat_next;
        public int threat_version;
        public java.util.List threat_list;
        public int threat_size;
        // Java: 278
        public void buildThreatGrid(java.util.List ws)
        {
            threat_dim = (ctx.map.n >> com.oddlabs.tt.player.ultra.ResourceTracker.THREAT_SHIFT) + 1;
            int cells = threat_dim * threat_dim;
            if (threat_head.Length != cells)
            {
                threat_head = new int[cells];
            }
            java.util.Arrays.fill(threat_head, (-1));
            if (threat_next.Length < ws.size())
            {
                threat_next = new int[java.lang.Math.max(16, ws.size() * 2)];
            }
            for (int i = 0; i < ws.size(); i++)
            {
                com.oddlabs.tt.model.Unit e = ((com.oddlabs.tt.model.Unit)ws.get(i));
                int cx = java.lang.Math.max(0, java.lang.Math.min(threat_dim - 1, e.getGridX() >> com.oddlabs.tt.player.ultra.ResourceTracker.THREAT_SHIFT));
                int cy = java.lang.Math.max(0, java.lang.Math.min(threat_dim - 1, e.getGridY() >> com.oddlabs.tt.player.ultra.ResourceTracker.THREAT_SHIFT));
                int c = cy * threat_dim + cx;
                threat_next[i] = threat_head[c];
                threat_head[c] = i;
            }
            threat_version = ctx.model.version;
            threat_list = ws;
            threat_size = ws.size();
        }
        // Java: 303
        public int slotsWithin(System.Type type, com.oddlabs.tt.model.Building drop, int reach, int min_nodes)
        {
            java.util.List list = (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? iron_f : rock_f);
            int[] dist = new int[list.size()];
            int n = 0;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(list))
            {
                if (s.isDead() || threatened(s.getGridX(), s.getGridY(), 12))
                {
                    continue;
                }
                dist[n++] = walkDistance(drop, s.getGridX(), s.getGridY());
            }
            java.util.Arrays.sort(dist, 0, n);
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                if (dist[i] <= reach || i < min_nodes)
                {
                    count++;
                }
            }
            if (n > 0)
            {
                effective_reach[(((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? 0 : 1)] = java.lang.Math.max(reach, dist[java.lang.Math.min(n, min_nodes) - 1]);
            }
            return count * node_cap;
        }
        public int[] effective_reach;
        public int reach_limit;
        // Java: 328
        public com.oddlabs.tt.model.Supply bestNode(System.Type type, com.oddlabs.tt.model.Building drop)
        {
            java.util.List list = (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? iron_f : rock_f);
            com.oddlabs.tt.model.Supply best = null;
            int best_cost = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(list))
            {
                if (s.isDead())
                {
                    continue;
                }
                int c = crowdOf(s);
                if (c >= node_cap)
                {
                    continue;
                }
                int cost = walkDistance(drop, s.getGridX(), s.getGridY()) + 6 * c;
                int eff = effective_reach[(((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? 0 : 1)];
                if (cost > ((eff < 0 ? reach_limit : java.lang.Math.max(reach_limit, eff + 20))))
                {
                    continue;
                }
                if (isBad(s.getGridX(), s.getGridY()) || nearerToAvoided(drop.getGridX(), drop.getGridY(), s.getGridX(), s.getGridY()))
                {
                    continue;
                }
                if (cost < best_cost && !threatened(s.getGridX(), s.getGridY(), 12))
                {
                    best_cost = cost;
                    best = s;
                }
            }
            return best;
        }
        // Java: 355
        public com.oddlabs.tt.landscape.TreeSupply bestTree(int cx, int cy, int min_r, int max_r, int cap)
        {
            com.oddlabs.tt.player.ultra.ResourceTracker.TreeFilter f = new com.oddlabs.tt.player.ultra.ResourceTracker.TreeFilter(this, min_r, max_r, cap);
            ctx.grid.scan(f, cx, cy);
            return f.result;
        }
        // Java: 366
        public com.oddlabs.tt.landscape.TreeSupply bestTreeFrom(com.oddlabs.tt.model.Building drop, int min_r, int max_r, int cap)
        {
            int[] field = fieldFor(drop);
            com.oddlabs.tt.player.ultra.ResourceTracker.TreeFilter f = new com.oddlabs.tt.player.ultra.ResourceTracker.TreeFilter(this, min_r, max_r, cap);
            f.field = field;
            f.bx = drop.getGridX();
            f.by = drop.getGridY();
            ctx.grid.scan(f, f.bx, f.by);
            if (f.result == null && max_r < 45)
            {
                com.oddlabs.tt.player.ultra.ResourceTracker.TreeFilter wide = new com.oddlabs.tt.player.ultra.ResourceTracker.TreeFilter(this, max_r, 45, cap);
                wide.field = field;
                wide.bx = f.bx;
                wide.by = f.by;
                ctx.grid.scan(wide, f.bx, f.by);
                return wide.result;
            }
            return f.result;
        }
        // Java: 385
        public class TreeFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
        {
            public com.oddlabs.tt.player.ultra.ResourceTracker this_0;
            public int min_r;
            public int max_r;
            public int cap;
            public com.oddlabs.tt.landscape.TreeSupply result;
            public int[] field;
            public int bx;
            public int by;
            // Java: 401
            public bool direct(int x, int y)
            {
                int[] f = field;
                if (f == null)
                {
                    return true;
                }
                int straight = com.oddlabs.tt.player.ultra.ResourceTracker.dropDistance(bx, by, x, y);
                return this_0.walkFrom(f, bx, by, x, y) <= straight + straight / 2 + 12;
            }
            // Java: 409
            public int getMinRadius()
            {
                return min_r;
            }
            // Java: 414
            public int getMaxRadius()
            {
                return max_r;
            }
            // Java: 419
            public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
            {
                if ((occ is com.oddlabs.tt.landscape.TreeSupply t) && !t.isDead() && this_0.crowdOf(t) < cap && !this_0.threatened(grid_x, grid_y, 10) && !this_0.isBad(grid_x, grid_y) && (field == null || !this_0.nearerToAvoided(bx, by, grid_x, grid_y)) && direct(grid_x, grid_y))
                {
                    result = t;
                    return true;
                }
                return false;
            }
            public TreeFilter(com.oddlabs.tt.player.ultra.ResourceTracker this_0_, int min_r, int max_r, int cap) : base()
            {
                this.this_0 = this_0_;
                this.min_r = min_r;
                this.max_r = max_r;
                this.cap = cap;
            }
        }
        // Java: 435
        public float tripTime(System.Type type, com.oddlabs.tt.model.Building drop, int g)
        {
            int bx = drop.getGridX();
            int by = drop.getGridY();
            float walk = default;
            if (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
            {
                int[] walks = armoryTreeWalks(drop);
                walk = com.oddlabs.tt.player.ultra.ResourceTracker.meanOfFirst(walks, walks.Length, java.lang.Math.max(4, g / com.oddlabs.tt.player.ultra.ResourceTracker.TREE_CAP + 1));
            }
            else if (((object)type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
            {
                walk = 60.0f;
            }
            else
            {
                java.util.List list = (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? iron_f : rock_f);
                int need = java.lang.Math.max(1, (g + node_cap - 1) / node_cap);
                int[] best = new int[need];
                java.util.Arrays.fill(best, java.lang.Integer.MAX_VALUE);
                foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(list))
                {
                    if (s.isDead())
                    {
                        continue;
                    }
                    int d = walkDistance(drop, s.getGridX(), s.getGridY());
                    for (int k = 0; k < need; k++)
                    {
                        if (d < best[k])
                        {
                            java.lang.JSystem.arraycopy(best, k, best, k + 1, need - k - 1);
                            best[k] = d;
                            break;
                        }
                    }
                }
                long sum = 0;
                int n = 0;
                foreach (int d_2 in best)
                {
                    if (d_2 != java.lang.Integer.MAX_VALUE)
                    {
                        sum += d_2;
                        n++;
                    }
                }
                walk = (n == 0 ? 200.0f : (float)(sum / ((float)(n))));
            }
            return (float)(10.2f + (float)(0.45f * walk));
        }
        // Java: 479
        public float treeWalk(int cx, int cy, int k, int adjacent_r)
        {
            int prev = ctx.map.treesAround(cx, cy, adjacent_r - 2);
            int got = 0;
            float sum = 0;
            for (int r = adjacent_r - 1; r <= 40 && got < k; r++)
            {
                int total = ctx.map.treesAround(cx, cy, r);
                int ring = total - prev;
                prev = total;
                if (ring <= 0)
                {
                    continue;
                }
                int take = java.lang.Math.min(ring, k - got);
                sum = (float)(sum + (float)((float)(take * 2.0f) * java.lang.Math.max(0, r - adjacent_r)));
                got += take;
            }
            if (got < k)
            {
                sum = (float)(sum + (float)((k - got) * 80.0f));
            }
            return (float)(sum / k);
        }
        private void init_ResourceTracker()
        {
            this.iron_f = new java.util.ArrayList();
            this.rock_f = new java.util.ArrayList();
            this.crowd = new java.util.LinkedHashMap();
            this.last_scan = java.lang.Float.NEGATIVE_INFINITY;
            this.tree_walks = new int[0];
            this.tree_walks_time = java.lang.Float.NEGATIVE_INFINITY;
            this.avoid_drops = java.util.List_.of();
            this.bad_spots = new java.util.ArrayList();
            this.threat_head = new int[0];
            this.threat_next = new int[0];
            this.threat_version = (-1);
            this.threat_size = (-1);
            this.effective_reach = new int[] { (-1), (-1) };
            this.reach_limit = java.lang.Integer.MAX_VALUE;
        }
        public ResourceTracker(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_ResourceTracker();
            this.ctx = ctx;
            this.path_aware = ctx.strategy.b("pathAware");
            this.node_cap = ctx.strategy.i("nodeCap");
        }
    }
}
