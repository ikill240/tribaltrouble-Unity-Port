// Translated from Lightning.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 18
    public class Lightning : com.oddlabs.tt.model.Element, com.oddlabs.tt.animation.Animated
    {
        public static float SQRT_2;
        public com.oddlabs.tt.animation.AnimationManager manager;
        public java.util.Deque particles;
        public org.joml.Vector3fc src;
        public org.joml.Vector3fc dst;
        public float width;
        public int num_particles;
        public org.joml.Vector4fc color;
        public org.joml.Vector4fc delta_color;
        public com.oddlabs.tt.render.TextureKey texture;
        public com.oddlabs.tt.landscape.World world;
        public float energy;
        // Java: 53
        public override com.oddlabs.tt.model.Element self()
        {
            return this;
        }
        // Java: 58
        public java.util.Deque getParticles()
        {
            return particles;
        }
        // Java: 62
        public com.oddlabs.tt.render.TextureKey getTexture()
        {
            return texture;
        }
        // Java: 66
        public void initParticles()
        {
            java.util.Random random = world.getRandom();
            random.nextFloat();
            float x = src.x();
            float y = src.y();
            float z = src.z();
            float height = (float)(dst.z() - src.z());
            float random_limit = (float)(height / 6.0f);
            float dz = (float)((height) / num_particles);
            for (int i = 0; i < num_particles; i++)
            {
                float base_dx = (float)(((float)(dst.x() - x)) / (num_particles - i));
                float base_dy = (float)(((float)(dst.y() - y)) / (num_particles - i));
                float dx = (float)(base_dx + (float)(((float)(random.nextFloat() - 0.5f)) * random_limit));
                float dy = (float)(base_dy + (float)(((float)(random.nextFloat() - 0.5f)) * random_limit));
                com.oddlabs.tt.particle.StretchParticle particle = new com.oddlabs.tt.particle.StretchParticle(world);
                particle.setSrc(x, y, z);
                if (i == num_particles - 1)
                {
                    x = dst.x();
                    y = dst.y();
                    z = dst.z();
                    particle.setDstWidth((float)(width / 2));
                }
                else
                {
                    x = (float)(x + dx);
                    y = (float)(y + dy);
                    z = (float)(z + dz);
                    particle.setDstWidth(width);
                }
                particle.setDst(x, y, z);
                initParticle(particle);
                particles.add(particle);
            }
        }
        // Java: 101
        public void initParticle(com.oddlabs.tt.particle.StretchParticle particle)
        {
            particle.setSrcWidth(width);
            particle.setColor(color.x(), color.y(), color.z(), color.w());
            particle.setDeltaColor(delta_color.x(), delta_color.y(), delta_color.z(), delta_color.w());
            particle.setRadius(0.0f, 0.0f, 0.0f);
            particle.setGrowthRate(0.0f, 0.0f, 0.0f);
            particle.setEnergy(energy);
        }
        // Java: 110
        public void animate(float t)
        {
            float x_min = java.lang.Float.POSITIVE_INFINITY;
            float x_max = java.lang.Float.NEGATIVE_INFINITY;
            float y_min = java.lang.Float.POSITIVE_INFINITY;
            float y_max = java.lang.Float.NEGATIVE_INFINITY;
            float z_min = java.lang.Float.POSITIVE_INFINITY;
            float z_max = java.lang.Float.NEGATIVE_INFINITY;
            java.util.Iterator each = particles.iterator();
            while (each.hasNext())
            {
                com.oddlabs.tt.particle.StretchParticle particle = ((com.oddlabs.tt.particle.StretchParticle)each.next());
                if (particle.getEnergy() > 0.0f)
                {
                    particle.update(t);
                    float x = particle.getSrcX();
                    float y = particle.getSrcY();
                    float z = particle.getSrcZ();
                    float radius_x = (float)(particle.getRadiusX() * com.oddlabs.tt.particle.Lightning.SQRT_2);
                    float radius_y = (float)(particle.getRadiusY() * com.oddlabs.tt.particle.Lightning.SQRT_2);
                    float radius_z = (float)(particle.getRadiusZ() * com.oddlabs.tt.particle.Lightning.SQRT_2);
                    x_min = java.lang.Math.min(x_min, (float)(x - radius_x));
                    x_max = java.lang.Math.max(x_max, (float)(x + radius_x));
                    y_min = java.lang.Math.min(y_min, (float)(y - radius_y));
                    y_max = java.lang.Math.max(y_max, (float)(y + radius_y));
                    z_min = java.lang.Math.min(z_min, (float)(z - radius_z));
                    z_max = java.lang.Math.max(z_max, (float)(z + radius_z));
                }
                else
                {
                    each.remove();
                }
            }
            setBounds(x_min, x_max, y_min, y_max, z_min, z_max);
            reregister();
            if (particles.isEmpty())
            {
                remove();
            }
        }
        // Java: 147
        public override void register()
        {
            base.register();
            manager.registerAnimation(this);
        }
        // Java: 153
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitLightning(this);
        }
        // Java: 158
        public override void remove()
        {
            base.remove();
            manager.removeAnimation(this);
        }
        static Lightning()
        {
            Lightning.SQRT_2 = ((float)(java.lang.Math.sqrt(2.0f)));
        }
        private void init_Lightning()
        {
            this.particles = new java.util.ArrayDeque();
        }
        public Lightning(com.oddlabs.tt.landscape.World world, org.joml.Vector3fc src, org.joml.Vector3fc dst, float width, int num_particles, org.joml.Vector4fc color, org.joml.Vector4fc delta_color, com.oddlabs.tt.render.TextureKey texture, float energy, com.oddlabs.tt.animation.AnimationManager manager) : base(world.getElementRoot())
        {
            init_Lightning();
            this.world = world;
            this.src = src;
            this.dst = dst;
            this.width = width;
            this.num_particles = num_particles;
            this.color = color;
            this.delta_color = delta_color;
            this.texture = texture;
            this.energy = energy;
            this.manager = manager;
            initParticles();
            register();
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
