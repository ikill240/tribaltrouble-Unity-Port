// Translated from GameMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 62
    public class GameMenu : com.oddlabs.tt.gui.Panel, com.oddlabs.tt.net.ConfigurationListener, com.oddlabs.tt.net.ChatListener
    {
        public static java.util.ResourceBundle bundle;
        // Java: 65
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.GameMenu.bundle, key, args);
        }
        public const int OPEN_INDEX = 0;
        public const int CLOSED_INDEX = 1;
        public const int COMPUTER_EASY_INDEX = 2;
        public const int COMPUTER_NORMAL_INDEX = 3;
        public const int COMPUTER_HARD_INDEX = 4;
        public const int COMPUTER_EXPERT_INDEX = 5;
        public const int COMPUTER_GAUNTLET_INDEX = 6;
        public const int SEND_BUTTON_WIDTH = 60;
        public const int RATING_WIDTH = 80;
        public const int DEFAULT_PLAYER_COUNT = 6;
        public com.oddlabs.tt.gui.PulldownButton[] slot_buttons;
        public com.oddlabs.tt.gui.PulldownButton[] race_buttons;
        public com.oddlabs.tt.gui.PulldownButton[] team_buttons;
        public com.oddlabs.tt.gui.Label[] ratings;
        public com.oddlabs.tt.gui.Label chat_info;
        public com.oddlabs.tt.gui.TextBox chat_box;
        public com.oddlabs.tt.gui.EditLine chat_line;
        public com.oddlabs.tt.gui.EditLine chief_line;
        public com.oddlabs.tt.gui.EditLine tribe_line;
        public com.oddlabs.tt.gui.Diode[] ready_marks;
        public com.oddlabs.tt.gui.HorizButton ready_button;
        public com.oddlabs.tt.gui.HorizButton start_button;
        public com.oddlabs.tt.form.SelectGameMenu owner;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public int local_player_slot;
        public bool rated;
        public com.oddlabs.matchmaking.Game game;
        public com.oddlabs.tt.net.GameNetwork game_network;
        public java.util.SortedSet human_names;
        public bool updating;
        public bool ready;
        // Java: 232
        public void applyInitialRoster(com.oddlabs.matchmaking.RosterTemplate roster)
        {
            if (local_player_slot != 0 || game_network.getClient() == null)
            {
                return;
            }
            com.oddlabs.tt.net.GameServerInterface server = game_network.getClient().getServerInterface();
            com.oddlabs.matchmaking.RosterTemplate.Slot[] slots = roster.getSlots();
            for (int i = 0; i < slot_buttons.Length && i < slots.Length; i++)
            {
                if (i == local_player_slot)
                {
                    continue;
                }
                com.oddlabs.matchmaking.RosterTemplate.Slot slot = slots[i];
                int race = (slot.getRace() != null ? ((int)slot.getRace()) : 0);
                int team = (slot.getTeam() != null ? ((int)slot.getTeam()) : i);
                switch ((slot.getFill()).ordinal())
                {
                    case 1 /*OPEN*/:
                    case 0 /*HOST*/:
                    {
                        server.resetSlotState(i, true);
                        break;
                    }
                    case 2 /*CLOSED*/:
                    {
                        server.resetSlotState(i, false);
                        break;
                    }
                    case 3 /*EASY_AI*/:
                    {
                        applyAi(server, i, race, team, com.oddlabs.tt.net.PlayerSlot.AI_EASY);
                        break;
                    }
                    case 4 /*NORMAL_AI*/:
                    {
                        applyAi(server, i, race, team, com.oddlabs.tt.net.PlayerSlot.AI_NORMAL);
                        break;
                    }
                    case 5 /*HARD_AI*/:
                    {
                        applyAi(server, i, race, team, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
                        break;
                    }
                    case 6 /*EXPERT_AI*/:
                    {
                        applyAi(server, i, race, team, com.oddlabs.tt.net.PlayerSlot.AI_EXPERT);
                        break;
                    }
                    case 7 /*ULTRA_AI*/:
                    case 8 /*FABLE_AI*/:
                    case 9 /*GAUNTLET_AI*/:
                    {
                        applyAi(server, i, race, team, com.oddlabs.tt.net.PlayerSlot.AI_GAUNTLET);
                        break;
                    }
                }
            }
        }
        // Java: 257
        public void applyAi(com.oddlabs.tt.net.GameServerInterface server, int slot, int race, int team, int difficulty)
        {
            if (rated)
            {
                return;
            }
            server.setPlayerSlot(slot, com.oddlabs.tt.net.PlayerSlot.AI, race, team, true, difficulty);
        }
        // Java: 264
        public void adjustPlayerSlot(int player_slot)
        {
            if (updating || game_network.getClient() == null)
            {
                return;
            }
            com.oddlabs.tt.net.PlayerSlot player = game_network.getClient().getPlayers()[player_slot];
            int index = slot_buttons[player_slot].getMenu().getChosenItemIndex();
            int race_index = race_buttons[player_slot].getMenu().getChosenItemIndex();
            int team_index = team_buttons[player_slot].getMenu().getChosenItemIndex();
            int difficulty_index = (index >= com.oddlabs.tt.form.GameMenu.COMPUTER_EASY_INDEX ? com.oddlabs.tt.net.PlayerSlot.difficultyOfMenuEntry(index - com.oddlabs.tt.form.GameMenu.COMPUTER_EASY_INDEX) : com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            bool race_changed = player.getInfo() == null || race_index != player.getInfo().getRaceChoice();
            bool team_changed = player.getInfo() == null || team_index != player.getInfo().getTeam();
            bool ready_changed = ready != player.isReady();
            bool difficulty_changed = player.getInfo() == null || player.getAIDifficulty() != difficulty_index;
            com.oddlabs.tt.gui.PulldownButton slot_button = slot_buttons[player_slot];
            switch ((index))
            {
                case 0:
                {
                    if ((player.getType() != com.oddlabs.tt.net.PlayerSlot.OPEN && player.getType() != com.oddlabs.tt.net.PlayerSlot.HUMAN) || race_changed || team_changed || ready_changed)
                    {
                        if (player_slot == local_player_slot)
                        {
                            int new_type = com.oddlabs.tt.net.PlayerSlot.HUMAN;
                            game_network.getClient().getServerInterface().setPlayerSlot(player_slot, new_type, race_index, team_index, ready, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
                        }
                        else
                        {
                            game_network.getClient().getServerInterface().resetSlotState(player_slot, true);
                        }
                    }
                    break;
                }
                case 1:
                {
                    if (player.getType() != com.oddlabs.tt.net.PlayerSlot.CLOSED || race_changed || team_changed)
                    {
                        slot_button.getMenu().getItem(com.oddlabs.tt.form.GameMenu.OPEN_INDEX).setLabelString(com.oddlabs.tt.form.GameMenu.i18n("open"));
                        game_network.getClient().getServerInterface().resetSlotState(player_slot, false);
                    }
                    break;
                }
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                {
                    bool new_ai = player.getType() != com.oddlabs.tt.net.PlayerSlot.AI;
                    if (new_ai || race_changed || team_changed || difficulty_changed)
                    {
                        slot_button.getMenu().getItem(com.oddlabs.tt.form.GameMenu.OPEN_INDEX).setLabelString(com.oddlabs.tt.form.GameMenu.i18n("open"));
                        if (new_ai)
                        {
                            team_index = player_slot;
                            race_index = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick()).nextInt(com.oddlabs.tt.model.RacesResources.getNumRaces());
                        }
                        game_network.getClient().getServerInterface().setPlayerSlot(player_slot, com.oddlabs.tt.net.PlayerSlot.AI, race_index, team_index, true, difficulty_index);
                    }
                    break;
                }
                default:
                {
                    throw new System.Exception("Invalid item index");
                }
            }
        }
        // Java: 320
        public void connected(com.oddlabs.tt.net.Client client, com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, int player_slot, int player_count)
        {
        }
        // Java: 325
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                chat_line.setFocus(direction);
            }
        }
        // Java: 334
        public int countHumans(com.oddlabs.tt.net.PlayerSlot[] players)
        {
            int result = 0;
            foreach (com.oddlabs.tt.net.PlayerSlot player in players)
            {
                if (player.getType() == com.oddlabs.tt.net.PlayerSlot.HUMAN)
                {
                    result++;
                }
            }
            return result;
        }
        // Java: 344
        public void setPlayers(com.oddlabs.tt.net.PlayerSlot[] players)
        {
            int num_humans = countHumans(players);
            int[] player_slots = new int[num_humans];
            int[] player_ratings = new int[num_humans];
            int[] player_teams = new int[num_humans];
            int human_index = 0;
            updating = true;
            java.util.SortedSet new_human_names = new java.util.TreeSet();
            for (int i = 0; i < players.Length; i++)
            {
                com.oddlabs.tt.net.PlayerSlot player = players[i];
                com.oddlabs.tt.gui.PulldownButton slot_button = slot_buttons[i];
                com.oddlabs.tt.gui.PulldownButton race_button = race_buttons[i];
                com.oddlabs.tt.gui.PulldownButton team_button = team_buttons[i];
                com.oddlabs.tt.gui.Diode ready_mark = ready_marks[i];
                ready_mark.setLit(player.isReady());
                race_button.setSelected((player.getInfo() != null ? player.getInfo().getRaceChoice() : 0));
                team_button.setSelected((player.getInfo() != null ? player.getInfo().getTeam() : 0));
                if (player.getType() != com.oddlabs.tt.net.PlayerSlot.CLOSED)
                {
                    slot_button.getMenu().getItem(com.oddlabs.tt.form.GameMenu.OPEN_INDEX).setLabelString(com.oddlabs.tt.form.GameMenu.i18n("open"));
                    slot_button.setSelected(com.oddlabs.tt.form.GameMenu.OPEN_INDEX);
                }
                else
                {
                    slot_button.setSelected(com.oddlabs.tt.form.GameMenu.CLOSED_INDEX);
                }
                race_button.setDisabled(true);
                team_button.setDisabled(true);
                if (player.getInfo() != null)
                {
                    com.oddlabs.tt.player.PlayerInfo player_info = player.getInfo();
                    switch ((player.getType()))
                    {
                        case 4:
                        {
                            slot_button.setSelected(com.oddlabs.tt.form.GameMenu.COMPUTER_EASY_INDEX + com.oddlabs.tt.net.PlayerSlot.menuEntryOfDifficulty(player.getAIDifficulty()));
                            race_button.setDisabled(!canControlSlot(i));
                            team_button.setDisabled(!canControlSlot(i));
                            break;
                        }
                        case 3:
                        {
                            string player_name = player_info.getName();
                            new_human_names.add(player_name);
                            slot_button.getMenu().getItem(com.oddlabs.tt.form.GameMenu.OPEN_INDEX).setLabelString(player_name);
                            slot_button.setSelected(com.oddlabs.tt.form.GameMenu.OPEN_INDEX);
                            race_button.setDisabled(i != local_player_slot);
                            team_button.setDisabled(i != local_player_slot);
                            player_slots[human_index] = i;
                            player_ratings[human_index] = player.getRating();
                            player_teams[human_index] = player_info.getTeam();
                            human_index++;
                            break;
                        }
                        default:
                        {
                            throw new System.Exception("Unknown Player type: " + (player.getType()));
                        }
                    }
                }
            }
            if (rated)
            {
                updateRatedLabels(player_slots, player_ratings, com.oddlabs.matchmaking.GameSession.calculateMatchPoints(player_ratings, player_teams));
            }
            java.util.Iterator it = new_human_names.iterator();
            while (it.hasNext())
            {
                string name = ((string)it.next());
                if (human_names.contains(name))
                {
                    human_names.remove(name);
                }
                else
                {
                    playerJoined(name);
                }
            }
            it = human_names.iterator();
            while (it.hasNext())
            {
                string name_2 = ((string)it.next());
                playerLeft(name_2);
            }
            human_names = new_human_names;
            setReady(players[local_player_slot].isReady());
            setStartEnable(players);
            updating = false;
            com.oddlabs.tt.islandwars.IslandWarsNet.lobby(this, game_network, players, local_player_slot);
        }
        // Java: 421
        public void readyUp()
        {
            setReady(true);
        }
        // Java: 425
        public com.oddlabs.matchmaking.Game getGame()
        {
            return game;
        }
        // Java: 429
        public void updateRatedLabels(int[] player_slots, int[] player_ratings, int[][] points)
        {
            foreach (com.oddlabs.tt.gui.Label rating in ratings)
            {
                rating.clear();
            }
            for (int i = 0; i < player_slots.Length; i++)
            {
                int slot = player_slots[i];
                if (slot == local_player_slot)
                {
                    int win = points[i][com.oddlabs.matchmaking.GameSession.WIN];
                    int lose = points[i][com.oddlabs.matchmaking.GameSession.LOSE];
                    string rating_change_message = com.oddlabs.tt.form.GameMenu.i18n("rating_change_message", java.lang.Integer.toString(win), java.lang.Integer.toString(-lose));
                    chat_info.set(rating_change_message);
                }
                ratings[slot].set("" + (player_ratings[i]));
            }
        }
        // Java: 447
        public bool canControlSlot(int slot)
        {
            return local_player_slot == 0 || slot == local_player_slot;
        }
        // Java: 451
        public com.oddlabs.tt.gui.GUIObject createPlayerPulldown(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.Group group, com.oddlabs.tt.gui.GUIObject previous, com.oddlabs.tt.gui.PulldownButton[] slot_buttons, com.oddlabs.tt.gui.PulldownButton[] race_buttons, com.oddlabs.tt.gui.PulldownButton[] team_buttons, com.oddlabs.tt.gui.Diode[] ready_marks, com.oddlabs.tt.gui.Label[] ratings, int index, int num_players)
        {
            com.oddlabs.tt.gui.PulldownMenu pulldown_menu = new com.oddlabs.tt.gui.PulldownMenu();
            com.oddlabs.tt.gui.PulldownItem open_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.GameMenu.i18n("open"));
            com.oddlabs.tt.gui.PulldownItem closed_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.GameMenu.i18n("closed"));
            com.oddlabs.tt.gui.PulldownItem computer_easy_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.GameMenu.i18n("easy_ai"));
            com.oddlabs.tt.gui.PulldownItem computer_normal_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.GameMenu.i18n("normal_ai"));
            com.oddlabs.tt.gui.PulldownItem computer_hard_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.GameMenu.i18n("hard_ai"));
            com.oddlabs.tt.gui.PulldownItem computer_expert_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.GameMenu.i18n("expert_ai"));
            com.oddlabs.tt.gui.PulldownItem computer_gauntlet_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.GameMenu.i18n("gauntlet_ai"));
            pulldown_menu.addItem(open_item);
            pulldown_menu.addItem(closed_item);
            if (!rated)
            {
                pulldown_menu.addItem(computer_easy_item);
                pulldown_menu.addItem(computer_normal_item);
                pulldown_menu.addItem(computer_hard_item);
                pulldown_menu.addItem(computer_expert_item);
                pulldown_menu.addItem(computer_gauntlet_item);
            }
            com.oddlabs.tt.gui.PulldownButton pulldown_button = new com.oddlabs.tt.gui.PulldownButton(gui_root, pulldown_menu, com.oddlabs.tt.form.GameMenu.CLOSED_INDEX, 150);
            slot_buttons[index] = pulldown_button;
            group.addChild(pulldown_button);
            if (previous != null)
            {
                pulldown_button.place(previous, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            else
            {
                pulldown_button.place();
            }
            pulldown_menu.addItemChosenListener(new com.oddlabs.tt.form.GameMenu.PlayerSlotListener(this, index));
            pulldown_button.setDisabled(local_player_slot != 0 || index == local_player_slot);
            com.oddlabs.tt.gui.PulldownMenu race_pulldown_menu = new com.oddlabs.tt.gui.PulldownMenu();
            for (int i = 0; i < com.oddlabs.tt.model.RacesResources.getNumRaceChoices(); i++)
            {
                com.oddlabs.tt.gui.PulldownItem race_item = new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.model.RacesResources.getRaceChoiceName(i));
                race_pulldown_menu.addItem(race_item);
            }
            com.oddlabs.tt.gui.PulldownMenu team_pulldown_menu = new com.oddlabs.tt.gui.PulldownMenu();
            int num_teams = num_players;
            if (rated)
            {
                num_teams = 2;
            }
            for (int i_2 = 0; i_2 < num_teams; i_2++)
            {
                string team_str = com.oddlabs.tt.form.GameMenu.i18n("team", java.lang.Integer.toString(i_2 + 1));
                com.oddlabs.tt.gui.PulldownItem race_item_2 = new com.oddlabs.tt.gui.PulldownItem(team_str);
                team_pulldown_menu.addItem(race_item_2);
            }
            com.oddlabs.tt.gui.PulldownButton race_pulldown_button = new com.oddlabs.tt.gui.PulldownButton(gui_root, race_pulldown_menu, 0, 115);
            com.oddlabs.tt.gui.PulldownButton team_pulldown_button = new com.oddlabs.tt.gui.PulldownButton(gui_root, team_pulldown_menu, index % num_teams, 115);
            race_buttons[index] = race_pulldown_button;
            team_buttons[index] = team_pulldown_button;
            group.addChild(race_pulldown_button);
            group.addChild(team_pulldown_button);
            race_pulldown_button.place(pulldown_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            team_pulldown_button.place(race_pulldown_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            race_pulldown_menu.addItemChosenListener(new com.oddlabs.tt.form.GameMenu.PlayerSlotListener(this, index));
            team_pulldown_menu.addItemChosenListener(new com.oddlabs.tt.form.GameMenu.PlayerSlotListener(this, index));
            race_pulldown_button.setDisabled(!canControlSlot(index));
            team_pulldown_button.setDisabled(!canControlSlot(index));
            com.oddlabs.tt.gui.Diode ready_mark = new com.oddlabs.tt.gui.Diode();
            ready_marks[index] = ready_mark;
            group.addChild(ready_mark);
            ready_mark.place(team_pulldown_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getEditFont();
            ratings[index] = new com.oddlabs.tt.gui.Label("", font, com.oddlabs.tt.form.GameMenu.RATING_WIDTH, com.oddlabs.tt.gui.Origin.AT_END);
            if (rated)
            {
                group.addChild(ratings[index]);
                ratings[index].place(ready_mark, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            }
            string player_str = com.oddlabs.tt.form.GameMenu.i18n("player", java.lang.Integer.toString(index + 1));
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(player_str, com.oddlabs.tt.gui.Skin.getSkin().getEditFont()).setColor(com.oddlabs.tt.global.Settings.getSettings().team_colours[index]);
            group.addChild(label);
            label.place(pulldown_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            return pulldown_button;
        }
        // Java: 535
        public override void doAdd()
        {
            base.doAdd();
            com.oddlabs.tt.net.Network.getChatHub().addListener(this);
        }
        // Java: 541
        public override void doRemove()
        {
            base.doRemove();
            com.oddlabs.tt.net.Network.getChatHub().removeListener(this);
        }
        // Java: 547
        public void connectionLost()
        {
            remove();
            owner.removeGameMenu();
            gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.form.GameMenu.i18n("connection_lost")));
        }
        // Java: 554
        public void gameStarted()
        {
            setDisabled(true);
        }
        // Java: 560
        public void finishChatAppend()
        {
            chat_box.setOffsetY(java.lang.Integer.MAX_VALUE);
            getTab().updateNotify();
        }
        // Java: 565
        public void chat(com.oddlabs.tt.net.ChatMessage message)
        {
            if (((object)message.type() != (object)com.oddlabs.tt.net.ChatMessage.Type.GAME_MENU))
            {
                return;
            }
            if (!chat_box.isEmpty())
            {
                chat_box.append("\n");
            }
            chat_box.append(message.formatLong());
            finishChatAppend();
        }
        // Java: 576
        public void playerLeft(string name)
        {
            if (!chat_box.isEmpty())
            {
                chat_box.append("\n");
            }
            chat_box.append(com.oddlabs.tt.form.GameMenu.i18n("left_game", name));
            finishChatAppend();
        }
        // Java: 583
        public void playerJoined(string name)
        {
            if (!chat_box.isEmpty())
            {
                chat_box.append("\n");
            }
            chat_box.append(com.oddlabs.tt.form.GameMenu.i18n("joined_game", name));
            finishChatAppend();
        }
        // Java: 590
        public void setReady(bool r)
        {
            if (r != ready)
            {
                ready = r;
                ready_button.setDisabled(ready);
                adjustPlayerSlot(local_player_slot);
            }
        }
        // Java: 598
        public void setStartEnable(com.oddlabs.tt.net.PlayerSlot[] players)
        {
            start_button.setDisabled(true);
            if (local_player_slot != 0)
            {
                return;
            }
            foreach (com.oddlabs.tt.net.PlayerSlot player in players)
            {
                if (!player.isReady())
                {
                    return;
                }
            }
            start_button.setDisabled(false);
        }
        // Java: 610
        public void cancel()
        {
            game_network.close();
            owner.removeGameMenu();
        }
        // Java: 615
        public class InfoButtonListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.GameMenu this_0;
            // Java: 616
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.gui_root.addModalForm(new com.oddlabs.tt.form.GameInfoForm(this_0.game));
            }
            public InfoButtonListener(com.oddlabs.tt.form.GameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 622
        public class CancelButtonListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.GameMenu this_0;
            // Java: 623
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.cancel();
            }
            public CancelButtonListener(com.oddlabs.tt.form.GameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 630
        public void sendNames()
        {
            if (chief_line == null || tribe_line == null)
            {
                return;
            }
            com.oddlabs.tt.global.Settings settings = com.oddlabs.tt.global.Settings.getSettings();
            settings.chief_name = com.oddlabs.tt.player.PlayerInfo.clean(chief_line.getContents());
            settings.tribe_name = com.oddlabs.tt.player.PlayerInfo.clean(tribe_line.getContents());
            settings.save();
            game_network.getClient().getServerInterface().setPlayerNames(settings.chief_name, settings.tribe_name);
        }
        // Java: 640
        public class ReadyListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.GameMenu this_0;
            // Java: 641
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.setReady(true);
            }
            public ReadyListener(com.oddlabs.tt.form.GameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 647
        public static int getNumTeams(com.oddlabs.tt.net.PlayerSlot[] players)
        {
            java.util.Set teams = new java.util.HashSet();
            foreach (com.oddlabs.tt.net.PlayerSlot current in players)
            {
                if (current.getInfo() != null)
                {
                    teams.add(current.getInfo().getTeam());
                }
            }
            return teams.size();
        }
        // Java: 656
        public class StartListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.GameMenu this_0;
            // Java: 657
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                int MIN_TEAMS = 2;
                int num_teams = com.oddlabs.tt.form.GameMenu.getNumTeams(this_0.game_network.getClient().getPlayers());
                if (num_teams < MIN_TEAMS)
                {
                    string err_msg = com.oddlabs.tt.form.GameMenu.i18n("min_teams", java.lang.Integer.toString(MIN_TEAMS));
                    this_0.gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(err_msg));
                }
                else
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.beforeStart(this_0.game_network);
                    this_0.game_network.getClient().getServerInterface().startServer();
                }
            }
            public StartListener(com.oddlabs.tt.form.GameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 671
        public class PlayerSlotListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.GameMenu this_0;
            public int player_slot;
            // Java: 678
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                this_0.setReady(false);
                this_0.adjustPlayerSlot(player_slot);
            }
            public PlayerSlotListener(com.oddlabs.tt.form.GameMenu this_0_, int player_slot) : base()
            {
                this.this_0 = this_0_;
                this.player_slot = player_slot;
            }
        }
        // Java: 685
        public class ChatListener : java.lang.Object, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.GameMenu this_0;
            // Java: 686
            public void enterPressed(string text)
            {
                string chat_v = java.lang.JO.toString(text);
                if (!chat_v.isEmpty())
                {
                    this_0.chat_line.clear();
                    if (!com.oddlabs.tt.net.ChatCommand.filterCommand(this_0.gui_root.getInfoPrinter(), chat_v))
                    {
                        this_0.game_network.getClient().getServerInterface().chat(chat_v);
                    }
                }
            }
            public ChatListener(com.oddlabs.tt.form.GameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 697
        public class SendListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.GameMenu this_0;
            // Java: 698
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.chat_line.enterPressedAll();
            }
            public SendListener(com.oddlabs.tt.form.GameMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static GameMenu()
        {
            GameMenu.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.GameMenu).getName());
        }
        private void init_GameMenu()
        {
            this.human_names = new java.util.TreeSet();
        }
        public GameMenu(com.oddlabs.tt.net.GameNetwork game_network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu owner, com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, int player_slot, int compare_width, int compare_height, int button_width, int player_count) : base(com.oddlabs.tt.form.GameMenu.i18n("game_caption"))
        {
            init_GameMenu();
            this.game_network = game_network;
            this.owner = owner;
            this.gui_root = gui_root;
            this.local_player_slot = player_slot;
            this.rated = game.isRated();
            this.game = game;
            string tag = (rated ? java.lang.JStr.valueOf(com.oddlabs.tt.form.GameMenu.i18n("rated")) + " " : "");
            com.oddlabs.tt.gui.Label game_name_label = new com.oddlabs.tt.gui.Label(java.lang.JStr.valueOf(com.oddlabs.tt.form.GameMenu.i18n("game")) + " " + java.lang.JStr.valueOf(tag) + java.lang.JStr.valueOf(game.getName()), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            slot_buttons = ((com.oddlabs.tt.gui.PulldownButton[])new com.oddlabs.tt.gui.PulldownButton[player_count]);
            race_buttons = ((com.oddlabs.tt.gui.PulldownButton[])new com.oddlabs.tt.gui.PulldownButton[player_count]);
            team_buttons = ((com.oddlabs.tt.gui.PulldownButton[])new com.oddlabs.tt.gui.PulldownButton[player_count]);
            ready_marks = new com.oddlabs.tt.gui.Diode[player_count];
            ratings = new com.oddlabs.tt.gui.Label[player_count];
            com.oddlabs.tt.gui.Group player_group = (player_count > com.oddlabs.tt.form.GameMenu.DEFAULT_PLAYER_COUNT ? (com.oddlabs.tt.gui.Group)(new com.oddlabs.tt.gui.ScrollableGroup(170, 64)) : new com.oddlabs.tt.gui.Group());
            com.oddlabs.tt.gui.GUIObject previous = null;
            for (int i = 0; i < player_count; i++)
            {
                previous = createPlayerPulldown(gui_root, player_group, previous, slot_buttons, race_buttons, team_buttons, ready_marks, ratings, i, player_count);
            }
            player_group.compileCanvas();
            addChild(player_group);
            com.oddlabs.tt.gui.Box pdata = com.oddlabs.tt.gui.Skin.getSkin().getPanelData().box();
            com.oddlabs.tt.gui.FormData fdata = com.oddlabs.tt.gui.Skin.getSkin().getFormData();
            int width = compare_width - pdata.getLeftOffset() - pdata.getRightOffset();
            com.oddlabs.tt.gui.Group names_group = new com.oddlabs.tt.gui.Group();
            if (com.oddlabs.tt.global.Edition.new_tides)
            {
                com.oddlabs.tt.gui.Label chief_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.GameMenu.i18n("chief_name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                com.oddlabs.tt.gui.Label tribe_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.GameMenu.i18n("tribe_name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                chief_line = new com.oddlabs.tt.gui.EditLine(150, 20);
                tribe_line = new com.oddlabs.tt.gui.EditLine(150, 20);
                chief_line.append(com.oddlabs.tt.global.Settings.getSettings().chief_name);
                tribe_line.append(com.oddlabs.tt.global.Settings.getSettings().tribe_name);
                com.oddlabs.tt.guievent.EnterListener send_names = new com.oddlabs.tt.guievent.EnterListener_L((string unused) =>
                {
                    sendNames();
                });
                chief_line.addEnterListener(send_names);
                tribe_line.addEnterListener(send_names);
                com.oddlabs.tt.gui.HorizButton names_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.GameMenu.i18n("set_names"), 80);
                names_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
                {
                    sendNames();
                }));
                names_group.addChild(chief_label);
                names_group.addChild(chief_line);
                names_group.addChild(tribe_label);
                names_group.addChild(tribe_line);
                names_group.addChild(names_button);
                chief_label.place();
                chief_line.place(chief_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                tribe_label.place(chief_line, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                tribe_line.place(tribe_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                names_button.place(tribe_line, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            }
            names_group.compileCanvas();
            addChild(names_group);
            chat_info = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.GameMenu.i18n("chat"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), width);
            com.oddlabs.tt.gui.Group chat_line_group = new com.oddlabs.tt.gui.Group();
            chat_line = new com.oddlabs.tt.gui.EditLine(width - com.oddlabs.tt.form.GameMenu.SEND_BUTTON_WIDTH - fdata.objectSpacing(), 100);
            com.oddlabs.tt.gui.HorizButton send_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.GameMenu.i18n("send"), com.oddlabs.tt.form.GameMenu.SEND_BUTTON_WIDTH);
            send_button.addMouseClickListener(new com.oddlabs.tt.form.GameMenu.SendListener(this));
            chat_line_group.addChild(chat_line);
            chat_line.place();
            chat_line_group.addChild(send_button);
            send_button.place(chat_line, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            chat_line_group.compileCanvas();
            addChild(chat_line_group);
            chat_line.addEnterListener(new com.oddlabs.tt.form.GameMenu.ChatListener(this));
            addChild(game_name_label);
            addChild(chat_info);
            start_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.GameMenu.i18n("start"), button_width);
            if (local_player_slot == 0)
            {
                addChild(start_button);
                start_button.addMouseClickListener(new com.oddlabs.tt.form.GameMenu.StartListener(this));
            }
            int height = compare_height - pdata.getTopOffset() - pdata.getBottomOffset() - chat_info.getHeight() - chat_line.getHeight() - game_name_label.getHeight() - player_group.getHeight() - start_button.getHeight() - 5 * fdata.objectSpacing() - ((names_group.getHeight() > 0 ? names_group.getHeight() + fdata.objectSpacing() : 0));
            chat_box = new com.oddlabs.tt.gui.TextBox(width, height, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), java.lang.Integer.MAX_VALUE);
            addChild(chat_box);
            ready_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.GameMenu.i18n("ready"), button_width);
            addChild(ready_button);
            ready_button.addMouseClickListener(new com.oddlabs.tt.form.GameMenu.ReadyListener(this));
            com.oddlabs.tt.gui.HorizButton cancel_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.GameMenu.i18n("cancel"), button_width);
            addChild(cancel_button);
            cancel_button.addMouseClickListener(new com.oddlabs.tt.form.GameMenu.CancelButtonListener(this));
            com.oddlabs.tt.gui.HorizButton info_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.GameMenu.i18n("info"), button_width);
            addChild(info_button);
            info_button.addMouseClickListener(new com.oddlabs.tt.form.GameMenu.InfoButtonListener(this));
            game_name_label.place();
            player_group.place(game_name_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            if (names_group.getHeight() > 0)
            {
                names_group.place(player_group, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                chat_info.place(names_group, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            }
            else
            {
                chat_info.place(player_group, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            }
            chat_box.place(chat_info, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            chat_line_group.place(chat_box, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cancel_button.place(chat_line_group, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            info_button.place(chat_line_group, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            ready_button.place(cancel_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            if (local_player_slot == 0)
            {
                start_button.place(ready_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            }
            com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getEditFont();
            if (rated)
            {
                com.oddlabs.tt.gui.Label rating = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.GameMenu.i18n("rating"), font, com.oddlabs.tt.form.GameMenu.RATING_WIDTH, com.oddlabs.tt.gui.Origin.AT_END);
                addChild(rating);
                rating.place(player_group, com.oddlabs.tt.gui.Placement.TOP_RIGHT);
                if (player_count > com.oddlabs.tt.form.GameMenu.DEFAULT_PLAYER_COUNT)
                {
                    rating.setPos(rating.getX() - 80, rating.getY());
                }
            }
            compileCanvas();
        }
    }
}
