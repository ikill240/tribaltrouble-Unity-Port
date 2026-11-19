// Translated from ChatHub.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 8
    public class ChatHub : java.lang.Object, com.oddlabs.tt.net.ChatListener
    {
        public java.util.List listeners;
        // Java: 11
        public void addListener(com.oddlabs.tt.net.ChatListener listener)
        {
            if (!listeners.contains(listener))
            {
                listeners.add(listener);
            }
        }
        // Java: 17
        public void removeListener(com.oddlabs.tt.net.ChatListener listener)
        {
            listeners.remove(listener);
        }
        // Java: 21
        public void chat(com.oddlabs.tt.net.ChatMessage message)
        {
            if (!com.oddlabs.tt.net.ChatCommand.isIgnoring(message.nick()))
            {
                listeners.forEach(new java.util.function.Consumer_L((object listener_l) =>
                {
                    com.oddlabs.tt.net.ChatListener listener = (com.oddlabs.tt.net.ChatListener)listener_l;
                    listener.chat(message);
                }));
            }
        }
        private void init_ChatHub()
        {
            this.listeners = new java.util.ArrayList();
        }
        public ChatHub() : base()
        {
            init_ChatHub();
        }
    }
}
