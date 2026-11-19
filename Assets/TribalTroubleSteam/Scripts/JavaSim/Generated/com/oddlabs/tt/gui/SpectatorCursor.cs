// Translated from SpectatorCursor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 20
    public class SpectatorCursor : com.oddlabs.tt.gui.GUIObject
    {
        public const string POINTER_TEXTURE = "/textures/gui/pointer_32_8";
        public const string TARGET_TEXTURE = "/textures/gui/pointer_target_32_8";
        public const float SMOOTHNESS_FACTOR = 15.0f;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public java.util.function.BooleanSupplier visible;
        public com.oddlabs.tt.render.Texture texture;
        public com.oddlabs.tt.render.Texture target_texture;
        public org.joml.Vector4f point;
        public org.joml.Vector4f color;
        public bool shown;
        public long time;
        public float x;
        public float y;
        // Java: 48
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
        }
        // Java: 53
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.viewer.SpectatorView view = viewer.getSpectatorView();
            com.oddlabs.tt.player.Player followed = (view != null && visible.getAsBoolean() ? view.getFollowedPlayer() : (com.oddlabs.tt.player.Player)(null));
            com.oddlabs.tt.viewer.PlayerView player_view = (followed != null ? view.getView(followed) : (com.oddlabs.tt.viewer.PlayerView)(null));
            if (player_view == null || !player_view.isCursorOnMap())
            {
                shown = false;
                return;
            }
            long now = java.lang.JSystem.nanoTime();
            if (shown)
            {
                float k = java.lang.Math.min((float)((float)((now - time) / 1.0E9f) * com.oddlabs.tt.gui.SpectatorCursor.SMOOTHNESS_FACTOR), 1.0f);
                x += (float)(((float)(player_view.getCursorX() - x)) * k);
                y += (float)(((float)(player_view.getCursorY() - y)) * k);
            }
            else
            {
                x = player_view.getCursorX();
                y = player_view.getCursorY();
                shown = true;
            }
            time = now;
            float z = viewer.getWorld().getHeightMap().getNearestHeight(x, y);
            gui_root.projectToScreen(x, y, z, point);
            if (point.w_f < com.oddlabs.tt.gui.GUIRoot.MIN_PROJECTED_W)
            {
                return;
            }
            float screen_x = point.x_f;
            float screen_y = point.y_f;
            org.joml.Vector4fc player_color = followed.getColor();
            color.set(player_color.x(), player_color.y(), player_color.z(), 1.0f);
            com.oddlabs.tt.render.Texture shown_texture = (player_view.isTargeting() ? target_texture : texture);
            int hot_x = (player_view.isTargeting() ? com.oddlabs.tt.input.PointerInput.TARGET_HOT_X : com.oddlabs.tt.input.PointerInput.NORMAL_HOT_X);
            int hot_y = (player_view.isTargeting() ? com.oddlabs.tt.input.PointerInput.TARGET_HOT_Y : com.oddlabs.tt.input.PointerInput.NORMAL_HOT_Y);
            renderer.drawTexture(shown_texture, (float)(screen_x - hot_x), (float)(screen_y + hot_y), shown_texture.getWidth(), -shown_texture.getHeight(), 0.0f, 0.0f, 1.0f, 1.0f, color);
        }
        private void init_SpectatorCursor()
        {
            this.point = new org.joml.Vector4f();
            this.color = new org.joml.Vector4f();
        }
        public SpectatorCursor(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.gui.GUIRoot gui_root, java.util.function.BooleanSupplier visible) : base()
        {
            init_SpectatorCursor();
            this.viewer = viewer;
            this.gui_root = gui_root;
            this.visible = visible;
            this.texture = ((com.oddlabs.tt.render.Texture)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.TextureFile(com.oddlabs.tt.gui.SpectatorCursor.POINTER_TEXTURE, org.lwjgl.opengl.GL11.GL_RGBA, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT)));
            this.target_texture = ((com.oddlabs.tt.render.Texture)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.TextureFile(com.oddlabs.tt.gui.SpectatorCursor.TARGET_TEXTURE, org.lwjgl.opengl.GL11.GL_RGBA, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT)));
            displayChangedNotify(gui_root.getWidth(), gui_root.getHeight());
        }
    }
}
