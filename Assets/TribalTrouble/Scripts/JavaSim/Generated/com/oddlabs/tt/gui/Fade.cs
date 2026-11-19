// Translated from Fade.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class Fade : java.lang.Object
    {
        public const float FADE_TIME = 1.0f;
        public com.oddlabs.tt.gui.Fadable fadable;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.render.UIRenderer renderer;
        public float time;
        public bool image_switched;
        // Java: 26
        public void animate(com.oddlabs.tt.gui.GUI gui, float t)
        {
            time += t;
            if (!image_switched && time >= (float)(com.oddlabs.tt.gui.Fade.FADE_TIME / 2))
            {
                image_switched = true;
                gui.switchRoot(gui_root, renderer);
            }
            if (time >= com.oddlabs.tt.gui.Fade.FADE_TIME)
            {
                gui.stopFade();
                if (fadable != null)
                {
                    fadable.fadingDone();
                }
            }
        }
        // Java: 40
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        // Java: 43
        public void render(com.oddlabs.tt.render.GUIRenderer guiRenderer)
        {
            float alpha = ((float)(java.lang.Math.sin(java.lang.Math.PI * time / com.oddlabs.tt.gui.Fade.FADE_TIME)));
            guiRenderer.drawColoredQuad(0, 0, gui_root.getWidth(), gui_root.getHeight(), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, alpha));
        }
        private void init_Fade()
        {
            this.time = 0;
            this.image_switched = false;
        }
        public Fade(com.oddlabs.tt.gui.Fadable fadable, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.render.UIRenderer renderer) : base()
        {
            init_Fade();
            this.fadable = fadable;
            this.gui_root = gui_root;
            this.renderer = renderer;
        }
    }
}
