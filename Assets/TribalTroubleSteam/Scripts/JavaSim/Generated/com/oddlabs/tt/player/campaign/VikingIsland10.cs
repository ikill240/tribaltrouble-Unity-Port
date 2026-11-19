// Translated from VikingIsland10.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 28
    public class VikingIsland10 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 31
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland10.bundle, key, args);
        }
        public int objective;
        // Java: 41
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 1.0f, 1.0f, 0.0f, (-1442873271), 10, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(false, false, 0, false, 0, 0, 0, 1));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 64
        public override void start()
        {
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            getViewer().getCamera().reset(142 * 2, 182 * 2);
            getViewer().getCamera().setPos(177 * 2, 156 * 2);
            java.lang.Runnable camera_jump = new java.lang.Runnable_L(() =>
            {
                getViewer().getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), getViewer().getCamera(), 142 * 2, 182 * 2, 200.0f, 3.0f));
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
            local_player.setActiveChieftain(new com.oddlabs.tt.model.Unit(local_player, 142 * 2, 182 * 2, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "chieftain_name"), false));
            local_player.getChieftain().increaseMagicEnergy(0, 1000);
            local_player.getChieftain().increaseMagicEnergy(1, 1000);
            for (int i = 0; i < 5; i++)
            {
                new com.oddlabs.tt.model.Unit(local_player, 142 * 2, 182 * 2, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            int unit_count = getCampaign().getState().getNumPeons() + getCampaign().getState().getNumRockWarriors() + getCampaign().getState().getNumIronWarriors() + getCampaign().getState().getNumRubberWarriors() - 5;
            for (int i_2 = 0; i_2 < unit_count; i_2++)
            {
                if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY)
                {
                    new com.oddlabs.tt.model.Unit(local_player, 142 * 2, 182 * 2, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                }
                else
                {
                    new com.oddlabs.tt.model.Unit(local_player, 142 * 2, 182 * 2, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                }
            }
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(10, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(13, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setHasMagic1(true);
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.MagicUsedTrigger(local_player.getChieftain(), 173 * 2, 153 * 2, 7, 1, runnable);
            java.lang.Runnable dialog11 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header11"), i18n("dialog11"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog_2);
                changeObjective(1);
            });
            java.lang.Runnable dialog10 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header10"), i18n("dialog10"), getCampaign().getIcons().getFaces()[8], com.oddlabs.tt.gui.Origin.AT_END, dialog11);
                addModalForm(dialog_3);
            });
            java.lang.Runnable dialog9 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_4 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header9"), i18n("dialog9"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog10);
                addModalForm(dialog_4);
            });
            java.lang.Runnable dialog8 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_5 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header8"), i18n("dialog8"), getCampaign().getIcons().getFaces()[8], com.oddlabs.tt.gui.Origin.AT_END, dialog9);
                addModalForm(dialog_5);
            });
            java.lang.Runnable dialog7 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_6 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header7"), i18n("dialog7"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog8);
                addModalForm(dialog_6);
            });
            java.lang.Runnable dialog6 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_7 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header6"), i18n("dialog6"), getCampaign().getIcons().getFaces()[8], com.oddlabs.tt.gui.Origin.AT_END, dialog7);
                addModalForm(dialog_7);
            });
            java.lang.Runnable dialog5 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_8 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header5"), i18n("dialog5"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog6);
                addModalForm(dialog_8);
            });
            java.lang.Runnable dialog4 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_9 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header4"), i18n("dialog4"), getCampaign().getIcons().getFaces()[8], com.oddlabs.tt.gui.Origin.AT_END, dialog5);
                addModalForm(dialog_9);
            });
            java.lang.Runnable dialog3 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_10 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header3"), i18n("dialog3"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog4);
                addModalForm(dialog_10);
            });
            java.lang.Runnable dialog2 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_11 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[8], com.oddlabs.tt.gui.Origin.AT_END, dialog3);
                addModalForm(dialog_11);
            });
            java.lang.Runnable dialog1 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_12 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, dialog2);
                addModalForm(dialog_12);
                getViewer().getLocalPlayer().enableMagic(1, true);
                local_player.getChieftain().increaseMagicEnergy(0, 1000);
                local_player.getChieftain().increaseMagicEnergy(1, 1000);
            });
            new com.oddlabs.tt.trigger.campaign.NearPointTrigger(173, 153, 3, local_player.getChieftain(), dialog1);
            float shadow_diameter = 2.6f;
            float offset = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID / 2.0f);
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(173 * 2 + offset), (float)(153 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[2], shadow_diameter, true, i18n("statue"));
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 177, 159);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 180, 176);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 165, 195);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 169, 198);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 152, 209);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 200, 197);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 199, 169);
            new com.oddlabs.tt.model.Unit(enemy, 173 * 2, 188 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 173 * 2, 188 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 173 * 2, 188 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 173 * 2, 188 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 173 * 2, 188 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 175 * 2, 190 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 175 * 2, 190 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 175 * 2, 190 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 175 * 2, 190 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 175 * 2, 190 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 178 * 2, 192 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 178 * 2, 192 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 178 * 2, 192 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 178 * 2, 192 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 178 * 2, 192 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 185 * 2, 194 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 185 * 2, 194 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 185 * 2, 194 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 185 * 2, 194 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 185 * 2, 194 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 181 * 2, 195 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 164 * 2, 203 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 114 * 2, 163 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 114 * 2, 163 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 118 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 118 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 109 * 2, 153 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 109 * 2, 153 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 122 * 2, 151 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 122 * 2, 151 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 98 * 2, 137 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 98 * 2, 137 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 93 * 2, 130 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 93 * 2, 130 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 86 * 2, 132 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 86 * 2, 132 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 72 * 2, 146 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 72 * 2, 146 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 158 * 2, 97 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 158 * 2, 97 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 132 * 2, 118 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 132 * 2, 118 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 157 * 2, 135 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 157 * 2, 135 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
        }
        // Java: 298
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 303
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 308
        public override string getCurrentObjective()
        {
            return i18n("objective" + (objective));
        }
        // Java: 313
        public void changeObjective(int objective)
        {
            this.objective = objective;
        }
        static VikingIsland10()
        {
            VikingIsland10.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland10).getName());
        }
        private void init_VikingIsland10()
        {
            this.objective = 0;
        }
        public VikingIsland10(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
            init_VikingIsland10();
        }
    }
}
