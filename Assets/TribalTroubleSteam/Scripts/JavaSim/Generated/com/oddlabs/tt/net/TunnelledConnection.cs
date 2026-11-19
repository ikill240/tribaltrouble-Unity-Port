// Translated from TunnelledConnection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 10
    public class TunnelledConnection : com.oddlabs.net.AbstractConnection
    {
        public com.oddlabs.net.HostSequenceID address;
        public bool open;
        // Java: 26
        public void tunnelClosed()
        {
            open = false;
            notifyError(new System.IO.IOException("Connection closed"));
        }
        // Java: 31
        public void connected()
        {
            notifyConnected();
        }
        // Java: 35
        public void accept()
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().acceptTunnel(address);
        }
        // Java: 39
        public override void handle(com.oddlabs.net.ARMIEvent @event)
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().routeEvent(address, @event);
            writeBufferDrained();
        }
        // Java: 45
        public com.oddlabs.net.HostSequenceID getAddress()
        {
            return address;
        }
        // Java: 49
        public override void doClose()
        {
            if (open)
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().unregisterTunnel(address, this);
                open = false;
            }
        }
        private void init_TunnelledConnection()
        {
            this.open = true;
        }
        public TunnelledConnection(com.oddlabs.net.HostSequenceID address, com.oddlabs.net.ConnectionInterface conn_interface) : base()
        {
            init_TunnelledConnection();
            setConnectionInterface(conn_interface);
            this.address = address;
            com.oddlabs.tt.net.Network.getMatchmakingClient().registerTunnel(this.address, this);
            notifyConnected();
        }
        public TunnelledConnection(int address, com.oddlabs.net.ConnectionInterface conn_interface) : base()
        {
            init_TunnelledConnection();
            setConnectionInterface(conn_interface);
            this.address = com.oddlabs.tt.net.Network.getMatchmakingClient().registerTunnel(address, this);
        }
    }
}
