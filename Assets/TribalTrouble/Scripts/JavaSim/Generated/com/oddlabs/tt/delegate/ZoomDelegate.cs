// Translated from ZoomDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 13
    public class ZoomDelegate : com.oddlabs.tt.@delegate.InGameDelegate
    {
        public const float ZOOM_FACTOR_CORRECTION = 0.25f;
        public int physical_start_x;
        public int physical_start_y;
        public com.oddlabs.tt.camera.GameCamera game_camera;
        public bool done;
        // Java: 31
        public void release()
        {
            done = true;
        }
        // Java: 35
        public override void doRemove()
        {
            base.doRemove();
            if (!done)
            {
                release();
            }
        }
        // Java: 43
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (!done && ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED) && @event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_MODE))
            {
                pop();
            }
            @event.consume();
        }
        // Java: 51
        public override void mouseScrolled(int amount)
        {
        }
        // Java: 55
        public override void mouseMoved(int x, int y)
        {
            if (!done)
            {
                int dy = com.oddlabs.tt.render.Renderer.getLocalInput().getMouseY() - physical_start_y;
                float zoom_factor = (float)(dy * com.oddlabs.tt.@delegate.ZoomDelegate.ZOOM_FACTOR_CORRECTION);
                game_camera.zoom(zoom_factor);
                com.oddlabs.tt.render.Renderer.getLocalInput().getPointerInput().setCursorPosition(physical_start_x, physical_start_y);
            }
        }
        // Java: 68
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
        }
        // Java: 73
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 77
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 81
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return com.oddlabs.tt.gui.CursorType.NULL;
        }
        private void init_ZoomDelegate()
        {
            this.done = false;
        }
        public ZoomDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera) : base(viewer, camera)
        {
            init_ZoomDelegate();
            game_camera = camera;
            com.oddlabs.tt.gui.LocalInput localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
            physical_start_x = localInput.getMouseX();
            physical_start_y = localInput.getMouseY();
        }
    }
}
