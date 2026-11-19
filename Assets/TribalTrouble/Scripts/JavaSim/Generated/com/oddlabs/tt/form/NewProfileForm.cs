// Translated from NewProfileForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 27
    public class NewProfileForm : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public const int BUTTON_WIDTH_LONG = 150;
        public const int EDITLINE_WIDTH = 240;
        public static java.util.ResourceBundle bundle;
        // Java: 33
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.NewProfileForm.bundle, key, args);
        }
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.form.ProfilesForm profiles_form;
        public com.oddlabs.tt.gui.EditLine editline_nick;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        // Java: 83
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                editline_nick.setFocus(direction);
            }
        }
        // Java: 92
        public override void doCancel()
        {
            done();
        }
        // Java: 97
        public void done()
        {
            main_menu.setMenuCentered(profiles_form);
        }
        // Java: 101
        public void createProfile()
        {
            string nick = editline_nick.getContents();
            gui_root.addModalForm(new com.oddlabs.tt.form.CreatingProfileForm(gui_root, profiles_form, main_menu, nick));
        }
        // Java: 106
        public void connectionLost()
        {
            remove();
        }
        // Java: 110
        public class CreateProfileListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.NewProfileForm this_0;
            // Java: 111
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.createProfile();
            }
            // Java: 116
            public void enterPressed(string text)
            {
                this_0.createProfile();
            }
            public CreateProfileListener(com.oddlabs.tt.form.NewProfileForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static NewProfileForm()
        {
            NewProfileForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.NewProfileForm).getName());
        }
        public NewProfileForm(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu, com.oddlabs.tt.form.ProfilesForm profiles_form) : base()
        {
            this.gui_root = gui_root;
            this.main_menu = main_menu;
            this.profiles_form = profiles_form;
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("create_profile_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.Label label_nick = new com.oddlabs.tt.gui.Label(i18n("nick"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_nick = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.NewProfileForm.EDITLINE_WIDTH, 255);
            editline_nick.addEnterListener(new com.oddlabs.tt.form.NewProfileForm.CreateProfileListener(this));
            addChild(label_nick);
            addChild(editline_nick);
            label_headline.place();
            label_nick.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            editline_nick.place(label_nick, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Group group_buttons = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.ButtonObject button_create = new com.oddlabs.tt.gui.HorizButton(i18n("create_profile"), com.oddlabs.tt.form.NewProfileForm.BUTTON_WIDTH_LONG);
            button_create.addMouseClickListener(new com.oddlabs.tt.form.NewProfileForm.CreateProfileListener(this));
            com.oddlabs.tt.gui.ButtonObject button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.NewProfileForm.BUTTON_WIDTH);
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
            group_buttons.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
        }
    }
}
