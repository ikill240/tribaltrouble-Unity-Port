// The Steam release's sea (scenery/Water.java + WaterShader), for URP.
//  - three Gerstner waves on the open sea (patches joined to the map border), scaled down over the last 16 m to the
//    map edge; none on enclosed inland water, none beyond the map
//  - the GeneratorOcean texture scrolled twice (the flow drifts: SteamWater), at WATER_REPEAT_RATE x 4 and x 5.2
//  - alpha from the depth under the surface (the height map at the nearest point of the map, plus the distance
//    beyond it): mix(min, max, smoothstep(sqrt(depth / scale))); inland water has min = max
//  - colour: ocean x 0.7 x scene tint, mixed by Fresnel (x 0.9) with the planar reflection (rippled by the gradient
//    and the detail texture, faded at the reflection's border) or the fog colour; + specular x 0.4; fog
// Like the Java, the specular's light direction is in eye space while the normal and the view direction are in the
// world (z up): the same highlights. Gamma-space maths, as in the Java.
Shader "TribalTroubleSteam/Water"
{
    Properties
    {
        _Ocean0 ("Ocean (GeneratorOcean 0)", 2D) = "gray" {}
        _Ocean1 ("Ocean detail (GeneratorOcean 1)", 2D) = "gray" {}
        _Reflection ("Reflection", 2D) = "black" {}
        _HasReflection ("Has reflection", Float) = 0
        _MapMeters ("Map size (m)", Float) = 512
        _DepthScale ("Depth scale", Float) = 1
        _MinAlpha ("Min alpha", Float) = 0.05
        _MaxAlpha ("Max alpha", Float) = 0.6
        _RepeatRate ("Repeat rate", Float) = 0.001
        _Scroll ("Scroll offsets (xy, zw)", Vector) = (0, 0, 0, 0)
        _WaveDirLen0 ("Wave 0 (dir xy, length)", Vector) = (1, 0, 60, 0)
        _WaveDirLen1 ("Wave 1", Vector) = (0.707, 0.707, 35, 0)
        _WaveDirLen2 ("Wave 2", Vector) = (-0.5, 0.866, 18, 0)
        _WaveAmpSteep0 ("Wave 0 (amp, steep)", Vector) = (0.15, 0.5, 0, 0)
        _WaveAmpSteep1 ("Wave 1", Vector) = (0.0795, 0.5, 0, 0)
        _WaveAmpSteep2 ("Wave 2", Vector) = (0.0405, 0.5, 0, 0)
        _WaveTime ("Wave time", Float) = 0
        _Detail ("Detail (Globals.draw_detail)", Float) = 1
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            TEXTURE2D(_Ocean0); SAMPLER(sampler_Ocean0);
            TEXTURE2D(_Ocean1);
            TEXTURE2D(_Reflection); SAMPLER(sampler_Reflection);
            TEXTURE2D_FLOAT(_TTS_Height);
            float4 _TTS_HeightParams;   // x: grid points per side, y: metres per grid unit
            float4 _TTS_SunDir;
            float4 _TTS_SceneTint;
            float _TTS_Ready;

            CBUFFER_START(UnityPerMaterial)
                float4 _Ocean0_ST;
                float _HasReflection, _MapMeters, _DepthScale, _MinAlpha, _MaxAlpha, _RepeatRate, _WaveTime, _Detail;
                float4 _Scroll;
                float4 _WaveDirLen0, _WaveDirLen1, _WaveDirLen2;
                float4 _WaveAmpSteep0, _WaveAmpSteep1, _WaveAmpSteep2;
            CBUFFER_END

            // COLOR.r = open sea (waves), COLOR.g = inside the map (0 on the horizon ring), COLOR.b = inland water (its
            // alpha starts at the maximum: Water.render draws the inland patches with u_minAlpha = u_maxAlpha)
            struct Attributes { float4 positionOS : POSITION; float4 color : COLOR; };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float2 uv0 : TEXCOORD2;
                float2 uv1 : TEXCOORD3;
                float2 baseXY : TEXCOORD4;
                float inland : TEXCOORD5;
                float fogFactor : TEXCOORD6;
            };

            static const float PI_ = 3.14159265358979;

            void Gerstner(float4 dl, float4 as, float2 xy, float scale, inout float3 disp)
            {
                float k = 2.0 * PI_ / dl.z;
                float omega = sqrt(9.81 * k);
                float phase = k * dot(dl.xy, xy) - omega * _WaveTime;
                float s = sin(phase), c = cos(phase);
                float A = as.x * scale;
                disp.x += as.y * A * dl.x * c;
                disp.z += as.y * A * dl.y * c;     // Java y -> Unity z
                disp.y += A * s;                   // Java z -> Unity y
            }

            Varyings Vert(Attributes v)
            {
                Varyings o;
                float3 w = TransformObjectToWorld(v.positionOS.xyz);
                float2 xy = w.xz;
                float3 disp = 0;
                if (_WaveAmpSteep0.x > 0.0001 && v.color.r > 0.5)
                {
                    float2 edge = min(xy, _MapMeters - xy);
                    float waveScale = saturate(min(edge.x, edge.y) / 16.0);
                    Gerstner(_WaveDirLen0, _WaveAmpSteep0, xy, waveScale, disp);
                    Gerstner(_WaveDirLen1, _WaveAmpSteep1, xy, waveScale, disp);
                    Gerstner(_WaveDirLen2, _WaveAmpSteep2, xy, waveScale, disp);
                }
                w += disp;
                o.positionWS = w;
                o.positionCS = TransformWorldToHClip(w);
                o.screenPos = ComputeScreenPos(o.positionCS);
                o.uv0 = w.xz * _RepeatRate * 4.0 + _Scroll.xy;
                o.uv1 = w.xz * _RepeatRate * 4.0 * 1.3 + _Scroll.zw;
                o.baseXY = xy;
                o.inland = v.color.b;
                o.fogFactor = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            // texture(u_HeightMap, uv): the height texture (one texel per grid point) filtered linearly
            float GroundHeight(float2 uv)
            {
                float n = _TTS_HeightParams.x;
                float2 grid = clamp(uv * n - 0.5, 0.0, n - 1.0);
                int2 cell = min((int2)floor(grid), (int)n - 2);
                float2 f = grid - (float2)cell;
                float h0 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell, 0).r;
                float h1 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell + int2(1, 0), 0).r;
                float h2 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell + int2(0, 1), 0).r;
                float h3 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell + int2(1, 1), 0).r;
                return lerp(lerp(h0, h1, f.x), lerp(h2, h3, f.x), f.y);
            }

            float Noise(float2 uv) { return SAMPLE_TEXTURE2D(_Ocean0, sampler_Ocean0, uv).r; }
            float2 Gradient(float2 uv)
            {
                float e = 0.005;
                float h = Noise(uv);
                return float2(h - Noise(uv + float2(e, 0)), h - Noise(uv + float2(0, e)));
            }

            half4 Frag(Varyings i) : SV_Target
            {
                half3 base = SAMPLE_TEXTURE2D(_Ocean0, sampler_Ocean0, i.uv0).rgb;   // gamma values (raw texture)

                // depth under the surface: the map's ground at the nearest point inside it, plus how far outside it is
                float2 hmUV = (i.baseXY + 1.0) / _MapMeters;
                float2 closest = saturate(hmUV);
                float ground = GroundHeight(closest);
                float outside = distance(hmUV, closest) * _MapMeters;
                float depth = i.positionWS.y - ground + outside;
                float depthFade = smoothstep(0.0, 1.0, sqrt(saturate(depth / _DepthScale)));
                float minAlpha = i.inland > 0.5 ? _MaxAlpha : _MinAlpha;
                float alpha = lerp(minAlpha, _MaxAlpha, depthFade);

                float2 g = (Gradient(i.uv0) + Gradient(i.uv1)) * 0.5;
                // Java's world (z up): the normal from the gradient, the view direction from the camera
                float3 nJ = normalize(float3(g * 0.8, 0.5));
                float3 camU = GetCameraPositionWS();
                float3 viewU = normalize(camU - i.positionWS);
                float3 viewJ = float3(viewU.x, viewU.z, viewU.y);
                // u_lightDirection: the sky body's direction in eye space (GlobalUniforms)
                float3 toSun = _TTS_Ready > 0.5 ? _TTS_SunDir.xyz : float3(0, 1, 0);
                float3 lightJ = normalize(mul((float3x3)UNITY_MATRIX_V, toSun));
                float3 halfDir = normalize(lightJ + viewJ);
                float specular = pow(saturate(dot(nJ, halfDir)), 40.0);
                float F = 0.02 + 0.98 * pow(1.0 - saturate(dot(nJ, viewJ)), 5.0);

                // u_fogColor (the fog colour times the scene tint: RenderSettings.fogColor, SteamGame.Fog)
                float3 fogG = LinearToSRGB(saturate(unity_FogColor.rgb));
                float3 refl = fogG;
                if (_HasReflection > 0.5)
                {
                    float2 offset = 0;
                    if (_Detail > 0.5)
                        offset = SAMPLE_TEXTURE2D(_Ocean1, sampler_Ocean0, i.uv0 * 2.0 + 0.01 * float2(sin(_Time.y * 4.0), cos(_Time.y * 0.23))).xy * 0.01;
                    float2 ruv = i.screenPos.xy / i.screenPos.w + g * 0.04 + offset;
                    float2 ed = min(ruv, 1.0 - ruv);
                    float fade = saturate(min(ed.x, ed.y) / 0.1);
                    if (fade > 0.0)
                        refl = lerp(refl, LinearToSRGB(saturate(SAMPLE_TEXTURE2D(_Reflection, sampler_Reflection, saturate(ruv)).rgb)), fade);
                }
                float3 tint = _TTS_Ready > 0.5 ? _TTS_SceneTint.rgb : float3(1, 1, 1);
                float3 water = base * 0.7 * tint;
                float3 col = lerp(water, refl, F * 0.9) + specular * 0.4;
                col = SRGBToLinear(saturate(col));
                col = MixFog(col, i.fogFactor);
                return half4(col, saturate(alpha));
            }
            ENDHLSL
        }
    }
}
