// Translated from ResearchContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 10
    public class ResearchContainer : com.oddlabs.tt.model.DeployContainer
    {
        public const int ARMOR = 0;
        public const int SPEED = 1;
        public const int TOOLS = 2;
        public const int RANGE = 3;
        public const int GROWTH = 4;
        public const int TOWER_RANGE = 5;
        public const int TOWER_ARMOR = 6;
        public const int BLESSING = 7;
        public const int LORE = 8;
        public const int WALLS = 9;
        public const int RACE_1 = 10;
        public const int RACE_2 = 11;
        public const int SMITHING = 12;
        public const int DRILL = 13;
        public const int STURDY = 14;
        public const int BASKETS = 15;
        public const int VOLLEY = 16;
        public const int STAKES = 17;
        public const int FOCUS = 18;
        public const int PROSPECTING = 19;
        public const int VIGOR = 20;
        public const int CRYSTAL_CRAFT = 21;
        public const int DIAMOND_CRAFT = 22;
        public const int ROCK_EDGE = 23;
        public const int IRON_TEMPER = 24;
        public const int CHICKEN_BOUNCE = 25;
        public const int CRYSTAL_FACET = 26;
        public const int DIAMOND_EDGE = 27;
        public const int SHIP_OARS = 28;
        public const int SHIP_HULL = 29;
        public const int ARCANE_POWER = 30;
        public const int COURAGE_MASTERY = 31;
        public const int FLEET = 32;
        public const int CARGO = 33;
        public const int BUILDINGS = 34;
        public const int ELITE_RANKS = 35;
        public const int ELITE_TRAINING = 36;
        public const int GREAT_TRIBE = 37;
        public const int BRIDGES = 38;
        public const int RUNE_REACH = 39;
        public const int RUNE_MIGHT = 40;
        public const int RUNE_WARD = 41;
        public const int NUM_UPGRADES = 42;
        public const int MAX_LEVEL = 5;
        public static int[][][] COSTS;
        public static int[][][][] RACE_COSTS;
        public static float[] SECONDS;
        public int upgrade;
        public com.oddlabs.tt.model.Building paid_at;
        public bool researching;
        public int researching_level;
        // Java: 127
        public int getUpgrade()
        {
            return upgrade;
        }
        // Java: 131
        public static int[] getCost(int upgrade, int level)
        {
            return com.oddlabs.tt.model.ResearchContainer.getCost(upgrade, level, false);
        }
        // Java: 135
        public static int[] getCost(int upgrade, int level, bool vikings)
        {
            int l = java.lang.Math.min(level, com.oddlabs.tt.model.ResearchContainer.getMaxLevel(upgrade) - 1);
            int key = ((upgrade * 8 + l) << 1) | ((vikings ? 1 : 0));
            int[] have = ((int[])com.oddlabs.tt.model.ResearchContainer.SCALED.get(key));
            if (have != null)
            {
                return have;
            }
            int[] @base = (upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_1 || upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_2 ? com.oddlabs.tt.model.ResearchContainer.RACE_COSTS[(vikings ? 1 : 0)][upgrade - com.oddlabs.tt.model.ResearchContainer.RACE_1][l] : com.oddlabs.tt.model.ResearchContainer.COSTS[upgrade][l]);
            int[] c = new int[@base.Length];
            for (int i = 0; i < @base.Length; i++)
            {
                c[i] = (@base[i] * com.oddlabs.tt.model.ResearchContainer.COST_SCALE_PERCENT + 99) / 100;
            }
            com.oddlabs.tt.model.ResearchContainer.SCALED.put(key, c);
            return c;
        }
        public const int COST_SCALE_PERCENT = 150;
        public static java.util.Map SCALED;
        // Java: 154
        public static int getMaxLevel(int upgrade)
        {
            if (upgrade == com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT || upgrade == com.oddlabs.tt.model.ResearchContainer.DIAMOND_CRAFT || upgrade == com.oddlabs.tt.model.ResearchContainer.BRIDGES)
            {
                return 1;
            }
            if (upgrade <= com.oddlabs.tt.model.ResearchContainer.GROWTH)
            {
                return 5;
            }
            if (upgrade == com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE)
            {
                return 4;
            }
            if (upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_1 || upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_2 || upgrade == com.oddlabs.tt.model.ResearchContainer.STAKES || upgrade == com.oddlabs.tt.model.ResearchContainer.FOCUS || upgrade == com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY || upgrade == com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING || upgrade == com.oddlabs.tt.model.ResearchContainer.RUNE_WARD)
            {
                return 2;
            }
            return 3;
        }
        // Java: 167
        public int[] cost(int level)
        {
            return com.oddlabs.tt.model.ResearchContainer.getCost(upgrade, level, owner());
        }
        // Java: 172
        public static int[] getCost(int upgrade, int level, com.oddlabs.tt.player.Player player)
        {
            int[] c = com.oddlabs.tt.model.ResearchContainer.getCost(upgrade, level, com.oddlabs.tt.model.ExtendedCombat.isVikings(player));
            int pct = player.getResearchCostPercent();
            if (pct == 100)
            {
                return c;
            }
            int[] r = new int[c.Length];
            for (int i = 0; i < c.Length; i++)
            {
                r[i] = (c[i] * pct + 99) / 100;
            }
            return r;
        }
        // Java: 183
        public static float getSeconds(int level)
        {
            return com.oddlabs.tt.model.ResearchContainer.SECONDS[java.lang.Math.min(level, com.oddlabs.tt.model.ResearchContainer.MAX_LEVEL - 1)];
        }
        // Java: 187
        public bool isResearching()
        {
            return researching;
        }
        // Java: 191
        public com.oddlabs.tt.player.Player owner()
        {
            return building.getOwner();
        }
        // Java: 195
        public bool canAfford()
        {
            if (upgrade == com.oddlabs.tt.model.ResearchContainer.DIAMOND_CRAFT && owner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT) == 0)
            {
                return false;
            }
            int level = owner().getUpgradeLevel(upgrade);
            if (level >= owner().getResearchCap(upgrade))
            {
                return false;
            }
            return findPayer(cost(level)) != null;
        }
        // Java: 205
        public com.oddlabs.tt.model.Building findPayer(int[] cost_v)
        {
            return com.oddlabs.tt.model.ExtendedResources.findPayer(owner(), building, cost_v);
        }
        // Java: 209
        public void pay(int level, int sign)
        {
            int[] cost_v = cost(level);
            if (sign > 0)
            {
                paid_at = findPayer(cost_v);
            }
            if (paid_at == null || paid_at.isDead())
            {
                return;
            }
            com.oddlabs.tt.model.ExtendedResources.pay(paid_at, cost_v, sign);
        }
        // Java: 218
        public override void orderSupply(int orders)
        {
            if (orders > 0 && !researching && !owner().isResearching(upgrade) && canAfford())
            {
                researching_level = owner().getUpgradeLevel(upgrade);
                pay(researching_level, 1);
                researching = true;
                time = 0;
                supply_count = 1;
                owner().setResearching(upgrade, this);
            }
            else if (orders < 0 && researching)
            {
                pay(researching_level, (-1));
                stop();
            }
        }
        // Java: 233
        public void stop()
        {
            researching = false;
            researching_level = (-1);
            time = 0;
            supply_count = 0;
            owner().setResearching(upgrade, null);
        }
        // Java: 241
        public override int increaseSupply(int amount)
        {
            supply_count += amount;
            return amount;
        }
        // Java: 248
        public override void deploy(float amount)
        {
            if (!researching)
            {
                return;
            }
            time += amount;
            if (time >= com.oddlabs.tt.model.ResearchContainer.getSeconds(researching_level))
            {
                stop();
                owner().upgrade(upgrade);
            }
        }
        // Java: 259
        public override float getBuildProgress()
        {
            return (researching ? (float)(time / com.oddlabs.tt.model.ResearchContainer.getSeconds(researching_level)) : 0.0f);
        }
        static ResearchContainer()
        {
            ResearchContainer.COSTS = new int[][][] { new int[][] { new int[] { 10, 0, 10, 0 }, new int[] { 20, 10, 20, 0 }, new int[] { 30, 20, 30, 0, 3 }, new int[] { 40, 30, 40, 0, 6, 0, 0, 3 }, new int[] { 50, 40, 50, 0, 10, 2, 0, 5, 1 } }, new int[][] { new int[] { 15, 10, 0, 0 }, new int[] { 25, 20, 0, 0 }, new int[] { 35, 30, 0, 0, 2, 0, 3 }, new int[] { 45, 40, 0, 0, 5, 0, 6 }, new int[] { 55, 50, 0, 0, 8, 2, 8, 0, 1 } }, new int[][] { new int[] { 10, 10, 0, 0 }, new int[] { 20, 20, 0, 0 }, new int[] { 30, 30, 10, 0, 2 }, new int[] { 40, 40, 20, 0, 5 }, new int[] { 50, 50, 30, 0, 8, 2, 0, 0, 1 } }, new int[][] { new int[] { 15, 0, 10, 0 }, new int[] { 25, 0, 20, 2 }, new int[] { 35, 0, 30, 3, 0, 2 }, new int[] { 45, 0, 40, 4, 4, 4 }, new int[] { 55, 0, 50, 5, 8, 6, 0, 0, 1 } }, new int[][] { new int[] { 20, 10, 0, 0 }, new int[] { 30, 20, 0, 0 }, new int[] { 40, 30, 0, 2, 0, 0, 4 }, new int[] { 50, 40, 0, 4, 4, 0, 6 }, new int[] { 60, 50, 0, 6, 8, 2, 8 } }, new int[][] { new int[] { 10, 10, 5, 0 }, new int[] { 20, 20, 10, 0 }, new int[] { 30, 30, 20, 0, 4, 0, 0, 3 } }, new int[][] { new int[] { 20, 20, 0, 0 }, new int[] { 30, 30, 10, 0 }, new int[] { 40, 40, 20, 0, 5, 0, 0, 3 } }, new int[][] { new int[] { 10, 10, 10, 0 }, new int[] { 20, 20, 20, 0 }, new int[] { 30, 30, 30, 0, 4, 3, 4 } }, new int[][] { new int[] { 0, 10, 10, 2 }, new int[] { 0, 20, 20, 4 }, new int[] { 0, 30, 30, 6, 4, 4 } }, new int[][] { new int[] { 20, 10, 0, 0 }, new int[] { 30, 20, 0, 0 }, new int[] { 40, 40, 10, 0, 4 } }, null, null, new int[][] { new int[] { 10, 0, 15, 0 }, new int[] { 20, 0, 25, 0, 2 }, new int[] { 30, 0, 40, 0, 6, 0, 0, 4 } }, new int[][] { new int[] { 15, 10, 0, 0 }, new int[] { 25, 20, 0, 0, 0, 0, 0, 3 }, new int[] { 35, 30, 0, 0, 4, 0, 0, 5 } }, new int[][] { new int[] { 15, 0, 0, 0, 0, 0, 3 }, new int[] { 25, 10, 0, 0, 0, 0, 5 }, new int[] { 35, 20, 0, 0, 3, 0, 7 } }, new int[][] { new int[] { 20, 0, 0, 0 }, new int[] { 30, 10, 0, 0, 0, 0, 2 }, new int[] { 40, 20, 0, 0, 3, 0, 4 } }, new int[][] { new int[] { 10, 10, 10, 0 }, new int[] { 20, 20, 20, 0, 0, 0, 0, 3 }, new int[] { 30, 30, 30, 0, 4, 0, 0, 5 } }, new int[][] { new int[] { 20, 10, 0, 0 }, new int[] { 30, 20, 5, 0 } }, new int[][] { new int[] { 0, 0, 0, 0, 2, 3 }, new int[] { 0, 0, 0, 0, 4, 5 } }, new int[][] { new int[] { 15, 10, 0, 0 }, new int[] { 25, 20, 5, 0, 2 }, new int[] { 35, 30, 10, 0, 6, 2 } }, new int[][] { new int[] { 10, 0, 0, 2, 0, 0, 4 }, new int[] { 20, 0, 0, 4, 2, 0, 6 }, new int[] { 30, 0, 0, 6, 6, 2, 8 } }, new int[][] { new int[] { 20, 10, 0, 0, 2, 4 } }, new int[][] { new int[] { 30, 0, 15, 0, 4, 6, 0, 4, 2 } }, new int[][] { new int[] { 15, 15, 0, 0 }, new int[] { 25, 30, 0, 0, 2 }, new int[] { 35, 45, 0, 0, 5, 0, 0, 3 } }, new int[][] { new int[] { 10, 0, 15, 0 }, new int[] { 20, 0, 30, 0, 2 }, new int[] { 30, 0, 45, 0, 5, 0, 0, 4 } }, new int[][] { new int[] { 10, 0, 5, 5 }, new int[] { 20, 0, 10, 10 }, new int[] { 30, 0, 15, 15, 3 } }, new int[][] { new int[] { 15, 0, 0, 0, 0, 4 }, new int[] { 25, 0, 0, 0, 2, 8 }, new int[] { 35, 0, 0, 0, 5, 12 } }, new int[][] { new int[] { 20, 0, 10, 0, 2, 4, 0, 2, 1 }, new int[] { 30, 0, 20, 0, 4, 6, 0, 4, 2 }, new int[] { 40, 0, 30, 0, 8, 8, 0, 6, 3 } }, new int[][] { new int[] { 20, 0, 5, 0 }, new int[] { 30, 0, 10, 0, 2 }, new int[] { 40, 0, 20, 0, 5 } }, new int[][] { new int[] { 20, 10, 10, 0 }, new int[] { 30, 20, 20, 0, 2 }, new int[] { 40, 30, 30, 0, 5, 0, 0, 3 } }, new int[][] { new int[] { 10, 10, 0, 0, 2, 4 }, new int[] { 20, 20, 0, 0, 4, 8 }, new int[] { 30, 30, 0, 0, 8, 12 } }, new int[][] { new int[] { 15, 10, 5, 0, 4, 2 }, new int[] { 25, 20, 10, 0, 6, 4 } }, new int[][] { new int[] { 30, 0, 10, 0, 4 }, new int[] { 40, 0, 20, 0, 8, 4 }, new int[] { 60, 0, 30, 0, 12, 8, 0, 0, 2 } }, new int[][] { new int[] { 20, 10, 0, 0, 4 }, new int[] { 30, 20, 0, 0, 8, 2 }, new int[] { 40, 30, 10, 0, 12, 4, 0, 0, 2 } }, new int[][] { new int[] { 30, 30, 0, 0, 6 }, new int[] { 40, 40, 10, 0, 10, 4 }, new int[] { 60, 60, 20, 0, 16, 8, 0, 0, 4 } }, new int[][] { new int[] { 20, 10, 10, 0, 8, 2 }, new int[] { 30, 20, 20, 0, 14, 4, 0, 0, 1 }, new int[] { 40, 30, 30, 0, 20, 8, 0, 0, 2 } }, new int[][] { new int[] { 20, 0, 10, 0, 6, 2 }, new int[] { 30, 0, 20, 0, 12, 4, 0, 0, 1 } }, new int[][] { new int[] { 60, 40, 10, 0, 10, 4 }, new int[] { 80, 60, 20, 0, 16, 6, 0, 0, 1 }, new int[] { 100, 80, 30, 0, 24, 10, 0, 0, 2 }, new int[] { 140, 100, 40, 0, 32, 14, 0, 0, 4 } }, new int[][] { new int[] { 60, 40, 20, 0, 10, 2 } }, new int[][] { new int[] { 15, 20, 0, 0, 2, 1 }, new int[] { 25, 35, 5, 0, 4, 2 }, new int[] { 35, 50, 10, 0, 6, 4, 0, 0, 1 } }, new int[][] { new int[] { 10, 15, 10, 0, 3, 2 }, new int[] { 20, 25, 20, 0, 6, 4, 0, 2 }, new int[] { 30, 35, 30, 0, 10, 6, 0, 4, 2 } }, new int[][] { new int[] { 15, 10, 0, 0, 2, 1, 4 }, new int[] { 25, 20, 10, 0, 5, 3, 8, 0, 1 } } };
            ResearchContainer.RACE_COSTS = new int[][][][] { new int[][][] { new int[][] { new int[] { 10, 0, 0, 0, 0, 1, 6, 0 }, new int[] { 15, 0, 0, 0, 0, 2, 10, 0 } }, new int[][] { new int[] { 10, 0, 0, 0, 2, 1, 8, 0 }, new int[] { 20, 0, 0, 0, 4, 2, 12, 0 } } }, new int[][][] { new int[][] { new int[] { 10, 0, 0, 0, 0, 1, 0, 6 }, new int[] { 15, 0, 5, 0, 0, 2, 0, 10 } }, new int[][] { new int[] { 10, 0, 5, 0, 4, 1, 0, 4 }, new int[] { 20, 0, 10, 0, 6, 2, 0, 6 } } } };
            ResearchContainer.SECONDS = new float[] { 40.0f, 70.0f, 100.0f, 130.0f, 160.0f };
            ResearchContainer.SCALED = new java.util.HashMap();
        }
        private void init_ResearchContainer()
        {
            this.researching_level = (-1);
        }
        public ResearchContainer(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, int upgrade) : base(building, com.oddlabs.tt.model.ResearchContainer.SECONDS[0], type, null)
        {
            init_ResearchContainer();
            this.upgrade = upgrade;
        }
    }
}
