// Translated from ReplayWorldStarter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 24
    public class ReplayWorldStarter : java.lang.Object, com.oddlabs.tt.form.LoadCallback
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
        // Java: 49
        public com.oddlabs.tt.render.UIRenderer load(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.tt.animation.AnimationManager.freezeTime();
            java.util.List player_slot_list = new java.util.ArrayList();
            java.util.List unit_info_list = new java.util.ArrayList();
            short corrected_player_slot = unchecked((short)((-1)));
            for (short i = unchecked((short)(0)); i < player_slots.Length; i++)
            {
                if (player_slots[i].getInfo() != null)
                {
                    if (player_slot == i)
                    {
                        corrected_player_slot = unchecked((short)(player_slot_list.size()));
                    }
                    player_slot_list.add(player_slots[i]);
                    unit_info_list.add(unit_infos[i]);
                }
            }
            com.oddlabs.tt.net.PlayerSlot[] corrected_slots = ((com.oddlabs.tt.net.PlayerSlot[])player_slot_list.toArray(new com.oddlabs.tt.net.PlayerSlot[0]));
            com.oddlabs.tt.player.UnitInfo[] corrected_unit_infos = ((com.oddlabs.tt.player.UnitInfo[])unit_info_list.toArray(new com.oddlabs.tt.player.UnitInfo[0]));
            com.oddlabs.tt.viewer.WorldViewer viewer = new com.oddlabs.tt.viewer.WorldViewer(network, gui_root, world_params, ingame_info, generator, corrected_slots, corrected_unit_infos, corrected_player_slot, new com.oddlabs.router.SessionID(session_id));
            com.oddlabs.tt.gamemode.GameModeRegistry.get(viewer.getWorld().getGameMode()).onGameStart(viewer);
            if (initial_action != null)
            {
                initial_action.run(viewer);
            }
            java.lang.IO.println("ReplayWorldStarter complete (session_id = " + (session_id) + ")");
            return viewer.getRenderer();
        }
        public ReplayWorldStarter(com.oddlabs.net.NetworkSelector network, int session_id, com.oddlabs.tt.resource.WorldGenerator generator, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.net.PlayerSlot[] player_slots, com.oddlabs.tt.player.UnitInfo[] unit_infos, short player_slot, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction initial_action) : base()
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
