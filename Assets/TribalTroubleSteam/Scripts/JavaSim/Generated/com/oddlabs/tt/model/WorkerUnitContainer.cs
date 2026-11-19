// Translated from WorkerUnitContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public class WorkerUnitContainer : com.oddlabs.tt.model.UnitContainer
    {
        public com.oddlabs.tt.model.Building building;
        // Java: 14
        public override void enter(com.oddlabs.tt.model.Unit unit)
        {
            unit.removeNow();
            increaseSupply(1);
        }
        // Java: 21
        public override bool canEnter(com.oddlabs.tt.model.Unit unit)
        {
            return getTotalSupplies() != getMaxSupplyCount();
        }
        // Java: 26
        public int getTotalSupplies()
        {
            return getNumSupplies() + getNumPreparing();
        }
        // Java: 31
        public override com.oddlabs.tt.model.Unit exit()
        {
            increaseSupply((-1));
            return null;
        }
        // Java: 38
        public override int increaseSupply(int amount)
        {
            int result = building.getOwner().getUnitCountContainer().increaseSupply(amount);
            return base.increaseSupply(amount);
        }
        public WorkerUnitContainer(com.oddlabs.tt.model.Building building) : base(building.getOwner().getWorld().getMaxUnitCount())
        {
            this.building = building;
        }
    }
}
