// Translated from Client.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 27
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
        public com.oddlabs.tt.net.WorldInitAction initial_action;
        public com.oddlabs.tt.viewer.InGameInfo ingame_info;
        public com.oddlabs.tt.gui.GUI gui;
        public com.oddlabs.net.NetworkSelector network;
        public java.lang.Runnable cleanup_action;
        public int state;
        public int session_id;
        public com.oddlabs.tt.resource.WorldGenerator generator;
        public com.oddlabs.tt.net.PlayerSlot[] player_slots;
        public short player_slot;
        public bool error_while_fading;
        public com.oddlabs.tt.net.ConfigurationListener configuration_listener;
        // Java: 75
        public com.oddlabs.tt.net.ConfigurationListener getConfigurationListener()
        {
            return configuration_listener;
        }
        // Java: 79
        public void setConfigurationListener(com.oddlabs.tt.net.ConfigurationListener listener)
        {
            configuration_listener = listener;
        }
        // Java: 83
        public void setUnitInfo(int slot, com.oddlabs.tt.player.UnitInfo unit_info)
        {
            this.unit_infos[slot] = unit_info;
        }
        // Java: 87
        public com.oddlabs.tt.net.GameServerInterface getServerInterface()
        {
            return gameserver_interface;
        }
        // Java: 91
        public void chat(int player_slot, string chat_v)
        {
            if (chat_v != null && player_slot >= 0 && player_slot < player_slots.Length)
            {
                com.oddlabs.tt.net.Network.getChatHub().chat(new com.oddlabs.tt.net.ChatMessage(player_slots[player_slot].getInfo().getName(), chat_v, com.oddlabs.tt.net.ChatMessage.Type.GAME_MENU));
            }
        }
        // Java: 98
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
        }
        // Java: 109
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 113
        public void close()
        {
            connection.close();
            state = com.oddlabs.tt.net.Client.CLOSED;
            if (cleanup_action != null)
            {
                cleanup_action.run();
            }
        }
        // Java: 120
        public com.oddlabs.tt.net.PlayerSlot[] getPlayers()
        {
            return player_slots;
        }
        // Java: 124
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
        // Java: 135
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
        // Java: 147
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
        // Java: 156
        public void connected(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 160
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            error();
        }
        // Java: 165
        public void error()
        {
            getConfigurationListener().connectionLost();
            close();
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
            for (int i = 0; i < unit_infos.Length; i++)
            {
                unit_infos[i] = new com.oddlabs.tt.player.UnitInfo(false, false, 0, false, world_params.getInitialUnitCount(), 0, 0, 0);
            }
        }
    }
}
