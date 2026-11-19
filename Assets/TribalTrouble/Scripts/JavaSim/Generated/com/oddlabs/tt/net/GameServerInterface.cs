// Translated from GameServerInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 3
    public interface GameServerInterface
    {
        // Java: 4
        void resetSlotState(int slot, bool open);
        // Java: 6
        void setPlayerSlot(int slot, int type, int race, int team, bool ready, int ai_difficulty);
        // Java: 8
        void startServer();
        // Java: 10
        void chat(string chat_v);
        // Java: 14
        void requestCustomMap();
        // Java: 16
        void customMapReady();
        // Java: 19
        void customMapChunkReceived();
        // Java: 22
        void reportBuild(string fingerprint);
        // Java: 25
        void setPlayerNames(string chief_name, string tribe_name);
    }
}
