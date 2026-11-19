// Translated from ZoomTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 7
    public class ZoomTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public bool[] zoom_dirs;
        // Java: 15
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.camera.GameCamera camera = tutorial.getViewer().getCamera();
            if (camera.getLastZoomFactor() > 0.0f)
            {
                zoom_dirs[0] = true;
            }
            else if (camera.getLastZoomFactor() < 0.0f)
            {
                zoom_dirs[1] = true;
            }
            foreach (bool zoomDir in zoom_dirs)
            {
                if (!zoomDir)
                {
                    return;
                }
            }
            tutorial.next(new com.oddlabs.tt.tutorial.RotateTrigger());
        }
        private void init_ZoomTrigger()
        {
            this.zoom_dirs = new bool[2];
        }
        public ZoomTrigger(com.oddlabs.tt.viewer.WorldViewer viewer) : base(0.0f, 2.0f, "zoom")
        {
            init_ZoomTrigger();
            viewer.getCamera().resetLastZoomFactor();
        }
    }
}
