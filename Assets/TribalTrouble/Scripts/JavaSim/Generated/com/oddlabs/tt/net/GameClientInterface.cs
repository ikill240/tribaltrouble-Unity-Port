// Translated from GameClientInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 7
    public interface GameClientInterface
    {
        // Java: 8
        void setWorldGeneratorAndPlayerSlot(com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, short player_slot, int player_count);
        // Java: 13
        void setWorldParameters(com.oddlabs.tt.landscape.WorldParameters world_params);
        // Java: 15
        void setPlayers(com.oddlabs.tt.net.PlayerSlot[] players);
        // Java: 17
        void startGame(int session_id);
        // Java: 19
        void chat(int player_slot, string chat_v);
        // Java: 23
        void receiveCustomMapChunk(int total_bytes, sbyte[] data);
        // Java: 26
        void customMapNotice(string text);
        // Java: 29
        void lobbyNotice(string label, string text);
    }
}
