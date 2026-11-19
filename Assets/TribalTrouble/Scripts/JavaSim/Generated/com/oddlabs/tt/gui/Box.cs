// Translated from Box.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class Box : java.lang.Object
    {
        public com.oddlabs.tt.gui.ModeIconQuads left_bottom;
        public com.oddlabs.tt.gui.ModeIconQuads bottom;
        public com.oddlabs.tt.gui.ModeIconQuads right_bottom;
        public com.oddlabs.tt.gui.ModeIconQuads right;
        public com.oddlabs.tt.gui.ModeIconQuads right_top;
        public com.oddlabs.tt.gui.ModeIconQuads top;
        public com.oddlabs.tt.gui.ModeIconQuads left_top;
        public com.oddlabs.tt.gui.ModeIconQuads left;
        public com.oddlabs.tt.gui.ModeIconQuads center;
        public int left_offset;
        public int bottom_offset;
        public int right_offset;
        public int top_offset;
        public int left_width;
        public int right_width;
        public int bottom_height;
        public int top_height;
        // Java: 59
        public void render(com.oddlabs.tt.render.GUIRenderer renderer, float x, float y, int width, int height, com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode)
        {
            int center_width = width - left_width - right_width;
            int center_height = height - bottom_height - top_height;
            renderer.drawModeIcon(left_bottom, skinMode, x, y);
            renderer.drawIcon(bottom.quad(skinMode), (float)(x + left_width), y, center_width, bottom_height);
            renderer.drawModeIcon(right_bottom, skinMode, (float)((float)(x + left_width) + center_width), y);
            renderer.drawIcon(right.quad(skinMode), (float)((float)(x + left_width) + center_width), (float)(y + bottom_height), right_width, center_height);
            renderer.drawModeIcon(right_top, skinMode, (float)((float)(x + left_width) + center_width), (float)((float)(y + bottom_height) + center_height));
            renderer.drawIcon(top.quad(skinMode), (float)(x + left_width), (float)((float)(y + bottom_height) + center_height), center_width, top_height);
            renderer.drawModeIcon(left_top, skinMode, x, (float)((float)(y + bottom_height) + center_height));
            renderer.drawIcon(left.quad(skinMode), x, (float)(y + bottom_height), left_width, center_height);
            renderer.drawIcon(center.quad(skinMode), (float)(x + left_width), (float)(y + bottom_height), center_width, center_height);
        }
        // Java: 77
        public int getLeftOffset()
        {
            return left_offset;
        }
        // Java: 81
        public int getBottomOffset()
        {
            return bottom_offset;
        }
        // Java: 85
        public int getRightOffset()
        {
            return right_offset;
        }
        // Java: 89
        public int getTopOffset()
        {
            return top_offset;
        }
        public Box(com.oddlabs.tt.gui.ModeIconQuads left_bottom, com.oddlabs.tt.gui.ModeIconQuads bottom, com.oddlabs.tt.gui.ModeIconQuads right_bottom, com.oddlabs.tt.gui.ModeIconQuads right, com.oddlabs.tt.gui.ModeIconQuads right_top, com.oddlabs.tt.gui.ModeIconQuads top, com.oddlabs.tt.gui.ModeIconQuads left_top, com.oddlabs.tt.gui.ModeIconQuads left, com.oddlabs.tt.gui.ModeIconQuads center, int left_offset, int bottom_offset, int right_offset, int top_offset) : base()
        {
            this.left_bottom = left_bottom;
            this.bottom = bottom;
            this.right_bottom = right_bottom;
            this.right = right;
            this.right_top = right_top;
            this.top = top;
            this.left_top = left_top;
            this.left = left;
            this.center = center;
            this.left_offset = left_offset;
            this.bottom_offset = bottom_offset;
            this.right_offset = right_offset;
            this.top_offset = top_offset;
            left_width = left.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
            right_width = right.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
            bottom_height = bottom.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight();
            top_height = top.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight();
        }
    }
}
