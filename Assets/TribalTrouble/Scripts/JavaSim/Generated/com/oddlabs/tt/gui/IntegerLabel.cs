// Translated from IntegerLabel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 8
    public class IntegerLabel : com.oddlabs.tt.gui.Label
    {
        public int val;
        // Java: 21
        public override int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.Label o = (com.oddlabs.tt.gui.Label)o_p;
            return ((o is com.oddlabs.tt.gui.IntegerLabel other) ? val - other.val : (-1));
        }
        public IntegerLabel(int val, com.oddlabs.tt.font.Font font, int width) : base(java.lang.Integer.toString(val), font, width, com.oddlabs.tt.gui.Origin.AT_END)
        {
            this.val = val;
        }
        public IntegerLabel(int val, com.oddlabs.tt.font.Font font) : base(java.lang.Integer.toString(val), font)
        {
            this.val = val;
        }
    }
}
