// Translated from GameInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 5
    public interface GameInterface
    {
        // Java: 6
        void relayEventTo(int client_id, com.oddlabs.net.ARMIEvent @event);
        // Java: 8
        void relayEvent(com.oddlabs.net.ARMIEvent @event);
        // Java: 10
        void relayGameStateEvent(com.oddlabs.net.ARMIEvent @event);
        // Java: 12
        void checksum(int checksum_v);
    }
}
