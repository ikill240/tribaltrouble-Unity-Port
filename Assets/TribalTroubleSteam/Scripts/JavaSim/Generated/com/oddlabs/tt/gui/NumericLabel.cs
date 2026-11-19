// Translated from NumericLabel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class NumericLabel : com.oddlabs.tt.gui.Label
    {
        public double @value;
        // Java: 24
        public double getValue()
        {
            return @value;
        }
        // Java: 28
        public void setNumeric(string text, double @value)
        {
            setText(text);
            this.@value = @value;
        }
        // Java: 33
        public override int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.Label o = (com.oddlabs.tt.gui.Label)o_p;
            return ((o is com.oddlabs.tt.gui.NumericLabel x) ? java.lang.Double.compare(@value, x.@value) : (-1));
        }
        public NumericLabel(string text, com.oddlabs.tt.font.Font font, double @value) : this(text, font, font.getWidth(text), @value)
        {
        }
        public NumericLabel(string text, com.oddlabs.tt.font.Font font, int width, double @value) : this(text, font, width, com.oddlabs.tt.gui.Origin.AT_END, @value)
        {
        }
        public NumericLabel(string text, com.oddlabs.tt.font.Font font, int width, com.oddlabs.tt.gui.Origin align, double @value) : base(text, font, width, align)
        {
            this.@value = @value;
        }
    }
}
