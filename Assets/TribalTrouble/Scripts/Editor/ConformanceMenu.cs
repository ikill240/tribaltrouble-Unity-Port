using System.IO;
using TribalTrouble.Sim.Conformance;
using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    static class ConformanceMenu
    {
        static string GoldenDir => Path.GetFullPath(Path.Combine(Application.dataPath, "..", "PortTools", "golden"));

        [MenuItem("Tools/Tribal Trouble/Conformance/Island Generator (quick: 256-512 m)", priority = 100)]
        static void RunQuick() => Run(c => c.meters <= 512);

        [MenuItem("Tools/Tribal Trouble/Conformance/Island Generator (all sizes)", priority = 101)]
        static void RunAll() => Run(null);

        [MenuItem("Tools/Tribal Trouble/Conformance/Diagnostics (float + stages)", priority = 102)]
        static void RunDiagnostics()
        {
            var report = ConformanceDiagnostics.Run();
            File.WriteAllText(Path.Combine(GoldenDir, "..", "diagnostics_report.txt"), report);
            Debug.Log("[Tribal Trouble] Conformance diagnostics\n" + report);
        }

        static void Run(System.Func<LandscapeConformance.Case, bool> filter)
        {
            try
            {
                EditorUtility.DisplayProgressBar("Tribal Trouble", "Generating islands and comparing with Java goldens...", 0.5f);
                var (passed, failed, report) = LandscapeConformance.RunAll(GoldenDir, filter);
                File.WriteAllText(Path.Combine(GoldenDir, "..", "conformance_report.txt"), report);
                if (failed == 0)
                    Debug.Log("[Tribal Trouble] Island generator conformance: ALL PASS\n" + report);
                else
                    Debug.LogError("[Tribal Trouble] Island generator conformance: " + failed + " FAILED\n" + report);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
