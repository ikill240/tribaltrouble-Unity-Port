// Translated from MainMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 23
    public class MainMenu : com.oddlabs.tt.@delegate.Menu
    {
        // Java: 31
        public void addGameTypeButtons()
        {
            com.oddlabs.tt.gui.MenuButton tutorial = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("tutorial"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            tutorial.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                setMenu(new com.oddlabs.tt.form.TutorialForm(getNetwork(), getGUIRoot()));
            }));
            addChild(tutorial);
            com.oddlabs.tt.gui.MenuButton campaign_menu = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("campaign"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            campaign_menu.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                setMenu(new com.oddlabs.tt.form.CampaignForm(getNetwork(), getGUIRoot(), this));
            }));
            addChild(campaign_menu);
            com.oddlabs.tt.gui.MenuButton single_player = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("skirmish"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            single_player.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                setMenu(new com.oddlabs.tt.form.TerrainMenuForm(getNetwork(), getGUIRoot(), this));
            }));
            addChild(single_player);
            if (!com.oddlabs.tt.global.Settings.getSettings().hide_multiplayer)
            {
                com.oddlabs.tt.gui.MenuButton multi_player = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("multiplayer"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
                multi_player.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_13, int unused_14, int unused_15, int unused_16) =>
                {
                    if (com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
                    {
                        new com.oddlabs.tt.form.SelectGameMenu(getNetwork(), getGUIRoot(), this);
                    }
                    else
                    {
                        com.oddlabs.tt.net.Network.getMatchmakingClient().close();
                        if (com.oddlabs.tt.global.Settings.getSettings().isOfficialServer() && com.oddlabs.tt.steam.SteamManager.getInstance() != null && com.oddlabs.tt.steam.SteamManager.getInstance().getAppID() != 480)   // (Steam's test app, 480: the official server doesn't take its sign-ins)
                        {
                            new com.oddlabs.tt.form.MatchmakingConnectingForm(getNetwork(), getGUIRoot(), null, this, true);
                        }
                        else
                        {
                            new com.oddlabs.tt.form.LoginForm(getNetwork(), getGUIRoot(), this);
                        }
                    }
                }));
                addChild(multi_player);
            }
            com.oddlabs.tt.gui.MenuButton map_editor = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("map_editor"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            map_editor.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_17, int unused_18, int unused_19, int unused_20) =>
            {
                setMenu(new com.oddlabs.tt.mapeditor.MapEditorForm(getNetwork(), getGUIRoot()));
            }));
            addChild(map_editor);
        }
        // Java: 76
        public override void addButtons()
        {
            addGameTypeButtons();
            addDefaultOptionsButton();
            addExitButton();
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                new com.oddlabs.tt.form.SelectGameMenu(getNetwork(), getGUIRoot(), this);
            }
        }
        public MainMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.camera.Camera camera) : base(network, gui_root, camera)
        {
            reload();
            com.oddlabs.tt.steam.SteamManager.clearRichPresence();
            com.oddlabs.tt.steam.SteamManager.setInActiveWorld(false);
        }
    }
}
