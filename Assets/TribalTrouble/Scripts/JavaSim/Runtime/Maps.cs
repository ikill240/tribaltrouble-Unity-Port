// java.util maps and sets with Java's iteration order.
using System;
using J2CS.Ext;
using System.Collections;
using System.Collections.Generic;
using JO = java.lang.JO;

namespace java.util
{
    public abstract class AbstractMap : java.lang.Object, Map
    {
        public abstract Set entrySet();
        public virtual int size() => entrySet().size();
        public virtual bool isEmpty() => size() == 0;
        public abstract object get(object k);
        public abstract object put(object k, object v);
        public abstract object remove(object k);
        public abstract bool containsKey(object k);
        public virtual bool containsValue(object v) { foreach (Map.Entry e in entrySet()) if (AbstractCollection.Eq(v, e.getValue())) return true; return false; }
        public virtual Set keySet() => new KeyView(this);
        public virtual Collection values() => new ValueView(this);
        public virtual object getOrDefault(object k, object d) { var v = get(k); return v != null || containsKey(k) ? v : d; }
        public virtual object putIfAbsent(object k, object v) { var cur = get(k); if (cur == null) cur = put(k, v); return cur; }
        public virtual object computeIfAbsent(object k, java.util.function.Function f)
        {
            var v = get(k);
            if (v == null) { v = f.apply(k); if (v != null) put(k, v); }
            return v;
        }
        public virtual object computeIfPresent(object k, java.util.function.BiFunction f)
        {
            var old = get(k);
            if (old == null) return null;
            var v = f.apply(k, old);
            if (v == null) remove(k); else put(k, v);
            return v;
        }
        public virtual object compute(object k, java.util.function.BiFunction f)
        {
            var old = get(k);
            var v = f.apply(k, old);
            if (v == null) { if (old != null || containsKey(k)) remove(k); }
            else put(k, v);
            return v;
        }
        public virtual object merge(object k, object v, java.util.function.BiFunction f)
        {
            var old = get(k);
            var nv = old == null ? v : f.apply(old, v);
            if (nv == null) remove(k); else put(k, nv);
            return nv;
        }
        public virtual void forEach(java.util.function.BiConsumer c) { foreach (Map.Entry e in entrySet().toArray()) c.accept(e.getKey(), e.getValue()); }
        public virtual void putAll(Map m) { foreach (Map.Entry e in m.entrySet().toArray()) put(e.getKey(), e.getValue()); }
        public virtual void clear() => entrySet().clear();
        public virtual void replaceAll(java.util.function.BiFunction f) { foreach (Map.Entry e in entrySet().toArray()) e.setValue(f.apply(e.getKey(), e.getValue())); }
        public override bool equals(object o)
        {
            if (ReferenceEquals(o, this)) return true;
            if (!(o is Map m) || m.size() != size()) return false;
            foreach (Map.Entry e in entrySet()) { if (!m.containsKey(e.getKey()) || !AbstractCollection.Eq(e.getValue(), m.get(e.getKey()))) return false; }
            return true;
        }
        public override int hashCode() { int h = 0; foreach (Map.Entry e in entrySet()) h += JO.hashCode(e); return h; }
        public override string toString()
        {
            var sb = new System.Text.StringBuilder("{");
            bool first = true;
            foreach (Map.Entry e in entrySet()) { if (!first) sb.Append(", "); first = false; sb.Append(java.lang.JStr.valueOf(e.getKey())).Append('=').Append(java.lang.JStr.valueOf(e.getValue())); }
            return sb.Append('}').ToString();
        }

        sealed class KeyView : AbstractCollection, Set
        {
            readonly AbstractMap m;
            public KeyView(AbstractMap m) { this.m = m; }
            public override int size() => m.size();
            public override bool contains(object o) => m.containsKey(o);
            public override bool remove(object o) { if (!m.containsKey(o)) return false; m.remove(o); return true; }
            public override void clear() => m.clear();
            public override Iterator iterator() => new MapIter(m.entrySet().iterator(), 0);
            public override bool equals(object o) => ReferenceEquals(o, this) || (o is Set s && s.size() == size() && containsAll(s));
            public override int hashCode() { int h = 0; foreach (var o in this) h += o == null ? 0 : JO.hashCode(o); return h; }
        }

        sealed class ValueView : AbstractCollection
        {
            readonly AbstractMap m;
            public ValueView(AbstractMap m) { this.m = m; }
            public override int size() => m.size();
            public override void clear() => m.clear();
            public override Iterator iterator() => new MapIter(m.entrySet().iterator(), 1);
        }

        sealed class MapIter : java.lang.Object, Iterator
        {
            readonly Iterator it; readonly int which;
            public MapIter(Iterator it, int which) { this.it = it; this.which = which; }
            public bool hasNext() => it.hasNext();
            public object next() { var e = (Map.Entry)it.next(); return which == 0 ? e.getKey() : e.getValue(); }
            public void remove() => it.remove();
        }
    }

    public class SimpleEntry : java.lang.Object, Map.Entry
    {
        internal object key, value;
        public SimpleEntry(object k, object v) { key = k; value = v; }
        public object getKey() => key;
        public object getValue() => value;
        public virtual object setValue(object v) { var o = value; value = v; return o; }
        public override bool equals(object o) => o is Map.Entry e && AbstractCollection.Eq(key, e.getKey()) && AbstractCollection.Eq(value, e.getValue());
        public override int hashCode() => JO.hashCode(key) ^ JO.hashCode(value);
        public override string toString() => java.lang.JStr.valueOf(key) + "=" + java.lang.JStr.valueOf(value);
    }

    /// <summary>
    /// java.util.HashMap with the same bucket layout, hash spreading and resize split as OpenJDK, so iteration order
    /// matches Java for keys whose hashCode is deterministic (Integer, String, records...). Tree bins are not
    /// modelled (they keep insertion order within a bin for iteration, which matches until 8+ collisions).
    /// </summary>
    public class HashMap : AbstractMap, java.lang.Object_Cloneable
    {
        internal sealed class Node : SimpleEntry
        {
            internal readonly int hash;
            internal Node next;
            internal Node before, after; // LinkedHashMap order
            internal Node(int h, object k, object v, Node n) : base(k, v) { hash = h; next = n; }
        }

        internal Node[] table;
        internal int count;
        int threshold;
        readonly float loadFactor = 0.75f;
        internal int modCount;

        public HashMap() { }
        public HashMap(int cap) { threshold = TableSizeFor(cap); }
        public HashMap(int cap, float lf) { threshold = TableSizeFor(cap); loadFactor = lf; }
        public HashMap(Map m)
        {
            int sz = m.size();
            if (sz > 0)
            {
                float ft = ((float)sz / loadFactor) + 1.0F;
                int t = ft < (float)(1 << 30) ? (int)ft : 1 << 30;
                if (t > threshold) threshold = TableSizeFor(t);
            }
            foreach (Map.Entry e in m.entrySet().toArray()) put(e.getKey(), e.getValue());
        }

        static int TableSizeFor(int c)
        {
            int n = -1 >> 0;
            n = c - 1;
            n |= (int)((uint)n >> 1); n |= (int)((uint)n >> 2); n |= (int)((uint)n >> 4); n |= (int)((uint)n >> 8); n |= (int)((uint)n >> 16);
            return n < 0 ? 1 : (n >= 1 << 30 ? 1 << 30 : n + 1);
        }

        internal static int Hash(object k)
        {
            if (k == null) return 0;
            int h = JO.hashCode(k);
            return h ^ (int)((uint)h >> 16);
        }

        public override int size() => count;
        public override bool isEmpty() => count == 0;

        internal Node GetNode(object k)
        {
            if (table == null) return null;
            int h = Hash(k);
            for (var e = table[(table.Length - 1) & h]; e != null; e = e.next)
                if (e.hash == h && (ReferenceEquals(e.key, k) || (k != null && AbstractCollection.Eq(k, e.key)))) return e;
            return null;
        }

        public override object get(object k) => GetNode(k)?.value;
        public override bool containsKey(object k) => GetNode(k) != null;

        public override object put(object k, object v) => PutVal(Hash(k), k, v, false);
        public override object putIfAbsent(object k, object v) => PutVal(Hash(k), k, v, true);

        internal virtual void AfterAccess(Node e) { }
        internal virtual void AfterInsert(Node e) { }
        internal virtual void AfterRemove(Node e) { }

        object PutVal(int h, object k, object v, bool onlyIfAbsent)
        {
            if (table == null || table.Length == 0) Resize();
            int i = (table.Length - 1) & h;
            Node p = table[i];
            if (p == null) { var nn = new Node(h, k, v, null); table[i] = nn; AfterInsert(nn); }
            else
            {
                Node e = null;
                for (Node q = p; ; q = q.next)
                {
                    if (q.hash == h && (ReferenceEquals(q.key, k) || (k != null && AbstractCollection.Eq(k, q.key)))) { e = q; break; }
                    if (q.next == null) { var nn = new Node(h, k, v, null); q.next = nn; AfterInsert(nn); break; }
                }
                if (e != null)
                {
                    var old = e.value;
                    if (!onlyIfAbsent || old == null) e.value = v;
                    AfterAccess(e);
                    return old;
                }
            }
            modCount++;
            if (++count > threshold) Resize();
            return null;
        }

        void Resize()
        {
            var old = table;
            int oldCap = old == null ? 0 : old.Length;
            int oldThr = threshold;
            int newCap, newThr = 0;
            if (oldCap > 0)
            {
                newCap = oldCap << 1;
                if (newCap < (1 << 30) && oldCap >= 16) newThr = oldThr << 1;
            }
            else if (oldThr > 0) newCap = oldThr;
            else { newCap = 16; newThr = 12; }
            if (newThr == 0) { float ft = (float)newCap * loadFactor; newThr = newCap < (1 << 30) && ft < (float)(1 << 30) ? (int)ft : int.MaxValue; }
            threshold = newThr;
            var nt = new Node[newCap];
            table = nt;
            if (old != null)
                for (int j = 0; j < oldCap; j++)
                {
                    Node e = old[j];
                    if (e == null) continue;
                    old[j] = null;
                    if (e.next == null) { nt[e.hash & (newCap - 1)] = e; continue; }
                    Node loH = null, loT = null, hiH = null, hiT = null;
                    for (Node nx; e != null; e = nx)
                    {
                        nx = e.next;
                        if ((e.hash & oldCap) == 0) { if (loT == null) loH = e; else loT.next = e; loT = e; }
                        else { if (hiT == null) hiH = e; else hiT.next = e; hiT = e; }
                    }
                    if (loT != null) { loT.next = null; nt[j] = loH; }
                    if (hiT != null) { hiT.next = null; nt[j + oldCap] = hiH; }
                }
        }

        public override object remove(object k)
        {
            var e = RemoveNode(Hash(k), k);
            return e?.value;
        }

        internal Node RemoveNode(int h, object k)
        {
            if (table == null) return null;
            int i = (table.Length - 1) & h;
            Node prev = null;
            for (var e = table[i]; e != null; prev = e, e = e.next)
                if (e.hash == h && (ReferenceEquals(e.key, k) || (k != null && AbstractCollection.Eq(k, e.key))))
                {
                    if (prev == null) table[i] = e.next; else prev.next = e.next;
                    modCount++; count--;
                    AfterRemove(e);
                    return e;
                }
            return null;
        }

        public override void clear()
        {
            modCount++;
            if (table != null && count > 0) { count = 0; Array.Clear(table, 0, table.Length); }
            ClearLinks();
        }

        internal virtual void ClearLinks() { }

        public override bool containsValue(object v)
        {
            if (table == null) return false;
            foreach (var b in table) for (var e = b; e != null; e = e.next) if (AbstractCollection.Eq(v, e.value)) return true;
            return false;
        }

        internal virtual IEnumerable<Node> Nodes()
        {
            if (table == null) yield break;
            var t = table;
            for (int i = 0; i < t.Length; i++)
                for (var e = t[i]; e != null; e = e.next) yield return e;
        }

        Set _entrySet, _keySet;   // (views keep no state of their own: one each, like Java's)
        public override Set entrySet() => _entrySet ??= new EntrySet(this);
        public override Set keySet() => _keySet ??= new KeySet(this);

        /// <summary>The first node in iteration order (null if empty) and the one after a node: a walk of the live
        /// table (or the insertion links), like Java's HashIterator, instead of a copy of the whole map per iterator.
        /// False: this map can't be walked live (an access-ordered LinkedHashMap) and is copied.</summary>
        internal virtual bool LiveIteration => true;
        internal virtual Node FirstNode(out int index)
        {
            index = 0;
            var t = table;
            if (t == null) return null;
            for (; index < t.Length; index++) if (t[index] != null) return t[index];
            return null;
        }
        internal virtual Node NextNode(Node e, ref int index)
        {
            if (e.next != null) return e.next;
            var t = table;
            for (index++; index < t.Length; index++) if (t[index] != null) return t[index];
            return null;
        }
        public object clone() { var m = (HashMap)Activator.CreateInstance(GetType()); m.putAll(this); return m; }

        internal sealed class NodeIter : java.lang.Object, Iterator
        {
            readonly HashMap m; readonly IEnumerator<Node> en; readonly int which; Node cur; bool has; int expected;
            readonly bool live; Node nextNode; int index;
            public NodeIter(HashMap m, int which)
            {
                this.m = m; this.which = which; expected = m.modCount;
                live = m.LiveIteration;
                if (live) { nextNode = m.FirstNode(out index); has = nextNode != null; }
                else { var snapshot = new List<Node>(m.Nodes()); en = snapshot.GetEnumerator(); has = en.MoveNext(); }
            }
            public bool hasNext() => has;
            public object next()
            {
                if (m.modCount != expected) throw new InvalidOperationException("ConcurrentModification");
                if (!has) throw new InvalidOperationException("NoSuchElement");
                if (live)
                {
                    cur = nextNode;
                    nextNode = m.NextNode(cur, ref index);   // (taken before the caller may remove cur)
                    has = nextNode != null;
                }
                else { cur = en.Current; has = en.MoveNext(); }
                return which == 0 ? cur : which == 1 ? cur.key : cur.value;
            }
            public void remove() { m.RemoveNode(cur.hash, cur.key); expected = m.modCount; }
        }

        sealed class EntrySet : AbstractCollection, Set
        {
            readonly HashMap m;
            public EntrySet(HashMap m) { this.m = m; }
            public override int size() => m.count;
            public override void clear() => m.clear();
            public override Iterator iterator() => new NodeIter(m, 0);
            public override bool contains(object o) => o is Map.Entry e && m.GetNode(e.getKey()) is Node n && AbstractCollection.Eq(n.value, e.getValue());
            public override bool equals(object o) => ReferenceEquals(o, this) || (o is Set s && s.size() == size() && containsAll(s));
            public override int hashCode() { int h = 0; foreach (var o in this) h += o == null ? 0 : JO.hashCode(o); return h; }
        }

        sealed class KeySet : AbstractCollection, Set
        {
            readonly HashMap m;
            public KeySet(HashMap m) { this.m = m; }
            public override int size() => m.count;
            public override void clear() => m.clear();
            public override Iterator iterator() => new NodeIter(m, 1);
            public override bool contains(object o) => m.containsKey(o);
            public override bool remove(object o) => m.RemoveNode(Hash(o), o) != null;
            public override bool equals(object o) => ReferenceEquals(o, this) || (o is Set s && s.size() == size() && containsAll(s));
            public override int hashCode() { int h = 0; foreach (var o in this) h += o == null ? 0 : JO.hashCode(o); return h; }
        }
    }

    public class LinkedHashMap : HashMap
    {
        Node head, tail;
        readonly bool accessOrder;
        public LinkedHashMap() { }
        public LinkedHashMap(int cap) : base(cap) { }
        public LinkedHashMap(int cap, float lf) : base(cap, lf) { }
        public LinkedHashMap(int cap, float lf, bool accessOrder) : base(cap, lf) { this.accessOrder = accessOrder; }
        public LinkedHashMap(Map m) { putAll(m); }
        internal override void AfterInsert(Node e)
        {
            e.before = tail; e.after = null;
            if (tail == null) head = e; else tail.after = e;
            tail = e;
            if (removeEldestEntry(head) && head != e) RemoveNode(head.hash, head.key);
        }
        internal override void AfterRemove(Node e)
        {
            if (e.before == null) head = e.after; else e.before.after = e.after;
            if (e.after == null) tail = e.before; else e.after.before = e.before;
            e.before = e.after = null;
        }
        internal override void AfterAccess(Node e)
        {
            if (!accessOrder || tail == e) return;
            AfterRemove(e); e.before = tail; if (tail == null) head = e; else tail.after = e; tail = e;
        }
        public override object get(object k) { var n = GetNode(k); if (n == null) return null; AfterAccess(n); return n.value; }
        protected virtual bool removeEldestEntry(Map.Entry eldest) => false;
        internal override void ClearLinks() { head = tail = null; }
        internal override IEnumerable<Node> Nodes() { for (var e = head; e != null; e = e.after) yield return e; }
        internal override bool LiveIteration => !accessOrder;
        internal override Node FirstNode(out int index) { index = 0; return head; }
        internal override Node NextNode(Node e, ref int index) => e.after;
    }

    public class TreeMap : AbstractMap, NavigableMap
    {
        readonly Comparator cmp;
        readonly List<SimpleEntry> list = new List<SimpleEntry>(); // sorted by key
        public TreeMap() { }
        public TreeMap(Comparator c) { cmp = c; }
        int C(object a, object b) => cmp != null ? cmp.compare(a, b) : Comparator_.Natural(a, b);
        int Find(object k)
        {
            int lo = 0, hi = list.Count - 1;
            while (lo <= hi) { int mid = (lo + hi) >> 1; int c = C(list[mid].key, k); if (c < 0) lo = mid + 1; else if (c > 0) hi = mid - 1; else return mid; }
            return ~lo;
        }
        public override int size() => list.Count;
        public override object get(object k) { int i = Find(k); return i >= 0 ? list[i].value : null; }
        public override bool containsKey(object k) => Find(k) >= 0;
        public override object put(object k, object v) { int i = Find(k); if (i >= 0) { var o = list[i].value; list[i].value = v; return o; } list.Insert(~i, new SimpleEntry(k, v)); return null; }
        public override object remove(object k) { int i = Find(k); if (i < 0) return null; var o = list[i].value; list.RemoveAt(i); return o; }
        public override void clear() => list.Clear();
        public object firstKey() { if (list.Count == 0) throw new InvalidOperationException("NoSuchElement"); return list[0].key; }
        public object lastKey() { if (list.Count == 0) throw new InvalidOperationException("NoSuchElement"); return list[list.Count - 1].key; }
        public Map.Entry firstEntry() => list.Count == 0 ? null : list[0];
        public Map.Entry lastEntry() => list.Count == 0 ? null : list[list.Count - 1];
        public Map.Entry pollFirstEntry() { if (list.Count == 0) return null; var e = list[0]; list.RemoveAt(0); return e; }
        public object floorKey(object k) { int i = Find(k); if (i >= 0) return list[i].key; i = ~i - 1; return i >= 0 ? list[i].key : null; }
        public object ceilingKey(object k) { int i = Find(k); if (i >= 0) return list[i].key; i = ~i; return i < list.Count ? list[i].key : null; }
        public Map.Entry floorEntry(object k) { int i = Find(k); if (i >= 0) return list[i]; i = ~i - 1; return i >= 0 ? list[i] : null; }
        public Map.Entry ceilingEntry(object k) { int i = Find(k); if (i >= 0) return list[i]; i = ~i; return i < list.Count ? list[i] : null; }
        public override Set entrySet() => new ES(this);
        sealed class ES : AbstractCollection, Set
        {
            readonly TreeMap m;
            public ES(TreeMap m) { this.m = m; }
            public override int size() => m.list.Count;
            public override void clear() => m.list.Clear();
            public override Iterator iterator() => new It(m);
            public override bool equals(object o) => ReferenceEquals(o, this) || (o is Set s && s.size() == size() && containsAll(s));
            public override int hashCode() { int h = 0; foreach (var o in this) h += o == null ? 0 : JO.hashCode(o); return h; }
        }
        sealed class It : java.lang.Object, Iterator
        {
            readonly TreeMap m; int i; int last = -1;
            public It(TreeMap m) { this.m = m; }
            public bool hasNext() => i < m.list.Count;
            public object next() { last = i; return m.list[i++]; }
            public void remove() { m.list.RemoveAt(last); i--; }
        }
    }

    public class HashSet : AbstractCollection, Set
    {
        internal readonly HashMap map;
        static readonly object PRESENT = new object();
        public HashSet() { map = new HashMap(); }
        public HashSet(int cap) { map = new HashMap(cap); }
        public HashSet(Collection c) { map = new HashMap(System.Math.Max((int)(c.size() / .75f) + 1, 16)); addAll(c); }
        internal HashSet(HashMap m) { map = m; }
        public override int size() => map.size();
        public override bool contains(object o) => map.containsKey(o);
        public override bool add(object o) => map.put(o, PRESENT) == null;
        public override bool remove(object o) => map.remove(o) != null;
        public override void clear() => map.clear();
        public override Iterator iterator() => map.keySet().iterator();
        public override bool equals(object o) => o is Set s && s.size() == size() && containsAll(s);
        public override int hashCode() { int h = 0; foreach (var o in this) h += JO.hashCode(o); return h; }
    }

    public class LinkedHashSet : HashSet
    {
        public LinkedHashSet() : base(new LinkedHashMap()) { }
        public LinkedHashSet(int cap) : base(new LinkedHashMap(cap)) { }
        public LinkedHashSet(Collection c) : base(new LinkedHashMap()) { addAll(c); }
    }

    public class TreeSet : AbstractCollection, NavigableSet
    {
        readonly TreeMap m;
        public TreeSet() { m = new TreeMap(); }
        public TreeSet(Comparator c) { m = new TreeMap(c); }
        public TreeSet(Collection c) { m = new TreeMap(); addAll(c); }
        public override int size() => m.size();
        public override bool contains(object o) => m.containsKey(o);
        public override bool add(object o) => m.put(o, true) == null;
        public override bool remove(object o) => m.remove(o) != null;
        public override void clear() => m.clear();
        public override Iterator iterator() => m.keySet().iterator();
        public object first() => m.firstKey();
        public object last() => m.lastKey();
        public object pollFirst() { if (m.size() == 0) return null; var k = m.firstKey(); m.remove(k); return k; }
        public object floor(object k) => m.floorKey(k);
        public object ceiling(object k) => m.ceilingKey(k);
        public override bool equals(object o) => ReferenceEquals(o, this) || (o is Set s && s.size() == size() && containsAll(s));
        public override int hashCode() { int h = 0; foreach (var o in this) h += o == null ? 0 : JO.hashCode(o); return h; }
    }

    /// <summary>Java EnumMap: a slot per ordinal, iterated in ordinal order and live (weakly consistent, like
    /// Java's), without copying the map for every iteration.</summary>
    public class EnumMap : AbstractMap
    {
        SimpleEntry[] slots = new SimpleEntry[8];
        int n;
        Set _es;
        public EnumMap(System.Type t) { }
        public EnumMap(EnumMap m) { putAll(m); }
        static int K(object k) => ((java.lang.Enum)k).ordinal();
        public override int size() => n;
        public override object get(object k) { if (!(k is java.lang.Enum)) return null; int i = K(k); return i < slots.Length ? slots[i]?.value : null; }
        public override bool containsKey(object k) { if (!(k is java.lang.Enum)) return false; int i = K(k); return i < slots.Length && slots[i] != null; }
        public override object put(object k, object v)
        {
            int i = K(k);
            if (i >= slots.Length) System.Array.Resize(ref slots, System.Math.Max(i + 1, slots.Length * 2));
            var e = slots[i];
            if (e != null) { var o = e.value; e.value = v; return o; }
            slots[i] = new SimpleEntry(k, v); n++;
            return null;
        }
        public override object remove(object k)
        {
            if (!(k is java.lang.Enum)) return null;
            int i = K(k);
            if (i >= slots.Length || slots[i] == null) return null;
            var o = slots[i].value; slots[i] = null; n--;
            return o;
        }
        public override void clear() { System.Array.Clear(slots, 0, slots.Length); n = 0; }
        public override Set entrySet() => _es ??= new EntryView(this);
        sealed class EntryView : AbstractCollection, Set
        {
            readonly EnumMap m;
            public EntryView(EnumMap m) { this.m = m; }
            public override int size() => m.n;
            public override Iterator iterator() => new It(m);
            public override bool equals(object o) => ReferenceEquals(o, this) || (o is Set s && s.size() == size() && containsAll(s));
            public override int hashCode() { int h = 0; foreach (var o in this) h += o == null ? 0 : JO.hashCode(o); return h; }
        }
        sealed class It : java.lang.Object, Iterator
        {
            readonly EnumMap m; int i = -1, nx; SimpleEntry cur;
            public It(EnumMap m) { this.m = m; nx = Find(0); }
            int Find(int from) { var s = m.slots; for (int j = from; j < s.Length; j++) if (s[j] != null) return j; return -1; }
            public bool hasNext() { if (nx >= 0 && (nx >= m.slots.Length || m.slots[nx] == null)) nx = Find(nx + 1); return nx >= 0; }
            public object next()
            {
                if (!hasNext()) throw new InvalidOperationException("NoSuchElement");
                i = nx; cur = m.slots[i]; nx = Find(i + 1);
                return cur;
            }
            public void remove() => m.remove(cur.key);
        }
    }

    /// <summary>An ordinal-ordered set of enum constants (Java EnumSet iterates in ordinal order).</summary>
    public class EnumSet : TreeSet
    {
        public EnumSet() : base(new Comparator_L((a, b) => ((java.lang.Enum)a).ordinal() - ((java.lang.Enum)b).ordinal())) { }
        public static EnumSet noneOf(System.Type t) => new EnumSet();
        public static EnumSet allOf(System.Type t) { var s = new EnumSet(); foreach (var o in J2CS.Ext.JTypeExt.getEnumConstants(t)) s.add(o); return s; }
        public static EnumSet of(object a, params object[] rest) { var s = new EnumSet(); s.add(a); foreach (var o in rest) s.add(o); return s; }
        public static EnumSet copyOf(Collection c) { var s = new EnumSet(); s.addAll(c); return s; }
        public static EnumSet range(object from, object to)
        {
            var s = new EnumSet();
            int a = ((java.lang.Enum)from).ordinal(), b = ((java.lang.Enum)to).ordinal();
            foreach (var o in J2CS.Ext.JTypeExt.getEnumConstants(from.GetType())) { int k = ((java.lang.Enum)o).ordinal(); if (k >= a && k <= b) s.add(o); }
            return s;
        }
    }
}

namespace java.lang
{
    public interface Object_Cloneable { }
}
