// Translated from BorderGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class BorderGroup : com.oddlabs.tt.gui.Group
    {
        public com.oddlabs.tt.gui.Label label;
        // Java: 19
        public override void compileCanvas()
        {
            com.oddlabs.tt.gui.GroupData data = com.oddlabs.tt.gui.Skin.getSkin().getGroupData();
            com.oddlabs.tt.gui.Box group = data.group();
            if (label != null)
            {
                base.compileCanvas(group.getLeftOffset(), group.getBottomOffset(), group.getRightOffset(), group.getTopOffset() + data.captionOffset());
                label.setPos(data.captionLeft(), getHeight() - data.captionY());
                addChild(label);
            }
            else
            {
                base.compileCanvas(group.getLeftOffset(), group.getBottomOffset(), group.getRightOffset(), group.getTopOffset());
            }
            setCanFocus(true);
        }
        // Java: 37
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Skin.getSkin().getGroupData().group().render(renderer, 0.0f, 0.0f, getWidth(), getHeight(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
        }
        public BorderGroup() : base()
        {
            label = null;
        }
        public BorderGroup(string caption) : base()
        {
            com.oddlabs.tt.gui.GroupData data = com.oddlabs.tt.gui.Skin.getSkin().getGroupData();
            label = new com.oddlabs.tt.gui.Label(caption, data.captionFont());
        }
    }
}
