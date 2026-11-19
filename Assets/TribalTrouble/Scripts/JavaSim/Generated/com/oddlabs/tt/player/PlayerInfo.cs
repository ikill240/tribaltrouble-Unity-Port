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
        public string chief_name;
        public string tribe_name;
        // Java: 35
        public static string clean(string s)
        {
            if (s == null)
            {
                return "";
            }
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            for (int i = 0; i < s.length() && sb.length() < 20; i++)
            {
                char c = s.charAt(i);
                if (java.lang.Character.isLetterOrDigit(c) || c == ' ' || c == '-' || c == '\'' || c == '.')
                {
                    sb.append(c);
                }
            }
            return java.lang.JO.toString(sb).trim();
        }
        // Java: 47
        public string getChiefName()
        {
            return (chief_name == null ? "" : chief_name);
        }
        // Java: 51
        public string getTribeName()
        {
            return (tribe_name == null ? "" : tribe_name);
        }
        // Java: 56
        public string getDisplayName()
        {
            return (getTribeName().isEmpty() ? name : java.lang.JStr.valueOf(getTribeName()) + " (" + java.lang.JStr.valueOf(name) + ")");
        }
        // Java: 60
        public override bool equals(object other)
        {
            return (other is com.oddlabs.tt.player.PlayerInfo player) && team == player.team && race == player.race;
        }
        // Java: 67
        // the race field also says whether the tribe is an Elemental one (the third saga's): base race + RACE_ELEMENTAL
        public int getRace()
        {
            return (race >= com.oddlabs.tt.model.RacesResources.RACE_ELEMENTAL ? race - com.oddlabs.tt.model.RacesResources.RACE_ELEMENTAL : race);
        }
        public bool isElementalRace()
        {
            return race >= com.oddlabs.tt.model.RacesResources.RACE_ELEMENTAL;
        }
        // the race as chosen in a menu (0..NUM_RACE_CHOICES-1)
        public int getRaceChoice()
        {
            return race;
        }
        // Java: 71
        public string getName()
        {
            return name;
        }
        // Java: 75
        public int getTeam()
        {
            return team;
        }
        // Java: 79
        public override string toString()
        {
            return name;
        }
        public PlayerInfo(int team, int race, string name) : this(team, race, name, "", "")
        {
        }
        public PlayerInfo(int team, int race, string name, string chief_name, string tribe_name) : base()
        {
            this.team = team;
            this.race = race;
            this.name = name;
            this.chief_name = com.oddlabs.tt.player.PlayerInfo.clean(chief_name);
            this.tribe_name = com.oddlabs.tt.player.PlayerInfo.clean(tribe_name);
        }
    }
}
