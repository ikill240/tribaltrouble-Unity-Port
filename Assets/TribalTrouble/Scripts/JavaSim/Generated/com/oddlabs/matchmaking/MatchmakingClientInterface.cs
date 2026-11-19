// Translated from MatchmakingClientInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 8
    public interface MatchmakingClientInterface
    {
        public const int PROFILE_ERROR_GUEST = 2;
        public const int USER_ERROR_INVALID_EMAIL = 7;
        public const int USER_ERROR_NO_SUCH_USER = 8;
        public const int USER_ERROR_VERSION_TOO_OLD = 9;
        public const int USERNAME_ERROR_TOO_MANY = 1;
        public const int USERNAME_ERROR_ALREADY_EXISTS = 3;
        public const int USERNAME_ERROR_INVALID_CHARACTERS = 4;
        public const int USERNAME_ERROR_TOO_LONG = 5;
        public const int USERNAME_ERROR_TOO_SHORT = 6;
        public const int USER_ERROR_STEAM_REQUIRED = 13;
        public const int CHAT_ERROR_TOO_MANY_USERS = 10;
        public const int CHAT_ERROR_INVALID_NAME = 11;
        public const int CHAT_ERROR_NO_SUCH_NICK = 12;
        public const int CHAT_ERROR_SPECTATE_FAILED = 14;
        // Java: 29
        void updateProfileList(com.oddlabs.matchmaking.Profile[] profiles, string last_profile_nick);
        // Java: 31
        void updateProfile(com.oddlabs.matchmaking.Profile profiles);
        // Java: 33
        void createProfileError(int error_code);
        // Java: 35
        void createProfileSuccess();
        // Java: 37
        void joiningChatRoom(string room_name);
        // Java: 39
        void error(int error_code);
        // Java: 41
        void receiveChatRoomUsers(com.oddlabs.matchmaking.ChatRoomUser[] users);
        // Java: 43
        void receiveChatRoomMessage(string nick, string msg);
        // Java: 45
        void receivePrivateMessage(string nick, string msg);
        // Java: 47
        void receiveInfo(com.oddlabs.matchmaking.Profile profile);
        // Java: 49
        void updateStart(int type);
        // Java: 51
        void updateList(int type, object[] names);
        // Java: 53
        void updateComplete(int next_update_key);
        // Java: 55
        void gameWonAck();
        // Java: 57
        void tunnelOpened(com.oddlabs.net.HostSequenceID from, java.net.InetAddress inet_address, java.net.InetAddress local_inet_address, com.oddlabs.matchmaking.Profile name);
        // Java: 59
        void tunnelClosed(com.oddlabs.net.HostSequenceID from);
        // Java: 61
        void tunnelAccepted(com.oddlabs.net.HostSequenceID from);
        // Java: 63
        void receiveRoutedEvent(com.oddlabs.net.HostSequenceID from, com.oddlabs.net.ARMIEvent @event);
        // Java: 65
        void loginOK(string username, com.oddlabs.matchmaking.TunnelAddress address);
        // Java: 67
        void loginError(int error_code);
        // Java: 69
        void receiveSpectatorData(sbyte[] world_params_data);
        // Java: 71
        void receiveSpectatorEventLog(sbyte[] chunk, int chunk_index, int total_chunks, int current_tick);
    }
}
