// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    public sealed class Tools {
        private Tools() {
            // no instances
        }

        public static float modulo(float x, float n) {
            return (float)(x - (float)(n * (float) J.Floor((float)(x / n))));
        }

        public static int modulo(int x, int n) {
            return J.I(((float)(x - (float)(n * (float) J.Floor((float)((float) x / n))))));
        }

        public static float interpolateLinear(float v1, float v2, float fraction) {
            return (float)((float)(v1 * ((float)(1f - fraction))) + (float)(v2 * fraction));
        }

        public static float interpolateSmooth(float v1, float v2, float fraction) {
            fraction = fraction < 0.5f ? (float)((float)(2f * fraction) * fraction) : (float)(1f - (float)((float)(2f * ((float)(fraction - 1f))) * ((float)(fraction - 1f))));
            return (float)((float)(v1 * ((float)(1f - fraction))) + (float)(v2 * fraction));
        }

        public static float interpolateSmooth2(float v1, float v2, float fraction) {
            float fraction2 = (float)(fraction * fraction);
            fraction = (float)((float)(3 * fraction2) - (float)((float)(2f * fraction) * fraction2));
            return (float)((float)(v1 * ((float)(1f - fraction))) + (float)(v2 * fraction));
        }

        public static float interpolateCubic(float v0, float v1, float v2, float v3, float fraction) {
            float p = (float)(((float)(v3 - v2)) - ((float)(v0 - v1)));
            float q = (float)(((float)(v0 - v1)) - p);
            float r = (float)(v2 - v0);
            float fraction2 = (float)(fraction * fraction);
            return (float)((float)((float)((float)((float)(p * fraction) * fraction2) + (float)(q * fraction2)) + (float)(r * fraction)) + v1);
        }

        public static float rampLinear(float start, float end, float segment_length, float x) {
            x = modulo(x, segment_length);
            if (x < start) return 0f;
            if (x > end) return 1f;
            return interpolateLinear(0f, 1f, (float)(((float)(x - start)) / ((float)(end - start))));
        }

        public static float step(float start, float end, float segment_length, float x) {
            x = modulo(x, segment_length);
            return x < start || x > end ? 0f : 1f;
        }

        public static float sawtooth(float x) {
            x = (float)(x + 1); // hack!
            float x_frac = (float)(x - J.I(x));
            return x_frac < 0.5 ? (float)(2 * x_frac) : (float)((float)(-2 * x_frac) + 2);
        }

        public static float gaussify(float x) {
            return x >= 0 && x < 0.5 ? (float)(0.5f * (float) J.Sqrt((float)(2 * x))) : x >= 0.5f
                    && x <= 1f ? (float)(1f - (float)(0.5f * (float) J.Sqrt((float)((float)(-2 * x) + 2)))) : 0;
        }

        public static float gaussify(float x, float exponent) {
            return x >= 0 && x < 0.5 ? (float)(0.5f * (float) J.Pow((float)(2 * x), exponent)) : x >= 0.5f
                    && x <= 1f ? (float)(1f - (float)(0.5f * (float) J.Pow((float)((float)(-2 * x) + 2), exponent))) : 0;
        }

        public static float gain(float gain, float x) {
            return x < 0.5f ? (float) (J.Pow((float)(2 * x), J.Log((float)(1 - gain)) / J.Log(0.5d)) / 2f) : (float)(1f - (float) (J.Pow(
                    (float)(2 - (float)(2 * x)), J.Log((float)(1 - gain)) / J.Log(0.5d)) / 2f));
        }
    }
}
