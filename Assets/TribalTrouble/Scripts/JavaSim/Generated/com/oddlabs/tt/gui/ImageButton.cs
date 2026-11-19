// Translated from ImageButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class ImageButton : com.oddlabs.tt.gui.ButtonObject
    {
        public com.oddlabs.tt.gui.GUIObject normal;
        public com.oddlabs.tt.gui.GUIObject hovered;
        public com.oddlabs.tt.gui.GUIObject disabled;
        // Java: 19
        public override void setPos(int x, int y)
        {
            base.setPos(x, y);
            normal.setPos(x, y);
            hovered.setPos(x, y);
            disabled.setPos(x, y);
        }
        // Java: 27
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.GUIObject render_v = (isDisabled() ? disabled : (isHovered() || isActive() ? hovered : normal));
            render_v.renderGeometry(renderer);
        }
        // Java: 33
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
        }
        public ImageButton(com.oddlabs.tt.gui.GUIObject normal, com.oddlabs.tt.gui.GUIObject hovered, com.oddlabs.tt.gui.GUIObject disabled) : base(com.oddlabs.tt.gui.Skin.getSkin().getEditFont())
        {
            setDim(normal.getWidth(), normal.getHeight());
            this.normal = normal;
            this.hovered = hovered;
            this.disabled = disabled;
        }
    }
}
