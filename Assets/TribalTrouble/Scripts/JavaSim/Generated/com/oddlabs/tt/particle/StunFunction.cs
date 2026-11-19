// Translated from StunFunction.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 3
    public class StunFunction : java.lang.Object, com.oddlabs.tt.particle.ParametricFunction
    {
        public float radius;
        public float height;
        // Java: 12
        public float getX(float u, float v)
        {
            return (float)(radius * ((float)(java.lang.StrictMath.cos(u))));
        }
        // Java: 17
        public float getY(float u, float v)
        {
            return (float)(radius * ((float)(java.lang.StrictMath.sin(u))));
        }
        // Java: 22
        public float getZ(float u, float v)
        {
            return (float)(height * ((float)(java.lang.StrictMath.cos(v))));
        }
        public StunFunction(float radius, float height) : base()
        {
            this.radius = radius;
            this.height = height;
        }
    }
}
