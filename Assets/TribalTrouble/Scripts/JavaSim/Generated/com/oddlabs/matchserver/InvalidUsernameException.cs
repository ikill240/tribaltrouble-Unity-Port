// Translated from InvalidUsernameException.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 3
    public class InvalidUsernameException : System.Exception
    {
        public int error_code;
        // Java: 10
        public int getErrorCode()
        {
            return error_code;
        }
        public InvalidUsernameException(int error_code) : base()
        {
            this.error_code = error_code;
        }
    }
}
