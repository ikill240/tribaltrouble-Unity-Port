// Translated from AbstractConnection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 9
    public abstract class AbstractConnection : java.lang.Object, com.oddlabs.net.ARMIEventWriter
    {
        public java.util.List event_backlog;
        public com.oddlabs.net.ConnectionInterface connection_interface;
        public System.IO.IOException error_flag;
        public bool connected_flag;
        public bool connected_signaled;
        // Java: 16
        public void setConnectionInterface(com.oddlabs.net.ConnectionInterface connection_interface)
        {
            this.connection_interface = connection_interface;
            if (connection_interface != null)
            {
                if (connected_flag)
                {
                    signalConnected();
                }
                foreach (com.oddlabs.net.ARMIEvent armiEvent in java.lang.JIter.of(event_backlog))
                {
                    connection_interface.handle(this, armiEvent);
                }
                event_backlog.clear();
                if (error_flag != null)
                {
                    connection_interface.error(this, error_flag);
                }
            }
        }
        // Java: 30
        public void writeBufferDrained()
        {
            if (connection_interface != null)
            {
                connection_interface.writeBufferDrained(this);
            }
        }
        // Java: 35
        public com.oddlabs.net.ConnectionInterface getConnectionInterface()
        {
            return connection_interface;
        }
        // Java: 39
        public void signalConnected()
        {
            if (!connected_signaled)
            {
                connection_interface.connected(this);
                connected_signaled = true;
            }
        }
        // Java: 46
        public void close()
        {
            connected_flag = false;
            doClose();
            connection_interface = null;
        }
        // Java: 52
        public abstract void doClose();
        // Java: 54
        public void receiveEvent(com.oddlabs.net.ARMIEvent @event)
        {
            if (connection_interface != null)
            {
                connection_interface.handle(this, @event);
            }
            else
            {
                event_backlog.add(@event);
            }
        }
        // Java: 61
        public bool isConnected()
        {
            return connected_flag;
        }
        // Java: 65
        public void notifyConnected()
        {
            if (!connected_flag)
            {
                connected_flag = true;
                if (connection_interface != null)
                {
                    signalConnected();
                }
            }
        }
        // Java: 73
        public void notifyError(System.IO.IOException e)
        {
            if (error_flag == null)
            {
                error_flag = e;
                if (connection_interface != null)
                {
                    connection_interface.error(this, e);
                }
            }
            close();
        }
        private void init_AbstractConnection()
        {
            this.event_backlog = new java.util.ArrayList();
        }
        public AbstractConnection() : base()
        {
            init_AbstractConnection();
        }
        public abstract void handle(com.oddlabs.net.ARMIEvent p0);
    }
}
