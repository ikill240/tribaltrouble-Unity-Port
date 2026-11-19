// Translated from MatchmakingClient.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 42
    public class MatchmakingClient : java.lang.Object, com.oddlabs.matchmaking.MatchmakingClientInterface, com.oddlabs.net.ConnectionInterface
    {
        public const int STATE_NOT_CONNECTED = 1;
        public const int STATE_AWAITING_OK = 2;
        public const int STATE_LOGGED_IN = 4;
        public java.util.Map tunnels;
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.tt.net.ChatRoomHistory chat_room_history;
        public com.oddlabs.tt.net.InGameChatHistory in_game_chat_history;
        public com.oddlabs.tt.gui.GUIRoot chat_gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public int current_seq_id;
        public com.oddlabs.net.SecureConnection conn;
        public com.oddlabs.matchmaking.MatchmakingServerInterface matchmaking_interface;
        public com.oddlabs.matchmaking.MatchmakingServerLoginInterface matchmaking_login_interface;
        public com.oddlabs.tt.net.TunnelledConnectionListener tunnelled_listener;
        public com.oddlabs.matchmaking.TunnelAddress local_address;
        public string username;
        public com.oddlabs.matchmaking.Profile active_profile;
        public int state;
        public bool update_allowed;
        public java.util.Set update_requested_types;
        public int update_key;
        public com.oddlabs.tt.net.ProfileListener create_profile_listener;
        public com.oddlabs.tt.gui.ChatRoomInfo chat_room_info;
        public com.oddlabs.matchmaking.Login login_f;
        public com.oddlabs.matchmaking.LoginDetails login_details;
        public bool steamLogin;
        // Java: 80
        public java.util.List getChatRoomHistory()
        {
            return chat_room_history.getMessages();
        }
        // Java: 84
        public java.util.List getInGameChatHistory()
        {
            return in_game_chat_history.getMessages();
        }
        // Java: 88
        public void clearInGameChatHistory()
        {
            in_game_chat_history.clear();
        }
        // Java: 92
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 96
        public void loginOK(string username, com.oddlabs.matchmaking.TunnelAddress address)
        {
            if (state == com.oddlabs.tt.net.MatchmakingClient.STATE_AWAITING_OK)
            {
                this.update_allowed = true;
                this.username = username;
                this.local_address = address;
                this.matchmaking_login_interface = null;
                this.matchmaking_interface = ((com.oddlabs.matchmaking.MatchmakingServerInterface)com.oddlabs.net.ARMIEvent.createProxy(conn.getWrappedConnectionAndShutdown(), typeof(com.oddlabs.matchmaking.MatchmakingServerInterface)));
                state = com.oddlabs.tt.net.MatchmakingClient.STATE_LOGGED_IN;
                com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
                listener.loggedIn();
            }
        }
        // Java: 111
        public void requestProfiles()
        {
            matchmaking_interface.requestProfiles();
        }
        // Java: 116
        public void requestList(int type)
        {
            if (update_allowed)
            {
                update_allowed = false;
                matchmaking_interface.requestList(type, update_key);
            }
            else
            {
                update_requested_types.add(type);
            }
        }
        // Java: 124
        public com.oddlabs.matchmaking.Profile getProfile()
        {
            return active_profile;
        }
        // Java: 135
        public void setProfile(string nick)
        {
            matchmaking_interface.setProfile(nick);
        }
        // Java: 139
        public void updateProfile(com.oddlabs.matchmaking.Profile profile)
        {
            active_profile = profile;
            com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
            if (listener != null)
            {
                listener.profileUpdated();
            }
        }
        // Java: 148
        public void setCreatingProfileListener(com.oddlabs.tt.net.ProfileListener listener)
        {
            create_profile_listener = listener;
        }
        // Java: 152
        public void createProfileSuccess()
        {
            if (create_profile_listener != null)
            {
                create_profile_listener.success();
                create_profile_listener = null;
            }
        }
        // Java: 160
        public void createProfileError(int error_code)
        {
            if (create_profile_listener != null)
            {
                create_profile_listener.error(error_code);
                create_profile_listener = null;
            }
        }
        // Java: 168
        public void createProfile(string nick)
        {
            matchmaking_interface.createProfile(nick);
        }
        // Java: 172
        public void deleteProfile(string nick)
        {
            matchmaking_interface.deleteProfile(nick);
        }
        // Java: 176
        public void logPriority(string nick, int priority)
        {
            matchmaking_interface.logPriority(nick, priority);
        }
        // Java: 180
        public void gameWonAck()
        {
            com.oddlabs.tt.net.PeerHub.receivedAck();
        }
        // Java: 185
        public void updateStart(int type)
        {
            com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
            if (listener != null)
            {
                listener.clearList(type);
            }
        }
        // Java: 193
        public void updateList(int type, object[] names)
        {
            if (type == com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_GAME)
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.gamesListed(names);
            }
            com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
            if (listener != null)
            {
                listener.receivedList(type, names);
            }
        }
        // Java: 202
        public void updateComplete(int next_update_key)
        {
            this.update_key = next_update_key;
            update_allowed = true;
            if (!update_requested_types.isEmpty())
            {
                java.util.Iterator it = update_requested_types.iterator();
                int type = ((int)((int?)it.next()));
                it.remove();
                requestList(type);
            }
        }
        // Java: 214
        public void updateProfileList(com.oddlabs.matchmaking.Profile[] profiles, string last_profile_nick)
        {
            com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
            if (listener != null)
            {
                listener.receivedProfiles(profiles, last_profile_nick);
            }
        }
        // Java: 221
        public void joiningChatRoom(string room_name)
        {
            chat_room_info = new com.oddlabs.tt.gui.ChatRoomInfo(room_name, null);
            chat_room_history.clear();
            com.oddlabs.tt.steam.SteamManager.setLobbyRichPresence(room_name);
            com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
            if (listener != null)
            {
                listener.joinedChat(chat_room_info);
            }
        }
        // Java: 233
        public void receiveChatRoomUsers(com.oddlabs.matchmaking.ChatRoomUser[] users)
        {
            if (chat_room_info != null)
            {
                chat_room_info = new com.oddlabs.tt.gui.ChatRoomInfo(chat_room_info.name(), users);
                com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
                chat_room_history.update(chat_room_info.users());
                if (listener != null)
                {
                    listener.updateChatRoom(chat_room_info);
                }
            }
        }
        // Java: 245
        public void receiveChatRoomMessage(string owner, string msg)
        {
            com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage(owner, msg, com.oddlabs.tt.net.ChatMessage.Type.CHATROOM));
        }
        // Java: 250
        public void receivePrivateMessage(string nick, string msg)
        {
            if (com.oddlabs.tt.islandwars.IslandWarsNet.privateMessage(nick, msg))
            {
                return;
            }
            com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage(nick, msg, com.oddlabs.tt.net.ChatMessage.Type.PRIVATE));
        }
        // Java: 257
        public void sendPrivateMessage(com.oddlabs.tt.gui.GUIRoot gui_root, string nick, string message)
        {
            this.chat_gui_root = gui_root;
            getInterface().sendPrivateMessage(nick, message);
        }
        // Java: 262
        public void joinRoom(com.oddlabs.tt.gui.GUIRoot gui_root, string name)
        {
            this.chat_gui_root = gui_root;
            getInterface().joinRoom(name);
        }
        // Java: 267
        public void requestInfo(com.oddlabs.tt.gui.GUIRoot gui_root, string nick)
        {
            this.chat_gui_root = gui_root;
            getInterface().requestInfo(nick);
        }
        // Java: 272
        public void receiveInfo(com.oddlabs.matchmaking.Profile profile)
        {
            if (chat_gui_root != null)
            {
                chat_gui_root.addModalForm(new com.oddlabs.tt.form.InfoForm(profile));
                chat_gui_root = null;
            }
        }
        // Java: 280
        public void leaveChatRoom()
        {
            if (isConnected())
            {
                getInterface().leaveRoom();
            }
            chat_room_info = null;
            com.oddlabs.tt.steam.SteamManager.clearRichPresence();
        }
        // Java: 287
        public com.oddlabs.tt.gui.ChatRoomInfo getChatRoomInfo()
        {
            return chat_room_info;
        }
        // Java: 291
        public void error(int error_code)
        {
            if (chat_gui_root != null)
            {
                chat_gui_root.addModalForm(new com.oddlabs.tt.form.ChatErrorForm(error_code));
                chat_gui_root = null;
            }
        }
        // Java: 299
        public bool isConnected()
        {
            return state == com.oddlabs.tt.net.MatchmakingClient.STATE_LOGGED_IN;
        }
        // Java: 303
        public void open(com.oddlabs.net.NetworkSelector network)
        {
            close();
            this.network = network;
            this.conn = new com.oddlabs.net.SecureConnection(network.getDeterministic(), new com.oddlabs.net.Connection(network, com.oddlabs.tt.global.Settings.getSettings().getMatchmakingAddress(), com.oddlabs.matchmaking.MatchmakingServerInterface.MATCHMAKING_SERVER_PORT, this), null);
            this.matchmaking_login_interface = ((com.oddlabs.matchmaking.MatchmakingServerLoginInterface)com.oddlabs.net.ARMIEvent.createProxy(conn, typeof(com.oddlabs.matchmaking.MatchmakingServerLoginInterface)));
        }
        // Java: 313
        public void login(com.oddlabs.net.NetworkSelector network, com.oddlabs.matchmaking.Login login_v, com.oddlabs.matchmaking.LoginDetails login_details)
        {
            this.login_f = login_v;
            this.login_details = login_details;
            this.steamLogin = false;
            open(network);
            state = com.oddlabs.tt.net.MatchmakingClient.STATE_AWAITING_OK;
        }
        // Java: 321
        public void loginWithSteam(com.oddlabs.net.NetworkSelector network)
        {
            this.login_f = null;
            this.login_details = null;
            this.steamLogin = true;
            com.oddlabs.tt.steam.SteamManager steam = com.oddlabs.tt.steam.SteamManager.getInstance();
            if (steam != null)
            {
                steam.requestWebApiTicket();
            }
            open(network);
            state = com.oddlabs.tt.net.MatchmakingClient.STATE_AWAITING_OK;
        }
        // Java: 333
        public void loginError(int error_code)
        {
            close();
            com.oddlabs.tt.net.MatchmakingListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
            listener.loginError(error_code);
        }
        // Java: 340
        public void requestSpectate(com.oddlabs.tt.gui.GUIRoot gui_root, string nick)
        {
            this.chat_gui_root = gui_root;
            getInterface().requestSpectate(nick);
        }
        // Java: 345
        public void receiveSpectatorData(sbyte[] world_params_data)
        {
            if (chat_gui_root == null || network == null)
            {
                return;
            }
            try
            {
                java.io.ByteArrayInputStream bais = new java.io.ByteArrayInputStream(world_params_data);
                java.io.ObjectInputStream ois = new java.io.ObjectInputStream(bais);
                ois.setObjectInputFilter(java.io.ObjectInputFilter.Config.createFilter("com.oddlabs.**;java.lang.*;java.net.*;!*"));
                com.oddlabs.tt.resource.WorldGenerator generator = ((com.oddlabs.tt.resource.WorldGenerator)ois.readObject());
                com.oddlabs.tt.landscape.WorldParameters world_params = ((com.oddlabs.tt.landscape.WorldParameters)ois.readObject());
                com.oddlabs.tt.net.PlayerSlot[] player_slots = ((com.oddlabs.tt.net.PlayerSlot[])ois.readObject());
                com.oddlabs.tt.player.UnitInfo[] unit_infos = ((com.oddlabs.tt.player.UnitInfo[])ois.readObject());
                float random_start_position = ois.readFloat();
                int session_id = ois.readInt();
                ois.close();
                com.oddlabs.tt.gui.GUI gui = chat_gui_root.getGUI();
                com.oddlabs.tt.form.ProgressForm.setProgressForm(network, gui, new com.oddlabs.tt.net.ReplayWorldStarter(network, session_id, generator, world_params, player_slots, unit_infos, unchecked((short)(0)), new com.oddlabs.tt.viewer.SpectatorInGameInfo(random_start_position), new com.oddlabs.tt.net.SpectatorWorldInitAction()));
                chat_gui_root = null;
            }
            catch (System.Exception e)
            {
                java.lang.IO.println("Failed to deserialize spectator data: " + java.lang.JStr.valueOf(e));
                error(com.oddlabs.matchmaking.MatchmakingClientInterface.CHAT_ERROR_SPECTATE_FAILED);
            }
        }
        public java.io.ByteArrayOutputStream pendingSpectatorEventLog;
        public int pendingSpectatorEventLogTotal;
        // Java: 377
        public void receiveSpectatorEventLog(sbyte[] chunk, int chunk_index, int total_chunks, int current_tick)
        {
            if (chunk_index == 0)
            {
                pendingSpectatorEventLog = new java.io.ByteArrayOutputStream();
                pendingSpectatorEventLogTotal = total_chunks;
            }
            if (pendingSpectatorEventLog == null)
            {
                return;
            }
            pendingSpectatorEventLog.write(chunk, 0, chunk.Length);
            if (chunk_index == pendingSpectatorEventLogTotal - 1)
            {
                sbyte[] full = pendingSpectatorEventLog.toByteArray();
                pendingSpectatorEventLog = null;
                com.oddlabs.tt.net.PeerHubSpectatorController controller = com.oddlabs.tt.net.PeerHubSpectatorController.getInstance();
                if (controller == null)
                {
                    return;
                }
                controller.fastForward(full, current_tick);
            }
        }
        // Java: 394
        public com.oddlabs.matchmaking.MatchmakingServerLoginInterface getLoginInterface()
        {
            return matchmaking_login_interface;
        }
        // Java: 399
        public com.oddlabs.matchmaking.MatchmakingServerInterface getInterface()
        {
            return matchmaking_interface;
        }
        // Java: 404
        public com.oddlabs.matchmaking.TunnelAddress getLocalAddress()
        {
            return local_address;
        }
        // Java: 408
        public string getUsername()
        {
            return username;
        }
        // Java: 412
        public void registerTunnel(com.oddlabs.net.HostSequenceID host_seq, com.oddlabs.tt.net.TunnelledConnection conn)
        {
            object old = ((com.oddlabs.tt.net.TunnelledConnection)tunnels.put(host_seq, conn));
        }
        // Java: 417
        public com.oddlabs.net.HostSequenceID registerTunnel(int address, com.oddlabs.tt.net.TunnelledConnection conn)
        {
            int seq_id = current_seq_id++;
            com.oddlabs.net.HostSequenceID host_seq = new com.oddlabs.net.HostSequenceID(local_address.getHostID(), seq_id);
            matchmaking_interface.openTunnel(address, seq_id);
            registerTunnel(host_seq, conn);
            return host_seq;
        }
        // Java: 425
        public void unregisterTunnel(com.oddlabs.net.HostSequenceID host_seq, com.oddlabs.tt.net.TunnelledConnection conn)
        {
            if (isConnected())
            {
                object old = ((com.oddlabs.tt.net.TunnelledConnection)tunnels.remove(host_seq));
                closeTunnel(host_seq);
            }
        }
        // Java: 433
        public void closeTunnel(com.oddlabs.net.HostSequenceID host_seq)
        {
            matchmaking_interface.closeTunnel(host_seq);
        }
        // Java: 437
        public void registerTunnelledListener(com.oddlabs.tt.net.TunnelledConnectionListener listener)
        {
            this.tunnelled_listener = listener;
        }
        // Java: 442
        public void unregisterTunnelledListener(com.oddlabs.tt.net.TunnelledConnectionListener listener)
        {
            tunnelled_listener = null;
        }
        // Java: 447
        public com.oddlabs.tt.net.TunnelledConnection removeTunnel(com.oddlabs.net.HostSequenceID from)
        {
            return ((com.oddlabs.tt.net.TunnelledConnection)tunnels.remove(from));
        }
        // Java: 451
        public com.oddlabs.tt.net.TunnelledConnection getTunnel(com.oddlabs.net.HostSequenceID from)
        {
            return ((com.oddlabs.tt.net.TunnelledConnection)tunnels.get(from));
        }
        // Java: 455
        public void tunnelClosed(com.oddlabs.net.HostSequenceID from)
        {
            com.oddlabs.tt.net.TunnelledConnection tunnel = removeTunnel(from);
            if (tunnel != null)
            {
                tunnel.tunnelClosed();
            }
        }
        // Java: 462
        public void tunnelAccepted(com.oddlabs.net.HostSequenceID from)
        {
            com.oddlabs.tt.net.TunnelledConnection tunnel = getTunnel(from);
            if (tunnel != null)
            {
                tunnel.connected();
            }
            else
            {
                closeTunnel(from);
            }
        }
        // Java: 471
        public void tunnelOpened(com.oddlabs.net.HostSequenceID from, java.net.InetAddress inet_from, java.net.InetAddress local_inet_from, com.oddlabs.matchmaking.Profile other)
        {
            if (tunnelled_listener != null)
            {
                tunnelled_listener.requestTunnelledConnection(from, inet_from, local_inet_from, other);
            }
            else
            {
                closeTunnel(from);
            }
        }
        // Java: 480
        public void receiveRoutedEvent(com.oddlabs.net.HostSequenceID from, com.oddlabs.net.ARMIEvent @event)
        {
            com.oddlabs.tt.net.TunnelledConnection tunnel = getTunnel(from);
            if (tunnel != null)
            {
                tunnel.receiveEvent(@event);
            }
            else
            {
                closeTunnel(from);
            }
        }
        // Java: 489
        public void handle(object sender, com.oddlabs.net.ARMIEvent @event)
        {
            try
            {
                @event.execute(interface_methods, this);
            }
            catch (com.oddlabs.net.IllegalARMIEventException e)
            {
                handleError(e);
            }
        }
        // Java: 498
        public void handleError(System.Exception e)
        {
            com.oddlabs.tt.net.ErrorListener listener = com.oddlabs.tt.net.Network.getMatchmakingListener();
            close();
            if (listener != null)
            {
                listener.connectionLost();
            }
        }
        // Java: 505
        public void connected(com.oddlabs.net.AbstractConnection connection)
        {
            com.oddlabs.net.Connection wrapped_connection = ((com.oddlabs.net.Connection)conn.getWrappedConnection());
            matchmaking_login_interface.setLocalRemoteAddress(wrapped_connection.getLocalAddress());
            java.lang.IO.println("wrapped_connection.getLocalAddress() = " + java.lang.JStr.valueOf(wrapped_connection.getLocalAddress()));
            int revision = com.oddlabs.util.Compatibility.API_VERSION;
            matchmaking_login_interface.setSimVersion(com.oddlabs.util.Compatibility.SIM_VERSION);
            if (steamLogin)
            {
                com.oddlabs.tt.steam.SteamManager steam = com.oddlabs.tt.steam.SteamManager.getInstance();
                if (steam == null)
                {
                    loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_NO_SUCH_USER);
                    return;
                }
                if (!steam.awaitWebApiTicket(3000))
                {
                    handleError(new System.IO.IOException("Steam Web API ticket not ready \u2014 Steam may not be running properly"));
                    return;
                }
                long accountId = steam.getAccountID();
                string personaName = steam.getPersonaName();
                sbyte[] authTicket = steam.getWebApiTicket();
                if (authTicket == null)
                {
                    handleError(new System.IO.IOException("Steam Web API ticket was null after awaiting"));
                    return;
                }
                matchmaking_login_interface.loginWithSteam(accountId, personaName, authTicket, steam.getAppID(), revision);
            }
            else if (!com.oddlabs.tt.render.Renderer.isRegistered())
            {
                matchmaking_login_interface.loginAsGuest(revision);
            }
            else if (login_details != null)
            {
                matchmaking_login_interface.createUser(login_f, login_details, null, revision);
            }
            else
            {
                matchmaking_login_interface.login(login_f, null, revision);
            }
        }
        // Java: 540
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            handleError(e);
        }
        // Java: 545
        public void close()
        {
            if (tunnelled_listener != null)
            {
                tunnelled_listener.connectionClosed();
                tunnelled_listener = null;
            }
            foreach (com.oddlabs.tt.net.TunnelledConnection tunnel in java.lang.JIter.of(tunnels.values()))
            {
                tunnel.tunnelClosed();
            }
            tunnels.clear();
            if (conn != null)
            {
                conn.close();
                conn = null;
            }
            if (!steamLogin)
            {
                com.oddlabs.tt.steam.SteamManager steam = com.oddlabs.tt.steam.SteamManager.getInstance();
                if (steam != null)
                {
                    steam.cancelAuthTicket();
                }
            }
            state = com.oddlabs.tt.net.MatchmakingClient.STATE_NOT_CONNECTED;
            matchmaking_interface = null;
            active_profile = null;
            chat_room_info = null;
            com.oddlabs.tt.steam.SteamManager.clearRichPresence();
        }
        private void init_MatchmakingClient()
        {
            this.tunnels = new java.util.HashMap();
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.matchmaking.MatchmakingClientInterface));
            this.current_seq_id = 1;
            this.username = com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.net.MatchmakingClient).getName()), "player");
            this.active_profile = null;
            this.state = com.oddlabs.tt.net.MatchmakingClient.STATE_NOT_CONNECTED;
            this.update_requested_types = new java.util.LinkedHashSet();
            this.update_key = 0;
        }
        public MatchmakingClient() : base()
        {
            init_MatchmakingClient();
            this.chat_room_history = new com.oddlabs.tt.net.ChatRoomHistory();
            this.in_game_chat_history = new com.oddlabs.tt.net.InGameChatHistory();
            com.oddlabs.tt.net.Network.getChatHub().addListener(chat_room_history);
            com.oddlabs.tt.net.Network.getChatHub().addListener(in_game_chat_history);
        }
    }
}
