using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// CPU copy of the sky model in Shaders/TTAtmosphere.hlsl (single Rayleigh + Mie scattering). Used once per
    /// island to derive the lighting the shaders share: sun colour after the atmosphere, zenith / horizon radiance,
    /// ambient sky light and the fog colour, so the sky, the haze and the lit scene match.
    /// </summary>
    public static class TTAtmosphere
    {
        const float Re = 6360e3f, Ra = 6420e3f, Hr = 8000f, Hm = 1200f, TMax = 60e3f, Bm = 8e-6f;
        static readonly Vector3 Br = new Vector3(5.8e-6f, 13.5e-6f, 33.1e-6f);
        public const float SkyExposure = 1.25f;

        static float RaySphere(Vector3 o, Vector3 d, float r)
        {
            float b = Vector3.Dot(o, d), c = Vector3.Dot(o, o) - r * r, disc = b * b - c;
            return disc < 0 ? -1f : -b + Mathf.Sqrt(disc);
        }

        static Vector3 Exp(Vector3 v) => new Vector3(Mathf.Exp(v.x), Mathf.Exp(v.y), Mathf.Exp(v.z));
        static Vector3 Mul(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);

        /// <summary>Sky radiance (unexposed) in direction d with the sun towards s (both normalized, y up).</summary>
        public static Vector3 Radiance(Vector3 d, Vector3 s, int n = 16, int m = 6)
        {
            var o = new Vector3(0, Re + 50f, 0);
            float t = Mathf.Min(RaySphere(o, d, Ra), TMax);
            float b = Vector3.Dot(o, d), c = Vector3.Dot(o, o) - Re * Re, disc = b * b - c;
            if (disc > 0 && -b - Mathf.Sqrt(disc) > 0) t = -b - Mathf.Sqrt(disc);
            float ds = t / n, odR = 0, odM = 0, mu = Vector3.Dot(d, s), g = 0.76f;
            float pR = 3f / (16f * Mathf.PI) * (1 + mu * mu);
            float pM = 3f / (8f * Mathf.PI) * ((1 - g * g) * (1 + mu * mu)) / ((2 + g * g) * Mathf.Pow(Mathf.Max(1 + g * g - 2 * g * mu, 1e-4f), 1.5f));
            Vector3 sr = Vector3.zero, sm = Vector3.zero;
            for (int i = 0; i < n; i++)
            {
                var p = o + d * (ds * (i + 0.5f));
                float h = p.magnitude - Re;
                float hr = Mathf.Exp(-h / Hr) * ds, hm = Mathf.Exp(-h / Hm) * ds;
                odR += hr; odM += hm;
                float tl = RaySphere(p, s, Ra), dl = tl / m, lR = 0, lM = 0;
                bool lit = true;
                for (int j = 0; j < m; j++)
                {
                    var q = p + s * (dl * (j + 0.5f));
                    float hh = q.magnitude - Re;
                    if (hh < 0) { lit = false; break; }
                    lR += Mathf.Exp(-hh / Hr) * dl; lM += Mathf.Exp(-hh / Hm) * dl;
                }
                if (!lit) continue;
                var att = Exp(-(Br * (odR + lR) + Vector3.one * (Bm * 1.1f * (odM + lM))));
                sr += att * hr; sm += att * hm;
            }
            return 20f * (Mul(sr, Br) * pR + sm * (Bm * pM));
        }

        /// <summary>Transmittance of sunlight from the top of the atmosphere to sea level.</summary>
        public static Vector3 SunTransmittance(Vector3 s)
        {
            var o = new Vector3(0, Re + 50f, 0);
            float t = RaySphere(o, s, Ra);
            const int n = 32;
            float ds = t / n, odR = 0, odM = 0;
            for (int i = 0; i < n; i++)
            {
                float h = (o + s * (ds * (i + 0.5f))).magnitude - Re;
                odR += Mathf.Exp(-h / Hr) * ds; odM += Mathf.Exp(-h / Hm) * ds;
            }
            return Exp(-(Br * odR + Vector3.one * (Bm * 1.1f * odM)));
        }

        public struct Lighting
        {
            public Color sun, zenith, horizon, ambientSky, ambientGround, fog;
        }

        /// <summary>The shared lighting for a sun direction (exposed, linear).</summary>
        public static Lighting Compute(Vector3 toSun)
        {
            toSun.Normalize();
            var L = new Lighting();
            var tr = SunTransmittance(toSun);
            const float sunScale = 1.3f;
            L.sun = new Color(tr.x, tr.y, tr.z) * (sunScale / Mathf.Max(0.2f, tr.y));   // keep brightness, keep the tint
            var z = Radiance(Vector3.up, toSun) * SkyExposure;
            L.zenith = new Color(z.x, z.y, z.z);
            Vector3 hsum = Vector3.zero, asum = Vector3.zero;
            float wsum = 0;
            for (int a = 0; a < 8; a++)
            {
                float az = a * Mathf.PI / 4f;
                var hd = new Vector3(Mathf.Cos(az), 0.03f, Mathf.Sin(az)).normalized;
                hsum += Radiance(hd, toSun, 10, 4);
                for (int e = 1; e <= 3; e++)
                {
                    float el = e * Mathf.PI / 8f;
                    var dd = new Vector3(Mathf.Cos(az) * Mathf.Cos(el), Mathf.Sin(el), Mathf.Sin(az) * Mathf.Cos(el));
                    float w = Mathf.Sin(el) * Mathf.Cos(el);   // cosine weight x solid angle
                    asum += Radiance(dd, toSun, 10, 4) * w;
                    wsum += w;
                }
            }
            var hz = hsum / 8f * SkyExposure;
            L.horizon = new Color(hz.x, hz.y, hz.z);
            var amb = asum / wsum * SkyExposure;
            // the single-scattering sky alone is too blue and (with the sun-side sky) too strong for diffuse fill:
            // half-desaturate it (multiple scattering, ground bounce) and fix its brightness so the sun stays dominant
            var ac = new Color(amb.x, amb.y, amb.z);
            float al = Mathf.Max(1e-3f, ac.grayscale);
            L.ambientSky = Color.Lerp(new Color(al, al, al), ac, 0.5f) * (0.5f / al);
            // light bounced off the (mostly sand / grass) ground onto vertical and downward faces
            L.ambientGround = new Color(0.55f, 0.48f, 0.38f) * ((L.sun.grayscale * Mathf.Max(0.2f, toSun.y) + 0.55f) * 0.45f);
            L.fog = L.horizon;
            return L;
        }
    }
}
