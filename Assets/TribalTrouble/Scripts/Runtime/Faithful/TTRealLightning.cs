using System.Collections.Generic;
using TribalTrouble.Faithful;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// New Tides: lightning drawn like the real thing instead of the original stretched sprites. The simulation's bolt
    /// path (the Lightning's segments, cloud to ground) is refined with midpoint displacement into a finely jagged
    /// channel, a few thinner forks branch off it, and each is drawn as camera-facing ribbons, a white-hot core inside
    /// a wide blue glow. A bolt flickers through two or three return strokes before fading, lights the ground around
    /// the strike for a moment and leaves a short glow where it hit. Purely visual: the shape comes from a hash of
    /// the bolt's own end points, never from the game's random numbers.
    /// </summary>
    public sealed class TTRealLightning
    {
        sealed class Bolt
        {
            public readonly List<Vector3> main = new List<Vector3>();
            public readonly List<List<Vector3>> forks = new List<List<Vector3>>();
            public Vector3 ground;
            public float born, seed, energy0;
        }

        readonly Dictionary<com.oddlabs.tt.particle.Lightning, Bolt> _bolts = new Dictionary<com.oddlabs.tt.particle.Lightning, Bolt>();
        readonly List<com.oddlabs.tt.particle.Lightning> _gone = new List<com.oddlabs.tt.particle.Lightning>();
        readonly List<Vector3> _v = new List<Vector3>(4096), _gv = new List<Vector3>(4096);
        readonly List<Color> _c = new List<Color>(4096), _gc = new List<Color>(4096);
        readonly List<Vector2> _uv = new List<Vector2>(4096), _guv = new List<Vector2>(4096);
        readonly List<int> _i = new List<int>(6144), _gi = new List<int>(6144);
        readonly Material _core, _glow, _impact;
        readonly Mesh _coreMesh, _glowMesh, _impactMesh;
        readonly Light[] _lights = new Light[4];
        readonly List<(Vector3 pos, float t)> _scorch = new List<(Vector3, float)>();
        GameObject _root;
        /// <summary>How brightly a bolt is flashing right now (0..1), for the storm's sky flash (TTWeatherFx).</summary>
        public static float FlashLevel;

        public TTRealLightning(Shader shader)
        {
            _core = Make(shader, "Lightning core", Profile("BoltCore", 0.10f, 6f));
            _glow = Make(shader, "Lightning glow", Profile("BoltGlow", 0.5f, 2.2f));
            _impact = Make(shader, "Lightning impact", Radial("BoltImpact"));
            _coreMesh = new Mesh { name = "Bolt core" }; _coreMesh.MarkDynamic();
            _glowMesh = new Mesh { name = "Bolt glow" }; _glowMesh.MarkDynamic();
            _impactMesh = new Mesh { name = "Bolt impact" }; _impactMesh.MarkDynamic();
        }

        static Material Make(Shader shader, string name, Texture2D tex)
        {
            var m = new Material(shader) { name = name, mainTexture = tex };
            m.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.One);
            m.renderQueue = 3100;
            return m;
        }

        /// <summary>A strip texture: alpha falls off across u from the centre line (core: tight, glow: wide).</summary>
        static Texture2D Profile(string name, float halfWidth, float sharpness)
        {
            const int w = 64, h = 4;
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, false) { name = name, wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };
            var px = new Color32[w * h];
            for (int x = 0; x < w; x++)
            {
                float d = Mathf.Abs((x + .5f) / w * 2f - 1f);
                float a = Mathf.Exp(-Mathf.Pow(d / Mathf.Max(halfWidth, 0.01f), 2f) * sharpness * 0.25f) * (1f - d);
                for (int y = 0; y < h; y++) px[y * w + x] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(a) * 255));
            }
            tex.SetPixels32(px); tex.Apply(false);
            return tex;
        }

        static Texture2D Radial(string name)
        {
            const int n = 64;
            var tex = new Texture2D(n, n, TextureFormat.RGBA32, false) { name = name, wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float dx = (x + .5f) / n * 2 - 1, dy = (y + .5f) / n * 2 - 1;
                    float r = Mathf.Sqrt(dx * dx + dy * dy);
                    float a = Mathf.Clamp01(1f - r); a = a * a * (0.6f + 0.4f * a);
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            tex.SetPixels32(px); tex.Apply(false);
            return tex;
        }

        // ---- shape ----------------------------------------------------------------------------------------------

        static uint Hash(float a, float b, float c)
        {
            unchecked
            {
                uint h = 2166136261u;
                foreach (var f in new[] { a, b, c })
                {
                    uint u = (uint)System.BitConverter.SingleToInt32Bits(f);
                    h = (h ^ u) * 16777619u;
                    h ^= h >> 13;
                }
                return h == 0 ? 1u : h;
            }
        }

        sealed class Rng
        {
            uint s;
            public Rng(uint seed) { s = seed; }
            public float Next() { s ^= s << 13; s ^= s >> 17; s ^= s << 5; return (s & 0xFFFFFF) / 16777216f; }
            public float Range(float a, float b) => a + (b - a) * Next();
        }

        /// <summary>Midpoint displacement: each pass splits every segment and kicks the midpoint sideways.</summary>
        static List<Vector3> Refine(List<Vector3> pts, Rng rng, int passes, float rough)
        {
            for (int p = 0; p < passes; p++)
            {
                var o = new List<Vector3>(pts.Count * 2);
                for (int i = 0; i < pts.Count - 1; i++)
                {
                    Vector3 a = pts[i], b = pts[i + 1], d = b - a;
                    float len = d.magnitude;
                    Vector3 side = Vector3.Cross(d, Vector3.up);
                    if (side.sqrMagnitude < 1e-6f) side = Vector3.right;
                    side.Normalize();
                    Vector3 side2 = Vector3.Cross(d.normalized, side);
                    Vector3 mid = (a + b) * .5f + (side * rng.Range(-1f, 1f) + side2 * rng.Range(-.5f, .5f)) * len * rough;
                    o.Add(a); o.Add(mid);
                }
                o.Add(pts[pts.Count - 1]);
                pts = o;
                rough *= 0.62f;
            }
            return pts;
        }

        Bolt Build(com.oddlabs.tt.particle.Lightning l)
        {
            var b = new Bolt { born = Time.time, energy0 = Mathf.Max(l.energy, 0.05f) };
            var raw = new List<Vector3>();
            var it = l.getParticles().iterator();
            com.oddlabs.tt.particle.StretchParticle last = null;
            while (it.hasNext())
            {
                var p = (com.oddlabs.tt.particle.StretchParticle)it.next();
                raw.Add(TTSpace.ToUnity(p.getSrcX(), p.getSrcY(), p.getSrcZ()));
                last = p;
            }
            if (last != null) raw.Add(TTSpace.ToUnity(last.getDstX(), last.getDstY(), last.getDstZ()));
            if (raw.Count < 2) return b;
            var src = l.src; var dst = l.dst;
            uint h = Hash(src.x() + dst.x() * 3.1f, src.y() + dst.y() * 1.7f, dst.z() + raw.Count);
            var rng = new Rng(h);
            b.seed = (h & 0xFFFF) / 65536f;
            b.ground = raw[raw.Count - 1];
            // the sim's zigzag is coarse and very wide: pull it towards the straight line a little, then refine it
            Vector3 top = raw[0], bottom = raw[raw.Count - 1];
            for (int i = 1; i < raw.Count - 1; i++)
            {
                float t = i / (float)(raw.Count - 1);
                raw[i] = Vector3.Lerp(raw[i], Vector3.Lerp(top, bottom, t), 0.45f);
            }
            b.main.AddRange(Refine(raw, rng, 3, 0.28f));
            // forks: off the upper two thirds, heading down and out, ending in the air
            float total = (bottom - top).magnitude;
            int forks = 3 + (int)(rng.Next() * 4f);   // more branching (was 2-4)
            for (int f = 0; f < forks; f++)
            {
                int at = (int)(b.main.Count * rng.Range(0.12f, 0.7f));
                at = Mathf.Clamp(at, 1, b.main.Count - 2);
                Vector3 start = b.main[at], along = (b.main[at + 1] - b.main[at - 1]).normalized;
                float ang = rng.Range(0f, Mathf.PI * 2f);
                Vector3 outward = new Vector3(Mathf.Cos(ang), 0f, Mathf.Sin(ang));
                Vector3 dir = (along + outward * rng.Range(0.6f, 1.3f)).normalized;
                float len = total * rng.Range(0.14f, 0.36f);
                var pts = new List<Vector3> { start };
                Vector3 cur = start;
                int n = 5;
                for (int k = 0; k < n; k++)
                {
                    dir = (dir + new Vector3(rng.Range(-.5f, .5f), rng.Range(-.45f, .1f), rng.Range(-.5f, .5f))).normalized;
                    cur += dir * len / n;
                    pts.Add(cur);
                }
                var fork = Refine(pts, rng, 2, 0.22f);
                b.forks.Add(fork);
                if (rng.Next() < 0.65f && fork.Count > 6)
                {
                    // a twig off the fork
                    int at2 = fork.Count / 3;
                    var twig = new List<Vector3> { fork[at2] };
                    Vector3 c2 = fork[at2], d2 = (dir + outward * .8f + Vector3.down * .3f).normalized;
                    for (int k = 0; k < 3; k++) { d2 = (d2 + new Vector3(rng.Range(-.6f, .6f), rng.Range(-.4f, .1f), rng.Range(-.6f, .6f))).normalized; c2 += d2 * len * .12f; twig.Add(c2); }
                    b.forks.Add(Refine(twig, rng, 1, 0.2f));
                }
            }
            return b;
        }

        // ---- brightness -----------------------------------------------------------------------------------------

        /// <summary>A stroke: full on for a few hundredths of a second, then a flickering fade. Repeat strikes on
        /// the same spot (the spell strikes six times, a storm bolt three) reuse the channel, so together they read
        /// as one bolt's return strokes.</summary>
        static float Stroke(float age, float seed)
        {
            if (age < 0.045f) return 1f;
            float t = age - 0.045f;
            float fade = Mathf.Exp(-t * 9f);
            float flick = 0.7f + 0.3f * Mathf.Sin(t * 95f + seed * 6.3f);
            return fade * flick;
        }

        const float Life = 0.5f;

        // ---- drawing --------------------------------------------------------------------------------------------

        public void Render(List<com.oddlabs.tt.particle.Lightning> lightnings, Camera cam)
        {
            _v.Clear(); _c.Clear(); _uv.Clear(); _i.Clear();
            _gv.Clear(); _gc.Clear(); _guv.Clear(); _gi.Clear();
            Vector3 camPos = cam.transform.position;
            int light = 0;
            float now = Time.time;
            foreach (var l in lightnings)
            {
                if (_bolts.ContainsKey(l)) continue;
                var nb = Build(l);
                if (nb.main.Count < 2) { _bolts[l] = nb; continue; }
                // a strike next to a fresh bolt follows the same channel (a return stroke) instead of a new path
                Bolt twin = null;
                foreach (var o in _bolts.Values)
                    if (o.main.Count > 1 && now - o.born < 0.6f && (o.ground - nb.ground).sqrMagnitude < 2.5f * 2.5f) { twin = o; break; }
                if (twin != null)
                {
                    Vector3 d = nb.ground - twin.ground;
                    nb.main.Clear(); for (int i = 0; i < twin.main.Count; i++) nb.main.Add(twin.main[i] + d * (i / (float)(twin.main.Count - 1)));
                    nb.forks.Clear();
                    if (now - twin.born > 0.05f) foreach (var f in twin.forks) nb.forks.Add(f);
                    nb.seed = twin.seed + 0.37f;
                }
                else _scorch.Add((nb.ground, now));
                _bolts[l] = nb;
            }
            _gone.Clear();
            float flash = 0f;
            foreach (var kv in _bolts)
            {
                var b = kv.Value;
                float age = now - b.born;
                if (age > Life || b.main.Count < 2) { if (age > Life) _gone.Add(kv.Key); continue; }
                float bright = Stroke(age, b.seed);
                if (bright <= 0.01f) continue;
                flash = Mathf.Max(flash, bright);
                Ribbon(b.main, camPos, 0.05f, 0.55f, bright, 1f);   // thinner (was 0.09 / 1.0)
                foreach (var f in b.forks) Ribbon(f, camPos, 0.026f, 0.3f, bright * 0.7f, 0.85f);
                if (light < _lights.Length) Flash(light++, b.ground + Vector3.up * 3f, bright);
            }
            for (int k = light; k < _lights.Length; k++) if (_lights[k] != null) _lights[k].enabled = false;
            foreach (var k in _gone) _bolts.Remove(k);
            FlashLevel = flash;

            Draw(_glowMesh, _glow, _gv, _gc, _guv, _gi, camPos);
            Draw(_coreMesh, _core, _v, _c, _uv, _i, camPos);
            DrawImpacts(camPos, now);
        }

        void Ribbon(List<Vector3> pts, Vector3 camPos, float coreWidth, float glowWidth, float bright, float taper)
        {
            int n = pts.Count;
            for (int i = 0; i < n - 1; i++)
            {
                Vector3 a = pts[i], b = pts[i + 1];
                float t0 = i / (float)(n - 1), t1 = (i + 1) / (float)(n - 1);
                float w0 = Mathf.Lerp(1f, taper * 0.55f, t0), w1 = Mathf.Lerp(1f, taper * 0.55f, t1);
                Vector3 view = ((a + b) * .5f - camPos);
                Vector3 side = Vector3.Cross(b - a, view);
                if (side.sqrMagnitude < 1e-8f) continue;
                side.Normalize();
                var core = new Color(1f, 0.98f, 1f, Mathf.Clamp01(bright * 1.1f));
                var glow = new Color(0.62f, 0.72f, 1f, Mathf.Clamp01(bright * 0.55f));
                Seg(_v, _c, _uv, _i, a, b, side, coreWidth * w0 * (0.8f + bright * 0.6f), coreWidth * w1 * (0.8f + bright * 0.6f), core);
                Seg(_gv, _gc, _guv, _gi, a, b, side, glowWidth * w0, glowWidth * w1, glow);
            }
        }

        static void Seg(List<Vector3> v, List<Color> c, List<Vector2> uv, List<int> idx, Vector3 a, Vector3 b, Vector3 side, float wa, float wb, Color col)
        {
            int i = v.Count;
            // overlap each segment a touch into the next so the joints don't show gaps
            Vector3 ext = (b - a) * 0.08f;
            v.Add(a - side * wa - ext); v.Add(a + side * wa - ext); v.Add(b + side * wb + ext); v.Add(b - side * wb + ext);
            c.Add(col); c.Add(col); c.Add(col); c.Add(col);
            uv.Add(new Vector2(0, 0)); uv.Add(new Vector2(1, 0)); uv.Add(new Vector2(1, 1)); uv.Add(new Vector2(0, 1));
            idx.Add(i); idx.Add(i + 1); idx.Add(i + 2); idx.Add(i); idx.Add(i + 2); idx.Add(i + 3);
        }

        void Draw(Mesh mesh, Material mat, List<Vector3> v, List<Color> c, List<Vector2> uv, List<int> idx, Vector3 camPos)
        {
            if (v.Count == 0) return;
            mesh.Clear();
            mesh.indexFormat = v.Count > 65000 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
            mesh.SetVertices(v); mesh.SetColors(c); mesh.SetUVs(0, uv); mesh.SetTriangles(idx, 0, false);
            mesh.bounds = new Bounds(camPos, Vector3.one * 100000f);
            Graphics.RenderMesh(new RenderParams(mat) { shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off }, mesh, 0, Matrix4x4.identity);
        }

        readonly List<Vector3> _sv = new List<Vector3>(); readonly List<Color> _sc = new List<Color>();
        readonly List<Vector2> _suv = new List<Vector2>(); readonly List<int> _sidx = new List<int>();

        /// <summary>A hot glow on the ground where a bolt struck, fading over a second and a half.</summary>
        void DrawImpacts(Vector3 camPos, float now)
        {
            for (int k = _scorch.Count - 1; k >= 0; k--) if (now - _scorch[k].t > 1.6f) _scorch.RemoveAt(k);
            if (_scorch.Count == 0) return;
            var v = _sv; var c = _sc; var uv = _suv; var idx = _sidx;
            v.Clear(); c.Clear(); uv.Clear(); idx.Clear();
            foreach (var (pos, t) in _scorch)
            {
                float age = now - t, k = Mathf.Clamp01(1f - age / 1.6f);
                float r = 1.2f + 2.6f * Mathf.Clamp01(age * 6f);
                var col = new Color(0.75f + 0.25f * k, 0.8f, 1f, k * k * 0.9f);
                Vector3 p = pos + Vector3.up * 0.15f;
                int i = v.Count;
                v.Add(p + new Vector3(-r, 0, -r)); v.Add(p + new Vector3(r, 0, -r)); v.Add(p + new Vector3(r, 0, r)); v.Add(p + new Vector3(-r, 0, r));
                for (int q = 0; q < 4; q++) c.Add(col);
                uv.Add(new Vector2(0, 0)); uv.Add(new Vector2(1, 0)); uv.Add(new Vector2(1, 1)); uv.Add(new Vector2(0, 1));
                idx.Add(i); idx.Add(i + 2); idx.Add(i + 1); idx.Add(i); idx.Add(i + 3); idx.Add(i + 2);
                // and a standing glow, camera facing
                Vector3 toCam = (camPos - p); toCam.y = 0; if (toCam.sqrMagnitude < 1e-4f) toCam = Vector3.forward; toCam.Normalize();
                Vector3 side = Vector3.Cross(Vector3.up, toCam) * r * 0.8f, upv = Vector3.up * r * 1.1f * k;
                int j = v.Count;
                v.Add(p - side); v.Add(p + side); v.Add(p + side + upv); v.Add(p - side + upv);
                var col2 = new Color(0.8f, 0.85f, 1f, k * 0.5f);
                for (int q = 0; q < 4; q++) c.Add(col2);
                uv.Add(new Vector2(0, 0.5f)); uv.Add(new Vector2(1, 0.5f)); uv.Add(new Vector2(1, 1)); uv.Add(new Vector2(0, 1));
                idx.Add(j); idx.Add(j + 1); idx.Add(j + 2); idx.Add(j); idx.Add(j + 2); idx.Add(j + 3);
            }
            Draw(_impactMesh, _impact, v, c, uv, idx, camPos);
        }

        void Flash(int k, Vector3 at, float bright)
        {
            if (_root == null) _root = new GameObject("TT lightning flashes");
            if (_lights[k] == null)
            {
                var go = new GameObject("Flash " + k);
                go.transform.SetParent(_root.transform, false);
                var li = go.AddComponent<Light>();
                li.type = LightType.Point;
                li.color = new Color(0.78f, 0.84f, 1f);
                li.shadows = LightShadows.None;
                _lights[k] = li;
            }
            var l = _lights[k];
            l.enabled = true;
            l.transform.position = at;
            l.range = 18f + 26f * bright;
            l.intensity = 9f * bright * bright;
        }

        public void Clear()
        {
            FlashLevel = 0f;
            _bolts.Clear();
            _scorch.Clear();
            if (_root != null) Object.Destroy(_root);
            _root = null;
            for (int k = 0; k < _lights.Length; k++) _lights[k] = null;
        }
    }
}
