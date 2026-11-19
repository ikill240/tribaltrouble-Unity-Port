// java.math.BigInteger over System.Numerics.BigInteger (map codes: WordsEncoding / MapSettings).
using System;
using SNB = System.Numerics.BigInteger;

namespace java.math
{
    public sealed class BigInteger : java.lang.Object, java.lang.Comparable
    {
        internal readonly SNB v;
        BigInteger(SNB v) { this.v = v; }
        public static BigInteger FromSystem(SNB v) => new BigInteger(v);
        public SNB ToSystem() => v;
        public static readonly BigInteger ZERO = new BigInteger(SNB.Zero), ONE = new BigInteger(SNB.One), TWO = new BigInteger(2), TEN = new BigInteger(10);
        public BigInteger(string s) : this(s, 10) { }
        /// <summary>Java BigInteger(numBits, rnd): uniform in [0, 2^numBits), from rnd.nextBytes like the JDK.</summary>
        public BigInteger(int numBits, java.util.Random rnd)
        {
            int numBytes = (int)(((long)numBits + 7) / 8);
            var bytes = new sbyte[numBytes];
            if (numBytes > 0)
            {
                rnd.nextBytes(bytes);
                int excess = 8 * numBytes - numBits;
                bytes[0] = (sbyte)(bytes[0] & ((1 << (8 - excess)) - 1));
            }
            var be = new byte[numBytes + 1];   // leading zero: positive
            for (int i = 0; i < numBytes; i++) be[i + 1] = (byte)bytes[i];
            System.Array.Reverse(be);
            v = new SNB(be);
        }
        public BigInteger(string s, int radix) { v = Parse(s, radix); }
        public BigInteger(int signum, sbyte[] magnitude)
        {
            var le = new byte[magnitude.Length + 1];
            for (int i = 0; i < magnitude.Length; i++) le[i] = (byte)magnitude[magnitude.Length - 1 - i];
            var m = new SNB(le);
            v = signum < 0 ? -m : m;
        }
        public BigInteger(sbyte[] twosComplement)
        {
            var le = new byte[twosComplement.Length];
            for (int i = 0; i < le.Length; i++) le[i] = (byte)twosComplement[twosComplement.Length - 1 - i];
            v = new SNB(le);
        }
        static SNB Parse(string s, int radix)
        {
            bool neg = s.StartsWith("-");
            if (neg || s.StartsWith("+")) s = s.Substring(1);
            if (s.Length == 0) throw new FormatException("Zero length BigInteger");
            SNB r = SNB.Zero;
            foreach (char c in s)
            {
                int d = java.lang.Character.digit(c, radix);
                if (d < 0) throw new FormatException("For input string: \"" + s + "\"");
                r = r * radix + d;
            }
            return neg ? -r : r;
        }
        public static BigInteger valueOf(long x) => new BigInteger(new SNB(x));
        public BigInteger add(BigInteger o) => new BigInteger(v + o.v);
        public BigInteger subtract(BigInteger o) => new BigInteger(v - o.v);
        public BigInteger multiply(BigInteger o) => new BigInteger(v * o.v);
        public BigInteger divide(BigInteger o) => new BigInteger(SNB.Divide(v, o.v));
        public BigInteger remainder(BigInteger o) => new BigInteger(SNB.Remainder(v, o.v));
        public BigInteger mod(BigInteger m) { var r = SNB.Remainder(v, m.v); if (r.Sign < 0) r += m.v; return new BigInteger(r); }
        public BigInteger[] divideAndRemainder(BigInteger o) { var q = SNB.DivRem(v, o.v, out var r); return new[] { new BigInteger(q), new BigInteger(r) }; }
        public BigInteger pow(int e) => new BigInteger(SNB.Pow(v, e));
        public BigInteger modPow(BigInteger e, BigInteger m) => new BigInteger(SNB.ModPow(v, e.v, m.v));
        public BigInteger negate() => new BigInteger(-v);
        public BigInteger abs() => new BigInteger(SNB.Abs(v));
        public BigInteger and(BigInteger o) => new BigInteger(v & o.v);
        public BigInteger or(BigInteger o) => new BigInteger(v | o.v);
        public BigInteger xor(BigInteger o) => new BigInteger(v ^ o.v);
        public BigInteger not() => new BigInteger(~v);
        public BigInteger shiftLeft(int n) => new BigInteger(n >= 0 ? v << n : v >> -n);
        public BigInteger shiftRight(int n) => new BigInteger(n >= 0 ? v >> n : v << -n);
        public bool testBit(int n) => !((v >> n) & SNB.One).IsZero;
        public BigInteger setBit(int n) => new BigInteger(v | (SNB.One << n));
        public int signum() => v.Sign;
        public int bitLength() { var x = v.Sign < 0 ? -(v + 1) : v; int n = 0; while (!x.IsZero) { x >>= 1; n++; } return n; }
        public int intValue() => unchecked((int)(uint)(v & uint.MaxValue));
        public long longValue() => unchecked((long)(ulong)(v & ulong.MaxValue));
        public double doubleValue() => (double)v;
        public sbyte[] toByteArray() { var le = v.ToByteArray(); var r = new sbyte[le.Length]; for (int i = 0; i < le.Length; i++) r[i] = (sbyte)le[le.Length - 1 - i]; return r; }
        public int compareTo(object o) => v.CompareTo(((BigInteger)o).v);
        public BigInteger max(BigInteger o) => v >= o.v ? this : o;
        public BigInteger min(BigInteger o) => v <= o.v ? this : o;
        public override bool equals(object o) => o is BigInteger b && b.v == v;
        public override int hashCode() => v.GetHashCode();
        public override string toString() => v.ToString();
        public string toString(int radix)
        {
            if (radix == 10) return v.ToString();
            if (v.IsZero) return "0";
            var x = SNB.Abs(v);
            var sb = new System.Text.StringBuilder();
            while (!x.IsZero) { int d = (int)(x % radix); sb.Insert(0, (char)(d < 10 ? '0' + d : 'a' + d - 10)); x /= radix; }
            if (v.Sign < 0) sb.Insert(0, '-');
            return sb.ToString();
        }
    }
}
