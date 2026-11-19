// Translated from VikingCampaign.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 15
    public class VikingCampaign : com.oddlabs.tt.player.campaign.Campaign
    {
        public const int MAX_UNITS = 46;
        public static int[] INITIAL_STATES;
        public com.oddlabs.tt.player.campaign.Island[] islands;
        // Java: 66
        public override com.oddlabs.tt.gui.CampaignIcons getIcons()
        {
            return com.oddlabs.tt.gui.VikingCampaignIcons.getIcons();
        }
        // Java: 71
        public override void islandChosen(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number)
        {
            com.oddlabs.tt.gui.Form dialog = new com.oddlabs.tt.form.CampaignDialogForm(islands[number].getHeader(), islands[number].getDescription(), null, com.oddlabs.tt.gui.Origin.AT_START, new java.lang.Runnable_L(() =>
            {
                startIsland(network, gui_root, number);
            }), true);
            gui_root.addModalForm(dialog);
        }
        // Java: 81
        public override string getCurrentObjective()
        {
            if (getState().getCurrentIsland() != (-1))
            {
                return islands[getState().getCurrentIsland()].getCurrentObjective();
            }
            throw new System.InvalidOperationException("No current island");
        }
        // Java: 89
        public override void defeated(com.oddlabs.tt.viewer.WorldViewer viewer, string game_over_message)
        {
            if (getState().getCurrentIsland() == 13)
            {
                (((com.oddlabs.tt.player.campaign.VikingIsland13)islands[13])).removeCounter();
            }
            base.defeated(viewer, game_over_message);
        }
        // Java: 96
        public override void startIsland(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number)
        {
            getState().setCurrentIsland(number);
            com.oddlabs.tt.steam.SteamManager.setCampaignRichPresence("Vikings");
            com.oddlabs.tt.steam.SteamManager.setInActiveWorld(true);
            islands[number].chosen(network, gui_root);
        }
        static VikingCampaign()
        {
            VikingCampaign.INITIAL_STATES = new int[] { com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE };
        }
        public VikingCampaign(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root) : this(network, gui_root, new com.oddlabs.tt.player.campaign.CampaignState(com.oddlabs.tt.player.campaign.VikingCampaign.INITIAL_STATES))
        {
        }
        public VikingCampaign(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.player.campaign.CampaignState campaign_state) : base(campaign_state)
        {
            islands = new com.oddlabs.tt.player.campaign.Island[com.oddlabs.tt.gui.VikingCampaignIcons.getIcons().getNumIslands()];
            islands[0] = new com.oddlabs.tt.player.campaign.VikingIsland0(this);
            islands[1] = new com.oddlabs.tt.player.campaign.VikingIsland1(this);
            islands[2] = new com.oddlabs.tt.player.campaign.VikingIsland2(this);
            islands[3] = new com.oddlabs.tt.player.campaign.VikingIsland3(this);
            islands[4] = new com.oddlabs.tt.player.campaign.VikingIsland4(this);
            islands[5] = new com.oddlabs.tt.player.campaign.VikingIsland5(this);
            islands[6] = new com.oddlabs.tt.player.campaign.VikingIsland6(this);
            islands[7] = new com.oddlabs.tt.player.campaign.VikingIsland7(this);
            islands[8] = new com.oddlabs.tt.player.campaign.VikingIsland8(this);
            islands[9] = new com.oddlabs.tt.player.campaign.VikingIsland9(this);
            islands[10] = new com.oddlabs.tt.player.campaign.VikingIsland10(this);
            islands[11] = new com.oddlabs.tt.player.campaign.VikingIsland11(this);
            islands[12] = new com.oddlabs.tt.player.campaign.VikingIsland12(this);
            islands[13] = new com.oddlabs.tt.player.campaign.VikingIsland13(this);
            islands[14] = new com.oddlabs.tt.player.campaign.VikingIsland14(this);
            if (getState().getCurrentIsland() == (-1))
            {
                startIsland(network, gui_root, 0);
            }
        }
    }
}
