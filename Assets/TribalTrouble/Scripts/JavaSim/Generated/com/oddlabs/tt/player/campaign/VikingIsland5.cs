// Translated from VikingIsland5.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 23
    public class VikingIsland5 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 26
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland5.bundle, key, args);
        }
        // Java: 34
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.85f, 1.0f, 0.9f, 89864, 5, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 25, 5, 0, 0));
            int ai_peons = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(5), 0 => (int)(10), 2 => (int)(25), _ => throw new System.ArgumentException() });
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, false, 0, 1, false, ai_peons, 0, 0, 1));
            game_network.getClient().getServerInterface().setPlayerSlot(3, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(3, new com.oddlabs.tt.player.UnitInfo(true, false, 0, 1, false, ai_peons, 0, 0, 1));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 83
        public override void start()
        {
            getViewer().getLocalPlayer().enableMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, false);
            getViewer().getLocalPlayer().enableChiefsCourage(false);
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player enemy0 = getViewer().getWorld().getPlayers()[2];
            com.oddlabs.tt.player.Player enemy1 = getViewer().getWorld().getPlayers()[3];
            java.lang.Runnable answer = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[5], com.oddlabs.tt.gui.Origin.AT_END, answer);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(5, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(4, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(6, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setNumRockWarriors(getCampaign().getState().getNumRockWarriors() + 5);
                getCampaign().victory(getViewer());
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[5], com.oddlabs.tt.gui.Origin.AT_END, prize);
                addModalForm(dialog_3);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            enemy0.getAI().manTowers(1);
            enemy1.getAI().manTowers(1);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            });
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(runnable, getViewer().getWorld().getPlayers()[1]);
        }
        // Java: 141
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 146
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 151
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland5()
        {
            VikingIsland5.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland5).getName());
        }
        public VikingIsland5(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
