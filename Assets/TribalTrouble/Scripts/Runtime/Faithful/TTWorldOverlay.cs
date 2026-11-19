// World markers the Java renderer drew and the Unity world view leaves to the host:
//  - selection rings under the selected and hovered selectables (SelectableShadowRenderer: own green, ally blue,
//    enemy red; health halos tint them red as they take damage), and a blinking ring on a target just ordered
//    (RespondManager.isResponding)
//  - the shrinking green rings where the ground was clicked (TargetRespondRenderer)
//  - while placing a building: the legal sites around the cursor (BuildingSiteRenderer) and the see-through
//    building, white where it may go and red where it may not (PlacingDelegate.render3D)
//  - the selected building's rally flag (DefaultRenderer.renderRallyPoint)
//  - in the map editor: the brush outline and start markers (BrushRenderer's dots) and the access tint (AccessOverlay)
using System.Collections.Generic;
using TribalTrouble.GuiPort;
using com.oddlabs.tt.model;
using TribalTrouble.Faithful;
using TribalTrouble.Presentation;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    public sealed class TTWorldOverlay
    {
        static readonly int ColorId = Shader.PropertyToID("_Color");
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        readonly Mesh _ring, _sites, _disc, _markers;
        readonly Material _mat, _siteMat, _ghostMat, _mapMat;
        readonly MaterialPropertyBlock _block = new MaterialPropertyBlock();
        readonly com.oddlabs.tt.render.LandscapeLocation _hit = new com.oddlabs.tt.render.LandscapeLocation();
        readonly List<Vector3> _sv = new List<Vector3>();
        readonly List<Color> _sc = new List<Color>();
        readonly List<Vector2> _su = new List<Vector2>();
        readonly List<int> _st = new List<int>();
        readonly Dictionary<string, GhostModel> _ghosts = new Dictionary<string, GhostModel>();

        sealed class GhostModel
        {
            public readonly List<(Mesh mesh, Matrix4x4 local)> parts = new List<(Mesh, Matrix4x4)>();
            public Texture2D texture;
        }

        public TTWorldOverlay(TTSpriteCatalog catalog)
        {
            const int seg = 40;
            var v = new Vector3[seg * 2];
            var t = new int[seg * 6];
            for (int i = 0; i < seg; i++)
            {
                float a = i * Mathf.PI * 2f / seg;
                var d = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
                v[i * 2] = d * 0.86f;
                v[i * 2 + 1] = d;
                int n = (i + 1) % seg, k = i * 6;
                t[k] = i * 2; t[k + 1] = n * 2; t[k + 2] = i * 2 + 1;
                t[k + 3] = i * 2 + 1; t[k + 4] = n * 2; t[k + 5] = n * 2 + 1;
            }
            _ring = new Mesh { name = "SelectionRing", vertices = v, triangles = t };
            _ring.RecalculateBounds();
            _sites = new Mesh { name = "BuildingSites" };
            _sites.MarkDynamic();
            _disc = new Mesh { name = "RoundFootprint" };
            _disc.MarkDynamic();
            _markers = new Mesh { name = "MapModeMarkers", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
            _markers.MarkDynamic();
            var shader = Shader.Find("TribalTrouble/WorldMarker");
            _mat = new Material(shader) { name = "WorldMarker", hideFlags = HideFlags.DontSave };
            _siteMat = new Material(shader) { name = "BuildingSites", hideFlags = HideFlags.DontSave, mainTexture = SiteTexture() };
            _ghostMat = new Material(shader) { name = "PlacingGhost", hideFlags = HideFlags.DontSave };
            _mapMat = new Material(shader) { name = "MapModeMarkers", hideFlags = HideFlags.DontSave };
            _mapMat.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        }

        /// <summary>BuildingSiteRenderer's 16x16 tile: white with a clear one-pixel border.</summary>
        static Texture2D SiteTexture()
        {
            var tex = new Texture2D(16, 16, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.DontSave };
            var px = new Color32[256];
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                    px[y * 16 + x] = x == 0 || y == 0 || x == 15 || y == 15 ? new Color32(255, 255, 255, 0) : new Color32(255, 255, 255, 255);
            tex.SetPixels32(px);
            tex.Apply(false, true);
            return tex;
        }

        public void Draw(com.oddlabs.tt.render.DefaultRenderer dr, com.oddlabs.tt.landscape.World world, FaithfulView view)
        {
            if (dr == null || world == null || dr.localPlayer == null) return;
            var local = dr.localPlayer;
            var hm = world.getHeightMap();
            var selected = dr.selection?.getCurrentSelection();
            if (selected != null)
            {
                var it = selected.getSet().iterator();
                while (it.hasNext()) Ring((Selectable)it.next(), true, local, hm);
            }
            var hovered = dr.picker?.getCurrentHovered() as Selectable;
            if (hovered != null && (selected == null || !selected.contains(hovered))) Ring(hovered, false, local, hm);
            Responding(dr, local, hm, selected);
            ClickRings(hm);
            // map mode (the camera far out): every unit and building as a square in its team colour, like the Java
            // renderer's no-detail sprites
            var cam = TribalTrouble.GuiPort.Host.CurrentCamera;
            if (cam != null && cam.inNoDetailMode()) MapMarkers(world);
            var del = dr.guiRoot?.getDelegate();
            if (del is com.oddlabs.tt.@delegate.PlacingDelegate placing) Placing(placing, world, view);
            RallyPoint(dr, world, view);
            EditorDots(hm);
            AccessTint(hm);
        }

        // ---- map editor

        readonly Mesh _dots = new Mesh { name = "EditorDots" };
        Material _dotMat;
        Texture2D _dotTex;

        /// <summary>BrushRenderer's 32x32 soft round dot.</summary>
        static Texture2D DotTexture()
        {
            const int n = 32;
            var tex = new Texture2D(n, n, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, hideFlags = HideFlags.DontSave };
            var px = new Color32[n * n];
            float c = (n - 1) / 2f;
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float dx = (x - c) / c, dy = (y - c) / c;
                    float alpha = Mathf.Clamp01(1f - Mathf.Sqrt(dx * dx + dy * dy));
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(Mathf.Min(1f, alpha * 2f) * 255f + .5f));
                }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            return tex;
        }

        void EditorDots(com.oddlabs.tt.landscape.HeightMap hm)
        {
            var dots = EditorView.Dots;
            if (dots == null || dots.Count == 0) return;
            if (_dotMat == null)
            {
                _dotTex = DotTexture();
                _dotMat = new Material(Shader.Find("TribalTrouble/WorldMarker")) { name = "EditorDots", hideFlags = HideFlags.DontSave, mainTexture = _dotTex };
            }
            const float half = com.oddlabs.tt.mapeditor.BrushRenderer.DOT_SIZE / 2f;
            _sv.Clear(); _sc.Clear(); _su.Clear(); _st.Clear();
            foreach (var d in dots)
            {
                int b = _sv.Count;
                var col = new Color(d.r, d.g, d.b, d.a);
                for (int k = 0; k < 4; k++)
                {
                    float px = d.x + (k == 1 || k == 2 ? half : -half), py = d.y + (k >= 2 ? half : -half);
                    _sv.Add(TTSpace.ToUnity(px, py, hm.computeInterpolatedHeight(0, px, py) + 0.15f));
                    _sc.Add(col);
                    _su.Add(new Vector2(k == 1 || k == 2 ? 1 : 0, k >= 2 ? 1 : 0));
                }
                _st.Add(b); _st.Add(b + 2); _st.Add(b + 1); _st.Add(b); _st.Add(b + 3); _st.Add(b + 2);
            }
            _dots.Clear();
            _dots.indexFormat = _sv.Count > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
            _dots.SetVertices(_sv); _dots.SetColors(_sc); _dots.SetUVs(0, _su); _dots.SetTriangles(_st, 0);
            _dots.RecalculateBounds();
            Graphics.RenderMesh(new RenderParams(_dotMat), _dots, 0, Matrix4x4.identity);
        }

        // the access tint: a texture of the AccessOverlay colours on a mesh following the ground
        Mesh _access;
        Material _accessMat;
        Texture2D _accessTex;
        int _accessVersion = -1, _accessFrame;
        com.oddlabs.tt.landscape.HeightMap _accessHm;

        void AccessTint(com.oddlabs.tt.landscape.HeightMap hm)
        {
            var colors = EditorView.AccessColors;
            if (!EditorView.AccessVisible || colors == null) return;
            int size = EditorView.AccessSize;
            if (_accessTex == null || _accessTex.width != size)
            {
                _accessTex = new Texture2D(size, size, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.DontSave };
                _accessMat = new Material(Shader.Find("TribalTrouble/WorldMarker")) { name = "AccessTint", hideFlags = HideFlags.DontSave, mainTexture = _accessTex };
                _accessMat.SetColor("_Color", new Color(1f, 1f, 1f, 0.55f));   // reads as the Java tint over the lit ground
                _accessVersion = -1;
            }
            if (_accessVersion != EditorView.AccessVersion)
            {
                _accessVersion = EditorView.AccessVersion;
                var px = new Color32[size * size];
                for (int i = 0; i < px.Length; i++)
                {
                    int c = colors[i];
                    // premultiplied in the Java tint; the marker shader blends straight alpha
                    byte a = (byte)(c >> 24);
                    float k = a > 0 ? 255f / a : 0f;
                    px[i] = new Color32((byte)Mathf.Min(255, (c & 0xff) * k), (byte)Mathf.Min(255, ((c >> 8) & 0xff) * k), (byte)Mathf.Min(255, ((c >> 16) & 0xff) * k), a);
                }
                _accessTex.SetPixels32(px);
                _accessTex.Apply(false);
            }
            // the mesh follows the ground: rebuilt now and then (heights change under the brush)
            if (_access == null || _accessHm != hm || Time.frameCount - _accessFrame > 20)
            {
                _accessHm = hm;
                _accessFrame = Time.frameCount;
                int step = Mathf.Max(1, size / 192);
                int n = (size - 1) / step + 1;
                var v = new Vector3[n * n];
                var uv = new Vector2[n * n];
                float mpg = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
                for (int j = 0; j < n; j++)
                    for (int i = 0; i < n; i++)
                    {
                        int gx = Mathf.Min(size - 1, i * step), gy = Mathf.Min(size - 1, j * step);
                        float x = gx * mpg, y = gy * mpg;
                        v[j * n + i] = TTSpace.ToUnity(x, y, hm.computeInterpolatedHeight(0, x, y) + 0.1f);
                        uv[j * n + i] = new Vector2((gx + .5f) / size, (gy + .5f) / size);
                    }
                var t = new int[(n - 1) * (n - 1) * 6];
                int q = 0;
                for (int j = 0; j < n - 1; j++)
                    for (int i = 0; i < n - 1; i++)
                    {
                        int v0 = j * n + i;
                        t[q++] = v0; t[q++] = v0 + n; t[q++] = v0 + 1;
                        t[q++] = v0 + 1; t[q++] = v0 + n; t[q++] = v0 + n + 1;
                    }
                if (_access == null) _access = new Mesh { name = "AccessTint", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
                _access.Clear();
                _access.vertices = v; _access.uv = uv; _access.triangles = t;
                _access.RecalculateBounds();
            }
            Graphics.RenderMesh(new RenderParams(_accessMat), _access, 0, Matrix4x4.identity);
        }

        void Ring(Selectable s, bool selected, com.oddlabs.tt.player.Player local, com.oddlabs.tt.landscape.HeightMap hm)
        {
            if (s.isDead() || !s.isEnabled()) return;
            DrawRing(s, RingColor(s, selected, local), hm);
        }

        void DrawRing(Selectable s, Color c, com.oddlabs.tt.landscape.HeightMap hm)
        {
            float x = s.getPositionX(), y = s.getPositionY();
            float r = s is Unit u ? ((UnitTemplate)u.getTemplate()).getSelectionRadius() + 0.3f : s is Building rb ? TTFit.RingRadius(rb) : 5f;   // (TT2: fitted to the model)
            float z = s is Building b ? b.getPositionZ() : hm.computeInterpolatedHeight(0, x, y);
            DrawRingAt(x, y, z + 0.15f, r, c);
        }

        void DrawRingAt(float x, float y, float z, float r, Color c)
        {
            _block.Clear();
            _block.SetColor(ColorId, c);
            Graphics.RenderMesh(new RenderParams(_mat) { matProps = _block }, _ring, 0,
                Matrix4x4.TRS(TTSpace.ToUnity(x, y, z), Quaternion.identity, new Vector3(r, 1, r)));
        }

        static Color RingColor(Selectable s, bool selected, com.oddlabs.tt.player.Player local)
        {
            var owner = s.getOwnerNoCheck();
            Color c = owner == local ? (selected ? new Color(0, 1, 0) : new Color(0, .5f, 0))
                : local.isEnemy(owner) ? (selected ? new Color(1, 0, 0) : new Color(.5f, 0, 0))
                : (selected ? new Color(0, 0, 1) : new Color(0, 0, .5f));
            if (com.oddlabs.tt.global.Settings.getSettings().health_halos_enabled)
            {
                float max = s is Unit u ? u.getEffectiveMaxHitPoints() : s is Building b ? b.getEffectiveMaxHitPoints() : 0;
                float hp = s is Unit u2 ? u2.getHitPoints() : s is Building b2 ? b2.getHitPoints() : 0;
                if (max > 0 && hp < max) c = Color.Lerp(c, Color.red, 1f - Mathf.Clamp01(hp / max));
            }
            return c;
        }

        /// <summary>The unit, building or tree just given as an order's target blinks its ring.</summary>
        void Responding(com.oddlabs.tt.render.DefaultRenderer dr, com.oddlabs.tt.player.Player local, com.oddlabs.tt.landscape.HeightMap hm,
            com.oddlabs.tt.viewer.SelectionArmy selected)
        {
            var rm = dr.picker?.getRespondManager();
            if (rm == null || rm.respond_targets == null || rm.respond_targets.isEmpty()) return;
            var it = rm.respond_targets.keySet().iterator();
            while (it.hasNext())
            {
                var target = it.next();
                if (!rm.isResponding(target)) continue;
                if (target is Selectable s)
                {
                    if (s.isDead() || (selected != null && selected.contains(s))) continue;
                    DrawRing(s, RingColor(s, true, local), hm);
                }
                else if (target is com.oddlabs.tt.util.Target t)
                {
                    float x = t.getPositionX(), y = t.getPositionY();
                    DrawRingAt(x, y, hm.computeInterpolatedHeight(0, x, y) + 0.15f, 2.2f, new Color(0, 1, 0));
                }
            }
        }

        /// <summary>The green ring where the ground was clicked, shrinking over a third of a second.</summary>
        void ClickRings(com.oddlabs.tt.landscape.HeightMap hm)
        {
            foreach (var r in com.oddlabs.tt.render.PortPicker.getResponds())
            {
                float p = r.getProgress();
                if (p <= 0f) continue;
                DrawRingAt(r.getPositionX(), r.getPositionY(), r.getPositionZ() + 0.15f, 0.8f * p, new Color(0, 1, 0, 1));   // 1.6 m across
            }
        }

        void Placing(com.oddlabs.tt.@delegate.PlacingDelegate placing, com.oddlabs.tt.landscape.World world, FaithfulView view)
        {
            var viewer = placing.getViewer();
            if (viewer == null || !viewer.getPicker().pickLocation(placing.getCamera().getState(), _hit)) return;
            var template = placing.getTemplate();
            var grid = world.getUnitGrid();
            var hm = world.getHeightMap();
            int size = template.getPlacingSize();
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(_hit.x) - (size - 1);
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(_hit.y) - (size - 1);
            int cgx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(_hit.x);
            int cgy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(_hit.y);
            float mpg = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            float cx = mpg * (gx + (size - .5f)), cy = mpg * (gy + (size - .5f));

            // Free-form rule: round footprints, so the building's own round pad instead of the grid of sites
            // (Extended games use the same small round footprints, on the cell's centre unless free-form)
            if ((world.isFreeForm() || world.isExtended()) && !placing.isWall())
            {
                // exactly under the cursor (the order carries the offset from the cell's centre), the footprint's size
                bool ok = template.isPlacingLegal(grid, cgx, cgy);
                float half = world.isFreeForm() ? com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * .5f : 0f;
                float fx = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(cgx) + Mathf.Clamp(_hit.x - com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(cgx), -half, half);
                float fy = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(cgy) + Mathf.Clamp(_hit.y - com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(cgy), -half, half);
                float radius = mpg * com.oddlabs.tt.model.LandBuilding.freeFormDiameter(size) * .5f;
                DrawDisc(hm, fx, fy, radius, ok ? new Color(0.35f, 1f, 0.45f, 1f) : new Color(1f, 0.25f, 0.2f, 1f));
                var g = template.getTemplateID() >= Race.BUILDING_PALISADE ? ExtendedGhost(template, world)
                    : Ghost(template.getBuiltRenderer()?.location, view);
                if (g != null) DrawGhost(g, fx, fy, ok, hm);
                return;
            }
            // legal sites around the cursor (the classic game only: New Tides is free-form, no grid at all)
            if (!world.isFreeForm() && !world.isExtended())
            {
                var filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(grid, template, com.oddlabs.tt.@delegate.PlacingDelegate.GRID_RADIUS, false);
                grid.scan(filter, cgx, cgy);
                var sites = filter.getResult();
                float maxR = 2 * com.oddlabs.tt.@delegate.PlacingDelegate.GRID_RADIUS, maxR2 = maxR * maxR;
                _sv.Clear(); _sc.Clear(); _su.Clear(); _st.Clear();
                var sit = sites.iterator();
                while (sit.hasNext())
                {
                    var t = (com.oddlabs.tt.util.Target)sit.next();
                    float x = t.getPositionX(), y = t.getPositionY();
                    float dx = x - cx, dy = y - cy, a = (dx * dx + dy * dy) / maxR2;
                    var col = dx == 0f && dy == 0f ? new Color(1, 1, 1, 1) : new Color(0, 1, 0, Mathf.Max(0f, 1f - a * a));
                    if (col.a <= 0.01f) continue;
                    AddSite(hm, x, y, 1f, col);   // DecalRenderer: the size is the full width (2 m, one grid cell)
                }
                if (_sv.Count > 0)
                {
                    _sites.Clear();
                    _sites.SetVertices(_sv); _sites.SetColors(_sc); _sites.SetUVs(0, _su); _sites.SetTriangles(_st, 0);
                    _sites.RecalculateBounds();
                    Graphics.RenderMesh(new RenderParams(_siteMat), _sites, 0, Matrix4x4.identity);
                }
            }

            // the building itself, see-through: white if it may go here, red if not
            var ghost = template.getTemplateID() >= Race.BUILDING_PALISADE ? ExtendedGhost(template, world)
                : Ghost(template.getBuiltRenderer()?.location, view);
            if (ghost == null) return;
            if (placing.isWall())
            {
                // Extended rules: a palisade wall, one post per footprint from where the drag started to the cursor
                foreach (var post in placing.getWallPosts())
                    DrawGhost(ghost, mpg * (post[0] + .5f), mpg * (post[1] + .5f), post[2] != 0, hm);
                return;
            }
            DrawGhost(ghost, cx, cy, template.isPlacingLegal(grid, cgx, cgy), hm);
        }

        /// <summary>Map mode: team-coloured squares (template no-detail size) over every living unit and building.</summary>
        void MapMarkers(com.oddlabs.tt.landscape.World world)
        {
            _sv.Clear(); _sc.Clear(); _su.Clear(); _st.Clear();
            foreach (var p in world.getPlayers())
            {
                if (p == null) continue;
                var pc = p.getColor();
                var c = new Color(pc.x(), pc.y(), pc.z(), 1f);
                var it = p.getUnits().getSet().iterator();
                while (it.hasNext())
                {
                    if (!(it.next() is Selectable s) || s.isDead() || !s.isEnabled()) continue;
                    float r = s.getNoDetailSize();
                    if (r <= 0f) continue;
                    float x = s.getPositionX(), y = s.getPositionY(), z = s.getPositionZ() + 0.1f;
                    int b = _sv.Count;
                    _sv.Add(TTSpace.ToUnity(x - r, y - r, z)); _sv.Add(TTSpace.ToUnity(x + r, y - r, z));
                    _sv.Add(TTSpace.ToUnity(x + r, y + r, z)); _sv.Add(TTSpace.ToUnity(x - r, y + r, z));
                    for (int k = 0; k < 4; k++) { _sc.Add(c); _su.Add(new Vector2(0.5f, 0.5f)); }
                    _st.Add(b); _st.Add(b + 1); _st.Add(b + 2); _st.Add(b); _st.Add(b + 2); _st.Add(b + 3);
                }
            }
            if (_sv.Count == 0) return;
            _markers.Clear();
            _markers.SetVertices(_sv); _markers.SetColors(_sc); _markers.SetUVs(0, _su); _markers.SetTriangles(_st, 0);
            _markers.RecalculateBounds();
            _block.Clear();
            _block.SetTexture(MainTexId, Texture2D.whiteTexture);
            _block.SetColor(ColorId, Color.white);
            Graphics.RenderMesh(new RenderParams(_mapMat) { matProps = _block }, _markers, 0, Matrix4x4.identity);
        }

        /// <summary>A disc draped over the ground: a faint fill and a bright rim (the free-form round footprint).</summary>
        void DrawDisc(com.oddlabs.tt.landscape.HeightMap hm, float cx, float cy, float r, Color c)
        {
            const int seg = 48, rings = 6;
            _sv.Clear(); _sc.Clear(); _su.Clear(); _st.Clear();
            for (int k = 0; k <= rings + 1; k++)
            {
                // rings out to the rim, then a thin band past it for the rim's soft outer edge
                float f = k <= rings ? (float)k / rings : 1.08f;
                float alpha = k <= rings ? Mathf.Lerp(0.18f, 0.32f, f * f) : 0f;
                if (k == rings) alpha = 0.95f;
                for (int i = 0; i < seg; i++)
                {
                    float a = i * Mathf.PI * 2f / seg;
                    float px = cx + Mathf.Cos(a) * r * f, py = cy + Mathf.Sin(a) * r * f;
                    _sv.Add(TTSpace.ToUnity(px, py, hm.computeInterpolatedHeight(0, px, py) + 0.15f));
                    _sc.Add(new Color(c.r, c.g, c.b, alpha));
                    _su.Add(new Vector2(0.5f, 0.5f));
                    if (k == 0) break;   // the centre: one vertex
                }
            }
            // centre fan, then quads between rings (the centre ring is a single vertex)
            for (int i = 0; i < seg; i++) { _st.Add(0); _st.Add(1 + (i + 1) % seg); _st.Add(1 + i); }
            for (int k = 1; k <= rings + 1; k++)
            {
                int b0 = k == 1 ? 1 : 1 + (k - 2) * seg, b1 = 1 + (k - 1) * seg;
                if (k == 1) continue;   // ring 1 hangs off the centre fan
                for (int i = 0; i < seg; i++)
                {
                    int n = (i + 1) % seg;
                    _st.Add(b0 + i); _st.Add(b0 + n); _st.Add(b1 + i);
                    _st.Add(b1 + i); _st.Add(b0 + n); _st.Add(b1 + n);
                }
            }
            _disc.Clear();
            _disc.SetVertices(_sv); _disc.SetColors(_sc); _disc.SetUVs(0, _su); _disc.SetTriangles(_st, 0);
            _disc.RecalculateBounds();
            _block.Clear();
            _block.SetTexture(MainTexId, Texture2D.whiteTexture);
            _block.SetColor(ColorId, Color.white);
            Graphics.RenderMesh(new RenderParams(_mat) { matProps = _block }, _disc, 0, Matrix4x4.identity);
        }

        void DrawGhost(GhostModel ghost, float cx, float cy, bool legal, com.oddlabs.tt.landscape.HeightMap hm)
        {
            float z = Mathf.Max(hm.getSeaLevelMeters(), hm.getNearestHeight(cx, cy));   // (a bridge's span over the water, not on the sea bed)
            var m = TTSpace.ToUnity(Xf.TRz(cx, cy, z, 0f));
            _block.Clear();
            _block.SetTexture(MainTexId, ghost.texture != null ? ghost.texture : Texture2D.whiteTexture);
            _block.SetColor(ColorId, legal ? new Color(1f, 1f, 1f, 0.55f) : new Color(1f, 0.15f, 0.1f, 0.6f));
            foreach (var (mesh, local) in ghost.parts)
                for (int sm = 0; sm < mesh.subMeshCount; sm++)
                    Graphics.RenderMesh(new RenderParams(_ghostMat) { matProps = _block }, mesh, sm, m * local);
        }

        void AddSite(com.oddlabs.tt.landscape.HeightMap hm, float x, float y, float half, Color c)
        {
            // a 2x2 patch following the ground
            int b = _sv.Count;
            for (int j = 0; j <= 2; j++)
                for (int i = 0; i <= 2; i++)
                {
                    float px = x - half + i * half, py = y - half + j * half;
                    _sv.Add(TTSpace.ToUnity(px, py, hm.computeInterpolatedHeight(0, px, py) + 0.12f));
                    _sc.Add(c);
                    _su.Add(new Vector2(i * 0.5f, j * 0.5f));
                }
            for (int j = 0; j < 2; j++)
                for (int i = 0; i < 2; i++)
                {
                    int v0 = b + j * 3 + i;
                    _st.Add(v0); _st.Add(v0 + 3); _st.Add(v0 + 1);
                    _st.Add(v0 + 1); _st.Add(v0 + 3); _st.Add(v0 + 4);
                }
        }

        GhostModel Ghost(string location, FaithfulView view)
        {
            if (location == null || view == null) return null;
            if (_ghosts.TryGetValue(location, out var g)) return g;
            g = null;
            var def = view.Def(location);
            if (def != null && def.model != null && def.lods.Length > 0)
            {
                g = new GhostModel();
                var root = def.model.transform;
                var node = FindChild(root, def.lods[0].nodeName) ?? root;
                // the instantiated model keeps the prefab root's own transform under the placed object, so a part's
                // matrix within the prefab asset is its matrix relative to the placed object
                foreach (var mf in node.GetComponentsInChildren<MeshFilter>(true))
                    if (mf.sharedMesh != null) g.parts.Add((mf.sharedMesh, mf.transform.localToWorldMatrix));
                foreach (var smr in node.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                    if (smr.sharedMesh != null) g.parts.Add((smr.sharedMesh, smr.transform.localToWorldMatrix));
                var tex = def.lods[0].textures;
                g.texture = tex.Length > 0 ? tex[0].baseMap : null;
                if (g.parts.Count == 0) g = null;
            }
            _ghosts[location] = g;
            return g;
        }

        /// <summary>The Extended rules' palisade post and shrine, which have no sprite file (TTExtendedArt draws them).</summary>
        GhostModel ExtendedGhost(BuildingTemplate template, com.oddlabs.tt.landscape.World world)
        {
            int templateId = template.getTemplateID();
            bool vik = TTExtendedArt.VikingTemplate(template, world);
            string key = "extended:" + templateId + (vik ? "v" : "n");
            if (_ghosts.TryGetValue(key, out var g)) return g;
            g = new GhostModel();
            var kit = TTExtendedArt.KitGhost(templateId, vik);   // the building kit's model of this race
            if (kit != null) foreach (var part in kit) g.parts.Add((part, Matrix4x4.identity));
            else if (templateId == Race.BUILDING_BRIDGE) { g.parts.Add((TTExtendedArt.BridgeDeck(), Matrix4x4.identity)); g.parts.Add((TTExtendedArt.BridgePiles(), Matrix4x4.identity)); }   // a bridge's span
            else if (templateId == Race.BUILDING_PALISADE) g.parts.Add((TTExtendedArt.PalisadePost(), Matrix4x4.identity));
            else if (templateId == Race.BUILDING_FORGE) foreach (var part in TTExtendedArt.ForgeParts()) g.parts.Add((part, Matrix4x4.identity));   // the arsenal
            else if (templateId == Race.BUILDING_TOTEM) g.parts.Add((TTExtendedArt.WarTotemPost(), Matrix4x4.identity));
            else if (templateId == Race.BUILDING_DOCK) foreach (var part in TTExtendedArt.DockParts()) g.parts.Add((part, Matrix4x4.identity));
            else if (templateId == Race.BUILDING_MARKET) foreach (var part in TTExtendedArt.MarketParts()) g.parts.Add((part, Matrix4x4.identity));
            else { g.parts.Add((TTExtendedArt.Stones(), Matrix4x4.identity)); g.parts.Add((TTExtendedArt.Totem(), Matrix4x4.identity)); }
            _ghosts[key] = g;
            return g;
        }

        static Transform FindChild(Transform t, string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (t.name == name) return t;
            for (int i = 0; i < t.childCount; i++)
            {
                var r = FindChild(t.GetChild(i), name);
                if (r != null) return r;
            }
            return null;
        }

        /// <summary>The flag at the selected building's rally point, turned towards the camera, in the owner's colour.</summary>
        void RallyPoint(com.oddlabs.tt.render.DefaultRenderer dr, com.oddlabs.tt.landscape.World world, FaithfulView view)
        {
            var b = dr.selectedBuilding;
            if (b == null || b.isDead() || !b.hasRallyPoint() || dr.camera == null) return;
            var rally = b.getRallyPoint();
            if (rally == null) return;
            var hm = world.getHeightMap();
            float x = rally.getPositionX(), y = rally.getPositionY();
            float z = Mathf.Max(hm.getSeaLevelMeters(), hm.getNearestHeight(x, y));
            if (rally is Building rb)
            {
                x += ((BuildingTemplate)rb.getTemplate()).getRallyX();
                y += ((BuildingTemplate)rb.getTemplate()).getRallyY();
                z += ((BuildingTemplate)rb.getTemplate()).getRallyZ();
            }
            var team = b.getOwner().getColor();
            // where the units gather, plain to see: pulsing rings on the ground under the flag, and a trail of dots
            // from the building out to it //added (rally flags)
            {
                var tc = new Color(Mathf.Lerp(1f, team.x(), .7f), Mathf.Lerp(1f, team.y(), .7f), Mathf.Lerp(1f, team.z(), .7f), 1f);
                float g = Mathf.Max(hm.getSeaLevelMeters(), hm.getNearestHeight(x, y)) + 0.15f;
                float p = 0.5f + 0.5f * Mathf.Sin(Time.time * 3f);
                DrawRingAt(x, y, g, 1.3f + 0.35f * p, new Color(tc.r, tc.g, tc.b, 0.9f));
                DrawRingAt(x, y, g, 0.6f, new Color(1f, 1f, 1f, 0.85f));
                float bx = b.getPositionX(), by = b.getPositionY();
                float len = Mathf.Sqrt((x - bx) * (x - bx) + (y - by) * (y - by));
                float start = Mathf.Min(len, ((BuildingTemplate)b.getTemplate()).getPlacingSize() * 2f + 1f);
                float phase = (Time.time * 2f) % 2.5f;
                for (float d = start + phase; d < len - 1.6f; d += 2.5f)
                {
                    float px = bx + (x - bx) * d / len, py = by + (y - by) * d / len;
                    DrawRingAt(px, py, Mathf.Max(hm.getSeaLevelMeters(), hm.getNearestHeight(px, py)) + 0.15f, 0.22f, new Color(tc.r, tc.g, tc.b, 0.65f));
                }
            }
            var model = Ghost(b.getOwner().getRace().getRallyPoint()?.location, view);
            if (model == null) return;
            float dx = dr.camera.getCurrentX() - x, dy = dr.camera.getCurrentY() - y;
            float ang = dx * dx + dy * dy > 0.01f ? Mathf.Atan2(dy, dx) : 0f;
            // a bigger flag than the original's (it was easy to miss at game distance), swaying a little
            var m = TTSpace.ToUnity(Xf.TRz(x, y, z, ang)) * Matrix4x4.Scale(Vector3.one * 1.7f)
                    * Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * 1.7f) * 3f));
            _block.Clear();
            _block.SetTexture(MainTexId, model.texture != null ? model.texture : Texture2D.whiteTexture);
            _block.SetColor(ColorId, new Color(Mathf.Lerp(1f, team.x(), .6f), Mathf.Lerp(1f, team.y(), .6f), Mathf.Lerp(1f, team.z(), .6f), 1f));
            foreach (var (mesh, local) in model.parts)
                for (int sm = 0; sm < mesh.subMeshCount; sm++)
                    Graphics.RenderMesh(new RenderParams(_ghostMat) { matProps = _block }, mesh, sm, m * local);
        }
    }
}
