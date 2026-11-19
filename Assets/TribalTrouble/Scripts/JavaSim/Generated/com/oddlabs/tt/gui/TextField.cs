// Translated from TextField.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public abstract class TextField : com.oddlabs.tt.gui.GUIObject
    {
        public static implicit operator string(TextField t) => t?.toString();
        public bool isEmpty() => length() == 0;
        public java.lang.StringBuilder text;
        public com.oddlabs.tt.font.Font font;
        public int max_chars;
        // Java: 27
        public com.oddlabs.tt.font.Font getFont()
        {
            return font;
        }
        // Java: 31
        public string getContents()
        {
            return java.lang.JO.toString(text);
        }
        // Java: 35
        public java.lang.StringBuilder getText()
        {
            return text;
        }
        // Java: 39
        public char charAt(int i)
        {
            return text.charAt(i);
        }
        // Java: 44
        public int length()
        {
            return text.length();
        }
        // Java: 49
        public int getTextWidth()
        {
            return font.getWidth(text);
        }
        // Java: 53
        public string subSequence(int start, int end)
        {
            return text.subSequence(start, end);
        }
        // Java: 58
        public override string toString()
        {
            return java.lang.JO.toString(text);
        }
        // Java: 63
        public virtual com.oddlabs.tt.gui.TextField setText(string text)
        {
            this.text.setLength(0);
            this.text.append(text);
            return this;
        }
        // Java: 69
        public void set(string str)
        {
            clear();
            append(java.lang.JO.toString(str));
        }
        // Java: 74
        public virtual void clear()
        {
            text.delete(0, text.length());
        }
        // Java: 78
        public virtual void append(string text)
        {
            this.text.append(text);
            appendNotify(text);
        }
        // Java: 83
        public void append(long i)
        {
            append(java.lang.Long.toString(i));
        }
        // Java: 87
        public virtual bool insert(int index, char key)
        {
            if (isAllowed(key))
            {
                text.insert(index, key);
                return true;
            }
            else
            {
                return false;
            }
        }
        // Java: 96
        public virtual bool isAllowed(char key)
        {
            return max_chars == (-1) || text.length() < max_chars;
        }
        // Java: 100
        public virtual void delete(int index)
        {
            text.deleteCharAt(index);
        }
        // Java: 104
        public virtual void appendNotify(string str)
        {
        }
        // Java: 107
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE);
            base.handleInput(@event);
        }
        public TextField(com.oddlabs.tt.font.Font font, int max_chars) : this("", font, max_chars)
        {
        }
        public TextField(string text, com.oddlabs.tt.font.Font font, int max_chars) : base()
        {
            this.font = font;
            this.text = new java.lang.StringBuilder((max_chars < java.lang.Integer.MAX_VALUE ? max_chars : text.length()));
            this.max_chars = max_chars;
            this.text.append(text);
        }
    }
}
