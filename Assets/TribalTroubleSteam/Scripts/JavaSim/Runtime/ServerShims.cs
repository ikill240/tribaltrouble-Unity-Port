// Small JDK pieces the embedded matchmaking server needs (java.sql value types, FileWriter, URLEncoder).
using System;
namespace java.sql
{
    public class SQLException : System.Exception
    {
        public SQLException() { }
        public SQLException(string m) : base(m) { }
    }
    public class Timestamp : java.lang.Object
    {
        readonly long t;
        public Timestamp(long millis) { t = millis; }
        public long getTime() => t;
        public override string toString() => DateTimeOffset.FromUnixTimeMilliseconds(t).UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        public override bool equals(object o) => o is Timestamp x && x.t == t;
        public override int hashCode() => t.GetHashCode();
    }
    public interface Connection { }
    public interface PreparedStatement { }
}
namespace java.io
{
    public class FileWriter : Writer_Base
    {
        readonly System.IO.StreamWriter w;
        public FileWriter(File f) : this(f.getPath(), false) { }
        public FileWriter(File f, bool append) : this(f.getPath(), append) { }
        public FileWriter(string path) : this(path, false) { }
        public FileWriter(string path, bool append)
        {
            try { w = new System.IO.StreamWriter(path, append, new System.Text.UTF8Encoding(false)); }
            catch (Exception e) { throw new System.IO.IOException(e.Message); }
        }
        public override void write(string s) => w.Write(s);
        public override void flush() => w.Flush();
        public override void close() => w.Dispose();
    }
    public abstract class Writer_Base : java.lang.Object, java.lang.AutoCloseable
    {
        public abstract void write(string s);
        public abstract void flush();
        public abstract void close();
    }
}
namespace java.net
{
    public static class URLEncoder
    {
        public static string encode(string s, java.nio.charset.Charset cs) => Encode(s);
        public static string encode(string s, string enc) => Encode(s);
        static string Encode(string s)
        {
            var sb = new System.Text.StringBuilder();
            foreach (byte b in System.Text.Encoding.UTF8.GetBytes(s))
            {
                char c = (char)b;
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '.' || c == '-' || c == '*' || c == '_') sb.Append(c);
                else if (c == ' ') sb.Append('+');
                else sb.Append('%').Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
