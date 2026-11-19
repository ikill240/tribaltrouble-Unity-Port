// Translated from PlayerInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 9
    public class PlayerInfo : java.lang.Object
    {
        public const long serialVersionUID = 3L;
        public const int TEAM_NEUTRAL = (-1);
        public int race;
        public string name;
        public int team;
        // Java: 25
        public override bool equals(object other)
        {
            return (other is com.oddlabs.tt.player.PlayerInfo player) && team == player.team && race == player.race;
        }
        // Java: 32
        public int getRace()
        {
            return race;
        }
        // Java: 36
        public string getName()
        {
            return name;
        }
        // Java: 40
        public int getTeam()
        {
            return team;
        }
        // Java: 44
        public override string toString()
        {
            return name;
        }
        public PlayerInfo(int team, int race, string name) : base()
        {
            this.team = team;
            this.race = race;
            this.name = name;
        }
    }
}
