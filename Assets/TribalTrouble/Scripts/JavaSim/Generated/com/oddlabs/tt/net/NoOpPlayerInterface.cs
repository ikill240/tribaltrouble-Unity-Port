// Translated from NoOpPlayerInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 17
    public class NoOpPlayerInterface : java.lang.Object, com.oddlabs.tt.player.PlayerInterface
    {
        // Java: 18
        public void deployUnits(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, int num_units)
        {
        }
        // Java: 22
        public void createHarvesters(com.oddlabs.tt.model.Building building, int num_tree, int num_rock, int num_iron, int num_rubber)
        {
        }
        // Java: 26
        public void buildRockWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
        }
        // Java: 30
        public void buildIronWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
        }
        // Java: 34
        public void buildRubberWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
        }
        // Java: 38
        public void doMagic(com.oddlabs.tt.model.Unit chieftain, int magic)
        {
        }
        // Java: 42
        public void exitTower(com.oddlabs.tt.model.Building building)
        {
        }
        // Java: 46
        public void standGround(com.oddlabs.tt.model.Selectable[] units)
        {
        }
        // Java: 50
        public void trainChieftain(com.oddlabs.tt.model.Building building, bool start)
        {
        }
        // Java: 54
        public void placeBuilding(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y, bool queue)
        {
        }
        // Java: 59
        public void placeBuildingFree(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y, int offset_x, int offset_y, bool queue)
        {
        }
        // Java: 64
        public void setFormation(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.player.Formation formation)
        {
        }
        // Java: 68
        public void setRallyPoint(com.oddlabs.tt.model.Building building, com.oddlabs.tt.util.Target target)
        {
        }
        // Java: 72
        public void setTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
        }
        // Java: 77
        public void setRallyPoint(com.oddlabs.tt.model.Building building, int grid_x, int grid_y)
        {
        }
        // Java: 81
        public void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target)
        {
        }
        // Java: 85
        public void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y)
        {
        }
        // Java: 89
        public void setLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive)
        {
        }
        // Java: 94
        public void queueTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
        }
        // Java: 99
        public void queueLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive)
        {
        }
        // Java: 104
        public void recallGatherers(com.oddlabs.tt.model.Building building, System.Type supply_type, int amount)
        {
        }
        // Java: 108
        public void setPreferredGamespeed(int speed)
        {
        }
        // Java: 112
        public void changePreferredGamespeed(int delta)
        {
        }
        // Java: 116
        public void islandWarsControl(int what)
        {
        }
        public NoOpPlayerInterface() : base()
        {
        }
    }
}
