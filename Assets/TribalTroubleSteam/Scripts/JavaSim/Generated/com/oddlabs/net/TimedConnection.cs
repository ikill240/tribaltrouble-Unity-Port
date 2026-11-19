// Translated from TimedConnection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 6
    public class TimedConnection : java.lang.Object
    {
        public long timeout;
        public com.oddlabs.net.Connection conn;
        // Java: 15
        public long getTimeout()
        {
            return timeout;
        }
        // Java: 19
        public com.oddlabs.net.Connection getConnection()
        {
            return conn;
        }
        // Java: 23
        public override bool equals(object other)
        {
            return (other is com.oddlabs.net.TimedConnection other_timed) && java.lang.JO.equals(other_timed.conn, this.conn);
        }
        // Java: 29
        public override int hashCode()
        {
            return java.lang.JO.hashCode(conn);
        }
        public TimedConnection(long timeout, com.oddlabs.net.Connection conn) : base()
        {
            this.timeout = timeout;
            this.conn = conn;
        }
    }
}
