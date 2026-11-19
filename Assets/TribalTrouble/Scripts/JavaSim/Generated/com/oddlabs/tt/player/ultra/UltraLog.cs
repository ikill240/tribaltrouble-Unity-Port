// Translated from UltraLog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 14
    public class UltraLog : java.lang.Object
    {
        public static java.util.function.Consumer sink;
        public static bool strict;
        // Java: 21
        public static void setSink(java.util.function.Consumer new_sink)
        {
            com.oddlabs.tt.player.ultra.UltraLog.sink = new_sink;
        }
        // Java: 26
        public static void setStrict(bool new_strict)
        {
            com.oddlabs.tt.player.ultra.UltraLog.strict = new_strict;
        }
        // Java: 30
        public static bool isStrict()
        {
            return com.oddlabs.tt.player.ultra.UltraLog.strict;
        }
        // Java: 34
        public static bool enabled()
        {
            return com.oddlabs.tt.player.ultra.UltraLog.sink != null;
        }
        // Java: 38
        public static void log(com.oddlabs.tt.player.Player player, java.util.function.Supplier message)
        {
            java.util.function.Consumer s = com.oddlabs.tt.player.ultra.UltraLog.sink;
            if (s != null)
            {
                s.accept("[" + java.lang.JStr.valueOf(player.getPlayerInfo().getName()) + "] " + java.lang.JStr.valueOf(((string)message.get())));
            }
        }
        public UltraLog() : base()
        {
        }
    }
}
