// Translated from TargetTrackerAlgorithm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 8
    public class TargetTrackerAlgorithm : java.lang.Object, com.oddlabs.tt.pathfinder.TrackerAlgorithm
    {
        public com.oddlabs.tt.pathfinder.UnitGrid unit_grid;
        public com.oddlabs.tt.util.Target target;
        public float max_dist;
        // Java: 19
        public bool isDone(int x, int y)
        {
            return target.isDead() || com.oddlabs.tt.model.Selectable.isCloseEnough(unit_grid, max_dist, x, y, target);
        }
        // Java: 24
        public bool acceptRegion(com.oddlabs.tt.pathfinder.Region region)
        {
            return !target.isDead() && ((object)targetRegion() == (object)region);
        }
        // Java: 29
        public com.oddlabs.tt.pathfinder.Region findPathRegion(int src_x, int src_y)
        {
            return (target.isDead() ? (com.oddlabs.tt.pathfinder.Region)(null) : com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(unit_grid, unit_grid.getRegion(src_x, src_y), targetRegion()));
        }
        // Java: 41
        public com.oddlabs.tt.pathfinder.Region targetRegion()
        {
            int tx = target.getGridX();
            int ty = target.getGridY();
            com.oddlabs.tt.pathfinder.Region r = unit_grid.getRegion(tx, ty);
            if (r != null || !((target is com.oddlabs.tt.model.Building b)) || !com.oddlabs.tt.model.BridgeSpan.isBridge(b))
            {
                return r;
            }
            int size = unit_grid.getGridSize();
            for (int ring = 1; ring <= 4; ring++)
            {
                for (int dy = -ring; dy <= ring; dy++)
                {
                    for (int dx = -ring; dx <= ring; dx++)
                    {
                        if (java.lang.Math.max(java.lang.Math.abs(dx), java.lang.Math.abs(dy)) != ring)
                        {
                            continue;
                        }
                        int x = tx + dx;
                        int y = ty + dy;
                        if (x < 0 || y < 0 || x >= size || y >= size)
                        {
                            continue;
                        }
                        com.oddlabs.tt.pathfinder.Region n = unit_grid.getRegion(x, y);
                        if (n != null)
                        {
                            return n;
                        }
                    }
                }
            }
            return null;
        }
        // Java: 62
        public com.oddlabs.tt.pathfinder.GridPathNode findPathGrid(com.oddlabs.tt.pathfinder.Region target_region, com.oddlabs.tt.pathfinder.Region next_region, int src_x, int src_y, bool allow_secondary_targets)
        {
            return (target.isDead() ? (com.oddlabs.tt.pathfinder.GridPathNode)(null) : com.oddlabs.tt.pathfinder.PathFinder.findPathGrid(unit_grid, next_region, null, src_x, src_y, target.getGridX(), target.getGridY(), target, max_dist, allow_secondary_targets));
        }
        public TargetTrackerAlgorithm(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, float max_dist, com.oddlabs.tt.util.Target target) : base()
        {
            this.unit_grid = unit_grid;
            this.max_dist = max_dist;
            this.target = target;
        }
    }
}
