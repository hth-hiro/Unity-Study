Shader "UI/ScrollPaper"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BaseMap ("Base Map (Paper Texture)", 2D) = "white" {}
        
        _PaperTint ("Paper Tint", Color) = (1.0, 0.98, 0.9, 1.0)
        _EdgeDarkness ("Edge Darkness", Float) = 0.6
        _CurvatureStrength ("Curvature Strength", Float) = 0.08
        
        // 스크립트 연동을 위한 흔들림 / 눌림 왜곡 파라미터 
        _BottomWobbleStrength ("Bottom Wobble Strength", Float) = 0.0
        _BottomBendStrength ("Bottom Bend Strength", Float) = 0.0
        _UnrollProgress ("Unroll Progress", Range(0, 1.5)) = 0.0

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
            
            float _BottomWobbleStrength;
            float _BottomBendStrength;
            float _UnrollProgress;

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

                // 하단부 종이 마스크 (스크립트의 흔들림 값이 하단에서만 적용되도록, 글자 왜곡 최소화)
                float bottomMask = pow(1.0 - uv.y, 4.0); 
                
                // 1. 기본 곡면 왜곡
                float curveOffset = pow(distFromCenter, 2.0) * _CurvatureStrength;
                
                // 2. 스크립트 코드가 전달한 Wobble 연동
                // BottomEdge가 좌우로 흔들리면, PaperBody도 좌우로 미세하게 파도치듯(x, y) UV 굴곡 생성
                float xWobble = sin(uv.y * 3.14) * _BottomWobbleStrength * 0.1 * bottomMask;
                
                // 3. 스크립트 코드가 전달한 Bend(휨/눌림) 연동
                // 정착 시 오버슈트 또는 전개 중에 종이 가운데가 눌리거나 부풀어 오르는 효과 유도
                float bottomBend = _BottomBendStrength * cos(distFromCenter * 1.57) * bottomMask;
                
                float2 distortedUV = uv;
                // 기본 펼침 과정(_UnrollProgress) + 하단부 물리 연동값 반영
                distortedUV.y += curveOffset * (1.0 - uv.y) * _UnrollProgress + bottomBend + (xWobble * 0.5);
                distortedUV.x += xWobble;

                // 텍스처 합성
                float4 texColor = tex2D(_MainTex, distortedUV) + _TextureSampleAdd;
                float4 baseDetail = tex2D(_BaseMap, distortedUV);
                float3 detailBlend = lerp(float3(1,1,1), baseDetail.rgb, _TextureDetailStrength);

                // 4. 질감 및 그림자 연동 (왜곡될수록 깊은 그림자와 하이라이트 발생)
                float edgeShadow = 1.0 - (pow(distFromCenter, 4.0) * _EdgeDarkness);
                float dynamicDistortion = abs(_BottomWobbleStrength) + abs(_BottomBendStrength);
                
                // 하단 물성 튕김이 강해질수록 어두워지는 텐션 섀도우 지원
                float bottomShadow = lerp(1.0, 0.6 - dynamicDistortion, bottomMask * saturate(_UnrollProgress));
                
                // 튕겨 오르거나 부풀 때 하이라이트 첨가 
                float highlightLayer = max(0, bottomBend + xWobble) * 2.5 * saturate(_UnrollProgress);
                
                float3 finalColor = texColor.rgb * _PaperTint.rgb * detailBlend * edgeShadow * bottomShadow;
                finalColor += highlightLayer;

                half4 color = half4(finalColor, texColor.a * _Alpha) * IN.color;

                // 마스크 클리핑
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
