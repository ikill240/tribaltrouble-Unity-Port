// Translated from RandomAccelerationEmitter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 15
    public class RandomAccelerationEmitter : com.oddlabs.tt.particle.LinearEmitter
    {
        public java.util.Random random;
        public float angle_bound;
        public float angle_max_jump;
        public org.joml.Vector3f acceleration;
        public org.joml.Vector3f offset_acceleration;
        public float acceleration_factor;
        public float x_angle;
        public float y_angle;
        // Java: 66
        public override int initParticle(org.joml.Vector3f position, org.joml.Vector3fc velocity, org.joml.Vector3fc acceleration, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, org.joml.Vector3fc particle_radius, org.joml.Vector3fc growth_rate, float energy)
        {
            randomizeAcceleration();
            com.oddlabs.tt.particle.LinearParticle particle = new com.oddlabs.tt.particle.LinearParticle(getWorld());
            org.joml.Vector3f pos = randomPosition();
            particle.setPos(pos.x(), pos.y(), pos.z());
            particle.setVelocity(velocity.x(), velocity.y(), velocity.z());
            particle.setAcceleration(acceleration.x(), acceleration.y(), acceleration.z());
            particle.setColor(color.x(), color.y(), color.z(), color.w());
            particle.setDeltaColor(delta_color.x(), delta_color.y(), delta_color.z(), delta_color.w());
            particle.setRadius(particle_radius.x(), particle_radius.y(), particle_radius.z());
            particle.setGrowthRate(growth_rate.x(), growth_rate.y(), growth_rate.z());
            particle.setEnergy(energy);
            particle.setType(random.nextInt(getTypes()));
            add(particle);
            return 1;
        }
        // Java: 87
        public void randomizeAcceleration()
        {
            float dx_angle = (float)((float)(random.nextFloat() * angle_max_jump) - (float)(0.5f * angle_max_jump));
            float dy_angle = (float)((float)(random.nextFloat() * angle_max_jump) - (float)(0.5f * angle_max_jump));
            if (((float)(x_angle + dx_angle) < (float)(-angle_bound)) || ((float)(x_angle + dx_angle) > angle_bound))
            {
                x_angle -= dx_angle;
            }
            else
            {
                x_angle += dx_angle;
            }
            if (((float)(y_angle + dy_angle) < (float)(-angle_bound)) || ((float)(y_angle + dy_angle) > angle_bound))
            {
                y_angle -= dy_angle;
            }
            else
            {
                y_angle += dy_angle;
            }
            float x = (float)(offset_acceleration.x() + (float)(acceleration_factor * ((float)(java.lang.StrictMath.sin(x_angle)))));
            float y = (float)(offset_acceleration.y() + (float)(acceleration_factor * ((float)(java.lang.StrictMath.sin(y_angle)))));
            acceleration.set(x, y, offset_acceleration.z());
        }
        private void init_RandomAccelerationEmitter()
        {
            this.x_angle = 0;
            this.y_angle = 0;
        }
        public RandomAccelerationEmitter(com.oddlabs.tt.landscape.World world, org.joml.Vector3f position, float offset_z, float emitter_radius, float emitter_height, float angle_bound, float angle_max_jump, int num_particles, float particles_per_second, org.joml.Vector3f velocity, org.joml.Vector3f acceleration, float acceleration_factor, org.joml.Vector4f color, org.joml.Vector4f delta_color, org.joml.Vector3f particle_radius, org.joml.Vector3f growth_rate, float energy, float friction, int src_blend_func, int dst_blend_func, com.oddlabs.tt.render.TextureKey[] textures, com.oddlabs.tt.render.SpriteKey[] sprite_renderers, int types, com.oddlabs.tt.animation.AnimationManager manager) : base(world, position, offset_z, emitter_radius, emitter_height, num_particles, particles_per_second, velocity, acceleration, color, delta_color, particle_radius, growth_rate, energy, friction, src_blend_func, dst_blend_func, textures, sprite_renderers, types, manager)
        {
            init_RandomAccelerationEmitter();
            this.random = world.getRandom();
            this.acceleration = acceleration;
            offset_acceleration = new org.joml.Vector3f(acceleration.x(), acceleration.y(), acceleration.z());
            this.angle_bound = angle_bound;
            this.angle_max_jump = angle_max_jump;
            this.acceleration_factor = acceleration_factor;
        }
        public RandomAccelerationEmitter(com.oddlabs.tt.landscape.World world, org.joml.Vector3f position, float offset_z, float emitter_radius, float emitter_height, float angle_bound, float angle_max_jump, int num_particles, float particles_per_second, org.joml.Vector3f velocity, org.joml.Vector3f acceleration, float acceleration_factor, org.joml.Vector4f color, org.joml.Vector4f delta_color, org.joml.Vector3f particle_radius, org.joml.Vector3f growth_rate, float energy, float friction, int src_blend_func, int dst_blend_func, com.oddlabs.tt.render.TextureKey[] textures, com.oddlabs.tt.animation.AnimationManager manager) : this(world, position, offset_z, emitter_radius, emitter_height, angle_bound, angle_max_jump, num_particles, particles_per_second, velocity, acceleration, acceleration_factor, color, delta_color, particle_radius, growth_rate, energy, friction, src_blend_func, dst_blend_func, textures, null, textures.Length, manager)
        {
        }
        public RandomAccelerationEmitter(com.oddlabs.tt.landscape.World world, org.joml.Vector3f position, float offset_z, float emitter_radius, float emitter_height, float angle_bound, float angle_max_jump, int num_particles, float particles_per_second, org.joml.Vector3f velocity, org.joml.Vector3f acceleration, float acceleration_factor, org.joml.Vector4f color, org.joml.Vector4f delta_color, org.joml.Vector3f particle_radius, org.joml.Vector3f growth_rate, float energy, float friction, com.oddlabs.tt.render.SpriteKey[] sprite_renderers, com.oddlabs.tt.animation.AnimationManager manager) : this(world, position, offset_z, emitter_radius, emitter_height, angle_bound, angle_max_jump, num_particles, particles_per_second, velocity, acceleration, acceleration_factor, color, delta_color, particle_radius, growth_rate, energy, friction, 0, 0, null, sprite_renderers, sprite_renderers.Length, manager)
        {
        }
    }
}
