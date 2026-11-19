using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.Presentation
{
    /// <summary>All sprite definitions, loadable at runtime via Resources.Load("SpriteCatalog").</summary>
    public sealed class TTSpriteCatalog : ScriptableObject
    {
        public const string ResourcePath = "SpriteCatalog";

        [Tooltip("Material using TribalTrouble/Sprite; referencing it here keeps the shader in builds.")]
        public Material baseMaterial;

        public List<TTSpriteDefinition> sprites = new List<TTSpriteDefinition>();

        [Header("Game")]
        public Material terrainMaterial;
        public Material waterMaterial;
        public List<AudioClip> sfx = new List<AudioClip>();
        public List<AudioClip> music = new List<AudioClip>();

        public TTSpriteDefinition Find(string group, string spriteName)
        {
            foreach (var s in sprites)
                if (s != null && s.group == group && s.spriteName == spriteName)
                    return s;
            return null;
        }

        public static TTSpriteCatalog Load() => Resources.Load<TTSpriteCatalog>(ResourcePath);
    }
}
