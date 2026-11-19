// Translated from Menu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 48
    public abstract class Menu : com.oddlabs.tt.@delegate.CameraDelegate
    {
        public static org.joml.Vector4fc COLOR_NORMAL;
        public static org.joml.Vector4fc COLOR_ACTIVE;
        public const int MENU_X = 160;
        public const int overlay_texture_width = 1024;
        public const int overlay_texture_height = 1024;
        public const int overlay_image_width = 800;
        public const int overlay_image_height = 600;
        public const string overlay_texture_name = "/textures/gui/mainmenu";
        public const string discord_texture_name = "/textures/gui/discord";
        public const string github_texture_name = "/textures/gui/github";
        public static java.util.ResourceBundle bundle;
        // Java: 62
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.Menu.bundle, key, args);
        }
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gui.Form current_menu;
        public bool current_menu_centered;
        public com.oddlabs.tt.gui.GUIImage overlay;
        public com.oddlabs.tt.gui.GUIImage logo;
        public com.oddlabs.tt.gui.GUIImage discord;
        public com.oddlabs.tt.gui.GUIImage github;
        // Java: 83
        public com.oddlabs.net.NetworkSelector getNetwork()
        {
            return network;
        }
        // Java: 87
        public void init()
        {
            clearChildren();
            int screen_width = getGUIRoot().getWidth();
            int screen_height = getGUIRoot().getHeight();
            overlay = new com.oddlabs.tt.gui.GUIImage(screen_width, screen_height, 0.0f, 0.0f, (float)(((float)(com.oddlabs.tt.@delegate.Menu.overlay_image_width)) / com.oddlabs.tt.@delegate.Menu.overlay_texture_width), (float)(((float)(com.oddlabs.tt.@delegate.Menu.overlay_image_height)) / com.oddlabs.tt.@delegate.Menu.overlay_texture_height), com.oddlabs.tt.@delegate.Menu.overlay_texture_name);
            overlay.setPos(0, 0);
            addChild(overlay);
            string logo_file = com.oddlabs.tt.@delegate.Menu.i18n("logo_file");
            float heightScale = (float)(screen_height / 600.0f);
            int logoHeight = java.lang.JC.f2i(((float)(206.0f * heightScale)));
            int logoWidth = java.lang.JC.f2i(((float)(347.0f * heightScale)));
            logo = new com.oddlabs.tt.gui.GUIImage(logoWidth, logoHeight, 0.0f, 0.0f, (float)(347.0f / 512.0f), (float)(206.0f / 256.0f), logo_file);
            logo.setPos(0, screen_height - logoHeight);
            addChild(logo);
            github = new com.oddlabs.tt.gui.GUIImage(76, 76, 0.0f, 0.0f, 1.0f, 1.0f, com.oddlabs.tt.@delegate.Menu.github_texture_name, true);
            github.setPos(screen_width - github.getWidth() - 20, github.getHeight() / 2 + 4);
            github.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                com.oddlabs.tt.@delegate.Menu.openURL(com.oddlabs.tt.global.Settings.GITHUB_URL);
            }));
            addChild(github);
            discord = new com.oddlabs.tt.gui.GUIImage(80, 80, 0.0f, 0.0f, 1.0f, 1.0f, com.oddlabs.tt.@delegate.Menu.discord_texture_name, true);
            discord.setPos(screen_width - discord.getWidth() - 20 - github.getWidth() - 20, discord.getHeight() / 2);
            discord.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                com.oddlabs.tt.@delegate.Menu.openURL(com.oddlabs.tt.global.Settings.DISCORD_URL);
            }));
            addChild(discord);
        }
        // Java: 118
        public static void openURL(string url)
        {
            com.oddlabs.tt.util.PortHost.browse(url);
        }
        // Java: 122
        public void addDefaultOptionsButton()
        {
            addOptionsButton(new com.oddlabs.tt.@delegate.FormFactory_L(() =>
            {
                return new com.oddlabs.tt.form.OptionsMenu(getGUIRoot());
            }));
        }
        // Java: 126
        public void addOptionsButton(com.oddlabs.tt.@delegate.FormFactory factory)
        {
            com.oddlabs.tt.gui.MenuButton options = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("options"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            options.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                setMenuCentered(factory.create());
            }));
            addChild(options);
        }
        // Java: 132
        public void addExitButton()
        {
            com.oddlabs.tt.gui.MenuButton exit = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("quit"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            exit.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                setMenuCentered(new com.oddlabs.tt.form.QuitForm(getGUIRoot()));
            }));
            addChild(exit);
        }
        // Java: 138
        public abstract void addButtons();
        // Java: 140
        public void reload()
        {
            init();
            addButtons();
            displayChangedNotify(getGUIRoot().getWidth(), getGUIRoot().getHeight());
        }
        // Java: 147
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if ((((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT)) && @event.hasActions())
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_NEXT))
                {
                    switchFocus(com.oddlabs.tt.gui.FocusDirection.FORWARD);
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_PREV))
                {
                    switchFocus(com.oddlabs.tt.gui.FocusDirection.BACKWARD);
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_UP))
                {
                    focusPrior();
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN))
                {
                    focusNext();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 179
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
            int y = height - java.lang.JC.f2i(((float)((float)(190.0f * height) / com.oddlabs.tt.@delegate.Menu.overlay_image_height)));
            int x = 15;
            overlay.setDim(width, height);
            float heightScale = (float)(height / 600.0f);
            int logoHeight = java.lang.JC.f2i(((float)(206.0f * heightScale)));
            int logoWidth = java.lang.JC.f2i(((float)(347.0f * heightScale)));
            logo.setDim(logoWidth, logoHeight);
            logo.setPos(0, height - logoHeight);
            if (github != null)
            {
                github.setPos(width - github.getWidth() - 20, github.getHeight() / 2 + 4);
            }
            if (discord != null)
            {
                discord.setPos(width - discord.getWidth() - 20 - ((github != null ? github.getWidth() + 20 : 0)), discord.getHeight() / 2);
            }
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)getLastChild());
            while (child != null)
            {
                if ((child is com.oddlabs.tt.gui.MenuButton))
                {
                    child.setPos(x, y - child.getHeight());
                    y -= java.lang.JC.d2i((child.getHeight() * 0.875d));
                }
                child = ((com.oddlabs.tt.gui.GUIObject)child.getPrior());
            }
            if (current_menu != null)
            {
                if (current_menu_centered)
                {
                    current_menu.centerPos();
                }
                else
                {
                    positionMenu();
                }
            }
        }
        // Java: 222
        public void disableButtons(bool disabled)
        {
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)getLastChild());
            while (child != null)
            {
                if ((child is com.oddlabs.tt.gui.MenuButton button))
                {
                    button.setDisabled(disabled);
                }
                child = ((com.oddlabs.tt.gui.GUIObject)child.getPrior());
            }
        }
        // Java: 232
        public override void setFocus()
        {
            if (current_menu != null)
            {
                current_menu.setFocus();
            }
            else
            {
                com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)getLastChild());
                while (child != null)
                {
                    if ((child is com.oddlabs.tt.gui.MenuButton button))
                    {
                        button.setFocus();
                        break;
                    }
                    child = ((com.oddlabs.tt.gui.GUIObject)child.getPrior());
                }
                base.setFocus();
                focusNext();
            }
        }
        // Java: 250
        public override void mouseScrolled(int amount)
        {
        }
        // Java: 254
        public void setMenuCentered(com.oddlabs.tt.gui.Form menu)
        {
            setMenu(menu);
            menu.centerPos();
            current_menu_centered = true;
        }
        // Java: 260
        public void setMenu(com.oddlabs.tt.gui.Form menu)
        {
            if (current_menu != null)
            {
                current_menu.remove();
            }
            disableButtons(true);
            menu.addCloseListener(new com.oddlabs.tt.guievent.CloseListener_L(() =>
            {
                disableButtons(false);
                current_menu = null;
            }));
            current_menu = menu;
            addChild(current_menu);
            current_menu.setFocus();
            positionMenu();
            current_menu_centered = false;
        }
        // Java: 275
        public void positionMenu()
        {
            current_menu.setPos(com.oddlabs.tt.@delegate.Menu.MENU_X, (getGUIRoot().getHeight() - current_menu.getHeight()) * 2 / 3);
        }
        // Java: 279
        public void addResumeButton()
        {
            com.oddlabs.tt.gui.MenuButton resume = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("resume"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            addChild(resume);
            resume.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                pop();
            }));
        }
        // Java: 285
        public static void completeGameSetupHack(com.oddlabs.tt.viewer.WorldViewer world_viewer)
        {
            world_viewer.getGUIRoot().pushDelegate(world_viewer.getDelegate());
            com.oddlabs.tt.render.Renderer.getRenderer().setMusicPath(world_viewer.getLocalPlayer().getRace().getMusicPath(), 10.0f);
        }
        // Java: 290
        public class DefaultWorldInitAction : java.lang.Object, com.oddlabs.tt.net.WorldInitAction
        {
            // Java: 291
            public void run(com.oddlabs.tt.viewer.WorldViewer viewer)
            {
                new com.oddlabs.tt.trigger.GameOverTrigger(viewer);
                if (viewer.isMultiplayer())
                {
                    viewer.getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.CountdownDelegate(viewer, viewer.getCamera()));
                }
                else
                {
                    com.oddlabs.tt.@delegate.Menu.completeGameSetupHack(viewer);
                }
            }
            public DefaultWorldInitAction() : base()
            {
            }
        }
        // Java: 302
        public com.oddlabs.tt.net.GameNetwork joinGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui, int host_id, bool rated, int gamespeed, string map_code, com.oddlabs.tt.form.SelectGameMenu owner, float random_start_pos, int max_unit_count, int map_size)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = getGUIRoot();
            com.oddlabs.tt.net.Client client = new com.oddlabs.tt.net.Client(null, network, gui, host_id, new com.oddlabs.tt.landscape.WorldParameters(gamespeed, map_code, com.oddlabs.tt.player.Player.INITIAL_UNIT_COUNT, max_unit_count, map_size), new com.oddlabs.tt.viewer.MultiplayerInGameInfo(random_start_pos, rated), new com.oddlabs.tt.@delegate.Menu.DefaultWorldInitAction());
            com.oddlabs.tt.net.GameNetwork game_network = new com.oddlabs.tt.net.GameNetwork(null, client);
            com.oddlabs.tt.form.ConnectingForm connecting_form = new com.oddlabs.tt.form.ConnectingForm(game_network, getGUIRoot(), owner, true);
            client.setConfigurationListener(connecting_form);
            gui_root.addModalForm(connecting_form);
            return game_network;
        }
        // Java: 318
        public static com.oddlabs.tt.net.GameNetwork startNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu owner, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction init_action, com.oddlabs.matchmaking.Game game, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float vegetation_amount, float supplies_amount, int seed, bool archipelago, string[] ai_names)
        {
            return com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, owner, world_params, ingame_info, init_action, game, meters_per_world, terrain, hills, vegetation_amount, supplies_amount, seed, archipelago, ai_names, com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS);
        }
        // Java: 328
        public static com.oddlabs.tt.net.GameNetwork startNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu owner, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction init_action, com.oddlabs.matchmaking.Game game, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float vegetation_amount, float supplies_amount, int seed, bool archipelago, string[] ai_names, int player_count)
        {
            bool multiplayer = ingame_info.isMultiplayer();
            com.oddlabs.tt.resource.WorldGenerator generator = new com.oddlabs.tt.resource.IslandGenerator(meters_per_world, terrain, hills, vegetation_amount, supplies_amount, seed, archipelago);
            java.net.InetAddress address = (multiplayer ? (java.net.InetAddress)(null) : com.oddlabs.util.Utils.getLoopbackAddress());
            com.oddlabs.tt.net.Server server = new com.oddlabs.tt.net.Server(network, game, address, generator, multiplayer, ai_names, player_count);
            server.setWorldParameters(world_params);
            com.oddlabs.tt.net.Client client = new com.oddlabs.tt.net.Client(new java.lang.Runnable_L(() => { server.close(); }), network, gui_root.getGUI(), (-1), world_params, ingame_info, init_action);
            com.oddlabs.tt.net.GameNetwork game_network = new com.oddlabs.tt.net.GameNetwork(server, client);
            com.oddlabs.tt.form.ConnectingForm connecting_form = new com.oddlabs.tt.form.ConnectingForm(game_network, gui_root, owner, multiplayer);
            client.setConfigurationListener(connecting_form);
            gui_root.addModalForm(connecting_form);
            return game_network;
        }
        // Java: 354
        public static com.oddlabs.tt.net.GameNetwork startNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu owner, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction init_action, com.oddlabs.matchmaking.Game game, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float vegetation_amount, float supplies_amount, int seed, bool archipelago, string custom_map_path, string[] ai_names)
        {
            return com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, owner, world_params, ingame_info, init_action, game, meters_per_world, terrain, hills, vegetation_amount, supplies_amount, seed, archipelago, custom_map_path, ai_names, com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS);
        }
        // Java: 364
        public static com.oddlabs.tt.net.GameNetwork startNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu owner, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction init_action, com.oddlabs.matchmaking.Game game, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float vegetation_amount, float supplies_amount, int seed, bool archipelago, string custom_map_path, string[] ai_names, int player_count)
        {
            return com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, owner, world_params, ingame_info, init_action, game, meters_per_world, terrain, hills, vegetation_amount, supplies_amount, seed, archipelago, custom_map_path, ai_names, player_count, null);
        }
        // Java: 376
        public static com.oddlabs.tt.net.GameNetwork startNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu owner, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.net.WorldInitAction init_action, com.oddlabs.matchmaking.Game game, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float vegetation_amount, float supplies_amount, int seed, bool archipelago, string custom_map_path, string[] ai_names, int player_count, com.oddlabs.tt.resource.WorldGenerator generator_override)
        {
            bool multiplayer = ingame_info.isMultiplayer();
            com.oddlabs.tt.resource.WorldGenerator generator = (generator_override != null ? generator_override : (custom_map_path != null ? (com.oddlabs.tt.resource.WorldGenerator)(new com.oddlabs.tt.resource.CustomMapGenerator(custom_map_path, meters_per_world, terrain)) : (com.oddlabs.tt.resource.WorldGenerator)(new com.oddlabs.tt.resource.IslandGenerator(meters_per_world, terrain, hills, vegetation_amount, supplies_amount, seed, archipelago))));
            java.net.InetAddress address = (multiplayer ? (java.net.InetAddress)(null) : com.oddlabs.util.Utils.getLoopbackAddress());
            com.oddlabs.tt.net.Server server = new com.oddlabs.tt.net.Server(network, game, address, generator, multiplayer, ai_names, player_count);
            server.setWorldParameters(world_params);
            com.oddlabs.tt.net.Client client = new com.oddlabs.tt.net.Client(new java.lang.Runnable_L(() => { server.close(); }), network, gui_root.getGUI(), (-1), world_params, ingame_info, init_action);
            com.oddlabs.tt.net.GameNetwork game_network = new com.oddlabs.tt.net.GameNetwork(server, client);
            com.oddlabs.tt.form.ConnectingForm connecting_form = new com.oddlabs.tt.form.ConnectingForm(game_network, gui_root, owner, multiplayer);
            client.setConfigurationListener(connecting_form);
            gui_root.addModalForm(connecting_form);
            return game_network;
        }
        static Menu()
        {
            Menu.COLOR_NORMAL = com.oddlabs.util.Color.WHITE;
            Menu.COLOR_ACTIVE = com.oddlabs.util.Color.argb4v((-13153));
            Menu.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.MainMenu).getName());
        }
        public Menu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.camera.Camera camera) : base(gui_root, camera)
        {
            this.network = network;
            setCanFocus(true);
            setFocusCycle(true);
        }
    }
}
