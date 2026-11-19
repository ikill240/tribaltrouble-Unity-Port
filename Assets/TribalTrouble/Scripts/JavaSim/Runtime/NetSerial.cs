// Java Object Serialization Stream Protocol (java.io.ObjectOutputStream / ObjectInputStream), driven by class
// descriptors generated from the JDK's ObjectStreamClass (j2cs/armi/GenNet.java -> NetTables.cs). This is what ARMI
// uses for every non-primitive argument, so the port's bytes must be readable by the Java servers and vice versa.
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace J2CS.Net
{
    public sealed class JFieldDesc
    {
        public readonly char code;
        public readonly string name, typeString;
        public JFieldDesc(char code, string name, string typeString) { this.code = code; this.name = name; this.typeString = typeString; }
        public bool IsPrimitive => code != 'L' && code != '[';
    }

    public sealed class JClassDesc
    {
        public const int SC_WRITE_METHOD = 0x01, SC_SERIALIZABLE = 0x02, SC_EXTERNALIZABLE = 0x04, SC_BLOCK_DATA = 0x08, SC_ENUM = 0x10;
        public readonly string name;
        public System.Type type;
        public readonly long suid;
        public readonly int flags;
        public readonly string superName;
        public readonly JFieldDesc[] fields;
        internal JClassDesc super;          // resolved on the stream side, or from superName locally
        internal FieldInfo[] infos;         // C# fields matching 'fields' (null entries for missing)

        public JClassDesc(string name, System.Type type, long suid, int flags, string superName, JFieldDesc[] fields)
        {
            this.name = name; this.type = type; this.suid = suid; this.flags = flags; this.superName = superName; this.fields = fields;
        }

        public bool IsEnum => (flags & SC_ENUM) != 0;

        internal FieldInfo[] Infos()
        {
            if (infos != null || type == null) return infos;
            var r = new FieldInfo[fields.Length];
            const BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            for (int i = 0; i < fields.Length; i++)
            {
                string n = fields[i].name;
                r[i] = type.GetField(n, bf) ?? type.GetField(n + "_f", bf) ?? type.GetField(n + "_", bf);
            }
            return infos = r;
        }
    }

    /// <summary>Descriptor registry: Java class name -> descriptor, and C# type -> descriptor.</summary>
    public static class JSerial
    {
        static readonly Dictionary<string, JClassDesc> byName = new Dictionary<string, JClassDesc>();
        static readonly Dictionary<System.Type, JClassDesc> byType = new Dictionary<System.Type, JClassDesc>();

        /// <summary>JDK classes the runtime implements itself.</summary>
        static readonly Dictionary<string, System.Type> jdk = new Dictionary<string, System.Type>
        {
            { "javax.crypto.SealedObject", typeof(javax.crypto.SealedObject) },
            { "java.security.SignedObject", typeof(java.security.SignedObject) },
        };

        public static void Register(JClassDesc d)
        {
            lock (byName)
            {
                if (d.type == null && jdk.TryGetValue(d.name, out var t)) d.type = t;
                byName[d.name] = d;
                if (d.type != null && !d.name.StartsWith("[")) byType[d.type] = d;
            }
        }

        public static void EnsureRegistered()
        {
            lock (byName) { if (byName.Count > 0) return; }
            JReflect.EnsureRegistered();
        }

        public static JClassDesc ByName(string n)
        {
            EnsureRegistered();
            lock (byName) return byName.TryGetValue(n, out var d) ? d : null;
        }

        public static JClassDesc ForType(System.Type t)
        {
            EnsureRegistered();
            lock (byName)
            {
                for (var x = t; x != null && x != typeof(object); x = x.BaseType)
                    if (byType.TryGetValue(x, out var d)) return d;
            }
            return null;
        }

        static readonly Dictionary<System.Type, JClassDesc> classOnly = new Dictionary<System.Type, JClassDesc>();

        /// <summary>The descriptor Java writes for a non-Serializable class object.</summary>
        public static JClassDesc ClassOnlyDesc(System.Type t)
        {
            lock (classOnly)
            {
                if (!classOnly.TryGetValue(t, out var d))
                    classOnly[t] = d = new JClassDesc(JavaName(t), t, 0, 0, null, new JFieldDesc[0]);
                return d;
            }
        }

        /// <summary>Java binary name of a translated type (namespaces mirror the Java packages; nested types use $).</summary>
        public static string JavaName(System.Type t) => t.FullName.Replace('+', '$');

        /// <summary>A translated type by its Java name (for Class objects arriving on the wire).</summary>
        public static System.Type TypeForName(string n)
        {
            if (n == null) return null;
            var d = ByName(n);
            if (d?.type != null) return d.type;
            string cs = n.Replace('$', '+');
            foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = a.GetType(cs, false);
                if (t != null) return t;
            }
            return null;
        }

        public static JClassDesc Super(JClassDesc d) => d.super ?? (d.superName == null ? null : d.super = ByName(d.superName));

        /// <summary>Java class name of a C# array type ("[B", "[Ljava.lang.String;", "[Lcom.oddlabs.x.Y;").</summary>
        public static string ArrayName(System.Type t)
        {
            var e = t.GetElementType();
            string en;
            if (e == typeof(sbyte) || e == typeof(byte)) en = "B";
            else if (e == typeof(int)) en = "I";
            else if (e == typeof(short)) en = "S";
            else if (e == typeof(long)) en = "J";
            else if (e == typeof(float)) en = "F";
            else if (e == typeof(double)) en = "D";
            else if (e == typeof(bool)) en = "Z";
            else if (e == typeof(char)) en = "C";
            else if (e.IsArray) en = ArrayName(e);
            else if (e == typeof(string)) en = "Ljava.lang.String;";
            else if (e == typeof(object)) en = "Ljava.lang.Object;";
            else
            {
                var d = ForType(e);
                if (d != null) en = "L" + d.name + ";";
                else if (typeof(java.net.InetAddress).IsAssignableFrom(e)) en = "Ljava.net.InetAddress;";
                else throw new System.IO.IOException("NotSerializableException: " + e.FullName + "[]");
            }
            return "[" + en;
        }

        /// <summary>C# array element type for a Java array class name.</summary>
        public static System.Type ElementType(string arrayName)
        {
            string e = arrayName.Substring(1);
            switch (e[0])
            {
                case 'B': return typeof(sbyte);
                case 'I': return typeof(int);
                case 'S': return typeof(short);
                case 'J': return typeof(long);
                case 'F': return typeof(float);
                case 'D': return typeof(double);
                case 'Z': return typeof(bool);
                case 'C': return typeof(char);
                case '[': return ElementType(e).MakeArrayType();
                case 'L':
                    string n = e.Substring(1, e.Length - 2);
                    if (n == "java.lang.String") return typeof(string);
                    if (n == "java.lang.Object") return typeof(object);
                    if (n == "java.net.InetAddress") return typeof(java.net.InetAddress);
                    var d = ByName(n);
                    return d?.type ?? typeof(object);
            }
            throw new System.IO.IOException("bad array class " + arrayName);
        }

        internal static object Prim(char code, object v)
        {
            switch (code)
            {
                case 'B': return v is sbyte ? v : unchecked((sbyte)Convert.ToInt64(v));
                case 'C': return v is char ? v : (char)Convert.ToInt32(v);
                case 'D': return v is double ? v : Convert.ToDouble(v);
                case 'F': return v is float ? v : Convert.ToSingle(v);
                case 'I': return v is int ? v : unchecked((int)Convert.ToInt64(v));
                case 'J': return v is long ? v : Convert.ToInt64(v);
                case 'S': return v is short ? v : unchecked((short)Convert.ToInt64(v));
                case 'Z': return v is bool ? v : Convert.ToBoolean(v);
            }
            throw new ArgumentException(code.ToString());
        }

        /// <summary>Instance without running constructors - what Java deserialization does for Serializable classes.</summary>
        internal static object NewInstance(System.Type t) => System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t);

        internal static object EnumConstant(System.Type t, string name)
        {
            foreach (var f in t.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                if (t.IsAssignableFrom(f.FieldType) && f.GetValue(null) is java.lang.Enum e && e.name() == name) return e;
            throw new System.IO.IOException("InvalidObjectException: enum constant " + name + " does not exist in " + t.FullName);
        }
    }

    sealed class RefEq : IEqualityComparer<object>
    {
        public static readonly RefEq I = new RefEq();
        public new bool Equals(object a, object b) => ReferenceEquals(a, b);
        public int GetHashCode(object o) => RuntimeHelpers.GetHashCode(o);
    }

    /// <summary>Java's modified UTF-8.</summary>
    static class MUtf8
    {
        public static byte[] Encode(string s)
        {
            var b = new List<byte>(s.Length);
            foreach (char c in s)
            {
                if (c >= 1 && c <= 0x7f) b.Add((byte)c);
                else if (c <= 0x7ff) { b.Add((byte)(0xc0 | (c >> 6))); b.Add((byte)(0x80 | (c & 0x3f))); }
                else { b.Add((byte)(0xe0 | (c >> 12))); b.Add((byte)(0x80 | ((c >> 6) & 0x3f))); b.Add((byte)(0x80 | (c & 0x3f))); }
            }
            return b.ToArray();
        }

        public static string Decode(byte[] b)
        {
            var sb = new System.Text.StringBuilder(b.Length);
            for (int i = 0; i < b.Length;)
            {
                int c = b[i];
                if (c < 0x80) { sb.Append((char)c); i++; }
                else if ((c & 0xe0) == 0xc0) { sb.Append((char)(((c & 0x1f) << 6) | (b[i + 1] & 0x3f))); i += 2; }
                else { sb.Append((char)(((c & 0x0f) << 12) | ((b[i + 1] & 0x3f) << 6) | (b[i + 2] & 0x3f))); i += 3; }
            }
            return sb.ToString();
        }
    }

    static class TC
    {
        public const byte NULL = 0x70, REFERENCE = 0x71, CLASSDESC = 0x72, OBJECT = 0x73, STRING = 0x74, ARRAY = 0x75, CLASS = 0x76,
            BLOCKDATA = 0x77, ENDBLOCKDATA = 0x78, RESET = 0x79, BLOCKDATALONG = 0x7A, EXCEPTION = 0x7B, LONGSTRING = 0x7C,
            PROXYCLASSDESC = 0x7D, ENUM = 0x7E;
        public const int BASE_HANDLE = 0x7E0000;
    }
}

namespace java.io
{
    using J2CS.Net;

    public class ObjectOutputStream : OutputStream
    {
        readonly OutputStream @out;
        readonly System.IO.MemoryStream block = new System.IO.MemoryStream();
        readonly Dictionary<object, int> handles = new Dictionary<object, int>(RefEq.I);
        readonly Dictionary<string, int> typeStrings = new Dictionary<string, int>();
        int nextHandle;
        readonly byte[] tmp = new byte[8];

        public ObjectOutputStream(OutputStream o)
        {
            @out = o;
            Raw16(0xACED);
            Raw16(5);
        }

        // ---------------------------------------------------------------- raw (non-block) writes
        void Raw(int b) => @out.write(b & 0xff);
        void Raw16(int v) { Raw(v >> 8); Raw(v); }
        void Raw32(int v) { Raw(v >> 24); Raw(v >> 16); Raw(v >> 8); Raw(v); }
        void Raw64(long v) { Raw32((int)(v >> 32)); Raw32((int)v); }
        void RawBytes(byte[] b) => @out.write((sbyte[])(System.Array)b, 0, b.Length);
        void RawUTF(string s) { var b = MUtf8.Encode(s); if (b.Length > 65535) throw new System.IO.IOException("UTFDataFormatException"); Raw16(b.Length); RawBytes(b); }

        void Prim(char code, object v)
        {
            switch (code)
            {
                case 'B': Raw((sbyte)v); break;
                case 'C': Raw16((char)v); break;
                case 'D': Raw64(BitConverter.DoubleToInt64Bits((double)v)); break;
                case 'F': Raw32(java.lang.Float.floatToIntBits((float)v)); break;
                case 'I': Raw32((int)v); break;
                case 'J': Raw64((long)v); break;
                case 'S': Raw16((short)v); break;
                case 'Z': Raw((bool)v ? 1 : 0); break;
            }
        }

        // ---------------------------------------------------------------- block data (writeInt & co.)
        void FlushBlock()
        {
            int n = (int)block.Length;
            if (n == 0) return;
            if (n <= 255) { Raw(TC.BLOCKDATA); Raw(n); }
            else { Raw(TC.BLOCKDATALONG); Raw32(n); }
            var b = block.ToArray();
            RawBytes(b);
            block.SetLength(0);
        }
        void B(int v) { block.WriteByte((byte)v); if (block.Length >= 1024) FlushBlock(); }

        public override void write(int b) => B(b);
        public override void write(sbyte[] b) => write(b, 0, b.Length);
        public override void write(sbyte[] b, int o, int l) { for (int i = 0; i < l; i++) B(b[o + i]); }
        public void writeByte(int v) => B(v);
        public void writeBoolean(bool v) => B(v ? 1 : 0);
        public void writeShort(int v) { B(v >> 8); B(v); }
        public void writeChar(int v) => writeShort(v);
        public void writeInt(int v) { B(v >> 24); B(v >> 16); B(v >> 8); B(v); }
        public void writeLong(long v) { writeInt((int)(v >> 32)); writeInt((int)v); }
        public void writeFloat(float v) => writeInt(java.lang.Float.floatToIntBits(v));
        public void writeDouble(double v) => writeLong(BitConverter.DoubleToInt64Bits(v));
        public void writeUTF(string s) { var b = MUtf8.Encode(s); writeShort(b.Length); foreach (var x in b) B(x); }
        public void defaultWriteObject() => throw new NotSupportedException("custom writeObject is not supported in the port");

        public override void flush() { FlushBlock(); @out.flush(); }
        public override void close() { flush(); @out.close(); }
        public void reset() { FlushBlock(); Raw(TC.RESET); handles.Clear(); typeStrings.Clear(); nextHandle = 0; }

        // ---------------------------------------------------------------- objects
        int Assign(object o) { int h = nextHandle++; if (o != null) handles[o] = h; return h; }

        public void writeObject(object o)
        {
            FlushBlock();
            WriteObject0(o);
        }

        void WriteRef(int h) { Raw(TC.REFERENCE); Raw32(TC.BASE_HANDLE + h); }

        void WriteString(string s)
        {
            Assign(s);
            var b = MUtf8.Encode(s);
            if (b.Length <= 65535) { Raw(TC.STRING); Raw16(b.Length); }
            else { Raw(TC.LONGSTRING); Raw64(b.Length); }
            RawBytes(b);
        }

        void WriteObject0(object o)
        {
            if (o == null) { Raw(TC.NULL); return; }
            if (!(o is ValueType) && handles.TryGetValue(o, out int h)) { WriteRef(h); return; }
            switch (o)
            {
                case string s: WriteString(s); return;
                case System.Array a: WriteArray(a); return;
                case java.lang.Enum e: WriteEnum(e); return;
                case int v: WriteBoxed("java.lang.Integer", 'I', v); return;
                case long v: WriteBoxed("java.lang.Long", 'J', v); return;
                case short v: WriteBoxed("java.lang.Short", 'S', v); return;
                case sbyte v: WriteBoxed("java.lang.Byte", 'B', v); return;
                case float v: WriteBoxed("java.lang.Float", 'F', v); return;
                case double v: WriteBoxed("java.lang.Double", 'D', v); return;
                case bool v: WriteBoxed("java.lang.Boolean", 'Z', v); return;
                case char v: WriteBoxed("java.lang.Character", 'C', v); return;
                case java.net.InetAddress ia: WriteInetAddress(ia); return;
                case System.Type t: WriteClass(t); return;
            }
            var d = JSerial.ForType(o.GetType());
            if (d == null) throw new System.IO.IOException("NotSerializableException: " + o.GetType().FullName);
            Raw(TC.OBJECT);
            WriteClassDesc(d);
            Assign(o);
            var chain = new List<JClassDesc>();
            for (var x = d; x != null; x = JSerial.Super(x)) chain.Insert(0, x);
            foreach (var c in chain)
            {
                if ((c.flags & JClassDesc.SC_WRITE_METHOD) != 0 && c.type != null && c.name != "java.lang.Throwable")
                    throw new System.IO.IOException("custom writeObject of " + c.name + " is not supported in the port");
                WriteFields(c, o);
                if ((c.flags & JClassDesc.SC_WRITE_METHOD) != 0) Raw(TC.ENDBLOCKDATA);
            }
        }

        void WriteFields(JClassDesc c, object o)
        {
            var infos = c.Infos();
            for (int i = 0; i < c.fields.Length; i++)
            {
                var f = c.fields[i];
                object v = infos?[i]?.GetValue(o);
                if (f.IsPrimitive) Prim(f.code, JSerial.Prim(f.code, v ?? DefaultPrim(f.code)));
            }
            for (int i = 0; i < c.fields.Length; i++)
            {
                var f = c.fields[i];
                if (!f.IsPrimitive) WriteObject0(infos?[i]?.GetValue(o));
            }
        }

        static object DefaultPrim(char code) => code switch { 'B' => (sbyte)0, 'C' => '\0', 'D' => 0.0, 'F' => 0f, 'I' => 0, 'J' => 0L, 'S' => (short)0, _ => false };

        void WriteBoxed(string cls, char code, object v)
        {
            Raw(TC.OBJECT);
            WriteClassDesc(JSerial.ByName(cls));
            Assign(null);
            Prim(code, v);
        }

        void WriteInetAddress(java.net.InetAddress ia)
        {
            // Inet4Address.writeReplace -> a plain InetAddress carrying (hostName, address, family); its writeObject uses putFields
            Raw(TC.OBJECT);
            WriteClassDesc(JSerial.ByName("java.net.InetAddress"));
            Assign(ia);
            Raw32(ia.AddressInt());
            Raw32(ia.Family());
            WriteObject0(ia.HostNameOrNull());
            Raw(TC.ENDBLOCKDATA);
        }

        /// <summary>
        /// A Class object (ObjectOutputStream.writeClass): TC_CLASS + the class descriptor. Classes that are not
        /// Serializable (the Supply types sent by recallGatherers) get Java's empty descriptor: suid 0, no flags, no
        /// fields, no superclass. Byte-identical to what the Java game sends.
        /// </summary>
        void WriteClass(System.Type t)
        {
            Raw(TC.CLASS);
            var d = JSerial.ForType(t);
            if (d == null || d.type != t) d = JSerial.ClassOnlyDesc(t);
            WriteClassDesc(d);
            Assign(t);
        }

        void WriteEnum(java.lang.Enum e)
        {
            var d = JSerial.ForType(e.getDeclaringClass()) ?? JSerial.ForType(e.GetType());
            if (d == null) throw new System.IO.IOException("NotSerializableException: " + e.GetType().FullName);
            Raw(TC.ENUM);
            WriteClassDesc(d);
            Assign(e);
            WriteObject0(e.name());
        }

        void WriteArray(System.Array a)
        {
            var name = JSerial.ArrayName(a.GetType());
            var d = JSerial.ByName(name) ?? new JClassDesc(name, null, 0, JClassDesc.SC_SERIALIZABLE, null, new JFieldDesc[0]);
            if (d.suid == 0) throw new System.IO.IOException("no serialVersionUID for array class " + name + " (regenerate NetTables)");
            Raw(TC.ARRAY);
            WriteClassDesc(d);
            Assign(a);
            Raw32(a.Length);
            char code = name[1];
            if (code == 'L' || code == '[') foreach (var x in a) WriteObject0(x);
            else if (code == 'B') { var b = new byte[a.Length]; Buffer.BlockCopy(a, 0, b, 0, b.Length); RawBytes(b); }
            else foreach (var x in a) Prim(code, x);
        }

        void WriteClassDesc(JClassDesc d)
        {
            if (d == null) { Raw(TC.NULL); return; }
            if (handles.TryGetValue(d, out int h)) { WriteRef(h); return; }
            Raw(TC.CLASSDESC);
            Assign(d);
            RawUTF(d.name);
            Raw64(d.suid);
            Raw(d.flags);
            Raw16(d.fields.Length);
            foreach (var f in d.fields)
            {
                Raw(f.code);
                RawUTF(f.name);
                if (!f.IsPrimitive)
                {
                    if (typeStrings.TryGetValue(f.typeString, out int th)) WriteRef(th);
                    else { typeStrings[f.typeString] = nextHandle; Raw(TC.STRING); nextHandle++; RawUTF(f.typeString); }
                }
            }
            Raw(TC.ENDBLOCKDATA);
            WriteClassDesc(JSerial.Super(d));
        }
    }

    public class ObjectInputStream : InputStream
    {
        readonly InputStream @in;
        readonly List<object> handles = new List<object>();
        int blockLeft;
        bool inBlock;
        public ObjectInputFilter filter;

        public ObjectInputStream(InputStream i)
        {
            @in = i;
            if (Raw16() != 0xACED) throw new System.IO.IOException("StreamCorruptedException: invalid stream header");
            if (Raw16() != 5) throw new System.IO.IOException("StreamCorruptedException: unsupported version");
        }

        public void setObjectInputFilter(ObjectInputFilter f) => filter = f;
        public ObjectInputFilter getObjectInputFilter() => filter;

        // ---------------------------------------------------------------- raw
        int Raw() { int c = @in.read(); if (c < 0) throw new System.IO.EndOfStreamException(); return c; }
        int Peek() { int c = @in.read(); if (c < 0) return -1; pushback = c; return c; }
        int pushback = -1;
        int RawByte() { if (pushback >= 0) { int c = pushback; pushback = -1; return c; } return Raw(); }
        int Raw16() => (RawByte() << 8) | RawByte();
        int Raw32() => (RawByte() << 24) | (RawByte() << 16) | (RawByte() << 8) | RawByte();
        long Raw64() => ((long)Raw32() << 32) | (Raw32() & 0xffffffffL);
        byte[] RawBytes(int n) { var b = new byte[n]; for (int i = 0; i < n; i++) b[i] = (byte)RawByte(); return b; }
        string RawUTF() => MUtf8.Decode(RawBytes(Raw16()));

        object Prim(char code)
        {
            switch (code)
            {
                case 'B': return unchecked((sbyte)RawByte());
                case 'C': return (char)Raw16();
                case 'D': return BitConverter.Int64BitsToDouble(Raw64());
                case 'F': return java.lang.Float.intBitsToFloat(Raw32());
                case 'I': return Raw32();
                case 'J': return Raw64();
                case 'S': return unchecked((short)Raw16());
                case 'Z': return RawByte() != 0;
            }
            throw new System.IO.IOException("bad type code " + code);
        }

        // ---------------------------------------------------------------- block data
        int B()
        {
            while (blockLeft == 0)
            {
                int tc = RawByte();
                if (tc == TC.BLOCKDATA) blockLeft = RawByte();
                else if (tc == TC.BLOCKDATALONG) blockLeft = Raw32();
                else if (tc == TC.RESET) handles.Clear();
                else throw new System.IO.IOException("OptionalDataException: expected primitive data, found 0x" + tc.ToString("x2"));
            }
            blockLeft--;
            return RawByte();
        }

        public override int read()
        {
            try { return B(); } catch (System.IO.EndOfStreamException) { return -1; }
        }
        public sbyte readByte() => unchecked((sbyte)B());
        public int readUnsignedByte() => B();
        public bool readBoolean() => B() != 0;
        public short readShort() => unchecked((short)((B() << 8) | B()));
        public int readUnsignedShort() => (B() << 8) | B();
        public char readChar() => (char)readUnsignedShort();
        public int readInt() => (B() << 24) | (B() << 16) | (B() << 8) | B();
        public long readLong() => ((long)readInt() << 32) | (readInt() & 0xffffffffL);
        public float readFloat() => java.lang.Float.intBitsToFloat(readInt());
        public double readDouble() => BitConverter.Int64BitsToDouble(readLong());
        public void readFully(sbyte[] b) { for (int i = 0; i < b.Length; i++) b[i] = readByte(); }
        public string readUTF() { int n = readUnsignedShort(); var b = new byte[n]; for (int i = 0; i < n; i++) b[i] = (byte)B(); return MUtf8.Decode(b); }
        public void defaultReadObject() => throw new NotSupportedException("custom readObject is not supported in the port");
        public override void close() => @in.close();

        // ---------------------------------------------------------------- objects
        public object readObject()
        {
            if (blockLeft > 0) throw new System.IO.IOException("OptionalDataException: primitive data pending");
            return ReadObject0();
        }

        int Assign(object o) { handles.Add(o); return handles.Count - 1; }

        object ReadObject0()
        {
            while (true)
            {
                int tc = RawByte();
                switch (tc)
                {
                    case TC.NULL: return null;
                    case TC.REFERENCE: return handles[Raw32() - TC.BASE_HANDLE];
                    case TC.STRING: { var s = RawUTF(); Assign(s); return s; }
                    case TC.LONGSTRING: { var s = MUtf8.Decode(RawBytes((int)Raw64())); Assign(s); return s; }
                    case TC.ARRAY: return ReadArray();
                    case TC.ENUM: return ReadEnum();
                    case TC.OBJECT: return ReadOrdinaryObject();
                    case TC.CLASS:
                    {
                        var cd = ReadClassDesc();
                        var t = cd?.type ?? JSerial.TypeForName(cd?.name);
                        if (t == null) throw new System.IO.IOException("ClassNotFoundException: " + cd?.name);
                        Assign(t);
                        return t;
                    }
                    case TC.CLASSDESC: pushback = tc; return ReadClassDesc();
                    case TC.RESET: handles.Clear(); continue;
                    case TC.EXCEPTION: throw new System.IO.IOException("WriteAbortedException");
                    default: throw new System.IO.IOException("StreamCorruptedException: invalid type code: " + tc.ToString("X2"));
                }
            }
        }

        JClassDesc ReadClassDesc()
        {
            int tc = RawByte();
            switch (tc)
            {
                case TC.NULL: return null;
                case TC.REFERENCE: return (JClassDesc)handles[Raw32() - TC.BASE_HANDLE];
                case TC.PROXYCLASSDESC: throw new System.IO.IOException("proxy class descriptors are not supported in the port");
                case TC.CLASSDESC: break;
                default: throw new System.IO.IOException("StreamCorruptedException: invalid class descriptor type code " + tc.ToString("X2"));
            }
            string name = RawUTF();
            long suid = Raw64();
            int h = Assign(null);
            int flags = RawByte();
            int n = Raw16();
            var fs = new JFieldDesc[n];
            for (int i = 0; i < n; i++)
            {
                char code = (char)RawByte();
                string fname = RawUTF();
                string ts = null;
                if (code == 'L' || code == '[') ts = (string)ReadObject0();
                fs[i] = new JFieldDesc(code, fname, ts);
            }
            SkipAnnotation();
            var local = JSerial.ByName(name);
            if (local != null && local.suid != suid && !name.StartsWith("["))
                throw new System.IO.IOException("InvalidClassException: " + name + "; local class incompatible: stream classdesc serialVersionUID = " + suid + ", local class serialVersionUID = " + local.suid);
            var d = new JClassDesc(name, local?.type, suid, flags, null, fs);
            handles[h] = d;
            d.super = ReadClassDesc();
            return d;
        }

        /// <summary>Skips class annotations / custom writeObject data up to TC_ENDBLOCKDATA.</summary>
        void SkipAnnotation()
        {
            while (true)
            {
                int tc = RawByte();
                if (tc == TC.ENDBLOCKDATA) return;
                if (tc == TC.BLOCKDATA) { RawBytes(RawByte()); continue; }
                if (tc == TC.BLOCKDATALONG) { RawBytes(Raw32()); continue; }
                pushback = tc;
                ReadObject0();
            }
        }

        object ReadArray()
        {
            var d = ReadClassDesc();
            int n = Raw32();
            var et = JSerial.ElementType(d.name);
            var a = System.Array.CreateInstance(et, n);
            Assign(a);
            char code = d.name[1];
            if (code == 'L' || code == '[')
                for (int i = 0; i < n; i++)
                {
                    var v = ReadObject0();
                    if (v != null && !et.IsInstanceOfType(v)) throw new System.IO.IOException("ArrayStoreException: " + v.GetType().FullName + " into " + d.name);
                    a.SetValue(v, i);
                }
            else if (code == 'B') { var b = RawBytes(n); Buffer.BlockCopy(b, 0, a, 0, n); }
            else for (int i = 0; i < n; i++) a.SetValue(Prim(code), i);
            return a;
        }

        object ReadEnum()
        {
            var d = ReadClassDesc();
            int h = Assign(null);
            string name = (string)ReadObject0();
            if (d.type == null) throw new System.IO.IOException("ClassNotFoundException: " + d.name);
            var e = JSerial.EnumConstant(d.type, name);
            handles[h] = e;
            return e;
        }

        object ReadOrdinaryObject()
        {
            var d = ReadClassDesc();
            if (d == null) throw new System.IO.IOException("StreamCorruptedException: null class descriptor");
            if ((d.flags & JClassDesc.SC_EXTERNALIZABLE) != 0) throw new System.IO.IOException("Externalizable " + d.name + " is not supported in the port");
            var chain = new List<JClassDesc>();
            for (var x = d; x != null; x = x.super) chain.Insert(0, x);

            switch (d.name)
            {
                case "java.lang.Integer": case "java.lang.Long": case "java.lang.Short": case "java.lang.Byte":
                case "java.lang.Float": case "java.lang.Double": case "java.lang.Boolean": case "java.lang.Character":
                case "java.net.InetAddress": case "java.net.Inet4Address": case "java.net.Inet6Address":
                    {
                        int h = Assign(null);
                        var values = new Dictionary<string, object>();
                        foreach (var c in chain) ReadFieldValues(c, values);
                        object r = d.name.StartsWith("java.net.") ? java.net.InetAddress.FromSerial(values) : values["value"];
                        handles[h] = r;
                        return r;
                    }
            }
            if (d.type == null) throw new System.IO.IOException("ClassNotFoundException: " + d.name);
            var o = JSerial.NewInstance(d.type);
            Assign(o);
            foreach (var c in chain)
            {
                var values = new Dictionary<string, object>();
                ReadFieldValues(c, values);
                var local = c.type == null ? null : JSerial.ByName(c.name);
                if (local != null)
                {
                    var infos = local.Infos();
                    for (int i = 0; i < local.fields.Length; i++)
                        if (infos[i] != null && values.TryGetValue(local.fields[i].name, out var v))
                            infos[i].SetValue(o, Convert(v, infos[i].FieldType));
                }
            }
            return o;
        }

        static object Convert(object v, System.Type t)
        {
            if (v == null || t.IsInstanceOfType(v)) return v;
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) return Convert(v, t.GetGenericArguments()[0]);
            if (t.IsPrimitive) return System.Convert.ChangeType(v, t);
            throw new System.IO.IOException("ClassCastException: cannot assign " + v.GetType().FullName + " to field of type " + t.FullName);
        }

        void ReadFieldValues(JClassDesc c, Dictionary<string, object> values)
        {
            foreach (var f in c.fields) if (f.IsPrimitive) values[f.name] = Prim(f.code);
            foreach (var f in c.fields) if (!f.IsPrimitive) values[f.name] = ReadObject0();
            if ((c.flags & JClassDesc.SC_WRITE_METHOD) != 0) SkipAnnotation();
        }
    }
}
