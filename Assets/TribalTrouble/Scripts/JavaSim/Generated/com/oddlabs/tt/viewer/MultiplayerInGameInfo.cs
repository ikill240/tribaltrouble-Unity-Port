// Translated from MultiplayerInGameInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 9
    public class MultiplayerInGameInfo : com.oddlabs.tt.viewer.DefaultInGameInfo
    {
        public float random_start_position;
        public bool is_rated;
        // Java: 19
        public override void addGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.InGameMainMenu menu, com.oddlabs.tt.gui.Group game_infos)
        {
            base.addGUI(viewer, menu, game_infos);
            com.oddlabs.tt.gui.FreeQuitLabel free_quit_label = new com.oddlabs.tt.gui.FreeQuitLabel(viewer.getWorld(), viewer.getAnimationManagerLocal());
            menu.addChild(free_quit_label);
            free_quit_label.setPos(0, 0);
        }
        // Java: 27
        public override void addGameOverGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.GameStatsDelegate @delegate, int header_y, com.oddlabs.tt.gui.Group buttons)
        {
            addGameOverGUI(viewer, @delegate, header_y, buttons, false);
        }
        // Java: 33
        public override bool isRated()
        {
            return is_rated;
        }
        // Java: 38
        public override bool isMultiplayer()
        {
            return true;
        }
        // Java: 43
        public override float getRandomStartPosition()
        {
            return random_start_position;
        }
        public MultiplayerInGameInfo(float random_start_position, bool is_rated) : base()
        {
            this.random_start_position = random_start_position;
            this.is_rated = is_rated;
        }
    }
}
