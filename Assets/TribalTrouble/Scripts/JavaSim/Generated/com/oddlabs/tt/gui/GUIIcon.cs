// Translated from GUIIcon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class GUIIcon : com.oddlabs.tt.gui.GUIObject
    {
        public com.oddlabs.tt.gui.IconQuad icon;
        // Java: 15
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawIcon(icon, 0, 0, getWidth(), getHeight());
        }
        public GUIIcon(com.oddlabs.tt.gui.IconQuad icon) : base()
        {
            this.icon = icon;
            setDim(icon.getWidth(), icon.getHeight());
            setCanFocus(false);
        }
    }
}
