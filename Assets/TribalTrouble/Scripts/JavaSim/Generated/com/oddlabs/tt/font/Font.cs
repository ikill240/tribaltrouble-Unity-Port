// Translated from Font.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.font
{
    // Java: 14
    public class Font : java.lang.Object
    {
        public com.oddlabs.util.HashTable key_map;
        public com.oddlabs.tt.render.Texture texture;
        public int x_border;
        public int y_border;
        public int height;
        public int max_ascension;
        public int max_descension;
        // Java: 39
        public com.oddlabs.util.Quad getQuad(int codepoint)
        {
            return ((com.oddlabs.util.Quad)key_map.get(codepoint));
        }
        // Java: 43
        public int getXBorder()
        {
            return x_border;
        }
        // Java: 47
        public int getYBorder()
        {
            return y_border;
        }
        // Java: 51
        public int getHeight()
        {
            return height;
        }
        // Java: 55
        public int getMaxAscension()
        {
            return max_ascension;
        }
        // Java: 59
        public int getMaxDescension()
        {
            return max_descension;
        }
        // Java: 63
        public com.oddlabs.tt.render.Texture getTexture()
        {
            return texture;
        }
        // Java: 67
        public int getWidestCodepoint(string text)
        {
            return text.codePoints().reduce(text.charAt(0), new java.util.function.IntBinaryOperator_L((int current, int codePoint) =>
            {
                com.oddlabs.util.Quad widest = getQuad(current);
                com.oddlabs.util.Quad quad = getQuad(codePoint);
                return (null != quad && quad.getWidth() > widest.getWidth() ? codePoint : current);
            }));
        }
        // Java: 78
        public int getWidth(string text)
        {
            if (text.isEmpty())
            {
                return 0;
            }
            int width = text.codePoints().reduce(0, new java.util.function.IntBinaryOperator_L((int current, int codePoint) =>
            {
                com.oddlabs.util.Quad quad = getQuad(codePoint);
                return current + ((null != quad ? quad.getWidth() - x_border : 0));
            }));
            return width + x_border;
        }
        public Font(com.oddlabs.util.FontInfo font_info) : base()
        {
            this.key_map = font_info.getKeyMap();
            com.oddlabs.tt.resource.TextureFile file = new com.oddlabs.tt.resource.TextureFile(font_info.getTextureName(), org.lwjgl.opengl.GL11.GL_RGBA, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE);
            this.texture = ((com.oddlabs.tt.render.Texture)com.oddlabs.tt.resource.Resources.findResource(file));
            this.x_border = font_info.getBorderX();
            this.y_border = font_info.getBorderY();
            this.height = font_info.getHeight();
            this.max_ascension = font_info.getMaxAscension();
            this.max_descension = font_info.getMaxDescension();
        }
    }
}
