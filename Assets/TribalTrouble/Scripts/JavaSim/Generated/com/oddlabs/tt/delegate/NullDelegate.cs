// Translated from NullDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 9
    public class NullDelegate : com.oddlabs.tt.@delegate.CameraDelegate
    {
        public bool render_cursor;
        // Java: 17
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
        }
        // Java: 22
        public override void mouseScrolled(int amount)
        {
        }
        // Java: 26
        public override void mouseMoved(int x, int y)
        {
        }
        // Java: 30
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
        }
        // Java: 35
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 39
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 43
        public override bool renderCursor()
        {
            return render_cursor;
        }
        public NullDelegate(com.oddlabs.tt.gui.GUIRoot gui_root, bool render_cursor) : base(gui_root, new com.oddlabs.tt.camera.NullCamera())
        {
            this.render_cursor = render_cursor;
        }
    }
}
