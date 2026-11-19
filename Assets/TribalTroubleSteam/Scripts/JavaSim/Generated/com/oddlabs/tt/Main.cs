// Translated from Main.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt
{
    // Java: 18
    public class Main : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public static java.util.ResourceBundle bundle;
        // Java: 22
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.Main.bundle, key, args);
        }
        // Java: 27
        public static void fail(System.Exception t)
        {
            com.oddlabs.tt.Main.fail(t, false);
        }
        // Java: 35
        public static void failFatal(System.Exception t)
        {
            com.oddlabs.tt.Main.fail(t, true);
        }
        // Java: 39
        public static void fail(System.Exception t, bool fatal)
        {
            com.oddlabs.tt.Main.logger.log(java.util.logging.Level.SEVERE, "Critical Failure", t);
            if (!java.lang.Boolean.getBoolean("com.oddlabs.tt.developer"))
            {
                System.Exception root = t;
                while (root.getCause() != null)
                {
                    root = root.getCause();
                }
                string error = com.oddlabs.tt.Main.i18n("error");
                string error_msg = default;
                try
                {
                    error_msg = com.oddlabs.tt.Main.i18n("error_message", java.lang.JO.toString(root));
                }
                catch (System.ArgumentException e)
                {
                    error_msg = "Error: " + java.lang.JStr.valueOf(root);
                }
                com.oddlabs.tt.Main.logger.log(java.util.logging.Level.SEVERE, java.lang.JStr.valueOf(error) + ": " + java.lang.JStr.valueOf(error_msg));
                if (fatal && !com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic().isPlayback())
                {
                    if (com.oddlabs.tt.Main.messageBox(error, java.lang.JStr.valueOf(error_msg) + "\n\n" + java.lang.JStr.valueOf(com.oddlabs.tt.Main.i18n("send_report")), "yesno", "error") == 1)
                    {
                        com.oddlabs.tt.Main.sendReport(t);
                    }
                }
                else
                {
                    com.oddlabs.tt.Main.messageBox(error, error_msg, "ok", "error");
                }
            }
        }
        // Java: 66
        public static void sendReport(System.Exception t)
        {
            com.oddlabs.tt.@event.LocalEventQueue queue = com.oddlabs.tt.@event.LocalEventQueue.getQueue();
            java.nio.file.Path event_log = queue.getEventLogFile();
            try
            {
                queue.close();
            }
            catch (System.Exception e)
            {
                com.oddlabs.tt.Main.logger.log(java.util.logging.Level.WARNING, "Could not end the event log", e);
            }
            if (event_log != null && !java.nio.file.Files.isRegularFile(event_log))
            {
                event_log = null;
            }
            try
            {
                string id = com.oddlabs.tt.bugreport.BugReporter.submit(t, event_log);
                com.oddlabs.tt.Main.messageBox(com.oddlabs.tt.Main.i18n("report_sent_title"), com.oddlabs.tt.Main.i18n("report_sent", id), "ok", "info");
            }
            catch (System.Exception e_2) when (e_2 is System.IO.IOException || e_2 is System.Exception)
            {
                com.oddlabs.tt.Main.logger.log(java.util.logging.Level.WARNING, "Could not send bug report", e_2);
                com.oddlabs.tt.Main.messageBox(com.oddlabs.tt.Main.i18n("error"), com.oddlabs.tt.Main.i18n("report_failed"), "ok", "warning");
            }
            catch (System.Exception e_3)
            {
                java.lang.Thread.currentThread().interrupt();
            }
        }
        // Java: 89
        public static int messageBox(string title, string message, string dialog_type, string icon_type)
        {
            return org.lwjgl.util.tinyfd.TinyFileDialogs.tinyfd_messageBox(com.oddlabs.tt.Main.withoutQuotes(title), com.oddlabs.tt.Main.withoutQuotes(message), dialog_type, icon_type, 1);
        }
        // Java: 99
        public static string withoutQuotes(string text)
        {
            java.lang.StringBuilder result = new java.lang.StringBuilder(text.length());
            bool open = true;
            for (int i = 0; i < text.length(); i++)
            {
                char c = text.charAt(i);
                if (c == '"')
                {
                    result.append((open ? '\u201c' : '\u201d'));
                    open = !open;
                }
                else if (c == '\'')
                {
                    result.append('\u2019');
                }
                else
                {
                    result.append(c);
                }
            }
            return java.lang.JO.toString(result);
        }
        // Java: 116
        public static void shutdown(int status)
        {
            com.oddlabs.tt.steam.SteamManager.shutdown();
            com.oddlabs.tt.render.Renderer.getRenderer().close();
            com.oddlabs.tt.Main.logger.info("Exiting");
            java.lang.JSystem.exit(status);
        }
        // Java: 123
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
