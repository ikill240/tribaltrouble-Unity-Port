// Translated from InputState.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 21
    public class InputState : java.lang.Object
    {
        public const float MOUSE_REPEAT_DELAY = 0.5f;
        public const float MOUSE_REPEAT_RATE = 0.05f;
        public const float DOUBLE_CLICK_TIMEOUT = 0.4f;
        public const int DOUBLE_CLICK_THRESHOLD = 2;
        public com.oddlabs.tt.animation.TimerAnimation double_click_timer;
        public com.oddlabs.tt.animation.TimerAnimation double_key_timer;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.animation.TimerAnimation mouse_timer;
        public int drag_x;
        public int drag_y;
        public int absolute_drag_x;
        public int absolute_drag_y;
        public com.oddlabs.tt.gui.GUIObject drag_obj;
        public com.oddlabs.tt.gui.MouseButton drag_button;
        public com.oddlabs.tt.gui.GUIObject press_obj;
        public com.oddlabs.tt.gui.GUIObject clicked_obj;
        public int click_counter;
        public int clicked_x;
        public int clicked_y;
        public com.oddlabs.tt.input.KeyboardEvent key_event;
        public int key_counter;
        public com.oddlabs.tt.input.KeyboardEvent held_event;
        // Java: 57
        public com.oddlabs.tt.gui.GUIObject pick()
        {
            gui_root.mousePick();
            return gui_root.getCurrentGUIObject();
        }
        // Java: 62
        public void mouseMoved(short x, short y)
        {
            com.oddlabs.tt.gui.GUIObject gui_hit = pick();
            float scale = gui_root.getGlobalScale();
            gui_hit.mouseMovedAll(unchecked((short)(java.lang.Math.round((float)(x / scale)))), unchecked((short)(java.lang.Math.round((float)(y / scale)))));
        }
        // Java: 68
        public void resetKeyTimer()
        {
            com.oddlabs.tt.font.Index.resetBlinking();
        }
        // Java: 72
        public void mouseScrolled(int dz)
        {
            com.oddlabs.tt.gui.GUIObject gui_hit = pick();
            int scroll_amount = java.lang.Math.round((float)(dz * com.oddlabs.tt.global.Globals.WHEEL_SCALE));
            gui_hit.setFocus();
            gui_hit.mouseScrolledAll(scroll_amount);
        }
        // Java: 79
        public void mousePressed(com.oddlabs.tt.gui.MouseButton button)
        {
            com.oddlabs.tt.gui.GUIObject gui_hit = pick();
            com.oddlabs.tt.gui.LocalInput localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
            float scale = gui_root.getGlobalScale();
            int scaledX = java.lang.Math.round((float)(localInput.getMouseX() / scale));
            int scaledY = java.lang.Math.round((float)(localInput.getMouseY() / scale));
            int local_x = gui_hit.translateXToLocal(scaledX);
            int local_y = gui_hit.translateYToLocal(scaledY);
            com.oddlabs.tt.font.Index.resetBlinking();
            drag_x = scaledX;
            drag_y = scaledY;
            absolute_drag_x = drag_x;
            absolute_drag_y = drag_y;
            if (drag_obj == null)
            {
                drag_obj = gui_hit;
                drag_button = button;
            }
            gui_hit.setFocus();
            press_obj = gui_hit;
            press_obj.mousePressedAll(button, local_x, local_y);
            if (mouse_timer != null)
            {
                mouse_timer.stop();
            }
            mouse_timer = new com.oddlabs.tt.animation.TimerAnimation(new com.oddlabs.tt.animation.Updatable_L((object timer_l) =>
            {
                com.oddlabs.tt.animation.TimerAnimation timer = (com.oddlabs.tt.animation.TimerAnimation)timer_l;
                timer.setTimerInterval(com.oddlabs.tt.gui.InputState.MOUSE_REPEAT_RATE);
                timer.resetTime();
                if (((object)press_obj == (object)gui_root.getCurrentGUIObject()))
                {
                    int local_x1 = press_obj.translateXToLocal(java.lang.Math.round((float)(localInput.getMouseX() / scale)));
                    int local_y1 = press_obj.translateYToLocal(java.lang.Math.round((float)(localInput.getMouseY() / scale)));
                    press_obj.mouseHeldAll(button, local_x1, local_y1);
                }
            }), com.oddlabs.tt.gui.InputState.MOUSE_REPEAT_DELAY);
            mouse_timer.start();
        }
        // Java: 115
        public void mouseReleased(com.oddlabs.tt.gui.MouseButton button)
        {
            com.oddlabs.tt.gui.GUIObject gui_hit = pick();
            com.oddlabs.tt.gui.LocalInput localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
            float scale = gui_root.getGlobalScale();
            int scaledX = java.lang.Math.round((float)(localInput.getMouseX() / scale));
            int scaledY = java.lang.Math.round((float)(localInput.getMouseY() / scale));
            com.oddlabs.tt.font.Index.resetBlinking();
            if (((object)button == (object)drag_button))
            {
                drag_obj = null;
            }
            if (press_obj == null)
            {
                return;
            }
            int local_x = press_obj.translateXToLocal(scaledX);
            int local_y = press_obj.translateYToLocal(scaledY);
            if (((object)gui_hit == (object)press_obj))
            {
                if (((object)gui_hit != (object)clicked_obj) || !clickedSameArea())
                {
                    if (double_click_timer.isRunning())
                    {
                        stopDoubleClickTimer();
                    }
                    double_click_timer.setTimerInterval(com.oddlabs.tt.gui.InputState.DOUBLE_CLICK_TIMEOUT);
                    double_click_timer.start();
                }
                click_counter++;
                clicked_obj = gui_hit;
                clicked_x = localInput.getMouseX();
                clicked_y = localInput.getMouseY();
                press_obj.mouseClickedAll(button, local_x, local_y, click_counter);
            }
            press_obj.mouseReleasedAll(button, local_x, local_y);
            if (mouse_timer != null)
            {
                mouse_timer.stop();
            }
        }
        // Java: 150
        public void mouseDragged(com.oddlabs.tt.gui.MouseButton button, short x, short y)
        {
            float scale = gui_root.getGlobalScale();
            int scaledX = java.lang.Math.round((float)(x / scale));
            int scaledY = java.lang.Math.round((float)(y / scale));
            if (drag_obj != null)
            {
                drag_obj.mouseDraggedAll(button, scaledX, scaledY, scaledX - drag_x, scaledY - drag_y, scaledX - absolute_drag_x, scaledY - absolute_drag_y);
            }
            drag_x = scaledX;
            drag_y = scaledY;
        }
        // Java: 162
        public bool clickedSameArea()
        {
            com.oddlabs.tt.gui.LocalInput localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
            return java.lang.Math.abs(localInput.getMouseX() - clicked_x) < com.oddlabs.tt.gui.InputState.DOUBLE_CLICK_THRESHOLD && java.lang.Math.abs(localInput.getMouseY() - clicked_y) < com.oddlabs.tt.gui.InputState.DOUBLE_CLICK_THRESHOLD;
        }
        // Java: 168
        public void stopDoubleClickTimer()
        {
            double_click_timer.stop();
            double_click_timer.resetTime();
            clicked_obj = null;
            click_counter = 0;
        }
        // Java: 175
        public void stopDoubleKeyTimer()
        {
            double_key_timer.stop();
            double_key_timer.resetTime();
            key_event = null;
            key_counter = 0;
        }
        // Java: 182
        public void keyTyped(int key_code, char key_char)
        {
            com.oddlabs.tt.input.Key key = com.oddlabs.tt.input.Key.fromGlfwCode(key_code);
            if (((object)com.oddlabs.tt.input.Key.KEY_UNKNOWN != (object)key) || key_char != 0)
            {
                com.oddlabs.tt.gui.GUIObject focused = gui_root.getGlobalFocus();
                com.oddlabs.tt.input.KeyboardEvent keyEvent = new com.oddlabs.tt.input.KeyboardEvent(key, key_char, java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Modifier)), 1);
                java.util.Set actions = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getActions(keyEvent);
                com.oddlabs.tt.input.InputEvent @event = new com.oddlabs.tt.input.InputEvent(keyEvent, actions, com.oddlabs.tt.input.InputPhase.REPEAT);
                focused.handleInputAll(@event);
            }
        }
        // Java: 193
        public void keyPressed(com.oddlabs.tt.input.Key key, char key_char, java.util.Set modifiers, bool repeat)
        {
            com.oddlabs.tt.gui.GUIObject focused = gui_root.getGlobalFocus();
            resetKeyTimer();
            if (!repeat && (key_event == null || ((object)key_event.keyCode() != (object)key) || key_event.keyChar() != key_char || !java.lang.JO.equals(key_event.modifiers(), modifiers)))
            {
                if (double_key_timer.isRunning())
                {
                    stopDoubleKeyTimer();
                }
                double_key_timer.setTimerInterval(com.oddlabs.tt.gui.InputState.DOUBLE_CLICK_TIMEOUT);
                double_key_timer.start();
            }
            if (!repeat)
            {
                key_counter++;
            }
            com.oddlabs.tt.input.KeyboardEvent keyEvent = new com.oddlabs.tt.input.KeyboardEvent(key, key_char, modifiers, key_counter);
            key_event = keyEvent;
            java.util.Set actions = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getActions(keyEvent);
            com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().updateState(keyEvent, true);
            if (!repeat)
            {
                com.oddlabs.tt.input.InputEvent @event = new com.oddlabs.tt.input.InputEvent(keyEvent, actions, com.oddlabs.tt.input.InputPhase.PRESSED);
                focused.handleInputAll(@event);
            }
            else
            {
                com.oddlabs.tt.input.InputEvent repeatEvent = new com.oddlabs.tt.input.InputEvent(keyEvent, actions, com.oddlabs.tt.input.InputPhase.REPEAT);
                focused.handleInputAll(repeatEvent);
            }
        }
        // Java: 224
        public void keyReleased(com.oddlabs.tt.input.Key key, char key_char, java.util.Set modifiers)
        {
            com.oddlabs.tt.gui.GUIObject focused = gui_root.getGlobalFocus();
            resetKeyTimer();
            com.oddlabs.tt.input.KeyboardEvent keyEvent = new com.oddlabs.tt.input.KeyboardEvent(key, key_char, modifiers, 0);
            java.util.Set actions = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getActions(keyEvent);
            com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().updateState(keyEvent, false);
            com.oddlabs.tt.input.InputEvent @event = new com.oddlabs.tt.input.InputEvent(keyEvent, actions, com.oddlabs.tt.input.InputPhase.RELEASED);
            focused.handleInputAll(@event);
        }
        // Java: 236
        public class DoubleClickTimer : java.lang.Object, com.oddlabs.tt.animation.Updatable
        {
            public com.oddlabs.tt.gui.InputState this_0;
            // Java: 237
            public void update(object anim_p)
            {
                com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
                this_0.stopDoubleClickTimer();
            }
            public DoubleClickTimer(com.oddlabs.tt.gui.InputState this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 243
        public class DoubleKeyTimer : java.lang.Object, com.oddlabs.tt.animation.Updatable
        {
            public com.oddlabs.tt.gui.InputState this_0;
            // Java: 244
            public void update(object anim_p)
            {
                com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
                this_0.stopDoubleKeyTimer();
            }
            public DoubleKeyTimer(com.oddlabs.tt.gui.InputState this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        private void init_InputState()
        {
            this.click_counter = 0;
            this.key_counter = 0;
        }
        public InputState(com.oddlabs.tt.gui.GUIRoot gui_root) : base()
        {
            init_InputState();
            this.gui_root = gui_root;
            double_click_timer = new com.oddlabs.tt.animation.TimerAnimation(new com.oddlabs.tt.gui.InputState.DoubleClickTimer(this), 0);
            double_key_timer = new com.oddlabs.tt.animation.TimerAnimation(new com.oddlabs.tt.gui.InputState.DoubleKeyTimer(this), 0);
            press_obj = gui_root;
        }
    }
}
