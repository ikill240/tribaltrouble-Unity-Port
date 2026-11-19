// Translated from NativeCampaign.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 16
    public class NativeCampaign : com.oddlabs.tt.player.campaign.Campaign
    {
        public const int MAX_UNITS = 41;
        public static int[] INITIAL_STATES;
        public com.oddlabs.tt.player.campaign.Island[] islands;
        // Java: 55
        public override com.oddlabs.tt.gui.CampaignIcons getIcons()
        {
            return com.oddlabs.tt.gui.NativeCampaignIcons.getIcons();
        }
        // Java: 60
        public override void islandChosen(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number)
        {
            if (com.oddlabs.tt.render.Renderer.isRegistered())
            {
                com.oddlabs.tt.gui.Form dialog = new com.oddlabs.tt.form.CampaignDialogForm(islands[number].getHeader(), islands[number].getDescription(), null, com.oddlabs.tt.gui.Origin.AT_START, new java.lang.Runnable_L(() =>
                {
                    startIsland(network, gui_root, number);
                }), true);
                gui_root.addModalForm(dialog);
            }
        }
        // Java: 72
        public override string getCurrentObjective()
        {
            if (getState().getCurrentIsland() != (-1))
            {
                return islands[getState().getCurrentIsland()].getCurrentObjective();
            }
            throw new System.ArgumentException("No current island");
        }
        // Java: 80
        public override void defeated(com.oddlabs.tt.viewer.WorldViewer viewer, string game_over_message)
        {
            if (getState().getCurrentIsland() == 4)
            {
                (((com.oddlabs.tt.player.campaign.NativeIsland4)islands[4])).removeCounter();
            }
            base.defeated(viewer, game_over_message);
        }
        // Java: 87
        public override void startIsland(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number)
        {
            getState().setCurrentIsland(number);
            com.oddlabs.tt.steam.SteamManager.setCampaignRichPresence("Natives");
            com.oddlabs.tt.steam.SteamManager.setInActiveWorld(true);
            islands[number].chosen(network, gui_root);
        }
        static NativeCampaign()
        {
            NativeCampaign.INITIAL_STATES = new int[] { com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN };
        }
        public NativeCampaign(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root) : this(network, gui_root, new com.oddlabs.tt.player.campaign.CampaignState(com.oddlabs.tt.player.campaign.NativeCampaign.INITIAL_STATES))
        {
        }
        public NativeCampaign(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.player.campaign.CampaignState campaign_state) : base(campaign_state)
        {
            islands = new com.oddlabs.tt.player.campaign.Island[com.oddlabs.tt.gui.NativeCampaignIcons.getIcons().getNumIslands()];
            islands[0] = new com.oddlabs.tt.player.campaign.NativeIsland0(this);
            islands[1] = new com.oddlabs.tt.player.campaign.NativeIsland1(this);
            islands[2] = new com.oddlabs.tt.player.campaign.NativeIsland2(this);
            islands[3] = new com.oddlabs.tt.player.campaign.NativeIsland3(this);
            islands[4] = new com.oddlabs.tt.player.campaign.NativeIsland4(this);
            islands[5] = new com.oddlabs.tt.player.campaign.NativeIsland5(this);
            islands[6] = new com.oddlabs.tt.player.campaign.NativeIsland6(this);
            islands[7] = new com.oddlabs.tt.player.campaign.NativeIsland7(this);
            if (getState().getCurrentIsland() == (-1))
            {
                startIsland(network, gui_root, 0);
            }
            getState().setHasMagic0(true);
            getState().setHasRubberWeapons(true);
        }
    }
}
