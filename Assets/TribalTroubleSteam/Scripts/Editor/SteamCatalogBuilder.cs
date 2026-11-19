using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TribalTroubleSteam.Presentation;
using UnityEditor;
using UnityEngine;

namespace TribalTroubleSteam.EditorTools
{
    /// <summary>
    /// The Steam release's sprite catalog: builds its TTSpriteDefinition assets + the runtime catalog
    /// (Resources/TTSteam/SpriteCatalog) from the converter output (Assets/TribalTroubleSteam/Art/Models/catalog.json,
    /// made by PortTools/tt_geometry_to_gltf.py from the Steam geometry, and the .glb files imported by glTFast).
    /// Runs by itself when catalog.json is newer than the catalog (SteamAutoSetup).
    /// </summary>
    static class SteamCatalogBuilder
    {
        const string ModelsDir = "Assets/TribalTroubleSteam/Art/Models";
        const string TexturesDir = "Assets/TribalTroubleSteam/Art/Textures";
        const string DefinitionsDir = "Assets/TribalTroubleSteam/Data/Sprites";
        const string MaterialPath = "Assets/TribalTroubleSteam/Materials/TTSteamSprite.mat";
        public const string CatalogPath = "Assets/TribalTroubleSteam/Resources/TTSteam/SpriteCatalog.asset";
        public const string CatalogJsonPath = ModelsDir + "/catalog.json";

        [MenuItem("Tools/Tribal Trouble (Steam)/Build Sprite Catalog", priority = 10)]
        public static void Build()
        {
            var jsonPath = ModelsDir + "/catalog.json";
            if (!File.Exists(jsonPath))
            {
                SteamAutoSetup.Log("Missing " + jsonPath + ": run PortTools/tt_geometry_to_gltf.py on the Steam geometry first.");
                return;
            }
            var catalogJson = JsonUtility.FromJson<CatalogJson>(File.ReadAllText(jsonPath));

            EnsureFolder(DefinitionsDir);
            EnsureFolder(Path.GetDirectoryName(MaterialPath));
            EnsureFolder(Path.GetDirectoryName(CatalogPath));

            var baseMaterial = FromFork(MaterialPath, "Assets/TribalTrouble/Materials/TTSprite.mat");
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
                        EditorUtility.DisplayProgressBar("Tribal Trouble (Steam)", $"{group.name}/{sprite.name}", (float)done++ / total);
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
                SteamAutoSetup.Log("warning: " + p);
            Debug.Log($"[Tribal Trouble (Steam)] Sprite catalog built: {catalog.sprites.Count} sprites, {problems.Count} warnings.");
        }

        const string TerrainMaterialPath = "Assets/TribalTroubleSteam/Materials/TTSteamTerrain.mat";
        const string WaterMaterialPath = "Assets/TribalTroubleSteam/Materials/TTSteamWater.mat";
        const string SfxDir = "Assets/TribalTroubleSteam/Audio/Sfx";
        const string MusicDir = "Assets/TribalTroubleSteam/Audio/Music";

        /// <summary>Terrain / water materials and audio clips the skirmish game needs at runtime.</summary>
        static void AddGameAssets(TTSpriteCatalog catalog, List<string> problems)
        {
            catalog.terrainMaterial = EnsureMaterial(TerrainMaterialPath, "TribalTrouble/Terrain", problems);
            catalog.waterMaterial = EnsureMaterial(WaterMaterialPath, "TribalTrouble/Water", problems);
            catalog.sfx = LoadClips(SfxDir);
            catalog.music = LoadClips(MusicDir);
            if (catalog.sfx.Count == 0) problems.Add("No sound effects found in " + SfxDir);
        }

        /// <summary>The Steam game's own copy of a material the 1.4 game's set-up tuned (the same shader and settings),
        /// kept in step with it; null when there is no such material in this project.</summary>
        static Material FromFork(string path, string forkPath)
        {
            var fork = AssetDatabase.LoadAssetAtPath<Material>(forkPath);
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (fork == null) return mat;
            if (mat == null) { AssetDatabase.CopyAsset(forkPath, path); return AssetDatabase.LoadAssetAtPath<Material>(path); }
            EditorUtility.CopySerialized(fork, mat);
            mat.name = Path.GetFileNameWithoutExtension(path);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        static Material EnsureMaterial(string path, string shaderName, List<string> problems)
        {
            var mat = FromFork(path, "Assets/TribalTrouble/Materials/" + Path.GetFileName(path).Replace("TTSteam", "TT"));
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

            var assetPath = $"{DefinitionsDir}/steam_{groupName}_{sprite.name}.asset";
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
            if (name != null && name.StartsWith("Generator:")) return null;   // made at run time (TTGeneratedTextures)
            var path = $"{TexturesDir}/{folder}/{name}.png";
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex == null)
                problems.Add($"{owner}: texture not found: {path}");
            return tex;
        }

        static void EnsureFolder(string path)
        {
            path = path.Replace('\\', '/');
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
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
