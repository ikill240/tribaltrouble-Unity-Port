// Translated from Server.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 34
    public class Server : java.lang.Object, com.oddlabs.net.ConnectionListenerInterface
    {
        public const int NEGOTIATING = 1;
        public const int SYNCHRONIZING = 2;
        public const int CLOSED = 3;
        public const int JOIN_DEFAULT_NONE = (-1);
        public com.oddlabs.tt.net.PlayerSlot[] players;
        public int[] join_default_race;
        public int[] join_default_team;
        public string[] ai_names;
        public com.oddlabs.tt.resource.WorldGenerator generator;
        public com.oddlabs.matchmaking.Game game;
        public com.oddlabs.tt.landscape.WorldParameters world_params;
        public com.oddlabs.net.AbstractConnectionListener local_listener;
        public java.util.Map connection_to_client;
        public java.util.Random random;
        public com.oddlabs.net.AbstractConnectionListener tunnelled_listener;
        public int state;
        public bool register_server;
        // Java: 89
        public void applyRosterJoinDefaults(com.oddlabs.matchmaking.RosterTemplate roster)
        {
            com.oddlabs.matchmaking.RosterTemplate.Slot[] slots = roster.getSlots();
            for (int i = 0; i < players.Length && i < slots.Length; i++)
            {
                int? race = slots[i].getRace();
                int? team = slots[i].getTeam();
                if (race != null)
                {
                    join_default_race[i] = ((int)race);
                }
                if (team != null)
                {
                    join_default_team[i] = ((int)team);
                }
            }
        }
        // Java: 101
        public java.util.Iterator getClientIterator()
        {
            return connection_to_client.values().iterator();
        }
        // Java: 105
        public int getNumClients()
        {
            return connection_to_client.size();
        }
        // Java: 109
        public com.oddlabs.tt.net.ClientConnection getClientFromConnection(com.oddlabs.net.AbstractConnection conn)
        {
            return ((com.oddlabs.tt.net.ClientConnection)connection_to_client.get(conn));
        }
        // Java: 113
        public void unregisterGame()
        {
            if (local_listener != null)
            {
                local_listener.close();
            }
            if (tunnelled_listener != null)
            {
                tunnelled_listener.close();
            }
            if (register_server && com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().unregisterGame();
            }
        }
        // Java: 126
        public void unregister()
        {
            state = com.oddlabs.tt.net.Server.CLOSED;
        }
        // Java: 130
        public void closeConnections()
        {
            foreach (com.oddlabs.net.AbstractConnection conn in java.lang.JIter.of(connection_to_client.keySet()))
            {
                conn.close();
            }
            connection_to_client.clear();
            unregister();
        }
        // Java: 142
        public void updateWorldParameters(com.oddlabs.tt.landscape.WorldParameters @params)
        {
            this.world_params = @params;
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                ((com.oddlabs.tt.net.ClientConnection)it.next()).getClientInterface().setWorldParameters(@params);
            }
        }
        // Java: 150
        public void setWorldParameters(com.oddlabs.tt.landscape.WorldParameters world_params)
        {
            this.world_params = world_params;
        }
        // Java: 154
        public void close()
        {
            unregisterGame();
            closeConnections();
        }
        // Java: 159
        public int getNumReady()
        {
            int count = 0;
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                if (client.getClient().getPlayerSlot().isReady())
                {
                    count++;
                }
            }
            return count;
        }
        // Java: 170
        public void error(com.oddlabs.net.AbstractConnectionListener listener, System.IO.IOException e)
        {
            java.lang.IO.println("Listener failed: " + java.lang.JStr.valueOf(e));
            e.printStackTrace();
            close();
        }
        // Java: 183
        public void handleError(com.oddlabs.net.AbstractConnection conn, System.Exception e)
        {
            java.lang.IO.println("Disconnecting client because of exception: " + java.lang.JStr.valueOf(e));
            com.oddlabs.tt.net.ClientConnection client = getClientFromConnection(conn);
            if (client != null)
            {
                disconnectClient(client);
                if (state == com.oddlabs.tt.net.Server.NEGOTIATING)
                {
                    resetSlotState(client.getClient().getPlayerSlot(), true);
                }
            }
        }
        // Java: 194
        public void disconnectClient(com.oddlabs.tt.net.ClientConnection client)
        {
            client.getConnection().close();
            connection_to_client.remove(client.getConnection());
        }
        // Java: 200
        public com.oddlabs.tt.net.ClientConnection locateClientForSlot(com.oddlabs.tt.net.PlayerSlot player_slot)
        {
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                if (((object)client.getClient().getPlayerSlot() == (object)player_slot))
                {
                    return client;
                }
            }
            return null;
        }
        // Java: 210
        public void resetSlotState(com.oddlabs.tt.net.PlayerSlot client_slot, int slot, bool open)
        {
            if (!canControlSlot(client_slot, slot))
            {
                return;
            }
            resetSlotState(players[slot], open);
        }
        // Java: 216
        public void resetSlotState(com.oddlabs.tt.net.PlayerSlot client_slot, bool open)
        {
            client_slot.setType((open ? com.oddlabs.tt.net.PlayerSlot.OPEN : com.oddlabs.tt.net.PlayerSlot.CLOSED));
            client_slot.setInfo(null);
            client_slot.setAddress(null);
            client_slot.setReady(true);
            client_slot.setAIDifficulty(com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            com.oddlabs.tt.net.ClientConnection player_client = locateClientForSlot(client_slot);
            if (player_client != null)
            {
                disconnectClient(player_client);
            }
            broadcastPlayers(true);
        }
        // Java: 228
        public bool canControlSlot(com.oddlabs.tt.net.PlayerSlot client_slot, int slot)
        {
            return slot >= 0 && slot < players.Length && state == com.oddlabs.tt.net.Server.NEGOTIATING && ((client_slot.getSlot() == 0 || client_slot.getSlot() == slot));
        }
        // Java: 233
        public void startServer(com.oddlabs.tt.net.PlayerSlot slot)
        {
            if (!canControlSlot(slot, 0) || getNumReady() != getNumClients())
            {
                return;
            }
            if (!buildCheckPassed())
            {
                return;
            }
            if (!allPlayersHaveCustomMap(slot))
            {
                return;
            }
            state = com.oddlabs.tt.net.Server.SYNCHRONIZING;
            unregisterGame();
            broadcastInits();
        }
        // Java: 246
        public void setPlayerNames(com.oddlabs.tt.net.PlayerSlot client_slot, string chief_name, string tribe_name)
        {
            com.oddlabs.tt.net.PlayerSlot player_slot = players[client_slot.getSlot()];
            com.oddlabs.tt.player.PlayerInfo info = player_slot.getInfo();
            if (info == null || player_slot.getType() != com.oddlabs.tt.net.PlayerSlot.HUMAN)
            {
                return;
            }
            player_slot.setInfo(new com.oddlabs.tt.player.PlayerInfo(info.getTeam(), info.getRaceChoice(), info.getName(), com.oddlabs.tt.player.PlayerInfo.clean(chief_name), com.oddlabs.tt.player.PlayerInfo.clean(tribe_name)));
            broadcastPlayers(false);
        }
        // Java: 256
        public void setPlayerSlot(com.oddlabs.tt.net.PlayerSlot client_slot, int slot, int type, int race, int team, bool ready, int ai_difficulty)
        {
            if (!com.oddlabs.tt.net.PlayerSlot.isValidType(type) || !com.oddlabs.tt.model.RacesResources.isValidRace(race))
            {
                return;
            }
            if (!canControlSlot(client_slot, slot) || (client_slot.getSlot() == slot && type != com.oddlabs.tt.net.PlayerSlot.HUMAN))
            {
                return;
            }
            com.oddlabs.tt.net.PlayerSlot player_slot = players[slot];
            com.oddlabs.tt.net.ClientConnection player_client = locateClientForSlot(player_slot);
            if (player_client != null && type != com.oddlabs.tt.net.PlayerSlot.HUMAN)
            {
                disconnectClient(player_client);
            }
            string name = default;
            if (type == com.oddlabs.tt.net.PlayerSlot.AI)
            {
                name = ai_names[slot];
            }
            else
            {
                name = player_slot.getInfo().getName();
            }
            com.oddlabs.tt.player.PlayerInfo old = player_slot.getInfo();
            com.oddlabs.tt.player.PlayerInfo player_info = (type == com.oddlabs.tt.net.PlayerSlot.HUMAN && old != null ? new com.oddlabs.tt.player.PlayerInfo(team, race, name, old.getChiefName(), old.getTribeName()) : new com.oddlabs.tt.player.PlayerInfo(team, race, name));
            bool reset_ready = player_slot.getInfo() == null || type != player_slot.getType() || ai_difficulty != player_slot.getAIDifficulty() || !java.lang.JO.equals(player_info, player_slot.getInfo());
            player_slot.setType(type);
            player_slot.setAIDifficulty(ai_difficulty);
            player_slot.setInfo(player_info);
            player_slot.setReady(type != com.oddlabs.tt.net.PlayerSlot.HUMAN || ready);
            broadcastPlayers(reset_ready);
        }
        // Java: 285
        public void resetReady()
        {
            int num_humans = 0;
            foreach (com.oddlabs.tt.net.PlayerSlot player_slot in players)
            {
                if (player_slot.getType() == com.oddlabs.tt.net.PlayerSlot.HUMAN)
                {
                    num_humans++;
                }
            }
            if (num_humans > 1)
            {
                foreach (com.oddlabs.tt.net.PlayerSlot player_slot_2 in players)
                {
                    if (player_slot_2.getType() == com.oddlabs.tt.net.PlayerSlot.HUMAN)
                    {
                        player_slot_2.setReady(false);
                    }
                }
            }
        }
        // Java: 299
        public void broadcastPlayers(bool reset_ready)
        {
            if (reset_ready)
            {
                resetReady();
            }
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                client.getClientInterface().setPlayers(players);
            }
        }
        // Java: 309
        public void chat(com.oddlabs.tt.net.PlayerSlot player_slot, string chat_v)
        {
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                client.getClientInterface().chat(player_slot.getSlot(), chat_v);
            }
        }
        // Java: 317
        public void broadcastInits()
        {
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                int session_id = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick()).nextInt();
                client.getClientInterface().startGame(session_id);
            }
        }
        // Java: 326
        public short locateAvailableSlot()
        {
            for (short i = unchecked((short)(0)); i < players.Length; i++)
            {
                if (players[i].getType() == com.oddlabs.tt.net.PlayerSlot.OPEN)
                {
                    return i;
                }
            }
            return unchecked((short)((-1)));
        }
        // Java: 334
        public void incomingConnection(com.oddlabs.net.AbstractConnectionListener connection_listener, object remote_address)
        {
            java.lang.IO.println("Incoming host connection from " + java.lang.JStr.valueOf(remote_address));
            short available_slot = locateAvailableSlot();
            if (state != com.oddlabs.tt.net.Server.NEGOTIATING || available_slot == (-1) || ((remote_address is java.net.InetAddress address) && !address.isLoopbackAddress()) || ((remote_address is com.oddlabs.tt.net.TunnelIdentifier identifier) && game != null && game.isRated() && identifier.profile().getWins() < com.oddlabs.matchmaking.GameSession.MIN_WINS_FOR_RANKING))
            {
                java.lang.IO.println("rejecting incoming connection since state = " + (state) + " | locateAvailableSlot() = " + (available_slot) + " remote_address = " + java.lang.JStr.valueOf(remote_address));
                connection_listener.rejectConnection();
                return;
            }
            com.oddlabs.tt.net.PlayerSlot player_slot = players[available_slot];
            int rating = 0;
            string name = default;
            com.oddlabs.matchmaking.TunnelAddress address_2 = default;
            if ((remote_address is java.net.InetAddress))
            {
                address_2 = com.oddlabs.tt.net.Network.getMatchmakingClient().getLocalAddress();
                if (register_server)
                {
                    tunnelled_listener = new com.oddlabs.tt.net.TunnelledConnectionListener(this);
                    com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().registerGame(game);
                }
                com.oddlabs.matchmaking.Profile profile = com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile();
                if (profile != null)
                {
                    name = profile.getNick();
                    rating = profile.getRating();
                }
                else
                {
                    name = com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.net.MatchmakingClient).getName()), "player");
                }
            }
            else
            {
                com.oddlabs.tt.net.TunnelIdentifier tunnel_id = ((com.oddlabs.tt.net.TunnelIdentifier)remote_address);
                name = tunnel_id.profile().getNick();
                rating = tunnel_id.profile().getRating();
                address_2 = tunnel_id.address();
            }
            player_slot.setReady(false);
            int max_teams = players.Length;
            if (game != null && game.isRated())
            {
                max_teams = 2;
            }
            int race = (join_default_race[available_slot] != com.oddlabs.tt.net.Server.JOIN_DEFAULT_NONE ? join_default_race[available_slot] : random.nextInt(com.oddlabs.tt.model.RacesResources.getNumRaces()));
            int team = (join_default_team[available_slot] != com.oddlabs.tt.net.Server.JOIN_DEFAULT_NONE ? join_default_team[available_slot] % max_teams : available_slot % max_teams);
            com.oddlabs.tt.player.PlayerInfo player_info = new com.oddlabs.tt.player.PlayerInfo(team, race, name);
            player_slot.setRating(rating);
            player_slot.setType(com.oddlabs.tt.net.PlayerSlot.HUMAN);
            player_slot.setAddress(address_2);
            player_slot.setInfo(player_info);
            com.oddlabs.tt.net.ClientInfo client = new com.oddlabs.tt.net.ClientInfo(this, player_slot);
            com.oddlabs.net.AbstractConnection conn = connection_listener.acceptConnection(client);
            com.oddlabs.tt.net.ClientConnection client_conn = new com.oddlabs.tt.net.ClientConnection(conn, client);
            connection_to_client.put(conn, client_conn);
            if (world_params != null)
            {
                client_conn.getClientInterface().setWorldParameters(world_params);
            }
            client_conn.getClientInterface().setWorldGeneratorAndPlayerSlot(game, generator, available_slot, players.Length);
            broadcastPlayers(true);
        }
        public const int CUSTOM_MAP_CHUNK_BYTES = 12288;
        public const int CUSTOM_MAP_WINDOW = 2;
        public sbyte[] custom_map_data;
        // Java: 409
        public void requestCustomMap(com.oddlabs.tt.net.PlayerSlot player_slot)
        {
            com.oddlabs.tt.net.ClientConnection client = locateClientForSlot(player_slot);
            if (state != com.oddlabs.tt.net.Server.NEGOTIATING || client == null || client.has_custom_map || client.custom_map_offset >= 0 || com.oddlabs.tt.resource.TransferableMap.of(generator) == null)
            {
                return;
            }
            com.oddlabs.tt.resource.TransferableMap map = com.oddlabs.tt.resource.TransferableMap.of(generator);
            if (custom_map_data == null)
            {
                try
                {
                    custom_map_data = map.readCompressed();
                }
                catch (System.Exception e) when (e is System.IO.IOException || e is System.Exception)
                {
                    java.lang.IO.println("Couldn't read the custom map to send it: " + java.lang.JStr.valueOf(e));
                    client.getClientInterface().receiveCustomMapChunk((-1), new sbyte[0]);
                    return;
                }
            }
            client.custom_map_offset = 0;
            client.custom_map_in_flight = 0;
            fillCustomMapWindow(client);
        }
        // Java: 429
        public void customMapChunkReceived(com.oddlabs.tt.net.PlayerSlot player_slot)
        {
            com.oddlabs.tt.net.ClientConnection client = locateClientForSlot(player_slot);
            if (state != com.oddlabs.tt.net.Server.NEGOTIATING || client == null || client.custom_map_offset < 0)
            {
                return;
            }
            client.custom_map_in_flight = java.lang.Math.max(0, client.custom_map_in_flight - 1);
            fillCustomMapWindow(client);
        }
        // Java: 437
        public void fillCustomMapWindow(com.oddlabs.tt.net.ClientConnection client)
        {
            sbyte[] data = custom_map_data;
            while (data != null && client.custom_map_in_flight < com.oddlabs.tt.net.Server.CUSTOM_MAP_WINDOW && client.custom_map_offset < data.Length)
            {
                sendNextCustomMapChunk(client);
            }
        }
        // Java: 443
        public void sendNextCustomMapChunk(com.oddlabs.tt.net.ClientConnection client)
        {
            sbyte[] data = custom_map_data;
            if (data == null || client.custom_map_offset >= data.Length)
            {
                return;
            }
            int end = java.lang.Math.min(client.custom_map_offset + com.oddlabs.tt.net.Server.CUSTOM_MAP_CHUNK_BYTES, data.Length);
            sbyte[] chunk = java.util.Arrays.copyOfRange(data, client.custom_map_offset, end);
            client.custom_map_offset = end;
            client.custom_map_in_flight++;
            client.getClientInterface().receiveCustomMapChunk(data.Length, chunk);
        }
        // Java: 454
        public void customMapReady(com.oddlabs.tt.net.PlayerSlot player_slot)
        {
            com.oddlabs.tt.net.ClientConnection client = locateClientForSlot(player_slot);
            if (client != null)
            {
                client.has_custom_map = true;
                client.custom_map_offset = (-1);
            }
        }
        // Java: 464
        public bool allPlayersHaveCustomMap(com.oddlabs.tt.net.PlayerSlot host_slot)
        {
            if (com.oddlabs.tt.resource.TransferableMap.of(generator) == null)
            {
                return true;
            }
            java.lang.StringBuilder waiting = new java.lang.StringBuilder();
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                com.oddlabs.tt.net.PlayerSlot slot = client.getClient().getPlayerSlot();
                if (((object)slot == (object)host_slot) || client.has_custom_map)
                {
                    continue;
                }
                if (waiting.length() > 0)
                {
                    waiting.append(", ");
                }
                waiting.append((slot.getInfo() != null ? slot.getInfo().getName() : "a player"));
            }
            if (waiting.isEmpty())
            {
                return true;
            }
            com.oddlabs.tt.net.ClientConnection host = locateClientForSlot(host_slot);
            if (host != null)
            {
                host.getClientInterface().customMapNotice("Can't start yet - still sending the map to: " + java.lang.JStr.valueOf(waiting));
            }
            return false;
        }
        public const string BUILD_CHECK_LABEL = "Build check";
        public bool build_start_warned;
        // Java: 495
        public void reportBuild(com.oddlabs.tt.net.PlayerSlot player_slot, string fingerprint)
        {
            com.oddlabs.tt.net.ClientConnection client = locateClientForSlot(player_slot);
            if (state != com.oddlabs.tt.net.Server.NEGOTIATING || client == null || fingerprint == null)
            {
                return;
            }
            client.build_fingerprint = (fingerprint.length() > 128 ? fingerprint.substring(0, 128) : fingerprint);
            string mine = com.oddlabs.tt.net.BuildFingerprint.get();
            if (player_slot.getSlot() == 0 || com.oddlabs.tt.net.BuildFingerprint.matches(mine, client.build_fingerprint))
            {
                return;
            }
            build_start_warned = false;
            string theirs = com.oddlabs.tt.net.BuildFingerprint.shortForm(client.build_fingerprint);
            string host = com.oddlabs.tt.net.BuildFingerprint.shortForm(mine);
            client.getClientInterface().lobbyNotice(com.oddlabs.tt.net.Server.BUILD_CHECK_LABEL, "Your game build (" + java.lang.JStr.valueOf(theirs) + ") is different from the host's (" + java.lang.JStr.valueOf(host) + "). The match will almost certainly go out of sync - use the exact same copy of the game as the host.");
            com.oddlabs.tt.net.ClientConnection host_client = hostClient();
            if (host_client != null)
            {
                host_client.getClientInterface().lobbyNotice(com.oddlabs.tt.net.Server.BUILD_CHECK_LABEL, java.lang.JStr.valueOf(com.oddlabs.tt.net.Server.playerName(player_slot)) + "'s game build (" + java.lang.JStr.valueOf(theirs) + ") is different from yours (" + java.lang.JStr.valueOf(host) + "). The match will almost certainly go out of sync.");
            }
        }
        // Java: 516
        public bool buildCheckPassed()
        {
            string mine = com.oddlabs.tt.net.BuildFingerprint.get();
            java.lang.StringBuilder mismatched = new java.lang.StringBuilder();
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                com.oddlabs.tt.net.PlayerSlot slot = client.getClient().getPlayerSlot();
                if (slot.getSlot() == 0 || (client.build_fingerprint != null && com.oddlabs.tt.net.BuildFingerprint.matches(mine, client.build_fingerprint)))
                {
                    continue;
                }
                if (!mismatched.isEmpty())
                {
                    mismatched.append(", ");
                }
                mismatched.append(com.oddlabs.tt.net.Server.playerName(slot)).append((client.build_fingerprint == null ? " (didn't report a build - probably an older version)" : ""));
            }
            if (mismatched.isEmpty() || build_start_warned)
            {
                return true;
            }
            build_start_warned = true;
            com.oddlabs.tt.net.ClientConnection host_client = hostClient();
            if (host_client != null)
            {
                host_client.getClientInterface().lobbyNotice(com.oddlabs.tt.net.Server.BUILD_CHECK_LABEL, "Not started: these players are on a different game build: " + java.lang.JStr.valueOf(mismatched) + ". The match will likely go out of sync. Press Start again to play anyway.");
            }
            return false;
        }
        // Java: 542
        public com.oddlabs.tt.net.ClientConnection hostClient()
        {
            java.util.Iterator it = getClientIterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.ClientConnection client = ((com.oddlabs.tt.net.ClientConnection)it.next());
                if (client.getClient().getPlayerSlot().getSlot() == 0)
                {
                    return client;
                }
            }
            return null;
        }
        // Java: 552
        public static string playerName(com.oddlabs.tt.net.PlayerSlot slot)
        {
            return (slot.getInfo() != null ? slot.getInfo().getName() : "A player");
        }
        private void init_Server()
        {
            this.connection_to_client = new java.util.LinkedHashMap();
            this.state = com.oddlabs.tt.net.Server.NEGOTIATING;
        }
        public Server(com.oddlabs.net.NetworkSelector network, com.oddlabs.matchmaking.Game game, java.net.InetAddress ip, com.oddlabs.tt.resource.WorldGenerator generator, bool register_server, string[] ai_names) : this(network, game, ip, generator, register_server, ai_names, com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS)
        {
        }
        public Server(com.oddlabs.net.NetworkSelector network, com.oddlabs.matchmaking.Game game, java.net.InetAddress ip, com.oddlabs.tt.resource.WorldGenerator generator, bool register_server, string[] ai_names, int player_count) : base()
        {
            init_Server();
            this.local_listener = new com.oddlabs.net.ConnectionListener(network, ip, com.oddlabs.tt.global.Globals.NET_PORT, this);
            this.game = game;
            this.generator = generator;
            this.register_server = register_server;
            this.ai_names = ai_names;
            this.random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick());
            players = new com.oddlabs.tt.net.PlayerSlot[player_count];
            for (short i = unchecked((short)(0)); i < players.Length; i++)
            {
                players[i] = new com.oddlabs.tt.net.PlayerSlot(i);
                players[i].setReady(i != 0);
            }
            join_default_race = new int[player_count];
            join_default_team = new int[player_count];
            java.util.Arrays.fill(join_default_race, com.oddlabs.tt.net.Server.JOIN_DEFAULT_NONE);
            java.util.Arrays.fill(join_default_team, com.oddlabs.tt.net.Server.JOIN_DEFAULT_NONE);
        }
    }
}
