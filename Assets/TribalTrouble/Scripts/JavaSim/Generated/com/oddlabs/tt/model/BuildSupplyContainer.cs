// Translated from BuildSupplyContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public abstract class BuildSupplyContainer : com.oddlabs.tt.model.SupplyContainer
    {
        public int num_orders;
        // Java: 10
        public virtual void orderSupply(int amount)
        {
            orderSupply(amount, amount);
        }
        // Java: 14
        public void orderSupply(int amount, int orders)
        {
            increaseSupply(amount);
            num_orders += orders;
        }
        // Java: 19
        public int getNumOrders()
        {
            return num_orders;
        }
        private void init_BuildSupplyContainer()
        {
            this.num_orders = 0;
        }
        public BuildSupplyContainer(int max_supply_count) : base(max_supply_count)
        {
            init_BuildSupplyContainer();
        }
    }
}
