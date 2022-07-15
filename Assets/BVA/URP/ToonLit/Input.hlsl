// #pragma once is a safe guard best practice in almost every .hlsl (need Unity2020 or up), 
// doing this can make sure your .hlsl's user can include this .hlsl anywhere anytime without producing any multi include conflict
//#pragma once

#include "BaseCalculates.hlsl"

struct JASurfaceData
{
    float3 albedo;
    float4 shadowMap;
    float3 specular;
    float  metallic;
    float  roughness;
    float3 normalTS;
    float3 emission;
    float  occlusion;
    float  alpha;
};
struct JAInputData
{
    float2  uv;
    float3  positionWS;
    float3  bitangent;
    float3  normalWS_Origin;
    float3  normalWS;
    float3  viewDirectionWS;
    float4  shadowCoord;
    float   fogCoord;
    float3  vertexLighting;
    float3  bakedGI;
};
///////////////////////////////////////////////////////////////////////////////
//                      Material Property Helpers                            //
///////////////////////////////////////////////////////////////////////////////


float3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), float scale = 1.0h)
{
#ifdef _NORMALMAP
    float4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
#if BUMP_SCALE_NOT_SUPPORTED
    return UnpackNormal(n);
#else
    return UnpackNormalScale(n, scale);
#endif
#else
    return float3(0.0h, 0.0h, 1.0h);
#endif
}
#ifdef _SPECULAR_SETUP
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_SpecGlossMap, sampler_SpecGlossMap, uv)
#else
    #define SAMPLE_METALLICSPECULAR(uv) SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv)
#endif

float4 SampleMetallicSpecGloss(float2 uv, float albedoAlpha)
{
    float4 specGloss;
    float roughness = SAMPLE_TEXTURE2D(_RoughnessMap, sampler_RoughnessMap, uv).r * _Roughness;
    //如果定义了_METALLICSPECGLOSSMAP，表示specular或者metallic的贴图是有的。
#ifdef _METALLICSPECGLOSSMAP
    specGloss = SAMPLE_METALLICSPECULAR(uv);
    #ifdef _ROUGHNESS_TEXTURE_ALBEDO_CHANNEL_A
        specGloss.a = albedoAlpha * (1.0 - roughness);
    #else
        specGloss.a *= (1.0-roughness);
    #endif
#else // _METALLICSPECGLOSSMAP
    #if _SPECULAR_SETUP
        specGloss.rgb = _SpecColor.rgb;
    #else
        specGloss.rgb = _Metallic.rrr;
    #endif

    #ifdef _ROUGHNESS_TEXTURE_ALBEDO_CHANNEL_A
        specGloss.a = albedoAlpha * (1.0 - roughness);
    #else
        specGloss.a = 1.0 - roughness;
    #endif
#endif

    return specGloss;
}
float3 SampleEmission(float2 uv, float3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
{
#ifndef _EMISSION
    return 0;
#else
    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
#endif
}

float SampleOcclusion(float2 uv)
{
#ifdef _OCCLUSIONMAP
// TODO: Controls things like these by exposing SHADER_QUALITY levels (low, medium, high)
// JTAOO: SHADER_API_GLES是一个渲染平台差异问题，暂时不清楚是干什么的, 先跟着URP的Lit抄下来。
#if defined(SHADER_API_GLES)
    return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
#else
    float occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
    return LerpWhiteTo(occ, _OcclusionStrength);
#endif
#else
    return 1.0;
#endif
}


inline void InitializeStandardLitSurfaceData(float2 uv, out JASurfaceData outSurfaceData)
{
    float4 albedoAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

    float4 specGloss = SampleMetallicSpecGloss(uv, albedoAlpha.a);
    outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
    outSurfaceData.shadowMap = SAMPLE_TEXTURE2D(_ShadowMap, sampler_ShadowMap, uv).rgba;
    
#if _SPECULAR_SETUP
    outSurfaceData.metallic = 1.0h;
    outSurfaceData.specular = specGloss.rgb;
#else
    outSurfaceData.metallic = specGloss.r;
    outSurfaceData.specular = float3(0.0h, 0.0h, 0.0h);
#endif

    outSurfaceData.roughness = specGloss.a;
    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
    outSurfaceData.occlusion = SampleOcclusion(uv);
    outSurfaceData.emission = float3(0.0h, 0.0h, 0.0h); //SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
    
}

void InitializeInputData(v2f input, float3 normalTS, out JAInputData inputData)
{
    inputData = (JAInputData)0;
    inputData.uv = input.uv;
#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    inputData.positionWS = input.positionWS;
#endif

    float3 viewDirWS = SafeNormalize(input.viewDirWS);

    inputData.normalWS_Origin = input.normalWS.xyz;
     
    float sgn = input.tangentWS.w;      // should be either +1 or -1
    float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
    inputData.bitangent = bitangent;
#ifdef _NORMALMAP
    inputData.normalWS = TransformTangentToWorld(normalTS, float3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
#else
    inputData.normalWS = input.normalWS;
#endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    inputData.viewDirectionWS = viewDirWS;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    inputData.shadowCoord = input.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
#else
    inputData.shadowCoord = float4(0, 0, 0, 0);
#endif

    inputData.fogCoord = input.fogFactorAndVertexLight.x;
    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
    inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
}



