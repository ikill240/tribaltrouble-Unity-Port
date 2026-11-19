// Translated from Main.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt
{
    // Java: 13
    public class Main : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public static java.util.ResourceBundle bundle;
        // Java: 17
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.Main.bundle, key, args);
        }
        // Java: 21
        public static void fail(System.Exception t)
        {
            com.oddlabs.tt.Main.logger.log(java.util.logging.Level.SEVERE, "Critical Failure", t);
            if (!java.lang.Boolean.getBoolean("com.oddlabs.tt.developer"))
            {
                string custom_map_problem = null;
                for (System.Exception c = t; c != null && custom_map_problem == null; c = c.getCause())
                {
                    if ((c is System.IO.IOException) && c.getMessage() != null && c.getMessage().startsWith(com.oddlabs.tt.resource.CustomMapGenerator.LOAD_FAILURE_PREFIX))
                    {
                        custom_map_problem = c.getMessage().substring(com.oddlabs.tt.resource.CustomMapGenerator.LOAD_FAILURE_PREFIX.length());
                    }
                }
                while (t.getCause() != null)
                {
                    t = t.getCause();
                }
                string error = com.oddlabs.tt.Main.i18n("error");
                string error_msg = default;
                if (custom_map_problem != null)
                {
                    error_msg = com.oddlabs.tt.Main.i18n("error_custom_map", custom_map_problem);
                }
                else if ((t is System.Exception))
                {
                    error_msg = com.oddlabs.tt.Main.i18n("error_out_of_memory");
                }
                else
                {
                    try
                    {
                        error_msg = com.oddlabs.tt.Main.i18n("error_message", java.lang.JO.toString(t));
                    }
                    catch (System.ArgumentException e)
                    {
                        error_msg = "Error: " + java.lang.JStr.valueOf(t);
                    }
                }
                com.oddlabs.tt.Main.logger.log(java.util.logging.Level.SEVERE, java.lang.JStr.valueOf(error) + ": " + java.lang.JStr.valueOf(error_msg));
                org.lwjgl.util.tinyfd.TinyFileDialogs.tinyfd_messageBox(error, error_msg.replace("\"", "\\\""), "ok", "error", 1);
            }
        }
        // Java: 63
        public static void shutdown(int status)
        {
            com.oddlabs.tt.steam.SteamManager.shutdown();
            com.oddlabs.tt.render.Renderer.getRenderer().close();
            com.oddlabs.tt.Main.logger.info("Exiting");
            java.lang.JSystem.exit(status);
        }
        // Java: 70
        public static void main(params string[] args)
        {
            throw new System.NotSupportedException("J2CS stub: Main.main");
        }
        static Main()
        {
            Main.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.Main).getName());
            Main.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.Main).getName());
        }
        public Main() : base()
        {
        }
    }
}
