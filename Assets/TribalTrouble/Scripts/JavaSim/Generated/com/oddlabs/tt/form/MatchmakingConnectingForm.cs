// Translated from MatchmakingConnectingForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 25
    public class MatchmakingConnectingForm : com.oddlabs.tt.gui.Form, com.oddlabs.tt.net.MatchmakingListener
    {
        public com.oddlabs.tt.gui.Form parent_form;
        public com.oddlabs.tt.@delegate.MainMenu main_menu;
        public static java.util.ResourceBundle bundle;
        // Java: 30
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.MatchmakingConnectingForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public bool steam_login;
        const int NO_SUCH_USER = com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_NO_SUCH_USER;
        // Java: 73
        public void clearList(int type)
        {
        }
        // Java: 78
        public void receivedList(int type, object[] names)
        {
        }
        // Java: 83
        public void joinedChat(com.oddlabs.tt.gui.ChatRoomInfo info)
        {
        }
        // Java: 88
        public void updateChatRoom(com.oddlabs.tt.gui.ChatRoomInfo info)
        {
        }
        // Java: 93
        public void receivedProfiles(com.oddlabs.matchmaking.Profile[] profiles, string last_nick)
        {
        }
        // Java: 98
        public void profileUpdated()
        {
        }
        // Java: 103
        public override void doRemove()
        {
            base.doRemove();
            com.oddlabs.tt.net.Network.setMatchmakingListener(null);
        }
        // Java: 109
        public void connectionLost()
        {
            remove();
            gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("connection_failed")));
        }
        // Java: 115
        public void loginError(int error_code)
        {
            remove();
            // a Steam sign-in the server doesn't take (the 1.4 / New Tides builds run as Steam's test app, which the
            // official server doesn't know: "user does not exist"): sign in with an account or as a guest instead
            if (steam_login && error_code == com.oddlabs.tt.form.MatchmakingConnectingForm.NO_SUCH_USER && main_menu != null)
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().close();
                new com.oddlabs.tt.form.LoginForm(network, gui_root, main_menu);
                return;
            }
            string error_message = ((error_code) switch { 1 => (string)(i18n("username_error_too_many")), 9 => (string)(i18n("user_error_version_too_old")), 8 => (string)(i18n("user_error_no_such_user")), 7 => (string)(i18n("user_error_invalid_email")), 3 => (string)(i18n("username_error_already_exists")), 4 => (string)(i18n("username_error_invalid_characters")), 5 => (string)(i18n("username_error_too_long")), 6 => (string)(i18n("username_error_too_short")), 13 => (string)(i18n("user_error_steam_required")), _ => (string)(java.lang.JStr.valueOf(i18n("connection_failed")) + " (" + (error_code) + ")") });
            if ((parent_form is com.oddlabs.tt.form.LoginForm login_form))
            {
                login_form.loginFailed();
            }
            gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(error_message));
        }
        // Java: 136
        public void loggedIn()
        {
            remove();
            if (parent_form != null)
            {
                parent_form.remove();
            }
            new com.oddlabs.tt.form.SelectGameMenu(network, gui_root, main_menu);
        }
        // Java: 144
        public override void doCancel()
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().close();
        }
        static MatchmakingConnectingForm()
        {
            MatchmakingConnectingForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.MatchmakingConnectingForm).getName());
        }
        public MatchmakingConnectingForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.Form parent_form, com.oddlabs.tt.@delegate.MainMenu main_menu, com.oddlabs.matchmaking.Login login, com.oddlabs.matchmaking.LoginDetails login_details) : this(network, gui_root, parent_form, main_menu)
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().login(network, login, login_details);
        }
        public MatchmakingConnectingForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.Form parent_form, com.oddlabs.tt.@delegate.MainMenu main_menu, bool steamLogin) : this(network, gui_root, parent_form, main_menu)
        {
            if (steamLogin)
            {
                steam_login = true;
                com.oddlabs.tt.net.Network.getMatchmakingClient().loginWithSteam(network);
            }
        }
        public MatchmakingConnectingForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.Form parent_form, com.oddlabs.tt.@delegate.MainMenu main_menu) : base()
        {
            this.parent_form = parent_form;
            this.main_menu = main_menu;
            this.gui_root = gui_root;
            this.network = network;
            com.oddlabs.tt.gui.Label info_label = new com.oddlabs.tt.gui.Label(i18n("connecting"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(info_label);
            com.oddlabs.tt.gui.HorizButton cancel_button = new com.oddlabs.tt.gui.CancelButton(120);
            addChild(cancel_button);
            cancel_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            info_label.place();
            cancel_button.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            compileCanvas();
            centerPos();
            com.oddlabs.tt.net.Network.setMatchmakingListener(this);
        }
    }
}
