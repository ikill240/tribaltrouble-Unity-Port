// Translated from Delegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 16
    public abstract class Delegate : com.oddlabs.tt.gui.GUIObject
    {
        public static org.joml.Vector4fc BACKGROUND_ALPHA;
        // Java: 24
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            base.setFocus(direction);
            switchFocus(direction, false);
        }
        // Java: 30
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
        }
        // Java: 35
        public override void doAdd()
        {
            base.doAdd();
            com.oddlabs.tt.gui.GUIRoot root = getParentGUIRoot();
            if (root != null)
            {
                displayChanged(root.getWidth(), root.getHeight());
                if (root.getModalDelegate() == null || ((object)root.getModalDelegate() == (object)this))
                {
                    restoreFocus();
                }
            }
            else
            {
                restoreFocus();
            }
        }
        // Java: 49
        public virtual void render3D(com.oddlabs.tt.render.LandscapeRenderer renderer, com.oddlabs.tt.render.RenderQueues render_queues, com.oddlabs.tt.camera.CameraState state, com.oddlabs.tt.render.MatrixStack modelViewStack, com.oddlabs.tt.render.MatrixStack projectionStack)
        {
        }
        // Java: 53
        public virtual void render2D(com.oddlabs.tt.render.GUIRenderer renderer)
        {
        }
        // Java: 56
        public virtual bool keyboardBlocked()
        {
            return false;
        }
        // Java: 61
        public virtual bool isTargeting()
        {
            return false;
        }
        public const int NOT_PLACING = (-1);
        // Java: 68
        public virtual int getPlacingBuildingIndex()
        {
            return com.oddlabs.tt.@delegate.Delegate.NOT_PLACING;
        }
        // Java: 72
        public void renderBackgroundAlpha(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), com.oddlabs.tt.@delegate.Delegate.BACKGROUND_ALPHA);
        }
        static Delegate()
        {
            Delegate.BACKGROUND_ALPHA = new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.3f);
        }
        public Delegate() : base()
        {
            setPos(0, 0);
            setCanFocus(true);
        }
    }
}
