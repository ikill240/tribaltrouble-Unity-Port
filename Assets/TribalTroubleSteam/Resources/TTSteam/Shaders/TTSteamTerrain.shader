// The Steam release's landscape (LandscapeBaker + LandscapeShader), for URP.
// Bake (LandscapeBaker, gamma space; the alpha maps cover the colormap, the structures repeat _TileMeters):
//   c = S0; c = mix(c, S1..S4, alpha0..3); c = c + c * (1, 0.6, 0.9) * highlight (BlendLighting, its g/b swap);
//   c = mix(c, S6, sea alpha)
// LandscapeShader:
//   shadow = max(terrain shadow alpha, dynamic ? 0.67 * sun shadow : supply shadow alpha)
//   c = mix(c, shadow structure (terrainShadows, same tiling), shadow)
//   c = mix(c, detail.rgb, detail.a)      (detail every 4 m; its mipmaps fade the alpha, as uploaded)
//   c *= sceneTint + torchLight;  fog
// UVs: (world + 1 m) / map size, like the Java's half-texel offset to the vertex-centred grid.
Shader "TribalTroubleSteam/Terrain"
{
    Properties
    {
        _S0 ("Structure 0 (base)", 2D) = "white" {}
        _S1 ("Structure 1", 2D) = "white" {}
        _S2 ("Structure 2", 2D) = "white" {}
        _S3 ("Structure 3", 2D) = "white" {}
        _S4 ("Structure 4", 2D) = "white" {}
        _S6 ("Structure 6 (sea bottom)", 2D) = "white" {}
        _ShadowTex ("Shadow structure (terrainShadows)", 2D) = "black" {}
        _AlphaA ("Alphas 0-3 (RGBA)", 2D) = "black" {}
        _AlphaB ("Highlight, terrain shadow, sea bottom, supply shadow (RGBA)", 2D) = "black" {}
        _Detail ("Detail", 2D) = "gray" {}
        _MapMeters ("Map size (m)", Float) = 512
        _TileMeters ("Structure tile size (m)", Float) = 64
        _DetailMeters ("Detail tile size (m)", Float) = 4
        _HighlightColor ("Highlight colour (BlendLighting r, b, g)", Color) = (1, 0.6, 0.9, 1)
        _SeaLevel ("Sea level (m)", Float) = 4.8
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
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _FORWARD_PLUS _CLUSTER_LIGHT_LOOP
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_S0); SAMPLER(sampler_S0);
            TEXTURE2D(_S1);
            TEXTURE2D(_S2);
            TEXTURE2D(_S3);
            TEXTURE2D(_S4);
            TEXTURE2D(_S6);
            TEXTURE2D(_ShadowTex);
            TEXTURE2D(_AlphaA); SAMPLER(sampler_AlphaA);
            TEXTURE2D(_AlphaB);
            TEXTURE2D(_Detail); SAMPLER(sampler_Detail);

            CBUFFER_START(UnityPerMaterial)
                float4 _S0_ST;
                float _MapMeters;
                float _TileMeters;
                float _DetailMeters;
                float4 _HighlightColor;
                float _SeaLevel;
            CBUFFER_END

            float4 _TTS_SceneTint;
            float _TTS_Dynamic;
            float _TTS_Ready;

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

            half3 TorchLight(float3 positionWS, float3 n, float4 positionCS)
            {
                half3 sum = 0;
            #if defined(_ADDITIONAL_LIGHTS) || defined(_ADDITIONAL_LIGHTS_VERTEX) || defined(_FORWARD_PLUS) || defined(_CLUSTER_LIGHT_LOOP)
                InputData inputData = (InputData)0;
                inputData.positionWS = positionWS;
                inputData.normalWS = n;
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(positionCS);
                uint lightCount = GetAdditionalLightsCount();
                LIGHT_LOOP_BEGIN(lightCount)
                    Light l = GetAdditionalLight(lightIndex, positionWS);
                    half wrap = saturate((dot(n, l.direction) + 0.6) / 1.6);
                    sum += l.color * (l.distanceAttenuation * wrap);
                LIGHT_LOOP_END
            #endif
                return sum;
            }

            half4 Frag(Varyings i) : SV_Target
            {
                // textures are uploaded raw (no sRGB decode): the maths is in gamma space like the Java's
                float2 world = i.positionWS.xz + 1.0;
                float2 mapUV = world / _MapMeters;
                float2 tileUV = world / _TileMeters;
                half4 a = SAMPLE_TEXTURE2D(_AlphaA, sampler_AlphaA, mapUV);
                half4 b = SAMPLE_TEXTURE2D(_AlphaB, sampler_AlphaA, mapUV);

                half3 col = SAMPLE_TEXTURE2D(_S0, sampler_S0, tileUV).rgb;
                col = lerp(col, SAMPLE_TEXTURE2D(_S1, sampler_S0, tileUV).rgb, a.r);
                col = lerp(col, SAMPLE_TEXTURE2D(_S2, sampler_S0, tileUV).rgb, a.g);
                col = lerp(col, SAMPLE_TEXTURE2D(_S3, sampler_S0, tileUV).rgb, a.b);
                col = lerp(col, SAMPLE_TEXTURE2D(_S4, sampler_S0, tileUV).rgb, a.a);
                col = saturate(col + col * _HighlightColor.rgb * b.r);
                col = lerp(col, SAMPLE_TEXTURE2D(_S6, sampler_S0, tileUV).rgb, b.b);

                half shadow = b.g;
                if (_TTS_Dynamic > 0.5)
                {
                    Light sun = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                    shadow = max(shadow, 0.67 * (1.0 - sun.shadowAttenuation));
                }
                else shadow = max(shadow, b.a);
                col = lerp(col, SAMPLE_TEXTURE2D(_ShadowTex, sampler_S0, tileUV).rgb, shadow);

                half4 detail = SAMPLE_TEXTURE2D(_Detail, sampler_Detail, (i.positionWS.xz) / _DetailMeters);
                col = lerp(col, detail.rgb, detail.a);

                half3 tint = _TTS_Ready > 0.5 ? (half3)_TTS_SceneTint.rgb : half3(1, 1, 1);
                col *= tint + TorchLight(i.positionWS, normalize(i.normalWS), i.positionCS);
                col = SRGBToLinear(saturate(col));
                col = MixFog(col, i.fogFactor);
                return half4(col, 1);
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
