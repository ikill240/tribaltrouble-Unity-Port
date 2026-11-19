// Translated from MarketContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 20
    public class MarketContainer : com.oddlabs.tt.model.DeployContainer
    {
        public const int LOT = 5;
        public const int GIFT_LOT = 10;
        public static int[] GOODS;
        public static float[] BASE;
        public const float SWING = 0.04f;
        public const float SELL_SHARE = 0.8f;
        public const float DRIFT = 0.02f;
        public int good;
        public int gift;
        // Java: 45
        public int getGood()
        {
            return (good >= 0 ? good : gift);
        }
        // Java: 49
        public bool isGift()
        {
            return gift >= 0;
        }
        // Java: 56
        public class Prices : java.lang.Object
        {
            public float[] factor;
            // Java: 64
            public void animate(float t)
            {
                for (int i = 0; i < factor.Length; i++)
                {
                    factor[i] += (float)(((float)(1.0f - factor[i])) * java.lang.Math.min(1.0f, (float)(com.oddlabs.tt.model.MarketContainer.DRIFT * t)));
                }
            }
            private void init_Prices()
            {
                this.factor = new float[com.oddlabs.tt.model.ExtendedResources.NUM_STORES];
            }
            public Prices() : base()
            {
                init_Prices();
                java.util.Arrays.fill(factor, 1.0f);
            }
        }
        // Java: 71
        public static float price(com.oddlabs.tt.landscape.World world, int good)
        {
            return (float)(com.oddlabs.tt.model.MarketContainer.BASE[good] * world.getMarketPrices().factor[good]);
        }
        // Java: 76
        public static int buyPrice(com.oddlabs.tt.landscape.World world, int good)
        {
            return java.lang.Math.max(1, java.lang.Math.round((float)(com.oddlabs.tt.model.MarketContainer.price(world, good) * com.oddlabs.tt.model.MarketContainer.LOT)));
        }
        // Java: 81
        public static int sellPrice(com.oddlabs.tt.landscape.World world, int good)
        {
            return java.lang.Math.max(1, java.lang.Math.round((float)((float)(com.oddlabs.tt.model.MarketContainer.price(world, good) * com.oddlabs.tt.model.MarketContainer.LOT) * com.oddlabs.tt.model.MarketContainer.SELL_SHARE)));
        }
        // Java: 87
        public com.oddlabs.tt.model.Building armory(com.oddlabs.tt.player.Player p)
        {
            com.oddlabs.tt.model.Building best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && b.isPlaced() && b.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)) != null && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                {
                    float dx = (float)(b.getPositionX() - building.getPositionX());
                    float dy = (float)(b.getPositionY() - building.getPositionY());
                    float d = (float)((float)(dx * dx) + (float)(dy * dy));
                    if (d < best_d)
                    {
                        best_d = d;
                        best = b;
                    }
                }
            }
            return best;
        }
        // Java: 103
        public static com.oddlabs.tt.model.SupplyContainer store(com.oddlabs.tt.model.Building b, int index)
        {
            return (b == null ? (com.oddlabs.tt.model.SupplyContainer)(null) : b.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[index]));
        }
        // Java: 107
        public override void orderSupply(int orders)
        {
            if (orders == 0 || building.isDead())
            {
                return;
            }
            num_orders += orders;
            if (gift >= 0)
            {
                if (orders > 0)
                {
                    give(orders);
                }
                return;
            }
            com.oddlabs.tt.landscape.World world = building.getOwner().getWorld();
            com.oddlabs.tt.model.MarketContainer.Prices prices = world.getMarketPrices();
            com.oddlabs.tt.model.Building at = armory(building.getOwner());
            com.oddlabs.tt.model.SupplyContainer gold = com.oddlabs.tt.model.MarketContainer.store(at, com.oddlabs.tt.model.ExtendedResources.GOLD);
            com.oddlabs.tt.model.SupplyContainer goods = com.oddlabs.tt.model.MarketContainer.store(at, good);
            if (gold == null || goods == null)
            {
                return;
            }
            for (int n = 0; n < java.lang.Math.abs(orders); n++)
            {
                if (orders > 0)
                {
                    int cost = com.oddlabs.tt.model.MarketContainer.buyPrice(world, good);
                    if (gold.getNumSupplies() < cost || goods.getMaxSupplyCount() - goods.getNumSupplies() < com.oddlabs.tt.model.MarketContainer.LOT)
                    {
                        break;
                    }
                    gold.increaseSupply(-cost);
                    goods.increaseSupply(com.oddlabs.tt.model.MarketContainer.LOT);
                    prices.factor[good] *= (float)(1.0f + com.oddlabs.tt.model.MarketContainer.SWING);
                }
                else
                {
                    if (goods.getNumSupplies() < com.oddlabs.tt.model.MarketContainer.LOT)
                    {
                        break;
                    }
                    int fetch = com.oddlabs.tt.model.MarketContainer.sellPrice(world, good);
                    goods.increaseSupply(-com.oddlabs.tt.model.MarketContainer.LOT);
                    gold.increaseSupply(java.lang.Math.min(fetch, gold.getMaxSupplyCount() - gold.getNumSupplies()));
                    prices.factor[good] = java.lang.Math.max(0.25f, (float)(prices.factor[good] * ((float)(1.0f - com.oddlabs.tt.model.MarketContainer.SWING))));
                }
            }
        }
        // Java: 143
        public void give(int lots)
        {
            com.oddlabs.tt.player.Player me = building.getOwner();
            com.oddlabs.tt.model.Building from = armory(me);
            com.oddlabs.tt.model.SupplyContainer src = com.oddlabs.tt.model.MarketContainer.store(from, gift);
            if (src == null)
            {
                return;
            }
            com.oddlabs.tt.model.Building to = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in me.getWorld().getPlayers())
            {
                if (((object)p == (object)me) || me.isEnemy(p) || !p.isAlive())
                {
                    continue;
                }
                com.oddlabs.tt.model.Building b = armory(p);
                if (b == null)
                {
                    continue;
                }
                float dx = (float)(b.getPositionX() - building.getPositionX());
                float dy = (float)(b.getPositionY() - building.getPositionY());
                if ((float)((float)(dx * dx) + (float)(dy * dy)) < best_d)
                {
                    best_d = (float)((float)(dx * dx) + (float)(dy * dy));
                    to = b;
                }
            }
            com.oddlabs.tt.model.SupplyContainer dst = com.oddlabs.tt.model.MarketContainer.store(to, gift);
            if (dst == null)
            {
                return;
            }
            int n = java.lang.Math.min(lots * com.oddlabs.tt.model.MarketContainer.GIFT_LOT, src.getNumSupplies());
            if (n <= 0)
            {
                return;
            }
            src.increaseSupply(-n);
            int arrive = n - n / 10;
            dst.increaseSupply(java.lang.Math.min(arrive, dst.getMaxSupplyCount() - dst.getNumSupplies()));
        }
        // Java: 174
        public override int increaseSupply(int amount)
        {
            supply_count += amount;
            return amount;
        }
        // Java: 180
        public override void deploy(float amount)
        {
        }
        // Java: 184
        public override float getBuildProgress()
        {
            return 0.0f;
        }
        static MarketContainer()
        {
            MarketContainer.GOODS = new int[] { com.oddlabs.tt.model.ExtendedResources.WOOD, com.oddlabs.tt.model.ExtendedResources.ROCK, com.oddlabs.tt.model.ExtendedResources.IRON, com.oddlabs.tt.model.ExtendedResources.CHICKEN, com.oddlabs.tt.model.ExtendedResources.CRYSTAL, com.oddlabs.tt.model.ExtendedResources.HERBS, com.oddlabs.tt.model.ExtendedResources.COAL };
            MarketContainer.BASE = new float[] { 0.25f, 0.35f, 0.5f, 0.8f, 1.0f, 1.5f, 0.6f, 0.6f };
        }
        public MarketContainer(com.oddlabs.tt.model.Building market, com.oddlabs.tt.model.DeployType type) : base(market, 1.0f, type, null)
        {
            int k = type.ordinal() - com.oddlabs.tt.model.DeployType.MARKET_WOOD.ordinal();
            int g = type.ordinal() - com.oddlabs.tt.model.DeployType.GIFT_WOOD.ordinal();
            good = (k >= 0 && k < com.oddlabs.tt.model.MarketContainer.GOODS.Length ? com.oddlabs.tt.model.MarketContainer.GOODS[k] : (-1));
            gift = (g >= 0 && g < 4 ? new int[] { com.oddlabs.tt.model.ExtendedResources.WOOD, com.oddlabs.tt.model.ExtendedResources.ROCK, com.oddlabs.tt.model.ExtendedResources.IRON, com.oddlabs.tt.model.ExtendedResources.GOLD }[g] : (-1));
        }
    }
}
