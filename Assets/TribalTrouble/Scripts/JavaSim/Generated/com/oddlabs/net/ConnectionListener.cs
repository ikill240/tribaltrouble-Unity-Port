// Translated from ConnectionListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 17
    public class ConnectionListener : com.oddlabs.net.AbstractConnectionListener, com.oddlabs.net.Handler
    {
        public com.oddlabs.net.NetworkSelector network;
        public java.nio.channels.SelectionKey key;
        public java.util.List incoming_connections;
        // Java: 23
        public static java.nio.channels.SelectionKey createServerSocket(com.oddlabs.net.NetworkSelector network, java.net.InetAddress ip, int port)
        {
            java.nio.channels.ServerSocketChannel server_channel = java.nio.channels.ServerSocketChannel.open();
            server_channel.configureBlocking(false);
            java.net.SocketAddress address = new java.net.InetSocketAddress(ip, port);
            server_channel.socket().setReuseAddress(true);
            server_channel.socket().bind(address);
            java.nio.channels.SelectionKey key = server_channel.register(network.getSelector(), java.nio.channels.SelectionKey.OP_ACCEPT);
            return key;
        }
        // Java: 53
        public int getPort()
        {
            return network.getDeterministic().log((key != null ? (((java.nio.channels.ServerSocketChannel)key.channel())).socket().getLocalPort() : (-1)));
        }
        // Java: 58
        public void handle()
        {
            System.IO.IOException exception = null;
            java.nio.channels.SocketChannel channel = null;
            if (!network.getDeterministic().isPlayback())
            {
                java.nio.channels.ServerSocketChannel server_channel = ((java.nio.channels.ServerSocketChannel)key.channel());
                channel = server_channel.accept();
                try
                {
                    com.oddlabs.net.Connection.configureChannel(channel);
                }
                catch (System.IO.IOException e)
                {
                    try
                    {
                        channel.close();
                    }
                    catch (System.IO.IOException e2)
                    {
                        e2.printStackTrace();
                    }
                    exception = e;
                }
            }
            if (network.getDeterministic().log(exception != null))
            {
                throw ((System.IO.IOException)network.getDeterministic().log(exception));
            }
            incoming_connections.add(channel);
            notifyIncomingConnection();
        }
        // Java: 82
        public void error(System.IO.IOException e)
        {
            notifyError(e);
        }
        // Java: 86
        public void incoming(java.net.InetAddress remote_address)
        {
            notifyIncomingConnection(remote_address);
        }
        // Java: 90
        public void notifyIncomingConnection()
        {
            java.net.InetAddress remote_inet_address = null;
            if (!network.getDeterministic().isPlayback())
            {
                java.nio.channels.SocketChannel channel = ((java.nio.channels.SocketChannel)incoming_connections.getFirst());
                java.net.SocketAddress remote_address = channel.socket().getRemoteSocketAddress();
                remote_inet_address = (((java.net.InetSocketAddress)remote_address)).getAddress();
            }
            incoming(((java.net.InetAddress)network.getDeterministic().log(remote_inet_address)));
        }
        // Java: 100
        public java.nio.channels.SocketChannel removeNextChannel()
        {
            return ((java.nio.channels.SocketChannel)incoming_connections.removeFirst());
        }
        // Java: 104
        public java.nio.channels.SocketChannel getNextConnection()
        {
            java.nio.channels.SocketChannel channel = removeNextChannel();
            if (!incoming_connections.isEmpty())
            {
                notifyIncomingConnection();
            }
            return channel;
        }
        // Java: 111
        public java.nio.channels.SelectionKey getNextConnectionKey()
        {
            try
            {
                java.nio.channels.SocketChannel channel = getNextConnection();
                java.nio.channels.SelectionKey socket_key = channel.register(network.getSelector(), java.nio.channels.SelectionKey.OP_READ);
                return socket_key;
            }
            catch (System.IO.IOException e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 121
        public override com.oddlabs.net.AbstractConnection doAcceptConnection(com.oddlabs.net.ConnectionInterface conn_interface)
        {
            java.nio.channels.SelectionKey socket_key = default;
            if (!network.getDeterministic().isPlayback())
            {
                socket_key = getNextConnectionKey();
            }
            else
            {
                socket_key = null;
            }
            return new com.oddlabs.net.Connection(network, socket_key, conn_interface);
        }
        // Java: 131
        public override void rejectConnection()
        {
            try
            {
                java.nio.channels.SocketChannel channel = getNextConnection();
                if (!network.getDeterministic().isPlayback())
                {
                    channel.close();
                }
            }
            catch (System.IO.IOException e)
            {
                e.printStackTrace();
            }
        }
        // Java: 142
        public override void close()
        {
            if (key != null && key.isValid())
            {
                try
                {
                    key.channel().close();
                    while (!incoming_connections.isEmpty())
                    {
                        removeNextChannel().close();
                    }
                }
                catch (System.IO.IOException e)
                {
                    e.printStackTrace();
                }
            }
            if (network.getDeterministic().log(key != null))
            {
                network.cancelKey(key, this);
            }
        }
        // Java: 157
        public void handleError(System.IO.IOException e)
        {
            error(e);
        }
        private void init_ConnectionListener()
        {
            this.incoming_connections = new java.util.LinkedList();
        }
        public ConnectionListener(com.oddlabs.net.NetworkSelector network, java.net.InetAddress ip, int port, com.oddlabs.net.ConnectionListenerInterface connection_listener_interface) : base(connection_listener_interface)
        {
            init_ConnectionListener();
            this.network = network;
            System.IO.IOException exception = default;
            try
            {
                if (!network.getDeterministic().isPlayback())
                {
                    key = com.oddlabs.net.ConnectionListener.createServerSocket(network, ip, port);
                }
                exception = null;
            }
            catch (System.IO.IOException e)
            {
                exception = e;
            }
            if (network.getDeterministic().log(exception != null))
            {
                error(((System.IO.IOException)network.getDeterministic().log(exception)));
            }
            else
            {
                network.attachToKey(key, this);
            }
        }
    }
}
