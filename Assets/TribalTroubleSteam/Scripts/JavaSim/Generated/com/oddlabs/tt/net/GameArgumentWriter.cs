// Translated from GameArgumentWriter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 9
    public class GameArgumentWriter : com.oddlabs.net.DefaultARMIArgumentWriter
    {
        public com.oddlabs.tt.net.DistributableTable distributable_table;
        // Java: 16
        public override void writeArgument(System.Type type, object arg, com.oddlabs.util.ByteBufferOutputStream @out)
        {
            if (typeof(com.oddlabs.tt.net.Distributable).isAssignableFrom(type))
            {
                int name = distributable_table.getName(((com.oddlabs.tt.net.Distributable)arg));
                @out.buffer().putInt(name);
            }
            else if (typeof(com.oddlabs.tt.net.Distributable[]).isAssignableFrom(type))
            {
                com.oddlabs.tt.net.Distributable[] distributables = ((com.oddlabs.tt.net.Distributable[])arg);
                @out.buffer().putShort(unchecked((short)(distributables.Length)));
                foreach (com.oddlabs.tt.net.Distributable distributable in distributables)
                {
                    int name_2 = distributable_table.getName(distributable);
                    @out.buffer().putInt(name_2);
                }
            }
            else
            {
                base.writeArgument(type, arg, @out);
            }
        }
        public GameArgumentWriter(com.oddlabs.tt.net.DistributableTable table) : base()
        {
            this.distributable_table = table;
        }
    }
}
