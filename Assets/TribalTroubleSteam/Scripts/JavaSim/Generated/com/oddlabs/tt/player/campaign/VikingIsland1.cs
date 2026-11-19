// Translated from VikingIsland1.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 27
    public class VikingIsland1 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 34
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland1.bundle, key, args);
        }
        // Java: 38
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.75f, 1.0f, 0.5f, 97455, 1, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, false, 0, 10, 5, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 72
        public override void start()
        {
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player captives = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            }));
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[3], com.oddlabs.tt.gui.Origin.AT_END, new java.lang.Runnable_L(() =>
                {
                    getCampaign().getState().setIslandState(1, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                    getCampaign().getState().setIslandState(2, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    getCampaign().getState().setNumPeons(getCampaign().getState().getNumPeons() + captives.getUnitCountContainer().getNumSupplies());
                    getCampaign().victory(getViewer());
                }));
                addModalForm(dialog_2);
            }));
            placePrisoners(captives, enemy, 10, 0, 0, 0, false);
            int attack1 = 5;
            int attack2 = 10;
            int defense = 10;
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
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(8.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(25.0f * 60.0f), attack2_runnable);
                    break;
                }
                case 0:
                {
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(5.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(8.5f * 60.0f), attack2_runnable);
                    break;
                }
                case 2:
                {
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(4.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(7.0f * 60.0f), attack2_runnable);
                    break;
                }
                default:
                {
                    throw new System.ArgumentException("unexpected difficulty");
                }
            }
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            }), captives);
        }
        // Java: 158
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 163
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 168
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland1()
        {
            VikingIsland1.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland1).getName());
        }
        public VikingIsland1(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
