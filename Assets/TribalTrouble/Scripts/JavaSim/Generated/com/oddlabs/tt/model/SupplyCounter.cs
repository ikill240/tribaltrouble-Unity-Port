// Translated from SupplyCounter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class SupplyCounter : java.lang.Object
    {
        public com.oddlabs.tt.model.Building building;
        public System.Type supply_type;
        public int delta;
        // Java: 15
        public void increaseSupply(int amount)
        {
            delta += amount;
        }
        // Java: 19
        public virtual int getNumSupplies()
        {
            if (!building.isDead())
            {
                return building.getSupplyContainer(supply_type).getNumSupplies() + delta;
            }
            else
            {
                return 0;
            }
        }
        // Java: 27
        public com.oddlabs.tt.model.Building getBuilding()
        {
            return building;
        }
        // Java: 31
        public System.Type getSupplyType()
        {
            return supply_type;
        }
        // Java: 35
        public int getDelta()
        {
            return delta;
        }
        // Java: 39
        public void setDelta(int delta)
        {
            this.delta = delta;
        }
        // Java: 43
        public int getMaxSupplies()
        {
            return (!building.isDead() ? building.getSupplyContainer(supply_type).getMaxSupplyCount() : 0);
        }
        public SupplyCounter(com.oddlabs.tt.model.Building building, System.Type supply_type) : base()
        {
            this.building = building;
            this.supply_type = supply_type;
            delta = 0;
        }
    }
}
