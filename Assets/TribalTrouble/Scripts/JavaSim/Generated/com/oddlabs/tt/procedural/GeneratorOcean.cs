// Translated from GeneratorOcean.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 14
    public class GeneratorOcean : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int TEXTURE_SIZE = 512;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        // Java: 23
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            int seed = com.oddlabs.tt.global.Globals.LANDSCAPE_SEED + 1;
            com.oddlabs.procedural.Channel perlin2 = new com.oddlabs.tt.procedural.Perlin(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, 4, 4, 0.5f, 1, seed, com.oddlabs.tt.procedural.Perlin.Interpolation.CUBIC, com.oddlabs.tt.procedural.Perlin.Summation.NORMAL).toChannel();
            com.oddlabs.procedural.Channel perlin4 = new com.oddlabs.tt.procedural.Perlin(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, 4, 4, 0.5f, 3, seed, com.oddlabs.tt.procedural.Perlin.Interpolation.CUBIC, com.oddlabs.tt.procedural.Perlin.Summation.NORMAL).toChannel();
            com.oddlabs.procedural.Channel perlin32 = new com.oddlabs.tt.procedural.Perlin(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, 32, 32, 0.5f, 2, seed, com.oddlabs.tt.procedural.Perlin.Interpolation.CUBIC, com.oddlabs.tt.procedural.Perlin.Summation.NORMAL).toChannel();
            com.oddlabs.procedural.Channel perlin64 = new com.oddlabs.tt.procedural.Perlin(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, 64, 64, 0.5f, 2, seed, com.oddlabs.tt.procedural.Perlin.Interpolation.CUBIC, com.oddlabs.tt.procedural.Perlin.Summation.NORMAL).toChannel();
            com.oddlabs.procedural.Channel noise32 = perlin32.copy().abs().dynamicRange().gamma8().gamma2().invert().perturb(perlin4.copy().rotate(90), 0.05f).perturb(perlin2, 0.05f);
            com.oddlabs.procedural.Channel noise64 = perlin64.copy().abs().dynamicRange().gamma2().invert().channelMultiply(perlin32.copy().rotate(90).contrast(2.0f)).perturb(perlin4.copy().rotate(180), 0.05f).perturb(perlin2.copy().rotate(90), 0.05f);
            com.oddlabs.procedural.Channel highlight = noise32.channelBrightest(noise64);
            com.oddlabs.procedural.Layer water1 = new com.oddlabs.procedural.Layer(new com.oddlabs.procedural.Channel(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE), perlin4.copy().dynamicRange(0.5f, 0.8f).rotate(180), perlin4.copy().rotate(270).dynamicRange(0.6f, 0.9f));
            water1.layerAdd(highlight.multiply(0.2f).toLayer());
            water1.addAlpha();
            water1.a.fill(0.5f);
            if (((object)terrain.@base() == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING))
            {
                water1.multiply(0.4f);
                water1.a.addClip(0.1f);
            }
            com.oddlabs.procedural.Channel voronoi12 = new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, 12, 12, 1, 1.0f, seed).getDistance(1.0f, 0.0f, 0.0f);
            com.oddlabs.procedural.Channel voronoi24 = new com.oddlabs.tt.procedural.Voronoi(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, 24, 24, 1, 1.0f, seed).getDistance(1.0f, 0.0f, 0.0f);
            com.oddlabs.procedural.Channel perturb = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE, 4, 0.4f, 42).toChannel();
            voronoi12.channelAverage(voronoi24);
            voronoi12.perturb(perturb, 0.025f);
            com.oddlabs.procedural.Layer water2 = new com.oddlabs.procedural.Layer(voronoi12.copy(), voronoi12.copy(), voronoi12.copy(), voronoi12.copy().gamma(1.5f));
            water2.bump(voronoi12, 3.5f, 0.0f, 0.5f, 0.5f, 0.8f, 1.0f, 0.0f, 0.0f, 0.0f);
            switch ((terrain).ordinal())
            {
                case 0 /*NATIVE*/:
                case 2 /*ELEMENTAL*/:
                {
                    water2.r.dynamicRange(0.0f, 0.4f);
                    water2.g.dynamicRange(0.6f, 1.0f);
                    water2.b.dynamicRange(0.9f, 1.0f);
                    break;
                }
                case 1 /*VIKING*/:
                {
                    water2.r.dynamicRange(0.5f, 1.0f);
                    water2.g.dynamicRange(0.7f, 1.0f);
                    water2.b.dynamicRange(0.8f, 1.0f);
                    water2.a.gamma(0.5f).dynamicRange(0.0f, 0.2f);
                    break;
                }
                default:
                {
                    break;
                }
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                new com.oddlabs.tt.resource.GLIntImage(water1).saveAsPNG("generator_water_1");
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                new com.oddlabs.tt.resource.GLIntImage(water2).saveAsPNG("generator_water_2");
            }
            com.oddlabs.tt.render.Texture[] textures = new com.oddlabs.tt.render.Texture[2];
            textures[0] = new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLImage[] { new com.oddlabs.tt.resource.GLIntImage(water1) }, org.lwjgl.opengl.GL11.GL_RGBA8, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT);
            textures[1] = new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLImage[] { new com.oddlabs.tt.resource.GLIntImage(water2) }, org.lwjgl.opengl.GL11.GL_RGBA8, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT);
            return textures;
        }
        // Java: 87
        public override int hashCode()
        {
            return com.oddlabs.tt.procedural.GeneratorOcean.TEXTURE_SIZE;
        }
        // Java: 92
        public override bool equals(object o)
        {
            return base.equals(o) && ((object)(((com.oddlabs.tt.procedural.GeneratorOcean)o)).terrain == (object)terrain);
        }
        public GeneratorOcean(com.oddlabs.tt.procedural.Landscape.TerrainType terrain) : base()
        {
            this.terrain = terrain;
        }
    }
}
