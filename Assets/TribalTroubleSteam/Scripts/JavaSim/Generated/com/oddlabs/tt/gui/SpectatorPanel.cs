// Translated from SpectatorPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 18
    public class SpectatorPanel : com.oddlabs.tt.gui.GUIObject, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public java.util.function.BooleanSupplier visible;
        // Java: 23
        public class Copy : java.lang.Object
        {
            public com.oddlabs.tt.gui.ActionButtonPanel panel() { return panel_f; }
            public com.oddlabs.tt.viewer.Selection selection() { return selection_f; }
            public Copy(com.oddlabs.tt.gui.ActionButtonPanel panel, com.oddlabs.tt.viewer.Selection selection)
            {
                this.panel_f = panel;
                this.selection_f = selection;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Copy r)) return false; return true && java.lang.JO.equals(panel_f, r.panel_f) && java.lang.JO.equals(selection_f, r.selection_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(panel_f); h = 31 * h + java.lang.JO.hashCode(selection_f); } return h; }
            public override string toString() { return "Copy[" + "panel=" + java.lang.JO.toString(panel_f) + ", selection=" + java.lang.JO.toString(selection_f) + "]"; }
            public com.oddlabs.tt.gui.ActionButtonPanel panel_f;
            public com.oddlabs.tt.viewer.Selection selection_f;
        }
        public java.util.Map copies;
        public com.oddlabs.tt.player.Player shown;
        public com.oddlabs.tt.viewer.Selection selection;
        public com.oddlabs.tt.gui.ActionButtonPanel panel;
        public int selection_version;
        public bool just_shown;
        // Java: 40
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
        }
        // Java: 45
        public override void doAdd()
        {
            base.doAdd();
            viewer.getAnimationManagerLocal().registerAnimation(this);
        }
        // Java: 51
        public override void doRemove()
        {
            base.doRemove();
            viewer.getAnimationManagerLocal().removeAnimation(this);
        }
        // Java: 57
        public void animate(float t)
        {
            com.oddlabs.tt.viewer.SpectatorView view = viewer.getSpectatorView();
            com.oddlabs.tt.player.Player followed = (view != null && visible.getAsBoolean() ? view.getFollowedPlayer() : (com.oddlabs.tt.player.Player)(null));
            if (((object)followed != (object)shown))
            {
                show(followed);
            }
            if (followed == null || panel == null || selection == null)
            {
                return;
            }
            com.oddlabs.tt.viewer.PlayerView player_view = view.getView(followed);
            bool resend = player_view.getSelectionVersion() != selection_version;
            if (resend || com.oddlabs.tt.gui.SpectatorPanel.hasDead(selection))
            {
                selection_version = player_view.getSelectionVersion();
                selection.clearSelection();
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player_view.getSelection()))
                {
                    if (!s.isDead())
                    {
                        selection.getCurrentSelection().add(s);
                    }
                }
                if (just_shown)
                {
                    panel.animate(t);
                }
            }
            just_shown = false;
            panel.refreshCounters();
            panel.setSubmenu(player_view.getPanelSubmenu());
        }
        // Java: 82
        public static bool hasDead(com.oddlabs.tt.viewer.Selection selection)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(selection.getCurrentSelection().getSet()))
            {
                if (s.isDead())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 90
        public void show(com.oddlabs.tt.player.Player player)
        {
            if (panel != null)
            {
                panel.remove();
            }
            if (selection != null)
            {
                selection.clearSelection();
            }
            panel = null;
            viewer.getRenderer().setSelectedBuilding(null);
            selection = null;
            selection_version = (-1);
            shown = player;
            if (player == null)
            {
                return;
            }
            just_shown = true;
            com.oddlabs.tt.gui.SpectatorPanel.Copy copy = ((com.oddlabs.tt.gui.SpectatorPanel.Copy)copies.computeIfAbsent(player, new java.util.function.Function_L((object a0) => { return this.copyFor(((com.oddlabs.tt.player.Player)a0)); })));
            selection = copy.selection();
            panel = copy.panel();
            addChild(panel);
        }
        // Java: 109
        public com.oddlabs.tt.gui.SpectatorPanel.Copy copyFor(com.oddlabs.tt.player.Player player)
        {
            com.oddlabs.tt.viewer.Selection copy_selection = new com.oddlabs.tt.viewer.Selection(player);
            return new com.oddlabs.tt.gui.SpectatorPanel.Copy(new com.oddlabs.tt.gui.ActionButtonPanel(viewer, viewer.getCamera(), gui_root.getWidth(), gui_root.getHeight(), player, copy_selection, true), copy_selection);
        }
        private void init_SpectatorPanel()
        {
            this.copies = new java.util.HashMap();
            this.selection_version = (-1);
        }
        public SpectatorPanel(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.gui.GUIRoot gui_root, java.util.function.BooleanSupplier visible) : base()
        {
            init_SpectatorPanel();
            this.viewer = viewer;
            this.gui_root = gui_root;
            this.visible = visible;
            displayChangedNotify(gui_root.getWidth(), gui_root.getHeight());
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
