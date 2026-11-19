// Translated from ShipUnitContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class ShipUnitContainer : com.oddlabs.tt.model.UnitContainer
    {
        public com.oddlabs.tt.model.Ship ship;
        // Java: 11
        public override void enter(com.oddlabs.tt.model.Unit unit)
        {
            com.oddlabs.tt.model.ShipAllocation allocation = ship.getShipHR().tryAllocate(unit);
            unit.mount(ship, allocation);
        }
        // Java: 16
        public override bool canEnter(com.oddlabs.tt.model.Unit unit)
        {
            return ship.getShipHR().canAllocate(unit);
        }
        // Java: 20
        public int getTotalSupplies()
        {
            return getNumSupplies() + getNumPreparing();
        }
        // Java: 24
        public override int getNumSupplies()
        {
            return ship.getShipHR().countUnits();
        }
        // Java: 28
        public override int capAmount(int amount)
        {
            int supply_count = getNumSupplies();
            return java.lang.Math.max(supply_count + amount, 0) - supply_count;
        }
        // Java: 33
        public override com.oddlabs.tt.model.Unit exit()
        {
            return null;
        }
        // Java: 37
        public override int increaseSupply(int amount)
        {
            return base.increaseSupply(amount);
        }
        // Java: 41
        public override void animate(float t)
        {
        }
        public ShipUnitContainer(com.oddlabs.tt.model.Ship ship) : base(ship.getOwner().getWorld().getMaxUnitCount())
        {
            this.ship = ship;
        }
    }
}
