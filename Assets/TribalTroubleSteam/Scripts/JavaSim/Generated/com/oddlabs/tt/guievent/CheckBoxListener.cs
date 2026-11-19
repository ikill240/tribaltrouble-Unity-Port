// Translated from CheckBoxListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 3
    public interface CheckBoxListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 5
        void @checked(bool marked);
    }
    public sealed class CheckBoxListener_L : java.lang.Object, CheckBoxListener
    {
        readonly System.Action<bool> f;
        public CheckBoxListener_L(System.Action<bool> f) { this.f = f; }
        public void @checked(bool p0) { f(p0); }
    }
}
