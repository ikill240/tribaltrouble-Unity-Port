// Translated from IconButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class IconButton : com.oddlabs.tt.gui.ButtonObject
    {
        public com.oddlabs.tt.gui.ModeIconQuads icon;
        public com.oddlabs.tt.gui.IconDisabler icon_disabler;
        // Java: 27
        public void setIconDisabler(com.oddlabs.tt.gui.IconDisabler icon_disabler)
        {
            this.icon_disabler = icon_disabler;
        }
        // Java: 31
        public void doUpdate()
        {
            setDisabled(icon_disabler != null && icon_disabler.isDisabled());
        }
        // Java: 35
        public virtual com.oddlabs.tt.gui.ModeIconQuads getIcon()
        {
            return icon;
        }
        // Java: 39
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isHovered() || isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            renderer.drawModeIcon(icon, skinMode, 0, 0);
        }
        public IconButton(com.oddlabs.tt.gui.ModeIconQuads icon, java.util.function.Supplier tool_tip) : this(icon, null, tool_tip)
        {
        }
        public IconButton(com.oddlabs.tt.gui.ModeIconQuads icon, com.oddlabs.tt.input.GameAction action, java.util.function.Supplier tool_tip) : base(com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), action, tool_tip)
        {
            this.icon = icon;
            com.oddlabs.tt.gui.IconQuad normal = icon.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            setDim(normal.getWidth(), normal.getHeight());
        }
    }
}
