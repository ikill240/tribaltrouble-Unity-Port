// Translated from GameArgumentReader.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 11
    public class GameArgumentReader : com.oddlabs.net.DefaultARMIArgumentReader
    {
        public com.oddlabs.tt.net.DistributableTable distributable_table;
        // Java: 18
        public override object readArgument(System.Type type, com.oddlabs.net.ByteBufferInputStream @in)
        {
            if (typeof(com.oddlabs.tt.net.Distributable).isAssignableFrom(type))
            {
                int name = @in.buffer().getInt();
                return distributable_table.getDistributable(name);
            }
            else if (typeof(com.oddlabs.tt.net.Distributable[]).isAssignableFrom(type))
            {
                short length = @in.buffer().getShort();
                com.oddlabs.tt.net.Distributable[] distributables = ((com.oddlabs.tt.net.Distributable[])java.lang.reflect.Array.newInstance(type.getComponentType(), length));
                for (int j = 0; j < distributables.Length; j++)
                {
                    int name_2 = @in.buffer().getInt();
                    distributables[j] = distributable_table.getDistributable(name_2);
                }
                return distributables;
            }
            else
            {
                return base.readArgument(type, @in);
            }
        }
        public GameArgumentReader(com.oddlabs.tt.net.DistributableTable table) : base()
        {
            this.distributable_table = table;
        }
    }
}
