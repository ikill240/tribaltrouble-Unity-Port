// Shared "Modern" atmosphere for Tribal Trouble: physically based sky (single Rayleigh + Mie scattering), a cheap
// analytic sky for reflections / ambient, height fog with sun in-scattering (aerial perspective) and cloud shadows.
// The globals are computed and set by TTEnvironment.cs from the same model, so sky, fog, lighting and water agree.
#ifndef TT_ATMOSPHERE_INCLUDED
#define TT_ATMOSPHERE_INCLUDED

float _TT_Modern;
float4 _TT_SunDir;          // xyz: direction to the sun (world)
float4 _TT_SunColor;        // rgb: sun colour * intensity after the atmosphere (lighting units)
float4 _TT_SkyZenith;       // rgb: sky radiance straight up (exposed)
float4 _TT_SkyHorizon;      // rgb: average sky radiance at the horizon (exposed)
float4 _TT_AmbientSky;      // rgb: diffuse light from the sky (exposed, cosine weighted)
float4 _TT_AmbientGround;   // rgb: diffuse light bounced from the ground
float4 _TT_FogParams;       // x: density at the base (1/m), y: height falloff (1/m), z: base height, w: max opacity
float4 _TT_CloudParams;     // x: coverage, yz: wind offset (m), w: cloud shadow strength
float4 _TT_Wind;            // xy: wind direction, z: strength
float _TT_SkyExposure;
TEXTURE2D(_TT_CloudNoise); SAMPLER(sampler_TT_CloudNoise);

#define TT_CLOUD_HEIGHT 1800.0
#define TT_CLOUD_SCALE (1.0 / 9000.0)

// ---------------------------------------------------------------- physically based sky (sky dome only)
static const float TT_RE = 6360e3;
static const float TT_RA = 6420e3;
static const float3 TT_BR = float3(5.8e-6, 13.5e-6, 33.1e-6);
static const float TT_BM = 8e-6;
static const float TT_HR = 8000.0;
static const float TT_HM = 1200.0;
static const float TT_TMAX = 60e3;

float TTRaySphere(float3 o, float3 d, float r)
{
    float b = dot(o, d), c = dot(o, o) - r * r, disc = b * b - c;
    return disc < 0 ? -1.0 : -b + sqrt(disc);
}

float3 TTSkyRadiance(float3 d, float3 sun)
{
    float3 o = float3(0, TT_RE + 50.0, 0);
    float t = min(TTRaySphere(o, d, TT_RA), TT_TMAX);
    float b = dot(o, d), c = dot(o, o) - TT_RE * TT_RE, disc = b * b - c;
    if (disc > 0 && -b - sqrt(disc) > 0) t = -b - sqrt(disc);
    const int N = 12, M = 4;
    float ds = t / N;
    float3 sr = 0, sm = 0;
    float odR = 0, odM = 0;
    float mu = dot(d, sun);
    float g = 0.76;
    float pR = 3.0 / (16.0 * PI) * (1.0 + mu * mu);
    float pM = 3.0 / (8.0 * PI) * ((1.0 - g * g) * (1.0 + mu * mu)) / ((2.0 + g * g) * pow(max(1.0 + g * g - 2.0 * g * mu, 1e-4), 1.5));
    [loop] for (int i = 0; i < N; i++)
    {
        float3 p = o + d * (ds * (i + 0.5));
        float h = length(p) - TT_RE;
        float hr = exp(-h / TT_HR) * ds, hm = exp(-h / TT_HM) * ds;
        odR += hr; odM += hm;
        float tl = TTRaySphere(p, sun, TT_RA);
        float dl = tl / M, lR = 0, lM = 0;
        bool lit = true;
        [loop] for (int j = 0; j < M; j++)
        {
            float3 q = p + sun * (dl * (j + 0.5));
            float hh = length(q) - TT_RE;
            if (hh < 0) { lit = false; break; }
            lR += exp(-hh / TT_HR) * dl; lM += exp(-hh / TT_HM) * dl;
        }
        if (lit)
        {
            float3 tau = TT_BR * (odR + lR) + TT_BM * 1.1 * (odM + lM);
            float3 att = exp(-tau);
            sr += att * hr; sm += att * hm;
        }
    }
    return 20.0 * (sr * TT_BR * pR + sm * TT_BM * pM);
}

// ---------------------------------------------------------------- cheap sky (reflections, fog colour)
float3 TTSkyApprox(float3 d)
{
    float3 sun = _TT_SunDir.xyz;
    float up = saturate(d.y);
    float3 c = lerp(_TT_SkyHorizon.rgb, _TT_SkyZenith.rgb, pow(up, 0.5));
    float mu = saturate(dot(d, sun));
    c += _TT_SunColor.rgb * (pow(mu, 8.0) * 0.25 + pow(mu, 64.0) * 0.6) * (1.0 - up * 0.5);
    // below the horizon: the sea / ground seen in the distance
    float down = saturate(-d.y * 3.0);
    c = lerp(c, _TT_SkyHorizon.rgb * 0.35, down);
    return c;
}

// ---------------------------------------------------------------- clouds
float TTCloudDensity(float2 posXZ)
{
    float2 uv = (posXZ + _TT_CloudParams.yz) * TT_CLOUD_SCALE;
    float4 n0 = SAMPLE_TEXTURE2D_LOD(_TT_CloudNoise, sampler_TT_CloudNoise, uv, 0);
    float4 n1 = SAMPLE_TEXTURE2D_LOD(_TT_CloudNoise, sampler_TT_CloudNoise, uv * 3.7 + 0.31, 0);
    float base = n0.r * 0.75 + n1.g * 0.25;
    float cov = _TT_CloudParams.x;
    float d = saturate((base - (1.0 - cov)) / max(cov, 0.05));
    d = saturate(d - n1.b * 0.25 * (1.0 - d));
    return d;
}

/// Fraction of sunlight reaching a world point through the cloud layer (1 = no cloud).
float TTCloudShadow(float3 posWS)
{
    if (_TT_Modern < 0.5 || _TT_CloudParams.w <= 0) return 1.0;
    float3 s = _TT_SunDir.xyz;
    float t = (TT_CLOUD_HEIGHT - posWS.y) / max(s.y, 0.1);
    float d = TTCloudDensity(posWS.xz + s.xz * t);
    return 1.0 - smoothstep(0.05, 0.6, d) * _TT_CloudParams.w;
}

// ---------------------------------------------------------------- height fog with sun in-scattering
float3 TTFogColor(float3 rd)
{
    float3 sun = _TT_SunDir.xyz;
    float3 c = lerp(_TT_SkyHorizon.rgb, _TT_SkyZenith.rgb, saturate(rd.y) * 0.5);
    float mu = saturate(dot(rd, sun));
    c += _TT_SunColor.rgb * pow(mu, 6.0) * 0.35;
    return c;
}

float TTFogAmount(float3 posWS)
{
    float3 ro = GetCameraPositionWS();
    float3 v = posWS - ro;
    float t = length(v);
    float3 rd = v / max(t, 1e-4);
    float a = _TT_FogParams.x, b = _TT_FogParams.y;
    float h0 = ro.y - _TT_FogParams.z;
    float k = rd.y * b;
    float amount = abs(k) > 1e-5 ? (a / b) * exp(-h0 * b) * (1.0 - exp(-t * k)) / rd.y : a * exp(-h0 * b) * t;
    return min(1.0 - exp(-max(amount, 0)), _TT_FogParams.w);
}

float3 TTApplyFog(float3 col, float3 posWS)
{
    float3 rd = normalize(posWS - GetCameraPositionWS());
    return lerp(col, TTFogColor(rd), TTFogAmount(posWS));
}

// ---------------------------------------------------------------- shading helpers
float TTD_GGX(float NoH, float a)
{
    float a2 = a * a;
    float d = NoH * NoH * (a2 - 1.0) + 1.0;
    return a2 / (PI * d * d + 1e-6);
}
float TTV_SmithJoint(float NoV, float NoL, float a)
{
    float a2 = a * a;
    float gv = NoL * sqrt(NoV * NoV * (1.0 - a2) + a2);
    float gl = NoV * sqrt(NoL * NoL * (1.0 - a2) + a2);
    return 0.5 / max(gv + gl, 1e-5);
}
float3 TTF_Schlick(float3 f0, float VoH) { return f0 + (1.0 - f0) * pow(1.0 - VoH, 5.0); }

/// Direct sun (GGX specular + Lambert) and image-free ambient (sky gradient + ground bounce + sky reflection).
float3 TTShadePBR(float3 albedo, float3 n, float3 v, float roughness, float3 f0, float sunVis, float ao, float specOcc)
{
    float3 l = _TT_SunDir.xyz;
    float3 h = normalize(l + v);
    float NoL = saturate(dot(n, l)), NoV = saturate(dot(n, v)) + 1e-4, NoH = saturate(dot(n, h)), VoH = saturate(dot(v, h));
    float a = max(roughness * roughness, 0.002);
    float3 F = TTF_Schlick(f0, VoH);
    float3 spec = TTD_GGX(NoH, a) * TTV_SmithJoint(NoV, NoL, a) * F;
    float3 diff = albedo * (1.0 - F) / PI;
    float3 direct = (diff + spec) * NoL * _TT_SunColor.rgb * PI * sunVis;
    float skyW = 0.5 + 0.5 * n.y;
    float3 ambient = albedo * lerp(_TT_AmbientGround.rgb, _TT_AmbientSky.rgb, skyW) * ao;
    float3 r = reflect(-v, n);
    float3 Fe = f0 + (max(1.0 - roughness, f0) - f0) * pow(1.0 - NoV, 5.0);
    float3 env = TTSkyApprox(r) * Fe * specOcc * lerp(1.0, 0.25, roughness);
    return direct + ambient + env;
}

#endif
