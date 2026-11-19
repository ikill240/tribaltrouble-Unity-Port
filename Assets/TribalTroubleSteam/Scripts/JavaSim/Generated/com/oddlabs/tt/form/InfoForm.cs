// Translated from InfoForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 21
    public class InfoForm : com.oddlabs.tt.gui.Form
    {
        public static java.util.ResourceBundle bundle;
        // Java: 24
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.InfoForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.HorizButton ok_button;
        // Java: 92
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                ok_button.setFocus(direction);
            }
        }
        static InfoForm()
        {
            InfoForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.InfoForm).getName());
        }
        public InfoForm(com.oddlabs.matchmaking.Profile profile) : base()
        {
            string profile_str = i18n("profile");
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(profile_str, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.Group types = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Group values = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_name = new com.oddlabs.tt.gui.Label(i18n("name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_name_value = new com.oddlabs.tt.gui.Label(profile.getNick(), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_name);
            values.addChild(label_name_value);
            com.oddlabs.tt.gui.Label label_rating = new com.oddlabs.tt.gui.Label(i18n("rating"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_rating_value = new com.oddlabs.tt.gui.Label("" + (profile.getRating()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_rating);
            values.addChild(label_rating_value);
            com.oddlabs.tt.gui.Label label_wins = new com.oddlabs.tt.gui.Label(i18n("wins"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_wins_value = new com.oddlabs.tt.gui.Label("" + (profile.getWins()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_wins);
            values.addChild(label_wins_value);
            com.oddlabs.tt.gui.Label label_losses = new com.oddlabs.tt.gui.Label(i18n("losses"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_losses_value = new com.oddlabs.tt.gui.Label("" + (profile.getLosses()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_losses);
            values.addChild(label_losses_value);
            com.oddlabs.tt.gui.Label label_invalid = new com.oddlabs.tt.gui.Label(i18n("invalid"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_invalid_value = new com.oddlabs.tt.gui.Label("" + (profile.getInvalid()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            types.addChild(label_invalid);
            values.addChild(label_invalid_value);
            label_name.place();
            label_rating.place(label_name, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_wins.place(label_rating, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_losses.place(label_wins, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_invalid.place(label_losses, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            types.compileCanvas();
            addChild(types);
            label_name_value.place();
            label_rating_value.place(label_name_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_wins_value.place(label_rating_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_losses_value.place(label_wins_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_invalid_value.place(label_losses_value, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            values.compileCanvas();
            addChild(values);
            ok_button = new com.oddlabs.tt.gui.OKButton(100);
            addChild(ok_button);
            ok_button.addMouseClickListener(new com.oddlabs.tt.gui.OKListener(this));
            label_headline.place();
            types.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            values.place(types, com.oddlabs.tt.gui.Placement.RIGHT_TOP);
            ok_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            centerPos();
        }
    }
}
