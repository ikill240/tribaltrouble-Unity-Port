using System.IO;
using TribalTrouble.Viewer;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    static class ModelViewerSceneBuilder
    {
        const string ScenePath = "Assets/TribalTrouble/Scenes/ModelViewer.unity";

        [MenuItem("Tools/Tribal Trouble/Create Model Viewer Scene", priority = 20)]
        public static void Create()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var lightGo = new GameObject("Sun");
            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            var camGo = new GameObject("Main Camera") { tag = "MainCamera" };
            var cam = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.55f, 0.72f, 0.85f);
            cam.nearClipPlane = 0.05f;
            camGo.AddComponent<AudioListener>();

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(4f, 1f, 4f);

            var viewerGo = new GameObject("ModelViewer");
            var viewer = viewerGo.AddComponent<ModelViewer>();
            viewer.viewCamera = cam;

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.55f, 0.6f, 0.7f);
            RenderSettings.ambientEquatorColor = new Color(0.4f, 0.42f, 0.4f);
            RenderSettings.ambientGroundColor = new Color(0.25f, 0.22f, 0.18f);

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log("[Tribal Trouble] Created " + ScenePath + " - press Play.");
        }
    }
}
