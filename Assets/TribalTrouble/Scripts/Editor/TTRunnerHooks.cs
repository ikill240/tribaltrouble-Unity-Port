// The runner's unattended editor (Tools/runner/tt_runner.bat, step "test"): the runner starts the editor with
// -executeMethod TribalTrouble.EditorTools.TTRunnerHooks.AutorunThenQuit after writing Benchmarks/autorun.txt;
// TTBenchAuto picks the request up and plays it, and when play mode ends this closes the editor again. If no run
// starts within 6 minutes (compile errors, a dialog), the editor closes too, so the runner never waits forever.
using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    [InitializeOnLoad]
    public static class TTRunnerHooks
    {
        const string Armed = "TT_RUNNER_ARMED", Played = "TT_RUNNER_PLAYED", Since = "TT_RUNNER_SINCE";

        static TTRunnerHooks()
        {
            if (SessionState.GetBool(Armed, false)) Arm();
        }

        /// <summary>-executeMethod TribalTrouble.EditorTools.TTRunnerHooks.AutorunThenQuit</summary>
        public static void AutorunThenQuit()
        {
            SessionState.SetBool(Armed, true);
            SessionState.SetBool(Played, false);
            SessionState.SetFloat(Since, (float)EditorApplication.timeSinceStartup);
            Debug.Log("[TT] Runner: waiting for the autorun, the editor closes when it is over");
            Arm();
        }

        static void Arm()
        {
            EditorApplication.playModeStateChanged -= Changed;
            EditorApplication.playModeStateChanged += Changed;
            EditorApplication.update -= Tick;
            EditorApplication.update += Tick;
        }

        static void Changed(PlayModeStateChange c)
        {
            if (c == PlayModeStateChange.EnteredPlayMode) SessionState.SetBool(Played, true);
        }

        static void Tick()
        {
            if (!SessionState.GetBool(Armed, false)) return;
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
            if (SessionState.GetBool(Played, false)) { Quit("the autorun is over"); return; }
            if (EditorApplication.timeSinceStartup - SessionState.GetFloat(Since, 0f) > 360.0) Quit("no autorun started within 6 minutes");
        }

        static void Quit(string why)
        {
            SessionState.SetBool(Armed, false);
            Debug.Log("[TT] Runner: " + why + " - closing the editor");
            EditorApplication.Exit(0);
        }
    }
}
