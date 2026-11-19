// Translated from VikingIsland6.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 24
    public class VikingIsland6 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 27
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland6.bundle, key, args);
        }
        // Java: 35
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.75f, 1.0f, 0.5f, 13462, 6, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, false, 0, 0, 0, 0));
            int ai_peons = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(1), 0 => (int)(3), 2 => (int)(15), _ => throw new System.ArgumentException() });
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, true, ai_peons, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 76
        public override void start()
        {
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player stranded = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            java.lang.Runnable answer = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[2], com.oddlabs.tt.gui.Origin.AT_END, answer);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            placePrisoners(stranded, local_player, 10, 0, 0, 0, false);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            });
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(runnable, stranded);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 39, 43);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 35, 53);
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(6, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(5, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(7, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setNumPeons(getCampaign().getState().getNumPeons() + stranded.getUnitCountContainer().getNumSupplies());
                getCampaign().victory(getViewer());
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                string new_units_message = i18n("new_units", stranded.getUnitCountContainer().getNumSupplies());
                string new_units_header = i18n("new_units_header");
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), new_units_header, new_units_message, getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, prize);
                addModalForm(dialog_3);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
        }
        // Java: 136
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 141
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 146
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland6()
        {
            VikingIsland6.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland6).getName());
        }
        public VikingIsland6(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
