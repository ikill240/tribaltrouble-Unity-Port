using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// The unattended benchmark: when Benchmarks/autorun.txt appears in the project folder (e.g. "players=8 size=4"),
    /// the editor enters play mode; the game (FaithfulGame's TTStressBench) then starts that skirmish, runs the Shift+F9
    /// benchmark, writes Benchmarks/bench_*.txt and leaves play mode again.
    /// </summary>
    [InitializeOnLoad]
    static class TTBenchAuto
    {
        static double s_next;

        static TTBenchAuto()
        {
            EditorApplication.update += Poll;
            Application.logMessageReceived += Log;
        }

        // the editor's messages (ours, warnings, errors) also go to Benchmarks/editor_log.txt, readable without the editor
        static int s_logged;
        static void Log(string msg, string stack, LogType type)
        {
            if (type == LogType.Log && !msg.StartsWith("[T")) return;
            if (s_logged++ > 2000) return;
            try
            {
                string dir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), "Benchmarks");
                string line = System.DateTime.Now.ToString("HH:mm:ss") + " " + type + ": " + msg + (type == LogType.Exception || type == LogType.Error ? "\n" + stack : "") + "\n";
                System.IO.File.AppendAllText(System.IO.Path.Combine(dir, "editor_log.txt"), line);
            }
            catch (System.Exception) { }
        }

        /// <summary>The single-game copies split_games.ps1 makes next to this project.</summary>
        static bool IsCopy(string name) =>
            name == "TribalTrouble Classic" || name == "TribalTrouble 1.4" || name == "TribalTrouble2 NewTides" || name == "TikiMadness";

        static void Poll()
        {
            if (EditorApplication.timeSinceStartup < s_next) return;
            s_next = EditorApplication.timeSinceStartup + 2.0;
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
            string dir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), "Benchmarks");
            // new scripts were copied in while the editor is in the background: Benchmarks/refresh.txt imports them
            // (the editor only looks for changed files when it gets the focus)
            string refresh = System.IO.Path.Combine(dir, "refresh.txt");
            if (System.IO.File.Exists(refresh))
            {
                try { System.IO.File.Delete(refresh); } catch (System.Exception) { return; }
                Debug.Log("[TT] Refresh requested");
                AssetDatabase.Refresh();
                return;
            }
            // the project setup steps again (Benchmarks/setup.txt)
            string setup = System.IO.Path.Combine(dir, "setup.txt");
            if (System.IO.File.Exists(setup))
            {
                try { System.IO.File.Delete(setup); } catch (System.Exception) { return; }
                Debug.Log("[TT] Setup requested");
                try { ProjectSetup.EnsureAll(); } catch (System.Exception e) { Debug.LogException(e); }
                Debug.Log("[TT] Setup done");
                return;
            }
            // a player build to measure outside the editor (Benchmarks/build.txt: "mono" or "il2cpp"); it gets its own
            // Benchmarks folder, so an autorun.txt put there runs the benchmark when the built game starts
            string build = System.IO.Path.Combine(dir, "build.txt");
            if (System.IO.File.Exists(build))
            {
                string kind = "";
                try { kind = System.IO.File.ReadAllText(build).Trim().ToLowerInvariant(); System.IO.File.Delete(build); } catch (System.Exception) { return; }
                Debug.Log("[TT] Benchmark build requested: " + kind);
                try
                {
                    PlayerSettings.enableFrameTimingStats = true;   // (the benchmark's CPU / GPU frame timing in the player)
                    // "il2cpp master": the C++ compiler's Master configuration
                    bool master = kind.EndsWith("master");
                    if (kind.StartsWith("il2cpp")) kind = "il2cpp";
                    if (kind != "il2cpp") TTPlayerBuild.BuildMono();
                    else if (master) TTPlayerBuild.BuildIl2Cpp();
                    else TTPlayerBuild.BuildIl2CppQuick();
                    string outDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), "Builds", "Windows", kind == "il2cpp" ? "IL2CPP" : "Mono", "Benchmarks");
                    System.IO.Directory.CreateDirectory(outDir);
                    System.IO.File.WriteAllText(System.IO.Path.Combine(dir, "build_done.txt"), System.DateTime.Now.ToString("s"));
                }
                catch (System.Exception e)
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(dir, "build_error.txt"), e.ToString());
                }
                return;
            }
            // make or refresh the single-game copies of this project (Benchmarks/split.txt, empty or "classic",
            // "classic14", "newtides" or "tiki"): PortTools/split/split_games.ps1 in the background, its log in
            // Benchmarks/split_log.txt
            string split = System.IO.Path.Combine(dir, "split.txt");
            if (System.IO.File.Exists(split))
            {
                string only = "";
                try { only = System.IO.File.ReadAllText(split).Trim().ToLowerInvariant(); System.IO.File.Delete(split); } catch (System.Exception) { return; }
                string root = System.IO.Path.GetDirectoryName(Application.dataPath);
                string ps1 = System.IO.Path.Combine(root, "PortTools", "split", "split_games.ps1");
                if (!System.IO.File.Exists(ps1)) { Debug.LogWarning("[TT] Split: " + ps1 + " is missing"); return; }
                if (only != "classic" && only != "classic14" && only != "newtides" && only != "tiki") only = "";
                Debug.Log("[TT] Split requested" + (only.Length > 0 ? " (" + only + ")" : ""));
                try
                {
                    var psi = new System.Diagnostics.ProcessStartInfo("powershell.exe",
                        "-NoProfile -ExecutionPolicy Bypass -File \"" + ps1 + "\" -Src \"" + root + "\"" + (only.Length > 0 ? " -Only " + only : ""))
                    { UseShellExecute = false, CreateNoWindow = true, WorkingDirectory = root };
                    System.Diagnostics.Process.Start(psi);
                }
                catch (System.Exception e) { Debug.LogException(e); }
                return;
            }
            // open one of the single-game copies headless to import and compile it (Benchmarks/checkproj.txt: its
            // folder name next to this project); the editor's log goes to Benchmarks/check_<folder>.log
            string check = System.IO.Path.Combine(dir, "checkproj.txt");
            if (System.IO.File.Exists(check))
            {
                string name = "";
                try { name = System.IO.File.ReadAllText(check).Trim(); System.IO.File.Delete(check); } catch (System.Exception) { return; }
                if (!IsCopy(name)) { Debug.LogWarning("[TT] Check: unknown project " + name); return; }
                string proj = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Application.dataPath)), name);
                string log = System.IO.Path.Combine(dir, "check_" + name.Replace(' ', '_') + ".log");
                Debug.Log("[TT] Checking " + proj + " (headless import and compile)");
                try
                {
                    var psi = new System.Diagnostics.ProcessStartInfo(EditorApplication.applicationPath,
                        "-batchmode -quit -nographics -projectPath \"" + proj + "\" -logFile \"" + log + "\"")
                    { UseShellExecute = false, CreateNoWindow = true };
                    System.Diagnostics.Process.Start(psi);
                }
                catch (System.Exception e) { Debug.LogException(e); }
                return;
            }
            // build one of the single-game copies headless (Benchmarks/buildproj.txt: its folder name, then optionally
            // "|mono" or "|il2cpp" - default IL2CPP, the Master configuration); the player goes to that project's
            // Builds/Windows/<IL2CPP|Mono>/TribalTrouble.exe, the editor's log to Benchmarks/build_<folder>.log
            string buildproj = System.IO.Path.Combine(dir, "buildproj.txt");
            if (System.IO.File.Exists(buildproj))
            {
                string req = "";
                try { req = System.IO.File.ReadAllText(buildproj).Trim(); System.IO.File.Delete(buildproj); } catch (System.Exception) { return; }
                string[] parts = req.Split('|');
                string name = parts[0].Trim();
                bool mono = parts.Length > 1 && parts[1].Trim().ToLowerInvariant() == "mono";
                if (!IsCopy(name)) { Debug.LogWarning("[TT] Build: unknown project " + name); return; }
                string proj = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Application.dataPath)), name);
                string log = System.IO.Path.Combine(dir, "build_" + name.Replace(' ', '_') + ".log");
                string method = mono ? "TribalTrouble.EditorTools.TTPlayerBuild.BuildMono" : "TribalTrouble.EditorTools.TTPlayerBuild.BuildFromCommandLine";
                Debug.Log("[TT] Building " + proj + " (" + (mono ? "Mono" : "IL2CPP") + ", headless)");
                try
                {
                    var psi = new System.Diagnostics.ProcessStartInfo(EditorApplication.applicationPath,
                        "-batchmode -quit -projectPath \"" + proj + "\" -executeMethod " + method + " -logFile \"" + log + "\"")
                    { UseShellExecute = false, CreateNoWindow = true };
                    System.Diagnostics.Process.Start(psi);
                }
                catch (System.Exception e) { Debug.LogException(e); }
                return;
            }
            // start a built game (Benchmarks/runbuild.txt: "mono" or "il2cpp"); with an autorun.txt in its own
            // Benchmarks folder it runs the benchmark and quits
            string run = System.IO.Path.Combine(dir, "runbuild.txt");
            if (System.IO.File.Exists(run))
            {
                string kind = "";
                try { kind = System.IO.File.ReadAllText(run).Trim().ToLowerInvariant(); System.IO.File.Delete(run); } catch (System.Exception) { return; }
                string exe = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), "Builds", "Windows", kind == "il2cpp" ? "IL2CPP" : "Mono", "TribalTrouble.exe");
                Debug.Log("[TT] Starting " + exe);
                try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(exe) { UseShellExecute = true, WorkingDirectory = System.IO.Path.GetDirectoryName(exe) }); }
                catch (System.Exception e) { System.IO.File.WriteAllText(System.IO.Path.Combine(dir, "runbuild_error.txt"), e.ToString()); }
                return;
            }
            string file = System.IO.Path.Combine(dir, "autorun.txt");
            if (!System.IO.File.Exists(file)) return;
            // handed over to the game (autorun_go.txt) before play mode starts: one request, one run - however it ends
            string go = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file), "autorun_go.txt");
            try { System.IO.File.WriteAllText(go, System.IO.File.ReadAllText(file)); System.IO.File.Delete(file); }
            catch (System.Exception e) { Debug.LogWarning("[TT] Auto benchmark: " + e.Message); return; }
            Debug.Log("[TT] Auto benchmark requested: entering play mode");
            TTPlayStart.EnsureGameScene();   // (a fresh copy has an empty scene open)
            EditorApplication.EnterPlaymode();
        }
    }
}
