Shader "Hidden/EncodeLightmap" {
	Properties{
		_MainTex("Base (HDR RT)", 2D) = "black" {}
		_FlipY("Flip texture Y", Int) = 0
		_Rgbd("Encode RGBD", Int) = 0
	}
		Subshader{
			ZTest Always Cull Off ZWrite Off lighting off
			Fog { Mode off }
			Pass {
				HLSLPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0 
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

				struct vertInput
				{
					float4 pos : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct vertOutput
				{
					float4 pos : SV_POSITION;
					float2 texcoord : TEXCOORD0;
				};

				sampler2D _MainTex;
				int _FlipY;
				int _MODE;
				static const float gamma_value = 2.2;
				static const float gamma_max_brightness = 5;
				static const float linear_max_brightness = pow(gamma_max_brightness, gamma_value);

				inline float4 EncodeDldr(float4 col)
				{
					// double LDR conversion
					float4 dldr;
					dldr.rgb = saturate(col.rgb * 0.5);
					dldr.a = 1;

					return dldr;
				}

				inline float4 DecodeDldr(float4 dldr)
				{
					// double LDR decode
					float4 col;
					col.rgb = dldr.rgb * 2.0;
					col.a = 1;

					return col;
				}

				inline float4 EncodeRgbm(float4 col)
				{
					// Encode to RGBM
					// Brightness to Alpha: f(x) = pow(x / pow(5, 2.2), 1 / 2.2)
					float brightness = clamp(max(col.r, max(col.g, col.b)), 0, linear_max_brightness);
					float multiplier = pow(brightness / linear_max_brightness, 1 / gamma_value);
					float quantized_multiplier = max(1, ceil(multiplier * 255.0)) / 255.0; // 8bit ¤Ç±í¬F¤Ç¤­¤ë‚Ž¤ËÍè¤á¤ë
					float quantized_brightness = linear_max_brightness * pow(quantized_multiplier, gamma_value);

					float4 rgbm;
					rgbm.rgb = col.rgb / quantized_brightness;
					rgbm.a = quantized_multiplier;

					return rgbm;
				}

				inline float4 DecodeRgbm(float4 rgbm)
				{
					// Decode RGBM
					// Alpha To Brightness: f(x) = pow(5, 2.2) * pow(x, 2.2)
					float4 col;
					col.rgb = linear_max_brightness * pow(rgbm.a, gamma_value) * rgbm.rgb;
					col.a = 1;

					return col;
				}
				vertOutput vert(vertInput input)
				{
					vertOutput o;
					o.pos = GetVertexPositionInputs(input.pos).positionCS;
					o.texcoord.x = input.texcoord.x;
					if (_FlipY == 1) o.texcoord.y = (1.0 - input.texcoord.y);
					else o.texcoord.y = input.texcoord.y;
					return o;
				}

				float4 frag(vertOutput output) : COLOR {
					float4 result = tex2D(_MainTex, output.texcoord);
					// ..
					 //#if defined(UNITY_LIGHTMAP_DLDR_ENCODING)
					 //#elif defined(UNITY_LIGHTMAP_RGBM_ENCODING)
					 //#elif defined(UNITY_LIGHTMAP_FULL_HDR)
					if (_MODE == 0) {
						//result.rgb = DecodeLightmap(result, float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
						result = EncodeRgbm(result);
					}
					else if (_MODE == 1) {
						//result.rgb = DecodeLightmap(result, float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
						result = EncodeDldr(result);
					}
					else if (_MODE == 2) {
						result = DecodeRgbm(result);
					}
					else if (_MODE == 3) {
						//result.rgb = DecodeLightmap(result, float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
						result = DecodeDldr(result);
					}
					 //#endif
					// ..
					return result;
				}
				ENDHLSL
			}
		}
			Fallback off
}