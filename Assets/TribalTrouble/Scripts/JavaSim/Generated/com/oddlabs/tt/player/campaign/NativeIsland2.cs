// Translated from NativeIsland2.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 28
    public class NativeIsland2 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 31
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland2.bundle, key, args);
        }
        // Java: 39
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING, 0.75f, 1.0f, 1.0f, 10, 2, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, 0, 10, 5, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 73
        public override void start()
        {
            getViewer().getLocalPlayer().enableChiefsCourage(false);
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player captives = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(2, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(3, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setNumPeons(getCampaign().getState().getNumPeons() + captives.getUnitCountContainer().getNumSupplies());
                getCampaign().victory(getViewer());
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                string message = i18n("dialog1", captives.getUnitCountContainer().getNumSupplies());
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), message, getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, prize);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            int start_x = 100 * 2;
            int start_y = 73 * 2;
            java.util.ResourceBundle player_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            local_player.setActiveChieftain(new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "native_chieftain_name"), false));
            local_player.getChieftain().increaseMagicEnergy(0, 1000);
            local_player.getChieftain().increaseMagicEnergy(1, 1000);
            for (int i = 0; i < getCampaign().getState().getNumPeons(); i++)
            {
                new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            for (int i_2 = 0; i_2 < getCampaign().getState().getNumRockWarriors(); i_2++)
            {
                new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            }
            for (int i_3 = 0; i_3 < getCampaign().getState().getNumIronWarriors(); i_3++)
            {
                new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            }
            for (int i_4 = 0; i_4 < getCampaign().getState().getNumRubberWarriors(); i_4++)
            {
                new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            }
            getViewer().getCamera().reset(start_x, start_y);
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
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(7.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(11.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(16.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(21.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(26.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(31.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(36.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(41.0f * 60.0f), attack2_runnable);
                    break;
                }
                case 0:
                {
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(5.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(8.5f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(13.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(17.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(21.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(25.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(29.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(33.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(37.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(41.0f * 60.0f), attack2_runnable);
                    break;
                }
                case 2:
                {
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(4.0f * 60.0f), attack1_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(7.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(11.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(15.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(19.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(23.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(27.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(31.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(35.0f * 60.0f), attack2_runnable);
                    new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(39.0f * 60.0f), attack2_runnable);
                    break;
                }
                default:
                {
                    throw new System.Exception();
                }
            }
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            });
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(runnable, captives);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 42, 83);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 63, 89);
        }
        // Java: 222
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 227
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 232
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static NativeIsland2()
        {
            NativeIsland2.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland2).getName());
        }
        public NativeIsland2(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
