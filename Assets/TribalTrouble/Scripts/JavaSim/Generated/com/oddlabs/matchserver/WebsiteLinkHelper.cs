// Translated from WebsiteLinkHelper.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 7
    public class WebsiteLinkHelper : java.lang.Object
    {
        // Java: 9
        public static string getPlayerHighscoreUrl(string nick)
        {
            string domain = com.oddlabs.matchserver.WebsiteLinkHelper.getDomain();
            string encoded = java.net.URLEncoder.encode(nick, java.nio.charset.StandardCharsets.UTF_8);
            return java.lang.JString.format("https://%s/#player#%s#0", domain, encoded);
        }
        // Java: 15
        public static string getReplayUrl(int gameId)
        {
            string dirPath = com.oddlabs.matchserver.ServerConfiguration.getInstance().get(com.oddlabs.matchserver.ServerConfiguration.SPECTATOR_DATA_DIR);
            if (dirPath == null || dirPath.isEmpty())
            {
                dirPath = "/var/games";
            }
            java.io.File spectatorFile = new java.io.File(dirPath, java.lang.JString.valueOf(gameId));
            if (!spectatorFile.exists())
            {
                return null;
            }
            return java.lang.JString.format("https://%s/watch.html#%d", com.oddlabs.matchserver.WebsiteLinkHelper.getDomain(), gameId);
        }
        // Java: 23
        public static string getProfileLink(string displayText, string nick)
        {
            return java.lang.JString.format("[%s](%s)", displayText, com.oddlabs.matchserver.WebsiteLinkHelper.getPlayerHighscoreUrl(nick));
        }
        // Java: 27
        public static string getDomain()
        {
            string domain = com.oddlabs.matchserver.ServerConfiguration.getInstance().get(com.oddlabs.matchserver.ServerConfiguration.WEBSITE_DOMAIN);
            return (domain != null ? domain : "tribaltrouble.org");
        }
        public WebsiteLinkHelper() : base()
        {
        }
    }
}
