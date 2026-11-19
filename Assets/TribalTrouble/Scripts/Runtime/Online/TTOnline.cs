using System;
using System.Collections.Generic;
using TribalTrouble.Faithful;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>Steam (implemented in TribalTrouble.Steam with Steamworks.NET; registers itself at start-up).</summary>
    public interface ISteamService
    {
        bool Running { get; }
        string PersonaName { get; }
        ulong SteamId { get; }
        uint AppId { get; }
        string LastError { get; }
        void Tick();
        /// <summary>Start accepting friends' tunnel connections (Steam Datagram Relay, no open ports needed).</summary>
        bool Listen();
        void StopListening();
        /// <summary>A friend who just connected to our hosted server, or null.</summary>
        ITunnelLink TakeIncoming();
        ITunnelLink Connect(ulong hostSteamId);
        /// <summary>Rich presence: friends see the game and get a "Join game" button while we host.</summary>
        void SetPresence(string status, string connect);
        void InviteFriends(string connect);
        /// <summary>A "+tt_steam &lt;id&gt;" join (invite accepted, Join game clicked, or on the command line), or 0.</summary>
        ulong TakeJoinRequest();
    }

    /// <summary>Unity Relay (TribalTrouble.Relay, needs the Multiplayer Services package and a linked Unity project).</summary>
    public interface IRelayService
    {
        string Status { get; }
        void StartHost(int maxPeers, Action<string> onJoinCode, Action<string> onError);
        void StopHost();
        ITunnelLink TakeIncoming();
        void Join(string joinCode, Action<ITunnelLink> onLink, Action<string> onError);
        void Tick();
    }

    public static class TTSteam
    {
        public static ISteamService Service;
        /// <summary>Set by TribalTrouble.Steam at load; run when this game is picked (not at start-up: the Steam
        /// release in the same project needs Steam as its own app id, and one process gets one).</summary>
        public static Action Starter;
        public static void Start() { if (Service == null) { try { Starter?.Invoke(); } catch (Exception e) { Debug.LogException(e); } } }
        public static bool Running => Service != null && Service.Running;
        public static string PersonaName => Service?.PersonaName ?? "";
    }

    public static class TTRelay
    {
        public static IRelayService Service;
        public static bool Available => Service != null;
        /// <summary>Why Unity Relay is off (shown on the Friends page), or null.</summary>
        public static string UnavailableReason;
    }

    /// <summary>
    /// Hosting and joining friends' games without the official server: the PlayerServer (matchmaking + router) on
    /// the host, reached directly (IP / LAN), through Steam, or through Unity Relay; invites through Steam and
    /// Discord. Lives on a DontDestroyOnLoad object; pumps the tunnels every frame.
    /// </summary>
    public sealed class TTOnline : MonoBehaviour
    {
        public static TTOnline I { get; private set; }

        readonly List<TunnelMux> _hostLinks = new List<TunnelMux>();
        TunnelClient _client;
        public string clientVia { get; private set; }
        public string relayCode { get; private set; }
        public string status;
        public string error;
        string _savedDomain;
        float _presenceAt;
        /// <summary>A join to perform once logged in (from an invite): auto-join the host's game.</summary>
        public bool autoJoinGame;
        /// <summary>Set when an invite / join request arrived; the menu opens the multiplayer screen.</summary>
        public event Action JoinStarted;

        public static TTOnline Ensure()
        {
            if (I != null) return I;
            var go = new GameObject("TT Online");
            DontDestroyOnLoad(go);
            I = go.AddComponent<TTOnline>();
            return I;
        }

        public bool Hosting => PlayerServer.Running || PlayerServer.Starting;
        public bool Joined => _client != null;
        public int Peers { get { int n = 0; foreach (var m in _hostLinks) if (m.link.IsOpen) n++; return n; } }

        // ------------------------------------------------------------------ hosting

        public void HostServer(bool useSteam, bool useRelay)
        {
            error = null;
            LeaveTunnel();
            PlayerServer.Start();
            UseLocalServer();
            if (useSteam && TTSteam.Running && !TTSteam.Service.Listen()) error = "Steam: " + TTSteam.Service.LastError;
            if (useRelay && TTRelay.Available && relayCode == null)
                TTRelay.Service.StartHost(15, c => relayCode = c, e => error = "Unity Relay: " + e);
            _presenceAt = 0;
        }

        public void StopServer()
        {
            foreach (var m in _hostLinks) { m.CloseAll(); m.link.Close(); }
            _hostLinks.Clear();
            if (TTSteam.Running) { TTSteam.Service.StopListening(); TTSteam.Service.SetPresence(null, null); }
            if (TTRelay.Available) TTRelay.Service.StopHost();
            relayCode = null;
            PlayerServer.Stop();
            RestoreDomain();
            TTDiscord.I?.SetActivity("In the menus", null, null, 0, 0);
        }

        public void InviteSteamFriends()
        {
            if (!TTSteam.Running) return;
            TTSteam.Service.InviteFriends(SteamConnect);
        }

        string SteamConnect => TTSteam.Running ? "+tt_steam " + TTSteam.Service.SteamId : null;

        /// <summary>The Discord join secret: how a friend reaches this server.</summary>
        string JoinSecret
        {
            get
            {
                if (relayCode != null) return "tt1|relay:" + relayCode;
                if (TTSteam.Running && PlayerServer.Running) return "tt1|steam:" + TTSteam.Service.SteamId;
                var lan = PlayerServer.LanAddresses();
                return lan.Count > 0 ? "tt1|ip:" + lan[0] : null;
            }
        }

        // ------------------------------------------------------------------ joining

        public void JoinDirect(string address)
        {
            error = null;
            LeaveTunnel();
            Game.OnlineSettings.Load();
            _savedDomain ??= Game.OnlineSettings.domain;
            try { com.oddlabs.tt.global.Settings.getSettings().setDomain(address.Trim()); } catch (Exception) { }
            clientVia = "address " + address.Trim();
            autoJoinGame = true;
            JoinStarted?.Invoke();
        }

        public void JoinSteam(ulong hostId)
        {
            error = null;
            if (!TTSteam.Running) { error = "Steam isn't running."; return; }
            if (PlayerServer.Running) StopServer();
            LeaveTunnel();
            var link = TTSteam.Service.Connect(hostId);
            if (link == null) { error = "Steam: " + TTSteam.Service.LastError; return; }
            StartTunnel(link, "Steam");
        }

        public void JoinRelay(string code)
        {
            error = null;
            if (!TTRelay.Available) { error = "Unity Relay isn't set up in this build (see Options)."; return; }
            if (PlayerServer.Running) StopServer();
            LeaveTunnel();
            status = "Joining " + code + " through Unity Relay...";
            TTRelay.Service.Join(code.Trim().ToUpperInvariant(), link => { status = null; StartTunnel(link, "Unity Relay"); }, e => { status = null; error = "Unity Relay: " + e; });
        }

        /// <summary>A Discord join secret ("tt1|steam:..", "tt1|relay:..", "tt1|ip:..").</summary>
        public void JoinSecret_(string secret)
        {
            if (string.IsNullOrEmpty(secret) || !secret.StartsWith("tt1|")) return;
            string s = secret.Substring(4);
            if (s.StartsWith("steam:") && ulong.TryParse(s.Substring(6), out var id)) JoinSteam(id);
            else if (s.StartsWith("relay:")) JoinRelay(s.Substring(6));
            else if (s.StartsWith("ip:")) JoinDirect(s.Substring(3));
        }

        void StartTunnel(ITunnelLink link, string via)
        {
            _client = new TunnelClient(link);
            if (_client.Error != null) { error = _client.Error; _client.Close(); _client = null; return; }
            clientVia = via;
            UseLocalServer();
            autoJoinGame = true;
            JoinStarted?.Invoke();
        }

        public void LeaveTunnel()
        {
            if (_client != null) { _client.Close(); _client = null; }
            clientVia = null;
            autoJoinGame = false;
            if (!PlayerServer.Running) RestoreDomain();
        }

        void UseLocalServer()
        {
            Game.OnlineSettings.Load();
            _savedDomain ??= Game.OnlineSettings.domain;
            try { com.oddlabs.tt.global.Settings.getSettings().setDomain("localhost"); } catch (Exception) { }
        }

        void RestoreDomain()
        {
            if (_savedDomain == null) return;
            try { com.oddlabs.tt.global.Settings.getSettings().setDomain(_savedDomain); } catch (Exception) { }
            _savedDomain = null;
        }

        /// <summary>The server the multiplayer screen logs in to right now.</summary>
        public static string CurrentServer
        {
            get { try { return com.oddlabs.tt.global.Settings.getSettings().getMatchmakingAddress(); } catch (Exception) { return "?"; } }
        }

        // ------------------------------------------------------------------ frame

        void Update()
        {
            try { TTSteam.Service?.Tick(); } catch (Exception e) { Debug.LogException(e); }
            try { TTRelay.Service?.Tick(); } catch (Exception e) { Debug.LogException(e); }
            // friends arriving
            if (PlayerServer.Running)
            {
                ITunnelLink l;
                while (TTSteam.Running && (l = TTSteam.Service.TakeIncoming()) != null) { _hostLinks.Add(new TunnelMux(l, true)); Debug.Log("[TT] Steam friend connected: " + l.Describe); }
                while (TTRelay.Available && (l = TTRelay.Service.TakeIncoming()) != null) { _hostLinks.Add(new TunnelMux(l, true)); Debug.Log("[TT] Relay player connected: " + l.Describe); }
            }
            for (int i = _hostLinks.Count - 1; i >= 0; i--)
            {
                var m = _hostLinks[i];
                try { m.Pump(); } catch (Exception e) { Debug.LogException(e); }
                if (!m.link.IsOpen) { m.CloseAll(); _hostLinks.RemoveAt(i); }
            }
            if (_client != null)
            {
                try { _client.Pump(); } catch (Exception e) { Debug.LogException(e); }
                if (!_client.IsOpen) { error = "The connection to the host (" + clientVia + ") was closed."; LeaveTunnel(); }
            }
            // invites / join buttons
            if (TTSteam.Running)
            {
                ulong id = TTSteam.Service.TakeJoinRequest();
                if (id != 0 && id != TTSteam.Service.SteamId) JoinSteam(id);
            }
            string secret = TTDiscord.I?.TakeJoinSecret();
            if (secret != null) JoinSecret_(secret);
            // presence
            if (Time.unscaledTime > _presenceAt)
            {
                _presenceAt = Time.unscaledTime + 10f;
                UpdatePresence();
            }
        }

        void UpdatePresence()
        {
            bool host = PlayerServer.Running;
            string st = host ? "Hosting a game" : Joined ? "Playing with friends" : "In the menus";
            if (TTSteam.Running) TTSteam.Service.SetPresence(st, host ? SteamConnect : null);
            if (TTDiscord.I != null)
            {
                string party = host ? (TTSteam.Running ? "s" + TTSteam.Service.SteamId : relayCode ?? SystemInfo.deviceUniqueIdentifier) : null;
                TTDiscord.I.SetActivity(st, host ? JoinSecret : null, party, host ? Math.Max(1, PlayerServer.Online) : 0, host ? 16 : 0);
            }
        }

        void OnDestroy()
        {
            if (I == this) I = null;
        }

        void OnApplicationQuit()
        {
            try { PlayerServer.Stop(); } catch (Exception) { }
            try { _client?.Close(); } catch (Exception) { }
        }
    }
}
