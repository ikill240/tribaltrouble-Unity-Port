// Translated from SessionID.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 9
    public class SessionID : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public long session_id;
        // Java: 19
        public override bool equals(object other)
        {
            return (other is com.oddlabs.router.SessionID sid) && sid.session_id == session_id;
        }
        // Java: 24
        public override int hashCode()
        {
            return unchecked((int)(session_id));
        }
        // Java: 29
        public override string toString()
        {
            return "(SessionID: session_id = " + (session_id) + ")";
        }
        public SessionID(long session_id) : base()
        {
            this.session_id = session_id;
        }
    }
}
