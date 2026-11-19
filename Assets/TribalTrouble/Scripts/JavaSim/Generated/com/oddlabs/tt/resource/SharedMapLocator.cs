// Translated from SharedMapLocator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 34
    public class SharedMapLocator : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        // Java: 39
        public interface Validator
        {
            // Java: 40
            void check(java.nio.file.Path file);
        }
        public sealed class Validator_L : java.lang.Object, Validator
        {
            readonly System.Action<java.nio.file.Path> f;
            public Validator_L(System.Action<java.nio.file.Path> f) { this.f = f; }
            public void check(java.nio.file.Path p0) { f(p0); }
        }
        public string host_path;
        public string file_name;
        public string fingerprint_f;
        // Java: 60
        public string getFileName()
        {
            return file_name;
        }
        // Java: 65
        public java.nio.file.Path resolve()
        {
            java.nio.file.Path shared_dir = com.oddlabs.tt.resource.CustomMapGenerator.sharedMapsDir();
            java.nio.file.Path host = null;
            try
            {
                host = java.nio.file.Path_.of(host_path);
            }
            catch (System.Exception e)
            {
            }
            java.nio.file.Path mismatched = null;
            foreach (java.nio.file.Path candidate in new java.nio.file.Path[] { host, shared_dir.resolve(file_name), shared_dir.resolve(alternateFileName()) })
            {
                if (candidate == null || !java.nio.file.Files.isRegularFile(candidate))
                {
                    continue;
                }
                if (fingerprint_f == null)
                {
                    return candidate;
                }
                try
                {
                    if (java.lang.JO.equals(fingerprint_f, com.oddlabs.tt.resource.SharedMapLocator.fingerprint(candidate)))
                    {
                        return candidate;
                    }
                }
                catch (System.IO.IOException e_2)
                {
                    continue;
                }
                mismatched = candidate;
            }
            if (mismatched != null)
            {
                throw new System.IO.IOException(java.lang.JStr.valueOf(com.oddlabs.tt.resource.CustomMapGenerator.LOAD_FAILURE_PREFIX) + "Your copy of \"" + java.lang.JStr.valueOf(file_name) + "\" (" + java.lang.JStr.valueOf(mismatched) + ") is different from the host's. Every player needs the exact same file - copy the host's version into " + java.lang.JStr.valueOf(shared_dir) + " and join again.", (System.Exception)new System.IO.IOException("map fingerprint mismatch"));
            }
            throw new System.IO.IOException(java.lang.JStr.valueOf(com.oddlabs.tt.resource.CustomMapGenerator.LOAD_FAILURE_PREFIX) + "This game uses the map \"" + java.lang.JStr.valueOf(file_name) + "\", which isn't on this computer. Every player needs their own copy - put " + java.lang.JStr.valueOf(file_name) + " in " + java.lang.JStr.valueOf(shared_dir) + " and join again.", (System.Exception)new System.IO.IOException(file_name));
        }
        // Java: 96
        public bool hasLocalCopy()
        {
            try
            {
                resolve();
                return true;
            }
            catch (System.IO.IOException e)
            {
                return false;
            }
        }
        // Java: 105
        public sbyte[] readCompressed()
        {
            java.io.ByteArrayOutputStream bytes = new java.io.ByteArrayOutputStream();
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(resolve());
                java.io.OutputStream @out = new java.util.zip.GZIPOutputStream(bytes, 1 << 16);
                try
                {
                    @in.transferTo(@out);
                }
                finally
                {
                    if (@out != null) @out.close();
                    if (@in != null) @in.close();
                }
            }
            return bytes.toByteArray();
        }
        // Java: 115
        public java.nio.file.Path install(sbyte[] compressed, com.oddlabs.tt.resource.SharedMapLocator.Validator validator)
        {
            java.nio.file.Path dir = com.oddlabs.tt.resource.CustomMapGenerator.sharedMapsDir();
            java.nio.file.Files.createDirectories(dir);
            java.nio.file.Path temp = java.nio.file.Files.createTempFile(dir, "download-", ".part");
            try
            {
                {
                    java.io.InputStream @in = new java.util.zip.GZIPInputStream(new java.io.ByteArrayInputStream(compressed), 1 << 16);
                    java.io.OutputStream @out = java.nio.file.Files.newOutputStream(temp);
                    try
                    {
                        @in.transferTo(@out);
                    }
                    finally
                    {
                        if (@out != null) @out.close();
                        if (@in != null) @in.close();
                    }
                }
                if (fingerprint_f != null && !java.lang.JO.equals(fingerprint_f, com.oddlabs.tt.resource.SharedMapLocator.fingerprint(temp)))
                {
                    throw new System.IO.IOException("the received map doesn't match the host's file");
                }
                validator.check(temp);
                java.nio.file.Path target = dir.resolve(file_name);
                if (java.nio.file.Files.exists(target))
                {
                    target = dir.resolve(alternateFileName());
                }
                java.nio.file.Files.move(temp, target, java.nio.file.StandardCopyOption.REPLACE_EXISTING);
                return target;
            }
            finally
            {
                java.nio.file.Files.deleteIfExists(temp);
            }
        }
        // Java: 138
        public string alternateFileName()
        {
            string tag = (fingerprint_f != null ? fingerprint_f.substring(0, 8) : "copy");
            int dot = file_name.lastIndexOf('.');
            return (dot > 0 ? java.lang.JStr.valueOf(file_name.substring(0, dot)) + "-" + java.lang.JStr.valueOf(tag) + java.lang.JStr.valueOf(file_name.substring(dot)) : java.lang.JStr.valueOf(file_name) + "-" + java.lang.JStr.valueOf(tag));
        }
        // Java: 144
        public static string fingerprint(java.nio.file.Path file)
        {
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(file);
                try
                {
                    java.security.MessageDigest digest = java.security.MessageDigest.getInstance("SHA-256");
                    sbyte[] buffer = new sbyte[1 << 16];
                    for (int n = default; (n = @in.read(buffer)) > 0; )
                    {
                        digest.update(buffer, 0, n);
                    }
                    return java.util.HexFormat.of().formatHex(digest.digest());
                }
                catch (System.Exception e)
                {
                    throw new System.IO.IOException(java.lang.JStr.valueOf(e), (System.Exception)e);
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
        }
        public SharedMapLocator(java.nio.file.Path host_file) : base()
        {
            this.host_path = java.lang.JO.toString(host_file.toAbsolutePath());
            this.file_name = java.lang.JO.toString(host_file.getFileName());
            string fp = default;
            try
            {
                fp = com.oddlabs.tt.resource.SharedMapLocator.fingerprint(host_file);
            }
            catch (System.IO.IOException e)
            {
                fp = null;
            }
            this.fingerprint_f = fp;
        }
    }
}
