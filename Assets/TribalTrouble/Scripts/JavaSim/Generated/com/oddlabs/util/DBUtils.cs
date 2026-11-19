// Translated from DBUtils.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 8
    public class DBUtils : java.lang.Object
    {
        // Java: 9
        public static void initConnection(string address, string user, string password)
        {
            java.lang.JSystem.@out.println("[DBUtils stub] initConnection ignored (in-memory DB): " + java.lang.JStr.valueOf(address));
        }
        // Java: 13
        public static java.sql.Connection createDatabaseConnection()
        {
            throw new System.Exception("DBUtils stub: no database in test build");
        }
        // Java: 17
        public static java.sql.PreparedStatement createStatement(string sql)
        {
            throw new System.Exception("DBUtils stub: no database in test build");
        }
        // Java: 21
        public static void postHermesMessage(string message)
        {
            java.lang.JSystem.@out.println("[DBUtils stub] hermes: " + java.lang.JStr.valueOf(message));
        }
        public DBUtils() : base()
        {
        }
    }
}
