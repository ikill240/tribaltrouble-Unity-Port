// Translated from MouseWheelListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 3
    public interface MouseWheelListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 5
        void mouseScrolled(int amount);
    }
    public sealed class MouseWheelListener_L : java.lang.Object, MouseWheelListener
    {
        readonly System.Action<int> f;
        public MouseWheelListener_L(System.Action<int> f) { this.f = f; }
        public void mouseScrolled(int p0) { f(p0); }
    }
}
