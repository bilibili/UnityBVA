// #pragma once is a safe guard best practice in almost every .hlsl (need Unity2020 or up), 
// doing this can make sure your .hlsl's user can include this .hlsl anywhere anytime without producing any multi include conflict
//#pragma once


struct JABRDFData
{
    float toonToPBR;
    float3 toonDiffuse;
    float3 toonShadow;
    float3 diffuse;
    float3 diffuseShadow;
    float3 specular;
    float perceptualRoughness;//perceptualRoughness = 1-smoothness
    float roughness;//roughness = perceptualRoughness * perceptualRoughness
    float roughness2;//roughness2 = roughness * roughness = perceptualRoughness ^ 4
    float grazingTerm;//grazingTerm = saturate(smoothness + reflectivity)

    // We save some light invariant BRDF terms so we don't have to recompute
    // them in the light loop. Take a look at DirectBRDF function for detailed explaination.
    float normalizationTerm;     // roughness * 4.0 + 2.0
    float roughness2MinusOne;    // roughness^2 - 1.0
};

float3 JAReflectivitySpecular(float3 specular)
{
#if defined(SHADER_API_GLES)
    return specular.r; // Red channel - because most metals are either monocrhome or with redish/yellowish tint
#else
    return max(max(specular.r, specular.g), specular.b); 
#endif
}

float JAOneMinusReflectivityMetallic(float metallic)
{
    // We'll need oneMinusReflectivity, so
    //   1-reflectivity = 1-lerp(dielectricSpec, 1, metallic) = lerp(1-dielectricSpec, 0, metallic)
    // store (1-dielectricSpec) in kDieletricSpec.a, then
    //   1-reflectivity = lerp(alpha, 0, metallic) = alpha + metallic*(0 - alpha) =
    //                  = alpha - metallic * alpha
    float oneMinusDielectricSpec = kDieletricSpec.a;//kDieletricSpec.a = 0.96
    return oneMinusDielectricSpec - metallic * oneMinusDielectricSpec;
}



inline void InitializeJABRDFData(JAInputData inputData, float3 albedo, float3 shadowColor, float metallic, float3 specular, float smoothness, float alpha, out JABRDFData outBRDFData)
{
    outBRDFData.toonToPBR = SAMPLE_TEXTURE2D(_ToonToPBRMap, sampler_ToonToPBRMap, inputData.uv).r * _ToonToPBR;
//如果使用的wolkflow是specular颜色的
#ifdef _SPECULAR_SETUP
    float reflectivity = JAReflectivitySpecular(specular);//JAReflectivitySpecular(): 只使用specular颜色rgb通道中最明显的那个通道作为反射度。
    float oneMinusReflectivity = 1.0 - reflectivity;

    outBRDFData.diffuse = albedo * (float3(1.0h, 1.0h, 1.0h) - specular);
    outBRDFData.diffuseShadow = shadowColor * (float3(1.0h, 1.0h, 1.0h) - specular);
    outBRDFData.specular = specular;
#else
//如果使用的wolkflow是metallic的
    float oneMinusReflectivity = JAOneMinusReflectivityMetallic(metallic);
    float reflectivity = 1.0 - oneMinusReflectivity;

    outBRDFData.diffuse = albedo * oneMinusReflectivity;
    outBRDFData.diffuseShadow = shadowColor * oneMinusReflectivity;
    outBRDFData.specular = lerp(kDieletricSpec.rgb, albedo, metallic);
#endif
    //toon diffuse & shadow Color
    outBRDFData.toonDiffuse = albedo;
    outBRDFData.toonShadow = shadowColor;

    outBRDFData.grazingTerm = saturate(smoothness + reflectivity);
    outBRDFData.perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(smoothness);// 1 - smoothness
    outBRDFData.roughness = max(PerceptualRoughnessToRoughness(outBRDFData.perceptualRoughness), HALF_MIN);//PerceptualRoughnessToRoughness(a)就是a*a, HALF_MIN = 6.103515625e-5  // 2^-14, the same value for 10, 11 and 16-bit: https://www.khronos.org/opengl/wiki/Small_Float_Formats
    outBRDFData.roughness2 = outBRDFData.roughness * outBRDFData.roughness;

    outBRDFData.normalizationTerm = outBRDFData.roughness * 4.0h + 2.0h;
    outBRDFData.roughness2MinusOne = outBRDFData.roughness2 - 1.0h;

#ifdef _ALPHAPREMULTIPLY_ON
    outBRDFData.diffuse *= alpha;
    alpha = alpha * oneMinusReflectivity + reflectivity;
#endif
}

float3 JAEnvironmentBRDF(JABRDFData brdfData, float3 indirectDiffuse, float3 indirectSpecular, float fresnelTerm)
{ 
    //float3 c = indirectDiffuse * brdfData.diffuse;  
    float3 c = indirectDiffuse * brdfData.toonDiffuse;   
    float surfaceReduction = 1.0 / (brdfData.roughness2 + 1.0);
    c += surfaceReduction * indirectSpecular * lerp(brdfData.specular, brdfData.grazingTerm, pow(fresnelTerm, 4)); 
    //return pow(fresnelTerm, 10); 
    return c;
}

float3 JAGlossyEnvironmentReflection(float3 reflectVector, float perceptualRoughness, float occlusion)
{
#if !defined(_ENVIRONMENTREFLECTIONS_OFF)
    float mip = PerceptualRoughnessToMipmapLevel(perceptualRoughness);
    float4 encodedIrradiance = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, reflectVector, mip); 
#if !defined(UNITY_USE_NATIVE_HDR)
    float3 irradiance = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
#else
    float3 irradiance = encodedIrradiance.rgb;
#endif

    return irradiance * occlusion;
#endif // GLOSSY_REFLECTIONS

    return _GlossyEnvironmentColor.rgb * occlusion;
}

float3 EnvBRDF(float3 specColor, float roughness, float NdotV) 
{
	// [ Lazarov 2013, "Getting More Physical in Call of Duty: Black Ops II" ]
	// Adaptation to fit our G term.
	const float4 c0 = { -1, -0.0275, -0.572, 0.022 };
	const float4 c1 = { 1, 0.0425, 1.04, -0.04 };
	float4 r = roughness * c0 + c1;
	float a004 = min( r.x * r.x, exp2( -9.28 * NdotV ) ) * r.x + r.y;
	float2 AB = float2( -1.04, 1.04 ) * a004 + r.zw;

	// Anything less than 2% is physically impossible and is instead considered to be shadowing
	// Note: this is needed for the 'specular' show flag to work, since it uses a SpecularColor of 0
	AB.y *= saturate( 50.0 * specColor.g );

	return specColor * AB.x + AB.y;
}
float GetSpecularOcclusion(float metallic, float roughness, float occlusion)
{
	return lerp(occlusion, 1.0, metallic * (1.0 - roughness) * (1.0 - roughness));
}
float DielectricSpecularToF0(float specular)
{
	return 0.08 * specular;
}

float3 ComputeF0(float3 baseColor, float metallic)
{
	//return lerp(DielectricSpecularToF0(specular).xxx, baseColor, metallic);
    return lerp(DielectricSpecularToF0(0.5).xxx, baseColor, metallic);
}
float ComputeEnvMapMipFromRoughness(float roughness)
{
	float perceptualRoughness = roughness;
    perceptualRoughness = perceptualRoughness * (1.7 - 0.7 * perceptualRoughness);
	return perceptualRoughness * 6;
}
// Decodes HDR textures
// handles dLDR, RGBM formats
inline float3 DecodeHDR (float4 data, float4 decodeInstructions)
{
    // Take into account texture alpha if decodeInstructions.w is true(the alpha value affects the RGB channels)
    float alpha = decodeInstructions.w * (data.a - 1.0) + 1.0;

    // If Linear mode is not supported we can skip exponent part
    #if defined(UNITY_COLORSPACE_GAMMA)
        return (decodeInstructions.x * alpha) * data.rgb;
    #else
    #   if defined(UNITY_USE_NATIVE_HDR)
            return decodeInstructions.x * data.rgb; // Multiplier for future HDRI relative to absolute conversion.
    #   else
            return (decodeInstructions.x * pow(abs(alpha), decodeInstructions.y)) * data.rgb;
    #   endif
    #endif
}
float3 JAGlobalIllumination(JABRDFData brdfData, float3 bakedGI, float occlusion, float3 normalWS, float3 viewDirectionWS, float metallic)
{
    float3 reflectVector = reflect(-viewDirectionWS, normalWS);//简单计算反射反向
    float nv = saturate(dot(normalWS, viewDirectionWS));
    float fresnelTerm = Pow4(1.0 - nv);

    float3 indirectDiffuse = bakedGI * occlusion * brdfData.diffuse;

    float3 specColor = ComputeF0(brdfData.toonDiffuse, metallic);
    float specOcclusion = GetSpecularOcclusion(metallic, brdfData.perceptualRoughness, occlusion);
    float envMip = ComputeEnvMapMipFromRoughness(brdfData.perceptualRoughness); 
    float4 rgbm = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, reflectVector, envMip); 
    float3 envMap = DecodeHDR(rgbm, unity_SpecCube0_HDR); 
    float3 indirectSpecular = envMap * specOcclusion * EnvBRDF(specColor, brdfData.perceptualRoughness, nv);
    //float3 indirectSpecular = JAGlossyEnvironmentReflection(reflectVector, lerp(1, brdfData.perceptualRoughness, 1/*brdfData.toonToPBR*/), occlusion);//perceptualRoughness=1.0-smoothness
    
    //float3 out1 =  JAEnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm * 1/*brdfData.toonToPBR*/); 
    float3 out1 = indirectDiffuse + indirectSpecular;
    return lerp(indirectDiffuse * brdfData.toonDiffuse * _ShadowColor.rgb * 0, out1, brdfData.toonToPBR);
}  

// Based on Minimalist CookTorrance BRDF
// Implementation is slightly different from original derivation: http://www.thetenthplanet.de/archives/255
//
// * NDF [Modified] GGX
// * Modified Kelemen and Szirmay-Kalos for Visibility term
// * Fresnel approximated with 1/LdotH
float3 JADirectBDRF(JABRDFData brdfData, float3 normalWS, float3 lightDirectionWS, float3 viewDirectionWS)
{
#ifndef _SPECULARHIGHLIGHTS_OFF
    float3 halfDir = SafeNormalize(float3(lightDirectionWS) + float3(viewDirectionWS));//SafeNormalize让数值不会出现除以零的危险

    float NoH = saturate(dot(normalWS, halfDir));
    float LoH = saturate(dot(lightDirectionWS, halfDir));

    // GGX Distribution multiplied by combined approximation of Visibility and Fresnel
    // BRDFspec = (D * V * F) / 4.0
    // D = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2
    // V * F = 1.0 / ( LoH^2 * (roughness + 0.5) )
    // See "Optimizing PBR for Mobile" from Siggraph 2015 moving mobile graphics course
    // https://community.arm.com/events/1155

    // Final BRDFspec = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2 * (LoH^2 * (roughness + 0.5) * 4.0)
    // We further optimize a few light invariant terms
    // brdfData.normalizationTerm = (roughness + 0.5) * 4.0 rewritten as roughness * 4.0 + 2.0 to a fit a MAD.
    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;

    float LoH2 = LoH * LoH;
    float specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);

    // On platforms where half actually means something, the denominator has a risk of overflow
    // clamp below was added specifically to "fix" that, but dx compiler (we convert bytecode to metal/gles)
    // sees that specularTerm have only non-negative terms, so it skips max(0,..) in clamp (leaving only min(100,...))
#if defined (SHADER_API_MOBILE) || defined (SHADER_API_SWITCH)
    specularTerm = specularTerm - HALF_MIN;
    specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
#endif

    float3 color = specularTerm * brdfData.specular + brdfData.diffuse;
    return color;
#else
    return brdfData.diffuse;
#endif
}

float3 JALightingPhysicallyBased(JABRDFData brdfData, float3 lightColor, float3 lightDirectionWS, float lightAttenuation, float3 normalWS, float3 viewDirectionWS)
{
    float NdotL = saturate(dot(normalWS, lightDirectionWS)); 
    float3 radiance = lightColor * (lightAttenuation * NdotL);  
    return JADirectBDRF(brdfData, normalWS, lightDirectionWS, viewDirectionWS) * radiance;
}
float3 JALightingPhysicallyBased(JABRDFData brdfData, Light light, float3 normalWS, float3 viewDirectionWS)
{
    return JALightingPhysicallyBased(brdfData, light.color, light.direction, light.distanceAttenuation * light.shadowAttenuation, normalWS, viewDirectionWS);
}

float warp(float x, float w) {
	return (x + w) / (1 + w);
}

float3 warp(float3 x, float3 w) {
	return (x + w) / (float3(1.0,1.0,1.0) + w);
}

float Pow2(float x) {
	return x * x;
}

float Gaussion(float x, float center, float var) {
	return pow(2.718, -1 * Pow2(x - center) / var);
}

float ndc2Normal(float x) {
	return x * 0.5 + 0.5;
}

//将颜色二值化，并通过调整sharp得到有过渡的效果或者进行一定程度的抗锯齿
float sigmoid(float x, float center, float sharp) {
    float s;
    s = 1 / (1 + pow(100000, (-3 * sharp * (x - center))));
    return s;
}
// Based on Minimalist CookTorrance BRDF
// Implementation is slightly different from original derivation: http://www.thetenthplanet.de/archives/255
//
// * NDF [Modified] GGX
// * Modified Kelemen and Szirmay-Kalos for Visibility term
// * Fresnel approximated with 1/LdotH
float3 JADirectBDRF_Toon(JABRDFData brdfData, float3 normalWS, float3 lightDirectionWS, float3 viewDirectionWS)
{
#ifndef _SPECULARHIGHLIGHTS_OFF
    float3 halfDir = SafeNormalize(float3(lightDirectionWS) + float3(viewDirectionWS));//SafeNormalize让数值不会出现除以零的危险

    float NoH = saturate(dot(normalWS, halfDir));
    float LoH = saturate(dot(lightDirectionWS, halfDir));

    // GGX Distribution multiplied by combined approximation of Visibility and Fresnel
    // BRDFspec = (D * V * F) / 4.0
    // D = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2
    // V * F = 1.0 / ( LoH^2 * (roughness + 0.5) )
    // See "Optimizing PBR for Mobile" from Siggraph 2015 moving mobile graphics course
    // https://community.arm.com/events/1155

    // Final BRDFspec = roughness^2 / ( NoH^2 * (roughness^2 - 1) + 1 )^2 * (LoH^2 * (roughness + 0.5) * 4.0)
    // We further optimize a few light invariant terms
    // brdfData.normalizationTerm = (roughness + 0.5) * 4.0 rewritten as roughness * 4.0 + 2.0 to a fit a MAD.
    float d = NoH * NoH * brdfData.roughness2MinusOne + 1.00001f;

    float LoH2 = LoH * LoH;
    float specularTerm = brdfData.roughness2 / ((d * d) * max(0.1h, LoH2) * brdfData.normalizationTerm);

    // On platforms where half actually means something, the denominator has a risk of overflow
    // clamp below was added specifically to "fix" that, but dx compiler (we convert bytecode to metal/gles)
    // sees that specularTerm have only non-negative terms, so it skips max(0,..) in clamp (leaving only min(100,...))
#if defined (SHADER_API_MOBILE) || defined (SHADER_API_SWITCH)
    specularTerm = specularTerm - HALF_MIN;
    specularTerm = clamp(specularTerm, 0.0, 100.0); // Prevent FP16 overflow on mobiles
#endif

    float3 color = specularTerm * brdfData.specular;
    color = color + brdfData.diffuse;// 
    return color;
#else
    return brdfData.diffuse;
#endif
}
 
 //用于计算头发高光
inline float StrandSpecular(float3 T, float3 V, float3 L, float exponent, float strength)
{
    float3 H = normalize(L + V);
    float dotTH = dot(T, H);
    float sinTH = sqrt(1.0 - dotTH * dotTH);
    float dirAtten = smoothstep(-1.0, 0.0, dotTH);
    return dirAtten * pow(sinTH, exponent) * strength;
}
//计算卡通高光
float3 JAToonCalcSpecular(float2 uv, float3 lightColor, float3 lightDirectionWS, float3 normalWS, float3 bitangent, float3 viewDirectionWS, float toonSpecMult)
{
    float3 finalSpec;
    float4 specMapColor = SAMPLE_TEXTURE2D(_ToonSpecMap, sampler_ToonSpecMap, uv);
    finalSpec = specMapColor.rgb * lightColor * _ToonSpecColor.rgb;
    
    //r: noise, g:noise mask, b: feather，这里的UV是经过ST缩放改变的
    float4 _1st_SpecularOptMap_var = SAMPLE_TEXTURE2D(_ToonSpecOptMap, sampler_ToonSpecOptMap, float2(uv.x * _ToonSpecOptMapST.x + _ToonSpecOptMapST.z, uv.y * _ToonSpecOptMapST.y + _ToonSpecOptMapST.w));
    //正常uv的高光OptMap
    float4 _1st_SpecularOptMap_Feather_var = SAMPLE_TEXTURE2D(_ToonSpecOptMap, sampler_ToonSpecOptMap, uv);

    float3 halfDirection = normalize(viewDirectionWS + lightDirectionWS);
    float _Specular_Area_var = 0.5 * dot(halfDirection, normalWS) + 0.5;
    float _1st_Specular_var = saturate(_Specular_Area_var - saturate(((1 - _1st_SpecularOptMap_var.r) * _1st_SpecularOptMap_Feather_var.g) * _ToonSpecMaskScale - _ToonSpecMaskOffset)); 
    float _1st_TweakSpecularMask_var = pow(_1st_Specular_var, exp2(lerp(11, 1, _ToonSpecGloss)));
    _1st_TweakSpecularMask_var = saturate((_1st_TweakSpecularMask_var - 0.01) / (saturate(_1st_SpecularOptMap_Feather_var.b * max(0.0001, _ToonSpecFeatherLevel))));
    
    float _TweakSpecularMask_var = saturate(_1st_TweakSpecularMask_var);

    float3 shiftedLowT = ShiftTangent(bitangent, normalWS, _ToonSpecShiftTangent_1st);//ShiftTangent在Core包的BSDF.hlsl中
    float anisoLow = StrandSpecular(shiftedLowT, viewDirectionWS, lightDirectionWS, _ToonSpecAnisoHighLightPower_1st, _ToonSpecAnisoHighLightStrength_1st);
    float3 shiftedHighT = ShiftTangent(bitangent, normalWS, specMapColor.a + _ToonSpecShiftTangent_2nd);
    float anisoHigh = StrandSpecular(shiftedHighT, viewDirectionWS, lightDirectionWS, _ToonSpecAnisoHighLightPower_2nd, _ToonSpecAnisoHighLightStrength_2nd);

    float anisoMask = saturate(saturate(anisoLow) + saturate(anisoHigh));
    _TweakSpecularMask_var = lerp(_TweakSpecularMask_var, anisoMask, _UseToonHairSpec);
     
    finalSpec = finalSpec * _TweakSpecularMask_var * _UseToonSpec * toonSpecMult;//toonSpecMult用来调整多光源中其他光源对这个高光的贡献
     
    return finalSpec;
} 

float3 JAToonClearCoat(float dividSharpness, float3 normalWS,  float3 viewDirectionWS, float2 uv)
{ 
    float3 clearCoatMaskMap = SAMPLE_TEXTURE2D(_ClearCoatMaskMap, sampler_ClearCoatMaskMap, uv).rgb;
    float NdotV = max(0, dot(normalWS, viewDirectionWS));
	//float ClearCoat = ((pow(NdotV, _ClearCoatGloss)) > (1 - _ClearCoatRange) ? _ClearCoatMult : 0);
    float sigmoid1 = sigmoid(pow(NdotV, _ClearCoatGloss * clearCoatMaskMap.g), 1 - _ClearCoatRange * clearCoatMaskMap.r, 5);
	float3 clearCoatColor = _ClearCoatColor * sigmoid1 * _ClearCoatMult * clearCoatMaskMap.b * _UseToonSpec;
    return clearCoatColor;
}
float3 JAToonSpecular(float3 lightColor, float3 normalWS, float3 lightDirectionWS, float3 viewDirectionWS, float2 uv)
{
    float3 specularMaskMap = SAMPLE_TEXTURE2D(_SpecularMaskMap, sampler_SpecularMaskMap, uv).rgb;
    float3 specular = 0;
	float3 halfDir = normalize(lightDirectionWS + viewDirectionWS);
	float NdotH = max(0, dot(normalWS, halfDir));
	float SpecularSize = pow(NdotH, _SpecularGloss * specularMaskMap.g);
	//float specularMask = ilmTex.b;
    specular = max(0, sigmoid(SpecularSize, 1 - _SpecularRange * specularMaskMap.r, 5)) * _SpecularMulti * specularMaskMap.b * _SpecularColor * _UseToonSpec * lightColor;
    return specular;
}

float3 JALightingToon(JABRDFData brdfData, float3 lightColor, float3 shadowColor, float NdotL, float2 uv, float3 lightDirectionWS, float3 normalWS, float3 bitangent, float3 viewDirectionWS, float toonMult)
{
    //float3 spec = JAToonCalcSpecular(uv, lightColor, lightDirectionWS, normalWS, bitangent, viewDirectionWS, toonSpecMult);
    //float3 toonOut = lerp(brdfData.toonShadow * shadowColor * brdfData.toonDiffuse , brdfData.toonDiffuse + spec, NdotL); 
    
    float NoL = NdotL * 2.0 - 1.0; 

    float _DividSharpness = 9.5 * Pow2(1/*_Roughness*/ - 1) + 0.5;

    // diffuse
	float MidLWin = sigmoid(NoL, _ToonLightDivid_M, _BoundSharp * _DividSharpness);
	float DarkSig = sigmoid(NoL, _ToonLightDivid_D, _BoundSharp * _DividSharpness);
     
	float MidDWin = DarkSig - MidLWin;
	float DarkWin = 1 - DarkSig; 
			
	float diffuseLumin1 = (1 + ndc2Normal(_ToonLightDivid_M)) / 2;
	float diffuseLumin2 = (ndc2Normal(_ToonLightDivid_M) + ndc2Normal(_ToonLightDivid_D)) / 2;
	float diffuseLumin3 = (ndc2Normal(_ToonLightDivid_D));
			
	float3 diffuseDeflectedColor1 = MidLWin  ;
	float3 diffuseDeflectedColor2 = MidDWin  * _DarkFaceColor.rgb * _ShadowColor.rgb * shadowColor;
	float3 diffuseDeflectedColor3 = DarkWin   * _DeepDarkColor.rgb * _ShadowColor.rgb * shadowColor;
	float3 diffuseBrightedColor = warp(diffuseDeflectedColor1 + diffuseDeflectedColor2 + diffuseDeflectedColor3, _ToonDiffuseBright.xxx);
	
	float3 diffuseResult = diffuseBrightedColor * brdfData.toonDiffuse.rgb; 

    // Subsurface Scattering 
	float SSMidLWin = Gaussion(NoL, _ToonLightDivid_M, _SSForwardAtt * _SSSSize);
    float SSMidDWin = Gaussion(NoL, _ToonLightDivid_M, _SSSSize);
	float3 SSLumin1 = (MidLWin * diffuseLumin2) * _SSForwardAtt * SSMidLWin;
	float3 SSLumin2 = ((MidDWin+ DarkWin) * diffuseLumin2) * SSMidDWin;
	float3 SS = _SSSWeight * (SSLumin1 + SSLumin2) * _SSSColor.rgb;


    float3 clearCoat = JAToonClearCoat(_DividSharpness, normalWS, viewDirectionWS, uv);
    float3 specular = JAToonSpecular(lightColor, normalWS, lightDirectionWS, viewDirectionWS, uv) * MidLWin;

	float3 toonOut = diffuseResult + SS + clearCoat + specular;  
    toonOut = toonOut * toonMult; 
    return toonOut;
}
float3 JALightingPhysicallyBased_Toon(JABRDFData brdfData, float3 lightColor, float occlusion, float3 shadowColor, float3 lightDirectionWS, float lightAttenuation, JAInputData inputData, float toonMult)
{   
    float3 normalWS = inputData.normalWS;
    float3 viewDirectionWS = inputData.viewDirectionWS;
    float2 uv = inputData.uv;
    float3 bitangent = inputData.bitangent;

    float NdotL = saturate(dot(normalWS, lightDirectionWS) ) * lightAttenuation;  

    //float texAddShadow = lerp(1.0, shadowMap.a, _TexAddShadowStrength);//手绘阴影
    //NdotL = NdotL * texAddShadow;
    //return lightAttenuation;
    float NdotL_Toon = (dot(normalWS, lightDirectionWS) * 0.5 + 0.5) * ((lightAttenuation) * occlusion); 
     
    float3 pbrLight = lerp(shadowColor * brdfData.toonDiffuse * _ShadowColor.rgb, JADirectBDRF_Toon(brdfData, normalWS, lightDirectionWS, viewDirectionWS), NdotL);
    float3 toonLight = JALightingToon(brdfData, lightColor, shadowColor, NdotL_Toon, uv, lightDirectionWS, normalWS, bitangent, viewDirectionWS, toonMult);
    //toonLight = lerp(toonLight, float3(0,0,0), brdfData.toonToPBR);
    //return brdfData.toonDiffuse;
    //return lerp(toonLight, pbrLight, brdfData.toonToPBR) * (lightColor);
    
    return lerp(toonLight, pbrLight, brdfData.toonToPBR) * (lightColor);
}
float3 JALightingPhysicallyBased_Toon(JABRDFData brdfData, Light light, float occlusion, float3 shadowColor, JAInputData inputData, float toonMult)
{
    return JALightingPhysicallyBased_Toon(brdfData, light.color, occlusion, shadowColor, light.direction, light.distanceAttenuation * light.shadowAttenuation, inputData, toonMult);
}


float3 JASubtractDirectMainLightFromLightmap(Light mainLight, float3 normalWS, float3 bakedGI)
{
    // Let's try to make realtime shadows work on a surface, which already contains
    // baked lighting and shadowing from the main sun light.
    // Summary:
    // 1) Calculate possible value in the shadow by subtracting estimated light contribution from the places occluded by realtime shadow:
    //      a) preserves other baked lights and light bounces
    //      b) eliminates shadows on the geometry facing away from the light
    // 2) Clamp against user defined ShadowColor.
    // 3) Pick original lightmap value, if it is the darkest one.


    // 1) Gives good estimate of illumination as if light would've been shadowed during the bake.
    // We only subtract the main direction light. This is accounted in the contribution term below.
    float shadowStrength = GetMainLightShadowStrength();
    float contributionTerm = saturate(dot(mainLight.direction, normalWS));//NdotL, 然后把-1到0的部分砍掉，即兰伯特光照模型的核心式子。 
    float3 lambert = mainLight.color * contributionTerm;
    float3 estimatedLightContributionMaskedByInverseOfShadow = lambert * (1.0 - mainLight.shadowAttenuation);//计算没有阴影的地方
    float3 subtractedLightmap = bakedGI - estimatedLightContributionMaskedByInverseOfShadow;

    // 2) Allows user to define overall ambient of the scene and control situation when realtime shadow becomes too dark.
    float3 realtimeShadow = max(subtractedLightmap, _SubtractiveShadowColor.xyz);
    realtimeShadow = lerp(bakedGI, realtimeShadow, shadowStrength);

    // 3) Pick darkest color
    return min(bakedGI, realtimeShadow);
}


void JAMixRealtimeAndBakedGI( Light light, float3 normalWS, inout float3 bakedGI, float4 shadowMask)
{
#if defined(_MIXED_LIGHTING_SUBTRACTIVE) && defined(LIGHTMAP_ON)
    bakedGI = JASubtractDirectMainLightFromLightmap(light, normalWS, bakedGI);
#endif
}


float JAMainLightRealtimeShadow(float4 shadowCoord)
{
#if !defined(MAIN_LIGHT_CALCULATE_SHADOWS)
    return 1.0h;
#endif

    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
    float4 shadowParams = GetMainLightShadowParams();
    return SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, shadowParams, false);
}
Light JAGetMainLight()
{
    Light light;
    light.direction = _MainLightPosition.xyz;
    // unity_LightData.z is 1 when not culled by the culling mask, otherwise 0.
    light.distanceAttenuation = unity_LightData.z;
#if defined(LIGHTMAP_ON) || defined(_MIXED_LIGHTING_SUBTRACTIVE)
    // unity_ProbesOcclusion.x is the mixed light probe occlusion data
    light.distanceAttenuation *= unity_ProbesOcclusion.x;
#endif
    light.shadowAttenuation = 1.0;
    light.color = _MainLightColor.rgb;

    return light;
}
Light JAGetMainLight(float4 shadowCoord)
{
    Light light = JAGetMainLight();
    light.shadowAttenuation = JAMainLightRealtimeShadow(shadowCoord);
    return light;
}



///////////////////////////////////////////////////////////////////////////////
//                      Fragment Functions                                   //
//       Used by ShaderGraph and others builtin renderers                    //
///////////////////////////////////////////////////////////////////////////////
float4 JAUniversalFragmentPBR(JAInputData inputData, float3 albedo, float4 shadowMap, float metallic, float3 specular,
    float smoothness, float occlusion, float3 emission, float alpha)
{
    JABRDFData brdfData;
    InitializeJABRDFData(inputData, albedo, shadowMap.rgb, metallic, specular, smoothness, alpha, brdfData); 
    Light mainLight = JAGetMainLight(inputData.shadowCoord);
    JAMixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, float4(0, 0, 0, 0));
     

    float3 color = JAGlobalIllumination(brdfData, inputData.bakedGI, occlusion, inputData.normalWS, inputData.viewDirectionWS, metallic); 
    float3 shadowColor = brdfData.diffuseShadow.rgb; 
    color += JALightingPhysicallyBased_Toon(brdfData, mainLight, occlusion, shadowColor, inputData, 1);  
    
    
#ifdef _ADDITIONAL_LIGHTS
    uint pixelLightCount = GetAdditionalLightsCount();
    for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
    {
        Light light = GetAdditionalLight(lightIndex, inputData.positionWS);
        color += JALightingPhysicallyBased_Toon(brdfData, light, occlusion, float3(0,0,0), inputData, 0.2);
        //color = inputData.positionWS;
    }
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX
    //color += inputData.vertexLighting * brdfData.diffuse; 
#endif

    color += emission;
    return float4(color, alpha);
}
