// Translated from Emitter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 19
    public abstract class Emitter : com.oddlabs.tt.model.Element, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.animation.AnimationManager manager;
        public java.util.List[] particles;
        public com.oddlabs.tt.render.TextureKey[] textures;
        public com.oddlabs.tt.render.SpriteKey[] sprite_renderers;
        public int src_blend_func;
        public int dst_blend_func;
        public int types;
        public com.oddlabs.tt.landscape.World world;
        public org.joml.Vector3f position;
        public float scale_x;
        public float scale_y;
        public float scale_z;
        public const int FX_NONE = 0;
        public const int FX_EMBER_STORM = 1;
        public const int FX_HEALING_RAIN = 2;
        public int fx_tag;
        public float fx_radius;
        // Java: 39
        public void setFxTag(int tag, float radius)
        {
            fx_tag = tag;
            fx_radius = radius;
        }
        // Java: 44
        public int getFxTag()
        {
            return fx_tag;
        }
        // Java: 48
        public float getFxRadius()
        {
            return fx_radius;
        }
        // Java: 68
        public override com.oddlabs.tt.model.Element self()
        {
            return this;
        }
        // Java: 73
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 77
        public com.oddlabs.tt.render.SpriteKey[] getSpriteRenderers()
        {
            return sprite_renderers;
        }
        // Java: 81
        public java.util.List[] getParticles()
        {
            return particles;
        }
        // Java: 85
        public com.oddlabs.tt.render.TextureKey[] getTextures()
        {
            return textures;
        }
        // Java: 89
        public int getSrcBlendFunc()
        {
            return src_blend_func;
        }
        // Java: 93
        public int getDstBlendFunc()
        {
            return dst_blend_func;
        }
        // Java: 97
        public void add(com.oddlabs.tt.particle.Particle particle)
        {
            particles[particle.getType()].add(particle);
        }
        // Java: 101
        public void setPosition(org.joml.Vector3f position)
        {
            this.position = position;
        }
        // Java: 105
        public org.joml.Vector3f getPosition()
        {
            return position;
        }
        // Java: 109
        public float getX()
        {
            return position.x();
        }
        // Java: 113
        public float getY()
        {
            return position.y();
        }
        // Java: 117
        public float getZ()
        {
            return position.z();
        }
        // Java: 121
        public void scale(float scale_x, float scale_y, float scale_z)
        {
            this.scale_x = scale_x;
            this.scale_y = scale_y;
            this.scale_z = scale_z;
        }
        // Java: 127
        public float getScaleX()
        {
            return scale_x;
        }
        // Java: 131
        public float getScaleY()
        {
            return scale_y;
        }
        // Java: 135
        public float getScaleZ()
        {
            return scale_z;
        }
        // Java: 139
        public void forceColorChange(float dr, float dg, float db, float da)
        {
            foreach (java.util.List particle1 in particles)
            {
                foreach (com.oddlabs.tt.particle.Particle particle in java.lang.JIter.of(particle1))
                {
                    particle.setColor((float)(particle.getColorR() + dr), (float)(particle.getColorG() + dg), (float)(particle.getColorB() + db), (float)(particle.getColorA() + da));
                }
            }
        }
        // Java: 148
        public int getTypes()
        {
            return types;
        }
        // Java: 152
        public override void register()
        {
            base.register();
            manager.registerAnimation(this);
        }
        // Java: 158
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitEmitter(this);
        }
        // Java: 163
        public override void remove()
        {
            base.remove();
            manager.removeAnimation(this);
        }
        // Java: 169
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        private void init_Emitter()
        {
            this.scale_x = 1.0f;
            this.scale_y = 1.0f;
            this.scale_z = 1.0f;
        }
        public Emitter(com.oddlabs.tt.landscape.World world, org.joml.Vector3f position, int src_blend_func, int dst_blend_func, com.oddlabs.tt.render.TextureKey[] textures, com.oddlabs.tt.render.SpriteKey[] sprite_renderers, int types, com.oddlabs.tt.animation.AnimationManager manager) : base(world.getElementRoot())
        {
            init_Emitter();
            this.world = world;
            this.position = position;
            this.src_blend_func = src_blend_func;
            this.dst_blend_func = dst_blend_func;
            this.textures = textures;
            this.sprite_renderers = sprite_renderers;
            this.types = types;
            this.manager = manager;
            particles = ((java.util.List[])java.util.stream.Stream_.generate(new java.util.function.Supplier_L(() => { return new java.util.ArrayList(); })).limit(types).toArray(new java.util.function.IntFunction_L((int a0) => { return new java.util.List[a0]; })));
        }
        public abstract void animate(float p0);
    }
}
