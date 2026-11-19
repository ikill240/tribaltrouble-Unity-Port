// Translated from SelectGameMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 62
    public class SelectGameMenu : com.oddlabs.tt.gui.Form, com.oddlabs.tt.net.MatchmakingListener, com.oddlabs.tt.form.TerrainMenuListener
    {
        public const int BUTTON_WIDTH_SHORT = 60;
        public const int BUTTON_WIDTH = 110;
        public const int BUTTON_WIDTH_LONG = 150;
        public const int BUTTON_WIDTH_EXTRA_LONG = 170;
        public const int PANEL_INDEX_GAME = 0;
        public const int PANEL_INDEX_CHAT = 1;
        public const int PANEL_INDEX_HIGHSCORE = 2;
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.form.ProfilesForm profiles_form;
        public com.oddlabs.tt.gui.Panel[] panels;
        public com.oddlabs.tt.gui.Panel game_list_panel;
        public com.oddlabs.tt.gui.MultiColumnComboBox game_list_box;
        public java.util.List game_hosts;
        public com.oddlabs.tt.gui.Panel chat_room_list_panel;
        public com.oddlabs.tt.gui.MultiColumnComboBox chat_room_list_box;
        public java.util.List chat_rooms;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gui.MultiColumnComboBox ranking_list_box;
        public com.oddlabs.tt.form.OpenSkillLeaderboard openskill_leaderboard;
        public int game_name_size;
        public int user_name_size;
        public int room_name_size;
        public static java.util.ResourceBundle bundle;
        // Java: 97
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.SelectGameMenu.bundle, key, args);
        }
        public com.oddlabs.tt.form.GameMenu game_panel;
        public com.oddlabs.tt.gui.ChatPanel chat_panel;
        public com.oddlabs.tt.gui.PanelGroup panel_group;
        public com.oddlabs.tt.gui.Panel highscore_list_panel;
        public com.oddlabs.tt.gui.Panel openskill_list_panel;
        // Java: 287
        public void setPanel(int index, com.oddlabs.tt.gui.Panel panel)
        {
            panels[index] = panel;
            com.oddlabs.tt.gui.PanelGroup temp_group = new com.oddlabs.tt.gui.PanelGroup(index, panels);
            temp_group.setPos(panel_group.getX(), panel_group.getY());
            panel_group.remove();
            panel_group = temp_group;
            addChild(panel_group);
            panel.setFocus();
        }
        // Java: 297
        public com.oddlabs.tt.gui.ChatPanel createChatRoomPanel(com.oddlabs.tt.gui.ChatRoomInfo info)
        {
            com.oddlabs.tt.gui.ChatPanel panel = new com.oddlabs.tt.gui.ChatPanel(gui_root, info, chat_room_list_panel.getWidth(), chat_room_list_panel.getHeight(), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH_SHORT, new com.oddlabs.tt.form.SelectGameMenu.SendChatListener(this), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                leaveChatRoom();
            }), new java.lang.Runnable_L(() => { this.leaveGameMenu(); }));
            com.oddlabs.tt.net.Network.getChatHub().addListener(panel);
            return panel;
        }
        // Java: 305
        public void createGameMenu(com.oddlabs.tt.net.GameNetwork game_network, com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, int player_slot, int player_count)
        {
            game_panel = new com.oddlabs.tt.form.GameMenu(game_network, gui_root, this, game, generator, player_slot, game_list_panel.getWidth(), game_list_panel.getHeight(), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH, player_count);
            setGameMenu(game_panel);
            game_network.getClient().setConfigurationListener(game_panel);
            com.oddlabs.matchmaking.RosterTemplate initial_roster = game_network.getInitialRoster();
            if (initial_roster != null)
            {
                game_panel.applyInitialRoster(initial_roster);
            }
        }
        // Java: 317
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                panel_group.setFocus(direction);
            }
        }
        // Java: 326
        public static void updateList(int type)
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().requestList(type);
        }
        // Java: 330
        public void connectionLost()
        {
            leaveChatRoom();
            remove();
            profiles_form.connectionLost();
            gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("connection_lost")));
        }
        // Java: 338
        public void loggedIn()
        {
        }
        // Java: 343
        public void loginError(int error_code)
        {
        }
        // Java: 348
        public void terrainMenuCancel()
        {
            setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_GAME, game_list_panel);
        }
        // Java: 353
        public void terrainMenuOK()
        {
        }
        // Java: 358
        public void setGameMenu(com.oddlabs.tt.gui.Panel panel)
        {
            com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_GAME);
            setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_GAME, panel);
        }
        // Java: 363
        public void removeGameMenu()
        {
            game_panel = null;
            setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_GAME, game_list_panel);
        }
        // Java: 368
        public void leaveGameMenu()
        {
            if (game_panel != null)
            {
                game_panel.cancel();
            }
        }
        // Java: 373
        public void joinedChat(com.oddlabs.tt.gui.ChatRoomInfo info)
        {
            if (chat_panel != null)
            {
                chat_panel.connectionLost();
                com.oddlabs.tt.net.Network.getChatHub().removeListener(chat_panel);
            }
            chat_panel = createChatRoomPanel(info);
            setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_CHAT, chat_panel);
        }
        // Java: 383
        public override void doRemove()
        {
            base.doRemove();
            com.oddlabs.tt.net.Network.getChatHub().removeListener(chat_panel);
        }
        // Java: 389
        public void receivedProfiles(com.oddlabs.matchmaking.Profile[] profiles, string last_nick)
        {
            profiles_form.receivedProfiles(profiles, last_nick);
        }
        // Java: 394
        public void profileUpdated()
        {
            com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_OPENSKILL_PERSONAL_RANKING);
        }
        // Java: 399
        public void updateChatRoom(com.oddlabs.tt.gui.ChatRoomInfo info)
        {
            chat_panel.update(info);
        }
        // Java: 404
        public void receivedList(int type, object[] names)
        {
            switch ((type))
            {
                case 1:
                {
                    game_hosts.addAll(java.util.Arrays.asList(((com.oddlabs.matchmaking.GameHost[])names)));
                    updateGameListGUI();
                    break;
                }
                case 2:
                {
                    chat_rooms.addAll(java.util.Arrays.asList(((com.oddlabs.matchmaking.ChatRoomEntry[])names)));
                    updateChatRoomListGUI();
                    break;
                }
                case 3:
                {
                    foreach (object name in names)
                    {
                        updateRankingList(((com.oddlabs.matchmaking.RankingEntry)name));
                    }
                    break;
                }
                case 4:
                {
                    foreach (object name_2 in names)
                    {
                        openskill_leaderboard.addRow(((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry)name_2));
                    }
                    break;
                }
                case 5:
                {
                    com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry entry = null;
                    if (names.Length > 0)
                    {
                        entry = ((com.oddlabs.matchmaking.OpenSkillLeaderboardRankingEntry)names[0]);
                    }
                    openskill_leaderboard.setPersonalEntry(entry);
                    break;
                }
                default:
                {
                    throw new System.ArgumentException("Unexpected list type " + (type));
                }
            }
        }
        // Java: 437
        public void clearList(int type)
        {
            switch ((type))
            {
                case 1:
                {
                    game_hosts.clear();
                    game_list_box.clear();
                    break;
                }
                case 2:
                {
                    chat_rooms.clear();
                    chat_room_list_box.clear();
                    break;
                }
                case 3:
                {
                    ranking_list_box.clear();
                    break;
                }
                case 4:
                {
                    openskill_leaderboard.clearRows();
                    break;
                }
                case 5:
                {
                    openskill_leaderboard.setPersonalEntry(null);
                    break;
                }
                default:
                {
                    throw new System.ArgumentException("Unexpected list type " + (type));
                }
            }
        }
        // Java: 462
        public void updateRankingList(com.oddlabs.matchmaking.RankingEntry ranking)
        {
            com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.IntegerLabel(ranking.getRanking(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()), new com.oddlabs.tt.gui.Label(ranking.getName(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), user_name_size), new com.oddlabs.tt.gui.IntegerLabel(ranking.getRating(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()), new com.oddlabs.tt.gui.IntegerLabel(ranking.getWins(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()), new com.oddlabs.tt.gui.IntegerLabel(ranking.getLosses(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()), new com.oddlabs.tt.gui.IntegerLabel(ranking.getInvalid(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font())), ranking);
            ranking_list_box.addRow(row);
        }
        // Java: 473
        public static bool joinable(com.oddlabs.matchmaking.GameHost game_host)
        {
            return game_host.getSimVersion() == com.oddlabs.util.Compatibility.SIM_VERSION;
        }
        // Java: 478
        public void updateGameListGUI()
        {
            com.oddlabs.tt.font.Font combofont = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font();
            java.util.List sorted = new java.util.ArrayList(game_hosts);
            sorted.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object game_host_l) =>
            {
                com.oddlabs.matchmaking.GameHost game_host = (com.oddlabs.matchmaking.GameHost)game_host_l;
                return (com.oddlabs.tt.form.SelectGameMenu.joinable(game_host) ? 0 : 1);
            })));
            foreach (com.oddlabs.matchmaking.GameHost game_host_2 in java.lang.JIter.of(sorted))
            {
                string rated = com.oddlabs.tt.util.ServerMessageBundler.getRatedString(game_host_2.getGame().isRated());
                string size = com.oddlabs.tt.util.ServerMessageBundler.getSizeString(game_host_2.getGame().getSize());
                org.joml.Vector4fc color = (com.oddlabs.tt.form.SelectGameMenu.joinable(game_host_2) ? com.oddlabs.tt.gui.Label.DEFAULT_COLOR : com.oddlabs.tt.gui.Label.DISABLED_COLOR);
                com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.Label(game_host_2.getGame().getName(), combofont, game_name_size).setColor(color), new com.oddlabs.tt.gui.Label(rated, combofont).setColor(color), new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(game_host_2.getGame().getGamespeed()), combofont).setColor(color), new com.oddlabs.tt.gui.Label(size, combofont).setColor(color)), game_host_2);
                game_list_box.addRow(row);
            }
        }
        // Java: 497
        public void updateChatRoomListGUI()
        {
            com.oddlabs.tt.font.Font combofont = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font();
            foreach (com.oddlabs.matchmaking.ChatRoomEntry chat_room_info in java.lang.JIter.of(chat_rooms))
            {
                string users_and_max = i18n("users_and_max", chat_room_info.getNumJoined(), com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_ROOM_USERS);
                com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.Label(chat_room_info.getName(), combofont, room_name_size), new com.oddlabs.tt.gui.Label(users_and_max, combofont)), chat_room_info);
                chat_room_list_box.addRow(row);
            }
        }
        // Java: 510
        public override void doCancel()
        {
            leaveChatRoom();
            leaveGameMenu();
            com.oddlabs.tt.net.Network.getMatchmakingClient().close();
        }
        // Java: 517
        public void leaveChatRoom()
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().leaveChatRoom();
            if (chat_panel != null)
            {
                chat_panel.connectionLost();
            }
            chat_panel = null;
            setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_CHAT, chat_room_list_panel);
        }
        // Java: 525
        public void joinGame(com.oddlabs.matchmaking.GameHost selected_game)
        {
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile() != null)
            {
                if (selected_game != null)
                {
                    bool rated = selected_game.getGame().isRated();
                    if (selected_game.getSimVersion() != com.oddlabs.util.Compatibility.SIM_VERSION)
                    {
                        string key = (selected_game.getSimVersion() > com.oddlabs.util.Compatibility.SIM_VERSION ? "game_version_newer" : "game_version_older");
                        gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n(key, java.lang.Integer.toString(selected_game.getSimVersion()), java.lang.Integer.toString(com.oddlabs.util.Compatibility.SIM_VERSION))));
                    }
                    else if (rated && com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile().getWins() < com.oddlabs.matchmaking.GameSession.MIN_WINS_FOR_RANKING)
                    {
                        string min_wins = i18n("min_wins", com.oddlabs.matchmaking.GameSession.MIN_WINS_FOR_RANKING);
                        gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(min_wins));
                    }
                    else
                    {
                        com.oddlabs.matchmaking.Game game = selected_game.getGame();
                        main_menu.joinGame(network, gui_root.getGUI(), selected_game.getHostID(), game, this);
                    }
                }
            }
        }
        // Java: 546
        public void joinRoom(com.oddlabs.matchmaking.ChatRoomEntry chat_room_info)
        {
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile() != null)
            {
                if (chat_room_info != null)
                {
                    com.oddlabs.tt.net.Network.getMatchmakingClient().joinRoom(gui_root, chat_room_info.getName());
                }
            }
        }
        // Java: 553
        public class RoomDoubleClickedListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.form.SelectGameMenu this_0;
            // Java: 554
            public void rowDoubleClicked(object chat_room_info_p)
            {
                com.oddlabs.matchmaking.ChatRoomEntry chat_room_info = (com.oddlabs.matchmaking.ChatRoomEntry)chat_room_info_p;
                this_0.joinRoom(chat_room_info);
            }
            public RoomDoubleClickedListener(com.oddlabs.tt.form.SelectGameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
            // mixin of default method RowListener.rowChosen
            public void rowChosen(object row_context)
            {
            }
        }
        // Java: 560
        public class GameListPanelListener : java.lang.Object, com.oddlabs.tt.guievent.FocusListener
        {
            // Java: 561
            public void activated(bool activated_v)
            {
                if (activated_v)
                {
                    com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_GAME);
                }
            }
            public GameListPanelListener() : base()
            {
            }
        }
        // Java: 568
        public class GameDoubleClickedListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.form.SelectGameMenu this_0;
            // Java: 569
            public void rowDoubleClicked(object selected_game_p)
            {
                com.oddlabs.matchmaking.GameHost selected_game = (com.oddlabs.matchmaking.GameHost)selected_game_p;
                this_0.joinGame(selected_game);
            }
            public GameDoubleClickedListener(com.oddlabs.tt.form.SelectGameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
            // mixin of default method RowListener.rowChosen
            public void rowChosen(object row_context)
            {
            }
        }
        // Java: 575
        public class UpdateScoresListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            // Java: 576
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_RANKING_LIST);
                com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_OPENSKILL_RANKING_LIST);
                com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_OPENSKILL_PERSONAL_RANKING);
            }
            public UpdateScoresListener() : base()
            {
            }
        }
        // Java: 584
        public class UpdateGameListListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            // Java: 585
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_GAME);
            }
            public UpdateGameListListener() : base()
            {
            }
        }
        // Java: 591
        public class CreateGameListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.SelectGameMenu this_0;
            // Java: 592
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                if (com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile() != null)
                {
                    com.oddlabs.tt.gui.Panel panel = new com.oddlabs.tt.gui.Panel(this_0.i18n("game"));
                    com.oddlabs.tt.gui.Group g = new com.oddlabs.tt.form.TerrainMenu(this_0.network, this_0.gui_root, this_0.main_menu, true, this.this_0);
                    panel.addChild(g);
                    g.place();
                    panel.compileCanvas();
                    this_0.setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_GAME, panel);
                }
            }
            public CreateGameListener(com.oddlabs.tt.form.SelectGameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 605
        public class JoinRoomListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.SelectGameMenu this_0;
            // Java: 606
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                com.oddlabs.matchmaking.ChatRoomEntry chat_room_info = ((com.oddlabs.matchmaking.ChatRoomEntry)this_0.chat_room_list_box.getSelected());
                this_0.joinRoom(chat_room_info);
            }
            public JoinRoomListener(com.oddlabs.tt.form.SelectGameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 613
        public class UpdateRoomListListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            // Java: 614
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_CHAT_ROOM_LIST);
            }
            public UpdateRoomListListener() : base()
            {
            }
        }
        // Java: 620
        public class CreateRoomListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.SelectGameMenu this_0;
            // Java: 621
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                if (com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile() != null)
                {
                    this_0.main_menu.setMenuCentered(new com.oddlabs.tt.form.CreateChatRoomForm(this_0.gui_root, this_0.main_menu, this.this_0));
                }
            }
            public CreateRoomListener(com.oddlabs.tt.form.SelectGameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 629
        public class SendChatListener : java.lang.Object, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.SelectGameMenu this_0;
            // Java: 630
            public void enterPressed(string text)
            {
                string chat = java.lang.JO.toString(text);
                if (!chat.isEmpty())
                {
                    if (!com.oddlabs.tt.net.ChatCommand.filterCommand(this_0.gui_root.getInfoPrinter(), chat))
                    {
                        com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().sendMessageToRoom(chat);
                    }
                }
            }
            public SendChatListener(com.oddlabs.tt.form.SelectGameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 641
        public class PulldownListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.SelectGameMenu this_0;
            public com.oddlabs.tt.gui.MultiColumnComboBox box;
            // Java: 648
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                com.oddlabs.matchmaking.GameHost host = ((com.oddlabs.matchmaking.GameHost)box.getRightClickedRowData());
                switch ((item_index))
                {
                    case 0:
                    {
                        this_0.joinGame(host);
                        break;
                    }
                    case 1:
                    {
                        this_0.gui_root.addModalForm(new com.oddlabs.tt.form.GameInfoForm(host.getGame(), host.getSimVersion()));
                        break;
                    }
                    default:
                    {
                        throw new System.ArgumentException("Unexpected action " + (item_index));
                    }
                }
                box.setFocus();
            }
            public PulldownListener(com.oddlabs.tt.form.SelectGameMenu this_0_, com.oddlabs.tt.gui.MultiColumnComboBox box) : base()
            {
                this.this_0 = this_0_;
                this.box = box;
            }
        }
        static SelectGameMenu()
        {
            SelectGameMenu.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.SelectGameMenu).getName());
        }
        private void init_SelectGameMenu()
        {
            this.panels = new com.oddlabs.tt.gui.Panel[3];
            this.game_hosts = new java.util.ArrayList();
            this.chat_rooms = new java.util.ArrayList();
        }
        public SelectGameMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu) : this(network, gui_root, main_menu, 0)
        {
        }
        public SelectGameMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu, int panel_index) : base()
        {
            init_SelectGameMenu();
            this.main_menu = main_menu;
            this.gui_root = gui_root;
            this.network = network;
            game_list_panel = new com.oddlabs.tt.gui.Panel(i18n("games_caption"));
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("multiplayer_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            game_list_panel.addChild(label_headline);
            game_list_panel.addFocusListener(new com.oddlabs.tt.form.SelectGameMenu.GameListPanelListener());
            game_name_size = 340;
            com.oddlabs.tt.gui.ColumnInfo[] infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(i18n("game_name"), game_name_size), new com.oddlabs.tt.gui.ColumnInfo(i18n("rated"), 120), new com.oddlabs.tt.gui.ColumnInfo(i18n("speed"), 120), new com.oddlabs.tt.gui.ColumnInfo(i18n("map_size"), 120) };
            game_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, infos, 350);
            game_list_box.addRowListener(new com.oddlabs.tt.form.SelectGameMenu.GameDoubleClickedListener(this));
            game_list_panel.addChild(game_list_box);
            com.oddlabs.tt.gui.PulldownMenu game_list_pulldown_menu = new com.oddlabs.tt.gui.PulldownMenu();
            game_list_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("join")));
            game_list_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("game_info")));
            game_list_pulldown_menu.addItemChosenListener(new com.oddlabs.tt.form.SelectGameMenu.PulldownListener(this, game_list_box));
            game_list_box.setPulldownMenu(game_list_pulldown_menu);
            com.oddlabs.tt.gui.HorizButton update_list_button = new com.oddlabs.tt.gui.HorizButton(i18n("update_list"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH_EXTRA_LONG);
            game_list_panel.addChild(update_list_button);
            update_list_button.addMouseClickListener(new com.oddlabs.tt.form.SelectGameMenu.UpdateGameListListener());
            com.oddlabs.tt.gui.HorizButton create_button = new com.oddlabs.tt.gui.HorizButton(i18n("create_game"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH_LONG);
            game_list_panel.addChild(create_button);
            create_button.addMouseClickListener(new com.oddlabs.tt.form.SelectGameMenu.CreateGameListener(this));
            com.oddlabs.tt.gui.HorizButton join_button = new com.oddlabs.tt.gui.HorizButton(i18n("join_game"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH);
            game_list_panel.addChild(join_button);
            join_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                com.oddlabs.matchmaking.GameHost selected_game = ((com.oddlabs.matchmaking.GameHost)game_list_box.getSelected());
                joinGame(selected_game);
            }));
            label_headline.place();
            game_list_box.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            update_list_button.place(game_list_box, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            create_button.place(update_list_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            join_button.place(create_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            game_list_panel.compileCanvas();
            panels[com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_GAME] = game_list_panel;
            highscore_list_panel = new com.oddlabs.tt.gui.Panel(i18n("league_caption"));
            label_headline = new com.oddlabs.tt.gui.Label(i18n("league_description"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            highscore_list_panel.addChild(label_headline);
            user_name_size = 250;
            com.oddlabs.tt.gui.ColumnInfo[] score_infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(i18n("rank"), 50), new com.oddlabs.tt.gui.ColumnInfo(i18n("name"), user_name_size), new com.oddlabs.tt.gui.ColumnInfo(i18n("rating"), 100), new com.oddlabs.tt.gui.ColumnInfo(i18n("wins"), 100), new com.oddlabs.tt.gui.ColumnInfo(i18n("losses"), 100), new com.oddlabs.tt.gui.ColumnInfo(i18n("invalid"), 100) };
            ranking_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, score_infos, 350);
            highscore_list_panel.addChild(ranking_list_box);
            com.oddlabs.tt.gui.HorizButton update_scores_button = new com.oddlabs.tt.gui.HorizButton(i18n("update_scores"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH_EXTRA_LONG);
            highscore_list_panel.addChild(update_scores_button);
            update_scores_button.addMouseClickListener(new com.oddlabs.tt.form.SelectGameMenu.UpdateScoresListener());
            label_headline.place();
            ranking_list_box.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            update_scores_button.place(ranking_list_box, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            highscore_list_panel.compileCanvas();
            panels[com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_HIGHSCORE] = highscore_list_panel;
            openskill_list_panel = new com.oddlabs.tt.gui.Panel(i18n("openskill_league_caption"));
            com.oddlabs.tt.gui.Label openskill_headline = new com.oddlabs.tt.gui.Label(i18n("openskill_league_description"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            openskill_list_panel.addChild(openskill_headline);
            openskill_leaderboard = new com.oddlabs.tt.form.OpenSkillLeaderboard(gui_root, new java.util.function.Function_L((object a0) => { return this.i18n(((string)a0)); }));
            openskill_list_panel.addChild(openskill_leaderboard);
            com.oddlabs.tt.gui.HorizButton update_openskill_scores_button = new com.oddlabs.tt.gui.HorizButton(i18n("update_scores"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH_EXTRA_LONG);
            openskill_list_panel.addChild(update_openskill_scores_button);
            update_openskill_scores_button.addMouseClickListener(new com.oddlabs.tt.form.SelectGameMenu.UpdateScoresListener());
            openskill_headline.place();
            openskill_leaderboard.place(openskill_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            update_openskill_scores_button.place(openskill_leaderboard, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            openskill_list_panel.compileCanvas();
            highscore_list_panel.addFocusListener(new com.oddlabs.tt.guievent.FocusListener_L((bool activated) =>
            {
                bool tabHovered = highscore_list_panel.getTab().isHovered();
                bool shiftDown = com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently();
                if (activated && tabHovered && shiftDown)
                {
                    setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_HIGHSCORE, openskill_list_panel);
                }
            }));
            openskill_list_panel.addFocusListener(new com.oddlabs.tt.guievent.FocusListener_L((bool activated_2) =>
            {
                bool tabHovered_2 = openskill_list_panel.getTab().isHovered();
                bool shiftDown_2 = com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently();
                if (activated_2 && tabHovered_2 && !shiftDown_2)
                {
                    setPanel(com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_HIGHSCORE, highscore_list_panel);
                }
            }));
            chat_room_list_panel = new com.oddlabs.tt.gui.Panel(i18n("chat_caption"));
            label_headline = new com.oddlabs.tt.gui.Label(i18n("chat_rooms_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            chat_room_list_panel.addChild(label_headline);
            room_name_size = 600;
            infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(i18n("room"), room_name_size), new com.oddlabs.tt.gui.ColumnInfo(i18n("users"), 100) };
            chat_room_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, infos, 350);
            chat_room_list_box.addRowListener(new com.oddlabs.tt.form.SelectGameMenu.RoomDoubleClickedListener(this));
            chat_room_list_panel.addChild(chat_room_list_box);
            update_list_button = new com.oddlabs.tt.gui.HorizButton(i18n("update_rooms"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH_EXTRA_LONG);
            chat_room_list_panel.addChild(update_list_button);
            update_list_button.addMouseClickListener(new com.oddlabs.tt.form.SelectGameMenu.UpdateRoomListListener());
            create_button = new com.oddlabs.tt.gui.HorizButton(i18n("create_room"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH_LONG);
            chat_room_list_panel.addChild(create_button);
            create_button.addMouseClickListener(new com.oddlabs.tt.form.SelectGameMenu.CreateRoomListener(this));
            join_button = new com.oddlabs.tt.gui.HorizButton(i18n("join_room"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH);
            chat_room_list_panel.addChild(join_button);
            join_button.addMouseClickListener(new com.oddlabs.tt.form.SelectGameMenu.JoinRoomListener(this));
            label_headline.place();
            chat_room_list_box.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            update_list_button.place(chat_room_list_box, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            create_button.place(update_list_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            join_button.place(create_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            chat_room_list_panel.compileCanvas();
            com.oddlabs.tt.gui.ChatRoomInfo info = com.oddlabs.tt.net.Network.getMatchmakingClient().getChatRoomInfo();
            if (info != null)
            {
                chat_panel = createChatRoomPanel(info);
                panels[com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_CHAT] = chat_panel;
            }
            else
            {
                panels[com.oddlabs.tt.form.SelectGameMenu.PANEL_INDEX_CHAT] = chat_room_list_panel;
            }
            panel_group = new com.oddlabs.tt.gui.PanelGroup(panel_index, panels);
            addChild(panel_group);
            com.oddlabs.tt.gui.HorizButton logout_button = new com.oddlabs.tt.gui.HorizButton(i18n("logout"), com.oddlabs.tt.form.SelectGameMenu.BUTTON_WIDTH);
            addChild(logout_button);
            logout_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                this.cancel();
            }));
            panel_group.place();
            logout_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            com.oddlabs.tt.net.Network.setMatchmakingListener(this);
            com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_GAME);
            com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_CHAT_ROOM_LIST);
            com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_RANKING_LIST);
            com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_OPENSKILL_RANKING_LIST);
            com.oddlabs.tt.form.SelectGameMenu.updateList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_OPENSKILL_PERSONAL_RANKING);
            profiles_form = new com.oddlabs.tt.form.ProfilesForm(gui_root, main_menu, this);
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile() == null)
            {
                main_menu.setMenuCentered(profiles_form);
                com.oddlabs.tt.net.Network.getMatchmakingClient().requestProfiles();
            }
            else
            {
                main_menu.setMenuCentered(this);
            }
        }
    }
}
