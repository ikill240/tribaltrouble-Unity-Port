// Translated from NativeIsland6.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 23
    public class NativeIsland6 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 26
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland6.bundle, key, args);
        }
        // Java: 34
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 1024, com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING, 0.5f, 0.8f, 0.9f, 44, 6, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 2, false, 1, 0, 2, 0));
            int ai_peons = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(10), 0 => (int)(20), 2 => (int)(35), _ => throw new System.ArgumentException() });
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, false, 0, 1, false, ai_peons, 0, 0, 1));
            game_network.getClient().getServerInterface().setPlayerSlot(3, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(3, new com.oddlabs.tt.player.UnitInfo(true, false, 0, 1, false, ai_peons, 0, 0, 1));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 83
        public override void start()
        {
            java.lang.Runnable runnable = default;
            java.lang.Runnable dialog3 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header3"), i18n("dialog3"), getCampaign().getIcons().getFaces()[1], com.oddlabs.tt.gui.Origin.AT_END);
                addModalForm(dialog);
            });
            java.lang.Runnable dialog2 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog3);
                addModalForm(dialog_2);
            });
            java.lang.Runnable dialog1 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[1], com.oddlabs.tt.gui.Origin.AT_END, dialog2);
                addModalForm(dialog_3);
            });
            java.lang.Runnable dialog0 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_4 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog1);
                addModalForm(dialog_4);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), dialog0);
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(6, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(7, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().victory(getViewer());
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_5 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header4"), i18n("dialog4"), getCampaign().getIcons().getFaces()[4], com.oddlabs.tt.gui.Origin.AT_END, prize);
                addModalForm(dialog_5);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            com.oddlabs.tt.player.Player friend = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy0 = getViewer().getWorld().getPlayers()[2];
            com.oddlabs.tt.player.Player enemy1 = getViewer().getWorld().getPlayers()[3];
            friend.getAI().manTowers(2);
            enemy0.getAI().manTowers(1);
            enemy1.getAI().manTowers(1);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            });
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(runnable, friend);
        }
        // Java: 152
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 157
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 162
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static NativeIsland6()
        {
            NativeIsland6.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland6).getName());
        }
        public NativeIsland6(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
