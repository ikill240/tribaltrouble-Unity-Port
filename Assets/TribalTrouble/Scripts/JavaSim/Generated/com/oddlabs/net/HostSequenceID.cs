// Translated from HostSequenceID.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 9
    public class HostSequenceID : java.lang.Object
    {
        public const long serialVersionUID = (-8670168662221748395L);
        public int host_id;
        public int seq_id;
        // Java: 21
        public int getHostID()
        {
            return host_id;
        }
        // Java: 25
        public int getSequenceID()
        {
            return seq_id;
        }
        // Java: 29
        public override int hashCode()
        {
            return host_id ^ seq_id;
        }
        // Java: 34
        public override string toString()
        {
            return (host_id) + " " + (seq_id);
        }
        // Java: 39
        public override bool equals(object other)
        {
            return (other is com.oddlabs.net.HostSequenceID other_id) && host_id == other_id.host_id && seq_id == other_id.seq_id;
        }
        public HostSequenceID(int host_id, int seq_id) : base()
        {
            this.host_id = host_id;
            this.seq_id = seq_id;
        }
    }
}
