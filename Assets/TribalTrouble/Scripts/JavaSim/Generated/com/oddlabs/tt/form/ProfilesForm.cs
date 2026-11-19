// Translated from ProfilesForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 29
    public class ProfilesForm : com.oddlabs.tt.gui.Form
    {
        public const int NICK_SIZE = 200;
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.form.SelectGameMenu game_menu;
        public com.oddlabs.tt.gui.MultiColumnComboBox profile_list_box;
        public com.oddlabs.tt.gui.HorizButton join_button;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public static java.util.ResourceBundle bundle;
        // Java: 39
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.ProfilesForm.bundle, key, args);
        }
        public com.oddlabs.tt.form.NewProfileForm new_profile_form;
        public com.oddlabs.tt.form.QuestionForm confirm_delete_form;
        // Java: 102
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                join_button.setFocus(direction);
            }
        }
        // Java: 111
        public override void doCancel()
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().close();
        }
        // Java: 116
        public void receivedProfiles(com.oddlabs.matchmaking.Profile[] profiles, string last_nick)
        {
            profile_list_box.clear();
            com.oddlabs.tt.gui.Row selected_row = null;
            foreach (com.oddlabs.matchmaking.Profile p in profiles)
            {
                com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.Label(p.getNick(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.form.ProfilesForm.NICK_SIZE), new com.oddlabs.tt.gui.IntegerLabel(p.getRating(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()), new com.oddlabs.tt.gui.IntegerLabel(p.getWins(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()), new com.oddlabs.tt.gui.IntegerLabel(p.getLosses(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()), new com.oddlabs.tt.gui.IntegerLabel(p.getInvalid(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font())), p.getNick());
                profile_list_box.addRow(row);
                if (p.getNick().equalsIgnoreCase(last_nick))
                {
                    selected_row = row;
                }
            }
            if (selected_row != null)
            {
                profile_list_box.selectRow(selected_row);
            }
        }
        // Java: 134
        public void join(string nick)
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().setProfile(nick);
            main_menu.setMenuCentered(game_menu);
        }
        // Java: 139
        public class DeleteProfileListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.ProfilesForm this_0;
            // Java: 140
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                string nick = ((string)this_0.profile_list_box.getSelected());
                if (nick == null)
                {
                    this_0.gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(this_0.i18n("no_profiles")));
                }
                else
                {
                    string confirm_str = this_0.i18n("confirm_delete", nick);
                    this_0.confirm_delete_form = new com.oddlabs.tt.form.QuestionForm(confirm_str, new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                    {
                        com.oddlabs.tt.net.Network.getMatchmakingClient().deleteProfile(nick);
                        com.oddlabs.tt.net.Network.getMatchmakingClient().requestProfiles();
                    }));
                    this_0.gui_root.addModalForm(this_0.confirm_delete_form);
                }
            }
            public DeleteProfileListener(com.oddlabs.tt.form.ProfilesForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 156
        public void connectionLost()
        {
            if (new_profile_form != null)
            {
                new_profile_form.connectionLost();
            }
            if (confirm_delete_form != null)
            {
                confirm_delete_form.connectionLost();
            }
            remove();
        }
        static ProfilesForm()
        {
            ProfilesForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.ProfilesForm).getName());
        }
        public ProfilesForm(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu, com.oddlabs.tt.form.SelectGameMenu game_menu) : base()
        {
            this.gui_root = gui_root;
            this.main_menu = main_menu;
            this.game_menu = game_menu;
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("profiles_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.ColumnInfo[] infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(i18n("nick"), com.oddlabs.tt.form.ProfilesForm.NICK_SIZE), new com.oddlabs.tt.gui.ColumnInfo(i18n("rating"), 120), new com.oddlabs.tt.gui.ColumnInfo(i18n("wins"), 100), new com.oddlabs.tt.gui.ColumnInfo(i18n("losses"), 100), new com.oddlabs.tt.gui.ColumnInfo(i18n("invalid"), 100) };
            profile_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, infos, 200);
            profile_list_box.addRowListener(new ProfilesForm.Anon1_RowListener(this));
            addChild(profile_list_box);
            com.oddlabs.tt.gui.HorizButton create_profile_button = new com.oddlabs.tt.gui.HorizButton(i18n("create_new_profile"), 150);
            create_profile_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                new_profile_form = new com.oddlabs.tt.form.NewProfileForm(gui_root, main_menu, this);
                main_menu.setMenu(new_profile_form);
            }));
            addChild(create_profile_button);
            com.oddlabs.tt.gui.HorizButton delete_profile_button = new com.oddlabs.tt.gui.HorizButton(i18n("delete_profile"), 150);
            delete_profile_button.addMouseClickListener(new com.oddlabs.tt.form.ProfilesForm.DeleteProfileListener(this));
            addChild(delete_profile_button);
            join_button = new com.oddlabs.tt.gui.HorizButton(i18n("join"), 100);
            join_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                string nick = ((string)profile_list_box.getSelected());
                if (nick == null)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("no_profiles")));
                }
                else
                {
                    join(nick);
                }
            }));
            addChild(join_button);
            com.oddlabs.tt.gui.HorizButton logout_button = new com.oddlabs.tt.gui.HorizButton(i18n("logout"), 100);
            logout_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                this.cancel();
            }));
            addChild(logout_button);
            label_headline.place();
            profile_list_box.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            logout_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            join_button.place(logout_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            delete_profile_button.place(join_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            create_profile_button.place(delete_profile_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
        }
        // Java: anonymous class at line 58
        public sealed class Anon1_RowListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.form.ProfilesForm this_0;
            public Anon1_RowListener(com.oddlabs.tt.form.ProfilesForm this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 59
            public void rowDoubleClicked(object nick_p)
            {
                string nick = (string)nick_p;
                this_0.join(nick);
            }
            // mixin of default method RowListener.rowChosen
            public void rowChosen(object row_context)
            {
            }
        }
    }
}
