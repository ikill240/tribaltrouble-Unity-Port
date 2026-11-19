// Translated from JumpDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 10
    public class JumpDelegate : com.oddlabs.tt.@delegate.InGameDelegate
    {
        public java.lang.Runnable runnable;
        // Java: 31
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            @event.consume();
        }
        // Java: 36
        public override void mouseScrolled(int amount)
        {
        }
        // Java: 40
        public override void doRemove()
        {
            base.doRemove();
            if (runnable != null)
            {
                runnable.run();
            }
        }
        public JumpDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera old_camera, float x, float y) : base(viewer, null)
        {
            setCamera(new com.oddlabs.tt.camera.JumpCamera(this, old_camera, x, y));
            runnable = null;
        }
        public JumpDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera old_camera, float x, float y, float meters_per_second, float max_seconds) : this(viewer, old_camera, x, y, meters_per_second, max_seconds, null)
        {
        }
        public JumpDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera old_camera, float x, float y, float meters_per_second, float max_seconds, java.lang.Runnable runnable) : base(viewer, null)
        {
            setCamera(new com.oddlabs.tt.camera.JumpCamera(this, old_camera, x, y, meters_per_second, max_seconds));
            this.runnable = runnable;
        }
    }
}
