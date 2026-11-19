// Translated from WalkController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 7
    public class WalkController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.util.Target target;
        public bool scan_attack;
        // Java: 19
        public override void decide()
        {
            if (shouldGiveUp(0))
            {
                unit.popController();
            }
            else
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, target, 0.0f, scan_attack));
            }
        }
        // Java: 27
        public bool isAgressive()
        {
            return scan_attack;
        }
        // Java: 31
        public com.oddlabs.tt.util.Target getTarget()
        {
            return target;
        }
        public WalkController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target t, bool scan_attack) : base(1)
        {
            this.unit = unit;
            this.target = t;
            this.scan_attack = scan_attack;
        }
    }
}
