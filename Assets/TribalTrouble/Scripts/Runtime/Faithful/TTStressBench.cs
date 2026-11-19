using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using TribalTrouble.GuiPort;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Shift+F9 in a game: the stress benchmark. Every tribe gets an army (half at home, half marching on your home,
    /// so a battle starts there) and towers and houses, then the camera holds four views for 10 s each - your home
    /// (the battle), the battle zoomed out, another tribe's home, and the whole island from high up - with vsync and
    /// the frame cap off. Frame times, where they go (game, scene, draw lists, overlay, render thread, GPU) and the
    /// draw counts are written to Benchmarks/bench_*.txt (the project folder in the editor, the game's data folder
    /// in a build) and the console. A second Shift+F9 while it runs stops it.
    /// </summary>
    public sealed partial class FaithfulGame
    {
        sealed class BenchPhase
        {
            public string name;
            public readonly List<float> frame = new List<float>();
            public double wait, sim, loop, guiMs, render, collect, build, overlay, terrain, mainCpu, renderCpu, gpu, farMs, placeMs, extMs, drawMs, partMs, fxMs; public long statics;
            public long batches, setPass, draws, tris, verts, casters, skinned, gcBytes;
            public int n, timed, units, live;
            public long near, gc0 = -1, gc1, far, culled, instBatches, instShadow, instances, trees, realTrees, impostors;
            public string cam, top;
            public double[] markers;
        }

        const float BenchWarmup = 2f, BenchHold = 10f;
        const int BenchArmy = 250, BenchBuildings = 12;
        int _benchStep = -1;
        int _autoPhases, _autoFrom;
        bool _autoMenuShots;
        int _menuShot;
        com.oddlabs.tt.gui.Form _menuPopup;
        string _autoFromName;
        float _benchClock;
        bool _benchTimed;
        readonly List<BenchPhase> _benchPhases = new List<BenchPhase>();
        int _benchSavedVsync, _benchSavedFps;
        float _benchHomeX, _benchHomeY, _benchFarX, _benchFarY;
        string _benchSetup;
        float _lastOverlayMs, _lastTerrainMs, _lastRenderMs;
        readonly FrameTiming[] _benchTiming = new FrameTiming[1];
        ProfilerRecorder _rBatches, _rSetPass, _rDraws, _rTris, _rVerts, _rCasters, _rSkinned, _rGc;
        // where the main thread's time goes (profiler markers; recorded in the editor and development builds)
        static readonly string[] BenchMarkers =
        {
            "PlayerLoop", "EditorLoop", "Update.ScriptRunBehaviourUpdate", "PreLateUpdate.ScriptRunBehaviourLateUpdate",
            "PostLateUpdate.FinishFrameRendering", "PostLateUpdate.UpdateAllSkinnedMeshes", "PostLateUpdate.UpdateAllRenderers",
            "PostLateUpdate.PlayerUpdateCanvases", "PostLateUpdate.PlayerSendFrameComplete", "GC.Collect", "Gfx.WaitForPresentOnGfxThread",
            "Gfx.WaitForRenderThread", "Gfx.WaitForGfxCommandsFromMainThread", "Inl_RenderCameraStack", "Culling", "CullAllVisibleLights",
            "Shadows.PrepareShadowMaps", "RenderLoop.ScheduleDraw", "ScriptableRenderer.Execute", "Camera.Render",
            "TransformChangeDispatch", "Physics.SyncColliderTransform", "GUI.Repaint", "GUIUtility.ProcessEvent", "Animation.Update",
            "Director.ProcessFrame", "ParticleSystem.Update", "Graphics.RenderMeshInstanced", "UpdateRendererBoundingVolumes",
            "MeshSkinning.Update", "Mesh.Bake Skinned", "FixedUpdate.PhysicsFixedUpdate", "PreUpdate.SendMouseEvents",
            "Application.UpdateScene", "EarlyUpdate.UpdatePreloading", "PostLateUpdate.UpdateAudio", "Initialization.PlayerUpdateTime",
        };
        readonly List<ProfilerRecorder> _rMarkers = new List<ProfilerRecorder>();
        string _benchMarkerInfo = "";

        // ---- the unattended run: Benchmarks/autorun.txt in the project folder (the editor's TTBenchAuto enters play mode
        // for it) starts a skirmish (players / size from the file, Extended on), runs the benchmark and leaves play mode
        int _autoStep = -1;
        float _autoClock;
        int _autoPlayers = 8, _autoSize = 4;
        float _autoRenderScale;   // > 0: the render scale for the run (restored afterwards)
        float _autoSavedScale = -1f;

        /// <summary>The project's Benchmarks folder in the editor; in a build, a Benchmarks folder next to the game if
        /// there is one (a build in the project's Builds folder), else the game's data folder.</summary>
        static string BenchDir
        {
            get
            {
                string beside = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.dataPath), "Benchmarks");
                if (Application.isEditor || System.IO.Directory.Exists(beside)) return beside;
                return System.IO.Path.Combine(Application.persistentDataPath, "Benchmarks");
            }
        }

        static object Field(object o, string name)
        {
            for (var t = o.GetType(); t != null; t = t.BaseType)
            {
                var f = t.GetField(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                if (f != null) return f.GetValue(o);
            }
            return null;
        }

        void AutoFail(string why)
        {
            Debug.LogWarning("[TT] Auto benchmark: " + why);
            try { System.IO.Directory.CreateDirectory(BenchDir); System.IO.File.WriteAllText(System.IO.Path.Combine(BenchDir, "autorun_error.txt"), why); } catch (System.Exception) { }
            _autoStep = -1;
            AutoQuit();
        }

        static void AutoQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>The request the editor handed over on entering play mode (autorun_go.txt), or one left for a build.</summary>
        static string AutoFile()
        {
            foreach (var n in new[] { "autorun_go.txt", "autorun.txt" })
            {
                string f = System.IO.Path.Combine(BenchDir, n);
                // (a request left over from a run that went wrong is stale after ten minutes: no surprise runs later)
                if (System.IO.File.Exists(f) && (System.DateTime.Now - System.IO.File.GetLastWriteTime(f)).TotalMinutes < 10) return f;
            }
            return null;
        }

        /// <summary>On the game picker: an unattended run starts Tribal Trouble 2 (New Tides).</summary>
        void AutoBenchFromLauncher()
        {
            if ((Time.frameCount % 30) != 0 || AutoFile() == null) return;
            Debug.Log("[TT] Auto benchmark: starting New Tides from the game picker");
            BootGui(true);
        }

        // an unattended run's warnings and errors go to Benchmarks/log.txt next to its reports (once each)
        bool _autoLogging;
        readonly HashSet<string> _autoLogSeen = new HashSet<string>();
        void AutoLog(string msg, string stack, LogType type)
        {
            if (type == LogType.Log && !msg.StartsWith("[TT]")) return;
            if (_autoLogSeen.Count > 400 || !_autoLogSeen.Add(msg)) return;
            try
            {
                System.IO.Directory.CreateDirectory(BenchDir);
                System.IO.File.AppendAllText(System.IO.Path.Combine(BenchDir, "log.txt"), System.DateTime.Now.ToString("HH:mm:ss") + " " + type + ": " + msg + (type == LogType.Exception || type == LogType.Error ? "\n" + stack : "") + "\n");
            }
            catch (System.Exception) { }
        }

        string _autoMapcode = "MAP DISTRIBUTION BRAIN AIR";
        bool _autoShots, _benchShot, _autoShowcase;

        void AutoBenchUpdate()
        {
            if (_autoStep < 0)
            {
                if ((Time.frameCount % 30) != 0) return;
                string file = AutoFile();
                if (file == null) return;
                try
                {
                    foreach (var w in System.IO.File.ReadAllText(file).Split(new[] { ' ', '\n', '\r', '\t' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        var kv = w.Split('=');
                        if (kv.Length != 2) continue;
                        if (kv[0] == "players") int.TryParse(kv[1], out _autoPlayers);
                        if (kv[0] == "size") int.TryParse(kv[1], out _autoSize);
                        if (kv[0] == "sweep") _autoSweep = kv[1] == "1";
                        if (kv[0] == "shots") _autoShots = kv[1] == "1";
                        if (kv[0] == "showcase") _autoShowcase = kv[1] == "1";
                        if (kv[0] == "phases") int.TryParse(kv[1], out _autoPhases);   // only N views (0: all)
                        if (kv[0] == "menushots") _autoMenuShots = kv[1] == "1";   // pictures of the skirmish form's tabs and its Advanced pop-up, then quit
                        if (kv[0] == "from" && !int.TryParse(kv[1], out _autoFrom)) _autoFromName = kv[1].Replace('_', ' ');   // starting at view N, or the first view whose name has that text ("_" for spaces)
                        if (kv[0] == "mapcode") _autoMapcode = kv[1] == "random" ? null : kv[1].Replace('_', ' ');
                        if (kv[0] == "threaded") TribalTrouble.Game.GameSettings.threadedSim = kv[1] != "0";
                        if (kv[0] == "renderscale") float.TryParse(kv[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _autoRenderScale);
                    }
                    System.IO.File.Delete(file);
                }
                catch (System.Exception e) { Debug.LogWarning("[TT] Auto benchmark: " + e.Message); return; }
                _autoStep = 0; _autoClock = 0f;
                if (!_autoLogging) { _autoLogging = true; Application.logMessageReceived += AutoLog; }
                Debug.Log($"[TT] Auto benchmark: {_autoPlayers} players, size {_autoSize}");
                return;
            }
            _autoClock += Time.unscaledDeltaTime;
            var root = _gui?.getGUIRoot();
            switch (_autoStep)
            {
                case 0:   // the main menu, then the skirmish form
                    if (_autoClock > 180f) { AutoFail("no main menu"); return; }
                    if (_autoClock < 3f || TribalTrouble.PortHooks.Loading || !(root?.getDelegate() is com.oddlabs.tt.@delegate.MainMenu mm)) return;
                    mm.setMenu(new com.oddlabs.tt.form.TerrainMenuForm(_network, root, mm));
                    _autoStep = 1; _autoClock = 0f;
                    return;
                case 1:   // the game's settings
                    if (_autoClock < 1f) return;
                    {
                        object tm = null;
                        var it = root.getDelegate() is com.oddlabs.tt.@delegate.Menu menu ? Field(menu, "current_menu") : null;
                        if (it != null) tm = Field(it, "terrain");
                        if (tm == null) { AutoFail("no skirmish form"); return; }
                        ((com.oddlabs.tt.gui.PulldownMenu)Field(tm, "pulldown_menu_slots"))?.chooseItem(Mathf.Max(0, _autoPlayers - 6));
                        ((com.oddlabs.tt.gui.PulldownMenu)Field(tm, "pulldown_size"))?.chooseItem(_autoSize);
                        ((com.oddlabs.tt.gui.CheckBox)Field(tm, "cb_extended"))?.setMarked(true);
                        // the same island every run (a densely wooded Unreal one), unless "mapcode=random"
                        if (_autoMapcode != null && tm is com.oddlabs.tt.form.TerrainMenu tmm)
                        {
                            try { tmm.parseMapcode(_autoMapcode); } catch (System.Exception e) { Debug.LogWarning("[TT] Auto benchmark: map code " + _autoMapcode + ": " + e.Message); }
                        }
                        _autoTerrain = tm;
                    }
                    if (_autoMenuShots) { _autoStep = 20; _menuShot = 0; _autoClock = 0f; return; }
                    _autoStep = 2; _autoClock = 0f;
                    return;
                case 20:   // "menushots=1": each tab of the skirmish form, then the Advanced pop-up's, one picture each
                    if (_autoClock < 1.5f) return;
                    {
                        var tmo = (com.oddlabs.tt.gui.Renderable)_autoTerrain;
                        var group = FindChild<com.oddlabs.tt.gui.PanelGroup>(_menuPopup ?? tmo);
                        int n = group != null ? CountPanels(group) : 0;
                        if (_menuShot > 0 && _menuShot <= n)
                            StartCoroutine(SaveScreenshot(System.IO.Path.Combine(BenchDir, "menu_" + (_menuPopup != null ? "advanced_" : "skirmish_") + (_menuShot - 1) + ".png")));
                        if (group != null && _menuShot < n) { group.selectPanel(_menuShot); _menuShot++; _autoClock = 0f; return; }
                        if (_menuShot == n) { _menuShot++; _autoClock = 0f; return; }   // (the last tab's picture)
                        if (_menuPopup == null)
                        {
                            com.oddlabs.tt.gui.HorizButton adv = null;
                            for (var c = tmo.getFirstChild(); c != null; c = (com.oddlabs.tt.gui.Renderable)c.getNext())
                                if (c is com.oddlabs.tt.gui.HorizButton hb && hb.getWidth() == 130) adv = hb;
                            if (adv == null) { Debug.Log("[TT] Menu shots: no Advanced button"); _autoStep = -1; AutoQuit(); return; }
                            adv.mouseClickedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 2, 2, 1);
                            _menuPopup = FindChild<com.oddlabs.tt.form.TerrainMenu.ExtraSettingsForm>(root);
                            Debug.Log("[TT] Menu shots: Advanced pop-up " + (_menuPopup?.GetType().Name ?? "not found"));
                            if (_menuPopup == null) { _autoStep = -1; AutoQuit(); return; }
                            _menuShot = 0; _autoClock = 0f;
                            return;
                        }
                        Debug.Log("[TT] Menu shots: done");
                        _menuPopup.cancel(); _menuPopup = null;
                        _autoStep = -1;
                        AutoQuit();
                    }
                    return;
                case 2:   // start it
                    if (_autoClock < 1f) return;
                    if (!((com.oddlabs.tt.form.TerrainMenu)_autoTerrain).startGame()) { AutoFail("the skirmish didn't start"); return; }
                    _autoStep = 3; _autoClock = 0f;
                    return;
                case 3:   // the island is up
                    if (_autoClock > 900f) { AutoFail("the island didn't load in 15 minutes"); return; }
                    if (TribalTrouble.PortHooks.Loading || !(root?.getDelegate() is com.oddlabs.tt.@delegate.SelectionDelegate)) return;
                    _autoStep = 4; _autoClock = 0f;
                    Debug.Log("[TT] Auto benchmark: in game");
                    return;
                case 4:   // settle, then the benchmark
                    if (_autoClock < 8f) return;
                    if (_autoRenderScale > 0f && UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline is UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp)
                    {
                        _autoSavedScale = urp.renderScale;
                        urp.renderScale = Mathf.Clamp(_autoRenderScale, 0.5f, 2f);
                        TribalTrouble.Game.GameSettings.renderScale = urp.renderScale;
                    }
                    BenchStart();
                    if (_benchStep < 0) { AutoFail("the benchmark didn't start"); return; }
                    _autoStep = 5;
                    return;
                case 5:
                    if (_benchStep >= 0) return;
                    if (_autoSavedScale > 0f && UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline is UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp2)
                    {
                        urp2.renderScale = _autoSavedScale;
                        TribalTrouble.Game.GameSettings.renderScale = _autoSavedScale;
                        _autoSavedScale = -1f;
                    }
                    SweepRestore(); _autoSweep = false;
                    _autoStep = -1;
                    AutoQuit();
                    return;
            }
        }

        object _autoTerrain;

        void BenchUpdate()
        {
            AutoBenchUpdate();
            if (Input.GetKeyDown(KeyCode.F9) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                if (_benchStep >= 0) BenchFinish(true);
                else BenchStart();
            }
            if (_benchStep < 0) return;
            var dr = Host.CurrentWorld;
            if (dr == null || dr.closed || dr.world == null) { BenchFinish(true); return; }
            FrameTimingManager.CaptureFrameTimings();
            float dt = Time.unscaledDeltaTime;
            _benchClock += dt;
            var ph = _benchPhases[_benchStep];
            if (_benchTimed)
            {
                ph.frame.Add(dt * 1000f);
                ph.sim += _lastSimRaw; ph.wait += _lastSimWait; ph.loop += com.oddlabs.tt.render.Renderer.LastLoopMs; ph.guiMs += com.oddlabs.tt.render.Renderer.LastGuiMs; ph.render += _lastRenderMs; ph.overlay += _lastOverlayMs; ph.terrain += _lastTerrainMs;
                if (_view != null) { ph.collect += _view.statCollectMs; ph.build += _view.statBuildMs; ph.live = _view.statLive; ph.far += _view.statFar; ph.near += _view.statNear; if (ph.gc0 < 0) ph.gc0 = System.GC.CollectionCount(0); ph.gc1 = System.GC.CollectionCount(0); ph.culled += _view.statCulled; ph.farMs += _view.statFarMs; ph.drawMs += _view.statDrawMs; ph.partMs += _view.statParticlesMs; ph.fxMs += _view.statFxMs; ph.statics += _view.statStatic; ph.instBatches += _view.statInstBatches; ph.instShadow += _view.statInstShadowBatches; ph.instances += _view.statInstances; ph.placeMs += _view.statPlaceMs; ph.extMs += _view.statExtMs; ph.trees += _view.statTrees; ph.realTrees += _view.statRealTrees; ph.impostors += _view.statImpostors; ph.top = _view.statTopBatches; if (ph.cam == null && Camera.main != null) { var cp = Camera.main.transform.position; ph.cam = $"camera at ({cp.x:0}, {cp.y:0}, {cp.z:0})"; } }
                if (FrameTimingManager.GetLatestTimings(1, _benchTiming) > 0)
                {
                    ph.mainCpu += _benchTiming[0].cpuMainThreadFrameTime; ph.renderCpu += _benchTiming[0].cpuRenderThreadFrameTime; ph.gpu += _benchTiming[0].gpuFrameTime;
                    ph.timed++;
                }
                ph.batches += Last(_rBatches); ph.setPass += Last(_rSetPass); ph.draws += Last(_rDraws); ph.tris += Last(_rTris);
                ph.verts += Last(_rVerts); ph.casters += Last(_rCasters); ph.skinned += Last(_rSkinned); ph.gcBytes += Last(_rGc);
                ph.markers ??= new double[BenchMarkers.Length];
                for (int i = 0; i < _rMarkers.Count && i < ph.markers.Length; i++) ph.markers[i] += Last(_rMarkers[i]) * 1e-6;
                ph.n++;
            }
            if (!_benchTimed && _benchClock >= BenchWarmup) { _benchTimed = true; _benchClock = 0f; _benchShot = false; }
            else if (_benchTimed && _autoShots && !_benchShot && _benchClock >= _benchHold * 0.5f)
            {
                // "shots=1": a picture of each view half way through (to check what is drawn, unattended)
                _benchShot = true;
                if (_showcase != null && _benchStep < _showcaseAt.Count && Camera.main != null)
                {
                    var sp = Camera.main.WorldToScreenPoint(_showcaseAt[_benchStep]);
                    Debug.Log($"[TT] Showcase shot {_benchStep}: the building at {sp.x:0} {Screen.height - sp.y:0} of {Screen.width}x{Screen.height}");
                    // the line-up's units on this picture (to check each one is drawn)
                    if (_benchStep < _showcaseRow.Count)
                    {
                        var ls = new StringBuilder();
                        foreach (var (u, row, k) in _lineup)
                        {
                            if (u == null || row != _showcaseRow[_benchStep] || u.isDead()) continue;
                            var up = Camera.main.WorldToScreenPoint(TTSpace.ToUnity(u.getPositionX(), u.getPositionY(), u.getPositionZ() + 1f));
                            string kn = k < UnitKindNames.Length ? UnitKindNames[k] : "kind " + k;
                            string how = FaithfulView.Probe != null && FaithfulView.Probe.TryGetValue(u, out var h) ? h : "?";
                            ls.Append($"{kn}@{up.x:0},{Screen.height - up.y:0} [{how}]; ");
                        }
                        if (_showcaseRow[_benchStep] == -2)
                            foreach (var (sm, i) in _props)
                            {
                                var pp = Camera.main.WorldToScreenPoint(TTSpace.ToUnity(sm.getPositionX(), sm.getPositionY(), sm.getPositionZ() + 0.5f));
                                string how = FaithfulView.Probe != null && FaithfulView.Probe.TryGetValue(sm, out var h2) ? h2 : "?";
                                ls.Append($"treasure {i}@{pp.x:0},{Screen.height - pp.y:0} z={sm.getPositionZ():0.0} [{how}]; ");
                            }
                        Debug.Log($"[TT] Showcase shot {_benchStep} units: {ls}");
                        if (_benchStep < _benchPhases.Count && _benchPhases[_benchStep].name.Contains("Blowgunners")) Debug.Log($"[TT] Showcase shot {_benchStep}: blow poses so far {TTExtendedArt.BlowPosed}");
                    }
                }
                try { System.IO.Directory.CreateDirectory(BenchDir); StartCoroutine(SaveScreenshot(System.IO.Path.Combine(BenchDir, "shot_" + _benchStep + ".png"))); }
                catch (System.Exception e) { Debug.LogWarning("[TT] Benchmark picture: " + e.Message); }
            }
            else if (_benchTimed && _benchClock >= _benchHold)
            {
                int units = 0;
                foreach (var p in dr.world.getPlayers()) if (p != null) units += p.getUnitCountContainer().getNumSupplies();
                ph.units = units;
                if (_benchStep + 1 >= _benchPhases.Count || (_autoPhases > 0 && _benchStep + 1 >= _autoFrom + _autoPhases)) { BenchFinish(false); return; }
                _benchStep++;
                BenchView(dr, _benchStep);
                _benchClock = 0f; _benchTimed = false;
            }
        }

        static long Last(ProfilerRecorder r) => r.Valid ? r.LastValue : 0;

        static string EditorCodeMode()
        {
#if UNITY_EDITOR
            return ", editor code " + UnityEditor.Compilation.CompilationPipeline.codeOptimization;
#else
            return "";
#endif
        }

        void BenchStart()
        {
            var dr = Host.CurrentWorld;
            if (dr == null || dr.closed || dr.world == null || dr.localPlayer == null) { Debug.Log("[TT] Benchmark: start a game first"); return; }
            var w = dr.world;
            var me = dr.localPlayer;
            var g = w.getUnitGrid(); int gs = g.getGridSize();
            com.oddlabs.tt.model.Unit Home(com.oddlabs.tt.player.Player p)
            {
                var it = p.getUnits().getSet().iterator();
                while (it.hasNext()) if (it.next() is com.oddlabs.tt.model.Unit u && !u.isDead() && !u.isMounted()) return u;
                return null;
            }
            var battle = Home(me);
            if (battle == null) { Debug.Log("[TT] Benchmark: you have no units on the island"); return; }
            int[] kinds = { com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, com.oddlabs.tt.model.Race.UNIT_PEON };
            if (w.isExtended())
                kinds = new[] { com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, com.oddlabs.tt.model.Race.UNIT_MELEE_LIGHT, com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY,
                    com.oddlabs.tt.model.Race.UNIT_SLINGER, com.oddlabs.tt.model.Race.UNIT_SCOUT, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER, com.oddlabs.tt.model.Race.UNIT_UNIQUE_2 };
            int spawned = 0, built = 0, pi = 0, np = w.getPlayers().Length;
            _benchFarX = _benchFarY = -1f;
            _showcase = null; _benchHold = BenchHold;
            bool showcase = _autoShowcase && ShowcaseStart();
            foreach (var p in showcase ? new com.oddlabs.tt.player.Player[0] : w.getPlayers())
            {
                if (p == null) continue;
                var home = Home(p);
                if (home == null) continue;
                if (p != me && _benchFarX < 0f) { _benchFarX = home.getPositionX(); _benchFarY = home.getPositionY(); }
                var raid = new List<com.oddlabs.tt.model.Selectable>();
                float ang = pi++ * 6.2832f / np;
                for (int i = 0; i < BenchArmy; i++)
                {
                    bool atBattle = i % 2 == 1 && p != me;
                    float a = i * 2.39996f, r = atBattle ? 3f + Mathf.Sqrt(i) * 1.2f : 6f + Mathf.Sqrt(i) * 1.6f;
                    float bx = atBattle ? battle.getPositionX() + 55f * Mathf.Cos(ang) : home.getPositionX();
                    float by = atBattle ? battle.getPositionY() + 55f * Mathf.Sin(ang) : home.getPositionY();
                    float x = bx + r * Mathf.Cos(a), y = by + r * Mathf.Sin(a);
                    int cx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), cy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y);
                    if (cx < 2 || cy < 2 || cx >= gs - 2 || cy >= gs - 2 || g.getRegion(cx, cy) == null || g.getOccupant(cx, cy) != null) continue;
                    var u = new com.oddlabs.tt.model.Unit(p, x, y, null, p.getRace().getUnitTemplate(kinds[i % kinds.Length]));
                    if (atBattle) raid.Add(u);
                    spawned++;
                }
                for (int i = 0; i < BenchBuildings; i++)
                {
                    int id = i % 3 == 0 ? com.oddlabs.tt.model.Race.BUILDING_QUARTERS : com.oddlabs.tt.model.Race.BUILDING_TOWER;
                    if (BenchBuild(p, id, home.getGridX() + (i % 6 - 3) * 9, home.getGridY() + (i / 6 - 2) * 9)) built++;
                }
                if (raid.Count > 0) p.setLandscapeTarget(raid.ToArray(), battle.getGridX(), battle.getGridY(), com.oddlabs.tt.model.Action.ATTACK, false);
            }
            _benchHomeX = battle.getPositionX(); _benchHomeY = battle.getPositionY();
            if (_benchFarX < 0f) { _benchFarX = w.getHeightMap().getMetersPerWorld() * .5f; _benchFarY = _benchFarX; }
            var gfx = TribalTrouble.Game.GameSettings.graphicsStyle == 1 ? "Modern" : "Classic";
            _benchSetup = $"island grid {gs} ({w.getHeightMap().getMetersPerWorld()} m), {np} tribes, extended {w.isExtended()}; spawned {spawned} warriors and {built} buildings\n"
                + $"graphics {gfx}, quality '{QualitySettings.names[QualitySettings.GetQualityLevel()]}', shadows {TribalTrouble.Game.GameSettings.shadows} ({TribalTrouble.Game.GameSettings.shadowDistance:0} m), post {TribalTrouble.Game.GameSettings.postEffects}, AO {TribalTrouble.Game.GameSettings.ambientOcclusion}, "
                + $"real trees {TribalTrouble.Game.GameSettings.realTrees}, real weapons {TribalTrouble.Game.GameSettings.realWeapons}, combat fx {TribalTrouble.Game.GameSettings.combatEffects}, render scale {TribalTrouble.Game.GameSettings.renderScale}, simulation thread {TribalTrouble.Game.GameSettings.threadedSim}, "
                + $"screen {Screen.width}x{Screen.height}, vsync setting {TribalTrouble.Game.GameSettings.vsync}\n"
                + $"{SystemInfo.processorType} ({SystemInfo.processorCount} threads), {SystemInfo.graphicsDeviceName} ({SystemInfo.graphicsDeviceType}, {SystemInfo.graphicsMemorySize} MB), editor {Application.isEditor}{EditorCodeMode()}";
            if (!showcase) _benchPhases.Clear();
            if (!showcase) foreach (var n in new[] { "your home, the battle", "the battle zoomed out", "another tribe's home", "the island from high up" }) _benchPhases.Add(new BenchPhase { name = n });
            if (_autoSweep && !showcase)   // after the usual views, on the same island: the settings one by one
                foreach (var n in SweepNames) _benchPhases.Add(new BenchPhase { name = "battle zoomed out, " + n });
            _benchSavedVsync = QualitySettings.vSyncCount; _benchSavedFps = Application.targetFrameRate;
            QualitySettings.vSyncCount = 0; Application.targetFrameRate = -1;
            _rBatches = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
            _rSetPass = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            _rDraws = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            _rTris = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
            _rVerts = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
            _rCasters = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Shadow Casters Count");
            _rSkinned = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Visible Skinned Meshes Count");
            _rGc = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
            _rMarkers.Clear();
            var handles = new List<ProfilerRecorderHandle>();
            ProfilerRecorderHandle.GetAvailable(handles);
            var byName = new Dictionary<string, ProfilerRecorderHandle>();
            foreach (var h in handles) { var d = ProfilerRecorderHandle.GetDescription(h); if (d.Name != null && !byName.ContainsKey(d.Name)) byName[d.Name] = h; }
            int found = 0;
            foreach (var mk in BenchMarkers)
            {
                bool ok = byName.TryGetValue(mk, out var mh);
                if (ok) found++;
                if (ok) { var rec = new ProfilerRecorder(mh, 1, ProfilerRecorderOptions.Default); rec.Start(); _rMarkers.Add(rec); } else _rMarkers.Add(default);
            }
            _benchMarkerInfo = $"profiler stats available {handles.Count}, markers found {found}/{BenchMarkers.Length}";
            if (_autoFromName != null)
                for (int i = 0; i < _benchPhases.Count; i++)
                    if (_benchPhases[i].name.Contains(_autoFromName)) { _autoFrom = i; break; }
            _benchStep = Mathf.Clamp(_autoFrom, 0, Mathf.Max(0, _benchPhases.Count - 1)); _benchClock = 0f; _benchTimed = false; _benchCam = null; _benchNote = null;
            BenchView(dr, _benchStep);
            Debug.Log("[TT] Benchmark started: " + _benchSetup.Replace('\n', ' '));
        }

        static bool BenchBuild(com.oddlabs.tt.player.Player p, int id, int nx, int ny) => BenchBuildAt(p, id, nx, ny, 12) != null;

        static com.oddlabs.tt.model.LandBuilding BenchBuildAt(com.oddlabs.tt.player.Player p, int id, int nx, int ny, int maxR)
        {
            var grid = p.getWorld().getUnitGrid();
            var t = p.getRace().getBuildingTemplate(id);
            for (int r = 0; r < maxR; r++)
                for (int dx = -r; dx <= r; dx++)
                    foreach (int dy in new[] { -r, r })
                    {
                        int x = nx + dx, y = ny + dy;
                        if (!com.oddlabs.tt.model.LandBuilding.isPlacingLegal(grid, t, x, y)) continue;
                        var b = (com.oddlabs.tt.model.LandBuilding)t.create(p, x, y);
                        b.place();
                        b.repair(b.getEffectiveMaxHitPoints() * 4);
                        return b;
                    }
            return null;
        }

        // ---- the building showcase (autorun "showcase=1"): one of each New Tides building for both races beside your
        // home, each looked at up close for a few seconds (with "shots=1", a picture of each)
        List<(float x, float y, float zoom)> _showcase;
        readonly List<Vector3> _showcaseAt = new List<Vector3>();
        readonly List<int> _showcaseRow = new List<int>();
        readonly List<(com.oddlabs.tt.model.SceneryModel, int)> _props = new List<(com.oddlabs.tt.model.SceneryModel, int)>();

        sealed class TreeCollector : java.lang.Object, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            public float x0, x1, y0, y1;
            public readonly List<com.oddlabs.tt.landscape.TreeSupply> trees = new List<com.oddlabs.tt.landscape.TreeSupply>();
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf leaf) => leaf.visitTrees(this);
            public void visitNode(com.oddlabs.tt.landscape.TreeGroup group) => group.visitChildren(this);
            public void visitTree(com.oddlabs.tt.landscape.TreeSupply t)
            {
                float x = t.getPositionX(), y = t.getPositionY();
                if (x >= x0 && x <= x1 && y >= y0 && y <= y1) trees.Add(t);
            }
        }
        float _benchHold = BenchHold;

        /// <summary>Where each kind of building blocks the ground against how big its model is drawn: the cells it
        /// occupies (their far edges, from its centre) and its fitted model radius - cells well past the model would
        /// be walls nobody can see.</summary>
        static readonly List<(com.oddlabs.tt.model.Unit u, int row, int kind)> _lineup = new List<(com.oddlabs.tt.model.Unit, int, int)>();
        static readonly string[] UnitKindNames = { "rock warrior", "iron warrior", "chicken warrior", "peon", "chieftain", "champion", "scout",
            "shaman/skald", "slinger", "melee light", "melee heavy", "unique 1", "unique 2", "crystal", "diamond", "unique 3", "minor god",
            "major god", "zombie", "zombie brute", "elite 1", "elite 2", "elite 3", "chicken spinner", "zombie boss" };

        /// <summary>The line-up's units and how each was drawn (the showcase): one with nothing drawing is a bug.</summary>
        static string UnitReport()
        {
            if (_lineup.Count == 0 || FaithfulView.Probe == null) return "";
            var sb = new StringBuilder("units (the line-up, every kind, both tribes):\n");
            foreach (var (u, row, k) in _lineup)
            {
                string name = k < UnitKindNames.Length ? UnitKindNames[k] : "kind " + k;
                string how = u == null ? "no room to stand" : FaithfulView.Probe.TryGetValue(u, out var h) ? h : "?";
                bool bad = u != null && !how.StartsWith("far") && !(how.StartsWith("own object") && !how.Contains(": 0 of"));
                sb.AppendLine($"   row {row} {name}: {how}{(bad ? "   <-- NOT VISIBLE" : "")}");
            }
            return sb.ToString();
        }

        static string FootprintReport()
        {
            var dr = Host.CurrentWorld;
            if (dr == null || dr.world == null) return "";
            var w = dr.world; var g = w.getUnitGrid(); int gs = g.getGridSize();
            var reach = new Dictionary<com.oddlabs.tt.model.BuildingTemplate, (float occ, float fit, int n, string name)>();
            var cells = new Dictionary<object, List<(int, int)>>(RefEq.Instance);
            for (int y = 0; y < gs; y++)
                for (int x = 0; x < gs; x++)
                    if (g.getOccupant(x, y) is com.oddlabs.tt.model.LandBuilding lb)
                    {
                        if (!cells.TryGetValue(lb, out var l)) cells[lb] = l = new List<(int, int)>();
                        l.Add((x, y));
                    }
            foreach (var kv in cells)
            {
                var b = (com.oddlabs.tt.model.LandBuilding)kv.Key;
                var t = (com.oddlabs.tt.model.BuildingTemplate)b.getTemplate();
                float cx = b.getPositionX(), cy = b.getPositionY(), max = 0f;
                foreach (var (x, y) in kv.Value)
                {
                    // the cell's far corner from the centre
                    float px = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(x), py = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(y);
                    float dx = Mathf.Abs(px - cx) + 1f, dy = Mathf.Abs(py - cy) + 1f;
                    max = Mathf.Max(max, Mathf.Sqrt(dx * dx + dy * dy));
                }
                reach.TryGetValue(t, out var e);
                e.occ = Mathf.Max(e.occ, max); e.n++;
                e.fit = t.isSelectionFitted(2) ? t.getBuiltSelectionRadius() : -1f;
                e.name = t.getName() + (com.oddlabs.tt.model.ExtendedCombat.isVikings(b.getOwner()) ? " (V)" : " (N)");
                reach[t] = e;
            }
            var sb = new StringBuilder("footprints (blocked ground reaches, from the centre / the model's fitted radius, m):");
            foreach (var kv in reach) sb.Append($" {kv.Value.name} x{kv.Value.n}: {kv.Value.occ:0.0} / {(kv.Value.fit > 0 ? kv.Value.fit.ToString("0.0") : "?")};");
            sb.AppendLine();
            return sb.ToString();
        }

        bool ShowcaseStart()
        {
            var dr = Host.CurrentWorld;
            if (dr == null || dr.closed || dr.world == null || dr.localPlayer == null) return false;
            var w = dr.world; var me = dr.localPlayer;
            com.oddlabs.tt.player.Player other = null;
            bool myVik = com.oddlabs.tt.model.ExtendedCombat.isVikings(me);
            foreach (var p in w.getPlayers()) if (p != null && p != me && com.oddlabs.tt.model.ExtendedCombat.isVikings(p) != myVik) { other = p; break; }
            var it = me.getUnits().getSet().iterator();
            com.oddlabs.tt.model.Unit home = null;
            while (it.hasNext() && home == null) home = it.next() as com.oddlabs.tt.model.Unit;
            if (home == null) return false;
            int hx = home.getGridX(), hy = home.getGridY();
            // a clearing for them: the trees there taken out (so they can be seen, and stand where they are meant to)
            var fell = new TreeCollector { x0 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(hx - 44), x1 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(hx + 48),
                y0 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(hy - 30), y1 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(hy + 30) };
            w.getTreeRoot().visit(fell);
            foreach (var t in fell.trees) w.getTreeRoot().removeTree(t);
            w.getTreeRoot().refreshBounds();
            _showcase = new List<(float, float, float)>();
            _showcaseAt.Clear(); _showcaseRow.Clear(); _props.Clear();
            _benchPhases.Clear();
            int[] ids = { com.oddlabs.tt.model.Race.BUILDING_HALL, com.oddlabs.tt.model.Race.BUILDING_FORGE, com.oddlabs.tt.model.Race.BUILDING_TOTEM,
                com.oddlabs.tt.model.Race.BUILDING_MARKET, com.oddlabs.tt.model.Race.BUILDING_SHRINE, com.oddlabs.tt.model.Race.BUILDING_DOCK,
                com.oddlabs.tt.model.Race.BUILDING_TOWER, com.oddlabs.tt.model.Race.BUILDING_PALISADE, com.oddlabs.tt.model.Race.BUILDING_QUARTERS, com.oddlabs.tt.model.Race.BUILDING_ARMORY };
            string[] names = { "hall", "forge", "totem", "market", "shrine", "dock", "tower", "palisade", "quarters", "armory" };
            var g = w.getUnitGrid(); int gs = g.getGridSize();
            // a line-up of every kind of unit for both tribes, standing still, each one's drawing checked (UnitReport)
            _lineup.Clear();
            FaithfulView.Probe = new Dictionary<object, string>();
            if (w.isExtended())
            {
                // first, a portrait of the four gods standing apart (before anyone moves them)
                int gi = 0;
                foreach (var p in new[] { other, me })
                {
                    if (p == null) continue;
                    foreach (int k in new[] { com.oddlabs.tt.model.Race.UNIT_MINOR_GOD, com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD })
                    {
                        float gx = home.getPositionX() + 12f + gi++ * 9f, gy = home.getPositionY() - 14f;
                        int cx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gx), cy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gy);
                        if (cx < 2 || cy < 2 || cx >= gs - 2 || cy >= gs - 2 || g.getRegion(cx, cy) == null || g.getOccupant(cx, cy) != null) continue;
                        _lineup.Add((new com.oddlabs.tt.model.Unit(p, gx, gy, null, p.getRace().getUnitTemplate(k)), -1, k));
                    }
                }
                var gat = TTSpace.ToUnity(home.getPositionX() + 25.5f, home.getPositionY() - 14f, home.getPositionZ() + 1f);
                for (int i = 0; i < gi; i++)   // each god close up, the camera on wherever it has walked to
                {
                    _showcaseAt.Add(gat); _showcaseRow.Add(-1);
                    _showcase.Add((float.NaN, i, 34f));
                    _benchPhases.Add(new BenchPhase { name = "showcase: god " + i + ", close up" });
                }
                // the game modes' objective props (the statue, the flags: treasure sprites as scenery), as the modes make them
                var tr = w.getRacesResources().getTreasures();
                for (int i = 0; i < tr.Length; i++)
                {
                    var sm = new com.oddlabs.tt.model.SceneryModel(w, home.getPositionX() + 12f + i * 5f, home.getPositionY() - 26f, 0f, 1f, tr[i], 2.6f, false, "Treasure " + i);
                    FaithfulView.Probe[sm] = "never in the scene";
                    _props.Add((sm, i));
                }
                var pat = TTSpace.ToUnity(home.getPositionX() + 24.5f, home.getPositionY() - 26f, home.getPositionZ() + 1f);
                _showcaseAt.Add(pat); _showcaseRow.Add(-2);
                _showcase.Add((home.getPositionX() + 24.5f, home.getPositionY() - 26f, 30f));
                _benchPhases.Add(new BenchPhase { name = "showcase: the objective props (treasure sprites 0-5)" });
                // the resources close up (their look): the nearest deposit of each kind to home, outside the clearing
                string[] resNames = { "rock", "iron", "gold", "coal", "crystal", "herbs", "diamond" };
                var resBest = new (float x, float y, float d)[resNames.Length];
                for (int i = 0; i < resBest.Length; i++) resBest[i] = (0f, 0f, float.MaxValue);
                int hgx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(home.getPositionX()), hgy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(home.getPositionY());
                for (int gy2 = 2; gy2 < gs - 2; gy2 += 1)
                    for (int gx2 = 2; gx2 < gs - 2; gx2 += 1)
                    {
                        var oc = g.getOccupant(gx2, gy2);
                        int rk = oc is com.oddlabs.tt.model.RockSupply ? 0 : oc is com.oddlabs.tt.model.IronSupply ? 1 : oc is com.oddlabs.tt.model.GoldSupply ? 2
                               : oc is com.oddlabs.tt.model.CoalSupply ? 3 : oc is com.oddlabs.tt.model.CrystalSupply ? 4 : oc is com.oddlabs.tt.model.HerbSupply ? 5
                               : oc is com.oddlabs.tt.model.DiamondSupply ? 6 : -1;
                        if (rk < 0) continue;
                        float d = (gx2 - hgx) * (gx2 - hgx) + (gy2 - hgy) * (gy2 - hgy);
                        if (d < resBest[rk].d) resBest[rk] = (com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(gx2), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(gy2), d);
                    }
                for (int i = 0; i < resNames.Length; i++)
                {
                    if (resBest[i].d == float.MaxValue) continue;
                    _showcaseAt.Add(TTSpace.ToUnity(resBest[i].x, resBest[i].y, w.getHeightMap().getNearestHeight(resBest[i].x, resBest[i].y) + 0.5f)); _showcaseRow.Add(-3);
                    _showcase.Add((resBest[i].x, resBest[i].y, 46f));
                    _benchPhases.Add(new BenchPhase { name = "showcase: resource close up - " + resNames[i] });
                }
                // and the edge of the forest, just past the clearing (its trees, close and from a little higher)
                float fx = home.getPositionX(), fyy = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(hy + 30) + 6f;
                _showcaseAt.Add(TTSpace.ToUnity(fx, fyy, w.getHeightMap().getNearestHeight(fx, fyy) + 3f)); _showcaseRow.Add(-3);
                _showcase.Add((fx, fyy, 26f));
                _benchPhases.Add(new BenchPhase { name = "showcase: the forest, close up" });
                _showcaseAt.Add(TTSpace.ToUnity(fx, fyy, w.getHeightMap().getNearestHeight(fx, fyy) + 3f)); _showcaseRow.Add(-3);
                _showcase.Add((fx, fyy, -30f));
                _benchPhases.Add(new BenchPhase { name = "showcase: the forest, from higher up" });
                int row0 = 0;
                foreach (var p in new[] { me, other })
                {
                    if (p == null) continue;
                    float ly = home.getPositionY() + (row0 == 0 ? -24f : 24f);
                    for (int k = 0; k < com.oddlabs.tt.model.Race.NUM_UNITS; k++)
                    {
                        var tpl = p.getRace().getUnitTemplate(k);
                        if (tpl == null) continue;
                        float lx = home.getPositionX() - 34f + k * 3.1f;
                        int cx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(lx), cy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(ly);
                        if (cx < 2 || cy < 2 || cx >= gs - 2 || cy >= gs - 2 || g.getRegion(cx, cy) == null || g.getOccupant(cx, cy) != null) { _lineup.Add((null, row0, k)); continue; }
                        var u = new com.oddlabs.tt.model.Unit(p, lx, ly, null, tpl);
                        _lineup.Add((u, row0, k));
                        FaithfulView.Probe[u] = "never in the scene";
                    }
                    var at = TTSpace.ToUnity(home.getPositionX() - 4f, ly, home.getPositionZ() + 1f);
                    _showcaseAt.Add(at); _showcaseAt.Add(at); _showcaseAt.Add(at);
                    _showcaseRow.Add(row0); _showcaseRow.Add(row0); _showcaseRow.Add(row0);
                    _showcase.Add((home.getPositionX() - 4f, ly, 9f));
                    _benchPhases.Add(new BenchPhase { name = "showcase: every unit, row " + row0 });
                    _showcase.Add((home.getPositionX() - 4f, ly, -24f));
                    _benchPhases.Add(new BenchPhase { name = "showcase: every unit, row " + row0 + ", further" });
                    _showcase.Add((home.getPositionX() - 4f, ly, -160f));   // zoomed right out: every unit drawn in bulk
                    _benchPhases.Add(new BenchPhase { name = "showcase: every unit, row " + row0 + ", zoomed out" });
                    foreach (int ck in new[] { 2, 6, 11, 16, 21, 24 })   // close up along the whole line: the warriors' helmets and weapons, the race units, the elites, the Risen Chieftain
                    {
                        _showcaseAt.Add(at); _showcaseRow.Add(row0);
                        _showcase.Add((home.getPositionX() - 34f + ck * 3.1f, ly, 34f));
                        _benchPhases.Add(new BenchPhase { name = "showcase: row " + row0 + ", close up on kinds " + (ck - 2) + "-" + (ck + 2) });
                    }
                    row0++;
                }
            }
            // the blowgunners shooting (their blow pose): the Natives' Blowgunners and Dart Masters out past the line-up,
            // shooting at the other tribe's quarters (a target that doesn't shoot back and lasts) - the very first view
            if (w.isExtended() && other != null)
            {
                var nat = myVik ? other : me; var foe = myVik ? me : other;
                float bx = home.getPositionX() + 62f, by = home.getPositionY();
                var target = BenchBuildAt(foe, com.oddlabs.tt.model.Race.BUILDING_QUARTERS, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(bx),
                    com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(by + 16f), 6);
                var shooters = new List<com.oddlabs.tt.model.Selectable>();
                for (int i = 0; target != null && i < 6; i++)
                {
                    float x = bx + (i - 2.5f) * 2.4f, y = by;
                    int cx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), cy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y);
                    if (cx < 2 || cy < 2 || cx >= gs - 2 || cy >= gs - 2 || g.getRegion(cx, cy) == null || g.getOccupant(cx, cy) != null) continue;
                    var tpl = nat.getRace().getUnitTemplate(i % 2 == 0 ? com.oddlabs.tt.model.Race.UNIT_UNIQUE_1 : com.oddlabs.tt.model.Race.UNIT_UNIQUE_3);
                    if (tpl == null) continue;
                    shooters.Add(new com.oddlabs.tt.model.Unit(nat, x, y, null, tpl));
                }
                if (shooters.Count > 0)
                {
                    nat.setLandscapeTarget(shooters.ToArray(), target.getGridX(), target.getGridY(), com.oddlabs.tt.model.Action.ATTACK, false);
                    float hz = w.getHeightMap().getNearestHeight(bx, by + 2f);
                    _showcaseAt.Insert(0, TTSpace.ToUnity(bx, by + 2f, hz + 1f)); _showcaseRow.Insert(0, -3);
                    _showcase.Insert(0, (bx, by + 2f, 38f));
                    _benchPhases.Insert(0, new BenchPhase { name = "showcase: the Blowgunners and Dart Masters shooting, close up" });
                    TTExtendedArt.BlowPosed = 0;
                }
            }
            // first: two warbands facing each other in the clearing, for their weapons and the shadows they throw
            var bands = new List<com.oddlabs.tt.model.Selectable>[2];
            int[] kinds = { com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER };
            float ux = home.getPositionX(), uy = home.getPositionY();
            for (int side = 0; side < 2; side++)
            {
                var p = side == 0 ? me : other;
                bands[side] = new List<com.oddlabs.tt.model.Selectable>();
                if (p == null) continue;
                // their armour: gold trim on one side, diamond on the other (the warriors' look, New Tides)
                if (w.isExtended())
                    for (int l = p.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ARMOR); l < (side == 0 ? 3 : 5); l++)
                        p.upgrade(com.oddlabs.tt.model.ResearchContainer.ARMOR);
                for (int i = 0; i < 18; i++)
                {
                    float x = ux - 14f + (i % 6) * 2.6f + (side == 0 ? -4f : 4f), y = uy + (side == 0 ? -4f : 4f) + (i / 6) * (side == 0 ? -2.4f : 2.4f);
                    int cx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), cy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y);
                    if (cx < 2 || cy < 2 || cx >= gs - 2 || cy >= gs - 2 || g.getRegion(cx, cy) == null || g.getOccupant(cx, cy) != null) continue;
                    bands[side].Add(new com.oddlabs.tt.model.Unit(p, x, y, null, p.getRace().getUnitTemplate(kinds[i / 6])));
                }
                // and their gods (drawn as grown chieftains) and a few of the New Tides warriors in their own skins
                int[] extra = { com.oddlabs.tt.model.Race.UNIT_MINOR_GOD, com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD, com.oddlabs.tt.model.Race.UNIT_CRYSTAL,
                    com.oddlabs.tt.model.Race.UNIT_DIAMOND, com.oddlabs.tt.model.Race.UNIT_ELITE_1, com.oddlabs.tt.model.Race.UNIT_CHAMPION };
                if (w.isExtended())
                    for (int i = 0; i < extra.Length; i++)
                    {
                        float x = ux - 12f + i * 3.4f, y = uy + (side == 0 ? -12f : 12f);
                        bands[side].Add(new com.oddlabs.tt.model.Unit(p, x, y, null, p.getRace().getUnitTemplate(extra[i])));
                    }
            }
            for (int side = 0; side < 2; side++)
                if (bands[side].Count > 0 && bands[1 - side].Count > 0)
                    (side == 0 ? me : other).setLandscapeTarget(bands[side].ToArray(), bands[1 - side][0].getGridX(), bands[1 - side][0].getGridY(), com.oddlabs.tt.model.Action.ATTACK, false);
            foreach (var zoom in new[] { 7f, 13f })
            {
                _showcaseAt.Add(TTSpace.ToUnity(ux - 8f, uy, home.getPositionZ() + 1f));
                _showcase.Add((ux - 8f, uy, zoom));
                _benchPhases.Add(new BenchPhase { name = "showcase: warriors and their weapons' shadows" + (zoom > 10f ? ", further" : "") });
            }
            // the buildings, upgraded in a mix of ways (stone / iron / crystal and diamond, chicken) to show the skins
            int[] tiers = { 1, 2, 3, 0, 3, 1, 2, 1, 2, 3 };
            bool[] chicken = { false, true, false, true, true, false, true, false, false, true };
            int row = 0;
            foreach (var p in new[] { me, other })
            {
                if (p == null) continue;
                string race = com.oddlabs.tt.model.ExtendedCombat.isVikings(p) ? "Vikings" : "Natives";
                for (int i = 0; i < ids.Length; i++)
                {
                    int nx = hx - 36 + i * 8, ny = hy + (row == 0 ? -16 : 16);
                    var b = BenchBuildAt(p, ids[i], nx, ny, ids[i] == com.oddlabs.tt.model.Race.BUILDING_DOCK ? 90 : 14);
                    if (b == null) { Debug.Log("[TT] Showcase: no room for the " + race + " " + names[i]); continue; }
                    int ti = (i + row * 3) % tiers.Length;
                    if (w.isExtended())
                    {
                        b.setTier(tiers[ti]);
                        if (chicken[ti]) b.setChickenBarbs();
                    }
                    string up = (tiers[ti] == 0 ? "" : tiers[ti] == 1 ? " stone" : tiers[ti] == 2 ? " iron" : " crystal+diamond") + (chicken[ti] && w.isExtended() ? " chicken" : "");
                    names[i] = names[i].Split(' ')[0] + up;
                    var at = TTSpace.ToUnity(b.getPositionX(), b.getPositionY(), b.getPositionZ() + 2f);
                    _showcaseAt.Add(at); _showcaseAt.Add(at);
                    _showcase.Add((b.getPositionX(), b.getPositionY(), 12f));
                    _benchPhases.Add(new BenchPhase { name = "showcase: " + race + " " + names[i] });
                    _showcase.Add((b.getPositionX(), b.getPositionY(), -15f));
                    _benchPhases.Add(new BenchPhase { name = "showcase: " + race + " " + names[i] + ", further" });
                }
                row++;
            }
            if (_showcase.Count == 0) return false;
            _autoShots = true;
            _benchHold = 3f;
            return true;
        }

        // ---- the settings sweep (autorun "sweep=1"): the zoomed-out battle under lighter and lighter settings, to see
        // what the graphics card spends its time on
        bool _autoSweep;
        com.oddlabs.tt.camera.GameCamera _benchCam;
        string _benchNote;
        static readonly string[] SweepNames = { "your settings", "shadows 220 m", "+ no MSAA", "+ realistic trees to 120 m", "+ no ambient occlusion", "+ no shadows" };
        float _sweepShadow = -1f, _sweepTrees = -1f; int _sweepMsaa = -1; bool _sweepAo, _sweepShadows, _sweepSaved;

        void SweepApply(int step)
        {
            if (!(UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline is UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp)) return;
            if (!_sweepSaved)
            {
                _sweepSaved = true;
                _sweepShadow = TribalTrouble.Game.GameSettings.shadowDistance; _sweepMsaa = urp.msaaSampleCount;
                _sweepTrees = _view != null ? _view.realTreeDistance : 220f; _sweepAo = TribalTrouble.Game.GameSettings.ambientOcclusion; _sweepShadows = TribalTrouble.Game.GameSettings.shadows;
            }
            if (step >= 1) TribalTrouble.Game.GameSettings.shadowDistance = 220f;
            if (step >= 2) urp.msaaSampleCount = 1;
            if (step >= 3 && _view != null) _view.realTreeDistance = 120f;
            if (step >= 4) { TribalTrouble.Game.GameSettings.ambientOcclusion = false; TTRenderFeatures.SetAmbientOcclusion(false); }
            if (step >= 5) TribalTrouble.Game.GameSettings.shadows = false;
            TribalTrouble.Game.GameSettings.Apply(false);
        }

        void SweepRestore()
        {
            if (!_sweepSaved || !(UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline is UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp)) return;
            _sweepSaved = false;
            TribalTrouble.Game.GameSettings.shadowDistance = _sweepShadow; urp.msaaSampleCount = _sweepMsaa;
            if (_view != null) _view.realTreeDistance = _sweepTrees;
            TribalTrouble.Game.GameSettings.ambientOcclusion = _sweepAo; TribalTrouble.Game.GameSettings.shadows = _sweepShadows;
            TribalTrouble.Game.GameSettings.Apply(false);
        }

        void BenchView(com.oddlabs.tt.render.DefaultRenderer dr, int step)
        {
            if (_showcase != null && step < _showcase.Count)
            {
                var top0 = dr.guiRoot?.getDelegate();
                if (top0 is com.oddlabs.tt.@delegate.CameraDelegate cd0 && cd0.getCamera() is com.oddlabs.tt.camera.GameCamera f0) _benchCam = f0;
                if (_benchCam == null) return;
                var sc = _showcase[step];
                if (float.IsNaN(sc.x))   // follow a portrait god (x NaN, y its index among them)
                {
                    int want = (int)sc.y, seen = 0;
                    foreach (var (gu, grow, _) in _lineup)
                        if (grow == -1 && gu != null && seen++ == want) { sc.x = gu.getPositionX(); sc.y = gu.getPositionY(); break; }
                    if (float.IsNaN(sc.x)) return;
                }
                _benchCam.setPos(sc.x, sc.y);
                if (sc.zoom != 0f) _benchCam.zoom(sc.zoom);
                Recenter(_benchCam, sc.x, sc.y);
                return;
            }
            if (_autoSweep && step >= 4) { SweepApply(step - 4); step = 1; }
            // the camera of the delegate on top - kept from the first view: another delegate (a menu, a jump to a
            // notification) can be on top later, and the views must still move
            var top = dr.guiRoot?.getDelegate();
            if (top is com.oddlabs.tt.@delegate.CameraDelegate cd && cd.getCamera() is com.oddlabs.tt.camera.GameCamera found) _benchCam = found;
            else _benchNote = "view " + step + ": delegate on top " + (top?.GetType().Name ?? "none");
            var gc = _benchCam;
            if (gc == null) return;
            switch (step)
            {
                case 0: gc.setPos(_benchHomeX, _benchHomeY); break;
                case 1: gc.setPos(_benchHomeX, _benchHomeY); gc.zoom(-160f); break;
                case 2: gc.setPos(_benchFarX, _benchFarY); break;
                default: gc.setPos(_benchHomeX, _benchHomeY); gc.zoom(-900f); break;
            }
        }

        /// <summary>Puts the spot back in the middle of the view after a zoom: a zoom at a shallow pitch moves the camera
        /// down more steeply than it looks, so the spot slid to the top of the picture (or off it).</summary>
        static void Recenter(com.oddlabs.tt.camera.GameCamera gc, float x, float y)
        {
            var st = gc.getState();
            if (st == null) return;
            float v = st.getTargetVertAngle(), h = st.getTargetHorizAngle();
            float down = -Mathf.Sin(v);
            if (down < 0.05f) return;
            float gz = gc.getWorld().getHeightMap().getNearestHeight(x, y);
            float z = Mathf.Max(st.getTargetZ(), gz + 4f);
            float d = (z - gz) / Mathf.Tan(-v);
            st.setCurrentX(x - Mathf.Cos(h) * d);
            st.setCurrentY(y - Mathf.Sin(h) * d);
            st.setCurrentZ(z);
        }

        static T FindChild<T>(com.oddlabs.tt.gui.Renderable r) where T : com.oddlabs.tt.gui.Renderable
        {
            if (r == null) return null;
            for (var c = r.getFirstChild(); c != null; c = (com.oddlabs.tt.gui.Renderable)c.getNext())
            {
                if (c is T t) return t;
                var deeper = FindChild<T>(c);
                if (deeper != null) return deeper;
            }
            return null;
        }

        static int CountPanels(com.oddlabs.tt.gui.PanelGroup g) => g.panels != null ? g.panels.Length : 0;

        void BenchFinish(bool aborted)
        {
            QualitySettings.vSyncCount = _benchSavedVsync; Application.targetFrameRate = _benchSavedFps;
            foreach (var r in new[] { _rBatches, _rSetPass, _rDraws, _rTris, _rVerts, _rCasters, _rSkinned, _rGc }) if (r.Valid) r.Dispose();
            foreach (var r in _rMarkers) if (r.Valid) r.Dispose();
            _benchStep = -1;
            var sb = new StringBuilder();
            sb.AppendLine("Tribal Trouble benchmark " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (aborted ? " (stopped early)" : ""));
            sb.AppendLine(_benchSetup);
            sb.AppendLine(_benchMarkerInfo);
            if (_benchNote != null) sb.AppendLine(_benchNote);
            try { sb.Append(FootprintReport()); } catch (System.Exception e) { sb.AppendLine("footprints: " + e.Message); }
            try { sb.Append(UnitReport()); } catch (System.Exception e) { sb.AppendLine("units: " + e.Message); }
            foreach (var ph in _benchPhases)
            {
                if (ph.n == 0) continue;
                var f = new List<float>(ph.frame); f.Sort();
                double sum = 0; foreach (var x in f) sum += x;
                double avg = sum / f.Count;
                float p99 = f[Mathf.Clamp((int)(f.Count * 0.99f), 0, f.Count - 1)], p50 = f[f.Count / 2];
                double k = 1.0 / ph.n, kt = ph.timed > 0 ? 1.0 / ph.timed : 0;
                sb.AppendLine();
                sb.AppendLine($"== {ph.name}: {1000.0 / avg:0} fps average, 1% low {1000.0 / p99:0} fps (frame ms: mean {avg:0.0}, median {p50:0.0}, 99th {p99:0.0}, worst {f[f.Count - 1]:0.0}); units {ph.units}, unit/building objects {ph.live}");
                string loopText = SimThreaded ? string.Format("waiting for the simulation thread {0:0.00}, its ticks and input {1:0.00}", ph.wait * k, ph.loop * k) : string.Format("ticks and input {0:0.00}", ph.loop * k);
                sb.AppendLine($"   main thread per frame (ms): game {ph.sim * k:0.00} ({loopText}, Java GUI {ph.guiMs * k:0.00})  world view {ph.render * k:0.00} (scene {ph.collect * k:0.00}, draw lists {ph.build * k:0.00}: unit objects {ph.placeMs * k:0.00}, far units {ph.farMs * k:0.00}, New Tides buildings {ph.extMs * k:0.00}; instanced draws {ph.drawMs * k:0.00}, particles {ph.partMs * k:0.00}, combat effects {ph.fxMs * k:0.00})  overlay {ph.overlay * k:0.00}  terrain {ph.terrain * k:0.00}");
                if (ph.timed > 0) sb.AppendLine($"   frame timing (ms): main thread {ph.mainCpu * kt:0.00}  render thread {ph.renderCpu * kt:0.00}  GPU {ph.gpu * kt:0.00}");
                sb.AppendLine($"   per frame: batches {ph.batches * k:0}  SetPass {ph.setPass * k:0}  draw calls {ph.draws * k:0}  triangles {ph.tris * k / 1e6:0.00} M  vertices {ph.verts * k / 1e6:0.00} M  shadow casters {ph.casters * k:0}  skinned meshes {ph.skinned * k:0}  GC alloc {ph.gcBytes * k / 1024:0} KB");
                sb.AppendLine($"   skinned units near the camera {ph.near * k:0}, units drawn in bulk {ph.far * k:0}, garbage collections {ph.gc1 - ph.gc0}, off-screen skipped {ph.culled * k:0}; instanced draws {ph.instBatches * k:0} ({ph.instShadow * k:0} casting shadows) for {ph.instances * k:0} instances ({ph.statics * k:0} cached plants / scenery); live objects {ph.live}");
                sb.AppendLine($"   trees {ph.trees * k:0} ({ph.realTrees * k:0} realistic models, {ph.impostors * k:0} impostors); {ph.cam}");
                if (ph.top != null) sb.AppendLine("   " + ph.top);
                if (ph.markers != null)
                {
                    var order = new List<int>();
                    for (int i = 0; i < ph.markers.Length; i++) if (ph.markers[i] * k >= 0.05) order.Add(i);
                    order.Sort((a, b) => ph.markers[b].CompareTo(ph.markers[a]));
                    var ms = new StringBuilder("   markers (ms/frame):");
                    foreach (int i in order) ms.Append($" {BenchMarkers[i]} {ph.markers[i] * k:0.0};");
                    sb.AppendLine(ms.ToString());
                }
            }
            string text = sb.ToString();
            Debug.Log("[TT] " + text);
            try
            {
                string dir = BenchDir;
                System.IO.Directory.CreateDirectory(dir);
                string file = System.IO.Path.Combine(dir, "bench_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                System.IO.File.WriteAllText(file, text);
                Debug.Log("[TT] Benchmark written to " + file);
            }
            catch (System.Exception e) { Debug.LogWarning("[TT] Benchmark: couldn't write the report: " + e.Message); }
            _benchDone = text;
            _benchDoneShown = Time.unscaledTime;
        }

        string _benchDone;
        float _benchDoneShown;

        void BenchGUI()
        {
            if (_benchStep >= 0 && _benchStep < _benchPhases.Count)
            {
                GUI.Box(new Rect(10, 60, 420, 26), GUIContent.none);
                GUI.Label(new Rect(16, 63, 410, 22), $"Benchmark {_benchStep + 1}/{_benchPhases.Count}: {_benchPhases[_benchStep].name}{(_benchTimed ? "" : " (settling)")}  - Shift+F9 stops");
            }
            else if (_benchDone != null && Time.unscaledTime - _benchDoneShown < 20f)
            {
                GUI.Box(new Rect(10, 60, 900, 320), GUIContent.none);
                GUI.Label(new Rect(16, 64, 890, 312), _benchDone);
            }
        }
    }
}
