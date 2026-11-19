// Translated from MapIslandButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class MapIslandButton : com.oddlabs.tt.gui.IconButton
    {
        public int islandIndex;
        // Java: 15
        public virtual int getIslandIndex()
        {
            return islandIndex;
        }
        // Java: 19
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = default;
            if (isDisabled())
            {
                skinMode = com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED;
            }
            else if (isHovered())
            {
                skinMode = com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE;
            }
            else if (isActive())
            {
                bool otherIslandHovered = false;
                com.oddlabs.tt.gui.GUIRoot root = getParentGUIRoot();
                if (root != null)
                {
                    com.oddlabs.tt.gui.GUIObject hoverObj = root.getCurrentGUIObject();
                    if ((hoverObj is com.oddlabs.tt.gui.MapIslandButton) && ((object)hoverObj != (object)this))
                    {
                        otherIslandHovered = true;
                    }
                }
                skinMode = (otherIslandHovered ? com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL : com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE);
            }
            else
            {
                skinMode = com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL;
            }
            renderer.drawModeIcon(getIcon(), skinMode, 0, 0);
        }
        public MapIslandButton(com.oddlabs.tt.gui.ModeIconQuads icon, int islandIndex) : base(icon, new java.util.function.Supplier_L(() =>
        {
            return "";
        }))
        {
            this.islandIndex = islandIndex;
            setCanFocus(true);
        }
    }
}
