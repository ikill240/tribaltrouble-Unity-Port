using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>Prefab lists for the Modern trees (built by Tools > Tribal Trouble > Build Real Trees).</summary>
    public sealed class TTRealTreeSet : ScriptableObject
    {
        public GameObject[] jungle = new GameObject[0];
        public GameObject[] palm = new GameObject[0];
        public GameObject[] oak = new GameObject[0];
        public GameObject[] pine = new GameObject[0];
    }
}
