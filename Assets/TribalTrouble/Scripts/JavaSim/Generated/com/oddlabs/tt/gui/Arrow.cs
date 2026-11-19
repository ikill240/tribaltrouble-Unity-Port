// Translated from Arrow.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class Arrow : com.oddlabs.tt.gui.GUIObject
    {
        public const float SECONDS_PER_FLASH = 0.5f;
        public const float COLOR_DELTA = 0.5f;
        public float target_x;
        public float target_y;
        public float target_z;
        public float r;
        public float g;
        public float b;
        public bool show_always;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        // Java: 35
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
        }
        public static org.joml.Vector4f point;
        // Java: 42
        public org.joml.Vector4f project3DTo2D(float x, float y, float z)
        {
            com.oddlabs.tt.gui.Arrow.point.set(x, y, z, 1);
            gui_root.getDelegate().getCamera().getState().getProjectionModelView().transform(com.oddlabs.tt.gui.Arrow.point, com.oddlabs.tt.gui.Arrow.point);
            if (com.oddlabs.tt.gui.Arrow.point.w_f < 0.1f)
            {
                com.oddlabs.tt.gui.Arrow.point.w_f = 0.1f;
            }
            float inv_w = (float)(1 / com.oddlabs.tt.gui.Arrow.point.w_f);
            com.oddlabs.tt.gui.Arrow.point.set((float)((float)(((float)((float)(com.oddlabs.tt.gui.Arrow.point.x_f * inv_w) + 1)) * 0.5f) * gui_root.getWidth()), (float)((float)(((float)((float)(com.oddlabs.tt.gui.Arrow.point.y_f * inv_w) + 1)) * 0.5f) * gui_root.getHeight()), 0, 0);
            return com.oddlabs.tt.gui.Arrow.point;
        }
        // Java: 53
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            int screen_width = gui_root.getWidth();
            int screen_height = gui_root.getHeight();
            org.joml.Vector4f result = project3DTo2D(target_x, target_y, target_z);
            float x = result.x_f;
            float y = result.y_f;
            float dx = (float)(x - (float)(screen_width / 2.0f));
            float dy = (float)(y - (float)(screen_height / 2.0f));
            float dist_sqr = (float)((float)(dx * dx) + (float)(dy * dy));
            if (dist_sqr < 1.0f)
            {
                dx = 1.0f;
                dy = 0.0f;
            }
            else
            {
                float inv_dist = (float)(1.0f / ((float)(java.lang.Math.sqrt(dist_sqr))));
                dx = (float)(dx * inv_dist);
                dy = (float)(dy * inv_dist);
            }
            float angle = ((float)(java.lang.Math.toDegrees(java.lang.Math.acos(dx))));
            if (dy < 0.0f)
            {
                angle = (float)(360.0f - angle);
            }
            float real_t = (float)(((float)(x - (float)(screen_width / 2.0f))) / dx);
            float t = real_t;
            float t_min_x = (float)(((float)(-screen_width / 2.0f)) / dx);
            float t_max_x = (float)(((float)(screen_width / 2.0f)) / dx);
            float t_x = java.lang.Math.max(t_min_x, t_max_x);
            t = java.lang.Math.min(t, t_x);
            float t_min_y = (float)(((float)(-screen_height / 2.0f)) / dy);
            float t_max_y = (float)(((float)(screen_height / 2.0f)) / dy);
            float t_y = java.lang.Math.max(t_min_y, t_max_y);
            t = java.lang.Math.min(t, t_y);
            if (show_always || gui_root.getDelegate().getCamera().getState().inNoDetailMode() || t < real_t)
            {
                com.oddlabs.tt.gui.NotifyArrowData data = com.oddlabs.tt.gui.GUIIcons.getIcons().getNotifyArrowData();
                float head_x = data.headX();
                float head_y = data.headY();
                renderer.getMatrixStack().push();
                renderer.getMatrixStack().translate((float)((float)(screen_width / 2.0f) + (float)(dx * t)), (float)((float)(screen_height / 2.0f) + (float)(dy * t)), 0.0f);
                renderer.getMatrixStack().rotate(angle, 0.0f, 0.0f, 1.0f);
                float val = (float)(((float)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getTime() % com.oddlabs.tt.gui.Arrow.SECONDS_PER_FLASH)) / ((float)(com.oddlabs.tt.gui.Arrow.SECONDS_PER_FLASH * 0.5f)));
                if (val > 1.0f)
                {
                    val = (float)(2.0f - val);
                }
                val = (float)(com.oddlabs.tt.gui.Arrow.COLOR_DELTA * val);
                com.oddlabs.tt.gui.IconQuad arrow = data.arrow();
                renderer.drawIcon(arrow, (float)(-head_x), (float)(-head_y), new org.joml.Vector4f(r, g, b, (float)(1.0f - val)));
                renderer.getMatrixStack().pop();
            }
        }
        static Arrow()
        {
            Arrow.point = new org.joml.Vector4f();
        }
        public Arrow(com.oddlabs.tt.landscape.HeightMap heightmap, com.oddlabs.tt.gui.GUIRoot gui_root, float target_x, float target_y, float r, float g, float b, bool show_always) : base()
        {
            this.gui_root = gui_root;
            this.target_x = target_x;
            this.target_y = target_y;
            this.target_z = heightmap.getNearestHeight(target_x, target_y);
            this.r = r;
            this.g = g;
            this.b = b;
            this.show_always = show_always;
            displayChangedNotify(gui_root.getWidth(), gui_root.getHeight());
        }
    }
}
