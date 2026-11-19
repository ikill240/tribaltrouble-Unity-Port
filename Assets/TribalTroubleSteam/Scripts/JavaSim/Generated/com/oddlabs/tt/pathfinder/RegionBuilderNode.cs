// Translated from RegionBuilderNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 5
    public class RegionBuilderNode : java.lang.Object
    {
        public int x;
        public int y;
        public int total_cost;
        public bool visited;
        // Java: 17
        public com.oddlabs.tt.pathfinder.Region getRegion(com.oddlabs.tt.pathfinder.UnitGrid unit_grid)
        {
            return unit_grid.getRegion(x, y);
        }
        // Java: 25
        public int getGridX()
        {
            return x;
        }
        // Java: 37
        public int getGridY()
        {
            return y;
        }
        // Java: 41
        public void setVisited(bool visited)
        {
            this.visited = visited;
        }
        // Java: 45
        public void setTotalCost(int total_cost)
        {
            this.total_cost = total_cost;
        }
        // Java: 49
        public int getTotalCost()
        {
            return total_cost;
        }
        // Java: 53
        public bool isVisited()
        {
            return visited;
        }
        public RegionBuilderNode(int x, int y) : base()
        {
            this.x = x;
            this.y = y;
            total_cost = java.lang.Integer.MAX_VALUE;
        }
    }
}
