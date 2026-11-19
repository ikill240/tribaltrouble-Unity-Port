// Extension methods giving C# types (string, System.Type, System.Exception, runtime interfaces) their Java method names.
using System;
using java.lang;
using java.util;
using java.util.function;

namespace J2CS.Ext
{
    /// <summary>String instance methods with Java names (extension methods).</summary>
    public static class JStringExt
    {
        public static int length(this string s) => s.Length;
        public static string subSequence(this string s, int a, int b) => s.Substring(a, b - a);
        public static java.util.stream.IntStream codePoints(this string s) => new java.util.stream.IntStream(CodePointsOf(s));
        static System.Collections.Generic.IEnumerable<int> CodePointsOf(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsHighSurrogate(s[i]) && i + 1 < s.Length && char.IsLowSurrogate(s[i + 1])) { yield return char.ConvertToUtf32(s[i], s[i + 1]); i++; }
                else yield return s[i];
            }
        }
        public static java.util.stream.IntStream chars(this string s) => new java.util.stream.IntStream(System.Linq.Enumerable.Select(s, c => (int)c));
        public static int lastIndexOf(this string s, string t, int from)
        {
            if (from < 0) return -1;
            int start = java.lang.Math.min(from, s.Length - t.Length);
            for (int i = start; i >= 0; i--) if (string.CompareOrdinal(s, i, t, 0, t.Length) == 0) return i;
            return -1;
        }
        public static int lastIndexOf(this string s, int c, int from) => from < 0 ? -1 : s.LastIndexOf((char)c, java.lang.Math.min(from, s.Length - 1));
        // Java 21 SequencedCollection on sorted sets
        public static object getFirst(this java.util.SortedSet set) => set.first();
        public static object getLast(this java.util.SortedSet set) => set.last();
        public static char charAt(this string s, int i) => s[i];
        public static bool isEmpty(this string s) => s.Length == 0;
        public static bool isBlank(this string s) => s.Trim().Length == 0;
        public static string substring(this string s, int a) => s.Substring(a);
        public static string substring(this string s, int a, int b) => s.Substring(a, b - a);
        public static int indexOf(this string s, string t) => s.IndexOf(t, StringComparison.Ordinal);
        public static int indexOf(this string s, char c) => s.IndexOf(c);
        public static int indexOf(this string s, int c) => s.IndexOf((char)c);
        public static int indexOf(this string s, string t, int from) => s.IndexOf(t, java.lang.Math.min(from, s.Length), StringComparison.Ordinal);
        public static int indexOf(this string s, int c, int from) => from >= s.Length ? -1 : s.IndexOf((char)c, java.lang.Math.max(from, 0));
        public static int lastIndexOf(this string s, string t) => s.LastIndexOf(t, StringComparison.Ordinal);
        public static int lastIndexOf(this string s, int c) => s.LastIndexOf((char)c);
        public static bool startsWith(this string s, string p) => s.StartsWith(p, StringComparison.Ordinal);
        public static bool endsWith(this string s, string p) => s.EndsWith(p, StringComparison.Ordinal);
        public static bool contains(this string s, string p) => s.Contains(p);
        public static bool equalsIgnoreCase(this string s, string t) => t != null && string.Equals(s, t, StringComparison.OrdinalIgnoreCase);
        public static int compareTo(this string s, string t)
        {
            int n = java.lang.Math.min(s.Length, t.Length);
            for (int i = 0; i < n; i++) if (s[i] != t[i]) return s[i] - t[i];
            return s.Length - t.Length;
        }
        public static int compareToIgnoreCase(this string s, string t) => compareTo(s.ToLowerInvariant(), t.ToLowerInvariant());
        public static string trim(this string s) => s.Trim(TrimChars);
        public static string strip(this string s) => s.Trim();
        public static string toLowerCase(this string s) => s.ToLowerInvariant();
        public static string toUpperCase(this string s) => s.ToUpperInvariant();
        public static string toLowerCase(this string s, java.util.Locale l) => s.ToLowerInvariant();
        public static string toUpperCase(this string s, java.util.Locale l) => s.ToUpperInvariant();
        public static char[] toCharArray(this string s) => s.ToCharArray();
        public static string replace(this string s, string a, string b) => s.Replace(a, b);
        public static string replace(this string s, char a, char b) => s.Replace(a, b);
        public static string replaceAll(this string s, string regex, string rep) => System.Text.RegularExpressions.Regex.Replace(s, regex, rep.Replace("$", "$$").Replace("$$1", "$1"));
        public static bool matches(this string s, string regex) => System.Text.RegularExpressions.Regex.IsMatch(s, "^(?:" + regex + ")$");
        public static string[] split(this string s, string regex)
        {
            var parts = new System.Collections.Generic.List<string>(System.Text.RegularExpressions.Regex.Split(s, regex));
            while (parts.Count > 0 && parts[parts.Count - 1].Length == 0) parts.RemoveAt(parts.Count - 1);
            if (parts.Count == 0) parts.Add(s.Length == 0 ? "" : "");
            return parts.ToArray();
        }
        public static string[] split(this string s, string regex, int limit)
        {
            if (limit == 0) return split(s, regex);
            if (limit > 0) return new System.Text.RegularExpressions.Regex(regex).Split(s, limit);
            return System.Text.RegularExpressions.Regex.Split(s, regex);
        }
        public static string repeat(this string s, int n) { var b = new System.Text.StringBuilder(); for (int i = 0; i < n; i++) b.Append(s); return b.ToString(); }
        public static string intern(this string s) => string.Intern(s);
        public static string formatted(this string s, params object[] a) => JFormat.format(s, a);
        public static int codePointAt(this string s, int i) => s[i];
        static readonly char[] TrimChars = BuildTrim();
        static char[] BuildTrim() { var c = new char[33]; for (int i = 0; i <= 32; i++) c[i] = (char)i; return c; }
    }
    /// <summary>Methods on Class objects (System.Type) with Java names.</summary>
    public static class JTypeExt
    {
        public static string getSimpleName(this System.Type t)
        {
            var n = t.Name;
            int i = n.LastIndexOf('+');
            return i >= 0 ? n.Substring(i + 1) : n;
        }
        public static string getName(this System.Type t) => t.FullName;
        public static bool isInstance(this System.Type t, object o) => o != null && t.IsInstanceOfType(o);
        public static object cast(this System.Type t, object o) => o;
        public static bool isAssignableFrom(this System.Type t, System.Type o) => t.IsAssignableFrom(o);
        public static bool isInterface(this System.Type t) => t.IsInterface;
        public static System.Type getSuperclass(this System.Type t) => t.BaseType;
        public static java.lang.Package_ getPackage(this System.Type t) => new java.lang.Package_(t.Namespace);
        public static java.net.URL getResource(this System.Type t, string name) => java.net.URL.Resolve(name);
        /// <summary>Class.getResourceAsStream: a relative name is resolved against the class's package (Java rules).</summary>
        public static java.io.InputStream getResourceAsStream(this System.Type t, string name)
        {
            string path = name.StartsWith("/") ? name.Substring(1) : (t.Namespace ?? "").Replace("@", "").Replace('.', '/') + "/" + name;
            var bytes = java.lang.JResources.Open?.Invoke(path);
            return bytes == null ? null : new java.io.ByteArrayInputStream((sbyte[])(System.Array)bytes);
        }
        public static object[] getEnumConstants(this System.Type t)
        {
            var m = t.GetMethod("values");
            return m == null ? null : (object[])m.Invoke(null, null);
        }
    }
    /// <summary>Comparator default methods (Java 8) as extension methods.</summary>
    public static class ComparatorExt
    {
        public static Comparator reversed(this Comparator c) => new Comparator_L((a, b) => c.compare(b, a));
        public static Comparator thenComparing(this Comparator c, Comparator d) => new Comparator_L((a, b) => { int r = c.compare(a, b); return r != 0 ? r : d.compare(a, b); });
        public static Comparator thenComparing(this Comparator c, java.util.function.Function key) => c.thenComparing(Comparator_.comparing(key));
        public static Comparator thenComparingInt(this Comparator c, java.util.function.ToIntFunction key) => c.thenComparing(Comparator_.comparingInt(key));
        public static Comparator thenComparingDouble(this Comparator c, java.util.function.ToDoubleFunction key) => c.thenComparing(Comparator_.comparingDouble(key));
    }
    public static class PredicateExt
    {
        public static Predicate negate(this Predicate p) => new Predicate_L(o => !p.test(o));
        public static Predicate and(this Predicate p, Predicate q) => new Predicate_L(o => p.test(o) && q.test(o));
        public static Predicate or(this Predicate p, Predicate q) => new Predicate_L(o => p.test(o) || q.test(o));
    }
    public static class FunctionExt
    {
        public static Function andThen(this Function a, Function b) => new Function_L(o => b.apply(a.apply(o)));
        public static Function compose(this Function a, Function b) => new Function_L(o => a.apply(b.apply(o)));
    }

    public static class ExceptionExt
    {
        public static void printStackTrace(this System.Exception e) => java.lang.IO.println(e.ToString());
        public static string getMessage(this System.Exception e) => e.Message;
        public static string getLocalizedMessage(this System.Exception e) => e.Message;
        public static System.Exception getCause(this System.Exception e) => e.InnerException;
        public static java.lang.StackTraceElement[] getStackTrace(this System.Exception e) => new java.lang.StackTraceElement[0];
        public static void addSuppressed(this System.Exception e, System.Exception s) { }
        public static string toString(this System.Exception e) => e.GetType().Name + ": " + e.Message;
    }

    public static class IterableExt
    {
        public static object spliterator(this java.lang.Iterable it) => it;
        public static void forEach(this java.lang.Iterable it, java.util.function.Consumer c) { foreach (var o in java.lang.JIter.of(it)) c.accept(o); }
    }
}
