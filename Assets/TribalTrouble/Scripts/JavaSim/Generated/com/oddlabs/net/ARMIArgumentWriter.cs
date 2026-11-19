// Translated from ARMIArgumentWriter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 7
    public interface ARMIArgumentWriter
    {
        // Java: 8
        void writeArgument(System.Type type, object arg, com.oddlabs.util.ByteBufferOutputStream @out);
    }
    public sealed class ARMIArgumentWriter_L : java.lang.Object, ARMIArgumentWriter
    {
        readonly System.Action<System.Type, object, com.oddlabs.util.ByteBufferOutputStream> f;
        public ARMIArgumentWriter_L(System.Action<System.Type, object, com.oddlabs.util.ByteBufferOutputStream> f) { this.f = f; }
        public void writeArgument(System.Type p0, object p1, com.oddlabs.util.ByteBufferOutputStream p2) { f(p0, p1, p2); }
    }
}
