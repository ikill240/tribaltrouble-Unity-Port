// Translated from InGameInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 7
    public interface InGameInfo
    {
        // Java: 8
        void addGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.InGameMainMenu menu, com.oddlabs.tt.gui.Group game_infos);
        // Java: 10
        void addGameOverGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.GameStatsDelegate @delegate, int header_y, com.oddlabs.tt.gui.Group buttons);
        // Java: 12
        void abort(com.oddlabs.tt.viewer.WorldViewer viewer);
        // Java: 14
        void close(com.oddlabs.tt.viewer.WorldViewer viewer);
        // Java: 16
        bool isMultiplayer();
        // Java: 18
        bool isRated();
        // Java: 20
        float getRandomStartPosition();
        // Java: 24
        int resolveAIDifficulty(int ai_difficulty);
        // Java: 29
        bool adaptiveAI();
        // Java: 34
        bool isCampaign();
    }
}
