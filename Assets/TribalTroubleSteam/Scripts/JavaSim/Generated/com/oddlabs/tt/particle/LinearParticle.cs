// Translated from LinearParticle.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 6
    public class LinearParticle : com.oddlabs.tt.particle.Particle
    {
        public float velocity_x;
        public float velocity_y;
        public float velocity_z;
        public float acceleration_x;
        public float acceleration_y;
        public float acceleration_z;
        // Java: 22
        public override void update(float t)
        {
            base.update(t);
            velocity_x += (float)(acceleration_x * t);
            velocity_y += (float)(acceleration_y * t);
            velocity_z += (float)(acceleration_z * t);
            float x = (float)(getPosX() + (float)(velocity_x * t));
            float y = (float)(getPosY() + (float)(velocity_y * t));
            float z = (float)(getPosZ() + (float)(velocity_z * t));
            setPos(x, y, z);
        }
        // Java: 34
        public void setVelocity(float x, float y, float z)
        {
            velocity_x = x;
            velocity_y = y;
            velocity_z = z;
        }
        // Java: 40
        public float getVelocityX()
        {
            return velocity_x;
        }
        // Java: 44
        public float getVelocityY()
        {
            return velocity_y;
        }
        // Java: 48
        public float getVelocityZ()
        {
            return velocity_z;
        }
        // Java: 52
        public void setAcceleration(float x, float y, float z)
        {
            acceleration_x = x;
            acceleration_y = y;
            acceleration_z = z;
        }
        // Java: 58
        public float getAccelerationX()
        {
            return acceleration_x;
        }
        // Java: 62
        public float getAccelerationY()
        {
            return acceleration_y;
        }
        // Java: 66
        public float getAccelerationZ()
        {
            return acceleration_z;
        }
        private void init_LinearParticle()
        {
            this.velocity_x = 0.0f;
            this.velocity_y = 0.0f;
            this.velocity_z = 0.0f;
            this.acceleration_x = 0.0f;
            this.acceleration_y = 0.0f;
            this.acceleration_z = 0.0f;
        }
        public LinearParticle(com.oddlabs.tt.landscape.World world) : this(world, 0.0f)
        {
        }
        public LinearParticle(com.oddlabs.tt.landscape.World world, float angle) : base(world, angle)
        {
            init_LinearParticle();
        }
    }
}
