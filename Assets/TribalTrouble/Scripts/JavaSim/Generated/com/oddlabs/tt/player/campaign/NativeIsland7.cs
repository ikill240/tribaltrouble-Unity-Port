// Translated from NativeIsland7.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 27
    public class NativeIsland7 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 30
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland7.bundle, key, args);
        }
        // Java: 38
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING, 0.75f, 1.0f, 0.75f, 925, 7, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            int ai_peons = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(10), 0 => (int)(20), 2 => (int)(40), _ => throw new System.ArgumentException() });
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, ai_peons, 0, 0, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 72
        public override void start()
        {
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            java.lang.Runnable dialog1 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            java.lang.Runnable dialog0 = new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[5], com.oddlabs.tt.gui.Origin.AT_END, dialog1);
                addModalForm(dialog_2);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), dialog0);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(7, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.VictoryTrigger(getViewer(), runnable);
            java.util.ResourceBundle player_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            enemy.setActiveChieftain(new com.oddlabs.tt.model.Unit(enemy, 97 * 2, 60 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "chieftain_name"), false));
            enemy.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, 105, 56);
            enemy.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, 108, 79);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 101, 64);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 90, 59);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 87, 70);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 93, 81);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 109, 89);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 115, 90);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 123, 93);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 132, 78);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 106, 118);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 112, 119);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 126, 117);
            float shadow_diameter = 2.6f;
            float dir = ((float)(java.lang.StrictMath.sin(java.lang.Math.PI / 4)));
            java.util.Random r = new java.util.Random(42);
            float w = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(98 * 2 + (float)(w * r.nextFloat())), (float)(62 * 2 + (float)(w * r.nextFloat())), 0, (-1), getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(94 * 2 + (float)(w * r.nextFloat())), (float)(67 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(83 * 2 + (float)(w * r.nextFloat())), (float)(58 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(93 * 2 + (float)(w * r.nextFloat())), (float)(49 * 2 + (float)(w * r.nextFloat())), (-1), 0, getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(97 * 2 + (float)(w * r.nextFloat())), (float)(59 * 2 + (float)(w * r.nextFloat())), (float)(-dir), (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(84 * 2 + (float)(w * r.nextFloat())), (float)(61 * 2 + (float)(w * r.nextFloat())), dir, dir, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(96 * 2 + (float)(w * r.nextFloat())), (float)(49 * 2 + (float)(w * r.nextFloat())), 1, 0, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(100 * 2 + (float)(w * r.nextFloat())), (float)(49 * 2 + (float)(w * r.nextFloat())), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(84 * 2 + (float)(w * r.nextFloat())), (float)(67 * 2 + (float)(w * r.nextFloat())), (-1), 0, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(83 * 2 + (float)(w * r.nextFloat())), (float)(64 * 2 + (float)(w * r.nextFloat())), (-1), 0, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(95 * 2 + (float)(w * r.nextFloat())), (float)(50 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(91 * 2 + (float)(w * r.nextFloat())), (float)(63 * 2 + (float)(w * r.nextFloat())), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(97 * 2 + (float)(w * r.nextFloat())), (float)(50 * 2 + (float)(w * r.nextFloat())), dir, dir, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(93 * 2 + (float)(w * r.nextFloat())), (float)(51 * 2 + (float)(w * r.nextFloat())), dir, dir, getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(93 * 2 + (float)(w * r.nextFloat())), (float)(65 * 2 + (float)(w * r.nextFloat())), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(98 * 2 + (float)(w * r.nextFloat())), (float)(54 * 2 + (float)(w * r.nextFloat())), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(96 * 2 + (float)(w * r.nextFloat())), (float)(51 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(96 * 2 + (float)(w * r.nextFloat())), (float)(54 * 2 + (float)(w * r.nextFloat())), (-1), 0, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(94 * 2 + (float)(w * r.nextFloat())), (float)(52 * 2 + (float)(w * r.nextFloat())), (float)(-dir), dir, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(97 * 2 + (float)(w * r.nextFloat())), (float)(56 * 2 + (float)(w * r.nextFloat())), 0, (-1), getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(94 * 2 + (float)(w * r.nextFloat())), (float)(59 * 2 + (float)(w * r.nextFloat())), (float)(-dir), (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(91 * 2 + (float)(w * r.nextFloat())), (float)(53 * 2 + (float)(w * r.nextFloat())), dir, dir, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(88 * 2 + (float)(w * r.nextFloat())), (float)(57 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(93 * 2 + (float)(w * r.nextFloat())), (float)(53 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(94 * 2 + (float)(w * r.nextFloat())), (float)(53 * 2 + (float)(w * r.nextFloat())), 0, (-1), getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(90 * 2 + (float)(w * r.nextFloat())), (float)(54 * 2 + (float)(w * r.nextFloat())), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(91 * 2 + (float)(w * r.nextFloat())), (float)(54 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(93 * 2 + (float)(w * r.nextFloat())), (float)(61 * 2 + (float)(w * r.nextFloat())), (-1), 0, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(93 * 2 + (float)(w * r.nextFloat())), (float)(54 * 2 + (float)(w * r.nextFloat())), dir, (float)(-dir), getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(94 * 2 + (float)(w * r.nextFloat())), (float)(63 * 2 + (float)(w * r.nextFloat())), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[1], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(90 * 2 + (float)(w * r.nextFloat())), (float)(55 * 2 + (float)(w * r.nextFloat())), 1, 0, getViewer().getWorld().getRacesResources().getTreasures()[3], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(92 * 2 + (float)(w * r.nextFloat())), (float)(55 * 2 + (float)(w * r.nextFloat())), (float)(-dir), dir, getViewer().getWorld().getRacesResources().getTreasures()[4], shadow_diameter, true, i18n("statue"));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(94 * 2 + (float)(w * r.nextFloat())), (float)(55 * 2 + (float)(w * r.nextFloat())), 0, (-1), getViewer().getWorld().getRacesResources().getTreasures()[5], shadow_diameter, true, i18n("statue"));
            shadow_diameter = 4.5f;
            float offset = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID / 2.0f);
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), (float)(91 * 2 + offset), (float)(51 * 2 + offset), 0, 1, getViewer().getWorld().getRacesResources().getTreasures()[0], shadow_diameter, true, i18n("statue"));
        }
        // Java: 208
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 213
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 218
        public override string getCurrentObjective()
        {
            return i18n("objective");
        }
        static NativeIsland7()
        {
            NativeIsland7.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland7).getName());
        }
        public NativeIsland7(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
        }
    }
}
