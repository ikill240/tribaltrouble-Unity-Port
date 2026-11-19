// Translated from GameModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode
{
    // Java: 14
    public interface GameModeRules
    {
        // Java: 15
        java.util.List getOptions();
        // Java: 22
        bool isPlayerAlive(com.oddlabs.tt.player.Player player);
        // Java: 28
        void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer);
    }
}
