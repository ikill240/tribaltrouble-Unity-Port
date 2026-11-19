// Translated from MultiColumnComboBoxData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class MultiColumnComboBoxData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Box box() { return box_f; }
        public com.oddlabs.tt.gui.Horizontal buttonPressed() { return buttonPressed_f; }
        public com.oddlabs.tt.gui.Horizontal buttonUnpressed() { return buttonUnpressed_f; }
        public com.oddlabs.tt.gui.ModeIconQuads descending() { return descending_f; }
        public com.oddlabs.tt.gui.ModeIconQuads ascending() { return ascending_f; }
        public org.joml.Vector4fc color1() { return color1_f; }
        public org.joml.Vector4fc color2() { return color2_f; }
        public org.joml.Vector4fc colorMarked() { return colorMarked_f; }
        public com.oddlabs.tt.font.Font font() { return font_f; }
        public int captionOffset() { return captionOffset_f; }
        public MultiColumnComboBoxData(com.oddlabs.tt.gui.Box box, com.oddlabs.tt.gui.Horizontal buttonPressed, com.oddlabs.tt.gui.Horizontal buttonUnpressed, com.oddlabs.tt.gui.ModeIconQuads descending, com.oddlabs.tt.gui.ModeIconQuads ascending, org.joml.Vector4fc color1, org.joml.Vector4fc color2, org.joml.Vector4fc colorMarked, com.oddlabs.tt.font.Font font, int captionOffset)
        {
            this.box_f = box;
            this.buttonPressed_f = buttonPressed;
            this.buttonUnpressed_f = buttonUnpressed;
            this.descending_f = descending;
            this.ascending_f = ascending;
            this.color1_f = color1;
            this.color2_f = color2;
            this.colorMarked_f = colorMarked;
            this.font_f = font;
            this.captionOffset_f = captionOffset;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is MultiColumnComboBoxData r)) return false; return true && java.lang.JO.equals(box_f, r.box_f) && java.lang.JO.equals(buttonPressed_f, r.buttonPressed_f) && java.lang.JO.equals(buttonUnpressed_f, r.buttonUnpressed_f) && java.lang.JO.equals(descending_f, r.descending_f) && java.lang.JO.equals(ascending_f, r.ascending_f) && java.lang.JO.equals(color1_f, r.color1_f) && java.lang.JO.equals(color2_f, r.color2_f) && java.lang.JO.equals(colorMarked_f, r.colorMarked_f) && java.lang.JO.equals(font_f, r.font_f) && java.lang.JO.equals(captionOffset_f, r.captionOffset_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(box_f); h = 31 * h + java.lang.JO.hashCode(buttonPressed_f); h = 31 * h + java.lang.JO.hashCode(buttonUnpressed_f); h = 31 * h + java.lang.JO.hashCode(descending_f); h = 31 * h + java.lang.JO.hashCode(ascending_f); h = 31 * h + java.lang.JO.hashCode(color1_f); h = 31 * h + java.lang.JO.hashCode(color2_f); h = 31 * h + java.lang.JO.hashCode(colorMarked_f); h = 31 * h + java.lang.JO.hashCode(font_f); h = 31 * h + java.lang.JO.hashCode(captionOffset_f); } return h; }
        public override string toString() { return "MultiColumnComboBoxData[" + "box=" + java.lang.JO.toString(box_f) + ", buttonPressed=" + java.lang.JO.toString(buttonPressed_f) + ", buttonUnpressed=" + java.lang.JO.toString(buttonUnpressed_f) + ", descending=" + java.lang.JO.toString(descending_f) + ", ascending=" + java.lang.JO.toString(ascending_f) + ", color1=" + java.lang.JO.toString(color1_f) + ", color2=" + java.lang.JO.toString(color2_f) + ", colorMarked=" + java.lang.JO.toString(colorMarked_f) + ", font=" + java.lang.JO.toString(font_f) + ", captionOffset=" + java.lang.JO.toString(captionOffset_f) + "]"; }
        public com.oddlabs.tt.gui.Box box_f;
        public com.oddlabs.tt.gui.Horizontal buttonPressed_f;
        public com.oddlabs.tt.gui.Horizontal buttonUnpressed_f;
        public com.oddlabs.tt.gui.ModeIconQuads descending_f;
        public com.oddlabs.tt.gui.ModeIconQuads ascending_f;
        public org.joml.Vector4fc color1_f;
        public org.joml.Vector4fc color2_f;
        public org.joml.Vector4fc colorMarked_f;
        public com.oddlabs.tt.font.Font font_f;
        public int captionOffset_f;
    }
}
