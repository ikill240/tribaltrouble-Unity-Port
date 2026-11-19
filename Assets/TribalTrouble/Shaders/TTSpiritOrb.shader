// The Shaman's / Skald's spirit orb (and other floating glows): a hot white-tinted core fading out to a coloured rim,
// slow swirling wisps over it, and (with _Halo = 1, on a quad) a soft camera-facing halo round it. Additive, no depth
// write, so it glows over the unit instead of reading as a flat painted ball.
Shader "TribalTrouble/SpiritOrb"
{
    Properties
    {
        _BaseColor ("Colour", Color) = (0.45, 1, 0.75, 1)
        _Intensity ("Intensity", Float) = 1.6
        _Halo ("Halo quad (billboard)", Float) = 0
        _HaloSize ("Halo size (object units)", Float) = 2.6
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent+10" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        Pass
        {
            Name "SpiritOrb"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Blend One One
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Intensity, _Halo, _HaloSize;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; float2 uv : TEXCOORD0; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewWS : TEXCOORD1;
                float3 posOS : TEXCOORD2;
                float2 uv : TEXCOORD3;
                float fog : TEXCOORD4;
            };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                float3 centreWS = TransformObjectToWorld(float3(0, 0, 0));
                float3 posWS;
                if (_Halo > 0.5)
                {
                    // a quad facing the camera, sized by the object's scale
                    float scale = length(float3(UNITY_MATRIX_M[0].x, UNITY_MATRIX_M[1].x, UNITY_MATRIX_M[2].x));
                    float3 right = UNITY_MATRIX_V[0].xyz, up = UNITY_MATRIX_V[1].xyz;
                    posWS = centreWS + (right * v.positionOS.x + up * v.positionOS.y) * scale * _HaloSize;
                }
                else posWS = TransformObjectToWorld(v.positionOS.xyz);
                o.positionCS = TransformWorldToHClip(posWS);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.viewWS = GetWorldSpaceViewDir(posWS);
                o.posOS = v.positionOS.xyz;
                o.uv = v.uv;
                o.fog = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            float Wisp(float3 p, float t)
            {
                float a = sin(p.x * 7.0 + t * 1.7 + sin(p.y * 5.0 - t));
                float b = sin(p.y * 6.0 - t * 1.3 + sin(p.z * 6.5 + t * 0.7));
                float c = sin(p.z * 8.0 + t * 2.1 + sin(p.x * 4.0 + t * 0.5));
                return saturate((a + b + c) / 3.0 * 0.5 + 0.5);
            }

            half4 Frag(Varyings i) : SV_Target
            {
                float t = _Time.y;
                float3 col;
                if (_Halo > 0.5)
                {
                    float2 d = i.uv * 2.0 - 1.0;
                    float r2 = dot(d, d);
                    float glow = exp(-r2 * 5.5) * 0.55 + exp(-r2 * 18.0) * 0.35;
                    glow *= 0.85 + 0.15 * sin(t * 3.1);
                    col = _BaseColor.rgb * glow;
                }
                else
                {
                    float ndv = saturate(dot(normalize(i.normalWS), normalize(i.viewWS)));
                    float core = pow(ndv, 3.0);
                    float rim = pow(1.0 - ndv, 2.5);
                    float w = Wisp(i.posOS * 2.0, t);
                    float3 hot = lerp(_BaseColor.rgb, float3(1, 1, 1), 0.65);
                    col = hot * core * (1.4 + 0.4 * w) + _BaseColor.rgb * (rim * 1.2 + w * w * 0.6 * (0.4 + 0.6 * ndv));
                }
                col *= _Intensity;
                col = MixFogColor(col, half3(0, 0, 0), i.fog);   // (additive: fades into the fog, never brightens it)
                return half4(col, 1);
            }
            ENDHLSL
        }
    }
}
