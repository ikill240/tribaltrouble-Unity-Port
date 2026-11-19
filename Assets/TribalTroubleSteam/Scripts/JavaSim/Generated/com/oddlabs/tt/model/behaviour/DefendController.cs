// Translated from DefendController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class DefendController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.util.Target target;
        // Java: 16
        public override void decide()
        {
            if (shouldGiveUp(0))
            {
                unit.popController();
            }
            else
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, target, 0.0f, true));
            }
        }
        public DefendController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target t) : base(1)
        {
            this.unit = unit;
            this.target = t;
        }
    }
}
