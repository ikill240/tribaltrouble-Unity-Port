// Translated from FocusListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 3
    public interface FocusListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 5
        void activated(bool activated_v);
    }
    public sealed class FocusListener_L : java.lang.Object, FocusListener
    {
        readonly System.Action<bool> f;
        public FocusListener_L(System.Action<bool> f) { this.f = f; }
        public void activated(bool p0) { f(p0); }
    }
}
