// Translated from ButtonObject.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public abstract class ButtonObject : com.oddlabs.tt.gui.GUIObject
    {
        public bool pressed;
        public com.oddlabs.tt.font.Font font;
        public com.oddlabs.tt.input.GameAction action;
        // Java: 31
        public virtual com.oddlabs.tt.font.Font getFont()
        {
            return font;
        }
        // Java: 35
        public com.oddlabs.tt.input.GameAction getAction()
        {
            return action;
        }
        // Java: 39
        public bool isPressed()
        {
            return pressed;
        }
        // Java: 43
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = false;
        }
        // Java: 48
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = true;
        }
        // Java: 53
        public override void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (pressed)
            {
                mousePressedAll(button, x, y);
            }
        }
        // Java: 59
        public override void setDisabled(bool disabled)
        {
            if (disabled)
            {
                pressed = false;
            }
            base.setDisabled(disabled);
        }
        private void init_ButtonObject()
        {
            this.pressed = false;
        }
        public ButtonObject(com.oddlabs.tt.font.Font font) : this(font, null)
        {
        }
        public ButtonObject(com.oddlabs.tt.font.Font font, com.oddlabs.tt.input.GameAction action) : this(font, action, null)
        {
        }
        public ButtonObject(com.oddlabs.tt.font.Font font, com.oddlabs.tt.input.GameAction action, java.util.function.Supplier tool_tip) : base(tool_tip)
        {
            init_ButtonObject();
            this.font = font;
            this.action = action;
            setCanFocus(true);
        }
    }
}
