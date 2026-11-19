// Translated from WarningForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 19
    public class WarningForm : com.oddlabs.tt.gui.Form
    {
        public const int MAX_WIDTH = 500;
        public static java.util.ResourceBundle bundle;
        // Java: 23
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.WarningForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.CheckBox show_next_time;
        static WarningForm()
        {
            WarningForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.WarningForm).getName());
        }
        public WarningForm(string head, string message) : base()
        {
            int head_width = java.lang.Math.min(com.oddlabs.tt.form.WarningForm.MAX_WIDTH, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont().getWidth(head));
            int message_width = java.lang.Math.min(com.oddlabs.tt.form.WarningForm.MAX_WIDTH, com.oddlabs.tt.gui.Skin.getSkin().getEditFont().getWidth(message));
            int width = java.lang.Math.max(head_width, message_width);
            com.oddlabs.tt.gui.Group group = new com.oddlabs.tt.gui.Group();
            addChild(group);
            com.oddlabs.tt.gui.LabelBox head_label = new com.oddlabs.tt.gui.LabelBox(head, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), width);
            group.addChild(head_label);
            com.oddlabs.tt.gui.LabelBox info_label = new com.oddlabs.tt.gui.LabelBox(message, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), width);
            group.addChild(info_label);
            show_next_time = new com.oddlabs.tt.gui.CheckBox(false, i18n("dont_show"));
            group.addChild(show_next_time);
            head_label.place();
            info_label.place(head_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            show_next_time.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group.compileCanvas();
            com.oddlabs.tt.gui.HorizButton ok_button = new com.oddlabs.tt.gui.OKButton(70);
            addChild(ok_button);
            ok_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().warning_no_sound = !show_next_time.isMarked();
                remove();
            }));
            group.place();
            ok_button.place(group, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            compileCanvas();
            centerPos();
        }
    }
}
