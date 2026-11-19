using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// Windows player settings and builds.
    /// <para>On load: Direct3D 11 is put ahead of Direct3D 12 for Windows. The D3D12 driver on some cards (RX 9070 XT)
    /// hung the device ("DXGI_ERROR_DEVICE_HUNG 887a0006") right after a match's view was built, which closed the game
    /// to the desktop; D3D11 doesn't. D3D12 stays in the list as the fallback.</para>
    /// <para>Tools > Tribal Trouble > Build Windows Player (IL2CPP): the game compiled to native C++ - faster
    /// simulation and garbage collection than Mono, no JIT hitches. Needs the Unity "Windows Build Support (IL2CPP)"
    /// module and Visual Studio's "Desktop development with C++" workload; without the module it says so and builds
    /// with Mono instead. Assets/link.xml keeps the reflected types from being stripped.</para>
    /// </summary>
    [InitializeOnLoad]
    public static class TTPlayerBuild
    {
        const string OutDir = "Builds/Windows";
        const string Exe = "TribalTrouble.exe";
        const string AppliedKey = "TT.PlayerBuild.D3D11First";

        static TTPlayerBuild()
        {
            EditorApplication.delayCall += () =>
            {
                if (SessionState.GetBool(AppliedKey, false)) return;
                SessionState.SetBool(AppliedKey, true);
                PreferD3D11();
            };
        }

        [MenuItem("Tools/Tribal Trouble/Prefer Direct3D 11 (fixes device-hung crashes)", priority = 20)]
        public static void PreferD3D11()
        {
            var target = BuildTarget.StandaloneWindows64;
            var want = new[] { GraphicsDeviceType.Direct3D11, GraphicsDeviceType.Direct3D12 };
            var have = PlayerSettings.GetGraphicsAPIs(target);
            if (!PlayerSettings.GetUseDefaultGraphicsAPIs(target) && have.SequenceEqual(want)) return;
            PlayerSettings.SetUseDefaultGraphicsAPIs(target, false);
            PlayerSettings.SetGraphicsAPIs(target, want);
            AssetDatabase.SaveAssets();
            Debug.Log("[Tribal Trouble] Windows graphics APIs: Direct3D 11, then Direct3D 12.");
        }

        /// <summary>Whether this editor has the IL2CPP Windows player (the "Windows Build Support (IL2CPP)" module).</summary>
        public static bool HasIl2Cpp()
        {
            string variations = Path.Combine(EditorApplication.applicationContentsPath,
                "PlaybackEngines", "windowsstandalonesupport", "Variations");
            return Directory.Exists(variations) &&
                   Directory.GetDirectories(variations).Any(d => Path.GetFileName(d).Contains("il2cpp"));
        }

        [MenuItem("Tools/Tribal Trouble/Build Windows Player (IL2CPP)", priority = 21)]
        public static void BuildIl2Cpp() { Master = true; try { Build(true); } finally { Master = false; } }

        /// <summary>IL2CPP with the C++ Release configuration: a few minutes quicker to build, ~10 % slower to run (the
        /// benchmark's quick builds).</summary>
        public static void BuildIl2CppQuick() => Build(true);

        [MenuItem("Tools/Tribal Trouble/Build Windows Player (Mono)", priority = 22)]
        public static void BuildMono() => Build(false);

        /// <summary>Command line: -executeMethod TribalTrouble.EditorTools.TTPlayerBuild.BuildFromCommandLine</summary>
        public static void BuildFromCommandLine() => BuildIl2Cpp();

        /// <summary>IL2CPP with the C++ compiler's Master configuration (link-time optimisation): the fastest game (10-15 %
        /// more frames, islands load faster), a build of ~15 minutes. The menu's IL2CPP build uses it.</summary>
        public static bool Master;

        static void Build(bool il2cpp)
        {
            PreferD3D11();
            var group = NamedBuildTarget.Standalone;
            if (il2cpp && !HasIl2Cpp())
            {
                Debug.LogWarning("[Tribal Trouble] The IL2CPP module isn't installed (Unity Hub > Installs > Add modules > " +
                                 "Windows Build Support (IL2CPP)). Building with Mono instead.");
                il2cpp = false;
            }
            PlayerSettings.SetScriptingBackend(group, il2cpp ? ScriptingImplementation.IL2CPP : ScriptingImplementation.Mono2x);
            if (il2cpp)
            {
                PlayerSettings.SetIl2CppCompilerConfiguration(group, Master ? Il2CppCompilerConfiguration.Master : Il2CppCompilerConfiguration.Release);
                PlayerSettings.SetIl2CppCodeGeneration(group, Il2CppCodeGeneration.OptimizeSpeed);
                PlayerSettings.SetManagedStrippingLevel(group, ManagedStrippingLevel.Low);
            }
            var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = Path.Combine(OutDir, il2cpp ? "IL2CPP" : "Mono", Exe),
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None,
            };
            var report = BuildPipeline.BuildPlayer(options);
            var s = report.summary;
            Debug.Log($"[Tribal Trouble] {(il2cpp ? "IL2CPP" : "Mono")} build {s.result}: {s.outputPath} " +
                      $"({s.totalSize / (1024 * 1024)} MB, {s.totalTime.TotalSeconds:0} s, {s.totalErrors} errors)");
            if (s.result != UnityEditor.Build.Reporting.BuildResult.Succeeded && il2cpp)
                Debug.LogError("[Tribal Trouble] If the error mentions a C++ compiler: install Visual Studio 2022 with " +
                               "\"Desktop development with C++\", or use Build Windows Player (Mono).");
        }
    }
}
