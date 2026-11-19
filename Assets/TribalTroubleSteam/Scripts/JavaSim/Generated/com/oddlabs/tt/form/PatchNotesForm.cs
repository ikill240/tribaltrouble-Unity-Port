// Translated from PatchNotesForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 24
    public class PatchNotesForm : com.oddlabs.tt.gui.Form
    {
        public static java.util.ResourceBundle bundle;
        public const string CHANGELOG_RESOURCE = "CHANGELOG.md";
        public const string UNRELEASED_HEADING = "## [Unreleased]";
        public const string VERSION_HEADING_PREFIX = "## ";
        public const string SECTION_HEADING_PREFIX = "### ";
        public const int BOX_WIDTH = 500;
        public const int BOX_HEIGHT = 350;
        public const int MAX_CHARS = 200000;
        public const int BUTTON_WIDTH = 160;
        // Java: 35
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.PatchNotesForm.bundle, key, args);
        }
        // Java: 62
        public void close(bool show_on_update)
        {
            com.oddlabs.tt.global.Settings.getSettings().show_patch_notes_on_update = show_on_update;
            com.oddlabs.tt.global.Settings.getSettings().save();
            cancel();
        }
        // Java: 68
        public static string loadNotes()
        {
            {
                java.io.InputStream stream = typeof(com.oddlabs.tt.form.PatchNotesForm).getResourceAsStream(com.oddlabs.tt.form.PatchNotesForm.CHANGELOG_RESOURCE);
                try
                {
                    if (stream == null)
                    {
                        return com.oddlabs.tt.form.PatchNotesForm.i18n("unavailable");
                    }
                    return com.oddlabs.tt.form.PatchNotesForm.toPlainText(java.lang.JString.create(stream.readAllBytes(), java.nio.charset.StandardCharsets.UTF_8));
                }
                catch (System.IO.IOException e)
                {
                    return com.oddlabs.tt.form.PatchNotesForm.i18n("unavailable");
                }
                finally
                {
                    if (stream != null) stream.close();
                }
            }
        }
        // Java: 80
        public static string toPlainText(string markdown)
        {
            java.lang.StringBuilder text = new java.lang.StringBuilder();
            bool in_versions = false;
            foreach (string raw_line in markdown.split("\n"))
            {
                string line = raw_line.stripTrailing();
                if (line.startsWith(com.oddlabs.tt.form.PatchNotesForm.VERSION_HEADING_PREFIX))
                {
                    in_versions = true;
                }
                if (!in_versions || com.oddlabs.tt.form.PatchNotesForm.isLinkReference(line))
                {
                    continue;
                }
                if (line.startsWith(com.oddlabs.tt.form.PatchNotesForm.UNRELEASED_HEADING))
                {
                    line = java.lang.JStr.valueOf(com.oddlabs.tt.form.PatchNotesForm.VERSION_HEADING_PREFIX) + java.lang.JStr.valueOf(com.oddlabs.util.BuildInfo.FULL_VERSION);
                }
                if (line.startsWith(com.oddlabs.tt.form.PatchNotesForm.SECTION_HEADING_PREFIX))
                {
                    text.append(line.substring(com.oddlabs.tt.form.PatchNotesForm.SECTION_HEADING_PREFIX.length()).toUpperCase(java.util.Locale.ROOT));
                }
                else if (line.startsWith(com.oddlabs.tt.form.PatchNotesForm.VERSION_HEADING_PREFIX))
                {
                    text.append(line.substring(com.oddlabs.tt.form.PatchNotesForm.VERSION_HEADING_PREFIX.length()).replace("[", "").replace("]", ""));
                }
                else
                {
                    text.append(line);
                }
                text.append('\n');
            }
            return java.lang.JO.toString(text).strip();
        }
        // Java: 106
        public static bool isLinkReference(string line)
        {
            return line.startsWith("[") && line.contains("]: ");
        }
        static PatchNotesForm()
        {
            PatchNotesForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.PatchNotesForm).getName());
        }
        public PatchNotesForm() : base()
        {
            com.oddlabs.tt.gui.Label head_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.PatchNotesForm.i18n("patch_notes"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(head_label);
            head_label.place();
            com.oddlabs.tt.gui.TextBox notes_box = new com.oddlabs.tt.gui.TextBox(com.oddlabs.tt.form.PatchNotesForm.BOX_WIDTH, com.oddlabs.tt.form.PatchNotesForm.BOX_HEIGHT, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.PatchNotesForm.MAX_CHARS);
            addChild(notes_box);
            notes_box.append(com.oddlabs.tt.form.PatchNotesForm.loadNotes());
            notes_box.place(head_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton ok_button = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.PatchNotesForm.BUTTON_WIDTH);
            addChild(ok_button);
            ok_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                close(true);
            }));
            ok_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            com.oddlabs.tt.gui.HorizButton dont_show_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.PatchNotesForm.i18n("dont_show_again"), com.oddlabs.tt.form.PatchNotesForm.BUTTON_WIDTH);
            addChild(dont_show_button);
            dont_show_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                close(false);
            }));
            dont_show_button.place(ok_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
        }
    }
}
