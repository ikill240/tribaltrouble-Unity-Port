// Translated from Vertical.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class Vertical : java.lang.Object
    {
        public com.oddlabs.tt.gui.ModeIconQuads bottom;
        public com.oddlabs.tt.gui.ModeIconQuads center;
        public com.oddlabs.tt.gui.ModeIconQuads top;
        public int bottom_height;
        public int top_height;
        public int width;
        // Java: 23
        public void render(com.oddlabs.tt.render.GUIRenderer renderer, float x, float y, int height, com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode)
        {
            int center_height = height - bottom_height - top_height;
            renderer.drawModeIcon(bottom, skinMode, x, y);
            renderer.drawIcon(center.quad(skinMode), x, (float)(y + bottom_height), width, center_height);
            renderer.drawModeIcon(top, skinMode, x, (float)((float)(y + bottom_height) + center_height));
        }
        // Java: 32
        public int getWidth()
        {
            return width;
        }
        // Java: 36
        public int getMinHeight()
        {
            return bottom.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight() + top.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight();
        }
        public Vertical(com.oddlabs.tt.gui.ModeIconQuads bottom, com.oddlabs.tt.gui.ModeIconQuads center, com.oddlabs.tt.gui.ModeIconQuads top) : base()
        {
            this.bottom = bottom;
            this.center = center;
            this.top = top;
            bottom_height = bottom.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight();
            top_height = top.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight();
            width = bottom.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
        }
    }
}
