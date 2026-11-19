// Translated from MenuButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class MenuButton : com.oddlabs.tt.gui.ButtonObject
    {
        public const float SECONDS_PER_HOVER_CYCLE = 1.5f;
        public const float HOVER_SCALE_FACTOR = 0.06f;
        public string text;
        public org.joml.Vector4fc color_normal;
        public org.joml.Vector4fc color_active;
        public float start_hover_time;
        // Java: 33
        public string getText()
        {
            return text;
        }
        // Java: 37
        public void scaleHovered(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            float time = (float)(((float)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getTime() - start_hover_time)) % com.oddlabs.tt.gui.MenuButton.SECONDS_PER_HOVER_CYCLE);
            float cycle_position = (float)(time / com.oddlabs.tt.gui.MenuButton.SECONDS_PER_HOVER_CYCLE);
            float scale = (float)(1.0f + (float)(com.oddlabs.tt.gui.MenuButton.HOVER_SCALE_FACTOR * ((float)(java.lang.Math.sin((float)(cycle_position * 2) * java.lang.Math.PI)))));
            renderer.getMatrixStack().scale(scale, scale, 1.0f);
        }
        // Java: 44
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.getMatrixStack().push().translate((float)(getWidth() / 2.0f), (float)(getHeight() / 2.0f), 0);
            org.joml.Vector4fc c = default;
            if (isActive())
            {
                c = color_active;
                scaleHovered(renderer);
            }
            else
            {
                c = (isDisabled() ? com.oddlabs.tt.gui.Label.DISABLED_COLOR : color_normal);
            }
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, getFont(), text, (float)(-getWidth() / 2.0f), (float)(-getHeight() / 2.0f), java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, c);
            renderer.getMatrixStack().pop();
        }
        // Java: 58
        public override void mouseEntered()
        {
            if (!isActive())
            {
                start_hover_time = (float)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getTime() % com.oddlabs.tt.gui.MenuButton.SECONDS_PER_HOVER_CYCLE);
                setFocus();
            }
        }
        public MenuButton(string caption, org.joml.Vector4fc color_normal, org.joml.Vector4fc color_active) : this(caption, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), color_normal, color_active)
        {
        }
        public MenuButton(string text, com.oddlabs.tt.font.Font font, org.joml.Vector4fc color_normal, org.joml.Vector4fc color_active) : base(font)
        {
            setDim(font.getWidth(text), font.getHeight());
            this.text = text;
            this.color_normal = color_normal;
            this.color_active = color_active;
        }
    }
}
