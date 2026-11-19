// Translated from NotificationManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 13
    public class NotificationManager : java.lang.Object
    {
        public java.util.List attack_notifies;
        public java.util.List notifies;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.viewer.Notification latest_notification;
        // Java: 23
        public com.oddlabs.tt.viewer.Notification getLatestNotification()
        {
            return latest_notification;
        }
        // Java: 27
        public void newAttackNotification(com.oddlabs.tt.animation.AnimationManager manager, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.player.Player local_player)
        {
            foreach (com.oddlabs.tt.viewer.AttackNotification current in java.lang.JIter.of(attack_notifies))
            {
                if (current.contains(target))
                {
                    current.restartTimer();
                    return;
                }
            }
            addNotification(new com.oddlabs.tt.viewer.AttackNotification(local_player, gui_root, target, this, manager), attack_notifies);
        }
        // Java: 38
        public void newSelectableNotification(com.oddlabs.tt.model.Selectable s, com.oddlabs.tt.animation.AnimationManager manager, com.oddlabs.tt.player.Player local_player)
        {
            newNotification(manager, local_player, s.getPositionX(), s.getPositionY(), 0.0f, 1.0f, 0.0f, false);
        }
        // Java: 43
        public void newBeacon(com.oddlabs.tt.animation.AnimationManager manager, com.oddlabs.tt.player.Player local_player, float x, float y)
        {
            newNotification(manager, local_player, x, y, 0.0f, 0.0f, 1.0f, true);
        }
        // Java: 47
        public void newNotification(com.oddlabs.tt.animation.AnimationManager manager, com.oddlabs.tt.player.Player local_player, float x, float y, float r, float g, float b, bool show_always)
        {
            addNotification(new com.oddlabs.tt.viewer.Notification(local_player.getWorld(), gui_root, x, y, this, r, g, b, local_player.getRace().getBuildingNotificationAudio(), show_always, manager), notifies);
        }
        // Java: 53
        public void addNotification(com.oddlabs.tt.viewer.Notification notification, java.util.List list)
        {
            list.add(notification);
            latest_notification = notification;
        }
        // Java: 58
        public void removeAttackNotification(com.oddlabs.tt.viewer.AttackNotification current)
        {
            attack_notifies.remove(current);
        }
        // Java: 62
        public void removeNotification(com.oddlabs.tt.viewer.Notification current)
        {
            notifies.remove(current);
        }
        private void init_NotificationManager()
        {
            this.attack_notifies = new java.util.ArrayList();
            this.notifies = new java.util.ArrayList();
            this.latest_notification = null;
        }
        public NotificationManager(com.oddlabs.tt.gui.GUIRoot gui_root) : base()
        {
            init_NotificationManager();
            this.gui_root = gui_root;
        }
    }
}
