// Translated from ArmyMenuTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class ArmyMenuTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 12
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if (tutorial.getViewer().getPanel().inArmyMenu())
            {
                tutorial.next(new com.oddlabs.tt.tutorial.ArmyTrigger(tutorial.getViewer().getLocalPlayer()));
            }
        }
        public ArmyMenuTrigger(com.oddlabs.tt.player.Player local_player) : base(0.1f, 1.0f, "army_menu")
        {
            local_player.enableArmies(true);
        }
    }
}
