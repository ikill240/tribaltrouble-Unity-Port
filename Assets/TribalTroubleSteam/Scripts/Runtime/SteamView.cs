using System.Collections.Generic;
using TribalTroubleSteam.Presentation;
using UnityEngine;
using com.oddlabs.tt.model;

namespace TribalTroubleSteam
{
    /// <summary>
    /// Draws the Steam release's simulation as its own renderer would: each frame takes the SteamScene snapshot and
    /// maps sprite locations onto the Steam sprite catalog (the Steam release's own models). Animated or team-coloured
    /// models (units and the items they hold, buildings, chickens, weapons, carried resources) get a pooled
    /// TTSpriteView keyed by the sim object; units far away are drawn in bulk from baked poses; static scenery (trees,
    /// plants, rocks) is GPU-instanced. At night the registry's glow lights (torches, windows) light their buildings.
    /// The 1.4 / New Tides view (FaithfulView) is a separate class of the other game; nothing here is shared with it.
    /// </summary>
    public sealed class SteamView
    {
        sealed class Entry
        {
            public GameObject go;
            public TTSpriteView view;
            public string location;
            public int texture;
            public int anim = int.MinValue;
            public float ticks = float.NaN;
            public Color team;
            public int lod = -1;
            public int seen;
            public int phase = -1;   // which of three frames a far unit's pose is updated on
            public bool lights;      // its glow lights were looked for (night)
        }

        sealed class Batch
        {
            public Mesh mesh;
            public int submesh;
            public Material material;
            public Matrix4x4 local;
            public bool shared;      // material owned by another batch
            public bool noShadows;   // beyond the shadow distance: don't cast
            public MaterialPropertyBlock props;   // far units: the team colour of the whole batch
            public Bounds bounds;
            public bool hasBounds;
            public readonly MatrixBuf matrices = new MatrixBuf();
        }

        /// <summary>The instance matrices of a batch, written straight into the array that is drawn.</summary>
        sealed class MatrixBuf
        {
            public Matrix4x4[] array = new Matrix4x4[256];
            public int Count;
            public void Clear() => Count = 0;
            public void AddRange(Matrix4x4[] src)
            {
                int n = src.Length;
                if (Count + n > array.Length) System.Array.Resize(ref array, System.Math.Max(array.Length * 2, Count + n));
                System.Array.Copy(src, 0, array, Count, n);
                Count += n;
            }
            public void Add(in Matrix4x4 m)
            {
                if (Count == array.Length) System.Array.Resize(ref array, array.Length * 2);
                array[Count++] = m;
            }
        }

        public readonly SteamScene scene = new SteamScene();
        public readonly SteamParticles particles = new SteamParticles();
        readonly TTSpriteCatalog _catalog;
        readonly Transform _root;
        readonly Dictionary<string, TTSpriteDefinition> _defs = new Dictionary<string, TTSpriteDefinition>();
        readonly Dictionary<object, TTSpriteDefinition> _defsByRef = new Dictionary<object, TTSpriteDefinition>(RefEq.Instance);
        readonly Dictionary<object, Entry> _live = new Dictionary<object, Entry>(1024, RefEq.Instance);
        readonly List<object> _dead = new List<object>();
        readonly Dictionary<(TTSpriteDefinition, int), Batch> _batches = new Dictionary<(TTSpriteDefinition, int), Batch>();
        readonly List<Batch> _batchList = new List<Batch>();
        readonly HashSet<string> _missing = new HashSet<string>();
        int _frame;

        public float lodDistance = 70f;
        const float AnimLodDistance = 90f;
        public float plantsCutDistance = 200f;   // RenderState.PLANTS_CUT_DIST
        public float treeDrawDistance = 900f;
        public float shadowDistance = 120f;
        static readonly Color IronTint = new Color(0.55f, 0.42f, 0.36f);

        /// <summary>Night (WorldParameters.night): the registry's glow lights are lit on the buildings.</summary>
        public bool night;

        public SteamView(TTSpriteCatalog catalog, Transform root, Material spriteMaterial = null)
        {
            _catalog = catalog;
            _root = root;
            _spriteMaterial = spriteMaterial;
        }

        readonly Material _spriteMaterial;
        /// <summary>The material every sprite is drawn with a copy of: the catalog's, on the Steam shader (SteamShading).</summary>
        Material BaseMaterial => _spriteMaterial != null ? _spriteMaterial : _catalog.baseMaterial;

        /// <summary>The selection halos, blob shadows, click rings, placing ghost and rally flag (SteamWorldOverlay).</summary>
        public SteamWorldOverlay overlay;

        /// <summary>"res:/geometry/natives/peon.binsprite" -> ("natives", "peon").</summary>
        public static bool ParseLocation(string location, out string group, out string name)
        {
            group = name = null;
            if (string.IsNullOrEmpty(location)) return false;
            string s = location;
            int colon = s.IndexOf(':');
            if (colon >= 0 && colon < 6) s = s.Substring(colon + 1);
            s = s.Replace('\\', '/');
            int g = s.IndexOf("/geometry/");
            if (g >= 0) s = s.Substring(g + 10);
            s = s.TrimStart('/');
            int slash = s.LastIndexOf('/');
            if (slash < 0) return false;
            group = s.Substring(0, slash);
            name = s.Substring(slash + 1);
            int dot = name.LastIndexOf('.');
            if (dot > 0) name = name.Substring(0, dot);
            return true;
        }

        public TTSpriteDefinition Def(string location)
        {
            if (location == null) return null;
            if (_defsByRef.TryGetValue(location, out var d)) return d;
            if (!_defs.TryGetValue(location, out d))
            {
                d = null;
                if (_catalog != null && ParseLocation(location, out var group, out var name)) d = _catalog.Find(group, name);
                if (d == null && _missing.Add(location)) Debug.LogWarning("[Tribal Trouble] No sprite in the Steam catalog for " + location);
                _defs[location] = d;
            }
            if (_defsByRef.Count > 8192) _defsByRef.Clear();
            _defsByRef[location] = d;
            return d;
        }

        TTSpriteDefinition TreeDef(string treeName, string part)
        {
            string key = "tree:" + treeName + part;
            if (_defs.TryGetValue(key, out var d)) return d;
            d = _catalog != null ? _catalog.Find("misc", treeName + part) : null;
            if (d == null && _missing.Add(key)) Debug.LogWarning("[Tribal Trouble] No tree sprite misc/" + treeName + part);
            _defs[key] = d;
            return d;
        }

        /// <summary>Animated / skinned or team-coloured models get their own GameObject.</summary>
        static bool NeedsObject(RKind kind, TTSpriteDefinition def) =>
            kind == RKind.Unit || kind == RKind.Building || kind == RKind.Rubber || kind == RKind.Weapon || kind == RKind.Carry
            || kind == RKind.Attach || def.skinned || def.animations.Length > 0;

        /// <summary>Draw trees.</summary>
        public bool drawTrees = true;

        readonly Plane[] _cullPlanes = new Plane[6];
        Vector3 _cullCamera;
        float _cullDistance = 900f;
        bool _viewCull;
        float _viewCull2;
        const float ShadowMargin = 14f;   // what stands just outside the view may still throw its shadow into it

        bool CullTest(float x0, float y0, float z0, float x1, float y1, float z1)
        {
            var bounds = new Bounds(new Vector3((x0 + x1) * .5f, (y0 + y1) * .5f, (z0 + z1) * .5f), new Vector3(x1 - x0, y1 - y0, z1 - z0));
            if (bounds.SqrDistance(_cullCamera) > _cullDistance * _cullDistance) return false;
            return GeometryUtility.TestPlanesAABB(_cullPlanes, bounds);
        }

        /// <summary>Whether a sphere (Unity space) is in the view (with a margin for shadows) and within the draw distance.</summary>
        /// <summary>Whether a sphere (Unity space) is in this frame's view.</summary>
        public bool IsInView(Vector3 p, float radius) => InView(p, radius);

        // ---- for the overlay: responding targets (RespondManager) blink desaturated with a half team decal

        /// <summary>The renderers of the object drawn this frame for a sim object (units, buildings..), or null.</summary>
        public Renderer[] LiveRenderers(object key)
        {
            if (key == null || !_live.TryGetValue(key, out var e) || e.seen != _frame || e.go == null || !e.go.activeInHierarchy) return null;
            return e.go.GetComponentsInChildren<Renderer>(false);
        }

        readonly Dictionary<Material, Material> _respondMaterials = new Dictionary<Material, Material>();

        Material RespondMaterial(Material m)
        {
            if (m == null) return null;
            if (_respondMaterials.TryGetValue(m, out var r)) return r;
            r = new Material(m) { name = m.name + " (responding)", hideFlags = HideFlags.DontSave, enableInstancing = true };
            r.SetFloat("_Respond", 1f);
            r.SetFloat("_Desaturate", 0.5f);
            r.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry + 1;   // over the tree drawn as usual
            _respondMaterials[m] = r;
            return r;
        }

        /// <summary>A standing tree drawn again over itself as responding (TreeRenderer's respond list).</summary>
        public void DrawRespondingTree(com.oddlabs.tt.landscape.TreeSupply tree)
        {
            if (tree == null || tree.isHidden()) return;
            var e = MakeTreeEntry(SteamScene.StandingTree(tree));
            for (int k = 0; k < 2; k++)
            {
                var def = k == 0 ? e.trunk : e.crown;
                if (def == null || def.model == null) continue;
                var b = InstanceBatch(def, 0, true);
                if (b == null || b.mesh == null) continue;
                var mat = RespondMaterial(b.material);
                Graphics.RenderMesh(new RenderParams(mat) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off }, b.mesh, b.submesh, e.m * b.local);
            }
        }

        bool InView(Vector3 p, float radius)
        {
            if (!_viewCull) return true;
            if ((p - _cullCamera).sqrMagnitude > _viewCull2) return false;
            float r = -(radius + ShadowMargin);
            for (int i = 0; i < 6; i++)
                if (_cullPlanes[i].GetDistanceToPoint(p) < r) return false;
            return true;
        }

        /// <summary>Per-frame counters for the F9 performance overlay.</summary>
        public int statTrees, statItems, statInstances, statCulled, statFar, statStatic, statNear;
        public float statCollectMs, statBuildMs, statDrawMs;
        public int statLive => _live.Count;
        readonly System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();

        public void Render(com.oddlabs.tt.landscape.World world, Camera cam)
        {
            _frame++;
            if (cam != null)
            {
                GeometryUtility.CalculateFrustumPlanes(cam, _cullPlanes);
                _cullCamera = cam.transform.position;
                _cullDistance = Mathf.Max(treeDrawDistance, cam.transform.position.y * 3f);
                scene.cullTest ??= CullTest;
            }
            else scene.cullTest = null;
            _viewCull = cam != null;
            _viewCull2 = _cullDistance * _cullDistance;
            _sw.Restart();
            if (!ReferenceEquals(_treeWorld, world)) { _treeCache.Clear(); _leafCache.Clear(); _leafGen++; _treeWorld = world; }
            _leafOn = cam != null && drawTrees;
            _visibleLeaves.Clear();
            scene.leafHandler = _leafHandler ??= HandleLeaf;
            scene.staticHandler = _staticHandler ??= HandleStatic;
            statStatic = 0;
            if (!ReferenceEquals(_staticWorld, world)) { _static.Clear(); _staticWorld = world; }
            float shadow2 = SteamGlobals.DynamicShadows ? shadowDistance * shadowDistance : 0f;
            _staticShadow2 = shadow2;
            _staticCam = cam != null ? cam.transform.position : Vector3.zero;
            foreach (var b in _batchList) b.matrices.Clear();
            var camState = TribalTroubleSteam.GuiPort.Host.CurrentCamera;
            scene.mapMode = camState != null && camState.inNoDetailMode();
            scene.localPlayer = TribalTroubleSteam.GuiPort.Host.CurrentWorld?.localPlayer;
            scene.Collect(world, drawTrees);
            overlay?.AddToScene(scene);   // the rally flag, drawn like any sprite
            statCollectMs = (float)_sw.Elapsed.TotalMilliseconds;
            _sw.Restart();
            if (!drawTrees) scene.trees.Clear();
            Vector3 camPos = _staticCam;
            float plantCut2 = plantsCutDistance * plantsCutDistance;
            statCulled = 0; statFar = 0;
            TTPoseBaker.BeginFrame();
            // a crowd close to the camera: only the nearest MaxNearUnits are skinned objects of their own
            if (_nearCount > MaxNearUnits) _nearScale = Mathf.Max(0.35f, _nearScale * 0.92f);
            else if (_nearCount < MaxNearUnits * 0.7f) _nearScale = Mathf.Min(1f, _nearScale * 1.02f);
            statNear = _nearCount;
            _nearCount = 0;
            _nearR2 = FarUnitDistance * _nearScale * FarUnitDistance * _nearScale;
            var items = scene.items;
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                var def = Def(it.location);
                if (def == null || def.model == null) continue;
                SteamSpriteLook.Note(def, it.kind);
                var m = TTSpace.ToUnity(it.xf);
                // units, buildings and what they carry, hold or throw, well outside the view: nothing to update or draw
                if (it.kind != RKind.Plants && it.kind != RKind.Supply && it.kind != RKind.Scenery && it.kind != RKind.Tree
                    && !InView(new Vector3(m.m03, m.m13, m.m23), it.kind == RKind.Building || it.kind == RKind.Prop ? 24f : 6f))
                {
                    if (_live.TryGetValue(it.key, out var hidden))
                    {
                        hidden.seen = _frame;
                        if (hidden.go != null && hidden.go.activeSelf) hidden.go.SetActive(false);
                    }
                    statCulled++;
                    continue;
                }
                if ((it.kind == RKind.Unit || it.kind == RKind.Carry || it.kind == RKind.Attach) && DrawFar(it, def, m, camPos, shadow2)) continue;
                if (NeedsObject(it.kind, def))
                {
                    var e = Place(it, def, m, camPos);
                    if (night && it.kind == RKind.Building && e.go != null && !e.lights) { e.lights = true; AddGlowLights(e.go, it.location, it.texture); }
                }
                else
                {
                    float id2 = (new Vector3(m.m03, m.m13, m.m23) - camPos).sqrMagnitude;
                    if (it.model is SceneryModel)   // plants and scenery never move: worked out once (HandleStatic)
                    {
                        var bS = InstanceBatch(def, it.texture, true);
                        var bN = InstanceBatch(def, it.texture, false);
                        if (bS != null && bN != null)
                            _static[it.model] = new StaticEntry { shadow = bS, noShadow = bN, mm = m * bS.local, pos = new Vector3(m.m03, m.m13, m.m23), plants = it.kind == RKind.Plants,
                                x = it.model.getPositionX(), y = it.model.getPositionY() };
                    }
                    if (it.kind == RKind.Plants && id2 > plantCut2) continue;
                    AddInstance(def, it.texture, m, id2 < shadow2);
                }
            }

            // trees falling or growing back, one by one (standing ones come from the leaves' caches)
            var trees = scene.trees;
            statTrees = trees.Count; statItems = scene.items.Count;
            for (int i = 0; i < trees.Count; i++)
            {
                var t = trees[i];
                var e = MakeTreeEntry(t);
                bool cast = (e.pos - camPos).sqrMagnitude < shadow2;
                if (e.trunk != null && e.trunk.model != null) AddInstance(e.trunk, 0, e.m, cast);
                if (e.crown != null && e.crown.model != null) AddInstance(e.crown, 0, e.m, cast);
            }
            if (drawTrees)
            {
                foreach (var b in _leafBatchList) { b.matrices.Clear(); b.hasBounds = false; }
                for (int i = 0; i < _visibleLeaves.Count; i++)
                {
                    var lc = _visibleLeaves[i];
                    statTrees += lc.count;
                    bool cast = (lc.center - camPos).sqrMagnitude < shadow2;
                    for (int p = 0; p < lc.keys.Length; p++)
                    {
                        var b = LeafBatch(lc.keys[p], cast);
                        if (b == null) continue;
                        b.matrices.AddRange(lc.mats[p]);
                        if (!b.hasBounds) { b.bounds = lc.bounds; b.hasBounds = true; } else b.bounds.Encapsulate(lc.bounds);
                    }
                }
            }

            // retire objects whose sim element is gone
            _dead.Clear();
            foreach (var kv in _live) if (kv.Value.seen != _frame) _dead.Add(kv.Key);
            foreach (var k in _dead)
            {
                var e = _live[k];
                // out of the view (not collected) but alive: kept, switched off, for a while
                if (k is Selectable sel && !sel.isDead() && _frame - e.seen < 1800)
                {
                    if (e.go != null && e.go.activeSelf) e.go.SetActive(false);
                    continue;
                }
                if (e.go != null) Object.Destroy(e.go);
                _live.Remove(k);
            }
            statBuildMs = (float)_sw.Elapsed.TotalMilliseconds;
            _sw.Restart();
            DrawBatches();
            if (cam != null && overlay != null) overlay.Draw(scene, cam);
            if (cam != null) particles.Render(scene, cam);
            statDrawMs = (float)_sw.Elapsed.TotalMilliseconds;
        }

        Entry Place(in Renderable it, TTSpriteDefinition def, Matrix4x4 m, Vector3 camPos)
        {
            if (!_live.TryGetValue(it.key, out var e))
            {
                e = new Entry();
                e.go = new GameObject(it.kind + " " + def.Key);
                e.go.transform.SetParent(_root, false);
                e.view = e.go.AddComponent<TTSpriteView>();
                _live[it.key] = e;
            }
            e.seen = _frame;
            if (e.go != null && !e.go.activeSelf) e.go.SetActive(true);
            TTSpace.Decompose(m, out var pos, out var rot, out float scale);
            e.go.transform.SetPositionAndRotation(pos, rot);
            if (Mathf.Abs(scale - 1f) > 1e-3f) e.go.transform.localScale = Vector3.one * scale;

            int lod = def.lods.Length > 1 && (pos - camPos).sqrMagnitude > lodDistance * lodDistance ? 1 : 0;
            if (e.phase < 0) e.phase = (_live.Count * 7) & 3;
            var team = new Color(it.r, it.g, it.b, 1f);
            if (e.location != it.location || e.texture != it.texture)
            {
                e.location = it.location;
                e.texture = it.texture;
                e.view.Init(def, BaseMaterial, team, lod, it.texture);
                // (a unit's small items cast no shadow: thousands of shadow draws nobody can make out)
                if (it.kind == RKind.Attach || it.kind == RKind.Carry)
                    foreach (var r in e.go.GetComponentsInChildren<Renderer>(true)) r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                e.team = team;
                e.lod = lod;
                e.anim = int.MinValue;
                e.lights = false;
            }
            else
            {
                if (lod != e.lod) { e.view.SetLod(lod, it.texture); e.lod = lod; }
                if (team != e.team) { e.view.SetTeamColor(team); e.team = team; }
            }
            // far away (beyond the full-detail distance), a unit's pose is updated every third frame (staggered);
            // a new animation still starts at once
            float d2 = (pos - camPos).sqrMagnitude;
            bool posePass = it.anim != e.anim || d2 < AnimLodDistance * AnimLodDistance || (_frame + e.phase) % 3 == 0;
            if (def.animations.Length > 0 && (it.anim != e.anim || it.ticks != e.ticks) && posePass)
            {
                int a = Mathf.Clamp(it.anim, 0, def.animations.Length - 1);
                if (a != e.view.AnimationIndex) e.view.SetAnimation(a, it.ticks);
                else e.view.SetAnimationTicks(it.ticks);
                e.anim = it.anim;
                e.ticks = it.ticks;
            }
            else e.view.PoseIfPending();
            return e;
        }

        // ---- night: the registry's glow lights (GeometryRegistry.getLights) as point lights on the building
        int _glowLights;
        const int MaxGlowLights = 48;

        void AddGlowLights(GameObject go, string location, int texture)
        {
            var lights = com.oddlabs.tt.model.GeometryRegistry.getLights(location, texture);
            foreach (var l in lights)
            {
                if (_glowLights >= MaxGlowLights) return;
                var lg = new GameObject("Glow light");
                lg.transform.SetParent(go.transform, false);
                // sprite space (TT, z up) -> the object's local space (models are mirrored in x by the import, TTSpace)
                lg.transform.localPosition = new Vector3(-l.x, l.z, -l.y);
                var light = lg.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = Mathf.Max(1f, l.reach);
                float max = Mathf.Max(l.r, Mathf.Max(l.g, l.b));
                light.color = max > 0f ? new Color(l.r / max, l.g / max, l.b / max) : Color.white;
                light.intensity = Mathf.Max(0.2f, max / 255f) * 3f / 0.4f;   // (added after the night tint in Steam's shaders: undo the frame's filter)
                light.shadows = LightShadows.None;
                _glowLights++;
            }
        }

        // ---- the trees: whole leaves of standing trees worked out once (their instance matrices per model)
        sealed class LeafCache
        {
            public com.oddlabs.tt.landscape.TreeSupply[] trees;   // the leaf's array when cached
            public TTSpriteDefinition[] keys;
            public Matrix4x4[][] mats;
            public Vector3 center;
            public Bounds bounds;
            public int count;
            public int version;   // TreeLeaf.getStateVersion when cached
            public int gen;
        }
        readonly List<LeafCache> _visibleLeaves = new List<LeafCache>();
        static int _leafGen = 1;
        System.Func<com.oddlabs.tt.landscape.TreeLeaf, bool> _leafHandler;
        bool _leafOn;
        readonly Dictionary<(TTSpriteDefinition, bool), Batch> _leafBatchMap = new Dictionary<(TTSpriteDefinition, bool), Batch>();
        readonly List<Batch> _leafBatchList = new List<Batch>();

        readonly Dictionary<object, LeafCache> _leafCache = new Dictionary<object, LeafCache>(RefEq.Instance);

        bool HandleLeaf(com.oddlabs.tt.landscape.TreeLeaf leaf)
        {
            if (!_leafOn) return false;
            var arr = leaf.infos;
            if (arr == null || arr.Length == 0) return true;
            // looked at again only when a tree of it was felled, vanished or grew back (the leaf's state: which of its
            // trees stand, one bit each, folded into a signature)
            int ver = arr.Length;
            bool moving = false;
            for (int i = 0; i < arr.Length; i++)
            {
                var t = arr[i];
                bool gone = t.isHidden() || t.isEmpty();
                if ((t.isEmpty() && !t.isHidden()) || t.getScale() < 1f) moving = true;   // falling or growing
                ver = ver * 31 + (gone ? 1 : 0) + (t.isHidden() ? 2 : 0);
            }
            if (moving) return false;
            if (!_leafCache.TryGetValue(leaf, out var lc) || lc.gen != _leafGen || !ReferenceEquals(lc.trees, arr) || lc.version != ver)
            {
                lc = BuildLeaf(arr);
                lc.version = ver;
                lc.gen = _leafGen;
                _leafCache[leaf] = lc;
            }
            if (lc.count == 0) return true;   // every tree of it felled
            _visibleLeaves.Add(lc);
            return true;
        }

        LeafCache BuildLeaf(com.oddlabs.tt.landscape.TreeSupply[] arr)
        {
            var d = new Dictionary<TTSpriteDefinition, List<Matrix4x4>>();
            Vector3 sum = Vector3.zero;
            int standing = 0;
            bool first = true;
            var lc = new LeafCache { trees = arr };
            foreach (var t in arr)
            {
                if (t.isHidden() || t.isEmpty()) continue;   // felled: gone until it grows back
                standing++;
                var e = MakeTreeEntry(SteamScene.StandingTree(t));
                sum += e.pos;
                if (first) { lc.bounds = new Bounds(e.pos, Vector3.zero); first = false; } else lc.bounds.Encapsulate(e.pos);
                foreach (var part in new[] { e.trunk, e.crown })
                {
                    if (part == null || part.model == null) continue;
                    var ib = InstanceBatch(part, 0, true);
                    if (ib == null) continue;
                    if (!d.TryGetValue(part, out var l)) d[part] = l = new List<Matrix4x4>();
                    l.Add(e.m * ib.local);
                }
            }
            lc.center = sum / Mathf.Max(1, standing);
            lc.count = standing;
            // a tree's crown spreads round its trunk and rises well above it
            lc.bounds.min -= new Vector3(14f, 4f, 14f);
            lc.bounds.max += new Vector3(14f, 40f, 14f);
            lc.keys = new TTSpriteDefinition[d.Count];
            lc.mats = new Matrix4x4[d.Count][];
            int k = 0;
            foreach (var kv in d) { lc.keys[k] = kv.Key; lc.mats[k++] = kv.Value.ToArray(); }
            return lc;
        }

        Batch LeafBatch(TTSpriteDefinition def, bool shadows)
        {
            if (_leafBatchMap.TryGetValue((def, shadows), out var b)) return b;
            var src = InstanceBatch(def, 0, shadows);
            b = src == null ? null : new Batch { mesh = src.mesh, submesh = src.submesh, material = src.material, local = src.local, shared = true, noShadows = !shadows };
            _leafBatchMap[(def, shadows)] = b;
            if (b != null) _leafBatchList.Add(b);
            return b;
        }

        struct TreeEntry
        {
            public Matrix4x4 m;
            public Vector3 pos;
            public TTSpriteDefinition trunk, crown;
        }
        readonly Dictionary<object, TreeEntry> _treeCache = new Dictionary<object, TreeEntry>(RefEq.Instance);
        object _treeWorld;
        readonly Dictionary<string, (TTSpriteDefinition trunk, TTSpriteDefinition crown)> _treeDefs = new Dictionary<string, (TTSpriteDefinition, TTSpriteDefinition)>();

        TreeEntry MakeTreeEntry(Renderable t)
        {
            var e = new TreeEntry { m = TTSpace.ToUnity(t.xf) };
            e.pos = new Vector3(e.m.m03, e.m.m13, e.m.m23);
            if (!_treeDefs.TryGetValue(t.location, out var defs))
                _treeDefs[t.location] = defs = (TreeDef(t.location, "_trunk"), TreeDef(t.location, "_crown"));
            e.trunk = defs.trunk; e.crown = defs.crown;
            return e;
        }

        // ---- plants and scenery: worked out once
        struct StaticEntry
        {
            public Batch shadow, noShadow;
            public Matrix4x4 mm;
            public Vector3 pos;
            public bool plants;
            public float x, y;   // where it stood (game coordinates) when worked out
        }
        readonly Dictionary<object, StaticEntry> _static = new Dictionary<object, StaticEntry>(RefEq.Instance);
        System.Func<Model, bool> _staticHandler;
        object _staticWorld;
        float _staticShadow2;
        Vector3 _staticCam;

        bool HandleStatic(Model m)
        {
            if (!_static.TryGetValue(m, out var e)) return false;
            if (m.getPositionX() != e.x || m.getPositionY() != e.y) { _static.Remove(m); return false; }   // moved: worked out again
            if (m is SceneryModel sm && sm.getAttachments() != null && sm.getAttachments().size() > 0) { _static.Remove(m); return false; }
            float d2 = (e.pos - _staticCam).sqrMagnitude;
            if (e.plants && d2 > plantsCutDistance * plantsCutDistance) return true;
            (d2 < _staticShadow2 ? e.shadow : e.noShadow).matrices.Add(e.mm);
            statStatic++;
            return true;
        }

        // ---- units (and what they carry or hold) far away: in bulk from baked poses (TTPoseBaker)
        const float FarUnitDistance = 70f;
        const int MaxNearUnits = 260;
        float _nearScale = 1f, _nearR2 = FarUnitDistance * FarUnitDistance;
        int _nearCount;
        readonly Dictionary<(Mesh, Material, int, bool), Batch> _farBatches = new Dictionary<(Mesh, Material, int, bool), Batch>();
        static readonly int TeamColorId = Shader.PropertyToID("_TeamColor");

        bool DrawFar(in Renderable it, TTSpriteDefinition def, Matrix4x4 m, Vector3 camPos, float shadow2)
        {
            if (_catalog == null || def.animations.Length == 0) return false;
            float d2 = (new Vector3(m.m03, m.m13, m.m23) - camPos).sqrMagnitude;
            if (d2 < _nearR2) { if (it.kind == RKind.Unit) _nearCount++; return false; }
            int want = def.lods.Length > 1 && d2 > lodDistance * lodDistance ? def.lods.Length - 1 : 0;
            int maxFrames = d2 > 240f * 240f ? 1 : d2 > 130f * 130f ? 2 : d2 > FarUnitDistance * FarUnitDistance ? 4 : 0;
            var vat = TTPoseBaker.GetVat(def, want);
            if (vat != null)
            {
                var vtex = def.lods[vat.lod].textures;
                var vmat = VatMaterial(def, vat, vtex.Length == 0 ? 0 : Mathf.Clamp(it.texture, 0, vtex.Length - 1));
                if (vmat != null)
                {
                    var vb = VatBatchFor(vat, vmat, d2 < shadow2 && it.kind == RKind.Unit);
                    var tc = new Color(it.r, it.g, it.b, 1f).linear;
                    vb.Add(m * vat.bakedToRoot, TTPoseBaker.VatPose(vat, def, it.anim, it.ticks), new Vector4(tc.r, tc.g, tc.b, 1f));
                    Hide(it.key);
                    statFar++;
                    return true;
                }
            }
            if (!TTPoseBaker.Get(def, want, it.anim, it.ticks, out var mesh, out var local, out int lod, maxFrames)) return false;
            var tex = def.lods[lod].textures;
            var mat = TTSpriteView.GetMaterial(def, lod, tex.Length == 0 ? 0 : Mathf.Clamp(it.texture, 0, tex.Length - 1), BaseMaterial);
            if (mat == null) return false;
            if (!mat.enableInstancing) mat.enableInstancing = true;
            bool shadows = d2 < shadow2 && it.kind == RKind.Unit;
            Color32 c = new Color(it.r, it.g, it.b, 1f);
            int tk = (c.r << 16) | (c.g << 8) | c.b;
            var key = (mesh, mat, tk, shadows);
            if (!_farBatches.TryGetValue(key, out var b))
            {
                var props = new MaterialPropertyBlock();
                props.SetColor(TeamColorId, new Color(it.r, it.g, it.b, 1f));
                b = new Batch { mesh = mesh, material = mat, local = Matrix4x4.identity, shared = true, noShadows = !shadows, props = props };
                _farBatches[key] = b;
                _batchList.Add(b);
            }
            b.matrices.Add(m * local);
            Hide(it.key);
            statFar++;
            return true;
        }

        void Hide(object key)
        {
            if (_live.TryGetValue(key, out var e))
            {
                e.seen = _frame;
                if (e.go != null && e.go.activeSelf) e.go.SetActive(false);
            }
        }

        // ---- units in bulk from pose textures (TTPoseBaker.Vat): one instanced draw per model, material and shadow
        sealed class VatBatch
        {
            public Mesh mesh;
            public Material mat;
            public bool shadows;
            public readonly MatrixBuf m = new MatrixBuf();
            public Vector4[] anim = new Vector4[64], team = new Vector4[64];
            public readonly List<MaterialPropertyBlock> blocks = new List<MaterialPropertyBlock>();
            public void Add(in Matrix4x4 mm, Vector4 a, Vector4 t)
            {
                int n = m.Count;
                if (n >= anim.Length) { System.Array.Resize(ref anim, anim.Length * 2); System.Array.Resize(ref team, team.Length * 2); }
                anim[n] = a; team[n] = t;
                m.Add(mm);
            }
        }
        readonly Dictionary<(object, Material, bool), VatBatch> _vatBatches = new Dictionary<(object, Material, bool), VatBatch>();
        readonly List<VatBatch> _vatList = new List<VatBatch>();
        readonly Dictionary<(object, int), Material> _vatMats = new Dictionary<(object, int), Material>();
        static readonly int TTAnimId = Shader.PropertyToID("_TTAnim"), TTTeamId = Shader.PropertyToID("_TTTeam"), TTVatTexId = Shader.PropertyToID("_TTVatTex");
        readonly Matrix4x4[] _vatM = new Matrix4x4[1023];
        readonly Vector4[] _vatA = new Vector4[1023], _vatT = new Vector4[1023];

        Material VatMaterial(TTSpriteDefinition def, TTPoseBaker.Vat vat, int variant)
        {
            if (_vatMats.TryGetValue((vat, variant), out var vm)) return vm;
            var bm = TTSpriteView.GetMaterial(def, vat.lod, variant, BaseMaterial);
            if (bm == null) { _vatMats[(vat, variant)] = null; return null; }
            if (!new UnityEngine.Rendering.LocalKeyword(bm.shader, "_TT_VAT").isValid)
            {
                TTPoseBaker.VatEnabled = false;
                _vatMats[(vat, variant)] = null;
                return null;
            }
            vm = new Material(bm) { name = bm.name + " (poses)", enableInstancing = true };
            vm.EnableKeyword("_TT_VAT");
            vm.SetTexture(TTVatTexId, vat.tex);
            _vatMats[(vat, variant)] = vm;
            return vm;
        }

        VatBatch VatBatchFor(TTPoseBaker.Vat vat, Material mat, bool shadows)
        {
            if (!_vatBatches.TryGetValue((vat, mat, shadows), out var b))
            {
                b = new VatBatch { mesh = vat.mesh, mat = mat, shadows = shadows };
                _vatBatches[(vat, mat, shadows)] = b;
                _vatList.Add(b);
            }
            return b;
        }

        void DrawVat()
        {
            foreach (var b in _vatList)
            {
                int n = b.m.Count;
                if (n == 0) continue;
                statInstances += n;
                int chunk = 0;
                for (int start = 0; start < n; start += 1023, chunk++)
                {
                    int c = Mathf.Min(1023, n - start);
                    System.Array.Copy(b.m.array, start, _vatM, 0, c);
                    System.Array.Copy(b.anim, start, _vatA, 0, c);
                    System.Array.Copy(b.team, start, _vatT, 0, c);
                    if (chunk >= b.blocks.Count) b.blocks.Add(new MaterialPropertyBlock());
                    var mpb = b.blocks[chunk];
                    mpb.SetVectorArray(TTAnimId, _vatA);
                    mpb.SetVectorArray(TTTeamId, _vatT);
                    Graphics.DrawMeshInstanced(b.mesh, 0, b.mat, _vatM, c, mpb,
                        b.shadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off, true, 0, null,
                        UnityEngine.Rendering.LightProbeUsage.Off);
                }
                b.m.Clear();
            }
        }

        // ---- instanced static models
        void AddInstance(TTSpriteDefinition def, int texture, Matrix4x4 m, bool shadows = true)
        {
            var b = InstanceBatch(def, texture, shadows);
            if (b != null) b.matrices.Add(m * b.local);
        }

        Batch InstanceBatch(TTSpriteDefinition def, int texture, bool shadows)
        {
            int key = shadows ? texture : -1 - texture;
            if (!_batches.TryGetValue((def, key), out var b))
            {
                b = MakeBatch(def, texture);
                if (b != null && !shadows)
                {
                    b.noShadows = true;
                    if (_batches.TryGetValue((def, texture), out var sb) && sb != null) { b.material = sb.material; b.shared = true; }
                }
                _batches[(def, key)] = b;
                if (b != null) _batchList.Add(b);
            }
            return b;
        }

        Batch MakeBatch(TTSpriteDefinition def, int texture)
        {
            MeshFilter mf = null;
            if (def.lods.Length > 0 && !string.IsNullOrEmpty(def.lods[0].nodeName))
                foreach (var f in def.model.GetComponentsInChildren<MeshFilter>(true))
                    if (f.name == def.lods[0].nodeName) { mf = f; break; }
            if (mf == null) mf = def.model.GetComponentInChildren<MeshFilter>(true);
            if (mf == null || mf.sharedMesh == null) return null;
            var mat = new Material(BaseMaterial) { enableInstancing = true, name = "Instanced " + def.Key + " #" + texture };
            var tex = def.lods.Length > 0 ? def.lods[0].textures : System.Array.Empty<TTSpriteDefinition.TextureVariant>();
            if (tex.Length > 0)
            {
                var v = tex[Mathf.Clamp(texture, 0, tex.Length - 1)];
                var bm = v.baseMap != null ? v.baseMap : TTGeneratedTextures.Get(v.name);
                mat.SetTexture("_BaseMap", bm != null ? bm : Texture2D.whiteTexture);
                if (texture >= tex.Length) mat.SetColor("_Tint", IronTint);   // variant missing from the import (iron rocks)
            }
            else mat.SetTexture("_BaseMap", Texture2D.whiteTexture);
            mat.SetFloat("_UseTeamMask", 0f);
            if (def.lods.Length > 0 && def.lods[0].alphaClip)
            {
                mat.EnableKeyword("_ALPHATEST_ON");
                mat.SetFloat("_AlphaClip", 1f);
                mat.SetFloat("_Cull", 0f);
            }
            SteamSpriteLook.Apply(mat, def, def.lods.Length > 0 && def.lods[0].alphaClip);
            return new Batch
            {
                mesh = mf.sharedMesh, material = mat,
                local = def.model.transform.worldToLocalMatrix * mf.transform.localToWorldMatrix,
            };
        }

        void DrawBatches()
        {
            statInstances = 0;
            for (int li = 0; li < 2; li++)
                foreach (var b in li == 0 ? _batchList : _leafBatchList)
                {
                    int n = b.matrices.Count;
                    if (n == 0) continue;
                    statInstances += n;
                    var rp = new RenderParams(b.material)
                    {
                        shadowCastingMode = b.noShadows ? UnityEngine.Rendering.ShadowCastingMode.Off : UnityEngine.Rendering.ShadowCastingMode.On,
                        receiveShadows = true, matProps = b.props
                    };
                    if (b.hasBounds) rp.worldBounds = b.bounds;
                    for (int start = 0; start < n; start += 1023)
                        Graphics.RenderMeshInstanced(rp, b.mesh, b.submesh, b.matrices.array, Mathf.Min(1023, n - start), start);
                }
            DrawVat();
        }

        public void Clear()
        {
            foreach (var e in _live.Values) if (e.go != null) Object.Destroy(e.go);
            _live.Clear();
            foreach (var b in _batchList) if (b.material != null && !b.shared) Object.Destroy(b.material);
            _batchList.Clear();
            _batches.Clear();
            _farBatches.Clear();
            _leafBatchMap.Clear(); _leafBatchList.Clear();
            foreach (var vb in _vatList) vb.m.Clear();
            particles.Clear();
            foreach (var m in _respondMaterials.Values) if (m != null) Object.Destroy(m);
            _respondMaterials.Clear();
            overlay?.Clear();
            _glowLights = 0;
        }
    }
}
