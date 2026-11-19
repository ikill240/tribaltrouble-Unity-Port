// Translated from CloseListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 3
    public interface CloseListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 5
        void closed();
    }
    public sealed class CloseListener_L : java.lang.Object, CloseListener
    {
        readonly System.Action f;
        public CloseListener_L(System.Action f) { this.f = f; }
        public void closed() { f(); }
    }
}
