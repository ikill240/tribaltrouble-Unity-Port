// Translated from ExtendedResources.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 27
    public class ExtendedResources : java.lang.Object
    {
        public static System.Type[] RARE;
        public static System.Type[] STORES;
        public const int WOOD = 0;
        public const int ROCK = 1;
        public const int IRON = 2;
        public const int CHICKEN = 3;
        public const int GOLD = 4;
        public const int CRYSTAL = 5;
        public const int HERBS = 6;
        public const int COAL = 7;
        public const int DIAMOND = 8;
        public const int NUM_STORES = 9;
        public const int START_CLEARANCE = 9;
        public const int SEARCH_RADIUS = 7;
        // Java: 42
        public static System.Type[] rare()
        {
            return new System.Type[] { typeof(com.oddlabs.tt.model.GoldSupply), typeof(com.oddlabs.tt.model.CrystalSupply), typeof(com.oddlabs.tt.model.HerbSupply), typeof(com.oddlabs.tt.model.CoalSupply), typeof(com.oddlabs.tt.model.DiamondSupply) };
        }
        // Java: 47
        public static System.Type[] stores()
        {
            return new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RubberSupply), typeof(com.oddlabs.tt.model.GoldSupply), typeof(com.oddlabs.tt.model.CrystalSupply), typeof(com.oddlabs.tt.model.HerbSupply), typeof(com.oddlabs.tt.model.CoalSupply), typeof(com.oddlabs.tt.model.DiamondSupply) };
        }
        // Java: 53
        public static bool isRare(System.Type type)
        {
            foreach (System.Type c in com.oddlabs.tt.model.ExtendedResources.RARE)
            {
                if (((object)c == (object)type))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 61
        public static int rareIndex(System.Type type)
        {
            for (int i = 0; i < com.oddlabs.tt.model.ExtendedResources.RARE.Length; i++)
            {
                if (((object)com.oddlabs.tt.model.ExtendedResources.RARE[i] == (object)type))
                {
                    return i;
                }
            }
            return (-1);
        }
        // Java: 70
        public static void placeDeposits(com.oddlabs.tt.landscape.World world, java.util.List rocks, java.util.List irons, float[][] starts)
        {
            int[][] start_cells = new int[starts.Length][];
            for (int i = 0; i < starts.Length; i++)
            {
                start_cells[i] = new int[] { com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(starts[i][0]), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(starts[i][1]) };
            }
            java.util.List rock_fields = com.oddlabs.tt.model.ExtendedResources.clusters(rocks);
            java.util.List iron_fields = com.oddlabs.tt.model.ExtendedResources.clusters(irons);
            int placed = 0;
            int iron_total = java.lang.Math.max(8, irons.size()) * 25;
            java.util.List coal_f = new java.util.ArrayList();
            java.util.List herb_f = new java.util.ArrayList();
            java.util.List diamond_f = new java.util.ArrayList();
            for (int i_2 = 0; i_2 < rock_fields.size(); i_2++)
            {
                ((i_2 % 3 == 0 ? coal_f : (i_2 % 3 == 1 ? herb_f : diamond_f))).add(((java.util.List)rock_fields.get(i_2)));
            }
            java.util.List gold_f = new java.util.ArrayList();
            java.util.List crystal_f = new java.util.ArrayList();
            for (int i_3 = 0; i_3 < iron_fields.size(); i_3++)
            {
                ((i_3 % 2 == 0 ? gold_f : crystal_f)).add(((java.util.List)iron_fields.get(i_3)));
            }
            if (diamond_f.isEmpty() && !coal_f.isEmpty())
            {
                diamond_f.add(((java.util.List)coal_f.get(coal_f.size() - 1)));
            }
            if (crystal_f.isEmpty() && !gold_f.isEmpty())
            {
                crystal_f.add(((java.util.List)gold_f.get(gold_f.size() - 1)));
            }
            placed += com.oddlabs.tt.model.ExtendedResources.spread(world, coal_f, typeof(com.oddlabs.tt.model.CoalSupply), start_cells, iron_total * 55 / 100 / com.oddlabs.tt.model.CoalSupply.INITIAL_SUPPLIES);
            foreach (java.util.List f in java.lang.JIter.of(herb_f))
            {
                placed += com.oddlabs.tt.model.ExtendedResources.place(world, f, typeof(com.oddlabs.tt.model.HerbSupply), start_cells, java.lang.Math.max(2, java.lang.Math.min(4, f.size() / 3)));
            }
            placed += com.oddlabs.tt.model.ExtendedResources.spread(world, gold_f, typeof(com.oddlabs.tt.model.GoldSupply), start_cells, iron_total * 30 / 100 / com.oddlabs.tt.model.GoldSupply.INITIAL_SUPPLIES);
            placed += com.oddlabs.tt.model.ExtendedResources.spread(world, crystal_f, typeof(com.oddlabs.tt.model.CrystalSupply), start_cells, iron_total * 16 / 100 / com.oddlabs.tt.model.CrystalSupply.INITIAL_SUPPLIES);
            placed += com.oddlabs.tt.model.ExtendedResources.spread(world, diamond_f, typeof(com.oddlabs.tt.model.DiamondSupply), start_cells, iron_total * 7 / 100 / com.oddlabs.tt.model.DiamondSupply.INITIAL_SUPPLIES);
            java.lang.IO.println("num_rare = " + (placed));
        }
        // Java: 100
        public static java.util.List clusters(java.util.List positions)
        {
            java.util.Map at = new java.util.HashMap();
            for (int i = 0; i < positions.size(); i++)
            {
                at.put(com.oddlabs.tt.model.ExtendedResources.key(((int[])positions.get(i))[0], ((int[])positions.get(i))[1]), i);
            }
            bool[] seen = new bool[positions.size()];
            java.util.List result = new java.util.ArrayList();
            for (int i_2 = 0; i_2 < positions.size(); i_2++)
            {
                if (seen[i_2])
                {
                    continue;
                }
                java.util.List cluster = new java.util.ArrayList();
                java.util.List open = new java.util.ArrayList();
                open.add(i_2);
                seen[i_2] = true;
                for (int o = 0; o < open.size(); o++)
                {
                    int[] p = ((int[])positions.get(((int)((int?)open.get(o)))));
                    cluster.add(p);
                    for (int dy = (-2); dy <= 2; dy++)
                    {
                        for (int dx = (-2); dx <= 2; dx++)
                        {
                            int? j = ((int?)at.get(com.oddlabs.tt.model.ExtendedResources.key(p[0] + dx, p[1] + dy)));
                            if (j != null && !seen[((int)j)])
                            {
                                seen[((int)j)] = true;
                                open.add(j);
                            }
                        }
                    }
                }
                result.add(cluster);
            }
            return result;
        }
        // Java: 130
        public static long key(int x, int y)
        {
            return (((long)(y)) << 32) | (x & 4294967295L);
        }
        // Java: 135
        public static int spread(com.oddlabs.tt.landscape.World world, java.util.List fields, System.Type type, int[][] starts, int n)
        {
            if (fields.isEmpty())
            {
                return 0;
            }
            n = java.lang.Math.max(1, n);
            int done = 0;
            for (int i = 0; i < fields.size() && done < n; i++)
            {
                int left = fields.size() - i;
                int want = java.lang.Math.max(1, java.lang.Math.min(4, (n - done + left - 1) / left));
                done += com.oddlabs.tt.model.ExtendedResources.place(world, ((java.util.List)fields.get(i)), type, starts, want);
            }
            return done;
        }
        // Java: 149
        public static int place(com.oddlabs.tt.landscape.World world, java.util.List field, System.Type type, int[][] starts, int want)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = world.getUnitGrid();
            int size = grid.getGridSize();
            long sx = 0;
            long sy = 0;
            foreach (int[] p in java.lang.JIter.of(field))
            {
                sx += p[0];
                sy += p[1];
            }
            int cx = unchecked((int)((sx / field.size())));
            int cy = unchecked((int)((sy / field.size())));
            com.oddlabs.tt.render.SpriteKey[] sprites = world.getLandscapeResources().getIronFragments();
            int done = 0;
            for (int r = 2; r <= com.oddlabs.tt.model.ExtendedResources.SEARCH_RADIUS && done < want; r++)
            {
                for (int i = 0; i < 8 * r && done < want; i++)
                {
                    int[] c = com.oddlabs.tt.model.ExtendedResources.ring(cx, cy, r, i);
                    int x = c[0];
                    int y = c[1];
                    if (!com.oddlabs.tt.model.ExtendedResources.free(grid, size, x, y, starts))
                    {
                        continue;
                    }
                    float px = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(x) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                    float py = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(y) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                    float rotation = (float)(world.getRandom().nextFloat() * 360.0f);
                    com.oddlabs.tt.render.SpriteKey sprite = sprites[(x + y) % sprites.Length];
                    if (((object)type == (object)typeof(com.oddlabs.tt.model.GoldSupply)))
                    {
                        new com.oddlabs.tt.model.GoldSupply(world, sprite, 2.0f, x, y, px, py, rotation, true);
                    }
                    else if (((object)type == (object)typeof(com.oddlabs.tt.model.CrystalSupply)))
                    {
                        new com.oddlabs.tt.model.CrystalSupply(world, sprite, 2.0f, x, y, px, py, rotation, true);
                    }
                    else if (((object)type == (object)typeof(com.oddlabs.tt.model.DiamondSupply)))
                    {
                        new com.oddlabs.tt.model.DiamondSupply(world, sprite, 2.0f, x, y, px, py, rotation, true);
                    }
                    else if (((object)type == (object)typeof(com.oddlabs.tt.model.HerbSupply)))
                    {
                        new com.oddlabs.tt.model.HerbSupply(world, sprite, 2.0f, x, y, px, py, rotation, true);
                    }
                    else
                    {
                        new com.oddlabs.tt.model.CoalSupply(world, sprite, 2.0f, x, y, px, py, rotation, true);
                    }
                    done++;
                    i += 2;
                }
            }
            return done;
        }
        // Java: 190
        public static int[] ring(int cx, int cy, int r, int i)
        {
            int side = i / (2 * r);
            int t = i % (2 * r);
            return ((side) switch { 0 => (int[])(new int[] { cx - r + t, cy - r }), 1 => (int[])(new int[] { cx + r, cy - r + t }), 2 => (int[])(new int[] { cx + r - t, cy + r }), _ => (int[])(new int[] { cx - r, cy + r - t }) });
        }
        // Java: 200
        public static bool free(com.oddlabs.tt.pathfinder.UnitGrid grid, int size, int x, int y, int[][] starts)
        {
            if (x < 3 || y < 3 || x >= size - 3 || y >= size - 3)
            {
                return false;
            }
            if (grid.getRegion(x, y) == null || grid.isGridOccupied(x, y) || grid.isWater(x, y))
            {
                return false;
            }
            foreach (int[] s in starts)
            {
                if (java.lang.Math.abs(s[0] - x) < com.oddlabs.tt.model.ExtendedResources.START_CLEARANCE && java.lang.Math.abs(s[1] - y) < com.oddlabs.tt.model.ExtendedResources.START_CLEARANCE)
                {
                    return false;
                }
            }
            int blocked = 0;
            for (int dy = (-1); dy <= 1; dy++)
            {
                for (int dx = (-1); dx <= 1; dx++)
                {
                    if ((dx != 0 || dy != 0) && (grid.getRegion(x + dx, y + dy) == null || grid.isGridOccupied(x + dx, y + dy)))
                    {
                        blocked++;
                    }
                }
            }
            return blocked <= 3;
        }
        // Java: 220
        public static bool hasStores(com.oddlabs.tt.model.Building b, int[] cost)
        {
            for (int i = 0; i < cost.Length && i < com.oddlabs.tt.model.ExtendedResources.STORES.Length; i++)
            {
                if (cost[i] <= 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.SupplyContainer c = b.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]);
                if (c == null || c.getNumSupplies() < cost[i])
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 232
        public static com.oddlabs.tt.model.Building findPayer(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.Building here, int[] cost)
        {
            if (here != null && here.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)) != null)
            {
                return (com.oddlabs.tt.model.ExtendedResources.hasStores(here, cost) ? here : (com.oddlabs.tt.model.Building)(null));
            }
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && b.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)) != null && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) && com.oddlabs.tt.model.ExtendedResources.hasStores(b, cost))
                {
                    return b;
                }
            }
            return null;
        }
        // Java: 244
        public static void pay(com.oddlabs.tt.model.Building payer, int[] cost, int sign)
        {
            for (int i = 0; i < cost.Length && i < com.oddlabs.tt.model.ExtendedResources.STORES.Length; i++)
            {
                if (cost[i] <= 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.SupplyContainer c = payer.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]);
                if (c != null)
                {
                    c.increaseSupply(-sign * cost[i]);
                }
            }
        }
        // Java: 255
        public static int stock(com.oddlabs.tt.player.Player owner, System.Type type)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                {
                    com.oddlabs.tt.model.SupplyContainer c = b.getSupplyContainer(type);
                    if (c != null)
                    {
                        n += c.getNumSupplies();
                    }
                }
            }
            return n;
        }
        static ExtendedResources()
        {
            ExtendedResources.RARE = com.oddlabs.tt.model.ExtendedResources.rare();
            ExtendedResources.STORES = com.oddlabs.tt.model.ExtendedResources.stores();
        }
        public ExtendedResources() : base()
        {
        }
    }
}
