using System.IO;
using System.Linq;
using TribalTrouble.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    static class SkirmishSceneBuilder
    {
        const string ScenePath = "Assets/TribalTrouble/Scenes/Skirmish.unity";

        [MenuItem("Tools/Tribal Trouble/Create Skirmish Scene (playable)", priority = 5)]
        public static void Create()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            // the game needs the catalog with terrain/water materials and sounds
            SpriteCatalogBuilder.Build();

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var lightGo = new GameObject("Sun");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.25f;
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            var camGo = new GameObject("Main Camera") { tag = "MainCamera" };
            var cam = camGo.AddComponent<Camera>();
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 3000f;
            camGo.AddComponent<AudioListener>();
            camGo.transform.position = new Vector3(0, 50, -50);

            var game = new GameObject("Tribal Trouble").AddComponent<SkirmishGame>();
            game.mainCamera = cam;
            game.sun = light;

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);

            var scenes = EditorBuildSettings.scenes.Where(s => s.path != ScenePath).ToList();
            scenes.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log("[Tribal Trouble] Created " + ScenePath + " (first scene in Build Settings) - press Play.");
        }
    }
}
