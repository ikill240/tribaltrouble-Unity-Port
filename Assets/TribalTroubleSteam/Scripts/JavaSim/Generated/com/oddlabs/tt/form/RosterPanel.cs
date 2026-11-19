// Translated from RosterPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 16
    public class RosterPanel : com.oddlabs.tt.gui.Panel
    {
        public static java.util.ResourceBundle bundle;
        public com.oddlabs.tt.gui.ScrollableGroup roster;
        // Java: 26
        public void setRoster(com.oddlabs.tt.gui.ScrollableGroup new_roster)
        {
            if (roster != null)
            {
                removeChild(roster);
            }
            roster = new_roster;
            addChild(new_roster);
            new_roster.place();
            compileCanvas();
        }
        // Java: 36
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
