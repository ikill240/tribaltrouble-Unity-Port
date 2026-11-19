using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    static class TTBenchmarkMenu
    {
        [MenuItem("Tools/Tribal Trouble/Benchmark Island Generator", priority = 50)]
        static void Run()
        {
            string result = null;
            var t = new System.Threading.Thread(() => { try { result = TribalTrouble.Faithful.TTBenchmark.Run(); } catch (System.Exception e) { result = e.ToString(); } }, 16 * 1024 * 1024);
            t.Start();
            t.Join();
            Debug.Log("[TT benchmark]\n" + result);
            EditorUtility.DisplayDialog("Tribal Trouble benchmark", result, "OK");
        }
    }
}
