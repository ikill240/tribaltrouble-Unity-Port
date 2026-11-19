// Translated from GridNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 7
    public class GridNode : com.oddlabs.tt.pathfinder.Node
    {
        public static com.oddlabs.tt.pathfinder.DirectionNode[][] dir_node_grid;
        public static com.oddlabs.tt.pathfinder.GridNode[][] pathfinder_grid;
        public int local_grid_x;
        public int local_grid_y;
        public com.oddlabs.tt.pathfinder.GridNode.Offset offset;
        // Java: 36
        public static com.oddlabs.tt.pathfinder.DirectionNode lookupDirectionNode(int dx, int dy)
        {
            return com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[dx + 1][dy + 1];
        }
        // Java: 40
        public override com.oddlabs.tt.pathfinder.PathNode newPath()
        {
            com.oddlabs.tt.pathfinder.Node graph_node = this;
            com.oddlabs.tt.pathfinder.GridPathNode current_node = null;
            while (graph_node.getParent() != null)
            {
                com.oddlabs.tt.pathfinder.Node parent = graph_node.getParent();
                int dx = graph_node.getGridX() - parent.getGridX();
                int dy = graph_node.getGridY() - parent.getGridY();
                graph_node = parent;
                com.oddlabs.tt.pathfinder.DirectionNode dir = com.oddlabs.tt.pathfinder.GridNode.lookupDirectionNode(dx, dy);
                current_node = new com.oddlabs.tt.pathfinder.GridPathNode(current_node, dir);
            }
            return current_node;
        }
        // Java: 60
        public override int getGridX()
        {
            return local_grid_x + offset.offset_x_f;
        }
        // Java: 65
        public override int getGridY()
        {
            return local_grid_y + offset.offset_y_f;
        }
        // Java: 70
        public static com.oddlabs.tt.pathfinder.GridNode.Offset setupPathFinding(int src_grid_x, int src_grid_y, int dst_grid_x, int dst_grid_y)
        {
            if (java.lang.Math.abs(dst_grid_x - src_grid_x) >= com.oddlabs.tt.pathfinder.RegionBuilder.GRID_SIZE && java.lang.Math.abs(dst_grid_y - src_grid_y) >= com.oddlabs.tt.pathfinder.RegionBuilder.GRID_SIZE)
            {
                return null;
            }
            int path_offset_x = (dst_grid_x + src_grid_x - com.oddlabs.tt.pathfinder.RegionBuilder.GRID_SIZE) / 2;
            int path_offset_y = (dst_grid_y + src_grid_y - com.oddlabs.tt.pathfinder.RegionBuilder.GRID_SIZE) / 2;
            return new com.oddlabs.tt.pathfinder.GridNode.Offset(path_offset_x, path_offset_y);
        }
        // Java: 79
        public static com.oddlabs.tt.pathfinder.GridNode getPathfinderNode(com.oddlabs.tt.pathfinder.GridNode.Offset offset, int x, int y)
        {
            com.oddlabs.tt.pathfinder.GridNode node = com.oddlabs.tt.pathfinder.GridNode.getPathfinderNodeOffset(x - offset.offset_x_f, y - offset.offset_y_f);
            if (node != null)
            {
                node.offset = offset;
            }
            return node;
        }
        // Java: 86
        public static com.oddlabs.tt.pathfinder.GridNode getPathfinderNodeOffset(int local_x, int local_y)
        {
            if (local_x < 0 || local_x >= com.oddlabs.tt.pathfinder.GridNode.pathfinder_grid.Length || local_y < 0 || local_y >= com.oddlabs.tt.pathfinder.GridNode.pathfinder_grid.Length)
            {
                return null;
            }
            else
            {
                return com.oddlabs.tt.pathfinder.GridNode.pathfinder_grid[local_y][local_x];
            }
        }
        // Java: 94
        public bool addNeighbour(com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int x, int y, int cost)
        {
            com.oddlabs.tt.pathfinder.GridNode node = com.oddlabs.tt.pathfinder.GridNode.getPathfinderNode(offset, x, y);
            if (node == null || node.isVisited())
            {
                return false;
            }
            com.oddlabs.tt.pathfinder.Occupant occupant = unit_grid.getOccupant(node.getGridX(), node.getGridY());
            if (occupant != null)
            {
                if (finder.touchNeighbour(occupant))
                {
                    return true;
                }
                int penalty = occupant.getPenalty();
                if (penalty >= com.oddlabs.tt.pathfinder.Occupant.STATIC)
                {
                    return false;
                }
                cost += penalty;
            }
            com.oddlabs.tt.pathfinder.PathFinder.addToOpenList(finder, node, this, cost);
            return false;
        }
        // Java: 112
        public override bool addNeighbours(com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.UnitGrid unit_grid)
        {
            int x = getGridX();
            int y = getGridY();
            return addNeighbour(finder, unit_grid, x, y - 1, com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT) || addNeighbour(finder, unit_grid, x - 1, y, com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT) || addNeighbour(finder, unit_grid, x + 1, y, com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT) || addNeighbour(finder, unit_grid, x, y + 1, com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT) || addNeighbour(finder, unit_grid, x - 1, y - 1, com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL) || addNeighbour(finder, unit_grid, x + 1, y - 1, com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL) || addNeighbour(finder, unit_grid, x - 1, y + 1, com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL) || addNeighbour(finder, unit_grid, x + 1, y + 1, com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
        }
        // Java: 126
        public class Offset : java.lang.Object
        {
            public int offset_x() { return offset_x_f; }
            public int offset_y() { return offset_y_f; }
            public Offset(int offset_x, int offset_y)
            {
                this.offset_x_f = offset_x;
                this.offset_y_f = offset_y;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Offset r)) return false; return true && java.lang.JO.equals(offset_x_f, r.offset_x_f) && java.lang.JO.equals(offset_y_f, r.offset_y_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(offset_x_f); h = 31 * h + java.lang.JO.hashCode(offset_y_f); } return h; }
            public override string toString() { return "Offset[" + "offset_x=" + java.lang.JO.toString(offset_x_f) + ", offset_y=" + java.lang.JO.toString(offset_y_f) + "]"; }
            public int offset_x_f;
            public int offset_y_f;
        }
        static GridNode()
        {
            GridNode.dir_node_grid = java.lang.JA.newArray2<com.oddlabs.tt.pathfinder.DirectionNode>(3, 3);
            {
                com.oddlabs.tt.pathfinder.GridNode.pathfinder_grid = java.lang.JA.newArray2<com.oddlabs.tt.pathfinder.GridNode>(com.oddlabs.tt.pathfinder.RegionBuilder.GRID_SIZE, com.oddlabs.tt.pathfinder.RegionBuilder.GRID_SIZE);
                for (int y = 0; y < com.oddlabs.tt.pathfinder.GridNode.pathfinder_grid.Length; y++)
                {
                    for (int x = 0; x < com.oddlabs.tt.pathfinder.GridNode.pathfinder_grid.Length; x++)
                    {
                        com.oddlabs.tt.pathfinder.GridNode.pathfinder_grid[y][x] = new com.oddlabs.tt.pathfinder.GridNode(x, y);
                    }
                }
                int unit = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
                float inv_unit = (float)(1.0f / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
                float inv_sqrt_2 = (float)(1.0f / ((float)(java.lang.Math.sqrt(unit * unit + unit * unit))));
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[0][0] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_sqrt_2, (-1), (-1));
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[0][1] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_unit, (-1), 0);
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[0][2] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_sqrt_2, (-1), 1);
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[1][0] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_unit, 0, (-1));
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[1][1] = null;
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[1][2] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_unit, 0, 1);
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[2][0] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_sqrt_2, 1, (-1));
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[2][1] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_unit, 1, 0);
                com.oddlabs.tt.pathfinder.GridNode.dir_node_grid[2][2] = new com.oddlabs.tt.pathfinder.DirectionNode(inv_sqrt_2, 1, 1);
            }
        }
        public GridNode(int x, int y) : base()
        {
            this.local_grid_x = x;
            this.local_grid_y = y;
        }
    }
}
