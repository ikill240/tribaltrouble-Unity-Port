// Translated from ConnectionListenerInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public interface ConnectionListenerInterface
    {
        // Java: 6
        void error(com.oddlabs.net.AbstractConnectionListener listener, System.IO.IOException e);
        // Java: 8
        void incomingConnection(com.oddlabs.net.AbstractConnectionListener listener, object address);
    }
}
