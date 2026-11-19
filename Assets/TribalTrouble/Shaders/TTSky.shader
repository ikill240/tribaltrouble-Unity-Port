// Tribal Trouble's sky dome (scenery/Sky.java + SkyShader): a vertex-colour gradient dome with two scrolling
// luminance cloud layers (GeneratorClouds) blended towards the terrain's sky colour, exactly as the GLSL does:
//   c0 = mix(vertex, sky, inner.r); c1 = mix(c0, sky, outer.r)
// Modern style replaces it with a physically based sky (Rayleigh + Mie single scattering, TTAtmosphere.hlsl), a sun
// disc and a lit, self-shadowing cloud layer that drifts with the wind.
Shader "TribalTrouble/Sky"
{
    Properties
    {
        _Inner ("Inner clouds", 2D) = "black" {}
        _Outer ("Outer clouds", 2D) = "black" {}
        _SkyColor ("Sky colour", Color) = (0.95, 0.975, 1, 1)
        _InnerOffset ("Inner offset", Vector) = (0, 0, 0, 0)
        _OuterOffset ("Outer offset", Vector) = (0, 0, 0, 0)
        _Density ("Cloud density (inner, outer)", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Background" "Queue" = "Geometry+500" "PreviewType" = "Skybox" }
        Pass
        {
            Name "Sky"
            Tags { "LightMode" = "UniversalForward" }
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "TTAtmosphere.hlsl"

            TEXTURE2D(_Inner); SAMPLER(sampler_Inner);
            TEXTURE2D(_Outer);
            CBUFFER_START(UnityPerMaterial)
                float4 _SkyColor;
                float4 _InnerOffset;
                float4 _OuterOffset;
                float4 _Density;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float4 color : COLOR; float2 uv0 : TEXCOORD0; float2 uv1 : TEXCOORD1; };
            struct Varyings { float4 positionCS : SV_POSITION; float4 color : COLOR; float2 uv0 : TEXCOORD0; float2 uv1 : TEXCOORD1; float3 dirWS : TEXCOORD2; };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                VertexPositionInputs p = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = p.positionCS;
                #if UNITY_REVERSED_Z
                    o.positionCS.z = o.positionCS.w * 1e-6;   // always behind everything
                #else
                    o.positionCS.z = o.positionCS.w * (1 - 1e-6);
                #endif
                o.color = v.color;
                o.uv0 = v.uv0 + _InnerOffset.xy;
                o.uv1 = v.uv1 + _OuterOffset.xy;
                o.dirWS = p.positionWS - GetCameraPositionWS();
                return o;
            }

            float3 ModernSky(float3 d)
            {
                float3 sun = _TT_SunDir.xyz;
                float3 dd = d;
                dd.y = max(dd.y, 0.002);
                dd = normalize(dd);
                float3 col = TTSkyRadiance(dd, sun) * _TT_SkyExposure;
                // below the horizon (the dome's skirt): distant sea haze
                col = lerp(col, TTFogColor(normalize(float3(d.x, 0.0, d.z))), saturate(-d.y * 20.0));

                // sun disc with limb darkening (bloom adds the glare)
                float cosA = dot(d, sun);
                const float cosR = 0.99996;   // ~0.5 deg
                if (cosA > cosR)
                {
                    float r = saturate((1.0 - cosA) / (1.0 - cosR));
                    float limb = 1.0 - 0.6 * (1.0 - sqrt(1.0 - r * r));
                    col += _TT_SunColor.rgb * 60.0 * limb;
                }
                col += _TT_SunColor.rgb * pow(saturate(cosA), 900.0) * 1.5;

                // the cloud layer, ray-intersected at TT_CLOUD_HEIGHT
                if (d.y > 0.003)
                {
                    float3 cam = GetCameraPositionWS();
                    float t = (TT_CLOUD_HEIGHT - cam.y) / d.y;
                    float2 p = cam.xz + d.xz * t;
                    float dens = TTCloudDensity(p);
                    if (dens > 0.001)
                    {
                        // light marched towards the sun across the layer
                        float2 sxz = normalize(sun.xz + 1e-4) * (1.0 - sun.y);
                        float od = dens;
                        od += TTCloudDensity(p + sxz * 250.0);
                        od += TTCloudDensity(p + sxz * 600.0);
                        od += TTCloudDensity(p + sxz * 1200.0) * 0.7;
                        float T = exp(-od * 1.6);
                        float powder = 1.0 - exp(-dens * 4.0);
                        float mu = dot(d, sun);
                        float g = 0.55;
                        float hg = (1.0 - g * g) / pow(max(1.0 + g * g - 2.0 * g * mu, 1e-3), 1.5) * 0.25;
                        float3 lit = _TT_SunColor.rgb * (T * (0.9 + hg * 1.6) * lerp(0.6, 1.0, powder));
                        float3 amb = _TT_SkyZenith.rgb * 1.1 * (1.0 - dens * 0.4) + _TT_SkyHorizon.rgb * 0.25;
                        float3 cloud = lit + amb;
                        float alpha = smoothstep(0.0, 0.45, dens) * saturate((d.y - 0.003) * 12.0);
                        // aerial perspective: far clouds fade into the sky
                        float aerial = 1.0 - exp(-t / 45000.0);
                        cloud = lerp(cloud, col, aerial);
                        col = lerp(col, cloud, alpha);
                    }
                }
                return col;
            }

            half4 Frag(Varyings i) : SV_Target
            {
                if (_TT_Modern > 0.5)
                    return half4(ModernSky(normalize(i.dirWS)), 1);
                // the textures and vertex colours are the Java values (gamma space)
                half c0 = saturate(SAMPLE_TEXTURE2D(_Inner, sampler_Inner, i.uv0).r + _Density.x);
                half c1 = saturate(SAMPLE_TEXTURE2D(_Outer, sampler_Inner, i.uv1).r + _Density.y);
                half3 vc = saturate(i.color.rgb);
                half3 sc = saturate(_SkyColor.rgb);
                half3 col = lerp(lerp(vc, sc, c0), sc, c1);
                col = SRGBToLinear(col);
                return half4(col, 1);
            }
            ENDHLSL
        }
    }
}
