// Translated from ControllableCameraDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 12
    public abstract class ControllableCameraDelegate : com.oddlabs.tt.@delegate.InGameDelegate
    {
        public com.oddlabs.tt.camera.GameCamera game_camera;
        public com.oddlabs.tt.@delegate.FirstPersonDelegate first_person_delegate;
        // Java: 21
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            base.handleInput(@event);
            if (@event.isConsumed())
            {
                return;
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_FIRST_PERSON))
                {
                    if (isCursorInWindow())
                    {
                        pushFirstPersonDelegate(true);
                    }
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_MODE))
                {
                    if (isCursorInWindow())
                    {
                        pushZoomDelegate();
                    }
                    @event.consume();
                    return;
                }
            }
        }
        // Java: 44
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.MIDDLE))
            {
                pushFirstPersonDelegate(false);
            }
        }
        // Java: 51
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.MIDDLE) && first_person_delegate != null)
            {
                first_person_delegate.mouseReleased(button, x, y);
            }
        }
        // Java: 58
        public override void mouseScrolled(int amount)
        {
            getCamera().mouseScrolled(amount);
        }
        // Java: 63
        public override void mouseMoved(int x, int y)
        {
            getCamera().mouseMoved(x, y);
        }
        // Java: 68
        public override bool canScroll()
        {
            com.oddlabs.tt.gui.LocalInput localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
            float scale = getGUIRoot().getGlobalScale();
            mouseMoved(java.lang.Math.round((float)(localInput.getMouseX() / scale)), java.lang.Math.round((float)(localInput.getMouseY() / scale)));
            return getGUIRoot().getModalDelegate() == null;
        }
        // Java: 76
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.MIDDLE) && first_person_delegate != null)
            {
                first_person_delegate.mouseDragged(button, x, y, relative_x, relative_y, absolute_x, absolute_y);
            }
        }
        // Java: 87
        public bool isCursorInWindow()
        {
            return com.oddlabs.tt.render.Renderer.getLocalInput().getInputProvider().isCursorInWindow();
        }
        // Java: 91
        public void pushFirstPersonDelegate(bool key_pressed)
        {
            first_person_delegate = new com.oddlabs.tt.@delegate.FirstPersonDelegate(getViewer(), getCamera().getState(), key_pressed);
            getGUIRoot().pushDelegate(first_person_delegate);
        }
        // Java: 96
        public void pushZoomDelegate()
        {
            getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.ZoomDelegate(getViewer(), game_camera));
        }
        public ControllableCameraDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera game_camera) : base(viewer, game_camera)
        {
            this.game_camera = game_camera;
        }
    }
}
