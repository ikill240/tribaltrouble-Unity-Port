using UnityEditor;
using UnityEngine;

namespace TribalTroubleSteam.EditorTools
{
    /// <summary>Import settings for the Steam release's textures: its GUI (Resources/TTSteam/Java, drawn 1:1 by the Java
    /// GUI: no compression, mipmaps or resizing; the pointers are cursors), its model textures (mipmapped) and team
    /// decals (a blend factor, not a colour: linear).</summary>
    sealed class SteamTextureImport : AssetPostprocessor
    {
        const string Gui = "Assets/TribalTroubleSteam/Resources/TTSteam/Java/";
        const string Art = "Assets/TribalTroubleSteam/Art/Textures/";

        public override uint GetVersion() => 2;   // (2: mipmaps preserve the cut-outs' coverage)

        void OnPreprocessTexture()
        {
            string path = assetPath.Replace('\\', '/');
            var ti = (TextureImporter)assetImporter;
            if (path.StartsWith(Gui))
            {
                bool cursor = path.Contains("/pointer_");
                ti.textureType = cursor ? TextureImporterType.Cursor : TextureImporterType.Default;
                ti.mipmapEnabled = false;
                ti.textureCompression = TextureImporterCompression.Uncompressed;
                ti.wrapMode = TextureWrapMode.Clamp;
                ti.filterMode = FilterMode.Bilinear;
                ti.alphaIsTransparency = true;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.maxTextureSize = 8192;
                ti.sRGBTexture = true;
                ti.isReadable = cursor;
                // particle textures (stun notes, star) are drawn in the world, mipmapped like the Java's
                if (path.Contains("/textures/effects/")) ti.mipmapEnabled = true;
                return;
            }
            if (path.StartsWith(Art + "teamdecals/"))
            {
                ti.sRGBTexture = false;
                ti.mipmapEnabled = true;
            }
            else if (path.StartsWith(Art + "models/"))
            {
                ti.mipmapEnabled = true;
                ti.alphaIsTransparency = true;
                ti.maxTextureSize = 2048;
                // the alpha-tested cut-outs (feathers, lures, leaves) keep their coverage in the smaller mipmaps, so they
                // don't thin away or fringe as the camera zooms out
                ti.mipMapsPreserveCoverage = true;
                ti.alphaTestReferenceValue = 0.5f;
            }
        }
    }
}
