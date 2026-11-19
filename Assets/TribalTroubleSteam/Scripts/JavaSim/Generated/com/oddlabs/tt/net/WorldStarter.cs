// Translated from WorldStarter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 29
    public class WorldStarter : java.lang.Object, com.oddlabs.tt.form.LoadCallback
    {
        public com.oddlabs.tt.player.UnitInfo[] unit_infos;
        public com.oddlabs.tt.net.PlayerSlot[] player_slots;
        public short player_slot;
        public com.oddlabs.tt.viewer.InGameInfo ingame_info;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.resource.WorldGenerator generator;
        public com.oddlabs.tt.landscape.WorldParameters world_params;
        public com.oddlabs.tt.net.WorldInitAction initial_action;
        public int session_id;
        // Java: 54
        public com.oddlabs.tt.render.UIRenderer load(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.tt.animation.AnimationManager.freezeTime();
            java.util.List player_slot_list = new java.util.ArrayList();
            java.util.List unit_info_list = new java.util.ArrayList();
            java.util.List color_list = new java.util.ArrayList();
            short corrected_player_slot = unchecked((short)((-1)));
            for (short i = unchecked((short)(0)); i < this.player_slots.Length; i++)
            {
                if (this.player_slots[i].getInfo() != null)
                {
                    if (player_slot == i)
                    {
                        corrected_player_slot = unchecked((short)(player_slot_list.size()));
                    }
                    player_slot_list.add(this.player_slots[i]);
                    unit_info_list.add(unit_infos[i]);
                    color_list.add(com.oddlabs.tt.global.Settings.getSettings().team_colours[i]);
                }
            }
            com.oddlabs.tt.net.PlayerSlot[] player_slots = ((com.oddlabs.tt.net.PlayerSlot[])player_slot_list.toArray(new com.oddlabs.tt.net.PlayerSlot[0]));
            com.oddlabs.tt.player.UnitInfo[] corrected_unit_infos = ((com.oddlabs.tt.player.UnitInfo[])unit_info_list.toArray(new com.oddlabs.tt.player.UnitInfo[0]));
            org.joml.Vector4fc[] colors = ((org.joml.Vector4fc[])color_list.toArray(new org.joml.Vector4fc[0]));
            com.oddlabs.tt.viewer.WorldViewer viewer = new com.oddlabs.tt.viewer.WorldViewer(network, gui_root, world_params, ingame_info, generator, player_slots, corrected_unit_infos, corrected_player_slot, new com.oddlabs.router.SessionID(session_id), colors);
            com.oddlabs.tt.gamemode.GameModeRegistry.get(viewer.getWorld().getGameMode()).onGameStart(viewer);
            if (initial_action != null)
            {
                initial_action.run(viewer);
            }
            com.oddlabs.matchmaking.Participant[] participants = com.oddlabs.tt.net.WorldStarter.getParticipants(viewer, player_slots);
            com.oddlabs.matchmaking.GamePlayer[] gamePlayers = com.oddlabs.tt.net.WorldStarter.getGamePlayers(viewer, player_slots);
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                com.oddlabs.matchmaking.GameSession game_session = new com.oddlabs.matchmaking.GameSession(session_id, participants, ingame_info.isRated(), gamePlayers);
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().gameStartedNotify(game_session);
                sendWorldParams(player_slots, corrected_unit_infos);
                com.oddlabs.tt.steam.SteamManager.setMultiplayerInGameRichPresence();
            }
            com.oddlabs.tt.steam.SteamManager.setInActiveWorld(true);
            java.lang.IO.println("PeerHub created (session_id = " + (session_id) + ") Player list:");
            return viewer.getRenderer();
        }
        // Java: 92
        public void sendWorldParams(com.oddlabs.tt.net.PlayerSlot[] player_slots, com.oddlabs.tt.player.UnitInfo[] unit_infos)
        {
            try
            {
                java.io.ByteArrayOutputStream baos = new java.io.ByteArrayOutputStream();
                java.io.ObjectOutputStream oos = new java.io.ObjectOutputStream(baos);
                oos.writeObject(generator);
                oos.writeObject(world_params);
                oos.writeObject(player_slots);
                oos.writeObject(unit_infos);
                oos.writeFloat(ingame_info.getRandomStartPosition());
                oos.writeInt(session_id);
                oos.close();
                java.lang.IO.println("sendWorldParams: serialized " + (baos.size()) + " bytes, sending to server");
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateWorldParams(baos.toByteArray());
            }
            catch (System.Exception e)
            {
                java.lang.IO.println("Exception serializing world params: " + java.lang.JStr.valueOf(e));
                e.printStackTrace();
            }
        }
        // Java: 111
        public static com.oddlabs.matchmaking.Participant[] getParticipants(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.net.PlayerSlot[] player_slots)
        {
            java.util.List participant_list = new java.util.ArrayList();
            com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
            for (short i = unchecked((short)(0)); i < players.Length; i++)
            {
                if (player_slots[i].getType() != com.oddlabs.tt.net.PlayerSlot.HUMAN)
                {
                    continue;
                }
                com.oddlabs.tt.player.Player player = players[i];
                int host_id = (player_slots[i].getAddress() != null ? player_slots[i].getAddress().getHostID() : (-1));
                participant_list.add(new com.oddlabs.matchmaking.Participant(host_id, player.getPlayerInfo().getName(), player.getPlayerInfo().getTeam(), player.getPlayerInfo().getRace()));
            }
            return ((com.oddlabs.matchmaking.Participant[])participant_list.toArray(new com.oddlabs.matchmaking.Participant[0]));
        }
        // Java: 126
        public static com.oddlabs.matchmaking.GamePlayer[] getGamePlayers(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.net.PlayerSlot[] player_slots)
        {
            com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
            com.oddlabs.matchmaking.GamePlayer[] gamePlayers = new com.oddlabs.matchmaking.GamePlayer[players.Length];
            for (short i = unchecked((short)(0)); i < players.Length; i++)
            {
                com.oddlabs.tt.player.Player player = players[i];
                com.oddlabs.matchmaking.PlayerTypes playerType = com.oddlabs.tt.net.WorldStarter.mapPlayerType(player_slots[i]);
                string name = (player_slots[i].getType() == com.oddlabs.tt.net.PlayerSlot.HUMAN ? player.getPlayerInfo().getName() : (string)(null));
                gamePlayers[i] = new com.oddlabs.matchmaking.GamePlayer(name, player.getPlayerInfo().getTeam(), player.getPlayerInfo().getRace(), playerType);
            }
            return gamePlayers;
        }
        // Java: 140
        public static com.oddlabs.matchmaking.PlayerTypes mapPlayerType(com.oddlabs.tt.net.PlayerSlot slot)
        {
            if (slot.getType() == com.oddlabs.tt.net.PlayerSlot.HUMAN)
            {
                return com.oddlabs.matchmaking.PlayerTypes.Human;
            }
            return ((slot.getAIDifficulty()) switch { 1 => (com.oddlabs.matchmaking.PlayerTypes)(com.oddlabs.matchmaking.PlayerTypes.AIEasy), 2 => (com.oddlabs.matchmaking.PlayerTypes)(com.oddlabs.matchmaking.PlayerTypes.AINormal), 3 => (com.oddlabs.matchmaking.PlayerTypes)(com.oddlabs.matchmaking.PlayerTypes.AIHard), _ => (com.oddlabs.matchmaking.PlayerTypes)(com.oddlabs.matchmaking.PlayerTypes.None) });
        }
        public WorldStarter(com.oddlabs.net.NetworkSelector network, int session_id, com.oddlabs.tt.resource.WorldGenerator generator, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.net.PlayerSlot[] player_slots, com.oddlabs.tt.player.UnitInfo[] unit_infos, short player_slot, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction initial_action) : base()
        {
            this.initial_action = initial_action;
            this.session_id = session_id;
            this.world_params = world_params;
            this.generator = generator;
            this.unit_infos = unit_infos;
            this.player_slots = player_slots;
            this.player_slot = player_slot;
            this.ingame_info = ingame_info;
            this.network = network;
        }
    }
}
