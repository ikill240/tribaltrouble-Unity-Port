// Translated from EnterListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 5
    public interface EnterListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 7
        void enterPressed(string text);
    }
    public sealed class EnterListener_L : java.lang.Object, EnterListener
    {
        readonly System.Action<string> f;
        public EnterListener_L(System.Action<string> f) { this.f = f; }
        public void enterPressed(string p0) { f(p0); }
    }
}
