// Translated from GUI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 16
    public class GUI : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.render.GUIRenderer guiRenderer;
        public com.oddlabs.tt.gui.GUIRoot current_root;
        public com.oddlabs.tt.gui.Fade fade;
        public com.oddlabs.tt.render.UIRenderer renderer;
        public com.oddlabs.tt.camera.CameraState frustum_state;
        // Java: 28
        public com.oddlabs.tt.gui.GUIRoot newFade()
        {
            return newFade(null, null);
        }
        // Java: 32
        public com.oddlabs.tt.gui.GUIRoot newFade(com.oddlabs.tt.gui.Fadable fadable, com.oddlabs.tt.render.UIRenderer renderer)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = createRoot();
            newFade(fadable, gui_root, renderer);
            return gui_root;
        }
        // Java: 38
        public void newFade(com.oddlabs.tt.gui.Fadable fadable, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.render.UIRenderer renderer)
        {
            fade = new com.oddlabs.tt.gui.Fade(fadable, gui_root, renderer);
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().registerAnimation(this);
        }
        // Java: 43
        public com.oddlabs.tt.gui.GUIRoot createRoot()
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = new com.oddlabs.tt.gui.GUIRoot(this);
            com.oddlabs.tt.render.Renderer renderer = com.oddlabs.tt.render.Renderer.getRenderer();
            gui_root.displayChanged(renderer.getDisplayWidth(), renderer.getDisplayHeight());
            return gui_root;
        }
        // Java: 50
        public void animate(float t)
        {
            if (fade != null)
            {
                fade.animate(this, t);
            }
        }
        // Java: 57
        public void stopFade()
        {
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().removeAnimation(this);
            fade = null;
        }
        // Java: 62
        public void switchRoot(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.render.UIRenderer renderer)
        {
            current_root.removeTree();
            current_root = gui_root;
            this.renderer = renderer;
        }
        // Java: 68
        public com.oddlabs.tt.gui.GUIRoot getGUIRoot()
        {
            return current_root;
        }
        // Java: 72
        public com.oddlabs.tt.gui.Fade getFade()
        {
            return fade;
        }
        // Java: 77
        public void render(com.oddlabs.tt.viewer.AmbientAudio ambient)
        {
            org.joml.Matrix4f proj = new org.joml.Matrix4f();
            org.joml.Matrix4f modelView = new org.joml.Matrix4f();
            com.oddlabs.tt.gui.GUIRoot guiRoot = getGUIRoot();
            com.oddlabs.tt.camera.CameraState camera = guiRoot.getDelegate().getCamera().getState();
            com.oddlabs.tt.render.Renderer renderer_instance = com.oddlabs.tt.render.Renderer.getRenderer();
            com.oddlabs.tt.render.state.RenderContext context = renderer_instance.getRenderContext();
            camera.setView(guiRoot.multProjection(proj.identity()), renderer_instance.getDisplayWidth(), renderer_instance.getDisplayHeight());
            modelView.set(camera.getModelView());
            if (!com.oddlabs.tt.global.Globals.frustum_freeze)
            {
                frustum_state.set(camera);
            }
            if (renderer != null)
            {
                renderer.startFrame(context);
            }
            else
            {
                context.clear(true, true);
            }
            if (renderer != null)
            {
                renderer.render(context, ambient, frustum_state, current_root);
            }
            if (renderer != null)
            {
                renderer.endFrame(context, new java.util.function.Consumer_L((object a0) => { this.renderGUI(((com.oddlabs.tt.render.state.RenderContext)a0)); }));
            }
            else
            {
                renderGUI(context);
            }
        }
        // Java: 110
        public void pickHover()
        {
            com.oddlabs.tt.gui.GUIRoot guiRoot = getGUIRoot();
            com.oddlabs.tt.camera.CameraState camera = guiRoot.getDelegate().getCamera().getState();
            com.oddlabs.tt.gui.GUIObject gui_hit = guiRoot.getCurrentGUIObject();
            if (renderer != null)
            {
                com.oddlabs.tt.gui.LocalInput localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
                renderer.pickHover(gui_hit.canHoverBehind(), camera, localInput.getMouseX(), localInput.getMouseY());
            }
        }
        // Java: 120
        public void renderGUI(com.oddlabs.tt.render.state.RenderContext context)
        {
            com.oddlabs.tt.gui.GUIRoot guiRoot = getGUIRoot();
            guiRenderer.renderFrame(context, guiRoot.getWidth(), guiRoot.getHeight(), new java.lang.Runnable_L(() =>
            {
                guiRoot.render(guiRenderer);
                guiRoot.renderTopmost(guiRenderer, (renderer != null ? renderer.getToolTip() : (com.oddlabs.tt.gui.ToolTip)(null)), renderer != null && renderer.isCheater());
            }));
        }
        private void init_GUI()
        {
            this.frustum_state = new com.oddlabs.tt.camera.CameraState();
        }
        public GUI() : base()
        {
            init_GUI();
            this.guiRenderer = new com.oddlabs.tt.render.GUIRenderer();
            this.current_root = createRoot();
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
