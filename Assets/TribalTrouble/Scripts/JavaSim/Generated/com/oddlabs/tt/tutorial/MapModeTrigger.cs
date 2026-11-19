// Translated from MapModeTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class MapModeTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 11
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if ((tutorial.getViewer().getDelegate().getCamera() is com.oddlabs.tt.camera.MapCamera))
            {
                tutorial.next(new com.oddlabs.tt.tutorial.FromMapModeTrigger());
            }
        }
        public MapModeTrigger() : base(0.1f, 1.0f, "map_mode")
        {
        }
    }
}
