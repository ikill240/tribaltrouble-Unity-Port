// Translated from NativeIsland0.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 36
    public class NativeIsland0 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 39
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland0.bundle, key, args);
        }
        public int objective;
        // Java: 49
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 1024, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.75f, 0.65f, 0.85f, 25, 0, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            switch ((getCampaign().getState().getDifficulty()))
            {
                case 1:
                case 0:
                case 2:
                {
                    break;
                }
                default:
                {
                    throw new System.ArgumentException("unexpected difficulty: " + (getCampaign().getState().getDifficulty()));
                }
            }
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(3, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(3, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 92
        public override void start()
        {
            getViewer().getLocalPlayer().enableMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, false);
            getViewer().getLocalPlayer().enableChiefsCourage(false);
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player reinforcements = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            com.oddlabs.tt.player.Player natives = getViewer().getWorld().getPlayers()[3];
            int chief_start_x = 140 * 2;
            int chief_start_y = 117 * 2;
            int viking_start_x = 179 * 2;
            int viking_start_y = 195 * 2;
            getViewer().getCamera().reset(viking_start_x, viking_start_y);
            java.lang.Runnable dialog1 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            java.lang.Runnable camera_jump0 = new java.lang.Runnable_L(() =>
            {
                getViewer().getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), getViewer().getCamera(), chief_start_x + 7, chief_start_y + 7, 200.0f, 3.0f, dialog1));
            });
            java.lang.Runnable dialog0 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, camera_jump0);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), dialog0);
            java.util.ResourceBundle player_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            local_player.setActiveChieftain(new com.oddlabs.tt.model.Unit(local_player, chief_start_x, chief_start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "native_chieftain_name"), false));
            natives.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, 135, 128);
            natives.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, 143, 124);
            new com.oddlabs.tt.model.Unit(natives, 145 * 2, 127 * 2, null, natives.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(natives, 149 * 2, 122 * 2, null, natives.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(natives, 145 * 2, 119 * 2, null, natives.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(natives, 150 * 2, 125 * 2, null, natives.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            int num_reinforcements = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(15), 0 => (int)(10), 2 => (int)(6), _ => throw new System.ArgumentException() });
            com.oddlabs.tt.model.Unit[] reinforcement_peons = new com.oddlabs.tt.model.Unit[num_reinforcements];
            reinforcement_peons[0] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            reinforcement_peons[1] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            reinforcement_peons[2] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            reinforcement_peons[3] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            reinforcement_peons[4] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            reinforcement_peons[5] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY || getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_NORMAL)
            {
                reinforcement_peons[6] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                reinforcement_peons[7] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                reinforcement_peons[8] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                reinforcement_peons[9] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY)
            {
                reinforcement_peons[10] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                reinforcement_peons[11] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                reinforcement_peons[12] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                reinforcement_peons[13] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                reinforcement_peons[14] = new com.oddlabs.tt.model.Unit(reinforcements, 230 * 2, 108 * 2, null, reinforcements.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            enemy.setActiveChieftain(new com.oddlabs.tt.model.Unit(enemy, viking_start_x, viking_start_y, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "chieftain_name"), false));
            enemy.getChieftain().increaseMagicEnergy(0, 1000);
            enemy.getChieftain().increaseMagicEnergy(1, 1000);
            int num_iron = 45;
            for (int i = 0; i < num_iron; i++)
            {
                new com.oddlabs.tt.model.Unit(enemy, viking_start_x, viking_start_y, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            }
            int num_rubber = 15;
            for (int i_2 = 0; i_2 < num_rubber; i_2++)
            {
                new com.oddlabs.tt.model.Unit(enemy, viking_start_x, viking_start_y, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            }
            com.oddlabs.tt.model.Building armory = natives.getArmory();
            attack(enemy, armory, num_iron + num_rubber + 1);
            java.lang.Runnable dialog7 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header7"), i18n("dialog7"), getCampaign().getIcons().getFaces()[6], com.oddlabs.tt.gui.Origin.AT_END, new java.lang.Runnable_L(() =>
                {
                    getCampaign().getState().setIslandState(0, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                    getCampaign().getState().setIslandState(1, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    getCampaign().victory(getViewer());
                }));
                addModalForm(dialog_3);
            });
            java.lang.Runnable dialog6 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_4 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header6"), i18n("dialog6"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog7);
                addModalForm(dialog_4);
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_5 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header5"), i18n("dialog5"), getCampaign().getIcons().getFaces()[6], com.oddlabs.tt.gui.Origin.AT_END, dialog6);
                addModalForm(dialog_5);
            }));
            com.oddlabs.tt.model.SceneryModel[] scenery_models = new com.oddlabs.tt.model.SceneryModel[14];
            float dir = ((float)(java.lang.StrictMath.sin(java.lang.Math.PI / 4)));
            float offset = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID / 2.0f);
            float shadow_diameter = 4.5f;
            com.oddlabs.tt.render.SpriteKey[] treasures = getViewer().getWorld().getRacesResources().getTreasures();
            scenery_models[0] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(163 * 2 + offset), (float)(126 * 2 + offset), 0, 1, treasures[0], shadow_diameter, true, i18n("statue"));
            shadow_diameter = 2.6f;
            scenery_models[1] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(130 * 2 + offset), (float)(124 * 2 + offset), (float)(-dir), (float)(-dir), treasures[3], shadow_diameter, true, i18n("statue"));
            scenery_models[2] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(152 * 2 + offset), (float)(138 * 2 + offset), dir, dir, treasures[1], shadow_diameter, true, i18n("statue"));
            scenery_models[3] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(152 * 2 + offset), (float)(144 * 2 + offset), 0, 1, treasures[3], shadow_diameter, true, i18n("statue"));
            scenery_models[4] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(140 * 2 + offset), (float)(140 * 2 + offset), 0, 1, treasures[4], shadow_diameter, true, i18n("statue"));
            scenery_models[5] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(143 * 2 + offset), (float)(116 * 2 + offset), 0, (-1), treasures[1], shadow_diameter, true, i18n("statue"));
            scenery_models[6] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(142 * 2 + offset), (float)(131 * 2 + offset), dir, (float)(-dir), treasures[5], shadow_diameter, true, i18n("statue"));
            scenery_models[7] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(423 * 2 + offset), (float)(174 * 2 + offset), 0, 1, treasures[1], shadow_diameter, true, i18n("statue"));
            scenery_models[8] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(408 * 2 + offset), (float)(161 * 2 + offset), (-1), 0, treasures[3], shadow_diameter, true, i18n("statue"));
            scenery_models[9] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(426 * 2 + offset), (float)(156 * 2 + offset), dir, (float)(-dir), treasures[5], shadow_diameter, true, i18n("statue"));
            scenery_models[10] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(418 * 2 + offset), (float)(165 * 2 + offset), 0, 1, treasures[1], shadow_diameter, true, i18n("statue"));
            scenery_models[11] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(430 * 2 + offset), (float)(165 * 2 + offset), 1, 0, treasures[3], shadow_diameter, true, i18n("statue"));
            scenery_models[12] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(419 * 2 + offset), (float)(170 * 2 + offset), (float)(-dir), dir, treasures[4], shadow_diameter, true, i18n("statue"));
            scenery_models[13] = new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(416 * 2 + offset), (float)(156 * 2 + offset), 0, (-1), treasures[5], shadow_diameter, true, i18n("statue"));
            java.lang.Runnable dialog4 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_6 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header4"), i18n("dialog4"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog_6);
                foreach (com.oddlabs.tt.model.Unit reinforcement_peon in reinforcement_peons)
                {
                    if (!reinforcement_peon.isDead())
                    {
                        changeOwner(reinforcement_peon, local_player);
                    }
                }
            });
            java.lang.Runnable dialog3 = new java.lang.Runnable_L(() =>
            {
                changeObjective(1);
                foreach (com.oddlabs.tt.model.SceneryModel scenery_model in scenery_models)
                {
                    scenery_model.remove();
                }
                com.oddlabs.tt.model.Selectable[] viking_units = ((com.oddlabs.tt.model.Selectable[])enemy.getUnits().getSet().toArray(com.oddlabs.tt.model.Selectable.newArray(0)));
                foreach (com.oddlabs.tt.model.Selectable viking_unit in viking_units)
                {
                    if ((viking_unit is com.oddlabs.tt.model.Unit unit) && !unit.isDead())
                    {
                        unit.removeNow();
                    }
                }
                int new_viking_start_x = 437 * 2;
                int new_viking_start_y = 140 * 2;
                int num_peons = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(5), 0 => (int)(10), 2 => (int)(15), _ => throw new System.ArgumentException() });
                for (int i_3 = 0; i_3 < num_peons; i_3++)
                {
                    new com.oddlabs.tt.model.Unit(enemy, new_viking_start_x, new_viking_start_y, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                com.oddlabs.tt.model.Selectable[] native_selectables = com.oddlabs.tt.model.Selectable.newArray(natives.getUnits().getSet().size());
                natives.getUnits().getSet().toArray(native_selectables);
                foreach (com.oddlabs.tt.model.Selectable native_selectable in native_selectables)
                {
                    if (!native_selectable.isDead())
                    {
                        native_selectable.hit(10000, 0, 1, enemy);
                    }
                }
                com.oddlabs.tt.form.CampaignDialogForm dialog_7 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header3"), i18n("dialog3"), getCampaign().getIcons().getFaces()[2], com.oddlabs.tt.gui.Origin.AT_END, dialog4);
                addModalForm(dialog_7);
            });
            java.lang.Runnable dialog2 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_8 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog3);
                addModalForm(dialog_8);
            });
            new com.oddlabs.tt.trigger.campaign.NearArmyTrigger(reinforcement_peons, 10.0f, local_player, new java.lang.Runnable_L(() =>
            {
                int x = 230 * 2;
                int y = 108 * 2;
                com.oddlabs.tt.camera.Camera camera = getViewer().getGUIRoot().getDelegate().getCamera();
                if ((camera is com.oddlabs.tt.camera.GameCamera gameCamera))
                {
                    getViewer().getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), gameCamera, x, y, 200.0f, 3.0f, dialog2));
                }
                else if ((camera is com.oddlabs.tt.camera.MapCamera mapCamera))
                {
                    mapCamera.mapGoto(x, y, true);
                    dialog2.run();
                }
                else if ((camera is com.oddlabs.tt.camera.JumpCamera) || (camera is com.oddlabs.tt.camera.FirstPersonCamera))
                {
                    getViewer().getGUIRoot().getDelegate().pop();
                    getViewer().getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), getViewer().getCamera(), x, y, 200.0f, 3.0f, dialog2));
                }
                else
                {
                    throw new System.Exception("Camera = " + java.lang.JStr.valueOf(camera));
                }
            }));
        }
        // Java: 362
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 367
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 372
        public override string getCurrentObjective()
        {
            return i18n("objective" + (objective));
        }
        // Java: 377
        public void changeObjective(int objective)
        {
            this.objective = objective;
        }
        static NativeIsland0()
        {
            NativeIsland0.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland0).getName());
        }
        private void init_NativeIsland0()
        {
            this.objective = 0;
        }
        public NativeIsland0(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
            init_NativeIsland0();
        }
    }
}
