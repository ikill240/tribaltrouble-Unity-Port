// Translated from SteamAuthValidator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 8
    public class SteamAuthValidator : java.lang.Object
    {
        public const long STEAM_ID_BASE = 76561197960265728L;
        // Java: 11
        public static bool validateTicket(long steamAccountId, sbyte[] authTicket, int appId)
        {
            com.oddlabs.matchserver.ServerConfiguration config = com.oddlabs.matchserver.ServerConfiguration.getInstance();
            if (authTicket == null || authTicket.Length == 0)
            {
                return false;
            }
            if (!config.isWhitelistedSteamAppId(appId))
            {
                return false;
            }
            return config.getBoolean(com.oddlabs.matchserver.ServerConfiguration.TRUST_STEAM_LOGINS, true);
        }
        public SteamAuthValidator() : base()
        {
        }
    }
}
