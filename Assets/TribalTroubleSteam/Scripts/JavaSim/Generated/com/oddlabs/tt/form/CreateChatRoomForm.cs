// Translated from CreateChatRoomForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 28
    public class CreateChatRoomForm : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public const int EDITLINE_WIDTH = 240;
        public com.oddlabs.tt.gui.EditLine editline_name;
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.form.SelectGameMenu menu;
        public static java.util.ResourceBundle bundle;
        // Java: 37
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.CreateChatRoomForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        // Java: 79
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                editline_name.setFocus(direction);
            }
        }
        // Java: 88
        public override void doCancel()
        {
            main_menu.setMenuCentered(menu);
        }
        // Java: 93
        public void create()
        {
            string name = editline_name.getContents();
            if (name.isEmpty())
            {
                string min_name_error = i18n("min_name_error", com.oddlabs.matchmaking.MatchmakingServerInterface.MIN_ROOM_NAME_LENGTH);
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(min_name_error));
            }
            else
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().joinRoom(gui_root, name);
            }
            main_menu.setMenuCentered(menu);
        }
        // Java: 104
        public class OKListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.CreateChatRoomForm this_0;
            // Java: 105
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.create();
            }
            // Java: 110
            public void enterPressed(string text)
            {
                this_0.create();
            }
            public OKListener(com.oddlabs.tt.form.CreateChatRoomForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static CreateChatRoomForm()
        {
            CreateChatRoomForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.CreateChatRoomForm).getName());
        }
        public CreateChatRoomForm(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu, com.oddlabs.tt.form.SelectGameMenu menu) : base()
        {
            this.gui_root = gui_root;
            this.main_menu = main_menu;
            this.menu = menu;
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("create_room"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.Label label_name = new com.oddlabs.tt.gui.Label(i18n("name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_name = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.CreateChatRoomForm.EDITLINE_WIDTH, com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_ROOM_NAME_LENGTH, com.oddlabs.matchmaking.MatchmakingServerInterface.ALLOWED_ROOM_CHARS, com.oddlabs.tt.gui.Origin.AT_START);
            editline_name.addEnterListener(new com.oddlabs.tt.form.CreateChatRoomForm.OKListener(this));
            addChild(label_name);
            addChild(editline_name);
            com.oddlabs.tt.gui.ButtonObject button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.CreateChatRoomForm.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.form.CreateChatRoomForm.OKListener(this));
            com.oddlabs.tt.gui.ButtonObject button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.CreateChatRoomForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            addChild(button_ok);
            addChild(button_cancel);
            label_headline.place();
            label_name.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            editline_name.place(label_name, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_cancel.place(com.oddlabs.tt.gui.Origin.AT_END);
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
        }
    }
}
