// Translated from ScrollBarData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public class ScrollBarData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Vertical scrollBar() { return scrollBar_f; }
        public com.oddlabs.tt.gui.ModeIconQuads scrollDownButtonPressed() { return scrollDownButtonPressed_f; }
        public com.oddlabs.tt.gui.ModeIconQuads scrollDownButtonUnpressed() { return scrollDownButtonUnpressed_f; }
        public com.oddlabs.tt.gui.ModeIconQuads scrollDownArrow() { return scrollDownArrow_f; }
        public com.oddlabs.tt.gui.ModeIconQuads scrollUpButtonPressed() { return scrollUpButtonPressed_f; }
        public com.oddlabs.tt.gui.ModeIconQuads scrollUpButtonUnpressed() { return scrollUpButtonUnpressed_f; }
        public com.oddlabs.tt.gui.ModeIconQuads scrollUpArrow() { return scrollUpArrow_f; }
        public com.oddlabs.tt.gui.Vertical scrollButton() { return scrollButton_f; }
        public int leftOffset() { return leftOffset_f; }
        public int bottomOffset() { return bottomOffset_f; }
        public int topOffset() { return topOffset_f; }
        public ScrollBarData(com.oddlabs.tt.gui.Vertical scrollBar, com.oddlabs.tt.gui.ModeIconQuads scrollDownButtonPressed, com.oddlabs.tt.gui.ModeIconQuads scrollDownButtonUnpressed, com.oddlabs.tt.gui.ModeIconQuads scrollDownArrow, com.oddlabs.tt.gui.ModeIconQuads scrollUpButtonPressed, com.oddlabs.tt.gui.ModeIconQuads scrollUpButtonUnpressed, com.oddlabs.tt.gui.ModeIconQuads scrollUpArrow, com.oddlabs.tt.gui.Vertical scrollButton, int leftOffset, int bottomOffset, int topOffset)
        {
            this.scrollBar_f = scrollBar;
            this.scrollDownButtonPressed_f = scrollDownButtonPressed;
            this.scrollDownButtonUnpressed_f = scrollDownButtonUnpressed;
            this.scrollDownArrow_f = scrollDownArrow;
            this.scrollUpButtonPressed_f = scrollUpButtonPressed;
            this.scrollUpButtonUnpressed_f = scrollUpButtonUnpressed;
            this.scrollUpArrow_f = scrollUpArrow;
            this.scrollButton_f = scrollButton;
            this.leftOffset_f = leftOffset;
            this.bottomOffset_f = bottomOffset;
            this.topOffset_f = topOffset;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ScrollBarData r)) return false; return true && java.lang.JO.equals(scrollBar_f, r.scrollBar_f) && java.lang.JO.equals(scrollDownButtonPressed_f, r.scrollDownButtonPressed_f) && java.lang.JO.equals(scrollDownButtonUnpressed_f, r.scrollDownButtonUnpressed_f) && java.lang.JO.equals(scrollDownArrow_f, r.scrollDownArrow_f) && java.lang.JO.equals(scrollUpButtonPressed_f, r.scrollUpButtonPressed_f) && java.lang.JO.equals(scrollUpButtonUnpressed_f, r.scrollUpButtonUnpressed_f) && java.lang.JO.equals(scrollUpArrow_f, r.scrollUpArrow_f) && java.lang.JO.equals(scrollButton_f, r.scrollButton_f) && java.lang.JO.equals(leftOffset_f, r.leftOffset_f) && java.lang.JO.equals(bottomOffset_f, r.bottomOffset_f) && java.lang.JO.equals(topOffset_f, r.topOffset_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(scrollBar_f); h = 31 * h + java.lang.JO.hashCode(scrollDownButtonPressed_f); h = 31 * h + java.lang.JO.hashCode(scrollDownButtonUnpressed_f); h = 31 * h + java.lang.JO.hashCode(scrollDownArrow_f); h = 31 * h + java.lang.JO.hashCode(scrollUpButtonPressed_f); h = 31 * h + java.lang.JO.hashCode(scrollUpButtonUnpressed_f); h = 31 * h + java.lang.JO.hashCode(scrollUpArrow_f); h = 31 * h + java.lang.JO.hashCode(scrollButton_f); h = 31 * h + java.lang.JO.hashCode(leftOffset_f); h = 31 * h + java.lang.JO.hashCode(bottomOffset_f); h = 31 * h + java.lang.JO.hashCode(topOffset_f); } return h; }
        public override string toString() { return "ScrollBarData[" + "scrollBar=" + java.lang.JO.toString(scrollBar_f) + ", scrollDownButtonPressed=" + java.lang.JO.toString(scrollDownButtonPressed_f) + ", scrollDownButtonUnpressed=" + java.lang.JO.toString(scrollDownButtonUnpressed_f) + ", scrollDownArrow=" + java.lang.JO.toString(scrollDownArrow_f) + ", scrollUpButtonPressed=" + java.lang.JO.toString(scrollUpButtonPressed_f) + ", scrollUpButtonUnpressed=" + java.lang.JO.toString(scrollUpButtonUnpressed_f) + ", scrollUpArrow=" + java.lang.JO.toString(scrollUpArrow_f) + ", scrollButton=" + java.lang.JO.toString(scrollButton_f) + ", leftOffset=" + java.lang.JO.toString(leftOffset_f) + ", bottomOffset=" + java.lang.JO.toString(bottomOffset_f) + ", topOffset=" + java.lang.JO.toString(topOffset_f) + "]"; }
        public com.oddlabs.tt.gui.Vertical scrollBar_f;
        public com.oddlabs.tt.gui.ModeIconQuads scrollDownButtonPressed_f;
        public com.oddlabs.tt.gui.ModeIconQuads scrollDownButtonUnpressed_f;
        public com.oddlabs.tt.gui.ModeIconQuads scrollDownArrow_f;
        public com.oddlabs.tt.gui.ModeIconQuads scrollUpButtonPressed_f;
        public com.oddlabs.tt.gui.ModeIconQuads scrollUpButtonUnpressed_f;
        public com.oddlabs.tt.gui.ModeIconQuads scrollUpArrow_f;
        public com.oddlabs.tt.gui.Vertical scrollButton_f;
        public int leftOffset_f;
        public int bottomOffset_f;
        public int topOffset_f;
    }
}
