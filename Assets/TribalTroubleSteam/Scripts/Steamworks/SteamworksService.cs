using System;
using Steamworks;
using UnityEngine;

namespace TribalTroubleSteam.Online
{
    /// <summary>
    /// Steam for the Steam release (v2.2.207), as the Java game does it with steamworks4j: Steamworks.NET under the
    /// Java SteamManager (TribalTroubleSteam.Bridge.ISteam). App 3945720; the matchmaking login is a Web API ticket
    /// for "tribaltrouble.org" that the official server checks; rich presence ("steam_display" tokens) and the
    /// achievements. Started when the Steam game starts (SteamGame.Awake -> OnlineHooks.StartSteam). Without the Steam
    /// client (or without the game in the Steam library) Steam is just off, and Multiplayer shows the classic login.
    ///
    /// One process, one Steam app: the 1.4 / New Tides game starts its own Steam service with its own app id when it
    /// is picked. Whichever starts first owns Steamworks for the session (the other reuses it); the official server
    /// only takes this game's app id, so when the other game got there first the Steam login stays off until a restart.
    /// </summary>
    public sealed class SteamworksService : Bridge.ISteam
    {
        public const uint TribalTroubleAppId = 3945720;
        /// <summary>Process-wide "Steamworks is initialized, as app N" (shared with the other game's Steam service).</summary>
        const string ApiFlag = "TT_STEAMWORKS_APP";

        static SteamworksService s_instance;
        static readonly object s_pump = new object();

        bool _ownsApi;
        ulong _steamId;
        uint _appId;
        string _persona = "";
        Callback<GetTicketForWebApiResponse_t> _cbTicket;
        HAuthTicket _ticketHandle = HAuthTicket.Invalid;
        volatile sbyte[] _ticket;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Register() => Bridge.OnlineHooks.StartSteam = Start;

        /// <summary>Starts Steam once (again: the same service).</summary>
        public static void Start()
        {
            if (s_instance != null) { Bridge.OnlineHooks.Steam = s_instance; return; }
            var s = new SteamworksService();
            if (!s.Init()) return;
            s_instance = s;
            Bridge.OnlineHooks.Steam = s;
            Application.quitting += s.Shutdown;
        }

        bool Init()
        {
            try
            {
                string running = Environment.GetEnvironmentVariable(ApiFlag);
                if (string.IsNullOrEmpty(running) || !Probe())
                {
                    if (!Packsize.Test() || !DllCheck.Test()) { Debug.LogWarning("[TT Steam] Steam is off: the Steamworks.NET binaries don't match"); return false; }
                    // the app id without a steam_appid.txt (the project's one is the other game's)
                    Environment.SetEnvironmentVariable("SteamAppId", TribalTroubleAppId.ToString());
                    Environment.SetEnvironmentVariable("SteamGameId", TribalTroubleAppId.ToString());
                    var r = SteamAPI.InitEx(out string msg);
                    if (r != ESteamAPIInitResult.k_ESteamAPIInitResult_OK)
                    {
                        Debug.Log("[TT Steam] Steam is off (" + r + ": " + msg + "): Multiplayer uses the classic login");
                        return false;
                    }
                    _ownsApi = true;
                }
                _appId = SteamUtils.GetAppID().m_AppId;
                if (_ownsApi) Environment.SetEnvironmentVariable(ApiFlag, _appId.ToString());
                if (_appId != TribalTroubleAppId)
                {
                    Debug.LogWarning("[TT Steam] Steam already runs as app " + _appId + " (the other game started it): the official server takes only "
                        + TribalTroubleAppId + ", so the Steam login is off until the next start");
                    if (_ownsApi) Shutdown();
                    return false;
                }
                _steamId = SteamUser.GetSteamID().m_SteamID;
                _persona = SteamFriends.GetPersonaName() ?? "";
                _cbTicket = Callback<GetTicketForWebApiResponse_t>.Create(OnTicket);
                Debug.Log("[TT Steam] Steam: " + _persona + " (" + _steamId + "), app " + _appId);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log("[TT Steam] Steam is off: " + e.Message);
                return false;
            }
        }

        /// <summary>Is Steamworks really up (the flag can outlive it in the editor)?</summary>
        static bool Probe()
        {
            try { return SteamUser.GetSteamID().m_SteamID != 0; }
            catch (Exception) { Environment.SetEnvironmentVariable(ApiFlag, null); return false; }
        }

        void Shutdown()
        {
            Application.quitting -= Shutdown;
            CancelTicket();
            _cbTicket?.Dispose();
            _cbTicket = null;
            if (ReferenceEquals(Bridge.OnlineHooks.Steam, this)) Bridge.OnlineHooks.Steam = null;
            if (ReferenceEquals(s_instance, this)) s_instance = null;
            if (!_ownsApi) return;
            _ownsApi = false;
            Environment.SetEnvironmentVariable(ApiFlag, null);
            try { SteamAPI.Shutdown(); } catch (Exception) { }
        }

        // ------------------------------------------------------------------ Bridge.ISteam (the Java SteamManager)

        // steamworks4j: SteamID.getAccountID() is an int (the low 32 bits), widened to a long by SteamManager
        public long AccountId => unchecked((int)(uint)(_steamId & 0xFFFFFFFFUL));
        public string PersonaName => _persona;
        public int AppId => unchecked((int)_appId);

        /// <summary>Main thread and the game's thread both pump (the login waits on the game's thread).</summary>
        public void RunCallbacks()
        {
            lock (s_pump) SteamAPI.RunCallbacks();
        }

        public void RequestWebApiTicket(string identity)
        {
            if (_ticketHandle != HAuthTicket.Invalid) SteamUser.CancelAuthTicket(_ticketHandle);
            _ticket = null;
            _ticketHandle = SteamUser.GetAuthTicketForWebApi(identity);
        }

        void OnTicket(GetTicketForWebApiResponse_t r)
        {
            if (r.m_hAuthTicket != _ticketHandle) return;
            if (r.m_eResult != EResult.k_EResultOK || r.m_cubTicket <= 0)
            {
                Debug.LogWarning("[TT Steam] No Steam login ticket: " + r.m_eResult);
                return;
            }
            var t = new sbyte[r.m_cubTicket];
            Buffer.BlockCopy(r.m_rgubTicket, 0, t, 0, r.m_cubTicket);
            _ticket = t;
        }

        public sbyte[] WebApiTicket(int waitMs)
        {
            var end = DateTime.UtcNow.AddMilliseconds(waitMs);
            while (_ticket == null && DateTime.UtcNow < end)
            {
                try { RunCallbacks(); } catch (Exception) { }
                System.Threading.Thread.Sleep(50);
            }
            return _ticket;
        }

        public void CancelTicket()
        {
            if (_ticketHandle != HAuthTicket.Invalid) { try { SteamUser.CancelAuthTicket(_ticketHandle); } catch (Exception) { } }
            _ticketHandle = HAuthTicket.Invalid;
            _ticket = null;
        }

        public void SetRichPresence(string key, string value) => SteamFriends.SetRichPresence(key, value ?? "");
        public void ClearRichPresence() => SteamFriends.ClearRichPresence();

        public void UnlockAchievement(string id)
        {
            if (SteamUserStats.GetAchievement(id, out bool done) && done) return;
            if (SteamUserStats.SetAchievement(id)) { SteamUserStats.StoreStats(); Debug.Log("[TT Steam] Achievement unlocked: " + id); }
            else Debug.LogWarning("[TT Steam] Failed to unlock achievement: " + id);
        }
    }
}
