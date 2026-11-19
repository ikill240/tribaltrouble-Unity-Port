// The Steam release's sprite shading (InstancedSpriteShader / SpriteShader, classic lighting), for URP.
// Java (gamma space, per vertex):
//   sun   = u_sunColor * max(N.L, 0)
//   light = clamp(u_globalAmbient + sun - sun * shadow, 0, 1) + torchLight        (shadow: dynamic shadows only)
//   base.rgb = mix(base.rgb, 1, u_desaturate)
//   c = base * light;  c = mix(c, decalColor * light, texture(u_texture1).rgb)    (team decal)
//   then fog.  Day: ambient 0.65, sun 1.  Night: ambient (0.22, 0.25, 0.36), sun (0.34, 0.40, 0.58).
// The maths runs in gamma space like the Java (sampled colours are turned back to gamma, the result to linear).
// Responding (an order's target blinking): desaturate 0.5 and a uniform 50% team decal (GeneratorRespond).
// Globals (SteamGame, every frame): _TTS_SunDir (to the sun, world), _TTS_SunColor, _TTS_Ambient, _TTS_SceneTint
// (gamma values), _TTS_Dynamic (1: dynamic shadows), _TTS_Ready (1 once set; until then the day values).
// The properties and the per-material constants are the 1.4 game's sprite shader's, so the same materials and
// property blocks work with it.
Shader "TribalTroubleSteam/Sprite"
{
    Properties
    {
        [MainTexture] _BaseMap ("Base", 2D) = "white" {}
        [NoScaleOffset] _TeamMask ("Team decal (mask, linear)", 2D) = "black" {}
        _TeamColor ("Team colour", Color) = (0, 0.5, 1, 1)
        _UseTeamMask ("Use team mask", Float) = 1
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Desaturate ("Desaturate", Range(0, 1)) = 0
        _Respond ("Responding (uniform 50% decal)", Float) = 0
        _Unlit ("Unlit (tree crowns, plants: texture x scene tint)", Float) = 0
        [Toggle(_ALPHATEST_ON)] _AlphaClip ("Alpha clip (foliage)", Float) = 0
        _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
        _Wind ("(unused)", Range(0, 2)) = 0
        _Relief ("(unused)", Range(0, 3)) = 1
        _Gloss ("(unused)", Range(0, 2)) = 1
        _Skin ("(unused)", Vector) = (0, 0, 0, 0)
        _SkinGlow ("(unused)", Color) = (0.3, 0.95, 1.3, 1)
        _Trim ("(unused)", Vector) = (0, 0, 0, 0)
        _Jitter ("(unused)", Vector) = (1, 1, 1, 0)
        [HideInInspector] _BaseColor ("Base colour", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Geometry" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

        TEXTURE2D(_BaseMap);  SAMPLER(sampler_BaseMap);
        TEXTURE2D(_TeamMask); SAMPLER(sampler_TeamMask);

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseMap_TexelSize;
            float4 _TeamColor;
            float4 _Tint;
            float4 _BaseColor;
            float _UseTeamMask;
            float _Desaturate;
            float _Cutoff;
            float _AlphaClip;
            float _Cull;
            float _Wind;
            float _Relief;
            float _Gloss;
            float4 _Skin;
            float4 _SkinGlow;
            float4 _Trim;
            float4 _Jitter;
            float _Respond;
            float _Unlit;
        CBUFFER_END

        float4 _TTS_SunDir;
        float4 _TTS_SunColor;
        float4 _TTS_Ambient;
        float4 _TTS_SceneTint;
        float _TTS_Dynamic;
        float _TTS_Ready;

        // Units drawn in bulk (_TT_VAT): positions and normals from a texture of baked poses, two rows per pose and
        // one column per vertex; each instance blends its own two poses and has its own team colour.
        #if defined(_TT_VAT)
        TEXTURE2D(_TTVatTex);
        UNITY_INSTANCING_BUFFER_START(TTVatProps)
            UNITY_DEFINE_INSTANCED_PROP(float4, _TTAnim)
            UNITY_DEFINE_INSTANCED_PROP(float4, _TTTeam)
        UNITY_INSTANCING_BUFFER_END(TTVatProps)
        #endif

        void TTVatPose(uint vid, inout float3 pos, inout float3 nrm)
        {
            #if defined(_TT_VAT)
                float4 a = UNITY_ACCESS_INSTANCED_PROP(TTVatProps, _TTAnim);
                int ra = (int)(a.x + 0.5), rb = (int)(a.y + 0.5);
                float3 pa = LOAD_TEXTURE2D_LOD(_TTVatTex, int2(vid, ra * 2), 0).xyz;
                float3 pb = LOAD_TEXTURE2D_LOD(_TTVatTex, int2(vid, rb * 2), 0).xyz;
                float3 na = LOAD_TEXTURE2D_LOD(_TTVatTex, int2(vid, ra * 2 + 1), 0).xyz;
                float3 nb = LOAD_TEXTURE2D_LOD(_TTVatTex, int2(vid, rb * 2 + 1), 0).xyz;
                pos = lerp(pa, pb, a.z);
                float3 n = lerp(na, nb, a.z);
                nrm = dot(n, n) > 1e-8 ? normalize(n) : nrm;
            #endif
        }

        float3 TTTeamColor()
        {
            #if defined(_TT_VAT)
                return UNITY_ACCESS_INSTANCED_PROP(TTVatProps, _TTTeam).rgb;
            #else
                return _TeamColor.rgb;
            #endif
        }
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_local _ _ALPHATEST_ON
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _FORWARD_PLUS _CLUSTER_LIGHT_LOOP
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ _TT_VAT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                uint   vid        : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                float  fogFactor  : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings Vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                float3 pos = v.positionOS.xyz, nrm = v.normalOS;
                TTVatPose(v.vid, pos, nrm);
                float3 ws = TransformObjectToWorld(pos);
                o.positionCS = TransformWorldToHClip(ws);
                o.positionWS = ws;
                o.normalWS = TransformObjectToWorldNormal(nrm);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.fogFactor = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            // LitShader.calculatePointLights: the torches lit at night (the view's point lights)
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

            half4 Frag(Varyings i, FRONT_FACE_TYPE isFrontFace : FRONT_FACE_SEMANTIC) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * _Tint;
                #if defined(_ALPHATEST_ON)
                    clip(tex.a - _Cutoff);
                #endif
                half3 base = LinearToSRGB(saturate(tex.rgb));
                half desat = max(_Desaturate, _Respond > 0.5 ? 0.5 : 0.0);
                base = lerp(base, half3(1, 1, 1), desat);

                float3 n = normalize(i.normalWS) * IS_FRONT_VFACE(isFrontFace, 1.0, -1.0);
                bool ready = _TTS_Ready > 0.5;
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                float3 L = ready ? normalize(_TTS_SunDir.xyz) : mainLight.direction;
                half3 sunColor = ready ? (half3)_TTS_SunColor.rgb : half3(1, 1, 1);
                half3 ambient = ready ? (half3)_TTS_Ambient.rgb : half3(0.65, 0.65, 0.65);
                half shadow = _TTS_Dynamic > 0.5 ? 1.0 - mainLight.shadowAttenuation : 0.0;
                half3 sun = sunColor * saturate(dot(n, L));
                half3 light = saturate(ambient + sun - sun * shadow) + TorchLight(i.positionWS, n, i.positionCS);

                half3 col;
                if (_Unlit > 0.5)
                {
                    // replace / modulate mode (sprites registered without lighting, modulated ones): daylight is in the
                    // texture, the scene tint darkens it at night and the sun's shadow by SHADOW_INTENSITY 0.72
                    half3 tint = ready ? (half3)_TTS_SceneTint.rgb : half3(1, 1, 1);
                    col = base * (tint * lerp(1.0, 0.72, shadow) + TorchLight(i.positionWS, n, i.positionCS));
                }
                else
                {
                    col = base * light;
                    half3 mask = _Respond > 0.5 ? half3(0.5, 0.5, 0.5) : SAMPLE_TEXTURE2D(_TeamMask, sampler_TeamMask, i.uv).rgb * _UseTeamMask;
                    half3 decal = LinearToSRGB(saturate((half3)TTTeamColor()));
                    col = lerp(col, decal * light, mask);
                }
                col = SRGBToLinear(saturate(col));
                col = MixFog(col, i.fogFactor);
                return half4(col, 1);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag
            #pragma multi_compile_local _ _ALPHATEST_ON
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ _TT_VAT
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float3 _LightDirection;
            float3 _LightPosition;

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; float2 uv : TEXCOORD0; uint vid : SV_VertexID; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; };

            Varyings ShadowVert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                float3 pos = v.positionOS.xyz, nrm = v.normalOS;
                TTVatPose(v.vid, pos, nrm);
                float3 ws = TransformObjectToWorld(pos);
                float3 nws = TransformObjectToWorldNormal(nrm);
                #if defined(_CASTING_PUNCTUAL_LIGHT_SHADOW)
                    float3 lightDir = normalize(_LightPosition - ws);
                #else
                    float3 lightDir = _LightDirection;
                #endif
                float4 cs = TransformWorldToHClip(ApplyShadowBias(ws, nws, lightDir));
                #if UNITY_REVERSED_Z
                    cs.z = min(cs.z, UNITY_NEAR_CLIP_VALUE);
                #else
                    cs.z = max(cs.z, UNITY_NEAR_CLIP_VALUE);
                #endif
                o.positionCS = cs;
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            half4 ShadowFrag(Varyings i) : SV_Target
            {
                #if defined(_ALPHATEST_ON)
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a * _Tint.a - _Cutoff);
                #endif
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex DepthVert
            #pragma fragment DepthFrag
            #pragma multi_compile_local _ _ALPHATEST_ON
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ _TT_VAT

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; float2 uv : TEXCOORD0; uint vid : SV_VertexID; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; };

            Varyings DepthVert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                float3 pos = v.positionOS.xyz, nrm = v.normalOS;
                TTVatPose(v.vid, pos, nrm);
                o.positionCS = TransformWorldToHClip(TransformObjectToWorld(pos));
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            half DepthFrag(Varyings i) : SV_Target
            {
                #if defined(_ALPHATEST_ON)
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a * _Tint.a - _Cutoff);
                #endif
                return i.positionCS.z;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthNormals"
            Tags { "LightMode" = "DepthNormals" }
            ZWrite On
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex DNVert
            #pragma fragment DNFrag
            #pragma multi_compile_local _ _ALPHATEST_ON
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ _TT_VAT

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; float2 uv : TEXCOORD0; uint vid : SV_VertexID; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; float3 normalWS : TEXCOORD1; };

            Varyings DNVert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                float3 pos = v.positionOS.xyz, nrm = v.normalOS;
                TTVatPose(v.vid, pos, nrm);
                o.positionCS = TransformWorldToHClip(TransformObjectToWorld(pos));
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.normalWS = TransformObjectToWorldNormal(nrm);
                return o;
            }

            half4 DNFrag(Varyings i, FRONT_FACE_TYPE isFrontFace : FRONT_FACE_SEMANTIC) : SV_Target
            {
                #if defined(_ALPHATEST_ON)
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a * _Tint.a - _Cutoff);
                #endif
                float3 n = normalize(i.normalWS) * IS_FRONT_VFACE(isFrontFace, 1.0, -1.0);
                return half4(n, 0.0);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
