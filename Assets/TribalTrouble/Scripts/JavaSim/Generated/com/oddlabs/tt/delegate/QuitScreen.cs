// Translated from QuitScreen.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 14
    public class QuitScreen : com.oddlabs.tt.@delegate.CameraDelegate, com.oddlabs.tt.animation.Updatable
    {
        public const float DELAY = 5.0f;
        public const int overlay_texture_width = 1024;
        public const int overlay_texture_height = 1024;
        public const int overlay_image_width = 800;
        public const int overlay_image_height = 600;
        public const string overlay_texture_name = "/textures/gui/quitscreen";
        public com.oddlabs.tt.gui.GUIImage overlay;
        public com.oddlabs.tt.animation.TimerAnimation delay_timer;
        public bool key_pressed;
        public bool time_out;
        // Java: 46
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
            overlay.setDim(width, height);
        }
        // Java: 52
        public void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            delay_timer.stop();
            time_out = true;
            quit();
        }
        // Java: 59
        public void quit()
        {
            if (key_pressed && time_out)
            {
                com.oddlabs.tt.render.Renderer.shutdown();
            }
        }
        // Java: 64
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                key_pressed = true;
                quit();
                @event.consume();
            }
            base.handleInput(@event);
        }
        // Java: 74
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            key_pressed = true;
            quit();
        }
        private void init_QuitScreen()
        {
            this.delay_timer = new com.oddlabs.tt.animation.TimerAnimation(this, com.oddlabs.tt.@delegate.QuitScreen.DELAY);
            this.key_pressed = false;
            this.time_out = false;
        }
        public QuitScreen(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.camera.Camera camera) : base(gui_root, camera)
        {
            init_QuitScreen();
            setCanFocus(true);
            setFocusCycle(true);
            overlay = new com.oddlabs.tt.gui.GUIImage(gui_root.getWidth(), gui_root.getHeight(), 0.0f, 0.0f, (float)(((float)(com.oddlabs.tt.@delegate.QuitScreen.overlay_image_width)) / com.oddlabs.tt.@delegate.QuitScreen.overlay_texture_width), (float)(((float)(com.oddlabs.tt.@delegate.QuitScreen.overlay_image_height)) / com.oddlabs.tt.@delegate.QuitScreen.overlay_texture_height), com.oddlabs.tt.@delegate.QuitScreen.overlay_texture_name);
            overlay.setPos(0, 0);
            addChild(overlay);
            com.oddlabs.tt.gui.GUIRoot quit_root = gui_root.getGUI().newFade();
            delay_timer.start();
            quit_root.pushDelegate(this);
        }
    }
}
