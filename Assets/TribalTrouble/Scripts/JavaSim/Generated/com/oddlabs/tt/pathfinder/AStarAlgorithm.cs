// Translated from AStarAlgorithm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public abstract class AStarAlgorithm : java.lang.Object, com.oddlabs.tt.pathfinder.PathFinderAlgorithm
    {
        public int dst_x;
        public int dst_y;
        public com.oddlabs.tt.pathfinder.Region dst_region;
        public bool allow_second_best;
        public com.oddlabs.tt.pathfinder.UnitGrid unit_grid;
        public int nodes_visited;
        public int best_dist_squared;
        public com.oddlabs.tt.pathfinder.Node second_best_node;
        // Java: 25
        public com.oddlabs.tt.pathfinder.UnitGrid getUnitGrid()
        {
            return unit_grid;
        }
        // Java: 29
        public int computeEstimatedCost(com.oddlabs.tt.pathfinder.Node node)
        {
            return node.estimateCost(dst_x, dst_y);
        }
        // Java: 34
        public abstract bool isPathComplete(int dist_squared, com.oddlabs.tt.pathfinder.Node node);
        // Java: 36
        public com.oddlabs.tt.pathfinder.NodeResult defaultTouchNode()
        {
            nodes_visited++;
            if (nodes_visited == com.oddlabs.tt.pathfinder.RegionBuilder.MAX_EXAMINED_NODES_PER_PATH)
            {
                if (allow_second_best)
                {
                    com.oddlabs.tt.pathfinder.Node result = second_best_node;
                    second_best_node = null;
                    return new com.oddlabs.tt.pathfinder.NodeResult(result);
                }
                else
                {
                    return new com.oddlabs.tt.pathfinder.NodeResult(null);
                }
            }
            else
            {
                return null;
            }
        }
        // Java: 49
        public com.oddlabs.tt.pathfinder.NodeResult touchNode(com.oddlabs.tt.pathfinder.Node node)
        {
            int dx = node.getGridX() - dst_x;
            int dy = node.getGridY() - dst_y;
            int dist_squared = dx * dx + dy * dy;
            com.oddlabs.tt.pathfinder.Region region = unit_grid.getRegion(node.getGridX(), node.getGridY());
            if (((object)region == (object)dst_region) && dist_squared < best_dist_squared)
            {
                second_best_node = node;
                best_dist_squared = dist_squared;
            }
            if (isPathComplete(dist_squared, node))
            {
                return new com.oddlabs.tt.pathfinder.NodeResult(node);
            }
            return defaultTouchNode();
        }
        // Java: 66
        public com.oddlabs.tt.pathfinder.NodeResult getBestNode()
        {
            return null;
        }
        private void init_AStarAlgorithm()
        {
            this.best_dist_squared = java.lang.Integer.MAX_VALUE;
        }
        public AStarAlgorithm(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int dst_x, int dst_y, bool allow_second_best) : base()
        {
            init_AStarAlgorithm();
            this.dst_x = dst_x;
            this.dst_y = dst_y;
            this.dst_region = unit_grid.getRegion(dst_x, dst_y);
            this.allow_second_best = allow_second_best;
            this.unit_grid = unit_grid;
        }
        public abstract bool touchNeighbour(com.oddlabs.tt.pathfinder.Occupant p0);
    }
}
