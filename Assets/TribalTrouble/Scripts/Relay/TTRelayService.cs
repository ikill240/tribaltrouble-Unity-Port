#if TT_UGS
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TribalTrouble.Faithful;
using TribalTrouble.FaithfulGame;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Networking.Transport.Utilities;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace TribalTrouble.RelayOnline
{
    /// <summary>
    /// Unity Relay: a friend joins with a short code, the traffic goes through Unity's relay servers (no ports to
    /// open). The host keeps one Unity Transport driver; each joining player is one connection carrying the
    /// tunnel's messages over a reliable, ordered pipeline. Needs the project linked to a Unity Cloud project
    /// (Edit → Project Settings → Services) with Relay enabled in the Unity dashboard.
    /// </summary>
    public sealed class TTRelayService : TribalTrouble.FaithfulGame.IRelayService
    {
        const string ConnectionType = "dtls";
        public string Status { get; private set; } = "";

        NetworkDriver _host, _client;
        NetworkPipeline _hostPipe, _clientPipe;
        readonly Dictionary<NetworkConnection, RelayLink> _hostLinks = new Dictionary<NetworkConnection, RelayLink>();
        readonly Queue<ITunnelLink> _incoming = new Queue<ITunnelLink>();
        RelayLink _clientLink;
        bool _signingIn;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Boot()
        {
            // Relay only works when the project is linked to a Unity Cloud project (Edit > Project Settings >
            // Services); without it every attempt fails, so the option stays hidden and the Friends page says why
            if (string.IsNullOrEmpty(Application.cloudProjectId))
            {
                TTRelay.UnavailableReason = "Unity Relay is off: link the project to a Unity Cloud project (Edit > Project Settings > Services) and turn on Relay in the Unity dashboard, then rebuild.";
                return;
            }
            var s = new TTRelayService();
            TTRelay.Service = s;
            Application.quitting += s.Dispose;
        }

        static async Task SignIn()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized) await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn) await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        static NetworkDriver CreateDriver(RelayServerData data, out NetworkPipeline pipeline)
        {
            var settings = new NetworkSettings();
            settings.WithRelayParameters(ref data);
            settings.WithFragmentationStageParameters(payloadCapacity: 20 * 1024);
            settings.WithReliableStageParameters(windowSize: 64);
            var driver = NetworkDriver.Create(settings);
            pipeline = driver.CreatePipeline(typeof(FragmentationPipelineStage), typeof(ReliableSequencedPipelineStage));
            return driver;
        }

        // ------------------------------------------------------------------ host

        public async void StartHost(int maxPeers, Action<string> onJoinCode, Action<string> onError)
        {
            if (_signingIn) return;
            _signingIn = true;
            try
            {
                Status = "Signing in to Unity Services...";
                await SignIn();
                Status = "Creating a relay...";
                var alloc = await RelayService.Instance.CreateAllocationAsync(maxPeers);
                var data = AllocationUtils.ToRelayServerData(alloc, ConnectionType);
                StopHost();
                _host = CreateDriver(data, out _hostPipe);
                if (_host.Bind(NetworkEndpoint.AnyIpv4) != 0) throw new Exception("couldn't bind the relay driver");
                if (_host.Listen() != 0) throw new Exception("couldn't listen on the relay");
                string code = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
                Status = "Relay code " + code;
                onJoinCode?.Invoke(code);
            }
            catch (Exception e)
            {
                Status = "";
                Debug.LogWarning("[TT] Unity Relay: " + e);
                onError?.Invoke(Explain(e));
            }
            finally { _signingIn = false; }
        }

        public void StopHost()
        {
            foreach (var l in _hostLinks.Values) l.closed = true;
            _hostLinks.Clear();
            if (_host.IsCreated) _host.Dispose();
            _host = default;
        }

        public ITunnelLink TakeIncoming() => _incoming.Count > 0 ? _incoming.Dequeue() : null;

        // ------------------------------------------------------------------ join

        public async void Join(string joinCode, Action<ITunnelLink> onLink, Action<string> onError)
        {
            try
            {
                Status = "Signing in to Unity Services...";
                await SignIn();
                Status = "Joining " + joinCode + "...";
                var alloc = await RelayService.Instance.JoinAllocationAsync(joinCode);
                var data = AllocationUtils.ToRelayServerData(alloc, ConnectionType);
                CloseClient();
                _client = CreateDriver(data, out _clientPipe);
                if (_client.Bind(NetworkEndpoint.AnyIpv4) != 0) throw new Exception("couldn't bind the relay driver");
                var conn = _client.Connect(data.Endpoint);
                _clientLink = new RelayLink(this, conn, false, "Unity Relay " + joinCode) { connecting = true };
                Status = "";
                onLink?.Invoke(_clientLink);
            }
            catch (Exception e)
            {
                Status = "";
                Debug.LogWarning("[TT] Unity Relay: " + e);
                onError?.Invoke(Explain(e));
            }
        }

        void CloseClient()
        {
            if (_clientLink != null) _clientLink.closed = true;
            _clientLink = null;
            if (_client.IsCreated) _client.Dispose();
            _client = default;
        }

        static string Explain(Exception e)
        {
            string m = e.Message;
            if (e is ServicesInitializationException || m.Contains("project") && m.Contains("link"))
                return "Unity Services aren't set up: link the project to a Unity Cloud project (Edit > Project Settings > Services) and enable Relay in the Unity dashboard. (" + m + ")";
            if (m.Contains("join code") || m.Contains("not found")) return "No game with that code (" + m + ")";
            return m;
        }

        // ------------------------------------------------------------------ frame

        public void Tick()
        {
            if (_host.IsCreated) Pump(_host, true);
            if (_client.IsCreated) Pump(_client, false);
        }

        void Pump(NetworkDriver driver, bool host)
        {
            driver.ScheduleUpdate().Complete();
            if (host)
            {
                NetworkConnection c;
                while ((c = driver.Accept()) != default)
                {
                    var l = new RelayLink(this, c, true, "Relay player " + c);
                    _hostLinks[c] = l;
                    _incoming.Enqueue(l);
                }
            }
            NetworkEvent.Type ev;
            while ((ev = driver.PopEvent(out var conn, out var reader, out _)) != NetworkEvent.Type.Empty)
            {
                RelayLink link = host ? (_hostLinks.TryGetValue(conn, out var hl) ? hl : null) : _clientLink;
                if (link == null) continue;
                switch (ev)
                {
                    case NetworkEvent.Type.Connect:
                        link.connecting = false;
                        break;
                    case NetworkEvent.Type.Data:
                        var buf = new NativeArray<byte>(reader.Length, Allocator.Temp);
                        reader.ReadBytes(buf);
                        link.inbox.Enqueue(buf.ToArray());
                        buf.Dispose();
                        break;
                    case NetworkEvent.Type.Disconnect:
                        link.closed = true;
                        if (host) _hostLinks.Remove(conn);
                        break;
                }
            }
            if (host) { foreach (var l in _hostLinks.Values) l.Flush(); }
            else _clientLink?.Flush();
        }

        internal bool TrySend(RelayLink l, byte[] data)
        {
            var driver = l.hostSide ? _host : _client;
            var pipe = l.hostSide ? _hostPipe : _clientPipe;
            if (!driver.IsCreated) { l.closed = true; return false; }
            if (driver.BeginSend(pipe, l.conn, out var writer, data.Length) != 0) return false;
            var na = new NativeArray<byte>(data, Allocator.Temp);
            writer.WriteBytes(na);
            na.Dispose();
            if (writer.HasFailedWrites) { driver.AbortSend(writer); return false; }
            return driver.EndSend(writer) >= 0;
        }

        internal void Disconnect(RelayLink l)
        {
            var driver = l.hostSide ? _host : _client;
            if (driver.IsCreated) driver.Disconnect(l.conn);
            if (l.hostSide) _hostLinks.Remove(l.conn);
        }

        void Dispose()
        {
            StopHost();
            CloseClient();
        }

        internal sealed class RelayLink : ITunnelLink
        {
            readonly TTRelayService _svc;
            public readonly NetworkConnection conn;
            public readonly bool hostSide;
            public bool connecting, closed;
            public readonly Queue<byte[]> inbox = new Queue<byte[]>();
            readonly Queue<byte[]> _out = new Queue<byte[]>();
            public string Describe { get; }

            public RelayLink(TTRelayService s, NetworkConnection c, bool host, string what) { _svc = s; conn = c; hostSide = host; Describe = what; }

            public bool IsOpen => !closed;
            public bool CanSend => !closed && !connecting && _out.Count < 32;

            public void Send(byte[] data, int length)
            {
                if (closed) return;
                var c = new byte[length];
                Buffer.BlockCopy(data, 0, c, 0, length);
                _out.Enqueue(c);
                Flush();
            }

            public void Flush()
            {
                if (closed || connecting) return;
                while (_out.Count > 0 && _svc.TrySend(this, _out.Peek())) _out.Dequeue();
            }

            public byte[] Receive() => inbox.Count > 0 ? inbox.Dequeue() : null;

            public void Close()
            {
                if (closed) return;
                closed = true;
                _svc.Disconnect(this);
            }
        }
    }
}
#endif
