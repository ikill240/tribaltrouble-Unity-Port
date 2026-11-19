// The Steam release's map mode marks (SpriteRenderer.renderNoDetail): the player's and his allies' units and
// buildings drawn as flat squares in their tribe's colour over everything (no depth test, blended), lit like a
// modulated sprite (colour x scene tint). Per instance: the matrix (centre and size), _PointColor (linear).
Shader "TribalTroubleSteam/MapPoint"
{
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent+50" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        Pass
        {
            Name "MapPoint"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

            float4 _TTS_SceneTint;
            float _TTS_Ready;

            UNITY_INSTANCING_BUFFER_START(PointProps)
                UNITY_DEFINE_INSTANCED_PROP(float4, _PointColor)
            UNITY_INSTANCING_BUFFER_END(PointProps)

            struct Attributes { float4 positionOS : POSITION; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; float4 color : COLOR; };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.color = UNITY_ACCESS_INSTANCED_PROP(PointProps, _PointColor);
                return o;
            }

            half4 Frag(Varyings i) : SV_Target
            {
                half3 tint = _TTS_Ready > 0.5 ? (half3)_TTS_SceneTint.rgb : half3(1, 1, 1);
                half3 c = SRGBToLinear(saturate(LinearToSRGB(saturate(i.color.rgb)) * tint));
                return half4(c, i.color.a);
            }
            ENDHLSL
        }
    }
}
