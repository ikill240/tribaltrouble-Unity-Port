// Translated from NativeIsland5.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 21
    public class NativeIsland5 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 24
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland5.bundle, key, args);
        }
        // Java: 32
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING, 1.0f, 1.0f, 1.0f, 4, 5, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            int ai_peons = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(5), 0 => (int)(10), 2 => (int)(25), _ => throw new System.ArgumentException() });
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, ai_peons, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 67
        public override void start()
        {
            java.lang.Runnable runnable = default;
            java.lang.Runnable dialog0 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), dialog0);
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(5, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(3, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(6, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().victory(getViewer());
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[8], com.oddlabs.tt.gui.Origin.AT_END, prize);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
        }
        // Java: 100
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 105
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 110
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static NativeIsland5()
        {
            NativeIsland5.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland5).getName());
        }
        public NativeIsland5(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
