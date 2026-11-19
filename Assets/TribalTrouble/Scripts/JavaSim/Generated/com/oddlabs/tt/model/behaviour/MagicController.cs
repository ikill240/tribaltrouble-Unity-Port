// Translated from MagicController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class MagicController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.weapon.MagicFactory magic_factory;
        public bool should_pop;
        // Java: 18
        public void popNextTime()
        {
            should_pop = true;
        }
        // Java: 22
        public override void decide()
        {
            if (should_pop)
            {
                unit.popController();
            }
            else
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.MagicBehaviour(unit, magic_factory, this));
            }
        }
        private void init_MagicController()
        {
            this.should_pop = false;
        }
        public MagicController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.weapon.MagicFactory magic_factory) : base(0)
        {
            init_MagicController();
            this.unit = unit;
            this.magic_factory = magic_factory;
        }
    }
}
