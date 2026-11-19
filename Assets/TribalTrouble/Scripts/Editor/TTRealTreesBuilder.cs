using System.Collections.Generic;
using System.Linq;
using TribalTrouble.FaithfulGame;
using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// Builds Resources/TTRealTrees.asset: which tree prefabs replace each Tribal Trouble tree type in the Modern look
    /// (from Assets/ThirdParty/Mobile_FV, copied from your Stick City project). Runs once automatically.
    /// </summary>
    [InitializeOnLoad]
    static class TTRealTreesBuilder
    {
        const string AssetPath = "Assets/TribalTrouble/Resources/TTRealTrees.asset";
        const string Root = "Assets/ThirdParty/Mobile_FV/";

        static TTRealTreesBuilder()
        {
            EditorApplication.delayCall += () =>
            {
                if (AssetDatabase.LoadAssetAtPath<TTRealTreeSet>(AssetPath) == null && AssetDatabase.IsValidFolder("Assets/ThirdParty/Mobile_FV")) Build();
            };
        }

        static GameObject[] Load(params string[] paths) =>
            paths.Select(p => AssetDatabase.LoadAssetAtPath<GameObject>(Root + p)).Where(g => g != null).ToArray();

        [MenuItem("Tools/Tribal Trouble/Build Real Trees", priority = 30)]
        public static void Build()
        {
            var set = AssetDatabase.LoadAssetAtPath<TTRealTreeSet>(AssetPath);
            // an asset saved while the class shared a file with TTRealTrees has no script: replace it
            if (set == null && System.IO.File.Exists(AssetPath)) AssetDatabase.DeleteAsset(AssetPath);
            bool create = set == null;
            if (create) set = ScriptableObject.CreateInstance<TTRealTreeSet>();
            set.jungle = Load("Prefabs/Summer/Sm1_Jungle1_v1_00.prefab", "Prefabs/Summer/Sm1_Jungle1_v2_00.prefab", "Prefabs/Summer/Sm1_Jungle1_v3_00.prefab",
                              "Prefabs/Summer/Sm2_Jungle1_v1_00.prefab", "Prefabs/Summer/Sm2_Jungle1_v2_00.prefab", "Prefabs/Summer/Sm2_Jungle1_v3_00.prefab");
            set.palm = new GameObject[0];   // no palm models in your projects: the original palms stay (HD textures)
            set.oak = Load("Optimized/Summer/Opt_S1_FullTree_v1_00.prefab", "Optimized/Summer/Opt_S1_FullTree_v3_00.prefab",
                           "Optimized/Summer/Opt_S1_Tree_short_v1_00.prefab", "Optimized/Summer/Opt_S1_Tree_short_v2_00.prefab",
                           "Optimized/Uniques/Opt_Unique_FullTree_v1.prefab", "Optimized/Uniques/Opt_Unique_FullTree_v3.prefab");
            set.pine = Load("Optimized/Winter/Opt_Conifer1snow_v1_00.prefab", "Optimized/Winter/Opt_Conifer1snow_v2_00.prefab", "Optimized/Winter/Opt_Conifer1snow_v3_00.prefab",
                            "Optimized/Winter/Opt_Conifer2snow_tall_v1_00.prefab", "Optimized/Winter/Opt_Conifer2snow_tall_v2_00.prefab",
                            "Resources/C1__Tree_ConiferTall.prefab", "Resources/C1__Tree_ConiferShort.prefab");
            if (create) AssetDatabase.CreateAsset(set, AssetPath);
            EditorUtility.SetDirty(set);
            AssetDatabase.SaveAssets();
            Debug.Log($"[Tribal Trouble] Real trees: {set.jungle.Length} jungle, {set.oak.Length} oak, {set.pine.Length} pine (palms keep the original model).");
        }
    }
}
