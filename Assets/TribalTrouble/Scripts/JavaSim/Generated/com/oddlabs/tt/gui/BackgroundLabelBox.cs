// Translated from BackgroundLabelBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class BackgroundLabelBox : com.oddlabs.tt.gui.LabelBox
    {
        // Java: 12
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Box background_box = com.oddlabs.tt.gui.Skin.getSkin().getBackgroundBox();
            background_box.render(renderer, 0.0f, 1.0f, getWidth(), getHeight() - 2, com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            base.renderGeometry(renderer);
        }
        public BackgroundLabelBox(string text, com.oddlabs.tt.font.Font font, int width) : base(text, font, width)
        {
        }
    }
}
