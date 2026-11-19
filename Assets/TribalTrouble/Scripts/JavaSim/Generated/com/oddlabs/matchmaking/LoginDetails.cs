// Translated from LoginDetails.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 9
    public class LoginDetails : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public const int MAX_EMAIL_LENGTH = 60;
        public string email;
        // Java: 21
        public override bool equals(object other)
        {
            if (!((other is com.oddlabs.matchmaking.LoginDetails other_login)))
            {
                return false;
            }
            return java.lang.JO.equals(other_login.getEmail(), email);
        }
        // Java: 28
        public override int hashCode()
        {
            return java.lang.JO.hashCode(email);
        }
        // Java: 33
        public bool isValid()
        {
            return email != null && email.length() <= com.oddlabs.matchmaking.LoginDetails.MAX_EMAIL_LENGTH && com.oddlabs.util.Utils.EMAIL_PATTERN.matcher(email).matches();
        }
        // Java: 37
        public string getEmail()
        {
            return email;
        }
        public LoginDetails(string email) : base()
        {
            this.email = email;
        }
    }
}
