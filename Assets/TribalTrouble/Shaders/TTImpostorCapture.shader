// Renders a tree's albedo (no lighting) into an impostor texture: TTImpostors draws distant realistic trees as
// crossed cards carrying this picture, lit by TribalTrouble/Sprite like the full models.
Shader "TribalTrouble/ImpostorCapture"
{
    Properties
    {
        _BaseMap ("Base", 2D) = "white" {}
        _Cutoff ("Cutoff", Float) = 0.4
        _Tint ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off ZWrite On ZTest LEqual
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float _Cutoff;
            float4 _Tint;
            struct a2v { float4 pos : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };
            v2f vert(a2v v) { v2f o; o.pos = UnityObjectToClipPos(v.pos); o.uv = TRANSFORM_TEX(v.uv, _BaseMap); return o; }
            float4 frag(v2f i) : SV_Target
            {
                // the camera is far from the tree, so plain sampling would pick tiny mips whose averaged alpha
                // falls under the cutoff and every leaf vanishes (bare trunks): sample near full resolution
                float4 c = tex2Dbias(_BaseMap, float4(i.uv, 0, -4));
                clip(c.a - _Cutoff);
                return float4(c.rgb * _Tint.rgb, 1);
            }
            ENDCG
        }
    }
}
