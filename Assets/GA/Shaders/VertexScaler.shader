Shader "Unlit/VertexScaler"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SurfaceColor("Surface Color", Color) = (.5, .5, .5, 1.0)
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Range(0, 5)) = 1.1
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 pos : POSITION;
        float3 normal : NORMAL;
    };

    float4 _OutlineWidth;
    float4 _OutlineColor;
    float4 _SurfaceColor;

    v2f vert (appdata v)
    {
		v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
    }
    ENDCG

    SubShader
    {
        Tags { "Queue" = "Transparent"}
        LOD 3000

        Pass // Render the Outline
        {
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			half4 frag(v2f i) :COLOR { return _OutlineColor; }

            ENDCG
        }

        Pass // Render the base Object on top
        {
			ZWrite On

            Material
            {
                Diffuse[_SurfaceColor]
                Ambient[_SurfaceColor]
            }

			SetTexture [_MainTex] 
            {
                ConstantColor[_SurfaceColor]
            }

			SetTexture [_MainTex] 
            {
               Combine previous * primary DOUBLE
            }
        }
    }
}
