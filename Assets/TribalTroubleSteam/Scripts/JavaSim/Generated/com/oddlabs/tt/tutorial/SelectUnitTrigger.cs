// Translated from SelectUnitTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 5
    public class SelectUnitTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 10
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if (tutorial.getViewer().getSelection().getCurrentSelection().getNumUnits() > 0)
            {
                tutorial.next(new com.oddlabs.tt.tutorial.MoveUnitTrigger(tutorial.getViewer().getLocalPlayer()));
            }
        }
        public SelectUnitTrigger() : base(0.1f, 15.0f, "select_unit")
        {
        }
    }
}
