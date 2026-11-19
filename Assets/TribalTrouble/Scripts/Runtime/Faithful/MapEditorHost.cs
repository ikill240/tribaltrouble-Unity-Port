using System.Collections.Generic;
using TribalTrouble.Faithful;
using TribalTrouble.Game;
using TribalTrouble.Presentation;
using UnityEngine;
using com.oddlabs.tt.mapeditor;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Your Java map editor in Unity (stage 7): the "Map editor" menu page (new map from settings or a map code, saved
    /// maps with previews), and the editor screen — the island through the game camera, the brush toolbar, start
    /// positions, the playable-area map, undo/redo and saving. All the editing is the translated editor code driven by
    /// <see cref="MapEditorSession"/>; maps are the same .ttmap files your Java game reads and writes.
    /// </summary>
    public sealed class MapEditorHost
    {
        public enum State { Menu, Building, Editing }
        public State state { get; private set; } = State.Menu;
        public bool Active => state != State.Menu;

        readonly TTSpriteCatalog _catalog;
        readonly Camera _cam;
        readonly GUIStyle _label;
        /// <summary>Called when the editor closes: the map to play (path) or null to go back to the menu.</summary>
        public System.Action<string> onExit;
        public Light sun;

        // ---- new-map settings (MapSettings: sliders 0..20, value / 10, same as the skirmish menu)
        int _size = 1, _terrainType, _hills = 5, _trees = 5, _supplies = 5, _seed = -1;
        string _seedText = "", _codeText = "", _menuError;
        // ---- saved maps
        List<MapFile.Entry> _entries;
        readonly Dictionary<string, Texture2D> _previews = new Dictionary<string, Texture2D>();
        Vector2 _listScroll;
        string _confirmDelete;

        // ---- building
        System.Threading.Thread _builder;
        MapEditorSession _built;
        System.Exception _buildError;
        float _buildStarted;
        string _buildWhat;

        // ---- editing
        MapEditorSession _s;
        GameObject _root;
        FaithfulTerrain _terrain;
        FaithfulView _view;
        RtsCamera _rts;
        LineRenderer _outline, _rampLine;
        readonly List<GameObject> _markers = new List<GameObject>();
        bool _hasCursor;
        float _cx, _cy;
        int _hx0 = int.MaxValue, _hy0 = int.MaxValue, _hx1 = int.MinValue, _hy1 = int.MinValue;
        bool _depthDirty;
        float _depthTimer;
        bool _menuOpen, _saveOpen, _exitConfirm, _playAfterSave, _showMap = true;
        string _saveName = "", _saveError;
        bool _overwriteAsk;
        int _autoCount = 4;
        Texture2D _mapTex;
        float _mapTimer;
        readonly List<(string text, float until)> _messages = new List<(string, float)>();
        readonly List<Rect> _guiRects = new List<Rect>();
        Rect _toolbarRect;

        static readonly Brush[] TerrainBrushes = { Brush.HEIGHT, Brush.FLATTEN, Brush.SMOOTH, Brush.RANDOM, Brush.RAMP, Brush.CLIFF, Brush.PLATEAU };
        static readonly Brush[] ResourceBrushes = { Brush.TREES, Brush.PALMS, Brush.ROCK, Brush.IRON, Brush.ERASE, Brush.START };

        public MapEditorHost(TTSpriteCatalog catalog, Camera cam, GUIStyle label)
        {
            _catalog = catalog;
            _cam = cam;
            _label = label;
        }

        static string T(string key, params object[] args)
        {
            try { return MapEditor.i18n(key, args); } catch { return key; }
        }

        // ================================================================== menu page

        public void MenuGUI()
        {
            GUILayout.Label("<b>" + T("new_map") + "</b>", _label);
            Row("Island size", () => _size = GUILayout.SelectionGrid(_size, IslandSettings.SizeNames, 3));
            Row("Terrain", () => _terrainType = GUILayout.Toolbar(_terrainType, new[] { "Tropical", "Northern" }));
            Row("Hills", () => _hills = Steps(_hills));
            Row("Vegetation", () => _trees = Steps(_trees));
            Row("Supplies", () => _supplies = Steps(_supplies));
            Row("Seed (blank = random)", () =>
            {
                _seedText = GUILayout.TextField(_seedText, 5, GUILayout.Width(80));
                _seed = int.TryParse(_seedText, out int v) && v >= 0 && v < 40000 ? v : -1;
                GUILayout.FlexibleSpace();
            });
            Row("Map code", () =>
            {
                _codeText = GUILayout.TextField(_codeText, 120);
                if (GUILayout.Button("Use", GUILayout.Width(50)))
                {
                    var cur = new MapSettings(_size, _terrainType, _hills, _trees, _supplies, System.Math.Max(0, _seed));
                    var parsed = cur.parseMapcode(_codeText);
                    if (parsed == null) _menuError = T("invalid_map_code");
                    else
                    {
                        _menuError = null;
                        _size = parsed.size(); _terrainType = parsed.terrain(); _hills = parsed.hills(); _trees = parsed.trees(); _supplies = parsed.supplies();
                        _seed = parsed.seed(); _seedText = _seed.ToString();
                    }
                }
            });
            if (_menuError != null) GUILayout.Label("<color=#ff8080>" + _menuError + "</color>", _label);
            GUI.enabled = _catalog != null;
            if (GUILayout.Button(T("new_map"), GUILayout.Height(32)))
            {
                int seed = _seed >= 0 ? _seed : Random.Range(0, 40000);
                _seedText = seed.ToString();
                int size = _size, terrain = _terrainType, hills = _hills, trees = _trees, supplies = _supplies;
                Build(() => MapEditorSession.New(size, terrain, hills, trees, supplies, seed), T("new_map"));
            }
            GUI.enabled = true;

            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Label("<b>" + T("load_caption") + "</b>", _label);
            if (GUILayout.Button("Refresh", GUILayout.Width(70))) _entries = null;
            if (GUILayout.Button(T("folder_preview"), GUILayout.Width(70))) Application.OpenURL("file://" + MapEditorSession.MapsDir);
            GUILayout.EndHorizontal();
            if (_entries == null) RefreshEntries();
            _listScroll = GUILayout.BeginScrollView(_listScroll, GUI.skin.box, GUILayout.Height(220));
            if (_entries.Count == 0) GUILayout.Label("<color=#aaaaaa>No saved maps yet (" + MapEditorSession.MapsDir + ")</color>", _label);
            foreach (var e in _entries)
            {
                string path = e.path().ToString();
                GUILayout.BeginHorizontal();
                var tex = Preview(path);
                if (tex != null) GUILayout.Box(tex, GUILayout.Width(56), GUILayout.Height(56)); else GUILayout.Box(T("no_preview"), GUILayout.Width(56), GUILayout.Height(56));
                var st = e.settings();
                GUILayout.BeginVertical();
                GUILayout.Label("<b>" + e.name() + "</b>", _label);
                string size = st.size() < IslandSettings.SizeNames.Length ? IslandSettings.SizeNames[st.size()] : "?";
                GUILayout.Label("<color=#bbbbbb>" + size + ", " + (st.terrain() == 1 ? "northern" : "tropical") + (e.edited() ? ", edited" : "") + " · " + System.IO.File.GetLastWriteTime(path).ToString("g") + "</color>", _label);
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUI.enabled = _catalog != null;
                if (GUILayout.Button("Edit", GUILayout.Width(50), GUILayout.Height(40))) Build(() => MapEditorSession.Load(path), e.name());
                GUI.enabled = true;
                if (GUILayout.Button(T("play_button"), GUILayout.Width(50), GUILayout.Height(40))) onExit?.Invoke(path);
                if (_confirmDelete == path)
                {
                    if (GUILayout.Button("Sure?", GUILayout.Width(55), GUILayout.Height(40)))
                    {
                        try { System.IO.File.Delete(path); } catch (System.Exception ex) { _menuError = T("delete_failed", ex.Message); }
                        _confirmDelete = null; _entries = null;
                        GUILayout.EndHorizontal();
                        break;
                    }
                }
                else if (GUILayout.Button(T("delete_button"), GUILayout.Width(55), GUILayout.Height(40))) _confirmDelete = path;
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        int Steps(int v)
        {
            int r = Mathf.RoundToInt(GUILayout.HorizontalSlider(v, 0, IslandSettings.SliderMax));
            GUILayout.Label((r * 10) + "%", _label, GUILayout.Width(44));
            return r;
        }

        void Row(string label, System.Action control)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, _label, GUILayout.Width(170));
            control();
            GUILayout.EndHorizontal();
        }

        void RefreshEntries()
        {
            _entries = new List<MapFile.Entry>();
            try
            {
                System.IO.Directory.CreateDirectory(MapEditorSession.MapsDir);
                var list = MapFile.list(java.nio.file.Path_.of(MapEditorSession.MapsDir));
                for (int i = 0; i < list.size(); i++) _entries.Add((MapFile.Entry)list.get(i));
            }
            catch (System.Exception e) { _menuError = e.Message; }
        }

        Texture2D Preview(string path)
        {
            if (_previews.TryGetValue(path, out var t)) return t;
            t = null;
            try
            {
                var p = MapFile.loadPreview(java.nio.file.Path_.of(path));
                if (p != null)
                {
                    int n = p.size();
                    var rgb = p.rgb();
                    t = new Texture2D(n, n, TextureFormat.RGB24, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
                    var px = new Color32[n * n];
                    for (int i = 0; i < px.Length; i++) px[i] = new Color32((byte)rgb[i * 3], (byte)rgb[i * 3 + 1], (byte)rgb[i * 3 + 2], 255);
                    t.SetPixels32(px);
                    t.Apply(false);
                }
            }
            catch { }
            _previews[path] = t;
            return t;
        }

        // ================================================================== building

        void Build(System.Func<MapEditorSession> make, string what)
        {
            _built = null; _buildError = null;
            _buildWhat = what;
            _buildStarted = Time.unscaledTime;
            state = State.Building;
            _builder = MapEditorSession.BuildAsync(make, (s, e) => { _buildError = e; _built = s; });
        }

        public void BuildingGUI(GUIStyle label)
        {
            float t = Time.unscaledTime - _buildStarted;
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 40, 400, 30), $"{T("map_editor")}: {_buildWhat}… {t:0}s", label);
        }

        public void Update()
        {
            if (state == State.Building)
            {
                if (_builder != null && !_builder.IsAlive)
                {
                    _builder = null;
                    if (_buildError != null || _built == null)
                    {
                        var e = _buildError; while (e?.InnerException != null) e = e.InnerException;
                        Debug.LogException(_buildError ?? new System.Exception("no map"));
                        _menuError = T("load_failed", e?.Message ?? "?");
                        state = State.Menu;
                    }
                    else StartEditing(_built);
                }
                return;
            }
            if (state != State.Editing) return;
            EditingUpdate();
        }

        // ================================================================== editing

        void StartEditing(MapEditorSession s)
        {
            _s = s;
            _root = new GameObject("Map Editor");
            _terrain = new FaithfulTerrain(s.info);
            _terrain.Build(s.info, _catalog.terrainMaterial, _catalog.waterMaterial, _root.transform);
            var entities = new GameObject("Entities").transform;
            entities.SetParent(_root.transform, false);
            _view = new FaithfulView(_catalog, entities);
            var gen = s.settings.createGenerator();
            TTEnvironment.Create(_root.transform, _cam, sun, _terrain, gen.getTerrainType(), gen.getFogInfo());

            s.HeightsChanged += (x0, y0, x1, y1) => { _hx0 = Mathf.Min(_hx0, x0); _hy0 = Mathf.Min(_hy0, y0); _hx1 = Mathf.Max(_hx1, x1); _hy1 = Mathf.Max(_hy1, y1); };
            s.AlphasChanged += (x0, y0, x1, y1, lo, hi) => _terrain.SetAlphas(x0, y0, x1, y1, lo, hi);
            s.Info += m => Message(m);
            if (s.NeedsGroundRebuild) s.RebuildGround();   // saved heights / resources: the generated ground texture doesn't fit them

            _rts = _cam.GetComponent<RtsCamera>();
            if (_rts == null) _rts = _cam.gameObject.AddComponent<RtsCamera>();
            _rts.enabled = true;
            _rts.world = null;
            _rts.letterKeys = true;
            _rts.reserveModifierWheel = true;
            _rts.mapMode = false;
            _rts.heightAt = (x, z) => _s.HeightAt(x, z);
            _rts.mapMeters = _s.MetersPerWorld;
            _rts.seaLevel = _s.SeaLevel;
            _rts.distance = 90f;
            _rts.yaw = 0f;
            _rts.pitchOffset = 0f;
            float c = _s.MetersPerWorld / 2f;
            _rts.LookAt(new Vector2(c, c * .75f));

            var lineMat = new Material(Shader.Find("Hidden/Internal-Colored") ?? Shader.Find("Sprites/Default"));
            lineMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            lineMat.SetInt("_ZWrite", 0);
            _outline = NewLine("Brush", lineMat);
            _rampLine = NewLine("Ramp", lineMat);
            _saveName = s.mapName ?? "";
            _menuOpen = _saveOpen = _exitConfirm = false;
            state = State.Editing;
            Message(T("hint_controls"));
        }

        LineRenderer NewLine(string name, Material mat)
        {
            var go = new GameObject(name);
            go.transform.SetParent(_root.transform, false);
            var lr = go.AddComponent<LineRenderer>();
            lr.sharedMaterial = mat;
            lr.widthMultiplier = 0.35f;
            lr.useWorldSpace = true;
            lr.loop = false;
            lr.positionCount = 0;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows = false;
            return lr;
        }

        void Message(string text) { _messages.Add((text, Time.unscaledTime + 5f)); if (_messages.Count > 5) _messages.RemoveAt(0); }

        bool DialogOpen => _menuOpen || _saveOpen || _exitConfirm;

        bool OverGui()
        {
            var m = Input.mousePosition;
            var p = new Vector2(m.x, Screen.height - m.y);
            foreach (var r in _guiRects) if (r.Contains(p)) return true;
            return false;
        }

        void EditingUpdate()
        {
            bool dialog = DialogOpen;
            _rts.enabled = !dialog;
            bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (!dialog)
            {
                if (Input.GetKeyDown(KeyCode.Escape)) { _s.CancelStroke(); _menuOpen = true; }
                if (Input.GetKeyDown(KeyCode.Space)) { _s.CancelStroke(); _rts.mapMode = !_rts.mapMode; }
                if (ctrl && (Input.GetKeyDown(KeyCode.Y) || (shift && Input.GetKeyDown(KeyCode.Z)))) _s.Redo();
                else if (ctrl && Input.GetKeyDown(KeyCode.Z)) _s.Undo();
                float wheel = Input.mouseScrollDelta.y;
                if (Mathf.Abs(wheel) > 0.01f && !OverGui())
                {
                    if (ctrl) _s.ScaleRadius(wheel > 0 ? 1 : -1);
                    else if (shift) _s.StepIntensity(wheel > 0 ? 1 : -1);
                }
            }

            PickCursor();
            if (!dialog && !_rts.mapMode)
            {
                bool over = OverGui();
                if (Input.GetMouseButtonDown(0) && !over) _s.BeginStroke(1, _hasCursor, _cx, _cy);
                else if (Input.GetMouseButtonDown(1) && !over) _s.BeginStroke(-1, _hasCursor, _cx, _cy);
                if (_s.Stroking && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)) _s.EndStroke(_hasCursor, _cx, _cy);
            }
            else if (_s.Stroking) _s.CancelStroke();

            _s.Tick(Time.unscaledDeltaTime, _hasCursor, _cx, _cy);

            if (_hx0 <= _hx1)
            {
                _terrain.RebuildCells(_hx0, _hy0, _hx1, _hy1);
                _hx0 = _hy0 = int.MaxValue; _hx1 = _hy1 = int.MinValue;
                _depthDirty = true;
                _mapTimer = Mathf.Min(_mapTimer, 0f);
            }
            _depthTimer += Time.unscaledDeltaTime;
            if (_depthDirty && !_s.Stroking && _depthTimer > 0.3f) { _depthDirty = false; _depthTimer = 0f; _terrain.RefreshWaterDepth(); }
            _mapTimer += Time.unscaledDeltaTime;
            if (_showMap && (_s.AccessChanged || _mapTex == null) && _mapTimer > 0.25f) { _s.AccessChanged = false; _mapTimer = 0f; RedrawMap(); }
        }

        /// <summary>The ground under the mouse; during a stroke the ray is followed to the stroke's starting level (Java pickCursor).</summary>
        void PickCursor()
        {
            _hasCursor = false;
            if (_rts.mapMode) return;
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (_s.Stroking && !_s.brush.isDragShape() && !_s.brush.isPlacementBrush() && ray.direction.y < -1e-4f)
            {
                float t = (_s.StrokeZ - ray.origin.y) / ray.direction.y;
                if (t > 0)
                {
                    var p = ray.origin + ray.direction * t;
                    if (p.x >= 0 && p.z >= 0 && p.x <= _s.MetersPerWorld && p.z <= _s.MetersPerWorld) { _hasCursor = true; _cx = p.x; _cy = p.z; return; }
                }
            }
            float best = float.MaxValue;
            foreach (var hit in Physics.RaycastAll(ray, 6000f))
                if (hit.collider.transform.parent == _terrain.root.transform && hit.distance < best)
                {
                    best = hit.distance; _hasCursor = true; _cx = hit.point.x; _cy = hit.point.z;
                }
        }

        public void LateUpdate()
        {
            if (state != State.Editing) return;
            _view.Render(_s.world, _cam);
            DrawOutline();
            DrawMarkers();
        }

        void DrawOutline()
        {
            bool show = _hasCursor && !_rts.mapMode && !_s.brush.isPlacementBrush();
            Color col = _s.Stroking ? new Color(1f, .85f, .3f) : new Color(1f, 1f, 1f, .9f);
            if (!show) _outline.positionCount = 0;
            else
            {
                var pts = new List<Vector3>(80);
                float r = _s.radius;
                var shape = _s.brush.isResourceBrush() || _s.brush.isDragShape() ? TerrainEditor.Shape.CIRCLE : _s.shape;
                if (shape == TerrainEditor.Shape.CIRCLE)
                    for (int i = 0; i <= 64; i++) { float a = i / 64f * Mathf.PI * 2f; pts.Add(Ground(_cx + Mathf.Cos(a) * r, _cy + Mathf.Sin(a) * r)); }
                else
                {
                    Vector2[] corners = shape == TerrainEditor.Shape.SQUARE
                        ? new[] { new Vector2(-r, -r), new Vector2(r, -r), new Vector2(r, r), new Vector2(-r, r), new Vector2(-r, -r) }
                        : new[] { new Vector2(0, -r), new Vector2(r, 0), new Vector2(0, r), new Vector2(-r, 0), new Vector2(0, -r) };
                    for (int c = 0; c < 4; c++)
                        for (int i = 0; i < 16; i++) { var p = Vector2.Lerp(corners[c], corners[c + 1], i / 16f); pts.Add(Ground(_cx + p.x, _cy + p.y)); }
                    pts.Add(pts[0]);
                }
                _outline.positionCount = pts.Count;
                _outline.SetPositions(pts.ToArray());
                _outline.startColor = _outline.endColor = col;
                _outline.sharedMaterial.color = col;
            }
            if (_s.RampPending(out float rx, out float ry) && _hasCursor)
            {
                var pts = new Vector3[17];
                for (int i = 0; i <= 16; i++) { float t = i / 16f; pts[i] = Ground(Mathf.Lerp(rx, _cx, t), Mathf.Lerp(ry, _cy, t)); }
                _rampLine.positionCount = pts.Length;
                _rampLine.SetPositions(pts);
            }
            else _rampLine.positionCount = 0;
        }

        Vector3 Ground(float x, float y) => new Vector3(x, Mathf.Max(_s.HeightAt(x, y), _s.SeaLevel) + 0.4f, y);

        void DrawMarkers()
        {
            int n = 0;
            foreach (var st in _s.Starts())
            {
                if (n >= _markers.Count)
                {
                    var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    Object.Destroy(go.GetComponent<Collider>());
                    go.name = "Start";
                    go.transform.SetParent(_root.transform, false);
                    go.transform.localScale = new Vector3(2f, 6f, 2f);
                    go.GetComponent<Renderer>().material = new Material(Shader.Find("Hidden/Internal-Colored") ?? Shader.Find("Sprites/Default"));
                    _markers.Add(go);
                }
                var m = _markers[n++];
                m.SetActive(true);
                m.transform.position = new Vector3(st.x, Mathf.Max(_s.HeightAt(st.x, st.y), _s.SeaLevel) + 6f, st.y);
                var c = FaithfulMatch.TeamColour(st.player);
                m.GetComponent<Renderer>().material.color = new Color(c.x(), c.y(), c.z());
            }
            for (int i = n; i < _markers.Count; i++) _markers[i].SetActive(false);
        }

        void RedrawMap()
        {
            int size = _s.size;
            int n = Mathf.Min(size, 256);
            if (_mapTex == null || _mapTex.width != n) _mapTex = new Texture2D(n, n, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[n * n];
            float sea = _s.SeaLevel;
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    int gx = x * size / n, gy = y * size / n;
                    float h = _s.heights[gy][gx];
                    int kind = _s.AccessKind(gx, gy);
                    Color32 c = kind switch
                    {
                        1 => new Color32(70, 160, 60, 255),
                        2 => new Color32(220, 140, 40, 255),
                        3 => new Color32(190, 50, 40, 255),
                        _ => h < sea ? new Color32(30, 70, 120, 255) : new Color32(120, 110, 90, 255),
                    };
                    px[y * n + x] = c;
                }
            _mapTex.SetPixels32(px);
            _mapTex.Apply(false);
        }

        // ================================================================== editor GUI

        public void OnGUI()
        {
            if (state != State.Editing) return;
            _guiRects.Clear();
            ToolbarGUI();
            if (_showMap) MapGUI();
            var e = Event.current;
            // start labels
            foreach (var st in _s.Starts())
            {
                var sp = _cam.WorldToScreenPoint(new Vector3(st.x, Mathf.Max(_s.HeightAt(st.x, st.y), _s.SeaLevel) + 13f, st.y));
                if (sp.z <= 0) continue;
                string txt = "P" + (st.player + 1) + (st.team >= 0 ? " T" + (st.team + 1) : "");
                GUI.Label(new Rect(sp.x - 30, Screen.height - sp.y - 10, 60, 20), "<b>" + txt + "</b>", new GUIStyle(_label) { alignment = TextAnchor.MiddleCenter, wordWrap = false });
            }
            // messages
            float y = Screen.height - 30;
            for (int i = _messages.Count - 1; i >= 0; i--)
            {
                if (_messages[i].until < Time.unscaledTime) { _messages.RemoveAt(i); continue; }
                GUI.Label(new Rect(12, y, Screen.width * .6f, 24), _messages[i].text, _label);
                y -= 22;
            }
            if (_menuOpen) MenuDialog();
            if (_saveOpen) SaveDialog();
            if (_exitConfirm) ExitDialog();
        }

        void ToolbarGUI()
        {
            float w = Mathf.Min(Screen.width - 20, 1180);
            _toolbarRect = new Rect((Screen.width - w) / 2, 6, w, 148);
            _guiRects.Add(_toolbarRect);
            GUI.Box(_toolbarRect, GUIContent.none);
            GUILayout.BeginArea(new Rect(_toolbarRect.x + 8, _toolbarRect.y + 6, _toolbarRect.width - 16, _toolbarRect.height - 10));
            GUILayout.BeginHorizontal();
            GUILayout.Label(T("tool_terrain") is var tt && tt != "tool_terrain" ? tt : "Terrain", _label, GUILayout.Width(70));
            int ti = System.Array.IndexOf(TerrainBrushes, _s.brush);
            int nti = GUILayout.Toolbar(ti, System.Array.ConvertAll(TerrainBrushes, b => _s.BrushName(b)));
            if (nti != ti && nti >= 0) _s.SelectBrush(TerrainBrushes[nti]);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(T("tool_resources") is var tr && tr != "tool_resources" ? tr : "Resources", _label, GUILayout.Width(70));
            int ri = System.Array.IndexOf(ResourceBrushes, _s.brush);
            int nri = GUILayout.Toolbar(ri, System.Array.ConvertAll(ResourceBrushes, b => _s.BrushName(b)));
            if (nri != ri && nri >= 0) _s.SelectBrush(ResourceBrushes[nri]);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(T("shape"), _label, GUILayout.Width(50));
            var shapes = TerrainEditor.Shape.values();
            int si = System.Array.IndexOf(shapes, _s.shape);
            int nsi = GUILayout.Toolbar(si, System.Array.ConvertAll(shapes, x => T("shape_" + x.name().ToLowerInvariant())), GUILayout.Width(220));
            if (nsi != si) _s.SetShape(shapes[nsi]);
            GUILayout.Space(12);
            GUILayout.Label(T("radius", Mathf.RoundToInt(_s.radius)), _label, GUILayout.Width(100));
            _s.radius = GUILayout.HorizontalSlider(_s.radius, 4f, 96f, GUILayout.Width(110));
            GUILayout.Space(8);
            GUILayout.Label(T("intensity", _s.intensity), _label, GUILayout.Width(110));
            _s.intensity = Mathf.RoundToInt(GUILayout.HorizontalSlider(_s.intensity, 0, 100, GUILayout.Width(110)));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(T("start_label") is var sl && sl != "start_label" ? sl : "Start:", _label, GUILayout.Width(50));
            if (GUILayout.Button("<", GUILayout.Width(24))) _s.startPlayer = (_s.startPlayer + MapEditorSession.MaxPlayers - 1) % MapEditorSession.MaxPlayers;
            GUILayout.Box(T("start_player", _s.startPlayer + 1), GUILayout.Width(90));
            if (GUILayout.Button(">", GUILayout.Width(24))) _s.startPlayer = (_s.startPlayer + 1) % MapEditorSession.MaxPlayers;
            if (GUILayout.Button("<", GUILayout.Width(24))) _s.startTeam = _s.startTeam <= -1 ? MapEditorSession.MaxPlayers - 1 : _s.startTeam - 1;
            GUILayout.Box(_s.startTeam < 0 ? T("no_team") : T("team_n", _s.startTeam + 1), GUILayout.Width(80));
            if (GUILayout.Button(">", GUILayout.Width(24))) _s.startTeam = _s.startTeam >= MapEditorSession.MaxPlayers - 1 ? -1 : _s.startTeam + 1;
            GUILayout.Space(12);
            if (GUILayout.Button("-", GUILayout.Width(24))) _autoCount = Mathf.Max(2, _autoCount - 1);
            GUILayout.Box(T("players_n", _autoCount), GUILayout.Width(80));
            if (GUILayout.Button("+", GUILayout.Width(24))) _autoCount = Mathf.Min(MapEditorSession.MaxPlayers, _autoCount + 1);
            if (GUILayout.Button(T("auto_place"), GUILayout.Width(130))) _s.AutoPlaceStarts(_autoCount);
            GUILayout.Space(12);
            if (_view != null) _view.drawTrees = GUILayout.Toggle(_view.drawTrees, " " + T("show_trees"), GUILayout.Width(70));
            _showMap = GUILayout.Toggle(_showMap, " " + T("show_access"), GUILayout.Width(120));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(T("undo"), GUILayout.Width(60))) _s.Undo();
            if (GUILayout.Button(T("redo"), GUILayout.Width(60))) _s.Redo();
            if (GUILayout.Button(T("menu"), GUILayout.Width(70))) { _s.CancelStroke(); _menuOpen = true; }
            GUILayout.EndHorizontal();
            string name = _s.mapName != null ? T("current_map", _s.mapName) + (_s.IsModified ? " *" : "") + "   " : (_s.IsModified ? "* " : "");
            GUILayout.Label("<color=#ffe08a>" + name + "</color>" + _s.brush.getHint() + "    <color=#aaaaaa>" + T("hint_controls") + "  |  Space: map</color>", _label);
            GUILayout.EndArea();
        }

        void MapGUI()
        {
            if (_mapTex == null) return;
            float side = Mathf.Min(220f, Screen.height * .3f);
            var r = new Rect(Screen.width - side - 12, Screen.height - side - 40, side, side);
            _guiRects.Add(new Rect(r.x, r.y - 20, r.width, r.height + 20));
            GUI.Box(new Rect(r.x - 4, r.y - 22, r.width + 8, r.height + 26), GUIContent.none);
            GUI.Label(new Rect(r.x, r.y - 21, r.width, 20), "<size=11>" + T("show_access") + "  <color=#7c7>■</color> <color=#e93>■</color> <color=#c44>■</color></size>", _label);
            // texture rows run bottom up = +y north, like the 3D view seen from the south
            GUI.DrawTexture(r, _mapTex);
            float m = _s.MetersPerWorld;
            foreach (var st in _s.Starts())
            {
                var c = FaithfulMatch.TeamColour(st.player);
                var p = new Vector2(r.x + st.x / m * r.width, r.yMax - st.y / m * r.height);
                var old = GUI.color; GUI.color = new Color(c.x(), c.y(), c.z());
                GUI.DrawTexture(new Rect(p.x - 4, p.y - 4, 8, 8), Texture2D.whiteTexture);
                GUI.color = old;
            }
            if (_hasCursor)
            {
                var p = new Vector2(r.x + _cx / m * r.width, r.yMax - _cy / m * r.height);
                GUI.DrawTexture(new Rect(p.x - 1, p.y - 1, 3, 3), Texture2D.whiteTexture);
            }
            // click to look there
            var e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && r.Contains(e.mousePosition))
            {
                _rts.LookAt(new Vector2((e.mousePosition.x - r.x) / r.width * m, (r.yMax - e.mousePosition.y) / r.height * m));
                _rts.mapMode = false;
                e.Use();
            }
        }

        Rect Dialog(float w, float h)
        {
            var r = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
            var old = GUI.color; GUI.color = new Color(0, 0, 0, .5f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = old;
            GUI.Box(r, GUIContent.none);
            GUI.Box(r, GUIContent.none);
            _guiRects.Add(new Rect(0, 0, Screen.width, Screen.height));
            return new Rect(r.x + 14, r.y + 12, r.width - 28, r.height - 24);
        }

        void MenuDialog()
        {
            GUILayout.BeginArea(Dialog(320, 250));
            GUILayout.Label("<b>" + T("headline") + "</b>", _label);
            if (GUILayout.Button(T("resume"), GUILayout.Height(32)) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)) _menuOpen = false;
            if (GUILayout.Button(T("save"), GUILayout.Height(30))) { _menuOpen = false; _saveOpen = true; _playAfterSave = false; _saveError = null; _overwriteAsk = false; }
            if (GUILayout.Button(T("play_button") + " (" + T("save").TrimEnd('.') + " first)", GUILayout.Height(30))) { _menuOpen = false; _saveOpen = true; _playAfterSave = true; _saveError = null; _overwriteAsk = false; }
            if (GUILayout.Button(T("exit_editor"), GUILayout.Height(30)))
            {
                _menuOpen = false;
                if (_s.IsModified) _exitConfirm = true; else Close(null);
            }
            GUILayout.Label("<color=#aaaaaa>Map code: " + _s.MapCode + "</color>", _label);
            GUILayout.EndArea();
        }

        void SaveDialog()
        {
            GUILayout.BeginArea(Dialog(420, 200));
            GUILayout.Label("<b>" + T("save_caption") + "</b>", _label);
            GUILayout.BeginHorizontal();
            GUILayout.Label(T("map_name"), _label, GUILayout.Width(90));
            GUI.SetNextControlName("tt.mapname");
            string n = GUILayout.TextField(_saveName, 48);
            if (n != _saveName) { _saveName = n; _overwriteAsk = false; _saveError = null; }
            GUILayout.EndHorizontal();
            if (GUI.GetNameOfFocusedControl() != "tt.mapname") GUI.FocusControl("tt.mapname");
            string name = _saveName.Trim();
            bool valid = MapEditorSession.IsValidName(name);
            if (_overwriteAsk) GUILayout.Label("<color=#ffd080>" + T("overwrite_confirm", name) + "</color>", _label);
            if (_saveError != null) GUILayout.Label("<color=#ff8080>" + _saveError + "</color>", _label);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUI.enabled = valid;
            bool enter = Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter);
            if (GUILayout.Button(_overwriteAsk ? "Overwrite" : T("save").TrimEnd('.'), GUILayout.Height(30)) || (enter && valid))
            {
                string path = System.IO.Path.Combine(MapEditorSession.MapsDir, name + MapFile.EXTENSION);
                if (System.IO.File.Exists(path) && name != _s.mapName && !_overwriteAsk) _overwriteAsk = true;
                else
                {
                    try
                    {
                        string saved = _s.Save(name);
                        _saveOpen = false;
                        _entries = null;
                        _previews.Remove(saved);
                        if (_playAfterSave) Close(saved);
                    }
                    catch (System.Exception ex) { var x = ex; while (x.InnerException != null) x = x.InnerException; _saveError = T("save_failed", x.Message); }
                }
            }
            GUI.enabled = true;
            if (GUILayout.Button("Cancel", GUILayout.Height(30)) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)) _saveOpen = false;
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void ExitDialog()
        {
            GUILayout.BeginArea(Dialog(380, 150));
            GUILayout.Label(T("exit_confirm"), _label);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(T("exit_editor"), GUILayout.Height(30))) { _exitConfirm = false; Close(null); }
            if (GUILayout.Button("Cancel", GUILayout.Height(30))) _exitConfirm = false;
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        /// <summary>Leaves the editor (back to the menu, or to play the given map).</summary>
        public void Close(string playPath)
        {
            if (state == State.Editing)
            {
                _view?.Clear();
                if (_root != null) Object.Destroy(_root);
                _markers.Clear();
                if (_rts != null) { _rts.reserveModifierWheel = false; _rts.mapMode = false; _rts.enabled = false; }
            }
            _root = null; _view = null; _terrain = null; _s = null; _built = null;
            _entries = null;
            state = State.Menu;
            onExit?.Invoke(playPath);
        }
    }
}
