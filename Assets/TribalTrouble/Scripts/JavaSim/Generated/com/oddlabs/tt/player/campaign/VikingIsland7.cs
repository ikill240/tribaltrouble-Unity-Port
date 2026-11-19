// Translated from VikingIsland7.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 25
    public class VikingIsland7 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 32
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland7.bundle, key, args);
        }
        // Java: 36
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.75f, 1.0f, 0.5f, 725925, 7, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            int ai_difficulty = default;
            int ai_peons = ((System.Func<int>)(() =>
            {
                switch ((getCampaign().getState().getDifficulty()))
                {
                    case 1:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_EASY;
                        return 5;
                    }
                    case 0:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_EASY;
                        return 15;
                    }
                    case 2:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_HARD;
                        return 20;
                    }
                    default:
                    {
                        throw new System.ArgumentException();
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }))();
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, ai_difficulty);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, ai_peons, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(3, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, ai_difficulty);
            game_network.getClient().setUnitInfo(3, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, ai_peons, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 86
        public override void start()
        {
            getViewer().getLocalPlayer().enableMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, false);
            com.oddlabs.tt.player.Player enemy0 = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy1 = getViewer().getWorld().getPlayers()[2];
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            }));
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(7, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(6, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(8, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(9, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE);
                getCampaign().getState().setIslandState(11, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE);
                getCampaign().victory(getViewer());
            }));
            insertGuardTower(enemy0, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 83, 70);
            insertGuardTower(enemy1, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 189, 74);
            float shadow_diameter = 2.6f;
            float offset = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID / 2.0f);
            float dir = ((float)(java.lang.StrictMath.sin(java.lang.Math.PI / 4)));
            com.oddlabs.tt.render.SpriteKey[] treasures = getViewer().getWorld().getRacesResources().getTreasures();
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(67 * 2 + offset), (float)(64 * 2 + offset), (-1), 0, treasures[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(70 * 2 + offset), (float)(52 * 2 + offset), (-1), 0, treasures[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(77 * 2 + offset), (float)(63 * 2 + offset), 0, 1, treasures[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(82 * 2 + offset), (float)(52 * 2 + offset), dir, (float)(-dir), treasures[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(76 * 2 + offset), (float)(75 * 2 + offset), dir, dir, treasures[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(205 * 2 + offset), (float)(81 * 2 + offset), dir, dir, treasures[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(199 * 2 + offset), (float)(42 * 2 + offset), dir, (float)(-dir), treasures[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(197 * 2 + offset), (float)(69 * 2 + offset), dir, (float)(-dir), treasures[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(194 * 2 + offset), (float)(77 * 2 + offset), 0, 1, treasures[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(187 * 2 + offset), (float)(70 * 2 + offset), (-1), 0, treasures[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(188 * 2 + offset), (float)(77 * 2 + offset), (float)(-dir), dir, treasures[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(190 * 2 + offset), (float)(65 * 2 + offset), 0, (-1), treasures[5], shadow_diameter, true, i18n("statue"));
        }
        // Java: 151
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 156
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 161
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland7()
        {
            VikingIsland7.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland7).getName());
        }
        public VikingIsland7(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
