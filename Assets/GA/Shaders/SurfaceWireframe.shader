Shader "Custom/Wireframe2"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex VSMain
            #pragma geometry GSMain
            #pragma fragment PSMain
            #pragma target 5.0

            struct Data
            {
                float4 vertex : SV_Position;
                float3 normal : NORMAL;
            };

            float3 GenerateNormal(float3 v1, float3 v2, float3 v3)
            {
                return normalize(cross(v2 - v1, v3 - v1));
            }

            void VSMain(inout float4 vertex:POSITION) { }

            [maxvertexcount(3)]
            void GSMain(triangle float4 patch[3]:SV_Position, inout TriangleStream<Data> stream)
            {
                Data GS;
                float3 trianglenormal = GenerateNormal(patch[0].xyz, patch[1].xyz, patch[2].xyz);
                for (uint i = 0; i < 3; i++)
                {
                    GS.vertex = UnityObjectToClipPos(patch[i]);
                    GS.normal = trianglenormal;
                    stream.Append(GS);
                }
                stream.RestartStrip();
            }

            float4 PSMain(Data PS) : SV_Target
            {
                return float4(PS.normal, 1.0);
            }
            ENDCG
        }

        GrabPass {"_BackgroundTexture"}

        Pass
        {
            CGPROGRAM
            #pragma vertex VSMain
            #pragma geometry GSMain
            #pragma fragment PSMain
            #pragma target 5.0

            sampler2D _BackgroundTexture;
            float4 _BackgroundTexture_TexelSize;

            struct Data { float4 vertex : SV_Position; };

            void VSMain(inout float4 vertex:POSITION) { }

            [maxvertexcount(3)]
            void GSMain(triangle float4 patch[3]:SV_Position, inout TriangleStream<Data> stream)
            {
                Data GS;
                for (uint i = 0; i < 3; i++)
                {
                    GS.vertex = UnityObjectToClipPos(patch[i]);
                    stream.Append(GS);
                }
                stream.RestartStrip();
            }

            float3 blur(float2 uv, float radius)
            {
                float2x2 m = float2x2(-0.736717, 0.6762, -0.6762, -0.736717);
                float3 total = 0..xxx;
                float2 texel = float2(0.002 * _BackgroundTexture_TexelSize.z / _BackgroundTexture_TexelSize.w, 0.002);
                float2 angle = float2(0.0,radius);
                radius = 1.0;
                for (int j = 0; j < 64; j++)
                {
                    radius += rcp(radius);
                    angle = mul(angle, m);
                    float3 color = tex2D(_BackgroundTexture, uv + texel * (radius - 1.0) * angle).rgb;
                    total += color;
                }
                return total / 64.0;
            }

            float4 PSMain(Data PS) : SV_Target
            {
                float3 color = blur(PS.vertex.xy / _ScreenParams.xy, 0.05);
                float3 value = smoothstep(0.,50., abs(color) / fwidth(color));
                return float4(min(min(value.x, min(value.y, value.z)).xxx , abs(color)), 1.0);
            }
            ENDCG
        }
    }
}