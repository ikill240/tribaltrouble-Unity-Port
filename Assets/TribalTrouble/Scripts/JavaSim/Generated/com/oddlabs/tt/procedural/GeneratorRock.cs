// Translated from GeneratorRock.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 11
    public class GeneratorRock : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int TEXTURE_SIZE = 128;
        // Java: 14
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            int seed = com.oddlabs.tt.global.Globals.LANDSCAPE_SEED;
            com.oddlabs.procedural.Channel voronoi4 = new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE, 4, 4, 1, 1.0f, seed).getDistance(0.0f, (float)(-1.0f), 1.0f);
            com.oddlabs.procedural.Channel voronoi8 = new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE, 8, 8, 1, 1.0f, seed).getDistance(0.0f, (float)(-1.0f), 1.0f);
            com.oddlabs.procedural.Channel voronoi16 = new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE, 16, 16, 1, 1.0f, seed).getDistance(0.0f, (float)(-1.0f), 1.0f);
            com.oddlabs.procedural.Channel noise8 = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE, 4, 0.5f, seed).toChannel();
            com.oddlabs.procedural.Channel noise256 = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE, 8, 0.5f, seed).toChannel();
            com.oddlabs.procedural.Channel perturb = noise8.copy().rotate(90);
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE).fill(1.0f);
            com.oddlabs.procedural.Channel rock_bump1 = voronoi4.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.35f);
            com.oddlabs.procedural.Channel rock_bump2 = voronoi8.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.3f);
            com.oddlabs.procedural.Channel rock_bump3 = voronoi16.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.2f);
            com.oddlabs.procedural.Layer rock = new com.oddlabs.procedural.Layer(empty.copy(), empty.copy().brightness(0.8f), empty.copy().brightness(0.6f));
            rock.toHSV();
            rock.r = noise8.copy().dynamicRange(0.05f, 0.1f);
            rock.toRGB();
            com.oddlabs.procedural.Channel rock_bump = rock_bump1.channelAdd(rock_bump2).channelAdd(rock_bump3).channelAdd(noise256.multiply(0.4f));
            rock_bump.perturb(perturb, 0.1f);
            rock.bump(rock_bump, (float)(com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE / 144.0f), 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            rock.gamma(1.25f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                new com.oddlabs.tt.resource.GLIntImage(rock).saveAsPNG("generator_rock");
            }
            com.oddlabs.procedural.Channel mica = noise256.copy().gamma(0.5f).threshold(0.6f, 1.0f).multiply(0.8f);
            com.oddlabs.procedural.Layer normalMapLayer = rock_bump.toNormalMap(3.0f, mica);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                new com.oddlabs.tt.resource.GLIntImage(normalMapLayer).saveAsPNG("generator_rock_normal");
            }
            return new com.oddlabs.tt.render.Texture[] { new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLIntImage(rock), org.lwjgl.opengl.GL11.GL_RGB, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT), new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLIntImage(normalMapLayer), org.lwjgl.opengl.GL11.GL_RGB, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT) };
        }
        // Java: 48
        public override int hashCode()
        {
            return com.oddlabs.tt.procedural.GeneratorRock.TEXTURE_SIZE;
        }
        public GeneratorRock() : base()
        {
        }
    }
}
