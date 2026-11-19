// A project with only the Steam game in it (no game picker: the 1.4 / New Tides host is not part of it) starts the
// Steam game in any scene. With the picker present it is the picker that starts it (SteamGame, by name).
using UnityEngine;

namespace TribalTroubleSteam
{
    static class SteamBoot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Start()
        {
            if (System.Type.GetType("TribalTrouble.FaithfulGame.FaithfulGame, TribalTrouble.Runtime") != null) return;
            if (Object.FindAnyObjectByType<SteamGame>() != null) return;
            new GameObject("Tribal Trouble (Steam)").AddComponent<SteamGame>();
        }
    }
}
