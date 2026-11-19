// Translated from PanelTab.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 8
    public class PanelTab : com.oddlabs.tt.gui.GUIObject
    {
        public static org.joml.Vector4fc HIGHLIGHT_COLOR;
        public bool selected;
        public com.oddlabs.tt.gui.Label label;
        // Java: 25
        public void select(bool selected)
        {
            this.selected = selected;
            focusNotifyAll(false);
            if (selected)
            {
                label.setColor(com.oddlabs.tt.gui.Label.DEFAULT_COLOR);
            }
        }
        // Java: 32
        public com.oddlabs.tt.gui.ModeIconQuads.Mode getRenderState()
        {
            return (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isActive() || selected ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
        }
        // Java: 37
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Skin.getSkin().getPanelData().tab().render(renderer, 0, 0, getWidth(), getRenderState());
        }
        // Java: 42
        public void updateNotify()
        {
            if (!selected)
            {
                label.setColor(com.oddlabs.tt.gui.PanelTab.HIGHLIGHT_COLOR);
            }
        }
        static PanelTab()
        {
            PanelTab.HIGHLIGHT_COLOR = com.oddlabs.util.Color.argb4v((-16711936));
        }
        public PanelTab(string caption) : base()
        {
            com.oddlabs.tt.gui.PanelData data = com.oddlabs.tt.gui.Skin.getSkin().getPanelData();
            com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getButtonFont();
            label = new com.oddlabs.tt.gui.Label(caption, font);
            label.setPos(data.leftCaptionOffset(), (data.tab().getHeight() - font.getHeight()) / 2 + data.bottomCaptionOffset());
            addChild(label);
            setDim(data.leftCaptionOffset() + label.getWidth() + data.rightCaptionOffset(), data.tab().getHeight());
            setCanFocus(true);
            setTabStop(false);
        }
    }
}
