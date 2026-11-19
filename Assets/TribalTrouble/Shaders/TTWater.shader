// Tribal Trouble's sea (scenery/Water.java + WaterShader): three Gerstner waves on the open sea (none on enclosed
// inland water), the GeneratorOcean textures scrolled twice, depth-faded alpha (depthScale / min / max alpha per
// terrain), Fresnel mix with the reflection (planar reflection camera; fog colour when there is none) and a
// Blinn specular. Values and formulas follow the Java shader.
// Modern style is a physically based sea: six Gerstner waves damped in the shallows, scrolling detail normals,
// refraction of the scene below with Beer-Lambert absorption (clear turquoise shallows, deep blue / green
// depths), sky / planar reflection by Fresnel, a GGX sun glint, light glowing through wave crests, and foam on
// the shore and on wave tops. Height fog and sky come from TTAtmosphere.hlsl.
Shader "TribalTrouble/Water"
{
    Properties
    {
        _Ocean0 ("Ocean (GeneratorOcean 0)", 2D) = "gray" {}
        _Ocean1 ("Ocean detail (GeneratorOcean 1)", 2D) = "gray" {}
        _Depth ("Depth map (R = metres / 8)", 2D) = "black" {}
        _Reflection ("Reflection", 2D) = "black" {}
        _HasReflection ("Has reflection", Float) = 0
        _MapMeters ("Map size (m)", Float) = 512
        _TerrainMeters ("Terrain extent (m)", Float) = 510
        _DepthScale ("Depth scale", Float) = 1
        _MinAlpha ("Min alpha", Float) = 0.05
        _MaxAlpha ("Max alpha", Float) = 0.6
        _RepeatRate ("Repeat rate", Float) = 0.001
        _Scroll ("Scroll offsets (xy, zw)", Vector) = (0, 0, 0, 0)
        _WaveDirLen0 ("Wave 0 (dir xy, length)", Vector) = (1, 0, 60, 0)
        _WaveDirLen1 ("Wave 1", Vector) = (0.707, 0.707, 35, 0)
        _WaveDirLen2 ("Wave 2", Vector) = (-0.5, 0.866, 18, 0)
        _WaveAmpSteep0 ("Wave 0 (amp, steep)", Vector) = (0.15, 0.5, 0, 0)
        _WaveAmpSteep1 ("Wave 1", Vector) = (0.08, 0.5, 0, 0)
        _WaveAmpSteep2 ("Wave 2", Vector) = (0.04, 0.5, 0, 0)
        _WaveTime ("Wave time", Float) = 0
        _SeaBottom ("Sea bottom colour (outside the map)", Color) = (0.216, 0.259, 0.471, 1)
        _WaterNormal ("Detail normals (modern)", 2D) = "bump" {}
        _FoamCaustics ("Foam (R), caustics (G)", 2D) = "black" {}
        _HasReal ("Has modern textures", Float) = 0
        _Absorb ("Absorption per metre (rgb)", Vector) = (0.42, 0.075, 0.055, 0)
        _Scatter ("Scattering colour", Color) = (0.0, 0.11, 0.15, 1)
        _SeabedColor ("Sea floor colour beyond the terrain (linear)", Color) = (0.45, 0.42, 0.35, 1)
        _WaveDirLen3 ("Wave 3", Vector) = (0.2, -0.98, 9, 0)
        _WaveDirLen4 ("Wave 4", Vector) = (-0.8, 0.6, 5.5, 0)
        _WaveDirLen5 ("Wave 5", Vector) = (0.95, 0.31, 3.2, 0)
        _WaveAmpSteep3 ("Wave 3 (amp, steep)", Vector) = (0.05, 0.6, 0, 0)
        _WaveAmpSteep4 ("Wave 4", Vector) = (0.03, 0.6, 0, 0)
        _WaveAmpSteep5 ("Wave 5", Vector) = (0.015, 0.55, 0, 0)
        // older material compatibility
        _Shallow ("Shallow colour", Color) = (0.25, 0.55, 0.65, 0.55)
        _Deep ("Deep colour", Color) = (0.05, 0.22, 0.38, 0.85)
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
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            #include "TTAtmosphere.hlsl"

            TEXTURE2D(_Ocean0); SAMPLER(sampler_Ocean0);
            TEXTURE2D(_Ocean1);
            TEXTURE2D(_Depth); SAMPLER(sampler_Depth);
            TEXTURE2D(_Reflection); SAMPLER(sampler_Reflection);
            TEXTURE2D(_WaterNormal); SAMPLER(sampler_WaterNormal);
            TEXTURE2D(_FoamCaustics);
            float4 _TT_FogColor;
            CBUFFER_START(UnityPerMaterial)
                float _HasReflection, _MapMeters, _TerrainMeters, _DepthScale, _MinAlpha, _MaxAlpha, _RepeatRate, _WaveTime;
                float4 _Scroll;
                float4 _WaveDirLen0, _WaveDirLen1, _WaveDirLen2;
                float4 _WaveAmpSteep0, _WaveAmpSteep1, _WaveAmpSteep2;
                float4 _Shallow, _Deep, _SeaBottom;
                float _HasReal;
                float4 _Absorb, _Scatter, _SeabedColor;
                float4 _WaveDirLen3, _WaveDirLen4, _WaveDirLen5;
                float4 _WaveAmpSteep3, _WaveAmpSteep4, _WaveAmpSteep5;
            CBUFFER_END

            // COLOR.r = open sea (waves), COLOR.g = inside the map (0 on the horizon ring)
            struct Attributes { float4 positionOS : POSITION; float4 color : COLOR; };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float2 uv0 : TEXCOORD3;
                float2 uv1 : TEXCOORD4;
                float2 data : TEXCOORD5;   // x: inland (1) / sea (0), y: inside map
                float fogFactor : TEXCOORD6;
                float crest : TEXCOORD7;   // wave height above rest (m)
            };

            static const float PI_ = 3.14159265358979;

            void Gerstner(float4 dl, float4 as, float2 xy, float scale, inout float3 disp, inout float3 n)
            {
                float k = 2.0 * PI_ / dl.z;
                float omega = sqrt(9.81 * k);
                float phase = k * dot(dl.xy, xy) - omega * _WaveTime;
                float s = sin(phase), c = cos(phase);
                float A = as.x * scale;
                disp.x += as.y * A * dl.x * c;
                disp.z += as.y * A * dl.y * c;     // Java y -> Unity z
                disp.y += A * s;                   // Java z -> Unity y
                float WA = k * A;
                n.x -= WA * dl.x * c;
                n.z -= WA * dl.y * c;
                n.y -= as.y * WA * s;
            }

            Varyings Vert(Attributes v)
            {
                Varyings o;
                float3 w = TransformObjectToWorld(v.positionOS.xyz);
                float2 xy = w.xz;
                float3 disp = 0, n = float3(0, 1, 0);
                // waves everywhere on the open sea (fading them out towards the map edge left a flat, mirror-bright
                // strip along the border; the horizon ring overlaps the grid, so no crack opens there)
                float scale = v.color.r;
                bool modern = _TT_Modern > 0.5 && _HasReal > 0.5;
                if (modern)
                {
                    // waves shrink in the shallows (no waves walking up the beach), small chop everywhere
                    float depthM = SAMPLE_TEXTURE2D_LOD(_Depth, sampler_Depth, saturate(xy / _MapMeters), 0).r * 8.0;
                    float outside = distance(xy, clamp(xy, 12.0, _TerrainMeters - 12.0));
                    scale = max(scale, 0.35) * saturate((depthM + outside) / 2.5);
                }
                if (scale > 0)
                {
                    Gerstner(_WaveDirLen0, _WaveAmpSteep0, xy, scale, disp, n);
                    Gerstner(_WaveDirLen1, _WaveAmpSteep1, xy, scale, disp, n);
                    Gerstner(_WaveDirLen2, _WaveAmpSteep2, xy, scale, disp, n);
                    if (modern)
                    {
                        Gerstner(_WaveDirLen3, _WaveAmpSteep3, xy, scale, disp, n);
                        Gerstner(_WaveDirLen4, _WaveAmpSteep4, xy, scale, disp, n);
                        Gerstner(_WaveDirLen5, _WaveAmpSteep5, xy, scale, disp, n);
                    }
                }
                o.crest = disp.y;
                w += disp;
                o.positionWS = w;
                o.normalWS = normalize(n);
                o.positionCS = TransformWorldToHClip(w);
                o.screenPos = ComputeScreenPos(o.positionCS);
                o.uv0 = w.xz * _RepeatRate * 4.0 + _Scroll.xy;
                o.uv1 = w.xz * _RepeatRate * 4.0 * 1.3 + _Scroll.zw;
                o.data = float2(1 - v.color.r, v.color.g);
                o.fogFactor = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            float Noise(float2 uv) { return SAMPLE_TEXTURE2D(_Ocean0, sampler_Ocean0, uv).r; }
            float2 Gradient(float2 uv)
            {
                float e = 0.005;
                float h = Noise(uv);
                return float2(h - Noise(uv + float2(e, 0)), h - Noise(uv + float2(0, e)));
            }

            float3 DetailNormal(float2 p)
            {
                float t = _Time.y;
                float2 d1 = normalize(_WaveDirLen0.xy), d2 = normalize(_WaveDirLen1.xy);
                // three layers at unrelated scales and rotations, so the 512 px tile never lines up into a grid
                const float2x2 r1 = float2x2(0.8, -0.6, 0.6, 0.8), r2 = float2x2(0.34, 0.94, -0.94, 0.34), r3 = float2x2(-0.5, 0.87, -0.87, -0.5);
                float4 a = SAMPLE_TEXTURE2D(_WaterNormal, sampler_WaterNormal, mul(r1, p) / 27.3 + d1 * t * 0.016);
                float4 b = SAMPLE_TEXTURE2D(_WaterNormal, sampler_WaterNormal, mul(r2, p) / 10.7 - d2 * t * 0.029 + 0.5);
                float4 c = SAMPLE_TEXTURE2D(_WaterNormal, sampler_WaterNormal, mul(r3, p) / 3.7 + float2(-d1.y, d1.x) * t * 0.047 + 0.25);
                float2 ga = mul(transpose(r1), a.xy * 2 - 1), gb = mul(transpose(r2), b.xy * 2 - 1), gc = mul(transpose(r3), c.xy * 2 - 1);
                float2 g = ga * 0.5 + gb * 0.42 + gc * 0.28;
                return float3(g.x, 1.0, g.y);
            }

            half4 FragModern(Varyings i)
            {
                float3 P = i.positionWS;
                float3 camPos = GetCameraPositionWS();
                float3 V = normalize(camPos - P);
                float dist = distance(camPos, P);
                float3 L = _TT_SunDir.xyz;
                float2 suv = i.screenPos.xy / i.screenPos.w;
                float waterEye = i.screenPos.w;

                // surface normal: Gerstner + detail ripples (calmer far away to avoid sparkle noise)
                float3 dn = DetailNormal(P.xz);
                float far = saturate(dist / 400.0);
                dn.xz *= lerp(0.55, 0.18, far);
                float3 n = normalize(float3(i.normalWS.x + dn.x, i.normalWS.y, i.normalWS.z + dn.z));

                // what's under the surface (scene depth / colour), refracted
                float sceneEye = LinearEyeDepth(SampleSceneDepth(suv), _ZBufferParams);
                float thick = max(sceneEye - waterEye, 0.0);
                float2 ruv = suv + n.xz * 0.045 * saturate(thick / 1.5) / max(1.0, waterEye / 60.0);
                float sceneEyeR = LinearEyeDepth(SampleSceneDepth(ruv), _ZBufferParams);
                if (sceneEyeR < waterEye) { ruv = suv; sceneEyeR = sceneEye; }   // never refract things above the water
                float thickR = max(sceneEyeR - waterEye, 0.0);
                float3 below = SampleSceneColor(ruv);
                // light path in the water: along the view ray, plus straight down to the bottom and back up
                float pathView = thickR * dist / max(waterEye, 1e-3);
                // nothing below at all (beyond the sea floor skirt, far out): a deep, dark sea floor
                float rawR = SampleSceneDepth(ruv);
                #if UNITY_REVERSED_Z
                    bool background = rawR <= 1e-6;
                #else
                    bool background = rawR >= 1.0 - 1e-6;
                #endif
                if (background)
                {
                    pathView = 400.0;
                    below = 0;
                }
                float depthV = pathView * saturate(V.y);
                float3 T = exp(-_Absorb.xyz * (pathView + depthV));
                // in-scattered light of the water body (sun from above + sky), what deep water looks like
                float3 lightIn = _TT_SunColor.rgb * saturate(L.y) * 0.9 + _TT_AmbientSky.rgb;
                float3 deep = _Scatter.rgb * lightIn;
                float3 water = below * T + deep * (1.0 - T);

                // light through the wave crests (sub-surface scattering)
                float sss = pow(saturate(dot(V, -normalize(L + n * 0.5))), 4.0) * saturate(i.crest * 3.0 + 0.25);
                water += _Scatter.rgb * float3(0.6, 1.4, 1.2) * _TT_SunColor.rgb * sss * 1.2;

                // reflection: the planar reflection when available, else the sky
                float3 R = reflect(-V, n);
                float3 refl = TTSkyApprox(R);
                if (_HasReflection > 0.5)
                {
                    float2 pr = suv + n.xz * 0.03;
                    float2 ed = min(pr, 1 - pr);
                    float fade = saturate(min(ed.x, ed.y) / 0.06);
                    refl = lerp(refl, SAMPLE_TEXTURE2D(_Reflection, sampler_Reflection, saturate(pr)).rgb, fade);
                }
                float NoV = saturate(dot(n, V));
                float F = 0.02 + 0.98 * pow(1.0 - NoV, 5.0);
                F = min(F, 0.85);

                // sun glint (GGX, smooth water)
                Light sun = GetMainLight(TransformWorldToShadowCoord(P));
                float sunVis = sun.shadowAttenuation * TTCloudShadow(P);
                float3 H = normalize(L + V);
                float NoL = saturate(dot(n, L));
                float a = lerp(0.14, 0.26, far);
                float spec = TTD_GGX(saturate(dot(n, H)), a * a) * TTV_SmithJoint(NoV + 1e-4, NoL, a * a) * NoL;
                float3 glint = _TT_SunColor.rgb * spec * F * PI * sunVis;

                float3 col = lerp(water, refl, F) + glint;

                // foam: along the shore (thin water) and on the highest crests
                float t = _Time.y;
                float f1 = SAMPLE_TEXTURE2D(_FoamCaustics, sampler_WaterNormal, P.xz / 6.0 + float2(t * 0.02, t * 0.011)).r;
                float f2 = SAMPLE_TEXTURE2D(_FoamCaustics, sampler_WaterNormal, P.xz / 2.7 - float2(t * 0.013, -t * 0.019)).r;
                float foamTex = saturate(f1 * 0.6 + f2 * 0.6);
                float shore = saturate(1.0 - thick / 0.7) * saturate(thick / 0.03);
                float wash = 0.5 + 0.5 * sin(t * 1.3 - thick * 9.0 + f1 * 3.0);
                float crestFoam = saturate((i.crest - 0.3) * 5.0) * i.data.y;
                float foam = saturate(shore * (0.55 + 0.45 * wash) + crestFoam * 0.7);
                foam = smoothstep(0.35, 0.75, foam * foamTex * 1.6);
                float3 foamLit = float3(0.92, 0.94, 0.95) * (_TT_SunColor.rgb * saturate(dot(i.normalWS, L)) * sunVis + _TT_AmbientSky.rgb);
                col = lerp(col, foamLit, foam);

                // soft contact with the shore: fade the surface itself out over the last few centimetres
                float edge = saturate(thick / 0.05);
                col = lerp(below, col, edge);

                col = TTApplyFog(col, P);
                return half4(col, 1);
            }

            half4 Frag(Varyings i) : SV_Target
            {
                if (_TT_Modern > 0.5 && _HasReal > 0.5) return FragModern(i);
                float3 base = SAMPLE_TEXTURE2D(_Ocean0, sampler_Ocean0, i.uv0).rgb;   // gamma values (linear texture)
                float2 mapUV = saturate(i.positionWS.xz / _MapMeters);
                // beyond the terrain mesh (it ends one cell short of the map edge) there is only the sea floor colour
                // (one cell of margin: the outermost terrain row is sea floor anyway, and this closes any seam)
                float2 tc = clamp(i.positionWS.xz, 12.0, _TerrainMeters - 12.0);
                float outside = distance(i.positionWS.xz, tc);
                float depth = SAMPLE_TEXTURE2D(_Depth, sampler_Depth, mapUV).r * 8.0 + outside;
                float depthFade = smoothstep(0.0, 1.0, sqrt(saturate(depth / _DepthScale)));
                float alpha = lerp(i.data.x > 0.5 ? _MaxAlpha : _MinAlpha, _MaxAlpha, depthFade);

                float2 g = (Gradient(i.uv0) + Gradient(i.uv1)) * 0.5;
                float3 n = normalize(float3(g.x * 0.8, 0.5, g.y * 0.8));
                n = normalize(n + (i.normalWS - float3(0, 1, 0)));

                Light sun = GetMainLight();
                float3 viewDir = normalize(GetCameraPositionWS() - i.positionWS);
                float3 h = normalize(sun.direction + viewDir);
                float spec = pow(saturate(dot(n, h)), 40.0);
                float F = 0.02 + 0.98 * pow(1.0 - saturate(dot(n, viewDir)), 5.0);

                float3 fogG = LinearToSRGB(_TT_FogColor.rgb);
                float3 refl = fogG;
                if (_HasReflection > 0.5)
                {
                    float2 ruv = i.screenPos.xy / i.screenPos.w;
                    float2 detail = SAMPLE_TEXTURE2D(_Ocean1, sampler_Ocean0, i.uv0 * 2.0 + 0.01 * float2(sin(_Time.y * 4.0), cos(_Time.y * 0.23))).xy * 0.1;
                    ruv += g * 0.04 + (detail - 0.05) * 0.2;
                    float2 ed = min(ruv, 1 - ruv);
                    float fade = saturate(min(ed.x, ed.y) / 0.1);
                    refl = lerp(refl, LinearToSRGB(SAMPLE_TEXTURE2D(_Reflection, sampler_Reflection, saturate(ruv)).rgb), fade);
                }
                float3 water = base * 0.7;
                float3 col = lerp(water, refl, F * 0.9) + spec * 0.4;
                col = SRGBToLinear(saturate(col));
                // outside the map Sky.renderSeaBottom draws a flat sea floor under the water: blend it in here
                if (outside > 0.0)
                {
                    float k = saturate(outside / 0.5);
                    col = lerp(col, lerp(_SeaBottom.rgb, col, alpha), k);
                    alpha = lerp(alpha, 1.0, k);
                }

                if (_TT_Modern > 0.5)
                {
                    // sun glints and a soft foam line along the shore
                    col += sun.color * pow(saturate(dot(n, h)), 300.0) * 1.5;
                    float foamBand = saturate(1.0 - depth / 0.25) * saturate(depth / 0.03) * i.data.y;
                    float foamNoise = SAMPLE_TEXTURE2D(_Ocean1, sampler_Ocean0, i.uv1 * 6.0).a;
                    float foam = foamBand * smoothstep(0.55, 0.85, foamNoise + 0.2 * sin(_Time.y * 1.7 + depth * 20.0));
                    col = lerp(col, float3(0.92, 0.95, 0.97), foam * 0.45);
                    alpha = max(alpha, foam * 0.5);
                }
                col = MixFog(col, i.fogFactor);
                return half4(col, saturate(alpha));
            }
            ENDHLSL
        }
    }
}
