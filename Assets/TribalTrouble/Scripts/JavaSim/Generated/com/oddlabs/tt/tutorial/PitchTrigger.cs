// Translated from PitchTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class PitchTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public bool[] pitch_dirs;
        // Java: 13
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.camera.GameCamera camera = tutorial.getViewer().getCamera();
            if (camera.pitchUp())
            {
                pitch_dirs[0] = true;
            }
            if (camera.pitchDown())
            {
                pitch_dirs[1] = true;
            }
            foreach (bool pitchDir in pitch_dirs)
            {
                if (!pitchDir)
                {
                    return;
                }
            }
            tutorial.next(new com.oddlabs.tt.tutorial.FirstPersonCameraTrigger());
        }
        private void init_PitchTrigger()
        {
            this.pitch_dirs = new bool[2];
        }
        public PitchTrigger() : base(0.1f, 2.0f, "pitch")
        {
            init_PitchTrigger();
        }
    }
}
