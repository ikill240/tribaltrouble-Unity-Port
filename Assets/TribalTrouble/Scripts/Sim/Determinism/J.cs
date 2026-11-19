using System;
using System.Collections.Generic;

namespace TribalTrouble.Sim.Determinism
{
    /// <summary>
    /// Java semantics for code ported from the Java client. Every numeric construct whose
    /// C# behaviour differs from Java goes through here, so the port stays bit-exact:
    ///   * float/double -> int/long casts (Java: NaN -> 0, out-of-range saturates; C# is undefined)
    ///   * Math.round (Java rounds half up; C# defaults to banker's rounding)
    ///   * Math.min/max on floats (Java: NaN wins, -0.0 &lt; +0.0)
    ///   * Collections.shuffle / Collections.sort (exact algorithms; sort is stable)
    /// Transcendental functions currently delegate to System.Math. The golden landscape tests
    /// show Java (JIT intrinsics) == Java (fdlibm) for the generator, so correctly rounded
    /// results reproduce it. If a conformance test ever disagrees, swap in an fdlibm port here.
    /// </summary>
    public static class J
    {
        // ------------------------------------------------------------ casts

        /// <summary>Java (int) cast of a float.</summary>
        public static int I(float f)
        {
            if (float.IsNaN(f)) return 0;
            if (f >= 2147483647f) return int.MaxValue;
            if (f <= -2147483648f) return int.MinValue;
            return (int)f;
        }

        /// <summary>Java (int) cast of a double.</summary>
        public static int I(double d)
        {
            if (double.IsNaN(d)) return 0;
            if (d >= 2147483647.0) return int.MaxValue;
            if (d <= -2147483648.0) return int.MinValue;
            return (int)d;
        }

        /// <summary>Java (int) cast of a long (wraps).</summary>
        public static int I(long l) { unchecked { return (int)l; } }

        public static int I(int i) => i;

        /// <summary>Java (long) cast of a float.</summary>
        public static long L(float f) => L((double)f);

        /// <summary>Java (long) cast of a double.</summary>
        public static long L(double d)
        {
            if (double.IsNaN(d)) return 0L;
            if (d >= 9223372036854775807.0) return long.MaxValue;
            if (d <= -9223372036854775808.0) return long.MinValue;
            return (long)d;
        }

        public static long L(long l) => l;

        /// <summary>Java (byte) cast (signed, wrapping).</summary>
        public static sbyte B(int i) { unchecked { return (sbyte)i; } }
        public static sbyte B(float f) => B(I(f));
        public static sbyte B(double d) => B(I(d));

        /// <summary>Byte.toUnsignedInt.</summary>
        public static int U8(sbyte b) => b & 0xff;

        /// <summary>Objects.requireNonNull.</summary>
        public static T RequireNonNull<T>(T obj, string message = null) where T : class
        {
            if (obj == null) throw new ArgumentNullException(null, message);
            return obj;
        }

        /// <summary>Java (short) cast.</summary>
        public static short S(int i) { unchecked { return (short)i; } }

        // ------------------------------------------------------------ Math

        public const double PI = Math.PI;
        public const double E = Math.E;

        /// <summary>Math.round(float): floor(x + 0.5) computed exactly (Java 7+ semantics).</summary>
        public static int Round(float a)
        {
            if (float.IsNaN(a)) return 0;
            // Java: (int)floor(a + 0.5f) but without the double-rounding error of a + 0.5f.
            double r = Math.Floor((double)a + 0.5);
            if (r >= 2147483647.0) return int.MaxValue;
            if (r <= -2147483648.0) return int.MinValue;
            return (int)r;
        }

        /// <summary>Math.round(double).</summary>
        public static long Round(double a)
        {
            if (double.IsNaN(a)) return 0L;
            // floor(a + 0.5) without rounding error: handle large values (already integral) directly.
            if (Math.Abs(a) >= 4503599627370496.0) return L(a); // 2^52: no fractional part
            double fl = Math.Floor(a);
            double r = (a - fl) >= 0.5 ? fl + 1.0 : fl;
            return L(r);
        }

        public static float Abs(float a) => Math.Abs(a);
        public static double Abs(double a) => Math.Abs(a);
        public static int Abs(int a) { unchecked { return a < 0 ? -a : a; } } // Java: abs(MIN_VALUE) == MIN_VALUE
        public static long Abs(long a) { unchecked { return a < 0 ? -a : a; } }

        public static int Max(int a, int b) => a >= b ? a : b;
        public static int Min(int a, int b) => a <= b ? a : b;
        public static long Max(long a, long b) => a >= b ? a : b;
        public static long Min(long a, long b) => a <= b ? a : b;

        public static float Max(float a, float b)
        {
            if (a != a) return a; // NaN
            if (a == 0f && b == 0f && IsNegativeZero(a)) return b;
            return a >= b ? a : b;
        }

        public static float Min(float a, float b)
        {
            if (a != a) return a;
            if (a == 0f && b == 0f && IsNegativeZero(b)) return b;
            return a <= b ? a : b;
        }

        public static double Max(double a, double b)
        {
            if (a != a) return a;
            if (a == 0.0 && b == 0.0 && IsNegativeZero(a)) return b;
            return a >= b ? a : b;
        }

        public static double Min(double a, double b)
        {
            if (a != a) return a;
            if (a == 0.0 && b == 0.0 && IsNegativeZero(b)) return b;
            return a <= b ? a : b;
        }

        /// <summary>Math.clamp(long value, int min, int max) -> int (Java 21).</summary>
        public static int Clamp(long value, int min, int max)
        {
            if (min > max) throw new ArgumentException(min + " > " + max);
            return (int)Math.Min(max, Math.Max(value, min));
        }

        /// <summary>Math.clamp(float, float, float) (Java 21).</summary>
        public static float Clamp(float value, float min, float max)
        {
            if (!(min <= max)) throw new ArgumentException(min + " > " + max);
            return Min(max, Max(value, min));
        }

        public static double Clamp(double value, double min, double max)
        {
            if (!(min <= max)) throw new ArgumentException(min + " > " + max);
            return Min(max, Max(value, min));
        }

        static bool IsNegativeZero(float f) => BitConverter.SingleToInt32Bits(f) == unchecked((int)0x80000000);
        static bool IsNegativeZero(double d) => BitConverter.DoubleToInt64Bits(d) == unchecked((long)0x8000000000000000UL);

        public static double Sqrt(double a) => Math.Sqrt(a); // IEEE correctly rounded on both platforms
        public static double Floor(double a) => Math.Floor(a);
        public static double Ceil(double a) => Math.Ceiling(a);
        public static double Pow(double a, double b) => Math.Pow(a, b);
        public static double Sin(double a) => Math.Sin(a);
        public static double Cos(double a) => Math.Cos(a);
        public static double Tan(double a) => Math.Tan(a);
        public static double Atan(double a) => Math.Atan(a);
        public static double Atan2(double y, double x) => Math.Atan2(y, x);
        public static double Acos(double a) => Math.Acos(a);
        public static double Asin(double a) => Math.Asin(a);
        public static double Log(double a) => Math.Log(a);
        public static double Log10(double a) => Math.Log10(a);
        public static double Exp(double a) => Math.Exp(a);
        public static double ToRadians(double deg) => deg / 180.0 * PI;
        public static double ToDegrees(double rad) => rad * 180.0 / PI;
        public static float Signum(float f) => f > 0 ? 1f : f < 0 ? -1f : f;
        public static double Signum(double d) => d > 0 ? 1.0 : d < 0 ? -1.0 : d;

        public static int FloorDiv(int x, int y)
        {
            int q = x / y;
            if ((x % y != 0) && ((x ^ y) < 0)) q--;
            return q;
        }

        public static int FloorMod(int x, int y)
        {
            int m = x % y;
            if (m != 0 && ((m ^ y) < 0)) m += y;
            return m;
        }

        /// <summary>Java &gt;&gt;&gt; for int.</summary>
        public static int Ushr(int x, int n) => (int)((uint)x >> (n & 31));
        public static long Ushr(long x, int n) => (long)((ulong)x >> (n & 63));

        // ------------------------------------------------------------ arrays

        /// <summary>java.util.Objects.checkIndex.</summary>
        public static int CheckIndex(int index, int length)
        {
            if ((uint)index >= (uint)length)
                throw new IndexOutOfRangeException("Index " + index + " out of bounds for length " + length);
            return index;
        }

        public static T[][] New2<T>(int a, int b)
        {
            var r = new T[a][];
            for (int i = 0; i < a; i++) r[i] = new T[b];
            return r;
        }

        public static T[][][] New3<T>(int a, int b, int c)
        {
            var r = new T[a][][];
            for (int i = 0; i < a; i++) r[i] = New2<T>(b, c);
            return r;
        }

        public static void Fill<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++) array[i] = value;
        }

        // ------------------------------------------------------------ collections

        /// <summary>java.util.Collections.shuffle(list, rnd) - identical swap sequence.</summary>
        public static void Shuffle<T>(IList<T> list, JavaRandom rnd)
        {
            for (int i = list.Count; i > 1; i--)
            {
                int j = rnd.NextInt(i);
                T tmp = list[i - 1];
                list[i - 1] = list[j];
                list[j] = tmp;
            }
        }

        /// <summary>Stable sort, like java.util.Collections.sort / List.sort (TimSort is stable).</summary>
        public static void StableSort<T>(List<T> list, Comparison<T> comparison)
        {
            var indexed = new List<KeyValuePair<int, T>>(list.Count);
            for (int i = 0; i < list.Count; i++) indexed.Add(new KeyValuePair<int, T>(i, list[i]));
            indexed.Sort((a, b) =>
            {
                int c = comparison(a.Value, b.Value);
                return c != 0 ? c : a.Key.CompareTo(b.Key);
            });
            for (int i = 0; i < list.Count; i++) list[i] = indexed[i].Value;
        }

        /// <summary>Float.compare semantics (NaN largest, -0 &lt; +0).</summary>
        public static int FloatCompare(float a, float b)
        {
            if (a < b) return -1;
            if (a > b) return 1;
            int ab = BitConverter.SingleToInt32Bits(a == a ? a : float.NaN);
            int bb = BitConverter.SingleToInt32Bits(b == b ? b : float.NaN);
            return ab == bb ? 0 : (ab < bb ? -1 : 1);
        }

        /// <summary>Float.floatToIntBits (canonical NaN).</summary>
        public static int FloatToIntBits(float f) => float.IsNaN(f) ? 0x7fc00000 : BitConverter.SingleToInt32Bits(f);
        public static float IntBitsToFloat(int bits) => BitConverter.Int32BitsToSingle(bits);
    }
}
