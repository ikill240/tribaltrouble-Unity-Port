// Translated from NotifyArrowData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public class NotifyArrowData : java.lang.Object
    {
        public com.oddlabs.tt.gui.IconQuad arrow() { return arrow_f; }
        public int headX() { return headX_f; }
        public int headY() { return headY_f; }
        public int endX() { return endX_f; }
        public int endY() { return endY_f; }
        public NotifyArrowData(com.oddlabs.tt.gui.IconQuad arrow, int headX, int headY, int endX, int endY)
        {
            this.arrow_f = arrow;
            this.headX_f = headX;
            this.headY_f = headY;
            this.endX_f = endX;
            this.endY_f = endY;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is NotifyArrowData r)) return false; return true && java.lang.JO.equals(arrow_f, r.arrow_f) && java.lang.JO.equals(headX_f, r.headX_f) && java.lang.JO.equals(headY_f, r.headY_f) && java.lang.JO.equals(endX_f, r.endX_f) && java.lang.JO.equals(endY_f, r.endY_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(arrow_f); h = 31 * h + java.lang.JO.hashCode(headX_f); h = 31 * h + java.lang.JO.hashCode(headY_f); h = 31 * h + java.lang.JO.hashCode(endX_f); h = 31 * h + java.lang.JO.hashCode(endY_f); } return h; }
        public override string toString() { return "NotifyArrowData[" + "arrow=" + java.lang.JO.toString(arrow_f) + ", headX=" + java.lang.JO.toString(headX_f) + ", headY=" + java.lang.JO.toString(headY_f) + ", endX=" + java.lang.JO.toString(endX_f) + ", endY=" + java.lang.JO.toString(endY_f) + "]"; }
        public com.oddlabs.tt.gui.IconQuad arrow_f;
        public int headX_f;
        public int headY_f;
        public int endX_f;
        public int endY_f;
    }
}
