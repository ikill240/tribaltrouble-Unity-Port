using System.Collections.Generic;
using TribalTrouble.Presentation;
using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Presents the simulation: a TTSpriteView per unit/building (team colour, animation by TT ticks),
    /// GPU-instanced trees / rocks / iron / plants, and selection rings.
    /// </summary>
    public sealed class GameView
    {
        readonly GameSim _sim;
        readonly TTSpriteCatalog _catalog;
        readonly Transform _root;
        readonly Dictionary<Entity, TTSpriteView> _views = new Dictionary<Entity, TTSpriteView>();
        readonly Dictionary<Building, string> _buildingStage = new Dictionary<Building, string>();
        readonly List<InstanceBatch> _batches = new List<InstanceBatch>();
        bool _suppliesDirty = true;
        Mesh _ring;
        Material _ringMaterial;
        static readonly int ColorId = Shader.PropertyToID("_BaseColor");
        public readonly HashSet<Entity> selected = new HashSet<Entity>();

        /// <summary>glTFast mirrors X when importing, so TT's +X "forward" ends up on -X: rotate so it faces movement.</summary>
        public static float ModelYawOffset = 90f;

        sealed class InstanceBatch
        {
            public Mesh mesh;
            public Material material;
            public readonly List<Matrix4x4> matrices = new List<Matrix4x4>();
            public Matrix4x4[] array = new Matrix4x4[0];
            public Matrix4x4 local = Matrix4x4.identity;   // mesh node's transform relative to the model root
        }

        public GameView(GameSim sim, TTSpriteCatalog catalog, Transform root)
        {
            _sim = sim;
            _catalog = catalog;
            _root = root;
            sim.Spawned += OnSpawned;
            sim.Removed += OnRemoved;
            foreach (var u in sim.units) OnSpawned(u);
            BuildRing();
        }

        string Group(Player p) => p.race == Race.Vikings ? "vikings" : "natives";

        TTSpriteDefinition Def(string group, string name) => _catalog != null ? _catalog.Find(group, name) : null;

        void OnSpawned(Entity e)
        {
            if (e == null || _views.ContainsKey(e)) return;
            var go = new GameObject(e is Unit ? "Unit " + e.id : "Building " + e.id);
            go.transform.SetParent(_root, false);
            var view = go.AddComponent<TTSpriteView>();
            _views[e] = view;
            e.view = go;
            if (e is Unit u)
            {
                string sprite = u.type == UnitType.Peon ? "peon" : "warrior";
                int variant = u.type == UnitType.WarriorIron ? 1 : 0;
                view.Init(Def(Group(u.owner), sprite), _catalog.baseMaterial, u.owner.color, 0, variant);
            }
            else if (e is Building b)
            {
                SetBuildingModel(b, view);
            }
            Sync(e, view, 1f);
        }

        void SetBuildingModel(Building b, TTSpriteView view)
        {
            string baseName = b.type == BuildingType.Quarters ? "quarters" : b.type == BuildingType.Armory ? "armory" : "tower";
            string name = b.stage == "built" ? baseName : baseName + "_" + b.stage;
            view.Init(Def(Group(b.owner), name), _catalog.baseMaterial, b.owner.color);
            _buildingStage[b] = b.stage;
        }

        void OnRemoved(Entity e)
        {
            if (e == null) { _suppliesDirty = true; return; }
            if (_views.TryGetValue(e, out var v))
            {
                if (v != null) Object.Destroy(v.gameObject);
                _views.Remove(e);
            }
            if (e is Building b) _buildingStage.Remove(b);
            selected.Remove(e);
        }

        /// <summary>Called every frame (alpha = interpolation 0..1 toward the latest tick).</summary>
        public void Update(float smoothing)
        {
            foreach (var u in _sim.units)
            {
                if (!_views.TryGetValue(u, out var v)) { OnSpawned(u); continue; }
                Sync(u, v, smoothing);
            }
            foreach (var b in _sim.buildings)
            {
                if (!_views.TryGetValue(b, out var v)) { OnSpawned(b); continue; }
                if (!_buildingStage.TryGetValue(b, out var st) || st != b.stage) SetBuildingModel(b, v);
            }
            if (_suppliesDirty) RebuildSupplies();
            DrawSupplies();
            DrawRings();
        }

        void Sync(Entity e, TTSpriteView v, float smoothing)
        {
            var target = _sim.world.ToWorld(e.pos);
            var t = v.transform;
            t.position = smoothing >= 1f ? target : Vector3.Lerp(t.position, target, smoothing);
            if (e is Unit u)
            {
                float yaw = -u.facing * Mathf.Rad2Deg + 90f + ModelYawOffset;
                t.rotation = Quaternion.Slerp(t.rotation, Quaternion.Euler(0, yaw, 0), Mathf.Clamp01(smoothing * 1.5f));
                var def = v.Definition;
                if (def != null)
                {
                    int ai = def.FindAnimation(u.anim);
                    if (ai < 0) ai = def.FindAnimation("idle");
                    if (ai >= 0)
                    {
                        // run ticks are metres (wpc = metres per cycle); other states count cycles
                        float cycles = u.anim == "attack" ? u.animTicks - Mathf.Floor(u.animTicks) : u.animTicks;   // repeated swings
                        float ticks = u.anim == "run" ? u.animTicks : cycles * def.animations[ai].wpc;
                        if (ai != v.AnimationIndex) v.SetAnimation(ai, ticks);
                        else v.SetAnimationTicks(ticks);
                    }
                }
            }
        }

        // ------------------------------------------------------------------ supplies (instanced)

        void RebuildSupplies()
        {
            _suppliesDirty = false;
            foreach (var b in _batches) if (b != null) b.matrices.Clear();   // MakeBatch returns null for missing sprites
            if (_batches.Count == 0) CreateSupplyBatches();
            foreach (var s in _sim.world.supplies)
            {
                if (s.Depleted) continue;
                var center = GameWorld.CellCenter(s.gx, s.gy);
                var pos = _sim.world.ToWorld(center);
                // stable per-node rotation / scale from its id
                float rot = (s.id * 137.5f) % 360f;
                float scale = 0.9f + ((s.id * 7919) % 100) / 500f;
                var m = Matrix4x4.TRS(pos, Quaternion.Euler(0, rot, 0), Vector3.one * scale);
                if (s.kind == SupplyKind.Wood)
                {
                    int bi = s.palm ? 2 : 0;   // [0,1] trees (trunk, crown), [2,3] palms/pines
                    if (bi + 1 < _batches.Count)
                    {
                        if (_batches[bi] != null) _batches[bi].matrices.Add(m * _batches[bi].local);
                        if (_batches[bi + 1] != null) _batches[bi + 1].matrices.Add(m * _batches[bi + 1].local);
                    }
                }
                else
                {
                    // [4..8] rock variants, [9..13] iron variants
                    bool iron = s.kind == SupplyKind.Iron;
                    int bi = (iron ? 9 : 4) + (s.id % 5);
                    if (bi < _batches.Count && _batches[bi] != null)
                        _batches[bi].matrices.Add(Matrix4x4.TRS(pos + Vector3.up * 0.3f, Quaternion.Euler(0, rot, 0),
                            Vector3.one * (iron ? 0.8f : 1f)) * _batches[bi].local);
                }
            }
            foreach (var b in _batches)
                if (b != null) b.array = b.matrices.ToArray();
        }

        void CreateSupplyBatches()
        {
            bool native = _sim.world.terrain == Sim.Procedural.Landscape.TerrainType.NATIVE;
            string treeA = native ? "jungle_tree" : "oak_tree";
            string treeB = native ? "palm" : "pine_tree";
            _batches.Add(MakeBatch(treeA + "_trunk", null));
            _batches.Add(MakeBatch(treeA + "_crown", null));
            _batches.Add(MakeBatch(treeB + "_trunk", null));
            _batches.Add(MakeBatch(treeB + "_crown", null));
            for (int i = 1; i <= 5; i++) _batches.Add(MakeBatch("rock_" + i, new Color(0.62f, 0.6f, 0.56f)));   // rock (procedural texture in TT)
            for (int i = 1; i <= 5; i++) _batches.Add(MakeBatch("rock_" + i, new Color(0.45f, 0.3f, 0.25f)));   // iron ore tint
        }

        InstanceBatch MakeBatch(string spriteName, Color? tint)
        {
            var def = Def("misc", spriteName);
            if (def == null || def.model == null) return null;
            MeshFilter mf = null;
            if (def.lods.Length > 0 && !string.IsNullOrEmpty(def.lods[0].nodeName))
                foreach (var f in def.model.GetComponentsInChildren<MeshFilter>(true))
                    if (f.name == def.lods[0].nodeName) { mf = f; break; }
            if (mf == null) mf = def.model.GetComponentInChildren<MeshFilter>(true);
            if (mf == null || mf.sharedMesh == null) return null;
            var mat = new Material(_catalog.baseMaterial) { enableInstancing = true, name = "Instanced " + spriteName };
            if (def.lods.Length > 0 && def.lods[0].textures.Length > 0 && def.lods[0].textures[0].baseMap != null)
                mat.SetTexture("_BaseMap", def.lods[0].textures[0].baseMap);
            else
                mat.SetTexture("_BaseMap", Texture2D.whiteTexture);
            mat.SetFloat("_UseTeamMask", 0f);
            if (tint.HasValue) mat.SetColor("_Tint", tint.Value);
            if (def.lods.Length > 0 && def.lods[0].alphaClip)
            {
                mat.EnableKeyword("_ALPHATEST_ON");
                mat.SetFloat("_AlphaClip", 1f);
                mat.SetFloat("_Cull", 0f);
            }
            return new InstanceBatch
            {
                mesh = mf.sharedMesh, material = mat,
                local = def.model.transform.worldToLocalMatrix * mf.transform.localToWorldMatrix,
            };
        }

        void DrawSupplies()
        {
            foreach (var b in _batches)
            {
                if (b == null || b.array.Length == 0) continue;
                var rp = new RenderParams(b.material) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On, receiveShadows = true };
                for (int start = 0; start < b.array.Length; start += 1023)
                    Graphics.RenderMeshInstanced(rp, b.mesh, 0, b.array, Mathf.Min(1023, b.array.Length - start), start);
            }
        }

        // ------------------------------------------------------------------ selection rings

        void BuildRing()
        {
            const int seg = 32;
            var v = new Vector3[seg * 2];
            var t = new int[seg * 6];
            for (int i = 0; i < seg; i++)
            {
                float a = i * Mathf.PI * 2f / seg;
                var d = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
                v[i * 2] = d * 0.85f;
                v[i * 2 + 1] = d * 1f;
                int n = (i + 1) % seg;
                int k = i * 6;
                t[k] = i * 2; t[k + 1] = n * 2; t[k + 2] = i * 2 + 1;
                t[k + 3] = i * 2 + 1; t[k + 4] = n * 2; t[k + 5] = n * 2 + 1;
            }
            _ring = new Mesh { name = "SelectionRing", vertices = v, triangles = t };
            _ring.RecalculateBounds();
            var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
            _ringMaterial = new Material(shader) { name = "SelectionRing" };
            _ringMaterial.SetColor(ColorId, new Color(0.4f, 1f, 0.4f));
        }

        void DrawRings()
        {
            if (selected.Count == 0 || _ring == null) return;
            var rp = new RenderParams(_ringMaterial);
            foreach (var e in selected)
            {
                if (e == null || !e.alive) continue;
                var p = _sim.world.ToWorld(e.pos) + Vector3.up * 0.15f;
                float r = e.Radius + 0.3f;
                Graphics.RenderMesh(rp, _ring, 0, Matrix4x4.TRS(p, Quaternion.identity, new Vector3(r, 1, r)));
            }
        }

        public TTSpriteView ViewOf(Entity e) => e != null && _views.TryGetValue(e, out var v) ? v : null;
    }
}
