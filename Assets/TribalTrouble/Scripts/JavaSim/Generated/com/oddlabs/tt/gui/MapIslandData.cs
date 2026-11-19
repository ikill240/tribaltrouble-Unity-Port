// Translated from MapIslandData.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public class MapIslandData : java.lang.Object
    {
        public com.oddlabs.tt.gui.ModeIconQuads button() { return button_f; }
        public int x() { return x_f; }
        public int y() { return y_f; }
        public com.oddlabs.tt.gui.IconQuad flag() { return flag_f; }
        public com.oddlabs.tt.gui.IconQuad boat() { return boat_f; }
        public int pinX() { return pinX_f; }
        public int pinY() { return pinY_f; }
        public MapIslandData(com.oddlabs.tt.gui.ModeIconQuads button, int x, int y, com.oddlabs.tt.gui.IconQuad flag, com.oddlabs.tt.gui.IconQuad boat, int pinX, int pinY)
        {
            this.button_f = button;
            this.x_f = x;
            this.y_f = y;
            this.flag_f = flag;
            this.boat_f = boat;
            this.pinX_f = pinX;
            this.pinY_f = pinY;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is MapIslandData r)) return false; return true && java.lang.JO.equals(button_f, r.button_f) && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(flag_f, r.flag_f) && java.lang.JO.equals(boat_f, r.boat_f) && java.lang.JO.equals(pinX_f, r.pinX_f) && java.lang.JO.equals(pinY_f, r.pinY_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(button_f); h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(flag_f); h = 31 * h + java.lang.JO.hashCode(boat_f); h = 31 * h + java.lang.JO.hashCode(pinX_f); h = 31 * h + java.lang.JO.hashCode(pinY_f); } return h; }
        public override string toString() { return "MapIslandData[" + "button=" + java.lang.JO.toString(button_f) + ", x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", flag=" + java.lang.JO.toString(flag_f) + ", boat=" + java.lang.JO.toString(boat_f) + ", pinX=" + java.lang.JO.toString(pinX_f) + ", pinY=" + java.lang.JO.toString(pinY_f) + "]"; }
        public com.oddlabs.tt.gui.ModeIconQuads button_f;
        public int x_f;
        public int y_f;
        public com.oddlabs.tt.gui.IconQuad flag_f;
        public com.oddlabs.tt.gui.IconQuad boat_f;
        public int pinX_f;
        public int pinY_f;
    }
}
