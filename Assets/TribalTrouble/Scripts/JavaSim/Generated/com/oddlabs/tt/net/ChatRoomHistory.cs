// Translated from ChatRoomHistory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 13
    public class ChatRoomHistory : com.oddlabs.tt.net.ChatHistory
    {
        public static java.util.ResourceBundle bundle;
        // Java: 16
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.net.ChatRoomHistory.bundle, key, args);
        }
        public com.oddlabs.matchmaking.ChatRoomUser[] old_users;
        // Java: 22
        public void update(com.oddlabs.matchmaking.ChatRoomUser[] new_users)
        {
            if (old_users == null)
            {
                return;
            }
            java.util.Set new_users_set = new java.util.HashSet(java.util.Arrays.asList(new_users));
            java.util.Set old_users_set = new java.util.HashSet(java.util.Arrays.asList(old_users));
            java.util.Set joined_users = new java.util.HashSet(new_users_set);
            joined_users.removeAll(old_users_set);
            foreach (com.oddlabs.matchmaking.ChatRoomUser user in java.lang.JIter.of(joined_users))
            {
                addMessage(i18n("user_joined", user.getNick()));
            }
            java.util.Set left_users = new java.util.HashSet(old_users_set);
            left_users.removeAll(new_users_set);
            foreach (com.oddlabs.matchmaking.ChatRoomUser user_2 in java.lang.JIter.of(left_users))
            {
                addMessage(i18n("user_left", user_2.getNick()));
            }
        }
        // Java: 39
        public override void chat(com.oddlabs.tt.net.ChatMessage message)
        {
            if (((object)message.type() != (object)com.oddlabs.tt.net.ChatMessage.Type.PRIVATE) && ((object)message.type() != (object)com.oddlabs.tt.net.ChatMessage.Type.CHATROOM))
            {
                return;
            }
            addMessage(message.formatLong());
        }
        static ChatRoomHistory()
        {
            ChatRoomHistory.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.ChatPanel).getName());
        }
        public ChatRoomHistory() : base()
        {
        }
    }
}
