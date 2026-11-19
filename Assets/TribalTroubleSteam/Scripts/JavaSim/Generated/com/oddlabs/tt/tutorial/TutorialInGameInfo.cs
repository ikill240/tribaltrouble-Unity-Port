// Translated from TutorialInGameInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 14
    public class TutorialInGameInfo : java.lang.Object, com.oddlabs.tt.viewer.InGameInfo
    {
        public int next_tutorial;
        // Java: 17
        public bool setNextTutorial(com.oddlabs.tt.gui.GUIRoot gui_root, int next_tutorial)
        {
            if (com.oddlabs.tt.form.TutorialForm.checkTutorial(gui_root, next_tutorial))
            {
                this.next_tutorial = next_tutorial;
                return true;
            }
            else
            {
                return false;
            }
        }
        // Java: 25
        public bool isRated()
        {
            return false;
        }
        // Java: 30
        public bool isMultiplayer()
        {
            return false;
        }
        // Java: 35
        public float getRandomStartPosition()
        {
            return 0.0f;
        }
        // Java: 40
        public void addGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.InGameMainMenu menu, com.oddlabs.tt.gui.Group game_infos)
        {
            menu.addAbortButton(com.oddlabs.tt.@delegate.Menu.i18n("end_tutorial"));
        }
        // Java: 45
        public void addGameOverGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.GameStatsDelegate @delegate, int header_y, com.oddlabs.tt.gui.Group group)
        {
            throw new System.Exception("Not implemented");
        }
        // Java: 50
        public void abort(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            next_tutorial = (-1);
            viewer.close();
        }
        // Java: 56
        public void close(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            if (next_tutorial != (-1))
            {
                com.oddlabs.tt.form.TutorialForm.startTutorial(viewer.getNetwork(), viewer.getGUIRoot(), next_tutorial);
            }
            else
            {
                com.oddlabs.tt.render.Renderer.startMenu(viewer.getNetwork(), viewer.getGUIRoot().getGUI());
            }
        }
        private void init_TutorialInGameInfo()
        {
            this.next_tutorial = (-1);
        }
        public TutorialInGameInfo() : base()
        {
            init_TutorialInGameInfo();
        }
    }
}
