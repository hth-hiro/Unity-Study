Shader "UI/ScrollPaper"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BaseMap ("Base Map (Paper Texture)", 2D) = "white" {}
        
        _PaperTint ("Paper Tint", Color) = (1.0, 0.98, 0.9, 1.0)
        _EdgeDarkness ("Edge Darkness", Float) = 0.5
        _CurvatureStrength ("Curvature Strength", Float) = 0.05
        
        // 추가된 물성 파라미터 (종이 하단의 출렁임 및 왜곡 보정용)
        _BottomBendStrength ("Bottom Bend Strength", Float) = 0.03
        _BottomWobbleSpeed ("Bottom Wobble Speed", Float) = 10.0
        
        _TextureDetailStrength ("Texture Detail Strength", Float) = 0.5
        _Alpha ("Alpha", Range(0, 1)) = 1.0

        // UI Default Properties
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
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _BaseMap;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            float4 _PaperTint;
            float _EdgeDarkness;
            float _CurvatureStrength;
            
            float _BottomBendStrength;
            float _BottomWobbleSpeed;

            float _TextureDetailStrength;
            float _Alpha;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color;
                
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord;
                float distFromCenter = abs(uv.x - 0.5) * 2.0;

                // 1. 하단부 물성 (Wobble) 효과
                // 하단(uv.y가 0에 가까움)으로 갈수록 강하게 작용하는 마스크를 만들어 출렁이는 느낌 추가
                float bottomMask = pow(1.0 - uv.y, 3.0); 
                float wobble = sin(_Time.y * _BottomWobbleSpeed + uv.x * 6.28) * _BottomBendStrength * bottomMask;
                
                // 2. 곡면 왜곡 (기존)
                float curveOffset = pow(distFromCenter, 2.0) * _CurvatureStrength;
                
                float2 distortedUV = uv;
                distortedUV.y += curveOffset * (1.0 - uv.y) + wobble;

                // 3. 텍스처 및 색상 샘플링
                float4 texColor = tex2D(_MainTex, distortedUV) + _TextureSampleAdd;
                float4 baseDetail = tex2D(_BaseMap, distortedUV);
                float3 detailBlend = lerp(float3(1,1,1), baseDetail.rgb, _TextureDetailStrength);

                // 4. 가장자리 밀도 및 음영 처리
                float edgeShadow = 1.0 - (pow(distFromCenter, 4.0) * _EdgeDarkness);
                // 하단 롤의 근접을 암시하는 수직 방향 그라데이션 섀도우 추가
                float bottomShadow = lerp(1.0, 0.85, bottomMask);

                // 5. 물성에 반응하는 텐션 하이라이트 (출렁일 때 살짝 빛을 반사함)
                float wobbleHighlight = max(0, wobble) * 1.5;
                
                float3 finalColor = texColor.rgb * _PaperTint.rgb * detailBlend * edgeShadow * bottomShadow;
                finalColor += wobbleHighlight;

                half4 color = half4(finalColor, texColor.a * _Alpha) * IN.color;

                // 마스크 연동 지원
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}
