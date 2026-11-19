// Translated from OpenSkillRating.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 5
    public class OpenSkillRating : java.lang.Object
    {
        public string nick() { return nick_f; }
        public double mu() { return mu_f; }
        public double sigma() { return sigma_f; }
        public OpenSkillRating(string nick, double mu, double sigma)
        {
            this.nick_f = nick;
            this.mu_f = mu;
            this.sigma_f = sigma;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is OpenSkillRating r)) return false; return true && java.lang.JO.equals(nick_f, r.nick_f) && java.lang.JO.equals(mu_f, r.mu_f) && java.lang.JO.equals(sigma_f, r.sigma_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(nick_f); h = 31 * h + java.lang.JO.hashCode(mu_f); h = 31 * h + java.lang.JO.hashCode(sigma_f); } return h; }
        public override string toString() { return "OpenSkillRating[" + "nick=" + java.lang.JO.toString(nick_f) + ", mu=" + java.lang.JO.toString(mu_f) + ", sigma=" + java.lang.JO.toString(sigma_f) + "]"; }
        public string nick_f;
        public double mu_f;
        public double sigma_f;
    }
}
