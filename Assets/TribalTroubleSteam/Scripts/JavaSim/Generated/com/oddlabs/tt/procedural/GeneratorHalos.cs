// Translated from GeneratorHalos.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 15
    public class GeneratorHalos : com.oddlabs.tt.procedural.TextureGenerator
    {
        public const int SHADOWED = 0;
        public const int SELECTED = 1;
        public int size;
        public float[][] shadow_parms;
        public float[][] ring_parms;
        // Java: 29
        public override com.oddlabs.tt.render.Texture[] generate()
        {
            com.oddlabs.procedural.Channel channel_shadow = new com.oddlabs.tt.procedural.Ring(size, size, shadow_parms, com.oddlabs.tt.procedural.Ring.Interpolation.SMOOTH).toChannel();
            com.oddlabs.procedural.Channel channel_ring = new com.oddlabs.tt.procedural.Ring(size, size, ring_parms, com.oddlabs.tt.procedural.Ring.Interpolation.LINEAR).toChannel();
            com.oddlabs.procedural.Channel channel_black = new com.oddlabs.procedural.Channel(size, size).fill(0.0f);
            com.oddlabs.procedural.Channel channel_white = new com.oddlabs.procedural.Channel(size, size).fill(1.0f);
            com.oddlabs.procedural.Layer[] layers = new com.oddlabs.procedural.Layer[2];
            layers[com.oddlabs.tt.procedural.GeneratorHalos.SHADOWED] = new com.oddlabs.procedural.Layer(channel_black, channel_black, channel_black, channel_shadow);
            layers[com.oddlabs.tt.procedural.GeneratorHalos.SELECTED] = new com.oddlabs.procedural.Layer(channel_white.copy(), channel_white.copy(), channel_white.copy(), channel_ring);
            layers[com.oddlabs.tt.procedural.GeneratorHalos.SELECTED] = layers[com.oddlabs.tt.procedural.GeneratorHalos.SHADOWED].copy().layerBlend(layers[com.oddlabs.tt.procedural.GeneratorHalos.SELECTED]);
            com.oddlabs.tt.render.Texture[] textures = new com.oddlabs.tt.render.Texture[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                {
                    new com.oddlabs.tt.resource.GLIntImage(layers[i]).saveAsPNG("generator_halos_" + (i));
                }
                textures[i] = new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLImage[] { new com.oddlabs.tt.resource.GLIntImage(layers[i]) }, org.lwjgl.opengl.GL11.GL_RGBA8, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE);
            }
            return textures;
        }
        // Java: 48
        public override int hashCode()
        {
            return size * java.util.Arrays.deepHashCode(shadow_parms) * java.util.Arrays.deepHashCode(ring_parms);
        }
        // Java: 53
        public override bool equals(object o)
        {
            return base.equals(o) && (o is com.oddlabs.tt.procedural.GeneratorHalos other) && size == other.size && java.util.Arrays.deepEquals(shadow_parms, other.shadow_parms) && java.util.Arrays.deepEquals(ring_parms, other.ring_parms);
        }
        public GeneratorHalos(int size, float[][] shadow_parms, float[][] ring_parms) : base()
        {
            this.size = size;
            this.shadow_parms = shadow_parms;
            this.ring_parms = ring_parms;
        }
    }
}
