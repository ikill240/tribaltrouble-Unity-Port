// Translated from Network.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 6
    public class Network : java.lang.Object
    {
        public static com.oddlabs.tt.net.ChatHub chat_hub;
        public static com.oddlabs.tt.net.MatchmakingClient matchmaking_client;
        public static com.oddlabs.tt.net.MatchmakingListener matchmaking_listener;
        // Java: 11
        public static com.oddlabs.tt.net.MatchmakingListener getMatchmakingListener()
        {
            return com.oddlabs.tt.net.Network.matchmaking_listener;
        }
        // Java: 15
        public static void setMatchmakingListener(com.oddlabs.tt.net.MatchmakingListener listener)
        {
            com.oddlabs.tt.net.Network.matchmaking_listener = listener;
        }
        // Java: 19
        public static void closeMatchmakingClient()
        {
            com.oddlabs.tt.net.Network.matchmaking_listener = null;
            com.oddlabs.tt.net.Network.matchmaking_client.close();
        }
        // Java: 24
        public static com.oddlabs.tt.net.ChatHub getChatHub()
        {
            return com.oddlabs.tt.net.Network.chat_hub;
        }
        // Java: 28
        public static com.oddlabs.tt.net.MatchmakingClient getMatchmakingClient()
        {
            return com.oddlabs.tt.net.Network.matchmaking_client;
        }
        static Network()
        {
            Network.chat_hub = new com.oddlabs.tt.net.ChatHub();
            Network.matchmaking_client = new com.oddlabs.tt.net.MatchmakingClient();
        }
        public Network() : base()
        {
        }
    }
}
