// Translated from PrivateMessageForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 24
    public class PrivateMessageForm : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public const int EDITLINE_WIDTH = 240;
        public static java.util.ResourceBundle bundle;
        // Java: 29
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.PrivateMessageForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.EditLine editline_name;
        public string nick;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        // Java: 70
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
        // Java: 79
        public void send()
        {
            string message = editline_name.getContents();
            com.oddlabs.tt.net.Network.getMatchmakingClient().sendPrivateMessage(gui_root, nick, message);
            remove();
        }
        static PrivateMessageForm()
        {
            PrivateMessageForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.PrivateMessageForm).getName());
        }
        public PrivateMessageForm(com.oddlabs.tt.gui.GUIRoot gui_root, string nick) : base()
        {
            this.gui_root = gui_root;
            this.nick = nick;
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("private_message_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.Label label_name = new com.oddlabs.tt.gui.Label(i18n("to", nick), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_name = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.PrivateMessageForm.EDITLINE_WIDTH, 256);
            editline_name.addEnterListener(new com.oddlabs.tt.guievent.EnterListener_L((string unused) =>
            {
                send();
            }));
            addChild(label_name);
            addChild(editline_name);
            com.oddlabs.tt.gui.ButtonObject button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.PrivateMessageForm.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
            {
                send();
            }));
            com.oddlabs.tt.gui.ButtonObject button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.PrivateMessageForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_6, int unused_7, int unused_8, int unused_9) =>
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
            centerPos();
        }
    }
}
