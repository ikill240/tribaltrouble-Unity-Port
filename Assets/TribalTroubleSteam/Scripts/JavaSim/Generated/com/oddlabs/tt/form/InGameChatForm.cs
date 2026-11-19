// Translated from InGameChatForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 32
    public class InGameChatForm : com.oddlabs.tt.gui.Form, com.oddlabs.tt.net.ChatListener
    {
        public const int CHAT_WIDTH = 400;
        public const int BUTTON_WIDTH = 50;
        public const int CHAT_HEIGHT = 150;
        public static java.util.ResourceBundle bundle;
        // Java: 38
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.InGameChatForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.EditLine chat_line;
        public com.oddlabs.tt.gui.InfoPrinter info_printer;
        public com.oddlabs.tt.gui.TextBox chat_box;
        public com.oddlabs.tt.gui.RadioButtonGroup radio_button_group;
        public com.oddlabs.tt.gui.RadioButton radio_all;
        public com.oddlabs.tt.gui.RadioButton radio_team;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        // Java: 83
        public override void doAdd()
        {
            base.doAdd();
            com.oddlabs.tt.net.Network.getChatHub().addListener(this);
            refreshMessages();
        }
        // Java: 90
        public override void doRemove()
        {
            base.doRemove();
            com.oddlabs.tt.net.Network.getChatHub().removeListener(this);
        }
        // Java: 96
        public void setReceivers(bool all)
        {
            radio_button_group.mark((all ? (com.oddlabs.tt.gui.RadioButtonGroupElement)(radio_all) : (com.oddlabs.tt.gui.RadioButtonGroupElement)(radio_team)));
        }
        // Java: 100
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
        // Java: 109
        public void chat(com.oddlabs.tt.net.ChatMessage message)
        {
            refreshMessages();
        }
        // Java: 114
        public void refreshMessages()
        {
            java.util.List messages = com.oddlabs.tt.net.Network.getMatchmakingClient().getInGameChatHistory();
            chat_box.clear();
            for (int i = 0; i < messages.size(); i++)
            {
                if (i != 0)
                {
                    chat_box.append("\n");
                }
                chat_box.append(((string)messages.get(i)));
            }
            chat_box.setOffsetY(java.lang.Integer.MAX_VALUE);
        }
        // Java: 125
        public override void mouseMoved(int x, int y)
        {
            (((com.oddlabs.tt.@delegate.ControllableCameraDelegate)((com.oddlabs.tt.gui.GUIObject)getParent()))).mouseMoved(x, y);
        }
        // Java: 130
        public class ChatListener : java.lang.Object, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.InGameChatForm this_0;
            // Java: 131
            public void enterPressed(string text)
            {
                string chat_v = java.lang.JO.toString(text);
                if (!chat_v.isEmpty())
                {
                    this_0.chat_line.clear();
                    java.util.Map commands = java.util.Map_.of("iamacheater", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter unused, string unused_2) =>
                    {
                        this_0.viewer.getCheat().enable();
                    }));
                    if (!com.oddlabs.tt.net.ChatCommand.filterCommand(this_0.info_printer, commands, chat_v))
                    {
                        this_0.viewer.getPeerHub().sendChat(chat_v, ((object)this_0.radio_button_group.getMarked() == (object)this_0.radio_team));
                    }
                }
                else
                {
                    this_0.cancel();
                }
            }
            public ChatListener(com.oddlabs.tt.form.InGameChatForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static InGameChatForm()
        {
            InGameChatForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.InGameChatForm).getName());
        }
        public InGameChatForm(com.oddlabs.tt.gui.InfoPrinter info_printer, com.oddlabs.tt.viewer.WorldViewer viewer) : base(com.oddlabs.tt.form.InGameChatForm.i18n("chat"))
        {
            this.viewer = viewer;
            this.info_printer = info_printer;
            chat_line = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.InGameChatForm.CHAT_WIDTH, 256);
            addChild(chat_line);
            chat_line.addEnterListener(new com.oddlabs.tt.form.InGameChatForm.ChatListener(this));
            com.oddlabs.tt.gui.HorizButton button_send = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.InGameChatForm.i18n("send"), com.oddlabs.tt.form.InGameChatForm.BUTTON_WIDTH);
            addChild(button_send);
            button_send.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                chat_line.enterPressedAll();
            }));
            chat_box = new com.oddlabs.tt.gui.TextBox(com.oddlabs.tt.form.InGameChatForm.CHAT_WIDTH + com.oddlabs.tt.form.InGameChatForm.BUTTON_WIDTH, com.oddlabs.tt.form.InGameChatForm.CHAT_HEIGHT, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), java.lang.Integer.MAX_VALUE);
            addChild(chat_box);
            radio_button_group = new com.oddlabs.tt.gui.RadioButtonGroup();
            radio_all = new com.oddlabs.tt.gui.RadioButton(true, radio_button_group, com.oddlabs.tt.form.InGameChatForm.i18n("send_to_all"));
            addChild(radio_all);
            radio_team = new com.oddlabs.tt.gui.RadioButton(false, radio_button_group, com.oddlabs.tt.form.InGameChatForm.i18n("send_to_team"));
            addChild(radio_team);
            chat_line.place();
            button_send.place(chat_line, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            chat_box.place(chat_line, com.oddlabs.tt.gui.Placement.TOP_LEFT);
            radio_all.place(chat_line, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            radio_team.place(chat_line, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            compileCanvas();
            com.oddlabs.tt.net.Network.getMatchmakingClient().clearInGameChatHistory();
        }
    }
}
