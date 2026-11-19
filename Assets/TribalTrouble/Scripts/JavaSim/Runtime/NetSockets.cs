// java.net addresses and java.nio non-blocking socket channels + Selector over System.Net.Sockets. The network
// layer (NetworkSelector / Connection / ConnectionListener) polls these once per frame, exactly like the Java game.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace java.net
{
    public class InetAddress : java.lang.Object
    {
        internal readonly byte[] addr;          // 4 bytes (IPv4) or 16 (IPv6)
        internal string hostName;

        internal InetAddress(byte[] addr, string hostName) { this.addr = addr; this.hostName = hostName; }

        internal static InetAddress Of(IPAddress ip, string host = null)
        {
            if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();
            var b = ip.GetAddressBytes();
            return b.Length == 4 ? new Inet4Address(b, host) : (InetAddress)new Inet6Address(b, host);
        }
        internal IPAddress ToIP() => new IPAddress(addr);

        /// <summary>The last connect / name lookup failure (for the multiplayer screens' error text).</summary>
        public static volatile string LastError;
        internal static System.IO.IOException NetError(string m)
        {
            LastError = m;
            try { java.lang.IO.println("network: " + m); } catch { }
            return new System.IO.IOException(m);
        }

        public static InetAddress getByName(string host)
        {
            if (string.IsNullOrEmpty(host)) return getLoopbackAddress();
            if (IPAddress.TryParse(host, out var ip)) return Of(ip);
            // RFC 6761: "localhost" and every "*.localhost" name is the loopback address (matchmaking.localhost / router.localhost for a local server)
            if (host.Equals("localhost", StringComparison.OrdinalIgnoreCase) || host.EndsWith(".localhost", StringComparison.OrdinalIgnoreCase))
                return new Inet4Address(new byte[] { 127, 0, 0, 1 }, host);
            try
            {
                var all = Dns.GetHostAddresses(host);
                foreach (var a in all) if (a.AddressFamily == AddressFamily.InterNetwork) return Of(a, host);
                if (all.Length > 0) return Of(all[0], host);
            }
            catch (Exception e) { throw NetError("UnknownHostException: " + host + ": " + e.Message); }
            throw NetError("UnknownHostException: " + host);
        }
        public static InetAddress[] getAllByName(string host) => new[] { getByName(host) };
        public static InetAddress getByAddress(sbyte[] a)
        {
            var b = (byte[])((byte[])(Array)a).Clone();
            return b.Length == 4 ? new Inet4Address(b, null) : (InetAddress)new Inet6Address(b, null);
        }
        public static InetAddress getByAddress(string host, sbyte[] a) { var r = getByAddress(a); r.hostName = host; return r; }
        public static InetAddress getLoopbackAddress() => new Inet4Address(new byte[] { 127, 0, 0, 1 }, "localhost");
        public static InetAddress getLocalHost()
        {
            try
            {
                string name = Dns.GetHostName();
                foreach (var a in Dns.GetHostAddresses(name)) if (a.AddressFamily == AddressFamily.InterNetwork) return Of(a, name);
            }
            catch { }
            return getLoopbackAddress();
        }

        public sbyte[] getAddress() => (sbyte[])(Array)addr.Clone();
        public string getHostAddress() => ToIP().ToString();
        public string getHostName()
        {
            if (hostName == null)
            {
                try { hostName = Dns.GetHostEntry(ToIP()).HostName; } catch { hostName = getHostAddress(); }
            }
            return hostName;
        }
        public string getCanonicalHostName() => getHostName();
        public bool isLoopbackAddress() => IPAddress.IsLoopback(ToIP());
        public bool isAnyLocalAddress() => Array.TrueForAll(addr, b => b == 0);
        public bool isSiteLocalAddress() => addr.Length == 4 && (addr[0] == 10 || (addr[0] == 172 && (addr[1] & 0xf0) == 16) || (addr[0] == 192 && addr[1] == 168));
        public bool isLinkLocalAddress() => addr.Length == 4 ? addr[0] == 169 && addr[1] == 254 : ToIP().IsIPv6LinkLocal;
        public bool isMulticastAddress() => addr.Length == 4 ? (addr[0] & 0xf0) == 224 : ToIP().IsIPv6Multicast;

        public override bool equals(object o) => o is InetAddress i && i.addr.Length == addr.Length && ((IStructuralEquatable)i.addr).Equals(addr, StructuralComparisons.StructuralEqualityComparer);
        public override int hashCode() => addr.Length == 4 ? AddressInt() : ((IStructuralEquatable)addr).GetHashCode(StructuralComparisons.StructuralEqualityComparer);
        public override string toString() => (hostName ?? "") + "/" + getHostAddress();

        // Java serialization (InetAddress's serialPersistentFields: hostName, address, family)
        internal int AddressInt() => addr.Length == 4 ? (addr[0] << 24) | (addr[1] << 16) | (addr[2] << 8) | addr[3] : 0;
        internal int Family() => addr.Length == 4 ? 1 : 2;
        internal string HostNameOrNull() => hostName;
        internal static InetAddress FromSerial(Dictionary<string, object> v)
        {
            int a = v.TryGetValue("address", out var x) && x is int i ? i : 0;
            v.TryGetValue("hostName", out var h);
            return new Inet4Address(new[] { (byte)(a >> 24), (byte)(a >> 16), (byte)(a >> 8), (byte)a }, h as string);
        }
    }

    public sealed class Inet4Address : InetAddress { internal Inet4Address(byte[] a, string h) : base(a, h) { } }
    public sealed class Inet6Address : InetAddress { internal Inet6Address(byte[] a, string h) : base(a, h) { } }

    public abstract class SocketAddress : java.lang.Object { }

    public sealed class InetSocketAddress : SocketAddress
    {
        readonly InetAddress address;
        readonly string host;
        readonly int port;
        public InetSocketAddress(InetAddress a, int port) { address = a ?? new Inet4Address(new byte[4], null); this.port = port; }
        public InetSocketAddress(int port) : this((InetAddress)null, port) { }
        public InetSocketAddress(string host, int port)
        {
            this.host = host;
            this.port = port;
            try { address = InetAddress.getByName(host); } catch { address = null; }
        }
        public static InetSocketAddress createUnresolved(string host, int port) => new InetSocketAddress(host, port);
        public InetAddress getAddress() => address;
        public int getPort() => port;
        public string getHostName() => host ?? address?.getHostName();
        public string getHostString() => host ?? address?.getHostAddress();
        public bool isUnresolved() => address == null;
        internal IPEndPoint ToEndPoint()
        {
            if (address == null) throw new System.IO.IOException("UnresolvedAddressException: " + host);
            return new IPEndPoint(address.ToIP(), port);
        }
        internal static InetSocketAddress Of(EndPoint e) => e is IPEndPoint ip ? new InetSocketAddress(InetAddress.Of(ip.Address), ip.Port) : null;
        public override bool equals(object o) => o is InetSocketAddress s && s.port == port && Equals(s.address, address);
        public override int hashCode() => (address?.hashCode() ?? 0) + port;
        public override string toString() => (host ?? "") + "/" + (address?.getHostAddress() ?? "<unresolved>") + ":" + port;
    }

    public sealed class Socket : java.lang.Object
    {
        internal readonly System.Net.Sockets.Socket s;
        internal Socket(System.Net.Sockets.Socket s) { this.s = s; }
        public void setTcpNoDelay(bool b) => s.NoDelay = b;
        public bool getTcpNoDelay() => s.NoDelay;
        public void setReuseAddress(bool b) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, b);
        public void setSoTimeout(int ms) { s.ReceiveTimeout = ms; }
        public void setKeepAlive(bool b) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, b);
        public void setReceiveBufferSize(int n) => s.ReceiveBufferSize = n;
        public void setSendBufferSize(int n) => s.SendBufferSize = n;
        public InetAddress getLocalAddress() => s.LocalEndPoint is IPEndPoint e ? InetAddress.Of(e.Address) : null;
        public int getLocalPort() => s.LocalEndPoint is IPEndPoint e ? e.Port : -1;
        public InetAddress getInetAddress() => s.RemoteEndPoint is IPEndPoint e ? InetAddress.Of(e.Address) : null;
        public int getPort() => s.RemoteEndPoint is IPEndPoint e ? e.Port : 0;
        public SocketAddress getRemoteSocketAddress() { try { return InetSocketAddress.Of(s.RemoteEndPoint); } catch { return null; } }
        public SocketAddress getLocalSocketAddress() => InetSocketAddress.Of(s.LocalEndPoint);
        public void shutdownInput() { try { s.Shutdown(SocketShutdown.Receive); } catch (Exception e) { throw new System.IO.IOException(e.Message); } }
        public void shutdownOutput() { try { s.Shutdown(SocketShutdown.Send); } catch (Exception e) { throw new System.IO.IOException(e.Message); } }
        public void close() => s.Close();
    }

    public sealed class ServerSocket : java.lang.Object
    {
        internal readonly System.Net.Sockets.Socket s;
        internal ServerSocket(System.Net.Sockets.Socket s) { this.s = s; }
        public void setReuseAddress(bool b) => s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, b);
        public void bind(SocketAddress a) => bind(a, 50);
        public void bind(SocketAddress a, int backlog)
        {
            try
            {
                s.Bind(a == null ? new IPEndPoint(IPAddress.Any, 0) : ((InetSocketAddress)a).ToEndPoint());
                s.Listen(backlog <= 0 ? 50 : backlog);
            }
            catch (SocketException e) { throw new System.IO.IOException("BindException: " + e.Message); }
        }
        public int getLocalPort() => s.LocalEndPoint is IPEndPoint e ? e.Port : -1;
        public InetAddress getInetAddress() => s.LocalEndPoint is IPEndPoint e ? InetAddress.Of(e.Address) : null;
        public void close() => s.Close();
    }

    public sealed class NetworkInterface : java.lang.Object
    {
        readonly System.Net.NetworkInformation.NetworkInterface ni;
        NetworkInterface(System.Net.NetworkInformation.NetworkInterface ni) { this.ni = ni; }
        public static java.util.Enumeration getNetworkInterfaces()
        {
            var l = new java.util.ArrayList();
            try { foreach (var n in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()) l.add(new NetworkInterface(n)); }
            catch { }
            return java.util.Collections.enumeration(l);
        }
        public java.util.Enumeration getInetAddresses()
        {
            var l = new java.util.ArrayList();
            try { foreach (var a in ni.GetIPProperties().UnicastAddresses) l.add(InetAddress.Of(a.Address)); } catch { }
            return java.util.Collections.enumeration(l);
        }
        public string getName() => ni.Name;
        public string getDisplayName() => ni.Description;
        public bool isUp() => ni.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up;
        public bool isLoopback() => ni.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Loopback;
    }
}

namespace java.nio.channels
{
    using java.net;

    public abstract class SelectableChannel : java.lang.Object, Channel
    {
        internal System.Net.Sockets.Socket sock;
        internal bool open_ = true;
        internal readonly List<SelectionKey> keys = new List<SelectionKey>();

        public SelectableChannel configureBlocking(bool b) { sock.Blocking = b; return this; }
        public bool isBlocking() => sock.Blocking;
        public bool isOpen() => open_;
        public SelectionKey register(Selector sel, int ops) => register(sel, ops, null);
        public SelectionKey register(Selector sel, int ops, object att)
        {
            if (!open_) throw new System.IO.IOException("ClosedChannelException");
            if (sock.Blocking) throw new InvalidOperationException("IllegalBlockingModeException");
            var k = keyFor(sel);
            if (k != null) { k.interestOps(ops); if (att != null) k.attach(att); return k; }
            k = new SelectionKey(this, sel, ops);
            k.attach(att);
            keys.Add(k);
            sel.Add(k);
            return k;
        }
        public SelectionKey keyFor(Selector sel) { foreach (var k in keys) if (k.selector() == sel && k.isValid()) return k; return null; }
        public bool isRegistered() => keys.Exists(k => k.isValid());
        public virtual void close()
        {
            if (!open_) return;
            open_ = false;
            foreach (var k in keys.ToArray()) k.cancel();
            try { sock.Close(); } catch { }
        }
        internal abstract int Ready(int interest);
    }

    public sealed class SocketChannel : SelectableChannel, ByteChannel
    {
        bool connected, pending, inputDown;
        Socket socket_;

        SocketChannel(System.Net.Sockets.Socket s, bool connected)
        {
            sock = s;
            this.connected = connected;
            socket_ = new Socket(s);
        }

        public static SocketChannel open() => new SocketChannel(new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), false);
        public static SocketChannel open(SocketAddress a) { var c = open(); c.configureBlocking(true); c.connect(a); return c; }
        internal static SocketChannel Accepted(System.Net.Sockets.Socket s) => new SocketChannel(s, true);

        public Socket socket() => socket_;
        public bool isConnected() => connected;
        public bool isConnectionPending() => pending;
        public SocketAddress getRemoteAddress() => socket_.getRemoteSocketAddress();
        public SocketAddress getLocalAddress() => socket_.getLocalSocketAddress();

        public bool connect(SocketAddress a)
        {
            var ep = ((InetSocketAddress)a).ToEndPoint();
            try
            {
                sock.Connect(ep);
                connected = true;
                return true;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock || e.SocketErrorCode == SocketError.InProgress || e.SocketErrorCode == SocketError.AlreadyInProgress)
            {
                pending = true;
                return false;
            }
            catch (SocketException e) { throw InetAddress.NetError("ConnectException: " + e.Message); }
        }

        public bool finishConnect()
        {
            if (connected) return true;
            if (!pending) throw new InvalidOperationException("NoConnectionPendingException");
            if (sock.Poll(0, SelectMode.SelectError))
            {
                int err = 0;
                try { err = (int)sock.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error); } catch { }
                pending = false;
                throw InetAddress.NetError("ConnectException: " + (err != 0 ? new SocketException(err).Message : "Connection refused"));
            }
            if (sock.Poll(0, SelectMode.SelectWrite))
            {
                pending = false;
                connected = true;
                return true;
            }
            return false;
        }

        public int read(java.nio.ByteBuffer b)
        {
            if (!open_) throw new System.IO.IOException("ClosedChannelException");
            if (!connected) throw new InvalidOperationException("NotYetConnectedException");
            if (inputDown) return -1;
            int n = b.remaining();
            if (n == 0) return 0;
            try
            {
                if (!sock.Blocking && sock.Available == 0 && !sock.Poll(0, SelectMode.SelectRead)) return 0;
                int r = sock.Receive((byte[])(Array)b.a, b.off + b.position(), n, SocketFlags.None);
                if (r == 0) { inputDown = true; return -1; }
                b.position(b.position() + r);
                return r;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock) { return 0; }
            catch (SocketException e) { throw new System.IO.IOException(e.Message); }
            catch (ObjectDisposedException) { throw new System.IO.IOException("ClosedChannelException"); }
        }

        public int write(java.nio.ByteBuffer b)
        {
            if (!open_) throw new System.IO.IOException("ClosedChannelException");
            if (!connected) throw new InvalidOperationException("NotYetConnectedException");
            int n = b.remaining();
            if (n == 0) return 0;
            try
            {
                int w = sock.Send((byte[])(Array)b.a, b.off + b.position(), n, SocketFlags.None);
                b.position(b.position() + w);
                return w;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock) { return 0; }
            catch (SocketException e) { throw new System.IO.IOException(e.Message); }
            catch (ObjectDisposedException) { throw new System.IO.IOException("ClosedChannelException"); }
        }

        public SocketChannel shutdownInput() { socket_.shutdownInput(); return this; }
        public SocketChannel shutdownOutput() { socket_.shutdownOutput(); return this; }

        internal override int Ready(int interest)
        {
            int r = 0;
            if (!open_) return 0;
            try
            {
                if ((interest & SelectionKey.OP_CONNECT) != 0 && pending && (sock.Poll(0, SelectMode.SelectWrite) || sock.Poll(0, SelectMode.SelectError))) r |= SelectionKey.OP_CONNECT;
                if ((interest & SelectionKey.OP_READ) != 0 && connected && (inputDown || sock.Poll(0, SelectMode.SelectRead))) r |= SelectionKey.OP_READ;
                if ((interest & SelectionKey.OP_WRITE) != 0 && connected && sock.Poll(0, SelectMode.SelectWrite)) r |= SelectionKey.OP_WRITE;
            }
            catch (ObjectDisposedException) { }
            catch (SocketException) { r |= interest & (SelectionKey.OP_READ | SelectionKey.OP_CONNECT); }
            return r;
        }
    }

    public sealed class ServerSocketChannel : SelectableChannel
    {
        readonly ServerSocket socket_;
        ServerSocketChannel(System.Net.Sockets.Socket s) { sock = s; socket_ = new ServerSocket(s); }
        public static ServerSocketChannel open() => new ServerSocketChannel(new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
        public ServerSocket socket() => socket_;
        public ServerSocketChannel bind(SocketAddress a) { socket_.bind(a); return this; }
        public ServerSocketChannel bind(SocketAddress a, int backlog) { socket_.bind(a, backlog); return this; }
        public SocketAddress getLocalAddress() => InetSocketAddress.Of(sock.LocalEndPoint);

        public SocketChannel accept()
        {
            if (!open_) throw new System.IO.IOException("ClosedChannelException");
            try
            {
                if (!sock.Blocking && !sock.Poll(0, SelectMode.SelectRead)) return null;
                return SocketChannel.Accepted(sock.Accept());
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.WouldBlock) { return null; }
            catch (SocketException e) { throw new System.IO.IOException(e.Message); }
        }

        internal override int Ready(int interest)
        {
            if (!open_ || (interest & SelectionKey.OP_ACCEPT) == 0) return 0;
            try { return sock.Poll(0, SelectMode.SelectRead) ? SelectionKey.OP_ACCEPT : 0; }
            catch { return 0; }
        }
    }

    public sealed class SelectionKey : java.lang.Object
    {
        public const int OP_READ = 1, OP_WRITE = 4, OP_CONNECT = 8, OP_ACCEPT = 16;
        readonly SelectableChannel ch;
        readonly Selector sel;
        volatile int interest;
        internal int ready;
        volatile bool valid = true;
        object att;

        internal SelectionKey(SelectableChannel ch, Selector sel, int ops) { this.ch = ch; this.sel = sel; interest = ops; }

        public SelectableChannel channel() => ch;
        public Selector selector() => sel;
        public bool isValid() => valid;
        public int interestOps() { if (!valid) throw new InvalidOperationException("CancelledKeyException"); return interest; }
        public SelectionKey interestOps(int ops) { if (!valid) throw new InvalidOperationException("CancelledKeyException"); interest = ops; return this; }
        public int readyOps() { if (!valid) throw new InvalidOperationException("CancelledKeyException"); return ready; }
        public bool isReadable() => (readyOps() & OP_READ) != 0;
        public bool isWritable() => (readyOps() & OP_WRITE) != 0;
        public bool isConnectable() => (readyOps() & OP_CONNECT) != 0;
        public bool isAcceptable() => (readyOps() & OP_ACCEPT) != 0;
        public object attach(object o) { var old = att; att = o; return old; }
        public object attachment() => att;
        public void cancel()
        {
            if (!valid) return;
            valid = false;
            sel.Remove(this);
            ch.keys.Remove(this);
        }
    }

    public sealed class Selector : java.lang.Object, java.lang.AutoCloseable
    {
        readonly List<SelectionKey> all = new List<SelectionKey>();
        readonly java.util.LinkedHashSet selected = new java.util.LinkedHashSet();
        readonly System.Threading.ManualResetEventSlim wake = new System.Threading.ManualResetEventSlim(false);
        bool open_ = true;

        public static Selector open() => new Selector();
        public bool isOpen() => open_;

        internal void Add(SelectionKey k) { lock (all) all.Add(k); }
        internal void Remove(SelectionKey k) { lock (all) all.Remove(k); selected.remove(k); }

        public java.util.Set keys() { var s = new java.util.LinkedHashSet(); lock (all) foreach (var k in all) s.add(k); return s; }
        public java.util.Set selectedKeys() => selected;

        int Poll()
        {
            SelectionKey[] ks;
            lock (all) ks = all.ToArray();
            int n = 0;
            foreach (var k in ks)
            {
                if (!k.isValid()) continue;
                int r = k.channel().Ready(k.interestOps());
                if (r == 0) continue;
                if (selected.contains(k))
                {
                    if ((k.ready | r) != k.ready) { k.ready |= r; n++; }
                }
                else
                {
                    k.ready = r;
                    selected.add(k);
                    n++;
                }
            }
            return n;
        }

        public int selectNow() => Poll();
        public int select() => select(0);
        public int select(long timeout)
        {
            var until = timeout > 0 ? DateTime.UtcNow.AddMilliseconds(timeout) : DateTime.MaxValue;
            while (true)
            {
                int n = Poll();
                if (n > 0) return n;
                if (wake.IsSet) { wake.Reset(); return 0; }
                if (DateTime.UtcNow >= until) return 0;
                wake.Wait(2);
            }
        }
        public Selector wakeup() { wake.Set(); return this; }
        public void close()
        {
            open_ = false;
            SelectionKey[] ks;
            lock (all) ks = all.ToArray();
            foreach (var k in ks) k.cancel();
        }
    }
}
