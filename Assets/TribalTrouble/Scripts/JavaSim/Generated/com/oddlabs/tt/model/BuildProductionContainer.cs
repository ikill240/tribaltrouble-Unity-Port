// Translated from BuildProductionContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public class BuildProductionContainer : com.oddlabs.tt.model.BuildSupplyContainer
    {
        public com.oddlabs.tt.model.SupplyContainer dest_container;
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.Cost cost;
        public float man_seconds_per_production;
        public float man_seconds;
        public bool infinite;
        // Java: 27
        public virtual void orderSupply(int amount, bool infinite)
        {
            this.infinite = infinite;
            if (infinite)
            {
                base.orderSupply(com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT - getNumSupplies(), amount);
            }
            else
            {
                base.orderSupply(amount);
            }
        }
        // Java: 37
        public bool isInfinite()
        {
            return infinite;
        }
        // Java: 41
        public bool hasEnoughSupplies()
        {
            for (int i = 0; i < cost.getSupplyTypes().Length; i++)
            {
                if (building.getSupplyContainer(cost.getSupplyTypes()[i]).getNumSupplies() < cost.getSupplyAmounts()[i])
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 50
        public void build(float delta)
        {
            man_seconds += delta;
            if (man_seconds >= man_seconds_per_production)
            {
                man_seconds = 0;
                if (!dest_container.isSupplyFull())
                {
                    for (int i = 0; i < cost.getSupplyTypes().Length; i++)
                    {
                        building.getSupplyContainer(cost.getSupplyTypes()[i]).increaseSupply(-cost.getSupplyAmounts()[i]);
                    }
                    if (!infinite)
                    {
                        increaseSupply((-1));
                    }
                    dest_container.increaseSupply(1);
                }
                else
                {
                    stopProduction();
                }
            }
        }
        // Java: 67
        public float getBuildProgress()
        {
            return (float)(man_seconds / man_seconds_per_production);
        }
        // Java: 71
        public void stopProduction()
        {
            increaseSupply(-getNumSupplies());
        }
        private void init_BuildProductionContainer()
        {
            this.man_seconds = 0;
            this.infinite = false;
        }
        public BuildProductionContainer(int max_supply_count, com.oddlabs.tt.model.SupplyContainer dest_container, com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.Cost cost, float man_seconds_per_production) : base(max_supply_count)
        {
            init_BuildProductionContainer();
            this.dest_container = dest_container;
            this.building = building;
            this.cost = cost;
            this.man_seconds_per_production = man_seconds_per_production;
        }
    }
}
