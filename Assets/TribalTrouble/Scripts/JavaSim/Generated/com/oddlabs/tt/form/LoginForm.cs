// Translated from LoginForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 34
    public class LoginForm : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public const int BUTTON_WIDTH_LONG = 150;
        public const int EDITLINE_WIDTH = 240;
        public com.oddlabs.tt.@delegate.MainMenu main_menu;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gui.EditLine editline_username;
        public com.oddlabs.tt.gui.PasswordLine editline_password;
        public com.oddlabs.tt.gui.CheckBox remember_checkbox;
        public static java.util.ResourceBundle bundle;
        // Java: 47
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.LoginForm.bundle, key, args);
        }
        // Java: 134
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
        public void loginFailed()
        {
            editline_password.forget();
            if (com.oddlabs.tt.global.Settings.getSettings().remember_login)
            {
                com.oddlabs.tt.global.Settings.getSettings().pw_digest = "";
                com.oddlabs.tt.global.Settings.getSettings().save();
            }
            editline_password.setFocus();
        }
        // Java: 153
        public void login()
        {
            string username = editline_username.getContents();
            string password = editline_password.getPasswordDigest();
            com.oddlabs.matchmaking.Login login_v = new com.oddlabs.matchmaking.Login(username, password);
            if (!login_v.isValid())
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("invalid_login")));
            }
            else
            {
                doLogin(username, password, login_v, remember_checkbox.isMarked());
            }
        }
        // Java: 163
        public void doLogin(string username, string password, com.oddlabs.matchmaking.Login login_v, bool remember_login)
        {
            if (remember_login)
            {
                com.oddlabs.tt.global.Settings.getSettings().username = username;
                com.oddlabs.tt.global.Settings.getSettings().pw_digest = password;
            }
            com.oddlabs.tt.global.Settings.getSettings().remember_login = remember_login;
            com.oddlabs.tt.gui.Form connecting_form = new com.oddlabs.tt.form.MatchmakingConnectingForm(network, gui_root, this, main_menu, login_v, null);
            gui_root.addModalForm(connecting_form);
        }
        // Java: 173
        public class NewUserListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.LoginForm this_0;
            // Java: 174
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.remove();
                this_0.main_menu.setMenu(new com.oddlabs.tt.form.NewUserForm(this_0.network, this_0.gui_root, this_0.main_menu));
            }
            public NewUserListener(com.oddlabs.tt.form.LoginForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 181
        public class LoginListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.LoginForm this_0;
            // Java: 182
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.login();
            }
            // Java: 187
            public void enterPressed(string text)
            {
                this_0.login();
            }
            public LoginListener(com.oddlabs.tt.form.LoginForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static LoginForm()
        {
            LoginForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.LoginForm).getName());
        }
        public LoginForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.MainMenu main_menu) : base()
        {
            this.main_menu = main_menu;
            this.gui_root = gui_root;
            this.network = network;
            bool remember = com.oddlabs.tt.global.Settings.getSettings().remember_login;
            if (!remember)
            {
                com.oddlabs.tt.global.Settings.getSettings().username = "";
                com.oddlabs.tt.global.Settings.getSettings().pw_digest = "";
            }
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("login_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.form.LoginForm.LoginListener login_listener = new com.oddlabs.tt.form.LoginForm.LoginListener(this);
            com.oddlabs.tt.gui.Group login_group = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_username = new com.oddlabs.tt.gui.Label(i18n("username"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_username = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.LoginForm.EDITLINE_WIDTH, 255);
            editline_username.addEnterListener(login_listener);
            editline_username.append(com.oddlabs.tt.global.Settings.getSettings().username);
            com.oddlabs.tt.gui.Label label_password = new com.oddlabs.tt.gui.Label(i18n("password"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_password = new com.oddlabs.tt.gui.PasswordLine(com.oddlabs.tt.form.LoginForm.EDITLINE_WIDTH, 255);
            editline_password.addEnterListener(login_listener);
            if (remember && !com.oddlabs.tt.global.Settings.getSettings().pw_digest.isEmpty())
            {
                editline_password.setRemembered(com.oddlabs.tt.global.Settings.getSettings().pw_digest);
            }
            remember_checkbox = new com.oddlabs.tt.gui.CheckBox(remember, i18n("remember_login"));
            login_group.addChild(label_username);
            login_group.addChild(editline_username);
            login_group.addChild(label_password);
            login_group.addChild(editline_password);
            login_group.addChild(remember_checkbox);
            label_username.place();
            editline_username.place(label_username, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            editline_password.place(editline_username, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            label_password.place(editline_password, com.oddlabs.tt.gui.Placement.LEFT_MID);
            remember_checkbox.place(editline_password, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            login_group.compileCanvas();
            addChild(login_group);
            com.oddlabs.tt.gui.Group group_buttons = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.ButtonObject button_newuser = new com.oddlabs.tt.gui.HorizButton(i18n("new_account"), com.oddlabs.tt.form.LoginForm.BUTTON_WIDTH);
            button_newuser.addMouseClickListener(new com.oddlabs.tt.form.LoginForm.NewUserListener(this));
            com.oddlabs.tt.gui.ButtonObject button_ok = new com.oddlabs.tt.gui.HorizButton(i18n("login"), com.oddlabs.tt.form.LoginForm.BUTTON_WIDTH);
            button_ok.addMouseClickListener(login_listener);
            com.oddlabs.tt.gui.ButtonObject button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.LoginForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            group_buttons.addChild(button_newuser);
            group_buttons.addChild(button_ok);
            group_buttons.addChild(button_cancel);
            button_cancel.place();
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_newuser.place(button_ok, com.oddlabs.tt.gui.Placement.LEFT_MID);
            group_buttons.compileCanvas();
            addChild(group_buttons);
            label_headline.place();
            login_group.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_buttons.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            if (com.oddlabs.tt.render.Renderer.isRegistered())
            {
                main_menu.setMenu(this);
            }
            else
            {
                com.oddlabs.tt.gui.Form form = new com.oddlabs.tt.form.MatchmakingConnectingForm(network, gui_root, null, main_menu, null, null);
                main_menu.setMenu(form);
                form.centerPos();
            }
        }
    }
}
