// Translated from AbstractConnectionListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public abstract class AbstractConnectionListener : java.lang.Object
    {
        public com.oddlabs.net.ConnectionListenerInterface listener_interface;
        // Java: 12
        public abstract void close();
        // Java: 14
        public virtual com.oddlabs.net.AbstractConnection acceptConnection(com.oddlabs.net.ConnectionInterface connection_interface)
        {
            return doAcceptConnection(connection_interface);
        }
        // Java: 18
        public abstract com.oddlabs.net.AbstractConnection doAcceptConnection(com.oddlabs.net.ConnectionInterface conn_interface);
        // Java: 20
        public abstract void rejectConnection();
        // Java: 22
        public void notifyIncomingConnection(object address)
        {
            listener_interface.incomingConnection(this, address);
        }
        // Java: 26
        public void notifyError(System.IO.IOException e)
        {
            close();
            listener_interface.error(this, e);
        }
        public AbstractConnectionListener(com.oddlabs.net.ConnectionListenerInterface connection_listener) : base()
        {
            this.listener_interface = connection_listener;
        }
    }
}
