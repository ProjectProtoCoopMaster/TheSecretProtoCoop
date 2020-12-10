Shader "Custom/XrayShader"
{
	// Variables
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}


		_XColor("Xray Color", Color) = (1,1,1,1)
		_XTex("Xray Texture", 2D) = "white" {}


		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}

		// The Singular Funciton that the shader can have
			SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			// Normal
			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows

			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

				void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG

			   //XRay

		  ZTEST GREATER

			  CGPROGRAM
			   #pragma surface surf Unlit

			   sampler2D _XTex;

			   struct Input
			   {
				   float2 uv_MainTex;
			   };

			   fixed4 _XColor;

			   UNITY_INSTANCING_BUFFER_START(Props)
			   UNITY_INSTANCING_BUFFER_END(Props)

				half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
			   {
				   half4 col;
				   col.rgb = s.Albedo;
				   col.a = s.Alpha;

				   return col;
			   }

			   void surf(Input IN, inout SurfaceOutput o)
			   {
					   fixed4 c = tex2D(_XTex, IN.uv_MainTex) * _XColor;
				   o.Albedo = c.rgb;
				   o.Alpha = c.a;
			   }
			ENDCG
		}
			FallBack "Diffuse"
}
