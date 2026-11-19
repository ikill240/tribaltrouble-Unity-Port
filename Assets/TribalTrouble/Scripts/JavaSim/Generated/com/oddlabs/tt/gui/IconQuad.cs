// Translated from IconQuad.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 13
    public class IconQuad : com.oddlabs.util.Quad
    {
        public com.oddlabs.tt.render.Texture atlas;
        // Java: 36
        public static float check(float u1, float v1, float u2, float v2, int width, int height)
        {
            if (!java.lang.Float.isFinite(u1) || !java.lang.Float.isFinite(v1) || !java.lang.Float.isFinite(u2) || !java.lang.Float.isFinite(v2))
            {
                throw new System.ArgumentException("Texture coordinates must be finite numbers.");
            }
            if (u1 > u2 || v1 > v2)
            {
                throw new System.ArgumentException("u1/v1 must be less than or equal to u2/v2, but got: u1=" + java.lang.JStr.valueOf(u1) + ", v1=" + java.lang.JStr.valueOf(v1) + ", u2=" + java.lang.JStr.valueOf(u2) + ", v2=" + java.lang.JStr.valueOf(v2));
            }
            if (width < 0 || height < 0)
            {
                throw new System.ArgumentException("Width and height must be non-negative. Got: width=" + (width) + ", height=" + (height));
            }
            return u1;
        }
        // Java: 68
        public static com.oddlabs.tt.render.Texture getCheckedTexture(int x, int y, int width, int height, com.oddlabs.tt.render.Texture texture)
        {
            java.util.Objects.requireNonNull(texture, "Texture cannot be null");
            java.util.Objects.checkFromIndexSize(x, width, texture.getWidth());
            java.util.Objects.checkFromIndexSize(y, height, texture.getHeight());
            return texture;
        }
        // Java: 75
        public com.oddlabs.tt.render.Texture getTexture()
        {
            return atlas;
        }
        public IconQuad(float u1, float v1, float u2, float v2, int width, int height, com.oddlabs.tt.render.Texture texture) : base(com.oddlabs.tt.gui.IconQuad.check(u1, v1, u2, v2, width, height), v1, u2, v2, width, height)
        {
            this.atlas = ((com.oddlabs.tt.render.Texture)java.util.Objects.requireNonNull(texture));
        }
        public IconQuad(int x, int y, int width, int height, com.oddlabs.tt.render.Texture texture) : this((float)(((float)(x)) / com.oddlabs.tt.gui.IconQuad.getCheckedTexture(x, y, width, height, texture).getWidth()), (float)(((float)(y)) / texture.getHeight()), (float)(((float)((x + width))) / texture.getWidth()), (float)(((float)((y + height))) / texture.getHeight()), width, height, texture)
        {
        }
    }
}
