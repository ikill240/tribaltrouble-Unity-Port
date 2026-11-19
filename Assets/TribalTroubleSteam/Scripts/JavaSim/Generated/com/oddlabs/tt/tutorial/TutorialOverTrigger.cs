// Translated from TutorialOverTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 7
    public class TutorialOverTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 13
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.player.Player[] players = tutorial.getViewer().getWorld().getPlayers();
            com.oddlabs.tt.player.Player local_player = tutorial.getViewer().getLocalPlayer();
            foreach (com.oddlabs.tt.player.Player current in players)
            {
                int units = current.getUnitCountContainer().getNumSupplies();
                int buildings = current.getBuildingCountContainer().getNumSupplies();
                if (units == 0 && (((object)current == (object)local_player) || buildings == 0))
                {
                    tutorial.done(com.oddlabs.tt.form.TutorialForm.TUTORIAL_BATTLE);
                }
            }
        }
        public TutorialOverTrigger() : base(1.0f, 0.0f, "tutorial_over")
        {
        }
    }
}
