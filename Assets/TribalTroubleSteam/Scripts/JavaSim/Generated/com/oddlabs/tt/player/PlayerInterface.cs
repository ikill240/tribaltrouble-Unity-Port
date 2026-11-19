// Translated from PlayerInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 12
    public interface PlayerInterface
    {
        // Java: 13
        void deployUnits(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, int num_units);
        // Java: 19
        void createHarvesters(com.oddlabs.tt.model.Building building, int num_tree, int num_rock, int num_iron, int num_rubber);
        // Java: 21
        void buildRockWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite);
        // Java: 23
        void buildIronWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite);
        // Java: 25
        void buildRubberWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite);
        // Java: 27
        void doMagic(com.oddlabs.tt.model.Unit chieftain, int magic);
        // Java: 29
        void exitTower(com.oddlabs.tt.model.Building building);
        // Java: 31
        void trainChieftain(com.oddlabs.tt.model.Building building, bool start);
        // Java: 33
        void deployChieftainFromShip(com.oddlabs.tt.model.Building building);
        // Java: 35
        void placeBuilding(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y);
        // Java: 37
        void setRallyPoint(com.oddlabs.tt.model.Building building, com.oddlabs.tt.util.Target target);
        // Java: 39
        void setTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 42
        void setRallyPoint(com.oddlabs.tt.model.Building building, int grid_x, int grid_y);
        // Java: 44
        void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target);
        // Java: 46
        void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y);
        // Java: 48
        void setLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 51
        void recallGatherers(com.oddlabs.tt.model.Building building, System.Type supply_type, int amount);
        // Java: 53
        void setPreferredGamespeed(int speed);
        // Java: 55
        void changePreferredGamespeed(int delta);
        // Java: 57
        void viewCamera(float target_x, float target_y, float target_z, float horiz_angle, float vert_angle);
        // Java: 59
        void viewCursor(float x, float y, bool on_map);
        // Java: 61
        void viewSelection(com.oddlabs.tt.model.Selectable[] selection);
        // Java: 63
        void viewMapMode(bool on);
        // Java: 65
        void viewTargeting(bool on);
        // Java: 67
        void viewPanelMenu(int submenu);
        // Java: 69
        void viewPlacing(int building_index, int grid_x, int grid_y, bool placing);
        // Java: 71
        void viewSelectionBox(float x1, float y1, float x2, float y2, bool active);
        // Java: 73
        void viewBeacon(float x, float y);
    }
}
