// Translated from GLIntImage.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 16
    public class GLIntImage : com.oddlabs.tt.resource.GLImage
    {
        public java.nio.IntBuffer pixels;
        // Java: 19
        public override int getPixelSize()
        {
            return java.lang.Integer.BYTES;
        }
        // Java: 24
        public java.nio.IntBuffer getIntPixels()
        {
            return pixels;
        }
        // Java: 57
        public override com.oddlabs.tt.resource.GLImage createImage(int width, int height, int format)
        {
            return new com.oddlabs.tt.resource.GLIntImage(width, height, format);
        }
        // Java: 62
        public override com.oddlabs.tt.resource.GLImage createFromLayer(com.oddlabs.procedural.Layer layer, int format)
        {
            return new com.oddlabs.tt.resource.GLIntImage(layer);
        }
        // Java: 67
        public override int getPixel(int x, int y)
        {
            return pixels.get(y * getWidth() + x);
        }
        // Java: 72
        public override void putPixel(int x, int y, int pixel)
        {
            pixels.put(y * getWidth() + x, pixel);
        }
        // Java: 77
        public static com.oddlabs.tt.resource.GLIntImage loadImage(java.net.URL url)
        {
            throw new System.NotSupportedException("J2CS stub: GLIntImage.loadImage");
        }
        public GLIntImage(int width, int height, java.nio.ByteBuffer pixel_data, int format) : base(width, height, pixel_data, format)
        {
            pixels = pixel_data.asIntBuffer();
        }
        public GLIntImage(int width, int height, int format) : this(width, height, ((java.nio.ByteBuffer)java.util.Objects.requireNonNull(org.lwjgl.BufferUtils.createByteBuffer(width * height * java.lang.Integer.BYTES))), format)
        {
        }
        public GLIntImage(com.oddlabs.procedural.Layer layer) : this(layer.getWidth(), layer.getHeight(), org.lwjgl.opengl.GL11.GL_RGBA)
        {
            for (int y = 0; y < getHeight(); y++)
            {
                for (int x = 0; x < getWidth(); x++)
                {
                    int ri = (java.lang.JC.f2i(((float)((float)(layer.r.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    int gi = (java.lang.JC.f2i(((float)((float)(layer.g.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    int bi = (java.lang.JC.f2i(((float)((float)(layer.b.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    int ai = default;
                    if (layer.a != null)
                    {
                        ai = (java.lang.JC.f2i(((float)((float)(layer.a.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    }
                    else
                    {
                        ai = 255;
                    }
                    int pixel = (ai << 24) | (bi << 16) | (gi << 8) | ri;
                    putPixel(x, y, pixel);
                }
            }
        }
    }
}
