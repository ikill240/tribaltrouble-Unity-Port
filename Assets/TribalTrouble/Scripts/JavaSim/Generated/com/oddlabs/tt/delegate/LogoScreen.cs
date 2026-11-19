// Translated from LogoScreen.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 19
    public class LogoScreen : com.oddlabs.tt.@delegate.CameraDelegate, com.oddlabs.tt.animation.Updatable
    {
        public const float DELAY = 4.0f;
        public const int overlay_texture_width = 1024;
        public const int overlay_texture_height = 1024;
        public const int overlay_image_width = 800;
        public const int overlay_image_height = 600;
        public com.oddlabs.tt.gui.GUIIcon overlay;
        public com.oddlabs.tt.animation.TimerAnimation delay_timer;
        public com.oddlabs.tt.gui.GUIRoot client_root;
        public com.oddlabs.tt.gui.Fadable fadable;
        public com.oddlabs.tt.render.UIRenderer renderer;
        public bool fade_started;
        // Java: 56
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
            if (overlay != null)
            {
                overlay.setDim(width, height);
            }
        }
        // Java: 63
        public void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            delay_timer.stop();
            fade();
        }
        // Java: 69
        public void fade()
        {
            if (!fade_started)
            {
                fade_started = true;
                getGUIRoot().getGUI().newFade(fadable, client_root, renderer);
            }
        }
        // Java: 76
        public void switchImage(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            pop();
        }
        // Java: 80
        public void fadingDone(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
        }
        // Java: 83
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                fade();
                @event.consume();
            }
            base.handleInput(@event);
        }
        // Java: 92
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            fade();
        }
        private void init_LogoScreen()
        {
            this.delay_timer = new com.oddlabs.tt.animation.TimerAnimation(this, com.oddlabs.tt.@delegate.LogoScreen.DELAY);
            this.fade_started = false;
        }
        public LogoScreen(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.render.Texture logo, com.oddlabs.tt.gui.Fadable fadable, com.oddlabs.tt.gui.GUIRoot client_root, com.oddlabs.tt.render.UIRenderer renderer) : base(gui_root, new com.oddlabs.tt.camera.StaticCamera(new com.oddlabs.tt.camera.CameraState()))
        {
            init_LogoScreen();
            this.client_root = client_root;
            this.fadable = fadable;
            this.renderer = renderer;
            setCanFocus(true);
            setFocusCycle(true);
            if (logo != null)
            {
                overlay = new com.oddlabs.tt.gui.GUIIcon(new com.oddlabs.tt.gui.IconQuad(0.0f, 0.0f, (float)(((float)(com.oddlabs.tt.@delegate.LogoScreen.overlay_image_width)) / com.oddlabs.tt.@delegate.LogoScreen.overlay_texture_width), (float)(((float)(com.oddlabs.tt.@delegate.LogoScreen.overlay_image_height)) / com.oddlabs.tt.@delegate.LogoScreen.overlay_texture_height), getGUIRoot().getWidth(), getGUIRoot().getHeight(), logo));
                overlay.setPos(0, 0);
                addChild(overlay);
            }
            else
            {
                overlay = null;
            }
            delay_timer.start();
            gui_root.pushDelegate(this);
        }
    }
}
