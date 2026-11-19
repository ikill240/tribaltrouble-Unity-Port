// Translated from GameStatsDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 31
    public class GameStatsDelegate : com.oddlabs.tt.@delegate.CameraDelegate, com.oddlabs.tt.animation.Updatable
    {
        public const int PLAYER_COLUMN_WIDTH = 100;
        public const int TEXT_OFFSET = (-4);
        public static java.util.ResourceBundle bundle;
        public const int PAGINATED_THRESHOLD = 6;
        public const int PAGINATED_STAT_COLUMN_WIDTH = 130;
        public const int PAGINATED_PLAYER_NAME_WIDTH = 120;
        public static int[][] STAT_PAGES;
        // Java: 46
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.GameStatsDelegate.bundle, key, args);
        }
        public com.oddlabs.tt.animation.TimerAnimation delay_timer;
        public com.oddlabs.tt.gui.Group group_buttons;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public string[] paginatedStatNames;
        public int[][] paginatedPlayerStats;
        public com.oddlabs.tt.player.Player[] paginatedPlayers;
        public com.oddlabs.tt.font.Font paginatedFont;
        public int currentPage;
        public com.oddlabs.tt.gui.MultiColumnComboBox paginatedScoreBox;
        // Java: 109
        public com.oddlabs.tt.gui.MultiColumnComboBox buildLegacyLayout(com.oddlabs.tt.player.Player[] players)
        {
            com.oddlabs.tt.gui.ColumnInfo[] score_infos = new com.oddlabs.tt.gui.ColumnInfo[players.Length + 1];
            score_infos[0] = new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("type"), 160);
            for (int i = 0; i < players.Length; i++)
            {
                score_infos[i + 1] = new com.oddlabs.tt.gui.ColumnInfo(players[i].getPlayerInfo().getDisplayName(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH);
            }
            com.oddlabs.tt.gui.MultiColumnComboBox score_box = new com.oddlabs.tt.gui.MultiColumnComboBox(viewer.getGUIRoot(), score_infos, 200);
            com.oddlabs.tt.gui.Label[] units_lost_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            units_lost_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("units_lost"), 0, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_2 = 0; i_2 < players.Length; i_2++)
            {
                units_lost_labels[i_2 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_2].getUnitsLost(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(units_lost_labels, null));
            com.oddlabs.tt.gui.Label[] units_killed_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            units_killed_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("units_killed"), 1, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_3 = 0; i_3 < players.Length; i_3++)
            {
                units_killed_labels[i_3 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_3].getUnitsKilled(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(units_killed_labels, null));
            com.oddlabs.tt.gui.Label[] buildings_lost_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            buildings_lost_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("buildings_lost"), 2, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_4 = 0; i_4 < players.Length; i_4++)
            {
                buildings_lost_labels[i_4 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_4].getBuildingsLost(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(buildings_lost_labels, null));
            com.oddlabs.tt.gui.Label[] buildings_destroyed_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            buildings_destroyed_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("buildings_wrecked"), 3, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_5 = 0; i_5 < players.Length; i_5++)
            {
                buildings_destroyed_labels[i_5 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_5].getBuildingsDestroyed(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(buildings_destroyed_labels, null));
            com.oddlabs.tt.gui.Label[] tree_harvested_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            tree_harvested_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("tree_resources"), 3, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_6 = 0; i_6 < players.Length; i_6++)
            {
                tree_harvested_labels[i_6 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_6].getTreeHarvested(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(tree_harvested_labels, null));
            com.oddlabs.tt.gui.Label[] rock_harvested_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            rock_harvested_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("rock_resources"), 4, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_7 = 0; i_7 < players.Length; i_7++)
            {
                rock_harvested_labels[i_7 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_7].getRockHarvested(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(rock_harvested_labels, null));
            com.oddlabs.tt.gui.Label[] iron_harvested_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            iron_harvested_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("iron_resources"), 5, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_8 = 0; i_8 < players.Length; i_8++)
            {
                iron_harvested_labels[i_8 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_8].getIronHarvested(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(iron_harvested_labels, null));
            com.oddlabs.tt.gui.Label[] rubber_harvested_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            rubber_harvested_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("chicken_resources"), 6, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_9 = 0; i_9 < players.Length; i_9++)
            {
                rubber_harvested_labels[i_9 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_9].getRubberHarvested(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(rubber_harvested_labels, null));
            com.oddlabs.tt.gui.Label[] walked_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            walked_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("meters_walked"), 7, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_10 = 0; i_10 < players.Length; i_10++)
            {
                walked_labels[i_10 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_10].getUnitsMoved() * 2, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(walked_labels, null));
            com.oddlabs.tt.gui.Label[] weapons_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            weapons_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("weapons_thrown"), 8, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_11 = 0; i_11 < players.Length; i_11++)
            {
                weapons_labels[i_11 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_11].getWeaponsThrown(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(weapons_labels, null));
            com.oddlabs.tt.gui.Label[] magics_labels = new com.oddlabs.tt.gui.Label[players.Length + 1];
            magics_labels[0] = new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("magics_used"), 9, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
            for (int i_12 = 0; i_12 < players.Length; i_12++)
            {
                magics_labels[i_12 + 1] = new com.oddlabs.tt.gui.IntegerLabel(players[i_12].getMagics(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(magics_labels, null));
            java.util.List total_labels = new java.util.ArrayList(players.Length + 1);
            total_labels.add(new com.oddlabs.tt.gui.SortedLabel(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("total"), 10, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()));
            foreach (com.oddlabs.tt.player.Player player in players)
            {
                int unit_killed = player.getUnitsKilled();
                int buildings_wrecked = player.getBuildingsDestroyed();
                int tree = player.getTreeHarvested();
                int rock = player.getRockHarvested();
                int iron = player.getIronHarvested();
                int chicken = player.getRubberHarvested();
                int total_score = unit_killed * 10 + buildings_wrecked * 100 + tree + rock + iron * 2 + chicken * 4;
                total_labels.add(new com.oddlabs.tt.gui.IntegerLabel(total_score, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.@delegate.GameStatsDelegate.PLAYER_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET));
            }
            score_box.addRow(new com.oddlabs.tt.gui.Row(((com.oddlabs.tt.gui.Label[])total_labels.toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.gui.Label[a0]; }))), null));
            return score_box;
        }
        // Java: 239
        public com.oddlabs.tt.gui.MultiColumnComboBox buildPaginatedLayout(com.oddlabs.tt.player.Player[] worldPlayers)
        {
            this.paginatedPlayers = worldPlayers;
            this.paginatedFont = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font();
            this.currentPage = 0;
            paginatedStatNames = new string[] { com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("units_lost"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("units_killed"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("buildings_lost"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("buildings_wrecked"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("tree_resources"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("rock_resources"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("iron_resources"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("chicken_resources"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("meters_walked"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("weapons_thrown"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("magics_used"), com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("total") };
            paginatedPlayerStats = new int[paginatedPlayers.Length][];
            for (int i = 0; i < paginatedPlayers.Length; i++)
            {
                com.oddlabs.tt.player.Player player = paginatedPlayers[i];
                int total_score = player.getUnitsKilled() * 10 + player.getBuildingsDestroyed() * 100 + player.getTreeHarvested() + player.getRockHarvested() + player.getIronHarvested() * 2 + player.getRubberHarvested() * 4;
                paginatedPlayerStats[i] = new int[] { player.getUnitsLost(), player.getUnitsKilled(), player.getBuildingsLost(), player.getBuildingsDestroyed(), player.getTreeHarvested(), player.getRockHarvested(), player.getIronHarvested(), player.getRubberHarvested(), player.getUnitsMoved() * 2, player.getWeaponsThrown(), player.getMagics(), total_score };
            }
            paginatedScoreBox = buildPaginatedScoreBox(com.oddlabs.tt.@delegate.GameStatsDelegate.STAT_PAGES[0]);
            return paginatedScoreBox;
        }
        // Java: 262
        public com.oddlabs.tt.gui.MultiColumnComboBox buildPaginatedScoreBox(int[] statIndices)
        {
            com.oddlabs.tt.gui.ColumnInfo[] columns = new com.oddlabs.tt.gui.ColumnInfo[statIndices.Length + 1];
            columns[0] = new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("player_header"), com.oddlabs.tt.@delegate.GameStatsDelegate.PAGINATED_PLAYER_NAME_WIDTH);
            for (int j = 0; j < statIndices.Length; j++)
            {
                columns[j + 1] = new com.oddlabs.tt.gui.ColumnInfo(paginatedStatNames[statIndices[j]], com.oddlabs.tt.@delegate.GameStatsDelegate.PAGINATED_STAT_COLUMN_WIDTH);
            }
            com.oddlabs.tt.gui.MultiColumnComboBox box = new com.oddlabs.tt.gui.MultiColumnComboBox(viewer.getGUIRoot(), columns, 200);
            for (int i = 0; i < paginatedPlayers.Length; i++)
            {
                com.oddlabs.tt.gui.Label[] row = new com.oddlabs.tt.gui.Label[statIndices.Length + 1];
                row[0] = new com.oddlabs.tt.gui.SortedLabel(paginatedPlayers[i].getPlayerInfo().getDisplayName(), i, paginatedFont);
                for (int j_2 = 0; j_2 < statIndices.Length; j_2++)
                {
                    row[j_2 + 1] = new com.oddlabs.tt.gui.IntegerLabel(paginatedPlayerStats[i][statIndices[j_2]], paginatedFont, com.oddlabs.tt.@delegate.GameStatsDelegate.PAGINATED_STAT_COLUMN_WIDTH + com.oddlabs.tt.@delegate.GameStatsDelegate.TEXT_OFFSET);
                }
                box.addRow(new com.oddlabs.tt.gui.Row(row, null));
            }
            return box;
        }
        // Java: 282
        public void switchPage(int direction)
        {
            currentPage = (currentPage + direction + com.oddlabs.tt.@delegate.GameStatsDelegate.STAT_PAGES.Length) % com.oddlabs.tt.@delegate.GameStatsDelegate.STAT_PAGES.Length;
            int old_x = paginatedScoreBox.getX();
            int old_y = paginatedScoreBox.getY();
            removeChild(paginatedScoreBox);
            paginatedScoreBox = buildPaginatedScoreBox(com.oddlabs.tt.@delegate.GameStatsDelegate.STAT_PAGES[currentPage]);
            addChild(paginatedScoreBox);
            paginatedScoreBox.setPos(old_x, old_y);
        }
        // Java: 292
        public void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            addChild(group_buttons);
            delay_timer.stop();
        }
        // Java: 298
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderBackgroundAlpha(renderer);
        }
        // Java: 303
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
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
            }
            base.handleInput(@event);
        }
        // Java: 320
        public void startMenu()
        {
            viewer.close();
            setDisabled(true);
        }
        // Java: 325
        public com.oddlabs.tt.viewer.WorldViewer getViewer()
        {
            return viewer;
        }
        static GameStatsDelegate()
        {
            GameStatsDelegate.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.GameStatsDelegate).getName());
            GameStatsDelegate.STAT_PAGES = new int[][] { new int[] { 0, 1, 2, 3, 9, 10 }, new int[] { 4, 5, 6, 7, 8, 11 } };
        }
        private void init_GameStatsDelegate()
        {
            this.delay_timer = new com.oddlabs.tt.animation.TimerAnimation(this, 0.6f);
        }
        public GameStatsDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.Camera old_camera, string label_str) : base(viewer.getGUIRoot(), new com.oddlabs.tt.camera.StaticCamera(old_camera.getState()))
        {
            init_GameStatsDelegate();
            this.viewer = viewer;
            setDim(getGUIRoot().getWidth(), getGUIRoot().getHeight());
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(label_str, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label);
            label.setPos((getWidth() - label.getWidth()) / 2, (getHeight() - label.getHeight()) * 4 / 5);
            com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
            com.oddlabs.tt.gui.MultiColumnComboBox score_box = default;
            if (players.Length > com.oddlabs.tt.@delegate.GameStatsDelegate.PAGINATED_THRESHOLD)
            {
                score_box = buildPaginatedLayout(players);
            }
            else
            {
                score_box = buildLegacyLayout(players);
            }
            addChild(score_box);
            score_box.setPos((getWidth() - score_box.getWidth()) / 2, (getHeight() - score_box.getHeight()) / 2);
            group_buttons = new com.oddlabs.tt.gui.Group();
            viewer.addGameOverGUI(this, score_box.getY(), group_buttons);
            if (players.Length > com.oddlabs.tt.@delegate.GameStatsDelegate.PAGINATED_THRESHOLD)
            {
                int nav_y = score_box.getY() - com.oddlabs.tt.gui.Skin.getSkin().getEditFont().getHeight() - 4;
                com.oddlabs.tt.gui.HorizButton button_prev = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("prev_page"), 100);
                button_prev.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    switchPage((-1));
                }));
                addChild(button_prev);
                button_prev.setPos(score_box.getX(), nav_y);
                com.oddlabs.tt.gui.HorizButton button_next = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.@delegate.GameStatsDelegate.i18n("next_page"), 100);
                button_next.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
                {
                    switchPage(1);
                }));
                addChild(button_next);
                button_next.setPos(score_box.getX() + score_box.getWidth() - button_next.getWidth(), nav_y);
            }
            group_buttons.compileCanvas();
            addChild(group_buttons);
            group_buttons.setPos((getWidth() - group_buttons.getWidth()) / 2, (getHeight() - group_buttons.getHeight()) * 1 / 5);
            setFocusCycle(true);
            delay_timer.start();
        }
    }
}
