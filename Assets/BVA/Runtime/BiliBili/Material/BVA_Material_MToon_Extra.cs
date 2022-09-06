using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;

namespace GLTF.Schema.BVA
{
    [MaterialExtra]
    public class BVA_Material_MToon_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_MToon_Extra";
        public const string SHADER_NAME = "VRM/URP/MToon";
        public const string CUTOFF = "_Cutoff";
        public const string COLOR = "_Color";
        public const string SHADECOLOR = "_ShadeColor";
        public const string MAINTEX = "_MainTex";
        public const string SHADETEXTURE = "_ShadeTexture";
        public const string BUMPSCALE = "_BumpScale";
        public const string BUMPMAP = "_BumpMap";
        public const string RECEIVESHADOWRATE = "_ReceiveShadowRate";
        public const string RECEIVESHADOWTEXTURE = "_ReceiveShadowTexture";
        public const string SHADINGGRADERATE = "_ShadingGradeRate";
        public const string SHADINGGRADETEXTURE = "_ShadingGradeTexture";
        public const string SHADESHIFT = "_ShadeShift";
        public const string SHADETOONY = "_ShadeToony";
        public const string LIGHTCOLORATTENUATION = "_LightColorAttenuation";
        public const string INDIRECTLIGHTINTENSITY = "_IndirectLightIntensity";
        public const string RIMCOLOR = "_RimColor";
        public const string RIMTEXTURE = "_RimTexture";
        public const string RIMLIGHTINGMIX = "_RimLightingMix";
        public const string RIMFRESNELPOWER = "_RimFresnelPower";
        public const string RIMLIFT = "_RimLift";
        public const string SPHEREADD = "_SphereAdd";
        public const string EMISSIONCOLOR = "_EmissionColor";
        public const string EMISSIONMAP = "_EmissionMap";
        public const string OUTLINEWIDTHTEXTURE = "_OutlineWidthTexture";
        public const string OUTLINEWIDTH = "_OutlineWidth";
        public const string OUTLINESCALEDMAXDISTANCE = "_OutlineScaledMaxDistance";
        public const string OUTLINECOLOR = "_OutlineColor";
        public const string OUTLINELIGHTINGMIX = "_OutlineLightingMix";
        public const string UVANIMMASKTEXTURE = "_UvAnimMaskTexture";
        public const string UVANIMSCROLLX = "_UvAnimScrollX";
        public const string UVANIMSCROLLY = "_UvAnimScrollY";
        public const string UVANIMROTATION = "_UvAnimRotation";
        public const string MTOONVERSION = "_MToonVersion";
        public const string DEBUGMODE = "_DebugMode";
        public const string BLENDMODE = "_BlendMode";
        public const string OUTLINEWIDTHMODE = "_OutlineWidthMode";
        public const string OUTLINECOLORMODE = "_OutlineColorMode";
        public const string CULLMODE = "_CullMode";
        public const string OUTLINECULLMODE = "_OutlineCullMode";
        public const string SRCBLEND = "_SrcBlend";
        public const string DSTBLEND = "_DstBlend";
        public const string ZWRITE = "_ZWrite";
        public const string ALPHATOMASK = "_AlphaToMask";
        public MaterialParam<float> parameter_AlphaCutoff = new MaterialParam<float>(CUTOFF, 1.0f);
        public MaterialParam<Color> parameter_LitColorAlpha = new MaterialParam<Color>(COLOR, Color.white);
        public MaterialParam<Color> parameter_ShadeColor = new MaterialParam<Color>(SHADECOLOR, new Color(0.97f, 0.81f, 0.86f, 1.0f));
        public MaterialTextureParam parameter_LitTextureAlpha = new MaterialTextureParam(MAINTEX);
        public MaterialTextureParam parameter_ShadeTexture = new MaterialTextureParam(SHADETEXTURE);
        public MaterialParam<float> parameter_NormalScale = new MaterialParam<float>(BUMPSCALE, 1.0f);
        public MaterialTextureParam parameter_NormalTexture = new MaterialTextureParam(BUMPMAP);
        public MaterialParam<float> parameter_ReceiveShadow = new MaterialParam<float>(RECEIVESHADOWRATE, 1.0f);
        public MaterialTextureParam parameter_ReceiveShadowTexture = new MaterialTextureParam(RECEIVESHADOWTEXTURE);
        public MaterialParam<float> parameter_ShadingGrade = new MaterialParam<float>(SHADINGGRADERATE, 1.0f);
        public MaterialTextureParam parameter_ShadingGradeTexture = new MaterialTextureParam(SHADINGGRADETEXTURE);
        public MaterialParam<float> parameter_ShadeShift = new MaterialParam<float>(SHADESHIFT, .0f);
        public MaterialParam<float> parameter_ShadeToony = new MaterialParam<float>(SHADETOONY, 0.9f);
        public MaterialParam<float> parameter_LightColorAttenuation = new MaterialParam<float>(LIGHTCOLORATTENUATION, 0.0f);
        public MaterialParam<float> parameter_IndirectLightIntensity = new MaterialParam<float>(INDIRECTLIGHTINTENSITY, 0.1f);
        public MaterialParam<Color> parameter_RimColor = new MaterialParam<Color>(RIMCOLOR, Color.black);
        public MaterialTextureParam parameter_RimTexture = new MaterialTextureParam(RIMTEXTURE);
        public MaterialParam<float> parameter_RimLightingMix = new MaterialParam<float>(RIMLIGHTINGMIX, 0.0f);
        public MaterialParam<float> parameter_RimFresnelPower = new MaterialParam<float>(RIMFRESNELPOWER, 1.0f);
        public MaterialParam<float> parameter_RimLift = new MaterialParam<float>(RIMLIFT, 0.0f);
        public MaterialTextureParam parameter_SphereTextureAdd = new MaterialTextureParam(SPHEREADD);
        public MaterialParam<Color> parameter_Color = new MaterialParam<Color>(EMISSIONCOLOR, Color.white);
        public MaterialTextureParam parameter_Emission = new MaterialTextureParam(EMISSIONMAP);
        public MaterialTextureParam parameter_OutlineWidthTex = new MaterialTextureParam(OUTLINEWIDTHTEXTURE);
        public MaterialParam<float> parameter_OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 0.5f);
        public MaterialParam<float> parameter_OutlineScaledMaxDistance = new MaterialParam<float>(OUTLINESCALEDMAXDISTANCE, 1.0f);
        public MaterialParam<Color> parameter_OutlineColor = new MaterialParam<Color>(OUTLINECOLOR, Color.black);
        public MaterialParam<float> parameter_OutlineLightingMix = new MaterialParam<float>(OUTLINELIGHTINGMIX, 1.0f);
        public MaterialTextureParam parameter_UVAnimationMask = new MaterialTextureParam(UVANIMMASKTEXTURE);
        public MaterialParam<float> parameter_UVAnimationScrollX = new MaterialParam<float>(UVANIMSCROLLX, 0.0f);
        public MaterialParam<float> parameter_UVAnimationScrollY = new MaterialParam<float>(UVANIMSCROLLY, 0.0f);
        public MaterialParam<float> parameter_UVAnimationRotation = new MaterialParam<float>(UVANIMROTATION, 0.0f);
        public MaterialParam<float> parameter__MToonVersion = new MaterialParam<float>(MTOONVERSION, 38.0f);
        public MaterialParam<float> parameter__DebugMode = new MaterialParam<float>(DEBUGMODE, 0.0f);
        public MaterialParam<float> parameter__BlendMode = new MaterialParam<float>(BLENDMODE, 0.0f);
        public MaterialParam<float> parameter__OutlineWidthMode = new MaterialParam<float>(OUTLINEWIDTHMODE, .0f);
        public MaterialParam<float> parameter__OutlineColorMode = new MaterialParam<float>(OUTLINECOLORMODE, .0f);
        public MaterialParam<float> parameter__CullMode = new MaterialParam<float>(CULLMODE, 2.0f);
        public MaterialParam<float> parameter__OutlineCullMode = new MaterialParam<float>(OUTLINECULLMODE, 1.0f);
        public MaterialParam<float> parameter__SrcBlend = new MaterialParam<float>(SRCBLEND, 1.0f);
        public MaterialParam<float> parameter__DstBlend = new MaterialParam<float>(DSTBLEND, .0f);
        public MaterialParam<float> parameter__ZWrite = new MaterialParam<float>(ZWRITE, 1.0f);
        public MaterialParam<float> parameter__AlphaToMask = new MaterialParam<float>(ALPHATOMASK, .0f);
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            parameter_AlphaCutoff.Value = material.GetFloat(parameter_AlphaCutoff.ParamName);
            parameter_LitColorAlpha.Value = material.GetColor(parameter_LitColorAlpha.ParamName);
            parameter_ShadeColor.Value = material.GetColor(parameter_ShadeColor.ParamName);
            var parameter_littexturealpha_temp = material.GetTexture(parameter_LitTextureAlpha.ParamName);
            if (parameter_littexturealpha_temp != null) parameter_LitTextureAlpha.Value = exportTextureInfo(parameter_littexturealpha_temp);
            var parameter_shadetexture_temp = material.GetTexture(parameter_ShadeTexture.ParamName);
            if (parameter_shadetexture_temp != null) parameter_ShadeTexture.Value = exportTextureInfo(parameter_shadetexture_temp);
            parameter_NormalScale.Value = material.GetFloat(parameter_NormalScale.ParamName);
            var parameter_normaltexture_temp = material.GetTexture(parameter_NormalTexture.ParamName);
            if (parameter_normaltexture_temp != null) parameter_NormalTexture.Value = exportNormalTextureInfo(parameter_normaltexture_temp);
            parameter_ReceiveShadow.Value = material.GetFloat(parameter_ReceiveShadow.ParamName);
            var parameter_receiveshadowtexture_temp = material.GetTexture(parameter_ReceiveShadowTexture.ParamName);
            if (parameter_receiveshadowtexture_temp != null) parameter_ReceiveShadowTexture.Value = exportTextureInfo(parameter_receiveshadowtexture_temp);
            parameter_ShadingGrade.Value = material.GetFloat(parameter_ShadingGrade.ParamName);
            var parameter_shadinggradetexture_temp = material.GetTexture(parameter_ShadingGradeTexture.ParamName);
            if (parameter_shadinggradetexture_temp != null) parameter_ShadingGradeTexture.Value = exportTextureInfo(parameter_shadinggradetexture_temp);
            parameter_ShadeShift.Value = material.GetFloat(parameter_ShadeShift.ParamName);
            parameter_ShadeToony.Value = material.GetFloat(parameter_ShadeToony.ParamName);
            parameter_LightColorAttenuation.Value = material.GetFloat(parameter_LightColorAttenuation.ParamName);
            parameter_IndirectLightIntensity.Value = material.GetFloat(parameter_IndirectLightIntensity.ParamName);
            parameter_RimColor.Value = material.GetColor(parameter_RimColor.ParamName);
            var parameter_rimtexture_temp = material.GetTexture(parameter_RimTexture.ParamName);
            if (parameter_rimtexture_temp != null) parameter_RimTexture.Value = exportTextureInfo(parameter_rimtexture_temp);
            parameter_RimLightingMix.Value = material.GetFloat(parameter_RimLightingMix.ParamName);
            parameter_RimFresnelPower.Value = material.GetFloat(parameter_RimFresnelPower.ParamName);
            parameter_RimLift.Value = material.GetFloat(parameter_RimLift.ParamName);
            var parameter_spheretextureadd_temp = material.GetTexture(parameter_SphereTextureAdd.ParamName);
            if (parameter_spheretextureadd_temp != null) parameter_SphereTextureAdd.Value = exportTextureInfo(parameter_spheretextureadd_temp);
            parameter_Color.Value = material.GetColor(parameter_Color.ParamName);
            var parameter_emission_temp = material.GetTexture(parameter_Emission.ParamName);
            if (parameter_emission_temp != null) parameter_Emission.Value = exportTextureInfo(parameter_emission_temp);
            var parameter_outlinewidthtex_temp = material.GetTexture(parameter_OutlineWidthTex.ParamName);
            if (parameter_outlinewidthtex_temp != null) parameter_OutlineWidthTex.Value = exportTextureInfo(parameter_outlinewidthtex_temp);
            parameter_OutlineWidth.Value = material.GetFloat(parameter_OutlineWidth.ParamName);
            parameter_OutlineScaledMaxDistance.Value = material.GetFloat(parameter_OutlineScaledMaxDistance.ParamName);
            parameter_OutlineColor.Value = material.GetColor(parameter_OutlineColor.ParamName);
            parameter_OutlineLightingMix.Value = material.GetFloat(parameter_OutlineLightingMix.ParamName);
            var parameter_uvanimationmask_temp = material.GetTexture(parameter_UVAnimationMask.ParamName);
            if (parameter_uvanimationmask_temp != null) parameter_UVAnimationMask.Value = exportTextureInfo(parameter_uvanimationmask_temp);
            parameter_UVAnimationScrollX.Value = material.GetFloat(parameter_UVAnimationScrollX.ParamName);
            parameter_UVAnimationScrollY.Value = material.GetFloat(parameter_UVAnimationScrollY.ParamName);
            parameter_UVAnimationRotation.Value = material.GetFloat(parameter_UVAnimationRotation.ParamName);
            parameter__MToonVersion.Value = material.GetFloat(parameter__MToonVersion.ParamName);
            parameter__DebugMode.Value = material.GetFloat(parameter__DebugMode.ParamName);
            parameter__BlendMode.Value = material.GetFloat(parameter__BlendMode.ParamName);
            parameter__OutlineWidthMode.Value = material.GetFloat(parameter__OutlineWidthMode.ParamName);
            parameter__OutlineColorMode.Value = material.GetFloat(parameter__OutlineColorMode.ParamName);
            parameter__CullMode.Value = material.GetFloat(parameter__CullMode.ParamName);
            parameter__OutlineCullMode.Value = material.GetFloat(parameter__OutlineCullMode.ParamName);
            parameter__SrcBlend.Value = material.GetFloat(parameter__SrcBlend.ParamName);
            parameter__DstBlend.Value = material.GetFloat(parameter__DstBlend.ParamName);
            parameter__ZWrite.Value = material.GetFloat(parameter__ZWrite.ParamName);
            parameter__AlphaToMask.Value = material.GetFloat(parameter__AlphaToMask.ParamName);
        }

        public string ShaderName => SHADER_NAME;

        public string ExtraName => typeof(BVA_Material_MToon_Extra).Name;

        public async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache, AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case BVA_Material_MToon_Extra.CUTOFF:
                            matCache.SetFloat(BVA_Material_MToon_Extra.CUTOFF, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.COLOR:
                            matCache.SetColor(BVA_Material_MToon_Extra.COLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_MToon_Extra.SHADECOLOR:
                            matCache.SetColor(BVA_Material_MToon_Extra.SHADECOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_MToon_Extra.MAINTEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.MAINTEX, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.SHADETEXTURE:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.SHADETEXTURE, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.BUMPSCALE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.BUMPSCALE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.BUMPMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.BUMPMAP, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.RECEIVESHADOWRATE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.RECEIVESHADOWRATE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.RECEIVESHADOWTEXTURE:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.RECEIVESHADOWTEXTURE, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.SHADINGGRADERATE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.SHADINGGRADERATE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.SHADINGGRADETEXTURE:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.SHADINGGRADETEXTURE, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.SHADESHIFT:
                            matCache.SetFloat(BVA_Material_MToon_Extra.SHADESHIFT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.SHADETOONY:
                            matCache.SetFloat(BVA_Material_MToon_Extra.SHADETOONY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.LIGHTCOLORATTENUATION:
                            matCache.SetFloat(BVA_Material_MToon_Extra.LIGHTCOLORATTENUATION, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.INDIRECTLIGHTINTENSITY:
                            matCache.SetFloat(BVA_Material_MToon_Extra.INDIRECTLIGHTINTENSITY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.RIMCOLOR:
                            matCache.SetColor(BVA_Material_MToon_Extra.RIMCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_MToon_Extra.RIMTEXTURE:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.RIMTEXTURE, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.RIMLIGHTINGMIX:
                            matCache.SetFloat(BVA_Material_MToon_Extra.RIMLIGHTINGMIX, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.RIMFRESNELPOWER:
                            matCache.SetFloat(BVA_Material_MToon_Extra.RIMFRESNELPOWER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.RIMLIFT:
                            matCache.SetFloat(BVA_Material_MToon_Extra.RIMLIFT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.SPHEREADD:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.SPHEREADD, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.EMISSIONCOLOR:
                            matCache.SetColor(BVA_Material_MToon_Extra.EMISSIONCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_MToon_Extra.EMISSIONMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.EMISSIONMAP, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.OUTLINEWIDTHTEXTURE:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.OUTLINEWIDTHTEXTURE, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.OUTLINEWIDTH:
                            matCache.SetFloat(BVA_Material_MToon_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.OUTLINESCALEDMAXDISTANCE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.OUTLINESCALEDMAXDISTANCE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.OUTLINECOLOR:
                            matCache.SetColor(BVA_Material_MToon_Extra.OUTLINECOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_MToon_Extra.OUTLINELIGHTINGMIX:
                            matCache.SetFloat(BVA_Material_MToon_Extra.OUTLINELIGHTINGMIX, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.UVANIMMASKTEXTURE:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_MToon_Extra.UVANIMMASKTEXTURE, tex);
                            }
                            break;
                        case BVA_Material_MToon_Extra.UVANIMSCROLLX:
                            matCache.SetFloat(BVA_Material_MToon_Extra.UVANIMSCROLLX, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.UVANIMSCROLLY:
                            matCache.SetFloat(BVA_Material_MToon_Extra.UVANIMSCROLLY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.UVANIMROTATION:
                            matCache.SetFloat(BVA_Material_MToon_Extra.UVANIMROTATION, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.MTOONVERSION:
                            matCache.SetFloat(BVA_Material_MToon_Extra.MTOONVERSION, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.DEBUGMODE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.DEBUGMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.BLENDMODE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.BLENDMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.OUTLINEWIDTHMODE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.OUTLINEWIDTHMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.OUTLINECOLORMODE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.OUTLINECOLORMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.CULLMODE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.CULLMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.OUTLINECULLMODE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.OUTLINECULLMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.SRCBLEND:
                            matCache.SetFloat(BVA_Material_MToon_Extra.SRCBLEND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.DSTBLEND:
                            matCache.SetFloat(BVA_Material_MToon_Extra.DSTBLEND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.ZWRITE:
                            matCache.SetFloat(BVA_Material_MToon_Extra.ZWRITE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_MToon_Extra.ALPHATOMASK:
                            matCache.SetFloat(BVA_Material_MToon_Extra.ALPHATOMASK, reader.ReadAsFloat());
                            break;
                    }
                }
            }
        }

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(parameter_AlphaCutoff.ParamName, parameter_AlphaCutoff.Value);
            jo.Add(parameter_LitColorAlpha.ParamName, parameter_LitColorAlpha.Value.ToJArray());
            jo.Add(parameter_ShadeColor.ParamName, parameter_ShadeColor.Value.ToJArray());
            if (parameter_LitTextureAlpha != null && parameter_LitTextureAlpha.Value != null) jo.Add(parameter_LitTextureAlpha.ParamName, parameter_LitTextureAlpha.Serialize());
            if (parameter_ShadeTexture != null && parameter_ShadeTexture.Value != null) jo.Add(parameter_ShadeTexture.ParamName, parameter_ShadeTexture.Serialize());
            jo.Add(parameter_NormalScale.ParamName, parameter_NormalScale.Value);
            if (parameter_NormalTexture != null && parameter_NormalTexture.Value != null) jo.Add(parameter_NormalTexture.ParamName, parameter_NormalTexture.Serialize());
            jo.Add(parameter_ReceiveShadow.ParamName, parameter_ReceiveShadow.Value);
            if (parameter_ReceiveShadowTexture != null && parameter_ReceiveShadowTexture.Value != null) jo.Add(parameter_ReceiveShadowTexture.ParamName, parameter_ReceiveShadowTexture.Serialize());
            jo.Add(parameter_ShadingGrade.ParamName, parameter_ShadingGrade.Value);
            if (parameter_ShadingGradeTexture != null && parameter_ShadingGradeTexture.Value != null) jo.Add(parameter_ShadingGradeTexture.ParamName, parameter_ShadingGradeTexture.Serialize());
            jo.Add(parameter_ShadeShift.ParamName, parameter_ShadeShift.Value);
            jo.Add(parameter_ShadeToony.ParamName, parameter_ShadeToony.Value);
            jo.Add(parameter_LightColorAttenuation.ParamName, parameter_LightColorAttenuation.Value);
            jo.Add(parameter_IndirectLightIntensity.ParamName, parameter_IndirectLightIntensity.Value);
            jo.Add(parameter_RimColor.ParamName, parameter_RimColor.Value.ToJArray());
            if (parameter_RimTexture != null && parameter_RimTexture.Value != null) jo.Add(parameter_RimTexture.ParamName, parameter_RimTexture.Serialize());
            jo.Add(parameter_RimLightingMix.ParamName, parameter_RimLightingMix.Value);
            jo.Add(parameter_RimFresnelPower.ParamName, parameter_RimFresnelPower.Value);
            jo.Add(parameter_RimLift.ParamName, parameter_RimLift.Value);
            if (parameter_SphereTextureAdd != null && parameter_SphereTextureAdd.Value != null) jo.Add(parameter_SphereTextureAdd.ParamName, parameter_SphereTextureAdd.Serialize());
            jo.Add(parameter_Color.ParamName, parameter_Color.Value.ToJArray());
            if (parameter_Emission != null && parameter_Emission.Value != null) jo.Add(parameter_Emission.ParamName, parameter_Emission.Serialize());
            if (parameter_OutlineWidthTex != null && parameter_OutlineWidthTex.Value != null) jo.Add(parameter_OutlineWidthTex.ParamName, parameter_OutlineWidthTex.Serialize());
            jo.Add(parameter_OutlineWidth.ParamName, parameter_OutlineWidth.Value);
            jo.Add(parameter_OutlineScaledMaxDistance.ParamName, parameter_OutlineScaledMaxDistance.Value);
            jo.Add(parameter_OutlineColor.ParamName, parameter_OutlineColor.Value.ToJArray());
            jo.Add(parameter_OutlineLightingMix.ParamName, parameter_OutlineLightingMix.Value);
            if (parameter_UVAnimationMask != null && parameter_UVAnimationMask.Value != null) jo.Add(parameter_UVAnimationMask.ParamName, parameter_UVAnimationMask.Serialize());
            jo.Add(parameter_UVAnimationScrollX.ParamName, parameter_UVAnimationScrollX.Value);
            jo.Add(parameter_UVAnimationScrollY.ParamName, parameter_UVAnimationScrollY.Value);
            jo.Add(parameter_UVAnimationRotation.ParamName, parameter_UVAnimationRotation.Value);
            jo.Add(parameter__MToonVersion.ParamName, parameter__MToonVersion.Value);
            jo.Add(parameter__DebugMode.ParamName, parameter__DebugMode.Value);
            jo.Add(parameter__BlendMode.ParamName, parameter__BlendMode.Value);
            jo.Add(parameter__OutlineWidthMode.ParamName, parameter__OutlineWidthMode.Value);
            jo.Add(parameter__OutlineColorMode.ParamName, parameter__OutlineColorMode.Value);
            jo.Add(parameter__CullMode.ParamName, parameter__CullMode.Value);
            jo.Add(parameter__OutlineCullMode.ParamName, parameter__OutlineCullMode.Value);
            jo.Add(parameter__SrcBlend.ParamName, parameter__SrcBlend.Value);
            jo.Add(parameter__DstBlend.ParamName, parameter__DstBlend.Value);
            jo.Add(parameter__ZWrite.ParamName, parameter__ZWrite.Value);
            jo.Add(parameter__AlphaToMask.ParamName, parameter__AlphaToMask.Value);
            return new JProperty(BVA_Material_MToon_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_MToon_Extra();
        }
    }
}
