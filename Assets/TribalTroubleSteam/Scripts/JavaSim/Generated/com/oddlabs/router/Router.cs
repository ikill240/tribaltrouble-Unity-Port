// Translated from Router.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 18
    public class Router : java.lang.Object, com.oddlabs.net.ConnectionListenerInterface
    {
        public java.util.logging.Logger logger;
        public com.oddlabs.router.SessionManager manager;
        public com.oddlabs.net.AbstractConnectionListener listener;
        public java.util.Set clients;
        public com.oddlabs.router.RouterListener router_listener;
        public int port;
        // Java: 40
        public int getPort()
        {
            return port;
        }
        // Java: 44
        public long getNextTimeout()
        {
            return manager.getNextTimeout();
        }
        // Java: 48
        public void process()
        {
            manager.process();
        }
        // Java: 52
        public void incomingConnection(com.oddlabs.net.AbstractConnectionListener connection_listener, object remote_address)
        {
            logger.log(java.util.logging.Level.INFO, "Incoming connection from {0}", remote_address);
            com.oddlabs.net.AbstractConnection conn = connection_listener.acceptConnection(null);
            com.oddlabs.router.RouterClient client = new com.oddlabs.router.RouterClient(manager, conn, logger, this);
            clients.add(client);
            conn.setConnectionInterface(client);
        }
        // Java: 61
        public void removeClient(com.oddlabs.router.RouterClient client)
        {
            clients.remove(client);
        }
        // Java: 65
        public void error(com.oddlabs.net.AbstractConnectionListener conn_id, System.IO.IOException e)
        {
            close();
            logger.log(java.util.logging.Level.SEVERE, "Server socket failed: {0}", e);
            if (router_listener != null)
            {
                router_listener.routerFailed(e);
            }
        }
        // Java: 73
        public void close()
        {
            if (listener != null)
            {
                listener.close();
            }
            java.util.Iterator it = clients.iterator();
            while (it.hasNext())
            {
                com.oddlabs.router.RouterClient client = ((com.oddlabs.router.RouterClient)it.next());
                it.remove();
                client.close(false);
            }
        }
        private void init_Router()
        {
            this.clients = new java.util.LinkedHashSet();
        }
        public Router(com.oddlabs.net.NetworkSelector network, java.util.logging.Logger logger) : this(network, null, com.oddlabs.router.RouterInterface.PORT, logger, null)
        {
        }
        public Router(com.oddlabs.net.NetworkSelector network, java.net.InetAddress address, int port, java.util.logging.Logger logger, com.oddlabs.router.RouterListener router_listener) : base()
        {
            init_Router();
            this.manager = new com.oddlabs.router.SessionManager(network.getTimeManager(), logger);
            this.logger = logger;
            this.router_listener = router_listener;
            com.oddlabs.net.ConnectionListener tmp_listener = new com.oddlabs.net.ConnectionListener(network, address, port, this);
            this.listener = tmp_listener;
            this.port = tmp_listener.getPort();
        }
    }
}
