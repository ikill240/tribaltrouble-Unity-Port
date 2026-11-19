// Translated from MoveUnitTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 8
    public class MoveUnitTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 14
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(tutorial.getViewer().getSelection().getCurrentSelection().getSet()))
            {
                if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController))
                {
                    tutorial.done(com.oddlabs.tt.form.TutorialForm.TUTORIAL_CAMERA);
                }
            }
        }
        public MoveUnitTrigger(com.oddlabs.tt.player.Player local_player) : base(1.0f, 2.0f, "move_unit")
        {
            local_player.enableMoving(true);
        }
    }
}
