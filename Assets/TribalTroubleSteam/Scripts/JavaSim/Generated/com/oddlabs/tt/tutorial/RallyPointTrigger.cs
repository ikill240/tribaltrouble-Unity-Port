// Translated from RallyPointTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class RallyPointTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 11
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(tutorial.getViewer().getSelection().getCurrentSelection().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b))
                {
                    if (b.hasRallyPoint())
                    {
                        tutorial.next(new com.oddlabs.tt.tutorial.UnitCountTrigger(30));
                    }
                }
            }
        }
        public RallyPointTrigger() : base(1.0f, 0.0f, "rally_point")
        {
        }
    }
}
