// Translated from GLByteImage.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 11
    public class GLByteImage : com.oddlabs.tt.resource.GLImage
    {
        // Java: 13
        public override int getPixelSize()
        {
            return java.lang.Byte.BYTES;
        }
        // Java: 40
        public override com.oddlabs.tt.resource.GLImage createImage(int width, int height, int format)
        {
            return new com.oddlabs.tt.resource.GLByteImage(width, height, format);
        }
        // Java: 45
        public override com.oddlabs.tt.resource.GLImage createFromLayer(com.oddlabs.procedural.Layer layer, int format)
        {
            com.oddlabs.procedural.Channel sourceChannel = default;
            if (format == org.lwjgl.opengl.GL11.GL_RED && layer.a != null)
            {
                sourceChannel = layer.a;
            }
            else
            {
                sourceChannel = layer.r;
            }
            return new com.oddlabs.tt.resource.GLByteImage(sourceChannel, format);
        }
        // Java: 58
        public override int getPixel(int x, int y)
        {
            return getPixels().get(y * getWidth() + x) & 255;
        }
        // Java: 63
        public override void putPixel(int x, int y, int pixel)
        {
            getPixels().put(y * getWidth() + x, unchecked((sbyte)(pixel)));
        }
        public GLByteImage(int width, int height, java.nio.ByteBuffer pixel_data, int format) : base(width, height, pixel_data, format)
        {
        }
        public GLByteImage(int width, int height, int format) : this(width, height, org.lwjgl.BufferUtils.createByteBuffer((width * height + 3) & ~3), format)
        {
        }
        public GLByteImage(com.oddlabs.procedural.Channel channel) : this(channel, org.lwjgl.opengl.GL11.GL_RED)
        {
        }
        public GLByteImage(com.oddlabs.procedural.Channel channel, int format) : this(channel.getWidth(), channel.getHeight(), format)
        {
            for (int y = 0; y < getHeight(); y++)
            {
                for (int x = 0; x < getWidth(); x++)
                {
                    int pixel = java.lang.Math.round((float)(channel.getPixel(x, y) * 255));
                    putPixel(x, y, java.lang.Math.clamp(pixel, 0, 255));
                }
            }
        }
    }
}
