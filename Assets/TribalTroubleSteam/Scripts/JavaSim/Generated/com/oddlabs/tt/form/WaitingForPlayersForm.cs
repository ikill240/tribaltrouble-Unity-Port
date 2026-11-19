// Translated from WaitingForPlayersForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 19
    public class WaitingForPlayersForm : com.oddlabs.tt.gui.Form
    {
        public static java.util.ResourceBundle bundle;
        // Java: 22
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.WaitingForPlayersForm.bundle, key, args);
        }
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        // Java: 42
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
            {
                return;
            }
            base.handleInput(@event);
        }
        // Java: 52
        public override void doCancel()
        {
            viewer.close();
        }
        // Java: 57
        public class AbortListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.WaitingForPlayersForm this_0;
            // Java: 58
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.viewer.getGUIRoot().addModalForm(new com.oddlabs.tt.form.QuestionForm(this_0.i18n("confirm_abort"), new com.oddlabs.tt.gui.CancelListener(this.this_0)));
            }
            public AbortListener(com.oddlabs.tt.form.WaitingForPlayersForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static WaitingForPlayersForm()
        {
            WaitingForPlayersForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.WaitingForPlayersForm).getName());
        }
        public WaitingForPlayersForm(com.oddlabs.tt.viewer.WorldViewer viewer) : base()
        {
            this.viewer = viewer;
            com.oddlabs.tt.gui.Label info_label = new com.oddlabs.tt.gui.Label(i18n("waiting"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            info_label.setDim(280, info_label.getHeight());
            com.oddlabs.tt.gui.HorizButton abort_button = new com.oddlabs.tt.gui.HorizButton(i18n("abort"), 120);
            abort_button.addMouseClickListener(new com.oddlabs.tt.form.WaitingForPlayersForm.AbortListener(this));
            addChild(info_label);
            addChild(abort_button);
            info_label.place();
            abort_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            centerPos();
        }
    }
}
