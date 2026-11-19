// Translated from QuartersTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class QuartersTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 11
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            java.util.Set set = tutorial.getViewer().getLocalPlayer().getUnits().getSet();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(set))
            {
                if ((s is com.oddlabs.tt.model.Building))
                {
                    tutorial.next(new com.oddlabs.tt.tutorial.SelectQuartersTrigger());
                }
            }
        }
        public QuartersTrigger() : base(1.0f, 0.0f, "quarters")
        {
        }
    }
}
