// Translated from SelectTowerTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 10
    public class SelectTowerTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public com.oddlabs.tt.model.Building tower;
        // Java: 29
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.model.Building building = tutorial.getViewer().getSelection().getCurrentSelection().getBuilding();
            if (building != null && building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                tutorial.next(new com.oddlabs.tt.tutorial.UnitInTowerTrigger(building));
            }
        }
        private void init_SelectTowerTrigger()
        {
            this.tower = null;
        }
        public SelectTowerTrigger(com.oddlabs.tt.player.Player player) : base(0.1f, 0.0f, "select_tower")
        {
            init_SelectTowerTrigger();
            player.enableRepairing(false);
            player.enableAttacking(false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false);
            player.enableHarvesting(false);
            player.enableWeapons(false);
            player.enableArmies(false);
            player.enableTransporting(false);
            player.enableRallyPoints(false);
            player.enableChieftains(false);
            player.enableTowerExits(false);
        }
    }
}
