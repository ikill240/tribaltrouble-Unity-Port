// Translated from DistanceFogInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 6
    public class DistanceFogInfo : com.oddlabs.tt.resource.FogInfo
    {
        public float start;
        public float end;
        public float height_factor;
        // Java: 20
        public float getStart()
        {
            return start;
        }
        // Java: 24
        public float getEnd()
        {
            return end;
        }
        // Java: 28
        public float getHeightFactor()
        {
            return height_factor;
        }
        public DistanceFogInfo(com.oddlabs.tt.resource.FogInfo.Mode mode, org.joml.Vector4fc color, float density, float height_factor, float start, float end) : base(mode, color, density)
        {
            this.height_factor = height_factor;
            this.start = start;
            this.end = end;
        }
    }
}
