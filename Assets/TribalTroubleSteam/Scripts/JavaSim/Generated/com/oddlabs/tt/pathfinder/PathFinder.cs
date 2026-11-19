// Translated from PathFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 11
    public class PathFinder : java.lang.Object
    {
        public static com.oddlabs.tt.util.PocketList open_list;
        public static java.util.List visited_list;
        public static int stat_pathfinder_per_frame;
        // Java: 16
        public static com.oddlabs.tt.pathfinder.Region findPathRegion(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.Region src_region, com.oddlabs.tt.pathfinder.Region dst_region)
        {
            if (src_region == null || dst_region == null)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder = new com.oddlabs.tt.pathfinder.RegionPathFinder(unit_grid, dst_region);
            return ((com.oddlabs.tt.pathfinder.Region)com.oddlabs.tt.pathfinder.PathFinder.doFindPath(finder, src_region, unit_grid));
        }
        // Java: 23
        public static com.oddlabs.tt.pathfinder.Region findPathRegion(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.Region current_region)
        {
            if (current_region == null)
            {
                return null;
            }
            return ((com.oddlabs.tt.pathfinder.Region)com.oddlabs.tt.pathfinder.PathFinder.doFindPath(finder, current_region, unit_grid));
        }
        // Java: 29
        public static com.oddlabs.tt.pathfinder.GridPathNode findPathGrid(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, int src_grid_x, int src_grid_y)
        {
            com.oddlabs.tt.pathfinder.GridNode.Offset offset = com.oddlabs.tt.pathfinder.GridNode.setupPathFinding(src_grid_x, src_grid_y, src_grid_x, src_grid_y);
            if (offset == null)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.Node current_node = com.oddlabs.tt.pathfinder.GridNode.getPathfinderNode(offset, src_grid_x, src_grid_y);
            com.oddlabs.tt.pathfinder.Node grid_node = com.oddlabs.tt.pathfinder.PathFinder.doFindPath(finder, current_node, unit_grid);
            return (grid_node != null ? ((com.oddlabs.tt.pathfinder.GridPathNode)grid_node.newPath()) : (com.oddlabs.tt.pathfinder.GridPathNode)(null));
        }
        // Java: 39
        public static com.oddlabs.tt.pathfinder.GridPathNode findPathGrid(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.Region dst_region, com.oddlabs.tt.pathfinder.Region dst_region2, int src_grid_x, int src_grid_y, int dst_grid_x, int dst_grid_y, com.oddlabs.tt.util.Target target, float max_dist, bool allow_second_best)
        {
            com.oddlabs.tt.pathfinder.GridNode.Offset offset = com.oddlabs.tt.pathfinder.GridNode.setupPathFinding(src_grid_x, src_grid_y, src_grid_x, src_grid_y);
            if (offset == null)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.Node current_node = com.oddlabs.tt.pathfinder.GridNode.getPathfinderNode(offset, src_grid_x, src_grid_y);
            com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder = new com.oddlabs.tt.pathfinder.TargetGridPathFinder(unit_grid, max_dist, dst_region, dst_region2, dst_grid_x, dst_grid_y, target, allow_second_best);
            com.oddlabs.tt.pathfinder.Node grid_node = com.oddlabs.tt.pathfinder.PathFinder.doFindPath(finder, current_node, unit_grid);
            return (grid_node != null ? ((com.oddlabs.tt.pathfinder.GridPathNode)grid_node.newPath()) : (com.oddlabs.tt.pathfinder.GridPathNode)(null));
        }
        // Java: 52
        public static com.oddlabs.tt.pathfinder.Node doFindPath(com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.Node start_node, com.oddlabs.tt.pathfinder.UnitGrid unit_grid)
        {
            if (start_node == null)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.Node current_node = start_node;
            com.oddlabs.tt.pathfinder.PathFinder.stat_pathfinder_per_frame++;
            com.oddlabs.tt.pathfinder.PathFinder.initSearch();
            current_node.setPathInitial(finder.computeEstimatedCost(current_node));
            com.oddlabs.tt.pathfinder.PathFinder.addToLists(current_node);
            while (!com.oddlabs.tt.pathfinder.PathFinder.open_list.isEmpty())
            {
                current_node = ((com.oddlabs.tt.pathfinder.Node)com.oddlabs.tt.pathfinder.PathFinder.open_list.removeBest());
                com.oddlabs.tt.pathfinder.NodeResult result = finder.touchNode(current_node);
                if (result != null)
                {
                    return result.get();
                }
                bool neighbour_result = current_node.addNeighbours(finder, unit_grid);
                if (neighbour_result)
                {
                    return current_node;
                }
            }
            com.oddlabs.tt.pathfinder.NodeResult result_2 = finder.getBestNode();
            return (result_2 != null ? result_2.get() : (com.oddlabs.tt.pathfinder.Node)(null));
        }
        // Java: 73
        public static void addToOpenList(com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.Node current_node, com.oddlabs.tt.pathfinder.Node parent, int cost)
        {
            current_node.setPath(parent, cost, finder.computeEstimatedCost(current_node));
            com.oddlabs.tt.pathfinder.PathFinder.addToLists(current_node);
        }
        // Java: 79
        public static void addToLists(com.oddlabs.tt.pathfinder.Node current_node)
        {
            com.oddlabs.tt.pathfinder.PathFinder.open_list.add(current_node.getTotalCost(), current_node);
            com.oddlabs.tt.pathfinder.PathFinder.visited_list.add(current_node);
        }
        // Java: 84
        public static void initSearch()
        {
            com.oddlabs.tt.pathfinder.PathFinder.open_list.clear();
            foreach (com.oddlabs.tt.pathfinder.Node node in java.lang.JIter.of(com.oddlabs.tt.pathfinder.PathFinder.visited_list))
            {
                node.reset();
            }
            com.oddlabs.tt.pathfinder.PathFinder.visited_list.clear();
        }
        static PathFinder()
        {
            PathFinder.open_list = new com.oddlabs.tt.util.PocketList(com.oddlabs.tt.pathfinder.RegionBuilder.MAX_PATH_COST);
            PathFinder.visited_list = new java.util.ArrayList();
            PathFinder.stat_pathfinder_per_frame = 0;
        }
        public PathFinder() : base()
        {
        }
    }
}
