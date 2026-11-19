// Translated from MultiWeaponDeployContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 7
    public class MultiWeaponDeployContainer : com.oddlabs.tt.model.DeployContainer
    {
        public int weapons_per_unit;
        // Java: 16
        public override void orderSupply(int orders)
        {
            int capped_amount = capAmount(orders);
            int result = -building.getUnitContainer().capAmount(-capped_amount);
            com.oddlabs.tt.model.SupplyContainer weapons = building.getSupplyContainer(supply_type);
            if (result > 0)
            {
                result = java.lang.Math.min(result, weapons.getNumSupplies() / weapons_per_unit);
                if (result > 0)
                {
                    weapons.prepareDeploy(result * weapons_per_unit);
                    building.getUnitContainer().prepareDeploy(result);
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
            }
        }
        // Java: 38
        public int getWeaponsPerUnit()
        {
            return weapons_per_unit;
        }
        public MultiWeaponDeployContainer(com.oddlabs.tt.model.Building building, float seconds_per_deploy, com.oddlabs.tt.model.DeployType deploy_type, System.Type supply_type, int weapons_per_unit) : base(building, seconds_per_deploy, deploy_type, supply_type)
        {
            this.weapons_per_unit = weapons_per_unit;
        }
    }
}
