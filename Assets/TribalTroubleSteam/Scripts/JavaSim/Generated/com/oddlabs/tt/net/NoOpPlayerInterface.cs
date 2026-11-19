// Translated from NoOpPlayerInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 16
    public class NoOpPlayerInterface : java.lang.Object, com.oddlabs.tt.player.PlayerInterface
    {
        // Java: 17
        public void deployUnits(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, int num_units)
        {
        }
        // Java: 21
        public void createHarvesters(com.oddlabs.tt.model.Building building, int num_tree, int num_rock, int num_iron, int num_rubber)
        {
        }
        // Java: 25
        public void buildRockWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
        }
        // Java: 29
        public void buildIronWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
        }
        // Java: 33
        public void buildRubberWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
        }
        // Java: 37
        public void doMagic(com.oddlabs.tt.model.Unit chieftain, int magic)
        {
        }
        // Java: 41
        public void exitTower(com.oddlabs.tt.model.Building building)
        {
        }
        // Java: 45
        public void trainChieftain(com.oddlabs.tt.model.Building building, bool start)
        {
        }
        // Java: 49
        public void deployChieftainFromShip(com.oddlabs.tt.model.Building building)
        {
        }
        // Java: 53
        public void placeBuilding(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y)
        {
        }
        // Java: 58
        public void setRallyPoint(com.oddlabs.tt.model.Building building, com.oddlabs.tt.util.Target target)
        {
        }
        // Java: 62
        public void setTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
        }
        // Java: 67
        public void setRallyPoint(com.oddlabs.tt.model.Building building, int grid_x, int grid_y)
        {
        }
        // Java: 71
        public void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target)
        {
        }
        // Java: 75
        public void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y)
        {
        }
        // Java: 79
        public void setLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive)
        {
        }
        // Java: 84
        public void recallGatherers(com.oddlabs.tt.model.Building building, System.Type supply_type, int amount)
        {
        }
        // Java: 88
        public void setPreferredGamespeed(int speed)
        {
        }
        // Java: 92
        public void changePreferredGamespeed(int delta)
        {
        }
        // Java: 96
        public void viewCamera(float target_x, float target_y, float target_z, float horiz_angle, float vert_angle)
        {
        }
        // Java: 100
        public void viewCursor(float x, float y, bool on_map)
        {
        }
        // Java: 104
        public void viewSelection(com.oddlabs.tt.model.Selectable[] selection)
        {
        }
        // Java: 108
        public void viewMapMode(bool on)
        {
        }
        // Java: 112
        public void viewTargeting(bool on)
        {
        }
        // Java: 116
        public void viewPanelMenu(int submenu)
        {
        }
        // Java: 120
        public void viewPlacing(int building_index, int grid_x, int grid_y, bool placing)
        {
        }
        // Java: 124
        public void viewSelectionBox(float x1, float y1, float x2, float y2, bool active)
        {
        }
        // Java: 128
        public void viewBeacon(float x, float y)
        {
        }
        public NoOpPlayerInterface() : base()
        {
        }
    }
}
