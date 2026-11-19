// Translated from GridPathFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 5
    public abstract class GridPathFinder : com.oddlabs.tt.pathfinder.AStarAlgorithm
    {
        public com.oddlabs.tt.pathfinder.Node dst_region;
        public com.oddlabs.tt.pathfinder.Node dst_region2;
        // Java: 16
        public override bool isPathComplete(int dist_squared, com.oddlabs.tt.pathfinder.Node node)
        {
            com.oddlabs.tt.pathfinder.GridNode grid_node = ((com.oddlabs.tt.pathfinder.GridNode)node);
            com.oddlabs.tt.pathfinder.Region region = getUnitGrid().getRegion(grid_node.getGridX(), grid_node.getGridY());
            return ((object)region == (object)dst_region) || ((object)region == (object)dst_region2);
        }
        public GridPathFinder(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.Node dst_region, com.oddlabs.tt.pathfinder.Node dst_region2, int dst_x, int dst_y, bool allow_second_best) : base(unit_grid, dst_x, dst_y, allow_second_best)
        {
            this.dst_region = dst_region;
            this.dst_region2 = dst_region2;
        }
    }
}
