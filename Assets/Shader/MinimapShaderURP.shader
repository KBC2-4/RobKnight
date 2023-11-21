    Shader "Custom/MinimapShaderURP" {
        Properties {
            _MainTex ("Main Texture", 2D) = "white" {}
            _MainTexRatio("Main Texture Ratio", Range(0, 1)) = 1
            _BaseColor ("Base Color", Color) = (0,0,0,1)
            _MapColor ("Map Color", Color) = (1,1,1,1)
            _MapMagnification ("Map Magnification", Range(0.1, 100000)) = 10000
            _Thick("Thick", Range(0.1, 5)) = 1
        }

        SubShader {
            Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
            ZTest Off
            ZWrite Off
            Lighting Off
            AlphaTest Off

            Pass {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                // sampler2D _MainTex;
                float _MainTexRatio;
                float4  _MainTex_ST;
                float _MapMagnification;
                float4 _MapColor;
                float4 _BaseColor;
                float _Thick;

                sampler2D _CameraDepthTexture;
                float4  _CameraDepthTexture_TexelSize;

                // テクスチャとサンプラーの宣言
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);

                struct Attributes {
                    float4 position : POSITION;
                    float2 uv : TEXCOORD0;
                };
                struct Varyings {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                Varyings vert(Attributes IN) {
                    Varyings OUT;
                    OUT.position = TransformObjectToHClip(IN.position.xyz);
                    OUT.uv = IN.uv;
                    return OUT;
                }

                half4 frag(Varyings IN) : SV_Target {
                    // half4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                    // half4 mainColor = _MainTex.Sample(_MainTexSampler, IN.uv);
                    half4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                    // 深度テクスチャとその他のエフェクト

                    // 簡単な色の混合
                    half4 finalColor = lerp(_BaseColor, mainColor, _MainTexRatio);
                    return finalColor;
                }
                ENDHLSL
            }
        } 
    }
