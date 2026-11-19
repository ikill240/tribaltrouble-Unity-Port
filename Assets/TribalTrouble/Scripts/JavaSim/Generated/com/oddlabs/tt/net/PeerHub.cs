// Translated from PeerHub.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 43
    public class PeerHub : java.lang.Object, com.oddlabs.tt.animation.Animated, com.oddlabs.tt.net.RouterHandler
    {
        public static java.util.logging.Logger logger;
        public static java.util.ResourceBundle bundle;
        // Java: 48
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.net.PeerHub.bundle, key, args);
        }
        public static string SYSTEM_NAME;
        public const int MILLISECONDS_PER_HEARTBEAT = 60;
        public const int CLIENT_MAX_DELAY_MILLIS = 60;
        public const float FREE_QUIT_TIME = 120.0f;
        public const int TICKS_PER_STATUS_UPDATE = 1000;
        public const int TICKS_PER_SPECTATOR_UPDATE = 5;
        public const int TICKS_PER_CHECKSUM = 500;
        public static bool waiting_for_ack;
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.tt.util.StateChecksum checksum;
        public com.oddlabs.tt.net.PeerHubInterface peerhubs_interface;
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public int num_participants;
        public com.oddlabs.tt.net.Peer[] peer_index_to_peer;
        public java.util.Map player_to_peer;
        public java.util.Map peer_to_player;
        public java.util.Set nonhuman_players;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.net.RouterClient router_client;
        public com.oddlabs.router.Router router;
        public com.oddlabs.tt.viewer.NotificationManager notification_manager;
        public com.oddlabs.tt.player.Player local_player;
        public com.oddlabs.tt.animation.AnimationManager manager;
        public bool is_multiplayer;
        public bool is_rated;
        public bool is_spectator;
        public com.oddlabs.tt.net.PeerHubSpectatorController spectatorController;
        public com.oddlabs.tt.net.StallHandler stall_handler;
        public int local_peer_index;
        public int pause_ticks;
        public int server_millis;
        public int paused;
        public bool is_synchronized;
        public bool sentMap;
        public bool sentInitInfo;
        public bool sentTrees;
        public int mapRowsSent;
        // Java: 97
        public bool isSynchronized()
        {
            return is_synchronized;
        }
        // Java: 181
        public void routerFailed(System.Exception e)
        {
            com.oddlabs.tt.net.PeerHub.logger.log(java.util.logging.Level.WARNING, "Router failed", e);
            closeNetwork();
            stall_handler.peerhubFailed();
        }
        // Java: 188
        public void heartbeat(int millis)
        {
            if (millis < server_millis)
            {
                routerFailed(new System.IO.IOException("Invalid time received: " + (millis) + " (tick currently at " + (getTick()) + ")"));
                return;
            }
            server_millis = millis;
        }
        // Java: 198
        public void receiveEvent(int client_id, com.oddlabs.net.ARMIEvent @event)
        {
            com.oddlabs.tt.net.Peer peer = getPeerFromClientID(client_id);
            if (peer == null)
            {
                java.lang.IO.println("Ignoring stale event from already-disconnected client_id: " + (client_id));
                return;
            }
            try
            {
                @event.execute(interface_methods, peer);
            }
            catch (com.oddlabs.net.ARMIHandlerException e)
            {
                com.oddlabs.tt.net.PeerHub.logger.log(java.util.logging.Level.SEVERE, "Handling an event from " + java.lang.JStr.valueOf(peer) + " failed; ignored, peer stays connected", e.getCause());
            }
            catch (com.oddlabs.net.IllegalARMIEventException e_2)
            {
                peerDisconnected(peer, e_2.getMessage());
            }
        }
        // Java: 231
        public void receiveGameStateEvent(int client_id, int millis, com.oddlabs.net.ARMIEvent @event)
        {
            if (millis < server_millis)
            {
                routerFailed(new System.IO.IOException("Invalid time received for event: " + (millis) + " (tick currently at " + (getTick()) + ")"));
                return;
            }
            com.oddlabs.tt.net.Peer peer = getPeerFromClientID(client_id);
            if (peer == null)
            {
                java.lang.IO.println("Ignoring stale game state event from already-disconnected client_id: " + (client_id));
                return;
            }
            server_millis = millis;
            int event_tick = millisToTickCeil(millis);
            peer.addEvent(event_tick, @event);
            if (!is_spectator && isFirstActivePeer() && com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                com.oddlabs.tt.net.PeerHubSpectatorController.sendCommandEvent(event_tick, client_id, @event);
            }
        }
        // Java: 253
        public bool isFirstActivePeer()
        {
            for (int i = 0; i < peer_index_to_peer.Length; i++)
            {
                if (peer_index_to_peer[i] != null)
                {
                    return i == local_peer_index;
                }
            }
            return false;
        }
        // Java: 260
        public void playerDisconnected(int client_id, bool checksum_error)
        {
            com.oddlabs.tt.net.Peer peer = getPeerFromClientID(client_id);
            if (checksum_error)
            {
                peerChecksumError(peer);
            }
            else
            {
                peerDisconnected(peer, "Peer disconnected");
            }
        }
        // Java: 269
        public void peerChecksumError(com.oddlabs.tt.net.Peer peer)
        {
            java.lang.IO.println("Disconnecting peer because of checksum mismatch: " + java.lang.JStr.valueOf(peer.getPlayerInfo().getName()));
            peerDisconnected(peer, "Checksum error");
            com.oddlabs.tt.global.Globals.checksum_error_in_last_game = true;
        }
        // Java: 275
        public com.oddlabs.tt.net.Peer getPeerFromClientID(int client_id)
        {
            if (client_id >= 0 && client_id < peer_index_to_peer.Length)
            {
                return unsafeGetPeerFromClientID(client_id);
            }
            else
            {
                return null;
            }
        }
        // Java: 283
        public com.oddlabs.tt.net.Peer unsafeGetPeerFromClientID(int client_id)
        {
            return peer_index_to_peer[client_id];
        }
        // Java: 287
        public void start()
        {
            if (spectatorController != null)
            {
                spectatorController.onStart();
            }
            else
            {
                is_synchronized = true;
            }
        }
        // Java: 296
        public com.oddlabs.tt.net.Peer locatePeerFromPlayer(com.oddlabs.tt.player.Player player)
        {
            return ((com.oddlabs.tt.net.Peer)player_to_peer.get(player));
        }
        // Java: 300
        public bool isDisconnected(com.oddlabs.tt.net.Peer peer)
        {
            return getPlayerFromPeer(peer) == null;
        }
        // Java: 304
        public com.oddlabs.tt.player.Player getPlayerFromPeer(com.oddlabs.tt.net.Peer peer)
        {
            return ((com.oddlabs.tt.player.Player)peer_to_player.get(peer));
        }
        // Java: 308
        public bool isAlive(com.oddlabs.tt.player.Player player)
        {
            return (nonhuman_players.contains(player) || locatePeerFromPlayer(player) != null) && player.isAlive();
        }
        // Java: 312
        public com.oddlabs.tt.player.PlayerInterface getPlayerInterface()
        {
            return player_interface;
        }
        // Java: 316
        public int millisToTickCeil(int millis)
        {
            return millisToTick(millis + unchecked((int)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_TICK)) - 1);
        }
        // Java: 320
        public int millisToTick(int millis)
        {
            return unchecked((int)((millis / com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_TICK))) - pause_ticks;
        }
        // Java: 324
        public void animate(float t)
        {
            if (router != null)
            {
                router.process();
            }
            if (spectatorController != null && spectatorController.animateCatchUp(t))
            {
                return;
            }
            int server_tick = millisToTick(server_millis);
            if (!is_synchronized || getTick() == server_tick)
            {
                processStall();
            }
            else
            {
                if (!isPaused())
                {
                    doTick(t);
                    int min_tick = millisToTick(server_millis - com.oddlabs.tt.net.PeerHub.MILLISECONDS_PER_HEARTBEAT - com.oddlabs.tt.net.PeerHub.CLIENT_MAX_DELAY_MILLIS);
                    while (getTick() < min_tick)
                    {
                        doTick(t);
                    }
                }
                else
                {
                    pause_ticks++;
                }
            }
        }
        // Java: 346
        public int getTick()
        {
            return local_player.getWorld().getTick();
        }
        // Java: 350
        public void setSynchronized(bool @value)
        {
            this.is_synchronized = @value;
        }
        // Java: 357
        public void doTickInternal(float t)
        {
            doTick(t);
        }
        // Java: 361
        public void doTick(float t)
        {
            stall_handler.stopStall();
            if (!is_spectator)
            {
                if (com.oddlabs.tt.net.PeerHub.getFreeQuitTicksLeft(local_player.getWorld()) == 0 && com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
                {
                    com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().freeQuitStopNotify();
                }
                if (getTick() % com.oddlabs.tt.net.PeerHub.TICKS_PER_STATUS_UPDATE == 0 && com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
                {
                    sendStatusUpdate();
                }
                if (is_multiplayer && com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
                {
                    if (!sentMap)
                    {
                        sendMap();
                    }
                    if (!sentInitInfo)
                    {
                        sendInitInfo();
                    }
                    if (!sentTrees)
                    {
                        sendTrees();
                    }
                    if (getTick() % com.oddlabs.tt.net.PeerHub.TICKS_PER_SPECTATOR_UPDATE == 0)
                    {
                        sendSpectatorInfo();
                    }
                }
            }
            foreach (com.oddlabs.tt.net.Peer peer in peer_index_to_peer)
            {
                if (peer != null)
                {
                    try
                    {
                        peer.executeEvents(getTick());
                    }
                    catch (com.oddlabs.net.IllegalARMIEventException e)
                    {
                        peerDisconnected(peer, e.getMessage());
                    }
                }
            }
            local_player.getWorld().tick(t);
            if (!is_spectator && getTick() % com.oddlabs.tt.net.PeerHub.TICKS_PER_CHECKSUM == 0)
            {
                sendChecksum();
            }
        }
        // Java: 393
        public void sendChecksum()
        {
            checksum.update(getTick());
            checksum.update(local_player.getWorld().getChecksum());
            local_player.getWorld().getAnimationManagerGameTime().updateChecksum(checksum);
            local_player.getWorld().getAnimationManagerRealTime().updateChecksum(checksum);
            router_client.getInterface().checksum(checksum.getValue());
        }
        // Java: 401
        public void sendMap()
        {
            com.oddlabs.tt.landscape.HeightMap map = local_player.getWorld().getHeightMap();
            int size = map.getGridUnitsPerWorld();
            if (mapRowsSent < size)
            {
                java.lang.StringBuilder info = new java.lang.StringBuilder("M ").append(mapRowsSent).append(' ');
                for (int x = 0; x < size; x++)
                {
                    info.append(map.getHeight(x, mapRowsSent)).append(' ');
                }
                info.append('\n');
                mapRowsSent++;
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateSpectatorInfo(-mapRowsSent, java.lang.JO.toString(info));
            }
            else
            {
                sentMap = true;
            }
        }
        // Java: 417
        public void sendInitInfo()
        {
            com.oddlabs.tt.player.Player[] players = local_player.getWorld().getPlayers();
            java.lang.StringBuilder info = new java.lang.StringBuilder("I ");
            foreach (com.oddlabs.tt.player.Player player in players)
            {
                org.joml.Vector4fc color = player.getColor();
                string name = player.getPlayerInfo().getName();
                int race = player.getPlayerInfo().getRace();
                int team = player.getPlayerInfo().getTeam();
                info.append("NAME ").append(name).append(' ');
                info.append("RACE ").append(race).append(' ');
                info.append("TEAM ").append(team).append(' ');
                info.append("COLOR ").append(color.x()).append(' ').append(color.y()).append(' ').append(color.z()).append(' ');
            }
            info.append('\n');
            com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateSpectatorInfo((-10001), java.lang.JO.toString(info));
            sentInitInfo = true;
        }
        public const int TREE_CHUNK_SIZE = 2000;
        // Java: 445
        public void sendTrees()
        {
            java.util.List trees = local_player.getWorld().getTreePositions();
            for (int start_v = 0; start_v < trees.size(); start_v += com.oddlabs.tt.net.PeerHub.TREE_CHUNK_SIZE)
            {
                int end = java.lang.Math.min(start_v + com.oddlabs.tt.net.PeerHub.TREE_CHUNK_SIZE, trees.size());
                java.lang.StringBuilder info = new java.lang.StringBuilder("T ");
                foreach (int[] pos in java.lang.JIter.of(trees.subList(start_v, end)))
                {
                    info.append(pos[0]).append(' ').append(pos[1]).append(' ');
                }
                info.append('\n');
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateSpectatorInfo((-10000), java.lang.JO.toString(info));
            }
            sentTrees = true;
        }
        public const int SPECTATOR_INFO_MAX_CHARS = 20000;
        // Java: 465
        public void sendSpectatorInfo()
        {
            int tick = getTick();
            string tick_prefix = (tick) + " ";
            java.lang.StringBuilder info = new java.lang.StringBuilder(tick_prefix);
            com.oddlabs.tt.player.Player[] players = local_player.getWorld().getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                java.lang.StringBuilder section = new java.lang.StringBuilder().append("P ").append(i).append(' ');
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(players[i].getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit u))
                    {
                        section.append("U ").append(u.getGridX()).append(' ').append(u.getGridY()).append(' ');
                    }
                    else if ((s is com.oddlabs.tt.model.Building b))
                    {
                        section.append("B ").append(b.getGridX()).append(' ').append(b.getGridY()).append(' ').append(((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getPlacingSize()).append(' ').append(b.getHitPoints()).append(' ');
                    }
                }
                if (info.length() > tick_prefix.length() && info.length() + section.length() > com.oddlabs.tt.net.PeerHub.SPECTATOR_INFO_MAX_CHARS)
                {
                    info.append('\n');
                    com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateSpectatorInfo(tick, java.lang.JO.toString(info));
                    info = new java.lang.StringBuilder(tick_prefix);
                }
                info.append(section);
            }
            info.append('\n');
            com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateSpectatorInfo(tick, java.lang.JO.toString(info));
        }
        // Java: 493
        public void setPaused(bool p)
        {
            if (!is_multiplayer && p)
            {
                paused++;
            }
            else if (!is_multiplayer && !p)
            {
                paused--;
            }
        }
        // Java: 502
        public bool isPaused()
        {
            return paused > 0;
        }
        // Java: 506
        public void sendStatusUpdate()
        {
            int[] status = new int[num_participants];
            for (int i = 0; i < status.Length; i++)
            {
                com.oddlabs.tt.net.Peer peer = getPeerFromClientID(i);
                if (peer != null)
                {
                    status[i] = peer.getPlayer().getStatus();
                }
            }
            com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateGameStatus(getTick(), status);
        }
        // Java: 516
        public java.util.Iterator getPeerIterator()
        {
            return peer_to_player.keySet().iterator();
        }
        // Java: 520
        public com.oddlabs.tt.net.PeerHubInterface getInterface()
        {
            return peerhubs_interface;
        }
        // Java: 524
        public void processStall()
        {
            stall_handler.processStall(getTick());
        }
        // Java: 528
        public void removePeerFromActiveList(com.oddlabs.tt.net.Peer peer)
        {
            java.lang.IO.println("Removing from active list:" + java.lang.JStr.valueOf(peer));
            peer_index_to_peer[peer.getPeerIndex()] = null;
            peer.getPlayer().setPreferredGamespeed(com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE);
        }
        // Java: 534
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum sum)
        {
            sum.update(getTick());
            sum.update(checksum.getValue());
        }
        // Java: 540
        public void peerDisconnected(com.oddlabs.tt.net.Peer peer, string reason)
        {
            if (!is_multiplayer && peer.getPeerIndex() == local_peer_index)
            {
                java.lang.IO.println("Ignoring spurious self-disconnect of the local player in a singleplayer session (reason: " + java.lang.JStr.valueOf(reason) + ") - likely a router timeout from this client falling behind, not a real disconnect.");
                return;
            }
            com.oddlabs.tt.player.Player player = getPlayerFromPeer(peer);
            if (player == null)
            {
                return;
            }
            peer_to_player.remove(peer);
            player_to_peer.remove(player);
            int peer_index = peer.getPeerIndex();
            removePeerFromActiveList(peer);
            string left_game_message = com.oddlabs.tt.net.PeerHub.i18n("left_game", peer.getPlayerInfo().getName(), reason);
            receiveChat(com.oddlabs.tt.net.PeerHub.SYSTEM_NAME, left_game_message, false);
            if (com.oddlabs.tt.net.PeerHub.getFreeQuitTicksLeft(local_player.getWorld()) >= 0 && com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().gameQuitNotify(peer.getPlayerInfo().getName());
            }
        }
        // Java: 578
        public void sendChat(string text, bool team_only)
        {
            java.util.Iterator it = getPeerIterator();
            int local_team = local_player.getPlayerInfo().getTeam();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.Peer peer = ((com.oddlabs.tt.net.Peer)it.next());
                int peer_team = peer.getPlayerInfo().getTeam();
                if (!team_only || local_team == peer_team)
                {
                    peer.getPeerHubInterface().chat(text, team_only);
                }
            }
        }
        // Java: 589
        public void sendBeacon(float x, float y)
        {
            java.util.Iterator it = getPeerIterator();
            int local_team = local_player.getPlayerInfo().getTeam();
            while (it.hasNext())
            {
                com.oddlabs.tt.net.Peer peer = ((com.oddlabs.tt.net.Peer)it.next());
                int peer_team = peer.getPlayerInfo().getTeam();
                if (local_team == peer_team)
                {
                    peer.getPeerHubInterface().beacon(x, y);
                }
            }
            receiveBeacon(x, y, local_player.getPlayerInfo().getName());
        }
        // Java: 611
        public void receiveChat(string name, string text, bool team)
        {
            if (team)
            {
                com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage(name, text, com.oddlabs.tt.net.ChatMessage.Type.TEAM));
            }
            else
            {
                com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage(name, text, com.oddlabs.tt.net.ChatMessage.Type.NORMAL));
            }
        }
        // Java: 618
        public void receiveBeacon(float x, float y, string owner)
        {
            if (!com.oddlabs.tt.net.ChatCommand.isIgnoring(owner))
            {
                notification_manager.newBeacon(manager, local_player, x, y);
            }
            int local_team = local_player.getPlayerInfo().getTeam();
            foreach (com.oddlabs.tt.player.Player player in local_player.getWorld().getPlayers())
            {
                if (player.getPlayerInfo().getTeam() == local_team && (player.getAI() is com.oddlabs.tt.player.AdvancedAI advanced_ai))
                {
                    advanced_ai.onAllyBeacon(x, y);
                }
            }
        }
        // Java: 636
        public void closeNetwork()
        {
            router_client.close();
            if (router != null)
            {
                router.close();
            }
            object[] peers = peer_to_player.keySet().toArray();
            foreach (object peer1 in peers)
            {
                com.oddlabs.tt.net.Peer peer = ((com.oddlabs.tt.net.Peer)peer1);
                if (((object)peer.getPlayer() != (object)local_player))
                {
                    peerDisconnected(peer, "Synchronization failed");
                }
            }
            stall_handler.stopStall();
            leaveGame();
        }
        // Java: 651
        public void close()
        {
            if (spectatorController != null)
            {
                com.oddlabs.tt.net.PeerHubSpectatorController.clearInstance(spectatorController);
            }
            closeNetwork();
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().removeAnimation(this);
            java.lang.IO.println("PeerHub closed");
        }
        // Java: 658
        public static int getFreeQuitTicksLeft(com.oddlabs.tt.landscape.World world)
        {
            return java.lang.JC.f2i(((float)(com.oddlabs.tt.net.PeerHub.FREE_QUIT_TIME / world.getSecondsPerTick()))) - world.getTick();
        }
        // Java: 662
        public static float getFreeQuitTimeLeft(com.oddlabs.tt.landscape.World world)
        {
            int left = com.oddlabs.tt.net.PeerHub.getFreeQuitTicksLeft(world);
            return (float)(left * com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK);
        }
        // Java: 667
        public void leaveGame()
        {
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                if (com.oddlabs.tt.net.PeerHub.getFreeQuitTicksLeft(local_player.getWorld()) >= 0)
                {
                    com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().gameQuitNotify(local_player.getPlayerInfo().getName());
                }
                else
                {
                    com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().gameLostNotify();
                }
            }
        }
        // Java: 677
        public void gameWon()
        {
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().gameWonNotify();
                com.oddlabs.tt.net.PeerHub.waiting_for_ack = true;
            }
        }
        // Java: 684
        public static void receivedAck()
        {
            com.oddlabs.tt.net.PeerHub.waiting_for_ack = false;
        }
        // Java: 688
        public static bool isWaitingForAck()
        {
            return com.oddlabs.tt.net.PeerHub.waiting_for_ack;
        }
        static PeerHub()
        {
            PeerHub.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.net.PeerHub).getName());
            PeerHub.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.net.PeerHub).getName());
            PeerHub.SYSTEM_NAME = com.oddlabs.tt.net.PeerHub.i18n("system_name");
            PeerHub.waiting_for_ack = false;
        }
        private void init_PeerHub()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.tt.net.PeerHubInterface));
            this.checksum = new com.oddlabs.tt.util.StateChecksum();
            this.player_to_peer = new java.util.LinkedHashMap();
            this.peer_to_player = new java.util.LinkedHashMap();
            this.nonhuman_players = new java.util.HashSet();
        }
        public PeerHub(com.oddlabs.tt.animation.AnimationManager manager, bool is_multiplayer, bool is_rated, com.oddlabs.tt.player.Player local_player, com.oddlabs.tt.net.PlayerSlot[] player_slots, com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.viewer.NotificationManager notification_manager, com.oddlabs.tt.net.DistributableTable distributable_table, com.oddlabs.router.SessionID session_id, com.oddlabs.tt.net.StallHandler stall_handler) : this(manager, is_multiplayer, is_rated, false, local_player, player_slots, network, gui_root, notification_manager, distributable_table, session_id, stall_handler)
        {
        }
        public PeerHub(com.oddlabs.tt.animation.AnimationManager manager, bool is_multiplayer, bool is_rated, bool is_spectator, com.oddlabs.tt.player.Player local_player, com.oddlabs.tt.net.PlayerSlot[] player_slots, com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.viewer.NotificationManager notification_manager, com.oddlabs.tt.net.DistributableTable distributable_table, com.oddlabs.router.SessionID session_id, com.oddlabs.tt.net.StallHandler stall_handler) : base()
        {
            init_PeerHub();
            this.stall_handler = stall_handler;
            this.is_rated = is_rated;
            this.is_spectator = is_spectator;
            this.local_player = local_player;
            this.network = network;
            this.notification_manager = notification_manager;
            this.is_multiplayer = is_multiplayer;
            this.manager = manager;
            com.oddlabs.tt.net.GameArgumentReader argument_reader = new com.oddlabs.tt.net.GameArgumentReader(distributable_table);
            java.util.List peer_index_to_peer_list = new java.util.ArrayList();
            com.oddlabs.tt.player.Player[] players = local_player.getWorld().getPlayers();
            this.local_peer_index = (-1);
            if (!is_multiplayer)
            {
                this.router = new com.oddlabs.router.Router(network, com.oddlabs.util.Utils.getLoopbackAddress(), 0, java.util.logging.Logger.getAnonymousLogger(), new com.oddlabs.router.RouterListener_L((System.IO.IOException e) =>
                {
                    throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
                }));
                this.router_client = new com.oddlabs.tt.net.RouterClient(network, this, router.getPort());
            }
            else
            {
                this.router = null;
                this.router_client = new com.oddlabs.tt.net.RouterClient(network, com.oddlabs.tt.global.Settings.getSettings().getRouterAddress(), this);
            }
            for (short i = unchecked((short)(0)); i < players.Length; i++)
            {
                com.oddlabs.tt.player.Player player = players[i];
                if (i >= player_slots.Length || player_slots[i].getType() != com.oddlabs.tt.net.PlayerSlot.HUMAN)
                {
                    nonhuman_players.add(player);
                    continue;
                }
                java.lang.IO.println("index " + (i) + " contains player " + java.lang.JStr.valueOf(player));
                int peer_index = peer_index_to_peer_list.size();
                com.oddlabs.net.ARMIEventWriter router_handler = new com.oddlabs.net.ARMIEventWriter_L((com.oddlabs.net.ARMIEvent @event) =>
                {
                    router_client.getInterface().relayEventTo(peer_index, @event);
                });
                com.oddlabs.tt.net.PeerHubInterface peer_interface = ((com.oddlabs.tt.net.PeerHubInterface)com.oddlabs.net.ARMIEvent.createProxy(router_handler, typeof(com.oddlabs.tt.net.PeerHubInterface)));
                com.oddlabs.tt.net.Peer peer = new com.oddlabs.tt.net.Peer(this, peer_index, player, argument_reader, peer_interface);
                com.oddlabs.net.ARMIEventWriter peer_broker = default;
                if (((object)player == (object)local_player))
                {
                    this.local_peer_index = peer_index;
                }
                peer_index_to_peer_list.add(peer);
                peer_to_player.put(peer, player);
                player_to_peer.put(player, peer);
            }
            this.peer_index_to_peer = new com.oddlabs.tt.net.Peer[peer_index_to_peer_list.size()];
            peer_index_to_peer_list.toArray(peer_index_to_peer);
            this.num_participants = peer_index_to_peer.Length;
            if (is_spectator)
            {
                this.player_interface = new com.oddlabs.tt.net.NoOpPlayerInterface();
            }
            else
            {
                com.oddlabs.net.ARMIEventWriter game_router_handler = new com.oddlabs.net.ARMIEventWriter_L((com.oddlabs.net.ARMIEvent a0) => { router_client.getInterface().relayGameStateEvent(a0); });
                this.player_interface = ((com.oddlabs.tt.player.PlayerInterface)com.oddlabs.net.ARMIEvent.createProxy(game_router_handler, new com.oddlabs.tt.net.GameArgumentWriter(distributable_table), typeof(com.oddlabs.tt.player.PlayerInterface)));
            }
            com.oddlabs.net.ARMIEventWriter hub_router_handler = new com.oddlabs.net.ARMIEventWriter_L((com.oddlabs.net.ARMIEvent a0) => { router_client.getInterface().relayEvent(a0); });
            this.peerhubs_interface = ((com.oddlabs.tt.net.PeerHubInterface)com.oddlabs.net.ARMIEvent.createProxy(hub_router_handler, typeof(com.oddlabs.tt.net.PeerHubInterface)));
            this.spectatorController = (is_spectator ? new com.oddlabs.tt.net.PeerHubSpectatorController(this) : (com.oddlabs.tt.net.PeerHubSpectatorController)(null));
            manager.registerAnimation(this);
            if (is_spectator)
            {
                router_client.connectSpectator(session_id);
            }
            else
            {
                router_client.connect(session_id, new com.oddlabs.router.SessionInfo(num_participants, com.oddlabs.tt.net.PeerHub.MILLISECONDS_PER_HEARTBEAT), local_peer_index);
            }
        }
    }
}
