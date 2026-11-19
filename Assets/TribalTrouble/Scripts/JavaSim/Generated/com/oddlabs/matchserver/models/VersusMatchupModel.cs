// Translated from VersusMatchupModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver.models
{
    // Java: 7
    public class VersusMatchupModel : java.lang.Object
    {
        public string player1;
        public string player2;
        public string winner;
        public int game_id;
        public string game_name;
        public string map_seed;
        public java.sql.Timestamp startTime;
        // Java: 33
        public virtual int getGameId()
        {
            return game_id;
        }
        // Java: 37
        public virtual java.sql.Timestamp getStartTime()
        {
            return startTime;
        }
        // Java: 41
        public virtual string getGameName()
        {
            return game_name;
        }
        // Java: 45
        public virtual string getMapSeed()
        {
            return map_seed;
        }
        // Java: 49
        public virtual string getPlayer1()
        {
            return player1;
        }
        // Java: 53
        public virtual string getPlayer2()
        {
            return player2;
        }
        // Java: 57
        public virtual string getWinner()
        {
            return winner;
        }
        // Java: 61
        public virtual string getGameReplayUrl()
        {
            return com.oddlabs.matchserver.WebsiteLinkHelper.getReplayUrl(game_id);
        }
        public VersusMatchupModel(string player1, string player2, string winner, int game_id, string game_name, string map_seed, java.sql.Timestamp startTime) : base()
        {
            this.player1 = player1;
            this.player2 = player2;
            this.winner = winner;
            this.game_id = game_id;
            this.game_name = game_name;
            this.map_seed = map_seed;
            this.startTime = startTime;
        }
    }
}
