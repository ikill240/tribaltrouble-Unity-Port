// Translated from Utils.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 22
    public class Utils : java.lang.Object
    {
        public static java.util.regex.Pattern EMAIL_PATTERN;
        public static java.nio.file.Path STD_OUT;
        public static java.nio.file.Path STD_ERR;
        public static java.nio.file.Path EVENT_LOG;
        public static java.nio.file.Path[] LOG_FILES;
        // Java: 30
        public static java.net.InetAddress getLoopbackAddress()
        {
            try
            {
                return com.oddlabs.util.Utils.tryGetLoopbackAddress();
            }
            catch (System.IO.IOException e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 38
        public static java.net.InetAddress tryGetLoopbackAddress()
        {
            java.util.Enumeration interfaces = default;
            interfaces = java.net.NetworkInterface.getNetworkInterfaces();
            java.net.InetAddress best_address = null;
            while (interfaces.hasMoreElements())
            {
                java.net.NetworkInterface network_interface = ((java.net.NetworkInterface)interfaces.nextElement());
                java.util.Enumeration addresses = network_interface.getInetAddresses();
                while (addresses.hasMoreElements())
                {
                    java.net.InetAddress address = ((java.net.InetAddress)addresses.nextElement());
                    if (address.isLoopbackAddress())
                    {
                        best_address = address;
                        if (!((address is java.net.Inet6Address)))
                        {
                            goto brk_out_3;
                        }
                    }
                }
            }
            brk_out_3: ;
            if (best_address != null)
            {
                java.lang.IO.println("loopback address = " + java.lang.JStr.valueOf(best_address));
                return best_address;
            }
            throw new System.IO.IOException("Could not find a loopback address");
        }
        // Java: 63
        public static int powerOf2Log2(int n)
        {
            for (int i = 0; i < 31; i++)
            {
                if ((n & 1) == 1)
                {
                    return i;
                }
                n >>= 1;
            }
            return 0;
        }
        // Java: 74
        public static bool isPowerOf2(int n)
        {
            return n == 0 || (n > 0 && (n & (n - 1)) == 0);
        }
        // Java: 78
        public static int nextPowerOf2(int n)
        {
            int x = 1;
            while (x < n)
            {
                x <<= 1;
            }
            return x;
        }
        // Java: 86
        public static void flip(sbyte[] bytes, int width, int height)
        {
            sbyte[] line = new sbyte[width];
            for (int i = 0; i < height / 2; i++)
            {
                java.lang.JSystem.arraycopy(bytes, i * width, line, 0, width);
                java.lang.JSystem.arraycopy(bytes, (height - i - 1) * width, bytes, i * width, width);
                java.lang.JSystem.arraycopy(line, 0, bytes, (height - i - 1) * width, width);
            }
        }
        // Java: 96
        public static void flip(java.nio.ByteBuffer bytes, int width, int height)
        {
            sbyte[] line = new sbyte[width];
            sbyte[] line2 = new sbyte[width];
            for (int i = 0; i < height / 2; i++)
            {
                bytes.position(i * width);
                bytes.get(line);
                bytes.position((height - i - 1) * width);
                bytes.get(line2);
                bytes.position(i * width);
                bytes.put(line2);
                bytes.position((height - i - 1) * width);
                bytes.put(line);
            }
        }
        // Java: 112
        public static object loadObject(java.net.URL url)
        {
            throw new System.NotSupportedException("J2CS stub: Utils.loadObject");
        }
        // Java: 116
        public static object loadObject(java.net.URL url, bool zipped)
        {
            throw new System.NotSupportedException("J2CS stub: Utils.loadObject");
        }
        // Java: 126
        public static object tryLoadObject(java.net.URL url)
        {
            return com.oddlabs.util.Utils.tryLoadObject(url, false);
        }
        // Java: 130
        public static object tryLoadObject(java.net.URL url, bool zipped)
        {
            {
                java.io.InputStream urlStream = url.openStream();
                try
                {
                    {
                        java.io.InputStream input_stream = (zipped ? (java.io.InputStream)(new java.util.zip.GZIPInputStream(urlStream)) : (java.io.InputStream)(new java.io.BufferedInputStream(urlStream)));
                        try
                        {
                            {
                                java.io.ObjectInputStream obj_stream = new java.io.ObjectInputStream(input_stream);
                                try
                                {
                                    object obj = ((object)obj_stream.readObject());
                                    return obj;
                                }
                                finally
                                {
                                    if (obj_stream != null) obj_stream.close();
                                }
                            }
                        }
                        finally
                        {
                            if (input_stream != null) input_stream.close();
                        }
                    }
                }
                finally
                {
                    if (urlStream != null) urlStream.close();
                }
            }
        }
        // Java: 142
        public static java.net.URI makeURI(string location)
        {
            try
            {
                return com.oddlabs.util.Utils.makeURL(location).toURI();
            }
            catch (System.Exception e)
            {
                throw new System.IO.IOException("Unusable location: " + java.lang.JStr.valueOf(location), (System.Exception)new System.IO.IOException(java.lang.JStr.valueOf(e), (System.Exception)e));
            }
        }
        // Java: 150
        public static java.net.URL makeURL(string location)
        {
            try
            {
                return com.oddlabs.util.Utils.tryMakeURL(location);
            }
            catch (System.IO.IOException e)
            {
                throw new System.IO.IOException("Unusable location: " + java.lang.JStr.valueOf(location), (System.Exception)e);
            }
        }
        // Java: 158
        public static java.net.URL tryMakeURL(string location)
        {
            java.net.URL url = typeof(com.oddlabs.util.Utils).getResource(location);
            if (url == null)
            {
                throw new System.IO.IOException(java.lang.JStr.valueOf(location) + " not found");
            }
            return url;
        }
        static Utils()
        {
            Utils.EMAIL_PATTERN = java.util.regex.Pattern.compile("(.+@.+\\.[a-z]+)?");
            Utils.STD_OUT = java.nio.file.Path_.of("std.out");
            Utils.STD_ERR = java.nio.file.Path_.of("std.err");
            Utils.EVENT_LOG = java.nio.file.Path_.of("event.log");
            Utils.LOG_FILES = new java.nio.file.Path[] { com.oddlabs.util.Utils.STD_OUT, com.oddlabs.util.Utils.STD_ERR, com.oddlabs.util.Utils.EVENT_LOG };
        }
        public Utils() : base()
        {
        }
    }
}
