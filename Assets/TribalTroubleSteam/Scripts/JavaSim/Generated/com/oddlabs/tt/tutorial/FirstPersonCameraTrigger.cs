// Translated from FirstPersonCameraTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 7
    public class FirstPersonCameraTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 12
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.@delegate.Delegate @delegate = tutorial.getViewer().getGUIRoot().getDelegate();
            if ((@delegate is com.oddlabs.tt.@delegate.FirstPersonDelegate))
            {
                tutorial.next(new com.oddlabs.tt.tutorial.MapModeTrigger());
            }
        }
        public FirstPersonCameraTrigger() : base(0.1f, 2.0f, "fpc")
        {
        }
    }
}
