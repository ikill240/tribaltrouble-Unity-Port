using TribalTrouble.Presentation;
using UnityEngine;

namespace TribalTrouble.Viewer
{
    /// <summary>
    /// Stage 0 tool: browse every converted Tribal Trouble model, play its animations with
    /// TT's own timing rules, switch LOD / texture variant / team colour.
    /// Controls: right-drag orbit, mouse wheel zoom, middle-drag pan.
    /// </summary>
    public sealed class ModelViewer : MonoBehaviour
    {
        public Camera viewCamera;
        [Tooltip("Animation cycles per second for the preview (TT runs cycles = ticks / wpc).")]
        public float cyclesPerSecond = 1f;

        TTSpriteCatalog _catalog;
        TTSpriteView _view;
        int _spriteIndex;
        int _teamIndex = 1;
        bool _playing = true;
        float _ticks;
        Vector2 _listScroll;
        float _yaw = 200f, _pitch = 20f, _distance = 6f;
        Vector3 _pivot = new Vector3(0, 1, 0);

        void Start()
        {
            if (viewCamera == null)
                viewCamera = Camera.main;
            _catalog = TTSpriteCatalog.Load();
            var go = new GameObject("SpriteView");
            _view = go.AddComponent<TTSpriteView>();
            if (_catalog != null && _catalog.sprites.Count > 0)
                Show(0);
        }

        void Show(int index)
        {
            _spriteIndex = Mathf.Clamp(index, 0, _catalog.sprites.Count - 1);
            var def = _catalog.sprites[_spriteIndex];
            _view.Init(def, _catalog.baseMaterial, TeamColors.Defaults[_teamIndex]);
            _ticks = 0f;
            FrameModel(def);
        }

        void FrameModel(TTSpriteDefinition def)
        {
            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            bool first = true;
            foreach (var r in _view.GetComponentsInChildren<Renderer>())
            {
                if (!r.enabled)
                    continue;
                if (first) { bounds = r.bounds; first = false; }
                else bounds.Encapsulate(r.bounds);
            }
            _pivot = bounds.center;
            _distance = Mathf.Max(2f, bounds.extents.magnitude * 2.8f);
        }

        void Update()
        {
            if (_catalog == null || _view.Definition == null)
                return;
            if (_playing && _view.AnimationIndex >= 0)
            {
                var anim = _view.Definition.animations[_view.AnimationIndex];
                _ticks += Time.deltaTime * cyclesPerSecond * anim.wpc;
                _view.SetAnimationTicks(_ticks);
            }

            if (Input.GetMouseButton(1))
            {
                _yaw += Input.GetAxis("Mouse X") * 4f;
                _pitch = Mathf.Clamp(_pitch - Input.GetAxis("Mouse Y") * 4f, -10f, 85f);
            }
            if (Input.GetMouseButton(2))
            {
                var t = viewCamera.transform;
                _pivot -= (t.right * Input.GetAxis("Mouse X") + t.up * Input.GetAxis("Mouse Y")) * _distance * 0.03f;
            }
            _distance = Mathf.Clamp(_distance * (1f - Input.mouseScrollDelta.y * 0.1f), 0.5f, 200f);
            if (Input.GetKeyDown(KeyCode.DownArrow)) Show((_spriteIndex + 1) % _catalog.sprites.Count);
            if (Input.GetKeyDown(KeyCode.UpArrow)) Show((_spriteIndex + _catalog.sprites.Count - 1) % _catalog.sprites.Count);
            if (Input.GetKeyDown(KeyCode.Space)) _playing = !_playing;

            var rot = Quaternion.Euler(_pitch, _yaw, 0f);
            viewCamera.transform.SetPositionAndRotation(_pivot - rot * Vector3.forward * _distance, rot);
        }

        void OnGUI()
        {
            if (_catalog == null)
            {
                GUI.Label(new Rect(10, 10, 700, 60),
                    "No SpriteCatalog found. Run Tools > Tribal Trouble > Build Sprite Catalog, then press Play again.");
                return;
            }

            GUILayout.BeginArea(new Rect(10, 10, 240, Screen.height - 20), GUI.skin.box);
            GUILayout.Label($"Models ({_catalog.sprites.Count})  [Up/Down]");
            _listScroll = GUILayout.BeginScrollView(_listScroll);
            for (int i = 0; i < _catalog.sprites.Count; i++)
            {
                var s = _catalog.sprites[i];
                if (s == null) continue;
                var label = (i == _spriteIndex ? "> " : "  ") + s.Key;
                if (GUILayout.Button(label, GUI.skin.label))
                    Show(i);
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            var def = _view.Definition;
            if (def == null) return;
            GUILayout.BeginArea(new Rect(Screen.width - 290, 10, 280, Screen.height - 20), GUI.skin.box);
            GUILayout.Label(def.Key);
            GUILayout.Label("Animations  [Space = play/pause]");
            for (int i = 0; i < def.animations.Length; i++)
            {
                var a = def.animations[i];
                var label = $"{(i == _view.AnimationIndex ? "> " : "")}{a.name}  ({a.frames}f, wpc {a.wpc}, {(a.loop ? "loop" : "plain")})";
                if (GUILayout.Button(label))
                {
                    _ticks = 0f;
                    _view.SetAnimation(i, 0f);
                }
            }
            GUILayout.Label($"Cycles / sec: {cyclesPerSecond:0.00}");
            cyclesPerSecond = GUILayout.HorizontalSlider(cyclesPerSecond, 0.05f, 3f);
            if (!_playing && _view.AnimationIndex >= 0)
            {
                GUILayout.Label("Scrub");
                var a = def.animations[_view.AnimationIndex];
                float cycle = GUILayout.HorizontalSlider(_ticks / a.wpc, 0f, 1f);
                _ticks = cycle * a.wpc;
                _view.SetAnimationTicks(_ticks);
            }

            GUILayout.Space(8);
            GUILayout.Label("LOD / texture");
            for (int l = 0; l < def.lods.Length; l++)
            {
                var lod = def.lods[l];
                for (int v = 0; v < Mathf.Max(1, lod.textures.Length); v++)
                {
                    string tname = lod.textures.Length > 0 ? lod.textures[v].name : "(none)";
                    bool on = l == _view.Lod && v == _view.Variant;
                    if (GUILayout.Toggle(on, $"lod{l}: {tname}") && !on)
                        _view.SetLod(l, v);
                }
            }

            GUILayout.Space(8);
            GUILayout.Label("Team colour: " + TeamColors.Names[_teamIndex]);
            GUILayout.BeginHorizontal();
            for (int i = 0; i < TeamColors.Defaults.Length; i++)
            {
                var prev = GUI.backgroundColor;
                GUI.backgroundColor = TeamColors.Defaults[i];
                if (GUILayout.Button(i == _teamIndex ? "*" : " ", GUILayout.Width(20)))
                {
                    _teamIndex = i;
                    _view.SetTeamColor(TeamColors.Defaults[i]);
                }
                GUI.backgroundColor = prev;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}
