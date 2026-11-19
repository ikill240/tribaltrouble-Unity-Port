// Translated from MatchmakingServerInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 6
    public interface MatchmakingServerInterface
    {
        public const int TYPE_NONE = 0;
        public const int TYPE_GAME = 1;
        public const int TYPE_CHAT_ROOM_LIST = 2;
        public const int TYPE_RANKING_LIST = 3;
        public const int TYPE_OPENSKILL_RANKING_LIST = 4;
        public const int TYPE_OPENSKILL_PERSONAL_RANKING = 5;
        public const int MATCHMAKING_SERVER_PORT = 33214;
        public const int MAX_PLAYERS = 12;
        public const int MIN_PLAYERS = 1;
        public const int MIN_ROOM_NAME_LENGTH = 1;
        public const int MAX_ROOM_NAME_LENGTH = 20;
        public const int MAX_ROOM_USERS = 50;
        public const string ALLOWED_ROOM_CHARS = "abcdefghijklmnopqrstuvwxyz\u00e6\u00f8\u00e5ABCDEFGHIJKLMNOPQRSTUVWXYZ\u00c6\u00d8\u00c50123456789\u00e8\u00e9\u00ea\u00eb\u00ec\u00ed\u00ee\u00ef\u00f0\u00f1\u00f2\u00f3\u00f4\u00f5\u00f6\u00f9\u00fa\u00fb\u00fc\u00fd\u00ff-_,.:;?+={}[]()/&%#!<\\>'*";
        // Java: 23
        void setProfile(string nick);
        // Java: 25
        void createProfile(string nick);
        // Java: 27
        void deleteProfile(string nick);
        // Java: 29
        void requestProfiles();
        // Java: 31
        void logPriority(string nick, int priority);
        // Java: 33
        void registerGame(com.oddlabs.matchmaking.Game game);
        // Java: 35
        void unregisterGame();
        // Java: 37
        void sendMessageToRoom(string msg);
        // Java: 39
        void sendPrivateMessage(string nick, string msg);
        // Java: 41
        void joinRoom(string name);
        // Java: 43
        void leaveRoom();
        // Java: 45
        void requestInfo(string nick);
        // Java: 47
        void requestList(int type, int update_key);
        // Java: 49
        void acceptTunnel(com.oddlabs.net.HostSequenceID host_seq);
        // Java: 51
        void openTunnel(int address_to, int seq);
        // Java: 53
        void closeTunnel(com.oddlabs.net.HostSequenceID address_to);
        // Java: 55
        void routeEvent(com.oddlabs.net.HostSequenceID from, com.oddlabs.net.ARMIEvent @event);
        // Java: 57
        void multicastEvent(com.oddlabs.net.ARMIEvent @event);
        // Java: 59
        void setMulticast(com.oddlabs.net.HostSequenceID[] addresses);
        // Java: 61
        void gameStartedNotify(com.oddlabs.matchmaking.GameSession game_session);
        // Java: 63
        void gameQuitNotify(string nick);
        // Java: 65
        void freeQuitStopNotify();
        // Java: 67
        void gameLostNotify();
        // Java: 69
        void gameWonNotify();
        // Java: 71
        void updateGameStatus(int tick, int[] status);
        // Java: 73
        void updateSpectatorInfo(int tick, string info);
        // Java: 75
        void requestSpectate(string nick);
        // Java: 77
        void updateCommandEvent(int tick, int client_id, short event_size, sbyte[] event_data);
        // Java: 79
        void updateWorldParams(sbyte[] world_params_data);
        // Java: 81
        void requestSpectatorEventLog();
    }
}
