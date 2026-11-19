// Translated from DisplayChangeForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 17
    public class DisplayChangeForm : com.oddlabs.tt.gui.Form
    {
        public static java.util.ResourceBundle bundle;
        // Java: 20
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.DisplayChangeForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.DoNowListener donow_listener;
        public com.oddlabs.tt.gui.HorizButton later_button;
        // Java: 50
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                later_button.setFocus(direction);
            }
        }
        // Java: 59
        public override void doCancel()
        {
            donow_listener.doChange(false);
        }
        static DisplayChangeForm()
        {
            DisplayChangeForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.DisplayChangeForm).getName());
        }
        public DisplayChangeForm(com.oddlabs.tt.gui.DoNowListener donow_listener) : base()
        {
            this.donow_listener = donow_listener;
            com.oddlabs.tt.gui.LabelBox info_label = new com.oddlabs.tt.gui.LabelBox(i18n("warning_message"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 500);
            addChild(info_label);
            com.oddlabs.tt.gui.HorizButton now_button = new com.oddlabs.tt.gui.HorizButton(i18n("now"), 120);
            addChild(now_button);
            now_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                remove();
                donow_listener.doChange(true);
            }));
            later_button = new com.oddlabs.tt.gui.HorizButton(i18n("later"), 120);
            addChild(later_button);
            later_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                this.cancel();
            }));
            info_label.place();
            now_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            later_button.place(now_button, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
        }
    }
}
