// Translated from AttackNotification.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 11
    public class AttackNotification : com.oddlabs.tt.viewer.Notification
    {
        public const float RADIUS = 30.0f;
        public const float FADE_OUT = 5.0f;
        public bool active;
        // Java: 22
        public bool contains(com.oddlabs.tt.util.Target target)
        {
            float dx = (float)(getX() - target.getPositionX());
            float dy = (float)(getY() - target.getPositionY());
            float dist = (float)((float)(dx * dx) + (float)(dy * dy));
            return dist <= (float)(com.oddlabs.tt.viewer.AttackNotification.RADIUS * com.oddlabs.tt.viewer.AttackNotification.RADIUS);
        }
        // Java: 29
        public void restartTimer()
        {
            if (!active)
            {
                getTimer().resetTime();
            }
        }
        // Java: 34
        public override void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            if (active)
            {
                active = false;
                getArrow().remove();
                getTimer().setTimerInterval(com.oddlabs.tt.viewer.AttackNotification.FADE_OUT);
            }
            else
            {
                getTimer().stop();
                getManager().removeAttackNotification(this);
            }
        }
        private void init_AttackNotification()
        {
            this.active = true;
        }
        public AttackNotification(com.oddlabs.tt.player.Player local_player, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.model.Selectable center, com.oddlabs.tt.viewer.NotificationManager manager, com.oddlabs.tt.animation.AnimationManager animation_manager) : base(local_player.getWorld(), gui_root, center.getPositionX(), center.getPositionY(), manager, 1.0f, 0.0f, 0.0f, local_player.getRace().getAttackNotificationAudio(), false, animation_manager)
        {
            init_AttackNotification();
        }
    }
}
