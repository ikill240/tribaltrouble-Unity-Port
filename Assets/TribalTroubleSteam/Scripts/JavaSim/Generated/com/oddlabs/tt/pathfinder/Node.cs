// Translated from Node.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public abstract class Node : java.lang.Object
    {
        public int cost;
        public int total_cost;
        public com.oddlabs.tt.pathfinder.Node parent;
        public bool visited;
        // Java: 35
        public com.oddlabs.tt.pathfinder.Node getParent()
        {
            return parent;
        }
        // Java: 39
        public void reset()
        {
            visited = false;
        }
        // Java: 43
        public void setPathInitial(int initial_cost)
        {
            cost = 0;
            parent = null;
            setTotalCost(initial_cost);
        }
        // Java: 49
        public void setPath(com.oddlabs.tt.pathfinder.Node parent, int cost, int estimated_cost)
        {
            this.cost = parent.cost + cost;
            this.parent = parent;
            setTotalCost(this.cost + estimated_cost);
        }
        // Java: 55
        public void setVisited(bool visited)
        {
            this.visited = visited;
        }
        // Java: 59
        public void setTotalCost(int total_cost)
        {
            setVisited(true);
            this.total_cost = total_cost;
        }
        // Java: 65
        public int getTotalCost()
        {
            return total_cost;
        }
        // Java: 69
        public bool isVisited()
        {
            return visited;
        }
        // Java: 73
        public int estimateCost(int dest_x, int dest_y)
        {
            int vec_x = dest_x - getGridX();
            int vec_y = dest_y - getGridY();
            int abs_dx = java.lang.Math.abs(vec_x);
            int abs_dy = java.lang.Math.abs(vec_y);
            int max = default;
            int min = default;
            if (abs_dx > abs_dy)
            {
                max = abs_dx;
                min = abs_dy;
            }
            else
            {
                max = abs_dy;
                min = abs_dx;
            }
            return com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT * (max - min) + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL * min;
        }
        // Java: 92
        public abstract bool addNeighbours(com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.UnitGrid unit_grid);
        // Java: 94
        public abstract com.oddlabs.tt.pathfinder.PathNode newPath();
        // Java: 96
        public abstract int getGridX();
        // Java: 98
        public abstract int getGridY();
        public Node() : base()
        {
            reset();
        }
    }
}
