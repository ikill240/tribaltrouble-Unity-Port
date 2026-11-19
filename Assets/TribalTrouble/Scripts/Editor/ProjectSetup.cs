using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// First-open setup: creates and assigns a URP pipeline asset if the project has none,
    /// and switches to Linear colour space. Safe to run again from the menu.
    /// </summary>
    [InitializeOnLoad]
    static class ProjectSetup
    {
        const string SettingsDir = "Assets/TribalTrouble/Settings";
        const string DoneKey = "TribalTrouble.ProjectSetup.Done";

        const int RenderingVersion = 2;
        const string RenderingKey = "TribalTrouble.ProjectSetup.Rendering";

        static ProjectSetup()
        {
            EditorApplication.delayCall += () =>
            {
                if (GraphicsSettings.defaultRenderPipeline == null && !SessionState.GetBool(DoneKey, false))
                {
                    SessionState.SetBool(DoneKey, true);
                    Run();
                }
                if (EditorPrefs.GetInt(RenderingKey + "." + Application.dataPath.GetHashCode(), 0) < RenderingVersion)
                    UpgradeRendering();
                EnsureAll();
            };
        }

        internal static void EnsureAll()
        {
            EnsureGpuSkinning();
            EnsureFastSsao();
            EnsureTreeMeshesReadable();
        }

        /// <summary>Units are skinned on the graphics card (compute, batched): with skinning on the CPU (the project's
        /// old setting) a big battle spent most of its frame deforming meshes.</summary>
        static void EnsureGpuSkinning()
        {
            if (PlayerSettings.meshDeformation == MeshDeformation.GPUBatched) return;
            PlayerSettings.meshDeformation = MeshDeformation.GPUBatched;
            AssetDatabase.SaveAssets();
            Debug.Log("[Tribal Trouble] Player settings: mesh skinning moved to the GPU (batched).");
        }

        /// <summary>Ambient occlusion worked out after the opaque pass from the depth copy, at half resolution, with
        /// the light blur: before the opaques it needs a depth pre-pass - the whole world drawn a second time, on the
        /// main thread and the graphics card - and at full resolution with the bilateral blur it cost ~4 ms of the GPU
        /// at 3440x1440 on its own.</summary>
        static void EnsureFastSsao()
        {
            var pipeline = GraphicsSettings.defaultRenderPipeline as UniversalRenderPipelineAsset;
            if (pipeline == null) return;
            var field = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (!(field?.GetValue(pipeline) is ScriptableRendererData[] list)) return;
            bool changed = false;
            foreach (var data in list)
            {
                if (data == null) continue;
                var dso = new SerializedObject(data);
                var cd = dso.FindProperty("m_CopyDepthMode");
                if (cd != null && cd.intValue != 0) { cd.intValue = 0; dso.ApplyModifiedPropertiesWithoutUndo(); changed = true; }   // after the opaques
                foreach (var f in data.rendererFeatures)
                {
                    if (f == null || f.GetType().Name != "ScreenSpaceAmbientOcclusion") continue;
                    var fs = new SerializedObject(f);
                    bool c = false;
                    void Set(string name, int v) { var p = fs.FindProperty("m_Settings." + name); if (p == null) return; if (p.propertyType == SerializedPropertyType.Boolean) { if (p.boolValue != (v != 0)) { p.boolValue = v != 0; c = true; } } else if (p.intValue != v) { p.intValue = v; c = true; } }
                    Set("AfterOpaque", 1);
                    Set("Downsample", 1);
                    Set("BlurQuality", 1);   // medium (gaussian)
                    if (c) { fs.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(f); changed = true; }
                }
                if (changed) EditorUtility.SetDirty(data);
            }
            if (!changed) { Debug.Log("[Tribal Trouble] Ambient occlusion settings already fast"); return; }
            AssetDatabase.SaveAssets();
            Debug.Log("[Tribal Trouble] Ambient occlusion: after the opaques, half resolution (no depth pre-pass).");
        }

        /// <summary>The jungle trees' source meshes kept readable, so a tree's twenty-odd parts can be merged into one
        /// mesh per material when the game starts (TTRealTrees) - one instanced draw per material instead of per part.</summary>
        static void EnsureTreeMeshesReadable()
        {
            foreach (var path in new[] { "Assets/ThirdParty/Mobile_FV/Resources/SourceMeshes.fbx" })
            {
                var imp = AssetImporter.GetAtPath(path);
                if (!(imp is ModelImporter mi)) { Debug.Log("[Tribal Trouble] no model importer for " + path + ": " + (imp == null ? "missing" : imp.GetType().Name)); continue; }
                if (mi.isReadable) continue;
                mi.isReadable = true;
                mi.SaveAndReimport();
                Debug.Log("[Tribal Trouble] " + path + ": read/write on (tree parts merged at start).");
            }
        }

        /// <summary>
        /// Modern rendering settings for the URP asset: soft shadows (4096, two cascades, 220 m), MSAA 4x, the depth
        /// texture (soft particles, SSAO), HDR grading, and a screen-space ambient occlusion renderer feature (from depth,
        /// so the game's own shaders need no normals pass). The game switches SSAO and the rest with its Graphics settings.
        /// </summary>
        [MenuItem("Tools/Tribal Trouble/Upgrade Rendering Settings", priority = 1)]
        public static void UpgradeRendering()
        {
            var pipeline = GraphicsSettings.defaultRenderPipeline as UniversalRenderPipelineAsset;
            if (pipeline == null) { Debug.LogWarning("[Tribal Trouble] No URP asset assigned; run Setup Project first."); return; }
            var so = new SerializedObject(pipeline);
            void Set(string name, System.Action<SerializedProperty> f) { var p = so.FindProperty(name); if (p != null) f(p); else Debug.LogWarning("[Tribal Trouble] URP setting not found: " + name); }
            Set("m_SoftShadowsSupported", p => p.boolValue = true);
            Set("m_MainLightShadowmapResolution", p => p.intValue = 4096);
            Set("m_ShadowCascadeCount", p => p.intValue = 2);
            Set("m_Cascade2Split", p => p.floatValue = 0.3f);
            Set("m_ShadowDistance", p => p.floatValue = 220f);
            Set("m_MSAA", p => p.intValue = 4);
            Set("m_RequireDepthTexture", p => p.boolValue = true);
            Set("m_SupportsHDR", p => p.boolValue = true);
            Set("m_ColorGradingMode", p => p.intValue = 1);   // HDR
            // v2: the modern sea refracts the opaque scene: full-resolution opaque copy
            Set("m_RequireOpaqueTexture", p => p.boolValue = true);
            Set("m_OpaqueDownsampling", p => p.intValue = 0);   // None
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(pipeline);

            var field = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field?.GetValue(pipeline) is ScriptableRendererData[] list)
                foreach (var data in list)
                    if (data != null) AddSsao(data);

            AssetDatabase.SaveAssets();
            EditorPrefs.SetInt(RenderingKey + "." + Application.dataPath.GetHashCode(), RenderingVersion);
            Debug.Log("[Tribal Trouble] Rendering settings upgraded: soft shadows, MSAA 4x, depth texture, HDR grading, SSAO, full-res opaque texture.");
        }

        static void AddSsao(ScriptableRendererData data)
        {
            foreach (var f in data.rendererFeatures)
                if (f != null && f.GetType().Name == "ScreenSpaceAmbientOcclusion") return;
            var type = typeof(UniversalRendererData).Assembly.GetType("UnityEngine.Rendering.Universal.ScreenSpaceAmbientOcclusion");
            if (type == null) { Debug.LogWarning("[Tribal Trouble] SSAO feature type not found"); return; }
            var feature = (ScriptableRendererFeature)ScriptableObject.CreateInstance(type);
            feature.name = "SSAO";
            AssetDatabase.AddObjectToAsset(feature, data);
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(feature, out _, out long localId);
            var so = new SerializedObject(data);
            var features = so.FindProperty("m_RendererFeatures");
            var map = so.FindProperty("m_RendererFeatureMap");
            features.arraySize++;
            features.GetArrayElementAtIndex(features.arraySize - 1).objectReferenceValue = feature;
            map.arraySize++;
            map.GetArrayElementAtIndex(map.arraySize - 1).longValue = localId;
            so.ApplyModifiedPropertiesWithoutUndo();
            // settings: from depth (no normals pass needed), medium radius, soft
            var fs = new SerializedObject(feature);
            void Set(string name, System.Action<SerializedProperty> f) { var p = fs.FindProperty("m_Settings." + name); if (p != null) f(p); }
            Set("Source", p => p.intValue = 0);
            Set("NormalSamples", p => p.intValue = 1);
            Set("Intensity", p => p.floatValue = 1.1f);
            Set("DirectLightingStrength", p => p.floatValue = 0.3f);
            Set("Radius", p => p.floatValue = 0.6f);
            Set("Downsample", p => p.boolValue = false);
            fs.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(data);
        }

        [MenuItem("Tools/Tribal Trouble/Setup Project (URP, Linear)", priority = 0)]
        public static void Run()
        {
            EnsureFolder(SettingsDir);

            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(SettingsDir + "/TT_URP.asset");
            if (pipeline == null)
            {
                var rendererPath = SettingsDir + "/TT_URP_Renderer.asset";
                var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(rendererPath);
                if (rendererData == null)
                {
                    rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
                    AssetDatabase.CreateAsset(rendererData, rendererPath);
                }
                pipeline = UniversalRenderPipelineAsset.Create(rendererData);
                pipeline.shadowDistance = 120f;
                AssetDatabase.CreateAsset(pipeline, SettingsDir + "/TT_URP.asset");
                AssetDatabase.SaveAssets();
            }

            if (GraphicsSettings.defaultRenderPipeline == null)
                GraphicsSettings.defaultRenderPipeline = pipeline;
            if (QualitySettings.renderPipeline == null)
                QualitySettings.renderPipeline = pipeline;

            if (PlayerSettings.colorSpace != ColorSpace.Linear)
                PlayerSettings.colorSpace = ColorSpace.Linear;

            Debug.Log("[Tribal Trouble] Project setup done: URP assigned (" +
                      AssetDatabase.GetAssetPath(GraphicsSettings.defaultRenderPipeline) + "), Linear colour space.");
        }

        /// <summary>Creates an Assets/... folder chain through the AssetDatabase (CreateAsset needs it registered).</summary>
        internal static void EnsureFolder(string folder)
        {
            folder = folder.Replace('\\', '/').TrimEnd('/');
            if (AssetDatabase.IsValidFolder(folder))
                return;
            var parent = Path.GetDirectoryName(folder).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(folder));
        }
    }
}
