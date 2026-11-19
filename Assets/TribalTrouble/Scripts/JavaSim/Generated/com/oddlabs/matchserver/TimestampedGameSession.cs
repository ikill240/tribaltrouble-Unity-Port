// Translated from TimestampedGameSession.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 22
    public class TimestampedGameSession : java.lang.Object
    {
        public const long JOIN_MAX_TIME = 180000L;
        public const long END_GAME_MIN_TIME = 60000L;
        public const int PARTICIPANT_UNKNOWN = 0;
        public const int PARTICIPANT_JOINED = 1;
        public const int PARTICIPANT_FREE_QUIT = 2;
        public const int PARTICIPANT_QUIT = 3;
        public const int PARTICIPANT_LOST = 4;
        public const int PARTICIPANT_WON = 5;
        public static string[] PARTICIPANT_DEBUG_CHARS;
        public const int TEAM_UNKNOWN = 0;
        public const int TEAM_QUIT = 1;
        public const int TEAM_LOST = 2;
        public const int TEAM_WON = 3;
        public const int GAME_STARTING = 1;
        public const int GAME_ALL_JOINED = 2;
        public const int GAME_INVALID = 3;
        public const float STATUS_WINNING_FACTOR = 2.0f;
        public const int STATUS_WINNING_TICK = 10000;
        public long create_timestamp;
        public int[] participant_state;
        public com.oddlabs.matchmaking.GameSession session;
        public int database_id;
        public int game_state;
        public long start_timestamp;
        public bool free_quit;
        public int[] last_status;
        public int last_tick;
        public bool game_ended;
        public bool all_5_wins;
        public int[] player_ratings;
        public java.io.File commandEventFile;
        public java.io.DataOutputStream commandEventStream;
        public sbyte[] worldParamsData;
        public java.io.FileWriter spectatorFileWriter;
        public java.util.Set spectatorTicksWritten;
        public bool spectatorFileChecked;
        // Java: 96
        public string getParticipantStates()
        {
            string result = "";
            for (int i = 0; i < participant_state.Length; i++)
            {
                result += java.lang.JStr.valueOf(com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_DEBUG_CHARS[participant_state[i]]);
            }
            return result;
        }
        // Java: 104
        public com.oddlabs.matchmaking.GameSession getSession()
        {
            return session;
        }
        // Java: 108
        public int getDatabaseID()
        {
            return database_id;
        }
        // Java: 112
        public bool join(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.matchserver.Client client)
        {
            if (!free_quit || game_state != com.oddlabs.matchserver.TimestampedGameSession.GAME_STARTING || java.lang.JSystem.currentTimeMillis() - create_timestamp > com.oddlabs.matchserver.TimestampedGameSession.JOIN_MAX_TIME)
            {
                return false;
            }
            int index = findIndex(server, client);
            if (index != (-1) && participant_state[index] == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_UNKNOWN)
            {
                com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
                participant_state[index] = com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_JOINED;
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ": joined " + java.lang.JStr.valueOf(getParticipantStates()));
                int num_joined = 0;
                for (int i = 0; i < participant_state.Length; i++)
                {
                    if (participant_state[i] != com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_UNKNOWN)
                    {
                        num_joined++;
                    }
                }
                if (num_joined == participant_state.Length)
                {
                    start_timestamp = java.lang.JSystem.currentTimeMillis();
                    game_state = com.oddlabs.matchserver.TimestampedGameSession.GAME_ALL_JOINED;
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ": all joined game " + java.lang.JStr.valueOf(getParticipantStates()));
                    com.oddlabs.matchserver.discord.DiscordEmbedCreator.SendGameStartedDiscordEmbed(session, database_id);
                    all_5_wins = true;
                    player_ratings = new int[participants.Length];
                    for (int i_2 = 0; i_2 < participants.Length; i_2++)
                    {
                        string nick = participants[i_2].getNick();
                        try
                        {
                            if (com.oddlabs.matchserver.DBInterface.getWins(nick) < com.oddlabs.matchmaking.GameSession.MIN_WINS_FOR_RANKING)
                            {
                                all_5_wins = false;
                            }
                        }
                        catch (System.Exception e)
                        {
                            all_5_wins = false;
                            return true;
                        }
                        try
                        {
                            player_ratings[i_2] = com.oddlabs.matchserver.DBInterface.getRating(participants[i_2].getNick());
                        }
                        catch (System.Exception e_2)
                        {
                            throw new System.Exception("Could you find rating for nick=" + java.lang.JStr.valueOf(nick) + " e=" + java.lang.JStr.valueOf(e_2));
                        }
                    }
                }
                return true;
            }
            return false;
        }
        // Java: 158
        public long getStartTime()
        {
            return start_timestamp;
        }
        // Java: 162
        public void freeQuitStop()
        {
            free_quit = false;
        }
        // Java: 166
        public void updateGameStatus(int tick, int[] status)
        {
            if (status.Length != session.getParticipants().Length || game_state != com.oddlabs.matchserver.TimestampedGameSession.GAME_ALL_JOINED)
            {
                return;
            }
            if (last_status != null)
            {
                if (tick > last_tick)
                {
                    last_tick = tick;
                    last_status = status;
                    com.oddlabs.matchserver.DBInterface.saveGameReport(database_id, tick, getTeamScores(status));
                }
                else if (tick == last_tick)
                {
                    for (int i = 0; i < status.Length; i++)
                    {
                        if (last_status[i] != status[i])
                        {
                            last_status = null;
                            return;
                        }
                    }
                }
            }
        }
        // Java: 184
        public void updateSpectatorInfo(int tick, string info)
        {
            if (spectatorFileWriter == null)
            {
                if (!spectatorFileChecked)
                {
                    spectatorFileChecked = true;
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Spectator file writer not initialized for game " + (database_id));
                }
                return;
            }
            try
            {
                if (!spectatorTicksWritten.contains(tick))
                {
                    spectatorFileWriter.write(info);
                    spectatorFileWriter.flush();
                    spectatorTicksWritten.add(tick);
                }
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Error writing spectator data for game " + (database_id) + ": " + java.lang.JStr.valueOf(e.getMessage()));
            }
        }
        // Java: 204
        public void updateWorldParams(sbyte[] data)
        {
            this.worldParamsData = data;
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ": world params stored (" + (data.Length) + " bytes)");
        }
        // Java: 209
        public sbyte[] getWorldParamsData()
        {
            return worldParamsData;
        }
        // Java: 213
        public int getLastTick()
        {
            return last_tick;
        }
        // Java: 217
        public void updateCommandEvent(int tick, int client_id, short event_size, sbyte[] event_data)
        {
            if (commandEventStream == null)
            {
                return;
            }
            if (event_size <= 0 || event_size > event_data.Length)
            {
                return;
            }
            if (tick > last_tick)
            {
                last_tick = tick;
            }
            try
            {
                commandEventStream.writeInt(tick);
                commandEventStream.writeInt(client_id);
                commandEventStream.writeShort(event_size);
                commandEventStream.write(event_data, 0, event_size);
                commandEventStream.flush();
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Error writing command event for game " + (database_id) + ": " + java.lang.JStr.valueOf(e.getMessage()));
            }
        }
        // Java: 233
        public sbyte[] readEventLog()
        {
            if (commandEventFile == null || !commandEventFile.exists())
            {
                return new sbyte[0];
            }
            try
            {
                if (commandEventStream != null)
                {
                    commandEventStream.flush();
                }
                {
                    java.io.FileInputStream fis = new java.io.FileInputStream(commandEventFile);
                    try
                    {
                        return fis.readAllBytes();
                    }
                    finally
                    {
                        if (fis != null) fis.close();
                    }
                }
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Error reading event log for game " + (database_id) + ": " + java.lang.JStr.valueOf(e.getMessage()));
                return new sbyte[0];
            }
        }
        // Java: 247
        public int getWinningTeamFromLastStatus()
        {
            if (last_status != null && last_tick > com.oddlabs.matchserver.TimestampedGameSession.STATUS_WINNING_TICK)
            {
                int[] team_score = getTeamScores(last_status);
                int best_team = (-1);
                int best_score = 1;
                int next_score = 1;
                for (int i = 0; i < team_score.Length; i++)
                {
                    if (team_score[i] > best_score)
                    {
                        next_score = best_score;
                        best_score = team_score[i];
                        best_team = i;
                    }
                    else if (team_score[i] > next_score)
                    {
                        next_score = team_score[i];
                    }
                }
                float factor = (float)(best_score / ((float)(next_score)));
                if (factor >= com.oddlabs.matchserver.TimestampedGameSession.STATUS_WINNING_FACTOR)
                {
                    return best_team;
                }
            }
            return (-1);
        }
        // Java: 270
        public int[] getTeamScores(int[] status)
        {
            int[] team_score = new int[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            for (int i = 0; i < participants.Length; i++)
            {
                team_score[participants[i].getTeam()] += status[i];
            }
            return team_score;
        }
        // Java: 278
        public void participantQuit(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.matchserver.Client client)
        {
            if (!free_quit)
            {
                return;
            }
            int index = findIndex(server, client);
            if (index == (-1))
            {
                return;
            }
            if (participant_state[index] == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_JOINED)
            {
                participant_state[index] = com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_FREE_QUIT;
            }
        }
        // Java: 290
        public void gameQuit(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.matchserver.Client client)
        {
            int index = findIndex(server, client);
            if (!free_quit && !(participant_state[index] == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_FREE_QUIT))
            {
                game_state = com.oddlabs.matchserver.TimestampedGameSession.GAME_INVALID;
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + " is now invalid. " + java.lang.JStr.valueOf(client.getUsername()) + " tried to free_quit. " + java.lang.JStr.valueOf(getParticipantStates()));
            }
            gameDone(server, client, com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_QUIT, "quit");
        }
        // Java: 301
        public void gameLost(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.matchserver.Client client)
        {
            gameDone(server, client, com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_LOST, "lost");
        }
        // Java: 305
        public void gameWon(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.matchserver.Client client)
        {
            int index = findIndex(server, client);
            if (participant_state[index] == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_FREE_QUIT)
            {
                game_state = com.oddlabs.matchserver.TimestampedGameSession.GAME_INVALID;
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + " is now invalid. " + java.lang.JStr.valueOf(client.getUsername()) + " tried to win while having free_quit. " + java.lang.JStr.valueOf(getParticipantStates()));
            }
            gameDone(server, client, com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_WON, "won");
        }
        // Java: 315
        public int findIndex(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.matchserver.Client client)
        {
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            for (int i = 0; i < participants.Length; i++)
            {
                com.oddlabs.matchserver.Client search_client = server.getClientFromID(participants[i].getMatchID());
                if (((object)search_client == (object)client))
                {
                    return i;
                }
            }
            return (-1);
        }
        // Java: 325
        public void gameDone(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.matchserver.Client client, int result, string result_string)
        {
            participant_state[findIndex(server, client)] = result;
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ": " + java.lang.JStr.valueOf(client.getUsername()) + " finished. Result " + java.lang.JStr.valueOf(result_string) + " " + java.lang.JStr.valueOf(getParticipantStates()));
            evaluateGame(server);
        }
        // Java: 333
        public void evaluateGame(com.oddlabs.matchserver.MatchmakingServer server)
        {
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            int[] team_sizes = new int[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            int[] team_done = new int[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            int[] team_result = new int[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            for (int i = 0; i < participants.Length; i++)
            {
                int team = participants[i].getTeam();
                int state = participant_state[i];
                team_sizes[team]++;
                if (state == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_QUIT || state == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_LOST || state == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_WON)
                {
                    team_done[team]++;
                    if (team_result[team] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_UNKNOWN && state == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_QUIT)
                    {
                        team_result[team] = com.oddlabs.matchserver.TimestampedGameSession.TEAM_QUIT;
                    }
                    if ((team_result[team] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_UNKNOWN || team_result[team] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_QUIT) && state == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_LOST)
                    {
                        team_result[team] = com.oddlabs.matchserver.TimestampedGameSession.TEAM_LOST;
                    }
                    if (state == com.oddlabs.matchserver.TimestampedGameSession.PARTICIPANT_WON)
                    {
                        team_result[team] = com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON;
                    }
                }
            }
            int winning_teams = 0;
            int winning_team_index = (-1);
            bool teams_lost = false;
            for (int i_2 = 0; i_2 < team_sizes.Length; i_2++)
            {
                if (team_sizes[i_2] == team_done[i_2])
                {
                    if (team_result[i_2] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON)
                    {
                        winning_teams++;
                        winning_team_index = i_2;
                    }
                    else if (team_result[i_2] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_LOST)
                    {
                        teams_lost = true;
                    }
                }
                else
                {
                    return;
                }
            }
            long end_time = java.lang.JSystem.currentTimeMillis();
            if (winning_teams == 0)
            {
                int aiOnlyTeamsMask = findAiOnlyTeams();
                for (int team_2 = 0; team_2 < team_result.Length; team_2++)
                {
                    if ((aiOnlyTeamsMask & (1 << team_2)) != 0)
                    {
                        team_result[team_2] = com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON;
                    }
                }
                updateOpenSkillRatings(server, team_result);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". No human team won; AI teams tie for first place. " + java.lang.JStr.valueOf(getParticipantStates()));
                com.oddlabs.matchserver.DBInterface.endGame(this, end_time, (-1));
                com.oddlabs.matchserver.discord.DiscordEmbedCreator.SendHumansLoseToBotsDiscordEmbed(session, database_id);
                game_ended = true;
                closeSpectatorStreams();
                return;
            }
            if (winning_teams > 1 || game_state == com.oddlabs.matchserver.TimestampedGameSession.GAME_INVALID)
            {
                winning_team_index = getWinningTeamFromLastStatus();
                if (winning_team_index != (-1))
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". Team " + ((winning_team_index + 1)) + " won from status reports. " + java.lang.JStr.valueOf(getParticipantStates()));
                    teams_lost = true;
                    for (int i_3 = 0; i_3 < team_result.Length; i_3++)
                    {
                        if (i_3 == winning_team_index)
                        {
                            team_result[i_3] = com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON;
                        }
                        else
                        {
                            team_result[i_3] = com.oddlabs.matchserver.TimestampedGameSession.TEAM_LOST;
                        }
                    }
                }
                else
                {
                    for (int i_4 = 0; i_4 < participants.Length; i_4++)
                    {
                        string nick = participants[i_4].getNick();
                        com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + ". " + java.lang.JStr.valueOf(nick) + " ended invalid game " + java.lang.JStr.valueOf(getParticipantStates()));
                        com.oddlabs.matchserver.DBInterface.increaseInvalidGames(nick);
                        com.oddlabs.matchserver.Client client = server.getClientFromID(participants[i_4].getMatchID());
                        if (client != null)
                        {
                            client.updateProfile();
                        }
                    }
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + " was invalid. " + (winning_teams) + " winning teams. " + java.lang.JStr.valueOf(getParticipantStates()));
                    com.oddlabs.matchserver.DBInterface.endGame(this, end_time, (-1));
                    com.oddlabs.matchserver.discord.DiscordEmbedCreator.SendInvalidatedGameDiscordEmbed(session, database_id);
                    game_ended = true;
                    closeSpectatorStreams();
                    return;
                }
            }
            updateOpenSkillRatings(server, team_result);
            if (teams_lost)
            {
                teamWon(server, team_result);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". Team " + ((winning_team_index + 1)) + " won (humans vs humans). " + java.lang.JStr.valueOf(getParticipantStates()));
                com.oddlabs.matchserver.DBInterface.endGame(this, end_time, winning_team_index);
                com.oddlabs.matchserver.discord.DiscordEmbedCreator.SendHumansWinAgainstOtherHumans(winning_team_index, session, database_id);
            }
            else
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". Team " + ((winning_team_index + 1)) + " won (humans vs bots). " + java.lang.JStr.valueOf(getParticipantStates()));
                com.oddlabs.matchserver.DBInterface.endGame(this, end_time, (-1));
                com.oddlabs.matchserver.discord.DiscordEmbedCreator.SendHumansWinAgainstBotsDiscordEmbed(winning_team_index, session, database_id);
            }
            game_ended = true;
            closeSpectatorStreams();
        }
        // Java: 442
        public int findAiOnlyTeams()
        {
            int humanTeamsMask = 0;
            int aiTeamsMask = 0;
            foreach (com.oddlabs.matchmaking.GamePlayer player in session.getPlayerInfo())
            {
                int bit = 1 << player.getTeam();
                if (((object)player.getPlayerType() == (object)com.oddlabs.matchmaking.PlayerTypes.Human))
                {
                    humanTeamsMask |= bit;
                }
                else
                {
                    aiTeamsMask |= bit;
                }
            }
            return aiTeamsMask & ~humanTeamsMask;
        }
        // Java: 456
        public void updateOpenSkillRatings(com.oddlabs.matchserver.MatchmakingServer server, int[] team_result)
        {
            com.oddlabs.matchserver.TimestampedGameSession.TransformTeamResultToRateGameFormatResult rateGameInput = transformTeamResultToRateGameFormat(team_result);
            if (rateGameInput == null)
            {
                return;
            }
            com.oddlabs.matchserver.OpenSkillRatingSystem.rateGame(rateGameInput.teams(), rateGameInput.ranks());
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            foreach (com.oddlabs.matchmaking.Participant participant in participants)
            {
                com.oddlabs.matchserver.Client client = server.getClientFromID(participant.getMatchID());
                if (client != null)
                {
                    client.updateProfile();
                }
            }
        }
        // Java: 475
        public class TransformTeamResultToRateGameFormatResult : java.lang.Object
        {
            public java.util.List teams() { return teams_f; }
            public int[] ranks() { return ranks_f; }
            public TransformTeamResultToRateGameFormatResult(java.util.List teams, int[] ranks)
            {
                this.teams_f = teams;
                this.ranks_f = ranks;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is TransformTeamResultToRateGameFormatResult r)) return false; return true && java.lang.JO.equals(teams_f, r.teams_f) && java.lang.JO.equals(ranks_f, r.ranks_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(teams_f); h = 31 * h + java.lang.JO.hashCode(ranks_f); } return h; }
            public override string toString() { return "TransformTeamResultToRateGameFormatResult[" + "teams=" + java.lang.JO.toString(teams_f) + ", ranks=" + java.lang.JO.toString(ranks_f) + "]"; }
            public java.util.List teams_f;
            public int[] ranks_f;
        }
        // Java: 481
        public com.oddlabs.matchserver.TimestampedGameSession.TransformTeamResultToRateGameFormatResult transformTeamResultToRateGameFormat(int[] team_result)
        {
            java.util.Map teamsByIndex = new java.util.LinkedHashMap();
            com.oddlabs.matchmaking.GamePlayer[] playerInfo = session.getPlayerInfo();
            foreach (com.oddlabs.matchmaking.GamePlayer player in playerInfo)
            {
                ((java.util.List)teamsByIndex.computeIfAbsent(player.getTeam(), new java.util.function.Function_L((object k_l) =>
                {
                    int? k = (int?)k_l;
                    return new java.util.ArrayList();
                }))).add(player.getNick());
            }
            java.util.List teams = new java.util.ArrayList();
            int[] ranks = new int[teamsByIndex.size()];
            java.util.Arrays.fill(ranks, 1);
            int i = 0;
            foreach (java.util.Map.Entry entry in java.lang.JIter.of(teamsByIndex.entrySet()))
            {
                teams.add(((java.util.List)entry.getValue()));
                if (team_result[((int)((int?)entry.getKey()))] != com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON)
                {
                    ranks[i] = 2;
                }
                i++;
            }
            if (teams.size() < 2)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ": fewer than 2 non-empty teams, skipping OpenSkill rating");
                return null;
            }
            return new com.oddlabs.matchserver.TimestampedGameSession.TransformTeamResultToRateGameFormatResult(teams, ranks);
        }
        // Java: 515
        public void closeSpectatorStreams()
        {
            try
            {
                if (commandEventStream != null)
                {
                    commandEventStream.close();
                }
                if (spectatorFileWriter != null)
                {
                    spectatorFileWriter.close();
                }
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Error closing spectator streams for game " + (database_id) + ": " + java.lang.JStr.valueOf(e.getMessage()));
            }
        }
        // Java: 525
        public override void finalize()
        {
            if (!game_ended)
            {
                com.oddlabs.matchserver.DBInterface.endGame(this, java.lang.JSystem.currentTimeMillis(), (-1));
            }
            closeSpectatorStreams();
        }
        // Java: 531
        public void teamWon(com.oddlabs.matchserver.MatchmakingServer server, int[] team_result)
        {
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            for (int i = 0; i < participants.Length; i++)
            {
                string nick = participants[i].getNick();
                int team = participants[i].getTeam();
                if (team_result[team] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON)
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". " + java.lang.JStr.valueOf(nick) + " won game");
                    com.oddlabs.matchserver.DBInterface.increaseWins(nick);
                }
                else if (team_result[team] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_LOST)
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". " + java.lang.JStr.valueOf(nick) + " lost game");
                    com.oddlabs.matchserver.DBInterface.increaseLosses(nick);
                }
                com.oddlabs.matchserver.Client client = server.getClientFromID(participants[i].getMatchID());
                if (client != null)
                {
                    client.updateProfile();
                }
            }
            updatePlayerStreaks(team_result);
            updateSteamAchievements(team_result);
            if (session.isRated() && all_5_wins)
            {
                rerateParticipants(server, team_result);
            }
        }
        // Java: 553
        public void updatePlayerStreaks(int[] team_result)
        {
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            for (int i = 0; i < participants.Length; i++)
            {
                string nick = participants[i].getNick();
                int team = participants[i].getTeam();
                try
                {
                    int[] streaks = com.oddlabs.matchserver.DBInterface.getStreaks(nick);
                    int currentStreak = streaks[0];
                    int bestStreak = streaks[1];
                    if (team_result[team] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON)
                    {
                        currentStreak++;
                        if (currentStreak > bestStreak)
                        {
                            bestStreak = currentStreak;
                        }
                    }
                    else if (team_result[team] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_LOST)
                    {
                        currentStreak = 0;
                    }
                    com.oddlabs.matchserver.DBInterface.updateStreaks(nick, currentStreak, bestStreak);
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". Updated streaks for " + java.lang.JStr.valueOf(nick) + " (currentStreak=" + (currentStreak) + ", bestStreak=" + (bestStreak) + ")");
                }
                catch (System.Exception e)
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + ". SQLException while updating streaks for " + java.lang.JStr.valueOf(nick) + ": " + java.lang.JStr.valueOf(e.getMessage()));
                }
            }
        }
        // Java: 583
        public void updateSteamAchievements(int[] team_result)
        {
            if (!com.oddlabs.matchserver.ServerConfiguration.getInstance().isSteamStatsConfigured())
            {
                return;
            }
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            for (int i = 0; i < participants.Length; i++)
            {
                string nick = participants[i].getNick();
                long? accountId = com.oddlabs.matchserver.DBInterface.getSteamIdByNick(nick);
                if (accountId == null)
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + ". No Steam ID linked for " + java.lang.JStr.valueOf(nick) + ", skipping Steam stats push");
                    continue;
                }
                try
                {
                    int totalWins = com.oddlabs.matchserver.DBInterface.getWins(nick);
                    int totalLosses = com.oddlabs.matchserver.DBInterface.getIntField("losses", nick);
                    int[] streaks = com.oddlabs.matchserver.DBInterface.getStreaks(nick);
                    long steamId64 = ((long)accountId) + com.oddlabs.matchserver.SteamAuthValidator.STEAM_ID_BASE;
                    bool success = com.oddlabs.matchserver.SteamAchievementService.updatePlayerStats(steamId64, totalWins, totalLosses, streaks[0], streaks[1]);
                    if (success)
                    {
                        com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". Updated Steam achievements for " + java.lang.JStr.valueOf(nick));
                    }
                    else
                    {
                        com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + ". Failed to update Steam achievements for " + java.lang.JStr.valueOf(nick));
                    }
                }
                catch (System.Exception e)
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + ". SQLException while reading stats for Steam achievements for " + java.lang.JStr.valueOf(nick) + ": " + java.lang.JStr.valueOf(e.getMessage()));
                }
            }
        }
        // Java: 627
        public void rerateParticipants(com.oddlabs.matchserver.MatchmakingServer server, int[] team_result)
        {
            com.oddlabs.matchmaking.Participant[] participants = session.getParticipants();
            int[] player_teams = new int[participants.Length];
            for (int i = 0; i < participants.Length; i++)
            {
                int team = participants[i].getTeam();
                player_teams[i] = team;
            }
            int[][] points = com.oddlabs.matchmaking.GameSession.calculateMatchPoints(player_ratings, player_teams);
            for (int i_2 = 0; i_2 < participants.Length; i_2++)
            {
                string nick = participants[i_2].getNick();
                int dpoints = default;
                if (team_result[player_teams[i_2]] == com.oddlabs.matchserver.TimestampedGameSession.TEAM_WON)
                {
                    dpoints = points[i_2][com.oddlabs.matchmaking.GameSession.WIN];
                }
                else
                {
                    dpoints = points[i_2][com.oddlabs.matchmaking.GameSession.LOSE];
                }
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ". " + java.lang.JStr.valueOf(nick) + " rating change was " + (dpoints));
                com.oddlabs.matchserver.DBInterface.updateRating(participants[i_2].getNick(), dpoints);
                com.oddlabs.matchserver.Client client = server.getClientFromID(participants[i_2].getMatchID());
                if (client != null)
                {
                    client.updateProfile();
                }
            }
        }
        static TimestampedGameSession()
        {
            TimestampedGameSession.PARTICIPANT_DEBUG_CHARS = new string[] { "U", "J", "F", "Q", "L", "W" };
        }
        private void init_TimestampedGameSession()
        {
            this.game_state = com.oddlabs.matchserver.TimestampedGameSession.GAME_STARTING;
            this.free_quit = true;
            this.last_tick = (-1);
            this.spectatorTicksWritten = new java.util.HashSet();
        }
        public TimestampedGameSession(com.oddlabs.matchmaking.GameSession session, int database_id) : base()
        {
            init_TimestampedGameSession();
            this.session = session;
            this.database_id = database_id;
            int num_participants = session.getParticipants().Length;
            participant_state = new int[num_participants];
            last_status = new int[num_participants];
            this.create_timestamp = java.lang.JSystem.currentTimeMillis();
            string nicks = " ";
            for (int i = 0; i < num_participants; i++)
            {
                nicks += java.lang.JStr.valueOf(session.getParticipants()[i].getNick()) + " ";
            }
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + " created. [" + java.lang.JStr.valueOf(nicks) + "] " + java.lang.JStr.valueOf(getParticipantStates()));
            try
            {
                string dirPath = com.oddlabs.matchserver.ServerConfiguration.getInstance().get(com.oddlabs.matchserver.ServerConfiguration.SPECTATOR_DATA_DIR);
                if (dirPath == null || dirPath.isEmpty())
                {
                    dirPath = "/var/games";
                }
                java.io.File spectatorDir = new java.io.File(dirPath);
                if (!spectatorDir.exists())
                {
                    spectatorDir.mkdirs();
                }
                spectatorFileWriter = new java.io.FileWriter(new java.io.File(spectatorDir, java.lang.JString.valueOf(database_id)));
                commandEventFile = new java.io.File(spectatorDir, (database_id) + ".events");
                commandEventStream = new java.io.DataOutputStream(new java.io.FileOutputStream(commandEventFile));
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Failed to create spectator files for game " + (database_id) + ": " + java.lang.JStr.valueOf(e.getMessage()));
            }
        }
    }
}
