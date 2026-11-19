// Translated from QuitForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 14
    public class QuitForm : com.oddlabs.tt.form.QuestionForm
    {
        public static java.util.ResourceBundle bundle;
        // Java: 17
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.QuitForm.bundle, key, args);
        }
        // Java: 26
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_QUIT))
                {
                    com.oddlabs.tt.render.Renderer.shutdown();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        static QuitForm()
        {
            QuitForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.QuitForm).getName());
        }
        public QuitForm(com.oddlabs.tt.gui.GUIRoot gui_root) : base(com.oddlabs.tt.form.QuitForm.i18n((com.oddlabs.tt.net.PeerHub.isWaitingForAck() ? "confirm_quit_waiting_for_ack" : "confirm_quit")), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
        {
            com.oddlabs.tt.render.Renderer.shutdown();
        }))
        {
        }
    }
}
