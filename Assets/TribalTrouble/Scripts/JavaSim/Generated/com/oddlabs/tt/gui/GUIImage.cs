// Translated from GUIImage.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 12
    public class GUIImage : com.oddlabs.tt.gui.GUIIcon
    {
        public bool clickable;
        // Java: 60
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return (clickable ? com.oddlabs.tt.gui.CursorType.TARGET : com.oddlabs.tt.gui.CursorType.NORMAL);
        }
        public GUIImage(int width, int height, float u1, float v1, float u2, float v2, string texture_name) : this(width, height, u1, v1, u2, v2, ((com.oddlabs.tt.render.Texture)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.TextureFile(texture_name, org.lwjgl.opengl.GL11.GL_RGBA, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT))), false)
        {
        }
        public GUIImage(int width, int height, float u1, float v1, float u2, float v2, string texture_name, bool clickable) : this(width, height, u1, v1, u2, v2, ((com.oddlabs.tt.render.Texture)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.resource.TextureFile(texture_name, org.lwjgl.opengl.GL11.GL_RGBA, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT))), clickable)
        {
        }
        public GUIImage(int width, int height, float u1, float v1, float u2, float v2, com.oddlabs.tt.render.Texture texture, bool clickable) : base(new com.oddlabs.tt.gui.IconQuad(u1, v1, u2, v2, (width < 0 || height < 0 ? texture.getWidth() : width), (width < 0 || height < 0 ? texture.getHeight() : height), texture))
        {
            this.clickable = clickable;
            if (clickable)
            {
                setCanFocus(true);
            }
        }
    }
}
