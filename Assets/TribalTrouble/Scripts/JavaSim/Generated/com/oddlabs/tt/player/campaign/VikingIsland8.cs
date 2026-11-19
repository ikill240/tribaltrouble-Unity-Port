// Translated from VikingIsland8.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 32
    public class VikingIsland8 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 35
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland8.bundle, key, args);
        }
        public int objective;
        // Java: 45
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 1024, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 1.0f, 1.0f, 0.0f, 285914281, 8, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 1));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 75
        public override void start()
        {
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player lost = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            getViewer().getCamera().reset(170 * 2, 160 * 2);
            getViewer().getCamera().setPos(358 * 2, 484 * 2);
            java.lang.Runnable camera_jump = new java.lang.Runnable_L(() =>
            {
                getViewer().getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), getViewer().getCamera(), 170 * 2, 160 * 2, 200.0f, 3.0f));
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, camera_jump);
                addModalForm(dialog);
            }));
            java.util.ResourceBundle player_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            local_player.setActiveChieftain(new com.oddlabs.tt.model.Unit(local_player, 170 * 2, 160 * 2, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.util.Utils.getBundleString(player_bundle, "chieftain_name"), false));
            local_player.getChieftain().increaseMagicEnergy(0, 1000);
            local_player.getChieftain().increaseMagicEnergy(1, 1000);
            int unit_count = getCampaign().getState().getNumPeons() + getCampaign().getState().getNumRockWarriors() + getCampaign().getState().getNumIronWarriors() + getCampaign().getState().getNumRubberWarriors();
            for (int i = 0; i < unit_count; i++)
            {
                new com.oddlabs.tt.model.Unit(local_player, 170 * 2, 160 * 2, null, local_player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            }
            new com.oddlabs.tt.trigger.campaign.NearPointTrigger(354, 478, 4, local_player.getChieftain(), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog_2);
                getViewer().getLocalPlayer().enableMagic(0, true);
                local_player.getChieftain().increaseMagicEnergy(0, 1000);
                local_player.getChieftain().increaseMagicEnergy(1, 1000);
                setMagicUsedTrigger();
                changeObjective(1);
            }));
            new com.oddlabs.tt.model.SceneryModel(getViewer().getWorld(), 354 * 2, 478 * 2, 0, (-1), local_player.getRace().getRallyPoint());
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 208, 210);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 124, 211);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 139, 223);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 171, 250);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 155, 244);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 68, 189);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 56, 187);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 44, 190);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 180, 124);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 224, 269);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 302, 370);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 312, 394);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 301, 388);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 253, 236);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 278, 316);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK, 261, 166);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 308, 467);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 192, 252);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 203, 267);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, 316, 435);
            com.oddlabs.tt.model.Unit bait = new com.oddlabs.tt.model.Unit(enemy, 240 * 2, 137 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            com.oddlabs.tt.model.Selectable[] army = com.oddlabs.tt.model.Selectable.newArray(new com.oddlabs.tt.model.Unit(enemy, 213 * 2, 122 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)), new com.oddlabs.tt.model.Unit(enemy, 213 * 2, 122 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK)), new com.oddlabs.tt.model.Unit(enemy, 213 * 2, 122 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 213 * 2, 122 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 213 * 2, 122 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 245 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)), new com.oddlabs.tt.model.Unit(enemy, 245 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)), new com.oddlabs.tt.model.Unit(enemy, 245 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 245 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 245 * 2, 170 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 239 * 2, 177 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)), new com.oddlabs.tt.model.Unit(enemy, 239 * 2, 177 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK)), new com.oddlabs.tt.model.Unit(enemy, 239 * 2, 177 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 239 * 2, 177 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 239 * 2, 177 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 286 * 2, 113 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)), new com.oddlabs.tt.model.Unit(enemy, 286 * 2, 113 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK)), new com.oddlabs.tt.model.Unit(enemy, 286 * 2, 113 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 286 * 2, 113 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 286 * 2, 113 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 289 * 2, 141 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)), new com.oddlabs.tt.model.Unit(enemy, 289 * 2, 141 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK)), new com.oddlabs.tt.model.Unit(enemy, 289 * 2, 141 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 289 * 2, 141 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)), new com.oddlabs.tt.model.Unit(enemy, 289 * 2, 141 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)));
            new com.oddlabs.tt.trigger.campaign.DeathTrigger(bait, new java.lang.Runnable_L(() =>
            {
                enemy.setLandscapeTarget(army, 238, 136, com.oddlabs.tt.model.Action.ATTACK, true);
            }));
            com.oddlabs.tt.model.Unit bait2 = new com.oddlabs.tt.model.Unit(enemy, 347 * 2, 455 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            int num_hidden = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(7), 0 => (int)(13), 2 => (int)(20), _ => throw new System.Exception() });
            com.oddlabs.tt.model.Selectable[] army2 = com.oddlabs.tt.model.Selectable.newArray(num_hidden);
            army2[0] = new com.oddlabs.tt.model.Unit(enemy, 364 * 2, 440 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            army2[1] = new com.oddlabs.tt.model.Unit(enemy, 364 * 2, 440 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            army2[2] = new com.oddlabs.tt.model.Unit(enemy, 364 * 2, 440 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            army2[3] = new com.oddlabs.tt.model.Unit(enemy, 364 * 2, 440 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            army2[4] = new com.oddlabs.tt.model.Unit(enemy, 364 * 2, 440 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            army2[5] = new com.oddlabs.tt.model.Unit(enemy, 364 * 2, 440 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            army2[6] = new com.oddlabs.tt.model.Unit(enemy, 364 * 2, 440 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_HARD || getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_NORMAL)
            {
                army2[7] = new com.oddlabs.tt.model.Unit(enemy, 365 * 2, 427 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                army2[8] = new com.oddlabs.tt.model.Unit(enemy, 365 * 2, 427 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                army2[9] = new com.oddlabs.tt.model.Unit(enemy, 365 * 2, 427 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                army2[10] = new com.oddlabs.tt.model.Unit(enemy, 365 * 2, 427 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                army2[11] = new com.oddlabs.tt.model.Unit(enemy, 365 * 2, 427 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                army2[12] = new com.oddlabs.tt.model.Unit(enemy, 365 * 2, 427 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            }
            if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_HARD)
            {
                army2[13] = new com.oddlabs.tt.model.Unit(enemy, 366 * 2, 419 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                army2[14] = new com.oddlabs.tt.model.Unit(enemy, 366 * 2, 419 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                army2[15] = new com.oddlabs.tt.model.Unit(enemy, 366 * 2, 419 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                army2[16] = new com.oddlabs.tt.model.Unit(enemy, 366 * 2, 419 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                army2[17] = new com.oddlabs.tt.model.Unit(enemy, 366 * 2, 419 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                army2[18] = new com.oddlabs.tt.model.Unit(enemy, 366 * 2, 419 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                army2[19] = new com.oddlabs.tt.model.Unit(enemy, 366 * 2, 419 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            }
            new com.oddlabs.tt.trigger.campaign.DeathTrigger(bait2, new java.lang.Runnable_L(() =>
            {
                enemy.setLandscapeTarget(army2, 352, 480, com.oddlabs.tt.model.Action.ATTACK, true);
            }));
            new com.oddlabs.tt.model.Unit(enemy, 348 * 2, 315 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 348 * 2, 315 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 348 * 2, 315 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 348 * 2, 315 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 299 * 2, 321 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 299 * 2, 321 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 299 * 2, 321 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 300 * 2, 453 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 300 * 2, 453 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            new com.oddlabs.tt.model.Unit(enemy, 300 * 2, 453 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 300 * 2, 453 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 300 * 2, 453 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 300 * 2, 453 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 300 * 2, 453 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 352 * 2, 456 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 355 * 2, 459 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 360 * 2, 461 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 365 * 2, 466 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 367 * 2, 474 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 369 * 2, 479 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 348 * 2, 455 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 342 * 2, 459 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 335 * 2, 467 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 334 * 2, 475 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 345 * 2, 465 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 346 * 2, 460 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 347 * 2, 467 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 331 * 2, 374 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 331 * 2, 374 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 331 * 2, 374 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 331 * 2, 374 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 348 * 2, 371 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 348 * 2, 371 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 399 * 2, 399 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 399 * 2, 399 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 399 * 2, 399 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 399 * 2, 399 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 399 * 2, 399 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 354 * 2, 474 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 354 * 2, 474 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            new com.oddlabs.tt.model.Unit(enemy, 354 * 2, 474 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 354 * 2, 474 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 354 * 2, 474 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            new com.oddlabs.tt.model.Unit(enemy, 354 * 2, 474 * 2, null, enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            int num_neutrals = ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(15), 0 => (int)(9), 2 => (int)(6), _ => throw new System.ArgumentException() });
            com.oddlabs.tt.model.Unit[] neutrals = new com.oddlabs.tt.model.Unit[num_neutrals];
            neutrals[0] = new com.oddlabs.tt.model.Unit(lost, 267 * 2, 325 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            neutrals[1] = new com.oddlabs.tt.model.Unit(lost, 268 * 2, 324 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            neutrals[2] = new com.oddlabs.tt.model.Unit(lost, 267 * 2, 323 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            neutrals[3] = new com.oddlabs.tt.model.Unit(lost, 265 * 2, 325 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            neutrals[4] = new com.oddlabs.tt.model.Unit(lost, 265 * 2, 324 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            neutrals[5] = new com.oddlabs.tt.model.Unit(lost, 266 * 2, 326 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
            if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY || getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_NORMAL)
            {
                neutrals[6] = new com.oddlabs.tt.model.Unit(lost, 267 * 2, 327 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                neutrals[7] = new com.oddlabs.tt.model.Unit(lost, 269 * 2, 327 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                neutrals[8] = new com.oddlabs.tt.model.Unit(lost, 270 * 2, 327 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            }
            if (getCampaign().getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY)
            {
                neutrals[9] = new com.oddlabs.tt.model.Unit(lost, 268 * 2, 326 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                neutrals[10] = new com.oddlabs.tt.model.Unit(lost, 271 * 2, 325 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                neutrals[11] = new com.oddlabs.tt.model.Unit(lost, 270 * 2, 324 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                neutrals[12] = new com.oddlabs.tt.model.Unit(lost, 271 * 2, 328 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                neutrals[13] = new com.oddlabs.tt.model.Unit(lost, 269 * 2, 330 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                neutrals[14] = new com.oddlabs.tt.model.Unit(lost, 272 * 2, 323 * 2, null, lost.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
            }
            new com.oddlabs.tt.trigger.campaign.NearArmyTrigger(neutrals, 10.0f, local_player, new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header2"), i18n("dialog2"), getCampaign().getIcons().getFaces()[4], com.oddlabs.tt.gui.Origin.AT_END);
                addModalForm(dialog_3);
                foreach (com.oddlabs.tt.model.Unit neutral in neutrals)
                {
                    if (!neutral.isDead())
                    {
                        changeOwner(neutral, local_player);
                    }
                }
            }));
        }
        // Java: 335
        public void setMagicUsedTrigger()
        {
            java.lang.Runnable runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(8, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(7, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(9, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE);
                getCampaign().getState().setIslandState(11, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE);
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.MagicUsedTrigger(getViewer().getLocalPlayer().getChieftain(), 354 * 2, 478 * 2, 15, 0, runnable);
        }
        // Java: 347
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 352
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 357
        public override string getCurrentObjective()
        {
            return i18n("objective" + (objective));
        }
        // Java: 362
        public void changeObjective(int objective)
        {
            this.objective = objective;
        }
        static VikingIsland8()
        {
            VikingIsland8.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland8).getName());
        }
        private void init_VikingIsland8()
        {
            this.objective = 0;
        }
        public VikingIsland8(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
            init_VikingIsland8();
        }
    }
}
