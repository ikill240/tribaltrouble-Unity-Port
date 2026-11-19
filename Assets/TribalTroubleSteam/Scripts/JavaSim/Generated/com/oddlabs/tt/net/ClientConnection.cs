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
        // Java: 18
        public com.oddlabs.tt.net.GameClientInterface getClientInterface()
        {
            return gameclient_interface;
        }
        // Java: 22
        public com.oddlabs.net.AbstractConnection getConnection()
        {
            return connection;
        }
        // Java: 26
        public com.oddlabs.tt.net.ClientInfo getClient()
        {
            return client;
        }
        public ClientConnection(com.oddlabs.net.AbstractConnection conn, com.oddlabs.tt.net.ClientInfo client) : base()
        {
            this.connection = conn;
            this.gameclient_interface = ((com.oddlabs.tt.net.GameClientInterface)com.oddlabs.net.ARMIEvent.createProxy(connection, typeof(com.oddlabs.tt.net.GameClientInterface)));
            this.client = client;
        }
    }
}
