// Translated from DBInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 39
    public class DBInterface : java.lang.Object
    {
        // Java: 41
        public class Registration : java.lang.Object
        {
            public int id;
            public string username;
            public string password;
            public string email;
            public string lastUsedProfile;
            public bool banned;
            public long? steamId;
            public Registration(int id, string username, string password, string email) : base()
            {
                this.id = id;
                this.username = username;
                this.password = password;
                this.email = email;
            }
        }
        // Java: 58
        public class ProfileRow : java.lang.Object
        {
            public int regId;
            public string nick;
            public int rating;
            public int wins;
            public int losses;
            public int invalid;
            public int currentStreak;
            public int bestStreak;
            // Java: 68
            public com.oddlabs.matchmaking.Profile toProfile(int revision)
            {
                return new com.oddlabs.matchmaking.Profile(nick, rating, wins, losses, invalid, revision);
            }
            private void init_ProfileRow()
            {
                this.rating = 1000;
            }
            public ProfileRow(int regId, string nick) : base()
            {
                init_ProfileRow();
                this.regId = regId;
                this.nick = nick;
            }
        }
        // Java: 73
        public class GameRow : java.lang.Object
        {
            public com.oddlabs.matchserver.models.GameDataModel data;
            public java.util.List players;
            private void init_GameRow()
            {
                this.data = new com.oddlabs.matchserver.models.GameDataModel();
                this.players = new java.util.ArrayList();
            }
            public GameRow() : base()
            {
                init_GameRow();
            }
        }
        public static object LOCK;
        public static bool AUTO_REGISTER;
        public static java.util.Map SETTINGS;
        public static java.util.Map registrations;
        public static java.util.Map profiles;
        public static java.util.Map openSkill;
        public static java.util.Map games;
        public static java.util.Map onlineProfiles;
        public static java.util.Map discordLinks;
        public static int nextRegId;
        public static int nextGameId;
        // Java: 98
        public static void log(string s)
        {
            com.oddlabs.matchserver.MatchmakingServer.getLogger().fine("[memdb] " + java.lang.JStr.valueOf(s));
        }
        // Java: 102
        public static com.oddlabs.matchserver.DBInterface.Registration reg(string username)
        {
            return (username == null ? (com.oddlabs.matchserver.DBInterface.Registration)(null) : ((com.oddlabs.matchserver.DBInterface.Registration)com.oddlabs.matchserver.DBInterface.registrations.get(username.toLowerCase())));
        }
        // Java: 106
        public static com.oddlabs.matchserver.DBInterface.Registration ensureReg(string username, string password, string email)
        {
            com.oddlabs.matchserver.DBInterface.Registration r = com.oddlabs.matchserver.DBInterface.reg(username);
            if (r == null)
            {
                r = new com.oddlabs.matchserver.DBInterface.Registration(com.oddlabs.matchserver.DBInterface.nextRegId++, username, password, email);
                com.oddlabs.matchserver.DBInterface.registrations.put(username.toLowerCase(), r);
                com.oddlabs.matchserver.DBInterface.log("registered " + java.lang.JStr.valueOf(username));
            }
            return r;
        }
        // Java: 116
        public static com.oddlabs.matchserver.DBInterface.ProfileRow prof(string nick)
        {
            return (nick == null ? (com.oddlabs.matchserver.DBInterface.ProfileRow)(null) : ((com.oddlabs.matchserver.DBInterface.ProfileRow)com.oddlabs.matchserver.DBInterface.profiles.get(nick.toLowerCase())));
        }
        // Java: 122
        public static bool usernameExists(string username)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return com.oddlabs.matchserver.DBInterface.reg(username) != null;
            }
        }
        // Java: 128
        public static void createUser(com.oddlabs.matchmaking.Login login, com.oddlabs.matchmaking.LoginDetails login_details, string reg_key)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ensureReg(login.getUsername(), login.getPasswordDigest(), (login_details != null ? login_details.getEmail() : (string)(null)));
            }
        }
        // Java: 135
        public static bool queryUser(string username, string password)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.Registration r = com.oddlabs.matchserver.DBInterface.reg(username);
                if (r == null)
                {
                    if (!com.oddlabs.matchserver.DBInterface.AUTO_REGISTER || password == null)
                    {
                        return false;
                    }
                    com.oddlabs.matchserver.DBInterface.ensureReg(username, password, null);
                    return true;
                }
                return !r.banned && password != null && java.lang.JO.equals(password, r.password);
            }
        }
        // Java: 148
        public static com.oddlabs.matchmaking.Profile[] getProfiles(string username, int revision)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.Registration r = com.oddlabs.matchserver.DBInterface.reg(username);
                java.util.List @out = new java.util.ArrayList();
                if (r != null)
                {
                    foreach (com.oddlabs.matchserver.DBInterface.ProfileRow p in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.profiles.values()))
                    {
                        if (p.regId == r.id)
                        {
                            @out.add(p.toProfile(revision));
                        }
                    }
                }
                return ((com.oddlabs.matchmaking.Profile[])@out.toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.matchmaking.Profile[a0]; })));
            }
        }
        // Java: 160
        public static com.oddlabs.matchmaking.Profile getProfile(string username, string nick, int revision)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.Registration r = com.oddlabs.matchserver.DBInterface.reg(username);
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                if (r == null || p == null || p.regId != r.id || !java.lang.JO.equals(p.nick, nick))
                {
                    return null;
                }
                return p.toProfile(revision);
            }
        }
        // Java: 170
        public static void setLastUsedProfile(string username, string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.Registration r = com.oddlabs.matchserver.DBInterface.reg(username);
                if (r != null)
                {
                    r.lastUsedProfile = nick;
                }
            }
        }
        // Java: 178
        public static string getLastUsedProfile(string username)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.Registration r = com.oddlabs.matchserver.DBInterface.reg(username);
                return (r == null ? (string)(null) : r.lastUsedProfile);
            }
        }
        // Java: 185
        public static bool nickExists(string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return com.oddlabs.matchserver.DBInterface.prof(nick) != null;
            }
        }
        public static java.util.Map bannedWords;
        // Java: 195
        public static java.util.List getBannedWords()
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                java.util.List @out = new java.util.ArrayList();
                com.oddlabs.matchserver.DBInterface.bannedWords.forEach(new java.util.function.BiConsumer_L((object w_l, object t_l) =>
                {
                    string w = (string)w_l;
                    string t = (string)t_l;
                    @out.add(new string[] { w, t });
                }));
                return @out;
            }
        }
        // Java: 203
        public static bool addBannedWord(string word, string match_type)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return ((string)com.oddlabs.matchserver.DBInterface.bannedWords.putIfAbsent(word, match_type)) == null;
            }
        }
        // Java: 209
        public static bool removeBannedWord(string word)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return ((string)com.oddlabs.matchserver.DBInterface.bannedWords.remove(word)) != null;
            }
        }
        // Java: 215
        public static bool setBannedByNick(string nick, bool banned)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                if (p == null)
                {
                    return false;
                }
                foreach (com.oddlabs.matchserver.DBInterface.Registration r in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.registrations.values()))
                {
                    if (r.id == p.regId)
                    {
                        r.banned = banned;
                        return true;
                    }
                }
                return false;
            }
        }
        // Java: 231
        public static void saveGameReport(int game_id, int tick, int[] team_score)
        {
            com.oddlabs.matchserver.DBInterface.log("saveGameReport game=" + (game_id) + " tick=" + (tick) + " scores=" + java.lang.JStr.valueOf(java.util.Arrays.toString(team_score)));
        }
        // Java: 235
        public static void logPriority(int game_id, string nick1, string nick2, int priority)
        {
            com.oddlabs.matchserver.DBInterface.log("logPriority game=" + (game_id) + " " + java.lang.JStr.valueOf(nick1) + "->" + java.lang.JStr.valueOf(nick2) + " = " + (priority));
        }
        // Java: 241
        public static void createProfile(string username, string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.Registration r = com.oddlabs.matchserver.DBInterface.ensureReg(username, null, null);
                com.oddlabs.matchserver.DBInterface.profiles.put(nick.toLowerCase(), new com.oddlabs.matchserver.DBInterface.ProfileRow(r.id, nick));
                com.oddlabs.matchserver.DBInterface.openSkill.put(nick, new com.oddlabs.matchmaking.OpenSkillRating(nick, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_MU, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_SIGMA));
                com.oddlabs.matchserver.DBInterface.log("createProfile " + java.lang.JStr.valueOf(username) + "/" + java.lang.JStr.valueOf(nick));
            }
        }
        // Java: 251
        public static void deleteProfile(string username, string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                if (com.oddlabs.matchserver.DBInterface.getProfile(username, nick, (-1)) != null)
                {
                    com.oddlabs.matchserver.DBInterface.profiles.remove(nick.toLowerCase());
                    com.oddlabs.matchserver.DBInterface.openSkill.remove(nick);
                }
            }
        }
        // Java: 262
        public static string getOrCreateSteamRegistration(long steamId)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                foreach (com.oddlabs.matchserver.DBInterface.Registration r in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.registrations.values()))
                {
                    if (r.steamId != null && ((long)r.steamId) == steamId)
                    {
                        return (r.banned ? (string)(null) : r.username);
                    }
                }
                com.oddlabs.matchserver.DBInterface.Registration r_2 = com.oddlabs.matchserver.DBInterface.ensureReg("steam_" + (steamId), "LOCKED", "steam_" + (steamId) + "@steam.internal");
                r_2.steamId = steamId;
                return r_2.username;
            }
        }
        // Java: 273
        public static string getProfileNickBySteamId(long steamId)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                foreach (com.oddlabs.matchserver.DBInterface.Registration r in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.registrations.values()))
                {
                    if (r.steamId != null && ((long)r.steamId) == steamId)
                    {
                        foreach (com.oddlabs.matchserver.DBInterface.ProfileRow p in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.profiles.values()))
                        {
                            if (p.regId == r.id)
                            {
                                return p.nick;
                            }
                        }
                    }
                }
                return null;
            }
        }
        // Java: 284
        public static long? getSteamIdByNick(string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                if (p == null)
                {
                    return null;
                }
                foreach (com.oddlabs.matchserver.DBInterface.Registration r in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.registrations.values()))
                {
                    if (r.id == p.regId)
                    {
                        return r.steamId;
                    }
                }
                return null;
            }
        }
        // Java: 298
        public static void updateStreaks(string nick, int currentStreak, int bestStreak)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                if (p != null)
                {
                    p.currentStreak = currentStreak;
                    p.bestStreak = bestStreak;
                }
            }
        }
        // Java: 308
        public static int[] getStreaks(string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                return (p == null ? new int[] { 0, 0 } : new int[] { p.currentStreak, p.bestStreak });
            }
        }
        // Java: 315
        public static void increaseLosses(string nick)
        {
            com.oddlabs.matchserver.DBInterface.increaseField("losses", nick);
        }
        // Java: 319
        public static void increaseWins(string nick)
        {
            com.oddlabs.matchserver.DBInterface.increaseField("wins", nick);
        }
        // Java: 323
        public static void increaseInvalidGames(string nick)
        {
            com.oddlabs.matchserver.DBInterface.increaseField("invalid", nick);
        }
        // Java: 327
        public static void increaseField(string field, string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                if (p == null)
                {
                    return;
                }
                switch ((field))
                {
                    case "wins":
                    {
                        p.wins++;
                        break;
                    }
                    case "losses":
                    {
                        p.losses++;
                        break;
                    }
                    case "invalid":
                    {
                        p.invalid++;
                        break;
                    }
                    case "rating":
                    {
                        p.rating++;
                        break;
                    }
                    default:
                    {
                        com.oddlabs.matchserver.DBInterface.log("increaseField: unknown field " + java.lang.JStr.valueOf(field));
                        break;
                    }
                }
            }
        }
        // Java: 342
        public static void updateRating(string nick, int rating_delta)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                if (p != null)
                {
                    p.rating += rating_delta;
                }
            }
        }
        // Java: 350
        public static int getWins(string nick)
        {
            return com.oddlabs.matchserver.DBInterface.getIntField("wins", nick);
        }
        // Java: 354
        public static int getRating(string nick)
        {
            return com.oddlabs.matchserver.DBInterface.getIntField("rating", nick);
        }
        // Java: 358
        public static int getIntField(string int_field, string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(nick);
                if (p == null)
                {
                    throw new System.Exception("no profile " + java.lang.JStr.valueOf(nick));
                }
                return ((int_field) switch { "wins" => (int)(p.wins), "losses" => (int)(p.losses), "invalid" => (int)(p.invalid), "rating" => (int)(p.rating), "current_win_streak" => (int)(p.currentStreak), "best_win_streak" => (int)(p.bestStreak), _ => throw new System.Exception("unknown field " + java.lang.JStr.valueOf(int_field)) });
            }
        }
        // Java: 377
        public static string getSetting(string setting)
        {
            string @override = java.lang.JSystem.getProperty("srvtest.setting." + java.lang.JStr.valueOf(setting));
            if (@override != null)
            {
                return @override;
            }
            string v = ((string)com.oddlabs.matchserver.DBInterface.SETTINGS.get(setting));
            if (v == null)
            {
                throw new System.Exception("Unknown setting (add a default in the test DBInterface): " + java.lang.JStr.valueOf(setting));
            }
            return v;
        }
        // Java: 387
        public static int getSettingsInt(string setting)
        {
            return java.lang.Integer.parseInt(com.oddlabs.matchserver.DBInterface.getSetting(setting).trim());
        }
        // Java: 393
        public static com.oddlabs.matchmaking.RankingEntry[] getRankings(string nick, int radius)
        {
            return new com.oddlabs.matchmaking.RankingEntry[0];
        }
        // Java: 397
        public static com.oddlabs.matchmaking.RankingEntry[] getRankings(int start, int count)
        {
            return new com.oddlabs.matchmaking.RankingEntry[0];
        }
        // Java: 401
        public static com.oddlabs.matchmaking.RankingEntry[] getTopRankings(int number)
        {
            return new com.oddlabs.matchmaking.RankingEntry[0];
        }
        // Java: 407
        public static com.oddlabs.matchmaking.OpenSkillRating getOpenSkillRating(string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return ((com.oddlabs.matchmaking.OpenSkillRating)com.oddlabs.matchserver.DBInterface.openSkill.get(nick));
            }
        }
        // Java: 413
        public static void upsertOpenSkillRating(com.oddlabs.matchmaking.OpenSkillRating rating)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.openSkill.put(rating.nick(), rating);
            }
        }
        // Java: 419
        public static com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry[] getTopOpenSkillRankingEntries(int n)
        {
            return new com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry[0];
        }
        // Java: 423
        public static com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry getOpenSkillRankingEntry(string nick)
        {
            return null;
        }
        // Java: 429
        public static void createGame(com.oddlabs.matchmaking.Game game, string nick, int sim_version)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                int id = com.oddlabs.matchserver.DBInterface.nextGameId++;
                com.oddlabs.matchserver.DBInterface.GameRow g = new com.oddlabs.matchserver.DBInterface.GameRow();
                g.data.setId(id);
                g.data.setTimeCreate(new java.sql.Timestamp(java.lang.JSystem.currentTimeMillis()));
                g.data.setName(game.getName());
                g.data.setRated((game.isRated() ? "Y" : "N"));
                g.data.setSpeed(unchecked((int)(game.getGamespeed())));
                g.data.setSize(game.getSize() + 1);
                g.data.setHills(unchecked((int)(game.getHills())));
                g.data.setTrees(unchecked((int)(game.getTrees())));
                g.data.setResources(unchecked((int)(game.getSupplies())));
                g.data.setMapcode(game.getMapcode());
                g.data.setStatus("created");
                com.oddlabs.matchserver.DBInterface.games.put(id, g);
                game.setDatabaseID(id);
                com.oddlabs.matchserver.DBInterface.log("createGame id=" + (id) + " name=" + java.lang.JStr.valueOf(game.getName()) + " by " + java.lang.JStr.valueOf(nick) + " sim=" + (sim_version));
            }
        }
        // Java: 450
        public static com.oddlabs.matchserver.models.GameDataModel getGame(int game_id, bool get_player_data)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.GameRow g = ((com.oddlabs.matchserver.DBInterface.GameRow)com.oddlabs.matchserver.DBInterface.games.get(game_id));
                return (g == null ? (com.oddlabs.matchserver.models.GameDataModel)(null) : g.data);
            }
        }
        // Java: 457
        public static com.oddlabs.matchserver.models.VersusMatchupResultModel getMatchupStats(string player1, string player2, bool only1v1Matchups)
        {
            return new com.oddlabs.matchserver.models.VersusMatchupResultModel(player1, player2, 0, 0, 0, new java.util.ArrayList());
        }
        // Java: 461
        public static com.oddlabs.matchmaking.Profile[] getProfilesByNick(string[] nicks)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                java.util.List @out = new java.util.ArrayList();
                if (nicks != null)
                {
                    foreach (string n in nicks)
                    {
                        com.oddlabs.matchserver.DBInterface.ProfileRow p = com.oddlabs.matchserver.DBInterface.prof(n);
                        if (p != null)
                        {
                            @out.add(p.toProfile((-1)));
                        }
                    }
                }
                return ((com.oddlabs.matchmaking.Profile[])@out.toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.matchmaking.Profile[a0]; })));
            }
        }
        // Java: 474
        public static void initDropGames()
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                foreach (com.oddlabs.matchserver.DBInterface.GameRow g in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.games.values()))
                {
                    if (java.lang.JO.equals("created", g.data.getStatus()))
                    {
                        g.data.setStatus("dropped");
                    }
                }
            }
        }
        // Java: 482
        public static void dropGame(string nick)
        {
            com.oddlabs.matchserver.DBInterface.log("dropGame " + java.lang.JStr.valueOf(nick));
        }
        // Java: 486
        public static void startGame(com.oddlabs.matchserver.TimestampedGameSession tgs, com.oddlabs.matchserver.MatchmakingServer server)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.GameRow g = ((com.oddlabs.matchserver.DBInterface.GameRow)com.oddlabs.matchserver.DBInterface.games.get(tgs.getDatabaseID()));
                if (g == null)
                {
                    return;
                }
                g.data.setTimeStart(new java.sql.Timestamp(java.lang.JSystem.currentTimeMillis()));
                g.data.setStatus("started");
                com.oddlabs.matchmaking.GameSession session = tgs.getSession();
                foreach (com.oddlabs.matchmaking.GamePlayer p in session.getPlayerInfo())
                {
                    g.players.add(new string[] { p.getNick(), java.lang.JString.valueOf(p.getTeam()), java.lang.JString.valueOf(p.getRace()) });
                }
                com.oddlabs.matchserver.DBInterface.log("startGame id=" + (tgs.getDatabaseID()));
            }
        }
        // Java: 500
        public static void endGame(com.oddlabs.matchserver.TimestampedGameSession tgs, long end_time, int winner)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.GameRow g = ((com.oddlabs.matchserver.DBInterface.GameRow)com.oddlabs.matchserver.DBInterface.games.get(tgs.getDatabaseID()));
                if (g == null)
                {
                    return;
                }
                g.data.setTimeStop(new java.sql.Timestamp(end_time));
                g.data.setStatus("completed");
                g.data.setWinner(winner);
                com.oddlabs.matchserver.DBInterface.log("endGame id=" + (tgs.getDatabaseID()) + " winner=" + (winner));
            }
        }
        // Java: 514
        public static bool isProfileRegisteredToDiscord(string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return com.oddlabs.matchserver.DBInterface.discordLinks.containsKey(nick);
            }
        }
        // Java: 520
        public static long getDiscordUserIdForProfile(string nick)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return ((long)((long?)com.oddlabs.matchserver.DBInterface.discordLinks.getOrDefault(nick, (-1L))));
            }
        }
        // Java: 526
        public static string[] getProfilesRegisteredToDiscordUser(long discord_user_id)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                java.util.List @out = new java.util.ArrayList();
                com.oddlabs.matchserver.DBInterface.discordLinks.forEach(new java.util.function.BiConsumer_L((object n_l, object id_l) =>
                {
                    string n = (string)n_l;
                    long? id = (long?)id_l;
                    if (((long)id) == discord_user_id)
                    {
                        @out.add(n);
                    }
                }));
                return ((string[])@out.toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            }
        }
        // Java: 537
        public static void registerProfileToDiscordUser(string nick, long discord_user_id)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.discordLinks.put(nick, discord_user_id);
            }
        }
        // Java: 545
        public static void profileOnline(string nick)
        {
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("profileOnline '" + java.lang.JStr.valueOf(nick) + "'");
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.onlineProfiles.put(nick, null);
            }
        }
        // Java: 552
        public static void profileOffline(string nick)
        {
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("profileOffline '" + java.lang.JStr.valueOf(nick) + "'");
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.onlineProfiles.remove(nick);
            }
        }
        // Java: 559
        public static void profileSetGame(string nick, int game_id)
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                if (com.oddlabs.matchserver.DBInterface.onlineProfiles.containsKey(nick))
                {
                    com.oddlabs.matchserver.DBInterface.onlineProfiles.put(nick, game_id);
                }
            }
        }
        // Java: 566
        public static void clearOnlineProfiles()
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                com.oddlabs.matchserver.DBInterface.onlineProfiles.clear();
            }
        }
        // Java: 572
        public static string[] getOnlineProfiles()
        {
            lock ((com.oddlabs.matchserver.DBInterface.LOCK))
            {
                return ((string[])new java.util.TreeSet(com.oddlabs.matchserver.DBInterface.onlineProfiles.keySet()).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            }
        }
        static DBInterface()
        {
            DBInterface.LOCK = new object();
            DBInterface.AUTO_REGISTER = java.lang.Boolean.parseBoolean(java.lang.JSystem.getProperty("srvtest.autoRegister", "true"));
            DBInterface.SETTINGS = new java.util.HashMap();
            {
                com.oddlabs.matchserver.DBInterface.SETTINGS.put("min_username_length", "4");
                com.oddlabs.matchserver.DBInterface.SETTINGS.put("max_username_length", "64");
                com.oddlabs.matchserver.DBInterface.SETTINGS.put("allowed_chars", "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_0123456789");
                com.oddlabs.matchserver.DBInterface.SETTINGS.put("max_profiles", "10");
            }
            DBInterface.registrations = new java.util.LinkedHashMap();
            DBInterface.profiles = new java.util.LinkedHashMap();
            DBInterface.openSkill = new java.util.HashMap();
            DBInterface.games = new java.util.LinkedHashMap();
            DBInterface.onlineProfiles = new java.util.LinkedHashMap();
            DBInterface.discordLinks = new java.util.HashMap();
            DBInterface.nextRegId = 1;
            DBInterface.nextGameId = 1;
            DBInterface.bannedWords = new java.util.LinkedHashMap();
        }
        public DBInterface() : base()
        {
        }
    }
}
