// Translated from RankingEntry.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 6
    public class RankingEntry : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public int ranking;
        public string name;
        public int rating;
        public int wins;
        public int losses;
        public int invalid;
        // Java: 26
        public int getRanking()
        {
            return ranking;
        }
        // Java: 30
        public string getName()
        {
            return name;
        }
        // Java: 34
        public int getRating()
        {
            return rating;
        }
        // Java: 38
        public int getWins()
        {
            return wins;
        }
        // Java: 42
        public int getLosses()
        {
            return losses;
        }
        // Java: 46
        public int getInvalid()
        {
            return invalid;
        }
        public RankingEntry(int ranking, string name, int rating, int wins, int losses, int invalid) : base()
        {
            this.ranking = ranking;
            this.name = name;
            this.rating = rating;
            this.wins = wins;
            this.losses = losses;
            this.invalid = invalid;
        }
    }
}
