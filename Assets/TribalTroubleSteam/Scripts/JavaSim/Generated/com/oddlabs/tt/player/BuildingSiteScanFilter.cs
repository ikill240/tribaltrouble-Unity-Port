// Translated from BuildingSiteScanFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 14
    public class BuildingSiteScanFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public com.oddlabs.tt.pathfinder.UnitGrid unit_grid;
        public com.oddlabs.tt.model.BuildingTemplate template;
        public int range;
        public int island;
        public bool check_island;
        public bool one_target;
        public java.util.List result;
        // Java: 42
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 47
        public int getMaxRadius()
        {
            return range;
        }
        // Java: 52
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            bool legal = template.isPlacingLegal(unit_grid, grid_x, grid_y);
            com.oddlabs.tt.landscape.HeightMap map = unit_grid.getHeightMap();
            if (check_island)
            {
                int curr_island = map.getIslandId(grid_x, grid_y);
                if (curr_island != island)
                {
                    return false;
                }
            }
            bool can_build = map.canBuild(grid_x, grid_y, template.getPlacingSize()) && template.getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING;
            bool can_dock = map.canDock(grid_x, grid_y) && (unit_grid.getRegion(grid_x, grid_y) != null) && template.getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_SHIP;
            if ((can_build || can_dock) && legal)
            {
                result.add(new com.oddlabs.tt.landscape.LandscapeTarget(grid_x, grid_y));
                if (one_target)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 74
        public java.util.List getResult()
        {
            return result;
        }
        private void init_BuildingSiteScanFilter()
        {
            this.result = new java.util.ArrayList();
        }
        public BuildingSiteScanFilter(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.model.BuildingTemplate template, int range, bool one_target) : base()
        {
            init_BuildingSiteScanFilter();
            this.unit_grid = unit_grid;
            this.template = template;
            this.range = range;
            this.one_target = one_target;
            this.island = 0;
            this.check_island = false;
        }
        public BuildingSiteScanFilter(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.model.BuildingTemplate template, int range, bool one_target, int island) : base()
        {
            init_BuildingSiteScanFilter();
            this.unit_grid = unit_grid;
            this.template = template;
            this.range = range;
            this.one_target = one_target;
            this.island = island;
            this.check_island = true;
        }
    }
}
