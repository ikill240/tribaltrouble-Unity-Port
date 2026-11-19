// Port of Tribal Trouble's SpriteShader for URP.
// Java original (render/shader/SpriteShader.java):
//   base = texture(u_texture0); base.rgb = mix(base.rgb, 1, u_desaturate)
//   lit = base * light
//   if team colour: rgb = mix(lit, u_decalColor * light, texture(u_texture1).rgb)
// Classic style keeps the Java lighting exactly. Modern style shades physically (TTAtmosphere.hlsl): GGX sun
// light with real-time and cloud shadows, sky + ground ambient with SSAO, sky reflections, surface relief from the
// painted texture (filtered height bump), roughness / reflectance estimated from the texture (metal, stone and
// bone shine, cloth and thatch stay matte), light through leaves, height fog, and wind sway for foliage (_Wind)
// that the shadow and depth passes follow too.
Shader "TribalTrouble/Sprite"
{
    Properties
    {
        [MainTexture] _BaseMap ("Base", 2D) = "white" {}
        [NoScaleOffset] _TeamMask ("Team decal (mask, linear)", 2D) = "black" {}
        _TeamColor ("Team colour", Color) = (0, 0.5, 1, 1)
        _UseTeamMask ("Use team mask", Float) = 1
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Desaturate ("Desaturate", Range(0, 1)) = 0
        [Toggle(_ALPHATEST_ON)] _AlphaClip ("Alpha clip (foliage)", Float) = 0
        _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
        _AlphaFloor ("Alpha test, everything else (the Java's 0.1 with alpha to coverage = 0.5 plain)", Range(0, 1)) = 0.5
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
        _Wind ("Wind sway (foliage)", Range(0, 2)) = 0
        _Relief ("Surface relief (modern)", Range(0, 3)) = 1
        _Gloss ("Specular strength (modern)", Range(0, 2)) = 1
        _Skin ("Tier skin (stone, iron, crystal + diamond, chicken)", Vector) = (0, 0, 0, 0)
        _SkinGlow ("Tier skin crystal glow", Color) = (0.3, 0.95, 1.3, 1)
        _Trim ("Unit armour trim (rgb, a: 1 iron, 2 gold, 3 crystal, 4 diamond)", Vector) = (0, 0, 0, 0)
        _Jitter ("Unit cloth variation (rgb, a: on)", Vector) = (1, 1, 1, 0)
        [HideInInspector] _BaseColor ("Base colour", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Geometry" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        TEXTURE2D(_BaseMap);  SAMPLER(sampler_BaseMap);
        TEXTURE2D(_TeamMask); SAMPLER(sampler_TeamMask);

        #include "TTAtmosphere.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseMap_TexelSize;
            float4 _TeamColor;
            float4 _Tint;
            float4 _BaseColor;
            float _UseTeamMask;
            float _Desaturate;
            float _Cutoff;
            float _AlphaFloor;
            float _AlphaClip;
            float _Cull;
            float _Wind;
            float _Relief;
            float _Gloss;
            float4 _Skin;
            float4 _SkinGlow;
            float4 _Trim;
            float4 _Jitter;
        CBUFFER_END

        // Units drawn in bulk (_TT_VAT): the mesh's positions and normals come from a texture of baked poses, two rows
        // per pose (positions, normals) and one column per vertex; every instance blends its own two poses (x, y rows,
        // z the blend - the Java renderer's tween), and has its own team colour.
        // (declared in that variant only: the others keep exactly the constant buffers the SRP batcher expects)
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

        // ---- a building's reinforced skin (Extended rules' tiers): drawn on the building itself, in world metres.
        // _Skin: x stone (blocks clad the walls up to ~2.5 m), y iron (straps and rivets over walls and roof), z crystal +
        // diamond (some stones are glowing crystal, thin crystal veins above, diamond studs), w chicken (feather
        // shingles on the roof, a lattice of lashed bones on the walls). They layer: an iron building is stone + iron.
        float TTSkinHash(float2 p) { p = frac(p * float2(123.34, 456.21)); p += dot(p, p + 45.32); return frac(p.x * p.y); }
        float TTSkinNoise(float2 p)
        {
            float2 i = floor(p), f = frac(p); f = f * f * (3.0 - 2.0 * f);
            return lerp(lerp(TTSkinHash(i), TTSkinHash(i + float2(1, 0)), f.x), lerp(TTSkinHash(i + float2(0, 1)), TTSkinHash(i + 1.0), f.x), f.y);
        }
        // 1 within w/2 of the nearest multiple of period (anti-aliased)
        float TTSkinLine(float x, float period, float w)
        {
            float d = abs(frac(x / period + 0.5) - 0.5) * period;
            float aa = max(fwidth(x), 1e-4);
            return 1.0 - smoothstep(w * 0.5 - aa, w * 0.5 + aa, d);
        }
        float TTSkinDot(float2 d, float r) { float l = length(d); float aa = max(fwidth(l), 1e-4); return 1.0 - smoothstep(r - aa, r + aa, l); }

        // albedo in, skinned albedo out; cover: how much of the original is covered; shine: the skin's own gloss
        void TTTierSkin(float3 ws, float3 n, half teamM, inout half3 albedo, out half cover, out half shine, out half3 emit)
        {
            cover = 0; shine = 0; emit = 0;
            float3 origin = TransformObjectToWorld(float3(0, 0, 0));
            float h = ws.y - origin.y;
            float keep = 1.0 - teamM;                        // team colours stay readable
            float wall = saturate((0.8 - abs(n.y)) * 4.0);  // steep faces
            float roof = 1.0 - wall;
            bool xFace = abs(n.x) > abs(n.z);
            float u = xFace ? ws.z : ws.x;
            float2 uw = float2(u, h);
            float t = _Time.y;

            float stoneM = 0, block = 1;
            float2 sid = 0;
            if (_Skin.x > 0.5)
            {
                float top = 2.4 + TTSkinNoise(uw * float2(1.1, 0.0) + origin.xz) * 0.7;
                stoneM = wall * keep * (1.0 - smoothstep(top - 0.04, top + 0.04, h)) * step(-1.0, h);
                float2 b = uw / float2(0.82, 0.38);
                b.x += floor(b.y) * 0.5;
                float2 id = floor(b), f = frac(b);
                sid = id;
                float2 fw = fwidth(b);
                float2 e = smoothstep(0.035, 0.07 + fw, f) * smoothstep(0.035, 0.07 + fw, 1.0 - f);
                block = e.x * e.y;
                half3 stone = lerp(half3(0.16, 0.155, 0.15), half3(0.34, 0.32, 0.29), TTSkinHash(id)) * (0.78 + 0.44 * TTSkinNoise(uw * 7.0));
                stone *= lerp(0.82, 1.0, saturate(f.y * 3.0));   // each block's lower lip in shade
                stone = lerp(half3(0.07, 0.068, 0.065), stone, block);
                albedo = lerp(albedo, stone, stoneM);
                cover = max(cover, stoneM);
                shine = lerp(shine, 0.3 * block, stoneM);
            }
            if (_Skin.z > 0.5)
            {
                // a few of the stones are crystal, glowing softly from within; thin crystal veins here and there above
                float pulse = 0.8 + 0.2 * sin(t * 1.7 + TTSkinHash(sid) * 6.28);
                float cb = stoneM * block * step(0.85, TTSkinHash(sid + 17.0));
                half facet = 0.75 + 0.5 * TTSkinHash(floor(uw * 9.0));
                albedo = lerp(albedo, _SkinGlow.rgb * 0.3 * facet, cb);
                emit += _SkinGlow.rgb * cb * pulse * 0.45 * facet;
                float vn = abs(TTSkinNoise(uw * float2(0.8, 0.55) + 7.3) - 0.5);
                float sparse = smoothstep(0.55, 0.65, TTSkinNoise(uw * 0.22 + 3.1));
                float vein = (1.0 - smoothstep(0.006, 0.012 + fwidth(vn), vn)) * sparse * keep * (1.0 - stoneM) * wall;
                albedo = lerp(albedo, _SkinGlow.rgb * 0.4, vein * 0.6);
                emit += _SkinGlow.rgb * vein * pulse * 0.35;
                float c = max(cb, vein);
                cover = max(cover, c);
                shine = lerp(shine, 1.0, c);
            }
            if (_Skin.w > 0.5)
            {
                // roof: overlapping feather shingles, white to rusty red at the tips
                float2 q = ws.xz / float2(0.5, 0.42);
                q.x += floor(q.y) * 0.5;
                float2 qf = frac(q) - float2(0.5, 0.0);
                float d = length(qf * float2(1.0, 0.85));
                float feather = roof * keep;
                // hen feathers: cream to russet brown, a dark tip, each its own shade
                float fh = TTSkinHash(floor(q));
                half3 fc = lerp(half3(0.60, 0.50, 0.36), half3(0.38, 0.20, 0.09), fh);
                fc = lerp(fc, half3(0.16, 0.08, 0.04), smoothstep(0.42, 0.6, d) * 0.8);
                fc *= lerp(0.78, 1.0, smoothstep(0.0, 0.25, qf.y + 0.5 - abs(qf.x)));
                albedo = lerp(albedo, fc, feather * 0.88);
                // walls above the stones: a lattice of lashed chicken bones, red lashing where they cross
                float a1 = TTSkinLine(u + h, 0.95, 0.07), a2 = TTSkinLine(u - h, 0.95, 0.07);
                float bone = max(a1, a2) * wall * keep * (1.0 - stoneM);
                half3 bc = lerp(half3(0.62, 0.58, 0.47), half3(0.38, 0.05, 0.03), a1 * a2);
                albedo = lerp(albedo, bc, bone);
                cover = max(cover, max(feather, bone));
                shine = lerp(shine, 0.35, bone);
            }
            if (_Skin.y > 0.5)
            {
                // iron: straps round the walls every 1.3 m and up them every 2.2 m, a grid of straps over the roof,
                // rivets along them (diamond studs on a crystal-bound building)
                float hb = TTSkinLine(h, 1.3, 0.17), vb = TTSkinLine(u, 2.2, 0.19);
                float strap = max(hb, vb) * wall + max(TTSkinLine(ws.x, 2.4, 0.15), TTSkinLine(ws.z, 2.4, 0.15)) * roof;
                strap = saturate(strap) * keep;
                float2 dh = float2((frac(u / 0.3 + 0.5) - 0.5) * 0.3, (frac(h / 1.3 + 0.5) - 0.5) * 1.3);
                float2 dv = float2((frac(u / 2.2 + 0.5) - 0.5) * 2.2, (frac(h / 0.3 + 0.5) - 0.5) * 0.3);
                float rivet = max(TTSkinDot(dh, 0.038), TTSkinDot(dv, 0.038)) * wall * keep;
                half3 iron = half3(0.045, 0.048, 0.056) * (0.85 + 0.3 * TTSkinNoise(uw * 11.0));
                albedo = lerp(albedo, iron, strap);
                bool diamond = _Skin.z > 0.5;
                half3 stud = diamond ? half3(0.85, 0.92, 1.0) : half3(0.2, 0.2, 0.22);
                albedo = lerp(albedo, stud, rivet);
                if (diamond) emit += half3(0.7, 0.85, 1.0) * rivet * (0.15 + 0.35 * pow(saturate(sin(t * 2.3 + dot(ws, float3(3.1, 1.7, 2.3)))), 8.0));
                cover = max(cover, max(strap, rivet));
                shine = lerp(shine, 1.0, max(strap, rivet));
            }
        }

        // Foliage sway: the whole tree bends from its foot as one piece - a slow sway with gusts, each tree in its own
        // phase - and keeps its length (it leans over, it doesn't stretch). The leaves only rustle a little, in the same
        // phase all over the crown (a ripple running through the crown made trees look wavy, like seen through water).
        float3 TTWind(float3 ws)
        {
            if (_Wind <= 0.0 || _TT_Modern < 0.5) return ws;
            float3 origin = TransformObjectToWorld(float3(0, 0, 0));
            float h = max(ws.y - origin.y, 0.0);
            float strength = _Wind * (_TT_Wind.z > 0.0 ? _TT_Wind.z : 1.0);
            float2 dir = dot(_TT_Wind.xy, _TT_Wind.xy) > 0.0 ? normalize(_TT_Wind.xy) : normalize(float2(1.0, 0.4));
            float phase = frac(sin(dot(floor(origin.xz * 2.0), float2(12.9898, 78.233))) * 43758.5453) * 6.2832;
            float t = _Time.y;
            float gust = 0.65 + 0.35 * sin(t * 0.23 + dot(origin.xz, float2(0.011, 0.007)));   // gusts roll over the island
            float sway = (sin(t * 0.9 + phase) * 0.55 + sin(t * 1.53 + phase * 1.3) * 0.2 + 0.35) * gust;
            float bend = h * h * 0.0016 * strength * sway;   // sideways lean at this height
            float2 side = float2(-dir.y, dir.x);
            float2 off = dir * bend + side * bend * 0.25 * sin(t * 0.71 + phase * 2.1);
            ws.xz += off;
            ws.y -= dot(off, off) / max(2.0 * h, 0.5);   // bending keeps the length: the top dips as it leans
            // leaves rustle: tiny, the same phase over the whole crown
            float rustle = sin(t * 4.1 + phase * 3.0) * 0.012 * saturate(h / 5.0) * strength;
            ws.xz += float2(rustle, -rustle * 0.6);
            return ws;
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
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
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
                float3 ws = TTWind(TransformObjectToWorld(pos));
                o.positionCS = TransformWorldToHClip(ws);
                o.positionWS = ws;
                o.normalWS = TransformObjectToWorldNormal(nrm);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.fogFactor = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            half Luma(half3 c) { return dot(c, half3(0.299, 0.587, 0.114)); }

            half4 Frag(Varyings i, FRONT_FACE_TYPE isFrontFace : FRONT_FACE_SEMANTIC) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half4 base = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * _Tint;
                #if defined(_ALPHATEST_ON)
                    clip(base.a - _Cutoff);
                #else
                    // the Java alpha tests every model at 0.1 (InstancedSpriteRenderer): the see-through texels of
                    // feathers, lures and cut-out cards are dropped, never drawn as dark quads
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a - _AlphaFloor);
                #endif
                base.rgb = lerp(base.rgb, half3(1, 1, 1), _Desaturate);

                float3 n = normalize(i.normalWS) * IS_FRONT_VFACE(isFrontFace, 1.0, -1.0);
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                half3 mask = SAMPLE_TEXTURE2D(_TeamMask, sampler_TeamMask, i.uv).rgb * _UseTeamMask;
                half skinCover = 0, skinShine = 0; half3 skinEmit = 0;
                if (dot(_Skin, _Skin) > 0.0)
                {
                    half3 skinned = base.rgb;
                    TTTierSkin(i.positionWS, n, max(mask.r, max(mask.g, mask.b)), skinned, skinCover, skinShine, skinEmit);
                    base.rgb = skinned;
                }
                // a warrior's look (New Tides): the metal of his gear in his tribe's armour (iron, gold, crystal,
                // diamond), and his clothes a shade of his own so a crowd isn't one colour
                if (_Trim.w > 0.5 || _Jitter.w > 0.5)
                {
                    half tl = Luma(base.rgb);
                    half tmax = max(base.r, max(base.g, base.b)), tmin = min(base.r, min(base.g, base.b));
                    half tsat = tmax > 1e-3 ? (tmax - tmin) / tmax : 0;
                    half tteam = max(mask.r, max(mask.g, mask.b));
                    if (_Trim.w > 0.5)
                    {
                        half metal = saturate((tl - 0.26) * 2.6) * saturate(1.0 - tsat * 2.2) * (1.0 - tteam);
                        half3 trimCol = _Trim.rgb * (0.5 + tl * 0.95);
                        base.rgb = lerp(base.rgb, trimCol, metal * 0.85);
                        skinCover = max(skinCover, metal * 0.85);
                        skinShine = 1.0;
                        if (_Trim.w > 2.5)   // crystal and diamond glint
                        {
                            float glint = step(0.9, TTSkinHash(floor(i.uv * 384.0)));
                            float pulse = 0.65 + 0.35 * sin(_Time.y * 2.3 + TTSkinHash(floor(i.uv * 64.0)) * 6.28);
                            skinEmit += _Trim.rgb * metal * (0.12 + 0.55 * glint * pulse);
                        }
                    }
                    if (_Jitter.w > 0.5)
                    {
                        half cloth = saturate(tsat * 2.0 - 0.25) * (1.0 - tteam);
                        base.rgb *= lerp(half3(1, 1, 1), (half3)_Jitter.rgb, cloth);
                    }
                }
                half3 col;
                if (_TT_Modern > 0.5)
                {
                    float3 camPos = GetCameraPositionWS();
                    float dist = distance(camPos, i.positionWS);
                    float3 v = normalize(camPos - i.positionWS);
                    float3 L = _TT_SunDir.xyz;

                    // relief from the painted texture: its brightness as a height field, read one texel apart on a
                    // slightly blurred mip (filtered, no 2x2 blockiness), turned into a surface gradient
                    float2 ts = _BaseMap_TexelSize.xy;
                    half hC = Luma(SAMPLE_TEXTURE2D_BIAS(_BaseMap, sampler_BaseMap, i.uv, 1.0).rgb);
                    half hU = Luma(SAMPLE_TEXTURE2D_BIAS(_BaseMap, sampler_BaseMap, i.uv + float2(ts.x, 0), 1.0).rgb);
                    half hV = Luma(SAMPLE_TEXTURE2D_BIAS(_BaseMap, sampler_BaseMap, i.uv + float2(0, ts.y), 1.0).rgb);
                    float hScale = 0.02 * _Relief * saturate(1.0 - dist / 220.0);
                    float dHdu = (hU - hC) / max(ts.x, 1e-6) * hScale, dHdv = (hV - hC) / max(ts.y, 1e-6) * hScale;
                    float2 duvx = ddx(i.uv), duvy = ddy(i.uv);
                    float dBs = dHdu * duvx.x + dHdv * duvx.y;
                    float dBt = dHdu * duvy.x + dHdv * duvy.y;
                    float3 dpdx = ddx(i.positionWS), dpdy = ddy(i.positionWS);
                    float3 r1 = cross(dpdy, n), r2 = cross(n, dpdx);
                    float det = dot(dpdx, r1);
                    float3 nb = normalize(abs(det) * n - sign(det) * (dBs * r1 + dBt * r2));
                    nb = any(isnan(nb)) || abs(det) < 1e-12 ? n : nb;

                    half3 albedo = lerp(base.rgb, (half3)TTTeamColor(), mask);
                    // material guess from the painting: bright grey = metal / stone / bone (smoother, more reflective),
                    // saturated or dark = cloth, wood, thatch, skin (rough); team cloth always matte
                    half lum = Luma(base.rgb);
                    half maxc = max(base.r, max(base.g, base.b)), minc = min(base.r, min(base.g, base.b));
                    half sat = maxc > 1e-3 ? (maxc - minc) / maxc : 0;
                    half teamM = max(mask.r, max(mask.g, mask.b));
                    half shiny = saturate((lum - 0.3) * 1.8) * saturate(1.0 - sat * 2.0) * (1.0 - teamM) * saturate(_Gloss);
                    shiny = lerp(shiny, skinShine, skinCover);
                    nb = normalize(lerp(nb, n, skinCover));   // (the painted relief is under the skin)
                    half roughness = lerp(0.82, 0.32, shiny);
                    half3 f0 = lerp(0.04.xxx, albedo * 0.6 + 0.1, shiny * 0.5);
                    // painted-in cavities (dark texels) occlude a little light
                    half cavity = saturate(lum * 1.6 + 0.45);

                    AmbientOcclusionFactor ao = GetScreenSpaceAmbientOcclusion(GetNormalizedScreenSpaceUV(i.positionCS));
                    // small figures sit inside SSAO's radius: use only part of it on models
                    ao.indirectAmbientOcclusion = lerp(1.0, ao.indirectAmbientOcclusion, 0.6);
                    ao.directAmbientOcclusion = lerp(1.0, ao.directAmbientOcclusion, 0.6);
                    float sunVis = mainLight.shadowAttenuation * TTCloudShadow(i.positionWS) * ao.directAmbientOcclusion;
                    col = TTShadePBR(albedo * (1.0 - shiny * 0.25), nb, v, roughness, f0, sunVis, ao.indirectAmbientOcclusion * cavity, ao.indirectAmbientOcclusion);

                    #if defined(_ALPHATEST_ON)
                        // leaves: sunlight shining through, seen against the sun or on the shaded side
                        // light scattered through the crown also reaches leaves in the crown's own shadow (dense
                        // crowns would otherwise go black), so only part of the translucency is shadowed
                        half back = pow(saturate(dot(v, -L)), 3.0) * 0.55 + saturate(dot(-nb, L)) * 0.35 + 0.12;
                        half leafVis = lerp(0.45, 1.0, mainLight.shadowAttenuation) * TTCloudShadow(i.positionWS);
                        col += albedo * float3(0.95, 1.0, 0.55) * _TT_SunColor.rgb * back * leafVis;
                        // sky light reaches the leaves from all sides (cards are thin): a little extra ambient fill
                        col += albedo * _TT_AmbientSky.rgb * 0.35 * ao.indirectAmbientOcclusion;
                    #endif
                    col += skinEmit;
                    col = TTApplyFog(col, i.positionWS);
                }
                else
                {
                    // Java classic lighting: clamp(globalAmbient 0.65 + N.L, 0, 1)
                    half ndl = saturate(dot(n, _TT_SunDir.xyz));
                    half3 light = pow(saturate(0.65 + ndl), 2.2).xxx;   // the Java multiply happens in gamma space
                    col = base.rgb * light;
                    col = lerp(col, (half3)TTTeamColor() * light, mask);
                    col += skinEmit;
                    col = MixFog(col, i.fogFactor);
                }
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
                float3 ws = TTWind(TransformObjectToWorld(pos));
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
                #else
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a - _AlphaFloor);
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
                o.positionCS = TransformWorldToHClip(TTWind(TransformObjectToWorld(pos)));
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            half DepthFrag(Varyings i) : SV_Target
            {
                #if defined(_ALPHATEST_ON)
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a * _Tint.a - _Cutoff);
                #else
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a - _AlphaFloor);
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
                o.positionCS = TransformWorldToHClip(TTWind(TransformObjectToWorld(pos)));
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.normalWS = TransformObjectToWorldNormal(nrm);
                return o;
            }

            half4 DNFrag(Varyings i, FRONT_FACE_TYPE isFrontFace : FRONT_FACE_SEMANTIC) : SV_Target
            {
                #if defined(_ALPHATEST_ON)
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a * _Tint.a - _Cutoff);
                #else
                    clip(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv).a - _AlphaFloor);
                #endif
                float3 n = normalize(i.normalWS) * IS_FRONT_VFACE(isFrontFace, 1.0, -1.0);
                return half4(n, 0.0);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
