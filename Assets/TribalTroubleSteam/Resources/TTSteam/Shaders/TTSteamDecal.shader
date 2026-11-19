// The Steam release's DecalRenderer / DecalShader: a 32 x 32 (or 8 x 8) grid laid over the ground under each
// instance, following the terrain's own triangles (heights read from _TTS_Height, one texel per grid point, split
// like the terrain mesh), textured and coloured, alpha blended, depth tested but not written, pulled 5 cm towards
// the camera. Used for the selection halos, the static blob shadows, the click rings and the building sites.
// Per instance: the matrix carries the centre (x, z) and the size (scale x), _DecalColor the colour (linear).
Shader "TribalTroubleSteam/Decal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent-50" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        Pass
        {
            Name "Decal"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Off
            Offset -1, -2

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D_FLOAT(_TTS_Height);
            float4 _TTS_HeightParams;   // x: grid points per side, y: metres per grid unit

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END

            UNITY_INSTANCING_BUFFER_START(DecalProps)
                UNITY_DEFINE_INSTANCED_PROP(float4, _DecalColor)
            UNITY_INSTANCING_BUFFER_END(DecalProps)

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };

            float GroundHeight(float2 xz)
            {
                float n = _TTS_HeightParams.x;
                float2 grid = clamp(xz / _TTS_HeightParams.y, 0.0, n - 1.0);
                int2 cell = min((int2)floor(grid), (int)n - 2);
                float2 f = grid - (float2)cell;
                float h0 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell, 0).r;
                float h1 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell + int2(1, 0), 0).r;
                float h2 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell + int2(0, 1), 0).r;
                float h3 = LOAD_TEXTURE2D_LOD(_TTS_Height, cell + int2(1, 1), 0).r;
                return f.x + f.y <= 1.0
                    ? h0 + (h1 - h0) * f.x + (h2 - h0) * f.y
                    : h3 + (h2 - h3) * (1.0 - f.x) + (h1 - h3) * (1.0 - f.y);
            }

            Varyings Vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                float3 ws = TransformObjectToWorld(float3(v.positionOS.x, 0, v.positionOS.z));
                ws.y = GroundHeight(ws.xz);
                float3 vs = TransformWorldToView(ws);
                vs.z += 0.05;   // u_DepthBias (eye space looks down -z: towards the camera)
                o.positionCS = TransformWViewToHClip(vs);
                o.uv = v.uv;
                o.color = UNITY_ACCESS_INSTANCED_PROP(DecalProps, _DecalColor);
                return o;
            }

            half4 Frag(Varyings i) : SV_Target
            {
                half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * i.color;
            #if !defined(UNITY_COLORSPACE_GAMMA)
                // blended in linear space a dark decal (the blob shadows, the halos' inner shadow) darkens the ground
                // less than the Java's gamma-space blend did: the same darkening for black, untouched for bright colours
                half lum = dot(c.rgb, half3(0.2126, 0.7152, 0.0722));
                c.a = lerp(1.0 - pow(saturate(1.0 - c.a), 2.2), c.a, saturate(lum * 4.0));
            #endif
                return c;
            }
            ENDHLSL
        }
    }
}
