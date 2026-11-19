// Translated from LWJGL3InputProvider.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 31
    public class LWJGL3InputProvider : java.lang.Object, com.oddlabs.tt.input.InputProvider
    {
        public com.oddlabs.tt.window.LWJGL3Window window;
        public long windowHandle;
        public java.util.Deque keyEvents;
        public com.oddlabs.tt.input.LWJGL3InputProvider.KeyEvent currentKeyEvent;
        public java.util.Deque mouseEvents;
        public com.oddlabs.tt.input.LWJGL3InputProvider.MouseEvent currentMouseEvent;
        public double mouseX;
        public double mouseY;
        // Java: 46
        public class KeyEvent : java.lang.Object
        {
            public int key;
            public int action;
            public int scancode;
            public int mods;
            public char character;
            public KeyEvent(int key, int action, int scancode, int mods) : base()
            {
                this.key = key;
                this.action = action;
                this.scancode = scancode;
                this.mods = mods;
            }
            public KeyEvent(int key, int action, int scancode, int mods, char character) : this(key, action, scancode, mods)
            {
                this.character = character;
            }
        }
        // Java: 66
        public class MouseEvent : java.lang.Object
        {
            public int button() { return button_f; }
            public bool state() { return state_f; }
            public int x() { return x_f; }
            public int y() { return y_f; }
            public int dWheel() { return dWheel_f; }
            public MouseEvent(int button, bool state, int x, int y, int dWheel)
            {
                this.button_f = button;
                this.state_f = state;
                this.x_f = x;
                this.y_f = y;
                this.dWheel_f = dWheel;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is MouseEvent r)) return false; return true && java.lang.JO.equals(button_f, r.button_f) && java.lang.JO.equals(state_f, r.state_f) && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(dWheel_f, r.dWheel_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(button_f); h = 31 * h + java.lang.JO.hashCode(state_f); h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(dWheel_f); } return h; }
            public override string toString() { return "MouseEvent[" + "button=" + java.lang.JO.toString(button_f) + ", state=" + java.lang.JO.toString(state_f) + ", x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", dWheel=" + java.lang.JO.toString(dWheel_f) + "]"; }
            public int button_f;
            public bool state_f;
            public int x_f;
            public int y_f;
            public int dWheel_f;
        }
        // Java: 73
        public void initCallbacks()
        {
            this.windowHandle = this.window.getHandle();
            if (windowHandle == org.lwjgl.system.MemoryUtil.NULL)
            {
                throw new System.InvalidOperationException("Window handle is NULL. Window might not be created yet.");
            }
            org.lwjgl.glfw.GLFW.glfwSetKeyCallback(windowHandle, new org.lwjgl.glfw.GLFWKeyCallbackI_L((long window, int key, int scancode, int action, int mods) =>
            {
                com.oddlabs.tt.input.LWJGL3InputProvider.KeyEvent @event = new com.oddlabs.tt.input.LWJGL3InputProvider.KeyEvent(key, action, scancode, mods);
                lock ((keyEvents))
                {
                    keyEvents.add(@event);
                }
            }));
            org.lwjgl.glfw.GLFW.glfwSetCharCallback(windowHandle, new org.lwjgl.glfw.GLFWCharCallbackI_L((long window_2, int codepoint) =>
            {
                lock ((keyEvents))
                {
                    if (!keyEvents.isEmpty())
                    {
                        com.oddlabs.tt.input.LWJGL3InputProvider.KeyEvent last = ((com.oddlabs.tt.input.LWJGL3InputProvider.KeyEvent)keyEvents.getLast());
                        if (last.action == org.lwjgl.glfw.GLFW.GLFW_PRESS || last.action == org.lwjgl.glfw.GLFW.GLFW_REPEAT)
                        {
                            last.character = unchecked((char)(codepoint));
                            return;
                        }
                    }
                    keyEvents.add(new com.oddlabs.tt.input.LWJGL3InputProvider.KeyEvent(0, org.lwjgl.glfw.GLFW.GLFW_PRESS, 0, 0, unchecked((char)(codepoint))));
                }
            }));
            org.lwjgl.glfw.GLFW.glfwSetCursorPosCallback(windowHandle, new org.lwjgl.glfw.GLFWCursorPosCallbackI_L((long window_3, double xpos, double ypos) =>
            {
                int fw = this.window.getWidth();
                int fh = this.window.getHeight();
                int lw = this.window.getLogicalWidth();
                int lh = this.window.getLogicalHeight();
                double scaleX = ((double)(fw)) / lw;
                double scaleY = ((double)(fh)) / lh;
                this.mouseX = xpos * scaleX;
                this.mouseY = fh - (ypos * scaleY) - 1;
                lock ((mouseEvents))
                {
                    mouseEvents.add(new com.oddlabs.tt.input.LWJGL3InputProvider.MouseEvent((-1), false, java.lang.JC.d2i(mouseX), java.lang.JC.d2i(mouseY), 0));
                }
            }));
            org.lwjgl.glfw.GLFW.glfwSetMouseButtonCallback(windowHandle, new org.lwjgl.glfw.GLFWMouseButtonCallbackI_L((long window_4, int button, int action_2, int mods_2) =>
            {
                lock ((mouseEvents))
                {
                    mouseEvents.add(new com.oddlabs.tt.input.LWJGL3InputProvider.MouseEvent(button, action_2 == org.lwjgl.glfw.GLFW.GLFW_PRESS, java.lang.JC.d2i(mouseX), java.lang.JC.d2i(mouseY), 0));
                }
            }));
            org.lwjgl.glfw.GLFW.glfwSetScrollCallback(windowHandle, new org.lwjgl.glfw.GLFWScrollCallbackI_L((long window_5, double xoffset, double yoffset) =>
            {
                lock ((mouseEvents))
                {
                    mouseEvents.add(new com.oddlabs.tt.input.LWJGL3InputProvider.MouseEvent((-1), false, java.lang.JC.d2i(mouseX), java.lang.JC.d2i(mouseY), java.lang.JC.d2i((yoffset * 120))));
                }
            }));
        }
        // Java: 133
        public void close()
        {
        }
        // Java: 138
        public void pollKeyboard()
        {
        }
        // Java: 143
        public bool nextKeyboardEvent()
        {
            lock ((keyEvents))
            {
                if (keyEvents.isEmpty())
                {
                    return false;
                }
                currentKeyEvent = ((com.oddlabs.tt.input.LWJGL3InputProvider.KeyEvent)keyEvents.poll());
                return true;
            }
        }
        // Java: 152
        public int getEventKey()
        {
            return (currentKeyEvent != null ? currentKeyEvent.key : 0);
        }
        // Java: 157
        public bool getEventKeyState()
        {
            return currentKeyEvent != null && currentKeyEvent.action != org.lwjgl.glfw.GLFW.GLFW_RELEASE;
        }
        // Java: 162
        public int getEventKeyMods()
        {
            return (currentKeyEvent != null ? currentKeyEvent.mods : 0);
        }
        // Java: 167
        public char getEventCharacter()
        {
            return (currentKeyEvent != null ? currentKeyEvent.character : unchecked((char)(0)));
        }
        // Java: 172
        public bool isRepeatEvent()
        {
            return currentKeyEvent != null && currentKeyEvent.action == org.lwjgl.glfw.GLFW.GLFW_REPEAT;
        }
        // Java: 177
        public bool isKeyDown(int keyCode)
        {
            return org.lwjgl.glfw.GLFW.glfwGetKey(windowHandle, keyCode) == org.lwjgl.glfw.GLFW.GLFW_PRESS;
        }
        // Java: 182
        public void pollMouse()
        {
        }
        // Java: 187
        public bool nextMouseEvent()
        {
            lock ((mouseEvents))
            {
                if (mouseEvents.isEmpty())
                {
                    return false;
                }
                currentMouseEvent = ((com.oddlabs.tt.input.LWJGL3InputProvider.MouseEvent)mouseEvents.poll());
                return true;
            }
        }
        // Java: 196
        public int getEventButton()
        {
            return (currentMouseEvent != null ? currentMouseEvent.button() : (-1));
        }
        // Java: 201
        public bool getEventButtonState()
        {
            return currentMouseEvent != null && currentMouseEvent.state();
        }
        // Java: 206
        public int getEventDWheel()
        {
            return (currentMouseEvent != null ? currentMouseEvent.dWheel() : 0);
        }
        // Java: 211
        public int getEventX()
        {
            return (currentMouseEvent != null ? currentMouseEvent.x() : java.lang.JC.d2i(mouseX));
        }
        // Java: 216
        public int getEventY()
        {
            return (currentMouseEvent != null ? currentMouseEvent.y() : java.lang.JC.d2i(mouseY));
        }
        // Java: 221
        public int getMouseX()
        {
            return java.lang.JC.d2i(mouseX);
        }
        // Java: 226
        public int getMouseY()
        {
            return java.lang.JC.d2i(mouseY);
        }
        // Java: 231
        public bool isButtonDown(int button)
        {
            return org.lwjgl.glfw.GLFW.glfwGetMouseButton(windowHandle, button) == org.lwjgl.glfw.GLFW.GLFW_PRESS;
        }
        // Java: 236
        public void setCursorPosition(int x, int y)
        {
            int fw = window.getWidth();
            int fh = window.getHeight();
            int lw = window.getLogicalWidth();
            int lh = window.getLogicalHeight();
            double scaleX = ((double)(fw)) / lw;
            double scaleY = ((double)(fh)) / lh;
            double screenX = x / scaleX;
            double screenY = (fh - y - 1) / scaleY;
            org.lwjgl.glfw.GLFW.glfwSetCursorPos(windowHandle, screenX, screenY);
        }
        // Java: 252
        public void setGrabbed(bool grabbed)
        {
            bool confine = com.oddlabs.tt.global.Settings.getSettings().confine_cursor && !com.oddlabs.tt.util.OsPlatform.IS_MAC;
            int ungrabbedMode = (confine ? org.lwjgl.glfw.GLFW.GLFW_CURSOR_CAPTURED : org.lwjgl.glfw.GLFW.GLFW_CURSOR_NORMAL);
            org.lwjgl.glfw.GLFW.glfwSetInputMode(windowHandle, org.lwjgl.glfw.GLFW.GLFW_CURSOR, (grabbed ? org.lwjgl.glfw.GLFW.GLFW_CURSOR_DISABLED : ungrabbedMode));
        }
        // Java: 261
        public bool isGrabbed()
        {
            return org.lwjgl.glfw.GLFW.glfwGetInputMode(windowHandle, org.lwjgl.glfw.GLFW.GLFW_CURSOR) == org.lwjgl.glfw.GLFW.GLFW_CURSOR_DISABLED;
        }
        // Java: 266
        public bool isCursorInWindow()
        {
            return org.lwjgl.glfw.GLFW.glfwGetWindowAttrib(windowHandle, org.lwjgl.glfw.GLFW.GLFW_HOVERED) == org.lwjgl.glfw.GLFW.GLFW_TRUE;
        }
        // Java: 271
        public void setNativeCursor(object cursor_p)
        {
            long? cursor = (long?)cursor_p;
            if (null != cursor && ((long)cursor) != org.lwjgl.system.MemoryUtil.NULL)
            {
                org.lwjgl.glfw.GLFW.glfwSetCursor(windowHandle, ((long)cursor));
            }
            else
            {
                org.lwjgl.glfw.GLFW.glfwSetCursor(windowHandle, org.lwjgl.system.MemoryUtil.NULL);
            }
        }
        private void init_LWJGL3InputProvider()
        {
            this.keyEvents = new java.util.ArrayDeque();
            this.mouseEvents = new java.util.ArrayDeque();
        }
        public LWJGL3InputProvider(com.oddlabs.tt.window.LWJGL3Window win) : base()
        {
            init_LWJGL3InputProvider();
            this.window = win;
        }
    }
}
