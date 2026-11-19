// Translated from ResourceMap.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 25
    public class ResourceMap : java.lang.Object
    {
        // Java: 26
        public sealed class Kind : java.lang.Enum
        {
            public static Kind[] values() { return (Kind[])VALUES_.Clone(); }
            public static Kind valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Kind TREE = new Kind("TREE", 0, typeof(com.oddlabs.tt.landscape.TreeSupply));
            public static readonly Kind ROCK = new Kind("ROCK", 1, typeof(com.oddlabs.tt.model.RockSupply));
            public static readonly Kind IRON = new Kind("IRON", 2, typeof(com.oddlabs.tt.model.IronSupply));
            public System.Type supply_class;
            // Java: 37
            public static com.oddlabs.tt.player.fable.ResourceMap.Kind of(System.Type supply_class)
            {
                foreach (com.oddlabs.tt.player.fable.ResourceMap.Kind k in com.oddlabs.tt.player.fable.ResourceMap.Kind.values())
                {
                    if (((object)k.supply_class == (object)supply_class))
                    {
                        return k;
                    }
                }
                return null;
            }
            public static readonly Kind[] VALUES_ = new Kind[] { TREE, ROCK, IRON };
            public Kind(string name_, int ordinal_, System.Type supply_class) : base(name_, ordinal_)
            {
                this.supply_class = supply_class;
            }
        }
        // Java: 46
        public class Node : java.lang.Object
        {
            public com.oddlabs.tt.player.fable.ResourceMap.Kind kind;
            public int grid_x;
            public int grid_y;
            public int island;
            // Java: 60
            public com.oddlabs.tt.model.Supply live(com.oddlabs.tt.pathfinder.UnitGrid grid)
            {
                com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(grid_x, grid_y);
                if (kind.supply_class.isInstance(occ))
                {
                    com.oddlabs.tt.model.Supply s = ((com.oddlabs.tt.model.Supply)occ);
                    return (s.isEmpty() ? (com.oddlabs.tt.model.Supply)(null) : s);
                }
                return null;
            }
            // Java: 69
            public bool isAlive(com.oddlabs.tt.pathfinder.UnitGrid grid)
            {
                return live(grid) != null;
            }
            // Java: 73
            public float distanceTo(int gx, int gy)
            {
                float dx = grid_x - gx;
                float dy = grid_y - gy;
                return ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            }
            // Java: 79
            public int distanceSquaredTo(int gx, int gy)
            {
                int dx = grid_x - gx;
                int dy = grid_y - gy;
                return dx * dx + dy * dy;
            }
            // Java: 85
            public override string toString()
            {
                return java.lang.JStr.valueOf(kind) + "@" + (grid_x) + "," + (grid_y);
            }
            public Node(com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int grid_x, int grid_y, int island) : base()
            {
                this.kind = kind;
                this.grid_x = grid_x;
                this.grid_y = grid_y;
                this.island = island;
            }
        }
        public const int BUCKET_SHIFT = 4;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public int grid_size;
        public int buckets_per_side;
        public java.util.List[][] buckets;
        public java.util.List[] by_kind;
        public int home_island;
        // Java: 135
        public java.util.List bucket(int gx, int gy)
        {
            int bx = gx >> com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT;
            int by = gy >> com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT;
            java.util.List list = buckets[by][bx];
            if (list == null)
            {
                list = new java.util.ArrayList();
                buckets[by][bx] = list;
            }
            return list;
        }
        // Java: 146
        public int getHomeIsland()
        {
            return home_island;
        }
        // Java: 151
        public java.util.List all(com.oddlabs.tt.player.fable.ResourceMap.Kind kind)
        {
            return by_kind[kind.ordinal()];
        }
        // Java: 155
        public int countTotal(com.oddlabs.tt.player.fable.ResourceMap.Kind kind)
        {
            return by_kind[kind.ordinal()].size();
        }
        // Java: 160
        public java.util.List within(com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int gx, int gy, int radius, bool alive_only)
        {
            java.util.List result = new java.util.ArrayList();
            int r2 = radius * radius;
            int bx0 = java.lang.Math.max(0, (gx - radius) >> com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT);
            int bx1 = java.lang.Math.min(buckets_per_side - 1, (gx + radius) >> com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT);
            int by0 = java.lang.Math.max(0, (gy - radius) >> com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT);
            int by1 = java.lang.Math.min(buckets_per_side - 1, (gy + radius) >> com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT);
            for (int by = by0; by <= by1; by++)
            {
                for (int bx = bx0; bx <= bx1; bx++)
                {
                    java.util.List list = buckets[by][bx];
                    if (list == null)
                    {
                        continue;
                    }
                    foreach (com.oddlabs.tt.player.fable.ResourceMap.Node n in java.lang.JIter.of(list))
                    {
                        if (((object)n.kind != (object)kind) || n.distanceSquaredTo(gx, gy) > r2)
                        {
                            continue;
                        }
                        if (alive_only && !n.isAlive(grid))
                        {
                            continue;
                        }
                        result.add(n);
                    }
                }
            }
            return result;
        }
        // Java: 185
        public int countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int gx, int gy, int radius)
        {
            return within(kind, gx, gy, radius, true).size();
        }
        // Java: 193
        public int estimateSupply(com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int gx, int gy, int radius)
        {
            return countAlive(kind, gx, gy, radius) * 10;
        }
        // Java: 201
        public com.oddlabs.tt.player.fable.ResourceMap.Node nearestAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int gx, int gy, int max_radius)
        {
            com.oddlabs.tt.player.fable.ResourceMap.Node best = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            int step = 1 << com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT;
            for (int radius = step; radius <= max_radius + step; radius += step)
            {
                java.util.List candidates = within(kind, gx, gy, java.lang.Math.min(radius, max_radius), true);
                foreach (com.oddlabs.tt.player.fable.ResourceMap.Node n in java.lang.JIter.of(candidates))
                {
                    int d2 = n.distanceSquaredTo(gx, gy);
                    if (d2 < best_d2 || (d2 == best_d2 && best != null && (n.grid_y < best.grid_y || (n.grid_y == best.grid_y && n.grid_x < best.grid_x))))
                    {
                        best_d2 = d2;
                        best = n;
                    }
                }
                if (best != null)
                {
                    return best;
                }
            }
            return null;
        }
        // Java: 226
        public java.util.List nearestAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int gx, int gy, int max_radius, int n)
        {
            java.util.List candidates = within(kind, gx, gy, max_radius, true);
            candidates.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.player.fable.ResourceMap.Node a = (com.oddlabs.tt.player.fable.ResourceMap.Node)a_l;
                com.oddlabs.tt.player.fable.ResourceMap.Node b = (com.oddlabs.tt.player.fable.ResourceMap.Node)b_l;
                int c = java.lang.Integer.compare(a.distanceSquaredTo(gx, gy), b.distanceSquaredTo(gx, gy));
                if (c != 0)
                {
                    return c;
                }
                c = java.lang.Integer.compare(a.grid_y, b.grid_y);
                return (c != 0 ? c : java.lang.Integer.compare(a.grid_x, b.grid_x));
            }));
            return (candidates.size() > n ? (java.util.List)(new java.util.ArrayList(candidates.subList(0, n))) : candidates);
        }
        // Java: 239
        public java.util.List findChickens()
        {
            java.util.List result = new java.util.ArrayList();
            com.oddlabs.tt.landscape.HeightMap map = world.getHeightMap();
            for (int y = 0; y < grid_size; y++)
            {
                for (int x = 0; x < grid_size; x++)
                {
                    com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
                    if ((occ is com.oddlabs.tt.model.RubberSupply chicken) && !chicken.isEmpty() && !chicken.isHit())
                    {
                        if (home_island < 0 || map.getIslandId(x, y) == home_island)
                        {
                            result.add(chicken);
                        }
                    }
                }
            }
            return result;
        }
        // Java: 256
        public int getGridSize()
        {
            return grid_size;
        }
        public ResourceMap(com.oddlabs.tt.landscape.World world, int home_island) : base()
        {
            this.world = world;
            this.grid = world.getUnitGrid();
            this.grid_size = grid.getGridSize();
            this.home_island = home_island;
            this.buckets_per_side = (grid_size + (1 << com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT) - 1) >> com.oddlabs.tt.player.fable.ResourceMap.BUCKET_SHIFT;
            this.buckets = java.lang.JA.newArray2<java.util.List>(buckets_per_side, buckets_per_side);
            this.by_kind = new java.util.List[com.oddlabs.tt.player.fable.ResourceMap.Kind.values().Length];
            for (int i = 0; i < by_kind.Length; i++)
            {
                by_kind[i] = new java.util.ArrayList();
            }
            com.oddlabs.tt.landscape.HeightMap map = world.getHeightMap();
            for (int y = 0; y < grid_size; y++)
            {
                for (int x = 0; x < grid_size; x++)
                {
                    com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
                    com.oddlabs.tt.player.fable.ResourceMap.Kind kind = null;
                    if ((occ is com.oddlabs.tt.landscape.TreeSupply))
                    {
                        kind = com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE;
                    }
                    else if ((occ is com.oddlabs.tt.model.RockSupply))
                    {
                        kind = com.oddlabs.tt.player.fable.ResourceMap.Kind.ROCK;
                    }
                    else if ((occ is com.oddlabs.tt.model.IronSupply))
                    {
                        kind = com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON;
                    }
                    if (kind == null)
                    {
                        continue;
                    }
                    int island = map.getIslandId(x, y);
                    if (home_island >= 0 && island != home_island)
                    {
                        continue;
                    }
                    com.oddlabs.tt.player.fable.ResourceMap.Node node = new com.oddlabs.tt.player.fable.ResourceMap.Node(kind, x, y, island);
                    by_kind[kind.ordinal()].add(node);
                    bucket(x, y).add(node);
                }
            }
        }
    }
}
