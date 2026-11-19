// java.util for J2CS-translated code: erased (object-typed) collections with Java method names and Java iteration order.
using System;
using J2CS.Ext;
using System.Collections;
using System.Collections.Generic;
using JO = java.lang.JO;

namespace java.util
{
    public interface Iterator { bool hasNext(); object next(); void remove(); }
    public interface ListIterator : Iterator { bool hasPrevious(); object previous(); int nextIndex(); void set(object o); void add(object o); }

    public interface Collection : java.lang.Iterable, IEnumerable
    {
        int size();
        bool isEmpty();
        bool contains(object o);
        bool add(object o);
        bool remove(object o);
        bool addAll(Collection c);
        bool removeAll(Collection c);
        bool retainAll(Collection c);
        bool containsAll(Collection c);
        void clear();
        object[] toArray();
        object[] toArray(object[] a);
        object[] toArray(java.util.function.IntFunction gen);
        java.util.stream.Stream stream();
        void forEach(java.util.function.Consumer c);
        bool removeIf(java.util.function.Predicate p);
    }

    public interface List : Collection
    {
        object get(int i);
        object set(int i, object o);
        void add(int i, object o);
        object remove(int i);
        int indexOf(object o);
        int lastIndexOf(object o);
        List subList(int a, int b);
        void sort(Comparator c);
        object getFirst();
        object getLast();
        object removeFirst();
        object removeLast();
        ListIterator listIterator();
    }

    public interface Set : Collection { }
    public interface SortedSet : Set { object first(); object last(); }
    public interface NavigableSet : SortedSet { }

    public interface Queue : Collection
    {
        bool offer(object o);
        object poll();
        object peek();
        object element();
        object remove();
    }

    public interface Deque : Queue
    {
        void addFirst(object o);
        void addLast(object o);
        bool offerFirst(object o);
        bool offerLast(object o);
        object pollFirst();
        object pollLast();
        object peekFirst();
        object peekLast();
        object getFirst();
        object getLast();
        object removeFirst();
        object removeLast();
        void push(object o);
        object pop();
        Iterator descendingIterator();
    }

    public interface Map
    {
        int size();
        bool isEmpty();
        object get(object k);
        object put(object k, object v);
        object remove(object k);
        bool containsKey(object k);
        bool containsValue(object v);
        Set keySet();
        Collection values();
        Set entrySet();
        object getOrDefault(object k, object d);
        object putIfAbsent(object k, object v);
        object computeIfAbsent(object k, java.util.function.Function f);
        object computeIfPresent(object k, java.util.function.BiFunction f);
        object compute(object k, java.util.function.BiFunction f);
        object merge(object k, object v, java.util.function.BiFunction f);
        void forEach(java.util.function.BiConsumer c);
        void putAll(Map m);
        void clear();

        public interface Entry { object getKey(); object getValue(); object setValue(object v); }
    }

    public interface SortedMap : Map { object firstKey(); object lastKey(); }
    public interface NavigableMap : SortedMap { }

    public interface Comparator { int compare(object a, object b); }
    public sealed class Comparator_L : java.lang.Object, Comparator
    {
        readonly Func<object, object, int> f;
        public Comparator_L(Func<object, object, int> f) { this.f = f; }
        public int compare(object a, object b) => f(a, b);
    }



    /// <summary>Static methods of java.util.Comparator.</summary>
    public static class Comparator_
    {
        public static Comparator comparing(java.util.function.Function key) => new Comparator_L((a, b) => ((java.lang.Comparable)key.apply(a)).compareTo(key.apply(b)));
        public static Comparator comparing(java.util.function.Function key, Comparator c) => new Comparator_L((a, b) => c.compare(key.apply(a), key.apply(b)));
        public static Comparator comparingInt(java.util.function.ToIntFunction key) => new Comparator_L((a, b) => java.lang.Integer.compare(key.applyAsInt(a), key.applyAsInt(b)));
        public static Comparator comparingDouble(java.util.function.ToDoubleFunction key) => new Comparator_L((a, b) => java.lang.Double.compare(key.applyAsDouble(a), key.applyAsDouble(b)));
        public static Comparator comparingLong(java.util.function.ToLongFunction key) => new Comparator_L((a, b) => java.lang.Long.compare(key.applyAsLong(a), key.applyAsLong(b)));
        public static Comparator naturalOrder() => new Comparator_L(Natural);
        public static Comparator reverseOrder() => new Comparator_L((a, b) => Natural(b, a));
        internal static int Natural(object a, object b)
        {
            switch (a)
            {
                case java.lang.Comparable c: return c.compareTo(b);
                case int i: return java.lang.Integer.compare(i, (int)b);
                case long l: return java.lang.Long.compare(l, (long)b);
                case float f: return java.lang.Float.compare(f, (float)b);
                case double d: return java.lang.Double.compare(d, (double)b);
                case string s: return J2CS.Ext.JStringExt.compareTo(s, (string)b);
                case char ch: return ch - (char)b;
                case bool x: return java.lang.Boolean.compare(x, (bool)b);
                default: throw new InvalidCastException("not Comparable: " + a);
            }
        }
        internal static Comparison<object> Of(Comparator c) => c == null ? Natural : new Comparison<object>(c.compare);
    }

    // ------------------------------------------------------------------ base classes

    public abstract class AbstractCollection : java.lang.Object, Collection
    {
        public abstract int size();
        public abstract Iterator iterator();
        public virtual bool isEmpty() => size() == 0;
        public virtual bool contains(object o) { for (var it = iterator(); it.hasNext();) if (Eq(o, it.next())) return true; return false; }
        public virtual bool add(object o) => throw new NotSupportedException();
        public virtual bool remove(object o)
        {
            for (var it = iterator(); it.hasNext();) if (Eq(o, it.next())) { it.remove(); return true; }
            return false;
        }
        public virtual bool addAll(Collection c) { bool ch = false; foreach (var o in c.toArray()) ch |= add(o); return ch; }
        public virtual bool removeAll(Collection c) { bool ch = false; for (var it = iterator(); it.hasNext();) if (c.contains(it.next())) { it.remove(); ch = true; } return ch; }
        public virtual bool retainAll(Collection c) { bool ch = false; for (var it = iterator(); it.hasNext();) if (!c.contains(it.next())) { it.remove(); ch = true; } return ch; }
        public virtual bool containsAll(Collection c) { foreach (var o in c) if (!contains(o)) return false; return true; }
        public virtual void clear() { for (var it = iterator(); it.hasNext();) { it.next(); it.remove(); } }
        public virtual object[] toArray() { var a = new object[size()]; int i = 0; for (var it = iterator(); it.hasNext();) a[i++] = it.next(); return a; }
        public virtual object[] toArray(object[] a)
        {
            int n = size();
            var r = a.Length >= n ? a : (object[])Array.CreateInstance(a.GetType().GetElementType(), n);
            int i = 0;
            for (var it = iterator(); it.hasNext();) r[i++] = it.next();
            if (r.Length > n) r[n] = null;
            return r;
        }
        public virtual object[] toArray(java.util.function.IntFunction gen) => toArray((object[])gen.apply(0));
        public virtual java.util.stream.Stream stream() => new java.util.stream.Stream(Enumerate());
        public virtual void forEach(java.util.function.Consumer c) { foreach (var o in toArray()) c.accept(o); }
        public virtual bool removeIf(java.util.function.Predicate p) { bool ch = false; for (var it = iterator(); it.hasNext();) if (p.test(it.next())) { it.remove(); ch = true; } return ch; }
        IEnumerable<object> Enumerate() { for (var it = iterator(); it.hasNext();) yield return it.next(); }
        public IEnumerator GetEnumerator() { for (var it = iterator(); it.hasNext();) yield return it.next(); }
        public override string toString()
        {
            var sb = new System.Text.StringBuilder("[");
            bool first = true;
            for (var it = iterator(); it.hasNext();) { if (!first) sb.Append(", "); first = false; var o = it.next(); sb.Append(ReferenceEquals(o, this) ? "(this Collection)" : java.lang.JStr.valueOf(o)); }
            return sb.Append(']').ToString();
        }
        internal static bool Eq(object a, object b) => a == null ? b == null : b != null && JO.equals(a, b);
    }

    public abstract class AbstractList : AbstractCollection, List
    {
        protected internal int modCount;
        public abstract object get(int i);
        public virtual object set(int i, object o) => throw new NotSupportedException();
        public virtual void add(int i, object o) => throw new NotSupportedException();
        public virtual object remove(int i) => throw new NotSupportedException();
        public override bool add(object o) { add(size(), o); return true; }
        public virtual int indexOf(object o) { int n = size(); for (int i = 0; i < n; i++) if (Eq(o, get(i))) return i; return -1; }
        public virtual int lastIndexOf(object o) { for (int i = size() - 1; i >= 0; i--) if (Eq(o, get(i))) return i; return -1; }
        public override bool contains(object o) => indexOf(o) >= 0;
        public override bool remove(object o) { int i = indexOf(o); if (i < 0) return false; remove(i); return true; }
        public override Iterator iterator() => new Itr(this, 0);
        public virtual ListIterator listIterator() => new Itr(this, 0);
        public virtual ListIterator listIterator(int i) => new Itr(this, i);
        public virtual List subList(int a, int b) { var r = new ArrayList(); for (int i = a; i < b; i++) r.add(get(i)); return new SubList(this, a, b); }
        public virtual void sort(Comparator c)
        {
            var a = toArray();
            Arrays.sort(a, c);
            for (int i = 0; i < a.Length; i++) set(i, a[i]);
        }
        public virtual object getFirst() { if (size() == 0) throw new InvalidOperationException("NoSuchElement"); return get(0); }
        public virtual object getLast() { if (size() == 0) throw new InvalidOperationException("NoSuchElement"); return get(size() - 1); }
        public virtual object removeFirst() { if (size() == 0) throw new InvalidOperationException("NoSuchElement"); return remove(0); }
        public virtual object removeLast() { if (size() == 0) throw new InvalidOperationException("NoSuchElement"); return remove(size() - 1); }
        public virtual void addFirst(object o) => add(0, o);
        public virtual void addLast(object o) => add(o);
        public virtual List reversed() { var r = new ArrayList(this); Collections.reverse(r); return r; }
        public virtual void replaceAll(java.util.function.UnaryOperator op) { for (int i = 0; i < size(); i++) set(i, op.apply(get(i))); }
        public override void clear() { for (int i = size() - 1; i >= 0; i--) remove(i); }
        public override bool equals(object o)
        {
            if (ReferenceEquals(o, this)) return true;
            if (!(o is List l) || l.size() != size()) return false;
            for (int i = 0; i < size(); i++) if (!Eq(get(i), l.get(i))) return false;
            return true;
        }
        public override int hashCode() { int h = 1; unchecked { for (int i = 0; i < size(); i++) h = 31 * h + JO.hashCode(get(i)); } return h; }

        sealed class SubList : AbstractList
        {
            readonly AbstractList p; readonly int off; int n;
            public SubList(AbstractList p, int a, int b) { this.p = p; off = a; n = b - a; }
            public override int size() => n;
            public override object get(int i) { if (i < 0 || i >= n) throw new IndexOutOfRangeException(); return p.get(off + i); }
            public override object set(int i, object o) => p.set(off + i, o);
            public override void add(int i, object o) { p.add(off + i, o); n++; }
            public override object remove(int i) { n--; return p.remove(off + i); }
        }

        internal sealed class Itr : java.lang.Object, ListIterator
        {
            readonly AbstractList l; int cursor; int last = -1;
            public Itr(AbstractList l, int start) { this.l = l; cursor = start; }
            public bool hasNext() => cursor < l.size();
            public object next() { if (cursor >= l.size()) throw new InvalidOperationException("NoSuchElement"); last = cursor; return l.get(cursor++); }
            public bool hasPrevious() => cursor > 0;
            public object previous() { last = --cursor; return l.get(cursor); }
            public int nextIndex() => cursor;
            public void remove() { if (last < 0) throw new InvalidOperationException(); l.remove(last); if (last < cursor) cursor--; last = -1; }
            public void set(object o) => l.set(last, o);
            public void add(object o) { l.add(cursor++, o); last = -1; }
        }
    }

    public class ArrayList : AbstractList, java.util.RandomAccess
    {
        object[] a;
        int n;
        public ArrayList() { a = new object[10]; }
        public ArrayList(int cap) { if (cap < 0) throw new ArgumentException(); a = new object[System.Math.Max(cap, 1)]; }
        public ArrayList(Collection c) { var src = c.toArray(); a = new object[System.Math.Max(src.Length, 1)]; Array.Copy(src, a, src.Length); n = src.Length; }
        public override int size() => n;
        public override object get(int i) { if ((uint)i >= (uint)n) throw new IndexOutOfRangeException("Index " + i + " out of bounds for length " + n); return a[i]; }
        public override object set(int i, object o) { if ((uint)i >= (uint)n) throw new IndexOutOfRangeException(); var old = a[i]; a[i] = o; return old; }
        void Grow(int min) { if (min > a.Length) { var b = new object[System.Math.Max(min, a.Length + (a.Length >> 1) + 1)]; Array.Copy(a, b, n); a = b; } }
        public override bool add(object o) { modCount++; Grow(n + 1); a[n++] = o; return true; }
        public override void add(int i, object o)
        {
            if (i < 0 || i > n) throw new IndexOutOfRangeException();
            modCount++; Grow(n + 1); Array.Copy(a, i, a, i + 1, n - i); a[i] = o; n++;
        }
        public override object remove(int i)
        {
            if ((uint)i >= (uint)n) throw new IndexOutOfRangeException();
            modCount++; var old = a[i]; Array.Copy(a, i + 1, a, i, n - i - 1); a[--n] = null; return old;
        }
        public override void clear() { modCount++; Array.Clear(a, 0, n); n = 0; }
        public override bool addAll(Collection c) { var src = c.toArray(); Grow(n + src.Length); Array.Copy(src, 0, a, n, src.Length); n += src.Length; modCount++; return src.Length > 0; }
        public virtual bool addAll(int i, Collection c) { var src = c.toArray(); Grow(n + src.Length); Array.Copy(a, i, a, i + src.Length, n - i); Array.Copy(src, 0, a, i, src.Length); n += src.Length; return src.Length > 0; }
        public override object[] toArray() { var r = new object[n]; Array.Copy(a, r, n); return r; }
        public override void sort(Comparator c) { var r = toArray(); Arrays.sort(r, c); Array.Copy(r, a, n); modCount++; }
        public void ensureCapacity(int c) => Grow(c);
        public void trimToSize() { }
        public override bool removeIf(java.util.function.Predicate p)
        {
            int j = 0;
            for (int i = 0; i < n; i++) if (!p.test(a[i])) a[j++] = a[i];
            bool ch = j != n;
            for (int i = j; i < n; i++) a[i] = null;
            n = j;
            return ch;
        }
        public override int indexOf(object o) { for (int i = 0; i < n; i++) if (Eq(o, a[i])) return i; return -1; }
        public override void forEach(java.util.function.Consumer c) { int m = modCount; for (int i = 0; i < n; i++) { c.accept(a[i]); if (m != modCount) throw new InvalidOperationException("ConcurrentModification"); } }
    }

    public interface RandomAccess { }

    public class LinkedList : ArrayList, Deque
    {
        public LinkedList() { }
        public LinkedList(Collection c) : base(c) { }
        public bool offer(object o) => add(o);
        public object poll() => size() == 0 ? null : remove(0);
        public object peek() => size() == 0 ? null : get(0);
        public object element() => getFirst();
        public object remove() => removeFirst();
        public bool offerFirst(object o) { add(0, o); return true; }
        public bool offerLast(object o) => add(o);
        public object pollFirst() => poll();
        public object pollLast() => size() == 0 ? null : remove(size() - 1);
        public object peekFirst() => peek();
        public object peekLast() => size() == 0 ? null : get(size() - 1);
        public void push(object o) => add(0, o);
        public object pop() => removeFirst();
        public Iterator descendingIterator() { var r = new ArrayList(this); Collections.reverse(r); return r.iterator(); }
    }

    /// <summary>Circular-buffer deque (java.util.ArrayDeque semantics; nulls not allowed).</summary>
    public class ArrayDeque : AbstractCollection, Deque
    {
        object[] a = new object[16];
        int head, n;
        public ArrayDeque() { }
        public ArrayDeque(int cap) { a = new object[System.Math.Max(16, cap + 1)]; }
        public ArrayDeque(Collection c) { foreach (var o in c.toArray()) addLast(o); }
        public override int size() => n;
        void Grow() { if (n < a.Length) return; var b = new object[a.Length * 2]; for (int i = 0; i < n; i++) b[i] = a[(head + i) % a.Length]; a = b; head = 0; }
        public void addFirst(object o) { if (o == null) throw new NullReferenceException(); Grow(); head = (head - 1 + a.Length) % a.Length; a[head] = o; n++; }
        public void addLast(object o) { if (o == null) throw new NullReferenceException(); Grow(); a[(head + n) % a.Length] = o; n++; }
        public override bool add(object o) { addLast(o); return true; }
        public bool offer(object o) { addLast(o); return true; }
        public bool offerFirst(object o) { addFirst(o); return true; }
        public bool offerLast(object o) { addLast(o); return true; }
        public object pollFirst() { if (n == 0) return null; var o = a[head]; a[head] = null; head = (head + 1) % a.Length; n--; return o; }
        public object pollLast() { if (n == 0) return null; int i = (head + n - 1) % a.Length; var o = a[i]; a[i] = null; n--; return o; }
        public object peekFirst() => n == 0 ? null : a[head];
        public object peekLast() => n == 0 ? null : a[(head + n - 1) % a.Length];
        public object getFirst() { if (n == 0) throw new InvalidOperationException("NoSuchElement"); return peekFirst(); }
        public object getLast() { if (n == 0) throw new InvalidOperationException("NoSuchElement"); return peekLast(); }
        public object removeFirst() { if (n == 0) throw new InvalidOperationException("NoSuchElement"); return pollFirst(); }
        public object removeLast() { if (n == 0) throw new InvalidOperationException("NoSuchElement"); return pollLast(); }
        public object poll() => pollFirst();
        public object peek() => peekFirst();
        public object element() => getFirst();
        public object remove() => removeFirst();
        public void push(object o) => addFirst(o);
        public object pop() => removeFirst();
        public override void clear() { a = new object[16]; head = 0; n = 0; }
        object At(int i) => a[(head + i) % a.Length];
        void RemoveAt(int i)
        {
            for (int k = i; k < n - 1; k++) a[(head + k) % a.Length] = a[(head + k + 1) % a.Length];
            a[(head + n - 1) % a.Length] = null;
            n--;
        }
        public override Iterator iterator() => new DItr(this, false);
        public Iterator descendingIterator() => new DItr(this, true);
        sealed class DItr : java.lang.Object, Iterator
        {
            readonly ArrayDeque d; readonly bool desc; int i; int last = -1;
            public DItr(ArrayDeque d, bool desc) { this.d = d; this.desc = desc; i = desc ? d.n - 1 : 0; }
            public bool hasNext() => desc ? i >= 0 : i < d.n;
            public object next() { last = i; var o = d.At(i); i += desc ? -1 : 1; return o; }
            public void remove() { d.RemoveAt(last); if (!desc) i--; last = -1; }
        }
    }

    /// <summary>Binary heap with java.util.PriorityQueue's sift algorithms (same order for ties).</summary>
    public class PriorityQueue : AbstractCollection, Queue
    {
        object[] q = new object[11];
        int n;
        readonly Comparator cmp;
        public PriorityQueue() { }
        public PriorityQueue(int cap) { q = new object[System.Math.Max(1, cap)]; }
        public PriorityQueue(Comparator c) { cmp = c; }
        public PriorityQueue(int cap, Comparator c) { q = new object[System.Math.Max(1, cap)]; cmp = c; }
        int C(object a, object b) => cmp != null ? cmp.compare(a, b) : Comparator_.Natural(a, b);
        public override int size() => n;
        public override bool add(object o) => offer(o);
        public bool offer(object o)
        {
            if (o == null) throw new NullReferenceException();
            if (n >= q.Length) Array.Resize(ref q, q.Length < 64 ? q.Length * 2 + 2 : q.Length + (q.Length >> 1));
            SiftUp(n++, o);
            return true;
        }
        void SiftUp(int k, object x)
        {
            while (k > 0) { int p = (k - 1) >> 1; var e = q[p]; if (C(x, e) >= 0) break; q[k] = e; k = p; }
            q[k] = x;
        }
        void SiftDown(int k, object x)
        {
            int half = n >> 1;
            while (k < half)
            {
                int c = (k << 1) + 1; var co = q[c]; int r = c + 1;
                if (r < n && C(co, q[r]) > 0) co = q[c = r];
                if (C(x, co) <= 0) break;
                q[k] = co; k = c;
            }
            q[k] = x;
        }
        public object poll()
        {
            if (n == 0) return null;
            var res = q[0]; int s = --n; var x = q[s]; q[s] = null;
            if (s != 0) SiftDown(0, x);
            return res;
        }
        public object peek() => n == 0 ? null : q[0];
        public object element() { if (n == 0) throw new InvalidOperationException("NoSuchElement"); return q[0]; }
        public object remove() { if (n == 0) throw new InvalidOperationException("NoSuchElement"); return poll(); }
        public override bool remove(object o)
        {
            for (int i = 0; i < n; i++) if (Eq(o, q[i])) { RemoveAt(i); return true; }
            return false;
        }
        void RemoveAt(int i)
        {
            int s = --n;
            if (s == i) q[i] = null;
            else { var moved = q[s]; q[s] = null; SiftDown(i, moved); if (q[i] == moved) SiftUp(i, moved); }
        }
        public override void clear() { Array.Clear(q, 0, n); n = 0; }
        public override Iterator iterator() => new PItr(this);
        sealed class PItr : java.lang.Object, Iterator
        {
            readonly PriorityQueue p; int i; int last = -1;
            public PItr(PriorityQueue p) { this.p = p; }
            public bool hasNext() => i < p.n;
            public object next() { last = i; return p.q[i++]; }
            public void remove() { p.RemoveAt(last); i--; last = -1; }
        }
    }
}
