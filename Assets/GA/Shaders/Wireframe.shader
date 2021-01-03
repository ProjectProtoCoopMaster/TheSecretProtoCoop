Shader "Unlit/Wireframe"
{
    // exposed parameters for the material inspector
    Properties
    {
        _LineColor("Line Color", Color) = (1, 1, 1, 1)
        _SurfaceColor("Surface Color", Color) = (1, 1, 1, 1)
        _LineWidth("Line Width", Float) = 1
        _OtherLine("Other Line", Float) = 0.1
    }
    SubShader
    {
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vin_vct
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f_vct
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            // re-declaration of the material inspector values
            fixed4 _LineColor;
            fixed4 _SurfaceColor;
            half _LineWidth;
            half _OtherLine;

            v2f_vct vert (vin_vct input)
            {
                v2f_vct output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            half4 frag(v2f_vct i) : COLOR
            {/*
                float2 d = fwidth(i.uv);

                float lineY = step(d.y * _LineWidth, i.uv.y);
                float lineX = step(d.x * _LineWidth, i.uv.x);

                float diagonal = step(_OtherLine, i.uv.x - i.uv.y);

                float4 color = lerp(_LineColor, _SurfaceColor, diagonal * lineX * lineY);*/
                return (1, 1, 1, 1);
            }
            ENDCG
        }
    }
}
