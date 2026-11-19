// Translated from AiDevOptions.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 34
    public class AiDevOptions : java.lang.Object
    {
        public const string PROP_DEBUG = "com.oddlabs.tt.ai.debug";
        public const string PROP_HARD_IMPL = "com.oddlabs.tt.ai.hardimpl";
        public const string PROP_LOCAL_AI = "com.oddlabs.tt.ai.local";
        public const string PROP_AUTO_SKIRMISH = "com.oddlabs.tt.ai.autoskirmish";
        public static bool DEBUG;
        // Java: 45
        public static bool isDebug()
        {
            return com.oddlabs.tt.player.fable.AiDevOptions.DEBUG;
        }
        // Java: 50
        public static bool useLegacyHard()
        {
            return "legacy".equalsIgnoreCase(java.lang.JSystem.getProperty(com.oddlabs.tt.player.fable.AiDevOptions.PROP_HARD_IMPL, "new"));
        }
        // Java: 55
        public static string getLocalAI()
        {
            string local = java.lang.JSystem.getProperty(com.oddlabs.tt.player.fable.AiDevOptions.PROP_LOCAL_AI);
            if (local == null || local.isBlank())
            {
                local = ((string)com.oddlabs.tt.player.fable.AiDevOptions.getAutoSkirmish().get("local"));
            }
            if (local == null || local.isBlank() || "none".equalsIgnoreCase(local))
            {
                return null;
            }
            return local.toLowerCase(java.util.Locale.ROOT);
        }
        // Java: 65
        public static bool isAutoSkirmish()
        {
            string spec = java.lang.JSystem.getProperty(com.oddlabs.tt.player.fable.AiDevOptions.PROP_AUTO_SKIRMISH);
            return spec != null && !spec.isBlank();
        }
        // Java: 71
        public static java.util.Map getAutoSkirmish()
        {
            java.util.Map result = new java.util.LinkedHashMap();
            string spec = java.lang.JSystem.getProperty(com.oddlabs.tt.player.fable.AiDevOptions.PROP_AUTO_SKIRMISH);
            if (spec == null)
            {
                return result;
            }
            foreach (string entry in spec.split(","))
            {
                int eq = entry.indexOf('=');
                if (eq <= 0)
                {
                    continue;
                }
                result.put(entry.substring(0, eq).trim().toLowerCase(java.util.Locale.ROOT), entry.substring(eq + 1).trim());
            }
            return result;
        }
        // Java: 86
        public static float getTimeLimit()
        {
            string limit = ((string)com.oddlabs.tt.player.fable.AiDevOptions.getAutoSkirmish().get("limit"));
            if (limit == null)
            {
                return (float)(-1.0f);
            }
            try
            {
                return java.lang.Float.parseFloat(limit);
            }
            catch (System.FormatException unused)
            {
                return (float)(-1.0f);
            }
        }
        // Java: 97
        public static int getInt(java.util.Map spec, string key, int def)
        {
            string @value = ((string)spec.get(key));
            if (@value == null)
            {
                return def;
            }
            try
            {
                return java.lang.Integer.parseInt(@value.trim());
            }
            catch (System.FormatException unused)
            {
                return def;
            }
        }
        static AiDevOptions()
        {
            AiDevOptions.DEBUG = java.lang.Boolean.getBoolean(com.oddlabs.tt.player.fable.AiDevOptions.PROP_DEBUG);
        }
        public AiDevOptions() : base()
        {
        }
    }
}
