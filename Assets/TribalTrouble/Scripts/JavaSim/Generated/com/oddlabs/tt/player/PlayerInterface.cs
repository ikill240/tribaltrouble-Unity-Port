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
        // Java: 36
        void standGround(com.oddlabs.tt.model.Selectable[] units);
        // Java: 38
        void trainChieftain(com.oddlabs.tt.model.Building building, bool start);
        // Java: 40
        void placeBuilding(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y, bool queue);
        // Java: 44
        void placeBuildingFree(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y, int offset_x, int offset_y, bool queue);
        // Java: 47
        void setFormation(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.player.Formation formation);
        // Java: 49
        void setRallyPoint(com.oddlabs.tt.model.Building building, com.oddlabs.tt.util.Target target);
        // Java: 51
        void setTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 54
        void setRallyPoint(com.oddlabs.tt.model.Building building, int grid_x, int grid_y);
        // Java: 56
        void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target);
        // Java: 58
        void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y);
        // Java: 60
        void setLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 64
        void queueTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 67
        void queueLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 70
        void recallGatherers(com.oddlabs.tt.model.Building building, System.Type supply_type, int amount);
        // Java: 72
        void setPreferredGamespeed(int speed);
        // Java: 74
        void changePreferredGamespeed(int delta);
        // Java: 77
        void islandWarsControl(int what);
    }
}
