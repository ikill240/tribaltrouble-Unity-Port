using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Steamworks;
using TribalTrouble.Faithful;
using TribalTrouble.FaithfulGame;
using UnityEngine;

namespace TribalTrouble.SteamOnline
{
    /// <summary>
    /// Steam through Steamworks.NET: login tickets for the matchmaking server (the Java SteamManager), rich presence,
    /// invites and "Join game", and P2P tunnels (Steam Datagram Relay) to a friend's hosted server.
    /// Starts by itself when the game starts and the Steam client is running.
    /// </summary>
    public sealed class TTSteamService : ISteamService, TribalTrouble.Bridge.ISteam
    {
        const int VirtualPort = 7;    // our P2P "port"
        public bool Running { get; private set; }
        public string LastError { get; private set; }
        public uint AppId { get; private set; }
        public ulong SteamId { get; private set; }
        public string PersonaName { get; private set; } = "";

        HSteamListenSocket _listen = HSteamListenSocket.Invalid;
        readonly Dictionary<HSteamNetConnection, SteamLink> _links = new Dictionary<HSteamNetConnection, SteamLink>();
        readonly Queue<ITunnelLink> _incoming = new Queue<ITunnelLink>();
        ulong _joinRequest;
        Callback<SteamNetConnectionStatusChangedCallback_t> _cbStatus;
        Callback<GameRichPresenceJoinRequested_t> _cbJoin;
        Callback<GetTicketForWebApiResponse_t> _cbTicket;
        HAuthTicket _ticketHandle = HAuthTicket.Invalid;
        sbyte[] _ticket;

        /// <summary>Process-wide "Steamworks is initialized, as app N" (shared with the Steam release's own Steam service).</summary>
        const string ApiFlag = "TT_STEAMWORKS_APP";
        bool _ownsApi;

        // started when 1.4 / New Tides is picked (FaithfulGame.BootGui -> TTSteam.Start), not at start-up
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register() => TTSteam.Starter = Boot;

        static void Boot()
        {
            if (TTSteam.Service != null) return;
            TribalTrouble.Game.OnlineSettings.Load();
            if (!TribalTrouble.Game.OnlineSettings.steamEnabled) return;
            var s = new TTSteamService();
            if (s.Init()) { TTSteam.Service = s; TribalTrouble.Bridge.OnlineHooks.Steam = s; Application.quitting += s.Shutdown; }
        }

        bool Init()
        {
            AppId = TribalTrouble.Game.OnlineSettings.steamAppId;
            try
            {
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(ApiFlag)) || !Probe())
                {
                    // the app id comes from the options (480 = Steam's public test app) instead of a steam_appid.txt
                    Environment.SetEnvironmentVariable("SteamAppId", AppId.ToString());
                    Environment.SetEnvironmentVariable("SteamGameId", AppId.ToString());
                    if (!Packsize.Test() || !DllCheck.Test()) { LastError = "Steamworks.NET binaries mismatch"; return false; }
                    var r = SteamAPI.InitEx(out string msg);
                    if (r != ESteamAPIInitResult.k_ESteamAPIInitResult_OK && AppId != TribalTrouble.Game.OnlineSettings.TestSteamAppId && r != ESteamAPIInitResult.k_ESteamAPIInitResult_NoSteamClient)
                    {
                        // (an account without Tribal Trouble: Steam's test app still gives invites and hosting through Steam)
                        Debug.Log("[TT] Steam as app " + AppId + ": " + msg + " - trying Steam's test app " + TribalTrouble.Game.OnlineSettings.TestSteamAppId);
                        AppId = TribalTrouble.Game.OnlineSettings.TestSteamAppId;
                        Environment.SetEnvironmentVariable("SteamAppId", AppId.ToString());
                        Environment.SetEnvironmentVariable("SteamGameId", AppId.ToString());
                        r = SteamAPI.InitEx(out msg);
                    }
                    if (r != ESteamAPIInitResult.k_ESteamAPIInitResult_OK) { LastError = "Steam not available: " + msg; Debug.Log("[TT] " + LastError); return false; }
                    _ownsApi = true;
                    Environment.SetEnvironmentVariable(ApiFlag, AppId.ToString());
                }
                else
                {
                    // the Steam release started Steamworks first in this session: share it (as its app id)
                    AppId = SteamUtils.GetAppID().m_AppId;
                    Debug.Log("[TT] Steam already runs as app " + AppId + " (started by the Steam release): sharing it");
                }
            }
            catch (Exception e) { LastError = "Steam not available: " + e.Message; Debug.Log("[TT] " + LastError); return false; }
            Running = true;
            SteamId = SteamUser.GetSteamID().m_SteamID;
            PersonaName = SteamFriends.GetPersonaName();
            try { SteamNetworkingUtils.InitRelayNetworkAccess(); } catch (Exception) { }
            _cbStatus = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(OnStatus);
            _cbJoin = Callback<GameRichPresenceJoinRequested_t>.Create(j => _joinRequest = ParseConnect(j.m_rgchConnect));
            _cbTicket = Callback<GetTicketForWebApiResponse_t>.Create(OnTicket);
            // launched by Steam from a friend's "Join game": +tt_steam <id> on the command line
            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i + 1 < args.Length; i++) if (args[i] == "+tt_steam") _joinRequest = ParseConnect("+tt_steam " + args[i + 1]);
            Debug.Log($"[TT] Steam: {PersonaName} ({SteamId}), app {AppId}");
            return true;
        }

        /// <summary>Is Steamworks really up (the flag can outlive it in the editor)?</summary>
        static bool Probe()
        {
            try { return SteamUser.GetSteamID().m_SteamID != 0; }
            catch (Exception) { Environment.SetEnvironmentVariable(ApiFlag, null); return false; }
        }

        static ulong ParseConnect(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            int i = s.IndexOf("+tt_steam", StringComparison.Ordinal);
            if (i < 0) return 0;
            ulong.TryParse(s.Substring(i + 9).Trim().Split(' ')[0], out var id);
            return id;
        }

        void Shutdown()
        {
            if (!Running) return;
            foreach (var l in _links.Values) l.Close();
            _links.Clear();
            StopListening();
            Running = false;
            if (ReferenceEquals(TTSteam.Service, this)) TTSteam.Service = null;
            if (ReferenceEquals(TribalTrouble.Bridge.OnlineHooks.Steam, this)) TribalTrouble.Bridge.OnlineHooks.Steam = null;
            if (!_ownsApi) return;   // the Steam release's service shuts Steamworks down
            _ownsApi = false;
            Environment.SetEnvironmentVariable(ApiFlag, null);
            try { SteamAPI.Shutdown(); } catch (Exception) { }
        }

        public void Tick()
        {
            if (Running) SteamAPI.RunCallbacks();
        }

        public void RunCallbacks() => Tick();

        // ------------------------------------------------------------------ P2P tunnels

        public bool Listen()
        {
            if (!Running) return false;
            if (_listen != HSteamListenSocket.Invalid) return true;
            _listen = SteamNetworkingSockets.CreateListenSocketP2P(VirtualPort, 0, null);
            if (_listen == HSteamListenSocket.Invalid) { LastError = "couldn't listen for friends"; return false; }
            return true;
        }

        public void StopListening()
        {
            if (_listen == HSteamListenSocket.Invalid) return;
            SteamNetworkingSockets.CloseListenSocket(_listen);
            _listen = HSteamListenSocket.Invalid;
        }

        public ITunnelLink TakeIncoming() => _incoming.Count > 0 ? _incoming.Dequeue() : null;

        public ITunnelLink Connect(ulong hostSteamId)
        {
            if (!Running) { LastError = "Steam isn't running"; return null; }
            var id = new SteamNetworkingIdentity();
            id.SetSteamID64(hostSteamId);
            var conn = SteamNetworkingSockets.ConnectP2P(ref id, VirtualPort, 0, null);
            if (conn == HSteamNetConnection.Invalid) { LastError = "couldn't connect to " + hostSteamId; return null; }
            var link = new SteamLink(conn, "Steam friend " + hostSteamId) { connecting = true };
            _links[conn] = link;
            return link;
        }

        void OnStatus(SteamNetConnectionStatusChangedCallback_t c)
        {
            var conn = c.m_hConn;
            var state = c.m_info.m_eState;
            switch (state)
            {
                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting:
                    if (c.m_info.m_hListenSocket != HSteamListenSocket.Invalid && c.m_info.m_hListenSocket == _listen)
                    {
                        if (SteamNetworkingSockets.AcceptConnection(conn) != EResult.k_EResultOK) SteamNetworkingSockets.CloseConnection(conn, 0, "accept failed", false);
                        else _links[conn] = new SteamLink(conn, "Steam " + c.m_info.m_identityRemote.GetSteamID64()) { connecting = true, incoming = true };
                    }
                    break;
                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected:
                    if (_links.TryGetValue(conn, out var l))
                    {
                        l.connecting = false;
                        if (l.incoming) _incoming.Enqueue(l);
                    }
                    break;
                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer:
                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally:
                    if (_links.TryGetValue(conn, out var d)) { d.Close(); _links.Remove(conn); }
                    else SteamNetworkingSockets.CloseConnection(conn, 0, null, false);
                    break;
            }
        }

        /// <summary>One Steam P2P connection as a reliable message link.</summary>
        sealed class SteamLink : ITunnelLink
        {
            readonly HSteamNetConnection _conn;
            readonly IntPtr[] _msgs = new IntPtr[64];
            readonly Queue<byte[]> _in = new Queue<byte[]>();
            public bool connecting, incoming, closed;
            public string Describe { get; }
            public SteamLink(HSteamNetConnection c, string what) { _conn = c; Describe = what; }

            // still connecting counts as open (the tunnel queues until then)
            public bool IsOpen => !closed;

            public bool CanSend
            {
                get
                {
                    if (closed || connecting) return false;
                    var st = new SteamNetConnectionRealTimeStatus_t();
                    var lanes = new SteamNetConnectionRealTimeLaneStatus_t();
                    if (SteamNetworkingSockets.GetConnectionRealTimeStatus(_conn, ref st, 0, ref lanes) != EResult.k_EResultOK) return true;
                    return st.m_cbPendingReliable < 256 * 1024;
                }
            }

            readonly Queue<byte[]> _pendingOut = new Queue<byte[]>();

            public void Send(byte[] data, int length)
            {
                if (closed) return;
                var copy = new byte[length];
                Buffer.BlockCopy(data, 0, copy, 0, length);
                _pendingOut.Enqueue(copy);
                Flush();
            }

            void Flush()
            {
                if (connecting) return;
                while (_pendingOut.Count > 0)
                {
                    var d = _pendingOut.Peek();
                    var h = GCHandle.Alloc(d, GCHandleType.Pinned);
                    EResult r;
                    try { r = SteamNetworkingSockets.SendMessageToConnection(_conn, h.AddrOfPinnedObject(), (uint)d.Length, Constants.k_nSteamNetworkingSend_Reliable, out _); }
                    finally { h.Free(); }
                    if (r == EResult.k_EResultLimitExceeded) return;   // try again next pump
                    if (r != EResult.k_EResultOK && r != EResult.k_EResultIgnored) { Close(); return; }
                    _pendingOut.Dequeue();
                }
            }

            public byte[] Receive()
            {
                if (closed) return null;
                Flush();
                if (_in.Count == 0)
                {
                    int n = SteamNetworkingSockets.ReceiveMessagesOnConnection(_conn, _msgs, _msgs.Length);
                    for (int i = 0; i < n; i++)
                    {
                        var m = SteamNetworkingMessage_t.FromIntPtr(_msgs[i]);
                        var b = new byte[m.m_cbSize];
                        Marshal.Copy(m.m_pData, b, 0, m.m_cbSize);
                        _in.Enqueue(b);
                        SteamNetworkingMessage_t.Release(_msgs[i]);
                    }
                }
                return _in.Count > 0 ? _in.Dequeue() : null;
            }

            public void Close()
            {
                if (closed) return;
                closed = true;
                SteamNetworkingSockets.CloseConnection(_conn, 0, "bye", true);
            }
        }

        // ------------------------------------------------------------------ presence and invites

        public void SetPresence(string status, string connect)
        {
            if (!Running) return;
            if (status == null) { SteamFriends.ClearRichPresence(); return; }
            SteamFriends.SetRichPresence("status", "Tribal Trouble: " + status);
            SteamFriends.SetRichPresence("connect", connect ?? "");
        }

        public void InviteFriends(string connect)
        {
            if (Running && connect != null) SteamFriends.ActivateGameOverlayInviteDialogConnectString(connect);
        }

        public ulong TakeJoinRequest() { var r = _joinRequest; _joinRequest = 0; return r; }

        // ------------------------------------------------------------------ the Java SteamManager (login tickets, presence, achievements)

        long TribalTrouble.Bridge.ISteam.AccountId => (long)(SteamId & 0xFFFFFFFFUL);
        int TribalTrouble.Bridge.ISteam.AppId => (int)AppId;

        public void RequestWebApiTicket(string identity)
        {
            if (!Running) return;
            if (_ticketHandle != HAuthTicket.Invalid) SteamUser.CancelAuthTicket(_ticketHandle);
            _ticket = null;
            _ticketHandle = SteamUser.GetAuthTicketForWebApi(identity);
        }

        void OnTicket(GetTicketForWebApiResponse_t r)
        {
            if (r.m_hAuthTicket != _ticketHandle) return;
            if (r.m_eResult != EResult.k_EResultOK || r.m_cubTicket <= 0) { LastError = "no Steam login ticket (" + r.m_eResult + ")"; return; }
            var t = new sbyte[r.m_cubTicket];
            Buffer.BlockCopy(r.m_rgubTicket, 0, t, 0, r.m_cubTicket);
            _ticket = t;
        }

        /// <summary>Called from the network thread (MatchmakingClient.connected): waits, pumping Steam callbacks.</summary>
        public sbyte[] WebApiTicket(int waitMs)
        {
            var end = DateTime.UtcNow.AddMilliseconds(waitMs);
            while (_ticket == null && DateTime.UtcNow < end) { try { SteamAPI.RunCallbacks(); } catch (Exception) { } System.Threading.Thread.Sleep(20); }
            return _ticket;
        }

        public void CancelTicket()
        {
            if (_ticketHandle != HAuthTicket.Invalid) SteamUser.CancelAuthTicket(_ticketHandle);
            _ticketHandle = HAuthTicket.Invalid;
            _ticket = null;
        }

        public void SetRichPresence(string key, string value) { if (Running) SteamFriends.SetRichPresence(key, value); }
        public void ClearRichPresence() { if (Running) SteamFriends.ClearRichPresence(); }

        public void UnlockAchievement(string id)
        {
            if (!Running) return;
            if (SteamUserStats.GetAchievement(id, out bool done) && done) return;
            if (SteamUserStats.SetAchievement(id)) SteamUserStats.StoreStats();
        }
    }
}
