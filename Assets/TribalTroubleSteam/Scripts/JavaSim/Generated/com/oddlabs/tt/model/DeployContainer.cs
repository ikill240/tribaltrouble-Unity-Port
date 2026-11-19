// Translated from DeployContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class DeployContainer : com.oddlabs.tt.model.SupplyContainer
    {
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.DeployType deploy_type;
        public System.Type supply_type;
        public float seconds_per_deploy;
        public float time;
        public int num_orders;
        // Java: 21
        public virtual void orderSupply(int orders)
        {
            int capped_amount = capAmount(orders);
            int result = -building.getUnitContainer().capAmount(-capped_amount);
            if (supply_type != null)
            {
                result = -building.getSupplyContainer(supply_type).capAmount(-result);
            }
            if (result > 0)
            {
                if (supply_type != null)
                {
                    building.getSupplyContainer(supply_type).prepareDeploy(result);
                }
                building.getUnitContainer().prepareDeploy(result);
                orderSupply(result, orders);
            }
            else
            {
                orderSupply(result, orders);
                building.getUnitContainer().prepareDeploy(result);
                if (supply_type != null)
                {
                    com.oddlabs.tt.model.SupplyContainer supply_container = building.getSupplyContainer(supply_type);
                    if (!supply_container.isSupplyFull())
                    {
                        supply_container.prepareDeploy(result);
                    }
                }
            }
        }
        // Java: 42
        public virtual void orderSupply(int amount, int orders)
        {
            increaseSupply(amount);
            num_orders += orders;
        }
        // Java: 47
        public virtual int getNumOrders()
        {
            return num_orders;
        }
        // Java: 51
        public virtual void deploy(float amount)
        {
            time += amount;
            if (time >= seconds_per_deploy)
            {
                time = 0;
                increaseSupply((-1));
                doDeploy();
            }
        }
        // Java: 60
        public virtual void doDeploy()
        {
            switch ((deploy_type).ordinal())
            {
                case 0 /*ROCK_WARRIOR*/:
                {
                    building.createArmy(0, 1, 0, 0);
                    break;
                }
                case 1 /*IRON_WARRIOR*/:
                {
                    building.createArmy(0, 0, 1, 0);
                    break;
                }
                case 2 /*RUBBER_WARRIOR*/:
                {
                    building.createArmy(0, 0, 0, 1);
                    break;
                }
                case 3 /*PEON*/:
                {
                    building.createArmy(1, 0, 0, 0);
                    break;
                }
                case 4 /*PEON_HARVEST_TREE*/:
                {
                    building.createHarvesters(1, 0, 0, 0);
                    break;
                }
                case 5 /*PEON_TRANSPORT_TREE*/:
                {
                    building.createTransporters(1, 0, 0, 0);
                    break;
                }
                case 6 /*PEON_HARVEST_ROCK*/:
                {
                    building.createHarvesters(0, 1, 0, 0);
                    break;
                }
                case 7 /*PEON_TRANSPORT_ROCK*/:
                {
                    building.createTransporters(0, 1, 0, 0);
                    break;
                }
                case 8 /*PEON_HARVEST_IRON*/:
                {
                    building.createHarvesters(0, 0, 1, 0);
                    break;
                }
                case 9 /*PEON_TRANSPORT_IRON*/:
                {
                    building.createTransporters(0, 0, 1, 0);
                    break;
                }
                case 10 /*PEON_HARVEST_RUBBER*/:
                {
                    building.createHarvesters(0, 0, 0, 1);
                    break;
                }
                case 11 /*PEON_TRANSPORT_RUBBER*/:
                {
                    building.createTransporters(0, 0, 0, 1);
                    break;
                }
            }
        }
        // Java: 101
        public override int increaseSupply(int amount)
        {
            int result = building.getOwner().getUnitCountContainer().increaseSupply(amount);
            return base.increaseSupply(amount);
        }
        // Java: 108
        public virtual float getBuildProgress()
        {
            return (float)(time / seconds_per_deploy);
        }
        private void init_DeployContainer()
        {
            this.time = 0;
            this.num_orders = 0;
        }
        public DeployContainer(com.oddlabs.tt.model.Building building, float seconds_per_deploy, com.oddlabs.tt.model.DeployType deploy_type, System.Type supply_type) : base(java.lang.Integer.MAX_VALUE)
        {
            init_DeployContainer();
            this.building = building;
            this.seconds_per_deploy = seconds_per_deploy;
            this.deploy_type = deploy_type;
            this.supply_type = supply_type;
        }
    }
}
