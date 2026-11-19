// Weapon shadows: the weapon's own mesh flattened onto the ground along the sun (TTPlanarShadows builds the matrix),
// drawn as a light see-through shade - exactly the weapon's shape, however thin (a shadow map turns a spear into a
// blotchy dark smear). The stencil darkens each spot once however many faces overlap there, and nothing is drawn
// where the ground already lies in the shadow map's shadow (a unit's own body, a tree), so shades never stack up.
Shader "TribalTrouble/PlanarShadow"
{
    Properties
    {
        _ShadowColor ("Shade", Color) = (0.03, 0.04, 0.07, 0.38)
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent-60" "IgnoreProjector" = "True" }
        Pass
        {
            Name "PlanarShadow"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Off
            Offset -2, -2
            Stencil
            {
                Ref 64
                ReadMask 64
                WriteMask 64
                Comp NotEqual
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _ShadowColor;
            CBUFFER_END
            float4 _TT_SunColor;

            struct Attributes { float4 positionOS : POSITION; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; float3 positionWS : TEXCOORD0; };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                float3 ws = TransformObjectToWorld(v.positionOS.xyz);
                o.positionWS = ws;
                o.positionCS = TransformWorldToHClip(ws);
                return o;
            }

            half4 Frag(Varyings i) : SV_Target
            {
                half lit = MainLightRealtimeShadow(TransformWorldToShadowCoord(i.positionWS));
                // as strong as the sun is bright (fades out at dusk, a faint moon shade at night)
                half sun = saturate(max(_TT_SunColor.r, max(_TT_SunColor.g, _TT_SunColor.b)) * 0.9);
                half a = _ShadowColor.a * lit * sun;
                clip(a - 0.004);
                return half4(_ShadowColor.rgb, a);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
