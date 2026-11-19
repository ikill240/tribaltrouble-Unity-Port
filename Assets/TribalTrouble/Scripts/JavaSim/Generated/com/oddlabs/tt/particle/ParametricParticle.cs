// Translated from ParametricParticle.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 6
    public class ParametricParticle : com.oddlabs.tt.particle.Particle
    {
        public com.oddlabs.tt.particle.ParametricFunction function;
        public float offset_x;
        public float offset_y;
        public float offset_z;
        public float velocity_u;
        public float velocity_v;
        public float u;
        public float v;
        // Java: 28
        public void update(float t, float scale_x, float scale_y, float scale_z)
        {
            base.update(t);
            u += (float)(velocity_u * t);
            v += (float)(velocity_v * t);
            float x = (float)(offset_x + (float)(scale_x * function.getX(u, v)));
            float y = (float)(offset_y + (float)(scale_y * function.getY(u, v)));
            float z = (float)(offset_z + (float)(scale_z * function.getZ(u, v)));
            setPos(x, y, z);
        }
        // Java: 39
        public void setVelocity(float u, float v)
        {
            velocity_u = u;
            velocity_v = v;
        }
        // Java: 44
        public float getVelocityU()
        {
            return velocity_u;
        }
        // Java: 48
        public float getVelocityV()
        {
            return velocity_v;
        }
        private void init_ParametricParticle()
        {
            this.velocity_u = 0.0f;
            this.velocity_v = 0.0f;
            this.u = 0.0f;
            this.v = 0.0f;
        }
        public ParametricParticle(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.particle.ParametricFunction function, float u, float v, float offset_x, float offset_y, float offset_z) : base(world)
        {
            init_ParametricParticle();
            this.function = function;
            this.u = u;
            this.v = v;
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.offset_z = offset_z;
        }
    }
}
