using System;
using System.Collections;
using System.Collections.Generic;

namespace TribalTrouble.Sim.Determinism
{
    /// <summary>
    /// Java collection method names (add, get, size, isEmpty, put, ...) as extension methods, so
    /// ported code keeps its Java call shape. Semantics follow java.util where they differ from
    /// .NET (e.g. Map.get returns null/default for a missing key instead of throwing).
    /// Note: Dictionary preserves insertion order only while nothing is removed - it stands in
    /// for LinkedHashMap. Ported code that iterates a java.util.HashMap must use JHashMap order.
    /// </summary>
    public static class JavaCollections
    {
        // ---- List
        public static bool add<T>(this List<T> list, T item) { list.Add(item); return true; }
        public static void add<T>(this List<T> list, int index, T item) => list.Insert(index, item);
        public static T get<T>(this List<T> list, int index) => list[index];
        public static T set<T>(this List<T> list, int index, T item) { var old = list[index]; list[index] = item; return old; }
        public static int size<T>(this List<T> list) => list.Count;
        public static bool isEmpty<T>(this List<T> list) => list.Count == 0;
        public static void clear<T>(this List<T> list) => list.Clear();
        public static bool contains<T>(this List<T> list, T item) => list.Contains(item);
        public static int indexOf<T>(this List<T> list, T item) => list.IndexOf(item);
        /// <summary>List.remove(int index) - removes by index (as Java does for an int argument).</summary>
        public static T remove<T>(this List<T> list, int index) { var v = list[index]; list.RemoveAt(index); return v; }
        public static bool removeItem<T>(this List<T> list, T item) => list.Remove(item);
        public static T removeFirst<T>(this List<T> list) => list.remove(0);
        public static T removeLast<T>(this List<T> list) => list.remove(list.Count - 1);
        public static T getFirst<T>(this List<T> list) => list[0];
        public static T getLast<T>(this List<T> list) => list[list.Count - 1];
        public static bool addAll<T>(this List<T> list, IEnumerable<T> items) { int n = list.Count; list.AddRange(items); return list.Count != n; }
        public static T[] toArray<T>(this List<T> list) => list.ToArray();

        // ---- Map (Dictionary)
        public static V put<K, V>(this Dictionary<K, V> map, K key, V value)
        {
            map.TryGetValue(key, out var old);
            map[key] = value;
            return old;
        }

        public static V get<K, V>(this Dictionary<K, V> map, K key) => map.TryGetValue(key, out var v) ? v : default;
        public static V getOrDefault<K, V>(this Dictionary<K, V> map, K key, V def) => map.TryGetValue(key, out var v) ? v : def;
        public static bool containsKey<K, V>(this Dictionary<K, V> map, K key) => map.ContainsKey(key);
        public static int size<K, V>(this Dictionary<K, V> map) => map.Count;
        public static bool isEmpty<K, V>(this Dictionary<K, V> map) => map.Count == 0;
        public static Dictionary<K, V>.KeyCollection keySet<K, V>(this Dictionary<K, V> map) => map.Keys;
        public static Dictionary<K, V>.ValueCollection values<K, V>(this Dictionary<K, V> map) => map.Values;
        public static V computeIfAbsent<K, V>(this Dictionary<K, V> map, K key, Func<K, V> fn)
        {
            if (!map.TryGetValue(key, out var v) || v == null)
            {
                v = fn(key);
                if (v != null) map[key] = v;
            }
            return v;
        }
    }

    /// <summary>
    /// java.util.LinkedList / ArrayDeque used as a list or queue: O(1) add/removeFirst on a ring buffer.
    /// Iteration order is first-to-last, like Java.
    /// </summary>
    public sealed class JDeque<T> : IEnumerable<T>
    {
        T[] _items = new T[16];
        int _head;
        int _count;

        public int Count => _count;
        public int size() => _count;
        public bool isEmpty() => _count == 0;

        public bool add(T item) { addLast(item); return true; }

        public void addLast(T item)
        {
            if (_count == _items.Length) Grow();
            _items[(_head + _count) % _items.Length] = item;
            _count++;
        }

        public void addFirst(T item)
        {
            if (_count == _items.Length) Grow();
            _head = (_head - 1 + _items.Length) % _items.Length;
            _items[_head] = item;
            _count++;
        }

        public T removeFirst()
        {
            if (_count == 0) throw new InvalidOperationException("NoSuchElementException");
            T v = _items[_head];
            _items[_head] = default;
            _head = (_head + 1) % _items.Length;
            _count--;
            return v;
        }

        public T removeLast()
        {
            if (_count == 0) throw new InvalidOperationException("NoSuchElementException");
            int i = (_head + _count - 1) % _items.Length;
            T v = _items[i];
            _items[i] = default;
            _count--;
            return v;
        }

        public T poll() => _count == 0 ? default : removeFirst();
        public T peek() => _count == 0 ? default : _items[_head];
        public T getFirst() => _count == 0 ? throw new InvalidOperationException("NoSuchElementException") : _items[_head];
        public T getLast() => _count == 0 ? throw new InvalidOperationException("NoSuchElementException") : _items[(_head + _count - 1) % _items.Length];
        public T get(int index)
        {
            if ((uint)index >= (uint)_count) throw new IndexOutOfRangeException();
            return _items[(_head + index) % _items.Length];
        }

        public void clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _head = 0;
            _count = 0;
        }

        void Grow()
        {
            var n = new T[_items.Length * 2];
            for (int i = 0; i < _count; i++) n[i] = _items[(_head + i) % _items.Length];
            _items = n;
            _head = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++) yield return _items[(_head + i) % _items.Length];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
