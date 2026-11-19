// Translated from GroupData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public class GroupData : java.lang.Object
    {
        public com.oddlabs.tt.gui.Box group() { return group_f; }
        public int captionLeft() { return captionLeft_f; }
        public int captionY() { return captionY_f; }
        public int captionOffset() { return captionOffset_f; }
        public com.oddlabs.tt.font.Font captionFont() { return captionFont_f; }
        public GroupData(com.oddlabs.tt.gui.Box group, int captionLeft, int captionY, int captionOffset, com.oddlabs.tt.font.Font captionFont)
        {
            this.group_f = group;
            this.captionLeft_f = captionLeft;
            this.captionY_f = captionY;
            this.captionOffset_f = captionOffset;
            this.captionFont_f = captionFont;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is GroupData r)) return false; return true && java.lang.JO.equals(group_f, r.group_f) && java.lang.JO.equals(captionLeft_f, r.captionLeft_f) && java.lang.JO.equals(captionY_f, r.captionY_f) && java.lang.JO.equals(captionOffset_f, r.captionOffset_f) && java.lang.JO.equals(captionFont_f, r.captionFont_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(group_f); h = 31 * h + java.lang.JO.hashCode(captionLeft_f); h = 31 * h + java.lang.JO.hashCode(captionY_f); h = 31 * h + java.lang.JO.hashCode(captionOffset_f); h = 31 * h + java.lang.JO.hashCode(captionFont_f); } return h; }
        public override string toString() { return "GroupData[" + "group=" + java.lang.JO.toString(group_f) + ", captionLeft=" + java.lang.JO.toString(captionLeft_f) + ", captionY=" + java.lang.JO.toString(captionY_f) + ", captionOffset=" + java.lang.JO.toString(captionOffset_f) + ", captionFont=" + java.lang.JO.toString(captionFont_f) + "]"; }
        public com.oddlabs.tt.gui.Box group_f;
        public int captionLeft_f;
        public int captionY_f;
        public int captionOffset_f;
        public com.oddlabs.tt.font.Font captionFont_f;
    }
}
