// Translated from ServerConfiguration.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 13
    public class ServerConfiguration : java.lang.Object
    {
        public const string DB_CONNECTION = "DB_CONNECTION";
        public const string DB_USER = "DB_USER";
        public const string SQL_PASS = "SQL_PASS";
        public const string DISCORD_BOT_TOKEN = "DISCORD_BOT_TOKEN";
        public const string DISCORD_SERVER_ID = "DISCORD_SERVER_ID";
        public const string WEBSITE_DOMAIN = "WEBSITE_DOMAIN";
        public const string VIKING_CHIEF_EMOJI = "VIKING_CHIEF_EMOJI";
        public const string NATIVE_CHIEF_EMOJI = "NATIVE_CHIEF_EMOJI";
        public const string STEAM_WEB_API_KEY = "STEAM_WEB_API_KEY";
        public const string STEAM_APP_ID = "STEAM_APP_ID";
        public const string STEAM_ONLY_AUTH = "STEAM_ONLY_AUTH";
        public const string SPECTATOR_DATA_DIR = "SPECTATOR_DATA_DIR";
        public const string EMOJI_ROLE_MAPPINGS = "EMOJI_ROLE_MAPPINGS";
        public const string REACTION_ROLE_MESSAGE_ID = "REACTION_ROLE_MESSAGE_ID";
        public const string TRUST_STEAM_LOGINS = "TRUST_STEAM_LOGINS";
        public static com.oddlabs.matchserver.ServerConfiguration instance;
        // Java: 33
        public static com.oddlabs.matchserver.ServerConfiguration getInstance()
        {
            if (com.oddlabs.matchserver.ServerConfiguration.instance == null)
            {
                com.oddlabs.matchserver.ServerConfiguration.instance = new com.oddlabs.matchserver.ServerConfiguration();
            }
            return com.oddlabs.matchserver.ServerConfiguration.instance;
        }
        public java.util.Map properties;
        // Java: 45
        public void set(string key, string @value)
        {
            if (@value == null)
            {
                properties.remove(key);
            }
            else
            {
                properties.put(key, @value);
            }
        }
        // Java: 52
        public string get(string key)
        {
            return ((string)properties.get(key));
        }
        // Java: 56
        public string get(string key, string defaultValue)
        {
            string v = ((string)properties.get(key));
            return (v == null ? defaultValue : v);
        }
        // Java: 61
        public int getInt(string key, int defaultValue)
        {
            try
            {
                string v = ((string)properties.get(key));
                return (v == null ? defaultValue : java.lang.Integer.parseInt(v.trim()));
            }
            catch (System.FormatException e)
            {
                return defaultValue;
            }
        }
        // Java: 70
        public java.util.Map getEmojiRoleMappings()
        {
            return new java.util.HashMap();
        }
        // Java: 74
        public bool getBoolean(string key, bool defaultValue)
        {
            string v = ((string)properties.get(key));
            return (v == null ? defaultValue : "true".equalsIgnoreCase(v.trim()));
        }
        // Java: 79
        public bool isSteamOnlyAuth()
        {
            return getBoolean(com.oddlabs.matchserver.ServerConfiguration.STEAM_ONLY_AUTH, false);
        }
        // Java: 83
        public java.util.Set getSteamAppIds()
        {
            java.util.Set ids = new java.util.HashSet();
            string raw = get(com.oddlabs.matchserver.ServerConfiguration.STEAM_APP_ID);
            if (raw == null)
            {
                return ids;
            }
            foreach (string part in raw.split(","))
            {
                try
                {
                    ids.add(java.lang.Integer.parseInt(part.trim()));
                }
                catch (System.FormatException e)
                {
                }
            }
            return ids;
        }
        // Java: 97
        public bool isWhitelistedSteamAppId(int appId)
        {
            java.util.Set ids = getSteamAppIds();
            return ids.isEmpty() || ids.contains(appId);
        }
        // Java: 102
        public int getMainSteamAppId()
        {
            java.util.Set ids = getSteamAppIds();
            return (ids.isEmpty() ? 0 : ((int)((int?)ids.iterator().next())));
        }
        // Java: 107
        public bool isSteamStatsConfigured()
        {
            return false;
        }
        private void init_ServerConfiguration()
        {
            this.properties = new java.util.HashMap();
        }
        public ServerConfiguration() : base()
        {
            init_ServerConfiguration();
            properties.put(com.oddlabs.matchserver.ServerConfiguration.TRUST_STEAM_LOGINS, "true");
        }
    }
}
