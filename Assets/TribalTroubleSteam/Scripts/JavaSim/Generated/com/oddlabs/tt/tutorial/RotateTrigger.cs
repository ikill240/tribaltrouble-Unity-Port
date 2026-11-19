// Translated from RotateTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class RotateTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public bool[] rotate_dirs;
        // Java: 13
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.camera.GameCamera camera = tutorial.getViewer().getCamera();
            if (camera.rotateRight())
            {
                rotate_dirs[0] = true;
            }
            if (camera.rotateLeft())
            {
                rotate_dirs[1] = true;
            }
            foreach (bool rotateDir in rotate_dirs)
            {
                if (!rotateDir)
                {
                    return;
                }
            }
            tutorial.next(new com.oddlabs.tt.tutorial.PitchTrigger());
        }
        private void init_RotateTrigger()
        {
            this.rotate_dirs = new bool[2];
        }
        public RotateTrigger() : base(0.1f, 2.0f, "rotate")
        {
            init_RotateTrigger();
        }
    }
}
