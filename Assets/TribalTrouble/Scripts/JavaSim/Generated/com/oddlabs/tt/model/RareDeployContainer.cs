// Translated from RareDeployContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 8
    public class RareDeployContainer : com.oddlabs.tt.model.DeployContainer
    {
        public int[] cost;
        public int required;
        public int weapons_per_unit;
        public int max_alive;
        // Java: 15
        public com.oddlabs.tt.model.RareDeployContainer limit(int max)
        {
            max_alive = max;
            return this;
        }
        // Java: 21
        public int getLimit()
        {
            if (max_alive == java.lang.Integer.MAX_VALUE)
            {
                return max_alive;
            }
            bool elite = ((object)deploy_type == (object)com.oddlabs.tt.model.DeployType.ELITE_1) || ((object)deploy_type == (object)com.oddlabs.tt.model.DeployType.ELITE_2) || ((object)deploy_type == (object)com.oddlabs.tt.model.DeployType.ELITE_3);
            return (elite ? max_alive + building.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ELITE_RANKS) : max_alive);
        }
        // Java: 29
        public int room()
        {
            if (max_alive == java.lang.Integer.MAX_VALUE)
            {
                return java.lang.Integer.MAX_VALUE;
            }
            com.oddlabs.tt.player.Player owner = building.getOwner();
            int unit = ((deploy_type).ordinal() switch { 54 /*MINOR_GOD*/ => (int)(com.oddlabs.tt.model.Race.UNIT_MINOR_GOD), 55 /*MAJOR_GOD*/ => (int)(com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD), 82 /*ELITE_1*/ => (int)(com.oddlabs.tt.model.Race.UNIT_ELITE_1), 83 /*ELITE_2*/ => (int)(com.oddlabs.tt.model.Race.UNIT_ELITE_2), 84 /*ELITE_3*/ => (int)(com.oddlabs.tt.model.Race.UNIT_ELITE_3), _ => (int)((-1)) });
            int limit_v = getLimit();
            int n = 0;
            com.oddlabs.tt.model.UnitTemplate t = (unit >= 0 ? owner.getRace().getUnitTemplate(unit) : (com.oddlabs.tt.model.UnitTemplate)(null));
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if (t != null && (s is com.oddlabs.tt.model.Unit u) && !u.isDead() && ((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)t))
                {
                    n++;
                }
                else if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && b.getDeployContainer(deploy_type) != null)
                {
                    n += b.getDeployContainer(deploy_type).getNumSupplies();
                }
            }
            return java.lang.Math.max(0, limit_v - n);
        }
        // Java: 67
        public int getWeaponsPerUnit()
        {
            return weapons_per_unit;
        }
        // Java: 71
        public int getRequiredBuilding()
        {
            return required;
        }
        // Java: 75
        public int[] getCost()
        {
            return cost;
        }
        // Java: 80
        public int affordable()
        {
            int n = java.lang.Integer.MAX_VALUE;
            for (int i = 0; i < cost.Length; i++)
            {
                if (cost[i] <= 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.SupplyContainer c = building.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]);
                n = java.lang.Math.min(n, (c == null ? 0 : c.getNumSupplies() / cost[i]));
            }
            return n;
        }
        // Java: 91
        public bool hallReady()
        {
            return com.oddlabs.tt.model.RareDeployContainer.has(building.getOwner(), required);
        }
        // Java: 95
        public static bool hasHall(com.oddlabs.tt.player.Player owner)
        {
            return com.oddlabs.tt.model.RareDeployContainer.has(owner, com.oddlabs.tt.model.Race.BUILDING_HALL);
        }
        // Java: 100
        public static bool has(com.oddlabs.tt.player.Player owner, int template_id)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isPlaced() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == template_id)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 108
        public override void orderSupply(int orders)
        {
            int capped_amount = capAmount(orders);
            int result = -building.getUnitContainer().capAmount(-capped_amount);
            com.oddlabs.tt.model.SupplyContainer weapons = building.getSupplyContainer(supply_type);
            if (result > 0)
            {
                result = (hallReady() ? java.lang.Math.min(java.lang.Math.min(result, room()), java.lang.Math.min(weapons.getNumSupplies() / weapons_per_unit, affordable())) : 0);
                if (result > 0)
                {
                    weapons.prepareDeploy(result * weapons_per_unit);
                    building.getUnitContainer().prepareDeploy(result);
                    for (int i = 0; i < cost.Length; i++)
                    {
                        if (cost[i] > 0)
                        {
                            building.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]).increaseSupply(-cost[i] * result);
                        }
                    }
                }
                orderSupply(result, orders);
            }
            else
            {
                orderSupply(result, orders);
                building.getUnitContainer().prepareDeploy(result);
                int give_back = java.lang.Math.min(-result * weapons_per_unit, weapons.getMaxSupplyCount() - weapons.getNumSupplies());
                if (give_back > 0)
                {
                    weapons.prepareDeploy(-give_back);
                }
                for (int i_2 = 0; i_2 < cost.Length; i_2++)
                {
                    if (cost[i_2] > 0)
                    {
                        building.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i_2]).increaseSupply(cost[i_2] * -result);
                    }
                }
            }
        }
        private void init_RareDeployContainer()
        {
            this.max_alive = java.lang.Integer.MAX_VALUE;
        }
        public RareDeployContainer(com.oddlabs.tt.model.Building building, float seconds_per_deploy, com.oddlabs.tt.model.DeployType deploy_type, System.Type weapon_type, int[] cost) : this(building, seconds_per_deploy, deploy_type, weapon_type, 1, cost, com.oddlabs.tt.model.Race.BUILDING_HALL)
        {
        }
        public RareDeployContainer(com.oddlabs.tt.model.Building building, float seconds_per_deploy, com.oddlabs.tt.model.DeployType deploy_type, System.Type weapon_type, int weapons, int[] cost, int required) : base(building, seconds_per_deploy, deploy_type, weapon_type)
        {
            init_RareDeployContainer();
            this.cost = cost;
            this.required = required;
            this.weapons_per_unit = java.lang.Math.max(1, weapons);
        }
    }
}
