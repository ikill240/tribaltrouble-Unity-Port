// Translated from VikingIsland11.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 23
    public class VikingIsland11 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 26
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland11.bundle, key, args);
        }
        // Java: 34
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.75f, 1.0f, 0.85f, 83493473, 11, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            int ai_peons = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(10), 0 => (int)(25), 2 => (int)(40), _ => throw new System.ArgumentException() });
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, false, 0, false, ai_peons, 0, 2, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 68
        public override void start()
        {
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            java.lang.Runnable dialog8 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header8"), i18n("dialog8"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            java.lang.Runnable dialog7 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header7"), i18n("dialog7"), getCampaign().getIcons().getFaces()[3], com.oddlabs.tt.gui.Origin.AT_END, dialog8);
                addModalForm(dialog_2);
            });
            java.lang.Runnable dialog6 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header6"), i18n("dialog6"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog7);
                addModalForm(dialog_3);
            });
            java.lang.Runnable dialog5 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_4 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header5"), i18n("dialog5"), getCampaign().getIcons().getFaces()[6], com.oddlabs.tt.gui.Origin.AT_END, dialog6);
                addModalForm(dialog_4);
            });
            java.lang.Runnable dialog4 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_5 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header4"), i18n("dialog4"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog5);
                addModalForm(dialog_5);
            });
            java.lang.Runnable dialog3 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_6 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header3"), i18n("dialog3"), getCampaign().getIcons().getFaces()[6], com.oddlabs.tt.gui.Origin.AT_END, dialog4);
                addModalForm(dialog_6);
            });
            java.lang.Runnable dialog2 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_7 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog3);
                addModalForm(dialog_7);
            });
            java.lang.Runnable dialog1 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_8 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[6], com.oddlabs.tt.gui.Origin.AT_END, dialog2);
                addModalForm(dialog_8);
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_9 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog1);
                addModalForm(dialog_9);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(11, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(12, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 47, 22);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 54, 36);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 68, 36);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 80, 36);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 94, 30);
        }
        // Java: 165
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 170
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 175
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland11()
        {
            VikingIsland11.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland11).getName());
        }
        public VikingIsland11(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
