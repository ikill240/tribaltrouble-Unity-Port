// Translated from NotificationListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 8
    public interface NotificationListener
    {
        // Java: 9
        void newAttackNotification(com.oddlabs.tt.model.Selectable target);
        // Java: 12
        void newSelectableNotification(com.oddlabs.tt.model.Selectable target);
        // Java: 15
        void registerTarget(com.oddlabs.tt.util.Target target);
        // Java: 18
        void unregisterTarget(com.oddlabs.tt.util.Target target);
        // Java: 21
        void patchesEdited(int patch_x0, int patch_y0, int patch_x1, int patch_y1);
        // Java: 24
        void gamespeedChanged(int speed);
        // Java: 27
        void playerGamespeedChanged();
        // Java: 30
        void playerCamera(com.oddlabs.tt.player.Player player, float x, float y, float z, float horiz_angle, float vert_angle);
        // Java: 33
        void playerCursor(com.oddlabs.tt.player.Player player, float x, float y, bool on_map);
        // Java: 36
        void playerSelection(com.oddlabs.tt.player.Player player, com.oddlabs.tt.model.Selectable[] selection);
        // Java: 39
        void playerMapMode(com.oddlabs.tt.player.Player player, bool on);
        // Java: 42
        void playerTargeting(com.oddlabs.tt.player.Player player, bool on);
        // Java: 45
        void playerPanelMenu(com.oddlabs.tt.player.Player player, int submenu);
        // Java: 48
        void playerPlacing(com.oddlabs.tt.player.Player player, int building_index, int grid_x, int grid_y, bool placing);
        // Java: 51
        void playerSelectionBox(com.oddlabs.tt.player.Player player, float x1, float y1, float x2, float y2, bool active);
        // Java: 54
        void playerBeacon(com.oddlabs.tt.player.Player player, float x, float y);
        // Java: 57
        void playerOrder(com.oddlabs.tt.player.Player player, float x, float y);
        // Java: 60
        void playerLeft(com.oddlabs.tt.player.Player player);
    }
}
