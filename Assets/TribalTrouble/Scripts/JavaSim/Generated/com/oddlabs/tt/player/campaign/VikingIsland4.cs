// Translated from VikingIsland4.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 30
    public class VikingIsland4 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 33
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland4.bundle, key, args);
        }
        // Java: 41
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.65f, 1.0f, 0.5f, 786433, 4, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 2, true, 10, 10, 10, 10));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 75
        public override void start()
        {
            getViewer().getLocalPlayer().enableMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, false);
            getViewer().getLocalPlayer().enableChiefsCourage(false);
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player captive = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            int attack1 = default;
            int attack2 = default;
            int defense = default;
            switch ((getCampaign().getState().getDifficulty()))
            {
                case 1:
                {
                    attack1 = 4;
                    attack2 = 8;
                    defense = 10;
                    break;
                }
                case 0:
                {
                    attack1 = 7;
                    attack2 = 12;
                    defense = 12;
                    break;
                }
                case 2:
                {
                    attack1 = 11;
                    attack2 = 16;
                    defense = 20;
                    break;
                }
                default:
                {
                    throw new System.Exception();
                }
            }
            java.lang.Runnable attack1_runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain = local_player.getChieftain();
                if (armory != null && !armory.isDead())
                {
                    attack(enemy, armory, attack1);
                }
                else if (chieftain != null && !chieftain.isDead())
                {
                    attack(enemy, chieftain, attack1);
                }
                refillArmory(enemy);
                deploy(enemy, attack2);
            });
            java.lang.Runnable attack2_runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory_2 = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain_2 = local_player.getChieftain();
                if (armory_2 != null && !armory_2.isDead())
                {
                    attack(enemy, armory_2, attack2);
                }
                else if (chieftain_2 != null && !chieftain_2.isDead())
                {
                    attack(enemy, chieftain_2, attack1);
                }
                refillArmory(enemy);
                deploy(enemy, defense);
            });
            switch ((getCampaign().getState().getDifficulty()))
            {
                case 1:
                {
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(7.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(11.0f * 60.0f), attack2_runnable);
                    break;
                }
                case 0:
                {
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(4.5f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(8.0f * 60.0f), attack2_runnable);
                    break;
                }
                case 2:
                {
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(4.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(6.5f * 60.0f), attack2_runnable);
                    break;
                }
                default:
                {
                    throw new System.Exception();
                }
            }
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(4, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(5, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setHasMagic0(true);
                getCampaign().victory(getViewer());
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[5], com.oddlabs.tt.gui.Origin.AT_END, prize);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            placePrisoners(captive, enemy, 0, 0, 0, 0, true);
            enemy.getAI().manTowers(1);
            int num_reinforcements = 100;
            if (enemy.getArmory().getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() < num_reinforcements)
            {
                enemy.getArmory().getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).increaseSupply(num_reinforcements);
            }
            if (enemy.getArmory().getUnitContainer().getNumSupplies() < num_reinforcements)
            {
                enemy.getArmory().getUnitContainer().increaseSupply(num_reinforcements);
            }
            new com.oddlabs.tt.trigger.campaign.ReinforcementsTrigger(enemy, com.oddlabs.tt.model.DeployType.IRON_WARRIOR);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            });
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(runnable, captive);
        }
        // Java: 202
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 207
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 212
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland4()
        {
            VikingIsland4.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland4).getName());
        }
        public VikingIsland4(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
