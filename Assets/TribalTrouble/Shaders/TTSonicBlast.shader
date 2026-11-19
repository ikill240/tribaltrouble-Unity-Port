// Port of the Java SonicBlastShader: a ground-parallel quad on which the blast's shock ring (and three trailing,
// slower rings) expand from the centre, with a little sine turbulence, fading as they grow. Additive, no depth test.
// _Blast: x = time since the blast, y = visual radius (1.2 x the damage radius), z = expansion speed.
Shader "TribalTrouble/SonicBlast"
{
    Properties
    {
        _Color ("Colour", Color) = (0.7, 0.85, 1, 1)
        _Blast ("Time, radius, speed", Vector) = (0, 10, 10, 0)
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent+20" "IgnoreProjector" = "True" }
        Pass
        {
            Name "SonicBlast"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha One
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _Blast;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; float fogFactor : TEXCOORD1; };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                VertexPositionInputs p = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = p.positionCS;
                o.uv = v.uv;
                o.fogFactor = ComputeFogFactor(p.positionCS.z);
                return o;
            }

            #define NUM_RINGS 4
            #define SECONDS_AFTER_FIRST 0.005
            #define TIME_BETWEEN_RINGS 0.01
            #define RING_WIDTH 6.0

            half4 Frag(Varyings i) : SV_Target
            {
                float t = _Blast.x, maxR = _Blast.y, speed = _Blast.z;
                float dist = distance(i.uv, float2(0.5, 0.5)) * maxR * 2.0;
                float total = 0.0;
                if (t > 0.0)
                {
                    float r = t * speed;
                    if (r <= maxR + RING_WIDTH)
                        total += (1.0 - smoothstep(0.0, RING_WIDTH, abs(dist - r))) * (1.0 - smoothstep(0.0, maxR, r));
                }
                [unroll] for (int k = 1; k < NUM_RINGS; k++)
                {
                    float start = SECONDS_AFTER_FIRST + (k - 1) * TIME_BETWEEN_RINGS;
                    if (t < start) continue;
                    float r = (t - start) * (speed - k * 5.0);
                    if (r > maxR + RING_WIDTH) continue;
                    total += (1.0 - smoothstep(0.0, RING_WIDTH * 0.5, abs(dist - r))) * (1.0 - smoothstep(0.0, maxR, r)) * 0.8;
                }
                float noise = sin(i.uv.x * 20.0 + t * 10.0) * cos(i.uv.y * 20.0 - t * 5.0);
                total = saturate(total * (0.6 + 0.4 * noise));
                half3 c = MixFogColor(_Color.rgb * total, half3(0, 0, 0), i.fogFactor);
                return half4(c, 1.0);
            }
            ENDHLSL
        }
    }
}
