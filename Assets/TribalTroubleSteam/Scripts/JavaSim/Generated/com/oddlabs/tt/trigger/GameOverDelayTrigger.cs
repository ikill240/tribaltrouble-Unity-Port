// Translated from GameOverDelayTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger
{
    // Java: 10
    public class GameOverDelayTrigger : java.lang.Object, com.oddlabs.tt.animation.Updatable
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.camera.Camera camera;
        public string label_str;
        // Java: 24
        public void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            anim.stop();
            viewer.getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.GameStatsDelegate(viewer, camera, label_str));
        }
        public GameOverDelayTrigger(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.Camera camera, string label_str) : base()
        {
            this.viewer = viewer;
            this.camera = camera;
            this.label_str = label_str;
            com.oddlabs.tt.animation.TimerAnimation delay_timer = new com.oddlabs.tt.animation.TimerAnimation(viewer.getWorld().getAnimationManagerRealTime(), this, 1.5f);
            delay_timer.start();
        }
    }
}
