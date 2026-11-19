// Translated from GeneratorSmoke.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 11
    public class GeneratorSmoke : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int TEXTURE_SIZE = 128;
        // Java: 14
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            com.oddlabs.procedural.Channel voronoi = new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorSmoke.TEXTURE_SIZE, 4, 4, 1, 1.0f, 42).getDistance((float)(-1.0f), 0.0f, 0.0f);
            com.oddlabs.procedural.Channel smoke_alpha = new com.oddlabs.tt.procedural.Ring(com.oddlabs.tt.procedural.GeneratorSmoke.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorSmoke.TEXTURE_SIZE, new float[][] { new float[] { 0.0f, 1.0f }, new float[] { 0.5f, 0.0f } }, com.oddlabs.tt.procedural.Ring.Interpolation.SMOOTH).toChannel().gamma(1.5f);
            com.oddlabs.procedural.Channel smoke_color = new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorSmoke.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorSmoke.TEXTURE_SIZE).fill(0.5f);
            com.oddlabs.procedural.Channel smoke_bump = voronoi.gamma(0.25f).smooth(3).smooth(1).dynamicRange(0.925f, 1.0f).channelMultiply(smoke_alpha);
            smoke_color.bump(smoke_bump, 0.0f, (float)(-4.0f), 0.0f, 1.0f, 0.0f);
            com.oddlabs.procedural.Layer smoke = new com.oddlabs.procedural.Layer(smoke_alpha.copy(), smoke_alpha.copy(), smoke_alpha.copy(), smoke_alpha);
            com.oddlabs.tt.resource.GLIntImage smoke_img = new com.oddlabs.tt.resource.GLIntImage(smoke);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                smoke_img.saveAsPNG("generator_smoke");
            }
            return new com.oddlabs.tt.render.Texture[] { new com.oddlabs.tt.render.Texture(smoke_img, org.lwjgl.opengl.GL11.GL_RGBA8, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE) };
        }
        // Java: 31
        public override int hashCode()
        {
            return com.oddlabs.tt.procedural.GeneratorSmoke.TEXTURE_SIZE + 3;
        }
        public GeneratorSmoke() : base()
        {
        }
    }
}
