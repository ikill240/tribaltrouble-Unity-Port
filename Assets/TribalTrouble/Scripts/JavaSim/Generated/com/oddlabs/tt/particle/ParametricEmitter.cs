// Translated from ParametricEmitter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 16
    public class ParametricEmitter : com.oddlabs.tt.particle.Emitter
    {
        public static float SQRT_2;
        public java.util.Random random;
        public org.joml.Vector3f randomized_offset;
        public com.oddlabs.tt.particle.ParametricFunction function;
        public float area_xy;
        public float area_z;
        public float velocity_u;
        public float velocity_v;
        public float velocity_random_margin;
        public float particles_per_second;
        public org.joml.Vector4fc color;
        public org.joml.Vector3fc particle_radius;
        public org.joml.Vector3fc growth_rate;
        public org.joml.Vector4fc delta_color;
        public float energy;
        public int num_particles;
        public float particle_counter;
        public bool started;
        // Java: 63
        public void setDeltaColor(org.joml.Vector4f delta_color)
        {
            this.delta_color = delta_color;
        }
        // Java: 67
        public void setEnergy(float energy)
        {
            this.energy = energy;
        }
        // Java: 71
        public void start()
        {
            started = true;
        }
        // Java: 75
        public void stop()
        {
            started = false;
        }
        // Java: 79
        public void done()
        {
            if (isRegistered())
            {
                remove();
            }
        }
        // Java: 85
        public override void animate(float t)
        {
            if (started)
            {
                particle_counter += (float)(particles_per_second * t);
            }
            while (particle_counter >= 1 && (num_particles == (-1) || num_particles != 0) && started)
            {
                int initiated = initParticle(function, velocity_u, velocity_v, color, delta_color, particle_radius, growth_rate, energy);
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
            foreach (java.util.List list in getParticles())
            {
                java.util.Iterator particles = list.iterator();
                while (particles.hasNext())
                {
                    com.oddlabs.tt.particle.ParametricParticle particle = ((com.oddlabs.tt.particle.ParametricParticle)particles.next());
                    if (particle.getEnergy() <= 0.0f)
                    {
                        particles.remove();
                        continue;
                    }
                    particle.update(t, getScaleX(), getScaleY(), getScaleZ());
                    float x = particle.getPosX();
                    float y = particle.getPosY();
                    float z = particle.getPosZ();
                    particle.setPos((float)(getX() + x), (float)(getY() + y), (float)(getZ() + z));
                    x = particle.getPosX();
                    y = particle.getPosY();
                    z = particle.getPosZ();
                    float radius_x = (float)(particle.getRadiusX() * com.oddlabs.tt.particle.ParametricEmitter.SQRT_2);
                    float radius_y = (float)(particle.getRadiusY() * com.oddlabs.tt.particle.ParametricEmitter.SQRT_2);
                    float radius_z = (float)(particle.getRadiusZ() * com.oddlabs.tt.particle.ParametricEmitter.SQRT_2);
                    x_min = java.lang.Math.min(x_min, (float)(x - radius_x));
                    x_max = java.lang.Math.max(x_max, (float)(x + radius_x));
                    y_min = java.lang.Math.min(y_min, (float)(y - radius_y));
                    y_max = java.lang.Math.max(y_max, (float)(y + radius_y));
                    z_min = java.lang.Math.min(z_min, (float)(z - radius_z));
                    z_max = java.lang.Math.max(z_max, (float)(z + radius_z));
                }
                size += list.size();
            }
            setBounds(x_min, x_max, y_min, y_max, z_min, z_max);
            reregister();
            if (size == 0 && num_particles == 0)
            {
                remove();
            }
        }
        // Java: 143
        public virtual int initParticle(com.oddlabs.tt.particle.ParametricFunction function, float velocity_u, float velocity_v, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, org.joml.Vector3fc particle_radius, org.joml.Vector3fc growth_rate, float energy)
        {
            org.joml.Vector3f offset = randomOffset(area_xy, area_xy, area_z);
            com.oddlabs.tt.particle.ParametricParticle particle = new com.oddlabs.tt.particle.ParametricParticle(getWorld(), function, (float)((float)(random.nextFloat() * ((float)(java.lang.Math.PI))) * 2.0f), (float)((float)(random.nextFloat() * ((float)(java.lang.Math.PI))) * 2.0f), offset.x(), offset.y(), offset.z());
            offset = randomOffset(velocity_random_margin, velocity_random_margin, 0.0f);
            particle.setVelocity((float)(velocity_u + offset.x()), (float)(velocity_v + offset.y()));
            particle.setColor(color.x(), color.y(), color.z(), color.w());
            particle.setDeltaColor(delta_color.x(), delta_color.y(), delta_color.z(), delta_color.w());
            particle.setRadius(particle_radius.x(), particle_radius.y(), particle_radius.z());
            particle.setGrowthRate(growth_rate.x(), growth_rate.y(), growth_rate.z());
            particle.setEnergy(energy);
            particle.setType(random.nextInt(getTypes()));
            particle.update(0);
            add(particle);
            return 1;
        }
        // Java: 166
        public org.joml.Vector3f randomOffset(float a, float b, float c)
        {
            float x = (float)((float)((float)(random.nextFloat() * 2) * a) - a);
            float y = (float)((float)((float)(random.nextFloat() * 2) * b) - b);
            float z = (float)((float)((float)(random.nextFloat() * 2) * c) - c);
            randomized_offset.set(x, y, z);
            return randomized_offset;
        }
        static ParametricEmitter()
        {
            ParametricEmitter.SQRT_2 = ((float)(java.lang.Math.sqrt(2.0f)));
        }
        private void init_ParametricEmitter()
        {
            this.randomized_offset = new org.joml.Vector3f();
            this.particle_counter = 0;
            this.started = true;
        }
        public ParametricEmitter(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.particle.ParametricFunction function, org.joml.Vector3f position, float area_xy, float area_z, float velocity_u, float velocity_v, float velocity_random_margin, int num_particles, float particles_per_second, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, org.joml.Vector3fc particle_radius, org.joml.Vector3fc growth_rate, float energy, int src_blend_func, int dst_blend_func, com.oddlabs.tt.render.TextureKey[] textures, com.oddlabs.tt.animation.AnimationManager manager) : base(world, position, src_blend_func, dst_blend_func, textures, null, textures.Length, manager)
        {
            init_ParametricEmitter();
            this.function = function;
            this.area_xy = area_xy;
            this.area_z = area_z;
            this.velocity_u = velocity_u;
            this.velocity_v = velocity_v;
            this.velocity_random_margin = velocity_random_margin;
            this.num_particles = num_particles;
            this.particles_per_second = particles_per_second;
            this.color = color;
            this.delta_color = delta_color;
            this.particle_radius = particle_radius;
            this.growth_rate = growth_rate;
            this.energy = energy;
            random = world.getRandom();
            register();
        }
    }
}
