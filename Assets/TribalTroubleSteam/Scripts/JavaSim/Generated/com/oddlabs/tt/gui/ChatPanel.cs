// Translated from ChatPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 24
    public class ChatPanel : com.oddlabs.tt.gui.Panel, com.oddlabs.tt.net.ChatListener
    {
        public const int PULLDOWN_INDEX_MESSAGE = 0;
        public const int PULLDOWN_INDEX_INFO = 1;
        public const int PULLDOWN_INDEX_IGNORE = 2;
        public const bool SPECTATE_ENABLED = true;
        public const int PULLDOWN_INDEX_PLAYING_SPECTATE = 2;
        public const int PULLDOWN_INDEX_PLAYING_IGNORE = 3;
        public com.oddlabs.tt.gui.MultiColumnComboBox lobby_users_list_box;
        public com.oddlabs.tt.gui.MultiColumnComboBox playing_users_list_box;
        public com.oddlabs.tt.gui.TextBox chat_box;
        public com.oddlabs.tt.gui.EditLine chat_line;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public java.lang.Runnable leave_game_listener;
        public int user_list_width;
        public com.oddlabs.tt.form.PrivateMessageForm private_message_form;
        // Java: 46
        public static java.util.ResourceBundle getBundle()
        {
            return java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.ChatPanel).getName());
        }
        // Java: 50
        public static string getI18N(string key)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.ChatPanel.getBundle(), key);
        }
        // Java: 134
        public void update(com.oddlabs.tt.gui.ChatRoomInfo info)
        {
            com.oddlabs.matchmaking.ChatRoomUser[] users = info.users();
            if (users != null)
            {
                lobby_users_list_box.clear();
                playing_users_list_box.clear();
                foreach (com.oddlabs.matchmaking.ChatRoomUser user in users)
                {
                    int label_width = user_list_width - (com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box().getLeftOffset() + com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box().getRightOffset());
                    com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(user.getNick(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), label_width);
                    com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(java.util.List_.of(label), user);
                    if (!user.isPlaying())
                    {
                        lobby_users_list_box.addRow(row);
                    }
                    else
                    {
                        playing_users_list_box.addRow(row);
                    }
                }
            }
            refreshMessages();
        }
        // Java: 154
        public void chat(com.oddlabs.tt.net.ChatMessage message)
        {
            if (((object)message.type() != (object)com.oddlabs.tt.net.ChatMessage.Type.PRIVATE) && ((object)message.type() != (object)com.oddlabs.tt.net.ChatMessage.Type.CHATROOM))
            {
                return;
            }
            if (((object)message.type() != (object)com.oddlabs.tt.net.ChatMessage.Type.PRIVATE))
            {
                getTab().updateNotify();
            }
            refreshMessages();
        }
        // Java: 164
        public void refreshMessages()
        {
            java.util.List messages = com.oddlabs.tt.net.Network.getMatchmakingClient().getChatRoomHistory();
            chat_box.clear();
            bool first = true;
            foreach (string message in java.lang.JIter.of(messages))
            {
                if (!first)
                {
                    chat_box.append("\n");
                }
                else
                {
                    first = false;
                }
                chat_box.append(message);
            }
            chat_box.setOffsetY(java.lang.Integer.MAX_VALUE);
        }
        // Java: 179
        public override void setFocus()
        {
            chat_line.setFocus();
        }
        // Java: 184
        public void connectionLost()
        {
            if (private_message_form != null)
            {
                private_message_form.remove();
            }
        }
        // Java: 189
        public class PulldownListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.gui.ChatPanel this_0;
            public com.oddlabs.tt.gui.MultiColumnComboBox box;
            // Java: 196
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                com.oddlabs.matchmaking.ChatRoomUser user = ((com.oddlabs.matchmaking.ChatRoomUser)box.getRightClickedRowData());
                string nick = user.getNick();
                switch ((item_index))
                {
                    case 0:
                    {
                        this_0.gui_root.addModalForm(new com.oddlabs.tt.form.PrivateMessageForm(this_0.gui_root, nick));
                        break;
                    }
                    case 1:
                    {
                        com.oddlabs.tt.net.Network.getMatchmakingClient().requestInfo(this_0.gui_root, nick);
                        break;
                    }
                    case 2:
                    {
                        if (com.oddlabs.tt.net.ChatCommand.isIgnoring(nick))
                        {
                            com.oddlabs.tt.net.ChatCommand.unignore(this_0.gui_root.getInfoPrinter(), nick);
                        }
                        else
                        {
                            com.oddlabs.tt.net.ChatCommand.ignore(this_0.gui_root.getInfoPrinter(), nick);
                        }
                        break;
                    }
                    default:
                    {
                        throw new System.ArgumentException("Unexpected pulldown index");
                    }
                }
                box.setFocus();
            }
            public PulldownListener(com.oddlabs.tt.gui.ChatPanel this_0_, com.oddlabs.tt.gui.MultiColumnComboBox box) : base()
            {
                this.this_0 = this_0_;
                this.box = box;
            }
        }
        // Java: 220
        public class PlayingPulldownListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.gui.ChatPanel this_0;
            public com.oddlabs.tt.gui.MultiColumnComboBox box;
            // Java: 227
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                com.oddlabs.matchmaking.ChatRoomUser user = ((com.oddlabs.matchmaking.ChatRoomUser)box.getRightClickedRowData());
                string nick = user.getNick();
                switch ((item_index))
                {
                    case 0:
                    {
                        this_0.gui_root.addModalForm(new com.oddlabs.tt.form.PrivateMessageForm(this_0.gui_root, nick));
                        break;
                    }
                    case 1:
                    {
                        com.oddlabs.tt.net.Network.getMatchmakingClient().requestInfo(this_0.gui_root, nick);
                        break;
                    }
                    case 2:
                    {
                        this_0.leave_game_listener.run();
                        com.oddlabs.tt.net.Network.getMatchmakingClient().requestSpectate(this_0.gui_root, nick);
                        break;
                    }
                    case 3:
                    {
                        if (com.oddlabs.tt.net.ChatCommand.isIgnoring(nick))
                        {
                            com.oddlabs.tt.net.ChatCommand.unignore(this_0.gui_root.getInfoPrinter(), nick);
                        }
                        else
                        {
                            com.oddlabs.tt.net.ChatCommand.ignore(this_0.gui_root.getInfoPrinter(), nick);
                        }
                        break;
                    }
                    default:
                    {
                        throw new System.ArgumentException("Unexpected pulldown index");
                    }
                }
                box.setFocus();
            }
            public PlayingPulldownListener(com.oddlabs.tt.gui.ChatPanel this_0_, com.oddlabs.tt.gui.MultiColumnComboBox box) : base()
            {
                this.this_0 = this_0_;
                this.box = box;
            }
        }
        // Java: 255
        public class ChatRoomUserDoubleClickedListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.gui.ChatPanel this_0;
            public com.oddlabs.tt.gui.PulldownMenu menu;
            public int ignoreIndex;
            // Java: 268
            public void rowDoubleClicked(object user_p)
            {
                com.oddlabs.matchmaking.ChatRoomUser user = (com.oddlabs.matchmaking.ChatRoomUser)user_p;
                this_0.private_message_form = new com.oddlabs.tt.form.PrivateMessageForm(this_0.gui_root, user.getNick());
                this_0.gui_root.addModalForm(this_0.private_message_form);
            }
            // Java: 274
            public void rowChosen(object user_p)
            {
                com.oddlabs.matchmaking.ChatRoomUser user = (com.oddlabs.matchmaking.ChatRoomUser)user_p;
                string item_text = (com.oddlabs.tt.net.ChatCommand.isIgnoring(user.getNick()) ? com.oddlabs.tt.gui.ChatPanel.getI18N("unignore") : com.oddlabs.tt.gui.ChatPanel.getI18N("ignore"));
                menu.getItem(ignoreIndex).setLabelString(item_text);
            }
            public ChatRoomUserDoubleClickedListener(com.oddlabs.tt.gui.ChatPanel this_0_, com.oddlabs.tt.gui.PulldownMenu menu) : this(this_0_, menu, com.oddlabs.tt.gui.ChatPanel.PULLDOWN_INDEX_IGNORE)
            {
            }
            public ChatRoomUserDoubleClickedListener(com.oddlabs.tt.gui.ChatPanel this_0_, com.oddlabs.tt.gui.PulldownMenu menu, int ignoreIndex) : base()
            {
                this.this_0 = this_0_;
                this.menu = menu;
                this.ignoreIndex = ignoreIndex;
            }
        }
        public ChatPanel(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.ChatRoomInfo info, int compare_width, int compare_height, int button_width, com.oddlabs.tt.guievent.EnterListener chat_listener, com.oddlabs.tt.guievent.MouseClickListener leave_listener, java.lang.Runnable leave_game_listener) : base(com.oddlabs.tt.gui.ChatPanel.getI18N("chat"))
        {
            this.gui_root = gui_root;
            this.leave_game_listener = leave_game_listener;
            com.oddlabs.tt.gui.FormData fdata = com.oddlabs.tt.gui.Skin.getSkin().getFormData();
            com.oddlabs.tt.gui.Box pdata = com.oddlabs.tt.gui.Skin.getSkin().getPanelData().box();
            com.oddlabs.tt.gui.Box edata = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(info.name(), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            int edit_line_height = edata.getBottomOffset() + edata.getTopOffset() + com.oddlabs.tt.gui.Skin.getSkin().getEditFont().getHeight();
            int height = compare_height - pdata.getTopOffset() - pdata.getBottomOffset() - edit_line_height - label_headline.getHeight() - 2 * fdata.objectSpacing();
            int user_list_height = (height - com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing()) / 2;
            user_list_width = 2 * button_width + 2 * fdata.objectSpacing() - com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollBar().getWidth();
            com.oddlabs.tt.gui.ColumnInfo[] lobby_infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.gui.ChatPanel.getI18N("lobby"), user_list_width) };
            lobby_users_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, lobby_infos, user_list_height, true);
            addChild(lobby_users_list_box);
            com.oddlabs.tt.gui.ColumnInfo[] playing_infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.gui.ChatPanel.getI18N("playing"), user_list_width) };
            playing_users_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, playing_infos, user_list_height, true);
            addChild(playing_users_list_box);
            com.oddlabs.tt.gui.PulldownMenu lobby_pulldown_menu = new com.oddlabs.tt.gui.PulldownMenu();
            lobby_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.gui.ChatPanel.getI18N("message")));
            lobby_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.gui.ChatPanel.getI18N("info")));
            lobby_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(""));
            lobby_pulldown_menu.addItemChosenListener(new com.oddlabs.tt.gui.ChatPanel.PulldownListener(this, lobby_users_list_box));
            lobby_users_list_box.setPulldownMenu(lobby_pulldown_menu);
            com.oddlabs.tt.gui.ChatPanel.ChatRoomUserDoubleClickedListener lobby_double_clicked = new com.oddlabs.tt.gui.ChatPanel.ChatRoomUserDoubleClickedListener(this, lobby_pulldown_menu);
            lobby_users_list_box.addRowListener(lobby_double_clicked);
            com.oddlabs.tt.gui.PulldownMenu playing_pulldown_menu = new com.oddlabs.tt.gui.PulldownMenu();
            playing_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.gui.ChatPanel.getI18N("message")));
            playing_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.gui.ChatPanel.getI18N("info")));
            if (com.oddlabs.tt.gui.ChatPanel.SPECTATE_ENABLED)
            {
                playing_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.gui.ChatPanel.getI18N("spectate")));
            }
            playing_pulldown_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(""));
            playing_pulldown_menu.addItemChosenListener(new com.oddlabs.tt.gui.ChatPanel.PlayingPulldownListener(this, playing_users_list_box));
            playing_users_list_box.setPulldownMenu(playing_pulldown_menu);
            com.oddlabs.tt.gui.ChatPanel.ChatRoomUserDoubleClickedListener playing_double_clicked = new com.oddlabs.tt.gui.ChatPanel.ChatRoomUserDoubleClickedListener(this, playing_pulldown_menu, com.oddlabs.tt.gui.ChatPanel.PULLDOWN_INDEX_PLAYING_IGNORE);
            playing_users_list_box.addRowListener(playing_double_clicked);
            int width = compare_width - pdata.getLeftOffset() - pdata.getRightOffset() - lobby_users_list_box.getWidth();
            chat_box = new com.oddlabs.tt.gui.TextBox(width, height, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), java.lang.Integer.MAX_VALUE);
            addChild(chat_box);
            chat_line = new com.oddlabs.tt.gui.EditLine(width, 256);
            chat_line.addEnterListener(chat_listener);
            chat_line.addEnterListener(new com.oddlabs.tt.guievent.EnterListener_L((string unused) =>
            {
                chat_line.clear();
            }));
            addChild(chat_line);
            com.oddlabs.tt.gui.HorizButton button_send = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.gui.ChatPanel.getI18N("send"), button_width);
            button_send.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
            {
                chat_line.enterPressedAll();
            }));
            addChild(button_send);
            com.oddlabs.tt.gui.HorizButton button_leave = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.gui.ChatPanel.getI18N("leave"), button_width);
            button_leave.addMouseClickListener(leave_listener);
            addChild(button_leave);
            label_headline.place(com.oddlabs.tt.gui.Origin.AT_START);
            chat_box.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            lobby_users_list_box.place(chat_box, com.oddlabs.tt.gui.Placement.RIGHT_TOP, 0);
            playing_users_list_box.place(lobby_users_list_box, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            chat_line.place(chat_box, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            button_send.place(chat_line, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_leave.place(button_send, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            compileCanvas();
            update(info);
        }
    }
}
