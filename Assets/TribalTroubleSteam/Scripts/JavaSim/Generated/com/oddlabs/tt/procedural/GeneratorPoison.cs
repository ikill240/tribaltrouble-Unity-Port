// Translated from GeneratorPoison.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 12
    public class GeneratorPoison : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int TEXTURE_SIZE = 128;
        // Java: 15
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            int seed = com.oddlabs.tt.global.Globals.LANDSCAPE_SEED;
            com.oddlabs.procedural.Channel blob = new com.oddlabs.tt.procedural.Hill(com.oddlabs.tt.procedural.GeneratorPoison.TEXTURE_SIZE, com.oddlabs.tt.procedural.Hill.CIRCLE).toChannel();
            com.oddlabs.procedural.Channel noise = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorPoison.TEXTURE_SIZE, 2, 0.45f, seed).toChannel();
            com.oddlabs.procedural.Channel poison_alpha = noise.copy().channelMultiply(blob.copy().dynamicRange(0.25f, 1.0f)).channelSubtract(blob.copy().invert().brightness(0.25f));
            com.oddlabs.procedural.Layer poison = new com.oddlabs.procedural.Layer(noise.copy().rotate(90).dynamicRange(0.5f, 1.0f), new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorPoison.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorPoison.TEXTURE_SIZE).fill(1.0f), new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorPoison.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorPoison.TEXTURE_SIZE), poison_alpha.brightness(0.75f));
            com.oddlabs.tt.resource.GLIntImage poison_img = new com.oddlabs.tt.resource.GLIntImage(poison);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                poison_img.saveAsPNG("generator_poison");
            }
            return new com.oddlabs.tt.render.Texture[] { new com.oddlabs.tt.render.Texture(poison_img, com.oddlabs.tt.global.Globals.COMPRESSED_RGBA_FORMAT, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE) };
        }
        // Java: 41
        public override int hashCode()
        {
            return com.oddlabs.tt.procedural.GeneratorPoison.TEXTURE_SIZE + 398;
        }
        public GeneratorPoison() : base()
        {
        }
    }
}
