// Translated from DoNowListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public interface DoNowListener
    {
        // Java: 5
        void doChange(bool now);
    }
    public sealed class DoNowListener_L : java.lang.Object, DoNowListener
    {
        readonly System.Action<bool> f;
        public DoNowListener_L(System.Action<bool> f) { this.f = f; }
        public void doChange(bool p0) { f(p0); }
    }
}
