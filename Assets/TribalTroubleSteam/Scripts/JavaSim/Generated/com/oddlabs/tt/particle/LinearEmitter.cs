// Translated from LinearEmitter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 18
    public abstract class LinearEmitter : com.oddlabs.tt.particle.Emitter
    {
        public static float SQRT_2;
        public java.util.Random random;
        public org.joml.Vector3f randomized_position;
        public float offset_z;
        public float emitter_radius;
        public float emitter_height;
        public float particles_per_second;
        public org.joml.Vector3fc velocity;
        public org.joml.Vector3fc acceleration;
        public org.joml.Vector4fc color;
        public org.joml.Vector3fc particle_radius;
        public org.joml.Vector3fc growth_rate;
        public float friction;
        public org.joml.Vector4fc delta_color;
        public float energy;
        public int num_particles;
        public float particle_counter;
        public bool started;
        // Java: 69
        public void setDeltaColor(org.joml.Vector4f delta_color)
        {
            this.delta_color = delta_color;
        }
        // Java: 73
        public void setEnergy(float energy)
        {
            this.energy = energy;
        }
        // Java: 77
        public void start()
        {
            started = true;
        }
        // Java: 81
        public void stop()
        {
            started = false;
        }
        // Java: 85
        public void done()
        {
            num_particles = 0;
        }
        // Java: 89
        public override void animate(float t)
        {
            if (started)
            {
                particle_counter += (float)(particles_per_second * t);
            }
            while (particle_counter >= 1 && (num_particles == (-1) || num_particles != 0) && started)
            {
                int initiated = initParticle(getPosition(), velocity, acceleration, color, delta_color, particle_radius, growth_rate, energy);
                particle_counter -= initiated;
                if (num_particles > 0)
                {
                    num_particles -= initiated;
                }
            }
            float x_min = java.lang.Float.POSITIVE_INFINITY;
            float x_max = java.lang.Float.NEGATIVE_INFINITY;
            float y_min = java.lang.Float.POSITIVE_INFINITY;
            float y_max = java.lang.Float.NEGATIVE_INFINITY;
            float z_min = java.lang.Float.POSITIVE_INFINITY;
            float z_max = java.lang.Float.NEGATIVE_INFINITY;
            int size = 0;
            foreach (java.util.List particles in getParticles())
            {
                java.util.Iterator iterator = particles.iterator();
                while (iterator.hasNext())
                {
                    com.oddlabs.tt.particle.LinearParticle particle = ((com.oddlabs.tt.particle.LinearParticle)iterator.next());
                    if (particle.getEnergy() <= 0.0f)
                    {
                        iterator.remove();
                        continue;
                    }
                    particle.update(t);
                    float x = particle.getPosX();
                    float y = particle.getPosY();
                    float z = particle.getPosZ();
                    float landscape_z = getWorld().getHeightMap().getNearestHeight(x, y);
                    if (z < (float)((float)(landscape_z + particle.getRadiusZ()) + offset_z))
                    {
                        particle.setPos(x, y, (float)((float)(landscape_z + particle.getRadiusZ()) + offset_z));
                        particle.setVelocity((float)(particle.getVelocityX() * friction), (float)(particle.getVelocityY() * friction), (float)((float)(-particle.getVelocityZ()) * friction));
                    }
                    float radius_x = (float)(particle.getRadiusX() * com.oddlabs.tt.particle.LinearEmitter.SQRT_2);
                    float radius_y = (float)(particle.getRadiusY() * com.oddlabs.tt.particle.LinearEmitter.SQRT_2);
                    float radius_z = (float)(particle.getRadiusZ() * com.oddlabs.tt.particle.LinearEmitter.SQRT_2);
                    x_min = java.lang.Math.min(x_min, (float)(x - radius_x));
                    x_max = java.lang.Math.max(x_max, (float)(x + radius_x));
                    y_min = java.lang.Math.min(y_min, (float)(y - radius_y));
                    y_max = java.lang.Math.max(y_max, (float)(y + radius_y));
                    z_min = java.lang.Math.min(z_min, (float)(z - radius_z));
                    z_max = java.lang.Math.max(z_max, (float)(z + radius_z));
                }
                size += particles.size();
            }
            setBounds(x_min, x_max, y_min, y_max, z_min, z_max);
            reregister();
            if (size == 0 && num_particles == 0)
            {
                remove();
            }
        }
        // Java: 150
        public abstract int initParticle(org.joml.Vector3f position, org.joml.Vector3fc velocity, org.joml.Vector3fc acceleration, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, org.joml.Vector3fc particle_radius, org.joml.Vector3fc growth_rate, float energy);
        // Java: 156
        public org.joml.Vector3f randomPosition()
        {
            float r = (float)(emitter_radius * ((float)((1 - random.nextGaussian()))));
            float a = (float)((float)(random.nextFloat() * ((float)(java.lang.Math.PI))) * 2);
            float x = (float)(((float)(java.lang.StrictMath.cos(a))) * r);
            float y = (float)(((float)(java.lang.StrictMath.sin(a))) * r);
            float z = (float)(random.nextFloat() * emitter_height);
            randomized_position.set((float)(getX() + x), (float)(getY() + y), (float)(getZ() + z));
            return randomized_position;
        }
        static LinearEmitter()
        {
            LinearEmitter.SQRT_2 = ((float)(java.lang.Math.sqrt(2.0f)));
        }
        private void init_LinearEmitter()
        {
            this.randomized_position = new org.joml.Vector3f();
            this.particle_counter = 0;
            this.started = true;
        }
        public LinearEmitter(com.oddlabs.tt.landscape.World world, org.joml.Vector3f position, float offset_z, float emitter_radius, float emitter_height, int num_particles, float particles_per_second, org.joml.Vector3fc velocity, org.joml.Vector3fc acceleration, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, org.joml.Vector3fc particle_radius, org.joml.Vector3fc growth_rate, float energy, float friction, int src_blend_func, int dst_blend_func, com.oddlabs.tt.render.TextureKey[] textures, com.oddlabs.tt.render.SpriteKey[] sprite_renderers, int types, com.oddlabs.tt.animation.AnimationManager manager) : base(world, position, src_blend_func, dst_blend_func, textures, sprite_renderers, types, manager)
        {
            init_LinearEmitter();
            this.offset_z = offset_z;
            this.emitter_radius = emitter_radius;
            this.emitter_height = emitter_height;
            this.num_particles = num_particles;
            this.particles_per_second = particles_per_second;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.color = color;
            this.delta_color = delta_color;
            this.particle_radius = particle_radius;
            this.growth_rate = growth_rate;
            this.energy = energy;
            this.friction = friction;
            random = new java.util.Random(java.lang.JC.f2l(((float)((float)(position.x() * position.y()) * position.z()))));
            position.set(position.x(), position.y(), (float)(position.z() + offset_z));
            register();
        }
    }
}
