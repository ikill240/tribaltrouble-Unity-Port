// Translated from ChieftainButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 13
    public class ChieftainButton : com.oddlabs.tt.gui.NonFocusIconButton
    {
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.model.Building current_building;
        // Java: 26
        public void setBuilding(com.oddlabs.tt.model.Building current_building)
        {
            this.current_building = current_building;
        }
        // Java: 30
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            player_interface.trainChieftain(current_building, !current_building.getChieftainContainer().isTraining());
        }
        // Java: 35
        public override void postRender(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            if (current_building.isAlive() && current_building.getChieftainContainer().isTraining())
            {
                com.oddlabs.tt.gui.IconQuad watchQuad = com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(getProgress());
                renderer.drawIcon(watchQuad, getWidth() - watchQuad.getWidth(), getHeight() - watchQuad.getHeight());
            }
        }
        // Java: 43
        public float getProgress()
        {
            return (current_building.isAlive() ? current_building.getChieftainContainer().getBuildProgress() : 0);
        }
        public ChieftainButton(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon) : base(icon, com.oddlabs.tt.input.GameAction.TRAIN_CHIEFTAIN, new java.util.function.Supplier_L(() =>
        {
            return com.oddlabs.tt.gui.ActionButtonPanel.i18n("train_chieftain_tip", com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(com.oddlabs.tt.input.GameAction.TRAIN_CHIEFTAIN));
        }))
        {
            this.player_interface = player_interface;
            setCanFocus(true);
        }
    }
}
