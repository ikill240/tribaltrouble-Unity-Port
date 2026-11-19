// Translated from DefaultARMIArgumentWriter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 9
    public class DefaultARMIArgumentWriter : java.lang.Object, com.oddlabs.net.ARMIArgumentWriter
    {
        // Java: 10
        public virtual void writeArgument(System.Type type, object arg, com.oddlabs.util.ByteBufferOutputStream @out)
        {
            if (java.lang.JO.equals(type, typeof(char)))
            {
                char c = ((char)(((char?)arg)));
                @out.buffer().putChar(c);
            }
            else if (java.lang.JO.equals(type, typeof(sbyte)))
            {
                sbyte b = ((sbyte)(((sbyte?)arg)));
                @out.buffer().put(b);
            }
            else if (java.lang.JO.equals(type, typeof(short)))
            {
                short s = ((short)(((short?)arg)));
                @out.buffer().putShort(s);
            }
            else if (java.lang.JO.equals(type, typeof(int)))
            {
                int integer = ((int)(((int?)arg)));
                @out.buffer().putInt(integer);
            }
            else if (java.lang.JO.equals(type, typeof(long)))
            {
                long l = ((long)(((long?)arg)));
                @out.buffer().putLong(l);
            }
            else if (java.lang.JO.equals(type, typeof(float)))
            {
                float f = (((object)arg)).floatValue();
                @out.buffer().putFloat(f);
            }
            else if (java.lang.JO.equals(type, typeof(double)))
            {
                double d = ((double)(((double?)arg)));
                @out.buffer().putDouble(d);
            }
            else if (java.lang.JO.equals(type, typeof(bool)))
            {
                bool @bool = ((bool)(((bool?)arg)));
                sbyte val = (@bool ? unchecked((sbyte)(1)) : unchecked((sbyte)(0)));
                @out.buffer().put(val);
            }
            else if (java.lang.JO.equals(type, typeof(com.oddlabs.net.HostSequenceID)))
            {
                com.oddlabs.net.HostSequenceID host_seq = ((com.oddlabs.net.HostSequenceID)arg);
                @out.buffer().putInt(host_seq.getHostID());
                @out.buffer().putInt(host_seq.getSequenceID());
            }
            else if (java.lang.JO.equals(type, typeof(com.oddlabs.net.ARMIEvent)))
            {
                com.oddlabs.net.ARMIEvent @event = ((com.oddlabs.net.ARMIEvent)arg);
                @out.buffer().putShort(@event.getEventSize());
                @event.write(@out.buffer());
            }
            else
            {
                {
                    java.io.ObjectOutputStream obj_output_stream = new java.io.ObjectOutputStream(@out);
                    try
                    {
                        obj_output_stream.writeObject(arg);
                    }
                    finally
                    {
                        if (obj_output_stream != null) obj_output_stream.close();
                    }
                }
            }
        }
        public DefaultARMIArgumentWriter() : base()
        {
        }
    }
}
