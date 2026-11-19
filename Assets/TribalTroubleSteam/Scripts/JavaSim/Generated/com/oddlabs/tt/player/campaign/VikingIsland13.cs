// Translated from VikingIsland13.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 27
    public class VikingIsland13 : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        // Java: 30
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.VikingIsland13.bundle, key, args);
        }
        public int minutes;
        public com.oddlabs.tt.gui.CounterLabel counter;
        public bool alive;
        // Java: 43
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string[] ai_names = ((string[])java.util.stream.IntStream_.range(0, 6).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return i18n("name" + (i));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, 512, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, 1.0f, 1.0f, 0.8f, 16, 13, com.oddlabs.tt.player.campaign.VikingCampaign.MAX_UNITS, ai_names);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, true, getCampaign().getState().getNumPeons(), getCampaign().getState().getNumRockWarriors(), getCampaign().getState().getNumIronWarriors(), getCampaign().getState().getNumRubberWarriors()));
            game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_PASSIVE_CAMPAIGN);
            game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, false, 0, 10, 30, 0));
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 71
        public override void start()
        {
            alive = true;
            counter.start(getViewer().getWorld().getAnimationManagerGameTime());
            counter.setPos(0, 0);
            getViewer().getGUIRoot().addChild(counter);
            java.lang.Runnable runnable = default;
            com.oddlabs.tt.player.Player local_player = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.Player enemy = getViewer().getWorld().getPlayers()[1];
            runnable = new java.lang.Runnable_L(() =>
            {
                string stay_alive_dialog = i18n("dialog0", minutes);
                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), i18n("header0"), stay_alive_dialog, getCampaign().getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START);
                addModalForm(dialog);
            });
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), runnable);
            runnable = new java.lang.Runnable_L(() =>
            {
                getCampaign().getState().setIslandState(13, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                getCampaign().getState().setIslandState(12, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                getCampaign().getState().setIslandState(14, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                if (isAlive())
                {
                    removeCounter();
                }
                getCampaign().victory(getViewer());
            });
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(minutes * 60.0f), runnable);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 167, 60);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 171, 55);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 160, 60);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 142, 70);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 135, 72);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 130, 74);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 125, 76);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 120, 71);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 115, 67);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 95, 68);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 93, 63);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 92, 57);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 90, 52);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 96, 38);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 99, 34);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 105, 24);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 164, 51);
            insertGuardTower(enemy, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, 103, 57);
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
                    throw new System.Exception();
                }
            }
            refillArmory(enemy);
            runnable = new java.lang.Runnable_L(() =>
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
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(3.5f * 60.0f), runnable);
            runnable = new java.lang.Runnable_L(() =>
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
            });
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(4.5f * 60.0f), runnable);
            runnable = new java.lang.Runnable_L(() =>
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
            });
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(6 * 60.0f), runnable);
            runnable = new java.lang.Runnable_L(() =>
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
            });
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(9 * 60.0f), runnable);
            runnable = new java.lang.Runnable_L(() =>
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
            });
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(11 * 60.0f), runnable);
            runnable = new java.lang.Runnable_L(() =>
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
            });
            new com.oddlabs.tt.trigger.campaign.TimeTrigger(getViewer().getWorld(), (float)(12.5f * 60.0f), runnable);
        }
        // Java: 257
        public bool isAlive()
        {
            return alive;
        }
        // Java: 261
        public void removeCounter()
        {
            alive = false;
            counter.stop();
            counter.remove();
        }
        // Java: 267
        public override string getHeader()
        {
            return i18n("header");
        }
        // Java: 272
        public override string getDescription()
        {
            return i18n("description");
        }
        // Java: 277
        public override string getCurrentObjective()
        {
            return i18n("objective", minutes);
        }
        static VikingIsland13()
        {
            VikingIsland13.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.VikingIsland13).getName());
        }
        private void init_VikingIsland13()
        {
            this.minutes = 15;
            this.counter = new com.oddlabs.tt.gui.CounterLabel((float)(minutes * 60.0f), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), true);
        }
        public VikingIsland13(com.oddlabs.tt.player.campaign.Campaign campaign) : base(campaign)
        {
            init_VikingIsland13();
        }
    }
}
