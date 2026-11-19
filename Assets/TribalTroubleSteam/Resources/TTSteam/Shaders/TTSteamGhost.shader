// The Steam release's placing ghost (BuildingGhostRenderer): the built sprite, desaturated 0.5 and modulated by
// white or red at 0.8 alpha (SpriteShader modulate mode: colour * base * scene tint), alpha tested at 0.5. Two
// passes like the Java: depth first (no colour), then the colour, blended, against that depth - so the
// building's own back faces don't show through it.
Shader "TribalTroubleSteam/Ghost"
{
    Properties
    {
        _BaseMap ("Base", 2D) = "white" {}
        _Color ("Colour", Color) = (1, 1, 1, 0.8)
        _Desaturate ("Desaturate", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent-10" "RenderType" = "Transparent" "IgnoreProjector" = "True" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _Color;
            float _Desaturate;
        CBUFFER_END
        float4 _TTS_SceneTint;
        float _TTS_Ready;
        struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
        struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; float fogFactor : TEXCOORD1; };
        Varyings Vert(Attributes v)
        {
            Varyings o;
            o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
            o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
            o.fogFactor = ComputeFogFactor(o.positionCS.z);
            return o;
        }
        ENDHLSL

        Pass
        {
            Name "GhostDepth"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            ColorMask 0
            ZWrite On
            ZTest LEqual
            Cull Back
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FragDepth
            half4 FragDepth(Varyings i) : SV_Target
            {
                clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a - 0.5);
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "GhostColour"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Back
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FragColour
            #pragma multi_compile_fog
            half4 FragColour(Varyings i) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                clip(tex.a - 0.5);
                half3 base = lerp(LinearToSRGB(saturate(tex.rgb)), half3(1, 1, 1), _Desaturate);
                half3 tint = _TTS_Ready > 0.5 ? (half3)_TTS_SceneTint.rgb : half3(1, 1, 1);
                half3 col = SRGBToLinear(saturate(base * LinearToSRGB(saturate(_Color.rgb)) * tint));
                col = MixFog(col, i.fogFactor);
                return half4(col, _Color.a * tex.a);
            }
            ENDHLSL
        }
    }
}
