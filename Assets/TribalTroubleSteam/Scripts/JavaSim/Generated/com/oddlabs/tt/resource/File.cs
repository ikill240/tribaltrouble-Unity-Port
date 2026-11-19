// Translated from File.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 18
    public abstract class File : java.lang.Object, java.util.function.Supplier
    {
        public java.net.URI uri;
        // Java: 30
        public java.net.URL getURL()
        {
            try
            {
                return uri.toURL();
            }
            catch (System.IO.IOException e)
            {
                throw new System.IO.IOException(java.lang.JStr.valueOf(new System.IO.IOException("bad location: " + java.lang.JStr.valueOf(uri), (System.Exception)e)), (System.Exception)new System.IO.IOException("bad location: " + java.lang.JStr.valueOf(uri), (System.Exception)e));
            }
        }
        // Java: 38
        public abstract object get();
        // Java: 41
        public override int hashCode()
        {
            return java.lang.JO.hashCode(uri);
        }
        // Java: 46
        public override bool equals(object o)
        {
            return (o is com.oddlabs.tt.resource.File other) && java.lang.JO.equals(uri, other.uri);
        }
        // Java: 51
        public override string toString()
        {
            return java.lang.JStr.valueOf(this.GetType().getSimpleName()) + "{uri=" + java.lang.JStr.valueOf(uri.toASCIIString()) + ('}').ToString();
        }
        // Java: 56
        public static java.util.Optional locate(string location)
        {
            java.net.URL url_classpath = typeof(com.oddlabs.util.Utils).getResource(location);
            if (url_classpath != null)
            {
                try
                {
                    return java.util.Optional.of(url_classpath.toURI());
                }
                catch (System.Exception e)
                {
                    throw new System.ArgumentException(java.lang.JStr.valueOf(e), (System.Exception)e);
                }
            }
            java.nio.file.Path file = com.oddlabs.tt.util.Utils.getInstallDir().resolve(location);
            return (java.nio.file.Files.isRegularFile(file) && java.nio.file.Files.isReadable(file) ? java.util.Optional.of(file.toUri()) : java.util.Optional.empty());
        }
        public File(java.net.URI uri) : base()
        {
            this.uri = uri;
        }
        public File(string location) : this(com.oddlabs.util.Utils.makeURI(location))
        {
        }
    }
}
