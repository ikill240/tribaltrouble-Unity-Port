using UnityEditor;

namespace TribalTrouble.EditorTools
{
    /// <summary>Import settings for the original TT textures.</summary>
    sealed class TextureImportRules : AssetPostprocessor
    {
        const string Root = "Assets/TribalTrouble/Art/Textures/";

        public override uint GetVersion() => 2;   // (2: mipmaps preserve the cut-outs' coverage)

        void OnPreprocessTexture()
        {
            if (assetPath.StartsWith("Assets/TribalTrouble/Resources/TTReal/"))
            {
                // generated detail / normal / noise maps for the Modern look: data, not colour (linear), tiling, mipmapped
                var real = (TextureImporter)assetImporter;
                real.textureType = TextureImporterType.Default;
                real.sRGBTexture = false;
                real.mipmapEnabled = true;
                real.alphaIsTransparency = false;
                real.wrapMode = UnityEngine.TextureWrapMode.Repeat;
                real.filterMode = UnityEngine.FilterMode.Trilinear;
                real.anisoLevel = 8;
                real.maxTextureSize = 2048;
                real.textureCompression = TextureImporterCompression.CompressedHQ;
                return;
            }
            if (assetPath.StartsWith("Assets/TribalTrouble/Resources/TTGui/"))
            {
                // GUI atlases drawn by IMGUI: crisp, uncompressed, no resizing (icons.xml regions are in pixels).
                var gui = (TextureImporter)assetImporter;
                gui.textureType = TextureImporterType.Default;
                gui.mipmapEnabled = false;
                gui.alphaIsTransparency = true;
                gui.npotScale = TextureImporterNPOTScale.None;
                gui.textureCompression = TextureImporterCompression.Uncompressed;
                gui.maxTextureSize = 4096;
                gui.wrapMode = UnityEngine.TextureWrapMode.Clamp;
                return;
            }
            if (!assetPath.StartsWith(Root))
                return;
            var importer = (TextureImporter)assetImporter;

            if (assetPath.StartsWith(Root + "teamdecals/"))
            {
                // Used as a blend factor (mix(base, teamColour, decal.rgb)), not as a colour.
                importer.sRGBTexture = false;
                importer.mipmapEnabled = true;
            }
            else if (assetPath.StartsWith(Root + "gui/") || assetPath.StartsWith(Root + "pointer/") ||
                     assetPath.StartsWith(Root + "pixelperfect/") || assetPath.StartsWith(Root + "font/"))
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = true;
                importer.npotScale = TextureImporterNPOTScale.None;
            }
            else if (assetPath.StartsWith(Root + "models/"))
            {
                importer.mipmapEnabled = true;
                importer.alphaIsTransparency = true;
                importer.maxTextureSize = 2048;
                // the alpha-tested cut-outs (feathers, lures, leaves) keep their coverage in the smaller mipmaps, so they
                // don't thin away or fringe as the camera zooms out
                importer.mipMapsPreserveCoverage = true;
                importer.alphaTestReferenceValue = 0.5f;
            }
        }
    }
}
