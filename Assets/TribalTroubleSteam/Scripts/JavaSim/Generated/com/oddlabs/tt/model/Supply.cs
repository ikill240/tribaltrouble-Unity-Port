// Translated from Supply.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 13
    public interface Supply : com.oddlabs.tt.pathfinder.Occupant
    {
        public const int HITS_PER_HARVEST = 10;
        // Java: 16
        string getName();
        // Java: 22
        bool isEmpty();
        // Java: 24
        bool hit();
        // Java: 26
        com.oddlabs.tt.model.Supply respawn();
        // Java: 29
        void animateSpawn(float t, float progress);
        // Java: 31
        void spawnComplete();
        // Java: 33
        com.oddlabs.tt.landscape.World getWorld();
    }
}
