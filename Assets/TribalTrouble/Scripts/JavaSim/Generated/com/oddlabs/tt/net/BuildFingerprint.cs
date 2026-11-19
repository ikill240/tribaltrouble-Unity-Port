// Translated from BuildFingerprint.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 34
    public class BuildFingerprint : java.lang.Object
    {
        public const string UNKNOWN = "unknown";
        public static string cached;
        // Java: 41
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static string get()
        {
            if (com.oddlabs.tt.net.BuildFingerprint.cached == null)
            {
                com.oddlabs.tt.net.BuildFingerprint.cached = com.oddlabs.tt.net.BuildFingerprint.compute();
                java.lang.IO.println("Build fingerprint: " + java.lang.JStr.valueOf(com.oddlabs.tt.net.BuildFingerprint.cached));
            }
            return com.oddlabs.tt.net.BuildFingerprint.cached;
        }
        // Java: 50
        public static string shortForm(string fingerprint)
        {
            if (fingerprint == null)
            {
                return com.oddlabs.tt.net.BuildFingerprint.UNKNOWN;
            }
            return (fingerprint.length() > 8 ? fingerprint.substring(0, 8) : fingerprint);
        }
        // Java: 57
        public static bool matches(string a, string b)
        {
            if (a == null || b == null || java.lang.JO.equals(com.oddlabs.tt.net.BuildFingerprint.UNKNOWN, a) || java.lang.JO.equals(com.oddlabs.tt.net.BuildFingerprint.UNKNOWN, b))
            {
                return true;
            }
            return java.lang.JO.equals(a, b);
        }
        // Java: 63
        public static string compute()
        {
            try
            {
                java.security.MessageDigest digest = java.security.MessageDigest.getInstance("SHA-256");
                foreach (System.Type anchor in new System.Type[] { typeof(com.oddlabs.tt.Main), typeof(com.oddlabs.net.ARMIEvent) })
                {
                    java.nio.file.Path root = com.oddlabs.tt.net.BuildFingerprint.codeRoot(anchor);
                    if (root == null)
                    {
                        return com.oddlabs.tt.net.BuildFingerprint.UNKNOWN;
                    }
                    if (java.nio.file.Files.isDirectory(root))
                    {
                        com.oddlabs.tt.net.BuildFingerprint.hashDirectory(root, digest);
                    }
                    else
                    {
                        com.oddlabs.tt.net.BuildFingerprint.hashJar(root, digest);
                    }
                }
                return java.util.HexFormat.of().formatHex(digest.digest());
            }
            catch (System.Exception e) when (e is System.IO.IOException || e is System.Exception || e is System.Exception)
            {
                java.lang.IO.println("Couldn't compute the build fingerprint: " + java.lang.JStr.valueOf(e));
                return com.oddlabs.tt.net.BuildFingerprint.UNKNOWN;
            }
        }
        // Java: 83
        public static java.nio.file.Path codeRoot(System.Type anchor)
        {
            return default;
        }
        // Java: 94
        public static void hashDirectory(java.nio.file.Path root, java.security.MessageDigest digest)
        {
            throw new System.NotSupportedException("J2CS stub: BuildFingerprint.hashDirectory");
        }
        // Java: 106
        public static void hashJar(java.nio.file.Path jar_path, java.security.MessageDigest digest)
        {
            throw new System.NotSupportedException("J2CS stub: BuildFingerprint.hashJar");
        }
        // Java: 120
        public static string relativeName(java.nio.file.Path root, java.nio.file.Path file)
        {
            return java.lang.JO.toString(root.relativize(file)).replace('\\', '/');
        }
        public BuildFingerprint() : base()
        {
        }
    }
}
