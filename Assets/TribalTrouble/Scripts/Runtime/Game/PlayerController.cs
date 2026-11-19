using System.Collections.Generic;
using TribalTrouble.Presentation;
using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Human input and HUD (IMGUI): box / click selection, right-click orders, building placement,
    /// building panels (quarters, armory, tower), minimap, control groups, notifications, game over.
    /// </summary>
    public sealed class PlayerController : MonoBehaviour
    {
        public GameSim sim;
        public GameView view;
        public Player player;
        public RtsCamera rtsCamera;
        public Camera cam;
        public SfxPlayer sfx;
        public TTSpriteCatalog catalog;
        public System.Action onQuit;
        public System.Action onRestart;
        public float gameSpeed = 1f;
        public bool paused;

        // selection
        readonly List<Unit> _units = new List<Unit>();
        Building _building;
        bool _dragging;
        Vector2 _dragStart;      // screen (bottom-left origin)
        float _lastClickTime;
        Entity _lastClicked;
        readonly List<Entity>[] _groups = new List<Entity>[10];

        // placement
        BuildingType? _placing;
        Vector2Int _placeCell;
        bool _placeOk;
        GameObject _ghost;
        Mesh _quad;
        Material _okMat, _badMat;

        // hud
        Texture2D _minimap;
        Texture2D _white;
        Rect _minimapRect;
        Rect _panelRect;
        readonly List<(string text, float until)> _messages = new List<(string, float)>();
        GUIStyle _label, _title, _small, _center;
        bool _showMenu;
        bool _showHelp = true;

        const float PanelHeight = 150f;
        const float MiniSize = 180f;

        public void Init()
        {
            sim.Notify += OnNotify;
            sim.Sound += (name, pos) => sfx?.Play(name, sim.world.ToWorld(pos));
            BuildMinimap();
            BuildQuad();
            _white = Texture2D.whiteTexture;
            AddMessage("Gather wood, rock and iron. Build quarters to grow, an armory to arm your warriors.", 10f);
            Invoke(nameof(HideHelp), 25f);
        }

        void HideHelp() => _showHelp = false;

        void OnDestroy()
        {
            if (_ghost != null) Destroy(_ghost);
            if (_minimap != null) Destroy(_minimap);
        }

        void OnNotify(Player p, string what)
        {
            if (what == "defeated")
            {
                AddMessage(p == player ? "You have been defeated." : p.name + " has been defeated!", 8f);
                return;
            }
            if (p != player) return;
            if (what == "attack")
                sfx?.Notify(player.race == Race.Vikings ? "attacknotify_viking" : "attacknotify_native", Time.unscaledTime);
        }

        public void AddMessage(string text, float seconds = 5f)
        {
            _messages.Add((text, Time.unscaledTime + seconds));
            if (_messages.Count > 6) _messages.RemoveAt(0);
        }

        // ================================================================== input

        void Update()
        {
            if (sim == null || cam == null) return;
            PruneSelection();
            HandleKeys();
            if (_showMenu || sim.over) { _dragging = false; return; }

            var mouse = (Vector2)Input.mousePosition;
            bool overHud = OverHud(mouse);

            if (_placing.HasValue)
            {
                UpdatePlacement(mouse, overHud);
                return;
            }

            if (Input.GetMouseButtonDown(0) && !overHud)
            {
                _dragging = true;
                _dragStart = mouse;
            }
            if (_dragging && Input.GetMouseButtonUp(0))
            {
                _dragging = false;
                if ((mouse - _dragStart).sqrMagnitude > 64f) BoxSelect(_dragStart, mouse);
                else ClickSelect(mouse);
            }
            if (Input.GetMouseButtonDown(1) && !overHud) IssueSmart(mouse);
            if (Input.GetMouseButtonDown(0) && overHud && _minimapRect.Contains(GuiPoint(mouse))) MinimapJump(mouse);
            if (Input.GetMouseButton(0) && !_dragging && _minimapRect.Contains(GuiPoint(mouse))) MinimapJump(mouse);
            if (Input.GetMouseButtonDown(1) && _minimapRect.Contains(GuiPoint(mouse)) && _units.Count > 0)
            {
                sim.CommandMove(_units, MinimapToMeters(mouse), true);
                sfx?.PlayUi("click");
            }
        }

        void HandleKeys()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_placing.HasValue) CancelPlacement();
                else if (_units.Count > 0 || _building != null) ClearSelection();
                else _showMenu = !_showMenu;
            }
            if (Input.GetKeyDown(KeyCode.F1)) _showHelp = !_showHelp;
            if (Input.GetKeyDown(KeyCode.Pause) || Input.GetKeyDown(KeyCode.P)) paused = !paused;
            if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals)) gameSpeed = Mathf.Min(4f, gameSpeed * 2f);
            if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus)) gameSpeed = Mathf.Max(0.25f, gameSpeed * 0.5f);
            if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.Home)) FocusHome();
            if (_showMenu || sim.over) return;

            bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            for (int i = 0; i <= 9; i++)
            {
                if (!Input.GetKeyDown(KeyCode.Alpha0 + i)) continue;
                if (ctrl)
                {
                    _groups[i] = new List<Entity>();
                    _groups[i].AddRange(_units);
                    if (_building != null) _groups[i].Add(_building);
                    AddMessage("Group " + i + " set", 2f);
                }
                else if (_groups[i] != null) SelectGroup(_groups[i], DoubleTap(i));
            }

            bool peons = _units.Exists(u => u.type == UnitType.Peon);
            if (peons)
            {
                if (Input.GetKeyDown(KeyCode.C)) StartPlacement(BuildingType.Quarters);
                if (Input.GetKeyDown(KeyCode.R)) StartPlacement(BuildingType.Armory);
                if (Input.GetKeyDown(KeyCode.T)) StartPlacement(BuildingType.Tower);
            }
            if (Input.GetKeyDown(KeyCode.X) && _units.Count > 0)
            {
                foreach (var u in _units) sim.CommandMove(new[] { u }, u.pos);   // stop
            }
            if (Input.GetKeyDown(KeyCode.Delete) && _building != null) sim.EjectAll(_building);
            if (Input.GetKeyDown(KeyCode.Period)) SelectIdlePeons();
        }

        float _lastGroupTap; int _lastGroup = -1;
        bool DoubleTap(int g)
        {
            bool d = g == _lastGroup && Time.unscaledTime - _lastGroupTap < 0.35f;
            _lastGroup = g; _lastGroupTap = Time.unscaledTime;
            return d;
        }

        void SelectGroup(List<Entity> group, bool focus)
        {
            ClearSelection();
            group.RemoveAll(e => e == null || !e.alive || (e is Unit u && u.state == UnitState.Dead));
            foreach (var e in group)
            {
                if (e is Unit u && sim.units.Contains(u)) _units.Add(u);
                else if (e is Building b && _units.Count == 0) _building = b;
            }
            SyncViewSelection();
            if (focus)
            {
                var c = Center();
                if (c.HasValue) rtsCamera.LookAt(c.Value);
            }
        }

        Vector2? Center()
        {
            if (_units.Count > 0)
            {
                Vector2 s = Vector2.zero;
                foreach (var u in _units) s += u.pos;
                return s / _units.Count;
            }
            return _building != null ? _building.pos : (Vector2?)null;
        }

        public void FocusHome()
        {
            foreach (var b in player.buildings) { rtsCamera.LookAt(b.pos); return; }
            rtsCamera.LookAt(player.units.Count > 0 ? player.units[0].pos : player.start);
        }

        void SelectIdlePeons()
        {
            ClearSelection();
            foreach (var u in player.units)
                if (u.type == UnitType.Peon && u.state == UnitState.Idle) _units.Add(u);
            SyncViewSelection();
            if (_units.Count > 0) rtsCamera.LookAt(_units[0].pos);
            else AddMessage("No idle peons", 2f);
        }

        // ---------------------------------------------------------------- selection

        void PruneSelection()
        {
            bool changed = _units.RemoveAll(u => !u.alive || u.state == UnitState.Dead || !sim.units.Contains(u)) > 0;
            if (_building != null && !_building.alive) { _building = null; changed = true; }
            if (changed) SyncViewSelection();
        }

        void ClearSelection()
        {
            _units.Clear();
            _building = null;
            SyncViewSelection();
        }

        void SyncViewSelection()
        {
            view.selected.Clear();
            foreach (var u in _units) view.selected.Add(u);
            if (_building != null) view.selected.Add(_building);
        }

        bool Additive => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        void BoxSelect(Vector2 a, Vector2 b)
        {
            var r = Rect.MinMaxRect(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
            if (!Additive) ClearSelection();
            _building = null;
            bool anyWarrior = false;
            var picked = new List<Unit>();
            foreach (var u in player.units)
            {
                if (u.state == UnitState.Dead) continue;
                var sp = cam.WorldToScreenPoint(sim.world.ToWorld(u.pos) + Vector3.up);
                if (sp.z > 0 && r.Contains(sp)) { picked.Add(u); anyWarrior |= u.IsWarrior; }
            }
            // like most RTS games: a box over an army ignores the peons in it
            foreach (var u in picked)
                if (!anyWarrior || u.IsWarrior)
                    if (!_units.Contains(u)) _units.Add(u);
            SyncViewSelection();
        }

        void ClickSelect(Vector2 mouse)
        {
            var e = PickEntity(mouse);
            bool dbl = e != null && e == _lastClicked && Time.unscaledTime - _lastClickTime < 0.35f;
            _lastClicked = e; _lastClickTime = Time.unscaledTime;
            if (e == null)
            {
                if (!Additive) ClearSelection();
                return;
            }
            if (e is Unit u)
            {
                if (u.owner != player) { ClearSelection(); _building = null; return; }
                if (dbl)
                {
                    // double click: every unit of that type on screen
                    ClearSelection();
                    foreach (var o in player.units)
                    {
                        if (o.type != u.type || o.state == UnitState.Dead) continue;
                        var sp = cam.WorldToScreenPoint(sim.world.ToWorld(o.pos));
                        if (sp.z > 0 && sp.x >= 0 && sp.y >= 0 && sp.x <= Screen.width && sp.y <= Screen.height) _units.Add(o);
                    }
                }
                else if (Additive)
                {
                    if (!_units.Remove(u)) _units.Add(u);
                    _building = null;
                }
                else
                {
                    ClearSelection();
                    _units.Add(u);
                }
            }
            else if (e is Building b)
            {
                ClearSelection();
                _building = b;      // enemy buildings can be inspected too
            }
            SyncViewSelection();
            sfx?.PlayUi("click");
        }

        Entity PickEntity(Vector2 mouse)
        {
            Entity best = null;
            float bestD = float.MaxValue;
            foreach (var u in sim.units)
            {
                if (u.state == UnitState.Dead) continue;
                var sp = cam.WorldToScreenPoint(sim.world.ToWorld(u.pos) + Vector3.up * 0.9f);
                if (sp.z <= 0) continue;
                float pr = PixelRadius(u.pos, 1.1f, sp);
                float d = ((Vector2)sp - mouse).magnitude;
                if (d < Mathf.Max(pr, 10f) && d < bestD) { bestD = d; best = u; }
            }
            if (best != null) return best;
            foreach (var b in sim.buildings)
            {
                if (!b.alive) continue;
                var sp = cam.WorldToScreenPoint(sim.world.ToWorld(b.pos) + Vector3.up * 2f);
                if (sp.z <= 0) continue;
                float pr = PixelRadius(b.pos, b.Radius, sp);
                float d = ((Vector2)sp - mouse).magnitude;
                if (d < pr && d < bestD) { bestD = d; best = b; }
            }
            return best;
        }

        float PixelRadius(Vector2 pos, float worldRadius, Vector3 screenCenter)
        {
            var edge = cam.WorldToScreenPoint(sim.world.ToWorld(pos) + cam.transform.right * worldRadius + Vector3.up);
            return Mathf.Abs(edge.x - screenCenter.x);
        }

        bool RaycastGround(Vector2 mouse, out Vector2 meters)
        {
            var ray = cam.ScreenPointToRay(mouse);
            if (Physics.Raycast(ray, out var hit, 5000f))
            {
                meters = new Vector2(hit.point.x, hit.point.z);
                return true;
            }
            // fall back to the sea plane
            var plane = new Plane(Vector3.up, new Vector3(0, sim.world.seaLevel, 0));
            if (plane.Raycast(ray, out float t))
            {
                var p = ray.GetPoint(t);
                meters = new Vector2(p.x, p.z);
                return true;
            }
            meters = default;
            return false;
        }

        Supply PickSupply(Vector2 meters)
        {
            var c = GameWorld.CellOf(meters);
            Supply best = null;
            float bestD = float.MaxValue;
            for (int dy = -2; dy <= 2; dy++)
                for (int dx = -2; dx <= 2; dx++)
                {
                    int x = c.x + dx, y = c.y + dy;
                    if (!sim.world.InBounds(x, y)) continue;
                    var s = sim.world.supplyAt[x, y];
                    if (s == null || s.Depleted) continue;
                    float d = (GameWorld.CellCenter(x, y) - meters).sqrMagnitude;
                    if (d < bestD) { bestD = d; best = s; }
                }
            return best;
        }

        void IssueSmart(Vector2 mouse)
        {
            if (_units.Count == 0)
            {
                // right-click with a building selected: nothing (rally points come later)
                return;
            }
            var e = PickEntity(mouse);
            if (!RaycastGround(mouse, out var point))
            {
                if (e == null) return;
                point = e.pos;
            }
            Supply s = e == null ? PickSupply(point) : null;
            sim.CommandSmart(player, new List<Unit>(_units), point, e, s);
            sfx?.PlayUi("click");
        }

        // ---------------------------------------------------------------- placement

        void StartPlacement(BuildingType type)
        {
            CancelPlacement();
            _placing = type;
            string group = player.race == Race.Vikings ? "vikings" : "natives";
            string name = type == BuildingType.Quarters ? "quarters" : type == BuildingType.Armory ? "armory" : "tower";
            var def = catalog != null ? catalog.Find(group, name) : null;
            if (def != null)
            {
                _ghost = new GameObject("Placement ghost");
                var v = _ghost.AddComponent<TTSpriteView>();
                v.Init(def, catalog.baseMaterial, player.color);
                foreach (var c in _ghost.GetComponentsInChildren<Collider>()) Destroy(c);
                foreach (var r in _ghost.GetComponentsInChildren<Renderer>()) r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }

        void CancelPlacement()
        {
            _placing = null;
            if (_ghost != null) Destroy(_ghost);
            _ghost = null;
        }

        void UpdatePlacement(Vector2 mouse, bool overHud)
        {
            var type = _placing.Value;
            int size = Rules.Size(type);
            if (RaycastGround(mouse, out var m))
            {
                _placeCell = GameWorld.CellOf(m);
                _placeOk = sim.world.CanPlaceBuilding(_placeCell.x, _placeCell.y, size);
                var center = sim.world.ToWorld(GameWorld.CellCenter(_placeCell.x, _placeCell.y));
                if (_ghost != null)
                {
                    _ghost.SetActive(!overHud);
                    _ghost.transform.position = center;
                    _ghost.transform.rotation = Quaternion.Euler(0, GameView.ModelYawOffset, 0);
                }
                float r = (GameWorld.FootprintRadius(size) + 0.5f) * GameWorld.MetersPerCell;
                var rp = new RenderParams(_placeOk ? _okMat : _badMat);
                if (!overHud) Graphics.RenderMesh(rp, _quad, 0, Matrix4x4.TRS(center + Vector3.up * 0.2f, Quaternion.identity, new Vector3(r * 2, 1, r * 2)));
            }
            if (Input.GetMouseButtonDown(1)) { CancelPlacement(); return; }
            if (Input.GetMouseButtonDown(0) && !overHud)
            {
                if (!_placeOk) { AddMessage("You can't build there", 2f); return; }
                var b = sim.PlaceBuilding(player, type, _placeCell);
                if (b != null)
                {
                    var builders = _units.FindAll(u => u.type == UnitType.Peon);
                    sim.CommandBuild(builders, b);
                    sfx?.PlayUi("click");
                }
                if (!Additive) CancelPlacement();
            }
        }

        void BuildQuad()
        {
            _quad = new Mesh { name = "Footprint" };
            _quad.vertices = new[] { new Vector3(-.5f, 0, -.5f), new Vector3(-.5f, 0, .5f), new Vector3(.5f, 0, -.5f), new Vector3(.5f, 0, .5f) };
            _quad.triangles = new[] { 0, 1, 2, 2, 1, 3 };
            _quad.RecalculateBounds();
            var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            _okMat = MakeTransparent(shader, new Color(0.2f, 1f, 0.2f, 0.35f));
            _badMat = MakeTransparent(shader, new Color(1f, 0.15f, 0.1f, 0.35f));
        }

        static Material MakeTransparent(Shader shader, Color c)
        {
            var m = new Material(shader);
            m.SetFloat("_Surface", 1f);
            m.SetFloat("_Blend", 0f);
            m.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetFloat("_ZWrite", 0f);
            m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            m.SetColor("_BaseColor", c);
            return m;
        }

        // ================================================================== minimap

        void BuildMinimap()
        {
            var w = sim.world;
            const int res = 256;
            _minimap = new Texture2D(res, res, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };
            var px = new Color32[res * res];
            for (int y = 0; y < res; y++)
                for (int x = 0; x < res; x++)
                {
                    int gx = x * w.size / res, gy = y * w.size / res;
                    float h = w.heights[gy][gx] - w.seaLevel;
                    Color c;
                    if (h < 0) c = Color.Lerp(new Color(0.25f, 0.55f, 0.7f), new Color(0.05f, 0.15f, 0.35f), Mathf.Clamp01(-h / 8f));
                    else if (h < 1.2f) c = new Color(0.85f, 0.78f, 0.55f);
                    else if (!w.walkable[gx, gy]) c = new Color(0.45f, 0.42f, 0.38f);
                    else c = Color.Lerp(new Color(0.3f, 0.55f, 0.2f), new Color(0.5f, 0.55f, 0.35f), Mathf.Clamp01(h / 30f));
                    var s = w.supplyAt[gx, gy];
                    if (s != null) c = s.kind == SupplyKind.Wood ? new Color(0.1f, 0.3f, 0.1f) : s.kind == SupplyKind.Rock ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.55f, 0.3f, 0.2f);
                    px[y * res + x] = c;
                }
            _minimap.SetPixels32(px);
            _minimap.Apply(false);
        }

        Vector2 MinimapToMeters(Vector2 mouse)
        {
            var g = GuiPoint(mouse);
            float u = (g.x - _minimapRect.x) / _minimapRect.width;
            float v = 1f - (g.y - _minimapRect.y) / _minimapRect.height;
            return new Vector2(u * sim.world.meters, v * sim.world.meters);
        }

        void MinimapJump(Vector2 mouse) => rtsCamera.LookAt(MinimapToMeters(mouse));

        // ================================================================== HUD

        static Vector2 GuiPoint(Vector2 mouse) => new Vector2(mouse.x, Screen.height - mouse.y);

        bool OverHud(Vector2 mouse)
        {
            var g = GuiPoint(mouse);
            return g.y > Screen.height - PanelHeight || g.y < 30f || _minimapRect.Contains(g) || _showMenu;
        }

        void Styles()
        {
            if (_label != null) return;
            _label = new GUIStyle(GUI.skin.label) { fontSize = 14, normal = { textColor = Color.white } };
            _small = new GUIStyle(_label) { fontSize = 12 };
            _title = new GUIStyle(_label) { fontSize = 16, fontStyle = FontStyle.Bold };
            _center = new GUIStyle(_label) { alignment = TextAnchor.MiddleCenter, fontSize = 30, fontStyle = FontStyle.Bold };
        }

        void Box(Rect r, Color c)
        {
            var old = GUI.color;
            GUI.color = c;
            GUI.DrawTexture(r, _white);
            GUI.color = old;
        }

        void OnGUI()
        {
            if (sim == null) return;
            Styles();
            int W = Screen.width, H = Screen.height;

            // ---- top bar
            Box(new Rect(0, 0, W, 30), new Color(0, 0, 0, 0.6f));
            GUI.Label(new Rect(10, 5, 900, 22),
                $"Wood {player.wood}    Rock {player.rock}    Iron {player.iron}    |    Rock weapons {player.rockWeapons}  Iron weapons {player.ironWeapons}" +
                $"    |    Units {player.UnitCount}/{Rules.MaxUnits}", _label);
            int t = (int)sim.time;
            string speed = paused ? "PAUSED" : gameSpeed == 1f ? "" : "x" + gameSpeed;
            GUI.Label(new Rect(W - 260, 5, 250, 22), $"{speed}   {t / 60:00}:{t % 60:00}   [F1 help] [Esc menu]", _label);

            // ---- drag box
            if (_dragging)
            {
                var a = GuiPoint(_dragStart); var b = GuiPoint(Input.mousePosition);
                var r = Rect.MinMaxRect(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
                Box(r, new Color(0.4f, 1f, 0.4f, 0.15f));
                Box(new Rect(r.x, r.y, r.width, 1), Color.green); Box(new Rect(r.x, r.yMax, r.width, 1), Color.green);
                Box(new Rect(r.x, r.y, 1, r.height), Color.green); Box(new Rect(r.xMax, r.y, 1, r.height), Color.green);
            }

            DrawHealthBars();

            // ---- bottom panel
            _panelRect = new Rect(0, H - PanelHeight, W, PanelHeight);
            Box(_panelRect, new Color(0.08f, 0.06f, 0.04f, 0.85f));
            _minimapRect = new Rect(W - MiniSize - 10, H - MiniSize - 10, MiniSize, MiniSize);
            DrawMinimap();
            GUILayout.BeginArea(new Rect(10, H - PanelHeight + 8, W - MiniSize - 40, PanelHeight - 12));
            DrawSelectionPanel();
            GUILayout.EndArea();

            // ---- messages
            float my = 40;
            _messages.RemoveAll(m => m.until < Time.unscaledTime);
            foreach (var m in _messages) { GUI.Label(new Rect(12, my, 900, 22), m.text, _label); my += 22; }

            if (_showHelp) DrawHelp();
            if (sim.over) DrawGameOver();
            else if (_showMenu) DrawMenu();
        }

        void DrawHealthBars()
        {
            foreach (var b in sim.buildings)
            {
                if (!b.alive) continue;
                bool show = b.hp < b.maxHp || view.selected.Contains(b);
                if (!show) continue;
                var sp = cam.WorldToScreenPoint(sim.world.ToWorld(b.pos) + Vector3.up * 7f);
                if (sp.z <= 0) continue;
                var g = GuiPoint(sp);
                float w = 60f;
                Box(new Rect(g.x - w / 2 - 1, g.y - 1, w + 2, 7), Color.black);
                float f = Mathf.Clamp01(b.hp / b.maxHp);
                Box(new Rect(g.x - w / 2, g.y, w * f, 5), b.complete ? Color.Lerp(Color.red, Color.green, f) : new Color(1f, 0.8f, 0.2f));
            }
        }

        void DrawMinimap()
        {
            Box(new Rect(_minimapRect.x - 2, _minimapRect.y - 2, _minimapRect.width + 4, _minimapRect.height + 4), Color.black);
            // texture row 0 is the south edge (y = 0) -> draw flipped so north is up
            GUI.DrawTextureWithTexCoords(_minimapRect, _minimap, new Rect(0, 0, 1, 1));
            float s = _minimapRect.width / sim.world.meters;
            foreach (var b in sim.buildings)
            {
                if (!b.alive) continue;
                var p = MiniPoint(b.pos, s);
                Box(new Rect(p.x - 3, p.y - 3, 6, 6), b.owner.color);
            }
            foreach (var u in sim.units)
            {
                if (u.state == UnitState.Dead) continue;
                var p = MiniPoint(u.pos, s);
                Box(new Rect(p.x - 1, p.y - 1, 2, 2), u.owner.color);
            }
            // camera focus
            var f = MiniPoint(new Vector2(rtsCamera.focus.x, rtsCamera.focus.z), s);
            float vs = rtsCamera.distance * 0.8f * s;
            var c = new Color(1, 1, 1, 0.8f);
            Box(new Rect(f.x - vs, f.y - vs * .6f, vs * 2, 1), c); Box(new Rect(f.x - vs, f.y + vs * .6f, vs * 2, 1), c);
            Box(new Rect(f.x - vs, f.y - vs * .6f, 1, vs * 1.2f), c); Box(new Rect(f.x + vs, f.y - vs * .6f, 1, vs * 1.2f), c);
        }

        Vector2 MiniPoint(Vector2 m, float s) => new Vector2(_minimapRect.x + m.x * s, _minimapRect.yMax - m.y * s);

        void DrawSelectionPanel()
        {
            if (_units.Count > 0)
            {
                int peons = 0, rock = 0, iron = 0;
                foreach (var u in _units) { if (u.type == UnitType.Peon) peons++; else if (u.type == UnitType.WarriorRock) rock++; else iron++; }
                GUILayout.Label($"Selected: {_units.Count}   (peons {peons}, rock warriors {rock}, iron warriors {iron})", _title);
                if (_units.Count == 1)
                {
                    var u = _units[0];
                    GUILayout.Label($"{UnitName(u.type)} - {u.state}" + (u.carrying ? $" (carrying {u.carryKind})" : ""), _small);
                }
                if (peons > 0)
                {
                    GUILayout.Label("Build:", _small);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Quarters [C]", GUILayout.Width(130), GUILayout.Height(34))) StartPlacement(BuildingType.Quarters);
                    if (GUILayout.Button("Armory [R]", GUILayout.Width(130), GUILayout.Height(34))) StartPlacement(BuildingType.Armory);
                    if (GUILayout.Button("Tower [T]", GUILayout.Width(130), GUILayout.Height(34))) StartPlacement(BuildingType.Tower);
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Stop [X]", GUILayout.Width(100))) foreach (var u in _units) sim.CommandMove(new[] { u }, u.pos);
                if (_placing.HasValue) GUILayout.Label($"Placing {_placing.Value}: left-click to place (Shift = several), right-click / Esc to cancel", _small);
                GUILayout.EndHorizontal();
                return;
            }
            if (_building != null)
            {
                var b = _building;
                bool mine = b.owner == player;
                GUILayout.Label($"{(mine ? "" : b.owner.name + "'s ")}{b.type}  -  {Mathf.CeilToInt(b.hp)}/{b.maxHp} HP" + (b.complete ? "" : "  (under construction)"), _title);
                if (!mine || !b.complete) return;
                switch (b.type)
                {
                    case BuildingType.Quarters:
                    {
                        float rate = Rules.ReproductionPerSecond(b.inside.Count) * 60f;
                        GUILayout.Label($"Peons inside: {b.inside.Count}/{b.Capacity}   -   about {rate:0.0} new peons per minute (more peons inside = faster)", _small);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Deploy 1", GUILayout.Width(100), GUILayout.Height(30))) sim.DeployPeons(b, 1);
                        if (GUILayout.Button("Deploy 5", GUILayout.Width(100), GUILayout.Height(30))) sim.DeployPeons(b, 5);
                        if (GUILayout.Button("Deploy all [Del]", GUILayout.Width(130), GUILayout.Height(30))) sim.EjectAll(b);
                        GUILayout.EndHorizontal();
                        GUILayout.Label("Tip: right-click the quarters with peons selected to put them inside.", _small);
                        break;
                    }
                    case BuildingType.Armory:
                    {
                        GUILayout.Label($"Workers: {b.inside.Count}/{b.Capacity}    Weapon progress {(int)(b.weaponProgress * 100)}%    " +
                                        $"Queued: rock {player.orderedRock}, iron {player.orderedIron}    In stock: rock {player.rockWeapons}, iron {player.ironWeapons}", _small);
                        GUILayout.Label($"A weapon costs {Rules.WeaponWood} wood + {Rules.WeaponRockOrIron} rock (or iron). Peons deliver resources here.", _small);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Make rock weapon +1", GUILayout.Width(150))) sim.OrderWeapons(player, UnitType.WarriorRock, 1);
                        if (GUILayout.Button("+5", GUILayout.Width(40))) sim.OrderWeapons(player, UnitType.WarriorRock, 5);
                        if (GUILayout.Button("Make iron weapon +1", GUILayout.Width(150))) sim.OrderWeapons(player, UnitType.WarriorIron, 1);
                        if (GUILayout.Button("+5", GUILayout.Width(40))) sim.OrderWeapons(player, UnitType.WarriorIron, 5);
                        if (GUILayout.Button("Clear queue", GUILayout.Width(100))) { player.orderedRock = 0; player.orderedIron = 0; }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Deploy rock warrior", GUILayout.Width(150))) Deploy(b, UnitType.WarriorRock, 1);
                        if (GUILayout.Button("x5", GUILayout.Width(40))) Deploy(b, UnitType.WarriorRock, 5);
                        if (GUILayout.Button("Deploy iron warrior", GUILayout.Width(150))) Deploy(b, UnitType.WarriorIron, 1);
                        if (GUILayout.Button("x5", GUILayout.Width(40))) Deploy(b, UnitType.WarriorIron, 5);
                        if (GUILayout.Button("Eject workers", GUILayout.Width(100))) sim.EjectAll(b);
                        GUILayout.EndHorizontal();
                        break;
                    }
                    case BuildingType.Tower:
                        GUILayout.Label(b.inside.Count > 0 ? $"Guarded by a {UnitName(b.inside[0].type)}" : "Empty - right-click it with a warrior to man it.", _small);
                        if (b.inside.Count > 0 && GUILayout.Button("Leave tower", GUILayout.Width(120))) sim.EjectAll(b);
                        break;
                }
                return;
            }
            GUILayout.Label("Nothing selected", _title);
            GUILayout.Label("Left-drag to select peons, right-click trees / rocks / iron to gather.  [.] selects idle peons, [H] jumps home.", _small);
        }

        void Deploy(Building armory, UnitType type, int count)
        {
            int n = sim.DeployWarriors(armory, type, count);
            if (n == 0)
            {
                bool weapons = type == UnitType.WarriorIron ? player.ironWeapons > 0 : player.rockWeapons > 0;
                AddMessage(weapons ? "No peons inside the armory to arm" : "No " + (type == UnitType.WarriorIron ? "iron" : "rock") + " weapons in stock", 3f);
            }
        }

        static string UnitName(UnitType t) => t == UnitType.Peon ? "Peon" : t == UnitType.WarriorRock ? "Rock warrior" : "Iron warrior";

        void DrawHelp()
        {
            var r = new Rect(Screen.width - 430, 40, 420, 300);
            Box(r, new Color(0, 0, 0, 0.7f));
            GUILayout.BeginArea(new Rect(r.x + 10, r.y + 8, r.width - 20, r.height - 16));
            GUILayout.Label("How to play  (F1 to hide)", _title);
            GUILayout.Label(
                "• Camera: WASD / arrows / screen edge, wheel zoom, Q/E or middle-drag rotate, H home\n" +
                "• Left-click / drag: select.  Double-click: all of that type.  Shift: add\n" +
                "• Right-click: move, gather (trees, rocks, iron), build, enter, attack\n" +
                "• Peons: C quarters, R armory, T tower\n" +
                "• Quarters grow new peons — the more inside, the faster\n" +
                "• Armory: peons inside make weapons from delivered wood + rock/iron;\n   deploy them as warriors\n" +
                "• Tower: put one warrior inside to shoot invaders\n" +
                "• Ctrl+0-9 set group, 0-9 select (twice to jump)\n" +
                "• P pause, +/- speed, Esc menu\n" +
                "Destroy every enemy unit and building to win.", _small);
            GUILayout.EndArea();
        }

        void DrawGameOver()
        {
            Box(new Rect(0, 0, Screen.width, Screen.height), new Color(0, 0, 0, 0.5f));
            bool won = sim.winningTeam == player.team && !player.defeated;
            GUI.Label(new Rect(0, Screen.height / 2 - 80, Screen.width, 50), won ? "Victory!" : "Defeat", _center);
            int t = (int)sim.time;
            GUI.Label(new Rect(0, Screen.height / 2 - 30, Screen.width, 30), $"Game time {t / 60}:{t % 60:00}", new GUIStyle(_label) { alignment = TextAnchor.MiddleCenter });
            if (GUI.Button(new Rect(Screen.width / 2 - 160, Screen.height / 2 + 20, 150, 40), "Play again")) onRestart?.Invoke();
            if (GUI.Button(new Rect(Screen.width / 2 + 10, Screen.height / 2 + 20, 150, 40), "Main menu")) onQuit?.Invoke();
        }

        void DrawMenu()
        {
            var r = new Rect(Screen.width / 2 - 130, Screen.height / 2 - 120, 260, 240);
            Box(r, new Color(0, 0, 0, 0.8f));
            GUILayout.BeginArea(new Rect(r.x + 15, r.y + 15, r.width - 30, r.height - 30));
            GUILayout.Label("Game menu", _title);
            if (GUILayout.Button("Resume", GUILayout.Height(34))) _showMenu = false;
            if (GUILayout.Button(paused ? "Unpause" : "Pause", GUILayout.Height(34))) paused = !paused;
            if (GUILayout.Button("Restart", GUILayout.Height(34))) onRestart?.Invoke();
            if (GUILayout.Button("Quit to main menu", GUILayout.Height(34))) onQuit?.Invoke();
            GUILayout.EndArea();
        }

        public bool MenuOpen => _showMenu;
    }
}
