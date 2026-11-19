// Translated from TabListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 5
    public interface TabListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 7
        void tabPressed(string[] words);
    }
    public sealed class TabListener_L : java.lang.Object, TabListener
    {
        readonly System.Action<string[]> f;
        public TabListener_L(System.Action<string[]> f) { this.f = f; }
        public void tabPressed(string[] p0) { f(p0); }
    }
}
