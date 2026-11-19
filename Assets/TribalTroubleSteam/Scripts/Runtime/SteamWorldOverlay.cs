// What the Steam release's renderer drew in the world besides the sprites, the landscape and the particles:
//  - SelectableShadowRenderer: a halo under every selected or hovered unit and building (its template's
//    GeneratorHalos SELECTED texture: a soft shadow and a ring, green / red / blue for own / enemy / ally, half as
//    bright when only hovered), and in the static shadow mode a round blob shadow (SHADOWED) under every other
//    unit, building, piece of scenery, plant and rubber tree, faded by its shadow fade
//  - TargetRespondRenderer: the green ring where the ground was clicked, shrinking over a third of a second
//  - BuildingGhostRenderer (PlacingDelegate.render3D): the legal building sites around the cursor (BuildingSiteRenderer:
//    2 m tiles, white at the centre, green fading out to 40 m) and the see-through building, white where it may go
//    and red where it may not
//  - DefaultRenderer.renderRallyPoint: the race's flag at the selected building's rally point, facing the camera
//  - RespondManager: an order's target blinks desaturated with a half team decal
// All decals are DecalRenderer's: a grid draped over the terrain triangles, alpha blended (TribalTroubleSteam/Decal).
using System.Collections.Generic;
using com.oddlabs.tt.model;
using TribalTroubleSteam.Presentation;
using UnityEngine;

namespace TribalTroubleSteam
{
    public sealed class SteamWorldOverlay
    {
        readonly SteamView _view;
        readonly SteamTerrain _terrain;
        readonly Mesh _grid8, _grid32;
        readonly Material _blobMat, _haloMat, _respondMat, _siteMat, _ghostMat, _pointMat;
        readonly Mesh _quad;
        static readonly int PointColorId = Shader.PropertyToID("_PointColor");
        readonly MaterialPropertyBlock _block = new MaterialPropertyBlock();
        readonly MaterialPropertyBlock _ghostBlock = new MaterialPropertyBlock();
        readonly bool _linear = QualitySettings.activeColorSpace == ColorSpace.Linear;
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        static readonly int DecalColorId = Shader.PropertyToID("_DecalColor");
        static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");
        static readonly int ColorId = Shader.PropertyToID("_Color");
        static readonly int RespondId = Shader.PropertyToID("_Respond");

        public SteamWorldOverlay(SteamView view, SteamTerrain terrain)
        {
            _view = view;
            _terrain = terrain;
            _grid8 = Grid(8);
            _grid32 = Grid(32);
            var decal = SteamShading.Decal;
            if (decal != null)
            {
                // one queue each, drawn in the Java's order: blob shadows, halos, click rings, building sites
                _blobMat = DecalMaterial(decal, "Blob shadows", 0);
                _haloMat = DecalMaterial(decal, "Selection halos", 1);
                _respondMat = DecalMaterial(decal, "Click rings", 2);
                _siteMat = DecalMaterial(decal, "Building sites", 3);
            }
            var point = SteamShading.MapPoint;
            if (point != null) _pointMat = new Material(point) { name = "Map marks", hideFlags = HideFlags.DontSave, enableInstancing = true };
            _quad = new Mesh
            {
                name = "Map mark", hideFlags = HideFlags.DontSave,
                vertices = new[] { new Vector3(-0.5f, 0f, -0.5f), new Vector3(0.5f, 0f, -0.5f), new Vector3(0.5f, 0f, 0.5f), new Vector3(-0.5f, 0f, 0.5f) },
                triangles = new[] { 0, 2, 1, 0, 3, 2 },
            };
            var ghost = SteamShading.Ghost;
            if (ghost != null) _ghostMat = new Material(ghost) { name = "Placing ghost", hideFlags = HideFlags.DontSave };
        }

        static Material DecalMaterial(Shader s, string name, int order) =>
            new Material(s) { name = name, hideFlags = HideFlags.DontSave, enableInstancing = true, renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent - 50 + order };

        /// <summary>DecalRenderer's mesh: n x n points over -0.5..0.5 (x, z), texture coordinates 0..1.</summary>
        static Mesh Grid(int n)
        {
            var v = new Vector3[n * n];
            var uv = new Vector2[n * n];
            float step = 1f / (n - 1);
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    v[y * n + x] = new Vector3(x * step - 0.5f, 0f, y * step - 0.5f);
                    uv[y * n + x] = new Vector2(x * step, y * step);
                }
            var t = new int[(n - 1) * (n - 1) * 6];
            int k = 0;
            for (int y = 0; y < n - 1; y++)
                for (int x = 0; x < n - 1; x++)
                {
                    int tl = y * n + x, tr = tl + 1, bl = tl + n, br = bl + 1;
                    t[k++] = tl; t[k++] = bl; t[k++] = tr;
                    t[k++] = tr; t[k++] = bl; t[k++] = br;
                }
            var m = new Mesh { name = "Decal grid " + n, vertices = v, uv = uv, triangles = t, hideFlags = HideFlags.DontSave };
            m.bounds = new Bounds(Vector3.zero, new Vector3(1f, 2000f, 1f));   // the heights come from the shader
            return m;
        }

        // ------------------------------------------------------------------ textures

        readonly Dictionary<object, Texture2D[]> _halos = new Dictionary<object, Texture2D[]>(RefEq.Instance);
        Texture2D _ring, _site;

        /// <summary>A template's halo textures ([SHADOWED, SELECTED]) from its GeneratorHalos, built once.</summary>
        Texture2D[] Halos(java.util.function.Supplier source)
        {
            source ??= RacesResources.DEFAULT_SHADOW_DESC;
            if (source == null) return null;
            if (_halos.TryGetValue(source, out var t)) return t;
            t = null;
            try
            {
                if (source.get() is com.oddlabs.tt.render.Texture[] arr && arr.Length >= 2)
                {
                    t = new Texture2D[2];
                    for (int i = 0; i < 2; i++) t[i] = Upload(arr[i]?.images != null && arr[i].images.Length > 0 ? arr[i].images[0] : null, "Halo " + i);
                    if (t[0] == null || t[1] == null) t = null;
                }
            }
            catch (System.Exception e) { Debug.LogWarning("[TT Steam] halo textures: " + e.Message); }
            _halos[source] = t;
            return t;
        }

        static Texture2D Upload(com.oddlabs.tt.resource.GLImage img, string name)
        {
            if (img == null) return null;
            int w = img.getWidth(), h = img.getHeight();
            var data = img.pixel_data.array();
            int bpp = img.getPixelSize();
            var px = new Color32[w * h];
            for (int i = 0; i < px.Length; i++)
            {
                if (bpp >= 4) px[i] = new Color32((byte)data[i * 4], (byte)data[i * 4 + 1], (byte)data[i * 4 + 2], (byte)data[i * 4 + 3]);
                else { byte b = (byte)data[i * bpp]; px[i] = new Color32(255, 255, 255, b); }
            }
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false) { name = name, wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.DontSave };
            tex.SetPixels32(px);
            tex.Apply(false, true);
            return tex;
        }

        /// <summary>RenderState's respond ring: GeneratorRing(LandscapeTargetRespond.SIZE, {0.40 0, 0.41 1, 0.48 1, 0.49 0}),
        /// white with the ring as its alpha.</summary>
        Texture2D Ring()
        {
            if (_ring != null) return _ring;
            int n = com.oddlabs.tt.landscape.LandscapeTargetRespond.SIZE;
            var px = new Color32[n * n];
            try
            {
                var ch = new com.oddlabs.tt.procedural.Ring(n, n, new float[][] { new[] { 0.40f, 0f }, new[] { 0.41f, 1f }, new[] { 0.48f, 1f }, new[] { 0.49f, 0f } },
                    com.oddlabs.tt.procedural.Ring.Interpolation.LINEAR).toChannel();
                for (int y = 0; y < n; y++)
                    for (int x = 0; x < n; x++)
                        px[y * n + x] = new Color32(255, 255, 255, (byte)Mathf.Clamp(Mathf.RoundToInt(ch.getPixel(x, y) * 255f), 0, 255));
            }
            catch (System.Exception)
            {
                // the same ring worked out here: linear from radius 0.40 to 0.41, solid to 0.48, out by 0.49
                for (int y = 0; y < n; y++)
                    for (int x = 0; x < n; x++)
                    {
                        float dx = (x + 0.5f) / n - 0.5f, dy = (y + 0.5f) / n - 0.5f, r = Mathf.Sqrt(dx * dx + dy * dy);
                        float a = r < 0.40f ? 0f : r < 0.41f ? (r - 0.40f) / 0.01f : r < 0.48f ? 1f : r < 0.49f ? (0.49f - r) / 0.01f : 0f;
                        px[y * n + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                    }
            }
            _ring = new Texture2D(n, n, TextureFormat.RGBA32, false) { name = "Respond ring", wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.DontSave };
            _ring.SetPixels32(px);
            _ring.Apply(false, true);
            return _ring;
        }

        /// <summary>BuildingSiteRenderer's tile: 16 x 16 white with a clear one-texel border.</summary>
        Texture2D Site()
        {
            if (_site != null) return _site;
            var px = new Color32[256];
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                    px[y * 16 + x] = x == 0 || y == 0 || x == 15 || y == 15 ? new Color32(0, 0, 0, 0) : new Color32(255, 255, 255, 255);
            _site = new Texture2D(16, 16, TextureFormat.RGBA32, false) { name = "Building site", wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.DontSave };
            _site.SetPixels32(px);
            _site.Apply(false, true);
            return _site;
        }

        // ------------------------------------------------------------------ decal batches

        sealed class DecalBatch
        {
            public Material material;
            public Texture texture;
            public Mesh mesh;
            public readonly List<Matrix4x4> m = new List<Matrix4x4>(64);
            public readonly List<Vector4> c = new List<Vector4>(64);
        }
        readonly Dictionary<(Material, Texture, Mesh), DecalBatch> _batches = new Dictionary<(Material, Texture, Mesh), DecalBatch>();
        readonly List<DecalBatch> _batchList = new List<DecalBatch>();
        readonly Matrix4x4[] _mArr = new Matrix4x4[1023];
        readonly Vector4[] _cArr = new Vector4[1023];

        void Decal(Material mat, Texture tex, float x, float y, float size, Color c)
        {
            if (mat == null || tex == null || size <= 0f || c.a <= 0f) return;
            var mesh = size > 4.5f ? _grid32 : _grid8;
            var key = (mat, tex, mesh);
            if (!_batches.TryGetValue(key, out var b))
            {
                b = new DecalBatch { material = mat, texture = tex, mesh = mesh };
                _batches[key] = b;
                _batchList.Add(b);
            }
            b.m.Add(Matrix4x4.TRS(new Vector3(x, 0f, y), Quaternion.identity, new Vector3(size, 1f, size)));   // (TT x, y -> Unity x, z)
            var lc = _linear ? new Color(c.r, c.g, c.b).linear : c;
            b.c.Add(new Vector4(lc.r, lc.g, lc.b, c.a));
        }

        void FlushDecals(Vector3 camPos)
        {
            foreach (var b in _batchList)
            {
                int n = b.m.Count;
                if (n == 0) continue;
                for (int start = 0; start < n; start += 1023)
                {
                    int cnt = Mathf.Min(1023, n - start);
                    b.m.CopyTo(start, _mArr, 0, cnt);
                    b.c.CopyTo(start, _cArr, 0, cnt);
                    _block.Clear();
                    _block.SetTexture(MainTexId, b.texture);
                    _block.SetVectorArray(DecalColorId, _cArr);
                    var rp = new RenderParams(b.material)
                    {
                        matProps = _block,
                        shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off,
                        receiveShadows = false,
                        worldBounds = new Bounds(camPos, Vector3.one * 100000f),
                    };
                    Graphics.RenderMeshInstanced(rp, b.mesh, 0, _mArr, cnt);
                }
                b.m.Clear();
                b.c.Clear();
            }
        }

        // ------------------------------------------------------------------ the frame

        static readonly object s_rallyKey = new object();

        /// <summary>Before the view draws the scene: the rally flag at the selected building's rally point
        /// (DefaultRenderer.renderRallyPoint), as a sprite of its own facing the camera.</summary>
        public void AddToScene(SteamScene scene)
        {
            var dr = TribalTroubleSteam.GuiPort.Host.CurrentWorld;
            var b = dr?.selectedBuilding;
            var cam = dr?.camera;
            var world = scene.World;
            if (!com.oddlabs.tt.global.Globals.draw_hud || b == null || b.isDead() || !b.hasRallyPoint() || cam == null || world == null) return;
            var rally = b.getRallyPoint();
            var key = b.getOwner()?.getRace()?.getRallyPoint();
            if (rally == null || key == null || key.location == null) return;
            var hm = world.getHeightMap();
            float x = rally.getPositionX(), y = rally.getPositionY();
            float z = Mathf.Max(hm.getSeaLevelMeters(), hm.getNearestHeight(x, y));
            if (rally is Building rb && rb.getTemplate() is BuildingTemplate rt)
            {
                x += rt.getRallyX(); y += rt.getRallyY(); z += rt.getRallyZ();
            }
            float dx = cam.getCurrentX() - x, dy = cam.getCurrentY() - y;
            float angle = dx * dx + dy * dy > 0.01f ? Mathf.Atan2(dy, dx) : 0f;
            var team = b.getOwner().getColor();
            scene.items.Add(new Renderable
            {
                kind = RKind.Building, key = s_rallyKey, model = null, location = key.drawn ?? key.location, texture = key.texture,
                anim = 0, ticks = 0f, xf = Xf.TRz(x, y, z, angle), r = team.x(), g = team.y(), b = team.z(),
            });
        }

        public void Draw(SteamScene scene, Camera cam)
        {
            var dr = TribalTroubleSteam.GuiPort.Host.CurrentWorld;
            var world = scene.World;
            if (dr == null || world == null || dr.localPlayer == null || _blobMat == null) { Responding(null); return; }
            _terrain?.BindHeights();
            var camState = dr.camera;
            bool mapMode = camState != null && camState.inNoDetailMode();
            bool hud = com.oddlabs.tt.global.Globals.draw_hud;
            bool staticShadows = !SteamGlobals.DynamicShadows || mapMode;
            Vector3 camPos = cam.transform.position;

            // halos and blob shadows (RenderState.visitSelectable: drawn in full detail only, not in map mode)
            if (!mapMode)
            {
                var local = dr.localPlayer;
                var selection = dr.selection?.getCurrentSelection();
                var hovered = dr.picker?.getCurrentHovered();
                foreach (var s in scene.selectables)
                {
                    if (s == null || !s.isEnabled()) continue;
                    var pos = TTSpace.ToUnity(s.getPositionX(), s.getPositionY(), s.getPositionZ());
                    float size = s.getShadowDiameter();
                    if (!_view.IsInView(pos, size)) continue;
                    var halos = Halos(s.getTemplate()?.getSelectableShadowRenderer()?.source);
                    if (halos == null) continue;
                    bool isSelected = selection != null && selection.contains(s);
                    bool isHovered = ReferenceEquals(hovered, s);
                    if (hud && (isSelected || isHovered))
                        Decal(_haloMat, halos[com.oddlabs.tt.procedural.GeneratorHalos.SELECTED], s.getPositionX(), s.getPositionY(), size, SelectionColor(s, local, isSelected));
                    else if (staticShadows)
                        Decal(_blobMat, halos[com.oddlabs.tt.procedural.GeneratorHalos.SHADOWED], s.getPositionX(), s.getPositionY(), size, new Color(1f, 1f, 1f, 1f - s.getShadowFade()));
                }
                if (staticShadows)
                {
                    var halos = Halos(null);
                    if (halos != null)
                        foreach (var m in scene.blobShadows)
                        {
                            float size = m.getShadowDiameter();
                            if (size <= 0f) continue;
                            var pos = TTSpace.ToUnity(m.getPositionX(), m.getPositionY(), m.getPositionZ());
                            // plants beyond RenderState.PLANTS_CUT_DIST are not drawn, nor their shadows
                            if (m is Plants && (pos - camPos).sqrMagnitude > 200f * 200f) continue;
                            if (!_view.IsInView(pos, size)) continue;
                            Decal(_blobMat, halos[com.oddlabs.tt.procedural.GeneratorHalos.SHADOWED], m.getPositionX(), m.getPositionY(), size, new Color(1f, 1f, 1f, 1f - m.getShadowFade()));
                        }
                }
            }

            // the click rings (TargetRespondRenderer: green, 1.6 m x progress)
            _responds.Clear();
            foreach (var r in scene.responds) _responds.Add(r);
            foreach (var r in com.oddlabs.tt.render.PortPicker.getResponds()) _responds.Add(r);
            foreach (com.oddlabs.tt.landscape.LandscapeTargetRespond r in _responds)
            {
                float p = r.getProgress();
                if (p > 0f) Decal(_respondMat, Ring(), r.getPositionX(), r.getPositionY(), 1.6f * p, new Color(0f, 1f, 0f, 1f));
            }

            // placing a building (PlacingDelegate.render3D -> BuildingGhostRenderer)
            com.oddlabs.tt.render.BuildingGhostRenderer.Pending = null;
            try { dr.guiRoot?.getDelegate()?.render3D(null, null, camState, null, null); }
            catch (System.Exception e) { if (!_ghostWarned) { _ghostWarned = true; Debug.LogWarning("[TT Steam] placing ghost: " + e.Message); } }
            var req = com.oddlabs.tt.render.BuildingGhostRenderer.Pending ?? DebugPlacing?.Invoke();
            com.oddlabs.tt.render.BuildingGhostRenderer.Pending = null;
            if (req != null) Placing(req);

            FlushDecals(camPos);
            if (mapMode) MapMarks(scene, camPos);
            Responding(dr);
        }

        readonly HashSet<object> _responds = new HashSet<object>(RefEq.Instance);

        /// <summary>SpriteRenderer.renderNoDetail: a square of the tribe's colour, twice the template's no-detail size,
        /// 10 cm over each unit and building drawn as a point on the map.</summary>
        void MapMarks(SteamScene scene, Vector3 camPos)
        {
            if (_pointMat == null || scene.points.Count == 0) return;
            int n = 0;
            foreach (var s in scene.points)
            {
                float size = s.getNoDetailSize() * 2f;
                var c = s.getOwner().getColor();
                var lc = _linear ? new Color(c.x(), c.y(), c.z()).linear : new Color(c.x(), c.y(), c.z());
                _mArr[n] = Matrix4x4.TRS(TTSpace.ToUnity(s.getPositionX(), s.getPositionY(), s.getPositionZ() + 0.1f), Quaternion.identity, new Vector3(size, 1f, size));
                _cArr[n] = new Vector4(lc.r, lc.g, lc.b, c.w());
                if (++n == 1023) { FlushMarks(n, camPos); n = 0; }
            }
            if (n > 0) FlushMarks(n, camPos);
        }

        void FlushMarks(int n, Vector3 camPos)
        {
            _block.Clear();
            _block.SetVectorArray(PointColorId, _cArr);
            var rp = new RenderParams(_pointMat)
            {
                matProps = _block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off, receiveShadows = false,
                worldBounds = new Bounds(camPos, Vector3.one * 100000f),
            };
            Graphics.RenderMeshInstanced(rp, _quad, 0, _mArr, n);
        }

        /// <summary>A placing ghost to draw when the game asks for none (the unattended check, SteamAutorun).</summary>
        public static System.Func<com.oddlabs.tt.render.BuildingGhostRenderer.Request> DebugPlacing;
        /// <summary>The building sites found for the last placing ghost.</summary>
        public int statSites = -1;
        bool _ghostWarned;

        // SelectableVisitor.getSelectionColor (its alpha is not used: SelectableShadowRenderer draws them opaque)
        static Color SelectionColor(Selectable s, com.oddlabs.tt.player.Player local, bool selected)
        {
            var owner = s.getOwnerNoCheck();
            float v = selected ? 1f : 0x7F / 255f;
            if (owner == local) return new Color(0f, v, 0f, 1f);
            if (local.isEnemy(owner)) return new Color(v, 0f, 0f, 1f);
            return new Color(0f, 0f, v, 1f);
        }

        // ------------------------------------------------------------------ placing

        void Placing(com.oddlabs.tt.render.BuildingGhostRenderer.Request req)
        {
            var template = req.template;
            var world = req.world;
            var grid = world.getUnitGrid();
            var hm = world.getHeightMap();
            int size = template.getPlacingSize();
            int pgx = req.gridX - (size - 1), pgy = req.gridY - (size - 1);
            float mpg = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            float cx = mpg * (pgx + (size - .5f)), cy = mpg * (pgy + (size - .5f));

            // the sites (BuildingSiteRenderer): 2 m tiles, white at the centre, green fading out over 2 x GRID_RADIUS
            const int GridRadius = 20;
            var filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(grid, template, GridRadius, false);
            grid.scan(filter, req.gridX, req.gridY);
            var targets = filter.getResult();
            float maxR = 2 * GridRadius, maxR2 = maxR * maxR;
            var site = Site();
            statSites = targets.size();
            for (int i = 0; i < targets.size(); i++)
            {
                if (!(targets.get(i) is com.oddlabs.tt.util.Target t)) continue;
                float dx = t.getPositionX() - cx, dy = t.getPositionY() - cy;
                float a = (dx * dx + dy * dy) / maxR2;
                var c = dx == 0f && dy == 0f ? new Color(1f, 1f, 1f, 1f) : new Color(0f, 1f, 0f, Mathf.Max(0f, 1f - a * a));
                Decal(_siteMat, site, t.getPositionX(), t.getPositionY(), 2f, c);
            }

            // the building: desaturated 0.5, white or red at 0.8 alpha
            if (_ghostMat == null) return;
            var key = template.getBuiltRenderer();
            var ghost = key != null ? GhostModel(key.drawn ?? key.location, key.texture) : null;
            if (ghost == null) return;
            bool legal = template.isPlacingLegal(grid, req.gridX, req.gridY);
            float z = hm.getNearestHeight(cx, cy);
            var m = TTSpace.ToUnity(Xf.TRz(cx, cy, z, 0f));
            _ghostBlock.Clear();
            _ghostBlock.SetTexture(BaseMapId, ghost.texture != null ? (Texture)ghost.texture : Texture2D.whiteTexture);
            _ghostBlock.SetColor(ColorId, legal ? new Color(1f, 1f, 1f, .8f) : new Color(1f, 0f, 0f, .8f));
            var rp = new RenderParams(_ghostMat) { matProps = _ghostBlock, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off, receiveShadows = false };
            foreach (var (mesh, local) in ghost.parts)
                for (int sm = 0; sm < mesh.subMeshCount; sm++)
                    Graphics.RenderMesh(rp, mesh, sm, m * local);
        }

        sealed class Ghost
        {
            public readonly List<(Mesh mesh, Matrix4x4 local)> parts = new List<(Mesh, Matrix4x4)>();
            public Texture2D texture;
        }
        readonly Dictionary<(string, int), Ghost> _ghosts = new Dictionary<(string, int), Ghost>();

        /// <summary>A sprite's meshes (its full-detail level) and texture, relative to the model's root.</summary>
        Ghost GhostModel(string location, int texture)
        {
            if (location == null) return null;
            if (_ghosts.TryGetValue((location, texture), out var g)) return g;
            g = null;
            var def = _view.Def(location);
            if (def != null && def.model != null && def.lods.Length > 0)
            {
                g = new Ghost();
                var root = def.model.transform;
                var node = FindChild(root, def.lods[0].nodeName) ?? root;
                var toRoot = root.worldToLocalMatrix;
                foreach (var mf in node.GetComponentsInChildren<MeshFilter>(true))
                    if (mf.sharedMesh != null) g.parts.Add((mf.sharedMesh, toRoot * mf.transform.localToWorldMatrix));
                foreach (var smr in node.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                    if (smr.sharedMesh != null) g.parts.Add((smr.sharedMesh, toRoot * smr.transform.localToWorldMatrix));
                var tex = def.lods[0].textures;
                if (tex.Length > 0)
                {
                    var v = tex[Mathf.Clamp(texture, 0, tex.Length - 1)];
                    g.texture = v.baseMap != null ? v.baseMap : TTGeneratedTextures.Get(v.name);
                }
                if (g.parts.Count == 0) g = null;
            }
            _ghosts[(location, texture)] = g;
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

        // ------------------------------------------------------------------ responding targets

        readonly List<Renderer> _tinted = new List<Renderer>();
        readonly List<Renderer> _tintedNow = new List<Renderer>();
        readonly MaterialPropertyBlock _tintBlock = new MaterialPropertyBlock();

        /// <summary>RespondManager.isResponding: units and buildings blink desaturated with a half decal
        /// (InstancedSpriteRenderer's respond batch), trees the same (TreeRenderer's respond list).</summary>
        void Responding(com.oddlabs.tt.render.DefaultRenderer dr)
        {
            _tintedNow.Clear();
            var rm = dr?.picker?.getRespondManager();
            var targets = rm?.respond_targets;
            if (targets != null && !targets.isEmpty())
            {
                var it = targets.keySet().iterator();
                while (it.hasNext())
                {
                    var target = it.next();
                    if (target == null || !rm.isResponding(target)) continue;
                    if (target is com.oddlabs.tt.landscape.TreeSupply tree) { _view.DrawRespondingTree(tree); continue; }
                    var rs = _view.LiveRenderers(target);
                    if (rs == null) continue;
                    foreach (var r in rs) if (r != null) _tintedNow.Add(r);
                }
            }
            foreach (var r in _tinted) if (r != null && !_tintedNow.Contains(r)) SetRespond(r, 0f);
            foreach (var r in _tintedNow) SetRespond(r, 1f);
            _tinted.Clear();
            _tinted.AddRange(_tintedNow);
        }

        void SetRespond(Renderer r, float v)
        {
            r.GetPropertyBlock(_tintBlock);
            _tintBlock.SetFloat(RespondId, v);
            r.SetPropertyBlock(_tintBlock);
        }

        public void Clear()
        {
            foreach (var r in _tinted) if (r != null) SetRespond(r, 0f);
            _tinted.Clear();
            foreach (var b in _batchList) { b.m.Clear(); b.c.Clear(); }
        }
    }
}
