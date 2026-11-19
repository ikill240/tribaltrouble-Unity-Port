// Translated from FinderFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 6
    public interface FinderFilter
    {
        // Java: 7
        com.oddlabs.tt.pathfinder.Occupant getOccupantFromRegion(com.oddlabs.tt.pathfinder.Region region, bool one_region);
        // Java: 10
        com.oddlabs.tt.pathfinder.Occupant getBest();
        // Java: 13
        bool acceptOccupant(com.oddlabs.tt.pathfinder.Occupant occ);
    }
}
