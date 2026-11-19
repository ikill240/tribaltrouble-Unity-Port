// Translated from MouseMotionListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 6
    public interface MouseMotionListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 7
        void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int rel_x, int rel_y, int abs_x, int abs_y);
        // Java: 9
        void mouseMoved(int x, int y);
        // Java: 11
        void mouseEntered();
        // Java: 13
        void mouseExited();
    }
}
