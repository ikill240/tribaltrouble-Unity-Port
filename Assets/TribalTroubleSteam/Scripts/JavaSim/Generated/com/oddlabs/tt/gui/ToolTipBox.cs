// Translated from ToolTipBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class ToolTipBox : com.oddlabs.tt.gui.TextField
    {
        public const float MAX_DELAY_SECONDS = 1.5f;
        public java.util.List icons;
        // Java: 20
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            throw new System.NotSupportedException("ToolTipBox.renderGeometry should not be called directly. Use render(GUIRenderer, ...)");
        }
        // Java: 26
        public void append(java.util.List icons)
        {
            this.icons = icons;
        }
        // Java: 30
        public override void clear()
        {
            base.clear();
            icons = null;
        }
        // Java: 36
        public void render(com.oddlabs.tt.render.GUIRenderer renderer, int center_x, int top_y, int width, int height)
        {
            if (getText().isEmpty())
            {
                return;
            }
            com.oddlabs.tt.gui.ToolTipBoxInfo box = com.oddlabs.tt.gui.Skin.getSkin().getToolTipInfo();
            int text_width = getFont().getWidth(getText());
            int box_width = text_width + box.leftOffset() + box.rightOffset();
            int box_height = box.box().getHeight();
            if (icons != null)
            {
                int i = default;
                for (i = 0; i < icons.size(); i++)
                {
                    box_width += ((com.oddlabs.tt.gui.IconQuad)icons.get(i)).getWidth() / 3;
                }
                box_width += ((com.oddlabs.tt.gui.IconQuad)icons.get(i - 1)).getWidth() * 2 / 3;
            }
            float x = java.lang.Math.clamp((float)(center_x - (float)(box_width / 2.0f)), 0, width - box_width);
            float y = java.lang.Math.clamp(top_y - box_height, 0, height - box_height);
            box.box().render(renderer, x, y, box_width, com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, getFont(), getText(), (float)(x + box.leftOffset()), (float)(y + box.bottomOffset()), java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, com.oddlabs.util.Color.WHITE);
            if (icons != null)
            {
                float render_x = box_width - box.rightOffset() - ((com.oddlabs.tt.gui.IconQuad)icons.getLast()).getWidth();
                foreach (com.oddlabs.tt.gui.IconQuad icon in java.lang.JIter.of(icons))
                {
                    renderer.drawIcon(icon, (float)(x + render_x), (float)(y + (float)((box_height - icon.getHeight()) / 2.0f)));
                    render_x = (float)(render_x - (float)(icon.getWidth() / 3.0f));
                }
            }
        }
        public ToolTipBox() : base(com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 200)
        {
        }
    }
}
