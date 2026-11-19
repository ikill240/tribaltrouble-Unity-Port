// Translated from FromMapModeTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class FromMapModeTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 11
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if ((tutorial.getViewer().getDelegate().getCamera() is com.oddlabs.tt.camera.GameCamera))
            {
                tutorial.next(new com.oddlabs.tt.tutorial.SelectUnitTrigger());
            }
        }
        public FromMapModeTrigger() : base(0.1f, 1.0f, "from_map_mode")
        {
        }
    }
}
