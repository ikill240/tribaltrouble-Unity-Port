// Translated from LandscapeBaker.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 24
    public class LandscapeBaker : java.lang.Object
    {
        public const string VERTEX_SHADER = "#version 410 core\nlayout(location = 0) in vec2 in_Position;\nlayout(location = 1) in vec2 in_TexCoord;\nout vec2 v_texCoord;\nvoid main() {\n    gl_Position = vec4(in_Position, 0.0, 1.0);\n    v_texCoord = in_TexCoord;\n}\n";
        public const string FRAGMENT_SHADER = "#version 410 core\nuniform sampler2D u_BaseDiffuse;\nuniform sampler2D u_LayerDiffuse;\nuniform sampler2D u_BaseNormal;\nuniform sampler2D u_LayerNormal;\nuniform sampler2D u_AlphaMap;\nuniform int u_Mode; // 0 = Blend, 1 = Light\nuniform float u_TextureScale;\nuniform vec3 u_Color;\n\nin vec2 v_texCoord;\n\nlayout(location = 0) out vec4 out_Diffuse;\nlayout(location = 1) out vec4 out_Normal;\n\nvoid main() {\n    vec4 baseDiff = texture(u_BaseDiffuse, v_texCoord);\n    vec4 baseNorm = texture(u_BaseNormal, v_texCoord);\n    float alpha = texture(u_AlphaMap, v_texCoord).r;\n\n    if (u_Mode == 0) { // Structure Blend\n        vec4 layerDiff = texture(u_LayerDiffuse, v_texCoord * u_TextureScale);\n        vec4 layerNorm = texture(u_LayerNormal, v_texCoord * u_TextureScale);\n\n        out_Diffuse = mix(baseDiff, layerDiff, alpha);\n        out_Normal = mix(baseNorm, layerNorm, alpha);\n    } else { // Lighting Blend\n        // Multiplicative-additive blend to match original glBlendFunc(GL_DST_COLOR, GL_ONE).\n        out_Diffuse = baseDiff + baseDiff * vec4(u_Color * alpha, 0.0);\n        out_Normal = baseNorm;         // Keep normals\n    }\n}\n";
        // Java: 72
        public class BlendShader : com.oddlabs.tt.render.shader.ShaderProgram
        {
            public BlendShader() : base(com.oddlabs.tt.landscape.LandscapeBaker.VERTEX_SHADER, com.oddlabs.tt.landscape.LandscapeBaker.FRAGMENT_SHADER)
            {
                link();
            }
        }
        // Java: 80
        public com.oddlabs.tt.resource.WorldInfo.Maps bake(int colormapSize, float textureScale, com.oddlabs.tt.resource.BlendInfo[] blendInfos)
        {
            return new com.oddlabs.tt.resource.WorldInfo.Maps(null, null);
        }
        public LandscapeBaker() : base()
        {
        }
    }
}
