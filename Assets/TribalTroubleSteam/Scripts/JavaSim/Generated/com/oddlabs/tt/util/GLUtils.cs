// Translated from GLUtils.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 16
    public class GLUtils : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const string SCREENSHOT_DEFAULT = "screenshot";
        // Java: 20
        public static string takeScreenshot(string filename)
        {
            return TribalTroubleSteam.PortHooks.Screenshot(filename);
        }
        // Java: 49
        public static void saveTexture(int mipmap_level, string filename)
        {
            throw new System.NotSupportedException("J2CS stub: GLUtils.saveTexture");
        }
        // Java: 71
        public static java.util.List checkGLError(string message)
        {
            java.util.List errors = new java.util.ArrayList();
            int error = default;
            while ((error = org.lwjgl.opengl.GL11.glGetError()) != org.lwjgl.opengl.GL11.GL_NO_ERROR)
            {
                com.oddlabs.tt.util.GLUtils.logger.log(java.util.logging.Level.WARNING, "OpenGL Error (" + java.lang.JStr.valueOf(message) + "): " + java.lang.JStr.valueOf(com.oddlabs.tt.util.GLUtils.errorToString(error)), new System.Exception("stacktrace"));
                errors.add(error);
            }
            return errors;
        }
        // Java: 88
        public static void checkAndThrow(string message)
        {
            java.util.List errors = com.oddlabs.tt.util.GLUtils.checkGLError(message);
            if (!errors.isEmpty())
            {
                throw new com.oddlabs.tt.util.OpenGLException("OpenGL failure (" + java.lang.JStr.valueOf(message) + "): " + java.lang.JStr.valueOf(com.oddlabs.tt.util.GLUtils.errorToString(((int)((int?)errors.getFirst())))));
            }
        }
        // Java: 101
        public static string errorToString(int error)
        {
            return ((error) switch { 0 => (string)("GL_NO_ERROR"), 1280 => (string)("GL_INVALID_ENUM"), 1281 => (string)("GL_INVALID_VALUE"), 1282 => (string)("GL_INVALID_OPERATION"), 1283 => (string)("GL_STACK_OVERFLOW"), 1284 => (string)("GL_STACK_UNDERFLOW"), 1285 => (string)("GL_OUT_OF_MEMORY"), _ => (string)("Unknown OpenGL Error: " + (error) + " (0x" + java.lang.JStr.valueOf(java.lang.Integer.toHexString(error)) + ")") });
        }
        static GLUtils()
        {
            GLUtils.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.util.GLUtils).getSimpleName());
        }
        public GLUtils() : base()
        {
        }
    }
}
