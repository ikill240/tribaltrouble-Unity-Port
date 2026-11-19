// Translated from ChatHistory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 8
    public abstract class ChatHistory : java.lang.Object, com.oddlabs.tt.net.ChatListener
    {
        public const int MAX_HISTORY = 50;
        public java.util.List messages;
        // Java: 13
        public void clear()
        {
            messages.clear();
        }
        // Java: 17
        public abstract void chat(com.oddlabs.tt.net.ChatMessage message);
        // Java: 20
        public void addMessage(string msg)
        {
            while (messages.size() >= com.oddlabs.tt.net.ChatHistory.MAX_HISTORY)
            {
                messages.removeFirst();
            }
            messages.add(msg);
        }
        // Java: 27
        public java.util.List getMessages()
        {
            return messages;
        }
        private void init_ChatHistory()
        {
            this.messages = new java.util.ArrayList(com.oddlabs.tt.net.ChatHistory.MAX_HISTORY);
        }
        public ChatHistory() : base()
        {
            init_ChatHistory();
        }
    }
}
