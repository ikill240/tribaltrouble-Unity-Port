// Translated from PathFinderAlgorithm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 5
    public interface PathFinderAlgorithm
    {
        // Java: 6
        com.oddlabs.tt.pathfinder.NodeResult touchNode(com.oddlabs.tt.pathfinder.Node node);
        // Java: 9
        com.oddlabs.tt.pathfinder.NodeResult getBestNode();
        // Java: 12
        int computeEstimatedCost(com.oddlabs.tt.pathfinder.Node node);
        // Java: 14
        bool touchNeighbour(com.oddlabs.tt.pathfinder.Occupant occ);
    }
}
