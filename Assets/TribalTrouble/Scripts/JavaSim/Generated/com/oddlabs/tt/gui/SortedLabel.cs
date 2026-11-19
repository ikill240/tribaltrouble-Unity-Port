// Translated from SortedLabel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class SortedLabel : com.oddlabs.tt.gui.Label
    {
        public int index;
        // Java: 14
        public override int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.Label o = (com.oddlabs.tt.gui.Label)o_p;
            if ((o is com.oddlabs.tt.gui.SortedLabel other))
            {
                return index - other.index;
            }
            else
            {
                return (-1);
            }
        }
        public SortedLabel(string text, int index, com.oddlabs.tt.font.Font font) : base(text, font)
        {
            this.index = index;
        }
    }
}
