// Translated from Color.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 10
    public class Color : java.lang.Object
    {
        public const int TRANSPARENT_INT = 0;
        public const int BLACK_INT = (-16777216);
        public const int WHITE_INT = (-1);
        public const int RED_INT = (-65536);
        public const int GREEN_INT = (-16711936);
        public const int BLUE_INT = (-16776961);
        public const int YELLOW_INT = (-256);
        public const int MAGENTA_INT = (-65281);
        public static org.joml.Vector4fc BLACK;
        public static org.joml.Vector4fc WHITE;
        public static org.joml.Vector4fc TRANSPARENT;
        public const float NORMALIZE_8_BIT = 255.0f;
        // Java: 38
        public static int argbi(org.joml.Vector4fc color)
        {
            return com.oddlabs.util.Color.argbi(color.w(), color.x(), color.y(), color.z());
        }
        // Java: 47
        public static int argbi(float a, float r, float g, float b)
        {
            return com.oddlabs.util.Color.argbi(unchecked((sbyte)java.lang.JC.d2i(((float)(a * com.oddlabs.util.Color.NORMALIZE_8_BIT)))), unchecked((sbyte)java.lang.JC.d2i(((float)(r * com.oddlabs.util.Color.NORMALIZE_8_BIT)))), unchecked((sbyte)java.lang.JC.d2i(((float)(g * com.oddlabs.util.Color.NORMALIZE_8_BIT)))), unchecked((sbyte)java.lang.JC.d2i(((float)(b * com.oddlabs.util.Color.NORMALIZE_8_BIT)))));
        }
        // Java: 59
        public static int argbi(sbyte a, sbyte r, sbyte g, sbyte b)
        {
            return (java.lang.Byte.toUnsignedInt(a) << 24) | (java.lang.Byte.toUnsignedInt(r) << 16) | (java.lang.Byte.toUnsignedInt(g) << 8) | java.lang.Byte.toUnsignedInt(b);
        }
        // Java: 70
        public static int abgri(org.joml.Vector4fc color)
        {
            return com.oddlabs.util.Color.abgri(color.w(), color.z(), color.y(), color.x());
        }
        // Java: 74
        public static int abgri(float a, float b, float g, float r)
        {
            return (java.lang.JC.f2i(((float)(a * com.oddlabs.util.Color.NORMALIZE_8_BIT))) << 24) | (java.lang.JC.f2i(((float)(b * com.oddlabs.util.Color.NORMALIZE_8_BIT))) << 16) | (java.lang.JC.f2i(((float)(g * com.oddlabs.util.Color.NORMALIZE_8_BIT))) << 8) | (java.lang.JC.f2i(((float)(r * com.oddlabs.util.Color.NORMALIZE_8_BIT))));
        }
        // Java: 85
        public static org.joml.Vector4f argb4v(int color)
        {
            return new org.joml.Vector4f((float)(((color >> 16) & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT), (float)(((color >> 8) & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT), (float)((color & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT), (float)(((color >> 24) & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT));
        }
        // Java: 99
        public static org.joml.Vector4f rgba4v(int color)
        {
            return new org.joml.Vector4f((float)(((color >> 24) & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT), (float)(((color >> 16) & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT), (float)(((color >> 8) & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT), (float)((color & 255) / com.oddlabs.util.Color.NORMALIZE_8_BIT));
        }
        static Color()
        {
            Color.BLACK = com.oddlabs.util.Color.argb4v(com.oddlabs.util.Color.BLACK_INT);
            Color.WHITE = com.oddlabs.util.Color.argb4v(com.oddlabs.util.Color.WHITE_INT);
            Color.TRANSPARENT = com.oddlabs.util.Color.argb4v(com.oddlabs.util.Color.TRANSPARENT_INT);
        }
        public Color() : base()
        {
        }
    }
}
