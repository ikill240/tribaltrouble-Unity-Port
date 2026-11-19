// Translated from BlendInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 7
    public abstract class BlendInfo : java.lang.Object
    {
        public com.oddlabs.tt.render.Texture alpha_map;
        public com.oddlabs.tt.resource.GLByteImage sourceImage;
        // Java: 11
        public com.oddlabs.tt.render.Texture createAlphaMap(com.oddlabs.tt.resource.GLByteImage alpha_image, int format)
        {
            com.oddlabs.tt.resource.GLImage[] mipmaps = alpha_image.buildMipMaps(0, 1.0f, true, false);
            return new com.oddlabs.tt.render.Texture(mipmaps, format, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT);
        }
        // Java: 22
        public virtual com.oddlabs.tt.render.Texture getAlphaMap()
        {
            return alpha_map;
        }
        // Java: 26
        public virtual com.oddlabs.tt.resource.GLByteImage getSourceImage()
        {
            return sourceImage;
        }
        // Java: 34
        public void bindAlpha()
        {
            org.lwjgl.opengl.GL11.glBindTexture(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, alpha_map.getHandle());
        }
        // Java: 38
        public abstract void setup();
        // Java: 40
        public abstract void reset();
        public BlendInfo(com.oddlabs.tt.resource.GLByteImage alpha_image, int format) : base()
        {
            this.sourceImage = alpha_image;
            alpha_map = createAlphaMap(alpha_image, format);
        }
    }
}
