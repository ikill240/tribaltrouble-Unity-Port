// Translated from DeterministicSerializerLoopbackInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 5
    public interface DeterministicSerializerLoopbackInterface
    {
        // Java: 6
        void saveSucceeded();
        // Java: 8
        void loadSucceeded(object @object);
        // Java: 10
        void failed(System.Exception e);
    }
}
