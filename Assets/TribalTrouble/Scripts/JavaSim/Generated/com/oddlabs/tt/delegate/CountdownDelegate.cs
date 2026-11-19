// Translated from CountdownDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 22
    public class CountdownDelegate : com.oddlabs.tt.@delegate.CameraDelegate
    {
        public static java.util.ResourceBundle bundle;
        public static org.joml.Vector4fc OVERLAY_COLOR;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.Label countdown_label;
        public com.oddlabs.tt.gui.Label waiting_label;
        public com.oddlabs.tt.animation.TimerAnimation timer_animation;
        public int countdown_time;
        public bool counting_down;
        // Java: 51
        public void tick(com.oddlabs.tt.animation.TimerAnimation anim)
        {
            if (!allPlayersReady())
            {
                timer_animation.start();
                return;
            }
            if (!counting_down)
            {
                counting_down = true;
                waiting_label.remove();
                countdown_label.set("3");
                timer_animation.start();
                return;
            }
            countdown_time--;
            if (countdown_time < 0)
            {
                timer_animation.stop();
                pop();
                com.oddlabs.tt.@delegate.Menu.completeGameSetupHack(viewer);
            }
            else if (countdown_time == 0)
            {
                countdown_label.set(com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.CountdownDelegate.bundle, "fight"));
                timer_animation.start();
            }
            else
            {
                countdown_label.set(java.lang.JString.valueOf(countdown_time));
                timer_animation.start();
            }
        }
        // Java: 77
        public override void displayChangedNotify(int width, int height)
        {
            base.displayChangedNotify(width, height);
            countdown_label.setPos((width - countdown_label.getWidth()) / 2, (height - countdown_label.getHeight()) / 2);
            waiting_label.setPos((width - waiting_label.getWidth()) / 2, (height - waiting_label.getHeight()) / 2);
        }
        // Java: 84
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), com.oddlabs.tt.@delegate.CountdownDelegate.OVERLAY_COLOR);
        }
        // Java: 89
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            @event.consume();
        }
        // Java: 95
        public bool allPlayersReady()
        {
            return viewer.getPeerHub() != null && viewer.getPeerHub().isSynchronized();
        }
        static CountdownDelegate()
        {
            CountdownDelegate.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.CountdownDelegate).getName());
            CountdownDelegate.OVERLAY_COLOR = new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.5f);
        }
        private void init_CountdownDelegate()
        {
            this.countdown_time = 3;
        }
        public CountdownDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.Camera camera) : base(viewer.getGUIRoot(), camera)
        {
            init_CountdownDelegate();
            this.viewer = viewer;
            string waitingText = com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.CountdownDelegate.bundle, "waiting");
            waiting_label = new com.oddlabs.tt.gui.Label(waitingText, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont().getWidth(waitingText), com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            addChild(waiting_label);
            string fightText = com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.CountdownDelegate.bundle, "fight");
            countdown_label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont().getWidth(fightText), com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            addChild(countdown_label);
            timer_animation = new com.oddlabs.tt.animation.TimerAnimation(new com.oddlabs.tt.animation.Updatable_L((object a0) => { this.tick(((com.oddlabs.tt.animation.TimerAnimation)a0)); }), 1.0f);
            timer_animation.start();
        }
    }
}
