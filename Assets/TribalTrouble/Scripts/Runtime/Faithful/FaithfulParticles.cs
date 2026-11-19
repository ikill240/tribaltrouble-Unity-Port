using System.Collections.Generic;
using TribalTrouble.Faithful;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Draws the simulation's particles the way the Java EmitterRenderer / LightningRenderer / SonicBlastRenderer do:
    /// camera-facing textured quads per particle (centre, radius, colour, the particle's own UV corners), batched by
    /// texture and blend mode; lightning as stretched quads; the sonic blast as an expanding ring. Textures come from
    /// the game's own procedural generators (GeneratorSmoke, GeneratorLightning, ...), run once and cached.
    /// </summary>
    public sealed class FaithfulParticles
    {
        sealed class Batch
        {
            public Material material;
            public Mesh mesh;
            public readonly List<Vector3> v = new List<Vector3>(1024);
            public readonly List<Color> c = new List<Color>(1024);
            public readonly List<Vector2> uv = new List<Vector2>(1024);
            public readonly List<int> idx = new List<int>(1536);
            public void Clear() { v.Clear(); c.Clear(); uv.Clear(); idx.Clear(); }
        }

        const int GL_ONE = 1, GL_SRC_ALPHA = 0x302, GL_ONE_MINUS_SRC_ALPHA = 0x303, GL_ZERO = 0;

        MaterialPropertyBlock _blastBlock;
        readonly Dictionary<(Texture, int, int), Batch> _batches = new Dictionary<(Texture, int, int), Batch>();
        readonly List<Batch> _order = new List<Batch>();
        readonly Dictionary<(object, int), Texture2D> _textures = new Dictionary<(object, int), Texture2D>();
        readonly Shader _shader;
        Texture2D _blob;
        Mesh _ring;
        Material _ringMat;
        TTRealLightning _real;   // New Tides: realistic bolts

        public FaithfulParticles()
        {
            _shader = Shader.Find("TribalTrouble/Particle") ?? Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Sprites/Default");
        }

        static UnityEngine.Rendering.BlendMode Blend(int gl) => gl switch
        {
            GL_ONE => UnityEngine.Rendering.BlendMode.One,
            GL_ZERO => UnityEngine.Rendering.BlendMode.Zero,
            GL_ONE_MINUS_SRC_ALPHA => UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha,
            _ => UnityEngine.Rendering.BlendMode.SrcAlpha,
        };

        Batch GetBatch(Texture tex, int src, int dst)
        {
            if (src == GL_ZERO && dst == GL_ZERO) { src = GL_SRC_ALPHA; dst = GL_ONE_MINUS_SRC_ALPHA; }
            var key = (tex, src, dst);
            if (_batches.TryGetValue(key, out var b)) return b;
            var mat = new Material(_shader) { name = "Particles " + (tex != null ? tex.name : "blob") + (dst == GL_ONE ? " add" : "") };
            mat.mainTexture = tex;
            mat.SetFloat("_SrcBlend", (float)Blend(src));
            mat.SetFloat("_DstBlend", (float)Blend(dst));
            b = new Batch { material = mat, mesh = new Mesh { name = "Particles" } };
            b.mesh.MarkDynamic();
            _batches[key] = b;
            _order.Add(b);
            return b;
        }

        /// <summary>The generator's texture for a TextureKey (built once), or a soft round blob.</summary>
        Texture TextureFor(com.oddlabs.tt.render.TextureKey key)
        {
            if (key == null || key.source == null) return Blob();
            var id = ((object)key.source.GetType(), key.index);
            if (_textures.TryGetValue(id, out var t)) return t != null ? (Texture)t : Blob();
            Texture2D tex = null;
            try
            {
                if (key.source.get() is com.oddlabs.tt.render.Texture[] arr && key.index < arr.Length && arr[key.index]?.images?.Length > 0)
                    tex = Upload(arr[key.index].images[0], key.Name);
            }
            catch (System.Exception e) { Debug.LogWarning("[Tribal Trouble] particle texture " + key.Name + ": " + e.Message); }
            _textures[id] = tex;
            return tex != null ? (Texture)tex : Blob();
        }

        static Texture2D Upload(com.oddlabs.tt.resource.GLImage img, string name)
        {
            int w = img.getWidth(), h = img.getHeight();
            var data = img.pixel_data.array();
            int bpp = img.getPixelSize();
            var px = new Color32[w * h];
            for (int i = 0; i < px.Length; i++)
            {
                if (bpp >= 4) px[i] = new Color32((byte)data[i * 4], (byte)data[i * 4 + 1], (byte)data[i * 4 + 2], (byte)data[i * 4 + 3]);
                else { byte b = (byte)data[i * bpp]; px[i] = new Color32(255, 255, 255, b); }
            }
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, true) { name = name, wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };
            tex.SetPixels32(px);
            tex.Apply(true);
            return tex;
        }

        Texture2D Blob()
        {
            if (_blob != null) return _blob;
            const int n = 64;
            _blob = new Texture2D(n, n, TextureFormat.RGBA32, true) { name = "ParticleBlob", wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float dx = (x + .5f) / n * 2 - 1, dy = (y + .5f) / n * 2 - 1;
                    float a = Mathf.Clamp01(1f - Mathf.Sqrt(dx * dx + dy * dy));
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(a * a * 255));
                }
            _blob.SetPixels32(px);
            _blob.Apply(true);
            return _blob;
        }

        public void Render(FaithfulScene scene, Camera cam)
        {
            foreach (var b in _order) b.Clear();
            Vector3 right = cam.transform.right, up = cam.transform.up;
            // the Java geometry shader works in game space (z up) and scales (right + up) by the particle's radius
            // per axis, so do the same there: right / up in game axes
            Vector3 jr = new Vector3(right.x, right.z, right.y), ju = new Vector3(up.x, up.z, up.y);
            Vector3 rpu = jr + ju, rmu = jr - ju;

            foreach (var emitter in scene.emitters)
            {
                var textures = emitter.getTextures();
                if (textures == null) continue;   // sprite particles are drawn by FaithfulView
                var lists = emitter.getParticles();
                float sx = emitter.getScaleX(), sy = emitter.getScaleY(), sz = emitter.getScaleZ();
                for (int j = 0; j < lists.Length && j < textures.Length; j++)
                {
                    var list = (java.util.List)lists[j];
                    int n = list.size();
                    if (n == 0) continue;
                    var b = GetBatch(TextureFor(textures[j]), emitter.getSrcBlendFunc(), emitter.getDstBlendFunc());
                    for (int i = n - 1; i >= 0; i--)
                    {
                        var p = (com.oddlabs.tt.particle.Particle)list.get(i);
                        float px = p.getPosX(), py = p.getPosY(), pz = p.getPosZ();
                        float rx = p.getRadiusX() * sx, ry = p.getRadiusY() * sy, rz = p.getRadiusZ() * sz;
                        float ax = rpu.x * rx, ay = rpu.y * ry, az = rpu.z * rz, bx = rmu.x * rx, by = rmu.y * ry, bz = rmu.z * rz;
                        var col = new Color(p.getColorR(), p.getColorG(), p.getColorB(), Mathf.Min(p.getColorA(), 1f));
                        Quad(b, TTSpace.ToUnity(px - ax, py - ay, pz - az), TTSpace.ToUnity(px + bx, py + by, pz + bz),
                            TTSpace.ToUnity(px + ax, py + ay, pz + az), TTSpace.ToUnity(px - bx, py - by, pz - bz), col,
                            new Vector2(p.getU1(), p.getV1()), new Vector2(p.getU2(), p.getV2()), new Vector2(p.getU3(), p.getV3()), new Vector2(p.getU4(), p.getV4()));
                    }
                }
            }

            bool real = scene.World != null && scene.World.isExtended();
            if (real) { _real ??= new TTRealLightning(_shader); _real.Render(scene.lightnings, cam); }
            else foreach (var lightning in scene.lightnings)
            {
                var b = GetBatch(TextureFor(lightning.getTexture()), GL_SRC_ALPHA, GL_ONE);
                var it = lightning.getParticles().iterator();
                while (it.hasNext())
                {
                    var p = (com.oddlabs.tt.particle.StretchParticle)it.next();
                    // LightningRenderer: two crossed quads, widened along the game's x and y axes
                    float ax = p.getSrcX(), ay = p.getSrcY(), az = p.getSrcZ(), dx = p.getDstX(), dy = p.getDstY(), dz = p.getDstZ();
                    float sw = p.getSrcWidth(), dw = p.getDstWidth();
                    var col = new Color(p.getColorR(), p.getColorG(), p.getColorB(), p.getColorA());
                    Vector2 t0 = new Vector2(0, 0), t1 = new Vector2(1, 0), t2 = new Vector2(1, 1), t3 = new Vector2(0, 1);
                    Quad(b, TTSpace.ToUnity(dx - dw, dy, dz), TTSpace.ToUnity(dx + dw, dy, dz), TTSpace.ToUnity(ax + sw, ay, az), TTSpace.ToUnity(ax - sw, ay, az), col, t0, t1, t2, t3);
                    Quad(b, TTSpace.ToUnity(dx, dy - dw, dz), TTSpace.ToUnity(dx, dy + dw, dz), TTSpace.ToUnity(ax, ay + sw, az), TTSpace.ToUnity(ax, ay - sw, az), col, t0, t1, t2, t3);
                }
            }

            foreach (var b in _order)
            {
                if (b.v.Count == 0) continue;
                b.mesh.Clear();
                b.mesh.SetVertices(b.v);
                b.mesh.SetColors(b.c);
                b.mesh.SetUVs(0, b.uv);
                b.mesh.indexFormat = b.v.Count > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
                b.mesh.SetTriangles(b.idx, 0, false);
                b.mesh.bounds = new Bounds(cam.transform.position, Vector3.one * 100000f);
                Graphics.RenderMesh(new RenderParams(b.material) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off }, b.mesh, 0, Matrix4x4.identity);
            }

            DrawSonicBlasts(scene);
        }

        static void Quad(Batch b, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Color col, Vector2 t0, Vector2 t1, Vector2 t2, Vector2 t3)
        {
            int i = b.v.Count;
            b.v.Add(p0); b.v.Add(p1); b.v.Add(p2); b.v.Add(p3);
            b.c.Add(col); b.c.Add(col); b.c.Add(col); b.c.Add(col);
            b.uv.Add(t0); b.uv.Add(t1); b.uv.Add(t2); b.uv.Add(t3);
            b.idx.Add(i); b.idx.Add(i + 1); b.idx.Add(i + 2);
            b.idx.Add(i); b.idx.Add(i + 2); b.idx.Add(i + 3);
        }

        /// <summary>SonicBlastRenderer: a ground-parallel quad 2.4 x the blast radius across, the SonicBlastShader drawing
        /// the main shock ring and three trailing rings with a little turbulence, additive and over everything.</summary>
        void DrawSonicBlasts(FaithfulScene scene)
        {
            if (scene.sonicBlasts.Count == 0) return;
            if (_ring == null)
            {
                _ring = new Mesh { name = "SonicQuad" };
                _ring.vertices = new[] { new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, 0.5f) };
                _ring.uv = new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
                _ring.triangles = new[] { 0, 2, 1, 1, 2, 3 };
                _ring.RecalculateBounds();
                var sh = Shader.Find("TribalTrouble/SonicBlast");
                _ringMat = new Material(sh != null ? sh : _shader) { name = "Sonic blast" };
            }
            foreach (var s in scene.sonicBlasts)
            {
                if (s.isDead()) continue;
                var p = s.getPosition();
                float visual = s.getMaxRadius() * 1.2f, d = visual * 2f;
                var m = Matrix4x4.TRS(TTSpace.ToUnity(p.x(), p.y(), p.z()), Quaternion.identity, new Vector3(d, 1, d));
                var block = _blastBlock ??= new MaterialPropertyBlock();
                block.Clear();
                block.SetVector("_Blast", new Vector4(s.getTime(), visual, visual / Mathf.Max(s.getDuration(), 0.001f), 0));
                block.SetColor("_Color", new Color(0.7f, 0.85f, 1f, 1f));
                Graphics.RenderMesh(new RenderParams(_ringMat) { matProps = block, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off }, _ring, 0, m);
            }
        }

        public void Clear()
        {
            foreach (var b in _order) { Object.Destroy(b.material); Object.Destroy(b.mesh); }
            _order.Clear();
            _batches.Clear();
            _real?.Clear();
            _real = null;
        }
    }
}
