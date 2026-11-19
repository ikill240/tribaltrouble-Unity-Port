// Translated from StartingSeaSpotFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public class StartingSeaSpotFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public int clearance;
        public int[] spot;
        // Java: 17
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 22
        public int getMaxRadius()
        {
            return grid.getGridSize();
        }
        // Java: 27
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occupant)
        {
            if (!grid.isDeepWater(grid_x, grid_y) || grid.getRegion(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.SEA) == null)
            {
                return false;
            }
            for (int y = grid_y - clearance; y <= grid_y + clearance; y++)
            {
                for (int x = grid_x - clearance; x <= grid_x + clearance; x++)
                {
                    if (x < 0 || y < 0 || x >= grid.getGridSize() || y >= grid.getGridSize() || grid.isGridOccupied(x, y, com.oddlabs.tt.pathfinder.UnitGrid.SEA))
                    {
                        return false;
                    }
                }
            }
            spot = new int[] { grid_x, grid_y };
            return true;
        }
        // Java: 45
        public int[] getSpot()
        {
            return spot;
        }
        public StartingSeaSpotFilter(com.oddlabs.tt.pathfinder.UnitGrid grid, int clearance) : base()
        {
            this.grid = grid;
            this.clearance = clearance;
        }
    }
}
