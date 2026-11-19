// Translated from DieController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class DieController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        // Java: 14
        public override void decide()
        {
            unit.setBehaviour(new com.oddlabs.tt.model.behaviour.DieBehaviour(unit));
        }
        public DieController(com.oddlabs.tt.model.Unit unit) : base(0)
        {
            this.unit = unit;
        }
    }
}
