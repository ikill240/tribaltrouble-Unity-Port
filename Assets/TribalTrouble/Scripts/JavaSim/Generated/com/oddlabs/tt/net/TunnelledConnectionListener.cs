// Translated from TunnelledConnectionListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 17
    public class TunnelledConnectionListener : com.oddlabs.net.AbstractConnectionListener
    {
        public java.util.List incoming_connections;
        public bool open;
        // Java: 26
        public void requestTunnelledConnection(com.oddlabs.net.HostSequenceID address, java.net.InetAddress inet_address, java.net.InetAddress local_address, com.oddlabs.matchmaking.Profile profile)
        {
            com.oddlabs.tt.net.TunnelledConnection conn = new com.oddlabs.tt.net.TunnelledConnection(address, null);
            incoming_connections.add(conn);
            notifyIncomingConnection(new com.oddlabs.tt.net.TunnelIdentifier(profile, new com.oddlabs.matchmaking.TunnelAddress(address.getHostID(), inet_address, local_address)));
        }
        // Java: 34
        public com.oddlabs.tt.net.TunnelledConnection getNextTunnel()
        {
            return ((com.oddlabs.tt.net.TunnelledConnection)incoming_connections.removeFirst());
        }
        // Java: 38
        public override com.oddlabs.net.AbstractConnection doAcceptConnection(com.oddlabs.net.ConnectionInterface connection_interface)
        {
            com.oddlabs.tt.net.TunnelledConnection conn = getNextTunnel();
            conn.setConnectionInterface(connection_interface);
            conn.accept();
            return conn;
        }
        // Java: 46
        public override void rejectConnection()
        {
            getNextTunnel().close();
        }
        // Java: 51
        public void connectionClosed()
        {
            open = false;
            notifyError(new System.IO.IOException());
        }
        // Java: 56
        public override void close()
        {
            if (open)
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().unregisterTunnelledListener(this);
                open = false;
            }
        }
        private void init_TunnelledConnectionListener()
        {
            this.incoming_connections = new java.util.LinkedList();
            this.open = true;
        }
        public TunnelledConnectionListener(com.oddlabs.net.ConnectionListenerInterface listener_interface) : base(listener_interface)
        {
            init_TunnelledConnectionListener();
            com.oddlabs.tt.net.Network.getMatchmakingClient().registerTunnelledListener(this);
        }
    }
}
