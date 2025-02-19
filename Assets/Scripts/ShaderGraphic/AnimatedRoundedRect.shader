Shader "UI/AnimatedRoundedRect"
{
    Properties
    {
        _BorderRadius("BorderRadius", Float) = 5
        _PixelRect ("Vector", Vector) = (1,1,1,1)



        _Color ("Tint", Color) = (1,1,1,1)

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
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "Assets/ShaderLib/Noise.hlsl"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

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

            fixed4 _Color;
            float4 _PixelRect;
            float _BorderRadius;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

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
            
            float RoundedRectSDF(float2 pixel)
            {
                float2 centered = abs(pixel - (_PixelRect * 0.5));
                float2 halfSize = (_PixelRect * 0.5) - _BorderRadius;
                float2 distance = max(centered - halfSize, 0.0);
                return 1 - (length(distance) - _BorderRadius);
            }

            float Noise(float angle, float offset)
            {
                float a = sin(angle * 1 + .5624334 * offset)    * 3;
                float b = sin(angle * 3 + 5.1236785 * offset)   * 2;
                float c = sin(angle * 5 + 3.9876 * offset)      * 1;
                return ((a + b + c) / 6.0 + 1) / 2.0;
            }

            fixed4 frag(v2f IN) : SV_Target
            {

                float alpha = RoundedRectSDF(IN.texcoord * _PixelRect);
                float2 centered = IN.texcoord - float2(.5,.5);
                float angle = atan2(centered.y, centered.x);
                float noise = Noise(angle, _Time.y) * 2.5;

                alpha -= noise;
                alpha = saturate(alpha);
                float4 color = fixed4(1, 1, 1, alpha);

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
                return color * IN.color;
            }
            ENDHLSL
        }
    }
}
