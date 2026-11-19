// Translated from StretchParticle.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 6
    public class StretchParticle : com.oddlabs.tt.particle.Particle
    {
        public float src_x;
        public float src_y;
        public float src_z;
        public float dst_x;
        public float dst_y;
        public float dst_z;
        public float src_width;
        public float dst_width;
        // Java: 20
        public override void update(float t)
        {
            base.update(t);
        }
        // Java: 25
        public void setSrc(float x, float y, float z)
        {
            src_x = x;
            src_y = y;
            src_z = z;
        }
        // Java: 31
        public float getSrcX()
        {
            return src_x;
        }
        // Java: 35
        public float getSrcY()
        {
            return src_y;
        }
        // Java: 39
        public float getSrcZ()
        {
            return src_z;
        }
        // Java: 43
        public void setDst(float x, float y, float z)
        {
            dst_x = x;
            dst_y = y;
            dst_z = z;
        }
        // Java: 49
        public float getDstX()
        {
            return dst_x;
        }
        // Java: 53
        public float getDstY()
        {
            return dst_y;
        }
        // Java: 57
        public float getDstZ()
        {
            return dst_z;
        }
        // Java: 61
        public void setSrcWidth(float width)
        {
            src_width = width;
        }
        // Java: 65
        public float getSrcWidth()
        {
            return src_width;
        }
        // Java: 69
        public void setDstWidth(float width)
        {
            dst_width = width;
        }
        // Java: 73
        public float getDstWidth()
        {
            return dst_width;
        }
        private void init_StretchParticle()
        {
            this.src_x = 0.0f;
            this.src_y = 0.0f;
            this.src_z = 0.0f;
            this.dst_x = 0.0f;
            this.dst_y = 0.0f;
            this.dst_z = 0.0f;
            this.src_width = 0.0f;
            this.dst_width = 0.0f;
        }
        public StretchParticle(com.oddlabs.tt.landscape.World world) : base(world)
        {
            init_StretchParticle();
        }
    }
}
