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
            string all = java.lang.JO.toString(getText());
            renderLines(renderer, all.split("\n"), center_x, top_y, width, height);
        }
        public static org.joml.Vector4fc PANEL;
        public static org.joml.Vector4fc RIM;
        public const float ICON_SCALE = 0.75f;
        public static org.joml.Vector4fc TITLE;
        // Java: 53
        public void renderLines(com.oddlabs.tt.render.GUIRenderer renderer, string[] lines, int center_x, int top_y, int width, int height)
        {
            int pad = 4;
            int line_h = getFont().getHeight() + 1;
            int icons_w = 0;
            if (icons != null && !icons.isEmpty())
            {
                foreach (com.oddlabs.tt.gui.IconQuad icon in java.lang.JIter.of(icons))
                {
                    icons_w += java.lang.JC.f2i(((float)(icon.getWidth() * com.oddlabs.tt.gui.ToolTipBox.ICON_SCALE))) / 3;
                }
                icons_w += java.lang.JC.f2i(((float)(((com.oddlabs.tt.gui.IconQuad)icons.getLast()).getWidth() * com.oddlabs.tt.gui.ToolTipBox.ICON_SCALE))) * 2 / 3 + pad;
                line_h = java.lang.Math.max(line_h, java.lang.JC.f2i(((float)(((com.oddlabs.tt.gui.IconQuad)icons.getLast()).getHeight() * com.oddlabs.tt.gui.ToolTipBox.ICON_SCALE))));
            }
            int w = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                w = java.lang.Math.max(w, getFont().getWidth(lines[i]) + ((i == 0 ? icons_w : 0)));
            }
            int box_w = w + 2 * pad;
            int text_h = getFont().getHeight() + 1;
            int box_h = line_h + (lines.Length - 1) * text_h + 2 * pad;
            float x = java.lang.Math.clamp((float)(center_x - (float)(box_w / 2.0f)), 0, java.lang.Math.max(0, width - box_w));
            float y = java.lang.Math.clamp(top_y - box_h, 0, java.lang.Math.max(0, height - box_h));
            renderer.drawColoredQuad(x, y, box_w, box_h, com.oddlabs.tt.gui.ToolTipBox.PANEL);
            renderer.drawColoredQuad(x, (float)((float)(y + box_h) - 2), box_w, 2, com.oddlabs.tt.gui.ToolTipBox.RIM);
            renderer.drawColoredQuad(x, y, box_w, 1, com.oddlabs.tt.gui.ToolTipBox.RIM);
            for (int i_2 = 0; i_2 < lines.Length; i_2++)
            {
                float ly = (i_2 == 0 ? (float)((float)((float)((float)(y + box_h) - pad) - line_h) + (float)((line_h - text_h) / 2.0f)) : (float)((float)((float)((float)(y + box_h) - pad) - line_h) - i_2 * text_h));
                com.oddlabs.tt.font.TextLineRenderer.render(renderer, getFont(), lines[i_2], (float)(x + pad), ly, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, (i_2 == 0 && lines.Length > 1 ? com.oddlabs.tt.gui.ToolTipBox.TITLE : com.oddlabs.util.Color.WHITE));
            }
            if (icons_w > 0)
            {
                float iw = (float)(((com.oddlabs.tt.gui.IconQuad)icons.getLast()).getWidth() * com.oddlabs.tt.gui.ToolTipBox.ICON_SCALE);
                float render_x = (float)((float)((float)(x + box_w) - pad) - iw);
                float title_mid = (float)((float)((float)(y + box_h) - pad) - (float)(line_h / 2.0f));
                foreach (com.oddlabs.tt.gui.IconQuad icon_2 in java.lang.JIter.of(icons))
                {
                    float icw = (float)(icon_2.getWidth() * com.oddlabs.tt.gui.ToolTipBox.ICON_SCALE);
                    float ich = (float)(icon_2.getHeight() * com.oddlabs.tt.gui.ToolTipBox.ICON_SCALE);
                    renderer.drawIcon(icon_2, render_x, (float)(title_mid - (float)(ich / 2.0f)), icw, ich);
                    render_x = (float)(render_x - (float)(icw / 3.0f));
                }
            }
        }
        static ToolTipBox()
        {
            ToolTipBox.PANEL = new org.joml.Vector4f(0.05f, 0.04f, 0.03f, 0.62f);
            ToolTipBox.RIM = new org.joml.Vector4f(0.86f, 0.68f, 0.32f, 0.55f);
            ToolTipBox.TITLE = new org.joml.Vector4f(1.0f, 0.84f, 0.45f, 1.0f);
        }
        public ToolTipBox() : base(com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 200)
        {
        }
    }
}
