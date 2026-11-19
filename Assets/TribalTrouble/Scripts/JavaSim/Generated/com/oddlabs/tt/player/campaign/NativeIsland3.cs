// Translated from NativeIsland3.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 30
    public class NativeIsland3 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 33
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland3.bundle, key, args);
        }
        public int objective;
        // Java: 43
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING, 1.0f, 1.0f, 0.0f, 808208041, 3, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(4, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(4, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 73
        public override void start()
        {
            getViewer().getLocalPlayer().enableChiefsCourage(false);
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player reinforcements = getViewer().getWorld().getPlayers()[2];
            int start_x = 125 * 2;
            int start_y = 222 * 2;
            int thor_x = 40 * 2;
            int thor_y = 40 * 2;
            getViewer().getCamera().reset(start_x, start_y);
            getViewer().getCamera().setPos(thor_x, thor_y + 9);
            java.lang.Runnable camera_jump = new java.lang.Runnable_L(() =>
            {
                getViewer().getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), getViewer().getCamera(), start_x, start_y, 200.0f, 3.0f));
            });
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, camera_jump);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            getViewer().getLocalPlayer().enableBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, false);
            getViewer().getLocalPlayer().enableBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false);
            getViewer().getLocalPlayer().enableBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, false);
            java.util.ResourceBundle player_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            local_player.setActiveChieftain(new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "native_chieftain_name"), false));
            local_player.getChieftain().increaseMagicEnergy(0, 1000);
            local_player.getChieftain().increaseMagicEnergy(1, 1000);
            for (int i = 0; i < 5; i++)
            {
                new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            int unit_count = getCampaign().getState().getNumPeons() + getCampaign().getState().getNumRockWarriors() + getCampaign().getState().getNumIronWarriors() + getCampaign().getState().getNumRubberWarriors() - 5;
            for (int i_2 = 0; i_2 < unit_count; i_2++)
            {
                if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY)
                {
                    new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                }
                else
                {
                    new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                }
            }
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(3, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(4, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setHasMagic1(true);
                getCampaign().victory(getViewer());
            });
            java.lang.Runnable dialog8 = new java.lang.Runnable_L(() =>
            {
                new com.oddlabs.tt.trigger.campaign.MagicUsedTrigger(local_player.getChieftain(), thor_x, thor_y, 20, 0, prize);
                changeObjective(1);
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header8"), i18n("dialog8"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog_2);
            });
            java.lang.Runnable dialog7 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header7"), i18n("dialog7"), getCampaign().getIcons().getFaces()[7], com.oddlabs.tt.gui.Origin.AT_END, dialog8);
                addModalForm(dialog_3);
            });
            java.lang.Runnable dialog6 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_4 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header6"), i18n("dialog6"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog7);
                addModalForm(dialog_4);
            });
            java.lang.Runnable dialog5 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_5 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header5"), i18n("dialog5"), getCampaign().getIcons().getFaces()[7], com.oddlabs.tt.gui.Origin.AT_END, dialog6);
                addModalForm(dialog_5);
            });
            java.lang.Runnable dialog4 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_6 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header4"), i18n("dialog4"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog5);
                addModalForm(dialog_6);
            });
            java.lang.Runnable dialog3 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_7 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header3"), i18n("dialog3"), getCampaign().getIcons().getFaces()[7], com.oddlabs.tt.gui.Origin.AT_END, dialog4);
                addModalForm(dialog_7);
            });
            java.lang.Runnable dialog2 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_8 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog3);
                addModalForm(dialog_8);
            });
            java.lang.Runnable dialog1 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_9 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[7], com.oddlabs.tt.gui.Origin.AT_END, dialog2);
                addModalForm(dialog_9);
                local_player.getChieftain().increaseMagicEnergy(0, 1000);
                local_player.getChieftain().increaseMagicEnergy(1, 1000);
            });
            new com.oddlabs.tt.trigger.campaign.NearPointTrigger(thor_x / 2, thor_x / 2, 8, local_player.getChieftain(), dialog1);
            float shadow_diameter = 4.5f;
            float dir = ((float)(java.lang.StrictMath.sin(java.lang.Math.PI / 4)));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), thor_x, thor_y, dir, dir, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN).getSpriteRenderer(), shadow_diameter, true, i18n("god"), com.oddlabs.tt.model.Unit.Animation.THOR, (float)(-1.0f), 0.0f);
            reinforcements.setStartX(126 * 2);
            reinforcements.setStartY(135 * 2);
            reinforcements.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, 96, 145);
            reinforcements.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, 126, 135);
            for (int i_3 = 0; i_3 < 30; i_3++)
            {
                new com.oddlabs.tt.model.Unit(reinforcements, 126 * 2, 135 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            }
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 60, 60);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 110, 167);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 102, 127);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 67, 163);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 71, 189);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 92, 205);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 128, 186);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 191, 185);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 145, 128);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 173, 90);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 161, 82);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 120, 140);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 124, 127);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 135, 140);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 100, 139);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 80, 97);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 112, 76);
            new com.oddlabs.tt.model.Unit(enemy, 180 * 2, 155 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 178 * 2, 145 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 178 * 2, 151 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 180 * 2, 153 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 103 * 2, 93 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 99 * 2, 99 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 113 * 2, 98 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 116 * 2, 98 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 83 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 110 * 2, 199 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 186 * 2, 215 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 186 * 2, 211 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 53 * 2, 105 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 51 * 2, 107 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 53 * 2, 110 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 53 * 2, 101 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            java.lang.Runnable reinforce = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Unit unit = getWarrior(reinforcements);
                if (unit != null && !unit.isDead())
                {
                    com.oddlabs.tt.model.Unit new_unit = changeOwner(unit, enemy);
                    if (new_unit != null && !new_unit.isDead())
                    {
                        new_unit.setTarget(new com.oddlabs.tt.landscape.LandscapeTarget(62, 62), com.oddlabs.tt.model.Action.DEFAULT, true);
                    }
                }
            });
            float interval = ((getCampaign().getState().getDifficulty()) switch { 1 => (float)(120.0f), 0 => (float)(60.0f), 2 => (float)(30.0f), _ => throw new System.Exception("unrecognized difficulty") });
            float time = interval;
            for (int i_4 = 0; i_4 < 10; i_4++)
            {
                new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), time, reinforce);
                time = (float)(time + interval);
            }
        }
        // Java: 290
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 295
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 300
        public override string getCurrentObjective()
        {
            return i18n("objective" + (objective));
        }
        // Java: 305
        public void changeObjective(int objective)
        {
            this.objective = objective;
        }
        static NativeIsland3()
        {
            NativeIsland3.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland3).getName());
        }
        private void init_NativeIsland3()
        {
            this.objective = 0;
        }
        public NativeIsland3(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
            init_NativeIsland3();
        }
    }
}
