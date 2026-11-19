using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    /// <summary>The original game's GUI textures (Resources/TTJava): drawn 1:1 by the Java GUI, so no compression or
    /// mipmaps, clamped, no resizing; the mouse pointers are cursor textures.</summary>
    sealed class TTJavaTextureImport : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (!assetPath.Replace('\\', '/').Contains("/Resources/TTJava/")) return;
            var ti = (TextureImporter)assetImporter;
            bool cursor = assetPath.Contains("/pointer_");
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
        }
    }
}
