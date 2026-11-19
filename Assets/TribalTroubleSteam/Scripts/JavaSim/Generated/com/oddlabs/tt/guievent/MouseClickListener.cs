// Translated from MouseClickListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 6
    public interface MouseClickListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 8
        void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks);
    }
    public sealed class MouseClickListener_L : java.lang.Object, MouseClickListener
    {
        readonly System.Action<com.oddlabs.tt.gui.MouseButton, int, int, int> f;
        public MouseClickListener_L(System.Action<com.oddlabs.tt.gui.MouseButton, int, int, int> f) { this.f = f; }
        public void mouseClicked(com.oddlabs.tt.gui.MouseButton p0, int p1, int p2, int p3) { f(p0, p1, p2, p3); }
    }
}
