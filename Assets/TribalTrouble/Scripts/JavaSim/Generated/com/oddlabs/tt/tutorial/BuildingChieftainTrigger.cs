// Translated from BuildingChieftainTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 9
    public class BuildingChieftainTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 25
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(tutorial.getViewer().getLocalPlayer().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b))
                {
                    com.oddlabs.tt.model.ChieftainContainer container = b.getChieftainContainer();
                    if (container != null && container.isTraining())
                    {
                        tutorial.next(new com.oddlabs.tt.tutorial.ChieftainBuiltTrigger());
                    }
                }
            }
        }
        public BuildingChieftainTrigger(com.oddlabs.tt.player.Player player) : base(1.0f, 0.0f, "building_chieftain")
        {
            player.enableRepairing(false);
            player.enableAttacking(false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, false);
            player.enableHarvesting(false);
            player.enableWeapons(false);
            player.enableArmies(false);
            player.enableTransporting(false);
            player.enableRallyPoints(false);
        }
    }
}
