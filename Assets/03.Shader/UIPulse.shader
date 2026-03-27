Shader "UI/UIPulse"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)

        [Header(Pulse Effect)]
        _PulseIntensity ("Pulse Intensity", Float) = 0.3
        _PulseSpeed ("Pulse Speed", Float) = 1.0

        [Header(Shine Effect)]
        [HDR] _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _ShineIntensity ("Shine Intensity", Float) = 0.8
        _ShineWidth ("Shine Width", Float) = 0.2
        _ShineSpeed ("Shine Speed", Float) = 0.7
        _ShineAngle ("Shine Angle", Range(0, 360)) = 45.0

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
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            float4 _Color;
            
            float _PulseIntensity;
            float _PulseSpeed;

            float4 _ShineColor;
            float _ShineIntensity;
            float _ShineWidth;
            float _ShineSpeed;
            float _ShineAngle;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord;
                float t = _Time.y;

                // 1. 원본 텍스처 컬러
                half4 color = (tex2D(_MainTex, uv) + _TextureSampleAdd) * IN.color;

                // 2. Pulse 효과 (Additive 밝기 추가)
                // 지정된 속도로 깜빡이도록 사인파 생성 (0 ~ 1)
                float pulseSine = sin(t * _PulseSpeed * 6.28318) * 0.5 + 0.5;
                
                // 단순 곱셈이 아니라 Additive(더하기) 방식을 사용하여 어두운 이미지도 확실히 밝아지게 처리
                color.rgb += color.rgb * (pulseSine * _PulseIntensity); 
                // 어두운/검은 기반의 픽셀도 시각 강도를 얻도록 범용적인 밝기 가산
                color.rgb += pulseSine * (_PulseIntensity * 0.3);

                // 3. Shine (반사 대각선 스윕) 효과
                // 입력된 각도(_ShineAngle)를 라디안으로 변환하여 수학적 회전 매트릭스 계산
                float angleRad = _ShineAngle * (3.141592 / 180.0);
                float s = sin(angleRad);
                float c = cos(angleRad);

                // 기준점을 uv 중앙(0.5, 0.5)으로 잡고 띠의 축을 회전시킴
                float2 shiftedUV = uv - 0.5;
                float2 rotatedUV = float2(
                    shiftedUV.x * c - shiftedUV.y * s,
                    shiftedUV.x * s + shiftedUV.y * c
                ) + 0.5;

                // frac을 이용하여 0 -> 1 로 반복되는 스윕 타이머 생성
                float sweepProgress = frac(t * _ShineSpeed);
                // 띠가 화면(UV 단위 기준) 밖에서부터 완전히 지나가도록 여유 공간을 더해 -0.5 ~ +1.5 범위로 진행시킴
                float sweepPos = sweepProgress * 2.0 - 0.5;

                // 회전된 UV의 가로축을 따라 띠가 지나가며 거리를 측정
                float distToSweep = abs(rotatedUV.x - sweepPos);

                // 부드러운 띠 마스크 생성 (중심부는 1.0, 멀어질수록 부드럽게 0.0으로 fade out)
                float shineMask = 1.0 - smoothstep(0.0, _ShineWidth, distToSweep);

                // 원본 알파에 영항을 받도록 제한하면서, Shine 색상을 더해줌(Additive 블렌딩)
                color.rgb += _ShineColor.rgb * _ShineIntensity * shineMask * color.a;

                // 4. UI Rect Masking (Scroll View 영역 호환 유지)
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
