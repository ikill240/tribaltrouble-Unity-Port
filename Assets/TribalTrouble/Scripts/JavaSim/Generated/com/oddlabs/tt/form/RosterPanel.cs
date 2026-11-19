// Translated from RosterPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 20
    public class RosterPanel : com.oddlabs.tt.gui.Panel
    {
        public static java.util.ResourceBundle bundle;
        public com.oddlabs.tt.gui.ScrollableGroup roster;
        public com.oddlabs.tt.gui.GUIObject header;
        // Java: 45
        public void setHeader(com.oddlabs.tt.gui.GUIObject header)
        {
            this.header = header;
        }
        // Java: 49
        public void setRoster(com.oddlabs.tt.gui.ScrollableGroup new_roster)
        {
            if (roster != null)
            {
                removeChild(roster);
            }
            roster = new_roster;
            addChild(new_roster);
            if (header != null)
            {
                new_roster.place(header, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            }
            else
            {
                new_roster.place();
            }
            compileCanvas();
        }
        // Java: 63
        public static string i18n(string key)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.RosterPanel.bundle, key);
        }
        static RosterPanel()
        {
            RosterPanel.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.RosterPanel).getName());
        }
        public RosterPanel() : base(com.oddlabs.tt.form.RosterPanel.i18n("caption"))
        {
            compileCanvas();
        }
    }
}
