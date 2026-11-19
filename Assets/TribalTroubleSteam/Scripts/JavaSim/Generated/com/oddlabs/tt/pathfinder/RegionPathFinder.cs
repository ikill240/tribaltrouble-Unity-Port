// Translated from RegionPathFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 5
    public class RegionPathFinder : com.oddlabs.tt.pathfinder.AStarAlgorithm
    {
        public com.oddlabs.tt.pathfinder.Node dst_region;
        // Java: 13
        public override bool touchNeighbour(com.oddlabs.tt.pathfinder.Occupant occ)
        {
            return false;
        }
        // Java: 18
        public override bool isPathComplete(int dist_squared, com.oddlabs.tt.pathfinder.Node node)
        {
            return ((object)node == (object)dst_region);
        }
        public RegionPathFinder(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.Node dst_region) : base(unit_grid, dst_region.getGridX(), dst_region.getGridY(), false)
        {
            this.dst_region = dst_region;
        }
    }
}
