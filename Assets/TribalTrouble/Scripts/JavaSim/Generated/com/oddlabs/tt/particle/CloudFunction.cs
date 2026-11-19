// Translated from CloudFunction.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 3
    public class CloudFunction : java.lang.Object, com.oddlabs.tt.particle.ParametricFunction
    {
        public float radius_xy;
        public float radius_z;
        // Java: 12
        public float getX(float u, float v)
        {
            return (float)((float)(radius_xy * ((float)(java.lang.StrictMath.sin(u)))) * ((float)(java.lang.StrictMath.cos(v))));
        }
        // Java: 17
        public float getY(float u, float v)
        {
            return (float)((float)(radius_xy * ((float)(java.lang.StrictMath.sin(u)))) * ((float)(java.lang.StrictMath.sin(v))));
        }
        // Java: 22
        public float getZ(float u, float v)
        {
            return (float)(radius_z * ((float)(java.lang.StrictMath.cos(u))));
        }
        public CloudFunction(float radius_xy, float radius_z) : base()
        {
            this.radius_xy = radius_xy;
            this.radius_z = radius_z;
        }
    }
}
