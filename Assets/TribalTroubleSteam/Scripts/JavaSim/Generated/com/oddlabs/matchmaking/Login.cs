// Translated from Login.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 9
    public class Login : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public string username;
        public string password_digest;
        // Java: 21
        public override bool equals(object other)
        {
            if (!((other is com.oddlabs.matchmaking.Login other_login)))
            {
                return false;
            }
            return java.lang.JO.equals(other_login.getUsername(), username) && java.lang.JO.equals(other_login.getPasswordDigest(), password_digest);
        }
        // Java: 28
        public override int hashCode()
        {
            return java.lang.JO.hashCode(username);
        }
        // Java: 33
        public bool isValid()
        {
            return username != null && password_digest != null;
        }
        // Java: 37
        public string getUsername()
        {
            return username;
        }
        // Java: 41
        public string getPasswordDigest()
        {
            return password_digest;
        }
        public Login(string username, string password_digest) : base()
        {
            this.username = username;
            this.password_digest = password_digest;
        }
    }
}
