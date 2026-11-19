// Translated from Connection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 17
    public class Connection : com.oddlabs.net.AbstractConnection, com.oddlabs.net.Handler, com.oddlabs.net.ConnectionPeerInterface
    {
        public const int BUFFER_SIZE = 65536;
        public const short HEADER_SIZE = unchecked((short)2);
        public java.nio.ByteBuffer read_buffer;
        public java.util.List back_log_list;
        public java.nio.ByteBuffer write_buffer;
        public com.oddlabs.net.ConnectionPeerInterface peer_interface;
        public bool ping_reply;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public bool writing;
        public bool pinged;
        public java.nio.channels.SelectionKey key;
        public java.net.InetAddress local_address;
        // Java: 33
        public static void configureChannel(java.nio.channels.SocketChannel channel)
        {
            channel.configureBlocking(false);
            channel.socket().setTcpNoDelay(true);
        }
        // Java: 73
        public void doPing()
        {
            if (isConnected())
            {
                peer_interface.ping();
            }
        }
        // Java: 78
        public void ping()
        {
            if (ping_reply)
            {
                peer_interface.ping();
            }
            pinged = true;
        }
        // Java: 85
        public void timeout()
        {
            if (!isConnected())
            {
                return;
            }
            if (!pinged)
            {
                handleError(new System.IO.IOException("Connection timed out"));
            }
            else
            {
                pinged = false;
                network.registerForPingTimeout(this);
            }
        }
        // Java: 96
        public void connected(java.net.InetAddress local_address)
        {
            this.local_address = local_address;
            notifyConnected();
        }
        // Java: 101
        public java.net.InetAddress getLocalAddress()
        {
            return local_address;
        }
        // Java: 105
        public void error(System.IO.IOException e)
        {
            notifyError(e);
        }
        // Java: 109
        public void connect(java.net.SocketAddress socket_address)
        {
            System.IO.IOException exception = null;
            java.net.InetAddress local_address = null;
            java.nio.channels.SelectionKey key = null;
            if (!network.getDeterministic().isPlayback())
            {
                java.nio.channels.Selector selector = network.getSelector();
                try
                {
                    java.nio.channels.SocketChannel channel = java.nio.channels.SocketChannel.open();
                    try
                    {
                        com.oddlabs.net.Connection.configureChannel(channel);
                        bool success = channel.connect(socket_address);
                        if (success)
                        {
                            key = channel.register(selector, java.nio.channels.SelectionKey.OP_READ);
                            local_address = channel.socket().getLocalAddress();
                        }
                        else
                        {
                            key = channel.register(selector, java.nio.channels.SelectionKey.OP_CONNECT);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        channel.close();
                        throw e;
                    }
                }
                catch (System.IO.IOException e_2)
                {
                    exception = e_2;
                }
            }
            setKey(key);
            if (writing)
            {
                doSetWriting();
            }
            else
            {
                doResetWriting();
            }
            if (network.getDeterministic().log(exception != null))
            {
                error(((System.IO.IOException)network.getDeterministic().log(exception)));
            }
            else if (network.getDeterministic().log(local_address != null))
            {
                connected(((java.net.InetAddress)network.getDeterministic().log(local_address)));
            }
        }
        // Java: 145
        public void dnsError(System.IO.IOException e)
        {
            error(e);
        }
        // Java: 149
        public void setKey(java.nio.channels.SelectionKey key)
        {
            this.key = key;
            network.attachToKey(key, this);
        }
        // Java: 155
        public void doSetWriting()
        {
            if (!network.getDeterministic().isPlayback())
            {
                int new_ops = key.interestOps() | java.nio.channels.SelectionKey.OP_WRITE;
                key.interestOps(new_ops);
            }
        }
        // Java: 162
        public void doResetWriting()
        {
            if (!network.getDeterministic().isPlayback())
            {
                int new_ops = key.interestOps() & ~java.nio.channels.SelectionKey.OP_WRITE;
                key.interestOps(new_ops);
            }
        }
        // Java: 169
        public void resetWriting()
        {
            if (isKeyValid())
            {
                doResetWriting();
            }
            writing = false;
        }
        // Java: 176
        public void setWriting()
        {
            if (writing)
            {
                return;
            }
            if (isKeyValid())
            {
                doSetWriting();
            }
            writing = true;
        }
        // Java: 184
        public override void handle(com.oddlabs.net.ARMIEvent @event)
        {
            peer_interface.receiveEvent(@event);
        }
        // Java: 189
        public bool writeNextEvent()
        {
            if (back_log_list.isEmpty())
            {
                return false;
            }
            com.oddlabs.net.ARMIEvent @event = ((com.oddlabs.net.ARMIEvent)back_log_list.getFirst());
            bool success = writeEvent(@event);
            if (success)
            {
                back_log_list.removeFirst();
            }
            return success;
        }
        // Java: 199
        public void writeBackLog()
        {
            while (writeNextEvent())
            {
                ;
            }
        }
        // Java: 204
        public void writeToChannel(java.nio.channels.SocketChannel channel)
        {
            int bytes_written = default;
            do
            {
                bytes_written = 0;
                if (write_buffer.position() > 0)
                {
                    write_buffer.flip();
                    if (!network.getDeterministic().isPlayback())
                    {
                        bytes_written = channel.write(write_buffer);
                    }
                    bytes_written = network.getDeterministic().log(bytes_written);
                    int new_position = network.getDeterministic().log(write_buffer.position());
                    write_buffer.position(new_position);
                    write_buffer.compact();
                }
                writeBackLog();
            }
            while (bytes_written > 0);
            if (write_buffer.position() == 0)
            {
                resetWriting();
                writeBufferDrained();
            }
        }
        // Java: 225
        public bool writeEvent(com.oddlabs.net.ARMIEvent @event)
        {
            short event_size = @event.getEventSize();
            int total_event_size = event_size + com.oddlabs.net.Connection.HEADER_SIZE;
            bool fits = total_event_size <= write_buffer.remaining();
            if (fits)
            {
                write_buffer.putShort(event_size);
                @event.write(write_buffer);
            }
            int new_position = network.getDeterministic().log(write_buffer.position());
            write_buffer.position(new_position);
            setWriting();
            return fits;
        }
        // Java: 241
        public int readDeterministic(java.nio.channels.SocketChannel channel)
        {
            int old_position = read_buffer.position();
            int num_bytes_read = (-1);
            System.IO.IOException exception = default;
            try
            {
                if (!network.getDeterministic().isPlayback())
                {
                    num_bytes_read = channel.read(read_buffer);
                }
                exception = null;
            }
            catch (System.IO.IOException e)
            {
                exception = e;
            }
            if (network.getDeterministic().log(exception != null))
            {
                throw ((System.IO.IOException)network.getDeterministic().log(exception));
            }
            else
            {
                num_bytes_read = network.getDeterministic().log(num_bytes_read);
            }
            int new_position = network.getDeterministic().log(read_buffer.position());
            int old_limit = read_buffer.limit();
            read_buffer.limit(new_position);
            read_buffer.position(old_position);
            network.getDeterministic().log(read_buffer);
            read_buffer.limit(old_limit);
            return num_bytes_read;
        }
        // Java: 267
        public void readFromChannel(java.nio.channels.SocketChannel channel)
        {
            bool bytes_read = default;
            do
            {
                int num_bytes_read = readDeterministic(channel);
                if (num_bytes_read == (-1))
                {
                    throw new System.IO.IOException("Channel closed");
                }
                bytes_read = num_bytes_read > 0;
                if (bytes_read)
                {
                    read_buffer.flip();
                    while (read_buffer.remaining() >= com.oddlabs.net.Connection.HEADER_SIZE)
                    {
                        short event_size = read_buffer.getShort(read_buffer.position());
                        if (event_size > read_buffer.capacity() - com.oddlabs.net.Connection.HEADER_SIZE)
                        {
                            handleError(new System.IO.IOException("Message too large: " + (event_size)));
                            return;
                        }
                        if (read_buffer.remaining() >= event_size + com.oddlabs.net.Connection.HEADER_SIZE)
                        {
                            read_buffer.position(read_buffer.position() + com.oddlabs.net.Connection.HEADER_SIZE);
                            com.oddlabs.net.ARMIEvent @event = com.oddlabs.net.ARMIEvent.read(read_buffer, event_size);
                            network.getDeterministic().checkpoint();
                            try
                            {
                                @event.execute(interface_methods, this);
                            }
                            catch (com.oddlabs.net.IllegalARMIEventException e)
                            {
                                throw new System.IO.IOException(java.lang.JStr.valueOf(e), (System.Exception)e);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    read_buffer.compact();
                }
            }
            while (bytes_read && network.getDeterministic().log(network.getDeterministic().isPlayback() || channel.isOpen()));
        }
        // Java: 302
        public void handle()
        {
            java.nio.channels.SocketChannel channel = default;
            if (!network.getDeterministic().isPlayback())
            {
                channel = ((java.nio.channels.SocketChannel)key.channel());
            }
            else
            {
                channel = null;
            }
            network.getDeterministic().checkpoint();
            if (network.getDeterministic().log(network.getDeterministic().isPlayback() || !channel.isConnected()))
            {
                bool success = false;
                System.IO.IOException exception = default;
                try
                {
                    if (!network.getDeterministic().isPlayback())
                    {
                        success = channel.finishConnect();
                    }
                    exception = null;
                }
                catch (System.IO.IOException e)
                {
                    exception = e;
                }
                if (network.getDeterministic().log(exception != null))
                {
                    throw ((System.IO.IOException)network.getDeterministic().log(exception));
                }
                else
                {
                    success = network.getDeterministic().log(success);
                }
                int interest_ops = default;
                if (network.getDeterministic().isPlayback())
                {
                    interest_ops = network.getDeterministic().log((-1));
                }
                else
                {
                    interest_ops = network.getDeterministic().log(key.interestOps());
                }
                int new_ops = (interest_ops | java.nio.channels.SelectionKey.OP_READ) & ~java.nio.channels.SelectionKey.OP_CONNECT;
                if (!network.getDeterministic().isPlayback())
                {
                    key.interestOps(new_ops);
                }
                connected(((java.net.InetAddress)network.getDeterministic().log((network.getDeterministic().isPlayback() ? (java.net.InetAddress)(null) : channel.socket().getLocalAddress()))));
            }
            else
            {
                network.getDeterministic().checkpoint();
                if (writing)
                {
                    writeToChannel(channel);
                }
                readFromChannel(channel);
            }
        }
        // Java: 343
        public bool isKeyValid()
        {
            return !network.getDeterministic().log(network.getDeterministic().isPlayback() || !(key != null && key.isValid()));
        }
        // Java: 349
        public override void doClose()
        {
            if (isKeyValid())
            {
                network.cancelKey(key, this);
                if (!network.getDeterministic().isPlayback())
                {
                    java.nio.channels.SocketChannel channel = ((java.nio.channels.SocketChannel)key.channel());
                    try
                    {
                        channel.socket().shutdownInput();
                    }
                    catch (System.IO.IOException unused)
                    {
                    }
                    try
                    {
                        channel.socket().shutdownOutput();
                    }
                    catch (System.IO.IOException unused_2)
                    {
                    }
                    try
                    {
                        channel.close();
                    }
                    catch (System.IO.IOException unused_3)
                    {
                    }
                }
            }
            network.unregisterForPinging(this);
        }
        // Java: 375
        public void handleError(System.IO.IOException e)
        {
            error(e);
        }
        private void init_Connection()
        {
            this.read_buffer = java.nio.ByteBuffer.allocateDirect(com.oddlabs.net.Connection.BUFFER_SIZE);
            this.back_log_list = new java.util.LinkedList();
            this.write_buffer = java.nio.ByteBuffer.allocateDirect(com.oddlabs.net.Connection.BUFFER_SIZE);
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.net.ConnectionPeerInterface));
            this.writing = false;
            this.pinged = false;
        }
        public Connection(com.oddlabs.net.NetworkSelector network, bool ping_reply) : base()
        {
            init_Connection();
            this.network = network;
            network.registerForPingTimeout(this);
            this.ping_reply = ping_reply;
            com.oddlabs.net.ARMIEventWriter event_writer = new com.oddlabs.net.ARMIEventWriter_L((com.oddlabs.net.ARMIEvent @event) =>
            {
                if (!back_log_list.isEmpty() || !writeEvent(@event))
                {
                    back_log_list.add(@event);
                }
            });
            this.peer_interface = ((com.oddlabs.net.ConnectionPeerInterface)com.oddlabs.net.ARMIEvent.createProxy(event_writer, typeof(com.oddlabs.net.ConnectionPeerInterface)));
            peer_interface.ping();
        }
        public Connection(com.oddlabs.net.NetworkSelector network, string dns_name, int port, com.oddlabs.net.ConnectionInterface connection_interface) : this(network, true)
        {
            setConnectionInterface(connection_interface);
            network.asyncConnect(dns_name, port, this);
        }
        public Connection(com.oddlabs.net.NetworkSelector network, java.net.SocketAddress address, com.oddlabs.net.ConnectionInterface connection_interface) : this(network, true)
        {
            setConnectionInterface(connection_interface);
            connect(address);
        }
        public Connection(com.oddlabs.net.NetworkSelector network, java.nio.channels.SelectionKey key, com.oddlabs.net.ConnectionInterface connection_interface) : this(network, false)
        {
            network.registerForPing(this);
            setConnectionInterface(connection_interface);
            notifyConnected();
            setKey(key);
        }
    }
}
