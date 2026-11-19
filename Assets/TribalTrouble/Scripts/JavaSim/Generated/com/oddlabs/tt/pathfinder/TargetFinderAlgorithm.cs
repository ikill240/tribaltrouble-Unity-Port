// Translated from TargetFinderAlgorithm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public class TargetFinderAlgorithm : com.oddlabs.tt.pathfinder.GridPathFinder
    {
        public com.oddlabs.tt.pathfinder.FinderFilter filter;
        public com.oddlabs.tt.pathfinder.Occupant target;
        // Java: 17
        public com.oddlabs.tt.pathfinder.Occupant getOccupant()
        {
            return target;
        }
        // Java: 21
        public override bool touchNeighbour(com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if (filter.acceptOccupant(occ))
            {
                target = ((com.oddlabs.tt.pathfinder.Occupant)occ);
                return true;
            }
            else
            {
                return false;
            }
        }
        public TargetFinderAlgorithm(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.FinderFilter filter, com.oddlabs.tt.pathfinder.Node dst_region, int dst_x, int dst_y, bool allow_second_best) : base(unit_grid, dst_region, null, dst_x, dst_y, allow_second_best)
        {
            this.filter = filter;
        }
    }
}
