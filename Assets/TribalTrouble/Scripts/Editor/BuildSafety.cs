using System.Collections.Generic;
using TribalTrouble.FaithfulGame;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// Keeps a player build drawing what the editor draws. The game makes most of its materials at runtime (instanced
    /// trees, rocks, iron and plants; real trees and weapons; selection rings), so Unity's build stripping can't see
    /// which shader variants and shaders they need and would drop them: instanced batches (trees, resources) then draw
    /// nothing. This keeps all instancing variants, always includes the shaders found by name at runtime, enables
    /// instancing on the sprite material and makes sure the real tree / weapon sets are built and valid.
    /// Runs on every editor load and before every build.
    /// </summary>
    [InitializeOnLoad]
    sealed class BuildSafety : IPreprocessBuildWithReport
    {
        public int callbackOrder => -100;

        // small shaders found by name at runtime; always included (all their variants are cheap)
        static readonly string[] RuntimeShaders = { "TribalTrouble/Sprite", "TribalTrouble/Particle", "TribalTrouble/Sky", "TribalTrouble/ImpostorCapture", "TribalTrouble/GuiQuad", "TribalTrouble/WorldMarker", "TribalTrouble/SonicBlast", "TribalTrouble/PlanarShadow", "TribalTrouble/SpiritOrb", "TikiMadness/Toon", "TikiMadness/Water", "TikiMadness/Unlit" };
        // never always-include these: URP's own shaders have hundreds of thousands of variants (the build fails), and the
        // terrain pulls in URP/Lit through UsePass. They reach the build through materials instead (catalog, Resources).
        // (and editor-only built-ins like Hidden/Internal-Colored can't be listed at all: the build fails writing them)
        static bool TooBig(Shader sh) => sh != null && (sh.name.StartsWith("Universal Render Pipeline/") || sh.name == "TribalTrouble/Terrain"
                                                     || sh.name == "TribalTrouble/Water" || sh.name == "Hidden/Internal-Colored");

        // URP shaders the game finds by name at runtime: a material for each in Resources puts them (and the keyword
        // variants used) in the build
        static readonly (string path, string shader, bool transparent)[] RuntimeMaterials =
        {
            ("Assets/TribalTrouble/Resources/TTRuntime/URPUnlit.mat", "Universal Render Pipeline/Unlit", false),
            ("Assets/TribalTrouble/Resources/TTRuntime/URPUnlitTransparent.mat", "Universal Render Pipeline/Unlit", true),
            ("Assets/TribalTrouble/Resources/TTRuntime/URPParticlesUnlit.mat", "Universal Render Pipeline/Particles/Unlit", true),
        };

        static BuildSafety() { EditorApplication.delayCall += () => Ensure(false); }

        public void OnPreprocessBuild(BuildReport report) => Ensure(true);

        [MenuItem("Tools/Tribal Trouble/Prepare For Build", priority = 40)]
        static void Menu() => Ensure(true);

        static void Ensure(bool building)
        {
            // The editor compiles scripts in Debug mode by default, which makes the translated simulation and the
            // island generator about 7x slower (an Unreal island took minutes). Release mode, like a player build.
            // (Switch back from the bug icon in the status bar if you want to attach a debugger.)
            if (!building && !SessionState.GetBool("TT.releaseSet", false))
            {
                SessionState.SetBool("TT.releaseSet", true);
                if (UnityEditor.Compilation.CompilationPipeline.codeOptimization != UnityEditor.Compilation.CodeOptimization.Release)
                {
                    UnityEditor.Compilation.CompilationPipeline.codeOptimization = UnityEditor.Compilation.CodeOptimization.Release;
                    Debug.Log("[Tribal Trouble] Script optimization set to Release (the simulation and island generator run ~7x faster in the editor).");
                }
            }

            var gs = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/GraphicsSettings.asset");
            if (gs != null)
            {
                var so = new SerializedObject(gs);
                bool changed = false;
                var strip = so.FindProperty("m_InstancingStripping");
                if (strip != null && strip.intValue != 2) { strip.intValue = 2; changed = true; }   // 2 = keep all
                var list = so.FindProperty("m_AlwaysIncludedShaders");
                if (list != null)
                {
                    for (int i = list.arraySize - 1; i >= 0; i--)
                        if (list.GetArrayElementAtIndex(i).objectReferenceValue is Shader big && TooBig(big))
                        {
                            list.GetArrayElementAtIndex(i).objectReferenceValue = null;
                            list.DeleteArrayElementAtIndex(i);
                            changed = true;
                        }
                    var have = new HashSet<Shader>();
                    for (int i = 0; i < list.arraySize; i++) if (list.GetArrayElementAtIndex(i).objectReferenceValue is Shader sh) have.Add(sh);
                    foreach (var name in RuntimeShaders)
                    {
                        var sh = Shader.Find(name);
                        if (sh == null || have.Contains(sh)) continue;
                        list.InsertArrayElementAtIndex(list.arraySize);
                        list.GetArrayElementAtIndex(list.arraySize - 1).objectReferenceValue = sh;
                        have.Add(sh);
                        changed = true;
                    }
                }
                if (changed) { so.ApplyModifiedPropertiesWithoutUndo(); Debug.Log("[Tribal Trouble] Build settings: keep instancing variants, runtime shaders always included."); }
            }

            foreach (var (path, shaderName, transparent) in RuntimeMaterials)
            {
                if (AssetDatabase.LoadAssetAtPath<Material>(path) != null) continue;
                var sh = Shader.Find(shaderName);
                if (sh == null) continue;
                var dir = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
                if (!AssetDatabase.IsValidFolder(dir)) AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(dir).Replace('\\', '/'), System.IO.Path.GetFileName(dir));
                var m = new Material(sh) { enableInstancing = true };
                if (transparent)
                {
                    if (m.HasProperty("_Surface")) m.SetFloat("_Surface", 1f);
                    m.SetOverrideTag("RenderType", "Transparent");
                    m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    m.renderQueue = (int)RenderQueue.Transparent;
                }
                AssetDatabase.CreateAsset(m, path);
            }

            var mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/TribalTrouble/Materials/TTSprite.mat");
            if (mat != null && !mat.enableInstancing) { mat.enableInstancing = true; EditorUtility.SetDirty(mat); }

            if (building)
            {
                var trees = Resources.Load<TTRealTreeSet>("TTRealTrees");
                if ((trees == null || trees.jungle.Length == 0) && AssetDatabase.IsValidFolder("Assets/ThirdParty/Mobile_FV")) TTRealTreesBuilder.Build();
                var weapons = Resources.Load<TTRealWeaponSet>("TTRealWeapons");
                if ((weapons == null || weapons.axeFit == null || weapons.spearFit == null || weapons.version < TTRealWeaponSet.CurrentVersion) && AssetDatabase.IsValidFolder("Assets/ThirdParty/MeleeWeapons")) TTRealWeaponsBuilder.Build();
            }
            else
            {
                // an older weapon fit (before v6 the weapons sat beside the fist): refit once, in the editor too
                var weapons = Resources.Load<TTRealWeaponSet>("TTRealWeapons");
                if (weapons != null && weapons.version < TTRealWeaponSet.CurrentVersion && AssetDatabase.IsValidFolder("Assets/ThirdParty/MeleeWeapons")) TTRealWeaponsBuilder.Build();
            }
            AssetDatabase.SaveAssets();
        }
    }
}
