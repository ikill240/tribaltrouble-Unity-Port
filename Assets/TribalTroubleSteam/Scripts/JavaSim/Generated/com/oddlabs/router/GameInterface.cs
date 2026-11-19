// Translated from GameInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 5
    public interface GameInterface
    {
        public const int SPECTATOR_CLIENT_ID = (-1);
        // Java: 9
        void relayEventTo(int client_id, com.oddlabs.net.ARMIEvent @event);
        // Java: 11
        void relayEvent(com.oddlabs.net.ARMIEvent @event);
        // Java: 13
        void relayGameStateEvent(com.oddlabs.net.ARMIEvent @event);
        // Java: 15
        void checksum(int checksum_v);
    }
}
