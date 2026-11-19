// Translated from SpectatorSelectionBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 14
    public class SpectatorSelectionBox : com.oddlabs.tt.gui.GUIObject
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public java.util.function.BooleanSupplier visible;
        // Java: 27
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
        }
        // Java: 32
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.viewer.SpectatorView view = viewer.getSpectatorView();
            com.oddlabs.tt.player.Player followed = (view != null && visible.getAsBoolean() ? view.getFollowedPlayer() : (com.oddlabs.tt.player.Player)(null));
            com.oddlabs.tt.viewer.PlayerView player_view = (followed != null ? view.getView(followed) : (com.oddlabs.tt.viewer.PlayerView)(null));
            if (player_view == null || !player_view.isSelectionBoxActive())
            {
                return;
            }
            float width = gui_root.getWidth();
            float height = gui_root.getHeight();
            com.oddlabs.tt.@delegate.SelectionDelegate.drawSelectionBox(renderer, (float)(player_view.getSelectionBoxX1() * width), (float)(player_view.getSelectionBoxY1() * height), (float)(player_view.getSelectionBoxX2() * width), (float)(player_view.getSelectionBoxY2() * height));
        }
        public SpectatorSelectionBox(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.gui.GUIRoot gui_root, java.util.function.BooleanSupplier visible) : base()
        {
            this.viewer = viewer;
            this.gui_root = gui_root;
            this.visible = visible;
            displayChangedNotify(gui_root.getWidth(), gui_root.getHeight());
        }
    }
}
