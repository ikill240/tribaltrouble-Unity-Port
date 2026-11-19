// Translated from NativeIsland1.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 31
    public class NativeIsland1 : com.oddlabs.tt.player.campaign.Island
    {
        public const int NUM_CAPTIVES = 10;
        public static java.util.ResourceBundle bundle;
        // Java: 35
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland1.bundle, key, args);
        }
        public int objective;
        // Java: 45
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 256, com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING, 0.75f, 1.0f, 0.5f, 1, 1, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(4, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(4, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 79
        public override void start()
        {
            getViewer().getLocalPlayer().enableMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, false);
            getViewer().getLocalPlayer().enableChiefsCourage(false);
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player guards = getViewer().getWorld().getPlayers()[2];
            int start_x = 24 * 2;
            int start_y = 86 * 2;
            java.lang.Runnable camera_jump0 = new java.lang.Runnable_L(() =>
            {
                getViewer().getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), getViewer().getCamera(), start_x, start_y, 200.0f, 3.0f));
            });
            java.lang.Runnable dialog1 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, camera_jump0);
                addModalForm(dialog);
            });
            java.lang.Runnable dialog0 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[4], com.oddlabs.tt.gui.Origin.AT_END, dialog1);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), dialog0);
            java.util.ResourceBundle player_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            local_player.setActiveChieftain(new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "native_chieftain_name"), false));
            local_player.getChieftain().increaseMagicEnergy(0, 1000);
            local_player.getChieftain().increaseMagicEnergy(1, 1000);
            for (int i = 0; i < getCampaign().getState().getNumPeons(); i++)
            {
                new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            }
            int captive_start_x = 48 * 2;
            int captive_start_y = 96 * 2;
            float shadow_diameter = local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON).getShadowDiameter();
            com.oddlabs.tt.render.SpriteKey sprite_renderer = local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON).getSpriteRenderer();
            float offset = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID / 2.0f);
            float dir = ((float)(java.lang.StrictMath.sin(java.lang.Math.PI / 4)));
            com.oddlabs.tt.model.SceneryModel[] scenery_models = new com.oddlabs.tt.model.SceneryModel[10];
            scenery_models[0] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(48 * 2 + offset), (float)(96 * 2 + offset), 1, 0, sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.1f);
            scenery_models[1] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(48 * 2 + offset), (float)(95 * 2 + offset), dir, dir, sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.15f);
            scenery_models[2] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(48 * 2 + offset), (float)(98 * 2 + offset), dir, (float)(-dir), sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.75f);
            scenery_models[3] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(49 * 2 + offset), (float)(98 * 2 + offset), 0, (-1), sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.54f);
            scenery_models[4] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(50 * 2 + offset), (float)(97 * 2 + offset), (-1), 0, sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.47f);
            scenery_models[5] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(51 * 2 + offset), (float)(96 * 2 + offset), 0, (-1), sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.9f);
            scenery_models[6] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(51 * 2 + offset), (float)(94 * 2 + offset), 0, 1, sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.23f);
            scenery_models[7] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(52 * 2 + offset), (float)(96 * 2 + offset), (float)(-dir), (float)(-dir), sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.7f);
            scenery_models[8] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(52 * 2 + offset), (float)(94 * 2 + offset), (float)(-dir), dir, sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.0f);
            scenery_models[9] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(50 * 2 + offset), (float)(95 * 2 + offset), 1, 0, sprite_renderer, shadow_diameter, true, i18n("captive"), com.oddlabs.tt.model.Unit.Animation.THROWING, 1, 0.34f);
            new com.oddlabs.tt.model.Unit(guards, 45 * 2, 98 * 2, null, guards.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(guards, 47 * 2, 92 * 2, null, guards.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            com.oddlabs.tt.model.Unit trigger = new com.oddlabs.tt.model.Unit(guards, 54 * 2, 97 * 2, null, guards.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            getViewer().getCamera().reset(start_x, start_y);
            getViewer().getCamera().setPos(captive_start_x - 6, captive_start_y);
            java.lang.Runnable free_captives = new java.lang.Runnable_L(() =>
            {
                changeObjective(1);
                foreach (com.oddlabs.tt.model.SceneryModel scenery_model in scenery_models)
                {
                    scenery_model.remove();
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(48 * 2 + offset), (float)(96 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(48 * 2 + offset), (float)(95 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(48 * 2 + offset), (float)(98 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(49 * 2 + offset), (float)(98 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(50 * 2 + offset), (float)(97 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(51 * 2 + offset), (float)(96 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(51 * 2 + offset), (float)(94 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(52 * 2 + offset), (float)(96 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(52 * 2 + offset), (float)(94 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                if (!local_player.getUnitCountContainer().isSupplyFull())
                {
                    new com.oddlabs.tt.model.Unit(local_player, (float)(50 * 2 + offset), (float)(95 * 2 + offset), null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
            });
            java.lang.Runnable dialog5 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header5"), i18n("dialog5"), getCampaign().getIcons().getFaces()[3], com.oddlabs.tt.gui.Origin.AT_END, free_captives);
                addModalForm(dialog_3);
            });
            java.lang.Runnable dialog4 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_4 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header4"), i18n("dialog4"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog5);
                addModalForm(dialog_4);
            });
            java.lang.Runnable dialog3 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_5 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header3"), i18n("dialog3"), getCampaign().getIcons().getFaces()[3], com.oddlabs.tt.gui.Origin.AT_END, dialog4);
                addModalForm(dialog_5);
            });
            new com.oddlabs.tt.trigger.campaign.DeathTrigger(trigger, new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_6 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog3);
                addModalForm(dialog_6);
            }));
            java.lang.Runnable prize = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(1, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(2, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(5, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setNumPeons(getCampaign().getState().getNumPeons() + com.oddlabs.tt.player.campaign.NativeIsland1.NUM_CAPTIVES);
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), prize);
            int attack1 = 5;
            int attack2 = 10;
            int defense = 20;
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
            enemy.setStartX(106 * 2);
            enemy.setStartY(54 * 2);
            enemy.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, 106, 54);
            enemy.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, 97, 50);
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, enemy.getStartX(), enemy.getStartY(), null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 97, 39);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 90, 52);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 96, 63);
        }
        // Java: 346
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 351
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 356
        public override string getCurrentObjective()
        {
            return i18n("objective" + (objective));
        }
        // Java: 361
        public void changeObjective(int objective)
        {
            this.objective = objective;
        }
        static NativeIsland1()
        {
            NativeIsland1.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland1).getName());
        }
        private void init_NativeIsland1()
        {
            this.objective = 0;
        }
        public NativeIsland1(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
            init_NativeIsland1();
        }
    }
}
