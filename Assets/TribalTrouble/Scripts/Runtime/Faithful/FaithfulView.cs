using System.Collections.Generic;
using TribalTrouble.Faithful;
using TribalTrouble.Presentation;
using UnityEngine;
using com.oddlabs.tt.model;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Draws the faithful simulation: each frame takes the FaithfulScene snapshot (what the Java RenderState /
    /// TreeRenderer would draw) and maps sprite locations onto the imported sprite catalog. Animated or
    /// team-coloured models (units, buildings, chickens, weapons, carried resources) get a pooled TTSpriteView
    /// keyed by the sim object; static scenery (trees, plants, rocks) is GPU-instanced.
    /// </summary>
    public sealed class FaithfulView
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
            public int trim;         // frames its small attachments have been checked for shadows
            public MeshFilter[] weapons;              // its weapons (their shadows are drawn flat on the ground: TTPlanarShadows)
            public TTHeldWeaponAligner[] aligners;
        }

        sealed class Batch
        {
            public Mesh mesh;
            public int submesh;
            public Material material;
            public Matrix4x4 local;
            public bool shared;   // material owned elsewhere (real trees)
            public bool noShadows;   // beyond the shadow distance: don't cast
            public MaterialPropertyBlock props;   // far units: the team colour of the whole batch
            public bool shadowOnly;   // drawn into the shadow maps only (a tree's impostor standing in for its shadow)
            public Bounds bounds;     // what it covers, when known (the leaves' batches): Unity needn't work it out
            public bool hasBounds;
            public readonly MatrixBuf matrices = new MatrixBuf();
        }

        /// <summary>The instance matrices of a batch, written straight into the array that is drawn (a List would be
        /// copied into it again every frame - megabytes a frame on the biggest islands).</summary>
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

        public readonly FaithfulScene scene = new FaithfulScene();
        public readonly FaithfulParticles particles = new FaithfulParticles();
        readonly TTSpriteCatalog _catalog;
        readonly Transform _root;
        readonly Dictionary<string, TTSpriteDefinition> _defs = new Dictionary<string, TTSpriteDefinition>();
        readonly Dictionary<object, Entry> _live = new Dictionary<object, Entry>(1024, RefEq.Instance);
        readonly List<object> _dead = new List<object>();
        readonly Dictionary<(TTSpriteDefinition, int), Batch> _batches = new Dictionary<(TTSpriteDefinition, int), Batch>();
        readonly List<Batch> _batchList = new List<Batch>();
        readonly Dictionary<(Mesh, Material, bool), Batch> _procBatches = new Dictionary<(Mesh, Material, bool), Batch>();

        /// <summary>Extended rules' procedural models (crystals, herbs, stone / iron footings, barbs): instanced, one
        /// draw per mesh and material instead of one per object - a big map has well over a thousand of them.</summary>
        System.Action<Mesh, Material, Matrix4x4, bool> _addProc;

        void AddProc(Mesh mesh, Material material, Matrix4x4 m, bool shadows)
        {
            if (mesh == null || material == null) return;
            if (!_procBatches.TryGetValue((mesh, material, shadows), out var b))
            {
                material.enableInstancing = true;
                b = new Batch { mesh = mesh, material = material, local = Matrix4x4.identity, shared = true, noShadows = !shadows };
                _procBatches[(mesh, material, shadows)] = b;
                _batchList.Add(b);
            }
            b.matrices.Add(m);
        }
        readonly HashSet<string> _missing = new HashSet<string>();
        int _frame;

        public float lodDistance = 70f;
        const float AnimLodDistance = 90f;
        public float plantsCutDistance = 2000f;   // (was RenderState.PLANTS_CUT_DIST, 200 m: the plants and herbs vanished as the camera zoomed out)
        static readonly Color IronTint = new Color(0.55f, 0.42f, 0.36f);

        public FaithfulView(TTSpriteCatalog catalog, Transform root)
        {
            _catalog = catalog;
            _root = root;
        }

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

        // the sprite keys' location strings are the same objects frame after frame: looked up by reference first
        readonly Dictionary<object, TTSpriteDefinition> _defsByRef = new Dictionary<object, TTSpriteDefinition>(RefEq.Instance);

        // ---- the showcase's line-up of every unit (TTStressBench): how each one was drawn, the last time it was in view
        public static Dictionary<object, string> Probe;
        static void ProbeNote(object model, string how) { if (Probe != null && model != null && Probe.ContainsKey(model)) Probe[model] = how; }
        static void ProbeGo(object model, GameObject go, string how)
        {
            if (Probe == null || model == null || !Probe.ContainsKey(model)) return;
            if (go == null) { Probe[model] = how + ": no object"; return; }
            int n = 0, on = 0; float size = 0f;
            foreach (var r in go.GetComponentsInChildren<Renderer>(true))
            {
                n++;
                if (!r.enabled || !r.gameObject.activeInHierarchy) continue;
                if (r is SkinnedMeshRenderer smr && smr.sharedMesh == null) continue;
                if (r is MeshRenderer mr && (mr.GetComponent<MeshFilter>()?.sharedMesh == null)) continue;
                on++; size = Mathf.Max(size, r.bounds.size.magnitude);
            }
            Probe[model] = $"{how}: {on} of {n} renderers drawing, {size:0.0} m across" + (go.activeInHierarchy ? "" : ", object switched off");
        }

        public TTSpriteDefinition Def(string location)
        {
            if (location == null) return null;
            if (_defsByRef.TryGetValue(location, out var d)) return d;
            d = DefByName(location);
            if (_defsByRef.Count > 8192) _defsByRef.Clear();
            _defsByRef[location] = d;
            return d;
        }

        TTSpriteDefinition DefByName(string location)
        {
            if (_defs.TryGetValue(location, out var d)) return d;
            d = null;
            if (_catalog != null && ParseLocation(location, out var group, out var name)) d = _catalog.Find(group, name);
            if (d == null && _missing.Add(location)) Debug.LogWarning("[Tribal Trouble] No sprite in the catalog for " + location);
            _defs[location] = d;
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
            || def.skinned || def.animations.Length > 0;

        /// <summary>Modern combat / damage effects (dust, sparks, blood, burning buildings).</summary>
        public TTCombatFx combatFx;

        /// <summary>Debug (F8 in a game): where to lay out the replacement weapons, or null.</summary>
        public Vector3? weaponShowcase;

        void DrawWeaponShowcase()
        {
            if (!weaponShowcase.HasValue || _catalog == null) return;
            var at = weaponShowcase.Value;
            int row = 0;
            foreach (var (g, n) in new[] { ("vikings", "axe"), ("natives", "spear") })
            {
                var def = _catalog.Find(g, n);
                for (int t = 0; t < 3 && def != null; t++)
                {
                    var p = TTRealWeapons.Get(def, t, _catalog.baseMaterial);
                    if (p == null) continue;
                    // shown 4x size so they can be judged from the game camera
                    AddReal(p, Matrix4x4.TRS(at + new Vector3(t * 6f - 6f, 2f, row * 10f), Quaternion.identity, Vector3.one * 4f));
                }
                row++;
            }
        }

        /// <summary>Trees further than this aren't drawn at all (fog hides them); realistic models within realTreeDistance.</summary>
        public float treeDrawDistance = 900f, realTreeDistance = 220f;

        /// <summary>Per-frame GPU budget for trees (see the tree loop in Render).</summary>
        const int MaxRealTrees = 2500, MaxTreeShadows = 2000;
        const float TreeShadowDistance = 160f;
        float _realScale = 1f, _shadowScale = 1f;
        int _lastRealTrees, _lastTreeShadows;

        /// <summary>Draw trees (the map editor's "Trees" switch).</summary>
        public bool drawTrees = true;

        readonly Plane[] _cullPlanes = new Plane[6];
        Vector3 _cullCamera;
        float _cullDistance = 900f;

        bool CullTest(float x0, float y0, float z0, float x1, float y1, float z1)
        {
            var bounds = new Bounds(new Vector3((x0 + x1) * .5f, (y0 + y1) * .5f, (z0 + z1) * .5f), new Vector3(x1 - x0, y1 - y0, z1 - z0));
            if (bounds.SqrDistance(_cullCamera) > _cullDistance * _cullDistance) return false;
            return GeometryUtility.TestPlanesAABB(_cullPlanes, bounds);
        }

        public void Render(FaithfulMatch match, Camera cam) => Render(match.world, cam);

        bool _viewCull;
        float _viewCull2;
        Vector3 _viewCam;
        const float ShadowMargin = 14f;   // what stands just outside the view may still throw its shadow into it

        /// <summary>Whether a sphere (Unity space) is in the view (with a margin for shadows) and within the draw distance.</summary>
        bool InView(Vector3 p, float radius)
        {
            if (!_viewCull) return true;
            if ((p - _viewCam).sqrMagnitude > _viewCull2) return false;
            float r = -(radius + ShadowMargin);
            for (int i = 0; i < 6; i++)
                if (_cullPlanes[i].GetDistanceToPoint(p) < r) return false;
            return true;
        }

        public void Render(com.oddlabs.tt.landscape.World world, Camera cam)
        {
            TTFit.OnWorld(world);
            _frame++;
            // Modern keeps the full-detail models much further out (the low-detail ones read as blurry)
            lodDistance = TribalTrouble.Game.GameSettings.graphicsStyle == 1 ? 260f : 70f;
            // only trees in (or just outside) the view and within the draw distance are collected
            if (cam != null)
            {
                GeometryUtility.CalculateFrustumPlanes(cam, _cullPlanes);
                _cullCamera = cam.transform.position;
                _cullDistance = Mathf.Max(treeDrawDistance, cam.transform.position.y * 3f);
                scene.cullTest ??= CullTest;
            }
            else scene.cullTest = null;
            _sw.Restart();
            bool realNow = TribalTrouble.Game.GameSettings.graphicsStyle == 1 && TribalTrouble.Game.GameSettings.realTrees;
            if (!ReferenceEquals(_treeWorld, world) || _treeReal != realNow) { _leafCache.Clear(); _leafGen++; }
            _leafReal = realNow;
            _leafOn = cam != null && drawTrees;
            _leafMat = _catalog != null ? _catalog.baseMaterial : null;
            _visibleLeaves.Clear();
            scene.leafHandler = _leafHandler ??= HandleLeaf;
            scene.staticHandler = _staticHandler ??= HandleStatic;
            statStatic = 0;
            if (!ReferenceEquals(_staticWorld, world)) { _static.Clear(); _staticWorld = world; }
            _staticShadow2 = TribalTrouble.Game.GameSettings.shadows ? TribalTrouble.Game.GameSettings.shadowDistance * TribalTrouble.Game.GameSettings.shadowDistance : 0f;
            _staticCam = cam != null ? cam.transform.position : Vector3.zero;
            // (cleared before the scene is collected: the cached scenery - the game modes' statues and flags, the
            // campaign's props - is added to its batches while collecting, and clearing after it wiped them every frame)
            foreach (var b in _batchList) b.matrices.Clear();
            scene.Collect(world, drawTrees);
            statCollectMs = (float)_sw.Elapsed.TotalMilliseconds;
            _sw.Restart();
            if (!drawTrees) scene.trees.Clear();
            float shadowDist = TribalTrouble.Game.GameSettings.shadows ? TribalTrouble.Game.GameSettings.shadowDistance : 0f;
            float shadow2 = shadowDist * shadowDist;
            TTExtendedArt.Instanced = _addProc ??= AddProc;
            Vector3 camPos = cam != null ? cam.transform.position : Vector3.zero;
            float plantCut2 = plantsCutDistance * plantsCutDistance;

            bool realWeapons = TribalTrouble.Game.GameSettings.graphicsStyle == 1 && TribalTrouble.Game.GameSettings.realWeapons;
            TTPlanarShadows.Begin(world);
            _weaponEntries.Clear();
            _vatStripWeapons = realWeapons;
            _viewCull = cam != null;
            _viewCull2 = _cullDistance * _cullDistance;
            _viewCam = camPos;
            statCulled = 0; statFar = 0; _tFar = _tPlace = _tExt = 0;
            TribalTrouble.Presentation.TTPoseBaker.BeginFrame();
            // a crowd close to the camera: only the nearest MaxNearUnits are skinned objects of their own, the rest are
            // drawn in bulk too (the radius shrinks while there are more, and grows back slowly)
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
                if (def == null || def.model == null) { if (Probe != null) ProbeNote(it.model, "no sprite for " + it.location); continue; }
                var m = TTSpace.ToUnity(it.xf);
                // units, buildings and what they carry or throw, well outside the view: nothing to update or draw
                // (their objects are kept, switched off, and come back as they were when they come into view)
                if (it.kind != RKind.Plants && it.kind != RKind.Supply && it.kind != RKind.Scenery && it.kind != RKind.Tree
                    && !InView(new Vector3(m.m03, m.m13, m.m23), it.kind == RKind.Building ? 24f : 6f))
                {
                    if (_live.TryGetValue(it.key, out var hidden))
                    {
                        hidden.seen = _frame;
                        if (hidden.go != null && hidden.go.activeSelf) hidden.go.SetActive(false);
                    }
                    statCulled++;
                    continue;
                }
                // Extended rules: palisade / shrine get their own models; champion / scout are sized and equipped
                if (it.kind == RKind.Building && it.model is LandBuilding xb && TTExtendedArt.IsExtendedBuilding(xb))
                {
                    long te = System.Diagnostics.Stopwatch.GetTimestamp();
                    bool fit = TTFit.NeedsCapture(xb);
                    if (fit) TTFit.BeginCapture();
                    TTExtendedArt.DrawBuilding(xb, m, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null);
                    if (fit) TTFit.EndCapture(xb);
                    _tExt += System.Diagnostics.Stopwatch.GetTimestamp() - te;
                    continue;
                }
                if (it.kind == RKind.Unit && it.model is com.oddlabs.tt.model.Unit xu)
                {
                    int xk = TTExtendedArt.UnitKind(xu);
                    if (xk >= 0)
                    {
                        m = m * Matrix4x4.Scale(Vector3.one * TTExtendedArt.UnitScale(xu, xk));
                        if (DrawFar(it, def, m, camPos, shadow2)) { if (Probe != null) ProbeNote(it.model, "far, in bulk (" + def.Key + ")"); continue; }
                        var ce = Place(it, def, m, camPos);
                        if (Probe != null) ProbeGo(it.model, ce.go, "own object (" + def.Key + ")");
                        if (ce.go != null)
                        {
                            if (xk == Race.UNIT_CHAMPION) TTExtendedArt.AttachShield(ce.go, xu, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null);
                            else if (xk == Race.UNIT_SHAMAN) TTExtendedArt.AttachCasterOrb(ce.go, xu);
                            else if (xk == Race.UNIT_MELEE_HEAVY) TTExtendedArt.AttachShield(ce.go, xu, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null, true);
                            else if (xk == Race.UNIT_UNIQUE_3 && TTExtendedArt.Vikings(xu)) TTExtendedArt.AttachShield(ce.go, xu, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null, true);   // the Shield Maiden
                            else if (xk == Race.UNIT_ELITE_1 && TTExtendedArt.Vikings(xu)) TTExtendedArt.AttachShield(ce.go, xu, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null, true);   // the Jarl
                            else if (xk == Race.UNIT_UNIQUE_1 || xk == Race.UNIT_UNIQUE_2) TTExtendedArt.DressRaceUnit(ce.go, xu, xk, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null);
                            TTExtendedArt.DressUnique(ce.go, xu, xk, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null);
                            // the gods (drawn on their chieftain's model, grown): a body of their own and their regalia - wings,
                            // crowns, a jaguar cowl, a tiki mask - so they no longer read as a big chief
                            if (xk == Race.UNIT_MINOR_GOD || xk == Race.UNIT_MAJOR_GOD)
                                TTExtendedArt.DressGod(ce.go, xu, xk, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null);
                            if (xu.getOwner().isElemental()) TTExtendedArt.ApplyElemental(ce.go);   // (the Rift's tribe)
                            if (xk != Race.UNIT_MINOR_GOD && xk != Race.UNIT_MAJOR_GOD) TTExtendedArt.UnitLook(ce.go, xu, xk);   // armour trim, own shade
                            TTExtendedArt.MeleeMotion(ce.go, xu, xk, it.anim, it.ticks);   // melee warriors step into the blow
                            TTExtendedArt.BlowMotion(ce.go, ce.view, xu, xk, it.anim, it.ticks);   // blowgunners shoot from the mouth
                        }
                        continue;
                    }
                }
                // Extended rules: gold / coal (the rock recoloured), crystal and herbs (their own models)
                if (it.kind == RKind.Supply && TTExtendedArt.RareKind(it.model) is int rk && rk >= 0)
                {
                    bool rsh = (new Vector3(m.m03, m.m13, m.m23) - camPos).sqrMagnitude < shadow2;
                    if (rk == 0 || rk == 3) AddInstance(def, 100 + rk, m, rsh);
                    else if (rk == 4) { AddInstance(def, 100 + rk, m, rsh); TTExtendedArt.DrawRare(rk, m, _catalog != null ? _catalog.baseMaterial : null, rsh); }
                    else if (rk == 1 || (new Vector3(m.m03, m.m13, m.m23) - camPos).sqrMagnitude < plantCut2)   // herbs: as far as plants
                        TTExtendedArt.DrawRare(rk, m, _catalog != null ? _catalog.baseMaterial : null, rsh);
                    continue;
                }
                // Extended rules: a quarters / armory / tower rebuilt in stone or iron, or chicken-barbed
                if (it.kind == RKind.Building && it.model is LandBuilding tb && (tb.getTier() > 0 || tb.hasChickenBarbs()))
                {
                    var te = Place(it, def, m, camPos);
                    if (te.go != null)
                    {
                        TTExtendedArt.DrawTier(tb, m, new Color(it.r, it.g, it.b, 1f), _catalog != null ? _catalog.baseMaterial : null, te.go);
                        if (tb.getOwner().isElemental()) TTExtendedArt.ApplyElemental(te.go);
                    }
                    continue;
                }
                // Extended rules: the race units' projectiles (darts, sling stones, spirit bolts, lighter scout weapons)
                if (it.kind == RKind.Weapon && it.model is com.oddlabs.tt.model.weapon.ThrowingWeapon xtw && xtw.getThrower() is com.oddlabs.tt.model.Unit xth
                    && xth.getOwner().getWorld().isExtended() && TTExtendedArt.DrawProjectile(xtw, xth, ref m, _catalog != null ? _catalog.baseMaterial : null))
                    continue;
                if (it.kind == RKind.Weapon && realWeapons)
                {
                    var rw = TTRealWeapons.Get(def, it.texture, _catalog != null ? _catalog.baseMaterial : null);
                    if (rw != null)
                    {
                        // a thrown weapon's shadow: its own shape, flat on the ground (not into the shadow map)
                        bool flat = TTPlanarShadows.Active && (new Vector3(m.m03, m.m13, m.m23) - camPos).sqrMagnitude < shadow2;
                        AddReal(rw, m, !TTPlanarShadows.Active);
                        if (flat) TTPlanarShadows.AddFlying(rw.mesh, m * rw.local);
                        continue;
                    }
                }
                if ((it.kind == RKind.Unit || it.kind == RKind.Carry) && DrawFar(it, def, m, camPos, shadow2)) { if (Probe != null) ProbeNote(it.model, "far, in bulk (" + def.Key + ")"); continue; }
                if (NeedsObject(it.kind, def))
                {
                    var pe = Place(it, def, m, camPos);
                    if (Probe != null) ProbeGo(it.model, pe.go, "own object (" + def.Key + ")");
                    if (pe.weapons != null && TTPlanarShadows.Active && it.kind == RKind.Unit
                        && (new Vector3(m.m03, m.m13, m.m23) - camPos).sqrMagnitude < shadow2) _weaponEntries.Add(pe);
                    if (pe.go != null && (it.kind == RKind.Unit || it.kind == RKind.Building)
                        && it.model is com.oddlabs.tt.model.Selectable es && es.getOwner().isElemental())
                        TTExtendedArt.ApplyElemental(pe.go);   // (the Rift's tribe: its own look)
                    if (pe.go != null && it.kind == RKind.Unit && it.model is com.oddlabs.tt.model.Unit cu && cu.isWarrior())
                        TTExtendedArt.UnitLook(pe.go, cu, -1);   // classic warriors: armour trim, own shade
                    if (pe.go != null && it.kind == RKind.Unit && it.model is com.oddlabs.tt.model.Unit mu && mu.getOwner().getWorld().isExtended())
                        TTExtendedArt.MeleeMotion(pe.go, mu, -1, it.anim, it.ticks);   // the chieftain's blows
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
                    if (Probe != null) ProbeNote(it.model, "instanced (" + def.Key + ", " + new Vector3(m.m03, m.m13, m.m23).ToString("F1") + ")");
                }
            }

            var trees = scene.trees;
            statTrees = trees.Count; statRealTrees = 0; statImpostors = 0; statItems = scene.items.Count;
            bool real = TribalTrouble.Game.GameSettings.graphicsStyle == 1 && TribalTrouble.Game.GameSettings.realTrees;
            if (!ReferenceEquals(_treeWorld, world) || _treeReal != real) { _treeCache.Clear(); _treeWorld = world; _treeReal = real; }
            var bmT = _catalog != null ? _catalog.baseMaterial : null;
            // GPU budget: on the biggest islands, looking out over a dense forest put tens of thousands of full tree
            // models (and their shadows) into one frame - enough for the graphics driver to give up ("device removed").
            // The full-model radius shrinks while there are too many and grows back when there's room; past the cap a
            // tree uses its impostor, and only the nearest trees cast shadows.
            if (_lastRealTrees > MaxRealTrees) _realScale = Mathf.Max(0.2f, _realScale * 0.8f);
            else if (_lastRealTrees < MaxRealTrees * 0.6f) _realScale = Mathf.Min(1f, _realScale * 1.05f);
            if (_lastTreeShadows > MaxTreeShadows) _shadowScale = Mathf.Max(0.2f, _shadowScale * 0.8f);
            else if (_lastTreeShadows < MaxTreeShadows * 0.6f) _shadowScale = Mathf.Min(1f, _shadowScale * 1.01f);   // (slowly: the shadows' edge creeping out, not sweeping)
            float realR = realTreeDistance * _realScale, real2 = realR * realR;
            float treeShadowR = Mathf.Min(shadowDist, TreeShadowDistance) * _shadowScale, treeShadow2 = treeShadowR * treeShadowR;
            int treeShadows = 0;
            for (int i = 0; i < trees.Count; i++)
            {
                var t = trees[i];
                // standing trees never move: their matrices and models are worked out once (felled ones every frame)
                bool falling = t.key is com.oddlabs.tt.landscape.TreeSupply ts && ts.isEmpty();
                TreeEntry e;
                if (falling || !_treeCache.TryGetValue(t.key, out e))
                {
                    e = MakeTreeEntry(t, real, bmT);
                    if (!falling) _treeCache[t.key] = e;
                }
                float d2 = (e.pos - camPos).sqrMagnitude;
                bool cast = d2 < treeShadow2 && treeShadows < MaxTreeShadows;
                if (cast) treeShadows++;
                if (e.v != null)
                {
                    if (d2 < real2 && statRealTrees < MaxRealTrees) { var parts = e.v.parts; for (int k = 0; k < parts.Count; k++) AddReal(parts[k], e.mm, cast); statRealTrees++; }
                    else
                    {
                        if (e.impostor == null)
                        {
                            e.impostor = TTImpostors.Get(e.v, bmT);
                            if (!falling) _treeCache[t.key] = e; // a struct: store the impostor once
                        }
                        AddReal(e.impostor, e.mm, cast); statImpostors++;
                    }
                    continue;
                }
                if (e.trunk != null && e.trunk.model != null) AddInstance(e.trunk, 0, e.m, cast);
                if (e.crown != null && e.crown.model != null) AddInstance(e.crown, 0, e.m, cast);
            }

            // the cached leaves: one distance per leaf picks full models or impostors and shadows for all its trees.
            // The same leaves with the same choices as the frame before (the camera still, or barely moving): their
            // instance lists are kept as they are instead of being copied together again.
            if (drawTrees)
            {
                ulong sig = 1469598103934665603UL;
                _leafFlags.Clear();
                for (int i = 0; i < _visibleLeaves.Count; i++)
                {
                    var lc = _visibleLeaves[i];
                    float d2 = (lc.center - camPos).sqrMagnitude;
                    bool cast = d2 < treeShadow2 && treeShadows < MaxTreeShadows;
                    if (cast) treeShadows += lc.count;
                    statTrees += lc.count;
                    bool nearModels = d2 < real2 && statRealTrees < MaxRealTrees;
                    bool impostorShadow = nearModels && cast && d2 > FullTreeShadowDistance * FullTreeShadowDistance
                        && lc.farKeys.Length > 0 && lc.farKeys[0] is TTRealTrees.Part && !ReferenceEquals(lc.farKeys, lc.nearKeys);
                    if (nearModels) statRealTrees += lc.count; else statImpostors += lc.count;
                    int f = (cast ? 1 : 0) | (nearModels ? 2 : 0) | (impostorShadow ? 4 : 0);
                    _leafFlags.Add((byte)f);
                    sig = (sig ^ (ulong)(uint)System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(lc)) * 1099511628211UL;
                    sig = (sig ^ (ulong)f) * 1099511628211UL;
                }
                if (sig != _leafSig || _visibleLeaves.Count != _leafSigCount)
                {
                    _leafSig = sig; _leafSigCount = _visibleLeaves.Count;
                    foreach (var b in _leafBatchList) { b.matrices.Clear(); b.hasBounds = false; }
                    for (int i = 0; i < _visibleLeaves.Count; i++)
                    {
                        var lc = _visibleLeaves[i];
                        int f = _leafFlags[i];
                        _fillBounds = lc.bounds;
                        bool cast = (f & 1) != 0, nearModels = (f & 2) != 0, impostorShadow = (f & 4) != 0;
                        var keys = nearModels ? lc.nearKeys : lc.farKeys;
                        var mats = nearModels ? lc.nearMats : lc.farMats;
                        int mode = cast && !impostorShadow ? 1 : 0;
                        for (int p = 0; p < keys.Length; p++) LeafFill(LeafB(keys[p], mode), mats[p]);
                        if (impostorShadow)
                            for (int p = 0; p < lc.farKeys.Length; p++)
                                if (lc.farKeys[p] is TTRealTrees.Part ip) LeafFill(LeafB(ip, 2), lc.farMats[p]);
                    }
                }
            }
            else if (_leafSigCount != 0)
            {
                foreach (var b in _leafBatchList) b.matrices.Clear();
                _leafSigCount = 0; _leafSig = 0;
            }
            _lastRealTrees = statRealTrees; _lastTreeShadows = treeShadows;

            // retire objects whose sim element is gone
            _dead.Clear();
            foreach (var kv in _live) if (kv.Value.seen != _frame) _dead.Add(kv.Key);
            foreach (var k in _dead)
            {
                var e = _live[k];
                // out of the view (not collected) but alive: kept, switched off, for a while - panning back to it
                // shouldn't build its model again
                if (k is com.oddlabs.tt.model.Selectable sel && !sel.isDead() && _frame - e.seen < 1800)
                {
                    if (e.go != null && e.go.activeSelf) e.go.SetActive(false);
                    continue;
                }
                if (e.go != null) Object.Destroy(e.go);
                _live.Remove(k);
            }
            DrawWeaponShowcase();
            HeldWeaponShadows();
            statBuildMs = (float)_sw.Elapsed.TotalMilliseconds;
            double tick = 1000.0 / System.Diagnostics.Stopwatch.Frequency;
            statFarMs = (float)(_tFar * tick); statPlaceMs = (float)(_tPlace * tick); statExtMs = (float)(_tExt * tick);
            _sw.Restart();
            DrawBatches();
            if (cam != null && TribalTrouble.Game.GameSettings.graphicsStyle == 1 && _catalog != null)
                TTGroundCover.Draw(world, cam, _catalog.baseMaterial);   // grass and pebbles near the camera (modern look)
            TTPlanarShadows.Flush();
            statDrawMs = (float)_sw.Elapsed.TotalMilliseconds; _sw.Restart();
            if (cam != null) particles.Render(scene, cam);
            statParticlesMs = (float)_sw.Elapsed.TotalMilliseconds; _sw.Restart();
            combatFx ??= new TTCombatFx(_root);
            combatFx.Update(scene, cam);
            statFxMs = (float)_sw.Elapsed.TotalMilliseconds;
        }

        Entry Place(in Renderable it, TTSpriteDefinition def, Matrix4x4 m, Vector3 camPos)
        {
            long t0 = System.Diagnostics.Stopwatch.GetTimestamp();
            try { return PlaceTimed(it, def, m, camPos); }
            finally { _tPlace += System.Diagnostics.Stopwatch.GetTimestamp() - t0; }
        }

        Entry PlaceTimed(in Renderable it, TTSpriteDefinition def, Matrix4x4 m, Vector3 camPos)
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
            // a unit's small attachments (weapons, shields, capes, glows) cast no shadow: thousands of shadow draws for
            // shadows nobody can make out. They are added over the unit's first frames, so it is checked a few times.
            if (it.kind == RKind.Unit && e.trim < 4 && e.go != null)
            {
                e.trim++;
                s_weaponScratch.Clear();
                foreach (var mr in e.go.GetComponentsInChildren<MeshRenderer>(true))
                {
                    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    string n = mr.gameObject.name;
                    if ((n.StartsWith("Real weapon") || n.StartsWith("Unique weapon")) && mr.TryGetComponent<MeshFilter>(out var mf)) s_weaponScratch.Add(mf);
                }
                e.weapons = s_weaponScratch.Count > 0 ? s_weaponScratch.ToArray() : null;
                if (e.weapons != null)
                {
                    e.aligners = new TTHeldWeaponAligner[e.weapons.Length];
                    for (int k = 0; k < e.weapons.Length; k++) e.aligners[k] = e.weapons[k].GetComponent<TTHeldWeaponAligner>();
                }
            }
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
                e.trim = 0; e.weapons = null;   // (a new model: its attachments are looked for again)
                e.view.Init(def, _catalog.baseMaterial, team, lod, it.texture);
                // real axe / spear in the warrior's hand instead of the painted one
                if (it.kind == RKind.Unit && TribalTrouble.Game.GameSettings.graphicsStyle == 1 && TribalTrouble.Game.GameSettings.realWeapons)
                    TTRealWeapons.ApplyHeld(e.go, def.Key, it.texture, _catalog.baseMaterial);
                e.team = team;
                e.lod = lod;
                e.anim = int.MinValue;
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
            else e.view.PoseIfPending(); // posed while off screen is skipped; catch up once it shows
            if (it.kind == RKind.Unit || it.kind == RKind.Building) TTFit.Measure(it.model, e.go);   // (TT2) hitboxes fitted to the model
            return e;
        }

        readonly Dictionary<(TTRealTrees.Part, bool), Batch> _realBatches = new Dictionary<(TTRealTrees.Part, bool), Batch>();

        // ---- whole leaves of standing realistic trees, worked out once: every part's instance matrices, full model and
        // impostor, per leaf; a visible leaf then costs a few array copies a frame instead of a pass over each tree
        sealed class LeafCache
        {
            public com.oddlabs.tt.landscape.TreeSupply[] trees;   // the leaf's array when cached (a new one when it changes)
            // what is drawn near and far: a realistic tree's parts (TTRealTrees.Part) or the original model's
            // trunk / crown (TTSpriteDefinition), each with its instance matrices
            public object[] nearKeys, farKeys;
            public Matrix4x4[][] nearMats, farMats;
            public Vector3 center;
            public Bounds bounds;   // the standing trees, generously
            public int count;
            public bool failed;   // drawn tree by tree (a tree falling or growing back) until the leaf's state changes
            public int version;   // TreeLeaf.getStateVersion when cached
            public int gen;       // the view's cache generation it belongs to
        }
        readonly Dictionary<object, LeafCache> _leafCache = new Dictionary<object, LeafCache>(RefEq.Instance);
        readonly List<LeafCache> _visibleLeaves = new List<LeafCache>();
        static int _leafGen = 1;   // (static: a new view must not take another view's caches off the leaves)
        System.Func<com.oddlabs.tt.landscape.TreeLeaf, bool> _leafHandler;
        bool _leafReal, _leafOn;
        Material _leafMat;

        bool HandleLeaf(com.oddlabs.tt.landscape.TreeLeaf leaf)
        {
            if (!_leafOn) return false;
            var arr = leaf.getTrees();
            if (arr.Length == 0) return true;
            int ver = leaf.getStateVersion();
            // looked at again only when a tree of it was added, taken out, fell, vanished or grew back
            // (kept on the leaf itself: no lookup for each of thousands of leaves a frame)
            var lc = leaf.render_cache as LeafCache;
            if (lc == null || lc.gen != _leafGen || !ReferenceEquals(lc.trees, arr) || lc.version != ver)
            {
                bool moving = false;
                for (int i = 0; i < arr.Length && !moving; i++)
                    if ((arr[i].isEmpty() && !arr[i].isHidden()) || arr[i].getScale() < 1f) moving = true;   // falling or growing
                lc = moving ? new LeafCache { trees = arr, failed = true } : BuildLeaf(arr) ?? new LeafCache { trees = arr, failed = true };
                lc.version = ver;
                lc.gen = _leafGen;
                leaf.render_cache = lc;
            }
            if (lc.failed) return false;
            if (lc.count == 0) return true;   // every tree of it felled
            _visibleLeaves.Add(lc);
            return true;
        }

        LeafCache BuildLeaf(com.oddlabs.tt.landscape.TreeSupply[] arr)
        {
            var near = new Dictionary<object, List<Matrix4x4>>();
            var far = new Dictionary<object, List<Matrix4x4>>();
            void Put(Dictionary<object, List<Matrix4x4>> d, object k, Matrix4x4 m)
            {
                if (!d.TryGetValue(k, out var l)) d[k] = l = new List<Matrix4x4>();
                l.Add(m);
            }
            Vector3 sum = Vector3.zero;
            int standing = 0;
            foreach (var t in arr)
            {
                if (t.isHidden() || t.isEmpty()) continue;   // felled: gone until it grows back
                standing++;
                var e = MakeTreeEntry(FaithfulScene.StandingTree(t), _leafReal, _leafMat);
                sum += e.pos;
                if (e.v != null)
                {
                    e.impostor ??= TTImpostors.Get(e.v, _leafMat);
                    foreach (var p in e.v.parts) Put(near, p, e.mm * p.local);
                    if (e.impostor != null) Put(far, e.impostor, e.mm * e.impostor.local);
                    else foreach (var p in e.v.parts) Put(far, p, e.mm * p.local);
                    continue;
                }
                // the original model: the same near and far
                foreach (var d in new[] { e.trunk, e.crown })
                {
                    if (d == null || d.model == null) continue;
                    var ib = InstanceBatch(d, 0, true);
                    if (ib == null) continue;
                    Put(near, d, e.m * ib.local);
                    Put(far, d, e.m * ib.local);
                }
            }
            var lc = new LeafCache { trees = arr, center = sum / Mathf.Max(1, standing), count = standing };
            bool first = true;
            foreach (var t in arr)
            {
                if (t.isHidden() || t.isEmpty()) continue;
                var tm = TTSpace.ToUnity(FaithfulScene.StandingTree(t).xf);
                var pos = new Vector3(tm.m03, tm.m13, tm.m23);
                if (first) { lc.bounds = new Bounds(pos, Vector3.zero); first = false; } else lc.bounds.Encapsulate(pos);
            }
            // a tree's crown spreads round its trunk and rises well above it
            lc.bounds.min -= new Vector3(14f, 4f, 14f);
            lc.bounds.max += new Vector3(14f, 40f, 14f);
            (lc.nearKeys, lc.nearMats) = Flatten(near);
            (lc.farKeys, lc.farMats) = Flatten(far);
            return lc;
        }

        static (object[], Matrix4x4[][]) Flatten(Dictionary<object, List<Matrix4x4>> d)
        {
            var keys = new object[d.Count]; var mats = new Matrix4x4[d.Count][];
            int k = 0;
            foreach (var kv in d) { keys[k] = kv.Key; mats[k++] = kv.Value.ToArray(); }
            return (keys, mats);
        }

        readonly Dictionary<TTRealTrees.Part, Batch> _shadowOnlyBatches = new Dictionary<TTRealTrees.Part, Batch>();

        /// <summary>A part drawn into the shadow maps only.</summary>
        Batch ShadowOnlyBatch(TTRealTrees.Part p)
        {
            if (!_shadowOnlyBatches.TryGetValue(p, out var b))
            {
                b = new Batch { mesh = p.mesh, submesh = p.submesh, material = p.material, local = p.local, shared = true, shadowOnly = true };
                _shadowOnlyBatches[p] = b;
                _batchList.Add(b);
            }
            return b;
        }

        /// <summary>Realistic trees nearer than this cast their own shadow; further out (up to the tree shadow distance)
        /// the full model casts none and its impostor's two cards cast it instead - a fraction of the triangles in
        /// every shadow cascade, and at that distance the same shadow.</summary>
        const float FullTreeShadowDistance = 70f;

        // the leaves' own batches (kept from frame to frame while the same leaves are drawn the same way)
        readonly Dictionary<(object, int), Batch> _leafBatchMap = new Dictionary<(object, int), Batch>();
        readonly List<Batch> _leafBatchList = new List<Batch>();
        readonly List<byte> _leafFlags = new List<byte>();
        ulong _leafSig;
        int _leafSigCount = -1;

        Bounds _fillBounds;
        void LeafFill(Batch b, Matrix4x4[] mats)
        {
            if (b == null) return;
            b.matrices.AddRange(mats);
            if (!b.hasBounds) { b.bounds = _fillBounds; b.hasBounds = true; } else b.bounds.Encapsulate(_fillBounds);
        }

        /// <summary>A leaf batch: mode 0 no shadows, 1 with shadows, 2 shadows only.</summary>
        Batch LeafB(object key, int mode)
        {
            if (_leafBatchMap.TryGetValue((key, mode), out var b)) return b;
            if (key is TTRealTrees.Part p)
                b = new Batch { mesh = p.mesh, submesh = p.submesh, material = p.material, local = p.local, shared = true, noShadows = mode == 0, shadowOnly = mode == 2 };
            else if (mode != 2 && key is TTSpriteDefinition d && InstanceBatch(d, 0, mode == 1) is Batch src)
                b = new Batch { mesh = src.mesh, submesh = src.submesh, material = src.material, local = src.local, shared = true, noShadows = mode == 0 };
            else return null;
            _leafBatchMap[(key, mode)] = b;
            _leafBatchList.Add(b);
            return b;
        }

        Batch LeafBatch(object key, bool shadows) => key is TTRealTrees.Part p ? RealBatch(p, shadows) : InstanceBatch((TTSpriteDefinition)key, 0, shadows);

        Batch RealBatch(TTRealTrees.Part p, bool shadows)
        {
            if (!_realBatches.TryGetValue((p, shadows), out var b))
            {
                b = new Batch { mesh = p.mesh, submesh = p.submesh, material = p.material, local = p.local, shared = true, noShadows = !shadows };
                _realBatches[(p, shadows)] = b;
                _batchList.Add(b);
            }
            return b;
        }
        struct TreeEntry
        {
            public Matrix4x4 m, mm;
            public Vector3 pos;
            public TTRealTrees.Variant v;
            public TTRealTrees.Part impostor;
            public TTSpriteDefinition trunk, crown;
        }
        readonly Dictionary<object, TreeEntry> _treeCache = new Dictionary<object, TreeEntry>(RefEq.Instance);
        object _treeWorld;
        bool _treeReal;
        readonly Dictionary<string, (TTSpriteDefinition trunk, TTSpriteDefinition crown)> _treeDefs = new Dictionary<string, (TTSpriteDefinition, TTSpriteDefinition)>();

        TreeEntry MakeTreeEntry(Renderable t, bool real, Material bm)
        {
            var e = new TreeEntry { m = TTSpace.ToUnity(t.xf) };
            e.pos = new Vector3(e.m.m03, e.m.m13, e.m.m23);
            if (!_treeDefs.TryGetValue(t.location, out var defs))
                _treeDefs[t.location] = defs = (TreeDef(t.location, "_trunk"), TreeDef(t.location, "_crown"));
            e.trunk = defs.trunk; e.crown = defs.crown;
            bool palm = t.location == "palm";
            if (real && (palm ? TTPalms.Available : TTRealTrees.Has(t.location)))
            {
                float th = TreeHeight(t.location, e.trunk, e.crown);
                e.v = palm ? TTPalms.Pick(e.m.m03, e.m.m23, th, bm) : TTRealTrees.Pick(t.location, e.m.m03, e.m.m23, th, bm);
                if (e.v != null)
                {
                    // yaw from the position too, so neighbouring copies don't line up
                    int hash = (int)(e.m.m03 * 13.1f + e.m.m23 * 7.3f);
                    float yaw = (hash & 255) * (360f / 256f);
                    float size = 0.86f + ((hash >> 8) & 63) * (0.26f / 63f);   // a little variety in size
                    e.mm = e.m * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, yaw, 0), Vector3.one * size);
                }
            }
            return e;
        }

        readonly Dictionary<string, float> _treeHeights = new Dictionary<string, float>();

        static readonly List<MeshFilter> s_weaponScratch = new List<MeshFilter>();
        readonly List<Entry> _weaponEntries = new List<Entry>();

        /// <summary>The near warriors' weapons, flat on the ground along the sun (after every unit is posed, the
        /// two-bone weapons turned for this pose first).</summary>
        void HeldWeaponShadows()
        {
            if (!TTPlanarShadows.Active) return;
            foreach (var e in _weaponEntries)
            {
                if (e.go == null || !e.go.activeInHierarchy || e.weapons == null) continue;
                var foot = e.go.transform.position;
                for (int k = 0; k < e.weapons.Length; k++)
                {
                    var mf = e.weapons[k];
                    if (mf == null || !mf.gameObject.activeInHierarchy) continue;
                    if (e.aligners != null && e.aligners[k] != null) { e.aligners[k].Align(); if (!e.aligners[k].Visible) continue; }
                    TTPlanarShadows.AddHeld(mf.sharedMesh, mf.transform.localToWorldMatrix, foot);
                }
            }
            _weaponEntries.Clear();
        }

        void AddReal(TTRealTrees.Part p, Matrix4x4 m, bool shadows = true)
        {
            if (p == null) return;
            if (!_realBatches.TryGetValue((p, shadows), out var b))
            {
                b = new Batch { mesh = p.mesh, submesh = p.submesh, material = p.material, local = p.local, shared = true, noShadows = !shadows };
                _realBatches[(p, shadows)] = b;
                _batchList.Add(b);
            }
            b.matrices.Add(m * b.local);
        }

        /// <summary>Per-frame counters for the F9 performance overlay.</summary>
        public int statTrees, statRealTrees, statImpostors, statItems, statInstances;
        public int statLive => _live.Count;
        public int statCulled, statFar, statInstBatches, statInstShadowBatches, statStatic;

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
            if (m.getPositionX() != e.x || m.getPositionY() != e.y) { _static.Remove(m); return false; }   // moved (a carried flag): worked out again
            float d2 = (e.pos - _staticCam).sqrMagnitude;
            if (e.plants && d2 > plantsCutDistance * plantsCutDistance) return true;
            (d2 < _staticShadow2 ? e.shadow : e.noShadow).matrices.Add(e.mm);
            statStatic++;
            return true;
        }

        /// <summary>Units (and what they carry) further than this are drawn in bulk from baked poses (TTPoseBaker)
        /// instead of as skinned objects of their own; their objects are switched off meanwhile.</summary>
        const float FarUnitDistance = 70f;
        const int MaxNearUnits = 260;
        float _nearScale = 1f, _nearR2 = FarUnitDistance * FarUnitDistance;
        int _nearCount;
        public int statNear;
        readonly Dictionary<(Mesh, Material, int, bool), Batch> _farBatches = new Dictionary<(Mesh, Material, int, bool), Batch>();
        static readonly int TeamColorId = Shader.PropertyToID("_TeamColor");

        bool DrawFar(in Renderable it, TTSpriteDefinition def, Matrix4x4 m, Vector3 camPos, float shadow2)
        {
            long t0 = System.Diagnostics.Stopwatch.GetTimestamp();
            try { return DrawFarTimed(it, def, m, camPos, shadow2); }
            finally { _tFar += System.Diagnostics.Stopwatch.GetTimestamp() - t0; }
        }

        long _tFar, _tPlace, _tExt;
        public float statFarMs, statPlaceMs, statExtMs, statDrawMs, statParticlesMs, statFxMs;

        bool DrawFarTimed(in Renderable it, TTSpriteDefinition def, Matrix4x4 m, Vector3 camPos, float shadow2)
        {
            if (_catalog == null || def.animations.Length == 0) return false;
            float d2 = (new Vector3(m.m03, m.m13, m.m23) - camPos).sqrMagnitude;
            if (d2 < _nearR2) { if (it.kind == RKind.Unit) _nearCount++; return false; }
            int want = def.lods.Length > 1 && d2 > lodDistance * lodDistance ? def.lods.Length - 1 : 0;
            // poses per animation: every one inside the near distance (a crowd), 4 near it, 2 beyond 130 m, one beyond 240 m
            int maxFrames = d2 > 240f * 240f ? 1 : d2 > 130f * 130f ? 2 : d2 > FarUnitDistance * FarUnitDistance ? 4 : 0;
            // the pose texture (smooth: the GPU blends each unit's poses), or until it is ready the nearest baked pose
            var vat = TribalTrouble.Presentation.TTPoseBaker.GetVat(def, want);
            if (vat != null)
            {
                var vtex = def.lods[vat.lod].textures;
                // a New Tides unit wears its own body skin in the crowd too (they all looked like the plain warriors):
                // handed to the draw (as the near units' skins are), not baked into a copy of the material - drawn
                // with such a copy, every elite and race unit in the crowd vanished (zoomed out, or in a big battle)
                var vskin = it.model is com.oddlabs.tt.model.Unit su && su.getOwner().getWorld().isExtended() ? TTExtendedArt.BodySkin(su) : null;
                var vmat = VatMaterial(def, vat, vtex.Length == 0 ? 0 : Mathf.Clamp(it.texture, 0, vtex.Length - 1));
                if (vmat != null)
                {
                    var vb = VatBatchFor(vat, vmat, d2 < shadow2, def, vskin);
                    var tc = new Color(it.r, it.g, it.b, 1f).linear;   // (SetColor would convert; a vector array doesn't)
                    vb.Add(m * vat.bakedToRoot, TribalTrouble.Presentation.TTPoseBaker.VatPose(vat, def, it.anim, it.ticks), new Vector4(tc.r, tc.g, tc.b, 1f));
                    if (_live.TryGetValue(it.key, out var ve))
                    {
                        ve.seen = _frame;
                        if (ve.go != null && ve.go.activeSelf) ve.go.SetActive(false);
                    }
                    statFar++;
                    return true;
                }
            }
            if (!TribalTrouble.Presentation.TTPoseBaker.Get(def, want, it.anim, it.ticks, out var mesh, out var local, out int lod, maxFrames)) return false;
            var tex = def.lods[lod].textures;
            var mat = TribalTrouble.Presentation.TTSpriteView.GetMaterial(def, lod, tex.Length == 0 ? 0 : Mathf.Clamp(it.texture, 0, tex.Length - 1), _catalog.baseMaterial);
            if (mat == null) return false;
            if (!mat.enableInstancing) mat.enableInstancing = true;
            bool shadows = d2 < shadow2;
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
            if (_live.TryGetValue(it.key, out var e))
            {
                e.seen = _frame;
                if (e.go != null && e.go.activeSelf) e.go.SetActive(false);
            }
            statFar++;
            return true;
        }
        public float statCollectMs, statBuildMs;
        readonly System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();

        /// <summary>Height of the original tree model (trunk + crown), so replacements keep the same size on the map.</summary>
        float TreeHeight(string tree, TTSpriteDefinition trunk, TTSpriteDefinition crown)
        {
            if (_treeHeights.TryGetValue(tree, out float h)) return h;
            h = 0f;
            foreach (var d in new[] { trunk, crown })
            {
                if (d == null || d.model == null) continue;
                foreach (var f in d.model.GetComponentsInChildren<MeshFilter>(true))
                {
                    if (f.sharedMesh == null) continue;
                    var mm = d.model.transform.worldToLocalMatrix * f.transform.localToWorldMatrix;
                    var bb = f.sharedMesh.bounds;
                    for (int i = 0; i < 8; i++)
                    {
                        var c = new Vector3((i & 1) == 0 ? bb.min.x : bb.max.x, (i & 2) == 0 ? bb.min.y : bb.max.y, (i & 4) == 0 ? bb.min.z : bb.max.z);
                        h = Mathf.Max(h, mm.MultiplyPoint3x4(c).y);
                    }
                }
            }
            if (h <= 0.5f) h = 10f;
            h *= 1.1f;
            _treeHeights[tree] = h;
            return h;
        }

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
                    // same mesh and material, drawn without shadow casting
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
            var mat = new Material(_catalog.baseMaterial) { enableInstancing = true, name = "Instanced " + def.Key + " #" + texture };
            var tex = def.lods.Length > 0 ? def.lods[0].textures : System.Array.Empty<TTSpriteDefinition.TextureVariant>();
            if (tex.Length > 0)
            {
                int rare = texture >= 100 ? texture - 100 : -1;   // Extended rules: recoloured rock (gold, coal)
                var v = tex[Mathf.Clamp(rare >= 0 ? 0 : texture, 0, tex.Length - 1)];
                var bm = v.baseMap != null ? v.baseMap : TTGeneratedTextures.Get(v.name);
                // the Modern look: the resource rocks wear HD ore textures of their own (PortTools/ores) instead of the
                // 128 px generated rock / iron and the tinted rock for gold, coal and diamonds
                var ore = TTOres.For(v.name, rare, texture >= tex.Length);
                if (ore != null) bm = ore;
                if (bm != null && bm.anisoLevel < 8) bm.anisoLevel = 8;
                mat.SetTexture("_BaseMap", bm != null ? bm : Texture2D.whiteTexture);
                if (ore != null) { mat.SetColor("_Tint", Color.white); mat.SetFloat("_Gloss", TTOres.Gloss(v.name, rare, texture >= tex.Length)); }
                else if (rare >= 0) { mat.SetColor("_Tint", TTExtendedArt.RareTints[rare]); mat.SetFloat("_Gloss", TTExtendedArt.RareGloss(rare)); }
                else if (texture >= tex.Length) mat.SetColor("_Tint", IronTint);   // variant missing from the import (iron rocks)
            }
            else mat.SetTexture("_BaseMap", Texture2D.whiteTexture);
            mat.SetFloat("_UseTeamMask", 0f);
            if (def.lods.Length > 0 && def.lods[0].alphaClip)
            {
                mat.EnableKeyword("_ALPHATEST_ON");
                mat.SetFloat("_AlphaClip", 1f);
                mat.SetFloat("_Cull", 0f);
                mat.SetFloat("_Wind", 1f);       // foliage sways (modern style)
                mat.SetFloat("_Gloss", 0.3f);
            }
            return new Batch
            {
                mesh = mf.sharedMesh, material = mat,
                local = def.model.transform.worldToLocalMatrix * mf.transform.localToWorldMatrix,
            };
        }

        /// <summary>The batches with the most instances (mesh, material: instances / draws), worked out now and then
        /// for the benchmark report.</summary>
        public string statTopBatches;
        int _topFrame;

        void TopBatches()
        {
            var top = new List<Batch>();
            foreach (var b in _batchList) if (b.matrices.Count > 0) top.Add(b);
            foreach (var b in _leafBatchList) if (b.matrices.Count > 0) top.Add(b);
            top.Sort((a, b) => b.matrices.Count.CompareTo(a.matrices.Count));
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < top.Count && i < 8; i++)
            {
                var b = top[i];
                sb.Append(i == 0 ? "" : "; ").Append(b.mesh != null ? b.mesh.name : "?").Append(" / ").Append(b.material != null ? b.material.name : "?")
                  .Append(b.shadowOnly ? " (shadow)" : b.noShadows ? "" : " (+shadow)").Append(": ").Append(b.matrices.Count);
            }
            statTopBatches = $"{top.Count} batches in use of {_batchList.Count}; top: {sb}";
        }

        // ---- units in bulk from pose textures (TTPoseBaker.Vat): one instanced draw per model, material and shadow
        // setting for every pose and team; each instance carries its two poses, blend and team colour
        sealed class VatBatch
        {
            public Mesh mesh;
            public Mesh shadowMesh;   // the same model without its painted weapon (the shadow it casts), or null
            public Material mat;
            public Texture2D skin;   // a New Tides unit's body skin (null: the material's own texture)
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
        readonly Dictionary<(object, Material, bool, Texture2D), VatBatch> _vatBatches = new Dictionary<(object, Material, bool, Texture2D), VatBatch>();
        static readonly int VatBaseMapId = Shader.PropertyToID("_BaseMap");
        readonly List<VatBatch> _vatList = new List<VatBatch>();
        readonly Dictionary<(object, int), Material> _vatMats = new Dictionary<(object, int), Material>();
        readonly Dictionary<(object, int, Texture2D), Material> _vatSkinMats = new Dictionary<(object, int, Texture2D), Material>();
        static readonly int TTAnimId = Shader.PropertyToID("_TTAnim"), TTTeamId = Shader.PropertyToID("_TTTeam"), TTVatTexId = Shader.PropertyToID("_TTVatTex");
        readonly Matrix4x4[] _vatM = new Matrix4x4[1023];
        readonly Vector4[] _vatA = new Vector4[1023], _vatT = new Vector4[1023];

        Material VatMaterial(TTSpriteDefinition def, TribalTrouble.Presentation.TTPoseBaker.Vat vat, int variant, Texture2D skin = null)
        {
            if (skin != null)
            {
                if (_vatSkinMats.TryGetValue((vat, variant, skin), out var sm)) return sm;
                var plain = VatMaterial(def, vat, variant);
                sm = plain != null ? new Material(plain) { name = plain.name + " " + skin.name } : null;
                if (sm != null) sm.SetTexture("_BaseMap", skin);
                _vatSkinMats[(vat, variant, skin)] = sm;
                return sm;
            }
            if (_vatMats.TryGetValue((vat, variant), out var vm)) return vm;
            var bm = TribalTrouble.Presentation.TTSpriteView.GetMaterial(def, vat.lod, variant, _catalog.baseMaterial);
            if (bm == null) { _vatMats[(vat, variant)] = null; return null; }
            // a shader without the pose-texture variant: bulk units keep their baked poses
            if (!new UnityEngine.Rendering.LocalKeyword(bm.shader, "_TT_VAT").isValid)
            {
                TribalTrouble.Presentation.TTPoseBaker.VatEnabled = false;
                Debug.LogWarning("[TT] " + bm.shader.name + " has no _TT_VAT variant: units in bulk use baked poses");
                _vatMats[(vat, variant)] = null;
                return null;
            }
            vm = new Material(bm) { name = bm.name + " (poses)", enableInstancing = true };
            vm.EnableKeyword("_TT_VAT");
            vm.SetTexture(TTVatTexId, vat.tex);
            _vatMats[(vat, variant)] = vm;
            return vm;
        }

        bool _vatStripWeapons;
        VatBatch VatBatchFor(TribalTrouble.Presentation.TTPoseBaker.Vat vat, Material mat, bool shadows, TTSpriteDefinition def, Texture2D skin = null)
        {
            if (!_vatBatches.TryGetValue((vat, mat, shadows, skin), out var b))
            {
                b = new VatBatch { mesh = vat.mesh, mat = mat, shadows = shadows, shadowMesh = shadows ? vat.shadowMesh : null, skin = skin };
                _vatBatches[(vat, mat, shadows, skin)] = b;
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
                    if (b.skin != null) mpb.SetTexture(VatBaseMapId, b.skin);
                    bool split = b.shadows && b.shadowMesh != null && _vatStripWeapons;   // shadow from the weaponless mesh
                    Graphics.DrawMeshInstanced(b.mesh, 0, b.mat, _vatM, c, mpb,
                        b.shadows && !split ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off, true, 0, null,
                        UnityEngine.Rendering.LightProbeUsage.Off);
                    if (split)
                        Graphics.DrawMeshInstanced(b.shadowMesh, 0, b.mat, _vatM, c, mpb, UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly, false, 0, null,
                            UnityEngine.Rendering.LightProbeUsage.Off);
                    statInstBatches++;
                    if (b.shadows) statInstShadowBatches++;
                }
                b.m.Clear();
            }
        }

        void DrawBatches()
        {
            if (++_topFrame % 120 == 0) TopBatches();
            statInstances = 0; statInstBatches = 0; statInstShadowBatches = 0;
            for (int li = 0; li < 2; li++)
            foreach (var b in li == 0 ? _batchList : _leafBatchList)
            {
                int n = b.matrices.Count;
                if (n == 0) continue;
                statInstances += n;
                statInstBatches += (n + 1022) / 1023;
                if (!b.noShadows) statInstShadowBatches += (n + 1022) / 1023;
                var rp = new RenderParams(b.material)
                {
                    shadowCastingMode = b.shadowOnly ? UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : b.noShadows ? UnityEngine.Rendering.ShadowCastingMode.Off : UnityEngine.Rendering.ShadowCastingMode.On,
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
            _procBatches.Clear();
            _realBatches.Clear();
            _farBatches.Clear();
            _shadowOnlyBatches.Clear();
            _leafBatchMap.Clear(); _leafBatchList.Clear(); _leafSigCount = -1; _leafSig = 0;
            foreach (var vb in _vatList) vb.m.Clear();
            particles.Clear();
        }
    }
}
