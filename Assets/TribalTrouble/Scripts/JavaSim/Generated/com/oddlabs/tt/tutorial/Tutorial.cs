// Translated from Tutorial.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 16
    public class Tutorial : java.lang.Object
    {
        public const int BORDER_OFFSET = 90;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.tutorial.TutorialInGameInfo tutorial_info;
        public com.oddlabs.tt.gui.GUIObject info;
        public com.oddlabs.tt.animation.TimerAnimation timer;
        public float old_after_done_time;
        // Java: 32
        public com.oddlabs.tt.viewer.WorldViewer getViewer()
        {
            return viewer;
        }
        // Java: 36
        public void removeInfo()
        {
            if (info != null)
            {
                info.remove();
            }
        }
        // Java: 41
        public void done(int next_tutorial)
        {
            timer.stop();
            removeInfo();
            viewer.getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.TutorialOverDelegate(viewer, tutorial_info, viewer.getGUIRoot().getDelegate().getCamera(), next_tutorial));
        }
        // Java: 48
        public void next(com.oddlabs.tt.tutorial.TutorialTrigger trigger)
        {
            timer.stop();
            com.oddlabs.tt.animation.TimerAnimation delay_timer = new com.oddlabs.tt.animation.TimerAnimation(viewer.getAnimationManagerLocal(), new com.oddlabs.tt.animation.Updatable_L((object anim_l) =>
            {
                com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_l;
                anim.stop();
                next0(trigger);
            }), old_after_done_time);
            delay_timer.start();
            old_after_done_time = trigger.getAfterDoneTime();
        }
        // Java: 58
        public void next0(com.oddlabs.tt.tutorial.TutorialTrigger trigger)
        {
            removeInfo();
            com.oddlabs.tt.animation.TimerAnimation delay_timer = new com.oddlabs.tt.animation.TimerAnimation(viewer.getAnimationManagerLocal(), new com.oddlabs.tt.animation.Updatable_L((object anim_l) =>
            {
                com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_l;
                anim.stop();
                next1(trigger);
            }), 0.5f);
            delay_timer.start();
        }
        // Java: 67
        public void next1(com.oddlabs.tt.tutorial.TutorialTrigger trigger)
        {
            string text = com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.tutorial.TutorialTrigger).getName()), trigger.getTextKey(), trigger.getFormatArgs());
            info = new com.oddlabs.tt.gui.LabelBox(text, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 400);
            info.setPos(com.oddlabs.tt.tutorial.Tutorial.BORDER_OFFSET, viewer.getGUIRoot().getHeight() - com.oddlabs.tt.tutorial.Tutorial.BORDER_OFFSET - info.getHeight());
            viewer.getGUIRoot().addChild(info);
            viewer.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(viewer.getLocalPlayer().getRace().getBuildingNotificationAudio(), 0.0f, 0.0f, 0.0f, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_NOTIFICATION, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_NOTIFICATION, 0.25f, 1.0f, 1.0f, false, true));
            timer = new com.oddlabs.tt.animation.TimerAnimation(viewer.getAnimationManagerLocal(), new com.oddlabs.tt.animation.Updatable_L((object unused_l) =>
            {
                com.oddlabs.tt.animation.TimerAnimation unused = (com.oddlabs.tt.animation.TimerAnimation)unused_l;
                trigger.run(this);
            }), trigger.getCheckInterval());
            timer.start();
        }
        public Tutorial(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.tutorial.TutorialInGameInfo tutorial_info, com.oddlabs.tt.tutorial.TutorialTrigger first_trigger) : base()
        {
            this.viewer = viewer;
            this.tutorial_info = tutorial_info;
            next0(first_trigger);
            old_after_done_time = first_trigger.getAfterDoneTime();
        }
    }
}
