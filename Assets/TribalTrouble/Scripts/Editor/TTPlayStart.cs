using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// Play always starts the game's scene (the first scene in the build settings), whatever scene is open: a project
    /// opened fresh - like the single-game copies split_games.ps1 makes - has an empty scene open, where Play did nothing.
    /// Both ways: the play-mode start scene, and (should that be ignored) the game's scene opened in place of an empty
    /// untitled one just before play mode starts.
    /// </summary>
    [InitializeOnLoad]
    static class TTPlayStart
    {
        static TTPlayStart()
        {
            EditorApplication.delayCall += Apply;
            EditorApplication.playModeStateChanged -= OnPlayMode;
            EditorApplication.playModeStateChanged += OnPlayMode;
        }

        static string FirstScene()
        {
            var first = EditorBuildSettings.scenes.FirstOrDefault(s => s.enabled);
            return first?.path;
        }

        static void Apply()
        {
            string path = FirstScene();
            if (string.IsNullOrEmpty(path)) return;
            var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            if (scene != null && EditorSceneManager.playModeStartScene != scene)
            {
                EditorSceneManager.playModeStartScene = scene;
                Debug.Log("[TT] Play starts " + path);
            }
        }

        static void OnPlayMode(PlayModeStateChange change)
        {
            if (change != PlayModeStateChange.ExitingEditMode) return;
            Apply();
            var active = EditorSceneManager.GetActiveScene();
            string path = FirstScene();
            // an empty, never saved scene: open the game's scene instead (nothing to lose)
            if (string.IsNullOrEmpty(active.path) && !active.isDirty && !string.IsNullOrEmpty(path) && EditorSceneManager.sceneCount == 1)
            {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                Debug.Log("[TT] Opened " + path + " for play");
            }
        }

        /// <summary>For the unattended runs (TTBenchAuto): the game's scene open before play mode is entered.</summary>
        public static void EnsureGameScene()
        {
            var active = EditorSceneManager.GetActiveScene();
            string path = FirstScene();
            if (string.IsNullOrEmpty(path) || active.path == path) return;
            if (string.IsNullOrEmpty(active.path) || !active.isDirty) EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        }
    }
}
