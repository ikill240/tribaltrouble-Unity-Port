// Translated from Profile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 6
    public class Profile : java.lang.Object
    {
        public const long serialVersionUID = (-3399364532017471737L);
        public string nick;
        public int rating;
        public int wins;
        public int losses;
        public int invalid;
        public int revision;
        // Java: 26
        public override string toString()
        {
            return nick;
        }
        // Java: 31
        public string getNick()
        {
            return nick;
        }
        // Java: 35
        public int getRating()
        {
            return rating;
        }
        // Java: 39
        public int getWins()
        {
            return wins;
        }
        // Java: 43
        public int getLosses()
        {
            return losses;
        }
        // Java: 47
        public int getInvalid()
        {
            return invalid;
        }
        // Java: 51
        public int getRevision()
        {
            return revision;
        }
        public Profile(string nick, int rating, int wins, int losses, int invalid, int revision) : base()
        {
            this.nick = nick;
            this.rating = rating;
            this.wins = wins;
            this.losses = losses;
            this.invalid = invalid;
            this.revision = revision;
        }
    }
}
