Shader "Unlit/WireframeMat"
{
    Properties
    {
        _LineColor("Line Color", Color) = (1, 1, 1, 1)
        _SurfaceColor("Surface Color", Color) = (0, 0, 0, 0)
        _LineWidth("Line Width", Float) = 1
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct vertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct vertexOutput
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color: COLOR;
            };

            fixed4 _LineColor;
            fixed4 _SurfaceColor;
            fixed _LineWidth;

            vertexOutput vert (vertexInput input)
            {
                vertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            fixed4 frag (vertexOutput vert) : Color
            {
                // get the fragment's width (i think ?)
                float2 d = fwidth(vert.uv);

                // interpolate between 0 and vert pos (?) * line width, based on the vert's position (?)
                float lineY = smoothstep(float(0), d.y * _LineWidth, vert.uv.y);
                float lineX = smoothstep(float(0), d.x * _LineWidth, vert.uv.x);

                // interpolate between 0 and 
                float diagonal = smoothstep(float(0), fwidth(vert.uv.x - vert.uv.y) * _LineWidth, vert.uv.x - vert.uv.y);

                float4 color = lerp(_LineColor, _SurfaceColor, lineX * lineY * _LineWidth);
                return color;
            }
            ENDCG
        }
    }
}
