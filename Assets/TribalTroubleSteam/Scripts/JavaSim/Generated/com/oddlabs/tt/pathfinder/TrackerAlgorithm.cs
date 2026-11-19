// Translated from TrackerAlgorithm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public interface TrackerAlgorithm
    {
        // Java: 7
        bool isDone(int x, int y);
        // Java: 9
        bool acceptRegion(com.oddlabs.tt.pathfinder.Region region);
        // Java: 11
        com.oddlabs.tt.pathfinder.Region findPathRegion(int src_x, int src_y);
        // Java: 14
        com.oddlabs.tt.pathfinder.GridPathNode findPathGrid(com.oddlabs.tt.pathfinder.Region target_region, com.oddlabs.tt.pathfinder.Region next_region, int src_x, int src_y, bool allow_secondary_targets);
    }
}
