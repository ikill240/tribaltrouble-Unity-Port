// Translated from ClientConnection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 7
    public class ClientConnection : java.lang.Object
    {
        public com.oddlabs.net.AbstractConnection connection;
        public com.oddlabs.tt.net.GameClientInterface gameclient_interface;
        public com.oddlabs.tt.net.ClientInfo client;
        public int custom_map_offset;
        public bool has_custom_map;
        public int custom_map_in_flight;
        public string build_fingerprint;
        // Java: 25
        public com.oddlabs.tt.net.GameClientInterface getClientInterface()
        {
            return gameclient_interface;
        }
        // Java: 29
        public com.oddlabs.net.AbstractConnection getConnection()
        {
            return connection;
        }
        // Java: 33
        public com.oddlabs.tt.net.ClientInfo getClient()
        {
            return client;
        }
        private void init_ClientConnection()
        {
            this.custom_map_offset = (-1);
        }
        public ClientConnection(com.oddlabs.net.AbstractConnection conn, com.oddlabs.tt.net.ClientInfo client) : base()
        {
            init_ClientConnection();
            this.connection = conn;
            this.gameclient_interface = ((com.oddlabs.tt.net.GameClientInterface)com.oddlabs.net.ARMIEvent.createProxy(connection, typeof(com.oddlabs.tt.net.GameClientInterface)));
            this.client = client;
        }
    }
}
