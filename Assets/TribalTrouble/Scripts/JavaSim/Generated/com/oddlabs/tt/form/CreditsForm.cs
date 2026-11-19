// Translated from CreditsForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 21
    public class CreditsForm : com.oddlabs.tt.gui.Form
    {
        public static java.util.ResourceBundle bundle;
        // Java: 24
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.CreditsForm.bundle, key, args);
        }
        // Java: 46
        public static com.oddlabs.tt.gui.Panel createAboutPanel(java.util.ResourceBundle bundle)
        {
            com.oddlabs.tt.gui.Panel about = new com.oddlabs.tt.gui.Panel(com.oddlabs.tt.form.CreditsForm.i18n("about"));
            com.oddlabs.tt.gui.TextBox about_box = new com.oddlabs.tt.gui.TextBox(400, 300, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 100000);
            about.addChild(about_box);
            about_box.append(java.lang.JStr.valueOf(com.oddlabs.util.BuildInfo.FULL_VERSION) + "\n\n");
            string about_text = com.oddlabs.tt.form.CreditsForm.i18n("about_text", java.lang.Integer.toString(com.oddlabs.tt.render.Renderer.getLocalInput().getRevision()));
            about_box.append(about_text);
            about_box.place();
            about.compileCanvas();
            return about;
        }
        // Java: 59
        public static com.oddlabs.tt.gui.Panel createCreditsPanel(java.util.ResourceBundle bundle)
        {
            com.oddlabs.tt.gui.Panel credits = new com.oddlabs.tt.gui.Panel(com.oddlabs.tt.form.CreditsForm.i18n("credits"));
            com.oddlabs.tt.gui.TextBox credits_box = new com.oddlabs.tt.gui.TextBox(400, 300, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 100000);
            credits.addChild(credits_box);
            credits_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("game_design_and_programming")) + "\n");
            credits_box.append("Elias Naur\n");
            credits_box.append("Mikkel Jensen\n");
            credits_box.append("Sune Nielsen\n");
            credits_box.append("Jacob Olsen\n");
            credits_box.append("\n");
            credits_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("3d_artwork_and_animation")) + "\n");
            credits_box.append("Chaz Willets\n");
            credits_box.append("\n");
            credits_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("audio")) + "\n");
            credits_box.append("Michael Huang\n");
            credits_box.append("Nicklas Schmidt\n");
            credits_box.append("Herman Witkam");
            credits_box.place();
            credits.compileCanvas();
            return credits;
        }
        // Java: 81
        public static com.oddlabs.tt.gui.Panel createThanksPanel(java.util.ResourceBundle bundle)
        {
            com.oddlabs.tt.gui.Panel thanks = new com.oddlabs.tt.gui.Panel(com.oddlabs.tt.form.CreditsForm.i18n("thanks_to"));
            com.oddlabs.tt.gui.TextBox thanks_box = new com.oddlabs.tt.gui.TextBox(400, 300, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 100000);
            thanks.addChild(thanks_box);
            thanks_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("thanks")) + "\n");
            thanks_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("oddlabs_thanks")) + "\n");
            thanks_box.append("\n");
            thanks_box.append("Caspian Rychlik-Prince\n");
            thanks_box.append("Brian Matzon\n");
            thanks_box.append("The LWJGL team\n");
            thanks_box.append("Johannes Sebastian J\u00f8rgen Erik M\u00f8gelvang\n");
            thanks_box.append("Martin B. K. Nielsen\n");
            thanks_box.append("Martin Vestergaard Madsen\n");
            thanks_box.append("Christian Mosb\u00e6k\n");
            thanks_box.append("Camilla Dahle\n");
            thanks_box.append("\n");
            thanks_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("beta_thank")) + "\n");
            thanks_box.append("\n");
            thanks_box.append("Patric Schenke\n");
            thanks_box.append("Oliver Hutt\n");
            thanks_box.append("Mathew Foscarini\n");
            thanks_box.append("Scott Call\n");
            thanks_box.append("Gheorghe Costin\n");
            thanks_box.append("Eric Spierings\n");
            thanks_box.append("Tomasz Malecki\n");
            thanks_box.append("Benjamin Dirks\n");
            thanks_box.append("Paulo Augusto\n");
            thanks_box.append("Carlos Rayon\n");
            thanks_box.append("Robert Speelpenning\n");
            thanks_box.append("Matt Chelen\n");
            thanks_box.append("Paul van Schayck\n");
            thanks_box.append("Binks\n");
            thanks_box.append("S\u00f8ren Vesti Lassen\n");
            thanks_box.append("Martyn Lewis\n");
            thanks_box.append("Benjamin Dirks\n");
            thanks_box.append("Tonny L. Rasmussen\n");
            thanks_box.append("Bj\u00f8rn Andersen\n");
            thanks_box.append("\n");
            thanks_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("additional_thanks")) + "\n");
            thanks_box.append("\n");
            thanks_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("special_thanks")) + "\n");
            thanks_box.append(java.lang.JStr.valueOf(com.oddlabs.tt.form.CreditsForm.i18n("special_thanks_text")) + "\n");
            thanks_box.append("\n");
            thanks_box.append("Niels Haulrich AKA \"DJ Fnillerz\"\n");
            thanks_box.append("Morten M. Hansen AKA \"blue\"\n");
            thanks_box.append("S\u00f8ren Garb\u00f8l AKA \"Mythor\"\n");
            thanks_box.append("Andreas Beier AKA \"AB\"\n");
            thanks_box.append("Jesper S. Pedersen AKA \"fronk\"\n");
            thanks_box.append("Kxre Schou Gjaldbxk AKA \"hardkxre\"");
            thanks_box.place();
            thanks.compileCanvas();
            return thanks;
        }
        static CreditsForm()
        {
            CreditsForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.CreditsForm).getName());
        }
        public CreditsForm() : base()
        {
            com.oddlabs.tt.gui.Label head_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.CreditsForm.i18n("about"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(head_label);
            head_label.place();
            com.oddlabs.tt.gui.PanelGroup panel_group = new com.oddlabs.tt.gui.PanelGroup(com.oddlabs.tt.form.CreditsForm.createAboutPanel(com.oddlabs.tt.form.CreditsForm.bundle), com.oddlabs.tt.form.CreditsForm.createCreditsPanel(com.oddlabs.tt.form.CreditsForm.bundle), com.oddlabs.tt.form.CreditsForm.createThanksPanel(com.oddlabs.tt.form.CreditsForm.bundle));
            addChild(panel_group);
            panel_group.place(head_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton ok_button = new com.oddlabs.tt.gui.OKButton(100);
            addChild(ok_button);
            ok_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            ok_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            centerPos();
        }
    }
}
