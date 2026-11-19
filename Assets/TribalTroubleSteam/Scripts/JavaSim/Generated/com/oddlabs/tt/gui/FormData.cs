// Translated from FormData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class FormData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Box form() { return form_f; }
        public com.oddlabs.tt.gui.Box slimForm() { return slimForm_f; }
        public com.oddlabs.tt.gui.ModeIconQuads formClose() { return formClose_f; }
        public int objectSpacing() { return objectSpacing_f; }
        public int sectionSpacing() { return sectionSpacing_f; }
        public int captionLeft() { return captionLeft_f; }
        public int captionY() { return captionY_f; }
        public int closeRight() { return closeRight_f; }
        public int closeTop() { return closeTop_f; }
        public com.oddlabs.tt.font.Font captionFont() { return captionFont_f; }
        public FormData(com.oddlabs.tt.gui.Box form, com.oddlabs.tt.gui.Box slimForm, com.oddlabs.tt.gui.ModeIconQuads formClose, int objectSpacing, int sectionSpacing, int captionLeft, int captionY, int closeRight, int closeTop, com.oddlabs.tt.font.Font captionFont)
        {
            this.form_f = form;
            this.slimForm_f = slimForm;
            this.formClose_f = formClose;
            this.objectSpacing_f = objectSpacing;
            this.sectionSpacing_f = sectionSpacing;
            this.captionLeft_f = captionLeft;
            this.captionY_f = captionY;
            this.closeRight_f = closeRight;
            this.closeTop_f = closeTop;
            this.captionFont_f = captionFont;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is FormData r)) return false; return true && java.lang.JO.equals(form_f, r.form_f) && java.lang.JO.equals(slimForm_f, r.slimForm_f) && java.lang.JO.equals(formClose_f, r.formClose_f) && java.lang.JO.equals(objectSpacing_f, r.objectSpacing_f) && java.lang.JO.equals(sectionSpacing_f, r.sectionSpacing_f) && java.lang.JO.equals(captionLeft_f, r.captionLeft_f) && java.lang.JO.equals(captionY_f, r.captionY_f) && java.lang.JO.equals(closeRight_f, r.closeRight_f) && java.lang.JO.equals(closeTop_f, r.closeTop_f) && java.lang.JO.equals(captionFont_f, r.captionFont_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(form_f); h = 31 * h + java.lang.JO.hashCode(slimForm_f); h = 31 * h + java.lang.JO.hashCode(formClose_f); h = 31 * h + java.lang.JO.hashCode(objectSpacing_f); h = 31 * h + java.lang.JO.hashCode(sectionSpacing_f); h = 31 * h + java.lang.JO.hashCode(captionLeft_f); h = 31 * h + java.lang.JO.hashCode(captionY_f); h = 31 * h + java.lang.JO.hashCode(closeRight_f); h = 31 * h + java.lang.JO.hashCode(closeTop_f); h = 31 * h + java.lang.JO.hashCode(captionFont_f); } return h; }
        public override string toString() { return "FormData[" + "form=" + java.lang.JO.toString(form_f) + ", slimForm=" + java.lang.JO.toString(slimForm_f) + ", formClose=" + java.lang.JO.toString(formClose_f) + ", objectSpacing=" + java.lang.JO.toString(objectSpacing_f) + ", sectionSpacing=" + java.lang.JO.toString(sectionSpacing_f) + ", captionLeft=" + java.lang.JO.toString(captionLeft_f) + ", captionY=" + java.lang.JO.toString(captionY_f) + ", closeRight=" + java.lang.JO.toString(closeRight_f) + ", closeTop=" + java.lang.JO.toString(closeTop_f) + ", captionFont=" + java.lang.JO.toString(captionFont_f) + "]"; }
        public com.oddlabs.tt.gui.Box form_f;
        public com.oddlabs.tt.gui.Box slimForm_f;
        public com.oddlabs.tt.gui.ModeIconQuads formClose_f;
        public int objectSpacing_f;
        public int sectionSpacing_f;
        public int captionLeft_f;
        public int captionY_f;
        public int closeRight_f;
        public int closeTop_f;
        public com.oddlabs.tt.font.Font captionFont_f;
    }
}
