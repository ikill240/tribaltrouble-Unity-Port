// Translated from ChatListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 3
    public interface ChatListener
    {
        // Java: 5
        void chat(com.oddlabs.tt.net.ChatMessage message);
    }
    public sealed class ChatListener_L : java.lang.Object, ChatListener
    {
        readonly System.Action<com.oddlabs.tt.net.ChatMessage> f;
        public ChatListener_L(System.Action<com.oddlabs.tt.net.ChatMessage> f) { this.f = f; }
        public void chat(com.oddlabs.tt.net.ChatMessage p0) { f(p0); }
    }
}
