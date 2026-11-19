// Translated from WelcomeForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 20
    public class WelcomeForm : com.oddlabs.tt.gui.Form
    {
        public static java.util.ResourceBundle bundle;
        // Java: 23
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.WelcomeForm.bundle, key, args);
        }
        static WelcomeForm()
        {
            WelcomeForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.WelcomeForm).getName());
        }
        public WelcomeForm(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.MainMenu main_menu) : base()
        {
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("welcome_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.LabelBox box = new com.oddlabs.tt.gui.LabelBox(i18n("welcome_message"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 400);
            addChild(box);
            com.oddlabs.tt.gui.HorizButton ok_button = new com.oddlabs.tt.gui.OKButton(100);
            addChild(ok_button);
            ok_button.addMouseClickListener(new com.oddlabs.tt.gui.OKListener(this));
            label_headline.place();
            box.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            ok_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            centerPos();
        }
    }
}
