// Translated from BlendLighting.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 7
    public class BlendLighting : com.oddlabs.tt.resource.BlendInfo
    {
        public float r;
        public float g;
        public float b;
        // Java: 20
        public float getR()
        {
            return r;
        }
        // Java: 24
        public float getG()
        {
            return g;
        }
        // Java: 28
        public float getB()
        {
            return b;
        }
        // Java: 32
        public override void setup()
        {
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE1);
            bindAlpha();
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE0);
        }
        // Java: 39
        public override void reset()
        {
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE1);
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE0);
        }
        public BlendLighting(com.oddlabs.tt.resource.GLByteImage alpha_image, float r, float g, float b) : base(alpha_image, com.oddlabs.tt.global.Globals.COMPRESSED_LUMINANCE_FORMAT)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}
