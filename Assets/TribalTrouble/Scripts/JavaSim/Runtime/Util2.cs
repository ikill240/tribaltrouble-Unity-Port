// java.util helpers: Collections, Arrays, Objects, Optional, Random, Locale; java.util.function; java.lang.StringBuilder.
using System;
using J2CS.Ext;
using System.Collections;
using System.Collections.Generic;
using JO = java.lang.JO;

namespace java.util
{
    public static class Collections
    {
        sealed class IterEnumeration : java.lang.Object, Enumeration
        {
            readonly Iterator i;
            public IterEnumeration(Iterator i) { this.i = i; }
            public bool hasMoreElements() => i.hasNext();
            public object nextElement() => i.next();
        }
        public static Enumeration enumeration(Collection c) => new IterEnumeration(c.iterator());
        public static void sort(List l) => l.sort(null);
        public static void sort(List l, Comparator c) => l.sort(c);
        public static void reverse(List l)
        {
            for (int i = 0, j = l.size() - 1; i < j; i++, j--) { var t = l.get(i); l.set(i, l.get(j)); l.set(j, t); }
        }
        public static void swap(List l, int i, int j) { var t = l.get(i); l.set(i, l.get(j)); l.set(j, t); }
        /// <summary>Same swaps as java.util.Collections.shuffle(list, rnd).</summary>
        public static void shuffle(List l, Random rnd)
        {
            for (int i = l.size(); i > 1; i--) swap(l, i - 1, rnd.nextInt(i));
        }
        public static void shuffle(List l) => shuffle(l, new Random());
        public static List unmodifiableList(List l) => l;
        public static Set unmodifiableSet(Set s) => s;
        public static Map unmodifiableMap(Map m) => m;
        public static SortedMap unmodifiableSortedMap(SortedMap m) => m;
        public static Collection unmodifiableCollection(Collection c) => c;
        public static List emptyList() => new ArrayList();
        public static Set emptySet() => new HashSet();
        public static NavigableSet emptyNavigableSet() => new TreeSet();
        public static SortedSet emptySortedSet() => new TreeSet();
        public static Map emptyMap() => new HashMap();
        public static List singletonList(object o) { var l = new ArrayList(); l.add(o); return l; }
        public static Set singleton(object o) { var s = new HashSet(); s.add(o); return s; }
        public static List nCopies(int n, object o) { var l = new ArrayList(n); for (int i = 0; i < n; i++) l.add(o); return l; }
        public static bool addAll(Collection c, params object[] items) { bool ch = false; foreach (var o in items) ch |= c.add(o); return ch; }
        public static int frequency(Collection c, object o) { int n = 0; foreach (var x in c) if (AbstractCollection.Eq(o, x)) n++; return n; }
        public static object max(Collection c) => max(c, null);
        public static object min(Collection c) => min(c, null);
        public static object max(Collection c, Comparator cmp)
        {
            var it = c.iterator(); var best = it.next(); var f = Comparator_.Of(cmp);
            while (it.hasNext()) { var o = it.next(); if (f(o, best) > 0) best = o; }
            return best;
        }
        public static object min(Collection c, Comparator cmp)
        {
            var it = c.iterator(); var best = it.next(); var f = Comparator_.Of(cmp);
            while (it.hasNext()) { var o = it.next(); if (f(o, best) < 0) best = o; }
            return best;
        }
        public static Comparator reverseOrder() => Comparator_.reverseOrder();
        public static Comparator reverseOrder(Comparator c) => c.reversed();
    }

    public static class Arrays
    {
        /// <summary>Stable merge sort (Java's object sort is stable, so any stable sort gives the same order).</summary>
        public static void sort<T>(T[] a, Comparator c) => SortAny(a, 0, a.Length, Comparator_.Of(c));
        public static void sort(object[] a) => StableSort(a, 0, a.Length, Comparator_.Natural);
        public static void sort<T>(T[] a, int from, int to, Comparator c) => SortAny(a, from, to, Comparator_.Of(c));
        static void SortAny<T>(T[] a, int from, int to, Comparison<object> c)
        {
            if (a is object[] oa) { StableSort(oa, from, to, c); return; }
            var tmp = new object[a.Length];
            for (int i = 0; i < a.Length; i++) tmp[i] = a[i];
            StableSort(tmp, from, to, c);
            for (int i = from; i < to; i++) a[i] = (T)tmp[i];
        }
        public static void sort(int[] a) => Array.Sort(a);
        public static void sort(int[] a, int from, int to) => Array.Sort(a, from, to - from);
        public static void sort(long[] a) => Array.Sort(a);
        public static void sort(short[] a) => Array.Sort(a);
        public static void sort(char[] a) => Array.Sort(a);
        public static void sort(float[] a) => Array.Sort(a, (x, y) => java.lang.Float.compare(x, y));
        public static void sort(double[] a) => Array.Sort(a, (x, y) => java.lang.Double.compare(x, y));
        static void StableSort(object[] a, int from, int to, Comparison<object> c)
        {
            int n = to - from;
            if (n < 2) return;
            var tmp = new object[n];
            Array.Copy(a, from, tmp, 0, n);
            MergeSort(tmp, a, from, to, -from, c);
        }
        static void MergeSort(object[] src, object[] dest, int low, int high, int off, Comparison<object> c)
        {
            int length = high - low;
            if (length < 7)
            {
                for (int i = low; i < high; i++)
                    for (int j = i; j > low && c(dest[j - 1], dest[j]) > 0; j--) { var t = dest[j]; dest[j] = dest[j - 1]; dest[j - 1] = t; }
                return;
            }
            int destLow = low, destHigh = high;
            low += off; high += off;
            int mid = (int)((uint)(low + high) >> 1);
            MergeSort(dest, src, low, mid, -off, c);
            MergeSort(dest, src, mid, high, -off, c);
            if (c(src[mid - 1], src[mid]) <= 0) { Array.Copy(src, low, dest, destLow, length); return; }
            for (int i = destLow, p = low, q = mid; i < destHigh; i++)
            {
                if (q >= high || p < mid && c(src[p], src[q]) <= 0) dest[i] = src[p++];
                else dest[i] = src[q++];
            }
        }
        public static void fill(int[] a, int v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(int[] a, int from, int to, int v) { for (int i = from; i < to; i++) a[i] = v; }
        public static void fill(float[] a, float v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(float[] a, int from, int to, float v) { for (int i = from; i < to; i++) a[i] = v; }
        public static void fill(double[] a, double v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(long[] a, long v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(short[] a, short v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(sbyte[] a, sbyte v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(char[] a, char v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(bool[] a, bool v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(bool[] a, int from, int to, bool v) { for (int i = from; i < to; i++) a[i] = v; }
        public static void fill(object[] a, object v) { for (int i = 0; i < a.Length; i++) a[i] = v; }
        public static void fill(object[] a, int from, int to, object v) { for (int i = from; i < to; i++) a[i] = v; }
        public static int[] copyOf(int[] a, int n) { var r = new int[n]; Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static float[] copyOf(float[] a, int n) { var r = new float[n]; Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static bool[] copyOf(bool[] a, int n) { var r = new bool[n]; Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static long[] copyOf(long[] a, int n) { var r = new long[n]; Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static sbyte[] copyOf(sbyte[] a, int n) { var r = new sbyte[n]; java.lang.JSystem.ArrCopy(a, 0, r, 0, System.Math.Min(n, a.Length)); return r; }
        public static short[] copyOf(short[] a, int n) { var r = new short[n]; Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static double[] copyOf(double[] a, int n) { var r = new double[n]; Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static char[] copyOf(char[] a, int n) { var r = new char[n]; Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static object[] copyOf(object[] a, int n) { var r = (object[])Array.CreateInstance(a.GetType().GetElementType(), n); Array.Copy(a, r, System.Math.Min(n, a.Length)); return r; }
        public static int[] copyOfRange(int[] a, int f, int t) { var r = new int[t - f]; Array.Copy(a, f, r, 0, System.Math.Min(t, a.Length) - f); return r; }
        public static float[] copyOfRange(float[] a, int f, int t) { var r = new float[t - f]; Array.Copy(a, f, r, 0, System.Math.Min(t, a.Length) - f); return r; }
        public static sbyte[] copyOfRange(sbyte[] a, int f, int t) { var r = new sbyte[t - f]; java.lang.JSystem.ArrCopy(a, f, r, 0, System.Math.Min(t, a.Length) - f); return r; }
        public static char[] copyOfRange(char[] a, int f, int t) { var r = new char[t - f]; Array.Copy(a, f, r, 0, System.Math.Min(t, a.Length) - f); return r; }
        public static long[] copyOfRange(long[] a, int f, int t) { var r = new long[t - f]; Array.Copy(a, f, r, 0, System.Math.Min(t, a.Length) - f); return r; }
        public static object[] copyOfRange(object[] a, int f, int t) { var r = (object[])Array.CreateInstance(a.GetType().GetElementType(), t - f); Array.Copy(a, f, r, 0, System.Math.Min(t, a.Length) - f); return r; }
        public static bool equals(int[] a, int[] b) { if (a == b) return true; if (a == null || b == null || a.Length != b.Length) return false; for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false; return true; }
        public static bool equals(float[] a, float[] b) { if (a == b) return true; if (a == null || b == null || a.Length != b.Length) return false; for (int i = 0; i < a.Length; i++) if (java.lang.Float.floatToIntBits(a[i]) != java.lang.Float.floatToIntBits(b[i])) return false; return true; }
        public static bool equals(object[] a, object[] b) { if (a == b) return true; if (a == null || b == null || a.Length != b.Length) return false; for (int i = 0; i < a.Length; i++) if (!AbstractCollection.Eq(a[i], b[i])) return false; return true; }
        public static int hashCode(int[] a) { if (a == null) return 0; int h = 1; unchecked { foreach (var v in a) h = 31 * h + v; } return h; }
        public static int hashCode(float[] a) { if (a == null) return 0; int h = 1; unchecked { foreach (var v in a) h = 31 * h + java.lang.Float.floatToIntBits(v); } return h; }
        public static bool deepEquals(object[] a, object[] b)
        {
            if (a == b) return true;
            if (a == null || b == null || a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                object x = a[i], y = b[i];
                if (x == y) continue;
                if (x == null || y == null) return false;
                bool eq = x is object[] xa && y is object[] ya ? deepEquals(xa, ya)
                    : x is int[] xi && y is int[] yi ? equals(xi, yi)
                    : x is float[] xf && y is float[] yf ? equals(xf, yf)
                    : AbstractCollection.Eq(x, y);
                if (!eq) return false;
            }
            return true;
        }
        public static int deepHashCode(object[] a)
        {
            if (a == null) return 0;
            int h = 1;
            unchecked
            {
                foreach (var e in a)
                {
                    int eh = e is object[] ea ? deepHashCode(ea) : e is int[] ei ? hashCode(ei) : e is float[] ef ? hashCode(ef) : JO.hashCode(e);
                    h = 31 * h + eh;
                }
            }
            return h;
        }
        public static int hashCode(object[] a) { if (a == null) return 0; int h = 1; unchecked { foreach (var v in a) h = 31 * h + JO.hashCode(v); } return h; }
        public static string toString(int[] a) => a == null ? "null" : "[" + string.Join(", ", a) + "]";
        public static string toString(float[] a) => a == null ? "null" : "[" + string.Join(", ", Array.ConvertAll(a, java.lang.Float.toString)) + "]";
        public static string toString(object[] a) => a == null ? "null" : "[" + string.Join(", ", Array.ConvertAll(a, java.lang.JStr.valueOf)) + "]";
        /// <summary>Fixed-size list view writing through to the array (like Java).</summary>
        public static List asList(params object[] a) => new ArrayView(a);
        public static java.util.stream.Stream stream(object[] a) => new java.util.stream.Stream(a);
        public static java.util.stream.IntStream stream(int[] a) => new java.util.stream.IntStream(a);
        public static int binarySearch(int[] a, int key) { int lo = 0, hi = a.Length - 1; while (lo <= hi) { int mid = (int)((uint)(lo + hi) >> 1); if (a[mid] < key) lo = mid + 1; else if (a[mid] > key) hi = mid - 1; else return mid; } return -(lo + 1); }
        sealed class ArrayView : AbstractList, RandomAccess
        {
            readonly object[] a;
            public ArrayView(object[] a) { this.a = a; }
            public override int size() => a.Length;
            public override object get(int i) => a[i];
            public override object set(int i, object o) { var old = a[i]; a[i] = o; return old; }
            public override object[] toArray() => (object[])a.Clone();
        }
    }

    public static class List_
    {
        public static List of(params object[] items) { var l = new ArrayList(items.Length); foreach (var o in items) l.add(o); return l; }
        public static List copyOf(Collection c) => new ArrayList(c);
    }
    public static class Set_
    {
        public static Set of(params object[] items) { var s = new LinkedHashSet(); foreach (var o in items) s.add(o); return s; }
        public static Set copyOf(Collection c) => new LinkedHashSet(c);
    }
    public static class Map_
    {
        /// <summary>Map.of: insertion-ordered here (Java's immutable map order is randomized per JVM run, so nothing deterministic can depend on it).</summary>
        public static Map of(params object[] kv) { var m = new LinkedHashMap(); for (int i = 0; i < kv.Length; i += 2) m.put(kv[i], kv[i + 1]); return m; }
        public static Map.Entry entry(object k, object v) => new SimpleEntry(k, v);
        public static Map copyOf(Map m) => new LinkedHashMap(m);
    }

    public static class Objects
    {
        public static object requireNonNullElseGet(object o, java.util.function.Supplier s) => o ?? s.get();
        public static object requireNonNullElse(object o, object d) => o ?? d;
        public static int checkFromIndexSize(int fromIndex, int size, int length)
        {
            if ((length | fromIndex | size) < 0 || size > length - fromIndex) throw new System.IndexOutOfRangeException("Range [" + fromIndex + ", " + fromIndex + " + " + size + ") out of bounds for length " + length);
            return fromIndex;
        }

        public static bool equals(object a, object b) => a == b || (a != null && b != null && JO.equals(a, b)) || (a == null && b == null);
        public static int hashCode(object o) => o == null ? 0 : JO.hashCode(o);
        public static int hash(params object[] values) => Arrays.hashCode(values);
        public static object requireNonNull(object o) { if (o == null) throw new NullReferenceException(); return o; }
        public static object requireNonNull(object o, string msg) { if (o == null) throw new NullReferenceException(msg); return o; }
        public static bool isNull(object o) => o == null;
        public static bool nonNull(object o) => o != null;
        public static string toString(object o) => java.lang.JStr.valueOf(o);
        public static string toString(object o, string d) => o == null ? d : java.lang.JStr.valueOf(o);
        public static int checkIndex(int i, int n) { if (i < 0 || i >= n) throw new IndexOutOfRangeException(); return i; }
    }

    public sealed class Optional : java.lang.Object
    {
        readonly object v;
        Optional(object v) { this.v = v; }
        static readonly Optional EMPTY = new Optional(null);
        public static Optional of(object v) { if (v == null) throw new NullReferenceException(); return new Optional(v); }
        public static Optional ofNullable(object v) => v == null ? EMPTY : new Optional(v);
        public static Optional empty() => EMPTY;
        public bool isPresent() => v != null;
        public bool isEmpty() => v == null;
        public object get() { if (v == null) throw new InvalidOperationException("NoSuchElement"); return v; }
        public object orElseThrow() => get();
        public object orElseThrow(java.util.function.Supplier s) { if (v == null) throw (System.Exception)s.get(); return v; }
        public object orElse(object o) => v ?? o;
        public object orElseGet(java.util.function.Supplier s) => v ?? s.get();
        public void ifPresent(java.util.function.Consumer c) { if (v != null) c.accept(v); }
        public Optional map(java.util.function.Function f) => v == null ? EMPTY : ofNullable(f.apply(v));
        public Optional filter(java.util.function.Predicate p) => v != null && p.test(v) ? this : EMPTY;
    }

    public sealed class OptionalInt : java.lang.Object
    {
        readonly bool has; readonly int v;
        OptionalInt(bool h, int v) { has = h; this.v = v; }
        public static OptionalInt of(int v) => new OptionalInt(true, v);
        public static OptionalInt empty() => new OptionalInt(false, 0);
        public bool isPresent() => has;
        public bool isEmpty() => !has;
        public int getAsInt() { if (!has) throw new InvalidOperationException("NoSuchElement"); return v; }
        public int orElse(int o) => has ? v : o;
        public int orElseThrow() => getAsInt();
    }

    public sealed class OptionalDouble : java.lang.Object
    {
        readonly bool has; readonly double v;
        OptionalDouble(bool h, double v) { has = h; this.v = v; }
        public static OptionalDouble of(double v) => new OptionalDouble(true, v);
        public static OptionalDouble empty() => new OptionalDouble(false, 0);
        public bool isPresent() => has;
        public double getAsDouble() { if (!has) throw new InvalidOperationException("NoSuchElement"); return v; }
        public double orElse(double o) => has ? v : o;
    }

    /// <summary>java.util.Random, bit-exact (48-bit LCG).</summary>
    public class Random : java.lang.Object
    {
        const long Multiplier = 0x5DEECE66DL;
        const long Addend = 0xBL;
        const long Mask = (1L << 48) - 1;
        long seed;
        double nextNextGaussian;
        bool haveNextNextGaussian;
        static long seedUniquifier = 8682522807148012L;

        public Random() : this(NextSeedUniquifier() ^ DateTime.UtcNow.Ticks) { }
        public Random(long seed) { setSeed(seed); }
        static long NextSeedUniquifier() { unchecked { seedUniquifier *= 1181783497276652981L; return seedUniquifier; } }

        public virtual void setSeed(long s)
        {
            seed = (s ^ Multiplier) & Mask;
            haveNextNextGaussian = false;
        }

        /// <summary>Raw 48-bit state (for conformance checks against java.util.Random.seed).</summary>
        public long getRawSeed() => seed;

        protected virtual int next(int bits)
        {
            unchecked { seed = (seed * Multiplier + Addend) & Mask; }
            return (int)(long)((ulong)seed >> (48 - bits));
        }

        public virtual int nextInt() => next(32);

        public virtual int nextInt(int bound)
        {
            if (bound <= 0) throw new ArgumentException("bound must be positive");
            int r = next(31);
            int m = bound - 1;
            if ((bound & m) == 0) return (int)((bound * (long)r) >> 31);
            for (int u = r; u - (r = u % bound) + m < 0; u = next(31)) { }
            return r;
        }

        public virtual int nextInt(int origin, int bound)
        {
            if (origin >= bound) throw new ArgumentException("bound must be greater than origin");
            int r = nextInt();
            int n = bound - origin, m = n - 1;
            if ((n & m) == 0) r = (r & m) + origin;
            else if (n > 0)
            {
                for (int u = (int)((uint)r >> 1); u + m - (r = u % n) < 0; u = (int)((uint)nextInt() >> 1)) { }
                r += origin;
            }
            else { while (r < origin || r >= bound) r = nextInt(); }
            return r;
        }

        public virtual long nextLong() { unchecked { return ((long)next(32) << 32) + next(32); } }
        public virtual bool nextBoolean() => next(1) != 0;
        public virtual float nextFloat() => next(24) / ((float)(1 << 24));
        public virtual double nextDouble() => (((long)next(26) << 27) + next(27)) * (1.0 / (1L << 53));

        public virtual double nextGaussian()
        {
            if (haveNextNextGaussian) { haveNextNextGaussian = false; return nextNextGaussian; }
            double v1, v2, s;
            do
            {
                v1 = 2 * nextDouble() - 1;
                v2 = 2 * nextDouble() - 1;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1 || s == 0);
            double multiplier = System.Math.Sqrt(-2 * System.Math.Log(s) / s); // Java uses StrictMath.sqrt/log
            nextNextGaussian = v2 * multiplier;
            haveNextNextGaussian = true;
            return v1 * multiplier;
        }

        public virtual void nextBytes(sbyte[] bytes)
        {
            for (int i = 0, len = bytes.Length; i < len;)
                for (int rnd = nextInt(), n = System.Math.Min(len - i, 4); n-- > 0; rnd >>= 8)
                    bytes[i++] = unchecked((sbyte)rnd);
        }
    }

    public sealed class Locale : java.lang.Object
    {
        readonly string _lang, _country, _variant;
        public Locale() : this("", "", "") { }
        public Locale(string lang) : this(lang, "", "") { }
        public Locale(string lang, string country) : this(lang, country, "") { }
        public Locale(string lang, string country, string variant) { _lang = (lang ?? "").ToLowerInvariant(); _country = (country ?? "").ToUpperInvariant(); _variant = variant ?? ""; }
        public static readonly Locale ROOT = new Locale(), ENGLISH = new Locale("en"), US = new Locale("en", "US");
        static Locale s_default = ROOT;
        public static Locale getDefault() => s_default;
        public static void setDefault(Locale l) { s_default = l ?? ROOT; }
        public static Locale of(string lang) => new Locale(lang);
        public static Locale of(string lang, string country) => new Locale(lang, country);
        public static Locale of(string lang, string country, string variant) => new Locale(lang, country, variant);
        public string getLanguage() => _lang;
        public string getCountry() => _country;
        public string getVariant() => _variant;
        public string getDisplayLanguage() => _lang;
        public override string toString() => _lang + (_country.Length > 0 || _variant.Length > 0 ? "_" + _country : "") + (_variant.Length > 0 ? "_" + _variant : "");
        public override bool equals(object o) => o is Locale l && l._lang == _lang && l._country == _country && l._variant == _variant;
        public override int hashCode() => (_lang + "_" + _country + "_" + _variant).GetHashCode();
    }

    public sealed class UUID : java.lang.Object
    {
        readonly System.Guid _g;
        UUID(System.Guid g) { _g = g; }
        public static UUID randomUUID() => new UUID(System.Guid.NewGuid());
        public static UUID fromString(string s) => new UUID(System.Guid.Parse(s));
        public override string toString() => _g.ToString("D");
        public override bool equals(object o) => o is UUID u && u._g == _g;
        public override int hashCode() => _g.GetHashCode();
    }

    public sealed class BitSet : java.lang.Object
    {
        bool[] b;
        public BitSet() { b = new bool[64]; }
        public BitSet(int n) { b = new bool[System.Math.Max(n, 1)]; }
        void Ensure(int i) { if (i >= b.Length) Array.Resize(ref b, System.Math.Max(i + 1, b.Length * 2)); }
        public bool get(int i) => i < b.Length && b[i];
        public void set(int i) { Ensure(i); b[i] = true; }
        public void set(int i, bool v) { Ensure(i); b[i] = v; }
        public void clear(int i) { if (i < b.Length) b[i] = false; }
        public void clear() => Array.Clear(b, 0, b.Length);
        public int cardinality() { int n = 0; foreach (var x in b) if (x) n++; return n; }
        public int nextSetBit(int f) { for (int i = System.Math.Max(f, 0); i < b.Length; i++) if (b[i]) return i; return -1; }
        public int nextClearBit(int f) { for (int i = f; ; i++) if (i >= b.Length || !b[i]) return i; }
        public bool isEmpty() => nextSetBit(0) < 0;
    }
}

namespace java.util.function
{
    public interface Predicate { bool test(object o); }
    public sealed class Predicate_L : java.lang.Object, Predicate { readonly Func<object, bool> f; public Predicate_L(Func<object, bool> f) { this.f = f; } public bool test(object o) => f(o); }

    public interface BiPredicate { bool test(object a, object b); }
    public sealed class BiPredicate_L : java.lang.Object, BiPredicate { readonly Func<object, object, bool> f; public BiPredicate_L(Func<object, object, bool> f) { this.f = f; } public bool test(object a, object b) => f(a, b); }
    public interface IntPredicate { bool test(int i); }
    public sealed class IntPredicate_L : java.lang.Object, IntPredicate { readonly Func<int, bool> f; public IntPredicate_L(Func<int, bool> f) { this.f = f; } public bool test(int i) => f(i); }
    public interface Function { object apply(object o); }
    public sealed class Function_L : java.lang.Object, Function { readonly Func<object, object> f; public Function_L(Func<object, object> f) { this.f = f; } public object apply(object o) => f(o); }
    public interface UnaryOperator : Function { }
    public sealed class UnaryOperator_L : java.lang.Object, UnaryOperator { readonly Func<object, object> f; public UnaryOperator_L(Func<object, object> f) { this.f = f; } public object apply(object o) => f(o); }
    public interface BiFunction { object apply(object a, object b); }
    public sealed class BiFunction_L : java.lang.Object, BiFunction { readonly Func<object, object, object> f; public BiFunction_L(Func<object, object, object> f) { this.f = f; } public object apply(object a, object b) => f(a, b); }
    public interface BinaryOperator : BiFunction { }
    public sealed class BinaryOperator_L : java.lang.Object, BinaryOperator { readonly Func<object, object, object> f; public BinaryOperator_L(Func<object, object, object> f) { this.f = f; } public object apply(object a, object b) => f(a, b); }
    public interface Consumer { void accept(object o); }
    public sealed class Consumer_L : java.lang.Object, Consumer { readonly Action<object> f; public Consumer_L(Action<object> f) { this.f = f; } public void accept(object o) => f(o); }
    public interface BiConsumer { void accept(object a, object b); }
    public sealed class BiConsumer_L : java.lang.Object, BiConsumer { readonly Action<object, object> f; public BiConsumer_L(Action<object, object> f) { this.f = f; } public void accept(object a, object b) => f(a, b); }
    public interface IntConsumer { void accept(int i); }
    public sealed class IntConsumer_L : java.lang.Object, IntConsumer { readonly Action<int> f; public IntConsumer_L(Action<int> f) { this.f = f; } public void accept(int i) => f(i); }
    public interface ObjIntConsumer { void accept(object o, int i); }
    public sealed class ObjIntConsumer_L : java.lang.Object, ObjIntConsumer { readonly Action<object, int> f; public ObjIntConsumer_L(Action<object, int> f) { this.f = f; } public void accept(object o, int i) => f(o, i); }
    public interface Supplier { object get(); }
    public sealed class Supplier_L : java.lang.Object, Supplier { readonly Func<object> f; public Supplier_L(Func<object> f) { this.f = f; } public object get() => f(); }
    public interface BooleanSupplier { bool getAsBoolean(); }
    public sealed class BooleanSupplier_L : java.lang.Object, BooleanSupplier { readonly Func<bool> f; public BooleanSupplier_L(Func<bool> f) { this.f = f; } public bool getAsBoolean() => f(); }
    public interface IntSupplier { int getAsInt(); }
    public sealed class IntSupplier_L : java.lang.Object, IntSupplier { readonly Func<int> f; public IntSupplier_L(Func<int> f) { this.f = f; } public int getAsInt() => f(); }
    public interface IntFunction { object apply(int i); }
    public sealed class IntFunction_L : java.lang.Object, IntFunction { readonly Func<int, object> f; public IntFunction_L(Func<int, object> f) { this.f = f; } public object apply(int i) => f(i); }
    public interface IntUnaryOperator { int applyAsInt(int i); }
    public sealed class IntUnaryOperator_L : java.lang.Object, IntUnaryOperator { readonly Func<int, int> f; public IntUnaryOperator_L(Func<int, int> f) { this.f = f; } public int applyAsInt(int i) => f(i); }
    public interface IntBinaryOperator { int applyAsInt(int a, int b); }
    public sealed class IntBinaryOperator_L : java.lang.Object, IntBinaryOperator { readonly Func<int, int, int> f; public IntBinaryOperator_L(Func<int, int, int> f) { this.f = f; } public int applyAsInt(int a, int b) => f(a, b); }
    public interface ToIntFunction { int applyAsInt(object o); }
    public sealed class ToIntFunction_L : java.lang.Object, ToIntFunction { readonly Func<object, int> f; public ToIntFunction_L(Func<object, int> f) { this.f = f; } public int applyAsInt(object o) => f(o); }
    public interface ToLongFunction { long applyAsLong(object o); }
    public sealed class ToLongFunction_L : java.lang.Object, ToLongFunction { readonly Func<object, long> f; public ToLongFunction_L(Func<object, long> f) { this.f = f; } public long applyAsLong(object o) => f(o); }
    public interface ToDoubleFunction { double applyAsDouble(object o); }
    public sealed class ToDoubleFunction_L : java.lang.Object, ToDoubleFunction { readonly Func<object, double> f; public ToDoubleFunction_L(Func<object, double> f) { this.f = f; } public double applyAsDouble(object o) => f(o); }
    public interface IntToDoubleFunction { double applyAsDouble(int i); }
    public interface DoubleBinaryOperator { double applyAsDouble(double a, double b); }
    public sealed class DoubleBinaryOperator_L : java.lang.Object, DoubleBinaryOperator { readonly Func<double, double, double> f; public DoubleBinaryOperator_L(Func<double, double, double> f) { this.f = f; } public double applyAsDouble(double a, double b) => f(a, b); }
    public sealed class IntToDoubleFunction_L : java.lang.Object, IntToDoubleFunction { readonly Func<int, double> f; public IntToDoubleFunction_L(Func<int, double> f) { this.f = f; } public double applyAsDouble(int i) => f(i); }
    public interface DoubleUnaryOperator { double applyAsDouble(double d); }
    public sealed class DoubleUnaryOperator_L : java.lang.Object, DoubleUnaryOperator { readonly Func<double, double> f; public DoubleUnaryOperator_L(Func<double, double> f) { this.f = f; } public double applyAsDouble(double d) => f(d); }

    public static class Function_
    {
        public static Function identity() => new Function_L(o => o);
    }
}

namespace java.lang
{
    public sealed class StringBuilder : Object, CharSequence
    {
        readonly System.Text.StringBuilder sb;
        public StringBuilder() { sb = new System.Text.StringBuilder(); }
        public StringBuilder(int cap) { sb = new System.Text.StringBuilder(cap); }
        public StringBuilder(string s) { sb = new System.Text.StringBuilder(s); }
        public StringBuilder append(object o) { sb.Append(JStr.valueOf(o)); return this; }
        public StringBuilder append(string s) { sb.Append(s ?? "null"); return this; }
        public StringBuilder append(char c) { sb.Append(c); return this; }
        public StringBuilder append(int i) { sb.Append(i); return this; }
        public StringBuilder append(long l) { sb.Append(l); return this; }
        public StringBuilder append(float f) { sb.Append(Float.toString(f)); return this; }
        public StringBuilder append(double d) { sb.Append(Double.toString(d)); return this; }
        public StringBuilder append(bool b) { sb.Append(b ? "true" : "false"); return this; }
        public StringBuilder append(char[] c) { sb.Append(c); return this; }
        public StringBuilder insert(int i, object o) { sb.Insert(i, JStr.valueOf(o)); return this; }
        public int length() => sb.Length;
        public char charAt(int i) => sb[i];
        public void setLength(int n) => sb.Length = n;
        public void setCharAt(int i, char c) => sb[i] = c;
        public StringBuilder reverse() { var a = sb.ToString().ToCharArray(); System.Array.Reverse(a); sb.Clear().Append(a); return this; }
        public StringBuilder deleteCharAt(int i) { sb.Remove(i, 1); return this; }
        public StringBuilder delete(int a, int b) { sb.Remove(a, System.Math.Min(b, sb.Length) - a); return this; }
        public int indexOf(string s) => sb.ToString().IndexOf(s, System.StringComparison.Ordinal);
        public bool isEmpty() => sb.Length == 0;
        public override string toString() => sb.ToString();
        // CharSequence is string in the port: a StringBuilder passed where Java takes a CharSequence is converted
        public static implicit operator string(StringBuilder b) => b?.sb.ToString();
        public string subSequence(int a, int b) => sb.ToString(a, b - a);
        public string substring(int a) => sb.ToString(a, sb.Length - a);
        public string substring(int a, int b) => sb.ToString(a, b - a);
        public StringBuilder insert(int i, string s) { sb.Insert(i, s ?? "null"); return this; }
        public StringBuilder insert(int i, char c) { sb.Insert(i, c); return this; }
        public StringBuilder replace(int a, int b, string s) { sb.Remove(a, System.Math.Min(b, sb.Length) - a); sb.Insert(a, s); return this; }
        public int indexOf(string s, int from) => from >= sb.Length ? (s.Length == 0 ? sb.Length : -1) : sb.ToString().IndexOf(s, System.Math.Max(from, 0), System.StringComparison.Ordinal);
        public int lastIndexOf(string s) => sb.ToString().LastIndexOf(s, System.StringComparison.Ordinal);
        public int lastIndexOf(string s, int from)
        {
            var str = sb.ToString();
            if (from < 0) return -1;
            int start = System.Math.Min(from, str.Length - s.Length);
            for (int i = start; i >= 0; i--) if (string.CompareOrdinal(str, i, s, 0, s.Length) == 0) return i;
            return -1;
        }
        public StringBuilder appendCodePoint(int cp) { sb.Append(char.ConvertFromUtf32(cp)); return this; }
        public java.util.stream.IntStream codePoints() => J2CS.Ext.JStringExt.codePoints(sb.ToString());
        public java.util.stream.IntStream chars() => new java.util.stream.IntStream(System.Linq.Enumerable.Select(sb.ToString(), c => (int)c));
    }
}
