Shader "Universal Render Pipeline/Toon Lit"
{
	Properties{

		// Specular vs Metallic workflow
		[HideInInspector] _WorkflowMode("WorkflowMode", Float) = 1.0

		[MainTexture] _BaseMap("Albedo Map", 2D) = "white" {}
		[MainColor] _BaseColor("Color", Color) = (0, 1, 1, 1)
		_ShadowMap("Shadow Map", 2D) = "white" {}
		_ShadowColor("Shadow Color", Color) = (1,1,1,1)
		_TexAddShadowStrength("Tex Add Shadow Strengh", Range(0,1)) = 0
		_ToonMaskMap("ToonMask Map", 2D) = "white" {}

		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_RoughnessMap("Roughness Map", 2D) = "white" {}
		_Roughness("Roughness", Range(0.0, 1.0)) = 0.5

		_Metallic("Metallic", Range(0.0, 1.0)) = 0.5
		_MetallicGlossMap("Metallic Gloss Map", 2D) = "white" {}

		_SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
		_SpecGlossMap("Spec Gloss Map", 2D) = "white" {}

		[Toggle] _UseToonSpec("Use Toon Specular", Float) = 0
		_ToonSpecMap("Toon Spec Map", 2D) = "white"{}
		_ToonSpecColor("Toon Spec Color", Color) = (1,1,1,1)
		_ToonSpecOptMap("Toon Spec Opt Map", 2D) = "white"{}
		_ToonSpecOptMapST("Toon Spec Opt Map ST", Vector) = (20,20,0,0)
		_ToonSpecGloss("Toon Spec Gloss", Range(0,1)) = 0.6
		_ToonSpecFeatherLevel("Toon Spec Feather Level", Range(0,1)) = 0.344
		_ToonSpecMaskScale("Toon Spec Mask Scale", Range(-0.5,0.5)) = 0.03
		_ToonSpecMaskOffset("Toon Spec Mask Offset", Range(-0.5,0.5)) = 0
		[Toggle] _UseToonHairSpec("Use Toon Hair Specular", Float) = 0
		_ToonSpecAnisoHighLightPower_1st("Toon Spec Aniso HighLight Power 1st(Hair Spec)", Range(0, 1000)) = 300
		_ToonSpecAnisoHighLightPower_2nd("Toon Spec Aniso HighLight Power 2nd(Hair Spec)", Range(0, 1000)) = 600
		_ToonSpecAnisoHighLightStrength_1st("Toon Spec Aniso HighLight Strength 1st(Hair Spec)", Range(0, 100)) = 1
		_ToonSpecAnisoHighLightStrength_2nd("Toon Spec Aniso HighLight Strength 2nd(Hair Spec)", Range(0, 100)) = 0.2
		_ToonSpecShiftTangent_1st("Toon Spec Shift Tangent 1st(Hair Spec)", Range(-10,10)) = 0
		_ToonSpecShiftTangent_2nd("Toon Spec Shift Tangent 2nd(Hair Spec)", Range(-10,10)) = 0

		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

		_OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion Map", 2D) = "white" {}

		//_EmissionColor("Emission Color", Color) = (0,0,0)
		//_EmissionMap("Emission Map", 2D) = "white" {}

		 // Blending state
		 _Surface("__surface", Float) = 0.0
		 _Blend("__blend", Float) = 0.0
		 _AlphaClip("__clip", Float) = 0.0
		 _SrcBlend("__src", Float) = 1.0
		 _DstBlend("__dst", Float) = 0.0
		 _ZWrite("__zw", Float) = 1.0
		 _Cull("__cull", Float) = 2.0

		 _ReceiveShadows("Receive Shadows", Float) = 1.0

		_ToonLightDivid_M("ToonLightDivid M", Range(-1.0,1.0)) = 0.5
		_ToonLightDivid_D("ToonLightDivid D", Range(-1.0,1.0)) = 0.2
		_ToonDiffuseBright("ToonDiffuseBrightness", Range(0.0,2.0)) = 0.0
		_BoundSharp("_BoundSharp", Range(0.2,5)) = 1
		_DarkFaceColor("ToonColor of Dark Face", Color) = (1.0, 1.0, 1.0, 1.0)
		_DeepDarkColor("ToonColor of Deep Dark Face", Color) = (1.0, 1.0, 1.0, 1.0)

		_SSSColor("Subsurface Scattering Color", Color) = (1,0,0,1)
		_SSSWeight("Weight of SSS", Range(0,1)) = 0.0
		_SSSSize("Size of SSS", Range(0.01,1)) = 0.5
		_SSForwardAtt("Atten of SS in forward Dir", Range(0,1)) = 0.5

		_ClearCoatMaskMap("ClearCoatMask Map", 2D) = "white" {}
		_ClearCoatColor("ClearCoatColor",Color) = (1,1,1, 1)
		_ClearCoatRange("ClearCoatRange", Range(0,1)) = 0.5
		_ClearCoatGloss("ClearCoatGloss", Range(0,1)) = 0.5
		_ClearCoatMult("ClearCoatMult", Range(0,1)) = 0.5

		_SpecularMaskMap("SpecularMask Map", 2D) = "white" {}
		_SpecularColor("Specular Color", Color) = (1,1,1)
		_SpecularRange("Specular Range",  Range(0, 1)) = 0.9
		_SpecularMulti("Specular Multi", Range(0, 1)) = 0.4
		_SpecularGloss("Sprecular Gloss", Range(0.001, 8)) = 4

		_ToonToPBRMap("Toon To PBR Map", 2D) = "white" {}
		_ToonToPBR("ToonToPBR", Range(0,1)) = 0

		_OutLineColor("OutLineColor", Color) = (0, 0, 0, 1)
		_OutLineThickness("OutLineThickness", Range(0,4)) = 1
	}
		SubShader{
			// Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
			// this Subshader will fail. One can add a subshader below or fallback to Standard built-in to make this
			// material work with both Universal Render Pipeline and Builtin Unity Pipeline
			Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
			LOD 300

			HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			CBUFFER_START(UnityPerMaterial)
			float _OutLineThickness;
			float4 _OutLineColor;
			//float _OutLineDistance_Far, _OutLineDistance_Near;

			float4 _BaseMap_ST, _ToonMaskMap_ST;
			float4 _BaseColor;
			float4 _ShadowColor;
			float _TexAddShadowStrength;
			float4 _SpecColor;
			float4 _ToonSpecColor;
			float4 _ToonSpecOptMapST;
			float _UseToonSpec, _ToonSpecGloss, _ToonSpecFeatherLevel, _ToonSpecMaskScale, _ToonSpecMaskOffset;
			float _UseToonHairSpec, _ToonSpecAnisoHighLightPower_1st, _ToonSpecAnisoHighLightPower_2nd, _ToonSpecAnisoHighLightStrength_1st, _ToonSpecAnisoHighLightStrength_2nd, _ToonSpecShiftTangent_1st, _ToonSpecShiftTangent_2nd;
			//float4 _EmissionColor;
			float _Cutoff;
			float _Roughness;
			float _Metallic;
			float _BumpScale;
			float _OcclusionStrength;
			float _Surface;

			float _ToonLightDivid_M, _ToonLightDivid_D, _ToonDiffuseBright, _BoundSharp, _ToonToPBR;
			float4 _DarkFaceColor, _DeepDarkColor;
			float4 _SSSColor;
			float _SSSWeight, _SSSSize, _SSForwardAtt;

			float3 _ClearCoatColor;
			float _ClearCoatRange;
			float _ClearCoatGloss;
			float _ClearCoatMult;

			float3 _SpecularColor;
			float _SpecularRange;
			float _SpecularMulti;
			float _SpecularGloss;


			CBUFFER_END
			ENDHLSL

			Pass
			{
				Name "OutLine"
				Tags{ "LightMode" = "ToonOutline" }
				Cull Front

				HLSLPROGRAM

				// Required to compile gles 2.0 with standard SRP library
				// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 3.0

				#pragma vertex vert
				#pragma fragment frag

				#include "Outline.hlsl"

				ENDHLSL
			}

				//ForwardLit
				Pass {
					Name "ForwardLit"
					Tags { "LightMode" = "UniversalForward" }

					Blend[_SrcBlend][_DstBlend]
					ZWrite[_ZWrite]
					Cull[_Cull]

					HLSLPROGRAM

				// Required to compile gles 2.0 with standard SRP library
				// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 3.0


				// -------------------------------------
				// Material Keywords
				#pragma shader_feature _NORMALMAP
				//#pragma shader_feature _ALPHATEST_ON
				#pragma shader_feature _ALPHAPREMULTIPLY_ON
				//#pragma shader_feature _EMISSION
				#pragma shader_feature _METALLICSPECGLOSSMAP
				//#pragma shader_feature _ROUGHNESS_TEXTURE_ALBEDO_CHANNEL_A
				#pragma shader_feature _OCCLUSIONMAP  

				//#pragma multi_compile _ _SPECULARHIGHLIGHTS_OFF
				//#pragma multi_compile _ _ENVIRONMENTREFLECTIONS_OFF
				#pragma multi_compile _ _SPECULAR_SETUP
				#pragma multi_compile _ _RECEIVE_SHADOWS_OFF
				#define _SPECULARHIGHLIGHTS_OFF
				#define _ENVIRONMENTREFLECTIONS_OFF

				// -------------------------------------
				// Universal Pipeline keywords
				#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
				#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
				#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
				#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
				#pragma multi_compile _ _SHADOWS_SOFT
				#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

				// -------------------------------------
				// Unity defined keywords
				//#pragma multi_compile _ DIRLIGHTMAP_COMBINED
				//#pragma multi_compile _ LIGHTMAP_ON
				#pragma multi_compile_fog 

				//--------------------------------------
				// GPU Instancing
				//#pragma multi_compile_instancing

				#pragma vertex vert
				#pragma fragment frag

				struct a2v {
					float4 color        : COLOR;
					float4 positionOS   : POSITION;
					float3 normalOS     : NORMAL;
					float4 tangentOS    : TANGENT;
					float2 texcoord     : TEXCOORD0;
					float2 lightmapUV   : TEXCOORD1;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float2 uv						 : TEXCOORD0;
					DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

				#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
					float3 positionWS				 : TEXCOORD2;
				#endif
					float3 normalWS					 : TEXCOORD3;
					//#ifdef _NORMALMAP
						float4 tangentWS				 : TEXCOORD4;    // xyz: tangent, w: sign。 现在有了头发高光，不管有没有法线贴图都需要用到tangent
					//#endif
						float3 viewDirWS				 : TEXCOORD5;
						half4 fogFactorAndVertexLight    : TEXCOORD6;	 // x: fogFactor, yzw: vertex light
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						float4 shadowCoord               : TEXCOORD7;
					#endif
						//float4 color				     : COLOR;
						float4 positionCS  : SV_POSITION;


						UNITY_VERTEX_INPUT_INSTANCE_ID
						UNITY_VERTEX_OUTPUT_STEREO
					};

					TEXTURE2D(_BaseMap);			SAMPLER(sampler_BaseMap);
					TEXTURE2D(_BumpMap);			SAMPLER(sampler_BumpMap);
					//TEXTURE2D(_EmissionMap);		SAMPLER(sampler_EmissionMap);
					TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
					TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
					TEXTURE2D(_RoughnessMap);		SAMPLER(sampler_RoughnessMap);

					TEXTURE2D(_ToonSpecMap);		SAMPLER(sampler_ToonSpecMap);
					TEXTURE2D(_ToonSpecOptMap);		SAMPLER(sampler_ToonSpecOptMap);
					TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);
					TEXTURE2D(_ShadowMap);			SAMPLER(sampler_ShadowMap);
					TEXTURE2D(_ToonToPBRMap);		SAMPLER(sampler_ToonToPBRMap);

					TEXTURE2D(_ClearCoatMaskMap);	SAMPLER(sampler_ClearCoatMaskMap);
					TEXTURE2D(_SpecularMaskMap);	SAMPLER(sampler_SpecularMaskMap);

					#include "Input.hlsl"
					#include "Lighting.hlsl" 

					v2f vert(a2v input) {
						v2f output = (v2f)0;;

						UNITY_SETUP_INSTANCE_ID(input);
						UNITY_TRANSFER_INSTANCE_ID(input, output);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

						VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

						// normalWS and tangentWS already normalize.
						// this is required to avoid skewing the direction during interpolation
						// also required for per-vertex lighting and SH evaluation
						VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

						float3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
						float3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
						float fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

						output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);

						// already normalized from normal transform to WS.
						output.normalWS = normalInput.normalWS;
						output.viewDirWS = viewDirWS;
						//#ifdef _NORMALMAP
							real sign = input.tangentOS.w * GetOddNegativeScale();
							output.tangentWS = float4(normalInput.tangentWS.xyz, sign);
							//#endif

								OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
								OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

								output.fogFactorAndVertexLight = float4(fogFactor, vertexLight);

							#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
								output.positionWS = vertexInput.positionWS;
							#endif

							#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
								output.shadowCoord = GetShadowCoord(vertexInput);
							#endif

								output.positionCS = vertexInput.positionCS;

								return output;
							}

							float4 frag(v2f input) : SV_Target {
								UNITY_SETUP_INSTANCE_ID(input);
								UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

								JASurfaceData surfaceData;
								InitializeStandardLitSurfaceData(input.uv, surfaceData);

								JAInputData inputData;
								InitializeInputData(input, surfaceData.normalTS, inputData);

								float4 color = JAUniversalFragmentPBR(inputData, surfaceData.albedo, surfaceData.shadowMap, surfaceData.metallic, surfaceData.specular, surfaceData.roughness, surfaceData.occlusion, surfaceData.emission, surfaceData.alpha);
								float4 outputColor = float4(1,1,1,1);
								outputColor.rgb = MixFog(color.rgb, inputData.fogCoord);
								outputColor.a = OutputAlpha(color.a, _Surface);
								return outputColor;
								/*
								Light mainLight = GetMainLight(inputData.shadowCoord);
								color = LightingToon(inputData, mainLight.direction);

			#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
								return float4(1, 0, 0, 1);
			#endif
								return color * float4(surfaceData.albedo, surfaceData.alpha);
								*/
							}
							ENDHLSL
						}

				//ShadowCaster
				Pass
				{
					Name "ShadowCaster"
					Tags{"LightMode" = "ShadowCaster"}

					ZWrite On
					ZTest LEqual
					ColorMask 0
					Cull[_Cull]

					HLSLPROGRAM
								// Required to compile gles 2.0 with standard srp library
								#pragma prefer_hlslcc gles
								#pragma exclude_renderers d3d11_9x
								#pragma target 3.0

								// -------------------------------------
								// Material Keywords
								#pragma shader_feature _ALPHATEST_ON

								//--------------------------------------
								// GPU Instancing
								#pragma multi_compile_instancing
								#pragma shader_feature _ROUGHNESS_TEXTURE_ALBEDO_CHANNEL_A

								#pragma vertex ShadowPassVertex
								#pragma fragment ShadowPassFragment

								TEXTURE2D(_BaseMap);			SAMPLER(sampler_BaseMap);
								TEXTURE2D(_BumpMap);			SAMPLER(sampler_BumpMap);
								//TEXTURE2D(_EmissionMap);		SAMPLER(sampler_EmissionMap);
								TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
								TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
								TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);

								#include "BaseCalculates.hlsl"
								#include "ShadowCasterPass.hlsl"
								ENDHLSL
							}

								//DepthOnly
								Pass
								{
									Name "DepthOnly"
									Tags{"LightMode" = "DepthOnly"}

									ZWrite On
									ColorMask 0
									Cull[_Cull]

									HLSLPROGRAM
									// Required to compile gles 2.0 with standard srp library
									#pragma prefer_hlslcc gles
									#pragma exclude_renderers d3d11_9x
									#pragma target 3.0

									#pragma vertex DepthOnlyVertex
									#pragma fragment DepthOnlyFragment

									// -------------------------------------
									// Material Keywords
									 #pragma shader_feature _ALPHATEST_ON
									#pragma multi_compile _ROUGHNESS_TEXTURE_ALBEDO_CHANNEL_A

									//--------------------------------------
									// GPU Instancing
									#pragma multi_compile_instancing

									TEXTURE2D(_BaseMap);			SAMPLER(sampler_BaseMap);
									TEXTURE2D(_BumpMap);			SAMPLER(sampler_BumpMap);
									//TEXTURE2D(_EmissionMap);		SAMPLER(sampler_EmissionMap);
									TEXTURE2D(_MetallicGlossMap);   SAMPLER(sampler_MetallicGlossMap);
									TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);
									TEXTURE2D(_OcclusionMap);       SAMPLER(sampler_OcclusionMap);

									#include "BaseCalculates.hlsl"
									#include "DepthOnlyPass.hlsl"
									ENDHLSL
								}
		}

			CustomEditor "ToonLitURPEditor"
}
