// Translated from FirstPersonDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 14
    public class FirstPersonDelegate : com.oddlabs.tt.@delegate.InGameDelegate
    {
        public bool key_pressed;
        public int created_tick;
        public bool done;
        // Java: 26
        public void release()
        {
            done = true;
        }
        // Java: 30
        public override void doRemove()
        {
            base.doRemove();
            if (!done)
            {
                release();
            }
        }
        // Java: 38
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            base.handleInput(@event);
            if (@event.isConsumed())
            {
                return;
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
            {
                if (key_pressed && !done)
                {
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_FIRST_PERSON))
                    {
                        pop();
                    }
                }
            }
            @event.consume();
        }
        // Java: 54
        public override void mouseScrolled(int amount)
        {
        }
        // Java: 58
        public override void mouseMoved(int x, int y)
        {
            if (!done)
            {
                getCamera().mouseMoved(x, y);
            }
        }
        // Java: 64
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
            if (created_tick == com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().getTick())
            {
                return;
            }
            if ((((object)button == (object)com.oddlabs.tt.gui.MouseButton.MIDDLE) || key_pressed) && !done && getGUIRoot().getModalDelegate() == null)
            {
                getCamera().mouseMoved(x, y);
            }
        }
        // Java: 74
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 78
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.MIDDLE) && !key_pressed && !done)
            {
                pop();
            }
        }
        // Java: 85
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return com.oddlabs.tt.gui.CursorType.NULL;
        }
        private void init_FirstPersonDelegate()
        {
            this.done = false;
        }
        public FirstPersonDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.CameraState camera_state, bool key_pressed) : base(viewer, new com.oddlabs.tt.camera.FirstPersonCamera(viewer, viewer.getWorld().getHeightMap(), camera_state))
        {
            init_FirstPersonDelegate();
            this.key_pressed = key_pressed;
            created_tick = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().getTick();
        }
    }
}
