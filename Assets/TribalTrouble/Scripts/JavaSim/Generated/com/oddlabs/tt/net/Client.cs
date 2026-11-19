// Translated from Client.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 31
    public class Client : java.lang.Object, com.oddlabs.net.ARMIEventBroker, com.oddlabs.tt.net.GameClientInterface, com.oddlabs.net.ConnectionInterface
    {
        public const int CONNECTING = 1;
        public const int NEGOTIATING = 2;
        public const int CLOSED = 5;
        public com.oddlabs.net.AbstractConnection connection;
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.tt.landscape.WorldParameters world_params;
        public com.oddlabs.tt.net.GameServerInterface gameserver_interface;
        public com.oddlabs.tt.player.UnitInfo[] unit_infos;
        public bool[] unit_info_set;
        public com.oddlabs.tt.net.WorldInitAction initial_action;
        public com.oddlabs.tt.viewer.InGameInfo ingame_info;
        public com.oddlabs.tt.gui.GUI gui;
        public com.oddlabs.net.NetworkSelector network;
        public java.lang.Runnable cleanup_action;
        public int state;
        public int session_id;
        public com.oddlabs.tt.resource.WorldGenerator generator;
        public java.io.ByteArrayOutputStream custom_map_download;
        public int custom_map_progress_shown;
        public com.oddlabs.tt.net.PlayerSlot[] player_slots;
        public short player_slot;
        public bool error_while_fading;
        public com.oddlabs.tt.net.ConfigurationListener configuration_listener;
        // Java: 84
        public void initUnitInfos()
        {
            for (int i = 0; i < unit_infos.Length; i++)
            {
                if (unit_info_set[i])
                {
                    continue;
                }
                int peons = world_params.getInitialUnitCount();
                int rock_warriors = world_params.getStartingRockWarriors();
                int iron_warriors = world_params.getStartingIronWarriors();
                int rubber_warriors = world_params.getStartingRubberWarriors();
                int max_units = world_params.getMaxUnitCount();
                int over = peons + rock_warriors + iron_warriors + rubber_warriors - max_units;
                if (over > 0)
                {
                    int rubber_trim = java.lang.Math.min(rubber_warriors, over);
                    rubber_warriors -= rubber_trim;
                    over -= rubber_trim;
                    int iron_trim = java.lang.Math.min(iron_warriors, over);
                    iron_warriors -= iron_trim;
                    over -= iron_trim;
                    int rock_trim = java.lang.Math.min(rock_warriors, over);
                    rock_warriors -= rock_trim;
                    over -= rock_trim;
                    peons = java.lang.Math.max(0, peons - over);
                }
                unit_infos[i] = new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, peons, rock_warriors, iron_warriors, rubber_warriors);
            }
            if (((object)world_params.getGameMode() == (object)com.oddlabs.matchmaking.GameMode.ISLAND_WARS))
            {
                com.oddlabs.tt.islandwars.IslandWarsSetup iw = com.oddlabs.tt.islandwars.IslandWarsSetup.decode(world_params.getIslandWars());
                if (iw != null)
                {
                    for (int i_2 = 0; i_2 < unit_infos.Length; i_2++)
                    {
                        if (iw.hasSlot(i_2) && !unit_info_set[i_2])
                        {
                            unit_infos[i_2] = iw.unitInfo(i_2);
                        }
                    }
                }
            }
        }
        // Java: 135
        public com.oddlabs.tt.net.ConfigurationListener getConfigurationListener()
        {
            return configuration_listener;
        }
        // Java: 139
        public void setConfigurationListener(com.oddlabs.tt.net.ConfigurationListener listener)
        {
            configuration_listener = listener;
        }
        // Java: 143
        public void setUnitInfo(int slot, com.oddlabs.tt.player.UnitInfo unit_info)
        {
            this.unit_infos[slot] = unit_info;
            this.unit_info_set[slot] = true;
        }
        // Java: 148
        public void setWorldParameters(com.oddlabs.tt.landscape.WorldParameters world_params)
        {
            if (world_params == null || (state != com.oddlabs.tt.net.Client.CONNECTING && state != com.oddlabs.tt.net.Client.NEGOTIATING))
            {
                return;
            }
            this.world_params = world_params;
            initUnitInfos();
        }
        // Java: 157
        public com.oddlabs.tt.landscape.WorldParameters getWorldParameters()
        {
            return world_params;
        }
        // Java: 161
        public com.oddlabs.tt.net.GameServerInterface getServerInterface()
        {
            return gameserver_interface;
        }
        // Java: 165
        public void chat(int player_slot, string chat_v)
        {
            if (chat_v != null && player_slot >= 0 && player_slot < player_slots.Length && com.oddlabs.tt.islandwars.IslandWarsNet.lobbyChat(player_slot, chat_v))
            {
                return;
            }
            if (chat_v != null && player_slot >= 0 && player_slot < player_slots.Length)
            {
                com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage(player_slots[player_slot].getInfo().getName(), chat_v, com.oddlabs.tt.net.ChatMessage.Type.GAME_MENU));
            }
        }
        // Java: 175
        public void setWorldGeneratorAndPlayerSlot(com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, short player_slot, int player_count)
        {
            if (state != com.oddlabs.tt.net.Client.CONNECTING)
            {
                return;
            }
            state = com.oddlabs.tt.net.Client.NEGOTIATING;
            this.generator = generator;
            this.player_slot = player_slot;
            getConfigurationListener().connected(this, game, generator, player_slot, player_count);
            gameserver_interface.reportBuild(com.oddlabs.tt.net.BuildFingerprint.get());
            com.oddlabs.tt.resource.TransferableMap map = com.oddlabs.tt.resource.TransferableMap.of(generator);
            if (map != null)
            {
                if (map.hasLocalCopy())
                {
                    gameserver_interface.customMapReady();
                }
                else
                {
                    custom_map_download = new java.io.ByteArrayOutputStream();
                    custom_map_progress_shown = 0;
                    customMapNotice("Downloading the custom map \"" + java.lang.JStr.valueOf(map.getMapFileName()) + "\" from the host...");
                    gameserver_interface.requestCustomMap();
                }
            }
        }
        // Java: 199
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 203
        public void close()
        {
            connection.close();
            state = com.oddlabs.tt.net.Client.CLOSED;
            if (cleanup_action != null)
            {
                cleanup_action.run();
            }
        }
        // Java: 210
        public com.oddlabs.tt.net.PlayerSlot[] getPlayers()
        {
            return player_slots;
        }
        // Java: 214
        public void startGame(int session_id)
        {
            if (state != com.oddlabs.tt.net.Client.NEGOTIATING)
            {
                return;
            }
            close();
            this.session_id = session_id;
            getConfigurationListener().gameStarted();
            com.oddlabs.tt.form.ProgressForm.setProgressForm(network, gui, new com.oddlabs.tt.net.WorldStarter(network, session_id, generator, world_params, player_slots, unit_infos, player_slot, ingame_info, initial_action));
        }
        // Java: 225
        public void setPlayers(com.oddlabs.tt.net.PlayerSlot[] player_slots)
        {
            this.player_slots = player_slots;
            foreach (com.oddlabs.tt.net.PlayerSlot playerSlot in player_slots)
            {
                if (playerSlot == null)
                {
                    error();
                    return;
                }
            }
            getConfigurationListener().setPlayers(player_slots);
        }
        // Java: 237
        public void handle(object sender, com.oddlabs.net.ARMIEvent armi_event)
        {
            try
            {
                armi_event.execute(interface_methods, this);
            }
            catch (com.oddlabs.net.IllegalARMIEventException unused)
            {
                error();
            }
        }
        // Java: 246
        public void connected(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 250
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            error();
        }
        // Java: 255
        public void error()
        {
            getConfigurationListener().connectionLost();
            close();
        }
        // Java: 260
        public void receiveCustomMapChunk(int total_bytes, sbyte[] data)
        {
            com.oddlabs.tt.resource.TransferableMap map = com.oddlabs.tt.resource.TransferableMap.of(generator);
            if (state != com.oddlabs.tt.net.Client.NEGOTIATING || custom_map_download == null || map == null)
            {
                return;
            }
            if (total_bytes < 0 || data == null)
            {
                custom_map_download = null;
                customMapNotice("The host couldn't send the custom map. Ask them for " + java.lang.JStr.valueOf(map.getMapFileName()) + " and put it in " + java.lang.JStr.valueOf(com.oddlabs.tt.resource.CustomMapGenerator.sharedMapsDir()) + ", then join again.");
                return;
            }
            custom_map_download.write(data, 0, data.Length);
            int received = custom_map_download.size();
            int percent = (total_bytes == 0 ? 100 : unchecked((int)((100L * received / total_bytes))));
            if (received < total_bytes)
            {
                gameserver_interface.customMapChunkReceived();
                if (percent >= custom_map_progress_shown + 25)
                {
                    custom_map_progress_shown = percent - percent % 25;
                    customMapNotice("Downloading the custom map... " + (custom_map_progress_shown) + "%");
                }
                return;
            }
            sbyte[] compressed = custom_map_download.toByteArray();
            custom_map_download = null;
            try
            {
                java.nio.file.Path saved = map.installDownloadedMap(compressed);
                customMapNotice("Custom map received and saved to " + java.lang.JStr.valueOf(saved));
                gameserver_interface.customMapReady();
            }
            catch (System.Exception e) when (e is System.IO.IOException || e is System.Exception)
            {
                customMapNotice("Couldn't save the custom map: " + java.lang.JStr.valueOf(e.getMessage()));
            }
        }
        // Java: 293
        public void customMapNotice(string text)
        {
            if (text != null)
            {
                com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage("Custom map", text, com.oddlabs.tt.net.ChatMessage.Type.GAME_MENU));
            }
        }
        // Java: 299
        public void lobbyNotice(string label, string text)
        {
            if (label != null && text != null)
            {
                com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage(label, text, com.oddlabs.tt.net.ChatMessage.Type.GAME_MENU));
            }
        }
        private void init_Client()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.tt.net.GameClientInterface));
            this.state = com.oddlabs.tt.net.Client.CONNECTING;
            this.generator = null;
            this.player_slot = unchecked((short)((-1)));
        }
        public Client(java.lang.Runnable cleanup_action, com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui, int host_id, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction initial_action) : base()
        {
            init_Client();
            this.cleanup_action = cleanup_action;
            this.network = network;
            this.gui = gui;
            this.ingame_info = ingame_info;
            this.world_params = world_params;
            this.initial_action = initial_action;
            if (host_id != (-1))
            {
                this.connection = new com.oddlabs.tt.net.TunnelledConnection(host_id, this);
            }
            else
            {
                this.connection = new com.oddlabs.net.Connection(network, new java.net.InetSocketAddress(com.oddlabs.util.Utils.getLoopbackAddress(), com.oddlabs.tt.global.Globals.NET_PORT), this);
            }
            gameserver_interface = ((com.oddlabs.tt.net.GameServerInterface)com.oddlabs.net.ARMIEvent.createProxy(connection, typeof(com.oddlabs.tt.net.GameServerInterface)));
            this.unit_infos = new com.oddlabs.tt.player.UnitInfo[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            this.unit_info_set = new bool[unit_infos.Length];
            initUnitInfos();
        }
    }
}
