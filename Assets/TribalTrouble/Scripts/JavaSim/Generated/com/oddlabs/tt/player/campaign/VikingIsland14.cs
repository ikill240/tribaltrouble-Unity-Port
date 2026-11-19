// Translated from VikingIsland14.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 26
    public class VikingIsland14 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 29
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland14.bundle, key, args);
        }
        // Java: 37
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 1024, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.75f, 0.65f, 0.85f, 25, 14, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            int ai_difficulty = default;
            int ai_peons = ((System.Func<int>)(() =>
            {
                switch ((getCampaign().getState().getDifficulty()))
                {
                    case 1:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_NORMAL;
                        return 1;
                    }
                    case 0:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_HARD;
                        return 5;
                    }
                    case 2:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_HARD;
                        return 12;
                    }
                    default:
                    {
                        throw new System.ArgumentException();
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }))();
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, ai_peons, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(3, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, ai_difficulty);
            game_network.getClient().setUnitInfo(3, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, ai_peons, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 83
        public override void start()
        {
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            int start_x = 236 * 2;
            int start_y = 362 * 2;
            java.util.ResourceBundle player_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            local_player.setActiveChieftain(new com.oddlabs.tt.model.Unit(local_player, start_x, start_y, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "chieftain_name"), false));
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
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(14, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            float dir = ((float)(java.lang.StrictMath.sin(java.lang.Math.PI / 4)));
            float offset = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID / 2.0f);
            float shadow_diameter = 4.5f;
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(163 * 2 + offset), (float)(126 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[0], shadow_diameter, true, i18n("statue"));
            shadow_diameter = 2.6f;
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(130 * 2 + offset), (float)(124 * 2 + offset), (float)(-dir), (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(152 * 2 + offset), (float)(138 * 2 + offset), dir, dir, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(152 * 2 + offset), (float)(144 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(140 * 2 + offset), (float)(140 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(143 * 2 + offset), (float)(116 * 2 + offset), 0, (-1), getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(142 * 2 + offset), (float)(131 * 2 + offset), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(423 * 2 + offset), (float)(174 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(408 * 2 + offset), (float)(161 * 2 + offset), (-1), 0, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(426 * 2 + offset), (float)(156 * 2 + offset), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(418 * 2 + offset), (float)(165 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(430 * 2 + offset), (float)(165 * 2 + offset), 1, 0, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(419 * 2 + offset), (float)(170 * 2 + offset), (float)(-dir), dir, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(416 * 2 + offset), (float)(156 * 2 + offset), 0, (-1), getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
        }
        // Java: 172
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 177
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 182
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland14()
        {
            VikingIsland14.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland14).getName());
        }
        public VikingIsland14(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
