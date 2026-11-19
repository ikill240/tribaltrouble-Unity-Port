// Translated from StatusIcon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class StatusIcon : com.oddlabs.tt.gui.GUIObject
    {
        public com.oddlabs.tt.gui.IconQuad icon;
        public com.oddlabs.tt.gui.TextField label;
        public string tooltip;
        public com.oddlabs.tt.model.SupplyCounter counter;
        public int text_count;
        // Java: 29
        public void setCounter(com.oddlabs.tt.model.SupplyCounter counter)
        {
            this.counter = counter;
        }
        // Java: 33
        public void doUpdate()
        {
            int count = counter.getNumSupplies();
            if (count != text_count)
            {
                text_count = count;
                label.clear();
                label.append(count);
            }
        }
        // Java: 42
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            string tooltip_str = com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.StatusIcon).getName()), "max", tooltip, counter.getMaxSupplies());
            tool_tip_box.append(tooltip_str);
        }
        // Java: 49
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawIcon(icon, getWidth() - icon.getWidth(), (float)((getHeight() - icon.getHeight()) / 2.0f));
        }
        private void init_StatusIcon()
        {
            this.text_count = (-1);
        }
        public StatusIcon(int label_width, com.oddlabs.tt.gui.IconQuad icon, string tooltip) : base()
        {
            init_StatusIcon();
            this.tooltip = tooltip;
            setDim(icon.getWidth() + label_width, icon.getHeight());
            setCanFocus(true);
            this.icon = icon;
            label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), label_width, com.oddlabs.tt.gui.Origin.AT_END);
            addChild(label);
            label.setPos(0, (getHeight() - label.getFont().getHeight()) / 2);
        }
    }
}
