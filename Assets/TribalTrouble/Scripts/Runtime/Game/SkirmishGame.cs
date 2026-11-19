using System;
using System.Collections.Generic;
using System.Threading;
using TribalTrouble.Presentation;
using UnityEngine;
using Landscape = TribalTrouble.Sim.Procedural.Landscape;
using SimCore = TribalTrouble.Sim;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Entry point of the playable skirmish: main menu -> island generation (background thread,
    /// the bit-exact port of the Java generator) -> game (fixed 20 ms ticks) -> game over.
    /// </summary>
    public sealed class SkirmishGame : MonoBehaviour
    {
        enum Phase { Menu, Generating, Playing }

        [Header("Scene references (created automatically when empty)")]
        public Camera mainCamera;
        public Light sun;

        // ---- menu settings
        int _sizeIndex = 1;                 // 256 / 512 / 1024
        static readonly int[] Sizes = { 256, 512, 1024 };
        static readonly string[] SizeNames = { "Small", "Medium", "Large" };
        Landscape.TerrainType _terrain = Landscape.TerrainType.NATIVE;
        Race _race = Race.Vikings;
        int _ais = 1;
        AIDifficulty _difficulty = AIDifficulty.Normal;
        float _hills = 0.5f, _vegetation = 0.5f, _supplies = 0.5f;
        string _seedText = "";
        int _colorIndex = 1;

        Phase _phase = Phase.Menu;
        TTSpriteCatalog _catalog;
        Thread _genThread;
        volatile float _genProgress;
        volatile bool _genDone;
        Landscape _genResult;
        Exception _genError;
        int _genSeed;

        // ---- game
        GameObject _gameRoot;
        GameSim _sim;
        GameView _view;
        PlayerController _controller;
        RtsCamera _rtsCamera;
        readonly List<SkirmishAI> _ais_ = new List<SkirmishAI>();
        SfxPlayer _sfx;
        AudioSource _music;
        float _accumulator;
        GUIStyle _title, _label;

        void Awake()
        {
            Application.targetFrameRate = 120;
            _catalog = TTSpriteCatalog.Load();
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null)
            {
                var cg = new GameObject("Main Camera") { tag = "MainCamera" };
                mainCamera = cg.AddComponent<Camera>();
                cg.AddComponent<AudioListener>();
            }
            mainCamera.farClipPlane = 3000f;
            mainCamera.nearClipPlane = 0.3f;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = new Color(0.55f, 0.72f, 0.85f);
            if (sun == null)
            {
                var lg = new GameObject("Sun");
                sun = lg.AddComponent<Light>();
                sun.type = LightType.Directional;
                sun.intensity = 1.25f;
                sun.shadows = LightShadows.Soft;
                lg.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            }
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.55f, 0.6f, 0.7f);
            RenderSettings.ambientEquatorColor = new Color(0.42f, 0.44f, 0.42f);
            RenderSettings.ambientGroundColor = new Color(0.25f, 0.22f, 0.18f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = mainCamera.backgroundColor;
            RenderSettings.fogStartDistance = 400f;
            RenderSettings.fogEndDistance = 1400f;

            _music = gameObject.AddComponent<AudioSource>();
            _music.loop = true;
            _music.volume = 0.35f;
            PlayMusic("menu");
        }

        void PlayMusic(string name)
        {
            if (_catalog == null || _music == null) return;
            foreach (var c in _catalog.music)
                if (c != null && c.name == name)
                {
                    if (_music.clip == c && _music.isPlaying) return;
                    _music.clip = c;
                    _music.Play();
                    return;
                }
        }

        // ================================================================== generation

        void StartGeneration()
        {
            if (_catalog == null)
            {
                Debug.LogError("[Tribal Trouble] Resources/SpriteCatalog.asset missing: run Tools > Tribal Trouble > Build Sprite Catalog.");
                return;
            }
            _genSeed = int.TryParse(_seedText, out int s) ? s : UnityEngine.Random.Range(1, 100000);
            _seedText = _genSeed.ToString();
            int players = 1 + _ais;
            int meters = Sizes[_sizeIndex];
            var terrain = _terrain;
            float hills = _hills, veg = _vegetation, sup = _supplies;
            int seed = _genSeed;
            _genProgress = 0;
            _genDone = false;
            _genError = null;
            _genResult = null;
            _phase = Phase.Generating;
            SimCore.ProgressForm.OnProgress = f => _genProgress = Mathf.Max(_genProgress, f);
            _genThread = new Thread(() =>
            {
                try
                {
                    _genResult = new Landscape(players, meters, terrain, 0.15f, hills, veg, sup, seed,
                        Rules.InitialUnits, 0.5f, false, null, false, null);
                }
                catch (Exception e) { _genError = e; }
                _genDone = true;
            }, 16 * 1024 * 1024) { IsBackground = true, Name = "TT island generator" };
            _genThread.Start();
        }

        void FinishGeneration()
        {
            SimCore.ProgressForm.OnProgress = null;
            if (_genError != null)
            {
                Debug.LogException(_genError);
                _phase = Phase.Menu;
                return;
            }
            StartGame(_genResult);
        }

        // ================================================================== game

        void StartGame(Landscape landscape)
        {
            _gameRoot = new GameObject("Skirmish");
            var world = new GameWorld(landscape, _terrain);
            var island = TerrainBuilder.Build(world, _catalog.terrainMaterial, _catalog.waterMaterial);
            island.transform.SetParent(_gameRoot.transform, false);

            var players = new List<Player>();
            var human = new Player { index = 0, name = "You", race = _race, color = TeamColors.Defaults[_colorIndex], team = 0, human = true };
            players.Add(human);
            int colour = 0;
            for (int i = 0; i < _ais; i++)
            {
                if (colour == _colorIndex) colour++;
                var race = (i % 2 == 0) ? (_race == Race.Vikings ? Race.Natives : Race.Vikings) : _race;
                players.Add(new Player
                {
                    index = i + 1, name = "Chieftain " + TeamColors.Names[colour % TeamColors.Names.Length], race = race,
                    color = TeamColors.Defaults[colour % TeamColors.Defaults.Length], team = i + 1,
                });
                colour++;
            }

            _sim = new GameSim(world, players, _genSeed);
            _sim.SpawnStartingUnits(Rules.InitialUnits);
            var unitsRoot = new GameObject("Entities").transform;
            unitsRoot.SetParent(_gameRoot.transform, false);
            _view = new GameView(_sim, _catalog, unitsRoot);

            _ais_.Clear();
            for (int i = 1; i < players.Count; i++) _ais_.Add(new SkirmishAI(_sim, players[i], _difficulty));

            _rtsCamera = mainCamera.GetComponent<RtsCamera>();
            if (_rtsCamera == null) _rtsCamera = mainCamera.gameObject.AddComponent<RtsCamera>();
            _rtsCamera.enabled = true;
            _rtsCamera.world = world;
            _rtsCamera.distance = 55f;
            _rtsCamera.yaw = 0f;
            _rtsCamera.LookAt(human.start);

            _sfx = new SfxPlayer(_catalog.sfx, mainCamera);
            _controller = _gameRoot.AddComponent<PlayerController>();
            _controller.sim = _sim;
            _controller.view = _view;
            _controller.player = human;
            _controller.rtsCamera = _rtsCamera;
            _controller.cam = mainCamera;
            _controller.sfx = _sfx;
            _controller.catalog = _catalog;
            _controller.onQuit = QuitToMenu;
            _controller.onRestart = Restart;
            _controller.Init();

            _accumulator = 0;
            _phase = Phase.Playing;
            PlayMusic(_race == Race.Vikings ? "viking" : "native");
        }

        void EndGame()
        {
            if (_gameRoot != null) Destroy(_gameRoot);
            _gameRoot = null;
            _sim = null;
            _view = null;
            _controller = null;
            _ais_.Clear();
            if (_rtsCamera != null) _rtsCamera.enabled = false;
        }

        void QuitToMenu()
        {
            EndGame();
            _phase = Phase.Menu;
            PlayMusic("menu");
        }

        void Restart()
        {
            EndGame();
            StartGeneration();   // same seed (kept in the seed field)
        }

        void Update()
        {
            if (_phase == Phase.Generating && _genDone) { FinishGeneration(); return; }
            if (_phase != Phase.Playing || _sim == null) return;

            if (!_controller.paused && !_controller.MenuOpen && !_sim.over)
            {
                _accumulator += Mathf.Min(Time.unscaledDeltaTime, 0.25f) * _controller.gameSpeed;
                int steps = 0;
                while (_accumulator >= Rules.TickSeconds && steps < 40)
                {
                    _accumulator -= Rules.TickSeconds;
                    float dt = Rules.TickSeconds;
                    foreach (var ai in _ais_) ai.Update(dt);
                    _sim.Tick();
                    steps++;
                }
                if (steps == 40) _accumulator = 0;   // can't keep up: slow down instead of spiralling
            }
            _view.Update(Mathf.Clamp01(Time.unscaledDeltaTime * 15f));
        }

        // ================================================================== menu UI

        void Styles()
        {
            if (_title != null) return;
            _title = new GUIStyle(GUI.skin.label) { fontSize = 34, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, normal = { textColor = new Color(1f, 0.85f, 0.4f) } };
            _label = new GUIStyle(GUI.skin.label) { fontSize = 14, normal = { textColor = Color.white } };
        }

        void OnGUI()
        {
            if (_phase == Phase.Playing) return;
            Styles();
            var old = GUI.color;
            GUI.color = new Color(0.1f, 0.08f, 0.05f, 0.92f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = old;
            GUI.Label(new Rect(0, 30, Screen.width, 50), "TRIBAL TROUBLE", _title);

            if (_phase == Phase.Generating)
            {
                GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 40, 300, 30), "Generating island " + _genSeed + "...", _label);
                var bar = new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 20);
                GUI.color = Color.black; GUI.DrawTexture(bar, Texture2D.whiteTexture);
                GUI.color = new Color(1f, 0.75f, 0.3f);
                GUI.DrawTexture(new Rect(bar.x + 2, bar.y + 2, (bar.width - 4) * Mathf.Clamp01(_genProgress), bar.height - 4), Texture2D.whiteTexture);
                GUI.color = old;
                return;
            }

            var r = new Rect(Screen.width / 2 - 230, 100, 460, Mathf.Min(560, Screen.height - 120));
            GUILayout.BeginArea(r, GUI.skin.box);
            GUILayout.Label("Skirmish", _label);
            if (_catalog == null)
                GUILayout.Label("<color=#ff8080>SpriteCatalog missing: run Tools > Tribal Trouble > Build Sprite Catalog first.</color>", new GUIStyle(_label) { richText = true, wordWrap = true });

            Row("Island size", () => _sizeIndex = GUILayout.Toolbar(_sizeIndex, SizeNames));
            Row("Terrain", () => _terrain = (Landscape.TerrainType)GUILayout.Toolbar((int)_terrain, new[] { "Tropical", "Northern" }));
            Row("Your tribe", () => _race = (Race)GUILayout.Toolbar((int)_race, new[] { "Natives", "Vikings" }));
            Row("Your colour", () =>
            {
                if (GUILayout.Button("<", GUILayout.Width(30))) _colorIndex = (_colorIndex + TeamColors.Defaults.Length - 1) % TeamColors.Defaults.Length;
                var c = GUI.color; GUI.color = TeamColors.Defaults[_colorIndex];
                GUILayout.Box(TeamColors.Names[_colorIndex], GUILayout.Width(140));
                GUI.color = c;
                if (GUILayout.Button(">", GUILayout.Width(30))) _colorIndex = (_colorIndex + 1) % TeamColors.Defaults.Length;
            });
            Row("Opponents", () => _ais = 1 + GUILayout.Toolbar(_ais - 1, new[] { "1", "2", "3", "4", "5" }));
            Row("Difficulty", () => _difficulty = (AIDifficulty)GUILayout.Toolbar((int)_difficulty, new[] { "Easy", "Normal", "Hard" }));
            Row("Hills", () => _hills = GUILayout.HorizontalSlider(_hills, 0f, 1f));
            Row("Vegetation", () => _vegetation = GUILayout.HorizontalSlider(_vegetation, 0f, 1f));
            Row("Resources", () => _supplies = GUILayout.HorizontalSlider(_supplies, 0f, 1f));
            Row("Seed (blank = random)", () => _seedText = GUILayout.TextField(_seedText, 9));
            if (_sizeIndex == 2) GUILayout.Label("Large islands take a while to generate.", _label);
            GUILayout.Space(12);
            GUI.enabled = _catalog != null;
            if (GUILayout.Button("Start game", GUILayout.Height(44))) StartGeneration();
            GUI.enabled = true;
            GUILayout.Space(6);
            if (GUILayout.Button("Quit", GUILayout.Height(28)))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            GUILayout.EndArea();
        }

        void Row(string label, Action control)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, _label, GUILayout.Width(160));
            control();
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
        }
    }
}
