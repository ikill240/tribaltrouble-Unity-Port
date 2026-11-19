// Translated from Client.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 34
    public class Client : java.lang.Object, com.oddlabs.matchmaking.MatchmakingServerInterface, com.oddlabs.net.ConnectionInterface
    {
        public const int CHUNK_SIZE = 10;
        public static java.util.Set game_hosts;
        public static java.util.Map active_clients;
        public static int current_random_seed;
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.matchserver.MatchmakingServer server;
        public com.oddlabs.matchmaking.MatchmakingClientInterface client_interface;
        public com.oddlabs.net.AbstractConnection conn;
        public java.util.Map tunnels;
        public java.net.InetAddress remote_address;
        public java.net.InetAddress local_remote_address;
        public int host_id;
        public com.oddlabs.net.HostSequenceID[] multicast_addresses;
        public java.util.Random random;
        public int update_key;
        public int revision;
        public int sim_version;
        public string username;
        public bool guest;
        public com.oddlabs.matchmaking.Profile active_profile;
        public com.oddlabs.matchmaking.Game current_game;
        public com.oddlabs.matchserver.TimestampedGameSession current_session;
        public com.oddlabs.matchserver.TimestampedGameSession spectated_session;
        public com.oddlabs.matchserver.ChatRoom current_room;
        // Java: 83
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 86
        public void requestProfiles()
        {
            if (!guest)
            {
                client_interface.updateProfileList(com.oddlabs.matchserver.DBInterface.getProfiles(username, revision), com.oddlabs.matchserver.DBInterface.getLastUsedProfile(username));
            }
        }
        // Java: 92
        public void setProfile(string nick)
        {
            closeProfile();
            if (!guest)
            {
                if (nick != null)
                {
                    updateProfile(nick);
                }
            }
            else
            {
                updateProfile(new com.oddlabs.matchmaking.Profile(username, 0, 0, 0, 0, revision));
            }
            if (active_profile != null)
            {
                com.oddlabs.matchserver.Client.active_clients.put(active_profile.getNick().toLowerCase(), this);
                com.oddlabs.matchserver.DBInterface.profileOnline(active_profile.getNick());
                com.oddlabs.matchserver.ChatRoom.joinStandardChatRoom(this);
            }
        }
        // Java: 107
        public void createProfile(string nick)
        {
            if (!guest)
            {
                com.oddlabs.matchmaking.Profile[] profiles = com.oddlabs.matchserver.DBInterface.getProfiles(username, revision);
                if (profiles.Length >= com.oddlabs.matchserver.DBInterface.getSettingsInt("max_profiles"))
                {
                    client_interface.createProfileError(com.oddlabs.matchmaking.MatchmakingClientInterface.USERNAME_ERROR_TOO_MANY);
                    return;
                }
                try
                {
                    com.oddlabs.matchserver.Authenticator.checkUsername(nick);
                }
                catch (com.oddlabs.matchserver.InvalidUsernameException e)
                {
                    client_interface.createProfileError(e.getErrorCode());
                    return;
                }
                if (nick.toLowerCase().startsWith("guest"))
                {
                    client_interface.createProfileError(com.oddlabs.matchmaking.MatchmakingClientInterface.PROFILE_ERROR_GUEST);
                    return;
                }
                if (com.oddlabs.matchserver.DBInterface.nickExists(nick))
                {
                    client_interface.createProfileError(com.oddlabs.matchmaking.MatchmakingClientInterface.USERNAME_ERROR_ALREADY_EXISTS);
                    return;
                }
                com.oddlabs.matchserver.DBInterface.createProfile(username, nick);
                client_interface.createProfileSuccess();
            }
        }
        // Java: 137
        public void logPriority(string other_nick, int priority)
        {
            if (current_session != null && active_profile != null && ((com.oddlabs.matchserver.Client)com.oddlabs.matchserver.Client.active_clients.get(other_nick.toLowerCase())) != null)
            {
                com.oddlabs.matchserver.DBInterface.logPriority(current_session.getDatabaseID(), active_profile.getNick(), other_nick, priority);
            }
        }
        // Java: 143
        public void deleteProfile(string nick)
        {
            if (!guest)
            {
                com.oddlabs.matchserver.DBInterface.deleteProfile(username, nick);
            }
        }
        // Java: 149
        public void updateProfile()
        {
            if (!guest)
            {
                if (active_profile != null)
                {
                    updateProfile(active_profile.getNick());
                }
            }
        }
        // Java: 156
        public void updateProfile(string nick)
        {
            if (!guest)
            {
                com.oddlabs.matchmaking.Profile profile = com.oddlabs.matchserver.DBInterface.getProfile(username, nick, revision);
                if (profile != null)
                {
                    updateProfile(profile);
                    com.oddlabs.matchserver.DBInterface.setLastUsedProfile(username, nick);
                }
            }
        }
        // Java: 166
        public void updateProfile(com.oddlabs.matchmaking.Profile profile)
        {
            active_profile = profile;
            client_interface.updateProfile(active_profile);
        }
        // Java: 171
        public com.oddlabs.matchmaking.Profile getProfile()
        {
            return active_profile;
        }
        // Java: 175
        public static java.util.Map getActiveClients()
        {
            return com.oddlabs.matchserver.Client.active_clients;
        }
        // Java: 179
        public void freeQuitStopNotify()
        {
            if (getGameSession() == null)
            {
                return;
            }
            getGameSession().freeQuitStop();
        }
        // Java: 185
        public void updateGameStatus(int tick, int[] status)
        {
            if (getGameSession() == null || status == null || tick < 0)
            {
                return;
            }
            getGameSession().updateGameStatus(tick, status);
        }
        // Java: 191
        public void updateSpectatorInfo(int tick, string info)
        {
            if (getGameSession() == null)
            {
                return;
            }
            getGameSession().updateSpectatorInfo(tick, info);
        }
        // Java: 196
        public void updateCommandEvent(int tick, int client_id, short event_size, sbyte[] event_data)
        {
            if (getGameSession() == null)
            {
                return;
            }
            getGameSession().updateCommandEvent(tick, client_id, event_size, event_data);
        }
        // Java: 201
        public void updateWorldParams(sbyte[] world_params_data)
        {
            if (getGameSession() == null)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("updateWorldParams: no game session for " + java.lang.JStr.valueOf(getUsername()));
                return;
            }
            getGameSession().updateWorldParams(world_params_data);
        }
        // Java: 209
        public void requestSpectate(string nick)
        {
            com.oddlabs.matchserver.Client target = ((com.oddlabs.matchserver.Client)((com.oddlabs.matchserver.Client)com.oddlabs.matchserver.Client.active_clients.get(nick.toLowerCase())));
            if (target == null || !target.isPlaying())
            {
                getClientInterface().error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_NO_SUCH_NICK);
                return;
            }
            com.oddlabs.matchserver.TimestampedGameSession game_session = target.getGameSession();
            if (game_session == null)
            {
                getClientInterface().error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_NO_SUCH_NICK);
                return;
            }
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info(java.lang.JStr.valueOf(getUsername()) + " requested to spectate " + java.lang.JStr.valueOf(nick) + " in game " + (game_session.getDatabaseID()));
            sbyte[] world_params_data = game_session.getWorldParamsData();
            if (world_params_data == null)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (game_session.getDatabaseID()) + ": no world params available for spectating");
                getClientInterface().error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_SPECTATE_FAILED);
                return;
            }
            unregisterGame();
            this.spectated_session = game_session;
            getClientInterface().receiveSpectatorData(world_params_data);
        }
        public const int SPECTATOR_CHUNK_SIZE = 16000;
        // Java: 236
        public void requestSpectatorEventLog()
        {
            if (spectated_session == null)
            {
                return;
            }
            sbyte[] event_log_data = spectated_session.readEventLog();
            int current_tick = spectated_session.getLastTick();
            int total_chunks = (event_log_data.Length == 0 ? 1 : (event_log_data.Length + com.oddlabs.matchserver.Client.SPECTATOR_CHUNK_SIZE - 1) / com.oddlabs.matchserver.Client.SPECTATOR_CHUNK_SIZE);
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Sending spectator event log for game " + (spectated_session.getDatabaseID()) + ": " + (event_log_data.Length) + " bytes in " + (total_chunks) + " chunks, tick=" + (current_tick));
            for (int i = 0; i < total_chunks; i++)
            {
                int offset = i * com.oddlabs.matchserver.Client.SPECTATOR_CHUNK_SIZE;
                int length = java.lang.Math.min(com.oddlabs.matchserver.Client.SPECTATOR_CHUNK_SIZE, event_log_data.Length - offset);
                if (length < 0)
                {
                    length = 0;
                }
                sbyte[] chunk = new sbyte[length];
                if (length > 0)
                {
                    java.lang.JSystem.arraycopy(event_log_data, offset, chunk, 0, length);
                }
                getClientInterface().receiveSpectatorEventLog(chunk, i, total_chunks, current_tick);
            }
            this.spectated_session = null;
        }
        // Java: 254
        public void gameQuitNotify(string nick)
        {
            if (getGameSession() == null)
            {
                return;
            }
            com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)((com.oddlabs.matchserver.Client)com.oddlabs.matchserver.Client.active_clients.get(nick.toLowerCase())));
            if (client == null)
            {
                return;
            }
            if (((object)client == (object)this))
            {
                getGameSession().gameQuit(server, this);
                setGameSession(null);
            }
            else
            {
                getGameSession().participantQuit(server, client);
            }
        }
        // Java: 269
        public void gameLostNotify()
        {
            if (getGameSession() == null)
            {
                return;
            }
            getGameSession().gameLost(server, this);
            setGameSession(null);
        }
        // Java: 276
        public void gameWonNotify()
        {
            if (getGameSession() == null)
            {
                return;
            }
            client_interface.gameWonAck();
            getGameSession().gameWon(server, this);
            setGameSession(null);
        }
        // Java: 284
        public void gameStartedNotify(com.oddlabs.matchmaking.GameSession game_session)
        {
            if (game_session == null || game_session.getParticipants() == null || game_session.getParticipants().Length == 0)
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Invalid GameSession received from " + java.lang.JStr.valueOf(getUsername()));
                return;
            }
            com.oddlabs.matchmaking.Participant[] participants = game_session.getParticipants();
            int database_id = (-1);
            for (int i = 0; i < participants.Length; i++)
            {
                com.oddlabs.matchserver.Client client = server.getClientFromID(participants[i].getMatchID());
                if (client == null)
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Invalid participant in GameSession from " + java.lang.JStr.valueOf(getUsername()));
                    break;
                }
                com.oddlabs.matchmaking.Profile p = client.getProfile();
                if (p == null || !java.lang.JO.equals(p.getNick(), participants[i].getNick()))
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Invalid nickparticipant in GameSession from " + java.lang.JStr.valueOf(getUsername()) + " or " + java.lang.JStr.valueOf(client.getUsername()) + " has given wrong nick");
                    break;
                }
                if (i == 0)
                {
                    database_id = client.getCurrentGame().getDatabaseID();
                }
                com.oddlabs.matchserver.TimestampedGameSession client_session = client.getGameSession();
                if (client_session != null && client_session.getSession().getID() == game_session.getID())
                {
                    if (!java.lang.JO.equals(client_session.getSession(), game_session))
                    {
                        com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("GameSession from " + java.lang.JStr.valueOf(getUsername()) + " does not match the one from " + java.lang.JStr.valueOf(client.getUsername()));
                        break;
                    }
                    if (!client_session.join(server, this))
                    {
                        com.oddlabs.matchserver.MatchmakingServer.getLogger().warning(java.lang.JStr.valueOf(getUsername()) + " joined session " + java.lang.JStr.valueOf(java.lang.Integer.toHexString(game_session.getID())) + " too late or seat already taken");
                        break;
                    }
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().info("GameSession " + java.lang.JStr.valueOf(java.lang.Integer.toHexString(game_session.getID())) + " joined by " + java.lang.JStr.valueOf(getUsername()));
                    setGameSession(client_session);
                    return;
                }
            }
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game " + (database_id) + ": New GameSession " + java.lang.JStr.valueOf(java.lang.Integer.toHexString(game_session.getID())) + " started by " + java.lang.JStr.valueOf(getUsername()));
            com.oddlabs.matchserver.TimestampedGameSession new_session = new com.oddlabs.matchserver.TimestampedGameSession(game_session, database_id);
            if (new_session.join(server, this))
            {
                setGameSession(new_session);
                com.oddlabs.matchserver.DBInterface.startGame(new_session, server);
            }
            else
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Game " + (database_id) + ": " + java.lang.JStr.valueOf(getUsername()) + " could not join own game");
            }
        }
        // Java: 338
        public void setGameSession(com.oddlabs.matchserver.TimestampedGameSession t)
        {
            if (t != null && current_session != null)
            {
                gameLostNotify();
            }
            current_session = t;
            int database_id = (-1);
            if (t != null)
            {
                database_id = t.getDatabaseID();
            }
            com.oddlabs.matchserver.DBInterface.profileSetGame(active_profile.getNick(), database_id);
            if (current_room != null)
            {
                current_room.sendUsers();
            }
        }
        // Java: 351
        public com.oddlabs.matchserver.TimestampedGameSession getGameSession()
        {
            return current_session;
        }
        // Java: 355
        public bool isPlaying()
        {
            return current_session != null;
        }
        // Java: 359
        public void handle(object sender, com.oddlabs.net.ARMIEvent @event)
        {
            try
            {
                @event.execute(interface_methods, this);
            }
            catch (com.oddlabs.net.IllegalARMIEventException e)
            {
                error(e);
            }
        }
        // Java: 367
        public string getUsername()
        {
            return username;
        }
        // Java: 371
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            error(e);
        }
        // Java: 375
        public void error(System.Exception e)
        {
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info(java.lang.JStr.valueOf(username) + " logged out. Caused by: " + java.lang.JStr.valueOf(e.getMessage()));
            com.oddlabs.matchserver.MatchmakingServer.getLogger().throwing("Client", "error", e);
            close();
        }
        // Java: 381
        public void connected(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 384
        public int getHostID()
        {
            return host_id;
        }
        // Java: 388
        public com.oddlabs.matchmaking.Game getCurrentGame()
        {
            return current_game;
        }
        // Java: 392
        public java.net.InetAddress getRemoteAddress()
        {
            return remote_address;
        }
        // Java: 396
        public int getSimVersion()
        {
            return sim_version;
        }
        // Java: 400
        public void requestList(int type, int update_key)
        {
            if (update_key != this.update_key)
            {
                client_interface.updateComplete(this.update_key);
                return;
            }
            client_interface.updateStart(type);
            java.util.Iterator it = default;
            int chunk_index = 0;
            switch ((type))
            {
                case 1:
                {
                    it = com.oddlabs.matchserver.Client.game_hosts.iterator();
                    com.oddlabs.matchmaking.GameHost[] game_hosts_chunk = new com.oddlabs.matchmaking.GameHost[com.oddlabs.matchserver.Client.CHUNK_SIZE];
                    while (it.hasNext())
                    {
                        com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)it.next());
                        if (client.getSimVersion() != sim_version)
                        {
                            continue;
                        }
                        com.oddlabs.matchmaking.Game game = client.getCurrentGame();
                        int host_id = client.getHostID();
                        game_hosts_chunk[chunk_index++] = new com.oddlabs.matchmaking.GameHost(game, host_id, client.getSimVersion());
                        if (chunk_index == game_hosts_chunk.Length)
                        {
                            client_interface.updateList(type, game_hosts_chunk);
                            chunk_index = 0;
                        }
                    }
                    if (chunk_index > 0)
                    {
                        com.oddlabs.matchmaking.GameHost[] capped_game_hosts_chunk = new com.oddlabs.matchmaking.GameHost[chunk_index];
                        for (int i = 0; i < capped_game_hosts_chunk.Length; i++)
                        {
                            capped_game_hosts_chunk[i] = game_hosts_chunk[i];
                        }
                        client_interface.updateList(type, capped_game_hosts_chunk);
                    }
                    break;
                }
                case 2:
                {
                    it = com.oddlabs.matchserver.ChatRoom.getChatRooms().values().iterator();
                    com.oddlabs.matchmaking.ChatRoomEntry[] chat_rooms_chunk = new com.oddlabs.matchmaking.ChatRoomEntry[com.oddlabs.matchserver.Client.CHUNK_SIZE];
                    while (it.hasNext())
                    {
                        com.oddlabs.matchserver.ChatRoom chat_room = ((com.oddlabs.matchserver.ChatRoom)it.next());
                        chat_rooms_chunk[chunk_index++] = new com.oddlabs.matchmaking.ChatRoomEntry(chat_room.getName(), chat_room.getUsers().size());
                        if (chunk_index == chat_rooms_chunk.Length)
                        {
                            client_interface.updateList(type, chat_rooms_chunk);
                            chunk_index = 0;
                        }
                    }
                    if (chunk_index > 0)
                    {
                        com.oddlabs.matchmaking.ChatRoomEntry[] capped_chat_rooms_chunk = new com.oddlabs.matchmaking.ChatRoomEntry[chunk_index];
                        for (int i_2 = 0; i_2 < capped_chat_rooms_chunk.Length; i_2++)
                        {
                            capped_chat_rooms_chunk[i_2] = chat_rooms_chunk[i_2];
                        }
                        client_interface.updateList(type, capped_chat_rooms_chunk);
                    }
                    break;
                }
                case 3:
                {
                    sendRankingChunk(type, com.oddlabs.matchserver.DBInterface.getTopRankings(50), chunk_index);
                    break;
                }
                case 4:
                {
                    sendRankingChunk(type, com.oddlabs.matchserver.DBInterface.getTopOpenSkillRankingEntries(50), chunk_index);
                    break;
                }
                case 5:
                {
                    com.oddlabs.matchmaking.Profile profile = getProfile();
                    com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry rankingEntry = (profile != null ? com.oddlabs.matchserver.Client.getPersonalOpenSkillRankingEntry(profile.getNick()) : (com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry)(null));
                    com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry[] entries = (rankingEntry != null ? new com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry[] { rankingEntry } : new com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry[0]);
                    sendRankingChunk(type, entries, chunk_index);
                    break;
                }
                default:
                {
                    com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Unexpected type requested");
                    break;
                }
            }
            this.update_key = random.nextInt();
            client_interface.updateComplete(this.update_key);
        }
        // Java: 476
        public static com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry getPersonalOpenSkillRankingEntry(string nick)
        {
            com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry = com.oddlabs.matchserver.DBInterface.getOpenSkillRankingEntry(nick);
            if (entry != null)
            {
                return entry;
            }
            return new com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry(0, nick, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_DISPLAY_RATING, true, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_MU, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_SIGMA);
        }
        // Java: 492
        public void sendRankingChunk(int type, object[] all, int chunk_index)
        {
            object[] ranking_chunk = new object[com.oddlabs.matchserver.Client.CHUNK_SIZE];
            for (int i = 0; i < all.Length; i++)
            {
                ranking_chunk[chunk_index++] = all[i];
                if (chunk_index == ranking_chunk.Length)
                {
                    client_interface.updateList(type, ranking_chunk);
                    chunk_index = 0;
                }
            }
            if (chunk_index > 0)
            {
                object[] capped_ranking_chunk = new object[chunk_index];
                for (int i_2 = 0; i_2 < capped_ranking_chunk.Length; i_2++)
                {
                    capped_ranking_chunk[i_2] = ranking_chunk[i_2];
                }
                client_interface.updateList(type, capped_ranking_chunk);
            }
        }
        // Java: 510
        public void closeTunnel(com.oddlabs.net.HostSequenceID address_to)
        {
            com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)((com.oddlabs.matchserver.Client)tunnels.remove(address_to)));
            if (client != null)
            {
                client.tunnelClosed(address_to);
            }
        }
        // Java: 516
        public void openTunnel(int address_to, int seq)
        {
            com.oddlabs.net.HostSequenceID host_seq_id = new com.oddlabs.net.HostSequenceID(getHostID(), seq);
            com.oddlabs.matchserver.Client client = server.getClientFromID(address_to);
            tunnels.put(host_seq_id, client);
            if (client != null && (!com.oddlabs.matchserver.Client.game_hosts.contains(client) || client.getSimVersion() == sim_version))
            {
                client.tunnelOpened(host_seq_id, remote_address, local_remote_address, active_profile, this);
            }
            else
            {
                tunnelClosed(host_seq_id);
            }
        }
        // Java: 528
        public void tunnelClosed(com.oddlabs.net.HostSequenceID address_from)
        {
            if (((com.oddlabs.matchserver.Client)tunnels.remove(address_from)) != null)
            {
                client_interface.tunnelClosed(address_from);
            }
        }
        // Java: 533
        public void close()
        {
            java.util.Iterator it = tunnels.keySet().iterator();
            while (it.hasNext())
            {
                com.oddlabs.net.HostSequenceID tunnel_address = ((com.oddlabs.net.HostSequenceID)it.next());
                com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)tunnels.get(tunnel_address));
                if (client != null && ((object)client != (object)this))
                {
                    client.tunnelClosed(tunnel_address);
                }
            }
            conn.close();
            closeProfile();
            server.logoutClient(this);
        }
        // Java: 546
        public void closeProfile()
        {
            gameLostNotify();
            leaveRoom();
            unregisterGame();
            if (active_profile != null)
            {
                com.oddlabs.matchserver.Client.active_clients.remove(active_profile.getNick().toLowerCase());
                com.oddlabs.matchserver.DBInterface.profileOffline(active_profile.getNick());
                active_profile = null;
            }
        }
        // Java: 557
        public void tunnelOpened(com.oddlabs.net.HostSequenceID address_to, java.net.InetAddress inet_address_to, java.net.InetAddress local_inet_address_to, com.oddlabs.matchmaking.Profile profile, com.oddlabs.matchserver.Client remote_client)
        {
            tunnels.put(address_to, remote_client);
            client_interface.tunnelOpened(address_to, inet_address_to, local_inet_address_to, profile);
        }
        // Java: 563
        public void receiveRoutedEvent(com.oddlabs.net.HostSequenceID address, com.oddlabs.net.ARMIEvent @event)
        {
            client_interface.receiveRoutedEvent(address, @event);
        }
        // Java: 567
        public void setMulticast(com.oddlabs.net.HostSequenceID[] addresses)
        {
            this.multicast_addresses = addresses;
        }
        // Java: 571
        public void multicastEvent(com.oddlabs.net.ARMIEvent @event)
        {
            for (int i = 0; i < multicast_addresses.Length; i++)
            {
                routeEvent(multicast_addresses[i], @event);
            }
        }
        // Java: 576
        public void routeEvent(com.oddlabs.net.HostSequenceID address_to, com.oddlabs.net.ARMIEvent @event)
        {
            com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)((com.oddlabs.matchserver.Client)tunnels.get(address_to)));
            if (client != null)
            {
                client.receiveRoutedEvent(address_to, @event);
            }
            else
            {
                tunnelClosed(address_to);
            }
        }
        // Java: 584
        public void tunnelAccepted(com.oddlabs.net.HostSequenceID host_seq)
        {
            client_interface.tunnelAccepted(host_seq);
        }
        // Java: 588
        public void acceptTunnel(com.oddlabs.net.HostSequenceID address_to)
        {
            com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)((com.oddlabs.matchserver.Client)tunnels.get(address_to)));
            if (client != null)
            {
                client.tunnelAccepted(address_to);
            }
            else
            {
                tunnelClosed(address_to);
            }
        }
        // Java: 596
        public void registerGame(com.oddlabs.matchmaking.Game game)
        {
            if (game != null && game.isValid() && getProfile() != null)
            {
                current_game = game;
                com.oddlabs.matchserver.Client.game_hosts.add(this);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game registered, name = " + java.lang.JStr.valueOf(current_game.getName()));
                com.oddlabs.matchserver.DBInterface.createGame(game, getProfile().getNick(), sim_version);
                if (current_room != null)
                {
                    string formatted_message = java.lang.JStr.valueOf(getProfile().getNick()) + " has created a game called \"" + java.lang.JStr.valueOf(current_game.getName()) + "\".";
                    server.getChatLogger().info(formatted_message);
                    current_room.sendMessage("Server", formatted_message);
                    com.oddlabs.matchserver.discord.DiscordBotService.getInstance().getChatroomCoordinator().ifPresent(new java.util.function.Consumer_L((object coordinator_l) =>
                    {
                        com.oddlabs.matchserver.discord.DiscordChatroomCoordinator coordinator = (com.oddlabs.matchserver.discord.DiscordChatroomCoordinator)coordinator_l;
                        coordinator.sendDiscordMessage(current_room, "Server", formatted_message);
                    }));
                }
            }
        }
        // Java: 613
        public void unregisterGame()
        {
            if (com.oddlabs.matchserver.Client.game_hosts.contains(this))
            {
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Game unregistered, name = " + java.lang.JStr.valueOf(current_game.getName()));
                com.oddlabs.matchserver.Client.game_hosts.remove(this);
                com.oddlabs.matchserver.DBInterface.dropGame(getProfile().getNick());
            }
        }
        // Java: 621
        public com.oddlabs.matchmaking.MatchmakingClientInterface getClientInterface()
        {
            return client_interface;
        }
        // Java: 625
        public void joinRoom(string room_name)
        {
            if (getProfile() != null)
            {
                if (current_room == null && com.oddlabs.matchserver.ChatRoom.isNameValid(room_name))
                {
                    com.oddlabs.matchserver.ChatRoom room = com.oddlabs.matchserver.ChatRoom.getChatRoom(room_name);
                    if (room.join(this))
                    {
                        com.oddlabs.matchserver.MatchmakingServer.getLogger().info(java.lang.JStr.valueOf(getProfile().getNick()) + " joined chat room, name = " + java.lang.JStr.valueOf(room.getName()));
                        current_room = room;
                        client_interface.joiningChatRoom(current_room.getName());
                        room.sendUsers();
                        client_interface.receiveChatRoomMessage("Server", "Welcome to the Tribal Trouble multiplayer server. Please keep a proper tone while playing online: All activity in the chatrooms and the game is logged and any abusive behavior will result in the immediate banning from the multiplayer server at Oddlabs' discretion.");
                    }
                    else
                    {
                        client_interface.error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_TOO_MANY_USERS);
                    }
                }
                else
                {
                    client_interface.error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_INVALID_NAME);
                }
            }
        }
        // Java: 646
        public void sendPrivateMessage(string nick, string msg)
        {
            if (nick == null || msg == null)
            {
                return;
            }
            if (getProfile() != null)
            {
                if (guest)
                {
                    client_interface.receivePrivateMessage("Server", "Sorry, only registered users are able to chat.");
                    return;
                }
                com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)((com.oddlabs.matchserver.Client)com.oddlabs.matchserver.Client.active_clients.get(nick.toLowerCase())));
                if (client != null)
                {
                    if (java.lang.JO.equals(getProfile().getNick().toLowerCase(), nick.toLowerCase()))
                    {
                        checkForRegisterProfileToDiscordResponse(nick, msg);
                    }
                    server.getChatLogger().info("To " + java.lang.JStr.valueOf(nick) + ": " + java.lang.JStr.valueOf(formatChat(msg)));
                    string censored_msg = com.oddlabs.matchserver.BannedWordFilter.censorChatMessage(msg);
                    client.getClientInterface().receivePrivateMessage(getProfile().getNick(), censored_msg);
                    if (((object)client != (object)this))
                    {
                        getClientInterface().receivePrivateMessage(getProfile().getNick(), censored_msg);
                    }
                }
                else
                {
                    getClientInterface().error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_NO_SUCH_NICK);
                }
            }
        }
        // Java: 671
        public void requestInfo(string nick)
        {
            com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)((com.oddlabs.matchserver.Client)com.oddlabs.matchserver.Client.active_clients.get(nick.toLowerCase())));
            if (client != null)
            {
                com.oddlabs.matchmaking.Profile profile = client.getProfile();
                if (profile != null)
                {
                    getClientInterface().receiveInfo(profile);
                }
                else
                {
                    getClientInterface().error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_NO_SUCH_NICK);
                }
            }
        }
        // Java: 682
        public void checkForRegisterProfileToDiscordResponse(string nick, string msg)
        {
            if (com.oddlabs.matchserver.discord.commands.RegisterProfileToDiscordUserCommand.processingProfiles.containsKey(nick.toLowerCase()))
            {
                string trimmed = msg.trim().toLowerCase();
                if (java.lang.JO.equals(trimmed, "/y") || java.lang.JO.equals(trimmed, "/yes"))
                {
                    ((java.lang.Runnable)com.oddlabs.matchserver.discord.commands.RegisterProfileToDiscordUserCommand.processingProfiles.get(nick.toLowerCase())).run();
                }
            }
        }
        // Java: 691
        public string formatChat(string message)
        {
            return "<" + java.lang.JStr.valueOf(getProfile().getNick()) + "> " + java.lang.JStr.valueOf(message);
        }
        // Java: 695
        public void sendMessageToRoom(string msg)
        {
            if (current_room != null)
            {
                if (guest)
                {
                    client_interface.receivePrivateMessage("Server", "Sorry, only registered users are able to chat.");
                    return;
                }
                server.getChatLogger().info(formatChat(msg));
                string censored_msg = com.oddlabs.matchserver.BannedWordFilter.censorChatMessage(msg);
                current_room.sendMessage(getProfile().getNick(), censored_msg);
                com.oddlabs.matchserver.discord.DiscordBotService.getInstance().getChatroomCoordinator().ifPresent(new java.util.function.Consumer_L((object coordinator_l) =>
                {
                    com.oddlabs.matchserver.discord.DiscordChatroomCoordinator coordinator = (com.oddlabs.matchserver.discord.DiscordChatroomCoordinator)coordinator_l;
                    coordinator.sendDiscordMessage(current_room, getProfile().getNick(), formatChat(censored_msg));
                }));
            }
        }
        // Java: 711
        public void leaveRoom()
        {
            if (current_room != null)
            {
                current_room.leave(this);
                current_room = null;
            }
        }
        static Client()
        {
            Client.game_hosts = new java.util.HashSet();
            Client.active_clients = new java.util.HashMap();
            Client.current_random_seed = 1;
        }
        private void init_Client()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.matchmaking.MatchmakingServerInterface));
            this.tunnels = new java.util.HashMap();
            this.random = new java.util.Random(com.oddlabs.matchserver.Client.current_random_seed++);
            this.update_key = 0;
        }
        public Client(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.net.AbstractConnection conn, java.net.InetAddress remote_address, java.net.InetAddress local_remote_address, string username, bool guest, int revision, int sim_version, int host_id) : base()
        {
            init_Client();
            this.conn = conn;
            this.server = server;
            this.remote_address = remote_address;
            this.local_remote_address = local_remote_address;
            this.client_interface = ((com.oddlabs.matchmaking.MatchmakingClientInterface)com.oddlabs.net.ARMIEvent.createProxy(conn, typeof(com.oddlabs.matchmaking.MatchmakingClientInterface)));
            this.username = username;
            this.guest = guest;
            this.revision = revision;
            this.sim_version = sim_version;
            this.host_id = host_id;
            conn.setConnectionInterface(this);
        }
    }
}
