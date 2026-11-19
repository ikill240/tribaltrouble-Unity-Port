// Translated from ProgressBarData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class ProgressBarData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Horizontal progressBar() { return progressBar_f; }
        public com.oddlabs.tt.gui.ModeIconQuads leftFill() { return leftFill_f; }
        public com.oddlabs.tt.gui.ModeIconQuads centerFill() { return centerFill_f; }
        public com.oddlabs.tt.gui.ModeIconQuads rightFill() { return rightFill_f; }
        public com.oddlabs.tt.font.Font font() { return font_f; }
        public ProgressBarData(com.oddlabs.tt.gui.Horizontal progressBar, com.oddlabs.tt.gui.ModeIconQuads leftFill, com.oddlabs.tt.gui.ModeIconQuads centerFill, com.oddlabs.tt.gui.ModeIconQuads rightFill, com.oddlabs.tt.font.Font font)
        {
            this.progressBar_f = progressBar;
            this.leftFill_f = leftFill;
            this.centerFill_f = centerFill;
            this.rightFill_f = rightFill;
            this.font_f = font;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ProgressBarData r)) return false; return true && java.lang.JO.equals(progressBar_f, r.progressBar_f) && java.lang.JO.equals(leftFill_f, r.leftFill_f) && java.lang.JO.equals(centerFill_f, r.centerFill_f) && java.lang.JO.equals(rightFill_f, r.rightFill_f) && java.lang.JO.equals(font_f, r.font_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(progressBar_f); h = 31 * h + java.lang.JO.hashCode(leftFill_f); h = 31 * h + java.lang.JO.hashCode(centerFill_f); h = 31 * h + java.lang.JO.hashCode(rightFill_f); h = 31 * h + java.lang.JO.hashCode(font_f); } return h; }
        public override string toString() { return "ProgressBarData[" + "progressBar=" + java.lang.JO.toString(progressBar_f) + ", leftFill=" + java.lang.JO.toString(leftFill_f) + ", centerFill=" + java.lang.JO.toString(centerFill_f) + ", rightFill=" + java.lang.JO.toString(rightFill_f) + ", font=" + java.lang.JO.toString(font_f) + "]"; }
        public com.oddlabs.tt.gui.Horizontal progressBar_f;
        public com.oddlabs.tt.gui.ModeIconQuads leftFill_f;
        public com.oddlabs.tt.gui.ModeIconQuads centerFill_f;
        public com.oddlabs.tt.gui.ModeIconQuads rightFill_f;
        public com.oddlabs.tt.font.Font font_f;
    }
}
