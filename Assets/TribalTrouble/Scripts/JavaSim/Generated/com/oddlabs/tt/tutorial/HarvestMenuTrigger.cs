// Translated from HarvestMenuTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class HarvestMenuTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 12
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if (tutorial.getViewer().getPanel().inHarvestMenu())
            {
                tutorial.next(new com.oddlabs.tt.tutorial.SupplyTrigger(tutorial.getViewer().getLocalPlayer()));
            }
        }
        public HarvestMenuTrigger(com.oddlabs.tt.player.Player local_player) : base(0.1f, 0.0f, "harvest_menu")
        {
            local_player.enableHarvesting(true);
        }
    }
}
