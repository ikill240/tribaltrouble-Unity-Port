// Translated from ByteBufferOutputStream.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 8
    public class ByteBufferOutputStream : java.io.OutputStream
    {
        public const int BUFFER_SIZE = 16382;
        public java.nio.ByteBuffer buffer_f;
        // Java: 21
        public void reset()
        {
            buffer_f.clear();
        }
        // Java: 25
        public sbyte[] toByteArray()
        {
            sbyte[] result = new sbyte[buffer_f.position()];
            buffer_f.flip();
            buffer_f.get(result);
            return result;
        }
        // Java: 32
        public java.nio.ByteBuffer buffer()
        {
            return buffer_f;
        }
        // Java: 42
        public void ensureCapacity(int size)
        {
            if (buffer_f.remaining() < size)
            {
                int new_capacity = buffer_f.capacity() * 2 + size;
                new_capacity = (new_capacity + 7) & ~7;
                java.nio.ByteBuffer new_buffer = default;
                if (buffer_f.isDirect())
                {
                    new_buffer = java.nio.ByteBuffer.allocateDirect(new_capacity);
                }
                else
                {
                    new_buffer = java.nio.ByteBuffer.allocate(new_capacity);
                }
                buffer_f.flip();
                new_buffer.put(buffer_f);
                buffer_f = new_buffer;
            }
        }
        // Java: 57
        public override void write(sbyte[] bytes, int offset, int length)
        {
            ensureCapacity(length);
            buffer_f.put(bytes, offset, length);
        }
        // Java: 63
        public override void write(int b_int)
        {
            ensureCapacity(1);
            sbyte b = unchecked((sbyte)((b_int & 255)));
            buffer_f.put(b);
        }
        public ByteBufferOutputStream(bool direct) : base()
        {
            if (direct)
            {
                buffer_f = java.nio.ByteBuffer.allocateDirect(com.oddlabs.util.ByteBufferOutputStream.BUFFER_SIZE);
            }
            else
            {
                buffer_f = java.nio.ByteBuffer.allocate(com.oddlabs.util.ByteBufferOutputStream.BUFFER_SIZE);
            }
        }
    }
}
