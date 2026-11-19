using System.Collections.Generic;
using UnityEngine;

namespace TribalTroubleSteam
{
    /// <summary>Plays the original TT sound effects by name prefix (e.g. "hit" picks hit1..hit7).</summary>
    public sealed class SfxPlayer
    {
        readonly Dictionary<string, List<AudioClip>> _byPrefix = new Dictionary<string, List<AudioClip>>();
        readonly Dictionary<string, AudioClip> _byName = new Dictionary<string, AudioClip>();
        readonly Camera _camera;
        readonly System.Random _rng = new System.Random();
        float _notifyCooldown;
        public float volume = 0.8f;

        public SfxPlayer(IEnumerable<AudioClip> clips, Camera camera)
        {
            _camera = camera;
            if (clips == null) return;
            foreach (var c in clips)
            {
                if (c == null) continue;
                _byName[c.name] = c;
                string prefix = c.name.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                if (!_byPrefix.TryGetValue(prefix, out var list)) _byPrefix[prefix] = list = new List<AudioClip>();
                list.Add(c);
            }
        }

        public AudioClip Get(string name)
        {
            if (_byName.TryGetValue(name, out var c)) return c;
            if (_byPrefix.TryGetValue(name, out var list) && list.Count > 0) return list[_rng.Next(list.Count)];
            return null;
        }

        public void Play(string name, Vector3 at)
        {
            var clip = Get(name);
            if (clip == null || _camera == null) return;
            // skip far-away sounds so big battles don't turn into noise
            if ((at - _camera.transform.position).sqrMagnitude > 160f * 160f) return;
            AudioSource.PlayClipAtPoint(clip, at, volume * SteamGlobals.SfxVolume);
        }

        public void PlayUi(string name)
        {
            var clip = Get(name);
            if (clip == null || _camera == null) return;
            AudioSource.PlayClipAtPoint(clip, _camera.transform.position, volume * SteamGlobals.SfxVolume);
        }

        /// <summary>"Your units are under attack" style notification with a cooldown.</summary>
        public void Notify(string name, float time)
        {
            if (time < _notifyCooldown) return;
            _notifyCooldown = time + 8f;
            PlayUi(name);
        }
    }
}
