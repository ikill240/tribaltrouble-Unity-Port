// Translated from SessionInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 8
    public class SessionInfo : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public int milliseconds_per_heartbeat;
        public int num_participants;
        // Java: 20
        public override bool equals(object other)
        {
            return (other is com.oddlabs.router.SessionInfo si) && si.num_participants == num_participants;
        }
        // Java: 25
        public override int hashCode()
        {
            return num_participants;
        }
        // Java: 30
        public override string toString()
        {
            return "(SessionInfo: num_participants = " + (num_participants) + ")";
        }
        public SessionInfo(int num_participants, int milliseconds_per_heartbeat) : base()
        {
            this.num_participants = num_participants;
            this.milliseconds_per_heartbeat = milliseconds_per_heartbeat;
        }
    }
}
