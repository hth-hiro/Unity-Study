// Assets/Shader/UIPulseWave.shader
Shader "UI/UIPulseWave"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        
        [Header(Color)]
        [HDR] _RingColor ("Ring Color", Color) = (0.5,0.8,1,1)

        [Header(Intensity)]
        _RingIntensity ("Ring Intensity", Float) = 1.0

        [Header(Speed)]
        _RingSpeed ("Ring Speed", Float) = 1.0

        [Header(Ring Shape)]
        _RingThickness ("Ring Thickness", Float) = 0.05
        _RingSoftness ("Ring Softness", Float) = 0.1

        [Header(Alpha Controls)]
        _RingAlpha ("Ring Alpha", Range(0,1)) = 1.0

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

            // Properties
            float4 _RingColor;

            float _RingIntensity;
            float _RingSpeed;
            float _RingThickness;
            float _RingSoftness;
            float _RingAlpha;

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
                float t = _Time.y;

                // 직사각형 링 (Rectangular Ring) 거리 계산
                // UV 중심(0.5, 0.5)으로부터 x축과 y축 중 가장 멀리 떨어진 거리를 기준으로 삼음
                float2 d = abs(uv - float2(0.5, 0.5));
                float dist = max(d.x, d.y) * 2.0; // 중심 0.0, 가장자리 1.0으로 매핑

                // 직사각형 파동 (Ring Wave) 계산
                float currentRadius = frac(t * _RingSpeed);
                float ringDiff = abs(dist - currentRadius);
                float softWidth = max(0.0001, _RingSoftness);

                // 링 마스크 생성 (링 형태 외에는 완벽한 0)
                float ringShape = 1.0 - smoothstep(_RingThickness, _RingThickness + softWidth, ringDiff);
                
                // 파동이 바깥으로 퍼질수록 부드럽게 사라지게 보정
                float ringFade = 1.0 - smoothstep(0.3, 0.7, currentRadius); 
                float ringMask = ringShape * ringFade;

                // 최종 색상 계산 (링 부분만 색상 발현)
                float3 finalColor = _RingColor.rgb * _RingIntensity * ringMask;
                
                // 최종 알파 계산 (링 부분만 불투명도 발현, 링 외부는 0)
                float finalAlpha = _RingAlpha * ringMask;

                // 텍스처 및 UI 컬러 반영
                half4 color = (tex2D(_MainTex, uv) + _TextureSampleAdd) * IN.color;
                
                color.rgb *= finalColor;
                color.a *= finalAlpha;

                // UI Rect Masking (Scroll View 등 완벽 호환 보장)
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
