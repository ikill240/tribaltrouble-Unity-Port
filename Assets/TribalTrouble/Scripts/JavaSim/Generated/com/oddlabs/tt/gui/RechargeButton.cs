// Translated from RechargeButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 12
    public class RechargeButton : com.oddlabs.tt.gui.NonFocusIconButton
    {
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public int magic_index;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 27
        public void setUnit(com.oddlabs.tt.model.Unit unit)
        {
            this.unit = unit;
        }
        // Java: 31
        public override bool isHidden()
        {
            return unit != null && unit.getOwner().isLocked(com.oddlabs.tt.model.Unlocks.spell(magic_index));
        }
        // Java: 36
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (unit != null && unit.canDoMagic(magic_index))   // (Steam fix: a magic hotkey before the button had its chieftain)
            {
                player_interface.doMagic(unit, magic_index);
            }
        }
        // Java: 42
        public override void postRender(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            float progress = unit.getMagicProgress(magic_index);
            if (!unit.isDead() && progress < 1.0f)
            {
                com.oddlabs.tt.gui.IconQuad watchQuad = com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(progress);
                renderer.drawIcon(watchQuad, getWidth() - watchQuad.getWidth(), getHeight() - watchQuad.getHeight());
            }
        }
        public RechargeButton(com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon, com.oddlabs.tt.input.GameAction action, java.util.function.Supplier tool_tip, int magic_index) : base(icon, action, tool_tip)
        {
            this.player_interface = player_interface;
            this.magic_index = magic_index;
            setCanFocus(true);
            com.oddlabs.tt.gui.IconQuad normal = icon.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            setDim(normal.getWidth(), normal.getHeight());
        }
    }
}
