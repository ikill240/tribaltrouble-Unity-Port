// Translated from MouseButtonListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 6
    public interface MouseButtonListener : com.oddlabs.tt.guievent.MouseClickListener
    {
        // Java: 7
        void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y);
        // Java: 9
        void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y);
        // Java: 11
        void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y);
    }
}
