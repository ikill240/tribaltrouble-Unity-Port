// Translated from NotificationListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 7
    public interface NotificationListener
    {
        // Java: 8
        void newAttackNotification(com.oddlabs.tt.model.Selectable target);
        // Java: 11
        void newSelectableNotification(com.oddlabs.tt.model.Selectable target);
        // Java: 14
        void registerTarget(com.oddlabs.tt.util.Target target);
        // Java: 17
        void unregisterTarget(com.oddlabs.tt.util.Target target);
        // Java: 20
        void patchesEdited(int patch_x0, int patch_y0, int patch_x1, int patch_y1);
        // Java: 23
        void gamespeedChanged(int speed);
        // Java: 26
        void playerGamespeedChanged();
    }
}
