// Translated from PanelData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public class PanelData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Box box() { return box_f; }
        public com.oddlabs.tt.gui.Horizontal tab() { return tab_f; }
        public int leftCaptionOffset() { return leftCaptionOffset_f; }
        public int rightCaptionOffset() { return rightCaptionOffset_f; }
        public int bottomCaptionOffset() { return bottomCaptionOffset_f; }
        public int leftTabOffset() { return leftTabOffset_f; }
        public int bottomTabOffset() { return bottomTabOffset_f; }
        public PanelData(com.oddlabs.tt.gui.Box box, com.oddlabs.tt.gui.Horizontal tab, int leftCaptionOffset, int rightCaptionOffset, int bottomCaptionOffset, int leftTabOffset, int bottomTabOffset)
        {
            this.box_f = box;
            this.tab_f = tab;
            this.leftCaptionOffset_f = leftCaptionOffset;
            this.rightCaptionOffset_f = rightCaptionOffset;
            this.bottomCaptionOffset_f = bottomCaptionOffset;
            this.leftTabOffset_f = leftTabOffset;
            this.bottomTabOffset_f = bottomTabOffset;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is PanelData r)) return false; return true && java.lang.JO.equals(box_f, r.box_f) && java.lang.JO.equals(tab_f, r.tab_f) && java.lang.JO.equals(leftCaptionOffset_f, r.leftCaptionOffset_f) && java.lang.JO.equals(rightCaptionOffset_f, r.rightCaptionOffset_f) && java.lang.JO.equals(bottomCaptionOffset_f, r.bottomCaptionOffset_f) && java.lang.JO.equals(leftTabOffset_f, r.leftTabOffset_f) && java.lang.JO.equals(bottomTabOffset_f, r.bottomTabOffset_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(box_f); h = 31 * h + java.lang.JO.hashCode(tab_f); h = 31 * h + java.lang.JO.hashCode(leftCaptionOffset_f); h = 31 * h + java.lang.JO.hashCode(rightCaptionOffset_f); h = 31 * h + java.lang.JO.hashCode(bottomCaptionOffset_f); h = 31 * h + java.lang.JO.hashCode(leftTabOffset_f); h = 31 * h + java.lang.JO.hashCode(bottomTabOffset_f); } return h; }
        public override string toString() { return "PanelData[" + "box=" + java.lang.JO.toString(box_f) + ", tab=" + java.lang.JO.toString(tab_f) + ", leftCaptionOffset=" + java.lang.JO.toString(leftCaptionOffset_f) + ", rightCaptionOffset=" + java.lang.JO.toString(rightCaptionOffset_f) + ", bottomCaptionOffset=" + java.lang.JO.toString(bottomCaptionOffset_f) + ", leftTabOffset=" + java.lang.JO.toString(leftTabOffset_f) + ", bottomTabOffset=" + java.lang.JO.toString(bottomTabOffset_f) + "]"; }
        public com.oddlabs.tt.gui.Box box_f;
        public com.oddlabs.tt.gui.Horizontal tab_f;
        public int leftCaptionOffset_f;
        public int rightCaptionOffset_f;
        public int bottomCaptionOffset_f;
        public int leftTabOffset_f;
        public int bottomTabOffset_f;
    }
}
