// Translated from ShipSupplyContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class ShipSupplyContainer : com.oddlabs.tt.model.SupplyContainer
    {
        public com.oddlabs.tt.model.ShipHR ship_hr;
        public System.Type type;
        // Java: 13
        public override int getNumSupplies()
        {
            return ship_hr.countUnitsOfType(type);
        }
        // Java: 17
        public override int capAmount(int amount)
        {
            int supplies = getNumSupplies();
            return java.lang.Math.clamp(supplies + amount, 0, max_supply_count) - supplies;
        }
        public ShipSupplyContainer(com.oddlabs.tt.model.ShipHR ship_hr, System.Type type) : base(200)
        {
            this.ship_hr = ship_hr;
            this.type = type;
        }
    }
}
