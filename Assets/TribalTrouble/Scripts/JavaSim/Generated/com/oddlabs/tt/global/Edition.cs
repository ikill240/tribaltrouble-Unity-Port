// Translated from Edition.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.global
{
    // Java: 11
    public class Edition : java.lang.Object
    {
        public static bool new_tides;
        public static bool extended_default;
        public const string GAME_NAME = "TribalTrouble";
        public const string DIR_NAME = "tribaltrouble";
        public const string TITLE = "Tribal Trouble";
        static Edition()
        {
            Edition.new_tides = false;
            Edition.extended_default = false;
        }
        public Edition() : base()
        {
        }
    }
}
