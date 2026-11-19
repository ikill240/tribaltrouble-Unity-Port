// Translated from CraftContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 17
    public class CraftContainer : com.oddlabs.tt.model.DeployContainer
    {
        public static int[] CRYSTAL_COST;
        public static int[] DIAMOND_COST;
        public static int[] DARTS_COST_NATIVES;
        public static int[] DARTS_COST_VIKINGS;
        public const float CRYSTAL_SECONDS = 30.0f;
        public const float DIAMOND_SECONDS = 50.0f;
        public const float DARTS_SECONDS = 12.0f;
        public const float FORGE_REACH = 30.0f;
        public const float FORGE_SPEEDUP = 1.35f;
        public const int MAX_QUEUE = 50;
        public int[] cost;
        public bool needs_forge;
        public int queued;
        // Java: 42
        public static com.oddlabs.tt.model.CraftContainer create(com.oddlabs.tt.model.Building armory, com.oddlabs.tt.model.DeployType type)
        {
            bool vikings = com.oddlabs.tt.model.ExtendedCombat.isVikings(armory.getOwner());
            return ((type).ordinal() switch { 48 /*CRAFT_CRYSTAL*/ => (com.oddlabs.tt.model.CraftContainer)(new com.oddlabs.tt.model.CraftContainer(armory, type, typeof(com.oddlabs.tt.model.weapon.CrystalWeapon), com.oddlabs.tt.model.CraftContainer.CRYSTAL_COST, com.oddlabs.tt.model.CraftContainer.CRYSTAL_SECONDS, false)), 49 /*CRAFT_DIAMOND*/ => (com.oddlabs.tt.model.CraftContainer)(new com.oddlabs.tt.model.CraftContainer(armory, type, typeof(com.oddlabs.tt.model.weapon.DiamondWeapon), com.oddlabs.tt.model.CraftContainer.DIAMOND_COST, com.oddlabs.tt.model.CraftContainer.DIAMOND_SECONDS, true)), 50 /*CRAFT_DARTS*/ => (com.oddlabs.tt.model.CraftContainer)(new com.oddlabs.tt.model.CraftContainer(armory, type, typeof(com.oddlabs.tt.model.weapon.DartWeapon), (vikings ? com.oddlabs.tt.model.CraftContainer.DARTS_COST_VIKINGS : com.oddlabs.tt.model.CraftContainer.DARTS_COST_NATIVES), com.oddlabs.tt.model.CraftContainer.DARTS_SECONDS, false)), _ => throw new System.ArgumentException("not a craft order: " + java.lang.JStr.valueOf(type)) });
        }
        // Java: 54
        public System.Type getProduct()
        {
            return supply_type;
        }
        // Java: 58
        public int[] getCost()
        {
            return cost;
        }
        // Java: 62
        public bool needsForge()
        {
            return needs_forge;
        }
        // Java: 67
        public bool isAvailable()
        {
            com.oddlabs.tt.player.Player owner = building.getOwner();
            if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.weapon.CrystalWeapon)))
            {
                return owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT) > 0;
            }
            if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.weapon.DiamondWeapon)))
            {
                return owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.DIAMOND_CRAFT) > 0;
            }
            return true;
        }
        // Java: 77
        public int affordable()
        {
            int n = com.oddlabs.tt.model.CraftContainer.MAX_QUEUE - queued;
            for (int i = 0; i < cost.Length; i++)
            {
                if (cost[i] <= 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.SupplyContainer c = building.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]);
                n = java.lang.Math.min(n, (c == null ? 0 : c.getNumSupplies() / cost[i]));
            }
            return java.lang.Math.max(0, n);
        }
        // Java: 88
        public static bool hasForge(com.oddlabs.tt.player.Player owner)
        {
            return com.oddlabs.tt.model.CraftContainer.nearestForge(owner, java.lang.Float.NaN, java.lang.Float.NaN) != null;
        }
        // Java: 93
        public static com.oddlabs.tt.model.LandBuilding nearestForge(com.oddlabs.tt.player.Player owner, float x, float y)
        {
            com.oddlabs.tt.model.LandBuilding best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isPlaced() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_FORGE)
                {
                    if (java.lang.Float.isNaN(x))
                    {
                        return b;
                    }
                    float dx = (float)(b.getPositionX() - x);
                    float dy = (float)(b.getPositionY() - y);
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
        // Java: 111
        public float speed()
        {
            com.oddlabs.tt.model.LandBuilding f = com.oddlabs.tt.model.CraftContainer.nearestForge(building.getOwner(), building.getPositionX(), building.getPositionY());
            if (f == null)
            {
                return 1.0f;
            }
            float dx = (float)(f.getPositionX() - building.getPositionX());
            float dy = (float)(f.getPositionY() - building.getPositionY());
            return ((float)((float)(dx * dx) + (float)(dy * dy)) <= (float)(com.oddlabs.tt.model.CraftContainer.FORGE_REACH * com.oddlabs.tt.model.CraftContainer.FORGE_REACH) ? com.oddlabs.tt.model.CraftContainer.FORGE_SPEEDUP : 1.0f);
        }
        // Java: 119
        public override void orderSupply(int orders)
        {
            if (orders > 0)
            {
                int n = (isAvailable() ? java.lang.Math.min(orders, affordable()) : 0);
                if (n <= 0)
                {
                    return;
                }
                com.oddlabs.tt.model.ExtendedResources.pay(building, scaled(n), 1);
                queued += n;
                num_orders += orders;
                supply_count += n;
            }
            else if (orders < 0)
            {
                int n_2 = java.lang.Math.min(-orders, queued);
                if (n_2 <= 0)
                {
                    return;
                }
                com.oddlabs.tt.model.ExtendedResources.pay(building, scaled(n_2), (-1));
                queued -= n_2;
                num_orders += orders;
                supply_count -= n_2;
                if (queued == 0)
                {
                    time = 0;
                }
            }
        }
        // Java: 142
        public int[] scaled(int n)
        {
            int[] c = new int[cost.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = cost[i] * n;
            }
            return c;
        }
        // Java: 149
        public override int increaseSupply(int amount)
        {
            supply_count += amount;
            return amount;
        }
        // Java: 156
        public override void deploy(float amount)
        {
            if (queued <= 0)
            {
                return;
            }
            time += (float)(amount * speed());
            if (time >= seconds_per_deploy)
            {
                time = 0;
                queued--;
                supply_count--;
                com.oddlabs.tt.model.SupplyContainer store = building.getSupplyContainer(supply_type);
                if (store != null)
                {
                    store.increaseSupply(1);
                }
            }
        }
        // Java: 171
        public override float getBuildProgress()
        {
            return (queued > 0 ? (float)(time / seconds_per_deploy) : 0.0f);
        }
        static CraftContainer()
        {
            CraftContainer.CRYSTAL_COST = new int[] { 3, 1, 1, 0, 1, 3, 0, 0 };
            CraftContainer.DIAMOND_COST = new int[] { 3, 0, 1, 0, 1, 2, 0, 2, 2 };
            CraftContainer.DARTS_COST_NATIVES = new int[] { 2, 1, 0, 0, 0, 0, 2, 0 };
            CraftContainer.DARTS_COST_VIKINGS = new int[] { 2, 0, 1, 0, 0, 0, 0, 2 };
        }
        public CraftContainer(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, System.Type product, int[] cost, float seconds, bool needs_forge) : base(building, seconds, type, product)
        {
            this.cost = cost;
            this.needs_forge = needs_forge;
        }
    }
}
