// Translated from Participant.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 9
    public class Participant : java.lang.Object
    {
        public const long serialVersionUID = (-3344403341742210958L);
        public int match_id;
        public string nick;
        public int team;
        public int race;
        // Java: 25
        public bool validate()
        {
            return team >= 0 && team < com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS;
        }
        // Java: 29
        public int getMatchID()
        {
            return match_id;
        }
        // Java: 33
        public string getNick()
        {
            return nick;
        }
        // Java: 37
        public int getTeam()
        {
            return team;
        }
        // Java: 41
        public int getRace()
        {
            return race;
        }
        // Java: 45
        public override int hashCode()
        {
            return match_id ^ team;
        }
        // Java: 50
        public override bool equals(object other)
        {
            return (other is com.oddlabs.matchmaking.Participant other_part) && match_id == other_part.match_id && team == other_part.team && race == other_part.race;
        }
        public Participant(int match_id, string nick, int team, int race) : base()
        {
            this.match_id = match_id;
            this.nick = nick;
            this.team = team;
            this.race = race;
        }
    }
}
