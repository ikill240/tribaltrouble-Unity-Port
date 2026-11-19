// Translated from TutorialForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 44
    public class TutorialForm : com.oddlabs.tt.gui.Form
    {
        public const int TUTORIAL_CAMERA = 1;
        public const int TUTORIAL_QUARTERS = 2;
        public const int TUTORIAL_ARMORY = 3;
        public const int TUTORIAL_TOWER = 4;
        public const int TUTORIAL_CHIEFTAIN = 5;
        public const int TUTORIAL_BATTLE = 6;
        public const int NUM_TUTORIALS = 6;
        public static java.util.ResourceBundle bundle;
        // Java: 56
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.TutorialForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        // Java: 63
        public static string formatTutorial(int tutorial_number)
        {
            return com.oddlabs.tt.form.TutorialForm.i18n("tutorial", java.lang.Integer.toString(tutorial_number));
        }
        // Java: 134
        public class TutorialAction : java.lang.Object, com.oddlabs.tt.net.WorldInitAction
        {
            public com.oddlabs.tt.form.TutorialForm.TriggerFactory factory() { return factory_f; }
            public com.oddlabs.tt.tutorial.TutorialInGameInfo ingame_info() { return ingame_info_f; }
            public TutorialAction(com.oddlabs.tt.form.TutorialForm.TriggerFactory factory, com.oddlabs.tt.tutorial.TutorialInGameInfo ingame_info)
            {
                this.factory_f = factory;
                this.ingame_info_f = ingame_info;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is TutorialAction r)) return false; return true && java.lang.JO.equals(factory_f, r.factory_f) && java.lang.JO.equals(ingame_info_f, r.ingame_info_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(factory_f); h = 31 * h + java.lang.JO.hashCode(ingame_info_f); } return h; }
            public override string toString() { return "TutorialAction[" + "factory=" + java.lang.JO.toString(factory_f) + ", ingame_info=" + java.lang.JO.toString(ingame_info_f) + "]"; }
            public com.oddlabs.tt.form.TutorialForm.TriggerFactory factory_f;
            public com.oddlabs.tt.tutorial.TutorialInGameInfo ingame_info_f;
            // Java: 136
            public void run(com.oddlabs.tt.viewer.WorldViewer viewer)
            {
                new com.oddlabs.tt.tutorial.Tutorial(viewer, ingame_info_f, factory_f.create(viewer));
            }
        }
        // Java: 142
        public interface TriggerFactory
        {
            // Java: 143
            com.oddlabs.tt.tutorial.TutorialTrigger create(com.oddlabs.tt.viewer.WorldViewer viewer);
        }
        public sealed class TriggerFactory_L : java.lang.Object, TriggerFactory
        {
            readonly System.Func<com.oddlabs.tt.viewer.WorldViewer, com.oddlabs.tt.tutorial.TutorialTrigger> f;
            public TriggerFactory_L(System.Func<com.oddlabs.tt.viewer.WorldViewer, com.oddlabs.tt.tutorial.TutorialTrigger> f) { this.f = f; }
            public com.oddlabs.tt.tutorial.TutorialTrigger create(com.oddlabs.tt.viewer.WorldViewer p0) { return f(p0); }
        }
        // Java: 146
        public static void startNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.TutorialForm.TriggerFactory factory, int tutorial_num)
        {
            com.oddlabs.tt.tutorial.TutorialInGameInfo ingame_info = new com.oddlabs.tt.tutorial.TutorialInGameInfo();
            com.oddlabs.tt.net.GameNetwork game_network = com.oddlabs.tt.form.TutorialForm.doStartNewGame(network, gui_root, ingame_info, new com.oddlabs.tt.form.TutorialForm.TutorialAction(factory, ingame_info), com.oddlabs.tt.player.Player.INITIAL_UNIT_COUNT, tutorial_num);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 156
        public static com.oddlabs.tt.net.GameNetwork doStartNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.tutorial.TutorialInGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction initial_action, int initial_unit_count, int tutorial_num)
        {
            int size = 256;
            float hills = 1.0f;
            float trees = 1.0f;
            float resources = 1.0f;
            int seed = 2;
            string ai_string = com.oddlabs.tt.form.TutorialForm.i18n("ai");
            com.oddlabs.tt.net.WorldInitAction compound_action = new com.oddlabs.tt.net.WorldInitAction_L((com.oddlabs.tt.viewer.WorldViewer world_viewer) =>
            {
                if (initial_action != null)
                {
                    initial_action.run(world_viewer);
                }
                com.oddlabs.tt.@delegate.Menu.completeGameSetupHack(world_viewer);
            });
            return com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, null, new com.oddlabs.tt.landscape.WorldParameters(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL, "Tutorial" + (tutorial_num), initial_unit_count, com.oddlabs.tt.player.Player.DEFAULT_MAX_UNIT_COUNT), ingame_info, compound_action, null, size, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, hills, trees, 0.0f, seed, false, new string[] { java.lang.JStr.valueOf(ai_string) + "0", java.lang.JStr.valueOf(ai_string) + "1", java.lang.JStr.valueOf(ai_string) + "2", java.lang.JStr.valueOf(ai_string) + "3", java.lang.JStr.valueOf(ai_string) + "4", java.lang.JStr.valueOf(ai_string) + "5" });
        }
        // Java: 176
        public static bool checkTutorial(com.oddlabs.tt.gui.GUIRoot gui_root, int tutorial_number)
        {
            return true;
        }
        // Java: 180
        public static void startTutorial(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int tutorial_number)
        {
            com.oddlabs.tt.tutorial.TutorialInGameInfo ingame_info = default;
            com.oddlabs.tt.net.GameNetwork game_network = default;
            switch ((tutorial_number))
            {
                case 1:
                {
                    com.oddlabs.tt.form.TutorialForm.startNewGame(network, gui_root, new com.oddlabs.tt.form.TutorialForm.TriggerFactory_L((com.oddlabs.tt.viewer.WorldViewer viewer) =>
                    {
                        return new com.oddlabs.tt.tutorial.ScrollTrigger(viewer.getLocalPlayer());
                    }), 1);
                    break;
                }
                case 2:
                {
                    com.oddlabs.tt.form.TutorialForm.startNewGame(network, gui_root, new com.oddlabs.tt.form.TutorialForm.TriggerFactory_L((com.oddlabs.tt.viewer.WorldViewer viewer_2) =>
                    {
                        return new com.oddlabs.tt.tutorial.PlacingDelegateTrigger(viewer_2.getLocalPlayer());
                    }), 2);
                    break;
                }
                case 3:
                {
                    com.oddlabs.tt.form.TutorialForm.startNewGame(network, gui_root, new com.oddlabs.tt.form.TutorialForm.TriggerFactory_L((com.oddlabs.tt.viewer.WorldViewer viewer_3) =>
                    {
                        return new com.oddlabs.tt.tutorial.SelectArmoryTrigger(viewer_3.getLocalPlayer());
                    }), 3);
                    break;
                }
                case 4:
                {
                    ingame_info = new com.oddlabs.tt.tutorial.TutorialInGameInfo();
                    com.oddlabs.tt.net.WorldInitAction action = new com.oddlabs.tt.net.WorldInitAction_L((com.oddlabs.tt.viewer.WorldViewer viewer_4) =>
                    {
                        com.oddlabs.tt.player.Player player = viewer_4.getLocalPlayer();
                        new com.oddlabs.tt.model.Unit(player, player.getStartX(), player.getStartY(), null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                        new com.oddlabs.tt.tutorial.Tutorial(viewer_4, ingame_info, new com.oddlabs.tt.tutorial.SelectTowerTrigger(viewer_4.getLocalPlayer()));
                    });
                    game_network = com.oddlabs.tt.form.TutorialForm.doStartNewGame(network, gui_root, ingame_info, action, 10, 4);
                    game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_TOWER_TUTORIAL);
                    game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 10, 0, 0, 0));
                    game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_TOWER_TUTORIAL);
                    game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
                    game_network.getClient().getServerInterface().startServer();
                    break;
                }
                case 5:
                {
                    ingame_info = new com.oddlabs.tt.tutorial.TutorialInGameInfo();
                    game_network = com.oddlabs.tt.form.TutorialForm.doStartNewGame(network, gui_root, ingame_info, new com.oddlabs.tt.form.TutorialForm.TutorialAction(new com.oddlabs.tt.form.TutorialForm.TriggerFactory_L((com.oddlabs.tt.viewer.WorldViewer viewer_5) =>
                    {
                        return new com.oddlabs.tt.tutorial.BuildingChieftainTrigger(viewer_5.getLocalPlayer());
                    }), ingame_info), com.oddlabs.tt.player.Player.INITIAL_UNIT_COUNT, 5);
                    game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
                    game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_CHIEFTAIN_TUTORIAL);
                    game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
                    game_network.getClient().getServerInterface().startServer();
                    break;
                }
                case 6:
                {
                    ingame_info = new com.oddlabs.tt.tutorial.TutorialInGameInfo();
                    game_network = com.oddlabs.tt.form.TutorialForm.doStartNewGame(network, gui_root, ingame_info, new com.oddlabs.tt.form.TutorialForm.TutorialAction(new com.oddlabs.tt.form.TutorialForm.TriggerFactory_L((com.oddlabs.tt.viewer.WorldViewer unused) =>
                    {
                        return new com.oddlabs.tt.tutorial.TutorialOverTrigger();
                    }), ingame_info), com.oddlabs.tt.player.Player.INITIAL_UNIT_COUNT, 6);
                    game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
                    game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, com.oddlabs.tt.model.RacesResources.RACE_VIKINGS, 1, true, com.oddlabs.tt.net.PlayerSlot.AI_BATTLE_TUTORIAL);
                    game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 0, false, 0, 15, 0, 0));
                    game_network.getClient().getServerInterface().startServer();
                    break;
                }
                default:
                {
                    throw new System.Exception();
                }
            }
        }
        // Java: 240
        public class TutorialListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TutorialForm this_0;
            public int number;
            // Java: 247
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                if (com.oddlabs.tt.form.TutorialForm.checkTutorial(this_0.gui_root, number))
                {
                    com.oddlabs.tt.form.TutorialForm.startTutorial(this_0.network, this_0.gui_root, number);
                    this.this_0.remove();
                }
            }
            public TutorialListener(com.oddlabs.tt.form.TutorialForm this_0_, int number) : base()
            {
                this.this_0 = this_0_;
                this.number = number;
            }
        }
        static TutorialForm()
        {
            TutorialForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.TutorialForm).getName());
        }
        public TutorialForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root) : base()
        {
            this.gui_root = gui_root;
            this.network = network;
            com.oddlabs.tt.gui.Label headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.TutorialForm.i18n("tutorial_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(headline);
            com.oddlabs.tt.gui.HorizButton button_tutorial1 = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.TutorialForm.formatTutorial(com.oddlabs.tt.form.TutorialForm.TUTORIAL_CAMERA), 120);
            button_tutorial1.addMouseClickListener(new com.oddlabs.tt.form.TutorialForm.TutorialListener(this, com.oddlabs.tt.form.TutorialForm.TUTORIAL_CAMERA));
            addChild(button_tutorial1);
            com.oddlabs.tt.gui.Label label_tutorial1 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.TutorialForm.i18n("tutorial1_tip"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label_tutorial1);
            com.oddlabs.tt.gui.HorizButton button_tutorial2 = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.TutorialForm.formatTutorial(com.oddlabs.tt.form.TutorialForm.TUTORIAL_QUARTERS), 120);
            button_tutorial2.addMouseClickListener(new com.oddlabs.tt.form.TutorialForm.TutorialListener(this, com.oddlabs.tt.form.TutorialForm.TUTORIAL_QUARTERS));
            addChild(button_tutorial2);
            com.oddlabs.tt.gui.Label label_tutorial2 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.TutorialForm.i18n("tutorial2_tip"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label_tutorial2);
            com.oddlabs.tt.gui.HorizButton button_tutorial3 = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.TutorialForm.formatTutorial(com.oddlabs.tt.form.TutorialForm.TUTORIAL_ARMORY), 120);
            button_tutorial3.addMouseClickListener(new com.oddlabs.tt.form.TutorialForm.TutorialListener(this, com.oddlabs.tt.form.TutorialForm.TUTORIAL_ARMORY));
            addChild(button_tutorial3);
            com.oddlabs.tt.gui.Label label_tutorial3 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.TutorialForm.i18n("tutorial3_tip"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label_tutorial3);
            com.oddlabs.tt.gui.HorizButton button_tutorial4 = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.TutorialForm.formatTutorial(com.oddlabs.tt.form.TutorialForm.TUTORIAL_TOWER), 120);
            button_tutorial4.addMouseClickListener(new com.oddlabs.tt.form.TutorialForm.TutorialListener(this, com.oddlabs.tt.form.TutorialForm.TUTORIAL_TOWER));
            addChild(button_tutorial4);
            com.oddlabs.tt.gui.Label label_tutorial4 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.TutorialForm.i18n("tutorial4_tip"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label_tutorial4);
            com.oddlabs.tt.gui.HorizButton button_tutorial5 = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.TutorialForm.formatTutorial(com.oddlabs.tt.form.TutorialForm.TUTORIAL_CHIEFTAIN), 120);
            button_tutorial5.addMouseClickListener(new com.oddlabs.tt.form.TutorialForm.TutorialListener(this, com.oddlabs.tt.form.TutorialForm.TUTORIAL_CHIEFTAIN));
            addChild(button_tutorial5);
            com.oddlabs.tt.gui.Label label_tutorial5 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.TutorialForm.i18n("tutorial5_tip"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label_tutorial5);
            com.oddlabs.tt.gui.HorizButton button_tutorial6 = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.TutorialForm.formatTutorial(com.oddlabs.tt.form.TutorialForm.TUTORIAL_BATTLE), 120);
            button_tutorial6.addMouseClickListener(new com.oddlabs.tt.form.TutorialForm.TutorialListener(this, com.oddlabs.tt.form.TutorialForm.TUTORIAL_BATTLE));
            addChild(button_tutorial6);
            com.oddlabs.tt.gui.Label label_tutorial6 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.TutorialForm.i18n("tutorial6_tip"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label_tutorial6);
            com.oddlabs.tt.gui.HorizButton cancel_button = new com.oddlabs.tt.gui.CancelButton(120);
            addChild(cancel_button);
            cancel_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            headline.place();
            button_tutorial1.place(headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_tutorial1.place(button_tutorial1, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_tutorial2.place(button_tutorial1, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_tutorial2.place(button_tutorial2, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_tutorial3.place(button_tutorial2, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_tutorial3.place(button_tutorial3, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_tutorial4.place(button_tutorial3, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_tutorial4.place(button_tutorial4, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_tutorial5.place(button_tutorial4, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_tutorial5.place(button_tutorial5, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_tutorial6.place(button_tutorial5, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_tutorial6.place(button_tutorial6, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            cancel_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            centerPos();
        }
    }
}
