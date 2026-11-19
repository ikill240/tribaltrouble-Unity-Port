// Translated from VikingIsland0.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 32
    public class VikingIsland0 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 35
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland0.bundle, key, args);
        }
        // Java: 43
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.5f, 1.0f, 0.1f, 45363, 0, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, false, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, false, 0, 10, 5, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 80
        public override void start()
        {
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player chieftain = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[1], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            }));
            getViewer().getLocalPlayer().enableChieftains(false);
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, new java.lang.Runnable_L(() =>
                {
                    getCampaign().getState().setIslandState(0, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                    getCampaign().getState().setIslandState(1, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    getCampaign().getState().setIslandState(3, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    getCampaign().victory(getViewer());
                }));
                addModalForm(dialog_2);
            }));
            placePrisoners(chieftain, enemy, 0, 0, 0, 0, true);
            int num_units = 10;
            if (enemy.getArmory().getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() < num_units * 3)
            {
                enemy.getArmory().getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).increaseSupply(num_units * 3);
            }
            if (enemy.getArmory().getUnitContainer().getNumSupplies() < num_units * 3)
            {
                enemy.getArmory().getUnitContainer().increaseSupply(num_units * 3);
            }
            java.lang.Runnable runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory = local_player.getArmory();
                if (armory != null && !armory.isDead())
                {
                    if (enemy.getArmory() != null && !enemy.getArmory().isDead())
                    {
                        enemy.deployUnits(enemy.getArmory(), com.oddlabs.tt.model.DeployType.IRON_WARRIOR, num_units);
                        com.oddlabs.tt.player.AI.attackLandscape(enemy, armory, num_units);
                    }
                }
            });
            if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_NORMAL)
            {
                new com.oddlabs.tt.trigger.campaign.SupplyGatheredTrigger(getViewer().getLocalPlayer(), runnable, typeof(com.oddlabs.tt.landscape.TreeSupply), 30);
                new com.oddlabs.tt.trigger.campaign.SupplyGatheredTrigger(getViewer().getLocalPlayer(), runnable, typeof(com.oddlabs.tt.model.RockSupply), 30);
                new com.oddlabs.tt.trigger.campaign.SupplyGatheredTrigger(getViewer().getLocalPlayer(), runnable, typeof(com.oddlabs.tt.model.IronSupply), 30);
            }
            else if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_HARD)
            {
                new com.oddlabs.tt.trigger.campaign.SupplyGatheredTrigger(getViewer().getLocalPlayer(), runnable, typeof(com.oddlabs.tt.landscape.TreeSupply), 20);
                new com.oddlabs.tt.trigger.campaign.SupplyGatheredTrigger(getViewer().getLocalPlayer(), runnable, typeof(com.oddlabs.tt.model.RockSupply), 15);
                new com.oddlabs.tt.trigger.campaign.SupplyGatheredTrigger(getViewer().getLocalPlayer(), runnable, typeof(com.oddlabs.tt.model.IronSupply), 15);
            }
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            }), chieftain);
        }
        // Java: 150
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 155
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 160
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland0()
        {
            VikingIsland0.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland0).getName());
        }
        public VikingIsland0(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
