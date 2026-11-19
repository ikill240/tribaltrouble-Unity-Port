// Translated from CreatingProfileForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 20
    public class CreatingProfileForm : com.oddlabs.tt.gui.Form, com.oddlabs.tt.net.ProfileListener
    {
        public static java.util.ResourceBundle bundle;
        // Java: 23
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.CreatingProfileForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.Form profiles_form;
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        // Java: 53
        public void success()
        {
            remove();
            main_menu.setMenuCentered(profiles_form);
            com.oddlabs.tt.net.Network.getMatchmakingClient().requestProfiles();
        }
        // Java: 60
        public void error(int error_code)
        {
            remove();
            string error_message = ((error_code) switch { 1 => (string)(i18n("username_error_too_many")), 2 => (string)(i18n("profile_error_guest", "Guest")), 3 => (string)(i18n("username_error_already_exists")), 4 => (string)(i18n("username_error_invalid_characters")), 5 => (string)(i18n("username_error_too_long")), 6 => (string)(i18n("username_error_too_short")), _ => throw new System.ArgumentException("Unknown error code: " + (error_code)) });
            gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(error_message));
        }
        static CreatingProfileForm()
        {
            CreatingProfileForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.CreatingProfileForm).getName());
        }
        public CreatingProfileForm(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.Form profiles_form, com.oddlabs.tt.@delegate.Menu main_menu, string nick) : base()
        {
            this.gui_root = gui_root;
            this.profiles_form = profiles_form;
            this.main_menu = main_menu;
            com.oddlabs.tt.gui.Label info_label = new com.oddlabs.tt.gui.Label(i18n("creating"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
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
            com.oddlabs.tt.net.Network.getMatchmakingClient().setCreatingProfileListener(this);
            com.oddlabs.tt.net.Network.getMatchmakingClient().createProfile(nick);
        }
    }
}
