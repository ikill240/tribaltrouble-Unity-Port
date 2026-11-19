// Translated from TargetGridPathFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public class TargetGridPathFinder : com.oddlabs.tt.pathfinder.GridPathFinder
    {
        public float max_dist_squared;
        public com.oddlabs.tt.util.Target target;
        // Java: 17
        public override bool touchNeighbour(com.oddlabs.tt.pathfinder.Occupant occ)
        {
            return ((object)occ == (object)target);
        }
        // Java: 22
        public override bool isPathComplete(int dist_squared, com.oddlabs.tt.pathfinder.Node node)
        {
            return dist_squared <= max_dist_squared || base.isPathComplete(dist_squared, node);
        }
        public TargetGridPathFinder(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, float max_dist, com.oddlabs.tt.pathfinder.Node dst_region, com.oddlabs.tt.pathfinder.Node dst_region2, int dst_x, int dst_y, com.oddlabs.tt.util.Target t, bool allow_second_best) : base(unit_grid, dst_region, dst_region2, dst_x, dst_y, allow_second_best)
        {
            this.max_dist_squared = (float)(max_dist * max_dist);
            this.target = t;
        }
    }
}
