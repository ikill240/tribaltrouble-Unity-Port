// Translated from ConnectionInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public interface ConnectionInterface : com.oddlabs.net.ARMIEventBroker
    {
        // Java: 6
        void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e);
        // Java: 8
        void connected(com.oddlabs.net.AbstractConnection conn);
        // Java: 10
        void writeBufferDrained(com.oddlabs.net.AbstractConnection conn);
    }
}
