// Translated from NativeIsland4.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 28
    public class NativeIsland4 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 31
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.NativeIsland4.bundle, key, args);
        }
        public int minutes;
        public com.oddlabs.tt.gui.CounterLabel counter;
        public bool alive;
        // Java: 44
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING, 0.8f, 0.8f, 0.8f, 19 * 19, 4, com.oddlabs.tt.player.campaign.NativeCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, 0, 10, 30, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 78
        public override void start()
        {
            getViewer().getLocalPlayer().enableChiefsCourage(false);
            alive = true;
            counter.start(getViewer().getWorld().getAnimationManagerGameTime());
            counter.setPos(0, 0);
            getViewer().getGUIRoot().addChild(counter);
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player captives = getViewer().getWorld().getPlayers()[1];
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[2];
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), new java.lang.Runnable_L(() =>
            {
                addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), i18n("dialog0"), getCampaign().getIcons().getFaces()[2], com.oddlabs.tt.gui.Origin.AT_END, new java.lang.Runnable_L(() =>
                {
                    addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header1"), i18n("dialog1"), getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START));
                })));
            }));
            int start_x = 45 * 2;
            int start_y = 44 * 2;
            getViewer().getCamera().reset(start_x, start_y);
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
            int captive_x = 39;
            int captive_y = 38;
            for (int i_5 = 0; i_5 < 10; i_5++)
            {
                new com.oddlabs.tt.model.Unit(captives, captive_x * 2, captive_y * 2, null, captives.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(minutes * 60.0f), new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(4, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setNumPeons(getCampaign().getState().getNumPeons() + captives.getUnitCountContainer().getNumSupplies());
                if (isAlive())
                {
                    removeCounter();
                }
                getCampaign().victory(getViewer());
            }));
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 125, 161);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 125, 150);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 136, 153);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 180, 184);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 158, 154);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 163, 177);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 173, 175);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 165, 167);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 104, 192);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 108, 185);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 103, 210);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 115, 205);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 155, 185);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 145, 171);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 108, 150);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 130, 189);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 82, 170);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 65, 167);
            int attack1 = default;
            int attack2 = default;
            int attack3 = default;
            int attack4 = default;
            int attack5 = default;
            int attack6 = default;
            switch ((getCampaign().getState().getDifficulty()))
            {
                case 1:
                {
                    attack1 = 5;
                    attack2 = 15;
                    attack3 = 20;
                    attack4 = 35;
                    attack5 = 35;
                    attack6 = 35;
                    break;
                }
                case 0:
                {
                    attack1 = 10;
                    attack2 = 30;
                    attack3 = 40;
                    attack4 = 70;
                    attack5 = 70;
                    attack6 = 70;
                    break;
                }
                case 2:
                {
                    attack1 = 20;
                    attack2 = 60;
                    attack3 = 80;
                    attack4 = 90;
                    attack5 = 90;
                    attack6 = 90;
                    break;
                }
                default:
                {
                    throw new System.ArgumentException("unexpected difficulty: " + (getCampaign().getState().getDifficulty()));
                }
            }
            refillArmory(enemy);
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(3.5f * 60.0f), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain = local_player.getChieftain();
                attack(enemy, new com.oddlabs.tt.landscape.LandscapeTarget(captive_x, captive_y), attack1);
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
            }));
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(4.5f * 60.0f), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory_2 = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain_2 = local_player.getChieftain();
                if (armory_2 != null && !armory_2.isDead())
                {
                    attack(enemy, armory_2, attack2);
                }
                else if (chieftain_2 != null && !chieftain_2.isDead())
                {
                    attack(enemy, chieftain_2, attack2);
                }
                refillArmory(enemy);
                deploy(enemy, attack3);
            }));
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(6 * 60.0f), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory_3 = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain_3 = local_player.getChieftain();
                if (armory_3 != null && !armory_3.isDead())
                {
                    attack(enemy, armory_3, attack3);
                }
                else if (chieftain_3 != null && !chieftain_3.isDead())
                {
                    attack(enemy, chieftain_3, attack3);
                }
                refillArmory(enemy);
                deploy(enemy, attack4);
            }));
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(9 * 60.0f), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory_4 = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain_4 = local_player.getChieftain();
                if (armory_4 != null && !armory_4.isDead())
                {
                    attack(enemy, armory_4, attack4);
                }
                else if (chieftain_4 != null && !chieftain_4.isDead())
                {
                    attack(enemy, chieftain_4, attack4);
                }
                refillArmory(enemy);
                deploy(enemy, attack5);
            }));
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(11 * 60.0f), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory_5 = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain_5 = local_player.getChieftain();
                if (armory_5 != null && !armory_5.isDead())
                {
                    attack(enemy, armory_5, attack5);
                }
                else if (chieftain_5 != null && !chieftain_5.isDead())
                {
                    attack(enemy, chieftain_5, attack5);
                }
                refillArmory(enemy);
                deploy(enemy, attack6);
            }));
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(12.5f * 60.0f), new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.model.Building armory_6 = local_player.getArmory();
                com.oddlabs.tt.model.Unit chieftain_6 = local_player.getChieftain();
                if (armory_6 != null && !armory_6.isDead())
                {
                    attack(enemy, armory_6, attack6);
                }
                else if (chieftain_6 != null && !chieftain_6.isDead())
                {
                    attack(enemy, chieftain_6, attack6);
                }
                refillArmory(enemy);
            }));
            new com.oddlabs.tt.trigger.campaign.PlayerEleminatedTrigger(new java.lang.Runnable_L(() =>
            {
                getCampaign().defeated(getViewer(), i18n("game_over"));
            }), captives);
        }
        // Java: 302
        public bool isAlive()
        {
            return alive;
        }
        // Java: 306
        public void removeCounter()
        {
            alive = false;
            counter.remove();
            getViewer().getWorld().getAnimationManagerGameTime().removeAnimation(counter);
        }
        // Java: 312
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 317
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 322
        public override string getCurrentObjective()
        {
            return i18n("objective", minutes);
        }
        static NativeIsland4()
        {
            NativeIsland4.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.NativeIsland4).getName());
        }
        private void init_NativeIsland4()
        {
            this.minutes = 15;
            this.counter = new com.oddlabs.tt.gui.CounterLabel((float)(minutes * 60.0f), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), true);
        }
        public NativeIsland4(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
            init_NativeIsland4();
        }
    }
}
