// Translated from Index.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.font
{
    // Java: 12
    public class Index : java.lang.Object, com.oddlabs.tt.animation.Updatable
    {
        public const int INDEX_WIDTH = 1;
        public const float BLINK_INTERVAL = 0.5f;
        public static com.oddlabs.tt.font.Index index;
        public com.oddlabs.tt.animation.TimerAnimation timer;
        public bool blink_on;
        // Java: 26
        public static void resetBlinking()
        {
            com.oddlabs.tt.font.Index.index.doResetBlinking();
        }
        // Java: 30
        public void doResetBlinking()
        {
            blink_on = true;
            timer.resetTime();
        }
        // Java: 35
        public static void renderIndex(com.oddlabs.tt.render.GUIRenderer renderer, int render_x, int render_y, com.oddlabs.tt.font.Font font, org.joml.Vector4fc color)
        {
            com.oddlabs.tt.font.Index.index.doRenderIndex(renderer, render_x, render_y, font, color);
        }
        // Java: 40
        public void doRenderIndex(com.oddlabs.tt.render.GUIRenderer renderer, int render_x, int render_y, com.oddlabs.tt.font.Font font, org.joml.Vector4fc color)
        {
            if (blink_on)
            {
                renderer.drawColoredQuad(render_x, render_y + 3, com.oddlabs.tt.font.Index.INDEX_WIDTH, font.getHeight() - 6, color);
            }
        }
        // Java: 47
        public void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            blink_on = !blink_on;
        }
        static Index()
        {
            Index.index = new com.oddlabs.tt.font.Index();
        }
        private void init_Index()
        {
            this.timer = new com.oddlabs.tt.animation.TimerAnimation(this, com.oddlabs.tt.font.Index.BLINK_INTERVAL);
        }
        public Index() : base()
        {
            init_Index();
            timer.start();
            blink_on = true;
        }
    }
}
