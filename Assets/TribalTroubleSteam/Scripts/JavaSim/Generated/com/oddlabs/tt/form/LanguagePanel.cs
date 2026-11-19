// Translated from LanguagePanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 24
    public class LanguagePanel : com.oddlabs.tt.gui.Panel
    {
        public LanguagePanel(com.oddlabs.tt.gui.GUIRoot gui_root) : base(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("language_caption"))
        {
            com.oddlabs.tt.gui.Group language_group = new com.oddlabs.tt.gui.Group();
            addChild(language_group);
            com.oddlabs.tt.gui.Label language_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("language_label"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            language_group.addChild(language_label);
            com.oddlabs.tt.gui.ColumnInfo[] language_infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo("", 300) };
            com.oddlabs.tt.gui.MultiColumnComboBox language_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, language_infos, 200, false);
            bool languageFound = false;
            if (!java.lang.JO.equals(com.oddlabs.tt.global.Settings.getSettings().language, "default"))
            {
                foreach (string[] lang in com.oddlabs.tt.gui.Languages.getLanguages())
                {
                    if (java.lang.JO.equals(com.oddlabs.tt.global.Settings.getSettings().language, lang[0]))
                    {
                        languageFound = true;
                        break;
                    }
                }
                if (!languageFound)
                {
                    com.oddlabs.tt.global.Settings.getSettings().language = "default";
                }
            }
            com.oddlabs.tt.gui.Row selectedLanguage = null;
            string[][] languages = com.oddlabs.tt.gui.Languages.getLanguages();
            com.oddlabs.tt.gui.IconQuad[] flags = com.oddlabs.tt.gui.Languages.getFlags();
            for (int i = 0; i < languages.Length; i++)
            {
                com.oddlabs.tt.gui.IconLabel label = new com.oddlabs.tt.gui.IconLabel(flags[i], new com.oddlabs.tt.gui.Label(languages[i][1], com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()));
                com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(new com.oddlabs.tt.gui.IconLabel[] { label }, java.util.Locale.of(languages[i][0]));
                language_list_box.addRow(row);
                if (java.lang.JO.equals(languages[i][0], com.oddlabs.tt.global.Settings.getSettings().language))
                {
                    selectedLanguage = row;
                }
            }
            com.oddlabs.tt.gui.IconLabel label_2 = new com.oddlabs.tt.gui.IconLabel(com.oddlabs.tt.gui.Skin.getSkin().getFlagDefault(), new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("system_default"), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font()));
            com.oddlabs.tt.gui.Row row_2 = new com.oddlabs.tt.gui.Row(java.util.List_.of(label_2), com.oddlabs.tt.render.Renderer.getRenderer().getDefaultLocale());
            language_list_box.addRow(row_2);
            if (java.lang.JO.equals(com.oddlabs.tt.global.Settings.getSettings().language, "default"))
            {
                selectedLanguage = row_2;
            }
            language_list_box.selectRow(selectedLanguage);
            language_list_box.addRowListener(new LanguagePanel.Anon1_RowListener(this, gui_root));
            language_group.addChild(language_list_box);
            language_label.place();
            language_list_box.place(language_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            language_group.compileCanvas();
            language_group.place();
            compileCanvas();
        }
        // Java: anonymous class at line 70
        public sealed class Anon1_RowListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.form.LanguagePanel this_0;
            public com.oddlabs.tt.gui.GUIRoot gui_root;
            public Anon1_RowListener(com.oddlabs.tt.form.LanguagePanel this_0_, com.oddlabs.tt.gui.GUIRoot gui_root_c)
            {
                this.this_0 = this_0_;
                this.gui_root = gui_root_c;
            }
            // Java: 71
            public void rowDoubleClicked(object locale_p)
            {
                java.util.Locale locale = (java.util.Locale)locale_p;
                com.oddlabs.tt.global.Settings.getSettings().language = (java.lang.JO.equals(locale.getVariant(), "default") ? "default" : locale.getLanguage());
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("language_change_next_run")));
            }
            // mixin of default method RowListener.rowChosen
            public void rowChosen(object row_context)
            {
            }
        }
    }
}
