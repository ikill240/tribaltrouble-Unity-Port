// Translated from DefaultInGameInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 30
    public class DefaultInGameInfo : java.lang.Object, com.oddlabs.tt.viewer.InGameInfo
    {
        public static java.util.ResourceBundle terrain_menu_bundle;
        public com.oddlabs.matchmaking.RosterTemplate roster;
        public bool replay_island_flag;
        // Java: 43
        public void addAbortButton(com.oddlabs.tt.@delegate.InGameMainMenu menu)
        {
            string abort_text = com.oddlabs.tt.@delegate.Menu.i18n("end_game");
            menu.addAbortButton(abort_text);
        }
        // Java: 48
        public virtual float getRandomStartPosition()
        {
            return 0.0f;
        }
        // Java: 53
        public virtual bool isRated()
        {
            return false;
        }
        // Java: 58
        public virtual void addGameOverGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.GameStatsDelegate @delegate, int header_y, com.oddlabs.tt.gui.Group group)
        {
            addGameOverGUI(viewer, @delegate, header_y, group, true);
        }
        // Java: 64
        public void addGameOverGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.GameStatsDelegate @delegate, int header_y, com.oddlabs.tt.gui.Group group, bool replay)
        {
            string map_code_str = com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("map_code", viewer.getParameters().getMapcode());
            com.oddlabs.tt.gui.Label map_code = new com.oddlabs.tt.gui.Label(map_code_str, com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            @delegate.addChild(map_code);
            map_code.setPos((@delegate.getWidth() - map_code.getWidth()) / 2, header_y - map_code.getHeight());
            com.oddlabs.tt.gui.HorizButton button_replay = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("replay_island"), 150);
            button_replay.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                replay_island_flag = true;
                @delegate.startMenu();
            }));
            com.oddlabs.tt.gui.HorizButton button_observer = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("observer_mode"), 150);
            button_observer.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                @delegate.getViewer().getDelegate().setObserverMode();
                @delegate.pop();
            }));
            com.oddlabs.tt.gui.HorizButton button_end = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("main_menu"), 150);
            button_end.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                @delegate.startMenu();
            }));
            if (replay)
            {
                group.addChild(button_replay);
            }
            group.addChild(button_observer);
            group.addChild(button_end);
            button_end.place();
            button_observer.place(button_end, com.oddlabs.tt.gui.Placement.LEFT_MID);
            if (replay)
            {
                button_replay.place(button_observer, com.oddlabs.tt.gui.Placement.LEFT_MID);
            }
        }
        // Java: 96
        public void addGameInfos(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.Menu menu, com.oddlabs.tt.gui.Group game_infos)
        {
            com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
            com.oddlabs.tt.gui.ScrollableGroup scrollable = new com.oddlabs.tt.gui.ScrollableGroup(300, 50);
            java.util.ArrayList nameLabels = new java.util.ArrayList();
            int largestLabel = 0;
            foreach (com.oddlabs.tt.player.Player player in players)
            {
                com.oddlabs.tt.gui.Label name = new com.oddlabs.tt.gui.Label(player.getPlayerInfo().getName(), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
                nameLabels.add(name);
                if (name.getWidth() > largestLabel)
                {
                    largestLabel = name.getWidth();
                }
            }
            com.oddlabs.tt.gui.GUIObject last_name = null;
            for (int i = 0; i < players.Length; i++)
            {
                com.oddlabs.tt.player.PlayerInfo player_info = players[i].getPlayerInfo();
                org.joml.Vector4fc color_floats = players[i].getColor();
                org.joml.Vector4fc color = (viewer.getPeerHub().isAlive(players[i]) ? color_floats : (org.joml.Vector4fc)(new org.joml.Vector4f(color_floats.x(), color_floats.y(), color_floats.z(), 0.25f)));
                com.oddlabs.tt.gui.Label name_2 = ((com.oddlabs.tt.gui.Label)nameLabels.get(i));
                name_2.setDim(largestLabel, name_2.getHeight());
                name_2.setColor(color);
                string race_str = com.oddlabs.tt.model.RacesResources.getRaceName(player_info.getRace());
                com.oddlabs.tt.gui.Label race = new com.oddlabs.tt.gui.Label(race_str, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont()).setColor(color);
                string team_str = com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.viewer.DefaultInGameInfo.terrain_menu_bundle, "team", java.lang.Integer.toString(player_info.getTeam() + 1));
                com.oddlabs.tt.gui.Label team = new com.oddlabs.tt.gui.Label(team_str, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont()).setColor(color);
                scrollable.addChild(name_2);
                if (last_name != null)
                {
                    name_2.place(last_name, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                }
                else
                {
                    name_2.place();
                }
                last_name = name_2;
                scrollable.addChild(race);
                race.place(last_name, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                scrollable.addChild(team);
                team.place(race, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            }
            scrollable.compileCanvas();
            game_infos.addChild(scrollable);
            scrollable.place();
            game_infos.compileCanvas();
        }
        // Java: 145
        public virtual void addGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.InGameMainMenu menu, com.oddlabs.tt.gui.Group game_infos)
        {
            addAbortButton(menu);
            addGameInfos(viewer, menu, game_infos);
        }
        // Java: 151
        public void close(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            if (replay_island_flag)
            {
                com.oddlabs.tt.form.TerrainMenu menu = new com.oddlabs.tt.form.TerrainMenu(viewer.getNetwork(), viewer.getGUIRoot(), null, false, null);
                menu.parseMapcode(viewer.getParameters().getMapcode());
                menu.applyWorldParameters(viewer.getParameters());
                if (roster != null)
                {
                    menu.applyRoster(roster);
                }
                menu.startGame();
            }
            else
            {
                com.oddlabs.tt.render.Renderer.startMenu(viewer.getNetwork(), viewer.getGUIRoot().getGUI());
            }
        }
        // Java: 164
        public void abort(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            viewer.getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.GameStatsDelegate(viewer, viewer.getGUIRoot().getDelegate().getCamera(), com.oddlabs.tt.@delegate.Menu.i18n("game_aborted")));
        }
        // Java: 170
        public virtual bool isMultiplayer()
        {
            return false;
        }
        static DefaultInGameInfo()
        {
            DefaultInGameInfo.terrain_menu_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.TerrainMenu).getName());
        }
        public DefaultInGameInfo() : this(null)
        {
        }
        public DefaultInGameInfo(com.oddlabs.matchmaking.RosterTemplate roster) : base()
        {
            this.roster = roster;
        }
    }
}
