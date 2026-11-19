// Translated from DefaultARMIArgumentReader.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 10
    public class DefaultARMIArgumentReader : java.lang.Object, com.oddlabs.net.ARMIArgumentReader
    {
        public static java.io.ObjectInputFilter ARMI_FILTER;
        // Java: 14
        public virtual object readArgument(System.Type type, com.oddlabs.net.ByteBufferInputStream @in)
        {
            if (java.lang.JO.equals(type, typeof(char)))
            {
                return @in.buffer().getChar();
            }
            else if (java.lang.JO.equals(type, typeof(sbyte)))
            {
                return @in.buffer().get();
            }
            else if (java.lang.JO.equals(type, typeof(short)))
            {
                return @in.buffer().getShort();
            }
            else if (java.lang.JO.equals(type, typeof(int)))
            {
                return @in.buffer().getInt();
            }
            else if (java.lang.JO.equals(type, typeof(long)))
            {
                return @in.buffer().getLong();
            }
            else if (java.lang.JO.equals(type, typeof(float)))
            {
                return @in.buffer().getFloat();
            }
            else if (java.lang.JO.equals(type, typeof(double)))
            {
                return @in.buffer().getDouble();
            }
            else if (java.lang.JO.equals(type, typeof(bool)))
            {
                return @in.buffer().get() != 0;
            }
            else if (java.lang.JO.equals(type, typeof(com.oddlabs.net.HostSequenceID)))
            {
                int host_id = @in.buffer().getInt();
                int seq_id = @in.buffer().getInt();
                return new com.oddlabs.net.HostSequenceID(host_id, seq_id);
            }
            else if (java.lang.JO.equals(type, typeof(com.oddlabs.net.ARMIEvent)))
            {
                short event_size = @in.buffer().getShort();
                return com.oddlabs.net.ARMIEvent.read(@in.buffer(), event_size);
            }
            else
            {
                {
                    java.io.ObjectInputStream input_stream = new java.io.ObjectInputStream(@in);
                    try
                    {
                        input_stream.setObjectInputFilter(com.oddlabs.net.DefaultARMIArgumentReader.ARMI_FILTER);
                        return input_stream.readObject();
                    }
                    finally
                    {
                        if (input_stream != null) input_stream.close();
                    }
                }
            }
        }
        static DefaultARMIArgumentReader()
        {
            DefaultARMIArgumentReader.ARMI_FILTER = java.io.ObjectInputFilter.Config.createFilter("com.oddlabs.**;java.lang.*;java.net.*;java.math.*;java.security.*;javax.crypto.*;!*");
        }
        public DefaultARMIArgumentReader() : base()
        {
        }
    }
}
