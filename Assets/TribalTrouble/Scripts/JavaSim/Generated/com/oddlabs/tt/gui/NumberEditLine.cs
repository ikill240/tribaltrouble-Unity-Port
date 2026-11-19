// Translated from NumberEditLine.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class NumberEditLine : com.oddlabs.tt.gui.EditLine
    {
        public java.util.Set value_listeners;
        public long min_value;
        public long max_value;
        public long @value;
        // Java: 28
        public void addValueListener(com.oddlabs.tt.guievent.ValueListener listener)
        {
            value_listeners.add(listener);
        }
        // Java: 32
        public void removeValueListener(com.oddlabs.tt.guievent.ValueListener listener)
        {
            value_listeners.remove(listener);
        }
        // Java: 36
        public override void enterPressed(string text)
        {
            validate();
        }
        // Java: 41
        public void validate()
        {
            string str = java.lang.JO.toString(getText());
            long @value = default;
            try
            {
                @value = crop(java.lang.Long.parseLong(str));
            }
            catch (System.FormatException unused)
            {
                @value = min_value;
            }
            setValue(@value);
        }
        // Java: 54
        public long crop(long @value)
        {
            return java.lang.Math.clamp(@value, min_value, max_value);
        }
        // Java: 58
        public void setValue(long @value)
        {
            clear();
            @value = crop(@value);
            append(@value);
            if (@value != this.@value)
            {
                this.@value = @value;
                foreach (com.oddlabs.tt.guievent.ValueListener listener in java.lang.JIter.of(value_listeners))
                {
                    listener.valueSet(this.@value);
                }
            }
        }
        // Java: 71
        public long getValue()
        {
            validate();
            return @value;
        }
        // Java: 76
        public override void focusNotify(bool focus)
        {
            validate();
            base.focusNotify(focus);
        }
        private void init_NumberEditLine()
        {
            this.value_listeners = new java.util.concurrent.CopyOnWriteArraySet();
        }
        public NumberEditLine(int width, int max_chars, int max_value) : this(width, max_chars, 0, max_value, 0)
        {
        }
        public NumberEditLine(int width, int max_chars, int min_value, int max_value, int init_value) : base(width, max_chars, "0123456789", com.oddlabs.tt.gui.Origin.AT_END)
        {
            init_NumberEditLine();
            this.min_value = min_value;
            this.max_value = max_value;
            setValue(init_value);
        }
    }
}
