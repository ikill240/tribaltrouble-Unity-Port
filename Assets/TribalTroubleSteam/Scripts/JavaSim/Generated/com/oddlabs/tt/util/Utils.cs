// Translated from Utils.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 22
    public class Utils : java.lang.Object
    {
        // Java: 24
        public interface I18N : java.util.function.BiFunction
        {
            // Java: 26
            object apply(object key_p, object args_p);
            // Java: 30
            string i18n(string key, params object[] args);
        }
        public sealed class I18N_L : java.lang.Object, I18N
        {
            readonly System.Func<string, object[], string> f;
            public I18N_L(System.Func<string, object[], string> f) { this.f = f; }
            public string i18n(string p0, object[] p1) { return f(p0, p1); }
            public object apply(object key_p, object args_p)
            {
                string key = (string)key_p;
                object[] args = (object[])args_p;
                return i18n(key, args);
            }
        }
        // Java: 34
        public static string getBundleString(java.util.ResourceBundle bundle, string key, params object[] object_array)
        {
            return java.text.MessageFormat.format(bundle.getString(key), object_array);
        }
        // Java: 39
        public static java.nio.file.Path getInstallDir()
        {
            return java.nio.file.Path_.of(java.lang.JSystem.getProperty("user.dir"));
        }
        // Java: 43
        public static java.nio.FloatBuffer toBuffer(float[] floats)
        {
            return org.lwjgl.BufferUtils.createFloatBuffer(floats.Length).put(floats).rewind();
        }
        // Java: 47
        public static java.nio.ShortBuffer toBuffer(short[] shorts)
        {
            return org.lwjgl.BufferUtils.createShortBuffer(shorts.Length).put(shorts).rewind();
        }
        // Java: 51
        public static java.nio.ByteBuffer ioResourceToByteBuffer(java.net.URL url)
        {
            {
                java.io.InputStream @is = url.openStream();
                try
                {
                    sbyte[] bytes = @is.readAllBytes();
                    java.nio.ByteBuffer buffer = org.lwjgl.BufferUtils.createByteBuffer(bytes.Length);
                    buffer.put(bytes);
                    buffer.flip();
                    return buffer;
                }
                finally
                {
                    if (@is != null) @is.close();
                }
            }
        }
        // Java: 61
        public static void saveAsBMP(string filename, java.nio.ByteBuffer pixel_data, int width, int height)
        {
            long before = java.lang.JSystem.nanoTime();
            int pad = 4 - (width * 3) % 4;
            if (pad == 4)
            {
                pad = 0;
            }
            int size = (width * 3 + pad) * height + 54;
            java.nio.ByteBuffer buffer = java.nio.ByteBuffer.allocate(size);
            buffer.put(unchecked((sbyte)(66)));
            buffer.put(unchecked((sbyte)(77)));
            buffer.put(unchecked((sbyte)((size & 255))));
            buffer.put(unchecked((sbyte)(((size & 65280) >> 8))));
            buffer.put(unchecked((sbyte)(((size & 16711680) >> 16))));
            buffer.put(unchecked((sbyte)(((size & (-16777216)) >> 24))));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(54)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(40)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)((width & 255))));
            buffer.put(unchecked((sbyte)(((width & 65280) >> 8))));
            buffer.put(unchecked((sbyte)(((width & 16711680) >> 16))));
            buffer.put(unchecked((sbyte)(((width & (-16777216)) >> 24))));
            buffer.put(unchecked((sbyte)((height & 255))));
            buffer.put(unchecked((sbyte)(((height & 65280) >> 8))));
            buffer.put(unchecked((sbyte)(((height & 16711680) >> 16))));
            buffer.put(unchecked((sbyte)(((height & (-16777216)) >> 24))));
            buffer.put(unchecked((sbyte)(1)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(24)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(((size - 54) & 255))));
            buffer.put(unchecked((sbyte)((((size - 54) & 65280) >> 8))));
            buffer.put(unchecked((sbyte)((((size - 54) & 16711680) >> 16))));
            buffer.put(unchecked((sbyte)((((size - 54) & (-16777216)) >> 24))));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            buffer.put(unchecked((sbyte)(0)));
            pixel_data.rewind();
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = (y * width + x) * 4;
                    sbyte r = pixel_data.get(offset);
                    sbyte g = pixel_data.get(offset + 1);
                    sbyte b = pixel_data.get(offset + 2);
                    buffer.put(b);
                    buffer.put(g);
                    buffer.put(r);
                }
                for (int i = 0; i < pad; i++)
                {
                    buffer.put(unchecked((sbyte)(0)));
                }
            }
            buffer.rewind();
            java.nio.file.Path image_file = java.nio.file.Path_.of(filename);
            {
                java.io.OutputStream fout = java.nio.file.Files.newOutputStream(image_file);
                try
                {
                    fout.write(buffer.array());
                }
                catch (System.IO.IOException e)
                {
                    e.printStackTrace();
                }
                finally
                {
                    if (fout != null) fout.close();
                }
            }
            long after = java.lang.JSystem.nanoTime();
            java.lang.IO.println("File " + java.lang.JStr.valueOf(filename) + " saved in " + (java.util.concurrent.TimeUnit.NANOSECONDS.toMillis(after - before)) + " milliseconds");
        }
        // Java: 153
        public static void saveAsTGA(string filename, java.nio.ByteBuffer pixel_data, int width, int height)
        {
            throw new System.NotSupportedException("J2CS stub: Utils.saveAsTGA");
        }
        // Java: 191
        public static int numTextureSplits(int size)
        {
            return (size >> com.oddlabs.tt.global.Globals.MAX_TEXTURE_POWER) + com.oddlabs.tt.global.Globals.TEXTURE_SPLITS[size & (com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE - 1)];
        }
        // Java: 195
        public static int toTextureSize(int size)
        {
            return (size & ~(com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE - 1)) + com.oddlabs.tt.global.Globals.TEXTURE_SIZES[size & (com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE - 1)];
        }
        // Java: 199
        public static int roundToTextureSize(int size)
        {
            int tex_size = 1;
            while (tex_size < size)
            {
                tex_size <<= 1;
            }
            return tex_size;
        }
        // Java: 207
        public static int bestTextureSize(int size)
        {
            if (size >= com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE)
            {
                return com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE;
            }
            return com.oddlabs.tt.global.Globals.BEST_SIZES[size];
        }
        // Java: 213
        public static float invsqrt(float x)
        {
            float xhalf = (float)(0.5f * x);
            int i = java.lang.Float.floatToRawIntBits(x);
            i = 1597463174 - (i >> 1);
            x = java.lang.Float.intBitsToFloat(i);
            x = (float)(x * ((float)(1.5f - (float)((float)(xhalf * x) * x))));
            return x;
        }
        public Utils() : base()
        {
        }
    }
}
