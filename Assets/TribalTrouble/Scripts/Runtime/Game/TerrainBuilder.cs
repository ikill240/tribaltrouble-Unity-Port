using UnityEngine;
using Sim = TribalTrouble.Sim;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Builds the island from the generator output: chunked height mesh (with colliders for picking),
    /// a terrain material fed with the generator's own structure textures + blend alpha maps
    /// (Landscape.getBlendInfos / getDetail), and the sea.
    /// </summary>
    public static class TerrainBuilder
    {
        const int Chunk = 64; // cells per chunk side (65x65 vertices)

        public static GameObject Build(GameWorld world, Material terrainTemplate, Material waterTemplate)
        {
            var root = new GameObject("Island");
            var mat = BuildTerrainMaterial(world, terrainTemplate);
            int n = world.size;
            for (int cy = 0; cy < n - 1; cy += Chunk)
                for (int cx = 0; cx < n - 1; cx += Chunk)
                {
                    var go = new GameObject($"Terrain {cx},{cy}");
                    go.transform.SetParent(root.transform, false);
                    go.layer = 0;
                    var mesh = BuildChunk(world, cx, cy, Mathf.Min(Chunk, n - 1 - cx), Mathf.Min(Chunk, n - 1 - cy));
                    go.AddComponent<MeshFilter>().sharedMesh = mesh;
                    var mr = go.AddComponent<MeshRenderer>();
                    mr.sharedMaterial = mat;
                    go.AddComponent<MeshCollider>().sharedMesh = mesh;
                }
            BuildWater(world, waterTemplate).transform.SetParent(root.transform, false);
            return root;
        }

        static Mesh BuildChunk(GameWorld w, int x0, int y0, int cw, int ch)
        {
            int vw = cw + 1, vh = ch + 1;
            var verts = new Vector3[vw * vh];
            var normals = new Vector3[vw * vh];
            for (int y = 0; y < vh; y++)
                for (int x = 0; x < vw; x++)
                {
                    int gx = x0 + x, gy = y0 + y;
                    float h = w.heights[gy][gx];
                    verts[y * vw + x] = new Vector3(gx * GameWorld.MetersPerCell, h, gy * GameWorld.MetersPerCell);
                    float hl = w.heights[gy][Mathf.Max(gx - 1, 0)], hr = w.heights[gy][Mathf.Min(gx + 1, w.size - 1)];
                    float hd = w.heights[Mathf.Max(gy - 1, 0)][gx], hu = w.heights[Mathf.Min(gy + 1, w.size - 1)][gx];
                    normals[y * vw + x] = new Vector3(hl - hr, 2f * GameWorld.MetersPerCell, hd - hu).normalized;
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
            var mesh = new Mesh { name = "TerrainChunk" };
            mesh.vertices = verts;
            mesh.normals = normals;
            mesh.triangles = tris;
            mesh.RecalculateBounds();
            return mesh;
        }

        static Material BuildTerrainMaterial(GameWorld world, Material template)
        {
            var shader = template != null ? template.shader : Shader.Find("TribalTrouble/Terrain");
            var mat = template != null ? new Material(template) : new Material(shader);
            var blends = world.landscape.getBlendInfos();
            // StructureBlend order: [0]=base, [1..4]=alpha0..3, [5]=lighting, [6]=black/shadow, [7]=sea bottom
            string[] slots = { "_S0", "_S1", "_S2", "_S3", "_S4" };
            for (int i = 0; i < 5; i++)
                if (blends[i] is Sim.StructureBlend sb)
                    mat.SetTexture(slots[i], ToTexture(sb.structure, true));
            if (blends.Length > 7 && blends[7] is Sim.StructureBlend sea)
                mat.SetTexture("_S6", ToTexture(sea.structure, true));

            int n = world.size;
            var alphaA = new Texture2D(n, n, TextureFormat.RGBA32, true, true) { wrapMode = TextureWrapMode.Clamp };
            var alphaB = new Texture2D(n, n, TextureFormat.RGBA32, true, true) { wrapMode = TextureWrapMode.Clamp };
            var a = new Color32[n * n];
            var b = new Color32[n * n];
            Sim.GLByteImage A(int i) => blends[i].getSourceImage();
            var a0 = A(1); var a1 = A(2); var a2 = A(3); var a3 = A(4);
            var hl = A(5); var sh = A(6); var sea6 = A(7);
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    int i = y * n + x;
                    a[i] = new Color32(Px(a0, x, y, n), Px(a1, x, y, n), Px(a2, x, y, n), Px(a3, x, y, n));
                    b[i] = new Color32(Px(hl, x, y, n), Px(sh, x, y, n), Px(sea6, x, y, n), 255);
                }
            alphaA.SetPixels32(a); alphaA.Apply(true);
            alphaB.SetPixels32(b); alphaB.Apply(true);
            mat.SetTexture("_AlphaA", alphaA);
            mat.SetTexture("_AlphaB", alphaB);
            mat.SetTexture("_Detail", ToTexture(world.landscape.getDetail(), false));
            mat.SetFloat("_MapMeters", world.meters);
            // structures repeat colormap_size / STRUCTURE_SIZE times = size * 8 / 256 => every 32 cells
            mat.SetFloat("_TileMeters", 32f * GameWorld.MetersPerCell);
            mat.SetFloat("_DetailMeters", 16f);
            return mat;
        }

        /// <summary>Sample an alpha image that may be a different resolution than the grid.</summary>
        static byte Px(Sim.GLByteImage img, int x, int y, int n)
        {
            if (img == null) return 0;
            int ix = img.getWidth() == n ? x : x * img.getWidth() / n;
            int iy = img.getHeight() == n ? y : y * img.getHeight() / n;
            return (byte)img.getPixel(Mathf.Min(ix, img.getWidth() - 1), Mathf.Min(iy, img.getHeight() - 1));
        }

        static Texture2D ToTexture(Sim.GLIntImage img, bool srgb)
        {
            int w = img.getWidth(), h = img.getHeight();
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, true, !srgb) { wrapMode = TextureWrapMode.Repeat, filterMode = FilterMode.Trilinear, anisoLevel = 4 };
            var px = new Color32[w * h];
            for (int i = 0; i < px.Length; i++)
            {
                int p = img.pixels[i];   // (a<<24)|(b<<16)|(g<<8)|r, as GLIntImage packs it
                px[i] = new Color32((byte)p, (byte)(p >> 8), (byte)(p >> 16), (byte)(p >> 24));
            }
            tex.SetPixels32(px);
            tex.Apply(true);
            return tex;
        }

        static GameObject BuildWater(GameWorld world, Material template)
        {
            var go = new GameObject("Sea");
            var mesh = new Mesh { name = "Sea" };
            float pad = world.meters * 1.5f;
            float y = world.seaLevel;
            mesh.vertices = new[] { new Vector3(-pad, y, -pad), new Vector3(-pad, y, world.meters + pad), new Vector3(world.meters + pad, y, -pad), new Vector3(world.meters + pad, y, world.meters + pad) };
            mesh.triangles = new[] { 0, 1, 2, 2, 1, 3 };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            var mat = template != null ? new Material(template) : new Material(Shader.Find("TribalTrouble/Water"));
            int n = world.size;
            var depth = new Texture2D(n, n, TextureFormat.R8, false, true) { wrapMode = TextureWrapMode.Clamp };
            var d = new Color32[n * n];
            for (int yy = 0; yy < n; yy++)
                for (int x = 0; x < n; x++)
                {
                    float dep = Mathf.Clamp01((world.seaLevel - world.heights[yy][x]) / 6f);
                    d[yy * n + x] = new Color32((byte)(dep * 255), 0, 0, 255);
                }
            depth.SetPixels32(d);
            depth.Apply(false);
            mat.SetTexture("_Depth", depth);
            mat.SetFloat("_MapMeters", world.meters);
            go.AddComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }
    }
}
