// Translated from Tools.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.procedural
{
    // Java: 3
    public class Tools : java.lang.Object
    {
        // Java: 8
        public static float modulo(float x, float n)
        {
            return (float)(x - (float)(n * ((float)(java.lang.Math.floor((float)(x / n))))));
        }
        // Java: 12
        public static int modulo(int x, int n)
        {
            return java.lang.JC.f2i(((float)(x - (float)(n * ((float)(java.lang.Math.floor((float)(((float)(x)) / n))))))));
        }
        // Java: 16
        public static float interpolateLinear(float v1, float v2, float fraction)
        {
            return (float)((float)(v1 * ((float)(1.0f - fraction))) + (float)(v2 * fraction));
        }
        // Java: 20
        public static float interpolateSmooth(float v1, float v2, float fraction)
        {
            fraction = (fraction < 0.5f ? (float)((float)(2.0f * fraction) * fraction) : (float)(1.0f - (float)((float)(2.0f * ((float)(fraction - 1.0f))) * ((float)(fraction - 1.0f)))));
            return (float)((float)(v1 * ((float)(1.0f - fraction))) + (float)(v2 * fraction));
        }
        // Java: 25
        public static float interpolateSmooth2(float v1, float v2, float fraction)
        {
            float fraction2 = (float)(fraction * fraction);
            fraction = (float)((float)(3 * fraction2) - (float)((float)(2.0f * fraction) * fraction2));
            return (float)((float)(v1 * ((float)(1.0f - fraction))) + (float)(v2 * fraction));
        }
        // Java: 31
        public static float interpolateCubic(float v0, float v1, float v2, float v3, float fraction)
        {
            float p = (float)(((float)(v3 - v2)) - ((float)(v0 - v1)));
            float q = (float)(((float)(v0 - v1)) - p);
            float r = (float)(v2 - v0);
            float fraction2 = (float)(fraction * fraction);
            return (float)((float)((float)((float)((float)(p * fraction) * fraction2) + (float)(q * fraction2)) + (float)(r * fraction)) + v1);
        }
        // Java: 39
        public static float rampLinear(float start, float end, float segment_length, float x)
        {
            x = com.oddlabs.procedural.Tools.modulo(x, segment_length);
            if (x < start)
            {
                return 0.0f;
            }
            if (x > end)
            {
                return 1.0f;
            }
            return com.oddlabs.procedural.Tools.interpolateLinear(0.0f, 1.0f, (float)(((float)(x - start)) / ((float)(end - start))));
        }
        // Java: 46
        public static float step(float start, float end, float segment_length, float x)
        {
            x = com.oddlabs.procedural.Tools.modulo(x, segment_length);
            return (x < start || x > end ? 0.0f : 1.0f);
        }
        // Java: 51
        public static float sawtooth(float x)
        {
            x = (float)(x + 1f);
            float x_frac = (float)(x - java.lang.JC.f2i(x));
            return (x_frac < 0.5d ? (float)(2 * x_frac) : (float)((float)((-2) * x_frac) + 2));
        }
        // Java: 57
        public static float gaussify(float x)
        {
            return (x >= 0 && x < 0.5d ? (float)(0.5f * ((float)(java.lang.Math.sqrt((float)(2 * x))))) : (x >= 0.5f && x <= 1.0f ? (float)(1.0f - (float)(0.5f * ((float)(java.lang.Math.sqrt((float)((float)((-2) * x) + 2)))))) : 0));
        }
        // Java: 62
        public static float gaussify(float x, float exponent)
        {
            return (x >= 0 && x < 0.5d ? (float)(0.5f * ((float)(java.lang.StrictMath.pow((float)(2 * x), exponent)))) : (x >= 0.5f && x <= 1.0f ? (float)(1.0f - (float)(0.5f * ((float)(java.lang.StrictMath.pow((float)((float)((-2) * x) + 2), exponent))))) : 0));
        }
        // Java: 67
        public static float gain(float gain_v, float x)
        {
            return (x < 0.5f ? ((float)((java.lang.StrictMath.pow((float)(2 * x), java.lang.StrictMath.log((float)(1 - gain_v)) / java.lang.StrictMath.log(0.5d)) / 2.0f))) : (float)(1.0f - ((float)((java.lang.StrictMath.pow((float)(2 - (float)(2 * x)), java.lang.StrictMath.log((float)(1 - gain_v)) / java.lang.StrictMath.log(0.5d)) / 2.0f)))));
        }
        public Tools() : base()
        {
        }
    }
}
