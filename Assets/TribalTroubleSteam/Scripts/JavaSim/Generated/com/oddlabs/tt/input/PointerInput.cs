// Translated from PointerInput.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 22
    public class PointerInput : java.lang.Object
    {
        public java.util.Set buttons;
        public short last_x;
        public short last_y;
        public com.oddlabs.tt.gui.Cursor active_cursor;
        public com.oddlabs.tt.gui.MouseButton drag_button;
        public com.oddlabs.tt.input.InputProvider inputProvider;
        public com.oddlabs.tt.gui.LocalInput localInput;
        public java.util.Map cursors;
        public com.oddlabs.tt.gui.Cursor debug_cursor;
        public const int NORMAL_HOT_X = 2;
        public const int NORMAL_HOT_Y = 2;
        public const int TARGET_HOT_X = 14;
        public const int TARGET_HOT_Y = 14;
        // Java: 40
        public void loadCursors()
        {
            debug_cursor = ((com.oddlabs.tt.gui.Cursor)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.CursorFile("/textures/gui/pointer_clientload_32_8.png", 2, 2)));
            cursors.put(com.oddlabs.tt.gui.CursorType.NORMAL, ((com.oddlabs.tt.gui.Cursor)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.CursorFile("/textures/gui/pointer_32_8.png", com.oddlabs.tt.input.PointerInput.NORMAL_HOT_X, com.oddlabs.tt.input.PointerInput.NORMAL_HOT_Y))));
            cursors.put(com.oddlabs.tt.gui.CursorType.TARGET, ((com.oddlabs.tt.gui.Cursor)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.CursorFile("/textures/gui/pointer_target_32_8.png", com.oddlabs.tt.input.PointerInput.TARGET_HOT_X, com.oddlabs.tt.input.PointerInput.TARGET_HOT_Y))));
            cursors.put(com.oddlabs.tt.gui.CursorType.TEXT, ((com.oddlabs.tt.gui.Cursor)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.CursorFile("/textures/gui/pointer_text_32_8.png", 6, 11))));
            cursors.put(com.oddlabs.tt.gui.CursorType.DEBUG, debug_cursor);
            cursors.put(com.oddlabs.tt.gui.CursorType.HIDDEN, new com.oddlabs.tt.gui.Cursor(new com.oddlabs.tt.resource.GLIntImage(1, 1, org.lwjgl.opengl.GL11.GL_RGBA), 0, 0));
            cursors.put(com.oddlabs.tt.gui.CursorType.NULL, com.oddlabs.tt.gui.Cursor.NULL_CURSOR);
        }
        // Java: 58
        public void setActiveCursor(com.oddlabs.tt.gui.CursorType type)
        {
            com.oddlabs.tt.gui.Cursor c = ((com.oddlabs.tt.gui.Cursor)cursors.get(type));
            if (c != null)
            {
                setActiveCursor(c);
            }
        }
        // Java: 65
        public void setActiveCursor(com.oddlabs.tt.gui.Cursor cursor)
        {
            if (((object)cursor != (object)com.oddlabs.tt.gui.Cursor.NULL_CURSOR))
            {
                bool wasGrabbed = inputProvider.isGrabbed();
                inputProvider.setGrabbed(false);
                if (wasGrabbed)
                {
                    resetCursorPos();
                }
            }
            else if (!inputProvider.isGrabbed())
            {
                inputProvider.setGrabbed(true);
                resetCursorPos();
            }
            if (((object)active_cursor != (object)cursor))
            {
                doSetActiveCursor(cursor);
            }
        }
        // Java: 81
        public bool isGrabbed()
        {
            return inputProvider.isGrabbed();
        }
        // Java: 85
        public void setCursorPosition(int x, int y)
        {
            if (!com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic().isPlayback())
            {
                inputProvider.setCursorPosition(x, y);
            }
        }
        // Java: 90
        public void resetCursorPos()
        {
            setCursorPosition(localInput.getMouseX(), localInput.getMouseY());
            while (inputProvider.nextMouseEvent())
            {
                ;
            }
        }
        // Java: 97
        public void doSetActiveCursor(com.oddlabs.tt.gui.Cursor cursor)
        {
            active_cursor = cursor;
            com.oddlabs.tt.input.InputProvider provider = ((com.oddlabs.tt.input.InputProvider)inputProvider);
            com.oddlabs.tt.gui.Cursor useCursor = (com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic().isPlayback() ? debug_cursor : cursor);
            provider.setNativeCursor(useCursor.getCursor());
        }
        // Java: 106
        public void deletingCursor(com.oddlabs.tt.gui.Cursor cursor)
        {
            if (((object)active_cursor == (object)cursor))
            {
                doSetActiveCursor(com.oddlabs.tt.gui.Cursor.NULL_CURSOR);
            }
        }
        // Java: 112
        public void updateMouse(com.oddlabs.tt.gui.GUIRoot gui_root, int x, int y, int dz)
        {
            if (x != last_x || y != last_y)
            {
                last_x = unchecked((short)(x));
                last_y = unchecked((short)(y));
                if (drag_button != null && buttons.contains(drag_button))
                {
                    localInput.mouseDragged(gui_root, drag_button, last_x, last_y);
                }
                else
                {
                    localInput.mouseMoved(gui_root, last_x, last_y);
                }
            }
            if (dz != 0)
            {
                localInput.mouseScrolled(gui_root, dz);
            }
        }
        // Java: 126
        public void poll(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.@event.Deterministic deterministic = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic();
            inputProvider.pollMouse();
            int accum_x = last_x;
            int accum_y = last_y;
            int accum_dz = 0;
            while (deterministic.log(inputProvider.nextMouseEvent()))
            {
                accum_x = deterministic.log(inputProvider.getEventX());
                accum_y = deterministic.log(inputProvider.getEventY());
                accum_dz += deterministic.log(inputProvider.getEventDWheel());
                com.oddlabs.tt.gui.MouseButton button = com.oddlabs.tt.gui.MouseButton.fromInt(deterministic.log(inputProvider.getEventButton()));
                if (button != null)
                {
                    updateMouse(gui_root, accum_x, accum_y, accum_dz);
                    accum_dz = 0;
                    if (deterministic.log(inputProvider.getEventButtonState()))
                    {
                        if (buttons.add(button))
                        {
                            if (drag_button == null)
                            {
                                drag_button = button;
                            }
                            localInput.mousePressed(gui_root, button);
                        }
                    }
                    else
                    {
                        if (buttons.remove(button))
                        {
                            drag_button = null;
                            localInput.mouseReleased(gui_root, button);
                        }
                    }
                }
            }
            updateMouse(gui_root, accum_x, accum_y, accum_dz);
        }
        private void init_PointerInput()
        {
            this.buttons = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.gui.MouseButton));
            this.active_cursor = com.oddlabs.tt.gui.Cursor.NULL_CURSOR;
            this.drag_button = null;
            this.cursors = new java.util.EnumMap(typeof(com.oddlabs.tt.gui.CursorType));
            this.debug_cursor = com.oddlabs.tt.gui.Cursor.NULL_CURSOR;
        }
        public PointerInput(com.oddlabs.tt.input.InputProvider inputProvider, com.oddlabs.tt.gui.LocalInput localInput) : base()
        {
            init_PointerInput();
            this.inputProvider = inputProvider;
            this.localInput = localInput;
        }
    }
}
