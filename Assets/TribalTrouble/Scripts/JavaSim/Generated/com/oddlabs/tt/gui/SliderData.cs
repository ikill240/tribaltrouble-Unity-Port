// Translated from SliderData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public class SliderData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Horizontal slider() { return slider_f; }
        public com.oddlabs.tt.gui.ModeIconQuads button() { return button_f; }
        public int leftOffset() { return leftOffset_f; }
        public int rightOffset() { return rightOffset_f; }
        public SliderData(com.oddlabs.tt.gui.Horizontal slider, com.oddlabs.tt.gui.ModeIconQuads button, int leftOffset, int rightOffset)
        {
            this.slider_f = slider;
            this.button_f = button;
            this.leftOffset_f = leftOffset;
            this.rightOffset_f = rightOffset;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is SliderData r)) return false; return true && java.lang.JO.equals(slider_f, r.slider_f) && java.lang.JO.equals(button_f, r.button_f) && java.lang.JO.equals(leftOffset_f, r.leftOffset_f) && java.lang.JO.equals(rightOffset_f, r.rightOffset_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(slider_f); h = 31 * h + java.lang.JO.hashCode(button_f); h = 31 * h + java.lang.JO.hashCode(leftOffset_f); h = 31 * h + java.lang.JO.hashCode(rightOffset_f); } return h; }
        public override string toString() { return "SliderData[" + "slider=" + java.lang.JO.toString(slider_f) + ", button=" + java.lang.JO.toString(button_f) + ", leftOffset=" + java.lang.JO.toString(leftOffset_f) + ", rightOffset=" + java.lang.JO.toString(rightOffset_f) + "]"; }
        public com.oddlabs.tt.gui.Horizontal slider_f;
        public com.oddlabs.tt.gui.ModeIconQuads button_f;
        public int leftOffset_f;
        public int rightOffset_f;
    }
}
