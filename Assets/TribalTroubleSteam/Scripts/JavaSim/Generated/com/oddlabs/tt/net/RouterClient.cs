// Translated from RouterClient.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 21
    public class RouterClient : java.lang.Object, com.oddlabs.net.ConnectionInterface
    {
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.net.AbstractConnection connection;
        public com.oddlabs.router.GameInterface game_interface;
        public com.oddlabs.tt.net.RouterHandler router_handler;
        // Java: 39
        public void connect(com.oddlabs.router.SessionID session_id, com.oddlabs.router.SessionInfo session_info, int client_id)
        {
            com.oddlabs.router.RouterInterface router_interface = ((com.oddlabs.router.RouterInterface)com.oddlabs.net.ARMIEvent.createProxy(connection, typeof(com.oddlabs.router.RouterInterface)));
            router_interface.login(session_id, session_info, client_id);
        }
        // Java: 44
        public void connectSpectator(com.oddlabs.router.SessionID session_id)
        {
            com.oddlabs.router.RouterInterface router_interface = ((com.oddlabs.router.RouterInterface)com.oddlabs.net.ARMIEvent.createProxy(connection, typeof(com.oddlabs.router.RouterInterface)));
            router_interface.loginSpectator(session_id);
        }
        // Java: 49
        public com.oddlabs.router.GameInterface getInterface()
        {
            return game_interface;
        }
        // Java: 53
        public void handle(object sender, com.oddlabs.net.ARMIEvent armi_event)
        {
            try
            {
                armi_event.execute(interface_methods, router_handler);
            }
            catch (com.oddlabs.net.IllegalARMIEventException e)
            {
                close();
                router_handler.routerFailed(e);
            }
        }
        // Java: 63
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 67
        public void connected(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 71
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            router_handler.routerFailed(e);
        }
        // Java: 76
        public void close()
        {
            connection.close();
        }
        private void init_RouterClient()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.router.RouterClientInterface));
        }
        public RouterClient(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.net.RouterHandler router_handler, int port) : base()
        {
            init_RouterClient();
            this.router_handler = router_handler;
            this.connection = new com.oddlabs.net.Connection(network, new java.net.InetSocketAddress(com.oddlabs.util.Utils.getLoopbackAddress(), port), this);
            this.game_interface = ((com.oddlabs.router.GameInterface)com.oddlabs.net.ARMIEvent.createProxy(connection, typeof(com.oddlabs.router.GameInterface)));
        }
        public RouterClient(com.oddlabs.net.NetworkSelector network, string address, com.oddlabs.tt.net.RouterHandler router_handler) : base()
        {
            init_RouterClient();
            this.router_handler = router_handler;
            this.connection = new com.oddlabs.net.Connection(network, address, com.oddlabs.router.RouterInterface.PORT, this);
            this.game_interface = ((com.oddlabs.router.GameInterface)com.oddlabs.net.ARMIEvent.createProxy(connection, typeof(com.oddlabs.router.GameInterface)));
        }
    }
}
