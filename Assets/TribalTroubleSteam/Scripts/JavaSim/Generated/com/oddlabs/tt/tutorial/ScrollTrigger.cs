// Translated from ScrollTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 8
    public class ScrollTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public bool[] scroll_dirs;
        // Java: 22
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.camera.GameCamera camera = tutorial.getViewer().getCamera();
            if (camera.getScrollX() > 0)
            {
                scroll_dirs[0] = true;
            }
            else if (camera.getScrollX() < 0)
            {
                scroll_dirs[1] = true;
            }
            if (camera.getScrollY() > 0)
            {
                scroll_dirs[2] = true;
            }
            else if (camera.getScrollY() < 0)
            {
                scroll_dirs[3] = true;
            }
            foreach (bool scrollDir in scroll_dirs)
            {
                if (!scrollDir)
                {
                    return;
                }
            }
            tutorial.next(new com.oddlabs.tt.tutorial.ZoomTrigger(tutorial.getViewer()));
        }
        private void init_ScrollTrigger()
        {
            this.scroll_dirs = new bool[4];
        }
        public ScrollTrigger(com.oddlabs.tt.player.Player player) : base(0.1f, 2.0f, "scroll")
        {
            init_ScrollTrigger();
            player.enableMoving(false);
            player.enableRepairing(false);
            player.enableAttacking(false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false);
            player.enableBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, false);
            player.enableChieftains(false);
        }
    }
}
