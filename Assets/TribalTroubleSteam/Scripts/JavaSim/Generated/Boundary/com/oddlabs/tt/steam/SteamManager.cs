// Hand-written replacement of the generated stub: the Java SteamManager on top of the Unity host's Steamworks.NET
// (TribalTroubleSteam.Bridge.OnlineHooks.Steam). getInstance() is null when Steam isn't running, as in Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.steam
{
    public class SteamManager : java.lang.Object
    {
        const string WEB_API_IDENTITY = "tribaltrouble.org";
        static readonly System.TimeSpan TICKET_MAX_AGE = System.TimeSpan.FromHours(1);
        static volatile bool inActiveWorld;
        static TribalTroubleSteam.Bridge.ISteam S => TribalTroubleSteam.Bridge.OnlineHooks.Steam;
        static SteamManager instance;

        // the Web API login ticket: requested when the matchmaking connection starts, reused for an hour (as in Java)
        sbyte[] cachedTicketData;
        bool ticketReady, ticketPending;
        System.DateTime ticketTimestamp;

        public static com.oddlabs.tt.steam.SteamManager getInstance()
        {
            if (S == null) { instance = null; return null; }
            return instance ??= new SteamManager();
        }

        public static bool isAvailable()
        {
            return com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic().log(S != null);
        }

        public static void runCallbacks() { try { S?.RunCallbacks(); } catch (System.Exception) { } }
        public static void shutdown() { }

        public static void setLobbyRichPresence(string room) { S?.SetRichPresence("steam_display", "#Status_InLobby"); }
        public static void setMultiplayerInGameRichPresence()
        {
            if (S == null) return;
            S.ClearRichPresence();
            S.SetRichPresence("steam_display", "#Status_InGame");
        }
        public static void clearRichPresence() { S?.ClearRichPresence(); }
        public static void setInActiveWorld(bool active) { inActiveWorld = active; }
        public static bool isInActiveWorld() => inActiveWorld;
        public static void setCampaignRichPresence(string race)
        {
            if (S == null) return;
            S.ClearRichPresence();
            S.SetRichPresence("steam_display", "#Status_Campaign" + race);
            S.SetRichPresence("race", race);
        }
        public static void unlockAchievement(string id) { S?.UnlockAchievement(id); }

        public virtual long getAccountID() { return S?.AccountId ?? 0; }
        public virtual int getAppID() { return S?.AppId ?? 0; }
        public virtual string getPersonaName() { return S?.PersonaName ?? ""; }

        public virtual void requestWebApiTicket()
        {
            if (S == null) return;
            if (ticketReady && System.DateTime.UtcNow - ticketTimestamp < TICKET_MAX_AGE) return;   // still fresh: reuse
            if (ticketReady) cancelAuthTicket();                                                     // expired
            if (ticketPending) return;                                                               // already asked
            ticketReady = false;
            cachedTicketData = null;
            ticketPending = true;
            S.RequestWebApiTicket(WEB_API_IDENTITY);
        }

        /// <summary>Waits up to maxWaitMs for the ticket, pumping Steam callbacks.</summary>
        public virtual bool awaitWebApiTicket(int maxWaitMs)
        {
            if (!ticketReady && S != null)
            {
                var t = S.WebApiTicket(maxWaitMs);
                if (t != null && t.Length > 0)
                {
                    cachedTicketData = (sbyte[])t.Clone();
                    ticketReady = true;
                    ticketPending = false;
                    ticketTimestamp = System.DateTime.UtcNow;
                }
                else ticketPending = false;   // failed or timed out: the next login asks for a new one (Java kept waiting on it)
            }
            return ticketReady;
        }

        public virtual sbyte[] getWebApiTicket() { return ticketReady ? cachedTicketData : null; }
        public virtual bool isWebApiTicketReady() { return ticketReady; }

        public virtual void cancelAuthTicket()
        {
            if (ticketPending || ticketReady) S?.CancelTicket();
            ticketPending = false;
            cachedTicketData = null;
            ticketReady = false;
        }

        public SteamManager() { }
    }
}
