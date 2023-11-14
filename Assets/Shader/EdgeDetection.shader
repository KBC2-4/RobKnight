Shader "Hidden/EdgeDetection"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float3x3 sobelX = float3x3(-1, 0, 1, -2, 0, 2, -1, 0, 1);
                float3x3 sobelY = float3x3(-1, -2, -1, 0, 0, 0, 1, 2, 1);

                float edgeX = 0;
                float edgeY = 0;

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        float4 sample = tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(x, y));
                        edgeX += sample.r * sobelX[x + 1][y + 1];
                        edgeY += sample.r * sobelY[x + 1][y + 1];
                    }
                }
        
                float edge = sqrt(edgeX * edgeX + edgeY * edgeY);
                return fixed4(edge, edge, edge, 1);
            }
            ENDCG
        }
    }
}
