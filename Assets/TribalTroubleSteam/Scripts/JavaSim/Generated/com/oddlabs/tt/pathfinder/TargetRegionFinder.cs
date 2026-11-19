// Translated from TargetRegionFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public class TargetRegionFinder : java.lang.Object, com.oddlabs.tt.pathfinder.PathFinderAlgorithm
    {
        public com.oddlabs.tt.pathfinder.FinderFilter filter;
        public com.oddlabs.tt.pathfinder.UnitGrid unit_grid;
        // Java: 15
        public int computeEstimatedCost(com.oddlabs.tt.pathfinder.Node node)
        {
            return 0;
        }
        // Java: 20
        public bool touchNeighbour(com.oddlabs.tt.pathfinder.Occupant occ)
        {
            return false;
        }
        // Java: 25
        public com.oddlabs.tt.pathfinder.NodeResult touchNode(com.oddlabs.tt.pathfinder.Node node)
        {
            com.oddlabs.tt.pathfinder.Region region = ((com.oddlabs.tt.pathfinder.Region)node);
            com.oddlabs.tt.pathfinder.Occupant occ = filter.getOccupantFromRegion(region, false);
            if (occ != null)
            {
                return new com.oddlabs.tt.pathfinder.NodeResult(unit_grid.getRegion(occ.getGridX(), occ.getGridY()));
            }
            else
            {
                return null;
            }
        }
        // Java: 35
        public com.oddlabs.tt.pathfinder.NodeResult getBestNode()
        {
            com.oddlabs.tt.pathfinder.Occupant occ = filter.getBest();
            if (occ != null)
            {
                return new com.oddlabs.tt.pathfinder.NodeResult(unit_grid.getRegion(occ.getGridX(), occ.getGridY()));
            }
            else
            {
                return null;
            }
        }
        public TargetRegionFinder(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.FinderFilter filter) : base()
        {
            this.unit_grid = unit_grid;
            this.filter = filter;
        }
    }
}
