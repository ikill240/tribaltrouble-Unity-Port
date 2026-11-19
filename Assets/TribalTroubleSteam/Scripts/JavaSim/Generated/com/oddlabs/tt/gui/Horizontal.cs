// Translated from Horizontal.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class Horizontal : java.lang.Object
    {
        public com.oddlabs.tt.gui.ModeIconQuads left;
        public com.oddlabs.tt.gui.ModeIconQuads center;
        public com.oddlabs.tt.gui.ModeIconQuads right;
        public int height;
        public int left_width;
        public int right_width;
        // Java: 23
        public void render(com.oddlabs.tt.render.GUIRenderer renderer, float x, float y, int width, com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode)
        {
            int center_width = width - left_width - right_width;
            renderer.drawModeIcon(left, skinMode, x, y);
            renderer.drawIcon(center.quad(skinMode), (float)(x + left_width), y, center_width, height);
            renderer.drawModeIcon(right, skinMode, (float)((float)(x + left_width) + center_width), y);
        }
        // Java: 32
        public int getHeight()
        {
            return height;
        }
        public Horizontal(com.oddlabs.tt.gui.ModeIconQuads left, com.oddlabs.tt.gui.ModeIconQuads center, com.oddlabs.tt.gui.ModeIconQuads right) : base()
        {
            this.left = left;
            this.center = center;
            this.right = right;
            height = left.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight();
            left_width = left.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
            right_width = right.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
        }
    }
}
