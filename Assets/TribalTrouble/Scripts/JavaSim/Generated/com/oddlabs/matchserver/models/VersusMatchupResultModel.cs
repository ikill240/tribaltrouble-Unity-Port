// Translated from VersusMatchupResultModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver.models
{
    // Java: 5
    public class VersusMatchupResultModel : java.lang.Object
    {
        public string player1;
        public string player2;
        public int player1Wins;
        public int player2Wins;
        public int neitherWins;
        public java.util.List recentMatchups;
        // Java: 28
        public virtual string getPlayer1()
        {
            return player1;
        }
        // Java: 32
        public virtual string getPlayer2()
        {
            return player2;
        }
        // Java: 36
        public virtual int getPlayer1Wins()
        {
            return player1Wins;
        }
        // Java: 40
        public virtual int getPlayer2Wins()
        {
            return player2Wins;
        }
        // Java: 44
        public virtual int getTotalGamesPlayed()
        {
            return player1Wins + player2Wins + neitherWins;
        }
        // Java: 48
        public virtual java.util.List getRecentMatchups()
        {
            return recentMatchups;
        }
        public VersusMatchupResultModel(string player1, string player2, int player1Wins, int player2Wins, int neitherWins, java.util.List recentMatchups) : base()
        {
            this.player1 = player1;
            this.player2 = player2;
            this.player1Wins = player1Wins;
            this.player2Wins = player2Wins;
            this.neitherWins = neitherWins;
            this.recentMatchups = recentMatchups;
        }
    }
}
