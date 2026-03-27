Shader "UI/ShopPanelSmokeEdge"
{
    Properties
    {
        [PerRendererData] _MainTex ("Main Tex", 2D) = "white" {}
        _BlackColor ("Black Color", Color) = (0.02, 0.02, 0.02, 1)

        _EdgeWidth ("Edge Width", Range(0.0, 0.5)) = 0.14
        _EdgeIrregularity ("Edge Irregularity", Range(0.0, 0.2)) = 0.05

        _NoiseScale ("Noise Scale", Float) = 6
        _NoiseSpeed ("Noise Speed", Float) = 0.25

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex        : SV_POSITION;
                fixed4 color         : COLOR;
                float2 texcoord      : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ClipRect;

            fixed4 _BlackColor;
            float _EdgeWidth;
            float _EdgeIrregularity;
            float _NoiseScale;
            float _NoiseSpeed;

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);

                float a = hash21(i);
                float b = hash21(i + float2(1.0, 0.0));
                float c = hash21(i + float2(0.0, 1.0));
                float d = hash21(i + float2(1.0, 1.0));

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            float fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;

                value += valueNoise(p) * amplitude;
                p = p * 2.03 + float2(3.1, 1.7);
                amplitude *= 0.5;
                value += valueNoise(p) * amplitude;
                p = p * 2.01 + float2(-2.4, 4.3);
                amplitude *= 0.5;
                value += valueNoise(p) * amplitude;

                return saturate(value / 0.875);
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord;
                float t = _Time.y * _NoiseSpeed;

                float largeNoise = fbm(float2(uv.y * (_NoiseScale * 0.55), t));
                float detailNoise = fbm(float2(uv.y * (_NoiseScale * 1.8) + 7.13, t * 1.37 + uv.y * 1.9));
                float combinedNoise = saturate(lerp(largeNoise, detailNoise, 0.35));

                float edge = _EdgeWidth + (combinedNoise - 0.5) * (_EdgeIrregularity * 2.0);
                float bodyMask = step(edge, uv.x);

                float uiClip = UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                clip(bodyMask * uiClip - 0.001);

                fixed3 finalRgb = _BlackColor.rgb * IN.color.rgb;
                return fixed4(finalRgb, 1.0);
            }
            ENDCG
        }
    }
}
