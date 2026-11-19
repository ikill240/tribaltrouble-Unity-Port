// Translated from GameClientInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 6
    public interface GameClientInterface
    {
        // Java: 7
        void setWorldGeneratorAndPlayerSlot(com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, short player_slot, int player_count);
        // Java: 9
        void setPlayers(com.oddlabs.tt.net.PlayerSlot[] players);
        // Java: 11
        void startGame(int session_id);
        // Java: 13
        void chat(int player_slot, string chat_v);
    }
}
