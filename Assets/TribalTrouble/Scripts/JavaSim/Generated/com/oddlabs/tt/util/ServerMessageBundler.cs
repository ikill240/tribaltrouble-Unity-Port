// Translated from ServerMessageBundler.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 9
    public class ServerMessageBundler : java.lang.Object
    {
        public static java.util.ResourceBundle bundle;
        // Java: 12
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.util.ServerMessageBundler.bundle, key, args);
        }
        // Java: 16
        public static string getSizeString(int index)
        {
            return com.oddlabs.tt.util.ServerMessageBundler.i18n(((index) switch { 0 => (string)("size_small"), 1 => (string)("size_medium"), 2 => (string)("size_large"), 3 => (string)("size_enormous"), 4 => (string)("size_unreal"), 5 => (string)("size_archipelago"), _ => throw new System.ArgumentException("unexpected size: " + (index)) }));
        }
        // Java: 28
        public static string getTerrainTypeString(int index)
        {
            if (index == com.oddlabs.matchmaking.Game.TERRAIN_TYPE_ELEMENTAL)
            {
                return "Elemental";
            }
            return com.oddlabs.tt.util.ServerMessageBundler.i18n(((index) switch { 0 => (string)("terrain_type_native"), 1 => (string)("terrain_type_viking"), _ => throw new System.ArgumentException("unexpected terrain_type: " + (index)) }));
        }
        // Java: 36
        public static string getRatedString(bool rated)
        {
            return com.oddlabs.tt.util.ServerMessageBundler.i18n((rated ? "rated_yes" : "rated_no"));
        }
        // Java: 40
        public static string getGamespeedString(int index)
        {
            return com.oddlabs.tt.util.ServerMessageBundler.i18n(((index) switch { 0 => (string)("gamespeed_pause"), 1 => (string)("gamespeed_slow"), 2 => (string)("gamespeed_normal"), 3 => (string)("gamespeed_fast"), 4 => (string)("gamespeed_ludicrous"), _ => throw new System.ArgumentException("unexpected gamespeed: " + (index)) }));
        }
        // Java: 51
        public static string getHillsString(int index)
        {
            return ((10 * index)) + "%";
        }
        // Java: 55
        public static string getTreesString(int index)
        {
            return ((10 * index)) + "%";
        }
        // Java: 59
        public static string getSuppliesString(int index)
        {
            return ((10 * index)) + "%";
        }
        // Java: 63
        public static string getRegistrationKeyFormatExceptionMessage(com.oddlabs.registration.RegistrationKeyFormatException e)
        {
            return ((e.getType()) switch { 0 => (string)(com.oddlabs.tt.util.ServerMessageBundler.i18n("invalid_char", e.getInvalidChar())), 1 => (string)(com.oddlabs.tt.util.ServerMessageBundler.i18n("invalid_length", e.getStrippedLength())), 2 => (string)(com.oddlabs.tt.util.ServerMessageBundler.i18n("invalid_key")), _ => throw new System.Exception() });
        }
        static ServerMessageBundler()
        {
            ServerMessageBundler.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.util.ServerMessageBundler).getName());
        }
        public ServerMessageBundler() : base()
        {
        }
    }
}
