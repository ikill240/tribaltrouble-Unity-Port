// Translated from SelectArmoryTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 9
    public class SelectArmoryTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 24
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.model.Building building = tutorial.getViewer().getSelection().getCurrentSelection().getBuilding();
            if (building != null && building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
            {
                tutorial.next(new com.oddlabs.tt.tutorial.HarvestMenuTrigger(tutorial.getViewer().getLocalPlayer()));
            }
        }
        public SelectArmoryTrigger(com.oddlabs.tt.player.Player player) : base(0.1f, 0.0f, "select_armory")
        {
            player.enableRepairing(false);
            player.enableAttacking(false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, false);
            player.enableHarvesting(false);
            player.enableWeapons(false);
            player.enableArmies(false);
            player.enableTransporting(false);
            player.enableRallyPoints(false);
            player.enableChieftains(false);
        }
    }
}
