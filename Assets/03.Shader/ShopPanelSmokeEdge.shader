Shader "UI/ShopPanelSmokeEdge"
{
    Properties
    {
        [PerRendererData] _MainTex ("Main Tex", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _SmokeColor ("Smoke Color", Color) = (0.1, 0.1, 0.1, 0.6)

        _EdgeWidth ("Edge Width", Range(0.01, 0.5)) = 0.15
        _EdgeSoftness ("Edge Softness", Range(0.001, 0.3)) = 0.08
        _EdgeIrregularity ("Edge Irregularity", Range(0,0.2)) = 0.05

        _NoiseScale ("Noise Scale", Float) = 6
        _DetailNoiseScale ("Detail Noise Scale", Float) = 12
        _DetailNoiseStrength ("Detail Noise Strength", Range(0,1)) = 0.35

        _NoiseSpeedX ("Noise Speed X", Float) = 0.1
        _NoiseSpeedY ("Noise Speed Y", Float) = 0.25
        _SmokeStrength ("Smoke Strength", Range(0,1)) = 0.5
        _DistortionStrength ("Distortion Strength", Range(0,0.05)) = 0.01
        _SmokeAlpha ("Smoke Alpha", Range(0,1)) = 0.5

        _BaseAlpha ("Base Alpha", Range(0,1)) = 1

        _ResidualEdgeCutoff ("Residual Edge Cutoff", Range(0,1)) = 0.8

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
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            fixed4 _Color;
            fixed4 _SmokeColor;
            float _EdgeWidth;
            float _EdgeSoftness;
            float _EdgeIrregularity;
            float _NoiseScale;
            float _DetailNoiseScale;
            float _DetailNoiseStrength;
            float _NoiseSpeedX;
            float _NoiseSpeedY;
            float _SmokeStrength;
            float _DistortionStrength;
            float _SmokeAlpha;
            float _BaseAlpha;
            float _ResidualEdgeCutoff;

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

            float fbm(float2 uv, float scale, float2 timeOffset)
            {
                float value = 0.0;
                float amp = 0.5;
                float2 p = uv * scale + timeOffset;

                value += valueNoise(p) * amp;
                p = p * 2.03 + float2(3.1, 1.7);
                amp *= 0.5;
                value += valueNoise(p) * amp;
                p = p * 2.01 + float2(-2.4, 4.3);
                amp *= 0.5;
                value += valueNoise(p) * amp;

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
                float2 timeOffset = float2(_Time.y * _NoiseSpeedX, _Time.y * _NoiseSpeedY);

                float largeNoise = fbm(uv + float2(0.0, uv.y * 0.08), _NoiseScale, timeOffset);
                float detailNoise = fbm(uv * float2(1.2, 1.5), _DetailNoiseScale, timeOffset * 1.3 + float2(2.7, -1.9));
                float combinedNoise = saturate(lerp(largeNoise, detailNoise, _DetailNoiseStrength));

                float edgeShift = (combinedNoise - 0.5) * (_EdgeIrregularity * 2.0);
                float distortedEdge = _EdgeWidth + edgeShift;
                float leftMask = 1.0 - smoothstep(distortedEdge, distortedEdge + _EdgeSoftness, uv.x);

                float inwardFlow = fbm(uv + float2(_Time.y * _NoiseSpeedX * 0.5, _Time.y * _NoiseSpeedY), _NoiseScale * 1.15, timeOffset * float2(0.85, 1.1) + float2(-1.4, 0.8));
                float plumeNoise = saturate(lerp(combinedNoise, inwardFlow, 0.45));
                float smokeShape = saturate((plumeNoise - 0.24) / 0.76);

                float smokeMask = saturate(leftMask * smokeShape);
                float finalSmokeMask = saturate(smokeMask * _SmokeStrength);
                float finalSmokeAlpha = saturate(finalSmokeMask * _SmokeAlpha);

                float edgeBand = 1.0 - smoothstep(0.0, _EdgeSoftness * 1.5 + 0.0001, abs(uv.x - distortedEdge));
                float distortionMask = saturate(edgeBand * finalSmokeMask);

                float2 distortedUV = uv;
                distortedUV.x += (plumeNoise * 2.0 - 1.0) * _DistortionStrength * distortionMask;
                distortedUV.y += (detailNoise * 2.0 - 1.0) * (_DistortionStrength * 0.35) * distortionMask;

                fixed4 sampledBase = tex2D(_MainTex, distortedUV) + _TextureSampleAdd;
                fixed4 baseColor = sampledBase * _Color * IN.color;

                float baseContributionAlpha = sampledBase.a * _BaseAlpha * IN.color.a;

                float smokeColorMix = saturate(finalSmokeMask * (0.45 + _SmokeStrength * 0.55) * _SmokeColor.a);
                float3 finalRgb = lerp(baseColor.rgb, _SmokeColor.rgb, smokeColorMix);

                float edgeFade = lerp(1.0, saturate(0.72 + plumeNoise * 0.28), finalSmokeMask);
                float finalAlpha = baseContributionAlpha * edgeFade;
                finalAlpha = lerp(finalAlpha, finalAlpha * saturate(0.84 + finalSmokeAlpha), finalSmokeMask);
                finalAlpha *= step(_ResidualEdgeCutoff, finalAlpha);

                fixed4 finalColor = fixed4(finalRgb, finalAlpha);
                finalColor.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                finalColor.rgb *= saturate(finalColor.a);

                #ifdef UNITY_UI_ALPHACLIP
                clip(finalColor.a - 0.001);
                #endif

                return saturate(finalColor);
            }
            ENDCG
        }
    }
}
