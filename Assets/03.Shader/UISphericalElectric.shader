Shader "UI/SphericalElectric"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HDR] _ElectricColor ("Electric Color", Color) = (0.25, 0.8, 1.8, 1)
        _Speed ("Speed", Vector) = (0.25, 0.15, 0, 0)
        _Thinness ("Thinness", Range(0.5, 8.0)) = 4.0

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
            "RenderPipeline"="UniversalPipeline"
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
            Name "Default"

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                float2 localPosition : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            half4 _ElectricColor;
            float4 _Speed;
            float _Thinness;
            half4 _TextureSampleAdd;
            float4 _ClipRect;
            CBUFFER_END

            float2 Hash22(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
                return frac(sin(p) * 43758.5453123);
            }

           float Voronoi(float2 uv)
           {
               float2 cell = floor(uv);
               float2 local = frac(uv);
               float minDist = 8.0;

               [unroll]
               for (int y = -1; y <= 1; y++)
               {
                   [unroll]
                   for (int x = -1; x <= 1; x++)
                   {
                       float2 offset = float2(x, y);
                       float2 p = Hash22(cell + offset);
                       float2 delta = offset + p - local;
                       minDist = min(minDist, length(delta));
                   }
               }

               return minDist;
           }

           float GetUIClipFactor(float2 localPosition)
           {
               #ifdef UNITY_UI_CLIP_RECT
               float2 insideMin = step(_ClipRect.xy, localPosition);
               float2 insideMax = step(localPosition, _ClipRect.zw);
               return insideMin.x * insideMin.y * insideMax.x * insideMax.y;
               #else
               return 1.0;
               #endif
           }

           Varyings vert(Attributes input)
           {
               Varyings output;
               output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
               output.color = input.color;
               output.uv = input.uv;
               output.localPosition = input.positionOS.xy;
               return output;
           }

           half4 frag(Varyings input) : SV_Target
           {
               float2 centered = input.uv * 2.0 - 1.0;
               float radiusSq = dot(centered, centered);
               float sphereMask = saturate(1.0 - smoothstep(0.92, 1.0, radiusSq));

               if (sphereMask <= 0.0)
               {
                   discard;
               }

               float z = sqrt(saturate(1.0 - radiusSq));
               float3 normalWS = normalize(float3(centered, z));
               float fresnel = pow(saturate(1.0 - normalWS.z), 2.8);

               float distortion = lerp(1.0, 1.65, 1.0 - z);
               float2 sphereUV = centered * distortion;

               float time = _Time.y;
               float2 speed = _Speed.xy;

               float2 layerAUV = sphereUV * 6.5 + speed * time;
               float2 layerBUV = sphereUV * 9.0 - speed * time * 1.15 + float2(13.1, 7.7);

               float layerA = 1.0 - saturate(Voronoi(layerAUV) * 1.85);
               float layerB = 1.0 - saturate(Voronoi(layerBUV) * 1.95);

               float electricA = pow(layerA, max(1.0, _Thinness * 2.5));
               float electricB = pow(layerB, max(1.0, _Thinness * 2.5));
               float electric = max(electricA, electricB);

               float rimMask = pow(saturate(fresnel), 1.35);
               float glow = pow(saturate(electric), 1.6) * rimMask;
               float arc = pow(saturate(electric), max(1.0, _Thinness * 3.5)) * rimMask;

               half4 spriteSample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) + _TextureSampleAdd;
               float alphaMask = spriteSample.a * sphereMask;
               float uiClip = GetUIClipFactor(input.localPosition);

               half3 finalRgb = _ElectricColor.rgb * input.color.rgb * (glow * 0.65 + arc);
               half finalAlpha = _ElectricColor.a * input.color.a * alphaMask * uiClip * saturate(glow * 0.8 + arc);

               #ifdef UNITY_UI_ALPHACLIP
               clip(finalAlpha - 0.001h);
               #endif

               return half4(finalRgb, finalAlpha);
           }
           ENDHLSL
        }
    }
}
