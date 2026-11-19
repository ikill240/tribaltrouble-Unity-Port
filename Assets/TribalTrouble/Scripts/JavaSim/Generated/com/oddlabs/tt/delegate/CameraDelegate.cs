// Translated from CameraDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 9
    public abstract class CameraDelegate : com.oddlabs.tt.@delegate.Delegate
    {
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.camera.Camera camera;
        // Java: 18
        public com.oddlabs.tt.gui.GUIRoot getGUIRoot()
        {
            return gui_root;
        }
        // Java: 22
        public void setCamera(com.oddlabs.tt.camera.Camera camera)
        {
            this.camera = camera;
        }
        // Java: 26
        public com.oddlabs.tt.camera.Camera getCamera()
        {
            return camera;
        }
        // Java: 30
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (camera != null)
            {
                camera.handleInput(@event);
            }
            if (!@event.isConsumed())
            {
                base.handleInput(@event);
            }
        }
        // Java: 40
        public override void doAdd()
        {
            base.doAdd();
            getCamera().enable();
        }
        // Java: 46
        public override void doRemove()
        {
            base.doRemove();
            getCamera().disable();
        }
        // Java: 52
        public virtual bool renderCursor()
        {
            return true;
        }
        // Java: 56
        public virtual bool canScroll()
        {
            return false;
        }
        // Java: 60
        public virtual bool forceRender()
        {
            return false;
        }
        // Java: 64
        public void pop()
        {
            gui_root.removeDelegate(this);
        }
        public CameraDelegate(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.camera.Camera camera) : base()
        {
            this.camera = camera;
            this.gui_root = gui_root;
        }
    }
}
