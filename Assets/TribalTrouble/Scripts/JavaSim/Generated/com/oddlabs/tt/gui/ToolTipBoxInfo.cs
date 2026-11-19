// Translated from ToolTipBoxInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public class ToolTipBoxInfo : java.lang.Object
    {
        public com.oddlabs.tt.gui.Horizontal box() { return box_f; }
        public int leftOffset() { return leftOffset_f; }
        public int bottomOffset() { return bottomOffset_f; }
        public int rightOffset() { return rightOffset_f; }
        public int topOffset() { return topOffset_f; }
        public ToolTipBoxInfo(com.oddlabs.tt.gui.Horizontal box, int leftOffset, int bottomOffset, int rightOffset, int topOffset)
        {
            this.box_f = box;
            this.leftOffset_f = leftOffset;
            this.bottomOffset_f = bottomOffset;
            this.rightOffset_f = rightOffset;
            this.topOffset_f = topOffset;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ToolTipBoxInfo r)) return false; return true && java.lang.JO.equals(box_f, r.box_f) && java.lang.JO.equals(leftOffset_f, r.leftOffset_f) && java.lang.JO.equals(bottomOffset_f, r.bottomOffset_f) && java.lang.JO.equals(rightOffset_f, r.rightOffset_f) && java.lang.JO.equals(topOffset_f, r.topOffset_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(box_f); h = 31 * h + java.lang.JO.hashCode(leftOffset_f); h = 31 * h + java.lang.JO.hashCode(bottomOffset_f); h = 31 * h + java.lang.JO.hashCode(rightOffset_f); h = 31 * h + java.lang.JO.hashCode(topOffset_f); } return h; }
        public override string toString() { return "ToolTipBoxInfo[" + "box=" + java.lang.JO.toString(box_f) + ", leftOffset=" + java.lang.JO.toString(leftOffset_f) + ", bottomOffset=" + java.lang.JO.toString(bottomOffset_f) + ", rightOffset=" + java.lang.JO.toString(rightOffset_f) + ", topOffset=" + java.lang.JO.toString(topOffset_f) + "]"; }
        public com.oddlabs.tt.gui.Horizontal box_f;
        public int leftOffset_f;
        public int bottomOffset_f;
        public int rightOffset_f;
        public int topOffset_f;
    }
}
