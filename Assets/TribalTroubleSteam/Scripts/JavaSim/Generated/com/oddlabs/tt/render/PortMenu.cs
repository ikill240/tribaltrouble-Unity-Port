// Translated from PortMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 45
    public class PortMenu : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        // Java: 55
        public static java.lang.Runnable boot(java.nio.file.Path game_dir, com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI[] gui_out)
        {
            com.oddlabs.tt.global.Settings settings = new com.oddlabs.tt.global.Settings();
            settings.load(game_dir);
            string language = settings.language;
            if (java.lang.JO.equals(language, "default"))
            {
                language = java.util.Locale.getDefault().getLanguage();
            }
            if (!com.oddlabs.tt.gui.Languages.hasLanguage(language))
            {
                language = "en";
            }
            java.util.Locale.setDefault(java.util.Locale.of(language));
            com.oddlabs.tt.global.Settings.setSettings(settings);
            com.oddlabs.tt.global.Globals.gamespeed = settings.gamespeed;
            com.oddlabs.tt.render.PortMenu.initNetwork(network);
            java.nio.file.Path event_log_dir = game_dir.resolve("logs").resolve(java.lang.Long.toString(java.lang.JSystem.currentTimeMillis()));
            com.oddlabs.tt.render.Renderer.getLocalInput().settings(game_dir, event_log_dir, settings);
            com.oddlabs.tt.audio.AudioManager.getManager();
            com.oddlabs.tt.global.GlobalsInit.init();
            com.oddlabs.tt.render.Renderer.getLocalInput().init();
            com.oddlabs.tt.render.Renderer.getRenderer().syncDisplaySize(null);
            com.oddlabs.tt.gui.GUI gui = new com.oddlabs.tt.gui.GUI();
            gui_out[0] = gui;
            return com.oddlabs.tt.render.PortMenu.setupMainMenu(network, gui, true);
        }
        // Java: 78
        public static java.lang.Runnable setupMainMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui, bool first_progress)
        {
            com.oddlabs.tt.resource.WorldGenerator generator = new com.oddlabs.tt.resource.IslandGenerator(256, com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE, com.oddlabs.tt.global.Globals.LANDSCAPE_HILLS, com.oddlabs.tt.global.Globals.LANDSCAPE_VEGETATION, com.oddlabs.tt.global.Globals.LANDSCAPE_RESOURCES, com.oddlabs.tt.global.Globals.LANDSCAPE_SEED, false);
            return com.oddlabs.tt.form.ProgressForm.setProgressForm(network, gui, new com.oddlabs.tt.form.LoadCallback_L((com.oddlabs.tt.gui.GUIRoot gui_root) =>
            {
                return com.oddlabs.tt.render.PortMenu.finishMainMenu(network, gui_root, first_progress, generator);
            }), first_progress);
        }
        // Java: 86
        public static com.oddlabs.tt.render.UIRenderer finishMainMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, bool first_progress, com.oddlabs.tt.resource.WorldGenerator generator)
        {
            com.oddlabs.tt.animation.AnimationManager.freezeTime();
            com.oddlabs.tt.player.PlayerInfo player_info = new com.oddlabs.tt.player.PlayerInfo(0, 0, "");
            com.oddlabs.tt.render.MatrixStack modelViewStack = new com.oddlabs.tt.render.MatrixStack();
            com.oddlabs.tt.render.MatrixStack projectionStack = new com.oddlabs.tt.render.MatrixStack();
            com.oddlabs.tt.landscape.WorldParameters world_params = com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL).initialUnitCount(2).maxUnitCount(com.oddlabs.tt.player.Player.DEFAULT_MAX_UNIT_COUNT).night(com.oddlabs.tt.model.GeometryRegistry.isEventActive(com.oddlabs.tt.model.GeometryRegistry.HALLOWEEN)).build();
            com.oddlabs.tt.player.PlayerInfo[] players = new com.oddlabs.tt.player.PlayerInfo[] { player_info };
            com.oddlabs.tt.resource.WorldInfo world_info = com.oddlabs.tt.render.PortMenu.menuWorld(generator, players.Length, world_params.getInitialUnitCount());
            com.oddlabs.tt.resource.FogInfo fog_info = generator.getFogInfo();
            com.oddlabs.tt.render.RenderQueues render_queues = new com.oddlabs.tt.render.RenderQueues();
            com.oddlabs.tt.landscape.LandscapeResources landscape_resources = com.oddlabs.tt.landscape.World.loadCommon(render_queues);
            com.oddlabs.tt.landscape.World world = com.oddlabs.tt.landscape.World.newWorld(com.oddlabs.tt.audio.AudioManager.getManager(), landscape_resources, null, new PortMenu.Anon1_NotificationListener(), world_params, world_info, generator.getTerrainType(), players, fog_info);
            com.oddlabs.tt.animation.AnimationManager manager = new com.oddlabs.tt.animation.AnimationManager();
            com.oddlabs.tt.render.LandscapeRenderer landscape_renderer = new com.oddlabs.tt.render.LandscapeRenderer(world, world_info, manager);
            com.oddlabs.tt.player.Player local_player = world.getPlayers()[0];
            com.oddlabs.tt.viewer.Selection selection = new com.oddlabs.tt.viewer.Selection(local_player);
            com.oddlabs.tt.render.UIRenderer renderer = new com.oddlabs.tt.render.DefaultRenderer(null, local_player, render_queues, world_info, landscape_renderer, new com.oddlabs.tt.render.Picker(manager, local_player, gui_root, render_queues, landscape_renderer, selection), selection, generator, modelViewStack, projectionStack, null);
            com.oddlabs.tt.render.Renderer.getRenderer().setMusicPath("/music/menu.ogg", 0.0f);
            com.oddlabs.tt.@delegate.MainMenu main_menu = new com.oddlabs.tt.@delegate.MainMenu(network, gui_root, new com.oddlabs.tt.camera.MenuCamera(world, manager));
            gui_root.pushDelegate(main_menu);
            if (first_progress && !java.lang.JO.equals(com.oddlabs.util.BuildInfo.VERSION, com.oddlabs.tt.global.Settings.getSettings().patch_notes_seen_version))
            {
                com.oddlabs.tt.global.Settings.getSettings().patch_notes_seen_version = com.oddlabs.util.BuildInfo.VERSION;
                com.oddlabs.tt.global.Settings.getSettings().save();
                if (com.oddlabs.tt.global.Settings.getSettings().show_patch_notes_on_update)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.PatchNotesForm());
                }
            }
            if (first_progress && com.oddlabs.tt.global.Settings.getSettings().warning_no_sound && !com.oddlabs.tt.render.Renderer.getLocalInput().audioIsCreated())
            {
                java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.render.Renderer).getName());
                gui_root.addModalForm(new com.oddlabs.tt.form.WarningForm(bundle.getString("sound_not_available_caption"), bundle.getString("sound_not_available_message")));
            }
            if (!com.oddlabs.tt.render.PortMenu.initNetwork(network))
            {
                java.util.ResourceBundle bundle_2 = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.render.Renderer).getName());
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(bundle_2.getString("network_not_available_caption"), bundle_2.getString("network_not_available_message"), bundle_2.getString("quit"), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    com.oddlabs.tt.render.Renderer.shutdown();
                })));
            }
            return renderer;
        }
        public static com.oddlabs.tt.resource.WorldInfo menu_world_info;
        public static int menu_world_detail;
        // Java: 138
        public static com.oddlabs.tt.resource.WorldInfo menuWorld(com.oddlabs.tt.resource.WorldGenerator generator, int players, int initial_units)
        {
            int detail = com.oddlabs.tt.global.Settings.getSettings().graphic_detail;
            if (com.oddlabs.tt.render.PortMenu.menu_world_info == null || com.oddlabs.tt.render.PortMenu.menu_world_detail != detail)
            {
                com.oddlabs.tt.render.PortMenu.menu_world_info = generator.generate(players, initial_units, 0.0f);
                com.oddlabs.tt.render.PortMenu.menu_world_detail = detail;
            }
            com.oddlabs.tt.resource.WorldInfo w = com.oddlabs.tt.render.PortMenu.menu_world_info;
            return new com.oddlabs.tt.resource.WorldInfo(w.meters_per_world(), w.sea_level_meters(), w.texels_per_colormap(), w.chunks_per_colormap(), w.colormaps(), w.maps(), w.detail(), com.oddlabs.tt.render.PortMenu.copy(w.heightmap()), w.trees(), w.palm_trees(), w.rocks(), w.iron(), com.oddlabs.tt.render.PortMenu.copy(w.plants()), com.oddlabs.tt.render.PortMenu.copy(w.access_grid()), com.oddlabs.tt.render.PortMenu.copy(w.dock_grid()), com.oddlabs.tt.render.PortMenu.copy(w.water_grid()), com.oddlabs.tt.render.PortMenu.copy(w.build_grid()), com.oddlabs.tt.render.PortMenu.copy(w.island_ids()), w.island_infos(), com.oddlabs.tt.render.PortMenu.copy(w.starting_locations()), w.blend_infos(), com.oddlabs.tt.render.PortMenu.copy(w.ground()), w.seed());
        }
        // Java: 153
        public static float[][] copy(float[][] a)
        {
            float[][] r = new float[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = ((float[])a[i].clone());
            }
            return r;
        }
        // Java: 160
        public static bool[][] copy(bool[][] a)
        {
            bool[][] r = new bool[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = ((bool[])a[i].clone());
            }
            return r;
        }
        // Java: 167
        public static sbyte[][] copy(sbyte[][] a)
        {
            sbyte[][] r = new sbyte[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = ((sbyte[])a[i].clone());
            }
            return r;
        }
        // Java: 174
        public static int[][] copy(int[][] a)
        {
            int[][] r = new int[a.Length][];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = ((int[])a[i].clone());
            }
            return r;
        }
        // Java: 181
        public static bool initNetwork(com.oddlabs.net.NetworkSelector network)
        {
            bool is_network_created = default;
            try
            {
                network.initSelector();
                com.oddlabs.util.Utils.tryGetLoopbackAddress();
                is_network_created = true;
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.tt.render.PortMenu.logger.log(java.util.logging.Level.SEVERE, "Failed to initialize network", e);
                is_network_created = false;
            }
            return com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic().log(is_network_created);
        }
        static PortMenu()
        {
            PortMenu.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.render.PortMenu).getName());
            PortMenu.menu_world_detail = (-1);
        }
        public PortMenu() : base()
        {
        }
        // Java: anonymous class at line 102
        public sealed class Anon1_NotificationListener : java.lang.Object, com.oddlabs.tt.landscape.NotificationListener
        {
            public Anon1_NotificationListener()
            {
            }
            // mixin of default method NotificationListener.newAttackNotification
            public void newAttackNotification(com.oddlabs.tt.model.Selectable target)
            {
            }
            // mixin of default method NotificationListener.newSelectableNotification
            public void newSelectableNotification(com.oddlabs.tt.model.Selectable target)
            {
            }
            // mixin of default method NotificationListener.registerTarget
            public void registerTarget(com.oddlabs.tt.util.Target target)
            {
            }
            // mixin of default method NotificationListener.unregisterTarget
            public void unregisterTarget(com.oddlabs.tt.util.Target target)
            {
            }
            // mixin of default method NotificationListener.patchesEdited
            public void patchesEdited(int patch_x0, int patch_y0, int patch_x1, int patch_y1)
            {
            }
            // mixin of default method NotificationListener.gamespeedChanged
            public void gamespeedChanged(int speed)
            {
            }
            // mixin of default method NotificationListener.playerGamespeedChanged
            public void playerGamespeedChanged()
            {
            }
            // mixin of default method NotificationListener.playerCamera
            public void playerCamera(com.oddlabs.tt.player.Player player, float x, float y, float z, float horiz_angle, float vert_angle)
            {
            }
            // mixin of default method NotificationListener.playerCursor
            public void playerCursor(com.oddlabs.tt.player.Player player, float x, float y, bool on_map)
            {
            }
            // mixin of default method NotificationListener.playerSelection
            public void playerSelection(com.oddlabs.tt.player.Player player, com.oddlabs.tt.model.Selectable[] selection)
            {
            }
            // mixin of default method NotificationListener.playerMapMode
            public void playerMapMode(com.oddlabs.tt.player.Player player, bool on)
            {
            }
            // mixin of default method NotificationListener.playerTargeting
            public void playerTargeting(com.oddlabs.tt.player.Player player, bool on)
            {
            }
            // mixin of default method NotificationListener.playerPanelMenu
            public void playerPanelMenu(com.oddlabs.tt.player.Player player, int submenu)
            {
            }
            // mixin of default method NotificationListener.playerPlacing
            public void playerPlacing(com.oddlabs.tt.player.Player player, int building_index, int grid_x, int grid_y, bool placing)
            {
            }
            // mixin of default method NotificationListener.playerSelectionBox
            public void playerSelectionBox(com.oddlabs.tt.player.Player player, float x1, float y1, float x2, float y2, bool active)
            {
            }
            // mixin of default method NotificationListener.playerBeacon
            public void playerBeacon(com.oddlabs.tt.player.Player player, float x, float y)
            {
            }
            // mixin of default method NotificationListener.playerOrder
            public void playerOrder(com.oddlabs.tt.player.Player player, float x, float y)
            {
            }
            // mixin of default method NotificationListener.playerLeft
            public void playerLeft(com.oddlabs.tt.player.Player player)
            {
            }
        }
    }
}
