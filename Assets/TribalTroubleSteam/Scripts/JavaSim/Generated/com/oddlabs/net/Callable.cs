// Translated from Callable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 3
    public interface Callable : com.oddlabs.net.TaskExecutorLoopbackInterface, java.util.concurrent.Callable
    {
        // Java: 9
        object call();
    }
}
