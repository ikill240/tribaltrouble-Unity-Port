// Translated from ChieftainBuiltTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 7
    public class ChieftainBuiltTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 13
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(tutorial.getViewer().getLocalPlayer().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                {
                    tutorial.next(new com.oddlabs.tt.tutorial.MagicTrigger(u));
                }
            }
        }
        public ChieftainBuiltTrigger() : base(0.1f, 0.0f, "chieftain_built")
        {
        }
    }
}
