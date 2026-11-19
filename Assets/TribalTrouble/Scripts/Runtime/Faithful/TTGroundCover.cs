using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Ground cover near the camera (modern look): tufts of grass and pebbles scattered over the island where the camera
    /// looks, so the ground between the trees isn't bare paint up close. Tropical islands get green grass with a few
    /// flowers, northern ones frost-tipped grass, the Rift withered tufts and dark basalt. Nothing on the beach, under
    /// the sea, on steep slopes or on building ground. Laid out per 8 m cell from the cell's own seed (so it stays put
    /// as the camera moves), drawn instanced, no shadows.
    /// </summary>
    public static class TTGroundCover
    {
        /// <summary>0 tropical, 1 northern, 2 the Rift (set when the world view is built).</summary>
        public static int Climate;

        const float Cell = 8f;
        const int MaxReach = 26;          // cells each way round the point the camera looks at, at most
        const int TuftsPerCell = 20, PebblesPerCell = 5;
        const int BuildsPerFrame = 14;    // cells laid out per frame at most
        const float NearCut = 75f;        // drawn this far from the camera close up (m) ...
        const float MaxCut = 210f;        // ... and further out as the camera rises, so the grass stays as one zooms out
        static float FarCut = NearCut;

        sealed class CellData { public Matrix4x4[] tufts, pebbles; public float made; }
        static readonly Dictionary<(int, int), CellData> s_cells = new Dictionary<(int, int), CellData>();
        static object s_world;
        static int s_climate = -1;
        static Mesh s_tuft, s_pebble;
        static Material s_grass, s_stone;
        static readonly List<Matrix4x4> s_t = new List<Matrix4x4>(4096), s_p = new List<Matrix4x4>(1024);
        static readonly Matrix4x4[] s_buf = new Matrix4x4[1023];

        public static void Draw(com.oddlabs.tt.landscape.World world, Camera cam, Material baseMaterial)
        {
            if (world == null || cam == null || baseMaterial == null) return;
            if (!ReferenceEquals(world, s_world) || s_climate != Climate) { s_cells.Clear(); s_world = world; s_climate = Climate; s_grass = null; s_stone = null; }
            Ensure(baseMaterial);
            var hm = world.getHeightMap();
            var cp = cam.transform.position;
            float camGround = hm.getNearestHeight(cp.x, cp.z);
            float above = cp.y - camGround;
            if (above > 320f) return;   // the whole island in view: too small to see
            FarCut = Mathf.Clamp(NearCut + (above - 30f) * 1.4f, NearCut, MaxCut);
            int Reach = Mathf.Min(MaxReach, Mathf.CeilToInt(FarCut / Cell) + 1);
            float thin2 = (NearCut + 15f) * (NearCut + 15f);   // beyond this, every other tuft and pebble (they're tiny there)
            // the ground point the camera looks at
            var f = cam.transform.forward;
            float t = f.y < -0.05f ? Mathf.Min((cp.y - camGround) / -f.y, Mathf.Max(70f, FarCut * 0.6f)) : 30f;
            var look = cp + f * t;
            int cx0 = Mathf.FloorToInt(look.x / Cell), cy0 = Mathf.FloorToInt(look.z / Cell);
            float now = Time.time;
            s_t.Clear(); s_p.Clear();
            float far2 = FarCut * FarCut;
            int budget = BuildsPerFrame;   // (bug sweep) lays out a few cells a frame, so panning and the refresh don't stutter
            for (int dy = -Reach; dy <= Reach; dy++)
                for (int dx = -Reach; dx <= Reach; dx++)
                {
                    int cx = cx0 + dx, cy = cy0 + dy;
                    float mx = (cx + 0.5f) * Cell - cp.x, my = (cy + 0.5f) * Cell - cp.z;
                    if (mx * mx + my * my > (FarCut + Cell) * (FarCut + Cell)) continue;
                    bool have = s_cells.TryGetValue((cx, cy), out var c);
                    // re-laid now and then (new buildings), each cell on its own beat so they don't all go at once
                    if ((!have || now - c.made > 10f + (((cx * 73856093) ^ (cy * 19349663)) & 15) * 0.4f) && budget > 0)
                    {
                        budget--;
                        s_cells[(cx, cy)] = c = Make(world, cx, cy, now);
                        have = true;
                    }
                    if (!have) continue;   // not laid out yet: next frame
                    for (int i = 0; i < c.tufts.Length; i++)
                    {
                        var m = c.tufts[i];
                        float ddx = m.m03 - cp.x, ddy = m.m23 - cp.z, d2 = ddx * ddx + ddy * ddy;
                        if (d2 < far2 && (d2 < thin2 || (i & 1) == 0)) s_t.Add(m);
                    }
                    for (int i = 0; i < c.pebbles.Length; i++)
                    {
                        var m = c.pebbles[i];
                        float ddx = m.m03 - cp.x, ddy = m.m23 - cp.z, d2 = ddx * ddx + ddy * ddy;
                        if (d2 < far2 && (d2 < thin2 || (i & 1) == 0)) s_p.Add(m);
                    }
                }
            if (s_cells.Count > 4000) Evict(cp);
            Submit(s_tuft, s_grass, s_t);
            Submit(s_pebble, s_stone, s_p);
        }

        static readonly List<(int, int)> s_drop = new List<(int, int)>();

        /// <summary>Forgets the cells far from the camera (not all of them: that rebuilt the whole view in one frame).</summary>
        static void Evict(Vector3 cp)
        {
            s_drop.Clear();
            float keep = (FarCut + Cell) * 2f;
            foreach (var kv in s_cells)
            {
                float mx = (kv.Key.Item1 + 0.5f) * Cell - cp.x, my = (kv.Key.Item2 + 0.5f) * Cell - cp.z;
                if (mx * mx + my * my > keep * keep) s_drop.Add(kv.Key);
            }
            foreach (var k in s_drop) s_cells.Remove(k);
            if (s_cells.Count > 4000) s_cells.Clear();
        }

        static void Submit(Mesh mesh, Material mat, List<Matrix4x4> list)
        {
            if (list.Count == 0) return;
            var rp = new RenderParams(mat) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off, receiveShadows = true };
            for (int start = 0; start < list.Count; start += 1023)
            {
                int n = Mathf.Min(1023, list.Count - start);
                list.CopyTo(start, s_buf, 0, n);
                Graphics.RenderMeshInstanced(rp, mesh, 0, s_buf, n);
            }
        }

        static uint Hash(int x, int y, int i)
        {
            uint h = (uint)(x * 73856093) ^ (uint)(y * 19349663) ^ (uint)(i * 83492791) ^ 0x9E3779B9u;
            h ^= h >> 13; h *= 1274126177u; h ^= h >> 16;
            return h;
        }

        static float Rand(int x, int y, int i, int k) => (Hash(x, y, i * 7 + k) & 0xFFFFFF) / (float)0xFFFFFF;

        static CellData Make(com.oddlabs.tt.landscape.World world, int cx, int cy, float now)
        {
            var hm = world.getHeightMap();
            var grid = world.getUnitGrid();
            int gs = grid.getGridSize();
            float sea = hm.getSeaLevelMeters();
            var tufts = new List<Matrix4x4>(TuftsPerCell);
            var pebbles = new List<Matrix4x4>(PebblesPerCell);
            // patchy: some cells thick with grass, some nearly bare
            float density = Mathf.Clamp01(0.25f + Rand(cx, cy, 999, 0) * 1.1f);
            for (int i = 0; i < TuftsPerCell + PebblesPerCell; i++)
            {
                bool pebble = i >= TuftsPerCell;
                if (!pebble && Rand(cx, cy, i, 5) > density) continue;
                float x = (cx + Rand(cx, cy, i, 0)) * Cell, y = (cy + Rand(cx, cy, i, 1)) * Cell;   // (Unity x, z = game x, y)
                float z = hm.computeInterpolatedHeight(0, x, y);
                if (z < sea + (pebble ? 0.3f : 1.1f)) continue;   // not on the beach or in the sea
                float sx = hm.computeInterpolatedHeight(0, x + 0.7f, y) - hm.computeInterpolatedHeight(0, x - 0.7f, y);
                float sy = hm.computeInterpolatedHeight(0, x, y + 0.7f) - hm.computeInterpolatedHeight(0, x, y - 0.7f);
                if (sx * sx + sy * sy > 0.9f) continue;          // not on steep slopes or cliffs
                int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y);
                if (gx < 0 || gy < 0 || gx >= gs || gy >= gs) continue;
                var occ = grid.getOccupant(gx, gy);
                if (occ != null && !(occ is com.oddlabs.tt.model.Unit)) continue;   // building ground, trees, rocks
                float yaw = Rand(cx, cy, i, 2) * 360f;
                if (pebble)
                {
                    float s = Mathf.Lerp(0.07f, 0.24f, Rand(cx, cy, i, 3) * Rand(cx, cy, i, 3));
                    var sc = new Vector3(s * Mathf.Lerp(0.8f, 1.4f, Rand(cx, cy, i, 4)), s * 0.6f, s);
                    pebbles.Add(Matrix4x4.TRS(new Vector3(x, z - s * 0.15f, y), Quaternion.Euler(0f, yaw, 0f), sc));
                }
                else
                {
                    float s = Mathf.Lerp(0.5f, 1.05f, Rand(cx, cy, i, 3));
                    // a tuft leans with the slope a little
                    var tilt = Quaternion.Euler(Mathf.Clamp(-sy * 12f, -10f, 10f), 0f, Mathf.Clamp(sx * 12f, -10f, 10f));
                    tufts.Add(Matrix4x4.TRS(new Vector3(x, z - 0.03f, y), tilt * Quaternion.Euler(0f, yaw, 0f), new Vector3(s, s * Mathf.Lerp(0.8f, 1.2f, Rand(cx, cy, i, 4)), s)));
                }
            }
            return new CellData { tufts = tufts.ToArray(), pebbles = pebbles.ToArray(), made = now };
        }

        // ------------------------------------------------------------------ the meshes and materials

        static void Ensure(Material bm)
        {
            if (s_tuft == null) s_tuft = TuftMesh();
            if (s_pebble == null) s_pebble = PebbleMesh();
            if (s_grass == null)
            {
                s_grass = new Material(bm) { name = "Ground cover grass", enableInstancing = true };
                s_grass.SetTexture("_BaseMap", GrassTexture(Climate));
                s_grass.SetTexture("_TeamMask", Texture2D.blackTexture);
                s_grass.SetFloat("_UseTeamMask", 0f);
                s_grass.EnableKeyword("_ALPHATEST_ON");
                s_grass.SetFloat("_AlphaClip", 1f);
                s_grass.SetFloat("_Cutoff", 0.45f);
                s_grass.SetFloat("_Cull", 0f);
                s_grass.SetFloat("_Gloss", 0.15f);
                s_grass.SetFloat("_Relief", 0f);
            }
            if (s_stone == null)
            {
                s_stone = new Material(bm) { name = "Ground cover pebbles", enableInstancing = true };
                s_stone.SetTexture("_BaseMap", Texture2D.whiteTexture);
                s_stone.SetTexture("_TeamMask", Texture2D.blackTexture);
                s_stone.SetFloat("_UseTeamMask", 0f);
                s_stone.SetColor("_Tint", Climate == 1 ? new Color(0.58f, 0.6f, 0.64f, 1f) : Climate == 2 ? new Color(0.2f, 0.18f, 0.18f, 1f) : new Color(0.62f, 0.55f, 0.45f, 1f));
                s_stone.SetFloat("_Gloss", Climate == 2 ? 0.9f : 0.35f);
            }
        }

        /// <summary>Three upright cards crossing at 60 degrees, 0.7 m wide and 0.5 m tall, their normals up (lit as the ground).</summary>
        static Mesh TuftMesh()
        {
            var v = new List<Vector3>(); var n = new List<Vector3>(); var uv = new List<Vector2>(); var t = new List<int>();
            for (int k = 0; k < 3; k++)
            {
                float a = k * Mathf.PI / 3f;
                var d = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * 0.35f;
                int b = v.Count;
                v.Add(-d); v.Add(d); v.Add(d + Vector3.up * 0.5f); v.Add(-d + Vector3.up * 0.5f);
                for (int i = 0; i < 4; i++) n.Add(Vector3.up);
                uv.Add(new Vector2(0, 0)); uv.Add(new Vector2(1, 0)); uv.Add(new Vector2(1, 1)); uv.Add(new Vector2(0, 1));
                t.Add(b); t.Add(b + 2); t.Add(b + 1); t.Add(b); t.Add(b + 3); t.Add(b + 2);
            }
            var m = new Mesh { name = "Grass tuft" };
            m.SetVertices(v); m.SetNormals(n); m.SetUVs(0, uv); m.SetTriangles(t, 0);
            m.RecalculateBounds();
            return m;
        }

        static Mesh PebbleMesh()
        {
            var v = new List<Vector3>(); var n = new List<Vector3>(); var uv = new List<Vector2>(); var t = new List<int>();
            const int rings = 4, seg = 7;
            for (int i = 0; i <= rings; i++)
            {
                float th = i * Mathf.PI / rings;
                for (int j = 0; j <= seg; j++)
                {
                    float ph = j * Mathf.PI * 2f / seg;
                    float wob = 1f + 0.18f * Mathf.Sin(ph * 3f + i) * Mathf.Sin(th);
                    var d = new Vector3(Mathf.Sin(th) * Mathf.Cos(ph), Mathf.Cos(th), Mathf.Sin(th) * Mathf.Sin(ph));
                    v.Add(d * wob); n.Add(d); uv.Add(new Vector2(j / (float)seg, i / (float)rings));
                }
            }
            for (int i = 0; i < rings; i++)
                for (int j = 0; j < seg; j++)
                {
                    int a = i * (seg + 1) + j, b = a + seg + 1;
                    t.Add(a); t.Add(a + 1); t.Add(b); t.Add(a + 1); t.Add(b + 1); t.Add(b);
                }
            var m = new Mesh { name = "Pebble" };
            m.SetVertices(v); m.SetNormals(n); m.SetUVs(0, uv); m.SetTriangles(t, 0);
            m.RecalculateBounds();
            return m;
        }

        /// <summary>Blades of grass on a transparent card: green with a few flowers (tropical), frost-tipped (north),
        /// withered rust and ash (the Rift).</summary>
        static Texture2D GrassTexture(int climate)
        {
            const int W = 128, H = 128;
            var px = new Color[W * H];
            var rnd = new System.Random(17 + climate);
            Color baseC = climate == 1 ? new Color(0.3f, 0.34f, 0.2f) : climate == 2 ? new Color(0.16f, 0.1f, 0.07f) : new Color(0.14f, 0.3f, 0.06f);
            Color tipC = climate == 1 ? new Color(0.86f, 0.9f, 0.86f) : climate == 2 ? new Color(0.5f, 0.27f, 0.12f) : new Color(0.5f, 0.72f, 0.2f);
            int blades = climate == 2 ? 16 : 26;
            // the clear background carries the blades' own colour (alpha 0): left black, it bled into the smaller mip
            // levels and the tufts turned to dark scribbles a little way off
            var fill = Color.Lerp(baseC, tipC, 0.45f);
            for (int i = 0; i < px.Length; i++) px[i] = new Color(fill.r, fill.g, fill.b, 0f);
            for (int b = 0; b < blades; b++)
            {
                float x0 = 8 + (float)rnd.NextDouble() * (W - 16), lean = ((float)rnd.NextDouble() - 0.5f) * 40f;
                float h = H * (0.45f + 0.55f * (float)rnd.NextDouble()), w = 2.2f + (float)rnd.NextDouble() * 2.2f;
                float shade = 0.75f + 0.5f * (float)rnd.NextDouble();
                for (int y = 0; y < (int)h; y++)
                {
                    float f = y / h;
                    float cx = x0 + lean * f * f;
                    float hw = w * (1f - f * 0.9f);
                    var c = Color.Lerp(baseC, tipC, Mathf.Pow(f, 1.3f)) * shade;
                    for (int x = Mathf.Max(0, (int)(cx - hw - 1)); x <= Mathf.Min(W - 1, (int)(cx + hw + 1)); x++)
                    {
                        float a = Mathf.Clamp01(hw + 0.5f - Mathf.Abs(x - cx));
                        if (a <= 0f) continue;
                        int i = y * W + x;
                        if (a >= px[i].a) px[i] = new Color(c.r, c.g, c.b, a);
                    }
                }
                if (climate == 0 && rnd.NextDouble() < 0.28)   // a flower at the tip
                {
                    var fc = rnd.NextDouble() < 0.5 ? new Color(0.95f, 0.85f, 0.25f) : rnd.NextDouble() < 0.5 ? new Color(0.92f, 0.3f, 0.35f) : new Color(0.95f, 0.95f, 0.92f);
                    float fx = x0 + lean, fy = h - 2;
                    for (int y = (int)fy - 4; y <= (int)fy + 4; y++)
                        for (int x = (int)fx - 4; x <= (int)fx + 4; x++)
                        {
                            if (x < 0 || y < 0 || x >= W || y >= H) continue;
                            float d = Mathf.Sqrt((x - fx) * (x - fx) + (y - fy) * (y - fy));
                            if (d < 3.6f) px[y * W + x] = new Color(fc.r, fc.g, fc.b, 1f);
                        }
                }
            }
            var tex = new Texture2D(W, H, TextureFormat.RGBA32, true) { name = "Ground cover grass " + climate, wrapMode = TextureWrapMode.Clamp, anisoLevel = 4 };
            tex.SetPixels(px);
            tex.Apply(true, true);
            return tex;
        }
    }
}
