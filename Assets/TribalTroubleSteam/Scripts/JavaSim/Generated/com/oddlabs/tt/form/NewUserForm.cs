// Translated from NewUserForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 33
    public class NewUserForm : com.oddlabs.tt.gui.Form
    {
        public const int MIN_PASSWORD_LENGTH = 6;
        public const int BUTTON_WIDTH = 100;
        public const int BUTTON_WIDTH_LONG = 150;
        public const int EDITLINE_WIDTH = 240;
        public com.oddlabs.tt.@delegate.MainMenu main_menu;
        public com.oddlabs.tt.gui.EditLine editline_username;
        public com.oddlabs.tt.gui.EditLine editline_email;
        public com.oddlabs.tt.gui.PasswordLine editline_password;
        public com.oddlabs.tt.gui.PasswordLine editline_verify;
        public static java.util.ResourceBundle bundle;
        // Java: 47
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.NewUserForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        // Java: 135
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                editline_username.setFocus(direction);
            }
        }
        // Java: 144
        public void createUser()
        {
            string username = editline_username.getContents();
            string password = editline_password.getPasswordDigest();
            com.oddlabs.matchmaking.LoginDetails login_details = new com.oddlabs.matchmaking.LoginDetails(editline_email.getContents());
            if (!java.lang.JO.equals(editline_password.getContents(), editline_verify.getContents()))
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("no_match")));
                editline_password.clear();
                editline_verify.clear();
            }
            else if (editline_password.getContents().length() < com.oddlabs.tt.form.NewUserForm.MIN_PASSWORD_LENGTH)
            {
                string min_length_err = i18n("min_length_error", com.oddlabs.tt.form.NewUserForm.MIN_PASSWORD_LENGTH);
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(min_length_err));
            }
            else if (!login_details.isValid())
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("invalid_email")));
            }
            else
            {
                com.oddlabs.matchmaking.Login login = new com.oddlabs.matchmaking.Login(username, password);
                if (!login.isValid())
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("invalid_login")));
                }
                else
                {
                    doCreateUser(username, login_details, password, login);
                }
            }
        }
        // Java: 166
        public void doCreateUser(string username, com.oddlabs.matchmaking.LoginDetails login_details, string password, com.oddlabs.matchmaking.Login login)
        {
            com.oddlabs.tt.global.Settings.getSettings().username = username;
            com.oddlabs.tt.global.Settings.getSettings().pw_digest = password;
            com.oddlabs.tt.gui.Form connecting_form = new com.oddlabs.tt.form.MatchmakingConnectingForm(network, gui_root, this, main_menu, login, login_details);
            gui_root.addModalForm(connecting_form);
        }
        // Java: 174
        public class CreateUserListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.NewUserForm this_0;
            // Java: 175
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.createUser();
            }
            // Java: 180
            public void enterPressed(string text)
            {
                this_0.createUser();
            }
            public CreateUserListener(com.oddlabs.tt.form.NewUserForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static NewUserForm()
        {
            NewUserForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.NewUserForm).getName());
        }
        public NewUserForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.MainMenu main_menu) : base()
        {
            this.main_menu = main_menu;
            this.gui_root = gui_root;
            this.network = network;
            com.oddlabs.tt.form.NewUserForm.CreateUserListener create_listener = new com.oddlabs.tt.form.NewUserForm.CreateUserListener(this);
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("create_new_user_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.Group login_group = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_username = new com.oddlabs.tt.gui.Label(i18n("user_name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_email = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.NewUserForm.EDITLINE_WIDTH, 255);
            editline_email.addEnterListener(create_listener);
            com.oddlabs.tt.gui.Label label_email = new com.oddlabs.tt.gui.Label(i18n("email"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_username = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.NewUserForm.EDITLINE_WIDTH, 255);
            editline_username.addEnterListener(create_listener);
            com.oddlabs.tt.gui.Label label_password = new com.oddlabs.tt.gui.Label(i18n("password"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_password = new com.oddlabs.tt.gui.PasswordLine(com.oddlabs.tt.form.NewUserForm.EDITLINE_WIDTH, 255);
            editline_password.addEnterListener(create_listener);
            com.oddlabs.tt.gui.Label label_verify = new com.oddlabs.tt.gui.Label(i18n("reenter_password"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_verify = new com.oddlabs.tt.gui.PasswordLine(com.oddlabs.tt.form.NewUserForm.EDITLINE_WIDTH, 255);
            editline_verify.addEnterListener(create_listener);
            login_group.addChild(label_username);
            login_group.addChild(editline_username);
            login_group.addChild(label_email);
            login_group.addChild(editline_email);
            login_group.addChild(label_password);
            login_group.addChild(editline_password);
            login_group.addChild(label_password);
            login_group.addChild(editline_verify);
            login_group.addChild(label_verify);
            label_username.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            editline_username.place(label_username, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            editline_email.place(editline_username, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            label_email.place(editline_email, com.oddlabs.tt.gui.Placement.LEFT_MID);
            editline_password.place(editline_email, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            label_password.place(editline_password, com.oddlabs.tt.gui.Placement.LEFT_MID);
            editline_verify.place(editline_password, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            label_verify.place(editline_verify, com.oddlabs.tt.gui.Placement.LEFT_MID);
            login_group.compileCanvas();
            addChild(login_group);
            com.oddlabs.tt.gui.Label label_one_user = new com.oddlabs.tt.gui.Label(i18n("one_user_per_key"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label_one_user);
            com.oddlabs.tt.gui.Group group_buttons = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.ButtonObject button_create = new com.oddlabs.tt.gui.HorizButton(i18n("create_user"), com.oddlabs.tt.form.NewUserForm.BUTTON_WIDTH_LONG);
            button_create.addMouseClickListener(create_listener);
            com.oddlabs.tt.gui.ButtonObject button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.NewUserForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            group_buttons.addChild(button_create);
            group_buttons.addChild(button_cancel);
            button_cancel.place();
            button_create.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            group_buttons.compileCanvas();
            addChild(group_buttons);
            label_headline.place();
            label_one_user.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            login_group.place(label_one_user, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_buttons.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
        }
    }
}
