// Tribal Trouble landscape: the generator's 7 blend layers (Landscape.blend_infos), in order:
//   base structure 0, structure 1..4 by alpha 0..3, lighting (alpha 4 = highlight, colour 1,.9,.6),
//   shadow (alpha 5, structure 5 is black), sea bottom structure 6 by alpha 6.
// Structures tile every 32 grid units (TEXELS_PER_GRID_UNIT 8 / STRUCTURE_SIZE 256); alpha maps cover the map.
//
// Classic: the Java bake, unchanged (baked highlight and shadow, intensity 1).
// Modern: the same structure colours and alphas, but lit in real time instead of the baked lighting:
//   - each layer gets a high-resolution detail set (albedo variation, normal map, roughness; Resources/TTReal),
//     sampled at two scales against tiling, with height-aware transitions between layers,
//   - steep ground is projected from the side (triplanar) so cliffs don't smear,
//   - large-scale colour variation, wet sand at the waterline, sun caustics in shallow water,
//   - GGX sun light with soft shadows, cloud shadows, sky + ground ambient with SSAO, height fog (TTAtmosphere).
Shader "TribalTrouble/Terrain"
{
    Properties
    {
        _S0 ("Structure 0 (base)", 2D) = "white" {}
        _S1 ("Structure 1", 2D) = "white" {}
        _S2 ("Structure 2", 2D) = "white" {}
        _S3 ("Structure 3", 2D) = "white" {}
        _S4 ("Structure 4", 2D) = "white" {}
        _S6 ("Structure 6 (sea bottom)", 2D) = "white" {}
        _AlphaA ("Alphas 0-3 (RGBA)", 2D) = "black" {}
        _AlphaB ("Highlight, shadow, sea bottom (RGB)", 2D) = "black" {}
        _Detail ("Detail", 2D) = "gray" {}
        _MapMeters ("Map size (m)", Float) = 512
        _TileMeters ("Structure tile size (m)", Float) = 64
        _DetailMeters ("Detail tile size (m)", Float) = 16
        _HighlightColor ("Highlight colour (BlendLighting r, b, g)", Color) = (1, 0.6, 0.9, 1)
        [Header(Modern detail sets per layer)]
        _D0 ("Detail 0 (albedo x2, A height)", 2D) = "gray" {}
        _D1 ("Detail 1", 2D) = "gray" {}
        _D2 ("Detail 2", 2D) = "gray" {}
        _D3 ("Detail 3", 2D) = "gray" {}
        _D4 ("Detail 4", 2D) = "gray" {}
        _N0 ("Normal 0 (A roughness)", 2D) = "bump" {}
        _N1 ("Normal 1", 2D) = "bump" {}
        _N2 ("Normal 2", 2D) = "bump" {}
        _N3 ("Normal 3", 2D) = "bump" {}
        _N4 ("Normal 4", 2D) = "bump" {}
        _RockLayer ("Layer used for cliffs (0-4)", Float) = 3
        _Caustics ("Foam (R) / caustics (G)", 2D) = "black" {}
        _RealTile ("Detail tile (m)", Float) = 3.5
        _SeaLevel ("Sea level (m)", Float) = 4.8
        _HasReal ("Has detail sets", Float) = 0
        _HMean0 ("Mean detail height, layer 0", Float) = 0.5
        _HMean ("Mean detail heights, layers 1-4", Vector) = (0.5, 0.5, 0.5, 0.5)
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Geometry" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "TTAtmosphere.hlsl"

            TEXTURE2D(_S0); SAMPLER(sampler_S0);
            TEXTURE2D(_S1);
            TEXTURE2D(_S2);
            TEXTURE2D(_S3);
            TEXTURE2D(_S4);
            TEXTURE2D(_S6);
            TEXTURE2D(_AlphaA); SAMPLER(sampler_AlphaA);
            TEXTURE2D(_AlphaB);
            TEXTURE2D(_Detail);
            TEXTURE2D(_D0); SAMPLER(sampler_D0);
            TEXTURE2D(_D1); TEXTURE2D(_D2); TEXTURE2D(_D3); TEXTURE2D(_D4);
            TEXTURE2D(_N0); TEXTURE2D(_N1); TEXTURE2D(_N2); TEXTURE2D(_N3); TEXTURE2D(_N4);
            TEXTURE2D(_Caustics);

            CBUFFER_START(UnityPerMaterial)
                float4 _S0_ST;
                float _MapMeters;
                float _TileMeters;
                float _DetailMeters;
                float4 _HighlightColor;
                float _RockLayer;
                float _RealTile;
                float _SeaLevel;
                float _HasReal;
                float _HMean0;
                float4 _HMean;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float  fogFactor  : TEXCOORD2;
            };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                VertexPositionInputs p = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = p.positionCS;
                o.positionWS = p.positionWS;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.fogFactor = ComputeFogFactor(p.positionCS.z);
                return o;
            }

            float _TT_ShadowStrength;
            float _TT_GroundExposure;

            // ---------------------------------------------------------------- triplanar helpers
            struct Tri { float3 w; float2 uvX, uvY, uvZ; bool steep; };

            Tri MakeTri(float3 p, float3 n, float scale)
            {
                Tri t;
                float3 w = pow(abs(n), 4.0);
                t.w = w / (w.x + w.y + w.z);
                t.uvX = p.zy * scale;
                t.uvY = p.xz * scale;
                t.uvZ = p.xy * scale;
                t.steep = t.w.y < 0.97;
                return t;
            }

            float4 TriSample(TEXTURE2D_PARAM(tex, smp), Tri t)
            {
                if (t.steep)
                    return SAMPLE_TEXTURE2D(tex, smp, t.uvX) * t.w.x + SAMPLE_TEXTURE2D(tex, smp, t.uvY) * t.w.y + SAMPLE_TEXTURE2D(tex, smp, t.uvZ) * t.w.z;
                return SAMPLE_TEXTURE2D(tex, smp, t.uvY);
            }
            #define TRI_SAMPLE(tex, smp, t) TriSample(TEXTURE2D_ARGS(tex, smp), t)
            // the Java structure colours as a smooth large-scale tint (their texel noise is replaced by the HD sets)
            float4 TriSampleSoft(TEXTURE2D_PARAM(tex, smp), Tri t)
            {
                const float bias = 2.5;
                if (t.steep)
                    return SAMPLE_TEXTURE2D_BIAS(tex, smp, t.uvX, bias) * t.w.x + SAMPLE_TEXTURE2D_BIAS(tex, smp, t.uvY, bias) * t.w.y + SAMPLE_TEXTURE2D_BIAS(tex, smp, t.uvZ, bias) * t.w.z;
                return SAMPLE_TEXTURE2D_BIAS(tex, smp, t.uvY, bias);
            }
            #define TRI_SOFT(tex, smp, t) TriSampleSoft(TEXTURE2D_ARGS(tex, smp), t)
            // mean colour of a detail set (its 1x1 mip): HD photos and generated sets are both used relative to it
            float3 DMean(TEXTURE2D_PARAM(tex, smp)) { return max(SAMPLE_TEXTURE2D_LOD(tex, smp, float2(0.5, 0.5), 12).rgb, 0.03); }

            // detail at two scales (the second rotated) so the 3.5 m tile never shows a grid
            float4 Detail2(TEXTURE2D_PARAM(tex, smp), float2 uv)
            {
                float4 a = SAMPLE_TEXTURE2D(tex, smp, uv);
                float2 uv2 = float2(uv.x * 0.809 - uv.y * 0.588, uv.x * 0.588 + uv.y * 0.809) * 0.23 + 0.37;
                float4 b = SAMPLE_TEXTURE2D(tex, smp, uv2);
                return lerp(a, b, 0.35);
            }

            float3 UnpackN(float4 n) { return float3(n.xy * 2.0 - 1.0, 1.0); }   // z rebuilt after blending

            // sharpen a layer transition by the layers' detail heights (rocks show through grass first, etc.)
            float HeightBlend(float a, float hTop, float hBelow)
            {
                float mid = a * (1.0 - a) * 4.0;
                // centred on the detail maps' mean height, so a layer's overall coverage stays what the Java alphas say
                // heights are passed centred on each detail map's mean (_HMean), so a layer's overall coverage stays
                // what the Java alphas say; only the shape of the transition changes
                float x = a + (hTop - hBelow) * 0.5 * mid;
                return lerp(a, smoothstep(0.2, 0.8, x), 0.6);
            }

            half4 Frag(Varyings i) : SV_Target
            {
                float2 tileUV = i.positionWS.xz / _TileMeters;
                float2 mapUV = i.positionWS.xz / _MapMeters;
                half4 a = SAMPLE_TEXTURE2D(_AlphaA, sampler_AlphaA, mapUV);
                half4 b = SAMPLE_TEXTURE2D(_AlphaB, sampler_AlphaA, mapUV);

                if (_TT_Modern < 0.5 || _HasReal < 0.5)
                {
                    // LandscapeBaker, step by step and in gamma space like the Java bake (textures are linear = raw):
                    half3 col = SAMPLE_TEXTURE2D(_S0, sampler_S0, tileUV).rgb;
                    col = lerp(col, SAMPLE_TEXTURE2D(_S1, sampler_S0, tileUV).rgb, a.r);
                    col = lerp(col, SAMPLE_TEXTURE2D(_S2, sampler_S0, tileUV).rgb, a.g);
                    col = lerp(col, SAMPLE_TEXTURE2D(_S3, sampler_S0, tileUV).rgb, a.b);
                    col = lerp(col, SAMPLE_TEXTURE2D(_S4, sampler_S0, tileUV).rgb, a.a);
                    col = saturate(col + col * _HighlightColor.rgb * b.r);    // BlendLighting: c + c * colour * alpha
                    col = lerp(col, half3(0, 0, 0), b.g);                     // structure 5 (black) by the shadow alpha
                    col = lerp(col, SAMPLE_TEXTURE2D(_S6, sampler_S0, tileUV).rgb, b.b);
                    // LandscapeShader: detail mixed in by its alpha, lighting already baked (intensity 1)
                    half4 detail = SAMPLE_TEXTURE2D(_Detail, sampler_S0, i.positionWS.xz / _DetailMeters);
                    col = lerp(col, detail.rgb, detail.a);
                    col = SRGBToLinear(col);
                    if (_TT_Modern > 0.5)
                    {
                        Light sun = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                        col *= lerp(1.0 - _TT_ShadowStrength, 1.0, sun.shadowAttenuation) * _TT_GroundExposure;
                    }
                    col = MixFog(col, i.fogFactor);
                    return half4(col, 1);
                }

                // ================================================================ modern
                float3 P = i.positionWS;
                float3 ng = normalize(i.normalWS);
                float3 camPos = GetCameraPositionWS();
                float3 V = normalize(camPos - P);
                float dist = distance(camPos, P);

                // the structure colours, projected from the side on steep ground
                Tri ts = MakeTri(P, ng, 1.0 / _TileMeters);
                Tri td = MakeTri(P, ng, 1.0 / _RealTile);

                // detail heights first (for height-aware blending)
                float4 d0 = Detail2(TEXTURE2D_ARGS(_D0, sampler_D0), td.uvY);
                float4 d1 = Detail2(TEXTURE2D_ARGS(_D1, sampler_D0), td.uvY);
                float4 d2 = Detail2(TEXTURE2D_ARGS(_D2, sampler_D0), td.uvY);
                float4 d3 = Detail2(TEXTURE2D_ARGS(_D3, sampler_D0), td.uvY);
                float4 d4 = Detail2(TEXTURE2D_ARGS(_D4, sampler_D0), td.uvY);
                // transition heights: the detail maps' fine relief plus non-repeating noise at a few metres, so the
                // 3.5 m detail tile can never print a grid into the layer borders
                float nA = SAMPLE_TEXTURE2D(_TT_CloudNoise, sampler_TT_CloudNoise, P.xz / 17.0).b - 0.5;
                float nB = SAMPLE_TEXTURE2D(_TT_CloudNoise, sampler_TT_CloudNoise, P.xz / 29.0 + 0.37).g - 0.5;
                float h0 = (d0.a - _HMean0) * 0.35 - nA * 0.3;
                float h1 = (d1.a - _HMean.x) * 0.35 + nA * 0.6;
                float h2 = (d2.a - _HMean.y) * 0.35 + nB * 0.6;
                float h3 = (d3.a - _HMean.z) * 0.35 - nA * 0.6;
                float h4 = (d4.a - _HMean.w) * 0.35 - nB * 0.6;
                float a1 = HeightBlend(a.r, h1, h0);
                float a2 = HeightBlend(a.g, h2, lerp(h0, h1, a1));
                float a3 = HeightBlend(a.b, h3, h2);
                float a4 = HeightBlend(a.a, h4, h3);
                // layer weights of the cascade
                float w4 = a4;
                float w3 = a3 * (1 - w4);
                float w2 = a2 * (1 - a3) * (1 - w4);
                float w1 = a1 * (1 - a2) * (1 - a3) * (1 - w4);
                float w0 = 1 - w1 - w2 - w3 - w4;

                // each layer: its Java colour (smooth) x the HD set's detail relative to the set's own mean colour
                float3 m0 = DMean(TEXTURE2D_ARGS(_D0, sampler_D0)), m1 = DMean(TEXTURE2D_ARGS(_D1, sampler_D0)), m2 = DMean(TEXTURE2D_ARGS(_D2, sampler_D0));
                float3 m3 = DMean(TEXTURE2D_ARGS(_D3, sampler_D0)), m4 = DMean(TEXTURE2D_ARGS(_D4, sampler_D0));
                half3 c0 = TRI_SOFT(_S0, sampler_S0, ts).rgb * (d0.rgb / m0);
                half3 c1 = TRI_SOFT(_S1, sampler_S0, ts).rgb * (d1.rgb / m1);
                half3 c2 = TRI_SOFT(_S2, sampler_S0, ts).rgb * (d2.rgb / m2);
                half3 c3 = TRI_SOFT(_S3, sampler_S0, ts).rgb * (d3.rgb / m3);
                half3 c4 = TRI_SOFT(_S4, sampler_S0, ts).rgb * (d4.rgb / m4);
                half3 col = c0 * w0 + c1 * w1 + c2 * w2 + c3 * w3 + c4 * w4;
                col = lerp(col, SAMPLE_TEXTURE2D(_S6, sampler_S0, tileUV).rgb, b.b);   // sea bottom
                col = saturate(col);
                // the Java colours were painted to be shown with 0.65 ambient + baked light: as physical reflectance
                // they are too bright and saturated (white sand, orange dirt), so calibrate them down a little
                float3 albedo = SRGBToLinear(col);
                float alum = dot(albedo, float3(0.299, 0.587, 0.114));
                albedo = lerp(alum.xxx, albedo, 0.93) * 0.8;

                // per-layer detail: albedo variation, normal, roughness
                float4 dm = d0 * w0 + d1 * w1 + d2 * w2 + d3 * w3 + d4 * w4;
                float4 n0 = Detail2(TEXTURE2D_ARGS(_N0, sampler_D0), td.uvY);
                float4 n1 = Detail2(TEXTURE2D_ARGS(_N1, sampler_D0), td.uvY);
                float4 n2 = Detail2(TEXTURE2D_ARGS(_N2, sampler_D0), td.uvY);
                float4 n3 = Detail2(TEXTURE2D_ARGS(_N3, sampler_D0), td.uvY);
                float4 n4 = Detail2(TEXTURE2D_ARGS(_N4, sampler_D0), td.uvY);
                float4 nm = n0 * w0 + n1 * w1 + n2 * w2 + n3 * w3 + n4 * w4;
                float roughness = nm.a;
                float3 tn = UnpackN(nm);
                float fade = saturate(1.0 - dist / 260.0);           // far ground: no micro detail (no shimmer)
                tn.xy *= 1.6 * fade;
                // top projection: tangent x -> world x, tangent y -> world z (whiteout blend with the ground normal)
                float3 nTop = normalize(float3(ng.x + tn.x, ng.y, ng.z + tn.y));
                float3 n = nTop;
                float3 detailAlb = 1.0;   // (already in the layer colours)
                if (td.steep)
                {
                    // cliffs: the rock set, projected from the sides
                    int rl = (int)_RockLayer;
                    float4 rx, rz, nx, nz;
                    if (rl == 2) { rx = SAMPLE_TEXTURE2D(_D2, sampler_D0, td.uvX); rz = SAMPLE_TEXTURE2D(_D2, sampler_D0, td.uvZ); nx = SAMPLE_TEXTURE2D(_N2, sampler_D0, td.uvX); nz = SAMPLE_TEXTURE2D(_N2, sampler_D0, td.uvZ); }
                    else { rx = SAMPLE_TEXTURE2D(_D3, sampler_D0, td.uvX); rz = SAMPLE_TEXTURE2D(_D3, sampler_D0, td.uvZ); nx = SAMPLE_TEXTURE2D(_N3, sampler_D0, td.uvX); nz = SAMPLE_TEXTURE2D(_N3, sampler_D0, td.uvZ); }
                    float sideW = td.w.x + td.w.z;
                    float3 rMean = rl == 2 ? m2 : m3;
                    float3 topRel = (d0.rgb / m0) * w0 + (d1.rgb / m1) * w1 + (d2.rgb / m2) * w2 + (d3.rgb / m3) * w3 + (d4.rgb / m4) * w4;
                    float3 sideRel = (rx.rgb * td.w.x + rz.rgb * td.w.z) / max(sideW, 1e-4) / rMean;
                    // on the cliff faces the rock set's side projection replaces the top-down detail
                    detailAlb = lerp(1.0, sideRel / max(topRel, 0.05), sideW);
                    roughness = lerp(roughness, (nx.a * td.w.x + nz.a * td.w.z) / max(sideW, 1e-4), sideW);
                    // side normals: x projection uses (z, y), z projection uses (x, y)
                    float2 tX = (nx.xy * 2 - 1) * 1.6 * fade, tZ = (nz.xy * 2 - 1) * 1.6 * fade;
                    float3 nX = float3(ng.x, ng.y + tX.y, ng.z + tX.x * sign(ng.x));
                    float3 nZ = float3(ng.x + tZ.x * sign(ng.z), ng.y + tZ.y, ng.z);
                    n = normalize(nTop * td.w.y + nX * td.w.x + nZ * td.w.z);
                    // steep faces read as rock: a little darker and greyer
                    albedo = lerp(albedo, albedo * float3(0.82, 0.8, 0.78), sideW * 0.6);
                }
                albedo *= detailAlb;

                // large-scale variation (breaks the structure tiling from the high camera)
                float mv1 = SAMPLE_TEXTURE2D(_TT_CloudNoise, sampler_TT_CloudNoise, P.xz / 420.0).g;
                float mv2 = SAMPLE_TEXTURE2D(_TT_CloudNoise, sampler_TT_CloudNoise, P.xz / 1700.0 + 0.5).r;
                float macro = (mv1 - 0.5) * 0.22 + (mv2 - 0.5) * 0.18;
                albedo *= 1.0 + macro;
                float lum = dot(albedo, float3(0.299, 0.587, 0.114));
                albedo = lerp(lum.xxx, albedo, 1.0 + (mv2 - 0.5) * 0.25);

                // waterline: wet, darker, glossier sand; underwater a little darker still
                float above = P.y - _SeaLevel;
                float wet = saturate(1.0 - above / 0.9) * saturate(1.0 + above / 0.3);
                float under = saturate(-above / 0.6);
                albedo *= lerp(1.0, 0.62, max(wet, under * 0.8));
                roughness = lerp(roughness, 0.18, wet * 0.85);
                n = normalize(lerp(n, ng, wet * 0.6));

                // lighting
                Light sun = GetMainLight(TransformWorldToShadowCoord(P));
                AmbientOcclusionFactor ao = GetScreenSpaceAmbientOcclusion(GetNormalizedScreenSpaceUV(i.positionCS));
                float cloud = TTCloudShadow(P);
                float sunVis = sun.shadowAttenuation * cloud * ao.directAmbientOcclusion;
                // micro-occlusion from the detail height (crevices between pebbles and blades)
                float meanH = _HMean0 * w0 + _HMean.x * w1 + _HMean.y * w2 + _HMean.z * w3 + _HMean.w * w4;
                float microAO = lerp(1.0, saturate(0.88 + (dm.a - meanH) * 1.4), fade);
                float3 lit = TTShadePBR(albedo, n, V, roughness, 0.04, sunVis, ao.indirectAmbientOcclusion * microAO, ao.indirectAmbientOcclusion);

                // sun caustics on the shallow sea floor
                if (above < 0.0)
                {
                    float depthW = -above;
                    float t = _Time.y;
                    float c1 = SAMPLE_TEXTURE2D(_Caustics, sampler_D0, P.xz / 7.0 + float2(t * 0.021, t * 0.013)).g;
                    float c2 = SAMPLE_TEXTURE2D(_Caustics, sampler_D0, P.xz / 9.3 - float2(t * 0.017, -t * 0.024)).g;
                    float caus = min(c1, c2) * 2.2;
                    float k = saturate(depthW / 0.4) * exp(-depthW / 3.5);
                    lit += _TT_SunColor.rgb * caus * k * sun.shadowAttenuation * cloud * saturate(_TT_SunDir.y) * 0.9;
                }

                lit = TTApplyFog(lit, P);
                return half4(lit, 1);
            }
            ENDHLSL
        }

        // the depth prepass (written out here: Unity 6.6 drops the whole shader when a UsePass of URP/Lit fails)
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex DepthVert
            #pragma fragment DepthFrag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            struct DepthAttributes { float4 positionOS : POSITION; };
            float4 DepthVert(DepthAttributes v) : SV_POSITION { return TransformObjectToHClip(v.positionOS.xyz); }
            half DepthFrag() : SV_Target { return 0; }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
