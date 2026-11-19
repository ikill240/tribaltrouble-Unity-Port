// Translated from SupplyContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class SupplyContainer : java.lang.Object
    {
        public int max_supply_count;
        public int supply_count;
        public int num_preparing;
        // Java: 13
        public virtual int increaseSupply(int amount)
        {
            int capped_amount = capAmount(amount);
            supply_count += capped_amount;
            return capped_amount;
        }
        // Java: 19
        public virtual int capAmount(int amount)
        {
            return java.lang.Math.clamp(supply_count + amount, 0, max_supply_count) - supply_count;
        }
        // Java: 23
        public virtual void prepareDeploy(int amount)
        {
            int result = increaseSupply(-amount);
            num_preparing += amount;
        }
        // Java: 29
        public bool isSupplyFull()
        {
            return supply_count == max_supply_count;
        }
        // Java: 33
        public virtual int getNumSupplies()
        {
            return supply_count;
        }
        // Java: 37
        public int getNumPreparing()
        {
            return num_preparing;
        }
        // Java: 41
        public int getMaxSupplyCount()
        {
            return max_supply_count;
        }
        private void init_SupplyContainer()
        {
            this.supply_count = 0;
            this.num_preparing = 0;
        }
        public SupplyContainer(int max_supply_count) : base()
        {
            init_SupplyContainer();
            this.max_supply_count = max_supply_count;
        }
    }
}
