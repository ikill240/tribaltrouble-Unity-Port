// The game: the original Java GUI and game loop (translated; JavaSim) hosted by Unity.
//  - Renderer.Boot / Renderer.Frame run what the Java Renderer.run and its main loop did: settings, the main menu
//    (with its island), every form, delegate and panel of the original game, and AnimationManager.runGameLoop.
//  - This class is the engine side (TribalTrouble.GuiPort.IGuiBackend): it feeds Unity's keyboard and mouse to the
//    Java input (GLFW codes), draws the GUI's quads, loads the GUI textures, plays music and sounds, and draws the
//    world the Java GUI is showing (FaithfulView / FaithfulTerrain / TTEnvironment) through the Java camera.
using System;
using System.Collections.Generic;
using TribalTrouble.Faithful;
using TribalTrouble.GuiPort;
using TribalTrouble.Presentation;
using UnityEngine;
using UnityEngine.Rendering;
using RacesResources = com.oddlabs.tt.model.RacesResources;
using SfxPlayer = TribalTrouble.Game.SfxPlayer;

namespace TribalTrouble.FaithfulGame
{
    // first in every frame: Update waits for the simulation thread before any other script looks at the game
    [DefaultExecutionOrder(-1000)]
    public sealed partial class FaithfulGame : MonoBehaviour, IGuiBackend
    {
        public Camera mainCamera;
        public Light sun;

        TTSpriteCatalog _catalog;
        com.oddlabs.net.NetworkSelector _network;
        com.oddlabs.tt.gui.GUI _gui;
        string _failure;
        bool _dedicated;
        float _dedicatedLog;

        // ------------------------------------------------------------------ lifecycle

        void Awake()
        {
            Application.targetFrameRate = 120;
            // keep running when the window is not in front: world loads wait for the main thread, and a lockstep
            // game (single player runs on a local server too) must keep ticking or it stalls / times out
            Application.runInBackground = true;
            _mainThread = System.Threading.Thread.CurrentThread.ManagedThreadId;
            _catalog = TTSpriteCatalog.Load();
            SetupCamera();
            java.lang.IO.Sink = s => Debug.Log("[TT] " + s);
            java.util.logging.Logger.Sink = s => { if (s.StartsWith("SEVERE") || s.StartsWith("WARNING")) Debug.LogWarning("[TT] " + s); };
            java.nio.file.Path.GameDirHook = Application.persistentDataPath;
            // Java class-path resources (gui_skin.xml, fonts, the map-code dictionary, ...) live under Resources/TTResources.
            // (Resources.Load is main-thread only; world loads run on a worker)
            java.lang.JResources.Open = path => OnMain(() =>
            {
                string p = path.TrimStart('/');
                int dot = p.LastIndexOf('.');
                if (dot > p.LastIndexOf('/')) p = p.Substring(0, dot);
                var ta = Resources.Load<TextAsset>("TTResources/" + p);
                return ta != null ? ta.bytes : null;
            });
            try { System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Application.persistentDataPath, "maps")); } catch { }
            java.io.FileOutputStream.WritesEnabled = true;
            TTStrings.Install();
            TribalTrouble.Game.GameSettings.Load();
            TTOnline.Ensure();
            TTDiscord.Configure();
            if (DedicatedRequested())
            {
                // the game as a dedicated server: the matchmaking server + router only, no menus or rendering
                _dedicated = true;
                Application.targetFrameRate = 10;
                QualitySettings.vSyncCount = 0;
                mainCamera.enabled = false;
                PlayerServer.Start();
                Debug.Log("[TT] Dedicated server mode (-server). LAN addresses: " + string.Join(", ", PlayerServer.LanAddresses()));
                return;
            }
            foreach (var t in com.oddlabs.tt.procedural.Landscape.TerrainType.values()) TTEnvironment.Prewarm(t);
            _music = gameObject.AddComponent<AudioSource>();
            _music.loop = true;
            _sfx = new SfxPlayer(_catalog.sfx, mainCamera);
            TribalTrouble.Bridge.SimHooks.Sound = p =>
            {
                if (p.sound is com.oddlabs.tt.audio.SoundRef sr) _sounds.Enqueue((sr.location, p.x, p.y, p.z, p.gain, p.relative));
            };
            SpriteKeyAnimations();
            // (the GUI's input runs on the simulation thread: what only the main thread may do goes there)
            PortHooks.GetClipboardImpl = () => OnMain(() => GUIUtility.systemCopyBuffer);
            PortHooks.SetClipboardImpl = s => OnMain(() => GUIUtility.systemCopyBuffer = s ?? "");
            PortHooks.BrowseImpl = url => { if (!OffMain(() => Application.OpenURL(url))) Application.OpenURL(url); };
            PortHooks.RunLoadImpl = RunLoad;
            PortHooks.OpenEngineSettingsImpl = () => _engineSettings = true;
            PortHooks.ScreenshotImpl = name => OnMain(() =>
            {
                // Ctrl+S / Ctrl+P: <persistent data>/screenshots/screenshotNNNNNN.png (Java wrote a .bmp in the home folder)
                string dir = System.IO.Path.Combine(Application.persistentDataPath, "screenshots");
                try { System.IO.Directory.CreateDirectory(dir); } catch (Exception) { }
                string path = name;
                if (string.IsNullOrEmpty(path))
                    for (int i = 0; ; i++) { path = System.IO.Path.Combine(dir, "screenshot" + i.ToString("000000") + ".png"); if (!System.IO.File.Exists(path)) break; }
                StartCoroutine(SaveScreenshot(path));   // (the ScreenCapture module isn't in this project)
                return path;
            });
            RenderPipelineManager.beginCameraRendering += BeginCamera;
            RenderPipelineManager.endCameraRendering += EndCamera;
            RenderPipelineManager.beginContextRendering += StartSimThread;
            ReadScreen();
            Host.Backend = this;
            // two games in one: start the one picked before (or on the command line), or show the launcher
            if (SteamAutorunAsked()) StartSteam();   // an unattended look at the Steam game (steam=1 in autorun)
            else if (TribalTrouble.Game.Edition.Preset(out int game)) StartGame(game);
            else _launcher = true;
        }

        bool _launcher, _launcherRemember;
        Texture2D _launcherArt;

        static bool SteamAutorunAsked()
        {
            try { var f = AutoFile(); return f != null && System.IO.File.ReadAllText(f).Contains("steam=1"); }
            catch (System.Exception) { return false; }
        }

        void StartGame(int game)
        {
            switch (game)
            {
                case TribalTrouble.Game.Edition.GameSteam: StartSteam(); break;
                case TribalTrouble.Game.Edition.GameTiki: StartTiki(); break;
                case TribalTrouble.Game.Edition.GameClassic14: BootGui(false); break;
                default: BootGui(true); break;
            }
        }

        /// <summary>
        /// Tribal Trouble - the Steam release (v2.2.207) - is its own game (TribalTroubleSteam.* assemblies: its own
        /// translated Java game, never referenced by this one) and is started by name, like Tiki Madness: our camera,
        /// sun and music step aside until "back to the game picker" (its main menu's Quit).
        /// </summary>
        void StartSteam()
        {
            _launcher = false;
            var t = Type.GetType("TribalTroubleSteam.SteamGame, TribalTroubleSteam.Runtime");
            if (t == null) { _failure = "Tribal Trouble (the Steam release) is not part of this build."; _launcher = true; return; }
            if (mainCamera != null) mainCamera.gameObject.SetActive(false);
            if (sun != null) sun.gameObject.SetActive(false);
            if (_music != null) _music.Stop();
            Action back = () =>
            {
                if (mainCamera != null) mainCamera.gameObject.SetActive(true);
                if (sun != null) sun.gameObject.SetActive(true);
                SetupCamera();   // the other game changed the fog and the ambient light
                enabled = true;
                _launcher = true;
            };
            var exit = t.GetField("ExitToLauncher");
            if (exit != null) exit.SetValue(null, TribalTrouble.Game.Edition.Locked == null ? back : null);   // a project made for one game has no picker to go back to
            new GameObject("Tribal Trouble (Steam)").AddComponent(t);
            enabled = false;
        }

        /// <summary>
        /// Tiki Madness: New Tides is its own game (TikiMadness.* assemblies) and is started by name, so this game
        /// doesn't depend on it: our camera, sun and music step aside until "back to the game picker".
        /// </summary>
        void StartTiki()
        {
            _launcher = false;
            var t = Type.GetType("TikiMadness.TikiApp, TikiMadness.Client");
            if (t == null) { _failure = "Tiki Madness is not part of this build."; _launcher = true; return; }
            if (mainCamera != null) mainCamera.gameObject.SetActive(false);
            if (sun != null) sun.gameObject.SetActive(false);
            if (_music != null) _music.Stop();
            GameObject tikiGo = null;
            Action back = () =>
            {
                // nothing of this game was started yet: just show the picker again
                if (tikiGo != null) Destroy(tikiGo);
                if (mainCamera != null) mainCamera.gameObject.SetActive(true);
                if (sun != null) sun.gameObject.SetActive(true);
                SetupCamera();   // the other game changed the fog and the ambient light
                enabled = true;
                _launcher = true;
            };
            if (TribalTrouble.Game.Edition.Locked == null)   // a project made for one game has no picker to go back to
                t.GetField("ExitToLauncher")?.SetValue(null, back);
            tikiGo = new GameObject("TikiMadness");
            tikiGo.AddComponent(t);
            enabled = false;
        }

        /// <summary>Starts the Java GUI for the chosen game (the skin and the skirmish options depend on it).</summary>
        void BootGui(bool newTides)
        {
            _launcher = false;
            TribalTrouble.Game.Edition.Apply(newTides);
            TTSteam.Start();   // Steam (its app id from the options) once this game is the one playing
            try
            {
                FaithfulMatch.InitGlobals();
                var q = com.oddlabs.tt.@event.LocalEventQueue.getQueue();
                _network = new com.oddlabs.net.NetworkSelector(q.getDeterministic(), new com.oddlabs.net.TimeManager_L(() => q.getMillis()));
                _gui = com.oddlabs.tt.render.Renderer.Boot(Application.persistentDataPath, _network);
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
                Debug.Log("[TT] Screenshot saved: " + path);
            }
            catch (Exception e) { Debug.LogWarning("[TT] Screenshot failed: " + e.Message); }
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
                var cg = new GameObject("Main Camera") { tag = "MainCamera" };
                mainCamera = cg.AddComponent<Camera>();
                cg.AddComponent<AudioListener>();
            }
            var rts = mainCamera.GetComponent<TribalTrouble.Game.RtsCamera>();
            if (rts != null) rts.enabled = false;
            mainCamera.farClipPlane = 3000f;
            mainCamera.nearClipPlane = 1.2f;
            mainCamera.fieldOfView = 70f;
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
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.55f, 0.6f, 0.7f);
            RenderSettings.ambientEquatorColor = new Color(0.42f, 0.44f, 0.42f);
            RenderSettings.ambientGroundColor = new Color(0.25f, 0.22f, 0.18f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = mainCamera.backgroundColor;
            RenderSettings.fogStartDistance = 400f;
            RenderSettings.fogEndDistance = 1400f;
        }

        /// <summary>SpriteKey.getAnimationType (LOOP = 0, PLAIN = 1) comes from the imported sprite definitions.</summary>
        void SpriteKeyAnimations()
        {
            var catalog = _catalog;
            com.oddlabs.tt.render.SpriteKey.AnimationTypes = location =>
            {
                if (catalog == null || !FaithfulView.ParseLocation(location, out var g, out var n)) return null;
                var def = catalog.Find(g, n);
                if (def == null) return null;
                var types = new int[def.animations.Length];
                for (int i = 0; i < types.Length; i++) types[i] = def.animations[i].loop ? 0 : 1;
                return types;
            };
        }

        static bool DedicatedRequested()
        {
            foreach (var a in Environment.GetCommandLineArgs())
                if (a == "-server" || a == "-dedicated" || a == "--server") return true;
            return Application.isBatchMode && !Application.isEditor;
        }

        // ------------------------------------------------------------------ world loads (ProgressForm)

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
            if (_dedicated)
            {
                if (Time.unscaledTime - _dedicatedLog > 60f) { _dedicatedLog = Time.unscaledTime; Debug.Log("[TT] Dedicated server: " + PlayerServer.Online + " logged in"); }
                return;
            }
            JoinSim();   // before anything else looks at the game
            _frameMs = Mathf.Lerp(_frameMs, Time.unscaledDeltaTime * 1000f, 0.05f);
            ThrottleWhileLoading();
            if (Input.GetKeyDown(KeyCode.F9)) _showPerf = !_showPerf;
            if (Input.GetKeyDown(KeyCode.F10)) s_invertCulling = !s_invertCulling;
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F8)) DevSkirmish();
#endif
            if (_music != null) _music.volume = 0.35f * TribalTrouble.Game.GameSettings.music;
            _frameWatch ??= new System.Diagnostics.Stopwatch();
            ReadScreen();
            RunMainQueue();
            if (_gui == null) { if (_launcher) AutoBenchFromLauncher(); return; }
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
            BenchUpdate();
            PlaySounds();
            if (Host.Finished)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

#if UNITY_EDITOR
        /// <summary>Editor only (F8): an armory for the local player and one for the nearest enemy close by, each with
        /// units and weapons inside; deploys melee warriors from ours and throwers from theirs - to look at fights.</summary>
        void DevSkirmish()
        {
            var dr = Host.CurrentWorld;
            if (dr == null || dr.world == null || dr.localPlayer == null) return;
            var me = dr.localPlayer;
            com.oddlabs.tt.player.Player foe = null;
            foreach (var p in dr.world.getPlayers()) if (p != null && p != me && me.isEnemy(p)) { foe = p; break; }
            com.oddlabs.tt.model.Unit first = null;
            var it = me.getUnits().getSet().iterator();
            while (it.hasNext() && first == null) first = it.next() as com.oddlabs.tt.model.Unit;
            if (first == null || foe == null) return;
            var a1 = DevArmory(me, first.getGridX() + 10, first.getGridY());
            var a2 = DevArmory(foe, first.getGridX() + 26, first.getGridY());
            if (a1 == null || a2 == null) { Debug.Log("[TT] F8: no room for the armories"); return; }
            foreach (var a in new[] { a1, a2 })
            {
                a.getUnitContainer().increaseSupply(12);
                a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).increaseSupply(10);
                a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).increaseSupply(10);
            }
            bool ext = dr.world.isExtended();
            if (ext)
            {
                me.deployUnits(a1, com.oddlabs.tt.model.DeployType.MELEE_LIGHT, 4);
                me.deployUnits(a1, com.oddlabs.tt.model.DeployType.MELEE_HEAVY, 3);
            }
            else me.deployUnits(a1, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, 6);
            foe.deployUnits(a2, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, 4);
            foe.deployUnits(a2, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, 3);
            Debug.Log("[TT] F8: armories placed, units deploying (extended " + ext + ")");
        }

        static com.oddlabs.tt.model.LandBuilding DevArmory(com.oddlabs.tt.player.Player p, int nx, int ny)
        {
            var grid = p.getWorld().getUnitGrid();
            var t = p.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            for (int r = 0; r < 40; r++)
                for (int dx = -r; dx <= r; dx++)
                    foreach (int dy in new[] { -r, r })
                    {
                        int x = nx + dx, y = ny + dy;
                        if (!com.oddlabs.tt.model.LandBuilding.isPlacingLegal(grid, t, x, y)) continue;
                        var b = (com.oddlabs.tt.model.LandBuilding)t.create(p, x, y);
                        b.place();
                        b.repair(t.getMaxHitPoints() * 4);
                        return b;
                    }
            return null;
        }
#endif

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
            if (_engineSettings) return;   // the settings window has the mouse
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
            if (_engineSettings)
            {
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape) { _engineSettings = false; e.Use(); }
                return;
            }
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
            // Tribal Trouble 2: New Tides has its own main-menu logo (every language)
            if (TribalTrouble.Game.Edition.NewTides && path.StartsWith("/textures/gui/tt_mainform_logo", System.StringComparison.Ordinal))
                path = "/textures/gui/tt2_mainform_logo";
            if (_textureByPath.TryGetValue(path, out int h)) { width = _textures[h].width; height = _textures[h].height; return h; }
            string p = path.TrimStart('/');
            int dot = p.LastIndexOf('.');
            if (dot > p.LastIndexOf('/')) p = p.Substring(0, dot);
            var tex = Resources.Load<Texture2D>("TTJava/" + p);
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
        public void SetCursor(string image, int hotX, int hotY)
        {
            if (OffMain(() => SetCursor(image, hotX, hotY))) return;
            if (string.IsNullOrEmpty(image)) { UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); return; }
            if (!_cursors.TryGetValue(image, out var tex))
            {
                string p = image.TrimStart('/');
                int dot = p.LastIndexOf('.');
                if (dot > p.LastIndexOf('/')) p = p.Substring(0, dot);
                tex = Resources.Load<Texture2D>("TTJava/" + p);
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
                UnityEngine.Cursor.lockState = TribalTrouble.Game.GameSettings.confineCursor ? CursorLockMode.Confined : CursorLockMode.None;
                UnityEngine.Cursor.visible = !grabbed;
                return;
            }
            UnityEngine.Cursor.lockState = grabbed ? CursorLockMode.Locked : (TribalTrouble.Game.GameSettings.confineCursor && !Application.isEditor ? CursorLockMode.Confined : CursorLockMode.None);
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
            UnityEngine.Cursor.lockState = grabbed ? CursorLockMode.Locked : (TribalTrouble.Game.GameSettings.confineCursor && !Application.isEditor ? CursorLockMode.Confined : CursorLockMode.None);
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
        FaithfulView _view;
        FaithfulTerrain _terrain;
        TTWorldOverlay _overlay;
        com.oddlabs.tt.landscape.World _viewWorld;

        void LateUpdate()
        {
            if (_dedicated || _gui == null) return;
            RunMainQueue();
            var dr = Host.CurrentWorld;
            var world = dr != null && !dr.closed ? dr.world : null;
            if (world != _viewWorld) BuildView(dr);
            if (_view == null || Host.CurrentCamera == null) return;
            EditorUpdates(dr);
            ApplyJavaCamera(Host.CurrentCamera);
            var lw = _frameWatch ??= new System.Diagnostics.Stopwatch();
            lw.Restart();
            _view.Render(world, mainCamera);
            _lastRenderMs = (float)lw.Elapsed.TotalMilliseconds; lw.Restart();
            _terrain?.UpdateLod(mainCamera.transform.position);
            _lastTerrainMs = (float)lw.Elapsed.TotalMilliseconds; lw.Restart();
            _overlay?.Draw(dr, world, _view);
            _lastOverlayMs = (float)lw.Elapsed.TotalMilliseconds;
        }

        // ---- the Java map editor (MapEditorDelegate): its edits reshape and repaint the terrain here
        bool _depthDirty;
        float _depthTimer;

        void EditorUpdates(com.oddlabs.tt.render.DefaultRenderer dr)
        {
            if (_terrain == null) return;
            if (dr.cheat != null) _view.drawTrees = dr.cheat.draw_trees;   // the editor's "Trees" switch
            while (EditorView.TakeAlphas(out var a)) _terrain.SetAlphas(a.x0, a.y0, a.x1, a.y1, a.low, a.high);
            // in a game: ground a building flattened (round pads in free-form games); a few tiles a frame
            const int T = EditorView.GroundTile;
            for (int n = 0; n < 8 && EditorView.TakeGroundTile(out int tx, out int ty); n++)
                _terrain.RebuildCells(tx * T, ty * T, tx * T + T - 1, ty * T + T - 1);
            if (EditorView.TakeHeights(out int x0, out int y0, out int x1, out int y1))
            {
                _terrain.RebuildCells(x0, y0, x1, y1);
                _depthDirty = true;
                _depthTimer = 0f;
            }
            else if (_depthDirty && (_depthTimer += Time.unscaledDeltaTime) > 0.4f)
            {
                _depthDirty = false;
                _terrain.RefreshWaterDepth();
            }
        }

        void BuildView(com.oddlabs.tt.render.DefaultRenderer dr)
        {
            _view?.Clear();
            if (_worldRoot != null) Destroy(_worldRoot);
            _worldRoot = null; _view = null; _terrain = null; _overlay = null; _viewWorld = null;
            if (dr == null || dr.closed || dr.world == null || dr.worldInfo == null) return;
            _viewWorld = dr.world;
            // edits queued for another world (a closed map editor) don't belong to this one
            if (!(dr.guiRoot?.getDelegate() is com.oddlabs.tt.mapeditor.MapEditorDelegate)) EditorView.Reset();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            _worldRoot = new GameObject("Tribal Trouble world");
            _terrain = new FaithfulTerrain(dr.worldInfo);
            _terrain.withColliders = _terrain.size < 1024;
            _terrain.Build(dr.worldInfo, _catalog.terrainMaterial, _catalog.waterMaterial, _worldRoot.transform);
            var entities = new GameObject("Entities").transform;
            entities.SetParent(_worldRoot.transform, false);
            _view = new FaithfulView(_catalog, entities);
            TTEnvironment.Create(_worldRoot.transform, mainCamera, sun, _terrain, dr.generator.getTerrainType(), dr.generator.getFogInfo());
            TTGroundCover.Climate = dr.generator.getTerrainType().ordinal();   // the ground cover's look: tropical, northern, the Rift
            _overlay = new TTWorldOverlay(_catalog);
            if (dr.world.isWeather() || dr.world.isExtended()) TTWeatherFx.Create(_worldRoot.transform, mainCamera, _terrain, _sfx);   // New Tides: natural disasters (and the major gods' summoned weather)
            TTCombatFx.Sfx = _sfx;
            Debug.Log($"[TT] View built in {sw.ElapsedMilliseconds} ms ({_terrain.size} grid)");
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
        void BeginCamera(ScriptableRenderContext ctx, Camera cam) { if (cam == mainCamera && _cameraMirrored) GL.invertCulling = true; }
        void EndCamera(ScriptableRenderContext ctx, Camera cam) { if (cam == mainCamera) GL.invertCulling = false; }

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
        public bool SimThreaded => _simThread != null && TribalTrouble.Game.GameSettings.threadedSim;

        void StartSimThread(ScriptableRenderContext ctx, List<Camera> cams)
        {
            if (!TribalTrouble.Game.GameSettings.threadedSim || _dedicated || _gui == null || _quitting || _simBusy || _simRan) return;
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

        // ------------------------------------------------------------------ OnGUI

        void OnGUI()
        {
            if (_dedicated) return;
            if (_launcher) { LauncherGUI(); return; }
            var e = Event.current;
            if (e.type == EventType.KeyDown || e.type == EventType.KeyUp) { KeyEvents(e); return; }
            if (_engineSettings && e.type != EventType.Repaint) { EngineSettingsGUI(); return; }
            if (e.type != EventType.Repaint) return;
            DrawGuiQuads();
            if (TTEnvironment.ClockText != null && _view != null) DrawClock();
            if (_engineSettings) EngineSettingsGUI();
            if (_failure != null)
            {
                var r = new Rect(20, Screen.height - 60, Screen.width - 40, 40);
                GUI.color = new Color(1f, 0.4f, 0.3f);
                GUI.Label(r, _failure);
                GUI.color = Color.white;
            }
            if (_showPerf) PerfGUI();
            BenchGUI();
        }

        // ---- the launcher: which of the two games to play
        static GUIStyle s_title, s_sub, s_card, s_btn;

        void LauncherGUI()
        {
            if (Event.current.type == EventType.Repaint || _launcherArt == null)
            {
                if (_launcherArt == null) _launcherArt = Resources.Load<Texture2D>("TTJava/textures/gui/mainmenu");
            }
            float W = Screen.width, H = Screen.height;
            GUI.color = Color.white;
            if (_launcherArt != null) GUI.DrawTexture(new Rect(0, 0, W, H), _launcherArt, ScaleMode.ScaleAndCrop);
            GUI.color = new Color(0f, 0f, 0f, 0.55f);
            GUI.DrawTexture(new Rect(0, 0, W, H), Texture2D.whiteTexture);
            GUI.color = Color.white;
            float s = Mathf.Clamp(H / 1080f, 0.6f, 2f);
            if (s_title == null)
            {
                s_title = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, wordWrap = true };
                s_sub = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, wordWrap = true };
                s_btn = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };
            }
            s_title.fontSize = Mathf.RoundToInt(34 * s);
            s_sub.fontSize = Mathf.RoundToInt(17 * s);
            s_btn.fontSize = Mathf.RoundToInt(22 * s);
            GUI.Label(new Rect(0, H * 0.08f, W, 60 * s), "Choose your game", s_title);
            float gap = 30 * s, cw = Mathf.Min(500 * s, (W - 4 * gap) / 3f), ch = 460 * s, top = H * 0.2f;
            float x0 = W / 2 - 1.5f * cw - gap;
            Card(new Rect(x0, top, cw, ch), TribalTrouble.Game.Edition.ClassicTitle, new Color(0.62f, 0.48f, 0.36f),
                "The original game as it is on Steam today (v2.2.207): its rules, islands, campaigns and look, night games, archipelago maps and its seasonal events.\n\nPlay online with Steam players and against the computer.",
                "Play Tribal Trouble", 0);
            Card(new Rect(x0 + cw + gap, top, cw, ch), TribalTrouble.Game.Edition.NewTidesTitle, new Color(0.25f, 0.62f, 0.78f),
                "The game reborn: gold, crystal, herbs and coal, each race's own units, halls and spells, stone and iron buildings, dozens of upgrades, day and night and a modern interface.",
                "Play New Tides", 1);
            Card(new Rect(x0 + 2 * (cw + gap), top, cw, ch), TribalTrouble.Game.Edition.TikiTitle, new Color(0.95f, 0.62f, 0.2f),
                "A brand-new island strategy game in the same spirit: the Reef Clan against the Saltfang Raiders, a story campaign with Kai and his home village, skirmishes against the computer, and four art styles to pick from.",
                "Play Tiki Madness", 2);
            var rr = new Rect(W / 2 - 200 * s, top + ch + 30 * s, 400 * s, 30 * s);
            _launcherRemember = GUI.Toggle(rr, _launcherRemember, " Remember my choice (change it in Settings)");
        }

        static GUIStyle s_cardTitle;
        static Texture2D s_logoClassic, s_logoNewTides;

        static Texture2D CardLogo(ref Texture2D cache, string path)
        {
            if (cache == null) cache = Resources.Load<Texture2D>(path);
            return cache;
        }

        void Card(Rect r, string title, Color accent, string text, string button, int game)
        {
            GUI.color = new Color(0.05f, 0.07f, 0.1f, 0.88f);
            GUI.DrawTexture(r, Texture2D.whiteTexture);
            GUI.color = accent;
            GUI.DrawTexture(new Rect(r.x, r.y, r.width, 4), Texture2D.whiteTexture);
            GUI.color = Color.white;
            float p = 24f;
            if (s_cardTitle == null) s_cardTitle = new GUIStyle(s_title);
            var t = s_cardTitle;
            t.fontSize = Mathf.RoundToInt(s_title.fontSize * 0.85f);
            t.normal.textColor = Color.Lerp(accent, Color.white, 0.45f);
            float headH = t.fontSize * 2.6f;
            var logo = game == 0 ? CardLogo(ref s_logoClassic, "TTJava/textures/gui/tt_mainform_logo")
                : game == 1 ? CardLogo(ref s_logoNewTides, "TTJava/textures/gui/tt2_mainform_logo") : null;
            if (logo != null)
            {
                // the logo sits in the texture's bottom-left 1388 x 826 (the Java menu's u 0..347/512, v 0..206/256)
                headH = Mathf.Min((r.width - 2 * p) * 826f / 1388f, r.height * 0.36f);
                float lw = headH * 1388f / 826f;
                GUI.DrawTextureWithTexCoords(new Rect(r.x + (r.width - lw) / 2f, r.y + p * 0.6f, lw, headH), logo,
                    new Rect(0f, 0f, 1388f / 2048f, 826f / 1024f));
                headH += p * 0.2f;
            }
            else
                GUI.Label(new Rect(r.x + p, r.y + p, r.width - 2 * p, headH), title, t);
            GUI.Label(new Rect(r.x + p, r.y + p + headH + t.fontSize * 0.2f, r.width - 2 * p, r.height * 0.5f), text, s_sub);
            if (GUI.Button(new Rect(r.x + p, r.yMax - p - s_btn.fontSize * 2.4f, r.width - 2 * p, s_btn.fontSize * 2.4f), button, s_btn))
            {
                if (_launcherRemember) TribalTrouble.Game.Edition.StartWith = game;
                StartGame(game);
            }
        }

        // ---- the day / night cycle's clock (top centre)
        static GUIStyle s_clockStyle;

        void DrawClock()
        {
            if (s_clockStyle == null)
                s_clockStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 13, fontStyle = FontStyle.Bold };
            var r = new Rect(Screen.width / 2f - 48f, 4f, 96f, 22f);
            var c = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.45f);
            GUI.DrawTexture(r, Texture2D.whiteTexture);
            GUI.color = c;
            GUI.Label(r, TTEnvironment.ClockText, s_clockStyle);
        }

        // ---- the options menu's "Modern graphics..." page (GameSettings: the port's graphics, camera and gameplay extras)
        bool _engineSettings;
        Rect _engineRect;

        void EngineSettingsGUI()
        {
            float w = Mathf.Min(640f, Screen.width - 40f), h = Mathf.Min(620f, Screen.height - 40f);
            _engineRect = new Rect((Screen.width - w) / 2f, (Screen.height - h) / 2f, w, h);
            GUI.depth = -10;
            // a solid backdrop over the Java options form underneath
            var c = GUI.color;
            GUI.color = new Color(0.06f, 0.05f, 0.04f, 0.96f);
            GUI.DrawTexture(new Rect(_engineRect.x - 6, _engineRect.y - 6, _engineRect.width + 12, _engineRect.height + 12), Texture2D.whiteTexture);
            GUI.color = c;
            GUILayout.BeginArea(_engineRect);
            GUILayout.Label("Modern graphics and other settings of this version");
            TribalTrouble.Game.GameSettings.OnGUI();
            if (GUILayout.Button("Close", GUILayout.Height(28))) _engineSettings = false;
            GUILayout.EndArea();
        }

        System.Diagnostics.Stopwatch _frameWatch;

        // While a world loads on its worker, the loading screen is drawn at 30 frames a second: drawing it as fast as
        // the GPU allows (vsync is often off or "broken") made the main thread allocate non-stop, and every garbage
        // collection stops the loading thread too - island builds took 20-50 s instead of a few.
        bool _throttled;
        int _savedVsync, _savedFps;

        void ThrottleWhileLoading()
        {
            bool loading = TribalTrouble.PortHooks.Loading;
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

        void PerfGUI()
        {
            string s = $"{1000f / Mathf.Max(1f, _frameMs):0} fps ({_frameMs:0.0} ms)  game {_simMs:0.0} ms";
            if (_view != null) s += $"\nitems {_view.statItems}  trees {_view.statTrees} (models {_view.statRealTrees}, impostors {_view.statImpostors})  instances {_view.statInstances}  collect {_view.statCollectMs:0.0} ms  build {_view.statBuildMs:0.0} ms";
            GUI.Box(new Rect(10, 10, 640, 44), GUIContent.none);
            GUI.Label(new Rect(16, 12, 630, 44), s);
        }
    }
}
