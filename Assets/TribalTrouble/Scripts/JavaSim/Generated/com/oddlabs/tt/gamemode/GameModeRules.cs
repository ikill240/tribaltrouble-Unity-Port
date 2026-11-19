// Translated from GameModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode
{
    // Java: 17
    public interface GameModeRules
    {
        // Java: 18
        java.util.List getOptions();
        // Java: 25
        bool isPlayerAlive(com.oddlabs.tt.player.Player player);
        // Java: 31
        void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer);
        // Java: 38
        void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status);
        // Java: 42
        string describe();
    }
}
