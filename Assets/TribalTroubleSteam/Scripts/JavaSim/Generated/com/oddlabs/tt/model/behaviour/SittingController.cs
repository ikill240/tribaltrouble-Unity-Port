// Translated from SittingController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 7
    public class SittingController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.behaviour.SittingBehaviour sitting_behaviour;
        // Java: 18
        public bool shouldSleep(float t)
        {
            return false;
        }
        // Java: 22
        public override void decide()
        {
            unit.setBehaviour(sitting_behaviour);
        }
        public SittingController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.ShipAllocation allocation) : base(0)
        {
            this.unit = unit;
            this.sitting_behaviour = new com.oddlabs.tt.model.behaviour.SittingBehaviour(this, unit, ship, allocation, false);
        }
    }
}
