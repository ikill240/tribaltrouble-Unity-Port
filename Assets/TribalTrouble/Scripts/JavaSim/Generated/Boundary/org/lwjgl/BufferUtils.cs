// Hand-written replacement for the generated stub: LWJGL BufferUtils (native-order heap buffers).
#pragma warning disable
namespace org.lwjgl
{
    public class BufferUtils : java.lang.Object
    {
        public static java.nio.ByteBuffer createByteBuffer(int n) => java.nio.ByteBuffer.allocate(n).order(java.nio.ByteOrder.LITTLE_ENDIAN);
        public static java.nio.FloatBuffer createFloatBuffer(int n) => createByteBuffer(n * 4).asFloatBuffer();
        public static java.nio.IntBuffer createIntBuffer(int n) => createByteBuffer(n * 4).asIntBuffer();
        public static java.nio.ShortBuffer createShortBuffer(int n) => java.nio.ShortBuffer.allocate(n);
    }
}
