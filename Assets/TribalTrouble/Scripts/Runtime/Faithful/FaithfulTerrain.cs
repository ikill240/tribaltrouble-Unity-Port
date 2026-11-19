using System.Collections.Generic;
using UnityEngine;
using com.oddlabs.tt.resource;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The island from the generator's WorldInfo: chunked height mesh (colliders for picking) textured with the
    /// generator's own structure textures and alpha maps (WorldInfo.blend_infos / detail, the inputs the Java
    /// LandscapeBaker composes), plus the sea.
    /// </summary>
    public sealed class FaithfulTerrain
    {
        const int Chunk = 128;   // (was 64: four times the terrain draws, each in every shadow cascade too)
        public const float MetersPerCell = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;

        public readonly int size;           // grid units per side
        public readonly float meters;
        public readonly float seaLevel;
        public readonly float[][] heights;  // [y][x]
        public GameObject root { get; private set; }
        public Material terrainMaterial { get; private set; }
        Color _seabed = new Color(0.45f, 0.42f, 0.35f);
        // kept for live editing (map editor): chunk meshes by chunk origin, the alpha maps and the sea depth map
        readonly System.Collections.Generic.Dictionary<(int, int), (MeshFilter, MeshCollider)> _chunks = new System.Collections.Generic.Dictionary<(int, int), (MeshFilter, MeshCollider)>();
        Texture2D _alphaA, _alphaB, _depth;
        public Texture2D minimapBase { get; private set; }

        public FaithfulTerrain(WorldInfo info)
        {
            heights = info.heightmap();
            size = heights.Length;
            meters = size * MetersPerCell;
            seaLevel = info.sea_level_meters();
        }

        public float HeightAt(float mx, float my)
        {
            float fx = Mathf.Clamp(mx / MetersPerCell, 0, size - 1.001f);
            float fy = Mathf.Clamp(my / MetersPerCell, 0, size - 1.001f);
            int x0 = (int)fx, y0 = (int)fy;
            float tx = fx - x0, ty = fy - y0;
            float h00 = heights[y0][x0], h10 = heights[y0][x0 + 1], h01 = heights[y0 + 1][x0], h11 = heights[y0 + 1][x0 + 1];
            return Mathf.Lerp(Mathf.Lerp(h00, h10, tx), Mathf.Lerp(h01, h11, tx), ty);
        }

        /// <summary>Mesh colliders on the chunks (the map editor picks with them). Big islands skip them: cooking
        /// a thousand chunk colliders takes long, and Raycast() below picks the ground from the height map.</summary>
        public bool withColliders = true;
        // chunk level of detail: far chunks use a quarter-resolution mesh with a skirt hiding the seams
        readonly System.Collections.Generic.Dictionary<(int, int), (Mesh full, Mesh coarse, Vector3 center)> _lods = new System.Collections.Generic.Dictionary<(int, int), (Mesh, Mesh, Vector3)>();
        public float lodDistance = 380f;

        /// <summary>The ground point under a ray (height map march + refine); false if it misses the island.</summary>
        public bool Raycast(Ray ray, out Vector3 point)
        {
            point = default;
            float maxT = 8000f, t = 0f;
            float prevT = 0f;
            bool prevAbove = true;
            // start where the ray enters the map's bounding box
            for (int i = 0; i < 4000 && t < maxT; i++)
            {
                var p = ray.origin + ray.direction * t;
                bool inside = p.x >= 0 && p.z >= 0 && p.x < meters && p.z < meters;
                float h = inside ? HeightAt(p.x, p.z) : seaLevel - 100f;
                bool above = p.y > h;
                if (!above && inside && prevAbove && i > 0)
                {
                    float lo = prevT, hi = t;
                    for (int k = 0; k < 20; k++)
                    {
                        float mid = (lo + hi) * .5f;
                        var q = ray.origin + ray.direction * mid;
                        if (q.y > HeightAt(q.x, q.z)) lo = mid; else hi = mid;
                    }
                    point = ray.origin + ray.direction * hi;
                    return true;
                }
                prevAbove = above; prevT = t;
                // big steps high above the ground, fine steps near it
                float clearance = Mathf.Abs(p.y - h);
                t += Mathf.Clamp(clearance * 0.5f, 0.5f, 25f);
            }
            return false;
        }

        /// <summary>Swaps chunk meshes between full and coarse detail by distance (call every frame).</summary>
        public void UpdateLod(Vector3 camera)
        {
            if (_lods.Count == 0) return;
            float d2 = lodDistance * lodDistance;
            foreach (var kv in _lods)
            {
                if (!_chunks.TryGetValue(kv.Key, out var c) || c.Item1 == null) continue;
                var want = (kv.Value.center - camera).sqrMagnitude > d2 ? kv.Value.coarse : kv.Value.full;
                if (c.Item1.sharedMesh != want) c.Item1.sharedMesh = want;
            }
        }

        Mesh BuildCoarseChunk(int x0, int y0, int cw, int ch, int step)
        {
            var verts = new System.Collections.Generic.List<Vector3>();
            var normals = new System.Collections.Generic.List<Vector3>();
            var tris = new System.Collections.Generic.List<int>();
            int nx = cw / step, ny = ch / step;
            if (nx < 1 || ny < 1) return null;
            int Vert(int gx, int gy, float drop)
            {
                gx = Mathf.Min(gx, size - 1); gy = Mathf.Min(gy, size - 1);
                verts.Add(new Vector3(gx * MetersPerCell, heights[gy][gx] - drop, gy * MetersPerCell));
                float hl = heights[gy][Mathf.Max(gx - step, 0)], hr = heights[gy][Mathf.Min(gx + step, size - 1)];
                float hd = heights[Mathf.Max(gy - step, 0)][gx], hu = heights[Mathf.Min(gy + step, size - 1)][gx];
                normals.Add(new Vector3(hl - hr, 2f * MetersPerCell * step, hd - hu).normalized);
                return verts.Count - 1;
            }
            var grid = new int[(nx + 1) * (ny + 1)];
            for (int y = 0; y <= ny; y++)
                for (int x = 0; x <= nx; x++)
                    grid[y * (nx + 1) + x] = Vert(x0 + Mathf.Min(x * step, cw), y0 + Mathf.Min(y * step, ch), 0f);
            for (int y = 0; y < ny; y++)
                for (int x = 0; x < nx; x++)
                {
                    int i = grid[y * (nx + 1) + x], r = grid[y * (nx + 1) + x + 1], u = grid[(y + 1) * (nx + 1) + x], ur = grid[(y + 1) * (nx + 1) + x + 1];
                    tris.Add(i); tris.Add(u); tris.Add(r); tris.Add(r); tris.Add(u); tris.Add(ur);
                }
            // skirts: a short wall hanging down from every edge, so the gap to a full-detail neighbour never shows
            float drop = 3f * step;
            void Skirt(int a, int b)
            {
                int a2 = verts.Count; verts.Add(verts[a] - Vector3.up * drop); normals.Add(normals[a]);
                int b2 = verts.Count; verts.Add(verts[b] - Vector3.up * drop); normals.Add(normals[b]);
                tris.Add(a); tris.Add(b); tris.Add(a2); tris.Add(b); tris.Add(b2); tris.Add(a2);
                tris.Add(a); tris.Add(a2); tris.Add(b); tris.Add(b); tris.Add(a2); tris.Add(b2);
            }
            for (int x = 0; x < nx; x++) { Skirt(grid[x], grid[x + 1]); Skirt(grid[ny * (nx + 1) + x], grid[ny * (nx + 1) + x + 1]); }
            for (int y = 0; y < ny; y++) { Skirt(grid[y * (nx + 1)], grid[(y + 1) * (nx + 1)]); Skirt(grid[y * (nx + 1) + nx], grid[(y + 1) * (nx + 1) + nx]); }
            var mesh = new Mesh { name = "TerrainChunkCoarse" };
            mesh.SetVertices(verts); mesh.SetNormals(normals); mesh.SetTriangles(tris, 0);
            mesh.RecalculateBounds();
            return mesh;
        }

        public GameObject Build(WorldInfo info, Material terrainTemplate, Material waterTemplate, Transform parent)
        {
            root = new GameObject("Island");
            root.transform.SetParent(parent, false);
            var mat = BuildTerrainMaterial(info, terrainTemplate);
            terrainMaterial = mat;
            mat.SetFloat("_SeaLevel", seaLevel);
            for (int cy = 0; cy < size - 1; cy += Chunk)
                for (int cx = 0; cx < size - 1; cx += Chunk)
                {
                    var go = new GameObject($"Terrain {cx},{cy}");
                    go.transform.SetParent(root.transform, false);
                    var mesh = BuildChunk(cx, cy, Mathf.Min(Chunk, size - 1 - cx), Mathf.Min(Chunk, size - 1 - cy));
                    var mf = go.AddComponent<MeshFilter>();
                    mf.sharedMesh = mesh;
                    go.AddComponent<MeshRenderer>().sharedMaterial = mat;
                    MeshCollider mc = null;
                    if (withColliders) { mc = go.AddComponent<MeshCollider>(); mc.sharedMesh = mesh; }
                    _chunks[(cx, cy)] = (mf, mc);
                    // big islands: a coarse version for far away
                    if (size >= 512)
                    {
                        var coarse = BuildCoarseChunk(cx, cy, Mathf.Min(Chunk, size - 1 - cx), Mathf.Min(Chunk, size - 1 - cy), 4);
                        if (coarse != null) _lods[(cx, cy)] = (mesh, coarse, mesh.bounds.center);
                    }
                }
            BuildSeaFloorSkirt(mat).transform.SetParent(root.transform, false);
            BuildWater(waterTemplate).transform.SetParent(root.transform, false);
            BuildMinimap();
            return root;
        }

        Mesh BuildChunk(int x0, int y0, int cw, int ch)
        {
            int vw = cw + 1, vh = ch + 1;
            var verts = new Vector3[vw * vh];
            var normals = new Vector3[vw * vh];
            for (int y = 0; y < vh; y++)
                for (int x = 0; x < vw; x++)
                {
                    int gx = x0 + x, gy = y0 + y;
                    verts[y * vw + x] = new Vector3(gx * MetersPerCell, heights[gy][gx], gy * MetersPerCell);
                    float hl = heights[gy][Mathf.Max(gx - 1, 0)], hr = heights[gy][Mathf.Min(gx + 1, size - 1)];
                    float hd = heights[Mathf.Max(gy - 1, 0)][gx], hu = heights[Mathf.Min(gy + 1, size - 1)][gx];
                    normals[y * vw + x] = new Vector3(hl - hr, 2f * MetersPerCell, hd - hu).normalized;
                }
            var tris = new int[cw * ch * 6];
            int t = 0;
            for (int y = 0; y < ch; y++)
                for (int x = 0; x < cw; x++)
                {
                    int i = y * vw + x;
                    tris[t++] = i; tris[t++] = i + vw; tris[t++] = i + 1;
                    tris[t++] = i + 1; tris[t++] = i + vw; tris[t++] = i + vw + 1;
                }
            var mesh = new Mesh { name = "TerrainChunk", vertices = verts, normals = normals, triangles = tris };
            mesh.RecalculateBounds();
            return mesh;
        }

        Material BuildTerrainMaterial(WorldInfo info, Material template)
        {
            var mat = template != null ? new Material(template) : new Material(Shader.Find("TribalTrouble/Terrain"));
            var blends = info.blend_infos();
            // Landscape.java order: [0] base, [1..4] structures over alpha0..3, [5] BlendLighting, [6] shadow/black, [7] sea bottom
            string[] slots = { "_S0", "_S1", "_S2", "_S3", "_S4" };
            for (int i = 0; i < 5 && i < blends.Length; i++)
                if (blends[i] is StructureBlend sb) mat.SetTexture(slots[i], ToTexture(sb.getStructureMap(), false, TextureWrapMode.Repeat));
            if (blends.Length > 7 && blends[7] is StructureBlend sea)
            {
                var s6 = ToTexture(sea.getStructureMap(), false, TextureWrapMode.Repeat);
                mat.SetTexture("_S6", s6);
                // average sea floor colour (gamma values -> linear) for the modern sea beyond the terrain
                if (s6 != null)
                {
                    var px = s6.GetPixels32(0);
                    double r = 0, g = 0, bl = 0;
                    for (int k = 0; k < px.Length; k += 7) { r += px[k].r; g += px[k].g; bl += px[k].b; }
                    int cnt = (px.Length + 6) / 7;
                    _seabed = new Color((float)(r / cnt / 255), (float)(g / cnt / 255), (float)(bl / cnt / 255)).linear;
                }
            }
            // the shader blends in gamma space like LandscapeBaker, so the structures are uploaded raw (linear = no conversion)
            mat.SetColor("_HighlightColor", new Color(1f, 0.6f, 0.9f, 1f));   // BlendLighting(1, 0.9, 0.6) passed as (r, b, g)

            int n = size;
            var alphaA = new Texture2D(n, n, TextureFormat.RGBA32, true, true) { wrapMode = TextureWrapMode.Clamp };
            var alphaB = new Texture2D(n, n, TextureFormat.RGBA32, true, true) { wrapMode = TextureWrapMode.Clamp };
            var a = new Color32[n * n];
            var b = new Color32[n * n];
            GLImage A(int i) => i < blends.Length ? blends[i].getSourceImage() : null;
            var a0 = A(1); var a1 = A(2); var a2 = A(3); var a3 = A(4);
            var hl = A(5); var sh = A(6); var sea7 = A(7);
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    int i = y * n + x;
                    a[i] = new Color32(Px(a0, x, y), Px(a1, x, y), Px(a2, x, y), Px(a3, x, y));
                    b[i] = new Color32(Px(hl, x, y), Px(sh, x, y), Px(sea7, x, y), 255);
                }
            _alphaA = alphaA; _alphaB = alphaB;
            alphaA.SetPixels32(a); alphaA.Apply(true);
            alphaB.SetPixels32(b); alphaB.Apply(true);
            mat.SetTexture("_AlphaA", alphaA);
            mat.SetTexture("_AlphaB", alphaB);
            var detail = ToTexture(info.detail(), false, TextureWrapMode.Repeat);
            if (detail != null) mat.SetTexture("_Detail", detail);
            mat.SetFloat("_MapMeters", meters);
            // structures repeat colormap_size / STRUCTURE_SIZE times over the world (IslandGenerator.generate)
            float repeats = (float)info.texels_per_colormap() / com.oddlabs.tt.global.Globals.STRUCTURE_SIZE;
            mat.SetFloat("_TileMeters", repeats > 0 ? meters / repeats : 32f * MetersPerCell);
            mat.SetFloat("_DetailMeters", 16f);
            return mat;
        }

        /// <summary>Samples a 1-byte alpha image that may be a different resolution than the grid.</summary>
        byte Px(GLImage img, int x, int y)
        {
            if (img == null) return 0;
            int w = img.getWidth(), h = img.getHeight();
            int ix = w == size ? x : x * w / size;
            int iy = h == size ? y : y * h / size;
            return (byte)img.getPixel(Mathf.Min(ix, w - 1), Mathf.Min(iy, h - 1));
        }

        /// <summary>Uploads the base level of a ported Texture (GLIntImage RGBA bytes, or a 1-byte GLByteImage).</summary>
        static Texture2D ToTexture(com.oddlabs.tt.render.Texture t, bool srgb, TextureWrapMode wrap)
        {
            if (t == null || t.images == null || t.images.Length == 0 || t.images[0] == null) return null;
            var img = t.images[0];
            int w = img.getWidth(), h = img.getHeight();
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, true, !srgb) { wrapMode = wrap, filterMode = FilterMode.Trilinear, anisoLevel = 8 };
            var px = new Color32[w * h];
            var data = img.pixel_data.array();
            int bpp = img.getPixelSize();
            for (int i = 0; i < px.Length; i++)
            {
                if (bpp >= 4) px[i] = new Color32((byte)data[i * 4], (byte)data[i * 4 + 1], (byte)data[i * 4 + 2], (byte)data[i * 4 + 3]);
                else if (bpp == 3) px[i] = new Color32((byte)data[i * 3], (byte)data[i * 3 + 1], (byte)data[i * 3 + 2], 255);
                else { byte v = (byte)data[i * bpp]; px[i] = new Color32(v, v, v, v); }
            }
            tex.SetPixels32(px);
            tex.Apply(true);
            return tex;
        }

        /// <summary>The sea's material (TTEnvironment sets its textures, waves and reflection).</summary>
        public Material waterMaterial { get; private set; }
        public GameObject water { get; private set; }

        /// <summary>
        /// The sea: a grid over the island (so the open sea can carry the Java Gerstner waves), and a ring out to the
        /// horizon. Vertex colour r = open sea (connected to the map border; inland lakes get no waves, as in Water.java),
        /// g = inside the map.
        /// </summary>
        GameObject BuildWater(Material template)
        {
            var go = new GameObject("Sea");
            go.layer = 4;   // "Water": left out of the reflection
            int cells = Mathf.Clamp(size / 2, 32, 256);
            float step = meters / cells;
            bool[] open = OpenSea();
            var verts = new List<Vector3>();
            var cols = new List<Color32>();
            var tris = new List<int>();
            for (int y = 0; y <= cells; y++)
                for (int x = 0; x <= cells; x++)
                {
                    verts.Add(new Vector3(x * step, seaLevel, y * step));
                    int gx = Mathf.Min(size - 1, Mathf.RoundToInt(x * step / MetersPerCell)), gy = Mathf.Min(size - 1, Mathf.RoundToInt(y * step / MetersPerCell));
                    cols.Add(new Color32(open[gy * size + gx] ? (byte)255 : (byte)0, 255, 0, 255));
                }
            int W = cells + 1;
            for (int y = 0; y < cells; y++)
                for (int x = 0; x < cells; x++)
                {
                    int i = y * W + x;
                    tris.Add(i); tris.Add(i + W); tris.Add(i + 1);
                    tris.Add(i + 1); tris.Add(i + W); tris.Add(i + W + 1);
                }
            // horizon ring: the map border out to 8 km, no waves
            int ring = 64;
            float far = 8000f, c = meters * 0.5f;
            int baseIdx = verts.Count;
            for (int k = 0; k < ring; k++)
            {
                float t = k / (float)ring;
                Vector3 inner = BorderPoint(t);
                Vector3 dir0 = new Vector3(inner.x - c, 0, inner.z - c);
                // overlap the grid by 1.5 m so the T-junctions along the border never open a crack to the background
                inner.x = Mathf.Clamp(inner.x, 1.5f, meters - 1.5f);
                inner.z = Mathf.Clamp(inner.z, 1.5f, meters - 1.5f);
                Vector3 dir = dir0.normalized;
                verts.Add(inner); cols.Add(new Color32(0, 255, 0, 255));
                verts.Add(new Vector3(c, seaLevel, c) + dir * far); cols.Add(new Color32(0, 0, 0, 255));
            }
            for (int k = 0; k < ring; k++)
            {
                int a0 = baseIdx + k * 2, a1 = baseIdx + ((k + 1) % ring) * 2;
                tris.Add(a0); tris.Add(a0 + 1); tris.Add(a1);
                tris.Add(a1); tris.Add(a0 + 1); tris.Add(a1 + 1);
            }
            var mesh = new Mesh { name = "Sea", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
            mesh.SetVertices(verts);
            mesh.SetColors(cols);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.bounds = new Bounds(new Vector3(c, seaLevel, c), new Vector3(far * 2, 10, far * 2));
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            var mat = template != null ? new Material(template) : new Material(Shader.Find("TribalTrouble/Water"));
            var depth = new Texture2D(size, size, TextureFormat.R8, false, true) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };
            _depth = depth;
            RefreshWaterDepth();
            mat.SetTexture("_Depth", depth);
            mat.SetFloat("_MapMeters", meters);
            mat.SetFloat("_TerrainMeters", (size - 1) * MetersPerCell);
            mat.SetColor("_SeabedColor", _seabed);
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false;
            waterMaterial = mat;
            water = go;
            return go;
        }

        /// <summary>
        /// The sea floor continued beyond the map (the Java game draws a flat colour there): square rings sloping from
        /// the map edge down into the deep, so the modern sea sees real depth everywhere and darkens gradually.
        /// </summary>
        GameObject BuildSeaFloorSkirt(Material mat)
        {
            var go = new GameObject("Sea floor skirt");
            float T = (size - 1) * MetersPerCell;
            float edgeH = 0f;
            for (int i = 0; i < size; i++) edgeH += heights[0][i] + heights[size - 1][i] + heights[i][0] + heights[i][size - 1];
            edgeH = edgeH / (4f * size) - 0.05f;
            float[] off = { -1f, 20f, 70f, 180f, 450f, 1100f, 2600f, 6000f };
            float[] dh = { 0f, -1.5f, -5f, -14f, -35f, -80f, -160f, -300f };
            const int seg = 24;   // per side
            var verts = new List<Vector3>();
            var tris = new List<int>();
            int per = seg * 4;
            for (int r = 0; r < off.Length; r++)
            {
                float lo = -off[r], hi = T + off[r];
                for (int k = 0; k < per; k++)
                {
                    float t = (k % seg) / (float)seg;
                    int side = k / seg;
                    Vector3 v = side == 0 ? new Vector3(Mathf.Lerp(lo, hi, t), 0, lo)
                              : side == 1 ? new Vector3(hi, 0, Mathf.Lerp(lo, hi, t))
                              : side == 2 ? new Vector3(Mathf.Lerp(hi, lo, t), 0, hi)
                              : new Vector3(lo, 0, Mathf.Lerp(hi, lo, t));
                    v.y = edgeH + dh[r];
                    verts.Add(v);
                }
            }
            for (int r = 0; r < off.Length - 1; r++)
                for (int k = 0; k < per; k++)
                {
                    int a0 = r * per + k, a1 = r * per + (k + 1) % per, b0 = (r + 1) * per + k, b1 = (r + 1) * per + (k + 1) % per;
                    tris.Add(a0); tris.Add(a1); tris.Add(b0);
                    tris.Add(a1); tris.Add(b1); tris.Add(b0);
                }
            var mesh = new Mesh { name = "SeaFloorSkirt", indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            // winding: make sure the floor faces up
            var nrm = mesh.normals;
            if (nrm.Length > 0 && nrm[per + 1].y < 0)
            {
                for (int i = 0; i < tris.Count; i += 3) { int x = tris[i]; tris[i] = tris[i + 1]; tris[i + 1] = x; }
                mesh.SetTriangles(tris, 0);
                mesh.RecalculateNormals();
            }
            mesh.RecalculateBounds();
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = mat;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            return go;
        }

        Vector3 BorderPoint(float t)
        {
            // walk the square border (perimeter parameter t in 0..1), counter-clockwise from (0,0)
            float p = t * 4f, m = meters;
            Vector3 v = p < 1 ? new Vector3(p * m, 0, 0) : p < 2 ? new Vector3(m, 0, (p - 1) * m) : p < 3 ? new Vector3((3 - p) * m, 0, m) : new Vector3(0, 0, (4 - p) * m);
            v.y = seaLevel;
            return v;
        }

        /// <summary>Cells under the sea that connect to the map border (flood fill, like Water.java's ocean patches).</summary>
        bool[] OpenSea()
        {
            var open = new bool[size * size];
            var q = new Queue<int>();
            void Push(int x, int y) { int i = y * size + x; if (!open[i] && heights[y][x] < seaLevel) { open[i] = true; q.Enqueue(i); } }
            for (int i = 0; i < size; i++) { Push(i, 0); Push(i, size - 1); Push(0, i); Push(size - 1, i); }
            while (q.Count > 0)
            {
                int i = q.Dequeue(), x = i % size, y = i / size;
                if (x > 0) Push(x - 1, y);
                if (x < size - 1) Push(x + 1, y);
                if (y > 0) Push(x, y - 1);
                if (y < size - 1) Push(x, y + 1);
            }
            return open;
        }

        // ------------------------------------------------------------------ live editing (map editor)

        /// <summary>Rebuilds the meshes (and colliders) of the chunks covering a rectangle of cells (inclusive).</summary>
        public void RebuildCells(int x0, int y0, int x1, int y1)
        {
            x0 = Mathf.Max(0, x0 - 1); y0 = Mathf.Max(0, y0 - 1);   // normals look one cell around
            x1 = Mathf.Min(size - 1, x1 + 1); y1 = Mathf.Min(size - 1, y1 + 1);
            // chunk cx holds vertices cx..cx+Chunk, so a vertex on a boundary is in two chunks
            for (int cy = (Mathf.Max(0, y0 - 1) / Chunk) * Chunk; cy <= y1 && cy < size - 1; cy += Chunk)
                for (int cx = (Mathf.Max(0, x0 - 1) / Chunk) * Chunk; cx <= x1 && cx < size - 1; cx += Chunk)
                    RebuildChunk(cx, cy);
        }

        void RebuildChunk(int cx, int cy)
        {
            if (!_chunks.TryGetValue((cx, cy), out var c)) return;
            var old = c.Item1.sharedMesh;
            var mesh = BuildChunk(cx, cy, Mathf.Min(Chunk, size - 1 - cx), Mathf.Min(Chunk, size - 1 - cy));
            c.Item1.sharedMesh = mesh;
            if (c.Item2 != null) c.Item2.sharedMesh = mesh;
            // edited ground: drop the (now stale) coarse version of this chunk
            if (_lods.TryGetValue((cx, cy), out var lod))
            {
                _lods.Remove((cx, cy));
                if (lod.coarse != null) Object.Destroy(lod.coarse);
                if (lod.full != null) Object.Destroy(lod.full);
                old = null;
            }
            if (old != null) Object.Destroy(old);
        }

        /// <summary>New ground alphas for a rectangle (GroundAlphas.compute layout: RGBA rows bottom up).</summary>
        public void SetAlphas(int x0, int y0, int x1, int y1, byte[] low, byte[] high)
        {
            if (_alphaA == null) return;
            int w = x1 - x0 + 1, h = y1 - y0 + 1;
            var a = new Color32[w * h];
            var b = new Color32[w * h];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = new Color32(low[i * 4], low[i * 4 + 1], low[i * 4 + 2], low[i * 4 + 3]);
                b[i] = new Color32(high[i * 4], high[i * 4 + 1], high[i * 4 + 2], 255);
            }
            _alphaA.SetPixels32(x0, y0, w, h, a); _alphaA.Apply(true);
            _alphaB.SetPixels32(x0, y0, w, h, b); _alphaB.Apply(true);
        }

        /// <summary>Redoes the sea depth map (shallow water colour) from the current heights.</summary>
        public void RefreshWaterDepth()
        {
            if (_depth == null) return;
            var d = new Color32[size * size];
            for (int yy = 0; yy < size; yy++)
                for (int x = 0; x < size; x++)
                    d[yy * size + x] = new Color32((byte)(Mathf.Clamp01((seaLevel - heights[yy][x]) / 8f) * 255), 0, 0, 255);   // metres / 8
            _depth.SetPixels32(d);
            _depth.Apply(false);
        }

        void BuildMinimap()
        {
            int n = Mathf.Min(size, 256);
            minimapBase = new Texture2D(n, n, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float h = heights[y * size / n][x * size / n];
                    Color c;
                    if (h < seaLevel - 2f) c = new Color(0.10f, 0.25f, 0.45f);
                    else if (h < seaLevel) c = new Color(0.18f, 0.42f, 0.58f);
                    else if (h < seaLevel + 1.2f) c = new Color(0.78f, 0.72f, 0.50f);
                    else c = Color.Lerp(new Color(0.25f, 0.48f, 0.20f), new Color(0.45f, 0.42f, 0.35f), Mathf.Clamp01((h - seaLevel) / 30f));
                    px[y * n + x] = c;
                }
            minimapBase.SetPixels32(px);
            minimapBase.Apply(false);
        }
    }
}
