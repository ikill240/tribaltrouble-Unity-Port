using System.IO;
using UnityEditor;
using UnityEngine;

namespace TribalTroubleSteam.EditorTools
{
    /// <summary>
    /// Keeps the Steam game ready to play in the editor: when its models' catalog.json is newer than the sprite
    /// catalog (a fresh import of the Steam assets), the catalog is built again once the editor is idle. Benchmarks/
    /// steamsetup.txt asks for it at any time; what happened goes to Benchmarks/steam_log.txt too.
    /// </summary>
    [InitializeOnLoad]
    static class SteamAutoSetup
    {
        static double s_next;
        static bool s_checked;

        static SteamAutoSetup()
        {
            EditorApplication.update += Poll;
        }

        public static void Log(string msg)
        {
            Debug.Log("[TT Steam] " + msg);
            try
            {
                string dir = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Benchmarks");
                Directory.CreateDirectory(dir);
                File.AppendAllText(Path.Combine(dir, "steam_log.txt"), System.DateTime.Now.ToString("HH:mm:ss") + " " + msg + "\n");
            }
            catch (System.Exception) { }
        }

        static void Poll()
        {
            if (EditorApplication.timeSinceStartup < s_next) return;
            s_next = EditorApplication.timeSinceStartup + 3.0;
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating) return;
            string root = Path.GetDirectoryName(Application.dataPath);
            string trigger = Path.Combine(root, "Benchmarks", "steamsetup.txt");
            bool asked = File.Exists(trigger);
            if (asked) { try { File.Delete(trigger); } catch (System.Exception) { return; } }
            if (!asked && s_checked) return;
            s_checked = true;
            string json = Path.Combine(root, SteamCatalogBuilder.CatalogJsonPath), cat = Path.Combine(root, SteamCatalogBuilder.CatalogPath);
            if (!File.Exists(json)) { if (asked) Log("no Steam models (" + SteamCatalogBuilder.CatalogJsonPath + ")"); return; }
            if (!asked && File.Exists(cat) && File.GetLastWriteTimeUtc(cat) >= File.GetLastWriteTimeUtc(json)) return;
            Log("building the Steam sprite catalog");
            try
            {
                SteamCatalogBuilder.Build();
                var c = AssetDatabase.LoadAssetAtPath<TribalTroubleSteam.Presentation.TTSpriteCatalog>(SteamCatalogBuilder.CatalogPath);
                Log("Steam sprite catalog: " + (c != null ? c.sprites.Count + " sprites, " + c.sfx.Count + " sounds, " + c.music.Count + " music" : "missing"));
            }
            catch (System.Exception e) { Log("catalog build failed: " + e); }
        }
    }
}
