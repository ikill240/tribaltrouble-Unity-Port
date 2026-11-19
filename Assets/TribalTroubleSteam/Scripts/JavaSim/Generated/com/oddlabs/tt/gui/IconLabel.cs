// Translated from IconLabel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class IconLabel : com.oddlabs.tt.gui.GUIObject, java.lang.Comparable
    {
        public com.oddlabs.tt.gui.IconQuad icon;
        public com.oddlabs.tt.gui.Label label;
        // Java: 20
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawIcon(icon, 0, 0);
        }
        // Java: 25
        public com.oddlabs.tt.gui.Label getLabel()
        {
            return label;
        }
        // Java: 29
        public int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.IconLabel o = (com.oddlabs.tt.gui.IconLabel)o_p;
            return label.compareTo(o.getLabel());
        }
        public IconLabel(com.oddlabs.tt.gui.IconQuad icon, com.oddlabs.tt.gui.Label label) : base()
        {
            this.icon = icon;
            this.label = label;
            label.setPos(icon.getWidth(), 0);
            addChild(label);
            int width = icon.getWidth() + label.getWidth();
            int height = java.lang.Math.max(icon.getHeight(), label.getHeight());
            setDim(width, height);
        }
    }
}
