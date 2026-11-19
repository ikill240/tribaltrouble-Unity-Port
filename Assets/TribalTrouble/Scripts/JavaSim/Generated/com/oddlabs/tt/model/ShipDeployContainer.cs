// Translated from ShipDeployContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class ShipDeployContainer : com.oddlabs.tt.model.DeployContainer
    {
        public com.oddlabs.tt.model.Ship ship;
        public bool is_transfer;
        // Java: 18
        public override void orderSupply(int orders)
        {
            int capped_amount = capAmount(orders);
            int result = -ship.getUnitContainer().capAmount(-capped_amount);
            if (supply_type != null)
            {
                result = -ship.getSupplyContainer(supply_type).capAmount(-result);
            }
            if (is_transfer)
            {
                int num_peons = ship.getShipHR().countPeons();
                if (result > 0)
                {
                    result = java.lang.StrictMath.min(num_peons, result);
                }
                else
                {
                    result = java.lang.StrictMath.max(-num_peons, result);
                }
            }
            if (result > 0)
            {
                if (supply_type != null)
                {
                    ship.getSupplyContainer(supply_type).prepareDeploy(result);
                }
                ship.getUnitContainer().prepareDeploy(result);
                orderSupply(result, orders);
            }
            else
            {
                orderSupply(result, orders);
                ship.getUnitContainer().prepareDeploy(result);
                if (supply_type != null)
                {
                    com.oddlabs.tt.model.SupplyContainer supply_container = ship.getSupplyContainer(supply_type);
                    if (!supply_container.isSupplyFull())
                    {
                        supply_container.prepareDeploy(result);
                    }
                }
            }
        }
        // Java: 53
        public override void orderSupply(int amount, int orders)
        {
            increaseSupply(amount);
            this.num_orders += orders;
        }
        // Java: 59
        public override int increaseSupply(int amount)
        {
            int capped_amount = capAmount(amount);
            this.supply_count += capped_amount;
            return capped_amount;
        }
        public ShipDeployContainer(com.oddlabs.tt.model.Ship ship, float seconds_per_deploy, com.oddlabs.tt.model.DeployType deploy_type, System.Type supply_type, bool is_transfer) : base(ship, seconds_per_deploy, deploy_type, supply_type)
        {
            this.ship = ship;
            this.is_transfer = is_transfer;
        }
    }
}
