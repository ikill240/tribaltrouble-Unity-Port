// Translated from FindTargetsFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 8
    public class FindTargetsFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public int max_radius;
        public com.oddlabs.tt.util.Target[] result;
        public bool grid_targets_only;
        public int index;
        // Java: 22
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 27
        public int getMaxRadius()
        {
            return max_radius;
        }
        // Java: 32
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occupant)
        {
            if ((!grid_targets_only || ((grid_x + grid_y) & 1) == 0) && occupant == null)
            {
                result[index] = new com.oddlabs.tt.landscape.LandscapeTarget(grid_x, grid_y);
                index++;
            }
            return index == result.Length;
        }
        // Java: 41
        public com.oddlabs.tt.util.Target[] getTargets()
        {
            return result;
        }
        public FindTargetsFilter(int num_targets, int max_radius, bool grid_targets_only) : base()
        {
            result = new com.oddlabs.tt.util.Target[num_targets];
            this.max_radius = max_radius;
            this.grid_targets_only = grid_targets_only;
            index = 0;
        }
    }
}
