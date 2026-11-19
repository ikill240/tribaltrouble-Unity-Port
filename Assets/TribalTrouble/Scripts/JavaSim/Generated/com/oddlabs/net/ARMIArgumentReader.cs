// Translated from ARMIArgumentReader.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 7
    public interface ARMIArgumentReader
    {
        // Java: 8
        object readArgument(System.Type type, com.oddlabs.net.ByteBufferInputStream @in);
    }
    public sealed class ARMIArgumentReader_L : java.lang.Object, ARMIArgumentReader
    {
        readonly System.Func<System.Type, com.oddlabs.net.ByteBufferInputStream, object> f;
        public ARMIArgumentReader_L(System.Func<System.Type, com.oddlabs.net.ByteBufferInputStream, object> f) { this.f = f; }
        public object readArgument(System.Type p0, com.oddlabs.net.ByteBufferInputStream p1) { return f(p0, p1); }
    }
}
