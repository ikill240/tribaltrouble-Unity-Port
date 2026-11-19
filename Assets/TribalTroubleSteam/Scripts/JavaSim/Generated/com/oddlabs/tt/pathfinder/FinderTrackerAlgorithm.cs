// Translated from FinderTrackerAlgorithm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 7
    public class FinderTrackerAlgorithm : java.lang.Object, com.oddlabs.tt.pathfinder.TrackerAlgorithm
    {
        public com.oddlabs.tt.pathfinder.FinderFilter filter;
        public com.oddlabs.tt.pathfinder.UnitGrid unit_grid;
        public com.oddlabs.tt.pathfinder.Occupant target;
        // Java: 17
        public bool isDone(int x, int y)
        {
            return target != null && !target.isDead() && com.oddlabs.tt.model.Selectable.isCloseEnough(unit_grid, 0.0f, x, y, target);
        }
        // Java: 22
        public bool acceptRegion(com.oddlabs.tt.pathfinder.Region region)
        {
            return filter.getOccupantFromRegion(region, true) != null;
        }
        // Java: 27
        public com.oddlabs.tt.pathfinder.Region findPathRegion(int src_x, int src_y)
        {
            com.oddlabs.tt.pathfinder.TargetRegionFinder region_finder = new com.oddlabs.tt.pathfinder.TargetRegionFinder(unit_grid, filter);
            com.oddlabs.tt.pathfinder.Region region = com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(unit_grid, region_finder, unit_grid.getRegion(src_x, src_y));
            if (region == null)
            {
                return null;
            }
            return region;
        }
        // Java: 36
        public com.oddlabs.tt.pathfinder.Occupant getOccupant()
        {
            return (target == null || target.isDead() ? (com.oddlabs.tt.pathfinder.Occupant)(null) : target);
        }
        // Java: 40
        public com.oddlabs.tt.pathfinder.GridPathNode findPathGrid(com.oddlabs.tt.pathfinder.Region target_region, com.oddlabs.tt.pathfinder.Region next_region, int src_x, int src_y, bool allow_secondary_targets)
        {
            com.oddlabs.tt.pathfinder.Occupant hint_occupant = filter.getOccupantFromRegion(target_region, true);
            com.oddlabs.tt.pathfinder.TargetFinderAlgorithm grid_finder = new com.oddlabs.tt.pathfinder.TargetFinderAlgorithm(unit_grid, filter, next_region, hint_occupant.getGridX(), hint_occupant.getGridY(), allow_secondary_targets);
            com.oddlabs.tt.pathfinder.GridPathNode path = com.oddlabs.tt.pathfinder.PathFinder.findPathGrid(unit_grid, grid_finder, src_x, src_y);
            target = grid_finder.getOccupant();
            return path;
        }
        public FinderTrackerAlgorithm(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.FinderFilter filter) : base()
        {
            this.unit_grid = unit_grid;
            this.filter = filter;
        }
    }
}
