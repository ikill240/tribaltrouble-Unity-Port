// Translated from Panel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public class Panel : com.oddlabs.tt.gui.Group
    {
        public com.oddlabs.tt.gui.PanelTab tab;
        // Java: 13
        public com.oddlabs.tt.gui.PanelTab getTab()
        {
            return tab;
        }
        // Java: 17
        public override void compileCanvas()
        {
            com.oddlabs.tt.gui.Box box = com.oddlabs.tt.gui.Skin.getSkin().getPanelData().box();
            base.compileCanvas(box.getLeftOffset(), box.getBottomOffset(), box.getRightOffset(), box.getTopOffset());
        }
        public Panel(string caption) : base(true)
        {
            tab = new com.oddlabs.tt.gui.PanelTab(caption);
        }
    }
}
