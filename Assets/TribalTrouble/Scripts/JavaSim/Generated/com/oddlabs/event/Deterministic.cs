// Translated from Deterministic.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.@event
{
    // Java: 9
    public abstract class Deterministic : java.lang.Object
    {
        public const int BUFFER_SIZE = 4096;
        public const int DEFAULTS_SIZE = 2;
        public const int MIN_DEFAULTS = (-32768);
        public const long CHECKPOINT_SIGNATURE = (-2401057697706361105L);
        public bool enabled;
        // Java: 18
        public bool log(bool b)
        {
            return log(b, false);
        }
        // Java: 22
        public bool log(bool b, bool def)
        {
            return log((b ? unchecked((sbyte)(1)) : unchecked((sbyte)(0))), (def ? unchecked((sbyte)(1)) : unchecked((sbyte)(0)))) != 0;
        }
        // Java: 27
        public sbyte log(sbyte b)
        {
            return log(b, unchecked((sbyte)(0)));
        }
        // Java: 32
        public abstract sbyte log(sbyte b, sbyte def);
        // Java: 34
        public char log(char c)
        {
            return log(c, unchecked((char)(0)));
        }
        // Java: 39
        public abstract char log(char c, char def);
        // Java: 41
        public int log(int i)
        {
            return log(i, 0);
        }
        // Java: 46
        public abstract int log(int i, int def);
        // Java: 48
        public long log(long l)
        {
            return log(l, 0);
        }
        // Java: 53
        public abstract long log(long l, long def);
        // Java: 55
        public float log(float f)
        {
            return log(f, 0.0f);
        }
        // Java: 60
        public abstract float log(float f, float def);
        // Java: 62
        public abstract java.nio.file.Path log(java.nio.file.Path p, java.nio.file.Path def);
        // Java: 64
        public java.nio.file.Path log(java.nio.file.Path p)
        {
            return log(p, java.nio.file.Path_.of(""));
        }
        // Java: 69
        public object log(object o)
        {
            return logObject(o);
        }
        // Java: 74
        public abstract object logObject(object o);
        // Java: 76
        public void log(java.nio.ByteBuffer o)
        {
            logBuffer(o);
        }
        // Java: 81
        public abstract void logBuffer(java.nio.ByteBuffer o);
        // Java: 83
        public abstract void endLog();
        // Java: 85
        public abstract bool isPlayback();
        // Java: 87
        public void setEnabled(bool enable)
        {
            this.enabled = enable;
        }
        // Java: 91
        public void checkpoint()
        {
            checkpoint(com.oddlabs.@event.Deterministic.CHECKPOINT_SIGNATURE);
        }
        // Java: 95
        public void checkpoint(long @value)
        {
            long logged_value = log(@value);
        }
        // Java: 101
        public void checkpoint(bool @value)
        {
            bool logged_value = log(@value);
        }
        // Java: 107
        public void checkpoint(float @value)
        {
            float logged_value = log(@value);
        }
        // Java: 113
        public static int getTraceId()
        {
            System.Exception t = new System.Exception();
            java.lang.StackTraceElement[] stack_trace_elements = t.getStackTrace();
            int hash = 0;
            foreach (java.lang.StackTraceElement stack_trace_element in stack_trace_elements)
            {
                if (stack_trace_element.getClassName().startsWith(typeof(com.oddlabs.@event.Deterministic).getPackage().getName()))
                {
                    continue;
                }
                hash += java.lang.JO.hashCode(stack_trace_element.getMethodName());
            }
            return hash;
        }
        private void init_Deterministic()
        {
            this.enabled = true;
        }
        public Deterministic() : base()
        {
            init_Deterministic();
        }
    }
}
