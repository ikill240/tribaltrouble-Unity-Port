// More of the Java class library, for the Expert / Ultra / Fable / Gauntlet AIs (logging, dev-time parameter
// overrides by reflection, timestamps).
using System;

namespace java.lang.@ref
{
    /// <summary>Held strongly: the AIs only use it so static caches don't pin a finished World.</summary>
    public class WeakReference : java.lang.Object
    {
        readonly System.WeakReference<object> r;
        public WeakReference(object o) { r = o == null ? null : new System.WeakReference<object>(o); }
        public object get() => r != null && r.TryGetTarget(out var o) ? o : null;
        public void clear() { }
    }
}

namespace java.lang.reflect
{
    /// <summary>java.lang.reflect.Field over a translated field (translated names may carry a "_f" suffix or an "@").</summary>
    public sealed class Field : java.lang.Object
    {
        readonly System.Reflection.FieldInfo f;
        readonly string name;
        internal Field(System.Reflection.FieldInfo f, string name) { this.f = f; this.name = name; }
        public string getName() => name;
        public System.Type getType()
        {
            var t = f.FieldType;
            return t;
        }
        public int getModifiers() => (f.IsStatic ? Modifier.STATIC : 0) | (f.IsPublic ? Modifier.PUBLIC : 0) | (f.IsInitOnly || f.IsLiteral ? Modifier.FINAL : 0);
        public object get(object o) => f.GetValue(o);
        public void set(object o, object v) => f.SetValue(o, v);
        public int getInt(object o) => Convert.ToInt32(f.GetValue(o));
        public float getFloat(object o) => Convert.ToSingle(f.GetValue(o));
        public double getDouble(object o) => Convert.ToDouble(f.GetValue(o));
        public bool getBoolean(object o) => (bool)f.GetValue(o);
        public long getLong(object o) => Convert.ToInt64(f.GetValue(o));
        public void setInt(object o, int v) => f.SetValue(o, v);
        public void setFloat(object o, float v) => f.SetValue(o, v);
        public void setDouble(object o, double v) => f.SetValue(o, v);
        public void setBoolean(object o, bool v) => f.SetValue(o, v);
        public void setLong(object o, long v) => f.SetValue(o, v);
        public void setAccessible(bool b) { }

        static string JavaName(string n) => n.EndsWith("_f") ? n.Substring(0, n.Length - 2) : n;

        internal static Field Find(System.Type t, string name, bool declaredOnly)
        {
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
            if (declaredOnly) flags |= System.Reflection.BindingFlags.DeclaredOnly;
            var fi = t.GetField(name, flags) ?? t.GetField(name + "_f", flags);
            if (fi == null || (!declaredOnly && !fi.IsPublic)) throw new NoSuchFieldException(name);
            return new Field(fi, name);
        }

        internal static Field[] All(System.Type t, bool declaredOnly)
        {
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
            if (declaredOnly) flags |= System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic;
            var fs = t.GetFields(flags);
            var r = new System.Collections.Generic.List<Field>();
            foreach (var fi in fs) if (!fi.Name.Contains("<")) r.Add(new Field(fi, JavaName(fi.Name)));
            return r.ToArray();
        }
    }

    public static class Modifier
    {
        public const int PUBLIC = 1, PRIVATE = 2, PROTECTED = 4, STATIC = 8, FINAL = 16;
        public static bool isStatic(int m) => (m & STATIC) != 0;
        public static bool isFinal(int m) => (m & FINAL) != 0;
        public static bool isPublic(int m) => (m & PUBLIC) != 0;
    }
}

namespace java.lang
{
    public class ReflectiveOperationException : System.Exception { public ReflectiveOperationException(string m) : base(m) { } }
    public class NoSuchFieldException : ReflectiveOperationException { public NoSuchFieldException(string m) : base(m) { } }
}

namespace J2CS.Ext
{
    public static class JReflectExt
    {
        public static java.lang.reflect.Field getField(this System.Type t, string name) => java.lang.reflect.Field.Find(t, name, false);
        public static java.lang.reflect.Field getDeclaredField(this System.Type t, string name) => java.lang.reflect.Field.Find(t, name, true);
        public static java.lang.reflect.Field[] getFields(this System.Type t) => java.lang.reflect.Field.All(t, false);
        public static java.lang.reflect.Field[] getDeclaredFields(this System.Type t) => java.lang.reflect.Field.All(t, true);
    }
}

namespace java.time
{
    public sealed class ZoneId : java.lang.Object
    {
        public static ZoneId systemDefault() => new ZoneId();
    }

    public sealed class LocalDateTime : java.lang.Object
    {
        readonly DateTime t;
        LocalDateTime(DateTime t) { this.t = t; }
        public static LocalDateTime now() => new LocalDateTime(DateTime.Now);
        public static LocalDateTime now(ZoneId z) => now();
        public LocalDateTime withNano(int n) => new LocalDateTime(new DateTime(t.Ticks - t.Ticks % TimeSpan.TicksPerSecond + n / 100));
        public override string toString() => t.ToString(t.Millisecond == 0 && t.Ticks % TimeSpan.TicksPerSecond == 0 ? "yyyy-MM-ddTHH:mm:ss" : "yyyy-MM-ddTHH:mm:ss.fffffff", System.Globalization.CultureInfo.InvariantCulture);
    }
}

namespace java.nio.charset
{
    public sealed class Charset : java.lang.Object
    {
        public readonly string name;
        internal Charset(string n) { name = n; }
        public static Charset forName(string n) => new Charset(n);
    }
    public static class StandardCharsets
    {
        public static readonly Charset UTF_8 = new Charset("UTF-8");
        public static readonly Charset US_ASCII = new Charset("US-ASCII");
        public static readonly Charset ISO_8859_1 = new Charset("ISO-8859-1");
    }
}

namespace java.nio.file
{
    public static class FilesExtra
    {
        internal static bool Has(object[] o, object opt) { foreach (var x in o) if (x == opt) return true; return false; }
    }
}

namespace java.io
{
    public abstract class Writer : java.lang.Object, java.lang.AutoCloseable
    {
        public abstract void write(string s);
        public virtual void write(int c) => write(((char)c).ToString());
        public virtual Writer append(string s) { write(s ?? "null"); return this; }
        public virtual void flush() { }
        public virtual void close() { }
    }

    public class BufferedWriter : Writer
    {
        readonly System.IO.TextWriter w;
        internal BufferedWriter(System.IO.TextWriter w) { this.w = w; }
        public BufferedWriter(Writer inner) { w = new WriterAdapter(inner); }
        public override void write(string s) => w.Write(s);
        public void newLine() => w.Write('\n');
        public override void flush() => w.Flush();
        public override void close() => w.Dispose();

        sealed class WriterAdapter : System.IO.TextWriter
        {
            readonly Writer inner;
            public WriterAdapter(Writer inner) { this.inner = inner; }
            public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
            public override void Write(char c) => inner.write(c.ToString());
            public override void Write(string s) => inner.write(s);
            public override void Flush() => inner.flush();
            protected override void Dispose(bool d) { if (d) inner.close(); }
        }
    }
}

namespace java.lang
{
    /// <summary>Host hook for class-path resources ("com/oddlabs/tt/util/dictionary" -> bytes, or null).</summary>
    public static class JResources
    {
        public static System.Func<string, byte[]> Open;
    }
}

namespace java.io
{
    public class InputStreamReader : Reader
    {
        internal readonly System.IO.StreamReader r;
        public InputStreamReader(InputStream i) : this(i, (object)null) { }
        public InputStreamReader(InputStream i, object charset) { r = new System.IO.StreamReader(i.Stream(), new System.Text.UTF8Encoding(false)); }
        public int read() => r.Read();
        public override void close() => r.Dispose();
    }
    public class BufferedReader : Reader
    {
        readonly System.IO.TextReader r;
        public BufferedReader(Reader rd) { r = rd is InputStreamReader isr ? isr.r : throw new System.NotSupportedException("BufferedReader over " + rd.GetType().Name); }
        public string readLine() => r.ReadLine();
        public int read() => r.Read();
        public java.util.stream.Stream lines() { var l = new System.Collections.Generic.List<object>(); string s; while ((s = r.ReadLine()) != null) l.Add(s); return new java.util.stream.Stream(l.ToArray()); }
        public override void close() => r.Dispose();
    }
}

namespace java.time.format
{
    public sealed class FormatStyle : java.lang.Enum
    {
        public static readonly FormatStyle FULL = new FormatStyle("FULL", 0), LONG = new FormatStyle("LONG", 1), MEDIUM = new FormatStyle("MEDIUM", 2), SHORT = new FormatStyle("SHORT", 3);
        FormatStyle(string n, int o) : base(n, o) { }
    }

    /// <summary>Localized date/time formatting on the user's culture (DateLabel, save lists).</summary>
    public sealed class DateTimeFormatter : java.lang.Object
    {
        readonly string _pattern;
        DateTimeFormatter(string p) { _pattern = p; }
        public static DateTimeFormatter ofLocalizedDateTime(FormatStyle date, FormatStyle time)
        {
            var c = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
            string d = date == FormatStyle.SHORT ? c.ShortDatePattern : date == FormatStyle.MEDIUM ? "MMM d, yyyy" : c.LongDatePattern;
            string t = time == FormatStyle.SHORT ? c.ShortTimePattern : c.LongTimePattern;
            return new DateTimeFormatter(d + " " + t);
        }
        public static DateTimeFormatter ofLocalizedDateTime(FormatStyle style) => ofLocalizedDateTime(style, style);
        public static DateTimeFormatter ofPattern(string javaPattern) => new DateTimeFormatter(javaPattern.Replace("a", "tt").Replace("E", "ddd").Replace("u", "y"));
        public DateTimeFormatter withZone(java.time.ZoneId z) => this;
        public DateTimeFormatter withLocale(java.util.Locale l) => this;
        public string format(object temporal)
        {
            DateTime t = temporal is java.time.Instant i ? DateTimeOffset.FromUnixTimeMilliseconds(i.toEpochMilli()).LocalDateTime : DateTime.Now;
            return t.ToString(_pattern, System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}

namespace java.lang
{
    public sealed class Runtime : java.lang.Object
    {
        static readonly Runtime R = new Runtime();
        public static Runtime getRuntime() => R;
        public long totalMemory() => System.GC.GetTotalMemory(false) * 2;
        public long freeMemory() => System.GC.GetTotalMemory(false);
        public long maxMemory() => long.MaxValue;
        public int availableProcessors() => Environment.ProcessorCount;
        public void gc() => System.GC.Collect();
    }
}

