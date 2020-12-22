Shader "Custom/S_NightVision"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _DitherPattern("Dithering Pattern", 2D) = "white" {}
        _Color1("Dither Color 1", Color) = (0, 0, 0, 1)
        _Color2("Dither Color 2", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        
        struct Input {
            float2 uv_MainTex;
            float4 screenPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        sampler2D _DitherPattern;
        float4 _DitherPattern_TexelSize;

        float4 _Color1;
        float4 _Color2;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
            //the surface shader function which sets parameters the lighting function then uses    
        void surf(Input i, inout SurfaceOutputStandard o) {
            //texture value the dithering is based on
            float texColor = tex2D(_MainTex, i.uv_MainTex).r;

            //value from the dither pattern
            float2 screenPos = i.screenPos.xy / i.screenPos.w;
            float2 ditherCoordinate = screenPos * _ScreenParams.xy * _DitherPattern_TexelSize.xy;
            float ditherValue = tex2D(_DitherPattern, ditherCoordinate).r;

            //combine dither pattern with texture value to get final result
            float ditheredValue = step(ditherValue, texColor);
            o.Albedo = lerp(_Color1, _Color2, ditheredValue);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
