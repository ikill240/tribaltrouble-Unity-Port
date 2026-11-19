// Translated from ConfigurationListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 6
    public interface ConfigurationListener : com.oddlabs.tt.net.ErrorListener
    {
        // Java: 7
        void connected(com.oddlabs.tt.net.Client client, com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, int player_slot, int player_count);
        // Java: 9
        void setPlayers(com.oddlabs.tt.net.PlayerSlot[] players);
        // Java: 11
        void gameStarted();
    }
}
