// Boundary class for org.lwjgl.opengl.GL11 (hand-written: the scissor test is kept, the rest does nothing). The GUI's
// Clipped components (text boxes) and scroll groups set a scissor box; GUIRenderer clips its quads to it.
#pragma warning disable
using J2CS.Ext;
namespace org.lwjgl.opengl
{
    public class GL11 : java.lang.Object
    {
        public static void glBindTexture(int p0, int p1) { }
        public const int GL_TEXTURE_2D = 3553;
        public static void glPixelStorei(int p0, int p1) { }
        public const int GL_UNPACK_ROW_LENGTH = 3314;
        public const int GL_UNPACK_SKIP_PIXELS = 3316;
        public const int GL_UNPACK_SKIP_ROWS = 3315;
        public const int GL_UNPACK_ALIGNMENT = 3317;
        public static void glTexSubImage2D(int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, java.nio.FloatBuffer p8) { }
        public const int GL_RED = 6403;
        public const int GL_FLOAT = 5126;
        public const int GL_LINEAR = 9729;
        public const int GL_REPEAT = 10497;
        public const int GL_SRC_ALPHA = 770;
        public const int GL_ONE_MINUS_SRC_ALPHA = 771;
        public const int GL_LINEAR_MIPMAP_LINEAR = 9987;
        public const int GL_RGBA8 = 32856;
        public const int GL_UNSIGNED_BYTE = 5121;
        public const int GL_RGBA = 6408;
        public const int GL_RGB = 6407;
        public const int GL_LINES = 1;
        public const int GL_POINTS = 0;
        public static int glGetError() { return default; }
        public const int GL_NO_ERROR = 0;
        public static int glGetInteger(int p0) { return p0 == GL_MAX_TEXTURE_SIZE ? 16384 : 0; }
        public const int GL_MAX_TEXTURE_SIZE = 3379;
        public const int GL_TEXTURE_BINDING_2D = 32873;
        public const int GL_ONE = 1;
        public static bool glIsEnabled(int p0) { return p0 == GL_SCISSOR_TEST && ScissorTest; }
        public const int GL_SCISSOR_TEST = 3089;
        public static void glGetIntegerv(int p0, java.nio.IntBuffer p1) { if (p0 == GL_SCISSOR_BOX && p1 != null) { p1.put(0, ScissorX); p1.put(1, ScissorY); p1.put(2, ScissorW); p1.put(3, ScissorH); } }
        public const int GL_SCISSOR_BOX = 3088;
        public static void glEnable(int p0) { if (p0 == GL_SCISSOR_TEST) ScissorTest = true; }
        public static void glScissor(int p0, int p1, int p2, int p3) { ScissorX = p0; ScissorY = p1; ScissorW = p2; ScissorH = p3; }
        public static void glDisable(int p0) { if (p0 == GL_SCISSOR_TEST) ScissorTest = false; }
        public static void glTexSubImage2D(int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, java.nio.ByteBuffer p8) { }
        public const int GL_POLYGON_OFFSET_FILL = 32823;
        public static void glPolygonOffset(float p0, float p1) { }
        public static void glDrawBuffer(int p0) { }
        public const int GL_VIEWPORT = 2978;
        public static void glViewport(int p0, int p1, int p2, int p3) { }
        public static void glReadBuffer(int p0) { }
        public const int GL_COLOR_BUFFER_BIT = 16384;
        /// <summary>The scissor test (window pixels, origin bottom left), for GUIRenderer.</summary>
        public static bool ScissorTest;
        public static int ScissorX, ScissorY, ScissorW, ScissorH;
        public GL11() { }
    }
}
