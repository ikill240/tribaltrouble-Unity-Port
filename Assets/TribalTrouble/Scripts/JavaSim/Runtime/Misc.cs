// Shells for JDK APIs that the simulation touches only incidentally (logging, I/O, buffers, time, zip).
using System;
using J2CS.Ext;
using System.Collections.Generic;

namespace java.util.logging
{
    public sealed class Level : java.lang.Object
    {
        public static readonly Level SEVERE = new Level("SEVERE", 1000), WARNING = new Level("WARNING", 900), INFO = new Level("INFO", 800),
            CONFIG = new Level("CONFIG", 700), FINE = new Level("FINE", 500), FINER = new Level("FINER", 400), FINEST = new Level("FINEST", 300),
            ALL = new Level("ALL", int.MinValue), OFF = new Level("OFF", int.MaxValue);
        readonly string n; internal readonly int v;
        Level(string n, int v) { this.n = n; this.v = v; }
        public override string toString() => n;
        public int intValue() => v;
    }

    public sealed class Logger : java.lang.Object
    {
        public static int MinLevel = 900;
        public static Action<string> Sink = s => java.lang.IO.println(s);
        readonly string name;
        Logger(string n) { name = n; }
        public static Logger getLogger(string n) => new Logger(n);
        public static Logger getAnonymousLogger() => new Logger("");
        public static Logger getGlobal() => new Logger("global");
        public bool isLoggable(Level l) => l.v >= MinLevel;
        public void log(Level l, string msg) { if (isLoggable(l)) Sink?.Invoke(l + ": " + msg); }
        public void log(Level l, string msg, object thrownOrParam) { if (isLoggable(l)) Sink?.Invoke(l + ": " + msg + " " + java.lang.JStr.valueOf(thrownOrParam)); }
        public void log(Level l, java.util.function.Supplier s) { if (isLoggable(l)) Sink?.Invoke(l + ": " + s.get()); }
        public void logp(Level l, string cls, string method, string msg) => log(l, msg);
        public void logp(Level l, string cls, string method, string msg, object thrownOrParam) => log(l, msg, thrownOrParam);
        public void throwing(string cls, string method, object thrown) => log(Level.FINER, "THROW " + cls + "." + method, thrown);
        public void severe(string m) => log(Level.SEVERE, m);
        public void warning(string m) => log(Level.WARNING, m);
        public void info(string m) => log(Level.INFO, m);
        public void config(string m) => log(Level.CONFIG, m);
        public void fine(string m) => log(Level.FINE, m);
        public void finer(string m) => log(Level.FINER, m);
        public void finest(string m) => log(Level.FINEST, m);
        public void severe(java.util.function.Supplier s) => log(Level.SEVERE, s);
        public void warning(java.util.function.Supplier s) => log(Level.WARNING, s);
        public void info(java.util.function.Supplier s) => log(Level.INFO, s);
        public void fine(java.util.function.Supplier s) => log(Level.FINE, s);
    }
}

namespace java.util.concurrent
{
    public sealed class TimeUnit : java.lang.Enum
    {
        public static readonly TimeUnit NANOSECONDS = new TimeUnit("NANOSECONDS", 0, 1L), MICROSECONDS = new TimeUnit("MICROSECONDS", 1, 1000L),
            MILLISECONDS = new TimeUnit("MILLISECONDS", 2, 1000000L), SECONDS = new TimeUnit("SECONDS", 3, 1000000000L),
            MINUTES = new TimeUnit("MINUTES", 4, 60000000000L), HOURS = new TimeUnit("HOURS", 5, 3600000000000L);
        readonly long ns;
        TimeUnit(string n, int o, long ns) : base(n, o) { this.ns = ns; }
        public long toMillis(long d) => d * ns / 1000000L;
        public long toNanos(long d) => d * ns;
        public long toSeconds(long d) => d * ns / 1000000000L;
        public long toMicros(long d) => d * ns / 1000L;
        public long convert(long d, TimeUnit u) => d * u.ns / ns;
    }

    /// <summary>Insertion-ordered set whose iterators see a snapshot (java.util.concurrent.CopyOnWriteArraySet).</summary>
    public class CopyOnWriteArraySet : java.util.AbstractCollection, java.util.Set
    {
        object[] a = new object[0];
        public CopyOnWriteArraySet() { }
        public CopyOnWriteArraySet(java.util.Collection c) { foreach (var o in c.toArray()) add(o); }
        public override int size() => a.Length;
        public override bool contains(object o) { foreach (var x in a) if (Eq(o, x)) return true; return false; }
        public override bool add(object o)
        {
            if (contains(o)) return false;
            var b = new object[a.Length + 1]; Array.Copy(a, b, a.Length); b[a.Length] = o; a = b; return true;
        }
        public override bool remove(object o)
        {
            for (int i = 0; i < a.Length; i++)
                if (Eq(o, a[i])) { var b = new object[a.Length - 1]; Array.Copy(a, 0, b, 0, i); Array.Copy(a, i + 1, b, i, a.Length - i - 1); a = b; return true; }
            return false;
        }
        public override void clear() => a = new object[0];
        public override object[] toArray() => (object[])a.Clone();
        public override java.util.Iterator iterator() => new Snap(a);
        public override void forEach(java.util.function.Consumer c) { foreach (var o in a) c.accept(o); }
        public override bool removeIf(java.util.function.Predicate p)
        {
            var keep = new List<object>(); bool ch = false;
            foreach (var o in a) if (p.test(o)) ch = true; else keep.Add(o);
            a = keep.ToArray(); return ch;
        }
        public override bool removeAll(java.util.Collection c) { return removeIf(new java.util.function.Predicate_L(o => c.contains(o))); }
        sealed class Snap : java.lang.Object, java.util.Iterator
        {
            readonly object[] s; int i;
            public Snap(object[] s) { this.s = s; }
            public bool hasNext() => i < s.Length;
            public object next() => s[i++];
            public void remove() => throw new NotSupportedException();
        }
        public override bool equals(object o) => ReferenceEquals(o, this) || (o is Set s && s.size() == size() && containsAll(s));
        public override int hashCode() { int h = 0; foreach (var o in this) h += o == null ? 0 : java.lang.JO.hashCode(o); return h; }
    }

    public class CopyOnWriteArrayList : java.util.ArrayList
    {
        public CopyOnWriteArrayList() { }
        public CopyOnWriteArrayList(java.util.Collection c) : base(c) { }
        public override java.util.Iterator iterator() => new java.util.ArrayList(this).iterator();
    }

    public interface ConcurrentMap : java.util.Map { }
    public class ConcurrentHashMap : java.util.HashMap, ConcurrentMap
    {
        public ConcurrentHashMap() { }
        public ConcurrentHashMap(int c) : base(c) { }
    }
    public class ConcurrentLinkedQueue : java.util.LinkedList { }
    public class ConcurrentLinkedDeque : java.util.LinkedList { }

    namespace atomic
    {
        public sealed class AtomicInteger : java.lang.Object
        {
            int v;
            public AtomicInteger() { }
            public AtomicInteger(int v) { this.v = v; }
            public int get() => v;
            public void set(int x) => v = x;
            public int incrementAndGet() => ++v;
            public int getAndIncrement() => v++;
            public int decrementAndGet() => --v;
            public int addAndGet(int d) => v += d;
        }
        public sealed class AtomicLong : java.lang.Object
        {
            long v;
            public AtomicLong() { }
            public AtomicLong(long v) { this.v = v; }
            public long get() => v;
            public void set(long x) => v = x;
            public long incrementAndGet() => ++v;
            public long getAndIncrement() => v++;
        }
        public sealed class AtomicBoolean : java.lang.Object
        {
            bool v;
            public AtomicBoolean() { }
            public AtomicBoolean(bool v) { this.v = v; }
            public bool get() => v;
            public void set(bool x) => v = x;
            public bool compareAndSet(bool e, bool n) { if (v != e) return false; v = n; return true; }
        }
    }
}

namespace java.util.zip
{
    public interface Checksum { void update(int b); void update(sbyte[] b, int off, int len); long getValue(); void reset(); }
    public sealed class Adler32 : java.lang.Object, Checksum
    {
        uint a = 1, b = 0;
        public void update(int x) { a = (a + (uint)(x & 0xff)) % 65521; b = (b + a) % 65521; }
        public void update(sbyte[] buf, int off, int len) { for (int i = 0; i < len; i++) update(buf[off + i]); }
        public void update(sbyte[] buf) => update(buf, 0, buf.Length);
        public long getValue() => ((long)b << 16) | a;
        public void reset() { a = 1; b = 0; }
    }
    public sealed class CRC32 : java.lang.Object, Checksum
    {
        static readonly uint[] T = Make();
        uint c = 0xffffffff;
        static uint[] Make() { var t = new uint[256]; for (uint n = 0; n < 256; n++) { uint x = n; for (int k = 0; k < 8; k++) x = (x & 1) != 0 ? 0xEDB88320u ^ (x >> 1) : x >> 1; t[n] = x; } return t; }
        public void update(int x) { c = T[(c ^ (uint)x) & 0xff] ^ (c >> 8); }
        public void update(sbyte[] buf, int off, int len) { for (int i = 0; i < len; i++) update(buf[off + i]); }
        public void update(sbyte[] buf) => update(buf, 0, buf.Length);
        public void update(java.nio.ByteBuffer bb) { while (bb.hasRemaining()) update(bb.get()); }
        public long getValue() => c ^ 0xffffffffL;
        public void reset() => c = 0xffffffff;
    }
    public class GZIPInputStream : java.io.InputStream
    {
        readonly System.IO.Stream s;
        public GZIPInputStream(java.io.InputStream i, int size) : this(i) { }
        public GZIPInputStream(java.io.InputStream i) { s = new System.IO.Compression.GZipStream(i.Stream(), System.IO.Compression.CompressionMode.Decompress); }
        public override int read() => s.ReadByte();
        internal override System.IO.Stream Stream() => s;
    }
    public class GZIPOutputStream : java.io.OutputStream
    {
        readonly System.IO.Compression.GZipStream s;
        public GZIPOutputStream(java.io.OutputStream o) : this(o, 512) { }
        public GZIPOutputStream(java.io.OutputStream o, int size) { s = new System.IO.Compression.GZipStream(new java.io.OutputStreamAdapter(o), System.IO.Compression.CompressionMode.Compress); }
        public override void write(int b) => s.WriteByte(unchecked((byte)b));
        public override void write(sbyte[] b) => write(b, 0, b.Length);
        public override void write(sbyte[] b, int o, int l) => s.Write((byte[])(Array)b, o, l);
        public void finish() => s.Flush();
        public override void flush() => s.Flush();
        public override void close() => s.Dispose();
    }
}

namespace java.util
{
    public class Properties : HashMap
    {
        public string getProperty(string k) => (string)get(k);
        public string getProperty(string k, string d) => (string)get(k) ?? d;
        public object setProperty(string k, string v) => put(k, v);
        public void load(java.io.InputStream i)
        {
            var bytes = new System.Collections.Generic.List<byte>();
            for (int b; (b = i.read()) >= 0;) bytes.Add((byte)b);
            var chars = new char[bytes.Count]; for (int k = 0; k < chars.Length; k++) chars[k] = (char)bytes[k]; Parse(new string(chars));
        }
        public void load(java.io.Reader r) => throw new System.IO.IOException("Properties.load(Reader) unsupported");
        /// <summary>Simple .properties parser: key=value / key:value lines, # and ! comments, \ escapes (no line continuations).</summary>
        void Parse(string text)
        {
            foreach (var raw in text.Split('\n'))
            {
                var line = raw.Trim(' ', '\t', '\r', '\f');
                if (line.Length == 0 || line[0] == '#' || line[0] == '!') continue;
                int sep = -1;
                for (int k = 0; k < line.Length; k++) { if (line[k] == '\\') { k++; continue; } if (line[k] == '=' || line[k] == ':') { sep = k; break; } }
                string key = sep < 0 ? line : line.Substring(0, sep).Trim(), val = sep < 0 ? "" : line.Substring(sep + 1).Trim();
                put(Unescape(key), Unescape(val));
            }
        }
        static string Unescape(string s) { if (s.IndexOf('\\') < 0) return s; var sb = new System.Text.StringBuilder(); for (int k = 0; k < s.Length; k++) { char c = s[k]; if (c == '\\' && k + 1 < s.Length) { c = s[++k]; c = c == 'n' ? '\n' : c == 't' ? '\t' : c == 'r' ? '\r' : c; } sb.Append(c); } return sb.ToString(); }
        static string Escape(string s, bool key) { var sb = new System.Text.StringBuilder(); foreach (char c in s) { if (c == '\\' || c == '=' || c == ':' || c == '#' || c == '!' || (key && c == ' ')) sb.Append('\\'); sb.Append(c == '\n' ? "\\n" : c == '\r' ? "\\r" : c == '\t' ? "\\t" : c.ToString()); } return sb.ToString(); }
        public void store(java.io.OutputStream o, string c)
        {
            var sb = new System.Text.StringBuilder();
            if (c != null) sb.Append('#').Append(c).Append('\n');
            sb.Append('#').Append(System.DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy", System.Globalization.CultureInfo.InvariantCulture)).Append('\n');
            foreach (var k in keySet()) sb.Append(Escape((string)k, true)).Append('=').Append(Escape((string)get(k), false)).Append('\n');
            var str = sb.ToString(); var bytes = new byte[str.Length]; for (int k = 0; k < bytes.Length; k++) bytes[k] = (byte)(str[k] < 256 ? str[k] : '?');
            o.write((sbyte[])(System.Array)bytes, 0, bytes.Length);
            o.flush();
        }
        public Set stringPropertyNames() { var s = new TreeSet(); foreach (var k in keySet()) s.add(k); return s; }
    }

    public interface Enumeration { bool hasMoreElements(); object nextElement(); }

    public class ResourceBundle : java.lang.Object
    {
        public static Func<string, string, string> Lookup = (bundle, key) => key;
        readonly string name;
        ResourceBundle(string n) { name = n; }
        public static ResourceBundle getBundle(string name) => new ResourceBundle(name);
        public static ResourceBundle getBundle(string name, Locale l) => new ResourceBundle(name);
        public string getString(string key) => Lookup(name, key);
        public bool containsKey(string key) => true;
    }
}

namespace java.time
{
    public sealed class Instant : java.lang.Object
    {
        internal readonly long ms;
        Instant(long ms) { this.ms = ms; }
        public static Instant now() => new Instant(java.lang.JSystem.currentTimeMillis());
        public long toEpochMilli() => ms;
        public static Instant ofEpochMilli(long m) => new Instant(m);
        public override string toString() => DateTimeOffset.FromUnixTimeMilliseconds(ms).ToString("o");
    }
    public sealed class Duration : java.lang.Object
    {
        readonly long ms;
        Duration(long ms) { this.ms = ms; }
        public static Duration between(Instant a, Instant b) => new Duration(b.ms - a.ms);
        public static Duration ofMillis(long m) => new Duration(m);
        public static Duration ofSeconds(long s) => new Duration(s * 1000);
        public long toMillis() => ms;
        public long getSeconds() => ms / 1000;
        public override string toString() => "PT" + (ms / 1000.0).ToString(System.Globalization.CultureInfo.InvariantCulture) + "S";
    }
}

namespace java.lang
{
    public sealed class StackTraceElement : Object
    {
        public string getClassName() => "?";
        public string getMethodName() => "?";
        public int getLineNumber() => -1;
    }
    public static class Thread_
    {
        public static void sleep(long ms) { }
    }
}

namespace java.io
{
    public abstract class InputStream : java.lang.Object, java.lang.AutoCloseable
    {
        public abstract int read();
        public virtual int read(sbyte[] b, int off, int len)
        {
            int n = 0;
            for (; n < len; n++) { int c = read(); if (c < 0) return n == 0 ? -1 : n; b[off + n] = unchecked((sbyte)c); }
            return n;
        }
        public virtual int read(sbyte[] b) => read(b, 0, b.Length);
        public virtual long transferTo(OutputStream o) { long n = 0; var b = new sbyte[8192]; int r; while ((r = read(b, 0, b.Length)) > 0) { o.write(b, 0, r); n += r; } return n; }
        public virtual sbyte[] readAllBytes() { var l = new List<sbyte>(); int c; while ((c = read()) >= 0) l.Add(unchecked((sbyte)c)); return l.ToArray(); }
        public virtual void close() { }
        public virtual int available() => 0;
        internal virtual System.IO.Stream Stream() => new Adapter(this);
        sealed class Adapter : System.IO.Stream
        {
            readonly InputStream i;
            public Adapter(InputStream i) { this.i = i; }
            public override bool CanRead => true; public override bool CanSeek => false; public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException(); public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
            public override void Flush() { }
            public override int Read(byte[] buffer, int offset, int count) { int n = 0; for (; n < count; n++) { int c = i.read(); if (c < 0) break; buffer[offset + n] = (byte)c; } return n; }
            public override long Seek(long o, System.IO.SeekOrigin s) => throw new NotSupportedException();
            public override void SetLength(long v) => throw new NotSupportedException();
            public override void Write(byte[] b, int o, int c) => throw new NotSupportedException();
        }
    }

    /// <summary>A java.io.OutputStream seen as a System.IO.Stream.</summary>
    public sealed class OutputStreamAdapter : System.IO.Stream
    {
        readonly OutputStream o;
        public OutputStreamAdapter(OutputStream o) { this.o = o; }
        public override bool CanRead => false; public override bool CanSeek => false; public override bool CanWrite => true;
        public override long Length => throw new NotSupportedException(); public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() => o.flush();
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long off, System.IO.SeekOrigin s) => throw new NotSupportedException();
        public override void SetLength(long v) => throw new NotSupportedException();
        public override void Write(byte[] b, int off, int c) => o.write((sbyte[])(Array)b, off, c);
        protected override void Dispose(bool d) { if (d) o.close(); base.Dispose(d); }
    }

    public class StreamInputStream : InputStream
    {
        readonly System.IO.Stream s;
        public StreamInputStream(System.IO.Stream s) { this.s = s; }
        public override int read() => s.ReadByte();
        public override int read(sbyte[] b, int off, int len) { if (len == 0) return 0; int n = s.Read((byte[])(Array)b, off, len); return n <= 0 ? -1 : n; }
        public override int available() { try { return s.CanSeek ? (int)System.Math.Min(int.MaxValue, s.Length - s.Position) : 0; } catch { return 0; } }
        public override void close() => s.Dispose();
        internal override System.IO.Stream Stream() => s;
    }

    public class ByteArrayInputStream : InputStream
    {
        readonly sbyte[] b; int p;
        public ByteArrayInputStream(sbyte[] b) { this.b = b; }
        public override int read() => p < b.Length ? b[p++] & 0xff : -1;
        public override int read(sbyte[] d, int off, int len) { if (len == 0) return 0; if (p >= b.Length) return -1; int n = System.Math.Min(len, b.Length - p); java.lang.JSystem.ArrCopy(b, p, d, off, n); p += n; return n; }
        public override int available() => b.Length - p;
        public long skip(long n) { int k = (int)System.Math.Max(0, System.Math.Min(n, b.Length - p)); p += k; return k; }
        public void reset() => p = 0;
    }

    public class OutputStream : java.lang.Object, java.lang.AutoCloseable
    {
        public virtual void write(int b) { }
        public virtual void write(sbyte[] b) { }
        public virtual void write(sbyte[] b, int o, int l) { }
        public virtual void flush() { }
        public virtual void close() { }
    }

    public class ByteArrayOutputStream : OutputStream
    {
        readonly List<sbyte> l = new List<sbyte>();
        public override void write(int b) => l.Add(unchecked((sbyte)b));
        public override void write(sbyte[] b, int o, int n) { for (int i = 0; i < n; i++) l.Add(b[o + i]); }
        public sbyte[] toByteArray() => l.ToArray();
        public int size() => l.Count;
    }

    public class BufferedInputStream : InputStream
    {
        readonly InputStream i;
        public BufferedInputStream(InputStream i) { this.i = i; }
        public BufferedInputStream(InputStream i, int size) { this.i = i; }
        public override int read() => i.read();
        public override int read(sbyte[] b, int off, int len) => i.read(b, off, len);
        public override int available() => i.available();
        public override void close() => i.close();
    }
    public class FileInputStream : StreamInputStream
    {
        public FileInputStream(File f) : base(Open(f.getPath())) { }
        public FileInputStream(string p) : base(Open(p)) { }
        static System.IO.Stream Open(string p) { try { return System.IO.File.OpenRead(p); } catch (Exception e) { throw new System.IO.IOException(e.Message); } }
    }
    public class StreamOutputStream : OutputStream
    {
        readonly System.IO.Stream s;
        public StreamOutputStream(System.IO.Stream s) { this.s = s; }
        public override void write(int b) => s.WriteByte(unchecked((byte)b));
        public override void write(sbyte[] b) => s.Write((byte[])(System.Array)b, 0, b.Length);
        public override void write(sbyte[] b, int o, int l) => s.Write((byte[])(System.Array)b, o, l);
        public override void flush() => s.Flush();
        public override void close() => s.Dispose();
    }
    /// <summary>File writing is off unless the host enables it (the Unity game does, for the AI profiles under its data folder).</summary>
    public class FileOutputStream : StreamOutputStream
    {
        public static bool WritesEnabled;
        public FileOutputStream(File f) : base(Create(f.getPath())) { }
        public FileOutputStream(string p) : base(Create(p)) { }
        internal static System.IO.Stream Create(string p, bool append = false)
        {
            if (!WritesEnabled) throw new System.IO.IOException("writing files is not enabled");
            try { return append ? new System.IO.FileStream(p, System.IO.FileMode.Append, System.IO.FileAccess.Write) : (System.IO.Stream)System.IO.File.Create(p); }
            catch (Exception e) { throw new System.IO.IOException(e.Message); }
        }
    }
    public class DataInputStream : InputStream
    {
        readonly InputStream i;
        public DataInputStream(InputStream i) { this.i = i; }
        public override int read() => i.read();
        public override int read(sbyte[] b, int off, int len) => i.read(b, off, len);
        public override int available() => i.available();
        int R() { int c = i.read(); if (c < 0) throw new System.IO.EndOfStreamException(); return c; }
        public int readInt() => (R() << 24) | (R() << 16) | (R() << 8) | R();
        public short readShort() => unchecked((short)((R() << 8) | R()));
        public int readUnsignedShort() => (R() << 8) | R();
        public sbyte readByte() => unchecked((sbyte)R());
        public int readUnsignedByte() => R();
        public bool readBoolean() => R() != 0;
        public long readLong() => ((long)readInt() << 32) | (readInt() & 0xffffffffL);
        public float readFloat() => java.lang.Float.intBitsToFloat(readInt());
        public double readDouble() => java.lang.Double.longBitsToDouble(readLong());
        public void readFully(sbyte[] b) { for (int k = 0; k < b.Length; k++) b[k] = readByte(); }
        public string readUTF() { int n = readUnsignedShort(); var b = new byte[n]; for (int k = 0; k < n; k++) b[k] = (byte)R(); return System.Text.Encoding.UTF8.GetString(b); }
        public override void close() => i.close();
    }
    public class DataOutputStream : OutputStream
    {
        readonly OutputStream o;
        int written;
        public DataOutputStream(OutputStream o) { this.o = o; }
        public override void write(int b) { o.write(b); written++; }
        public override void write(sbyte[] b) => write(b, 0, b.Length);
        public override void write(sbyte[] b, int off, int len) { o.write(b, off, len); written += len; }
        public void writeByte(int v) => write(v & 0xff);
        public void writeBoolean(bool v) => write(v ? 1 : 0);
        public void writeShort(int v) { write((v >> 8) & 0xff); write(v & 0xff); }
        public void writeChar(int v) => writeShort(v);
        public void writeInt(int v) { write((v >> 24) & 0xff); write((v >> 16) & 0xff); write((v >> 8) & 0xff); write(v & 0xff); }
        public void writeLong(long v) { writeInt((int)(v >> 32)); writeInt((int)v); }
        public void writeFloat(float v) => writeInt(java.lang.Float.floatToIntBits(v));
        public void writeDouble(double v) => writeLong(java.lang.Double.doubleToRawLongBits(v));
        public void writeUTF(string s)
        {
            var b = new System.Collections.Generic.List<byte>();
            foreach (char c in s)
            {
                if (c >= 1 && c <= 0x7f) b.Add((byte)c);
                else if (c <= 0x7ff) { b.Add((byte)(0xc0 | (c >> 6))); b.Add((byte)(0x80 | (c & 0x3f))); }
                else { b.Add((byte)(0xe0 | (c >> 12))); b.Add((byte)(0x80 | ((c >> 6) & 0x3f))); b.Add((byte)(0x80 | (c & 0x3f))); }
            }
            writeShort(b.Count);
            foreach (var x in b) write(x);
        }
        public void writeBytes(string s) { foreach (char c in s) write((byte)c); }
        public int size() => written;
        public override void flush() => o.flush();
        public override void close() { o.flush(); o.close(); }
    }
    public class BufferedOutputStream : OutputStream
    {
        readonly OutputStream o;
        public BufferedOutputStream(OutputStream o) { this.o = o; }
        public BufferedOutputStream(OutputStream o, int size) { this.o = o; }
        public override void write(int b) => o.write(b);
        public override void write(sbyte[] b) => o.write(b, 0, b.Length);
        public override void write(sbyte[] b, int off, int len) => o.write(b, off, len);
        public override void flush() => o.flush();
        public override void close() { o.flush(); o.close(); }
    }
    public abstract class Reader : java.lang.Object, java.lang.AutoCloseable { public virtual void close() { } }
    public sealed class File : java.lang.Object
    {
        public static readonly string separator = System.IO.Path.DirectorySeparatorChar.ToString();
        readonly string p;
        public File(string p) { this.p = p; }
        public File(File parent, string c) { p = System.IO.Path.Combine(parent.p, c); }
        public File(string parent, string c) { p = System.IO.Path.Combine(parent, c); }
        public string getPath() => p;
        public string getName() => System.IO.Path.GetFileName(p);
        public string getAbsolutePath() => System.IO.Path.GetFullPath(p);
        public bool exists() => System.IO.File.Exists(p) || System.IO.Directory.Exists(p);
        public bool isFile() => System.IO.File.Exists(p);
        public bool isDirectory() => System.IO.Directory.Exists(p);
        public bool mkdirs() { System.IO.Directory.CreateDirectory(p); return true; }
        public bool delete() { try { System.IO.File.Delete(p); return true; } catch { return false; } }
        public long length() => System.IO.File.Exists(p) ? new System.IO.FileInfo(p).Length : 0;
        public File getParentFile() { var d = System.IO.Path.GetDirectoryName(p); return d == null ? null : new File(d); }
        public java.nio.file.Path toPath() => java.nio.file.Path_.of(p);
        public override string toString() => p;
        public File[] listFiles() => System.IO.Directory.Exists(p) ? Array.ConvertAll(System.IO.Directory.GetFileSystemEntries(p), x => new File(x)) : null;
    }
}

namespace java.nio.file
{
    public sealed class Path : java.lang.Object, java.lang.Comparable
    {
        internal readonly string p;
        internal Path(string p) { this.p = p; }
        /// <summary>Host hook for LocalInput.getGameDir() (null = no game folder, as in the headless Java runs).</summary>
        public static string GameDirHook;
        public static Path GameDir => GameDirHook == null ? null : new Path(GameDirHook);
        public Path resolve(string o) => new Path(System.IO.Path.Combine(p, o));
        public Path resolve(Path o) => new Path(System.IO.Path.Combine(p, o.p));
        public Path getParent() { var d = System.IO.Path.GetDirectoryName(p); return string.IsNullOrEmpty(d) ? null : new Path(d); }
        public Path getFileName() => new Path(System.IO.Path.GetFileName(p));
        public Path toAbsolutePath() => new Path(System.IO.Path.GetFullPath(p));
        public Path normalize() => this;
        public java.io.File toFile() => new java.io.File(p);
        public java.net.URI toUri() => new java.net.URI("file://" + System.IO.Path.GetFullPath(p));
        public Path relativize(Path o) => new Path(System.IO.Path.GetRelativePath(p, o.p));
        public bool startsWith(Path o) => p.StartsWith(o.p, StringComparison.Ordinal);
        public bool endsWith(string s) => p.EndsWith(s, StringComparison.Ordinal);
        public int compareTo(object o) => string.CompareOrdinal(p, ((Path)o).p);
        public override bool equals(object o) => o is Path q && q.p == p;
        public override int hashCode() => java.lang.JString.hashCode(p);
        public override string toString() => p;
    }
    public static class Path_
    {
        public static Path of(string first, params string[] more) { var p = first; foreach (var m in more) p = System.IO.Path.Combine(p, m); return new Path(p); }
    }
    public static class Paths
    {
        public static Path get(string first, params string[] more) => Path_.of(first, more);
    }
    public interface OpenOption { }
    public sealed class StandardOpenOption : java.lang.Enum, OpenOption
    {
        public static readonly StandardOpenOption READ = new StandardOpenOption("READ", 0), WRITE = new StandardOpenOption("WRITE", 1),
            CREATE = new StandardOpenOption("CREATE", 2), TRUNCATE_EXISTING = new StandardOpenOption("TRUNCATE_EXISTING", 3), APPEND = new StandardOpenOption("APPEND", 4);
        StandardOpenOption(string n, int o) : base(n, o) { }
    }
    public sealed class StandardCopyOption : java.lang.Enum
    {
        public static readonly StandardCopyOption REPLACE_EXISTING = new StandardCopyOption("REPLACE_EXISTING", 0), COPY_ATTRIBUTES = new StandardCopyOption("COPY_ATTRIBUTES", 1),
            ATOMIC_MOVE = new StandardCopyOption("ATOMIC_MOVE", 2);
        StandardCopyOption(string n, int o) : base(n, o) { }
    }
    public sealed class LinkOption : java.lang.Enum
    {
        public static readonly LinkOption NOFOLLOW_LINKS = new LinkOption("NOFOLLOW_LINKS", 0);
        LinkOption(string n, int o) : base(n, o) { }
    }
    public static class Files
    {
        public static bool exists(Path p, params object[] o) => System.IO.File.Exists(p.p) || System.IO.Directory.Exists(p.p);
        public static bool isRegularFile(Path p, params object[] o) => System.IO.File.Exists(p.p);
        public static bool isDirectory(Path p, params object[] o) => System.IO.Directory.Exists(p.p);
        public static bool isReadable(Path p) => System.IO.File.Exists(p.p);
        public static java.io.InputStream newInputStream(Path p, params object[] o) => new java.io.FileInputStream(p.p);
        public static java.io.OutputStream newOutputStream(Path p, params object[] o) => new java.io.StreamOutputStream(java.io.FileOutputStream.Create(p.p, FilesExtra.Has(o, StandardOpenOption.APPEND)));
        public static java.util.stream.Stream list(Path p)
        {
            if (!System.IO.Directory.Exists(p.p)) throw new System.IO.IOException("NoSuchFileException: " + p.p);
            var l = new System.Collections.Generic.List<object>();
            foreach (var e in System.IO.Directory.GetFileSystemEntries(p.p)) l.Add(new Path(e));
            return new java.util.stream.Stream(l.ToArray());
        }
        public static attribute.FileTime getLastModifiedTime(Path p, params object[] o)
        {
            if (!System.IO.File.Exists(p.p) && !System.IO.Directory.Exists(p.p)) throw new System.IO.IOException("NoSuchFileException: " + p.p);
            return attribute.FileTime.fromMillis(new System.DateTimeOffset(System.IO.File.GetLastWriteTimeUtc(p.p)).ToUnixTimeMilliseconds());
        }
        public static java.nio.channels.SeekableByteChannel newByteChannel(Path p, params object[] o) => throw new System.IO.IOException("channels are not supported in the port");
        public static Path createDirectories(Path p, params object[] a)
        {
            // Java: Path.of("") is the current directory, which always exists
            if (string.IsNullOrWhiteSpace(p.p)) return p;
            try { System.IO.Directory.CreateDirectory(p.p); }
            catch (System.IO.IOException) { throw; }
            catch (System.Exception e) { throw new System.IO.IOException(e.Message, e); }   // Java only throws IOException here
            return p;
        }
        public static sbyte[] readAllBytes(Path p) => (sbyte[])(Array)System.IO.File.ReadAllBytes(p.p);
        public static string readString(Path p) => System.IO.File.ReadAllText(p.p);
        public static java.util.List readAllLines(Path p) { var l = new java.util.ArrayList(); foreach (var s in System.IO.File.ReadAllLines(p.p)) l.add(s); return l; }
        public static long size(Path p) => new System.IO.FileInfo(p.p).Length;
        public static java.io.BufferedWriter newBufferedWriter(Path p, params object[] o)
        {
            bool append = FilesExtra.Has(o, StandardOpenOption.APPEND);
            var stream = java.io.FileOutputStream.Create(p.p, append);
            return new java.io.BufferedWriter(new System.IO.StreamWriter(stream, new System.Text.UTF8Encoding(false)) { NewLine = "\n" });
        }
        public static Path writeString(Path p, object cs, params object[] o)
        {
            bool append = FilesExtra.Has(o, StandardOpenOption.APPEND);
            using (var stream = java.io.FileOutputStream.Create(p.p, append))
            {
                var b = new System.Text.UTF8Encoding(false).GetBytes(java.lang.JStr.valueOf(cs));
                stream.Write(b, 0, b.Length);
            }
            return p;
        }
        public static void delete(Path p) => System.IO.File.Delete(p.p);
        public static Path createTempFile(Path dir, string prefix, string suffix, params object[] a)
        {
            var f = System.IO.Path.Combine(dir.p, prefix + System.Guid.NewGuid().ToString("N").Substring(0, 12) + suffix);
            java.io.FileOutputStream.Create(f).Dispose();
            return new Path(f);
        }
        public static Path move(Path from, Path to, params object[] options)
        {
            if (!java.io.FileOutputStream.WritesEnabled) throw new System.IO.IOException("writing files is not enabled");
            if (FilesExtra.Has(options, StandardCopyOption.REPLACE_EXISTING) && System.IO.File.Exists(to.p)) System.IO.File.Delete(to.p);
            System.IO.File.Move(from.p, to.p);
            return to;
        }
        public static java.util.stream.Stream walk(Path p, params object[] o) => throw new System.IO.IOException("Files.walk is not supported in the port");
        public static bool deleteIfExists(Path p) { if (!System.IO.File.Exists(p.p)) return false; System.IO.File.Delete(p.p); return true; }
    }
    namespace attribute
    {
        public interface FileAttribute { }
        public sealed class FileTime : java.lang.Object, java.lang.Comparable
        {
            readonly long ms;
            FileTime(long ms) { this.ms = ms; }
            public static FileTime fromMillis(long ms) => new FileTime(ms);
            public long toMillis() => ms;
            public int compareTo(object o) => ms.CompareTo(((FileTime)o).ms);
            public override bool equals(object o) => o is FileTime f && f.ms == ms;
            public override int hashCode() => ms.GetHashCode();
            public override string toString() => System.DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}

namespace java.nio.channels
{
    public interface Channel : java.lang.AutoCloseable { bool isOpen(); }
    public interface ReadableByteChannel : Channel { int read(java.nio.ByteBuffer b); }
    public interface WritableByteChannel : Channel { int write(java.nio.ByteBuffer b); }
    public interface ByteChannel : ReadableByteChannel, WritableByteChannel { }
    public interface SeekableByteChannel : ByteChannel { long size(); long position(); }
    public abstract class FileChannel : java.lang.Object, SeekableByteChannel
    {
        public static FileChannel open(java.nio.file.Path p, params object[] o) => throw new System.IO.IOException("channels are not supported in the port");
        public abstract bool isOpen(); public abstract void close(); public abstract int read(java.nio.ByteBuffer b); public abstract int write(java.nio.ByteBuffer b);
        public abstract long size(); public abstract long position();
    }
    public static class Channels
    {
        public static ReadableByteChannel newChannel(java.io.InputStream i) => throw new System.IO.IOException("channels are not supported in the port");
        public static WritableByteChannel newChannel(java.io.OutputStream o) => throw new System.IO.IOException("channels are not supported in the port");
    }
}

namespace java.nio
{
    public sealed class ByteOrder : java.lang.Object
    {
        public static readonly ByteOrder BIG_ENDIAN = new ByteOrder(), LITTLE_ENDIAN = new ByteOrder();
        public static ByteOrder nativeOrder() => LITTLE_ENDIAN;
    }

    public abstract class Buffer : java.lang.Object
    {
        protected int pos, lim, cap;
        public int position() => pos;
        public int limit() => lim;
        public int capacity() => cap;
        public int remaining() => lim - pos;
        public bool hasRemaining() => pos < lim;
        public Buffer position(int p) { pos = p; return this; }
        public Buffer limit(int l) { lim = l; if (pos > l) pos = l; return this; }
        public Buffer flip() { lim = pos; pos = 0; return this; }
        public Buffer rewind() { pos = 0; return this; }
        public Buffer clear() { pos = 0; lim = cap; return this; }
    }

    public sealed class ByteBuffer : Buffer
    {
        internal readonly sbyte[] a;
        internal readonly int off;
        bool little;
        ByteBuffer(sbyte[] a, int off, int cap) { this.a = a; this.off = off; this.cap = cap; lim = cap; }
        public static ByteBuffer allocate(int n) => new ByteBuffer(new sbyte[n], 0, n);
        public static ByteBuffer allocateDirect(int n) => allocate(n);
        public static ByteBuffer wrap(sbyte[] b) => new ByteBuffer(b, 0, b.Length);
        public static ByteBuffer wrap(sbyte[] b, int o, int n) { var r = new ByteBuffer(b, 0, b.Length); r.pos = o; r.lim = o + n; return r; }
        public ByteBuffer order(ByteOrder o) { little = o == ByteOrder.LITTLE_ENDIAN; return this; }
        public ByteOrder order() => little ? ByteOrder.LITTLE_ENDIAN : ByteOrder.BIG_ENDIAN;
        public sbyte[] array() => a;
        public bool isDirect() => false;
        public sbyte get() => a[off + pos++];
        public sbyte get(int i) => a[off + i];
        public ByteBuffer get(sbyte[] d) { for (int i = 0; i < d.Length; i++) d[i] = get(); return this; }
        public ByteBuffer get(sbyte[] d, int o, int n) { if (n > remaining()) throw new BufferUnderflowException(); java.lang.JSystem.ArrCopy(a, off + pos, d, o, n); pos += n; return this; }
        public ByteBuffer put(sbyte[] s, int o, int n) { if (n > remaining()) throw new BufferOverflowException(); java.lang.JSystem.ArrCopy(s, o, a, off + pos, n); pos += n; return this; }
        public short getShort(int i) { int b0 = a[off + i] & 0xff, b1 = a[off + i + 1] & 0xff; return unchecked((short)(little ? (b1 << 8) | b0 : (b0 << 8) | b1)); }
        public ByteBuffer putShort(int i, short s) { int p = pos; pos = i; putShort(s); pos = p; return this; }
        public double getDouble() => java.lang.Double.longBitsToDouble(getLong());
        public ByteBuffer putDouble(double d) => putLong(java.lang.Double.doubleToRawLongBits(d));
        public long getLong(int i) { int p = pos; pos = i; long v = getLong(); pos = p; return v; }
        public bool hasArray() => true;
        public int arrayOffset() => off;
        public ByteBuffer put(sbyte b) { a[off + pos++] = b; return this; }
        public ByteBuffer put(int i, sbyte b) { a[off + i] = b; return this; }
        public ByteBuffer put(sbyte[] s) { foreach (var b in s) put(b); return this; }
        public ByteBuffer put(ByteBuffer s) { while (s.hasRemaining()) put(s.get()); return this; }
        int RI(int i) { int b0 = a[off + i] & 0xff, b1 = a[off + i + 1] & 0xff, b2 = a[off + i + 2] & 0xff, b3 = a[off + i + 3] & 0xff; return little ? (b3 << 24) | (b2 << 16) | (b1 << 8) | b0 : (b0 << 24) | (b1 << 16) | (b2 << 8) | b3; }
        void WI(int i, int v) { if (little) { a[off + i] = (sbyte)v; a[off + i + 1] = (sbyte)(v >> 8); a[off + i + 2] = (sbyte)(v >> 16); a[off + i + 3] = (sbyte)(v >> 24); } else { a[off + i] = (sbyte)(v >> 24); a[off + i + 1] = (sbyte)(v >> 16); a[off + i + 2] = (sbyte)(v >> 8); a[off + i + 3] = (sbyte)v; } }
        public int getInt() { int v = RI(pos); pos += 4; return v; }
        public int getInt(int i) => RI(i);
        public ByteBuffer putInt(int v) { WI(pos, v); pos += 4; return this; }
        public ByteBuffer putInt(int i, int v) { WI(i, v); return this; }
        public float getFloat() => java.lang.Float.intBitsToFloat(getInt());
        public float getFloat(int i) => java.lang.Float.intBitsToFloat(RI(i));
        public ByteBuffer putFloat(float f) => putInt(java.lang.Float.floatToRawIntBits(f));
        public ByteBuffer putFloat(int i, float f) => putInt(i, java.lang.Float.floatToRawIntBits(f));
        public short getShort() { int b0 = a[off + pos] & 0xff, b1 = a[off + pos + 1] & 0xff; pos += 2; return unchecked((short)(little ? (b1 << 8) | b0 : (b0 << 8) | b1)); }
        public ByteBuffer putShort(short s) { if (little) { a[off + pos] = (sbyte)s; a[off + pos + 1] = (sbyte)(s >> 8); } else { a[off + pos] = (sbyte)(s >> 8); a[off + pos + 1] = (sbyte)s; } pos += 2; return this; }
        public ByteBuffer compact() { int n = lim - pos; java.lang.JSystem.ArrCopy(a, off + pos, a, off, n); pos = n; lim = cap; return this; }
        public char getChar() => (char)getShort();
        public ByteBuffer putChar(char c) => putShort(unchecked((short)c));
        public ByteBuffer putLong(long v) { if (little) { putInt((int)v); putInt((int)(v >> 32)); } else { putInt((int)(v >> 32)); putInt((int)v); } return this; }
        public long getLong() { long hi = getInt(), lo = getInt(); return little ? (lo << 32) | (hi & 0xffffffffL) : (hi << 32) | (lo & 0xffffffffL); }
        public FloatBuffer asFloatBuffer() => new FloatBuffer(this, pos, (lim - pos) / 4);
        public IntBuffer asIntBuffer() => new IntBuffer(this, pos, (lim - pos) / 4);
        public ShortBuffer asShortBuffer() => new ShortBuffer(remaining() / 2);
        public ByteBuffer slice() { var r = new ByteBuffer(a, off + pos, lim - pos); r.little = little; return r; }
        public ByteBuffer duplicate() { var r = new ByteBuffer(a, off, cap); r.pos = pos; r.lim = lim; r.little = little; return r; }
        public new ByteBuffer flip() { base.flip(); return this; }
        public new ByteBuffer rewind() { base.rewind(); return this; }
        public new ByteBuffer clear() { base.clear(); return this; }
        public new ByteBuffer position(int p) { base.position(p); return this; }
        public new ByteBuffer limit(int l) { base.limit(l); return this; }
    }

    public sealed class FloatBuffer : Buffer
    {
        readonly ByteBuffer bb; readonly int start; readonly float[] own;
        internal FloatBuffer(ByteBuffer bb, int start, int n) { this.bb = bb; this.start = start; cap = lim = n; }
        FloatBuffer(float[] a) { own = a; cap = lim = a.Length; }
        public static FloatBuffer allocate(int n) => new FloatBuffer(new float[n]);
        public static FloatBuffer wrap(float[] a) => new FloatBuffer(a);
        public float get(int i) => own != null ? own[i] : bb.getFloat(start + i * 4);
        public float get() => get(pos++);
        public FloatBuffer put(int i, float f) { if (own != null) own[i] = f; else bb.putFloat(start + i * 4, f); return this; }
        public FloatBuffer put(float f) { put(pos++, f); return this; }
        public FloatBuffer put(float[] fs) { foreach (var f in fs) put(f); return this; }
        public FloatBuffer put(float[] fs, int off, int len) { for (int i = 0; i < len; i++) put(fs[off + i]); return this; }
        public FloatBuffer get(float[] d) { for (int i = 0; i < d.Length; i++) d[i] = get(); return this; }
        public new FloatBuffer flip() { base.flip(); return this; }
        public new FloatBuffer rewind() { base.rewind(); return this; }
        public new FloatBuffer clear() { base.clear(); return this; }
        public new FloatBuffer position(int p) { base.position(p); return this; }
        public new FloatBuffer limit(int l) { base.limit(l); return this; }
    }

    public sealed class IntBuffer : Buffer
    {
        readonly ByteBuffer bb; readonly int start; readonly int[] own;
        internal IntBuffer(ByteBuffer bb, int start, int n) { this.bb = bb; this.start = start; cap = lim = n; }
        IntBuffer(int[] a) { own = a; cap = lim = a.Length; }
        public static IntBuffer allocate(int n) => new IntBuffer(new int[n]);
        public static IntBuffer wrap(int[] a) => new IntBuffer(a);
        public int get(int i) => own != null ? own[i] : bb.getInt(start + i * 4);
        public int get() => get(pos++);
        public IntBuffer put(int i, int v) { if (own != null) own[i] = v; else bb.putInt(start + i * 4, v); return this; }
        public IntBuffer put(int v) { put(pos++, v); return this; }
        public IntBuffer put(int[] vs) { foreach (var v in vs) put(v); return this; }
        public IntBuffer get(int[] d) { for (int i = 0; i < d.Length; i++) d[i] = get(); return this; }
        public IntBuffer get(int index, int[] d, int off, int len) { for (int i = 0; i < len; i++) d[off + i] = get(index + i); return this; }
        public new IntBuffer flip() { base.flip(); return this; }
        public new IntBuffer rewind() { base.rewind(); return this; }
        public new IntBuffer clear() { base.clear(); return this; }
        public new IntBuffer position(int p) { base.position(p); return this; }
    }

    public sealed class ShortBuffer : Buffer
    {
        readonly short[] a;
        internal ShortBuffer(int n) { a = new short[n]; cap = lim = n; }
        public static ShortBuffer allocate(int n) => new ShortBuffer(n);
        public short get(int i) => a[i];
        public short get() => a[pos++];
        public ShortBuffer put(short s) { a[pos++] = s; return this; }
        public ShortBuffer put(int i, short s) { a[i] = s; return this; }
        public ShortBuffer put(short[] ss) { foreach (var s in ss) put(s); return this; }
        public new ShortBuffer flip() { base.flip(); return this; }
        public new ShortBuffer rewind() { base.rewind(); return this; }
    }
}

namespace java.net
{
    public sealed class URL : java.lang.Object
    {
        readonly string s;
        public static Func<string, string> ResourceRoot = n => null;
        public URL(string s) { this.s = s; }
        internal static URL Resolve(string name) { var p = ResourceRoot(name); return p == null ? null : new URL(p); }
        public java.io.InputStream openStream()
        {
            if (s.StartsWith("res:"))
            {
                // class-path resources of the port: provided by the host (Unity Resources)
                var bytes = java.lang.JResources.Open?.Invoke(s.Substring(4));
                if (bytes == null) throw new System.IO.IOException("resource not found: " + s.Substring(4));
                return new java.io.ByteArrayInputStream((sbyte[])(System.Array)bytes);
            }
            return new java.io.FileInputStream(s.StartsWith("file:") ? new Uri(s).LocalPath : s);
        }
        public string getPath() => s;
        public string getFile() => s;
        public string getProtocol() => s.Contains(":") ? s.Substring(0, s.IndexOf(':')) : "file";
        public URI toURI() => new URI(s);
        public override string toString() => s;
        public override bool equals(object o) => o is URL u && u.s == s;
        public override int hashCode() => java.lang.JString.hashCode(s);
    }
    public sealed class URI : java.lang.Object
    {
        readonly string s;
        public URI(string s) { this.s = s; }
        public static URI create(string s) => new URI(s);
        public URL toURL() => new URL(s);
        public string getPath() => s;
        public string toASCIIString() => s;
        public string getScheme() => s.Contains(":") ? s.Substring(0, s.IndexOf(':')) : null;
        public override string toString() => s;
        public override bool equals(object o) => o is URI u && u.s == s;
        public override int hashCode() => java.lang.JString.hashCode(s);
    }
}

namespace java.awt
{
    public static class Color
    {
        public static float[] RGBtoHSB(int r, int g, int b, float[] hsbvals)
        {
            float hue, saturation, brightness;
            if (hsbvals == null) hsbvals = new float[3];
            int cmax = System.Math.Max(System.Math.Max(r, g), b), cmin = System.Math.Min(System.Math.Min(r, g), b);
            brightness = cmax / 255.0f;
            saturation = cmax != 0 ? (float)(cmax - cmin) / cmax : 0;
            if (saturation == 0) hue = 0;
            else
            {
                float redc = (float)(cmax - r) / (cmax - cmin), greenc = (float)(cmax - g) / (cmax - cmin), bluec = (float)(cmax - b) / (cmax - cmin);
                if (r == cmax) hue = bluec - greenc;
                else if (g == cmax) hue = 2.0f + redc - bluec;
                else hue = 4.0f + greenc - redc;
                hue = hue / 6.0f;
                if (hue < 0) hue = hue + 1.0f;
            }
            hsbvals[0] = hue; hsbvals[1] = saturation; hsbvals[2] = brightness;
            return hsbvals;
        }

        /// <summary>java.awt.Color.HSBtoRGB (same arithmetic as the JDK).</summary>
        public static int HSBtoRGB(float hue, float saturation, float brightness)
        {
            int r = 0, g = 0, b = 0;
            if (saturation == 0) { r = g = b = (int)((float)(brightness * 255.0f) + 0.5f); }
            else
            {
                float h = (float)((float)(hue - (float)System.Math.Floor(hue)) * 6.0f);
                float f = (float)(h - (float)System.Math.Floor(h));
                float p = (float)(brightness * (float)(1.0f - saturation));
                float q = (float)(brightness * (float)(1.0f - (float)(saturation * f)));
                float t = (float)(brightness * (float)(1.0f - (float)(saturation * (float)(1.0f - f))));
                switch ((int)h)
                {
                    case 0: r = (int)((float)(brightness * 255.0f) + 0.5f); g = (int)((float)(t * 255.0f) + 0.5f); b = (int)((float)(p * 255.0f) + 0.5f); break;
                    case 1: r = (int)((float)(q * 255.0f) + 0.5f); g = (int)((float)(brightness * 255.0f) + 0.5f); b = (int)((float)(p * 255.0f) + 0.5f); break;
                    case 2: r = (int)((float)(p * 255.0f) + 0.5f); g = (int)((float)(brightness * 255.0f) + 0.5f); b = (int)((float)(t * 255.0f) + 0.5f); break;
                    case 3: r = (int)((float)(p * 255.0f) + 0.5f); g = (int)((float)(q * 255.0f) + 0.5f); b = (int)((float)(brightness * 255.0f) + 0.5f); break;
                    case 4: r = (int)((float)(t * 255.0f) + 0.5f); g = (int)((float)(p * 255.0f) + 0.5f); b = (int)((float)(brightness * 255.0f) + 0.5f); break;
                    case 5: r = (int)((float)(brightness * 255.0f) + 0.5f); g = (int)((float)(p * 255.0f) + 0.5f); b = (int)((float)(q * 255.0f) + 0.5f); break;
                }
            }
            return unchecked((int)0xff000000) | (r << 16) | (g << 8) | (b << 0);
        }
    }
    namespace image
    {
        public class BufferedImage : java.lang.Object { public const int TYPE_INT_ARGB = 2, TYPE_INT_RGB = 1; public BufferedImage(int w, int h, int t) { } }
        public interface RenderedImage { }
        public class WritableRaster : java.lang.Object { }
    }
}

namespace java.text
{
    public static class MessageFormat
    {
        public static string format(string pattern, params object[] args)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < pattern.Length; i++)
            {
                char c = pattern[i];
                if (c == '\'' ) { int j = pattern.IndexOf('\'', i + 1); if (j == i + 1) { sb.Append('\''); i++; continue; } if (j < 0) j = pattern.Length; sb.Append(pattern, i + 1, j - i - 1); i = j; continue; }
                if (c == '{')
                {
                    int j = pattern.IndexOf('}', i);
                    var spec = pattern.Substring(i + 1, j - i - 1).Split(',');
                    int idx = int.Parse(spec[0].Trim());
                    sb.Append(idx < args.Length ? java.lang.JStr.valueOf(args[idx]) : "{" + idx + "}");
                    i = j; continue;
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
    public abstract class NumberFormat : java.lang.Object { public abstract string format(double d); }
}

namespace java.util.regex
{
    public sealed class Pattern : java.lang.Object
    {
        internal readonly System.Text.RegularExpressions.Regex rx;
        Pattern(string p) { rx = new System.Text.RegularExpressions.Regex(p); }
        public static Pattern compile(string p) => new Pattern(p);
        public Matcher matcher(string s) => new Matcher(this, s);
        public static bool matches(string p, string s) => System.Text.RegularExpressions.Regex.IsMatch(s, "^(?:" + p + ")$");
    }
    public sealed class Matcher : java.lang.Object
    {
        readonly Pattern p; readonly string s; System.Text.RegularExpressions.Match m;
        internal Matcher(Pattern p, string s) { this.p = p; this.s = s; }
        public bool matches() { m = System.Text.RegularExpressions.Regex.Match(s, "^(?:" + p.rx + ")$"); return m.Success; }
        public bool find() { m = m == null ? p.rx.Match(s) : m.NextMatch(); return m.Success; }
        public string group(int i) => m.Groups[i].Value;
        public string group() => m.Value;
    }
}
