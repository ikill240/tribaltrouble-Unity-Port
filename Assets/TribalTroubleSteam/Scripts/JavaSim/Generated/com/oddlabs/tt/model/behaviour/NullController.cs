// Translated from NullController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 7
    public class NullController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Selectable selectable;
        // Java: 15
        public override string getKey()
        {
            return java.lang.JStr.valueOf(base.getKey()) + java.lang.JStr.valueOf(selectable.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES)) + java.lang.JStr.valueOf(selectable.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE)) + java.lang.JStr.valueOf(selectable.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK)) + java.lang.JStr.valueOf(selectable.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SAIL));
        }
        // Java: 23
        public override void decide()
        {
            selectable.setBehaviour(new com.oddlabs.tt.model.behaviour.NullBehaviour());
        }
        public NullController(com.oddlabs.tt.model.Selectable s) : base(0)
        {
            this.selectable = s;
        }
    }
}
