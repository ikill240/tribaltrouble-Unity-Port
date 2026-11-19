// Hand-written replacement of the generated stub: the Java SteamManager on top of the Unity host's Steamworks.NET
// (TribalTrouble.Bridge.OnlineHooks.Steam). getInstance() is null when Steam isn't running, as in Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.steam
{
    public class SteamManager : java.lang.Object
    {
        const string WEB_API_IDENTITY = "tribaltrouble.org";
        static bool inActiveWorld;
        static TribalTrouble.Bridge.ISteam S => TribalTrouble.Bridge.OnlineHooks.Steam;
        sbyte[] ticket;

        public static void runCallbacks() { try { S?.RunCallbacks(); } catch (System.Exception) { } }
        static SteamManager instance;
        public static com.oddlabs.tt.steam.SteamManager getInstance() { if (S == null) return null; return instance ??= new SteamManager(); }
        public virtual long getAccountID() { return S?.AccountId ?? 0; }
        public static void setCampaignRichPresence(string race)
        {
            if (S == null) return;
            S.ClearRichPresence();
            S.SetRichPresence("steam_display", "#Status_Campaign" + race);
            S.SetRichPresence("race", race);
        }
        public static void setInActiveWorld(bool active) { inActiveWorld = active; }
        public static bool isInActiveWorld() => inActiveWorld;
        public static void unlockAchievement(string id) { S?.UnlockAchievement(id); }
        public static void shutdown() { }
        public static void setLobbyRichPresence(string room) { S?.SetRichPresence("steam_display", "#Status_InLobby"); }
        public static void clearRichPresence() { S?.ClearRichPresence(); }
        public virtual void requestWebApiTicket() { ticket = null; S?.RequestWebApiTicket(WEB_API_IDENTITY); }
        public virtual bool awaitWebApiTicket(int waitMs) { ticket = S?.WebApiTicket(waitMs); return ticket != null; }
        public virtual string getPersonaName() { return S?.PersonaName ?? ""; }
        public virtual sbyte[] getWebApiTicket() { return ticket; }
        public virtual int getAppID() { return S?.AppId ?? 0; }
        public virtual void cancelAuthTicket() { ticket = null; S?.CancelTicket(); }
        public static void setMultiplayerInGameRichPresence()
        {
            if (S == null) return;
            S.ClearRichPresence();
            S.SetRichPresence("steam_display", "#Status_InGame");
        }
        public SteamManager() { }
    }
}
