// Translated from InGameChatHistory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 5
    public class InGameChatHistory : com.oddlabs.tt.net.ChatHistory
    {
        // Java: 6
        public override void chat(com.oddlabs.tt.net.ChatMessage message)
        {
            if (((object)message.type() == (object)com.oddlabs.tt.net.ChatMessage.Type.PRIVATE) || ((object)message.type() == (object)com.oddlabs.tt.net.ChatMessage.Type.NORMAL) || ((object)message.type() == (object)com.oddlabs.tt.net.ChatMessage.Type.TEAM) || ((object)message.type() == (object)com.oddlabs.tt.net.ChatMessage.Type.SPECTATOR_CHAT))
            {
                addMessage(message.formatLong());
            }
        }
        public InGameChatHistory() : base()
        {
        }
    }
}
