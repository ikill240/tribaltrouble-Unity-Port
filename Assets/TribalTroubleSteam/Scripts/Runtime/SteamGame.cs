// Tribal Trouble, the Steam release (v2.2.207): its Java GUI and game loop (translated; TribalTroubleSteam.JavaSim)
// hosted by Unity, as the 1.4 game is by FaithfulGame - but nothing is shared with that game:
//  - Renderer.Boot / Renderer.Frame run what the Java Renderer.run and its main loop did: settings, the main menu
//    (with its island), every form, delegate and panel of the Steam game, and AnimationManager.runGameLoop.
//  - This class is the engine side (TribalTroubleSteam.GuiPort.IGuiBackend): it feeds Unity's keyboard and mouse to
//    the Java input (GLFW codes), draws the GUI's quads, loads the GUI textures, plays music and sounds, and draws the
//    world the Java GUI is showing (SteamView / SteamTerrain) through the Java camera, lit like the Steam renderer
//    (its classic lighting by day, its night colours and glow lights by night).
// Started by the game picker of the merged project (by name: no assembly of the other games refers to this one), or
// on its own by SteamBoot when this is the only game of the project.
using System;
using System.Collections.Generic;
using TribalTroubleSteam.GuiPort;
using TribalTroubleSteam.Presentation;
using UnityEngine;
using UnityEngine.Rendering;

namespace TribalTroubleSteam
{
    // first in every frame: Update waits for the simulation thread before any other script looks at the game
    [DefaultExecutionOrder(-1000)]
    public sealed class SteamGame : MonoBehaviour, IGuiBackend
    {
        /// <summary>Set by the game picker: "back to the game picker" instead of quitting (null: quit the application).</summary>
        public static Action ExitToLauncher;

        /// <summary>Where the Steam release's resources are imported: Resources/TTSteam/Java (GUI textures, cursors and
        /// fonts) and Resources/TTSteam/Resources (class-path files: gui_skin.xml, fonts, the map-code dictionary..).</summary>
        const string JavaTextures = "TTSteam/Java/";
        const string JavaResources = "TTSteam/Resources/";

        public Camera mainCamera;
        public Light sun;

        TTSpriteCatalog _catalog;
        com.oddlabs.net.NetworkSelector _network;
        com.oddlabs.tt.gui.GUI _gui;
        string _failure;

        static bool ConfineCursor => false;
        static bool ThreadedSim => true;

        // ------------------------------------------------------------------ lifecycle

        void Awake()
        {
            Application.targetFrameRate = 120;
            // keep running when the window is not in front: world loads wait for the main thread, and a lockstep
            // game (single player runs on a local server too) must keep ticking or it stalls / times out
            Application.runInBackground = true;
            _mainThread = System.Threading.Thread.CurrentThread.ManagedThreadId;
            _catalog = TTSpriteCatalog.Load();
            if (_catalog == null) _failure = "The Steam game's models are not imported yet (Tools > Tribal Trouble (Steam) > Build Sprite Catalog).";
            SetupCamera();
            java.lang.IO.Sink = s => Debug.Log("[TT Steam] " + s);
            java.util.logging.Logger.Sink = s => { if (s.StartsWith("SEVERE") || s.StartsWith("WARNING")) Debug.LogWarning("[TT Steam] " + s); };
            // its own folder: the Steam game's settings and saves never mix with the 1.4 / New Tides ones
            string dir = System.IO.Path.Combine(Application.persistentDataPath, "Steam");
            try { System.IO.Directory.CreateDirectory(dir); } catch { }
            _gameDir = dir;
            java.nio.file.Path.GameDirHook = dir;
            java.lang.JResources.Open = path => OnMain(() =>
            {
                string p = path.TrimStart('/');
                // fonts are their text dumps (font/<name>.font.txt), the rest a TextAsset without its extension
                var ta = Resources.Load<TextAsset>(JavaResources + p);
                if (ta == null)
                {
                    int dot = p.LastIndexOf('.');
                    if (dot > p.LastIndexOf('/')) ta = Resources.Load<TextAsset>(JavaResources + p.Substring(0, dot));
                }
                return ta != null ? ta.bytes : null;
            });
            com.oddlabs.tt.model.GeometryRegistry.ReadRegistry = () => OnMain(() => Resources.Load<TextAsset>(JavaResources + "geometry/registry")?.text);
            java.io.FileOutputStream.WritesEnabled = true;
            SteamStrings.Install();
            _music = gameObject.AddComponent<AudioSource>();
            _music.loop = true;
            _sfx = new SfxPlayer(_catalog != null ? _catalog.sfx : null, mainCamera);
            TribalTroubleSteam.Bridge.SimHooks.Sound = p =>
            {
                if (p.sound is com.oddlabs.tt.audio.SoundRef sr) _sounds.Enqueue((sr.location, p.x, p.y, p.z, p.gain, p.relative));
            };
            SpriteKeyAnimations();
            // (the GUI's input runs on the simulation thread: what only the main thread may do goes there)
            PortHooks.GetClipboardImpl = () => OnMain(() => GUIUtility.systemCopyBuffer);
            PortHooks.SetClipboardImpl = s => OnMain(() => GUIUtility.systemCopyBuffer = s ?? "");
            PortHooks.BrowseImpl = url => { if (!OffMain(() => Application.OpenURL(url))) Application.OpenURL(url); };
            PortHooks.RunLoadImpl = RunLoad;
            PortHooks.ScreenshotImpl = name => OnMain(() =>
            {
                // Ctrl+S / Ctrl+P: <persistent data>/Steam/screenshots/screenshotNNNNNN.png
                string sdir = System.IO.Path.Combine(_gameDir, "screenshots");
                try { System.IO.Directory.CreateDirectory(sdir); } catch (Exception) { }
                string path = name;
                if (string.IsNullOrEmpty(path))
                    for (int i = 0; ; i++) { path = System.IO.Path.Combine(sdir, "screenshot" + i.ToString("000000") + ".png"); if (!System.IO.File.Exists(path)) break; }
                StartCoroutine(SaveScreenshot(path));
                return path;
            });
            RenderPipelineManager.beginCameraRendering += BeginCamera;
            RenderPipelineManager.endCameraRendering += EndCamera;
            RenderPipelineManager.beginContextRendering += StartSimThread;
            ReadScreen();
            Host.Backend = this;
            // Steam first, as Main.main does (SteamManager.init): the menus ask SteamManager.isAvailable()
            try { TribalTroubleSteam.Bridge.OnlineHooks.StartSteam?.Invoke(); } catch (Exception e) { Debug.LogException(e); }
            BootGui();
            SteamAutorun.StartIfAsked(gameObject);
        }

        string _gameDir;

        /// <summary>The Java GUI's root (the autorun finds its buttons there).</summary>
        public com.oddlabs.tt.gui.GUIRoot GuiRoot => _gui?.getGUIRoot();
        public SteamView View => _view;
        /// <summary>The materials the world is drawn with (the autorun reports their shaders).</summary>
        public Material TerrainMaterialInUse => _terrain?.terrainMaterial;
        public Material SpriteMaterialInUse => _spriteMaterial;

        void BootGui()
        {
            try
            {
                SteamGlobals.InitGlobals();
                var q = com.oddlabs.tt.@event.LocalEventQueue.getQueue();
                _network = new com.oddlabs.net.NetworkSelector(q.getDeterministic(), new com.oddlabs.net.TimeManager_L(() => q.getMillis()));
                _gui = com.oddlabs.tt.render.Renderer.Boot(_gameDir, _network);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _failure = "The game failed to start: " + e.Message;
            }
        }

        /// <summary>The finished frame, read back and saved as a PNG.</summary>
        System.Collections.IEnumerator SaveScreenshot(string path)
        {
            yield return new WaitForEndOfFrame();
            var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            try
            {
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                tex.Apply(false);
                System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
                Debug.Log("[TT Steam] Screenshot saved: " + path);
            }
            catch (Exception e) { Debug.LogWarning("[TT Steam] Screenshot failed: " + e.Message); }
            finally { Destroy(tex); }
        }

        void OnDestroy()
        {
            JoinSim();
            StopSimThread();
            RenderPipelineManager.beginContextRendering -= StartSimThread;
            RenderPipelineManager.beginCameraRendering -= BeginCamera;
            RenderPipelineManager.endCameraRendering -= EndCamera;
            if (ReferenceEquals(Host.Backend, this)) Host.Backend = new NullBackend();
            _view?.Clear();
            _water?.Dispose(); _water = null;
            if (_worldRoot != null) Destroy(_worldRoot);
            if (_nightVolume != null) Destroy(_nightVolume.gameObject);
        }

        void OnApplicationQuit()
        {
            JoinSim();
            _quitting = true;
            try { com.oddlabs.tt.global.Settings.getSettings().save(); } catch (Exception) { }
        }

        void SetupCamera()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null)
            {
                var cg = new GameObject("Main Camera (Steam)") { tag = "MainCamera" };
                mainCamera = cg.AddComponent<Camera>();
                cg.AddComponent<AudioListener>();
                cg.AddComponent<SteamCameraTag>();
                cg.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
            }
            mainCamera.farClipPlane = 3000f;
            mainCamera.nearClipPlane = 1.2f;
            mainCamera.fieldOfView = 70f;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = new Color(0.55f, 0.72f, 0.85f);
            if (sun == null)
            {
                var lg = new GameObject("Sun (Steam)");
                sun = lg.AddComponent<Light>();
                sun.type = LightType.Directional;
                lg.AddComponent<SteamCameraTag>();
            }
            Lighting(false);
        }

        /// <summary>The Steam renderer's lighting (DefaultRenderer.render): classic by day - white sun from
        /// Globals.REAL_SKY_BODY_DIRECTION, ambient 0.65 - and at night its moonlight (sun 0.34/0.40/0.58, ambient
        /// 0.22/0.25/0.36) and scene tint 0.30/0.35/0.52 over the ground, the sea and the fog. The Steam shaders read
        /// them as globals (SteamShading); the Unity sun only casts the dynamic shadows. The glow lights add on top,
        /// as in its shaders (SteamView).</summary>
        void Lighting(bool night)
        {
            var dir = com.oddlabs.tt.global.Globals.REAL_SKY_BODY_DIRECTION;
            var toSun = TTSpace.ToUnity(dir.x(), dir.y(), dir.z()).normalized;
            sun.transform.rotation = Quaternion.LookRotation(-toSun, Vector3.up);
            sun.color = Color.white;
            sun.intensity = 1.15f;
            sun.shadows = SteamGlobals.DynamicShadows ? LightShadows.Soft : LightShadows.None;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.65f, 0.65f, 0.65f);
            RenderSettings.ambientEquatorColor = new Color(0.65f, 0.65f, 0.65f);
            RenderSettings.ambientGroundColor = new Color(0.65f, 0.65f, 0.65f);
            // (the night colours are the shaders' own now: no colour filter over the frame, the sky and GUI included)
            NightFilter(false);
            SteamShading.SetGlobals(night, SteamGlobals.DynamicShadows);
            _lit = night;
        }

        UnityEngine.Rendering.Volume _nightVolume;

        void NightFilter(bool night)
        {
            var data = mainCamera != null ? mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>() : null;
            if (night && _nightVolume == null)
            {
                var go = new GameObject("Night (Steam scene tint)");
                go.AddComponent<SteamCameraTag>();
                _nightVolume = go.AddComponent<UnityEngine.Rendering.Volume>();
                _nightVolume.isGlobal = true;
                _nightVolume.priority = 100f;
                var profile = ScriptableObject.CreateInstance<UnityEngine.Rendering.VolumeProfile>();
                var ca = profile.Add<UnityEngine.Rendering.Universal.ColorAdjustments>(true);
                ca.colorFilter.Override(new Color(0.30f, 0.35f, 0.52f));
                _nightVolume.sharedProfile = profile;
            }
            if (_nightVolume != null) _nightVolume.weight = night ? 1f : 0f;
            if (data != null) data.renderPostProcessing = night;
        }
        bool _lit;

        /// <summary>The island's fog (Landscape.getFogInfo: the terrain type's colour and density), darkened at night.</summary>
        void Fog(com.oddlabs.tt.resource.FogInfo fog, bool night)
        {
            RenderSettings.fog = true;
            var c = fog != null ? fog.getColor() : null;
            var color = c != null ? new Color(c.x(), c.y(), c.z()) : new Color(0.55f, 0.72f, 0.85f);
            // GlobalUniforms: the fog colour times the scene tint, so night fog darkens with the rest of the scene
            if (night) { var t = SteamShading.SceneTint(true); color = new Color(color.r * t.r, color.g * t.g, color.b * t.b); }
            RenderSettings.fogColor = color;
            mainCamera.backgroundColor = color;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 400f;
            RenderSettings.fogEndDistance = 1400f;
        }

        /// <summary>SpriteKey.getAnimationType (LOOP = 0, PLAIN = 1) comes from the imported sprite definitions.</summary>
        void SpriteKeyAnimations()
        {
            var catalog = _catalog;
            com.oddlabs.tt.render.SpriteKey.AnimationTypes = location =>
            {
                if (catalog == null || !SteamView.ParseLocation(location, out var g, out var n)) return null;
                var def = catalog.Find(g, n);
                if (def == null) return null;
                var types = new int[def.animations.Length];
                for (int i = 0; i < types.Length; i++) types[i] = def.animations[i].loop ? 0 : 1;
                return types;
            };
        }

        System.Threading.Thread _loadThread;
        volatile bool _loadDone;
        object _loadResult;
        Exception _loadError;
        Action<object> _loadContinue;

        /// <summary>The world load runs on a worker (with the large stack the island generator needs) while the
        /// screen keeps drawing the loading form; the game loop waits, like Java's blocking load.</summary>
        void RunLoad(Func<object> load, Action<object> done)
        {
            if (OffMain(() => RunLoad(load, done))) return;   // (asked for from the simulation thread)
            PortHooks.Loading = true;
            _loadDone = false; _loadResult = null; _loadError = null; _loadContinue = done;
            // No garbage collection while a world loads. Unity's collector is incremental: it does its marking in
            // slices on the main thread, a few milliseconds a frame, and a worker that needs memory while a collection
            // is under way waits for it to finish - so the island builder, which allocates all the time, crawled at
            // the loading screen's frame rate (the menu island took 24 s, a second game could seem to hang). The
            // heap grows by some hundred MB for the length of the load and is collected once afterwards.
            _loadGcCount = GC.CollectionCount(0);
            _loadWatch = System.Diagnostics.Stopwatch.StartNew();
            SetGcDuringLoad(true);
            _loadThread = new System.Threading.Thread(() =>
            {
                try { _loadResult = load(); }
                catch (Exception e) { _loadError = e; }
                finally { _loadDone = true; }
            }, 256 * 1024 * 1024) { IsBackground = true, Name = "TT world load" };
            _loadThread.Start();
        }

        int _loadGcCount;
        System.Diagnostics.Stopwatch _loadWatch;
        bool _gcHeld;

        void SetGcDuringLoad(bool loading)
        {
            if (Application.isEditor) return;   // (the editor can't switch its collector off)
            try
            {
                if (loading && !_gcHeld)
                {
                    UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Disabled;
                    _gcHeld = true;
                }
                else if (!loading && _gcHeld)
                {
                    UnityEngine.Scripting.GarbageCollector.GCMode = UnityEngine.Scripting.GarbageCollector.Mode.Enabled;
                    _gcHeld = false;
                    GC.Collect();
                }
            }
            catch (Exception e) { Debug.LogWarning("[TT] GC mode: " + e.Message); }
        }

        void FinishLoad()
        {
            if (!PortHooks.Loading || !_loadDone) return;
            PortHooks.Loading = false;
            _loadThread = null;
            SetGcDuringLoad(false);
            if (_loadWatch != null)
                Debug.Log($"[TT] World load took {_loadWatch.ElapsedMilliseconds} ms ({GC.CollectionCount(0) - _loadGcCount} collections during it)");
            var cont = _loadContinue; _loadContinue = null;
            if (_loadError != null)
            {
                Debug.LogException(_loadError);
                _failure = "Loading failed: " + _loadError.Message;
                try { com.oddlabs.tt.render.Renderer.startMenu(_network, _gui); } catch (Exception e) { Debug.LogException(e); }
                return;
            }
            try { cont?.Invoke(_loadResult); }
            catch (Exception e) { Debug.LogException(e); _failure = "Loading failed: " + e.Message; }
        }

        // ------------------------------------------------------------------ frame

        float _frameMs, _simMs, _lastSimRaw;
        bool _showPerf;

        void Update()
        {
            JoinSim();   // before anything else looks at the game
            _frameMs = Mathf.Lerp(_frameMs, Time.unscaledDeltaTime * 1000f, 0.05f);
            ThrottleWhileLoading();
            if (Input.GetKeyDown(KeyCode.F9)) _showPerf = !_showPerf;
            if (_music != null) _music.volume = SteamGlobals.MusicVolume;
            _frameWatch ??= new System.Diagnostics.Stopwatch();
            ReadScreen();
            RunMainQueue();
            // the overlay, presence and login callbacks (Java pumped them in its frame loop)
            try { TribalTroubleSteam.Bridge.OnlineHooks.Steam?.RunCallbacks(); } catch (Exception) { }
            if (_gui == null) return;
            PollMouseAndModifiers();
            FinishLoad();
            var sw = _frameWatch;
            sw.Restart();
            try
            {
                // the game loop ran on the simulation thread while the last frame was drawn: only the GUI is left
                if (_simRan) { _simRan = false; com.oddlabs.tt.render.Renderer.FrameRest(_gui); }
                else com.oddlabs.tt.render.Renderer.Frame(_network, _gui);
            }
            catch (Exception e) { Debug.LogException(e); }
            _lastSimRaw = (float)sw.Elapsed.TotalMilliseconds + _lastSimWait;
            _simMs = Mathf.Lerp(_simMs, _lastSimRaw, 0.1f);
            PlaySounds();
            if (Host.Finished) Leave();
        }

        /// <summary>Quit from the main menu: back to the game picker when there is one, else the application ends.</summary>
        void Leave()
        {
            enabled = false;
            JoinSim();
            StopSimThread();
            try { com.oddlabs.tt.global.Settings.getSettings().save(); } catch (Exception) { }
            if (ExitToLauncher != null)
            {
                var back = ExitToLauncher;
                if (_music != null) _music.Stop();
                Destroy(gameObject);
                if (mainCamera != null && mainCamera.gameObject.scene.IsValid() && mainCamera.GetComponent<SteamCameraTag>() != null) Destroy(mainCamera.gameObject);
                if (sun != null && sun.GetComponent<SteamCameraTag>() != null) Destroy(sun.gameObject);
                if (_nightVolume != null) Destroy(_nightVolume.gameObject);
                back();
                return;
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        // ------------------------------------------------------------------ input (GLFW codes, y from the bottom)

        static readonly Dictionary<KeyCode, int> s_keys = BuildKeyMap();
        static readonly KeyCode[] s_modKeys = { KeyCode.LeftShift, KeyCode.RightShift, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftAlt, KeyCode.RightAlt, KeyCode.LeftCommand, KeyCode.RightCommand };
        readonly HashSet<int> _keysDown = new HashSet<int>();
        Vector3 _lastMouse = new Vector3(-1, -1);

        static Dictionary<KeyCode, int> BuildKeyMap()
        {
            var m = new Dictionary<KeyCode, int>
            {
                [KeyCode.Space] = 32, [KeyCode.Quote] = 39, [KeyCode.Comma] = 44, [KeyCode.Minus] = 45, [KeyCode.Period] = 46,
                [KeyCode.Slash] = 47, [KeyCode.Semicolon] = 59, [KeyCode.Equals] = 61, [KeyCode.LeftBracket] = 91,
                [KeyCode.Backslash] = 92, [KeyCode.RightBracket] = 93, [KeyCode.BackQuote] = 96,
                [KeyCode.Escape] = 256, [KeyCode.Return] = 257, [KeyCode.Tab] = 258, [KeyCode.Backspace] = 259,
                [KeyCode.Insert] = 260, [KeyCode.Delete] = 261, [KeyCode.RightArrow] = 262, [KeyCode.LeftArrow] = 263,
                [KeyCode.DownArrow] = 264, [KeyCode.UpArrow] = 265, [KeyCode.PageUp] = 266, [KeyCode.PageDown] = 267,
                [KeyCode.Home] = 268, [KeyCode.End] = 269, [KeyCode.CapsLock] = 280, [KeyCode.ScrollLock] = 281,
                [KeyCode.Numlock] = 282, [KeyCode.Print] = 283, [KeyCode.Pause] = 284,
                [KeyCode.KeypadPeriod] = 330, [KeyCode.KeypadDivide] = 331, [KeyCode.KeypadMultiply] = 332,
                [KeyCode.KeypadMinus] = 333, [KeyCode.KeypadPlus] = 334, [KeyCode.KeypadEnter] = 335, [KeyCode.KeypadEquals] = 336,
                [KeyCode.LeftShift] = 340, [KeyCode.LeftControl] = 341, [KeyCode.LeftAlt] = 342, [KeyCode.LeftCommand] = 343,
                [KeyCode.RightShift] = 344, [KeyCode.RightControl] = 345, [KeyCode.RightAlt] = 346, [KeyCode.RightCommand] = 347,
                [KeyCode.Menu] = 348,
            };
            for (int i = 0; i <= 9; i++) { m[KeyCode.Alpha0 + i] = 48 + i; m[KeyCode.Keypad0 + i] = 320 + i; }
            for (int i = 0; i < 26; i++) m[KeyCode.A + i] = 65 + i;
            for (int i = 0; i < 15; i++) m[KeyCode.F1 + i] = 290 + i;
            return m;
        }

        static int Mods(EventModifiers m)
        {
            int r = 0;
            if ((m & EventModifiers.Shift) != 0) r |= 1;
            if ((m & EventModifiers.Control) != 0) r |= 2;
            if ((m & EventModifiers.Alt) != 0) r |= 4;
            if ((m & EventModifiers.Command) != 0) r |= 8;
            return r;
        }

        static int CurrentMods()
        {
            int r = 0;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) r |= 1;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) r |= 2;
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) r |= 4;
            if (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) r |= 8;
            return r;
        }

        void PollMouseAndModifiers()
        {
            if (!Application.isFocused) { if (_keysDown.Count > 0) { _keysDown.Clear(); Host.Input.ReleaseAll(); } return; }
            var mp = Input.mousePosition;
            if (_relative && (Input.GetMouseButton(2) || Time.frameCount - _relativeFrame < 20))
            {
                // first person look (middle mouse held): the original warps the cursor back after every move, so the
                // view turns with the mouse's movement, not with how far it has gone from where it was clicked.
                // Unity can't warp the cursor, so it is locked and the game gets its movement instead.
                _relDX += Input.GetAxisRaw("Mouse X") * MouseAxisToPixels;
                _relDY += Input.GetAxisRaw("Mouse Y") * MouseAxisToPixels;
                int dx = (int)_relDX, dy = (int)_relDY;
                if (dx != 0 || dy != 0)
                {
                    _relDX -= dx; _relDY -= dy;
                    Host.Input.MouseMoveIn(_relativeX + dx, _relativeY + dy);
                }
            }
            else
            {
                if (_relative) EndRelative();
                if (mp != _lastMouse)
                {
                    _lastMouse = mp;
                    Host.Input.MouseMoveIn(Mathf.Clamp((int)mp.x, 0, Screen.width - 1), Mathf.Clamp((int)mp.y, 0, Screen.height - 1));
                }
            }
            for (int b = 0; b < 3; b++)
            {
                if (Input.GetMouseButtonDown(b)) Host.Input.MouseButtonIn(b, true);
                if (Input.GetMouseButtonUp(b)) Host.Input.MouseButtonIn(b, false);
            }
            float wheel = Input.mouseScrollDelta.y;
            if (wheel != 0f) Host.Input.WheelIn(wheel);
            // modifier keys on their own (OnGUI doesn't report them as key events everywhere)
            foreach (var k in s_modKeys)
            {
                int code = s_keys[k];
                if (Input.GetKeyDown(k) && _keysDown.Add(code)) Host.Input.KeyEventIn(code, true, false, CurrentMods());
                if (Input.GetKeyUp(k) && _keysDown.Remove(code)) Host.Input.KeyEventIn(code, false, false, CurrentMods());
            }
        }

        void KeyEvents(Event e)
        {
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode != KeyCode.None && s_keys.TryGetValue(e.keyCode, out int code) && Array.IndexOf(s_modKeys, e.keyCode) < 0)
                {
                    bool repeat = !_keysDown.Add(code);
                    Host.Input.KeyEventIn(code, true, repeat, Mods(e.modifiers));
                }
                if (e.character != 0 && e.character != '\n' && e.character != '\r' && e.character != '\t' && !char.IsControl(e.character))
                    Host.Input.CharIn(e.character);
                e.Use();
            }
            else if (e.type == EventType.KeyUp)
            {
                if (e.keyCode != KeyCode.None && s_keys.TryGetValue(e.keyCode, out int code) && Array.IndexOf(s_modKeys, e.keyCode) < 0)
                {
                    _keysDown.Remove(code);
                    Host.Input.KeyEventIn(code, false, false, Mods(e.modifiers));
                }
                e.Use();
            }
        }
        // ------------------------------------------------------------------ IGuiBackend

        // read on the main thread each frame (the world-load worker asks too)
        int _screenW = 1280, _screenH = 720;
        bool _focused = true, _fullscreen;
        public int Width => _screenW;
        public int Height => _screenH;
        public bool Active => _focused;
        public bool Fullscreen => _fullscreen;

        void ReadScreen()
        {
            _screenW = Mathf.Max(1, Screen.width); _screenH = Mathf.Max(1, Screen.height);
            _focused = Application.isFocused; _fullscreen = Screen.fullScreen;
        }

        readonly List<Texture2D> _textures = new List<Texture2D>();
        readonly Dictionary<string, int> _textureByPath = new Dictionary<string, int>();
        readonly Dictionary<int, Material> _guiMaterials = new Dictionary<int, Material>();
        Shader _guiShader;

        // Texture calls from the world-load worker run on the main thread (Unity objects are main-thread only).
        int _mainThread = -1;
        volatile bool _quitting;

        readonly System.Collections.Concurrent.ConcurrentQueue<Action> _mainQueue = new System.Collections.Concurrent.ConcurrentQueue<Action>();

        T OnMain<T>(Func<T> f)
        {
            if (_mainThread < 0 || System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThread) return f();
            T r = default; Exception err = null;
            using (var done = new System.Threading.ManualResetEventSlim())
            {
                _mainQueue.Enqueue(() => { try { r = f(); } catch (Exception e) { err = e; } finally { done.Set(); } });
                _simWake.Set();
                // no time limit: the main thread can be away for a long time (window minimised, a slow first frame)
                while (!done.Wait(1000))
                    if (_quitting) throw new OperationCanceledException("the game is closing");
            }
            if (err != null) throw err;
            return r;
        }

        void RunMainQueue()
        {
            while (_mainQueue.TryDequeue(out var a)) a();
        }

        public int LoadTexture(string path, out int width, out int height)
        {
            var r = OnMain(() => { int h = LoadTextureMain(path, out int w, out int hh); return (h, w, hh); });
            width = r.w; height = r.hh;
            return r.h;
        }

        public int CreateTexture(int width, int height, int[] abgr) => OnMain(() => CreateTextureMain(width, height, abgr));

        int LoadTextureMain(string path, out int width, out int height)
        {
            if (_textureByPath.TryGetValue(path, out int h)) { width = _textures[h].width; height = _textures[h].height; return h; }
            string p = path.TrimStart('/');
            int dot = p.LastIndexOf('.');
            if (dot > p.LastIndexOf('/')) p = p.Substring(0, dot);
            var tex = Resources.Load<Texture2D>(JavaTextures + p);
            if (tex == null) { Debug.LogWarning("[TT] GUI texture missing: " + path); tex = Texture2D.whiteTexture; }
            h = _textures.Count;
            _textures.Add(tex);
            _textureByPath[path] = h;
            width = tex.width; height = tex.height;
            return h;
        }

        int CreateTextureMain(int width, int height, int[] abgr)
        {
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[width * height];
            for (int i = 0; i < px.Length && i < abgr.Length; i++)
            {
                int c = abgr[i];
                px[i] = new Color32((byte)c, (byte)(c >> 8), (byte)(c >> 16), (byte)(c >> 24));
            }
            tex.SetPixels32(px);
            tex.Apply(false, false);
            _textures.Add(tex);
            return _textures.Count - 1;
        }

        public void ReleaseTexture(int handle) { }

        List<GuiQuad> _frameQuads = new List<GuiQuad>();
        float _guiW = 1, _guiH = 1;

        public void DrawGui(float guiWidth, float guiHeight, List<GuiQuad> quads)
        {
            _guiW = guiWidth; _guiH = guiHeight;
            _frameQuads.Clear();
            _frameQuads.AddRange(quads);
        }

        Material GuiMaterial(int tex)
        {
            if (_guiMaterials.TryGetValue(tex, out var m)) return m;
            _guiShader ??= Shader.Find("TribalTrouble/GuiQuad");
            m = new Material(_guiShader) { hideFlags = HideFlags.DontSave };
            m.mainTexture = tex >= 0 && tex < _textures.Count ? _textures[tex] : Texture2D.whiteTexture;
            _guiMaterials[tex] = m;
            return m;
        }

        // The Java GUI's quads, as one mesh (a submesh per run of quads sharing a texture) rebuilt only when the GUI
        // changed - drawing them one by one in GL immediate mode cost a millisecond or more every frame.
        Mesh _guiMesh;
        GuiQuad[] _guiDrawn = new GuiQuad[0];
        int _guiDrawnCount = -1;
        bool _guiDrawnLinear;
        readonly List<Vector3> _gv = new List<Vector3>();
        readonly List<Vector2> _guv = new List<Vector2>();
        readonly List<Color> _gc = new List<Color>();
        readonly List<int> _gi = new List<int>();
        readonly List<int> _guiRunTex = new List<int>();

        static bool SameQuad(in GuiQuad a, in GuiQuad b) =>
            a.tex == b.tex && a.abgr == b.abgr && a.x0 == b.x0 && a.y0 == b.y0 && a.x1 == b.x1 && a.y1 == b.y1
            && a.x2 == b.x2 && a.y2 == b.y2 && a.x3 == b.x3 && a.y3 == b.y3 && a.u1 == b.u1 && a.v1 == b.v1 && a.u2 == b.u2 && a.v2 == b.v2;

        void DrawGuiQuads()
        {
            int n = _frameQuads.Count;
            if (n == 0) return;
            bool linear = QualitySettings.activeColorSpace == ColorSpace.Linear;
            bool changed = n != _guiDrawnCount || linear != _guiDrawnLinear || _guiMesh == null;
            if (!changed)
                for (int i = 0; i < n; i++)
                    if (!SameQuad(_frameQuads[i], _guiDrawn[i])) { changed = true; break; }
            if (changed) BuildGuiMesh(linear);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, _guiW, 0, _guiH);
            for (int r = 0; r < _guiRunTex.Count; r++)
            {
                GuiMaterial(_guiRunTex[r]).SetPass(0);
                Graphics.DrawMeshNow(_guiMesh, Matrix4x4.identity, r);
            }
            GL.PopMatrix();
        }

        void BuildGuiMesh(bool linear)
        {
            int n = _frameQuads.Count;
            if (_guiDrawn.Length < n) _guiDrawn = new GuiQuad[Mathf.NextPowerOfTwo(n)];
            _frameQuads.CopyTo(_guiDrawn);
            _guiDrawnCount = n;
            _guiDrawnLinear = linear;
            if (_guiMesh == null) { _guiMesh = new Mesh { name = "Java GUI", hideFlags = HideFlags.DontSave }; _guiMesh.MarkDynamic(); }
            _gv.Clear(); _guv.Clear(); _gc.Clear(); _guiRunTex.Clear();
            for (int i = 0; i < n; i++)
            {
                var q = _frameQuads[i];
                var c = new Color((q.abgr & 0xff) / 255f, ((q.abgr >> 8) & 0xff) / 255f, ((q.abgr >> 16) & 0xff) / 255f, ((q.abgr >> 24) & 0xff) / 255f);
                if (linear) { var l = c.linear; l.a = c.a; c = l; }
                _gv.Add(new Vector3(q.x0, q.y0, 0)); _guv.Add(new Vector2(q.u1, q.v1));
                _gv.Add(new Vector3(q.x1, q.y1, 0)); _guv.Add(new Vector2(q.u2, q.v1));
                _gv.Add(new Vector3(q.x2, q.y2, 0)); _guv.Add(new Vector2(q.u2, q.v2));
                _gv.Add(new Vector3(q.x3, q.y3, 0)); _guv.Add(new Vector2(q.u1, q.v2));
                _gc.Add(c); _gc.Add(c); _gc.Add(c); _gc.Add(c);
            }
            _guiMesh.Clear();
            _guiMesh.indexFormat = _gv.Count > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
            _guiMesh.SetVertices(_gv); _guiMesh.SetUVs(0, _guv); _guiMesh.SetColors(_gc);
            // one submesh per run of quads with the same texture, in drawing order
            int runs = 0;
            for (int i = 0; i < n; i++) if (i == 0 || _frameQuads[i].tex != _frameQuads[i - 1].tex) runs++;
            _guiMesh.subMeshCount = runs;
            int sub = 0, start = 0;
            for (int i = 1; i <= n; i++)
            {
                if (i < n && _frameQuads[i].tex == _frameQuads[start].tex) continue;
                _gi.Clear();
                for (int k = start; k < i; k++) { int v = k * 4; _gi.Add(v); _gi.Add(v + 1); _gi.Add(v + 2); _gi.Add(v); _gi.Add(v + 2); _gi.Add(v + 3); }
                _guiMesh.SetTriangles(_gi, sub, false);
                _guiRunTex.Add(_frameQuads[start].tex);
                sub++; start = i;
            }
            _guiMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1e6f);
        }

        // music: /music/menu.ogg etc. from the sprite catalog's clips
        AudioSource _music;
        bool OffMain(Action a)
        {
            if (_mainThread < 0 || System.Threading.Thread.CurrentThread.ManagedThreadId == _mainThread) return false;
            _mainQueue.Enqueue(a);
            _simWake.Set();
            return true;
        }

        public void SetMusic(string path, float delay)
        {
            if (OffMain(() => SetMusic(path, delay))) return;
            if (_catalog == null || _music == null || string.IsNullOrEmpty(path)) return;
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            foreach (var c in _catalog.music)
                if (c != null && c.name == name)
                {
                    if (_music.clip == c && _music.isPlaying) return;
                    _music.clip = c;
                    _music.PlayDelayed(delay);
                    return;
                }
        }

        public void ToggleFullscreen() { if (!OffMain(ToggleFullscreen)) Screen.fullScreen = !Screen.fullScreen; }

        readonly Dictionary<string, Texture2D> _cursors = new Dictionary<string, Texture2D>();
        bool _cursorHidden;
        public void SetCursor(string image, int hotX, int hotY)
        {
            if (OffMain(() => SetCursor(image, hotX, hotY))) return;
            if (image == com.oddlabs.tt.gui.Cursor.HIDDEN) { UnityEngine.Cursor.visible = false; _cursorHidden = true; return; }
            if (_cursorHidden) { _cursorHidden = false; UnityEngine.Cursor.visible = !(Host.Input != null && Host.Input.Grabbed); }
            if (string.IsNullOrEmpty(image)) { UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); return; }
            if (!_cursors.TryGetValue(image, out var tex))
            {
                string p = image.TrimStart('/');
                int dot = p.LastIndexOf('.');
                if (dot > p.LastIndexOf('/')) p = p.Substring(0, dot);
                tex = Resources.Load<Texture2D>(JavaTextures + p);
                _cursors[image] = tex;
            }
            UnityEngine.Cursor.SetCursor(tex, new Vector2(hotX, hotY), CursorMode.Auto);
        }

        public void SetGrabbed(bool grabbed)
        {
            if (OffMain(() => SetGrabbed(grabbed))) return;
            if (WarpCursor.Available)
            {
                // Windows build: like the original, the hidden cursor stays where it was pressed (the game puts it back
                // after every move), and shows up there again afterwards - no locking to the middle of the screen.
                UnityEngine.Cursor.lockState = ConfineCursor ? CursorLockMode.Confined : CursorLockMode.None;
                UnityEngine.Cursor.visible = !grabbed;
                return;
            }
            UnityEngine.Cursor.lockState = grabbed ? CursorLockMode.Locked : (ConfineCursor && !Application.isEditor ? CursorLockMode.Confined : CursorLockMode.None);
            UnityEngine.Cursor.visible = !grabbed;
        }

        // ---- relative mouse (first person look): see PollMouseAndModifiers
        const float MouseAxisToPixels = 10f;   // "Mouse X/Y" = pixels moved x the axis sensitivity (0.1)
        bool _relative;
        int _relativeX, _relativeY, _relativeFrame;
        float _relDX, _relDY;

        /// <summary>The game puts the cursor back where it was (first person look does it after every move).</summary>
        public void SetCursorPosition(int x, int y)
        {
            if (OffMain(() => SetCursorPosition(x, y))) return;
            if (WarpCursor.Available)
            {
                WarpCursor.To(x, Screen.height - 1 - y);
                _lastMouse = new Vector3(x, y);   // the next move is measured from here, as in the original
                return;
            }
            _relativeX = x; _relativeY = y; _relativeFrame = Time.frameCount;
            if (_relative) return;
            _relative = true;
            _relDX = _relDY = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        void EndRelative()
        {
            _relative = false;
            bool grabbed = Host.Input != null && Host.Input.Grabbed;
            UnityEngine.Cursor.lockState = grabbed ? CursorLockMode.Locked : (ConfineCursor && !Application.isEditor ? CursorLockMode.Confined : CursorLockMode.None);
            UnityEngine.Cursor.visible = !grabbed;
            _lastMouse = new Vector3(-1, -1);   // report where the cursor is now
        }

        public void Quit() { }
        /// <summary>Moving the real cursor (Windows player only; the editor's game view isn't the window's client area,
        /// so there the cursor is locked and the mouse's movement used instead).</summary>
        static class WarpCursor
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            public static bool Available => true;
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            struct POINT { public int X, Y; }
            [System.Runtime.InteropServices.DllImport("user32.dll")] static extern IntPtr GetActiveWindow();
            [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool ClientToScreen(IntPtr hWnd, ref POINT p);
            [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
            static IntPtr s_window;
            /// <summary>x, y: client pixels, top-left origin.</summary>
            public static void To(int x, int y)
            {
                if (s_window == IntPtr.Zero) s_window = GetActiveWindow();
                var p = new POINT { X = x, Y = y };
                if (s_window != IntPtr.Zero && ClientToScreen(s_window, ref p)) SetCursorPos(p.X, p.Y);
            }
#else
            public static bool Available => false;
            public static void To(int x, int y) { }
#endif
        }
        // ------------------------------------------------------------------ sounds

        SfxPlayer _sfx;
        readonly System.Collections.Concurrent.ConcurrentQueue<(string sound, float x, float y, float z, float gain, bool relative)> _sounds =
            new System.Collections.Concurrent.ConcurrentQueue<(string, float, float, float, float, bool)>();

        void PlaySounds()
        {
            while (_sounds.TryDequeue(out var s))
            {
                if (_sfx == null || string.IsNullOrEmpty(s.sound)) continue;
                string name = System.IO.Path.GetFileNameWithoutExtension(s.sound);
                if (s.relative) _sfx.PlayUi(name);
                else _sfx.Play(name, TTSpace.ToUnity(s.x, s.y, s.z));
            }
        }

        // ------------------------------------------------------------------ world view

        GameObject _worldRoot;
        Material _spriteMaterial, _terrainMaterial, _waterMaterial;
        SteamWater _water;
        SteamView _view;
        SteamTerrain _terrain;
        com.oddlabs.tt.landscape.World _viewWorld;

        void LateUpdate()
        {
            if (_gui == null) return;
            RunMainQueue();
            var dr = Host.CurrentWorld;
            var world = dr != null && !dr.closed ? dr.world : null;
            if (world != _viewWorld) BuildView(dr);
            if (_view == null || Host.CurrentCamera == null) return;
            GroundUpdates();
            // night can change during a game (World.toggleNight, the Steam console's /night)
            bool night = world.isNight();
            if (night != _lit) { Lighting(night); Fog(dr.generator.getFogInfo(), night); }
            _view.night = night;
            // (map mode is a flat overview: no sun pass, static shadows, as DefaultRenderer.render)
            SteamShading.SetGlobals(night, SteamGlobals.DynamicShadows && !Host.CurrentCamera.inNoDetailMode());
            ApplyJavaCamera(Host.CurrentCamera);
            _water?.Update(mainCamera);
            _view.Render(world, mainCamera);
            _terrain?.UpdateLod(mainCamera.transform.position);
        }

        /// <summary>Ground a building flattened (EditorView.GroundEdited): a few tiles a frame.</summary>
        void GroundUpdates()
        {
            if (_terrain == null) return;
            const int T = EditorView.GroundTile;
            for (int n = 0; n < 8 && EditorView.TakeGroundTile(out int tx, out int ty); n++)
                _terrain.RebuildCells(tx * T, ty * T, tx * T + T - 1, ty * T + T - 1);
        }

        void BuildView(com.oddlabs.tt.render.DefaultRenderer dr)
        {
            _view?.Clear();
            _water?.Dispose(); _water = null;
            if (_worldRoot != null) Destroy(_worldRoot);
            _worldRoot = null; _view = null; _terrain = null; _viewWorld = null;
            if (dr == null || dr.closed || dr.world == null || dr.worldInfo == null || _catalog == null) return;
            _viewWorld = dr.world;
            EditorView.Reset();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            _worldRoot = new GameObject("Tribal Trouble (Steam) world");
            _terrain = new SteamTerrain(dr.worldInfo);
            _terrain.withColliders = _terrain.size < 1024;
            // the Steam game's own shading: copies of the catalog's materials on its shaders (SteamShading)
            _spriteMaterial ??= SteamShading.OnShader(_catalog.baseMaterial, SteamShading.Sprite, "TTSteamSprite (Steam shading)");
            _terrainMaterial ??= SteamShading.OnShader(_catalog.terrainMaterial, SteamShading.Terrain, "TTSteamTerrain (Steam shading)");
            _waterMaterial ??= SteamShading.OnShader(_catalog.waterMaterial, SteamShading.Water, "TTSteamWater (Steam shading)");
            _terrain.Build(dr.worldInfo, _terrainMaterial, _waterMaterial, _worldRoot.transform);
            com.oddlabs.tt.procedural.Landscape.TerrainType terrainType = null;
            try { terrainType = dr.generator?.getTerrainType(); } catch (Exception) { }
            _water = new SteamWater(_terrain.waterMaterial, _terrain.seaLevel, terrainType, _worldRoot.transform);
            var entities = new GameObject("Entities").transform;
            entities.SetParent(_worldRoot.transform, false);
            _view = new SteamView(_catalog, entities, _spriteMaterial);
            _view.overlay = new SteamWorldOverlay(_view, _terrain);
            bool night = dr.world.isNight();
            Lighting(night);
            Fog(dr.generator.getFogInfo(), night);
            QualitySettings.shadowDistance = SteamGlobals.DynamicShadows ? _view.shadowDistance : 0f;
            Debug.Log($"[TT Steam] View built in {sw.ElapsedMilliseconds} ms ({_terrain.size} grid){(night ? ", night" : "")}");
        }

        /// <summary>The Java camera's view and projection, with the TT (z up) to Unity (y up) axis swap folded in.
        /// (F10 inverts culling, to check the handedness by eye.)</summary>
        void ApplyJavaCamera(com.oddlabs.tt.camera.CameraState cs)
        {
            var mv = FromJoml(cs.getModelView());
            var proj = FromJoml(cs.getProjectionMatrix());
            var swap = new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 0, 1));
            var view = mv * swap;
            var eye = TTSpace.ToUnity(cs.getCurrentX(), cs.getCurrentY(), cs.getCurrentZ());
            float h = cs.getHorizAngle(), v = cs.getCurrentVertAngle();
            var fwd = TTSpace.ToUnity(Mathf.Cos(h) * Mathf.Cos(v), Mathf.Sin(h) * Mathf.Cos(v), Mathf.Sin(v));
            mainCamera.transform.SetPositionAndRotation(eye, Quaternion.LookRotation(fwd.sqrMagnitude > 1e-6f ? fwd : Vector3.forward, Vector3.up));
            // near and far from the Java projection (OpenGL: m22 = -(f+n)/(f-n), m23 = -2fn/(f-n)), so URP's fog and
            // depth terms (which read the camera's planes) agree with the matrix
            float n = proj.m23 / (proj.m22 - 1f), f = proj.m23 / (proj.m22 + 1f);
            if (!(n > 0f && f > n) || float.IsInfinity(f)) n = com.oddlabs.tt.global.Globals.VIEW_MIN;
            f = Mathf.Max(float.IsInfinity(f) ? 0f : f, 9000f);    // the far plane reaches the sky dome
            proj.m22 = -(f + n) / (f - n);
            proj.m23 = -2f * f * n / (f - n);
            mainCamera.nearClipPlane = n; mainCamera.farClipPlane = f;
            mainCamera.fieldOfView = 2f * Mathf.Atan(1f / proj.m11) * Mathf.Rad2Deg;
            mainCamera.worldToCameraMatrix = view;
            mainCamera.projectionMatrix = proj;
            // Java's eye space is OpenGL's, like Unity's worldToCameraMatrix, and the TT->Unity axis swap turns TT's
            // right-handed world into Unity's left-handed one: the product has Unity's usual handedness, so no culling flip
            _cameraMirrored = s_invertCulling;
        }

        static Matrix4x4 FromJoml(org.joml.Matrix4f m)
        {
            var r = new Matrix4x4();
            r.m00 = m.m00(); r.m01 = m.m10(); r.m02 = m.m20(); r.m03 = m.m30();
            r.m10 = m.m01(); r.m11 = m.m11(); r.m12 = m.m21(); r.m13 = m.m31();
            r.m20 = m.m02(); r.m21 = m.m12(); r.m22 = m.m22(); r.m23 = m.m32();
            r.m30 = m.m03(); r.m31 = m.m13(); r.m32 = m.m23(); r.m33 = m.m33();
            return r;
        }

        bool _cameraMirrored;
        static bool s_invertCulling;   // F10 flips it (diagnostics)
        void BeginCamera(ScriptableRenderContext ctx, Camera cam)
        {
            if (cam == mainCamera && _cameraMirrored) GL.invertCulling = true;
            // the reflection's view is mirrored (SteamWater): its faces turn the other way
            else if (_water != null && cam == _water.ReflectionCamera) GL.invertCulling = !_cameraMirrored;
        }
        void EndCamera(ScriptableRenderContext ctx, Camera cam) { if (cam == mainCamera || (_water != null && cam == _water.ReflectionCamera)) GL.invertCulling = false; }
        // ------------------------------------------------------------------ the simulation thread
        // The game loop (ticks, input, network) of the next frame runs on a thread of its own from the moment this
        // frame's drawing starts (every script's Update and LateUpdate done, the world view built) until the next
        // Update, which waits for it before anything looks at the game. Same order as one thread - loop, then GUI
        // and world view - so the game is the same; what only the main thread may do the loop sends there
        // (OnMain / OffMain), and the wait runs it.
        System.Threading.Thread _simThread;
        readonly System.Threading.AutoResetEvent _simGo = new System.Threading.AutoResetEvent(false);
        readonly System.Threading.ManualResetEventSlim _simWake = new System.Threading.ManualResetEventSlim(false);
        volatile bool _simBusy, _simStop;
        bool _simRan;
        int _simFrame = -1;
        Exception _simError;
        float _lastSimWait;
        public float LastSimWaitMs => _lastSimWait;
        public bool SimThreaded => _simThread != null && ThreadedSim;

        void StartSimThread(ScriptableRenderContext ctx, List<Camera> cams)
        {
            if (!ThreadedSim  || _gui == null || _quitting || _simBusy || _simRan) return;
            if (Host.Finished || PortHooks.Loading || mainCamera == null || !cams.Contains(mainCamera)) return;
            if (_simFrame == Time.frameCount) return;
            _simFrame = Time.frameCount;
            if (_simThread == null)
            {
                _simThread = new System.Threading.Thread(SimThreadMain, 64 * 1024 * 1024) { IsBackground = true, Name = "TT simulation" };
                _simThread.Start();
            }
            _simRan = true;
            _simBusy = true;
            _simGo.Set();
        }

        void SimThreadMain()
        {
            while (true)
            {
                _simGo.WaitOne();
                if (_simStop) return;
                try { com.oddlabs.tt.render.Renderer.FrameLoop(_network, _gui); }
                catch (Exception e) { _simError = e; }
                finally { _simBusy = false; _simWake.Set(); }
            }
        }

        /// <summary>Waits for the simulation thread (doing what it sends to the main thread meanwhile).</summary>
        void JoinSim()
        {
            _lastSimWait = 0f;
            if (!_simBusy) { RunMainQueue(); ReportSimError(); return; }
            long t0 = System.Diagnostics.Stopwatch.GetTimestamp();
            while (true)
            {
                _simWake.Reset();
                RunMainQueue();
                if (!_simBusy) break;
                _simWake.Wait(100);
            }
            RunMainQueue();
            _lastSimWait = (float)((System.Diagnostics.Stopwatch.GetTimestamp() - t0) * 1000.0 / System.Diagnostics.Stopwatch.Frequency);
            ReportSimError();
        }

        void ReportSimError()
        {
            var e = _simError;
            if (e == null) return;
            _simError = null;
            Debug.LogException(e);
        }

        void StopSimThread()
        {
            if (_simThread == null) return;
            _simStop = true;
            _simGo.Set();
            _simThread = null;
        }
        System.Diagnostics.Stopwatch _frameWatch;

        // While a world loads on its worker, the loading screen is drawn at 30 frames a second: drawing it as fast as
        // the GPU allows (vsync is often off or "broken") made the main thread allocate non-stop, and every garbage
        // collection stops the loading thread too - island builds took 20-50 s instead of a few.
        bool _throttled;
        int _savedVsync, _savedFps;

        void ThrottleWhileLoading()
        {
            bool loading = PortHooks.Loading;
            if (loading && !_throttled)
            {
                _savedVsync = QualitySettings.vSyncCount; _savedFps = Application.targetFrameRate;
                QualitySettings.vSyncCount = 0; Application.targetFrameRate = 30;
                _throttled = true;
            }
            else if (!loading && _throttled)
            {
                QualitySettings.vSyncCount = _savedVsync; Application.targetFrameRate = _savedFps;
                _throttled = false;
            }
        }


        // ------------------------------------------------------------------ OnGUI

        void OnGUI()
        {
            var e = Event.current;
            if (e.type == EventType.KeyDown || e.type == EventType.KeyUp) { KeyEvents(e); return; }
            if (e.type != EventType.Repaint) return;
            DrawGuiQuads();
            if (_failure != null)
            {
                var r = new Rect(20, Screen.height - 60, Screen.width - 40, 40);
                GUI.color = new Color(1f, 0.4f, 0.3f);
                GUI.Label(r, _failure);
                GUI.color = Color.white;
            }
            if (_showPerf) PerfGUI();
        }


        void PerfGUI()
        {
            string s = $"{1000f / Mathf.Max(1f, _frameMs):0} fps ({_frameMs:0.0} ms)  game {_simMs:0.0} ms";
            if (_view != null) s += $"\nitems {_view.statItems}  trees {_view.statTrees}  instances {_view.statInstances}  far {_view.statFar}  live {_view.statLive}  collect {_view.statCollectMs:0.0} ms  build {_view.statBuildMs:0.0} ms  draw {_view.statDrawMs:0.0} ms";
            GUI.Box(new Rect(10, 10, 720, 44), GUIContent.none);
            GUI.Label(new Rect(16, 12, 710, 44), s);
        }
    }

    /// <summary>Marks the camera and sun the Steam game made itself (removed with it on "back to the game picker").</summary>
    public sealed class SteamCameraTag : MonoBehaviour { }
}
