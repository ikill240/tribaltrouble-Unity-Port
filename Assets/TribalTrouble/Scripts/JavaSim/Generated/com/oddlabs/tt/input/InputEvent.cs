// Translated from InputEvent.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 9
    public class InputEvent : java.lang.Object
    {
        public java.util.Set actions;
        public com.oddlabs.tt.input.InputPhase phase;
        public char character;
        public bool shiftDown;
        public bool controlDown;
        public bool altDown;
        public bool metaDown;
        public int clicks;
        public com.oddlabs.tt.input.Key keyCode;
        public bool consumed;
        // Java: 38
        public java.util.Set getActions()
        {
            return actions;
        }
        // Java: 42
        public com.oddlabs.tt.input.InputPhase getPhase()
        {
            return phase;
        }
        // Java: 46
        public char getCharacter()
        {
            return character;
        }
        // Java: 50
        public bool isShiftDown()
        {
            return shiftDown;
        }
        // Java: 54
        public bool isControlDown()
        {
            return controlDown;
        }
        // Java: 58
        public bool isAltDown()
        {
            return altDown;
        }
        // Java: 62
        public bool isMetaDown()
        {
            return metaDown;
        }
        // Java: 66
        public int getClicks()
        {
            return clicks;
        }
        // Java: 73
        public com.oddlabs.tt.input.Key getKeyCode()
        {
            return keyCode;
        }
        // Java: 85
        public bool isConsumed()
        {
            return consumed;
        }
        // Java: 92
        public void consume()
        {
            this.consumed = true;
        }
        // Java: 99
        public bool hasActions()
        {
            return !actions.isEmpty();
        }
        // Java: 103
        public bool hasAction(com.oddlabs.tt.input.GameAction action)
        {
            return actions.contains(action);
        }
        // Java: 110
        public bool consumeAction(com.oddlabs.tt.input.GameAction action)
        {
            return actions.remove(action);
        }
        public InputEvent(com.oddlabs.tt.input.KeyboardEvent keyboardEvent, java.util.Set actions, com.oddlabs.tt.input.InputPhase phase) : base()
        {
            this.actions = java.util.EnumSet.copyOf(actions);
            this.phase = phase;
            this.keyCode = keyboardEvent.keyCode();
            this.character = keyboardEvent.keyChar();
            this.shiftDown = keyboardEvent.shiftDown();
            this.controlDown = keyboardEvent.controlDown();
            this.altDown = keyboardEvent.altDown();
            this.metaDown = keyboardEvent.metaDown();
            this.clicks = keyboardEvent.clicks();
            this.consumed = false;
        }
    }
}
