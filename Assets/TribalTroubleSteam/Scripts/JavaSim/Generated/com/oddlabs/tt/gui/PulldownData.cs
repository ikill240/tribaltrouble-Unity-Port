// Translated from PulldownData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class PulldownData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Horizontal pulldownTop() { return pulldownTop_f; }
        public com.oddlabs.tt.gui.Horizontal pulldownBottom() { return pulldownBottom_f; }
        public com.oddlabs.tt.gui.Box pulldownItem() { return pulldownItem_f; }
        public com.oddlabs.tt.gui.Horizontal pulldownButton() { return pulldownButton_f; }
        public com.oddlabs.tt.gui.ModeIconQuads arrow() { return arrow_f; }
        public int arrowOffsetRight() { return arrowOffsetRight_f; }
        public int textOffsetLeft() { return textOffsetLeft_f; }
        public com.oddlabs.tt.font.Font font() { return font_f; }
        public PulldownData(com.oddlabs.tt.gui.Horizontal pulldownTop, com.oddlabs.tt.gui.Horizontal pulldownBottom, com.oddlabs.tt.gui.Box pulldownItem, com.oddlabs.tt.gui.Horizontal pulldownButton, com.oddlabs.tt.gui.ModeIconQuads arrow, int arrowOffsetRight, int textOffsetLeft, com.oddlabs.tt.font.Font font)
        {
            this.pulldownTop_f = pulldownTop;
            this.pulldownBottom_f = pulldownBottom;
            this.pulldownItem_f = pulldownItem;
            this.pulldownButton_f = pulldownButton;
            this.arrow_f = arrow;
            this.arrowOffsetRight_f = arrowOffsetRight;
            this.textOffsetLeft_f = textOffsetLeft;
            this.font_f = font;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is PulldownData r)) return false; return true && java.lang.JO.equals(pulldownTop_f, r.pulldownTop_f) && java.lang.JO.equals(pulldownBottom_f, r.pulldownBottom_f) && java.lang.JO.equals(pulldownItem_f, r.pulldownItem_f) && java.lang.JO.equals(pulldownButton_f, r.pulldownButton_f) && java.lang.JO.equals(arrow_f, r.arrow_f) && java.lang.JO.equals(arrowOffsetRight_f, r.arrowOffsetRight_f) && java.lang.JO.equals(textOffsetLeft_f, r.textOffsetLeft_f) && java.lang.JO.equals(font_f, r.font_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(pulldownTop_f); h = 31 * h + java.lang.JO.hashCode(pulldownBottom_f); h = 31 * h + java.lang.JO.hashCode(pulldownItem_f); h = 31 * h + java.lang.JO.hashCode(pulldownButton_f); h = 31 * h + java.lang.JO.hashCode(arrow_f); h = 31 * h + java.lang.JO.hashCode(arrowOffsetRight_f); h = 31 * h + java.lang.JO.hashCode(textOffsetLeft_f); h = 31 * h + java.lang.JO.hashCode(font_f); } return h; }
        public override string toString() { return "PulldownData[" + "pulldownTop=" + java.lang.JO.toString(pulldownTop_f) + ", pulldownBottom=" + java.lang.JO.toString(pulldownBottom_f) + ", pulldownItem=" + java.lang.JO.toString(pulldownItem_f) + ", pulldownButton=" + java.lang.JO.toString(pulldownButton_f) + ", arrow=" + java.lang.JO.toString(arrow_f) + ", arrowOffsetRight=" + java.lang.JO.toString(arrowOffsetRight_f) + ", textOffsetLeft=" + java.lang.JO.toString(textOffsetLeft_f) + ", font=" + java.lang.JO.toString(font_f) + "]"; }
        public com.oddlabs.tt.gui.Horizontal pulldownTop_f;
        public com.oddlabs.tt.gui.Horizontal pulldownBottom_f;
        public com.oddlabs.tt.gui.Box pulldownItem_f;
        public com.oddlabs.tt.gui.Horizontal pulldownButton_f;
        public com.oddlabs.tt.gui.ModeIconQuads arrow_f;
        public int arrowOffsetRight_f;
        public int textOffsetLeft_f;
        public com.oddlabs.tt.font.Font font_f;
    }
}
