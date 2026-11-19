// Translated from PlacingDelegateTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 8
    public class PlacingDelegateTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 18
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if ((tutorial.getViewer().getGUIRoot().getDelegate() is com.oddlabs.tt.@delegate.PlacingDelegate))
            {
                tutorial.next(new com.oddlabs.tt.tutorial.QuartersTrigger());
            }
        }
        public PlacingDelegateTrigger(com.oddlabs.tt.player.Player player) : base(0.1f, 0.0f, "placing")
        {
            player.enableRepairing(false);
            player.enableAttacking(false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, false);
            player.enableChieftains(false);
        }
    }
}
