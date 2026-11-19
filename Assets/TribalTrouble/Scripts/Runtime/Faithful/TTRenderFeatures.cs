using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>Turns renderer features of the active URP renderer on and off (the SSAO the project setup adds).</summary>
    public static class TTRenderFeatures
    {
        public static void SetAmbientOcclusion(bool on) => SetActive("ScreenSpaceAmbientOcclusion", on);

        public static void SetActive(string typeName, bool on)
        {
            try
            {
                if (!(GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset urp)) return;
                var f = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (!(f?.GetValue(urp) is ScriptableRendererData[] list)) return;
                foreach (var data in list)
                {
                    if (data == null) continue;
                    foreach (var feature in data.rendererFeatures)
                        if (feature != null && feature.GetType().Name == typeName && feature.isActive != on)
                            feature.SetActive(on);
                }
            }
            catch (System.Exception e) { Debug.LogWarning("[TT] renderer feature " + typeName + ": " + e.Message); }
        }
    }
}
