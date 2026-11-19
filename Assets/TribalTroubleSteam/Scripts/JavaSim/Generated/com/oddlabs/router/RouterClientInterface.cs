// Translated from RouterClientInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 5
    public interface RouterClientInterface
    {
        // Java: 6
        void start();
        // Java: 8
        void heartbeat(int tick);
        // Java: 10
        void receiveEvent(int client_id, com.oddlabs.net.ARMIEvent @event);
        // Java: 12
        void receiveGameStateEvent(int client_id, int tick, com.oddlabs.net.ARMIEvent @event);
        // Java: 14
        void playerDisconnected(int client_id, bool checksum_error);
    }
}
