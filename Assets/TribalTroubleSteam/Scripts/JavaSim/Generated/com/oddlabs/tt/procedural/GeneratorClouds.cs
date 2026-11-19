// Translated from GeneratorClouds.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 15
    public class GeneratorClouds : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int TEXTURE_SIZE = 512;
        public const int INNER = 0;
        public const int OUTER = 1;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        // Java: 27
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            int seed = com.oddlabs.tt.global.Globals.LANDSCAPE_SEED;
            com.oddlabs.procedural.Channel clouds1 = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorClouds.TEXTURE_SIZE, 3, 0.55f, seed).toChannel();
            com.oddlabs.procedural.Channel clouds2 = new com.oddlabs.tt.procedural.Midpoint(com.oddlabs.tt.procedural.GeneratorClouds.TEXTURE_SIZE, 2, 0.4f, seed).toChannel();
            java.util.function.IntSupplier debugImageCount = new java.util.function.IntSupplier_L(() => { return (new java.util.concurrent.atomic.AtomicInteger()).incrementAndGet(); });
            return ((com.oddlabs.tt.render.Texture[])java.util.stream.Stream_.of(terrain).flatMap(new java.util.function.Function_L((object terrainType_l) =>
            {
                com.oddlabs.tt.procedural.Landscape.TerrainType terrainType = (com.oddlabs.tt.procedural.Landscape.TerrainType)terrainType_l;
                return ((terrainType).ordinal() switch { 0 /*NATIVE*/ => (java.util.stream.Stream)(java.util.stream.Stream_.of(clouds1.dynamicRange(0.5f, 1.0f, 0.0f, 1.0f).gamma(0.75f).brightness(0.5f), clouds2.dynamicRange(0.25f, 1.0f, 0.0f, 1.0f).gamma(0.5f).brightness(0.33f))), 1 /*VIKING*/ => (java.util.stream.Stream)(java.util.stream.Stream_.of(clouds1.dynamicRange(0.5f, 1.0f, 0.0f, 0.75f), clouds2.dynamicRange(0.5f, 1.0f, 0.0f, 0.75f))), _ => throw new System.InvalidOperationException("MatchException") });
            })).peek(new java.util.function.Consumer_L((object img_l) =>
            {
                com.oddlabs.procedural.Channel img = (com.oddlabs.procedural.Channel)img_l;
                if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                {
                    new com.oddlabs.tt.resource.GLIntImage(img.toLayer()).saveAsPNG("generator_clouds_" + (debugImageCount.getAsInt()));
                }
            })).map(new java.util.function.Function_L((object cloud_l) =>
            {
                com.oddlabs.procedural.Channel cloud = (com.oddlabs.procedural.Channel)cloud_l;
                return new com.oddlabs.tt.resource.GLByteImage(cloud, org.lwjgl.opengl.GL11.GL_RED);
            })).map(new java.util.function.Function_L((object image_l) =>
            {
                com.oddlabs.tt.resource.GLByteImage image = (com.oddlabs.tt.resource.GLByteImage)image_l;
                return new com.oddlabs.tt.render.Texture(image, com.oddlabs.tt.global.Globals.COMPRESSED_LUMINANCE_FORMAT, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT);
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.render.Texture[a0]; })));
        }
        // Java: 49
        public override int hashCode()
        {
            return com.oddlabs.tt.procedural.GeneratorClouds.TEXTURE_SIZE;
        }
        // Java: 54
        public override bool equals(object o)
        {
            return base.equals(o) && ((object)(((com.oddlabs.tt.procedural.GeneratorClouds)o)).terrain == (object)terrain);
        }
        public GeneratorClouds(com.oddlabs.tt.procedural.Landscape.TerrainType terrain) : base()
        {
            this.terrain = terrain;
        }
    }
}
