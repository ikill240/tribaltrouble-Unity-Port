// Translated from StructureBlend.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 9
    public class StructureBlend : com.oddlabs.tt.resource.BlendInfo
    {
        public com.oddlabs.tt.render.Texture structure_map;
        public com.oddlabs.tt.render.Texture normal_map;
        // Java: 13
        public com.oddlabs.tt.render.Texture createStructureMap(com.oddlabs.tt.resource.GLIntImage structure_image)
        {
            return new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLIntImage[] { structure_image }, org.lwjgl.opengl.GL11.GL_RGB, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT);
        }
        // Java: 24
        public com.oddlabs.tt.render.Texture getStructureMap()
        {
            return structure_map;
        }
        // Java: 28
        public com.oddlabs.tt.render.Texture getNormalMap()
        {
            return normal_map;
        }
        // Java: 32
        public void bindStructure()
        {
            org.lwjgl.opengl.GL11.glBindTexture(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, structure_map.getHandle());
        }
        // Java: 36
        public override void setup()
        {
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE1);
            bindAlpha();
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE0);
            bindStructure();
        }
        // Java: 44
        public override void reset()
        {
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE1);
            org.lwjgl.opengl.GL13.glActiveTexture(org.lwjgl.opengl.GL13.GL_TEXTURE0);
        }
        public StructureBlend(com.oddlabs.tt.resource.GLIntImage structure_image, com.oddlabs.tt.resource.GLIntImage normal_image, com.oddlabs.tt.resource.GLByteImage alpha_image) : base(alpha_image, com.oddlabs.tt.global.Globals.COMPRESSED_A_FORMAT)
        {
            structure_map = createStructureMap(structure_image);
            normal_map = createStructureMap(normal_image);
        }
    }
}
