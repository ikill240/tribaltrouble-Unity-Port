// Translated from TutorialOverDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 24
    public class TutorialOverDelegate : com.oddlabs.tt.@delegate.CameraDelegate, com.oddlabs.tt.animation.Updatable
    {
        public const float DELAY = 1.0f;
        public static java.util.ResourceBundle bundle;
        // Java: 28
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.TutorialOverDelegate.bundle, key, args);
        }
        public com.oddlabs.tt.animation.TimerAnimation delay_timer;
        public com.oddlabs.tt.gui.Group group_buttons;
        public com.oddlabs.tt.tutorial.TutorialInGameInfo tutorial_info;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        // Java: 77
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderBackgroundAlpha(renderer);
        }
        // Java: 82
        public void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            addChild(group_buttons);
            delay_timer.stop();
        }
        // Java: 88
        public class StartTutorialListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.@delegate.TutorialOverDelegate this_0;
            public int tutorial_number;
            // Java: 95
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                if (this_0.tutorial_info.setNextTutorial(this_0.viewer.getGUIRoot(), tutorial_number))
                {
                    this_0.viewer.close();
                }
            }
            public StartTutorialListener(com.oddlabs.tt.@delegate.TutorialOverDelegate this_0_, int tutorial_number) : base()
            {
                this.this_0 = this_0_;
                this.tutorial_number = tutorial_number;
            }
        }
        static TutorialOverDelegate()
        {
            TutorialOverDelegate.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.TutorialOverDelegate).getName());
        }
        private void init_TutorialOverDelegate()
        {
            this.delay_timer = new com.oddlabs.tt.animation.TimerAnimation(this, com.oddlabs.tt.@delegate.TutorialOverDelegate.DELAY);
        }
        public TutorialOverDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.tutorial.TutorialInGameInfo tutorial_info, com.oddlabs.tt.camera.Camera old_camera, int tutorial_number) : base(viewer.getGUIRoot(), new com.oddlabs.tt.camera.StaticCamera(old_camera.getState()))
        {
            init_TutorialOverDelegate();
            this.viewer = viewer;
            this.tutorial_info = tutorial_info;
            setDim(getGUIRoot().getWidth(), getGUIRoot().getHeight());
            string tutorial_completed_str = i18n("tutorial_completed", tutorial_number);
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(tutorial_completed_str, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label);
            label.setPos((getWidth() - label.getWidth()) / 2, (getHeight() - label.getHeight()) * 2 / 3);
            group_buttons = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.HorizButton button_next = new com.oddlabs.tt.gui.HorizButton(i18n("next_tutorial"), 170);
            button_next.addMouseClickListener(new com.oddlabs.tt.@delegate.TutorialOverDelegate.StartTutorialListener(this, tutorial_number + 1));
            com.oddlabs.tt.gui.HorizButton button_restart = new com.oddlabs.tt.gui.HorizButton(i18n("restart_tutorial"), 170);
            button_restart.addMouseClickListener(new com.oddlabs.tt.@delegate.TutorialOverDelegate.StartTutorialListener(this, tutorial_number));
            com.oddlabs.tt.gui.HorizButton button_end = new com.oddlabs.tt.gui.HorizButton(i18n("main_menu"), 170);
            button_end.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                viewer.close();
                this.setDisabled(true);
            }));
            if (tutorial_number < com.oddlabs.tt.form.TutorialForm.NUM_TUTORIALS)
            {
                group_buttons.addChild(button_next);
            }
            group_buttons.addChild(button_end);
            group_buttons.addChild(button_restart);
            button_end.place();
            button_restart.place(button_end, com.oddlabs.tt.gui.Placement.LEFT_MID);
            if (tutorial_number < com.oddlabs.tt.form.TutorialForm.NUM_TUTORIALS)
            {
                button_next.place(button_restart, com.oddlabs.tt.gui.Placement.LEFT_MID);
            }
            group_buttons.compileCanvas();
            group_buttons.setPos((getWidth() - group_buttons.getWidth()) / 2, (getHeight() - group_buttons.getHeight()) * 1 / 3);
            delay_timer.start();
        }
    }
}
