// An unattended look at the Steam game (Benchmarks/autorun_go.txt or autorun.txt containing "steam=1", handed over by
// the editor's TTBenchAuto): screenshots of the main menu, of a single player game started through the game's own
// menus, the same game at night, and a report of what the game did - then play mode ends. Options in the file:
// "size=N" (0 small .. 4 archipelago is not picked here: the menu's default island is used), "frames=N" to wait.
using System;
using System.IO;
using System.Text;
using TribalTroubleSteam.GuiPort;
using UnityEngine;

namespace TribalTroubleSteam
{
    public sealed class SteamAutorun : MonoBehaviour
    {
        string _dir;
        int _step;
        float _wait;
        int _shot;
        readonly StringBuilder _log = new StringBuilder();

        public static void StartIfAsked(GameObject host)
        {
            string dir = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Benchmarks");
            foreach (var n in new[] { "autorun_go.txt", "autorun.txt" })
            {
                string f = Path.Combine(dir, n);
                try
                {
                    if (!File.Exists(f) || (DateTime.Now - File.GetLastWriteTime(f)).TotalMinutes > 10) continue;
                    string text = File.ReadAllText(f);
                    if (!text.Contains("steam=1")) continue;
                    File.Delete(f);
                    host.AddComponent<SteamAutorun>()._dir = dir;
                    return;
                }
                catch (Exception e) { Debug.LogWarning("[TT Steam] autorun: " + e.Message); }
            }
        }

        void Note(string s)
        {
            string line = DateTime.Now.ToString("HH:mm:ss") + " " + s;
            _log.AppendLine(line);
            Debug.Log("[TT Steam] autorun: " + s);
            try { File.AppendAllText(Path.Combine(_dir, "steam_autorun.txt"), line + "\n"); } catch (Exception) { }
        }

        void Start()
        {
            try { File.Delete(Path.Combine(_dir, "steam_autorun.txt")); } catch (Exception) { }
            Application.logMessageReceived += OnLog;
            Note("started");
        }

        void OnDestroy() => Application.logMessageReceived -= OnLog;

        void OnLog(string msg, string stack, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception) return;
            try { File.AppendAllText(Path.Combine(_dir, "steam_autorun.txt"), "  " + type + ": " + msg + "\n" + stack + "\n"); } catch (Exception) { }
        }

        /// <summary>The first GUI object labelled text (a Label - also a button's caption - or a MenuButton).</summary>
        static com.oddlabs.tt.gui.GUIObject Find(com.oddlabs.tt.gui.GUIRoot root, string text)
        {
            com.oddlabs.tt.gui.GUIObject found = null;
            void Walk(com.oddlabs.tt.gui.Renderable r)
            {
                if (found != null || r == null) return;
                if ((r is com.oddlabs.tt.gui.Label l && l.toString() == text)
                    || (r is com.oddlabs.tt.gui.MenuButton mb && java.lang.JStr.valueOf(mb.getText()) == text))
                { found = r as com.oddlabs.tt.gui.GUIObject; return; }
                for (var c = r.getFirstChild(); c != null; c = (com.oddlabs.tt.gui.Renderable)c.getNext()) Walk(c);
            }
            Walk(root);
            return found;
        }

        /// <summary>
        /// Clicks it the way the GUI does after a press and release on it (GUIObject.mouseClickedAll: a caption passes
        /// the click on to its button). Straight into the GUI rather than through synthetic mouse events, which raced
        /// the menu's fade-in and the real cursor. Safe here: SteamGame.Update joined the game's thread first.
        /// </summary>
        bool Click(com.oddlabs.tt.gui.GUIRoot root, string text)
        {
            var o = root != null ? Find(root, text) : null;
            if (o == null) return false;
            Note("click '" + text + "'");
            try { o.mouseClickedAll(com.oddlabs.tt.gui.MouseButton.LEFT, o.getWidth() / 2, o.getHeight() / 2, 1); }
            catch (Exception e) { Note("click '" + text + "' failed: " + e.Message); }
            return true;
        }

        readonly System.Collections.Generic.List<(float, Action)> _script = new System.Collections.Generic.List<(float, Action)>();
        com.oddlabs.tt.model.Building _armory;
        com.oddlabs.tt.gui.IconSpinner _spinner;

        static T FindOf<T>(com.oddlabs.tt.gui.Renderable r) where T : class
        {
            if (r == null) return null;
            if (r is T t) return t;
            try
            {
                for (var c = r.getFirstChild(); c != null; c = (com.oddlabs.tt.gui.Renderable)c.getNext())
                {
                    var f = FindOf<T>(c);
                    if (f != null) return f;
                }
            }
            catch (Exception e) { Debug.LogWarning("[TT autorun] GUI walk: " + e.Message); }
            return null;
        }

        /// <summary>Window pixels of a GUI object's centre (GUI units times the root's scale; y from the bottom).</summary>
        static Vector2 ScreenOf(com.oddlabs.tt.gui.Renderable r)
        {
            float scale = r.getGlobalScale();
            return new Vector2((r.getRootX() + r.getWidth() / 2f) * scale, (r.getRootY() + r.getHeight() / 2f) * scale);
        }

        void Press(Vector2 p, int button = 0)
        {
            _script.Add((0.2f, () => Host.Input.MouseMoveIn((int)p.x, (int)p.y)));
            _script.Add((0.15f, () => Host.Input.MouseButtonIn(button, true)));
            _script.Add((0.3f, () => Host.Input.MouseButtonIn(button, false)));
        }

        void SpinnerNote(string when)
        {
            if (_spinner == null) { Note(when + ": no production spinner"); return; }
            Note($"{when}: spinner {_spinner.GetType().Name} shows {_spinner.getDisplayCount()} (order {_spinner.getOrderSize()}), minus at {ScreenOf(_spinner.button_minus)} size {_spinner.button_minus.getWidth()}x{_spinner.button_minus.getHeight()}");
        }

        /// <summary>An armory and quarters for the player; the armory clicked; its first weapon ordered three times with
        /// the + button and one taken off with the - button; then the view zoomed out, and map mode held (Space).</summary>
        void Script(com.oddlabs.tt.render.DefaultRenderer dr)
        {
            _script.Clear();
            _script.Add((1.5f, () =>
            {
                var p = dr.localPlayer;
                int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
                int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
                _armory = p.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, gx + 8, gy + 4);
                var q = p.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, gx - 8, gy + 4);
                var t = p.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, gx, gy + 12);
                Note($"built: armory {_armory != null}, quarters {q != null}, tower {t != null}");
            }));
            _script.Add((1f, () =>
            {
                if (_armory == null) return;
                var cam = Camera.main;
                var sp = cam != null ? cam.WorldToScreenPoint(TTSpace.ToUnity(_armory.getPositionX(), _armory.getPositionY(), _armory.getPositionZ() + 2f)) : Vector3.zero;
                Note($"armory on screen at {sp}");
                Host.Input.MouseMoveIn((int)sp.x, (int)sp.y);
                Host.Input.MouseButtonIn(0, true);
            }));
            _script.Add((0.12f, () => Host.Input.MouseButtonIn(0, false)));
            _script.Add((0.5f, () =>
            {
                var army = dr.selection?.getCurrentSelection();
                bool clicked = army != null && _armory != null && army.contains(_armory);
                Note("selected by the click: " + clicked);
                if (!clicked && army != null && _armory != null) { army.clear(); army.add(_armory); Note("selected it directly"); }
                Shot("in game, buildings, armory clicked");
            }));
            _script.Add((1.0f, () =>
            {
                // the armory's weapons (its build submenu: the spinners with their + and - buttons)
                var panel = FindOf<com.oddlabs.tt.gui.ActionButtonPanel>(dr.guiRoot);
                Note("action panel: " + (panel != null));
                try { panel?.setSubmenu(com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_BUILD); } catch (Exception e) { Note("submenu: " + e.Message); }
            }));
            _script.Add((1.0f, () =>
            {
                _spinner = FindOf<com.oddlabs.tt.gui.BuildSpinner>(dr.guiRoot) ?? (com.oddlabs.tt.gui.IconSpinner)FindOf<com.oddlabs.tt.gui.IconSpinner>(dr.guiRoot);
                SpinnerNote("before");
                if (_spinner != null)
                {
                    try { Note("hotkeys: " + com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(com.oddlabs.tt.input.GameAction.RES_ROCK) + " / " + com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(com.oddlabs.tt.input.GameAction.RES_ROCK_DEC)); } catch (Exception e) { Note("hotkeys: " + e.Message); }
                    var plus = ScreenOf(_spinner.button_plus);
                    var minus = ScreenOf(_spinner.button_minus);
                    var keep = new System.Collections.Generic.List<(float, Action)>(_script);
                    _script.Clear();
                    Press(plus); Press(plus); Press(plus);
                    _script.Add((0.3f, () => SpinnerNote("after + x3")));
                    Press(minus);
                    _script.Add((0.3f, () => { SpinnerNote("after - x1"); Shot("in game, the armory's weapons after + + + -"); }));
                    // the keyboard: R twice, then Shift+R (RES_ROCK, RES_ROCK_DEC) on the rock axes
                    const int KeyR = 82, KeyShift = 340;
                    void Key(int k, int mods) { _script.Add((0.15f, () => Host.Input.KeyEventIn(k, true, false, mods))); _script.Add((0.15f, () => Host.Input.KeyEventIn(k, false, false, mods))); }
                    _script.Add((0.2f, () => { var pnl = FindOf<com.oddlabs.tt.gui.ActionButtonPanel>(dr.guiRoot); _spinner = pnl?.build_weapon_rock_button ?? _spinner; SpinnerNote("rock, before keys"); }));
                    Key(KeyR, 0); Key(KeyR, 0);
                    _script.Add((0.4f, () => SpinnerNote("rock, after R R")));
                    _script.Add((0.1f, () => Host.Input.KeyEventIn(KeyShift, true, false, 1)));
                    Key(KeyR, 1);
                    _script.Add((0.1f, () => Host.Input.KeyEventIn(KeyShift, false, false, 0)));
                    _script.Add((0.4f, () => SpinnerNote("rock, after Shift+R")));
                    _script.AddRange(keep);
                }
            }));
            // zoomed out (the wheel), then map mode (Space held)
            for (int i = 0; i < 12; i++) _script.Add((0.08f, () => Host.Input.WheelIn(-1f)));
            _script.Add((1.5f, () => { }));
            _script.Add((0.5f, () => Shot("in game, zoomed out")));
            _script.Add((1.2f, () => Host.Input.KeyEventIn(32, true, false, 0)));
            _script.Add((0.5f, () => Shot("in game, map mode")));
            _script.Add((0.5f, () => Host.Input.KeyEventIn(32, false, false, 0)));
            for (int i = 0; i < 12; i++) _script.Add((0.05f, () => Host.Input.WheelIn(1f)));
        }

        void Shot(string what)
        {
            string path = Path.Combine(_dir, "steam_shot_" + _shot++ + ".png");
            Note("screenshot " + Path.GetFileName(path) + ": " + what);
            StartCoroutine(Save(path));
        }

        System.Collections.IEnumerator Save(string path)
        {
            yield return new WaitForEndOfFrame();
            var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply(false);
            try { File.WriteAllBytes(path, tex.EncodeToPNG()); } catch (Exception e) { Note("screenshot failed: " + e.Message); }
            Destroy(tex);
        }

        void Update()
        {
            if ((_wait -= Time.unscaledDeltaTime) > 0f) return;
            var game = GetComponent<SteamGame>();
            var root = game != null ? game.GuiRoot : null;
            var dr = Host.CurrentWorld;
            switch (_step)
            {
                case 0:   // the main menu with its island
                    if (root == null || dr == null || PortHooks.Loading) { _wait = 0.5f; return; }
                    _menuWorld = dr.world;
                    _wait = 6f; _step++;
                    return;
                case 1:
                    Shot("main menu");
                    // the patch notes (their text box clips its lines to the panel: GL11 scissor)
                    try { root.addModalForm(new com.oddlabs.tt.form.PatchNotesForm()); _patchNotes = true; }
                    catch (System.Exception e) { Note("patch notes: " + e.Message); }
                    _wait = 1.5f; _step++;
                    return;
                case 2:   // (first-start dialogs: the patch notes after an update, warnings ..)
                    if (_patchNotes) { _patchNotes = false; Shot("patch notes"); _wait = 0.5f; return; }
                    if (_oks++ < 4 && Click(root, "OK")) { _wait = 1.5f; return; }
                    _wait = 0.5f; _step++;
                    return;
                case 3:
                    if (!Click(root, "Single player")) { Note("no 'Single player' button"); _step = 90; return; }
                    _wait = 3f; _step++;
                    return;
                case 4:
                    Shot("single player menu");
                    _wait = 1f; _step++;
                    return;
                case 5:
                    if (!Click(root, "OK")) { Note("no OK in the single player menu"); _step = 90; return; }
                    _wait = 2f; _step++; _t = 0f;
                    return;
                case 6:   // the game loads and starts
                    _t += 0.5f;
                    if (dr == null || dr.world == null || dr.world == _menuWorld || dr.localPlayer == null || PortHooks.Loading || dr.world.getTick() < 50)
                    {
                        if (_t > 240f) { Note("the game did not start"); _step = 90; return; }
                        _wait = 0.5f; return;
                    }
                    Note("in game: tick " + dr.world.getTick() + ", players " + dr.world.getPlayers().Length);
                    _wait = 8f; _step++;
                    return;
                case 7:
                    Shot("in game, day");
                    _wait = 20f; _step++;
                    return;
                case 8:
                    Shot("in game, 20 s later");
                    Report(dr);
                    dr.world.toggleNight();   // (render-only: the Steam console's night switch)
                    _wait = 4f; _step++;
                    return;
                case 9:
                    Shot("in game, night");
                    Note($"night {dr.world.isNight()}: globals tint {Shader.GetGlobalVector("_TTS_SceneTint")} ambient {Shader.GetGlobalVector("_TTS_Ambient")} ready {Shader.GetGlobalFloat("_TTS_Ready")}");
                    _wait = 0.5f; _step++;
                    return;
                case 10:  // a few of the player's units selected: their halos
                {
                    dr.world.toggleNight();
                    var army = dr.selection?.getCurrentSelection();
                    if (army != null)
                    {
                        army.clear();
                        var it = dr.localPlayer.getUnits().getSet().iterator();
                        int n = 0;
                        while (it.hasNext() && n < 8) { if (it.next() is com.oddlabs.tt.model.Selectable s) { army.add(s); n++; } }
                        Note("selected " + n);
                    }
                    _wait = 1.5f; _step++;
                    return;
                }
                case 11:
                    Shot("in game, units selected");
                    // the placing ghost and the building sites of a quarters next to the first selected unit
                    {
                        var army = dr.selection?.getCurrentSelection();
                        com.oddlabs.tt.model.Selectable first = null;
                        if (army != null) { var it = army.getSet().iterator(); if (it.hasNext()) first = it.next() as com.oddlabs.tt.model.Selectable; }
                        var template = dr.localPlayer.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
                        if (first != null && template != null)
                        {
                            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(first.getPositionX() + 8f);
                            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(first.getPositionY());
                            var world = dr.world;
                            SteamWorldOverlay.DebugPlacing = () => new com.oddlabs.tt.render.BuildingGhostRenderer.Request { world = world, template = template, gridX = gx, gridY = gy };
                        }
                    }
                    _wait = 1.5f; _step++;
                    return;
                case 12:
                {
                    // a click on the ground: its green ring, at full size this frame
                    var army = dr.selection?.getCurrentSelection();
                    if (army != null) { var it = army.getSet().iterator(); if (it.hasNext() && it.next() is com.oddlabs.tt.model.Selectable s) com.oddlabs.tt.render.PortPicker.addRespond(new com.oddlabs.tt.landscape.LandscapeTargetRespond(dr.world, s.getPositionX() - 6f, s.getPositionY())); }
                    Shot("in game, placing a quarters, a click on the ground");
                    Note("building sites around the ghost: " + (GetComponent<SteamGame>()?.View?.overlay?.statSites ?? -2));
                    _wait = 1f; _step++;
                    return;
                }
                case 13:
                    SteamWorldOverlay.DebugPlacing = null;
                    dr.selection?.getCurrentSelection()?.clear();
                    Script(dr);
                    _wait = 1f; _step = 50;
                    return;
                case 50:   // the scripted part: real input through the engine's queues (clicks, keys, the wheel)
                    if (_script.Count == 0) { _step = 90; return; }
                    {
                        var (wait, act) = _script[0];
                        _script.RemoveAt(0);
                        try { act(); } catch (Exception e) { Note("script step failed: " + e.Message); }
                        _wait = wait;
                    }
                    return;
                case 90:
                    Note("done");
                    try { File.WriteAllText(Path.Combine(_dir, "steam_autorun_done.txt"), DateTime.Now.ToString("s")); } catch (Exception) { }
                    _step = 91; _wait = 1f;
                    return;
                case 91:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();   // (a built player run by the runner's "play" step)
#endif
                    enabled = false;
                    return;
            }
        }

        float _t;
        int _oks;
        bool _patchNotes;
        object _menuWorld;

        void Report(com.oddlabs.tt.render.DefaultRenderer dr)
        {
            var w = dr.world;
            var sb = new StringBuilder("world tick " + w.getTick() + ", night " + w.isNight());
            foreach (var p in w.getPlayers())
                sb.Append("; " + p.getPlayerInfo().getName() + ": units " + p.getUnitCountContainer().getNumSupplies() + ", buildings " + p.getBuildingCountContainer().getNumSupplies());
            Note(sb.ToString());
            var view = GetComponent<SteamGame>()?.View;
            if (view != null)
            {
                Note($"view: items {view.statItems}, trees {view.statTrees}, instances {view.statInstances}, live {view.statLive}, far {view.statFar}, culled {view.statCulled}");
                var kinds = new System.Collections.Generic.Dictionary<string, int>();
                foreach (var it in view.scene.items) { string k = it.kind.ToString(); kinds[k] = kinds.TryGetValue(k, out var n) ? n + 1 : 1; }
                var ks = new StringBuilder("scene:");
                foreach (var kv in kinds) ks.Append(" " + kv.Key + " " + kv.Value);
                Note(ks.ToString());
            }
            var game = GetComponent<SteamGame>();
            Note($"shaders: terrain {game?.TerrainMaterialInUse?.shader?.name}, sprites {game?.SpriteMaterialInUse?.shader?.name}; Steam shaders loaded: sprite {SteamShading.Sprite != null}, terrain {SteamShading.Terrain != null}, decal {SteamShading.Decal != null}, ghost {SteamShading.Ghost != null}, water {SteamShading.Water != null}");
            var cs = Host.CurrentCamera;
            if (cs != null) Note($"java camera at {cs.getCurrentX():0.0},{cs.getCurrentY():0.0},{cs.getCurrentZ():0.0} horiz {cs.getHorizAngle():0.00} vert {cs.getCurrentVertAngle():0.00}");
            var cam = Camera.main;
            if (cam != null) Note($"unity camera at {cam.transform.position} looking {cam.transform.forward}");
            var it2 = dr.localPlayer.getUnits().getSet().iterator();
            int shown = 0;
            while (it2.hasNext() && shown < 3)
            {
                var sel = (com.oddlabs.tt.model.Selectable)it2.next();
                Note($"unit {sel.GetType().Name} at {sel.getPositionX():0.0},{sel.getPositionY():0.0},{sel.getPositionZ():0.0}");
                shown++;
            }
            Note($"fps {1f / Mathf.Max(1e-4f, Time.smoothDeltaTime):0}");
        }
    }
}
