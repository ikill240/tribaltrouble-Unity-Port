// Translated from TaskExecutorLoopbackInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public interface TaskExecutorLoopbackInterface
    {
        // Java: 6
        void taskCompleted(object result);
        // Java: 8
        void taskFailed(System.Exception e);
    }
}
