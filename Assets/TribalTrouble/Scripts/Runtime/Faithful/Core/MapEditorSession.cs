// The map editor without its screen: your Java MapEditorLoader (world built from MapSettings or a saved .ttmap) and the
// non-GUI half of MapEditorDelegate (brushes, strokes, undo/redo, playable-area pruning, start positions, saving),
// plus GroundTextures' bookkeeping, which here hands the recomputed ground alphas to the view instead of to OpenGL.
// The brushes themselves are the translated TerrainEditor / ResourceLayer / StartLayer / AccessMap.
// No UnityEngine here, so it also runs in the headless test harness.
using System;
using System.Collections.Generic;
using com.oddlabs.tt.mapeditor;
using com.oddlabs.tt.landscape;

namespace TribalTrouble.Faithful
{
    public sealed class MapEditorSession
    {
        const float MinRadius = 4f, MaxRadius = 96f, RadiusStep = 1.15f;
        const int MaxIntensity = 100, IntensityStep = 5;
        const float ResourceDabInterval = .05f, GroundUpdateInterval = .1f, AccessUpdateInterval = .25f;
        public const int MaxPlayers = com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS;

        public readonly World world;
        public readonly com.oddlabs.tt.resource.WorldInfo info;
        public readonly MapSettings settings;
        public readonly float[][] heights;      // [y][x], the live array the height map reads
        public readonly int size;
        public readonly TerrainEditor editor;
        public readonly AccessMap access;
        public readonly ResourceLayer layer;
        public readonly StartLayer starts;
        readonly Ground _ground;
        readonly Snapper _snapper;

        public string mapName;
        bool _edited, _resourcesEdited, _resourcesModified, _startsModified;

        // ---- what the view needs to follow
        /// <summary>Heights reached the height map in this rectangle of cells (inclusive).</summary>
        public event Action<int, int, int, int> HeightsChanged;
        /// <summary>Ground alphas for a rectangle (inclusive): RGBA rows bottom up, alphas 0-3 and 4-6 (+0), as GroundAlphas.compute.</summary>
        public event Action<int, int, int, int, byte[], byte[]> AlphasChanged;
        /// <summary>Trees, rocks or iron came or went.</summary>
        public event Action ResourcesChanged;
        /// <summary>A message for the player (the Java info printer).</summary>
        public event Action<string> Info;

        // ---- tool state
        public Brush brush = Brush.HEIGHT;
        public float radius = 16f;
        public int intensity = 50;
        public TerrainEditor.Shape shape = TerrainEditor.Shape.CIRCLE;
        public int startPlayer, startTeam = -1;

        int _strokeSign;
        float _strokeZ, _rampX, _rampY, _resourceTimer, _groundTimer, _accessTimer;
        bool _groundSettle;
        int _randomSeed;
        readonly Random _random = new Random();
        ResourceLayer.Stroke _resourceStroke, _pruned;
        readonly LinkedList<object> _history = new LinkedList<object>(), _future = new LinkedList<object>();
        sealed class TerrainStep { public ResourceLayer.Stroke pruned; }

        public bool Stroking => _strokeSign != 0;
        public float StrokeZ => _strokeZ;
        public bool IsModified => editor.isModified() || _resourcesModified || _startsModified;
        public float MetersPerWorld => size * HeightMap.METERS_PER_UNIT_GRID;
        public float SeaLevel => info.sea_level_meters();
        /// <summary>Where the ramp drag began (meters), while one is in progress.</summary>
        public bool RampPending(out float x, out float y) { x = _rampX; y = _rampY; return _strokeSign != 0 && brush.isDragShape(); }

        public static string MapsDir => System.IO.Path.Combine(java.nio.file.Path.GameDirHook ?? ".", "maps");

        // ================================================================== building (MapEditorLoader.load)

        public static MapEditorSession New(int size, int terrain, int hills, int trees, int supplies, int seed)
            => new MapEditorSession(new MapSettings(size, terrain, hills, trees, supplies, seed), null, null, null, null);

        /// <summary>Runs a build (New / Load) on a thread with the big stack the island generator needs, like GameSession's loader.</summary>
        public static System.Threading.Thread BuildAsync(Func<MapEditorSession> build, Action<MapEditorSession, Exception> done)
        {
            var t = new System.Threading.Thread(() =>
            {
                MapEditorSession r = null; Exception error = null;
                try { r = build(); } catch (Exception e) { error = e; }
                done(r, error);
            }, 16 * 1024 * 1024) { IsBackground = true, Name = "Map editor loader" };   // a big stack makes every Mono GC crawl
            t.Start();
            return t;
        }

        public static MapSettings RandomSettings() => MapSettings.random(new java.util.Random());

        public static MapEditorSession Load(string path)
        {
            var map = MapFile.load(java.nio.file.Path_.of(path));
            return new MapEditorSession(map.settings(), map.name(), map.heights(), map.resources(), map.starts());
        }

        MapEditorSession(MapSettings settings, string name, float[][] savedHeights, MapFile.Resources resources, java.util.List savedStarts)
        {
            FaithfulMatch.InitGlobals();
            this.settings = settings;
            mapName = name;
            var generator = settings.createGenerator();
            var players = new[] { new com.oddlabs.tt.player.PlayerInfo(0, 0, "") };
            var wp = new WorldParameters(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL, "", 2, com.oddlabs.tt.player.Player.DEFAULT_MAX_UNIT_COUNT);
            info = generator.generate(players.Length, wp.getInitialUnitCount(), 0f, false, null);
            heights = info.heightmap();
            size = heights.Length;
            _edited = savedHeights != null && savedHeights.Length == heights.Length;
            if (_edited)
            {
                for (int y = 0; y < size; y++) Array.Copy(savedHeights[y], 0, heights[y], 0, size);
                TerrainEditor.pinEdges(heights);
            }
            if (resources != null)
            {
                var lists = new[] { info.trees(), info.palm_trees(), info.rocks(), info.iron() };
                foreach (var kind in Resource.values())
                {
                    lists[kind.ordinal()].clear();
                    lists[kind.ordinal()].addAll(resources.of(kind));
                }
            }
            _resourcesEdited = resources != null;

            var landscapeResources = World.loadCommon(new com.oddlabs.tt.render.RenderQueues());
            world = World.newWorld(com.oddlabs.tt.audio.AudioManager.getManager(), landscapeResources, null, new NoNotifications(), wp, info,
                generator.getTerrainType(), players, new org.joml.Vector4fc[] { com.oddlabs.tt.global.Settings.getSettings().team_colours[0] }, generator.getFogInfo());

            _ground = new Ground(this, resources == null);
            _snapper = new Snapper(world);
            access = new AccessMap(heights, settings);
            editor = new TerrainEditor(world.getHeightMap(), heights, new HeightListener(this));
            layer = new ResourceLayer(world, access, settings, new ResourceListener(this));
            starts = new StartLayer(size, savedStarts);
            access.compute();
            if (_edited || resources != null) _ground.RebuildAll();
        }

        // ================================================================== per frame (MapEditorDelegate.tick)

        /// <summary>Advances one frame. cursor: ground under the mouse in meters (has = false when off the island).</summary>
        public void Tick(float dt, bool hasCursor, float cursorX, float cursorY)
        {
            if (_strokeSign != 0 && hasCursor && !brush.isDragShape()) Paint(dt, cursorX, cursorY);
            editor.flush();
            UpdateGround(dt);
            UpdateAccess(dt);
        }

        static float ToGrid(float meters) => meters / HeightMap.METERS_PER_UNIT_GRID;

        public float HeightAt(float mx, float my) => editor.getHeight(ToGrid(mx), ToGrid(my));

        public void SelectBrush(Brush b) { CancelStroke(); brush = b; }
        public void SetShape(TerrainEditor.Shape s) { shape = s; editor.setShape(s); }
        public void ScaleRadius(int steps) => radius = Math.Clamp(steps > 0 ? radius * RadiusStep : radius / RadiusStep, MinRadius, MaxRadius);
        public void StepIntensity(int steps) => intensity = Math.Clamp(intensity + steps * IntensityStep, 0, MaxIntensity);
        public string BrushName(Brush b) => b.getName(com.oddlabs.tt.procedural.Landscape.TerrainType.values()[settings.terrain()]);

        public void BeginStroke(int sign, bool hasCursor, float cursorX, float cursorY)
        {
            if (_strokeSign != 0 || !hasCursor) return;
            if (brush.isPlacementBrush())
            {
                float gx = ToGrid(cursorX), gy = ToGrid(cursorY);
                if (sign > 0) starts.place(startPlayer, (int)Math.Round(gx), (int)Math.Round(gy), startTeam);
                else if (!starts.removeNearest(gx, gy, Math.Max(4f, ToGrid(radius)))) { Info?.Invoke(MapEditor.i18n("no_start_here")); return; }
                _startsModified = true;
                return;
            }
            _strokeSign = sign;
            if (brush.isDragShape()) { _rampX = cursorX; _rampY = cursorY; return; }
            _strokeZ = editor.getHeight(ToGrid(cursorX), ToGrid(cursorY));
            if (brush.isResourceBrush())
            {
                _resourceStroke = new ResourceLayer.Stroke();
                _resourceTimer = ResourceDabInterval;
                return;
            }
            _randomSeed = _random.Next();
            BeginTerrainStroke();
        }

        public void EndStroke(bool hasCursor, float cursorX, float cursorY)
        {
            if (_strokeSign == 0) return;
            _groundSettle = true;
            if (brush.isResourceBrush()) FinishResourceStroke();
            else if (brush.isDragShape())
            {
                if (hasCursor)
                {
                    float ax = ToGrid(_rampX), ay = ToGrid(_rampY), bx = ToGrid(cursorX), by = ToGrid(cursorY);
                    BeginTerrainStroke();
                    editor.applyRamp(ax, ay, editor.getHeight(ax, ay), bx, by, editor.getHeight(bx, by), ToGrid(radius), intensity / 100f, _strokeSign);
                    FinishTerrainStroke();
                }
            }
            else FinishTerrainStroke();
            _strokeSign = 0;
        }

        public void CancelStroke()
        {
            if (_strokeSign != 0 && brush.isResourceBrush()) { FinishResourceStroke(); _groundSettle = true; }
            else if (_strokeSign != 0 && !brush.isDragShape()) { FinishTerrainStroke(); _groundSettle = true; }
            _strokeSign = 0;
        }

        void Remember(object step)
        {
            _future.Clear();
            editor.clearRedo();
            _history.AddFirst(step);
            while (_history.Count > TerrainEditor.MAX_UNDO_STEPS) _history.RemoveLast();
        }

        void BeginTerrainStroke() { _pruned = new ResourceLayer.Stroke(); editor.beginStroke(); }

        void FinishTerrainStroke()
        {
            var stroke = _pruned;
            if (editor.endStroke() && stroke != null) Remember(new TerrainStep { pruned = stroke });
            else _pruned = null;
        }

        void FinishResourceStroke()
        {
            var stroke = _resourceStroke;
            _resourceStroke = null;
            if (stroke != null && !stroke.isEmpty()) { Remember(stroke); _resourcesEdited = true; _resourcesModified = true; }
        }

        void Paint(float t, float cx, float cy)
        {
            if (brush.isResourceBrush())
            {
                var resource = brush.getResource();
                _resourceTimer += t;
                if (_resourceTimer < ResourceDabInterval || _resourceStroke == null) return;
                _resourceTimer = 0f;
                if (resource != null && _strokeSign > 0) layer.paint(resource, cx, cy, radius, intensity / 100f, _resourceStroke);
                else layer.erase(resource, cx, cy, radius, _resourceStroke);
                return;
            }
            float gx = ToGrid(cx), gy = ToGrid(cy), r = ToGrid(radius), s = intensity / 100f;
            if (brush == Brush.HEIGHT) editor.applyHeight(gx, gy, r, s, _strokeSign, t);
            else if (brush == Brush.FLATTEN) editor.applyFlatten(gx, gy, r, s, _strokeSign, t, _strokeZ);
            else if (brush == Brush.SMOOTH) editor.applySmooth(gx, gy, r, s, _strokeSign, t);
            else if (brush == Brush.RANDOM) editor.applyRandom(gx, gy, r, s, _strokeSign, t, _randomSeed);
            else if (brush == Brush.CLIFF) editor.applyCliff(gx, gy, r, s, _strokeSign, t);
            else if (brush == Brush.PLATEAU) editor.applyPlateau(gx, gy, r, _strokeZ);
        }

        public void Undo()
        {
            CancelStroke();
            _groundSettle = true;
            object step = _history.First?.Value;
            if (step != null) _history.RemoveFirst();
            _pruned = null;
            if (step is ResourceLayer.Stroke stroke) { layer.undo(stroke); _resourcesModified = true; _future.AddFirst(step); }
            else if (step is TerrainStep terrain && editor.undo())
            {
                _future.AddFirst(step);
                if (!terrain.pruned.isEmpty()) { layer.undo(terrain.pruned); _resourcesModified = true; }
            }
            else Info?.Invoke(MapEditor.i18n("nothing_to_undo"));
        }

        public void Redo()
        {
            CancelStroke();
            _groundSettle = true;
            object step = _future.First?.Value;
            if (step != null) _future.RemoveFirst();
            _pruned = null;
            if (step is ResourceLayer.Stroke stroke) { layer.redo(stroke); _resourcesModified = true; _history.AddFirst(step); }
            else if (step is TerrainStep terrain && editor.redo())
            {
                if (!terrain.pruned.isEmpty()) { layer.redo(terrain.pruned); _resourcesModified = true; }
                _history.AddFirst(step);
            }
            else Info?.Invoke(MapEditor.i18n("nothing_to_redo"));
        }

        public void AutoPlaceStarts(int count) { starts.autoPlace(count); _startsModified = true; }

        /// <summary>Start positions: (player, x meters, y meters, team).</summary>
        public IEnumerable<(int player, float x, float y, int team)> Starts()
        {
            var all = starts.all();
            for (int i = 0; i < all.size(); i++)
            {
                var s = (StartLayer.Start)all.get(i);
                yield return (s.player(), s.x() * HeightMap.METERS_PER_UNIT_GRID, s.y() * HeightMap.METERS_PER_UNIT_GRID, s.team());
            }
        }

        void UpdateGround(float t)
        {
            _groundTimer += t;
            if (_strokeSign == 0 && _groundSettle) { _groundSettle = false; _ground.Settle(); }
            else if (_ground.HasChanges && _groundTimer >= GroundUpdateInterval) { _groundTimer = 0f; _ground.Update(); }
        }

        void UpdateAccess(float t)
        {
            _accessTimer += t;
            if (access.isStale() && (_strokeSign == 0 || _accessTimer >= AccessUpdateInterval))
            {
                _accessTimer = 0f;
                access.compute();
                if (layer.prune(_pruned ?? new ResourceLayer.Stroke()))
                {
                    _resourcesEdited = true;
                    _resourcesModified = true;
                    _groundSettle = true;
                }
                AccessChanged = true;
            }
        }

        /// <summary>Set when the playable area was recomputed (the view clears it after redrawing its overlay).</summary>
        public bool AccessChanged;

        /// <summary>Playable-area colour index of a cell: 0 none, 1 playable, 2 cut off, 3 too steep.</summary>
        public int AccessKind(int x, int y) => access.get(x, y).ordinal();

        // ================================================================== saving (MapEditorDelegate.save)

        public static bool IsValidName(string name) => MapFile.isValidName(name ?? "");

        public string Save(string name)
        {
            CancelStroke();
            bool keepHeights = _edited || editor.isModified();
            var positions = new java.util.List[Resource.values().Length];
            foreach (var kind in Resource.values()) positions[kind.ordinal()] = layer.positions(kind);
            var current = new MapFile.Resources(positions);
            float[][] copy = editor.copyHeights();
            var preview = MapPreview.render(copy, settings, current);
            new MapFile(name, settings, keepHeights ? copy : null, _resourcesEdited ? current : null, preview, starts.toSaved())
                .save(java.nio.file.Path_.of(MapsDir));
            mapName = name;
            _edited = keepHeights;
            editor.markSaved();
            _resourcesModified = false;
            _startsModified = false;
            Info?.Invoke(MapEditor.i18n("saved", name));
            return System.IO.Path.Combine(MapsDir, name + MapFile.EXTENSION);
        }

        public string MapCode => settings.toMapcode();

        // ================================================================== listeners

        sealed class HeightListener : java.lang.Object, TerrainEditor.ChangeListener
        {
            readonly MapEditorSession s;
            public HeightListener(MapEditorSession s) { this.s = s; }
            public void heightsChanged(int x0, int y0, int x1, int y1)
            {
                s._snapper.Snap(x0, y0, x1, y1);
                s._ground.HeightsChanged(x0, y0, x1, y1);
                s.access.heightsChanged();
                s.HeightsChanged?.Invoke(x0, y0, x1, y1);
            }
        }

        sealed class ResourceListener : java.lang.Object, ResourceLayer.ChangeListener
        {
            readonly MapEditorSession s;
            public ResourceListener(MapEditorSession s) { this.s = s; }
            public void resourcesChanged(ResourceLayer layer, int x0, int y0, int x1, int y1)
            {
                s._ground?.ResourcesChanged(layer, x0, y0, x1, y1);
                s.ResourcesChanged?.Invoke();
            }
        }

        sealed class NoNotifications : java.lang.Object, NotificationListener
        {
            public void newAttackNotification(com.oddlabs.tt.model.Selectable target) { }
            public void newSelectableNotification(com.oddlabs.tt.model.Selectable target) { }
            public void registerTarget(com.oddlabs.tt.util.Target target) { }
            public void unregisterTarget(com.oddlabs.tt.util.Target target) { }
            public void patchesEdited(int patch_x0, int patch_y0, int patch_x1, int patch_y1) { }
            public void gamespeedChanged(int speed) { }
            public void playerGamespeedChanged() { }
        }

        // ================================================================== GroundTextures (bookkeeping only)

        sealed class Ground
        {
            const int ShadowBlend = 6;
            readonly MapEditorSession s;
            readonly GroundAlphas alphas;
            readonly bool[] unshaded;
            readonly int size;
            int dx0 = int.MaxValue, dy0 = int.MaxValue, dx1 = int.MinValue, dy1 = int.MinValue;

            public Ground(MapEditorSession s, bool generatedSupplies)
            {
                this.s = s;
                var info = s.info;
                size = s.heights.Length;
                unshaded = new bool[size * size];
                var shaded = new java.util.ArrayList();
                var shadow = info.blend_infos()[ShadowBlend].getSourceImage();
                foreach (var supplies in new[] { info.rocks(), info.iron() })
                    for (int i = 0; i < supplies.size(); i++)
                    {
                        var p = (int[])supplies.get(i);
                        if (!generatedSupplies || shadow.getPixel(p[0], p[1]) >= 128) shaded.add(p);
                        else unshaded[p[1] * size + p[0]] = true;
                    }
                alphas = new GroundAlphas(s.heights, s.settings, info.trees(), info.palm_trees(), shaded);
            }

            public bool HasChanges => dx0 <= dx1;

            public void HeightsChanged(int x0, int y0, int x1, int y1) { alphas.heightsChanged(x0, y0, x1, y1); MarkDirty(x0, y0, x1, y1); }

            public void ResourcesChanged(ResourceLayer layer, int x0, int y0, int x1, int y1)
            {
                int reach = alphas.getSupplyShadowReach();
                int sx0 = x0 - reach, sy0 = y0 - reach, sx1 = x1 + reach, sy1 = y1 + reach;
                var shaded = new java.util.ArrayList();
                foreach (var kind in new[] { Resource.ROCK, Resource.IRON })
                {
                    var l = layer.positionsIn(kind, sx0, sy0, sx1, sy1);
                    for (int i = 0; i < l.size(); i++) { var p = (int[])l.get(i); if (!unshaded[p[1] * size + p[0]]) shaded.add(p); }
                }
                for (int y = Math.Max(0, y0); y <= Math.Min(size - 1, y1); y++)
                    for (int x = Math.Max(0, x0); x <= Math.Min(size - 1, x1); x++)
                        if (layer.get(x, y) != Resource.ROCK && layer.get(x, y) != Resource.IRON) unshaded[y * size + x] = false;
                alphas.restampSupplies(layer.positionsIn(Resource.TREE, sx0 - reach, sy0 - reach, sx1 + reach, sy1 + reach),
                    layer.positionsIn(Resource.PALM, sx0 - reach, sy0 - reach, sx1 + reach, sy1 + reach), shaded, sx0, sy0, sx1, sy1);
                MarkDirty(sx0, sy0, sx1, sy1);
            }

            void MarkDirty(int x0, int y0, int x1, int y1) { dx0 = Math.Min(dx0, x0); dy0 = Math.Min(dy0, y0); dx1 = Math.Max(dx1, x1); dy1 = Math.Max(dy1, y1); }

            public void Update()
            {
                if (!HasChanges) return;
                int[] r = alphas.reach(dx0, dy0, dx1, dy1);
                dx0 = dy0 = int.MaxValue; dx1 = dy1 = int.MinValue;
                Rebuild(r[0], r[1], r[2], r[3]);
            }

            public void Settle() { Update(); if (alphas.rangesMoved()) Rebuild(0, 0, size - 1, size - 1); }

            public void RebuildAll() { alphas.rangesMoved(); Rebuild(0, 0, size - 1, size - 1); }

            void Rebuild(int x0, int y0, int x1, int y1)
            {
                if (s.AlphasChanged == null) return;
                int n = (x1 - x0 + 1) * (y1 - y0 + 1) * 4;
                var low = java.nio.ByteBuffer.allocate(n);
                var high = java.nio.ByteBuffer.allocate(n);
                alphas.compute(x0, y0, x1, y1, low, high);
                s.AlphasChanged(x0, y0, x1, y1, (byte[])(Array)low.array(), (byte[])(Array)high.array());
            }
        }

        /// <summary>Saved heights or resources: the generator's ground texture doesn't match them (MapEditorLoader rebuilds it).</summary>
        public bool NeedsGroundRebuild => _edited || _resourcesEdited;

        /// <summary>Recomputes the whole island's ground alphas now (the view calls this once it is listening).</summary>
        public void RebuildGround() => _ground.RebuildAll();

        // ================================================================== ResourceSnapper

        sealed class Snapper : java.lang.Object, com.oddlabs.tt.model.ElementNodeVisitor, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            const float Margin = 4f;
            readonly World world;
            readonly HeightMap hm;
            readonly List<com.oddlabs.tt.model.Model> models = new List<com.oddlabs.tt.model.Model>();
            float x0, y0, x1, y1;

            public Snapper(World world) { this.world = world; hm = world.getHeightMap(); }

            public void Snap(int gx0, int gy0, int gx1, int gy1)
            {
                x0 = gx0 * HeightMap.METERS_PER_UNIT_GRID - Margin; y0 = gy0 * HeightMap.METERS_PER_UNIT_GRID - Margin;
                x1 = gx1 * HeightMap.METERS_PER_UNIT_GRID + Margin; y1 = gy1 * HeightMap.METERS_PER_UNIT_GRID + Margin;
                world.getElementRoot().visit(this);
                foreach (var m in models) m.setPosition(m.getPositionX(), m.getPositionY());
                models.Clear();
                SnapTrees(world.getTreeRoot());
            }

            bool Inside(float x, float y) => x >= x0 && x <= x1 && y >= y0 && y <= y1;
            bool Overlaps(com.oddlabs.tt.util.BoundingBox b) => b.bmin_x <= x1 && b.bmax_x >= x0 && b.bmin_y <= y1 && b.bmax_y >= y0;

            public void visitNode(com.oddlabs.tt.model.ElementNode node) { if (Overlaps(node)) { node.visitElements(this); node.visitChildren(this); } }
            public void visitLeaf(com.oddlabs.tt.model.ElementLeaf leaf) { if (Overlaps(leaf)) leaf.visitElements(this); }
            public void visit(com.oddlabs.tt.model.Element e)
            {
                if ((e is com.oddlabs.tt.model.SupplyModel || e is com.oddlabs.tt.model.SceneryModel) && e is com.oddlabs.tt.model.Model m && Inside(m.getPositionX(), m.getPositionY()))
                    models.Add(m);
            }

            bool SnapTrees(com.oddlabs.tt.landscape.AbstractTreeGroup group)
            {
                bool hasBounds = group.bmin_x <= group.bmax_x;
                if (!hasBounds || !Overlaps(group)) return hasBounds;
                if (group is com.oddlabs.tt.landscape.TreeLeaf leaf) return SnapLeaf(leaf);
                var node = (com.oddlabs.tt.landscape.TreeGroup)group;
                bool nodeBounds = false;
                foreach (var child in new[] { node.getChild0(), node.getChild1(), node.getChild2(), node.getChild3() })
                {
                    if (!SnapTrees(child)) continue;
                    if (nodeBounds) node.checkBounds(child); else { node.setBounds(child); nodeBounds = true; }
                }
                return nodeBounds;
            }

            com.oddlabs.tt.landscape.TreeLeaf _leaf;
            bool _leafBounds;
            bool SnapLeaf(com.oddlabs.tt.landscape.TreeLeaf leaf)
            {
                _leaf = leaf; _leafBounds = false;
                leaf.visitTrees(this);
                return _leafBounds;
            }
            public void visitTree(com.oddlabs.tt.landscape.TreeSupply tree)
            {
                if (Inside(tree.getPositionX(), tree.getPositionY()))
                {
                    float z = hm.getNearestHeight(tree.getPositionX(), tree.getPositionY());
                    var m = tree.getMatrix();
                    float dz = z - m._m32;
                    m._m32 = z;
                    tree.bmin_z += dz;
                    tree.bmax_z += dz;
                }
                if (_leafBounds) _leaf.checkBounds(tree); else { _leaf.setBounds(tree); _leafBounds = true; }
            }
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf l) => throw new InvalidOperationException();
            public void visitNode(com.oddlabs.tt.landscape.TreeGroup g) => throw new InvalidOperationException();
        }
    }
}
