// java.lang.reflect for ARMI (method tables and proxies generated at translation time - no runtime codegen, so it
// works under IL2CPP), plus threads, wait/notify and the smaller JDK pieces the network layer uses.
using System;
using System.Collections.Generic;

namespace java.lang.reflect
{
    /// <summary>A method of an ARMI interface: the Java Method.toString() text (ARMI sorts by it), the parameter types and an invoker.</summary>
    public sealed class Method : java.lang.Object
    {
        readonly System.Type decl;
        readonly string name, text;
        readonly System.Type[] parameterTypes;
        readonly Action<object, object[]> invoker;

        public Method(System.Type decl, string name, string text, System.Type[] parameterTypes, Action<object, object[]> invoker)
        {
            this.decl = decl;
            this.name = name;
            this.text = text;
            this.parameterTypes = parameterTypes;
            this.invoker = invoker;
        }

        public string getName() => name;
        public System.Type getDeclaringClass() => decl;
        public System.Type[] getParameterTypes() => (System.Type[])parameterTypes.Clone();
        public int getParameterCount() => parameterTypes.Length;
        public System.Type getReturnType() => typeof(void);
        public System.Type[] getExceptionTypes() => new System.Type[0];
        public override string toString() => text;
        public override string ToString() => text;

        public object invoke(object instance, params object[] args)
        {
            if (instance == null) throw new NullReferenceException();
            if (!decl.IsInstanceOfType(instance)) throw new ArgumentException("object is not an instance of declaring class");
            int n = args == null ? 0 : args.Length;
            if (n != parameterTypes.Length) throw new ArgumentException("wrong number of arguments: " + n + " expected: " + parameterTypes.Length);
            for (int i = 0; i < n; i++)
            {
                var a = args[i];
                var t = parameterTypes[i];
                if (a == null ? t.IsValueType : !Boxes(t, a)) throw new ArgumentException("argument type mismatch");
            }
            try { invoker(instance, args); }
            catch (Exception e) { throw new InvocationTargetException(e); }
            return null;
        }

        static bool Boxes(System.Type t, object a) => t.IsInstanceOfType(a);
    }

    public interface InvocationHandler
    {
        object invoke(object proxy, Method method, object[] args);
    }

    public class InvocationTargetException : Exception
    {
        readonly Exception target;
        public InvocationTargetException(Exception target) : base(null, target) { this.target = target; }
        public Exception getTargetException() => target;
        public Exception getCause() => target;
    }

    public static class Proxy
    {
        public static object newProxyInstance(object loader, System.Type[] interfaces, InvocationHandler h)
        {
            if (interfaces.Length != 1) throw new NotSupportedException("proxies implement exactly one interface in the port");
            return J2CS.Net.JReflect.NewProxy(interfaces[0], h);
        }
    }

    public static class Array
    {
        public static object newInstance(System.Type component, int length) => System.Array.CreateInstance(component, length);
        public static int getLength(object a) => ((System.Array)a).Length;
        public static object get(object a, int i) => ((System.Array)a).GetValue(i);
        public static void set(object a, int i, object v) => ((System.Array)a).SetValue(v, i);
    }
}

namespace J2CS.Net
{
    /// <summary>Registry the generated NetTables fills: ARMI interface -> sorted methods, and a proxy factory.</summary>
    public static class JReflect
    {
        static readonly Dictionary<System.Type, java.lang.reflect.Method[]> methods = new Dictionary<System.Type, java.lang.reflect.Method[]>();
        static readonly Dictionary<System.Type, Func<java.lang.reflect.InvocationHandler, object>> proxies = new Dictionary<System.Type, Func<java.lang.reflect.InvocationHandler, object>>();
        static bool searched;

        public static void Register(System.Type iface, java.lang.reflect.Method[] ms, Func<java.lang.reflect.InvocationHandler, object> proxy)
        {
            lock (methods)
            {
                methods[iface] = ms;
                proxies[iface] = proxy;
            }
        }

        /// <summary>Finds and runs the generated registration (J2CS.Generated.NetTables.Register) if the host hasn't.</summary>
        public static void EnsureRegistered()
        {
            lock (methods)
            {
                if (searched || methods.Count > 0) return;
                searched = true;
            }
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = asm.GetType("J2CS.Generated.NetTables", false);
                if (t == null) continue;
                t.GetMethod("Register").Invoke(null, null);
                JSerial.EnsureRegistered();
                return;
            }
        }

        public static java.lang.reflect.Method[] MethodsOf(System.Type iface)
        {
            EnsureRegistered();
            lock (methods)
            {
                if (methods.TryGetValue(iface, out var ms)) return ms;
            }
            throw new NotSupportedException("no ARMI method table for " + iface.FullName + " (add it to j2cs/armi/ifaces.txt)");
        }

        public static object NewProxy(System.Type iface, java.lang.reflect.InvocationHandler h)
        {
            EnsureRegistered();
            Func<java.lang.reflect.InvocationHandler, object> f;
            lock (methods)
            {
                if (!proxies.TryGetValue(iface, out f)) throw new NotSupportedException("no ARMI proxy for " + iface.FullName);
            }
            return f(h);
        }
    }
}

namespace J2CS.Ext
{
    public static class JNetExt
    {
        public static java.lang.reflect.Method[] getMethods(this System.Type t) => (java.lang.reflect.Method[])J2CS.Net.JReflect.MethodsOf(t).Clone();
        public static object getClassLoader(this System.Type t) => null;
        public static System.Type getComponentType(this System.Type t) => t.IsArray ? t.GetElementType() : null;
        /// <summary>array.clone() (a shallow copy, as in Java).</summary>
        public static object clone(this System.Array a) => a.Clone();
        public static bool isArray(this System.Type t) => t.IsArray;

        // Object.wait / notify / notifyAll: the translator turns synchronized blocks into C# lock (same monitor).
        public static void wait(this object o) => System.Threading.Monitor.Wait(o);
        public static void wait(this object o, long ms) => System.Threading.Monitor.Wait(o, ms <= 0 ? System.Threading.Timeout.Infinite : (int)System.Math.Min(ms, int.MaxValue));
        public static void notify(this object o) => System.Threading.Monitor.Pulse(o);
        public static void notifyAll(this object o) => System.Threading.Monitor.PulseAll(o);

        public static float floatValue(this object o) => o switch
        {
            float f => f, double d => (float)d, int i => i, long l => l, short s => s, sbyte b => b, char c => c,
            _ => throw new InvalidCastException("not a number: " + o)
        };
        public static int intValue(this object o) => o switch
        {
            int i => i, long l => (int)l, short s => s, sbyte b => b, char c => c,
            float f => java.lang.JC.f2i(f), double d => java.lang.JC.d2i(d),
            _ => throw new InvalidCastException("not a number: " + o)
        };
    }

    public static class JExc
    {
        /// <summary>new Exception(cause) - Java uses cause.toString() as the message.</summary>
        public static string CauseMessage(Exception cause) => cause == null ? null : cause.GetType().FullName + (cause.Message != null ? ": " + cause.Message : "");
    }
}

namespace java.lang
{
    public class InterruptedException : Exception
    {
        public InterruptedException() { }
        public InterruptedException(string m) : base(m) { }
    }

    /// <summary>java.lang.Thread over a background System.Threading.Thread.</summary>
    public class Thread : Object, Runnable
    {
        readonly Runnable target;
        System.Threading.Thread thread;
        string name = "Thread";
        bool daemon;
        volatile bool interrupted;
        [ThreadStatic] static Thread current;

        public Thread() { }
        public Thread(Runnable target) { this.target = target; }
        public Thread(Runnable target, string name) { this.target = target; this.name = name; }
        public Thread(string name) { this.name = name; }

        public virtual void run() => target?.run();
        public void setName(string n) { name = n; if (thread != null) try { thread.Name = n; } catch { } }
        public string getName() => name;
        public void setDaemon(bool d) => daemon = d;
        public bool isDaemon() => daemon;
        public void setPriority(int p) { }
        public bool isAlive() => thread != null && thread.IsAlive;

        public void start()
        {
            if (thread != null) throw new InvalidOperationException("thread already started");
            thread = new System.Threading.Thread(() =>
            {
                current = this;
                try { run(); }
                catch (System.Threading.ThreadInterruptedException) { }
                catch (Exception e) { java.lang.IO.println("Exception in thread \"" + name + "\" " + e); }
            }) { IsBackground = true, Name = name };
            thread.Start();
        }

        public void interrupt()
        {
            interrupted = true;
            try { thread?.Interrupt(); } catch { }
        }

        public bool isInterrupted() => interrupted;
        public static bool interrupted_() { var t = current; if (t == null) return false; bool r = t.interrupted; t.interrupted = false; return r; }
        public void join() => thread?.Join();
        public void join(long ms) => thread?.Join((int)ms);
        public static Thread currentThread() => current ?? (current = new Thread("main"));
        public static void sleep(long ms)
        {
            try { System.Threading.Thread.Sleep((int)System.Math.Max(0, ms)); }
            catch (System.Threading.ThreadInterruptedException) { throw new InterruptedException(); }
        }
        public static void yield_() => System.Threading.Thread.Yield();
        public static void onSpinWait() => System.Threading.Thread.SpinWait(1);
    }
}

namespace java.util.concurrent
{
    public interface Callable
    {
        object call();
    }
}

namespace java.util
{
    public sealed class HexFormat : java.lang.Object
    {
        public static HexFormat of() => new HexFormat();
        public string formatHex(sbyte[] b)
        {
            var sb = new System.Text.StringBuilder(b.Length * 2);
            foreach (var x in b) sb.Append(((byte)x).ToString("x2"));
            return sb.ToString();
        }
        public sbyte[] parseHex(string s)
        {
            var r = new sbyte[s.Length / 2];
            for (int i = 0; i < r.Length; i++) r[i] = unchecked((sbyte)Convert.ToByte(s.Substring(i * 2, 2), 16));
            return r;
        }
    }
}

namespace java.io
{
    public class PushbackInputStream : InputStream
    {
        readonly InputStream i;
        readonly Stack<int> back = new Stack<int>();
        public PushbackInputStream(InputStream i) { this.i = i; }
        public override int read() => back.Count > 0 ? back.Pop() : i.read();
        public void unread(int b) => back.Push(b & 0xff);
        public override void close() => i.close();
    }

    /// <summary>ObjectInputFilter: the port's ObjectInputStream only resolves the classes the generated tables know, which is stricter than any filter the game sets.</summary>
    public class ObjectInputFilter : java.lang.Object
    {
        public readonly string pattern;
        ObjectInputFilter(string p) { pattern = p; }
        public static class Config
        {
            public static ObjectInputFilter createFilter(string pattern) => new ObjectInputFilter(pattern);
        }
    }
}

namespace java.nio
{
    public class BufferOverflowException : InvalidOperationException { public BufferOverflowException() : base("BufferOverflowException") { } }
    public class BufferUnderflowException : InvalidOperationException { public BufferUnderflowException() : base("BufferUnderflowException") { } }
}
