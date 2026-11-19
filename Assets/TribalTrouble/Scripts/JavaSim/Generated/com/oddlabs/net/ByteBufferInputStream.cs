// Translated from ByteBufferInputStream.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 8
    public class ByteBufferInputStream : java.io.InputStream
    {
        public java.nio.ByteBuffer buffer_f;
        // Java: 15
        public java.nio.ByteBuffer buffer()
        {
            return buffer_f;
        }
        // Java: 19
        public override int available()
        {
            return buffer_f.remaining();
        }
        // Java: 24
        public override int read(sbyte[] bytes, int offset, int length)
        {
            if (available() == 0)
            {
                return (-1);
            }
            length = java.lang.Math.min(length, available());
            buffer_f.get(bytes, offset, length);
            return length;
        }
        // Java: 33
        public override int read()
        {
            if (available() > 0)
            {
                int b = buffer_f.get();
                return b & 255;
            }
            else
            {
                return (-1);
            }
        }
        public ByteBufferInputStream(sbyte[] array) : base()
        {
            buffer_f = java.nio.ByteBuffer.wrap(array);
        }
    }
}
