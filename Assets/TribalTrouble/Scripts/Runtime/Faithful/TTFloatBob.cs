using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>Gentle float and pulse (the Shaman's / Skald's spirit orb).</summary>
    public sealed class TTFloatBob : MonoBehaviour
    {
        Vector3 _base;
        float _phase;
        void Start() { _base = transform.localPosition; _phase = Random.value * 6.28f; }
        void Update()
        {
            float t = Time.time * 2.2f + _phase;
            transform.localPosition = _base + new Vector3(0, Mathf.Sin(t) * 0.12f, 0);
            transform.localScale = Vector3.one * (0.3f + 0.04f * Mathf.Sin(t * 1.7f));
        }
    }

    /// <summary>Turns round its own up axis (the sparks circling a spirit orb).</summary>
    public sealed class TTOrbit : MonoBehaviour
    {
        public float speed = 120f;
        void Update() { transform.localRotation = Quaternion.AngleAxis(Time.time * speed, Vector3.up); }
    }
}
