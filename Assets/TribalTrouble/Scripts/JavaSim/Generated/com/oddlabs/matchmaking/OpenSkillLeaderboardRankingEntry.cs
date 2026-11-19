// Translated from OpenSkillLeaderboardRankingEntry.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 7
    public class OpenSkillLeaderboardRankingEntry : java.lang.Object
    {
        public int rank() { return rank_f; }
        public string nick() { return nick_f; }
        public int rating() { return rating_f; }
        public bool provisional() { return provisional_f; }
        public double mu() { return mu_f; }
        public double sigma() { return sigma_f; }
        public OpenSkillLeaderboardRankingEntry(int rank, string nick, int rating, bool provisional, double mu, double sigma)
        {
            this.rank_f = rank;
            this.nick_f = nick;
            this.rating_f = rating;
            this.provisional_f = provisional;
            this.mu_f = mu;
            this.sigma_f = sigma;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is OpenSkillLeaderboardRankingEntry r)) return false; return true && java.lang.JO.equals(rank_f, r.rank_f) && java.lang.JO.equals(nick_f, r.nick_f) && java.lang.JO.equals(rating_f, r.rating_f) && java.lang.JO.equals(provisional_f, r.provisional_f) && java.lang.JO.equals(mu_f, r.mu_f) && java.lang.JO.equals(sigma_f, r.sigma_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(rank_f); h = 31 * h + java.lang.JO.hashCode(nick_f); h = 31 * h + java.lang.JO.hashCode(rating_f); h = 31 * h + java.lang.JO.hashCode(provisional_f); h = 31 * h + java.lang.JO.hashCode(mu_f); h = 31 * h + java.lang.JO.hashCode(sigma_f); } return h; }
        public override string toString() { return "OpenSkillLeaderboardRankingEntry[" + "rank=" + java.lang.JO.toString(rank_f) + ", nick=" + java.lang.JO.toString(nick_f) + ", rating=" + java.lang.JO.toString(rating_f) + ", provisional=" + java.lang.JO.toString(provisional_f) + ", mu=" + java.lang.JO.toString(mu_f) + ", sigma=" + java.lang.JO.toString(sigma_f) + "]"; }
        public int rank_f;
        public string nick_f;
        public int rating_f;
        public bool provisional_f;
        public double mu_f;
        public double sigma_f;
    }
}
