// Translated from PulldownItem.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class PulldownItem : com.oddlabs.tt.gui.ButtonObject
    {
        public com.oddlabs.tt.gui.Label label;
        public object attachment;
        // Java: 25
        public virtual object getAttachment()
        {
            return attachment;
        }
        // Java: 29
        public virtual int getTextHeight()
        {
            return label.getHeight();
        }
        // Java: 33
        public virtual int getTextWidth()
        {
            return label.getTextWidth();
        }
        // Java: 38
        public override com.oddlabs.tt.gui.Renderable setDim(int width, int height)
        {
            base.setDim(width, height);
            com.oddlabs.tt.gui.Box item = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownItem();
            label.setDim(getWidth() - item.getLeftOffset() - item.getRightOffset(), label.getHeight());
            label.setPos(item.getLeftOffset(), (getHeight() - label.getHeight()) / 2);
            return this;
        }
        // Java: 47
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Box item = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownItem();
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL : (isActive() || isHovered() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            item.render(renderer, 0.0f, 0.0f, getWidth(), getHeight(), skinMode);
        }
        // Java: 55
        public virtual void setLabelString(string label_str)
        {
            label.set(label_str);
        }
        // Java: 59
        public virtual string getLabelString()
        {
            return label;
        }
        // Java: 63
        public virtual org.joml.Vector4fc getLabelColor()
        {
            return label.getColor();
        }
        // Java: 67
        public virtual void setLabelColor(org.joml.Vector4fc color)
        {
            label.setColor(color);
        }
        // Java: 71
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
        }
        public PulldownItem(string label_str) : this(label_str, null)
        {
        }
        public PulldownItem(string label_str, object attachment) : base(com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().font())
        {
            this.attachment = attachment;
            label = new com.oddlabs.tt.gui.Label(label_str, getFont(), 0, com.oddlabs.tt.gui.Origin.AT_START);
            addChild(label);
            setDim(0, label.getHeight());
        }
    }
}
