// Translated from Notification.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 14
    public class Notification : java.lang.Object, com.oddlabs.tt.animation.Updatable
    {
        public const float ACTIVE_SECONDS = 5.0f;
        public float center_x;
        public float center_y;
        public com.oddlabs.tt.viewer.NotificationManager manager;
        public com.oddlabs.tt.animation.TimerAnimation timer;
        public com.oddlabs.tt.gui.Arrow arrow;
        // Java: 38
        public virtual void remove()
        {
            arrow.remove();
            timer.stop();
        }
        // Java: 43
        public virtual void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            remove();
            manager.removeNotification(this);
        }
        // Java: 49
        public com.oddlabs.tt.gui.Arrow getArrow()
        {
            return arrow;
        }
        // Java: 53
        public com.oddlabs.tt.animation.TimerAnimation getTimer()
        {
            return timer;
        }
        // Java: 57
        public com.oddlabs.tt.viewer.NotificationManager getManager()
        {
            return manager;
        }
        // Java: 61
        public float getX()
        {
            return center_x;
        }
        // Java: 65
        public float getY()
        {
            return center_y;
        }
        public Notification(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.gui.GUIRoot gui_root, float x, float y, com.oddlabs.tt.viewer.NotificationManager manager, float r, float g, float b, com.oddlabs.tt.audio.Audio sound, bool show_always, com.oddlabs.tt.animation.AnimationManager animation_manager) : base()
        {
            this.center_x = x;
            this.center_y = y;
            this.manager = manager;
            this.timer = new com.oddlabs.tt.animation.TimerAnimation(animation_manager, this, com.oddlabs.tt.viewer.Notification.ACTIVE_SECONDS);
            timer.start();
            this.arrow = new com.oddlabs.tt.gui.Arrow(world.getHeightMap(), gui_root, center_x, center_y, r, g, b, show_always);
            gui_root.addChild(arrow);
            world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(sound, 0.0f, 0.0f, 0.0f, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_NOTIFICATION, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_NOTIFICATION, 0.25f, 1.0f, 1.0f, false, true));
        }
    }
}
