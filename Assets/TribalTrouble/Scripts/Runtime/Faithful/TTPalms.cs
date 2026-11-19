using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Realistic palms for the Modern look (none of your asset packs has a palm): built procedurally from a curved,
    /// ringed trunk and a crown of arching fronds, textured with the HD frond and trunk images in
    /// Resources/TTExtended (cut from the Mobile_FV photo atlas). Six shapes, each tree picks one from its position.
    /// </summary>
    public static class TTPalms
    {
        const int VariantCount = 6;
        static TTRealTrees.Variant[] s_variants;
        static float s_height;
        static Material s_trunk, s_frond;

        public static bool Available => Resources.Load<Texture2D>("TTExtended/palm_frond") != null;

        public static TTRealTrees.Variant Pick(float x, float y, float targetHeight, Material baseMaterial)
        {
            if (s_variants == null || Mathf.Abs(s_height - targetHeight) > 0.01f)
            {
                s_height = targetHeight;
                s_variants = new TTRealTrees.Variant[VariantCount];
                for (int i = 0; i < VariantCount; i++) s_variants[i] = Build(i, targetHeight, baseMaterial);
            }
            int h = (int)(x * 51.7f) * 13 + (int)(y * 23.3f) * 29;
            return s_variants[(h & 0x7fffffff) % VariantCount];
        }

        static Material Mat(Material baseMaterial, string tex, bool leaves)
        {
            var m = baseMaterial != null ? new Material(baseMaterial) : new Material(Shader.Find("TribalTrouble/Sprite"));
            m.name = leaves ? "Palm fronds" : "Palm trunk";
            m.enableInstancing = true;
            var t = Resources.Load<Texture2D>("TTExtended/" + tex);
            if (t != null && t.anisoLevel < 8) t.anisoLevel = 8;
            m.SetTexture("_BaseMap", t != null ? t : Texture2D.whiteTexture);
            m.SetColor("_Tint", Color.white);
            m.SetFloat("_UseTeamMask", 0f);
            m.SetFloat("_Wind", 0.8f);   // fronds and trunk bend as one (the fronds swaying 5x the trunk slid off its top)
            m.SetFloat("_Gloss", leaves ? 0.35f : 0.15f);
            m.SetFloat("_Relief", leaves ? 0f : 0.9f);
            if (leaves)
            {
                m.EnableKeyword("_ALPHATEST_ON");
                m.SetFloat("_AlphaClip", 1f);
                m.SetFloat("_Cutoff", 0.4f);
                m.SetFloat("_Cull", 0f);
            }
            return m;
        }

        sealed class MB
        {
            public readonly List<Vector3> v = new List<Vector3>();
            public readonly List<Vector3> n = new List<Vector3>();
            public readonly List<Vector2> uv = new List<Vector2>();
            public readonly List<int> t = new List<int>();
            public Mesh Build(string name)
            {
                var m = new Mesh { name = name };
                m.SetVertices(v); m.SetNormals(n); m.SetUVs(0, uv); m.SetTriangles(t, 0);
                m.RecalculateBounds();
                return m;
            }
        }

        static TTRealTrees.Variant Build(int seed, float height, Material baseMaterial)
        {
            s_trunk ??= Mat(baseMaterial, "palm_trunk", false);
            s_frond ??= Mat(baseMaterial, "palm_frond", true);
            var rnd = new System.Random(1000 + seed * 7919);
            float R(float a, float b) => a + (float)rnd.NextDouble() * (b - a);

            float trunkH = height * R(0.78f, 0.86f);
            float lean = R(0.06f, 0.2f) * trunkH;                 // how far the top leans out
            float leanAz = R(0, Mathf.PI * 2);
            var leanDir = new Vector3(Mathf.Cos(leanAz), 0, Mathf.Sin(leanAz));
            float r0 = height * 0.025f + 0.12f;
            Vector3 Center(float t) => leanDir * (lean * t * t) + Vector3.up * (trunkH * t);

            // ---- trunk: a bent, tapering tube with a flared base
            var tr = new MB();
            const int sides = 10, segs = 18;
            for (int s = 0; s <= segs; s++)
            {
                float t = s / (float)segs;
                var c = Center(t);
                var tangent = (Center(Mathf.Min(1, t + 0.02f)) - Center(Mathf.Max(0, t - 0.02f))).normalized;
                var side = Vector3.Cross(tangent, Vector3.forward).sqrMagnitude > 1e-4f ? Vector3.Cross(tangent, Vector3.forward).normalized : Vector3.right;
                var side2 = Vector3.Cross(tangent, side).normalized;
                float r = r0 * (1f - 0.38f * t) * (1f + 0.55f * Mathf.Pow(1f - t, 8f));
                for (int i = 0; i <= sides; i++)
                {
                    float a = i * Mathf.PI * 2f / sides;
                    var d = side * Mathf.Cos(a) + side2 * Mathf.Sin(a);
                    tr.v.Add(c + d * r);
                    tr.n.Add(d);
                    tr.uv.Add(new Vector2(i / (float)sides * 2f, c.y / 2.2f));
                }
            }
            for (int s = 0; s < segs; s++)
                for (int i = 0; i < sides; i++)
                {
                    int a = s * (sides + 1) + i, b = a + 1, c2 = a + sides + 1, d2 = c2 + 1;
                    tr.t.Add(a); tr.t.Add(c2); tr.t.Add(b);
                    tr.t.Add(b); tr.t.Add(c2); tr.t.Add(d2);
                }

            // ---- crown: arching fronds, each a slightly V-folded strip following a drooping curve
            var fr = new MB();
            var top = Center(1f);
            int fronds = rnd.Next(11, 16);
            float frondLen = height * R(0.42f, 0.52f);
            for (int f = 0; f < fronds; f++)
            {
                float az = f * Mathf.PI * 2f / fronds + R(-0.25f, 0.25f);
                var dir = new Vector3(Mathf.Cos(az), 0, Mathf.Sin(az));
                var across = new Vector3(-dir.z, 0, dir.x);
                float rise = R(0.35f, 1.05f);                     // initial elevation (radians)
                float droop = R(1.5f, 2.3f);                      // how much it bends down by the tip
                float len = frondLen * R(0.8f, 1.1f);
                float width = len * 0.36f;
                const int n = 10;
                var pts = new Vector3[n + 1];
                pts[0] = top + Vector3.up * R(-0.1f, 0.15f);
                for (int s = 1; s <= n; s++)
                {
                    float u = s / (float)n;
                    float ang = rise - droop * u * u;
                    pts[s] = pts[s - 1] + (dir * Mathf.Cos(ang) + Vector3.up * Mathf.Sin(ang)) * (len / n);
                }
                float fold = R(0.25f, 0.45f);                     // V fold of the leaflets
                for (int half = 0; half < 2; half++)
                {
                    float sgn = half == 0 ? -1f : 1f;
                    int baseIndex = fr.v.Count;
                    for (int s = 0; s <= n; s++)
                    {
                        float u = s / (float)n;
                        var tangent = (pts[Mathf.Min(n, s + 1)] - pts[Mathf.Max(0, s - 1)]).normalized;
                        var normal = Vector3.Cross(across, tangent).normalized;
                        var leafSide = (across * sgn * Mathf.Cos(fold) + normal * Mathf.Sin(fold)).normalized;
                        float w = width * 0.5f * (0.35f + 0.65f * Mathf.Sin(Mathf.PI * Mathf.Min(1f, u * 1.1f + 0.05f)));
                        fr.v.Add(pts[s]); fr.n.Add(normal); fr.uv.Add(new Vector2(u, 0.56f));
                        fr.v.Add(pts[s] + leafSide * w); fr.n.Add(normal); fr.uv.Add(new Vector2(u, half == 0 ? 0f : 1f));
                    }
                    for (int s = 0; s < n; s++)
                    {
                        int a = baseIndex + s * 2, b = a + 1, c2 = a + 2, d2 = a + 3;
                        if (half == 0) { fr.t.Add(a); fr.t.Add(b); fr.t.Add(c2); fr.t.Add(c2); fr.t.Add(b); fr.t.Add(d2); }
                        else { fr.t.Add(a); fr.t.Add(c2); fr.t.Add(b); fr.t.Add(c2); fr.t.Add(d2); fr.t.Add(b); }
                    }
                }
            }

            var v = new TTRealTrees.Variant { height = height };
            v.parts.Add(new TTRealTrees.Part { mesh = tr.Build("Palm trunk " + seed), submesh = 0, material = s_trunk, local = Matrix4x4.identity });
            v.parts.Add(new TTRealTrees.Part { mesh = fr.Build("Palm fronds " + seed), submesh = 0, material = s_frond, local = Matrix4x4.identity });
            return v;
        }
    }
}
