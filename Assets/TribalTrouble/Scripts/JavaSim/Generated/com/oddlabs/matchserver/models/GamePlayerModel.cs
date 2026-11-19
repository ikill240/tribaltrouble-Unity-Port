// Translated from GamePlayerModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver.models
{
    // Java: 5
    public class GamePlayerModel : java.lang.Object
    {
        public string playerName;
        public string playerRace;
        public int? playerTeam;
        public com.oddlabs.matchmaking.Profile profile;
        // Java: 17
        public virtual string getPlayerName()
        {
            return playerName;
        }
        // Java: 21
        public virtual string getPlayerRace()
        {
            return playerRace;
        }
        // Java: 25
        public virtual int? getPlayerTeam()
        {
            return playerTeam;
        }
        // Java: 29
        public virtual com.oddlabs.matchmaking.Profile getProfile()
        {
            return profile;
        }
        // Java: 33
        public virtual void setProfile(com.oddlabs.matchmaking.Profile profile)
        {
            this.profile = profile;
        }
        public GamePlayerModel(string playerName, string playerRace, int? playerTeam) : base()
        {
            this.playerName = playerName;
            this.playerRace = playerRace;
            this.playerTeam = playerTeam;
        }
    }
}
