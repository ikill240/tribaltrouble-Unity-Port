// TCP over a reliable message link (Steam P2P, Unity Relay): lets players reach a server hosted by a friend
// without opening ports. The joining player's game connects to 127.0.0.1:33214 / :11221 as usual (server domain
// "localhost"); TunnelClient carries each TCP connection over the link to the host, where TunnelMux opens the same
// port on the host's own machine (the PlayerServer). Engine-free; call Pump() often (every frame).
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TribalTrouble.Faithful
{
    /// <summary>A reliable, ordered message channel to one remote player.</summary>
    public interface ITunnelLink
    {
        bool IsOpen { get; }
        /// <summary>False while the link's send buffer is full (try again next pump).</summary>
        bool CanSend { get; }
        void Send(byte[] data, int length);
        /// <summary>The next received message, or null.</summary>
        byte[] Receive();
        void Close();
        string Describe { get; }
    }

    /// <summary>The TCP streams multiplexed over one link (either end).</summary>
    public sealed class TunnelMux
    {
        const byte OPEN = 1, DATA = 2, CLOSE = 3;
        const int Chunk = 16 * 1024;

        sealed class Stream
        {
            public Socket sock;
            public readonly Queue<byte[]> toSocket = new Queue<byte[]>();
            public int toSocketOffset;
            public bool remoteClosed, localClosed;
        }

        public readonly ITunnelLink link;
        readonly bool _hostSide;
        readonly Dictionary<int, Stream> _streams = new Dictionary<int, Stream>();
        readonly byte[] _buf = new byte[Chunk + 5];
        int _nextId = 1;
        public long bytesIn, bytesOut;

        public TunnelMux(ITunnelLink link, bool hostSide) { this.link = link; _hostSide = hostSide; }

        public int StreamCount => _streams.Count;

        /// <summary>Client side: a local TCP connection to carry to the host's port.</summary>
        public void Open(Socket local, int port)
        {
            int id = _nextId++;
            local.Blocking = false;
            local.NoDelay = true;
            _streams[id] = new Stream { sock = local };
            var m = new byte[7];
            m[0] = OPEN; WriteInt(m, 1, id); m[5] = (byte)(port >> 8); m[6] = (byte)port;
            link.Send(m, m.Length);
        }

        public void Pump()
        {
            if (!link.IsOpen) { CloseAll(); return; }
            // link -> sockets
            for (int n = 0; n < 512; n++)
            {
                var msg = link.Receive();
                if (msg == null) break;
                if (msg.Length < 5) continue;
                int id = ReadInt(msg, 1);
                switch (msg[0])
                {
                    case OPEN:
                        if (!_hostSide || msg.Length < 7) break;
                        int port = (msg[5] << 8) | msg[6];
                        if (port != PlayerServer.MatchmakingPort && port != PlayerServer.RouterPort) { SendClose(id); break; }   // only the game's ports
                        try
                        {
                            var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
                            s.Connect(IPAddress.Loopback, port);
                            s.Blocking = false;
                            _streams[id] = new Stream { sock = s };
                        }
                        catch (Exception) { SendClose(id); }
                        break;
                    case DATA:
                        if (_streams.TryGetValue(id, out var st) && !st.localClosed)
                        {
                            var d = new byte[msg.Length - 5];
                            Buffer.BlockCopy(msg, 5, d, 0, d.Length);
                            st.toSocket.Enqueue(d);
                            bytesIn += d.Length;
                        }
                        break;
                    case CLOSE:
                        if (_streams.TryGetValue(id, out var sc)) sc.remoteClosed = true;
                        break;
                }
            }
            // sockets <-> link
            List<int> dead = null;
            foreach (var kv in _streams)
            {
                var st = kv.Value;
                try
                {
                    // write what arrived
                    while (st.toSocket.Count > 0)
                    {
                        var d = st.toSocket.Peek();
                        int w = st.sock.Send(d, st.toSocketOffset, d.Length - st.toSocketOffset, SocketFlags.None, out var err);
                        if (err == SocketError.WouldBlock) break;
                        if (err != SocketError.Success) throw new SocketException((int)err);
                        st.toSocketOffset += w;
                        if (st.toSocketOffset < d.Length) break;
                        st.toSocket.Dequeue();
                        st.toSocketOffset = 0;
                    }
                    // read what the local side sent
                    for (int k = 0; k < 8 && link.CanSend && !st.localClosed; k++)
                    {
                        if (st.sock.Available <= 0 && !st.sock.Poll(0, SelectMode.SelectRead)) break;
                        int r = st.sock.Receive(_buf, 5, Chunk, SocketFlags.None, out var err);
                        if (err == SocketError.WouldBlock) break;
                        if (err != SocketError.Success || r == 0) { st.localClosed = true; break; }
                        _buf[0] = DATA; WriteInt(_buf, 1, kv.Key);
                        link.Send(_buf, r + 5);
                        bytesOut += r;
                    }
                }
                catch (Exception) { st.localClosed = true; }
                if (st.localClosed || (st.remoteClosed && st.toSocket.Count == 0))
                    (dead ??= new List<int>()).Add(kv.Key);
            }
            if (dead != null)
                foreach (int id in dead)
                {
                    var st = _streams[id];
                    if (!st.remoteClosed) SendClose(id);
                    try { st.sock.Shutdown(SocketShutdown.Both); } catch (Exception) { }
                    try { st.sock.Close(); } catch (Exception) { }
                    _streams.Remove(id);
                }
        }

        void SendClose(int id)
        {
            var m = new byte[5];
            m[0] = CLOSE; WriteInt(m, 1, id);
            if (link.IsOpen) link.Send(m, 5);
        }

        public void CloseAll()
        {
            foreach (var st in _streams.Values) { try { st.sock.Close(); } catch (Exception) { } }
            _streams.Clear();
        }

        static void WriteInt(byte[] b, int o, int v) { b[o] = (byte)(v >> 24); b[o + 1] = (byte)(v >> 16); b[o + 2] = (byte)(v >> 8); b[o + 3] = (byte)v; }
        static int ReadInt(byte[] b, int o) => (b[o] << 24) | (b[o + 1] << 16) | (b[o + 2] << 8) | b[o + 3];
    }

    /// <summary>
    /// Joining side: listens on 127.0.0.1:33214 and :11221 and carries every connection to the host over the link.
    /// While it runs, the server domain is "localhost".
    /// </summary>
    public sealed class TunnelClient
    {
        readonly TcpListener[] _listeners;
        readonly int[] _ports = { PlayerServer.MatchmakingPort, PlayerServer.RouterPort };
        public readonly TunnelMux mux;
        public string Error { get; private set; }

        public TunnelClient(ITunnelLink link)
        {
            mux = new TunnelMux(link, false);
            _listeners = new TcpListener[_ports.Length];
            for (int i = 0; i < _ports.Length; i++)
            {
                try
                {
                    _listeners[i] = new TcpListener(IPAddress.Loopback, _ports[i]);
                    _listeners[i].Start();
                }
                catch (Exception e)
                {
                    Error = "Port " + _ports[i] + " is in use on this PC (a server running here?): " + e.Message;
                    _listeners[i] = null;
                }
            }
        }

        public bool IsOpen => mux.link.IsOpen;

        public void Pump()
        {
            for (int i = 0; i < _listeners.Length; i++)
            {
                var l = _listeners[i];
                if (l == null) continue;
                try { while (l.Pending()) mux.Open(l.AcceptSocket(), _ports[i]); } catch (Exception) { }
            }
            mux.Pump();
        }

        public void Close()
        {
            foreach (var l in _listeners) { try { l?.Stop(); } catch (Exception) { } }
            mux.CloseAll();
            mux.link.Close();
        }
    }

    /// <summary>An in-process link pair, for tests.</summary>
    public sealed class LoopbackLink : ITunnelLink
    {
        readonly Queue<byte[]> _in = new Queue<byte[]>();
        LoopbackLink _peer;
        bool _open = true;
        public static (LoopbackLink, LoopbackLink) Pair() { var a = new LoopbackLink(); var b = new LoopbackLink(); a._peer = b; b._peer = a; return (a, b); }
        public bool IsOpen => _open && _peer._open;
        public bool CanSend => true;
        public void Send(byte[] data, int length) { var c = new byte[length]; Buffer.BlockCopy(data, 0, c, 0, length); lock (_peer._in) _peer._in.Enqueue(c); }
        public byte[] Receive() { lock (_in) return _in.Count > 0 ? _in.Dequeue() : null; }
        public void Close() { _open = false; }
        public string Describe => "loopback";
    }
}
