using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TribalTrouble.Presentation;
using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// Builds TTSpriteDefinition assets + the runtime SpriteCatalog from the converter output
    /// (Art/Models/catalog.json and the .glb files imported by glTFast).
    /// </summary>
    static class SpriteCatalogBuilder
    {
        const string ModelsDir = "Assets/TribalTrouble/Art/Models";
        const string TexturesDir = "Assets/TribalTrouble/Art/Textures";
        const string DefinitionsDir = "Assets/TribalTrouble/Data/Sprites";
        const string MaterialPath = "Assets/TribalTrouble/Materials/TTSprite.mat";
        const string CatalogPath = "Assets/TribalTrouble/Resources/SpriteCatalog.asset";

        [MenuItem("Tools/Tribal Trouble/Build Sprite Catalog", priority = 10)]
        public static void Build()
        {
            var jsonPath = ModelsDir + "/catalog.json";
            if (!File.Exists(jsonPath))
            {
                EditorUtility.DisplayDialog("Tribal Trouble", "Missing " + jsonPath +
                    "\nRun the converter (Tools~/tt_geometry_to_gltf.py) first.", "OK");
                return;
            }
            var catalogJson = JsonUtility.FromJson<CatalogJson>(File.ReadAllText(jsonPath));

            ProjectSetup.EnsureFolder(DefinitionsDir);
            ProjectSetup.EnsureFolder(Path.GetDirectoryName(MaterialPath));
            ProjectSetup.EnsureFolder(Path.GetDirectoryName(CatalogPath));

            var baseMaterial = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
            if (baseMaterial == null)
            {
                var shader = Shader.Find("TribalTrouble/Sprite");
                if (shader == null)
                {
                    Debug.LogError("[Tribal Trouble] Shader 'TribalTrouble/Sprite' not found or failed to compile.");
                    return;
                }
                baseMaterial = new Material(shader) { name = "TTSprite" };
                AssetDatabase.CreateAsset(baseMaterial, MaterialPath);
            }

            var catalog = AssetDatabase.LoadAssetAtPath<TTSpriteCatalog>(CatalogPath);
            if (catalog == null)
            {
                catalog = ScriptableObject.CreateInstance<TTSpriteCatalog>();
                AssetDatabase.CreateAsset(catalog, CatalogPath);
            }
            catalog.baseMaterial = baseMaterial;
            catalog.sprites.Clear();

            var problems = new List<string>();
            try
            {
                int total = catalogJson.groups.Sum(g => g.sprites.Length), done = 0;
                foreach (var group in catalogJson.groups)
                {
                    foreach (var sprite in group.sprites)
                    {
                        EditorUtility.DisplayProgressBar("Tribal Trouble", $"{group.name}/{sprite.name}", (float)done++ / total);
                        var def = BuildDefinition(group.name, sprite, problems);
                        if (def != null)
                            catalog.sprites.Add(def);
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            AddGameAssets(catalog, problems);

            EditorUtility.SetDirty(catalog);
            AssetDatabase.SaveAssets();
            foreach (var p in problems)
                Debug.LogWarning("[Tribal Trouble] " + p);
            Debug.Log($"[Tribal Trouble] Sprite catalog built: {catalog.sprites.Count} sprites, {problems.Count} warnings.");
            Selection.activeObject = catalog;
        }

        const string TerrainMaterialPath = "Assets/TribalTrouble/Materials/TTTerrain.mat";
        const string WaterMaterialPath = "Assets/TribalTrouble/Materials/TTWater.mat";
        const string SfxDir = "Assets/TribalTrouble/Audio/Sfx";
        const string MusicDir = "Assets/TribalTrouble/Audio/Music";

        /// <summary>Terrain / water materials and audio clips the skirmish game needs at runtime.</summary>
        static void AddGameAssets(TTSpriteCatalog catalog, List<string> problems)
        {
            catalog.terrainMaterial = EnsureMaterial(TerrainMaterialPath, "TribalTrouble/Terrain", problems);
            catalog.waterMaterial = EnsureMaterial(WaterMaterialPath, "TribalTrouble/Water", problems);
            catalog.sfx = LoadClips(SfxDir);
            catalog.music = LoadClips(MusicDir);
            if (catalog.sfx.Count == 0) problems.Add("No sound effects found in " + SfxDir);
        }

        static Material EnsureMaterial(string path, string shaderName, List<string> problems)
        {
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null) return mat;
            var shader = Shader.Find(shaderName);
            if (shader == null)
            {
                problems.Add($"Shader '{shaderName}' not found or failed to compile.");
                return null;
            }
            mat = new Material(shader) { name = Path.GetFileNameWithoutExtension(path) };
            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }

        static List<AudioClip> LoadClips(string dir)
        {
            if (!AssetDatabase.IsValidFolder(dir)) return new List<AudioClip>();
            return AssetDatabase.FindAssets("t:AudioClip", new[] { dir })
                .Select(g => AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(g)))
                .Where(c => c != null).OrderBy(c => c.name).ToList();
        }

        static TTSpriteDefinition BuildDefinition(string groupName, SpriteJson sprite, List<string> problems)
        {
            var glbPath = ModelsDir + "/" + sprite.file;
            var model = AssetDatabase.LoadAssetAtPath<GameObject>(glbPath);
            if (model == null)
            {
                problems.Add($"{glbPath}: not imported as a GameObject (is glTFast installed?)");
                return null;
            }
            var clips = AssetDatabase.LoadAllAssetsAtPath(glbPath).OfType<AnimationClip>()
                .Where(c => !c.name.StartsWith("__preview__")).ToList();

            var assetPath = $"{DefinitionsDir}/{groupName}_{sprite.name}.asset";
            var def = AssetDatabase.LoadAssetAtPath<TTSpriteDefinition>(assetPath);
            if (def == null)
            {
                def = ScriptableObject.CreateInstance<TTSpriteDefinition>();
                AssetDatabase.CreateAsset(def, assetPath);
            }
            def.group = groupName;
            def.spriteName = sprite.name;
            def.model = model;
            def.skinned = sprite.skinned;

            def.lods = sprite.models.Select(m => new TTSpriteDefinition.LodInfo
            {
                nodeName = "lod" + m.lod,
                clearColor = m.clearColor != null && m.clearColor.Length >= 3
                    ? new Color(m.clearColor[0], m.clearColor[1], m.clearColor[2])
                    : Color.black,
                textures = m.textures.Select(t => new TTSpriteDefinition.TextureVariant
                {
                    name = t.name,
                    baseMap = LoadTexture("models", t.name, problems, $"{groupName}/{sprite.name}"),
                    teamMask = string.IsNullOrEmpty(t.team)
                        ? null
                        : LoadTexture("teamdecals", t.team, problems, $"{groupName}/{sprite.name}"),
                }).ToArray(),
                alphaClip = m.textures.Any(t => NeedsAlphaClip(t.name)),
            }).ToArray();

            def.animations = sprite.animations.Select((a, index) =>
            {
                var clip = clips.FirstOrDefault(c => string.Equals(c.name, a.name, StringComparison.OrdinalIgnoreCase))
                           ?? clips.FirstOrDefault(c => c.name.EndsWith(a.name, StringComparison.OrdinalIgnoreCase))
                           ?? (index < clips.Count ? clips[index] : null);
                if (clip == null)
                    problems.Add($"{groupName}/{sprite.name}: animation clip '{a.name}' not found in {glbPath}");
                return new TTSpriteDefinition.AnimInfo
                {
                    name = a.name,
                    clip = clip,
                    wpc = a.wpc,
                    loop = a.type == "loop",
                    frames = a.frames,
                };
            }).ToArray();

            EditorUtility.SetDirty(def);
            return def;
        }

        static bool NeedsAlphaClip(string textureName)
        {
            var n = textureName.ToLowerInvariant();
            return n.Contains("tree") || n.Contains("plant");
        }

        static Texture2D LoadTexture(string folder, string name, List<string> problems, string owner)
        {
            var path = $"{TexturesDir}/{folder}/{name}.png";
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex == null)
                problems.Add($"{owner}: texture not found: {path}");
            return tex;
        }

        // ---- catalog.json schema (written by tt_geometry_to_gltf.py) ----
#pragma warning disable 0649
        [Serializable] class CatalogJson { public GroupJson[] groups; }
        [Serializable] class GroupJson { public string name; public SpriteJson[] sprites; }

        [Serializable]
        class SpriteJson
        {
            public string name;
            public string file;
            public float scale;
            public bool skinned;
            public ModelJson[] models;
            public AnimJson[] animations;
        }

        [Serializable]
        class ModelJson
        {
            public int lod;
            public string source;
            public TextureJson[] textures;
            public float[] clearColor;
            public int vertices;
            public int triangles;
        }

        [Serializable] class TextureJson { public string name; public string team; }
        [Serializable] class AnimJson { public string name; public float wpc; public string type; public int frames; }
#pragma warning restore 0649
    }
}
