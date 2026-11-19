// Translated from VikingIsland3.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 24
    public class VikingIsland3 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 27
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland3.bundle, key, args);
        }
        // Java: 35
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 0.75f, 1.0f, 0.5f, 96443, 3, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            int ai_difficulty = default;
            int ai_peons = ((System.Func<int>)(() =>
            {
                switch ((getCampaign().getState().getDifficulty()))
                {
                    case 1:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_NORMAL;
                        return 2;
                    }
                    case 0:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_HARD;
                        return 5;
                    }
                    case 2:
                    {
                        ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_HARD;
                        return 15;
                    }
                    default:
                    {
                        throw new System.ArgumentException();
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }))();
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, ai_difficulty);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 1, false, ai_peons, 0, 5, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 79
        public override void start()
        {
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            runnable = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(3, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(2, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(4, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(8, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            enemy.getAI().manTowers(1);
            float shadow_diameter = 2.6f;
            float offset = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID / 2.0f);
            float dir = ((float)(java.lang.StrictMath.sin(java.lang.Math.PI / 4)));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(134 * 2 + offset), (float)(29 * 2 + offset), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(130 * 2 + offset), (float)(28 * 2 + offset), 0, (-1), getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(130 * 2 + offset), (float)(34 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(125 * 2 + offset), (float)(37 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(121 * 2 + offset), (float)(32 * 2 + offset), (-1), 0, getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(124 * 2 + offset), (float)(28 * 2 + offset), (float)(-dir), (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(136 * 2 + offset), (float)(38 * 2 + offset), dir, dir, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(139 * 2 + offset), (float)(33 * 2 + offset), 1, 0, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
        }
        // Java: 131
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 136
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 141
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static VikingIsland3()
        {
            VikingIsland3.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland3).getName());
        }
        public VikingIsland3(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
