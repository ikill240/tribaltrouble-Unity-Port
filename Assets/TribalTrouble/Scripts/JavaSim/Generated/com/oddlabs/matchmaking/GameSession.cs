// Translated from GameSession.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 6
    public class GameSession : java.lang.Object
    {
        public const long serialVersionUID = 2768150608081852612L;
        public const int MIN_WINS_FOR_RANKING = 5;
        public const int WIN = 0;
        public const int LOSE = 1;
        public int session_id;
        public com.oddlabs.matchmaking.Participant[] participants;
        public com.oddlabs.matchmaking.GamePlayer[] playerInfo;
        public bool rated;
        // Java: 36
        public bool validateTeams()
        {
            bool[] teams = new bool[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            int team_count = 0;
            for (int i = 0; i < participants.Length; i++)
            {
                com.oddlabs.matchmaking.Participant p = participants[i];
                if (!p.validate())
                {
                    return false;
                }
                if (!teams[p.getTeam()])
                {
                    teams[p.getTeam()] = true;
                    team_count++;
                }
            }
            return team_count >= com.oddlabs.matchmaking.MatchmakingServerInterface.MIN_PLAYERS;
        }
        // Java: 50
        public override int hashCode()
        {
            return session_id;
        }
        // Java: 54
        public override bool equals(object other)
        {
            if (!((other is com.oddlabs.matchmaking.GameSession)))
            {
                return false;
            }
            com.oddlabs.matchmaking.GameSession other_game = ((com.oddlabs.matchmaking.GameSession)other);
            return other_game.session_id == session_id && java.util.Arrays.equals(other_game.participants, participants) && rated == other_game.rated;
        }
        // Java: 62
        public bool validate()
        {
            return participants != null && participants.Length <= com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS && participants.Length >= com.oddlabs.matchmaking.MatchmakingServerInterface.MIN_PLAYERS && validateTeams();
        }
        // Java: 69
        public int getID()
        {
            return session_id;
        }
        // Java: 76
        public com.oddlabs.matchmaking.Participant[] getParticipants()
        {
            return participants;
        }
        // Java: 85
        public com.oddlabs.matchmaking.GamePlayer[] getPlayerInfo()
        {
            return playerInfo;
        }
        // Java: 89
        public bool isRated()
        {
            return rated;
        }
        // Java: 93
        public static int[][] calculateMatchPoints(int[] player_ratings, int[] player_teams)
        {
            int num_players = player_ratings.Length;
            int[] team_ratings = new int[2];
            int[] team_sizes = new int[2];
            for (int i = 0; i < num_players; i++)
            {
                int team = player_teams[i];
                team_sizes[team]++;
                team_ratings[team] += player_ratings[i];
            }
            int[][] result = java.lang.JA.newArray2<int>(num_players, 2);
            if (team_sizes[0] > 0 && team_sizes[1] > 0)
            {
                int K = 16 * num_players;
                float E0 = (float)(1.0f / ((float)(1.0f + ((float)(java.lang.StrictMath.pow(10.0d, (float)((team_ratings[1] - team_ratings[0]) / 400.0f)))))));
                float E1 = (float)(1.0f / ((float)(1.0f + ((float)(java.lang.StrictMath.pow(10.0d, (float)((team_ratings[0] - team_ratings[1]) / 400.0f)))))));
                int team_0_wins = java.lang.StrictMath.round((float)(K * ((float)(1 - E0))));
                int team_1_looses = java.lang.StrictMath.round((float)(K * ((float)(0 - E1))));
                int team_0_looses = java.lang.StrictMath.round((float)(K * ((float)(0 - E0))));
                int team_1_wins = java.lang.StrictMath.round((float)(K * ((float)(1 - E1))));
                for (int i_2 = 0; i_2 < num_players; i_2++)
                {
                    int team_size = team_sizes[player_teams[i_2]];
                    if (player_teams[i_2] == 0)
                    {
                        result[i_2][com.oddlabs.matchmaking.GameSession.WIN] = team_0_wins / team_size;
                        result[i_2][com.oddlabs.matchmaking.GameSession.LOSE] = team_0_looses / team_size;
                    }
                    else
                    {
                        result[i_2][com.oddlabs.matchmaking.GameSession.WIN] = team_1_wins / team_size;
                        result[i_2][com.oddlabs.matchmaking.GameSession.LOSE] = team_1_looses / team_size;
                    }
                }
            }
            return result;
        }
        public GameSession(int session_id, com.oddlabs.matchmaking.Participant[] participants, bool rated, com.oddlabs.matchmaking.GamePlayer[] playerInfo) : base()
        {
            this.session_id = session_id;
            this.participants = participants;
            this.rated = rated;
            this.playerInfo = playerInfo;
        }
    }
}
