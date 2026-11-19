// Translated from GeneratorIron.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 11
    public class GeneratorIron : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int TEXTURE_SIZE = 128;
        // Java: 14
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            int seed = com.oddlabs.tt.global.Globals.LANDSCAPE_SEED;
            com.oddlabs.procedural.Channel rock_bump = new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, 8, 8, 1, 1.0f, seed).getDistance((float)(-1.0f), 0.0f, 0.0f).multiply(0.49f);
            rock_bump.channelAdd(new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, 8, 8, 1, 1.0f, seed + 1).getDistance((float)(-1.0f), 0.0f, 0.0f).multiply(0.49f));
            com.oddlabs.procedural.Channel noise = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, 7, 0.4f, seed).toChannel();
            com.oddlabs.procedural.Channel noise2 = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, 3, 0.45f, seed).toChannel();
            rock_bump.channelAdd(noise.copy().multiply(0.02f));
            com.oddlabs.procedural.Layer rock = noise.copy().dynamicRange(0.5f, 0.75f).toLayer();
            com.oddlabs.procedural.Channel rustAlpha = noise2.copy().gamma2().invert();
            com.oddlabs.procedural.Layer rust = new com.oddlabs.procedural.Layer(new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE).fill(0.4f), new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE).fill(0.15f), new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE).fill(0.0f), rustAlpha);
            rock.layerBlend(rust);
            rock.bump(rock_bump, (float)(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE / 256.0f), 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f).gamma(0.5f).dynamicRange(0.0f, 0.75f);
            com.oddlabs.procedural.Channel stainAlpha = noise2.copy().gamma(0.75f).rotate(90);
            com.oddlabs.procedural.Layer stain = new com.oddlabs.procedural.Layer(noise2.copy().multiply(0.4f), noise2.copy().multiply(0.15f), noise2.copy().multiply(0.0f), stainAlpha);
            stain.bump(noise2.copy(), 8.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            rock.layerBlend(stain);
            com.oddlabs.procedural.Channel specular = new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE).fill(0.65f).channelSubtract(rustAlpha).channelSubtract(stainAlpha).clip();
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                new com.oddlabs.tt.resource.GLIntImage(rock).saveAsPNG("generator_iron");
            }
            com.oddlabs.procedural.Layer normalMapLayer = rock_bump.toNormalMap(2.5f, specular);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                new com.oddlabs.tt.resource.GLIntImage(normalMapLayer).saveAsPNG("generator_iron_normal");
            }
            return new com.oddlabs.tt.render.Texture[] { new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLIntImage(rock), org.lwjgl.opengl.GL11.GL_RGB, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT), new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLIntImage(normalMapLayer), org.lwjgl.opengl.GL11.GL_RGB, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT) };
        }
        // Java: 50
        public override int hashCode()
        {
            return com.oddlabs.tt.procedural.GeneratorIron.TEXTURE_SIZE;
        }
        public GeneratorIron() : base()
        {
        }
    }
}
