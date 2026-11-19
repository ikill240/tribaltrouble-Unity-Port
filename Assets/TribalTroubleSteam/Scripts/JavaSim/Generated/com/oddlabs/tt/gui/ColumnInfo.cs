// Translated from ColumnInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public class ColumnInfo : java.lang.Object
    {
        public string caption() { return caption_f; }
        public int width() { return width_f; }
        public ColumnInfo(string caption, int width)
        {
            this.caption_f = caption;
            this.width_f = width;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ColumnInfo r)) return false; return true && java.lang.JO.equals(caption_f, r.caption_f) && java.lang.JO.equals(width_f, r.width_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(caption_f); h = 31 * h + java.lang.JO.hashCode(width_f); } return h; }
        public override string toString() { return "ColumnInfo[" + "caption=" + java.lang.JO.toString(caption_f) + ", width=" + java.lang.JO.toString(width_f) + "]"; }
        public string caption_f;
        public int width_f;
    }
}
