// Translated from MatchmakingListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 6
    public interface MatchmakingListener : com.oddlabs.tt.net.ErrorListener
    {
        // Java: 7
        void clearList(int type);
        // Java: 9
        void receivedList(int type, object[] names);
        // Java: 11
        void loggedIn();
        // Java: 13
        void loginError(int error_code);
        // Java: 15
        void receivedProfiles(com.oddlabs.matchmaking.Profile[] profiles, string last_nick);
        // Java: 17
        void profileUpdated();
        // Java: 19
        void joinedChat(com.oddlabs.tt.gui.ChatRoomInfo info);
        // Java: 21
        void updateChatRoom(com.oddlabs.tt.gui.ChatRoomInfo info);
    }
}
