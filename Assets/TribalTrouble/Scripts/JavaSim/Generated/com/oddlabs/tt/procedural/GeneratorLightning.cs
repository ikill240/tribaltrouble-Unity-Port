// Translated from GeneratorLightning.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 11
    public class GeneratorLightning : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int TEXTURE_SIZE = 128;
        // Java: 14
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            com.oddlabs.procedural.Channel gradient = new com.oddlabs.tt.procedural.Gradient(com.oddlabs.tt.procedural.GeneratorLightning.TEXTURE_SIZE, 1, new float[][] { new float[] { 0.0f, 0.0f }, new float[] { 0.47f, 0.25f }, new float[] { 0.5f, 1.0f }, new float[] { 0.53f, 0.25f }, new float[] { 1.0f, 0.0f } }, com.oddlabs.tt.procedural.Gradient.HORIZONTAL, com.oddlabs.tt.procedural.Gradient.SMOOTH).toChannel();
            com.oddlabs.procedural.Layer layer = new com.oddlabs.procedural.Layer(new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorLightning.TEXTURE_SIZE, 1).fill(1.0f), new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorLightning.TEXTURE_SIZE, 1).fill(1.0f), gradient.copy(), gradient.copy());
            com.oddlabs.tt.resource.GLIntImage img = new com.oddlabs.tt.resource.GLIntImage(layer);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                img.saveAsPNG("generator_lightning");
            }
            return new com.oddlabs.tt.render.Texture[] { new com.oddlabs.tt.render.Texture(img, com.oddlabs.tt.global.Globals.COMPRESSED_RGBA_FORMAT, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT) };
        }
        // Java: 28
        public override int hashCode()
        {
            return com.oddlabs.tt.procedural.GeneratorLightning.TEXTURE_SIZE + 3346;
        }
        public GeneratorLightning() : base()
        {
        }
    }
}
