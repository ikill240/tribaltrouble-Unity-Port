// Translated from BalancedParametricEmitter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 11
    public class BalancedParametricEmitter : com.oddlabs.tt.particle.ParametricEmitter
    {
        public int num_particles;
        public float dist_u;
        public float dist_v;
        public float margin_u;
        public float margin_v;
        // Java: 35
        public override int initParticle(com.oddlabs.tt.particle.ParametricFunction function, float velocity_u, float velocity_v, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, org.joml.Vector3fc particle_radius, org.joml.Vector3fc growth_rate, float energy)
        {
            for (int i = 0; i < num_particles; i++)
            {
                float u = (float)((float)(dist_u * i) / num_particles);
                float v = (float)((float)(dist_v * i) / num_particles);
                com.oddlabs.tt.particle.ParametricParticle particle = new com.oddlabs.tt.particle.ParametricParticle(getWorld(), function, u, v, 0.0f, 0.0f, 0.0f);
                org.joml.Vector3f offset = randomOffset(margin_u, margin_v, 0.0f);
                particle.setVelocity((float)(velocity_u + offset.x()), (float)(velocity_v + offset.y()));
                particle.setColor(color.x(), color.y(), color.z(), color.w());
                particle.setDeltaColor(delta_color.x(), delta_color.y(), delta_color.z(), delta_color.w());
                particle.setRadius(particle_radius.x(), particle_radius.y(), particle_radius.z());
                particle.setGrowthRate(growth_rate.x(), growth_rate.y(), growth_rate.z());
                particle.setEnergy(energy);
                particle.setType(getWorld().getRandom().nextInt(getTypes()));
                particle.update(0);
                add(particle);
            }
            return num_particles;
        }
        public BalancedParametricEmitter(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.particle.ParametricFunction function, org.joml.Vector3f position, float velocity_u, float velocity_v, float dist_u, float dist_v, int num_particles, float margin_u, float margin_v, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, org.joml.Vector3fc particle_radius, org.joml.Vector3fc growth_rate, float energy, int src_blend_func, int dst_blend_func, com.oddlabs.tt.render.TextureKey[] textures, com.oddlabs.tt.animation.AnimationManager manager) : base(world, function, position, 0.0f, 0.0f, velocity_u, velocity_v, 0.0f, num_particles, java.lang.Float.MAX_VALUE, color, delta_color, particle_radius, growth_rate, energy, src_blend_func, dst_blend_func, textures, manager)
        {
            this.num_particles = num_particles;
            this.dist_u = dist_u;
            this.dist_v = dist_v;
            this.margin_u = margin_u;
            this.margin_v = margin_v;
        }
    }
}
