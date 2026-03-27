// Assets/Shader/UIHighlightImage.shader
Shader "UI/UIHighlightImage"
{
    Properties
    {
        [PerRendererData] _MainTex ("Main Tex", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)

        [Header(Intensity)]
        _Intensity ("Intensity", Float) = 1.0
        _PulseIntensity ("Pulse Intensity", Float) = 0.15
        _PulseSpeed ("Pulse Speed", Float) = 1.0

        [Header(Alpha)]
        _BaseAlpha ("Base Alpha", Range(0,1)) = 1.0
        _AlphaPulseMin ("Alpha Pulse Min", Range(0,1)) = 0.9
        _AlphaPulseMax ("Alpha Pulse Max", Range(0,1)) = 1.0
        _AlphaPulseSpeed ("Alpha Pulse Speed", Float) = 1.0

        [Header(Toggles)]
        [Toggle] _UseBrightnessPulse ("Use Brightness Pulse", Float) = 1.0
        [Toggle] _UseAlphaPulse ("Use Alpha Pulse", Float) = 0.0

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
            float4 _Color;

            float _Intensity;
            float _PulseIntensity;
            float _PulseSpeed;

            float _BaseAlpha;
            float _AlphaPulseMin;
            float _AlphaPulseMax;
            float _AlphaPulseSpeed;

            float _UseBrightnessPulse;
            float _UseAlphaPulse;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color; // Tint Color 적용
                
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord;
                float t = _Time.y;

                // 원본 텍스처 컬러
                half4 color = (tex2D(_MainTex, uv) + _TextureSampleAdd) * IN.color;

                // 1. 밝기 펄스 (Brightness Pulse)
                float brightSine = sin(t * _PulseSpeed) * 0.5 + 0.5; // 0 ~ 1
                float brightFactor = _Intensity;
                
                // Toggle에 따라 펄스 적용 (1.0이면 적용, 0.0이면 무시)
                if (_UseBrightnessPulse > 0.5)
                {
                    // 강조 밝기는 원본 기본값(1.0)에서 PulseIntensity만큼 추가로 밝아지도록
                    brightFactor += _PulseIntensity * brightSine;
                }
                
                color.rgb *= brightFactor;

                // 2. 알파 펄스 (Alpha Pulse)
                float alphaSine = sin(t * _AlphaPulseSpeed) * 0.5 + 0.5; // 0 ~ 1
                float currentAlpha = _BaseAlpha;

                if (_UseAlphaPulse > 0.5)
                {
                    currentAlpha *= lerp(_AlphaPulseMin, _AlphaPulseMax, alphaSine);
                }

                color.a *= currentAlpha;

                // UI Rect Masking 대상
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
