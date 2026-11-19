// java.util.stream subset (sequential, erased).
using System;
using J2CS.Ext;
using System.Collections.Generic;
using System.Linq;
using java.util.function;

namespace java.util.stream
{
    public class Stream : java.lang.Object
    {
        internal readonly IEnumerable<object> e;
        public Stream(IEnumerable<object> e) { this.e = e; }
        public Stream(object[] a) { e = a; }
        public java.util.Iterator iterator() { var l = new java.util.ArrayList(); foreach (var o in e) l.add(o); return l.iterator(); }
        public void close() { }
        public Stream filter(Predicate p) => new Stream(e.Where(p.test));
        public Stream map(Function f) => new Stream(e.Select(f.apply));
        public IntStream mapToInt(ToIntFunction f) => new IntStream(e.Select(f.applyAsInt));
        public DoubleStream mapToDouble(ToDoubleFunction f) => new DoubleStream(e.Select(f.applyAsDouble));
        public Stream flatMap(Function f) => new Stream(e.SelectMany(o => ((Stream)f.apply(o)).e));
        public bool anyMatch(Predicate p) => e.Any(p.test);
        public bool allMatch(Predicate p) => e.All(p.test);
        public bool noneMatch(Predicate p) => !e.Any(p.test);
        public long count() => e.LongCount();
        public object[] toArray() => e.ToArray();
        public object[] toArray(IntFunction gen)
        {
            var items = e.ToArray();
            var arr = (object[])gen.apply(items.Length);
            Array.Copy(items, arr, items.Length);
            return arr;
        }
        public List toList() { var l = new ArrayList(); foreach (var o in e) l.add(o); return l; }
        public object collect(Collector c) => c.f(e);
        public void forEach(Consumer c) { foreach (var o in e) c.accept(o); }
        public Stream sorted() { var a = e.ToArray(); Arrays.sort(a); return new Stream(a); }
        public Stream sorted(Comparator c) { var a = e.ToArray(); Arrays.sort(a, c); return new Stream(a); }
        public Optional min(Comparator c) { object best = null; bool any = false; foreach (var o in e) { if (!any || c.compare(o, best) < 0) best = o; any = true; } return any ? Optional.of(best) : Optional.empty(); }
        public Optional max(Comparator c) { object best = null; bool any = false; foreach (var o in e) { if (!any || c.compare(o, best) > 0) best = o; any = true; } return any ? Optional.of(best) : Optional.empty(); }
        public Optional findFirst() { foreach (var o in e) return Optional.of(o); return Optional.empty(); }
        public Optional findAny() => findFirst();
        public Stream distinct() { var s = new LinkedHashSet(); foreach (var o in e) s.add(o); return new Stream(s.toArray()); }
        public Stream limit(long n) => new Stream(e.Take((int)n));
        public Stream skip(long n) => new Stream(e.Skip((int)n));
        public Stream peek(Consumer c) => new Stream(e.Select(o => { c.accept(o); return o; }));
        public object reduce(object identity, BinaryOperator op) { var acc = identity; foreach (var o in e) acc = op.apply(acc, o); return acc; }
        public Optional reduce(BinaryOperator op) { object acc = null; bool any = false; foreach (var o in e) { acc = any ? op.apply(acc, o) : o; any = true; } return any ? Optional.of(acc) : Optional.empty(); }
    }

    public static class Stream_
    {
        public static Stream of(params object[] a) => new Stream(a);
        public static Stream empty() => new Stream(new object[0]);
        public static Stream concat(Stream a, Stream b) => new Stream(a.e.Concat(b.e));
        public static Stream generate(Supplier s) => new Stream(Gen(s));
        static IEnumerable<object> Gen(Supplier s) { while (true) yield return s.get(); }
    }

    public class IntStream : java.lang.Object
    {
        internal readonly IEnumerable<int> e;
        public IntStream(IEnumerable<int> e) { this.e = e; }
        public int sum() { int s = 0; unchecked { foreach (var i in e) s += i; } return s; }
        public long count() => e.LongCount();
        public OptionalInt max() { bool any = false; int m = 0; foreach (var i in e) { if (!any || i > m) m = i; any = true; } return any ? OptionalInt.of(m) : OptionalInt.empty(); }
        public OptionalInt min() { bool any = false; int m = 0; foreach (var i in e) { if (!any || i < m) m = i; any = true; } return any ? OptionalInt.of(m) : OptionalInt.empty(); }
        public OptionalDouble average() { long s = 0; int n = 0; foreach (var i in e) { s += i; n++; } return n == 0 ? OptionalDouble.empty() : OptionalDouble.of((double)s / n); }
        public IntStream map(IntUnaryOperator f) => new IntStream(e.Select(f.applyAsInt));
        public IntStream filter(IntPredicate p) => new IntStream(e.Where(p.test));
        public Stream mapToObj(IntFunction f) => new Stream(e.Select(f.apply));
        public DoubleStream mapToDouble(IntToDoubleFunction f) => new DoubleStream(e.Select(f.applyAsDouble));
        public Stream boxed() => new Stream(e.Select(i => (object)i));
        public int[] toArray() => e.ToArray();
        public bool anyMatch(IntPredicate p) => e.Any(p.test);
        public bool allMatch(IntPredicate p) => e.All(p.test);
        public bool noneMatch(IntPredicate p) => !e.Any(p.test);
        public void forEach(IntConsumer c) { foreach (var i in e) c.accept(i); }
        public OptionalInt findFirst() { foreach (var i in e) return OptionalInt.of(i); return OptionalInt.empty(); }
        public IntStream sorted() => new IntStream(e.OrderBy(i => i));
        public IntStream distinct() => new IntStream(e.Distinct());
        public IntStream limit(long n) => new IntStream(e.Take((int)n));
        public int reduce(int identity, IntBinaryOperator op) { int acc = identity; foreach (var i in e) acc = op.applyAsInt(acc, i); return acc; }
        public DoubleStream asDoubleStream() => new DoubleStream(e.Select(i => (double)i));
        public LongStream asLongStream() => new LongStream(e.Select(i => (long)i));
    }

    public class LongStream : java.lang.Object
    {
        internal readonly IEnumerable<long> e;
        public LongStream(IEnumerable<long> e) { this.e = e; }
        public long sum() { long s = 0; unchecked { foreach (var i in e) s += i; } return s; }
        public long count() => e.LongCount();
        public long[] toArray() => e.ToArray();
    }

    public static class IntStream_
    {
        public static IntStream range(int a, int b) => new IntStream(a >= b ? Enumerable.Empty<int>() : Enumerable.Range(a, b - a));
        public static IntStream rangeClosed(int a, int b) => new IntStream(a > b ? Enumerable.Empty<int>() : Enumerable.Range(a, b - a + 1));
        public static IntStream of(params int[] v) => new IntStream(v);
    }

    public class DoubleStream : java.lang.Object
    {
        internal readonly IEnumerable<double> e;
        public DoubleStream(IEnumerable<double> e) { this.e = e; }
        public double sum() { double s = 0; foreach (var d in e) s += d; return s; }
        public OptionalDouble max() { bool any = false; double m = 0; foreach (var d in e) { if (!any || d > m) m = d; any = true; } return any ? OptionalDouble.of(m) : OptionalDouble.empty(); }
        public OptionalDouble average() { double s = 0; int n = 0; foreach (var d in e) { s += d; n++; } return n == 0 ? OptionalDouble.empty() : OptionalDouble.of(s / n); }
        public double[] toArray() => e.ToArray();
        public double reduce(double identity, DoubleBinaryOperator op) { double acc = identity; foreach (var d in e) acc = op.applyAsDouble(acc, d); return acc; }
        public long count() => e.LongCount();
    }

    public sealed class Collector : java.lang.Object
    {
        internal readonly Func<IEnumerable<object>, object> f;
        internal Collector(Func<IEnumerable<object>, object> f) { this.f = f; }
    }

    public static class Collectors
    {
        public static Collector toList() => new Collector(e => { var l = new ArrayList(); foreach (var o in e) l.add(o); return l; });
        public static Collector toSet() => new Collector(e => { var s = new HashSet(); foreach (var o in e) s.add(o); return s; });
        public static Collector toMap(Function k, Function v) => new Collector(e => { var m = new HashMap(); foreach (var o in e) m.put(k.apply(o), v.apply(o)); return m; });
        public static Collector joining(string sep) => new Collector(e => string.Join(sep, e.Select(java.lang.JStr.valueOf)));
        public static Collector joining() => joining("");
        public static Collector joining(string sep, string prefix, string suffix) => new Collector(e => prefix + string.Join(sep, e.Select(java.lang.JStr.valueOf)) + suffix);
        public static Collector toCollection(Supplier s) => new Collector(e => { var c = (Collection)s.get(); foreach (var o in e) c.add(o); return c; });
        public static Collector counting() => new Collector(e => (long?)e.LongCount());
        public static Collector groupingBy(Function k, Supplier mapFactory, Collector downstream) => new Collector(e =>
        {
            var groups = new System.Collections.Generic.List<KeyValuePair<object, List<object>>>();
            var index = new System.Collections.Generic.Dictionary<object, int>(JavaEq.Instance);
            foreach (var o in e)
            {
                var key = k.apply(o);
                if (!index.TryGetValue(key ?? s_nullKey, out int i)) { i = groups.Count; index[key ?? s_nullKey] = i; groups.Add(new KeyValuePair<object, List<object>>(key, new List<object>())); }
                groups[i].Value.Add(o);
            }
            var m = (Map)mapFactory.get();
            foreach (var g in groups) m.put(g.Key, downstream.f(g.Value));
            return m;
        });
        static readonly object s_nullKey = new object();
        sealed class JavaEq : IEqualityComparer<object>
        {
            public static readonly JavaEq Instance = new JavaEq();
            public new bool Equals(object a, object b) => a == b || (a != null && b != null && java.lang.JO.equals(a, b));
            public int GetHashCode(object o) => o == null ? 0 : java.lang.JO.hashCode(o);
        }
        public static Collector groupingBy(Function k) => new Collector(e => { var m = new HashMap(); foreach (var o in e) ((List)m.computeIfAbsent(k.apply(o), new Function_L(_ => new ArrayList()))).add(o); return m; });
    }

    public static class StreamSupport
    {
        public static Stream stream(object spliteratorOrIterable, bool parallel) => new Stream(java.lang.JIter.of(spliteratorOrIterable).Cast<object>());
    }
}
