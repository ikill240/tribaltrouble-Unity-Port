// Translated from Quad.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 6
    public class Quad : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public float u1;
        public float v1;
        public float u2;
        public float v2;
        public int width;
        public int height;
        // Java: 26
        public virtual float getU1()
        {
            return u1;
        }
        // Java: 30
        public virtual float getV1()
        {
            return v1;
        }
        // Java: 34
        public virtual float getU2()
        {
            return u2;
        }
        // Java: 38
        public virtual float getV2()
        {
            return v2;
        }
        // Java: 42
        public virtual int getWidth()
        {
            return width;
        }
        // Java: 46
        public virtual int getHeight()
        {
            return height;
        }
        public Quad(float u1, float v1, float u2, float v2, int width, int height) : base()
        {
            this.u1 = u1;
            this.v1 = v1;
            this.u2 = u2;
            this.v2 = v2;
            this.width = width;
            this.height = height;
        }
    }
}
