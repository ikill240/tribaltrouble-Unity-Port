// Translated from Selection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 11
    public class Selection : java.lang.Object
    {
        public com.oddlabs.tt.model.Army[] shortcut_armies;
        public com.oddlabs.tt.player.Player local_player;
        public com.oddlabs.tt.viewer.SelectionArmy current_selection;
        // Java: 21
        public com.oddlabs.tt.viewer.SelectionArmy getCurrentSelection()
        {
            return current_selection;
        }
        // Java: 25
        public void clearSelection()
        {
            current_selection = new com.oddlabs.tt.viewer.SelectionArmy(local_player);
        }
        // Java: 29
        public void clearShortcutArmies()
        {
            java.util.Arrays.fill(shortcut_armies, null);
        }
        // Java: 33
        public void removeFromArmies(com.oddlabs.tt.model.Selectable selectable)
        {
            current_selection.remove(selectable);
            foreach (com.oddlabs.tt.model.Army shortcut_armie in shortcut_armies)
            {
                if (shortcut_armie != null)
                {
                    shortcut_armie.remove(selectable);
                }
            }
        }
        // Java: 42
        public void setShortcutArmy(int index)
        {
            if (shortcut_armies[index] != null)
            {
                shortcut_armies[index].clear();
            }
            else
            {
                shortcut_armies[index] = new com.oddlabs.tt.model.Army();
            }
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(current_selection.getSet()))
            {
                shortcut_armies[index].add(s);
            }
        }
        // Java: 53
        public bool enableShortcutArmy(int index)
        {
            bool empty = true;
            if (shortcut_armies[index] != null)
            {
                current_selection.clear();
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(shortcut_armies[index].getSet()))
                {
                    current_selection.add(s);
                    empty = false;
                }
            }
            return !empty;
        }
        private void init_Selection()
        {
            this.shortcut_armies = new com.oddlabs.tt.model.Army[10];
        }
        public Selection(com.oddlabs.tt.player.Player local_player) : base()
        {
            init_Selection();
            this.local_player = local_player;
            clearSelection();
        }
    }
}
