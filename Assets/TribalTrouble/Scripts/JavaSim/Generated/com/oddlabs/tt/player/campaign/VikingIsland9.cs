// Translated from VikingIsland9.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 29
    public class VikingIsland9 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 32
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland9.bundle, key, args);
        }
        // Java: 40
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 1.0f, 0.85f, 0.85f, 777777777, 9, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(3, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(3, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 75
        public override void start()
        {
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player chief_tribe = getViewer().getWorld().getPlayers()[2];
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            chief_tribe.setActiveChieftain(new com.oddlabs.tt.model.Unit(chief_tribe, 56 * 2, 110 * 2, null, chief_tribe.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN)));
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            });
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(runnable, chief_tribe);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 50, 85);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 52, 81);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 54, 96);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 61, 104);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 57, 104);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 78, 90);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 72, 88);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 71, 83);
            int num_extra_units = 130;
            if (enemy.getArmory().getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() < num_extra_units)
            {
                enemy.getArmory().getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).increaseSupply(num_extra_units);
            }
            if (enemy.getArmory().getUnitContainer().getNumSupplies() < num_extra_units)
            {
                enemy.getArmory().getUnitContainer().increaseSupply(num_extra_units);
            }
            enemy.deployUnits(enemy.getArmory(), com.oddlabs.tt.model.DeployType.IRON_WARRIOR, 20);
            new com.oddlabs.tt.trigger.campaign.ReinforcementsTrigger(enemy, com.oddlabs.tt.model.DeployType.IRON_WARRIOR);
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(9, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(10, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().victory(getViewer());
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[7], com.oddlabs.tt.gui.Origin.AT_END, prize);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
        }
        // Java: 139
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 144
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 149
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland9()
        {
            VikingIsland9.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland9).getName());
        }
        public VikingIsland9(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
