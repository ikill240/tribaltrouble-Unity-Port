// Translated from ScanFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public interface ScanFilter
    {
        // Java: 4
        int getMinRadius();
        // Java: 6
        int getMaxRadius();
        // Java: 8
        bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ);
    }
}
