// Translated from Color.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public class Color : java.lang.Object
    {
        public float r() { return r_f; }
        public float g() { return g_f; }
        public float b() { return b_f; }
        public float a() { return a_f; }
        public Color(float r, float g, float b, float a)
        {
            this.r_f = r;
            this.g_f = g;
            this.b_f = b;
            this.a_f = a;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Color r)) return false; return true && java.lang.JO.equals(r_f, r.r_f) && java.lang.JO.equals(g_f, r.g_f) && java.lang.JO.equals(b_f, r.b_f) && java.lang.JO.equals(a_f, r.a_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(r_f); h = 31 * h + java.lang.JO.hashCode(g_f); h = 31 * h + java.lang.JO.hashCode(b_f); h = 31 * h + java.lang.JO.hashCode(a_f); } return h; }
        public override string toString() { return "Color[" + "r=" + java.lang.JO.toString(r_f) + ", g=" + java.lang.JO.toString(g_f) + ", b=" + java.lang.JO.toString(b_f) + ", a=" + java.lang.JO.toString(a_f) + "]"; }
        public float r_f;
        public float g_f;
        public float b_f;
        public float a_f;
        public Color(int r_v, int g_v, int b_v, int a_v) : this((float)(r_v / 255.0f), (float)(g_v / 255.0f), (float)(b_v / 255.0f), (float)(a_v / 255.0f))
        {
        }
    }
}
