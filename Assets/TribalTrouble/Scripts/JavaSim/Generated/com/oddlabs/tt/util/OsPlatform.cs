// Translated from OsPlatform.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 3
    public class OsPlatform : java.lang.Object
    {
        public static string OS_NAME;
        public static bool IS_MAC;
        public static bool IS_WINDOWS;
        public static bool IS_LINUX;
        static OsPlatform()
        {
            OsPlatform.OS_NAME = java.lang.JSystem.getProperty("os.name", "").toLowerCase();
            OsPlatform.IS_MAC = com.oddlabs.tt.util.OsPlatform.OS_NAME.contains("mac");
            OsPlatform.IS_WINDOWS = com.oddlabs.tt.util.OsPlatform.OS_NAME.contains("windows");
            OsPlatform.IS_LINUX = com.oddlabs.tt.util.OsPlatform.OS_NAME.contains("linux") || com.oddlabs.tt.util.OsPlatform.OS_NAME.contains("unix");
        }
        public OsPlatform() : base()
        {
        }
    }
}
