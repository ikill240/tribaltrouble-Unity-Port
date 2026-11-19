// Minimal Java runtime for J2CS-translated code (erased generics, Java method names, Java semantics).
using System;
using J2CS.Ext;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace java.lang
{
    /// <summary>Base of every translated class (Java's java.lang.Object).</summary>
    public class Object
    {
        // the identity hash, kept on the object itself (see JO.identityHashCode): a dictionary keyed by game objects
        // used to take a lock and a weak-table lookup on every access
        internal int __hid;
        internal bool __hidSet;

        public virtual bool equals(object o) => ReferenceEquals(this, o);
        public virtual int hashCode() => JO.identityHashCode(this);
        public virtual string toString() => GetType().Name + "@" + hashCode().ToString("x");
        public virtual object clone() { var c = (Object)MemberwiseClone(); c.__hidSet = false; c.__hid = 0; return c; }
        public virtual void finalize() { }
        public override string ToString() => toString();
        public override bool Equals(object o) => equals(o);
        public override int GetHashCode() => hashCode();
    }

    public interface Runnable { void run(); }
    public sealed class Runnable_L : Object, Runnable
    {
        readonly Action f;
        public Runnable_L(Action f) { this.f = f; }
        public void run() => f();
    }

    public interface Comparable { int compareTo(object o); }
    public interface AutoCloseable { void close(); }
    public interface Iterable { java.util.Iterator iterator(); }
    public sealed class Iterable_L : Object, Iterable
    {
        readonly Func<java.util.Iterator> f;
        public Iterable_L(Func<java.util.Iterator> f) { this.f = f; }
        public java.util.Iterator iterator() => f();
    }
    public interface CharSequence { }

    /// <summary>Java enums: name/ordinal, identity equality.</summary>
    public abstract class Enum : Object, Comparable
    {
        readonly string name_;
        readonly int ordinal_;
        protected Enum(string name, int ordinal) { name_ = name; ordinal_ = ordinal; }
        public string name() => name_;
        public int ordinal() => ordinal_;
        public override string toString() => name_;
        public override bool equals(object o) => ReferenceEquals(this, o);
        public override int hashCode() => JO.identityHashCode(this);
        public virtual int compareTo(object o) => ordinal_ - ((Enum)o).ordinal_;
        public System.Type getDeclaringClass() => GetType().IsNested && GetType().BaseType != typeof(Enum) ? GetType().BaseType : GetType();
    }

    /// <summary>java.lang.Object methods for any receiver (strings, boxed values, interfaces, arrays).</summary>
    public static class JO
    {
        static readonly ConditionalWeakTable<object, object> ids = new ConditionalWeakTable<object, object>();
        static int nextId = 0x1234567;

        /// <summary>Stable per-object hash (Java's is random; nothing deterministic may depend on it).</summary>
        public static int identityHashCode(object o)
        {
            if (o == null) return 0;
            if (o is Object jo)
            {
                if (System.Threading.Volatile.Read(ref jo.__hidSet)) return jo.__hid;
                lock (ids)
                {
                    if (!jo.__hidSet)
                    {
                        // the same id sequence as before, handed out in the same order
                        unchecked { nextId = nextId * 1103515245 + 12345; }
                        jo.__hid = nextId & 0x7fffffff;
                        System.Threading.Volatile.Write(ref jo.__hidSet, true);
                    }
                    return jo.__hid;
                }
            }
            lock (ids)
            {
                if (ids.TryGetValue(o, out var v)) return (int)v;
                unchecked { nextId = nextId * 1103515245 + 12345; }
                int id = nextId & 0x7fffffff;
                ids.Add(o, id);
                return id;
            }
        }

        public static bool equals(object a, object b)
        {
            if (a == null) throw new NullReferenceException();
            switch (a)
            {
                case Object jo: return jo.equals(b);
                case string s: return b is string t && s == t;
                case int i: return b is int j && i == j;
                case long l: return b is long m && l == m;
                case float f: return b is float g && Float.floatToIntBits(f) == Float.floatToIntBits(g);
                case double d: return b is double e && Double.doubleToLongBits(d) == Double.doubleToLongBits(e);
                case bool x: return b is bool y && x == y;
                case char c: return b is char c2 && c == c2;
                case short sh: return b is short sh2 && sh == sh2;
                case sbyte by: return b is sbyte by2 && by == by2;
                default: return ReferenceEquals(a, b) || (a is System.Type && a.Equals(b));
            }
        }

        public static int hashCode(object o)
        {
            switch (o)
            {
                case null: return 0;
                case Object jo: return jo.hashCode();
                case string s: return JString.hashCode(s);
                case int i: return i;
                case long l: return (int)(l ^ (long)((ulong)l >> 32));
                case float f: return Float.floatToIntBits(f);
                case double d: { long b = Double.doubleToLongBits(d); return (int)(b ^ (long)((ulong)b >> 32)); }
                case bool x: return x ? 1231 : 1237;
                case char c: return c;
                case short sh: return sh;
                case sbyte by: return by;
                default: return identityHashCode(o);
            }
        }

        public static string toString(object o)
        {
            switch (o)
            {
                case null: throw new NullReferenceException();
                default: return JStr.valueOf(o);
            }
        }
    }

    /// <summary>Java numeric conversions and operators that differ from C#.</summary>
    public static class JC
    {
        public static int f2i(float f)
        {
            if (float.IsNaN(f)) return 0;
            if (f >= 2147483647f) return int.MaxValue;
            if (f <= -2147483648f) return int.MinValue;
            return (int)f;
        }
        public static int d2i(double d)
        {
            if (double.IsNaN(d)) return 0;
            if (d >= 2147483647.0) return int.MaxValue;
            if (d <= -2147483648.0) return int.MinValue;
            return (int)d;
        }
        public static long f2l(float f) => d2l(f);
        public static long d2l(double d)
        {
            if (double.IsNaN(d)) return 0;
            if (d >= 9223372036854775807.0) return long.MaxValue;
            if (d <= -9223372036854775808.0) return long.MinValue;
            return (long)d;
        }
        public static int ushr(int x, int n) => (int)((uint)x >> (n & 31));
        public static long ushr(long x, int n) => (long)((ulong)x >> (n & 63));
    }

    public static class JA
    {
        public static T[][] newArray2<T>(int a, int b)
        {
            var r = new T[a][];
            for (int i = 0; i < a; i++) r[i] = new T[b];
            return r;
        }
        public static T[][][] newArray3<T>(int a, int b, int c)
        {
            var r = new T[a][][];
            for (int i = 0; i < a; i++) r[i] = newArray2<T>(b, c);
            return r;
        }
        public static T[] newArray1<T>(int a) => new T[a];
    }

    /// <summary>foreach over a Java Iterable.</summary>
    public static class JIter
    {
        public static System.Collections.IEnumerable of(object it)
        {
            if (it is System.Collections.IEnumerable e && !(it is string)) return e;
            return Wrap((Iterable)it);
        }
        static System.Collections.IEnumerable Wrap(Iterable it)
        {
            var i = it.iterator();
            while (i.hasNext()) yield return i.next();
        }
    }

    /// <summary>Java string conversion (String.valueOf semantics).</summary>
    public static class JStr
    {
        public static string valueOf(object o)
        {
            switch (o)
            {
                case null: return "null";
                case string s: return s;
                case Object jo: return jo.toString();
                case float f: return Float.toString(f);
                case double d: return Double.toString(d);
                case bool b: return b ? "true" : "false";
                case char c: return c.ToString();
                case int i: return i.ToString(CultureInfo.InvariantCulture);
                case long l: return l.ToString(CultureInfo.InvariantCulture);
                case System.Type t: return (t.IsInterface ? "interface " : "class ") + t.FullName;
                case Array a: return (a is int[] ? "[I" : a is float[] ? "[F" : "[L") + "@" + JO.identityHashCode(a).ToString("x");
                default: return o.ToString();
            }
        }
        public static string valueOf(float f) => Float.toString(f);
        public static string valueOf(double d) => Double.toString(d);
        public static string valueOf(bool b) => b ? "true" : "false";
        public static string valueOf(int i) => i.ToString(CultureInfo.InvariantCulture);
        public static string valueOf(long l) => l.ToString(CultureInfo.InvariantCulture);
        public static string valueOf(char c) => c.ToString();
    }

    /// <summary>Static members of java.lang.String.</summary>
    public static class JString
    {
        public static int hashCode(string s)
        {
            int h = 0;
            unchecked { foreach (char c in s) h = 31 * h + c; }
            return h;
        }
        public static string valueOf(object o) => JStr.valueOf(o);
        public static string valueOf(float f) => JStr.valueOf(f);
        public static string valueOf(double d) => JStr.valueOf(d);
        public static string valueOf(int i) => JStr.valueOf(i);
        public static string valueOf(long l) => JStr.valueOf(l);
        public static string valueOf(bool b) => JStr.valueOf(b);
        public static string valueOf(char c) => c.ToString();
        public static string valueOf(char[] c) => new string(c);
        public static string create(char[] c) => new string(c);
        public static string create(char[] c, int off, int len) => new string(c, off, len);
        public static string create(string s) => s;
        public static string create() => "";
        public static string join(object sep, params object[] parts) => string.Join(JStr.valueOf(sep), Array.ConvertAll(parts, JStr.valueOf));
        public static string join(object sep, java.lang.Iterable parts)
        {
            var l = new List<string>();
            foreach (var p in JIter.of(parts)) l.Add(JStr.valueOf(p));
            return string.Join(JStr.valueOf(sep), l);
        }
        public static string format(string fmt, params object[] args) => JFormat.format(fmt, args);
        public static string format(java.util.Locale l, string fmt, params object[] args) => JFormat.format(fmt, args);
    }



    public static class JClass
    {
        public static System.Type forName(string n) => System.Type.GetType(n);
    }



    public static class JSystem
    {
        public static readonly java.io.PrintStream @out = new java.io.PrintStream(false);
        public static readonly java.io.PrintStream err = new java.io.PrintStream(true);
        public static readonly java.io.InputStream @in = new java.io.ByteArrayInputStream(new sbyte[0]);
        static readonly System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        public static long currentTimeMillis() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public static long nanoTime() => (long)(sw.ElapsedTicks * (1e9 / System.Diagnostics.Stopwatch.Frequency));
        public static void arraycopy(object src, int sp, object dst, int dp, int n) => ArrCopy((Array)src, sp, (Array)dst, dp, n);

        /// <summary>
        /// Array.Copy that also copies between byte[] and sbyte[] (or any same-size primitive pair). The port reinterprets
        /// byte[] as sbyte[] (Java bytes) and back; .NET's Array.Copy allows that, but Unity's Mono throws
        /// ArrayTypeMismatchException, so primitive copies go through Buffer.BlockCopy.
        /// </summary>
        public static void ArrCopy(Array src, int sp, Array dst, int dp, int n)
        {
            var st = src.GetType().GetElementType();
            if (st.IsPrimitive && dst.GetType().GetElementType().IsPrimitive)
            {
                if (n < 0 || sp < 0 || dp < 0 || sp + n > src.Length || dp + n > dst.Length) throw new IndexOutOfRangeException("arraycopy: last source index " + (sp + n) + " out of bounds");
                if (n == 0) return;
                int size = Buffer.ByteLength(src) / src.Length;
                Buffer.BlockCopy(src, sp * size, dst, dp * size, n * size);
                return;
            }
            Array.Copy(src, sp, dst, dp, n);
        }
        public static int identityHashCode(object o) => JO.identityHashCode(o);
        public static string getProperty(string k) => k == "line.separator" ? "\n" : k == "os.name" ? Environment.OSVersion.Platform.ToString()
            : k == "user.dir" ? Environment.CurrentDirectory : k == "user.home" ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) : null;
        public static string getProperty(string k, string d) => getProperty(k) ?? d;
        public static string getenv(string k) => Environment.GetEnvironmentVariable(k);
        public static string lineSeparator() => "\n";
        public static void exit(int code) => throw new InvalidOperationException("System.exit(" + code + ")");
        public static void gc() { }
    }

    public static class IO
    {
        public static Action<string> Sink = s => Console.WriteLine(s);
        public static void println(object o) => Sink?.Invoke(JStr.valueOf(o));
        public static void println() => Sink?.Invoke("");
        public static void print(object o) => Sink?.Invoke(JStr.valueOf(o));
    }

    public static class Math
    {
        public const double PI = 3.141592653589793;
        public const double E = 2.718281828459045;
        public static int abs(int a) { unchecked { return a < 0 ? -a : a; } }
        public static long abs(long a) { unchecked { return a < 0 ? -a : a; } }
        public static float abs(float a) => System.Math.Abs(a);
        public static double abs(double a) => System.Math.Abs(a);
        public static int max(int a, int b) => a >= b ? a : b;
        public static int min(int a, int b) => a <= b ? a : b;
        public static long max(long a, long b) => a >= b ? a : b;
        public static long min(long a, long b) => a <= b ? a : b;
        public static float max(float a, float b)
        {
            if (a != a) return a;
            if (a == 0f && b == 0f && Float.floatToRawIntBits(a) == Float.floatToRawIntBits(-0f)) return b;
            return a >= b ? a : b;
        }
        public static float min(float a, float b)
        {
            if (a != a) return a;
            if (a == 0f && b == 0f && Float.floatToRawIntBits(b) == Float.floatToRawIntBits(-0f)) return b;
            return a <= b ? a : b;
        }
        public static double max(double a, double b)
        {
            if (a != a) return a;
            if (a == 0.0 && b == 0.0 && BitConverter.DoubleToInt64Bits(a) == BitConverter.DoubleToInt64Bits(-0.0)) return b;
            return a >= b ? a : b;
        }
        public static double min(double a, double b)
        {
            if (a != a) return a;
            if (a == 0.0 && b == 0.0 && BitConverter.DoubleToInt64Bits(b) == BitConverter.DoubleToInt64Bits(-0.0)) return b;
            return a <= b ? a : b;
        }
        public static int clamp(long v, int lo, int hi) => (int)System.Math.Min(hi, System.Math.Max(v, lo));
        public static long clamp(long v, long lo, long hi) => System.Math.Min(hi, System.Math.Max(v, lo));
        public static float clamp(float v, float lo, float hi) => min(hi, max(v, lo));
        public static double clamp(double v, double lo, double hi) => min(hi, max(v, lo));
        public static double sqrt(double a) => System.Math.Sqrt(a);
        public static double cbrt(double a) => System.Math.Cbrt(a);
        public static double floor(double a) => System.Math.Floor(a);
        public static double ceil(double a) => System.Math.Ceiling(a);
        public static double rint(double a) => System.Math.Round(a, MidpointRounding.ToEven);
        public static double pow(double a, double b) => System.Math.Pow(a, b);
        public static double sin(double a) => System.Math.Sin(a);
        public static double cos(double a) => System.Math.Cos(a);
        public static double tan(double a) => System.Math.Tan(a);
        public static double asin(double a) => System.Math.Asin(a);
        public static double acos(double a) => System.Math.Acos(a);
        public static double atan(double a) => System.Math.Atan(a);
        public static double atan2(double y, double x) => System.Math.Atan2(y, x);
        public static double exp(double a) => System.Math.Exp(a);
        public static double log(double a) => System.Math.Log(a);
        public static double log10(double a) => System.Math.Log10(a);
        public static double hypot(double a, double b) => System.Math.Sqrt(a * a + b * b);
        public static double toRadians(double d) => d / 180.0 * PI;
        public static double toDegrees(double r) => r * 180.0 / PI;
        public static float signum(float f) => f > 0 ? 1f : f < 0 ? -1f : f;
        public static double signum(double d) => d > 0 ? 1.0 : d < 0 ? -1.0 : d;
        public static int round(float a)
        {
            if (float.IsNaN(a)) return 0;
            return JC.f2i((float)System.Math.Floor((double)a + 0.5));
        }
        public static long round(double a)
        {
            if (double.IsNaN(a)) return 0;
            return JC.d2l(System.Math.Floor(a + 0.5));
        }
        public static int floorDiv(int x, int y) { int q = x / y; if ((x % y != 0) && ((x ^ y) < 0)) q--; return q; }
        public static int floorMod(int x, int y) { int m = x % y; if (m != 0 && ((m ^ y) < 0)) m += y; return m; }
        public static long floorDiv(long x, long y) { long q = x / y; if ((x % y != 0) && ((x ^ y) < 0)) q--; return q; }
        public static long floorMod(long x, long y) { long m = x % y; if (m != 0 && ((m ^ y) < 0)) m += y; return m; }
        public static int addExact(int a, int b) => checked(a + b);
        public static int multiplyExact(int a, int b) => checked(a * b);
        public static int toIntExact(long a) => checked((int)a);
        public static int absExact(int a) => checked(a < 0 ? -a : a);
        static readonly System.Random rnd = new System.Random();
        public static double random() => rnd.NextDouble();
        public static double IEEEremainder(double a, double b) => System.Math.IEEERemainder(a, b);
        public static double expm1(double a) => j2cs.fdlibm.FdLibm.Expm1.compute(a);
        public static double log1p(double a) => j2cs.fdlibm.FdLibm.Log1p.compute(a);
        static double PowerOfTwoD(int n) => BitConverter.Int64BitsToDouble(((long)n + 1023L) << 52);
        /// <summary>java.lang.Math.scalb (exact, same stepping as the JDK).</summary>
        public static double scalb(double d, int scaleFactor)
        {
            const int MAX_SCALE = 1023 + 1022 + 53 + 1;
            int scale_increment; double exp_delta;
            if (scaleFactor < 0) { scaleFactor = System.Math.Max(scaleFactor, -MAX_SCALE); scale_increment = -512; exp_delta = PowerOfTwoD(-512); }
            else { scaleFactor = System.Math.Min(scaleFactor, MAX_SCALE); scale_increment = 512; exp_delta = PowerOfTwoD(512); }
            int t = (int)((uint)(scaleFactor >> (9 - 1)) >> (32 - 9));
            int exp_adjust = ((scaleFactor + t) & (512 - 1)) - t;
            d *= PowerOfTwoD(exp_adjust);
            scaleFactor -= exp_adjust;
            while (scaleFactor != 0) { d *= exp_delta; scaleFactor -= scale_increment; }
            return d;
        }
        public static float ulp(float f) { float a = System.Math.Abs(f); return (float)(nextUp(a) - a); }
        public static float nextUp(float f)
        {
            if (float.IsNaN(f) || float.IsPositiveInfinity(f)) return f;
            if (f == 0f) return float.Epsilon;
            int b = BitConverter.SingleToInt32Bits(f);
            return BitConverter.Int32BitsToSingle(f > 0 ? b + 1 : b - 1);
        }
        public static float nextDown(float f)
        {
            if (float.IsNaN(f) || float.IsNegativeInfinity(f)) return f;
            if (f == 0f) return -float.Epsilon;
            int b = BitConverter.SingleToInt32Bits(f);
            return BitConverter.Int32BitsToSingle(f > 0 ? b - 1 : b + 1);
        }
        public static float copySign(float m, float s) { int mb = BitConverter.SingleToInt32Bits(m) & 0x7fffffff, sb = BitConverter.SingleToInt32Bits(s) & unchecked((int)0x80000000); return BitConverter.Int32BitsToSingle(mb | sb); }
        public static double copySign(double m, double s) { long mb = BitConverter.DoubleToInt64Bits(m) & 0x7fffffffffffffffL, sb = BitConverter.DoubleToInt64Bits(s) & long.MinValue; return BitConverter.Int64BitsToDouble(mb | sb); }
    }

    public static class Float
    {
        public const float MAX_VALUE = float.MaxValue;
        public const float MIN_VALUE = float.Epsilon;
        public const float MIN_NORMAL = 1.17549435E-38f;
        public const float POSITIVE_INFINITY = float.PositiveInfinity;
        public const float NEGATIVE_INFINITY = float.NegativeInfinity;
        public const float NaN = float.NaN;
        public const int SIZE = 32, BYTES = 4;
        public static bool isNaN(float f) => float.IsNaN(f);
        public static bool isInfinite(float f) => float.IsInfinity(f);
        public static bool isFinite(float f) => !float.IsNaN(f) && !float.IsInfinity(f);
        public static int floatToIntBits(float f) => float.IsNaN(f) ? 0x7fc00000 : BitConverter.SingleToInt32Bits(f);
        public static int floatToRawIntBits(float f) => BitConverter.SingleToInt32Bits(f);
        public static float intBitsToFloat(int b) => BitConverter.Int32BitsToSingle(b);
        public static int compare(float a, float b)
        {
            if (a < b) return -1;
            if (a > b) return 1;
            int x = floatToIntBits(a), y = floatToIntBits(b);
            return x == y ? 0 : (x < y ? -1 : 1);
        }
        public static int hashCode(float f) => floatToIntBits(f);
        public static float? valueOf(float f) => f;
        public static float? valueOf(string s) => parseFloat(s);
        public static float parseFloat(string s) => float.Parse(s.Trim().TrimEnd('f', 'F', 'd', 'D'), NumberStyles.Float, CultureInfo.InvariantCulture);
        public static float sum(float a, float b) => a + b;
        public static float max(float a, float b) => Math.max(a, b);
        public static float min(float a, float b) => Math.min(a, b);
        public static string toString(float f) => JFloatFormat.ToJava(f);
    }

    public static class Double
    {
        public const double MAX_VALUE = double.MaxValue;
        public const double MIN_VALUE = double.Epsilon;
        public const double MIN_NORMAL = 2.2250738585072014E-308;
        public const int MAX_EXPONENT = 1023, MIN_EXPONENT = -1022;
        public const double POSITIVE_INFINITY = double.PositiveInfinity;
        public const double NEGATIVE_INFINITY = double.NegativeInfinity;
        public const double NaN = double.NaN;
        public static bool isNaN(double d) => double.IsNaN(d);
        public static bool isInfinite(double d) => double.IsInfinity(d);
        public static bool isFinite(double d) => !double.IsNaN(d) && !double.IsInfinity(d);
        public static long doubleToLongBits(double d) => double.IsNaN(d) ? 0x7ff8000000000000L : BitConverter.DoubleToInt64Bits(d);
        public static long doubleToRawLongBits(double d) => BitConverter.DoubleToInt64Bits(d);
        public static double longBitsToDouble(long b) => BitConverter.Int64BitsToDouble(b);
        public static int compare(double a, double b)
        {
            if (a < b) return -1;
            if (a > b) return 1;
            long x = doubleToLongBits(a), y = doubleToLongBits(b);
            return x == y ? 0 : (x < y ? -1 : 1);
        }
        public static double parseDouble(string s) => double.Parse(s.Trim().TrimEnd('d', 'D', 'f', 'F'), NumberStyles.Float, CultureInfo.InvariantCulture);
        public static double? valueOf(double d) => d;
        public static double sum(double a, double b) => a + b;
        public static int hashCode(double d) { long b = doubleToLongBits(d); return (int)(b ^ (long)((ulong)b >> 32)); }
        public static string toString(double d) => JFloatFormat.ToJava(d);
    }

    public static class Integer
    {
        public const int MAX_VALUE = int.MaxValue, MIN_VALUE = int.MinValue, SIZE = 32, BYTES = 4;
        public static int parseInt(string s) => int.Parse(s.Trim(), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
        public static int parseInt(string s, int radix) => Convert.ToInt32(s.Trim(), radix);
        public static int? valueOf(int i) => i;
        public static int? valueOf(string s) => parseInt(s);
        public static string toString(int i) => i.ToString(CultureInfo.InvariantCulture);
        public static string toString(int i, int radix) => radix == 10 ? toString(i) : Convert.ToString(i, radix);
        public static string toHexString(int i) => ((uint)i).ToString("x");
        public static string toBinaryString(int i) => Convert.ToString(i, 2);
        public static int compare(int a, int b) => a < b ? -1 : (a == b ? 0 : 1);
        public static int compareUnsigned(int a, int b) => ((uint)a).CompareTo((uint)b);
        public static int signum(int i) => i > 0 ? 1 : i < 0 ? -1 : 0;
        public static int bitCount(int i) { uint u = (uint)i; int c = 0; while (u != 0) { c += (int)(u & 1); u >>= 1; } return c; }
        public static int highestOneBit(int i) => i == 0 ? 0 : (int)(0x80000000u >> numberOfLeadingZeros(i));
        public static int lowestOneBit(int i) => i & -i;
        public static int numberOfLeadingZeros(int i) { if (i == 0) return 32; int n = 0; uint u = (uint)i; while ((u & 0x80000000u) == 0) { n++; u <<= 1; } return n; }
        public static int numberOfTrailingZeros(int i) { if (i == 0) return 32; int n = 0; uint u = (uint)i; while ((u & 1) == 0) { n++; u >>= 1; } return n; }
        public static int sum(int a, int b) => a + b;
        public static int max(int a, int b) => Math.max(a, b);
        public static int min(int a, int b) => Math.min(a, b);
        public static int hashCode(int i) => i;
        public static int reverse(int i) { uint u = (uint)i, r = 0; for (int k = 0; k < 32; k++) { r = (r << 1) | (u & 1); u >>= 1; } return (int)r; }
    }

    public static class Long
    {
        public const long MAX_VALUE = long.MaxValue, MIN_VALUE = long.MinValue;
        public const int SIZE = 64, BYTES = 8;
        public static long parseLong(string s) => long.Parse(s.Trim(), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
        public static long parseLong(string s, int radix) => Convert.ToInt64(s.Trim(), radix);
        public static long? valueOf(long l) => l;
        public static string toString(long l) => l.ToString(CultureInfo.InvariantCulture);
        public static string toHexString(long l) => ((ulong)l).ToString("x");
        public static int compare(long a, long b) => a < b ? -1 : (a == b ? 0 : 1);
        public static int compareUnsigned(long a, long b) => ((ulong)a).CompareTo((ulong)b);
        public static int hashCode(long l) => (int)(l ^ (long)((ulong)l >> 32));
        public static int signum(long l) => l > 0 ? 1 : l < 0 ? -1 : 0;
        public static int bitCount(long l) { ulong u = (ulong)l; int c = 0; while (u != 0) { c += (int)(u & 1); u >>= 1; } return c; }
        public static int numberOfTrailingZeros(long l) { if (l == 0) return 64; int n = 0; ulong u = (ulong)l; while ((u & 1) == 0) { n++; u >>= 1; } return n; }
        public static long max(long a, long b) => Math.max(a, b);
        public static long min(long a, long b) => Math.min(a, b);
        public static long sum(long a, long b) => a + b;
    }

    public static class Short
    {
        public const short MAX_VALUE = short.MaxValue, MIN_VALUE = short.MinValue;
        public const int BYTES = 2, SIZE = 16;
        public static short parseShort(string s) => short.Parse(s.Trim(), CultureInfo.InvariantCulture);
        public static int toUnsignedInt(short s) => s & 0xffff;
    }

    public static class Byte
    {
        public const sbyte MAX_VALUE = sbyte.MaxValue, MIN_VALUE = sbyte.MinValue;
        public const int BYTES = 1, SIZE = 8;
        public static int toUnsignedInt(sbyte b) => b & 0xff;
        public static sbyte parseByte(string s) => sbyte.Parse(s.Trim(), CultureInfo.InvariantCulture);
    }

    public static class Boolean
    {
        public static readonly bool? TRUE = true, FALSE = false;
        public static bool getBoolean(string prop) => parseBoolean(JSystem.getProperty(prop));
        public static bool parseBoolean(string s) => s != null && s.Equals("true", StringComparison.OrdinalIgnoreCase);
        public static bool? valueOf(bool b) => b;
        public static bool? valueOf(string s) => parseBoolean(s);
        public static string toString(bool b) => b ? "true" : "false";
        public static int compare(bool a, bool b) => a == b ? 0 : (a ? 1 : -1);
        public static int hashCode(bool b) => b ? 1231 : 1237;
        public static bool logicalAnd(bool a, bool b) => a && b;
        public static bool logicalOr(bool a, bool b) => a || b;
    }

    public static class Character
    {
        public const char MAX_VALUE = char.MaxValue, MIN_VALUE = char.MinValue;
        public const int BYTES = 2, SIZE = 16;
        public static bool isDigit(char c) => char.IsDigit(c);
        public static bool isISOControl(char c) => c <= '\u001F' || (c >= '\u007F' && c <= '\u009F');
        public static bool isISOControl(int c) => (c >= 0 && c <= 0x1F) || (c >= 0x7F && c <= 0x9F);
        public static char[] toChars(int codePoint) => char.ConvertFromUtf32(codePoint).ToCharArray();
        public static string toString(int codePoint) => char.ConvertFromUtf32(codePoint);
        public static bool isLetter(char c) => char.IsLetter(c);
        public static bool isLetterOrDigit(char c) => char.IsLetterOrDigit(c);
        public static bool isWhitespace(char c) => char.IsWhiteSpace(c);
        public static bool isUpperCase(char c) => char.IsUpper(c);
        public static bool isLowerCase(char c) => char.IsLower(c);
        public static char toUpperCase(char c) => char.ToUpperInvariant(c);
        public static char toLowerCase(char c) => char.ToLowerInvariant(c);
        public static int digit(char c, int radix) { int v = c >= '0' && c <= '9' ? c - '0' : c >= 'a' && c <= 'z' ? c - 'a' + 10 : c >= 'A' && c <= 'Z' ? c - 'A' + 10 : -1; return v >= 0 && v < radix ? v : -1; }
        public static string toString(char c) => c.ToString();
        public static char? valueOf(char c) => c;
        public static int compare(char a, char b) => a - b;
    }

    /// <summary>Java's Float/Double.toString formatting (shortest round-trip digits, Java layout).</summary>
    public static class JFloatFormat
    {
        public static string ToJava(float f)
        {
            if (float.IsNaN(f)) return "NaN";
            if (float.IsPositiveInfinity(f)) return "Infinity";
            if (float.IsNegativeInfinity(f)) return "-Infinity";
            if (f == 0) return BitConverter.SingleToInt32Bits(f) < 0 ? "-0.0" : "0.0";
            return Layout(f.ToString("R", CultureInfo.InvariantCulture), System.Math.Abs(f) >= 1e-3f && System.Math.Abs(f) < 1e7f);
        }
        public static string ToJava(double d)
        {
            if (double.IsNaN(d)) return "NaN";
            if (double.IsPositiveInfinity(d)) return "Infinity";
            if (double.IsNegativeInfinity(d)) return "-Infinity";
            if (d == 0) return BitConverter.DoubleToInt64Bits(d) < 0 ? "-0.0" : "0.0";
            return Layout(d.ToString("R", CultureInfo.InvariantCulture), System.Math.Abs(d) >= 1e-3 && System.Math.Abs(d) < 1e7);
        }
        static string Layout(string r, bool plain)
        {
            bool neg = r.StartsWith("-");
            if (neg) r = r.Substring(1);
            string mant = r; int exp = 0;
            int e = r.IndexOfAny(new[] { 'E', 'e' });
            if (e >= 0) { mant = r.Substring(0, e); exp = int.Parse(r.Substring(e + 1), CultureInfo.InvariantCulture); }
            string digits = mant.Replace(".", "");
            int point = mant.IndexOf('.'); if (point < 0) point = mant.Length;
            int lead = 0; while (lead < digits.Length - 1 && digits[lead] == '0') { lead++; }
            int decExp = point + exp - lead; // position of decimal point relative to first significant digit
            digits = digits.Substring(lead).TrimEnd('0');
            if (digits.Length == 0) digits = "0";
            string res;
            if (plain)
            {
                if (decExp <= 0) res = "0." + new string('0', -decExp) + digits;
                else if (decExp >= digits.Length) res = digits + new string('0', decExp - digits.Length) + ".0";
                else res = digits.Substring(0, decExp) + "." + digits.Substring(decExp);
            }
            else
            {
                res = digits.Substring(0, 1) + "." + (digits.Length > 1 ? digits.Substring(1) : "0") + "E" + (decExp - 1);
            }
            return neg ? "-" + res : res;
        }
    }

    /// <summary>Subset of java.util.Formatter (%d %s %f %.Nf %x %n %%).</summary>
    public static class JFormat
    {
        public static string format(string fmt, object[] args)
        {
            var sb = new System.Text.StringBuilder();
            int ai = 0;
            for (int i = 0; i < fmt.Length; i++)
            {
                char c = fmt[i];
                if (c != '%') { sb.Append(c); continue; }
                int j = i + 1;
                string flags = "";
                while (j < fmt.Length && "-+ 0,#(".IndexOf(fmt[j]) >= 0) flags += fmt[j++];
                int width = -1, prec = -1;
                int ws = j; while (j < fmt.Length && char.IsDigit(fmt[j])) j++;
                if (j > ws) width = int.Parse(fmt.Substring(ws, j - ws));
                if (j < fmt.Length && fmt[j] == '.') { j++; int ps = j; while (j < fmt.Length && char.IsDigit(fmt[j])) j++; prec = int.Parse(fmt.Substring(ps, j - ps)); }
                char conv = fmt[j];
                i = j;
                string s;
                switch (conv)
                {
                    case '%': s = "%"; break;
                    case 'n': s = "\n"; break;
                    case 'd': { object a = args[ai++]; long v = Convert.ToInt64(a); s = flags.Contains(",") ? v.ToString("N0", CultureInfo.InvariantCulture) : v.ToString(CultureInfo.InvariantCulture); if (flags.Contains("+") && v >= 0) s = "+" + s; break; }
                    case 'f': case 'e': case 'g':
                    {
                        double v = Convert.ToDouble(args[ai++]);
                        s = v.ToString((conv == 'e' ? "E" : "F") + (prec < 0 ? 6 : prec), CultureInfo.InvariantCulture);
                        if (flags.Contains("+") && v >= 0) s = "+" + s;
                        break;
                    }
                    case 'x': case 'X': { object a = args[ai++]; s = a is long l ? ((ulong)l).ToString("x") : ((uint)Convert.ToInt32(a)).ToString("x"); if (conv == 'X') s = s.ToUpperInvariant(); break; }
                    case 'c': s = JStr.valueOf(args[ai++]); break;
                    case 'b': s = JStr.valueOf(args[ai++]); break;
                    default: s = JStr.valueOf(args[ai++]); if (prec >= 0 && s.Length > prec) s = s.Substring(0, prec); break;
                }
                if (width > s.Length) s = flags.Contains("-") ? s.PadRight(width) : (flags.Contains("0") ? s.PadLeft(width, '0') : s.PadLeft(width));
                sb.Append(s);
            }
            return sb.ToString();
        }
    }

    public sealed class Package_ : Object
    {
        readonly string n;
        public Package_(string n) { this.n = n; }
        public string getName() => n;
    }
}

namespace java.io
{
    public class PrintStream : java.lang.Object
    {
        readonly bool err;
        public PrintStream(bool err) { this.err = err; }
        public void println(object o) => java.lang.IO.println(o);
        public void println() => java.lang.IO.println("");
        public void print(object o) => java.lang.IO.print(o);
        public void printf(string f, params object[] a) => java.lang.IO.print(java.lang.JFormat.format(f, a));
        public void flush() { }
    }
}

namespace java.lang
{
    /// <summary>java.lang.StrictMath: fdlibm algorithms (bit-identical to Java on every platform).</summary>
    public static class StrictMath
    {
        public const double PI = Math.PI, E = Math.E;
        public static double sin(double a) => j2cs.fdlibm.FdLibm.Sin.compute(a);
        public static double cos(double a) => j2cs.fdlibm.FdLibm.Cos.compute(a);
        public static double tan(double a) => j2cs.fdlibm.FdLibm.Tan.compute(a);
        public static double asin(double a) => j2cs.fdlibm.FdLibm.Asin.compute(a);
        public static double acos(double a) => j2cs.fdlibm.FdLibm.Acos.compute(a);
        public static double atan(double a) => j2cs.fdlibm.FdLibm.Atan.compute(a);
        public static double atan2(double y, double x) => j2cs.fdlibm.FdLibm.Atan2.compute(y, x);
        public static double exp(double a) => j2cs.fdlibm.FdLibm.Exp.compute(a);
        public static double log(double a) => j2cs.fdlibm.FdLibm.Log.compute(a);
        public static double log10(double a) => j2cs.fdlibm.FdLibm.Log10.compute(a);
        public static double pow(double a, double b) => j2cs.fdlibm.FdLibm.Pow.compute(a, b);
        public static double cbrt(double a) => j2cs.fdlibm.FdLibm.Cbrt.compute(a);
        public static double hypot(double a, double b) => j2cs.fdlibm.FdLibm.Hypot.compute(a, b);
        public static double sqrt(double a) => System.Math.Sqrt(a);
        public static double toRadians(double d) => Math.toRadians(d);
        public static double toDegrees(double r) => Math.toDegrees(r);
        public static double floor(double a) => System.Math.Floor(a);
        public static double ceil(double a) => System.Math.Ceiling(a);
        public static double abs(double a) => System.Math.Abs(a);
        public static float abs(float a) => System.Math.Abs(a);
        public static int abs(int a) => Math.abs(a);
        public static double max(double a, double b) => Math.max(a, b);
        public static double min(double a, double b) => Math.min(a, b);
        public static float max(float a, float b) => Math.max(a, b);
        public static float min(float a, float b) => Math.min(a, b);
        public static int max(int a, int b) => Math.max(a, b);
        public static int min(int a, int b) => Math.min(a, b);
        public static int round(float a) => Math.round(a);
        public static long round(double a) => Math.round(a);
        public static double signum(double d) => Math.signum(d);
        public static float signum(float f) => Math.signum(f);
        public static double IEEEremainder(double a, double b) => System.Math.IEEERemainder(a, b);
    }
}
