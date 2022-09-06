using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;

namespace GLTF.Schema.BVA
{
    [MaterialExtra]
    public class BVA_Material_ToonLit_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_ToonLit_Extra";
        public const string SHADER_NAME = "Universal Render Pipeline/Toon Lit";
        public const string WORKFLOWMODE = "_WorkflowMode";
        public const string BASEMAP = "_BaseMap";
        public const string BASECOLOR = "_BaseColor";
        public const string SHADOWMAP = "_ShadowMap";
        public const string SHADOWCOLOR = "_ShadowColor";
        public const string TEXADDSHADOWSTRENGTH = "_TexAddShadowStrength";
        public const string TOONMASKMAP = "_ToonMaskMap";
        public const string CUTOFF = "_Cutoff";
        public const string ROUGHNESSMAP = "_RoughnessMap";
        public const string ROUGHNESS = "_Roughness";
        public const string METALLIC = "_Metallic";
        public const string METALLICGLOSSMAP = "_MetallicGlossMap";
        public const string SPECCOLOR = "_SpecColor";
        public const string SPECGLOSSMAP = "_SpecGlossMap";
        public const string USETOONSPEC = "_UseToonSpec";
        public const string TOONSPECMAP = "_ToonSpecMap";
        public const string TOONSPECCOLOR = "_ToonSpecColor";
        public const string TOONSPECOPTMAP = "_ToonSpecOptMap";
        public const string TOONSPECOPTMAPST = "_ToonSpecOptMapST";
        public const string TOONSPECGLOSS = "_ToonSpecGloss";
        public const string TOONSPECFEATHERLEVEL = "_ToonSpecFeatherLevel";
        public const string TOONSPECMASKSCALE = "_ToonSpecMaskScale";
        public const string TOONSPECMASKOFFSET = "_ToonSpecMaskOffset";
        public const string USETOONHAIRSPEC = "_UseToonHairSpec";
        public const string TOONSPECANISOHIGHLIGHTPOWERFIRST = "_ToonSpecAnisoHighLightPower_1st";
        public const string TOONSPECANISOHIGHLIGHTPOWERSECOND = "_ToonSpecAnisoHighLightPower_2nd";
        public const string TOONSPECANISOHIGHLIGHTSTRENGTHFIRST = "_ToonSpecAnisoHighLightStrength_1st";
        public const string TOONSPECANISOHIGHLIGHTSTRENGTHSECOND = "_ToonSpecAnisoHighLightStrength_2nd";
        public const string TOONSPECSHIFTTANGENTFIRST = "_ToonSpecShiftTangent_1st";
        public const string TOONSPECSHIFTTANGENTSECOND = "_ToonSpecShiftTangent_2nd";
        public const string BUMPSCALE = "_BumpScale";
        public const string BUMPMAP = "_BumpMap";
        public const string OCCLUSIONSTRENGTH = "_OcclusionStrength";
        public const string OCCLUSIONMAP = "_OcclusionMap";
        public const string SURFACE = "_Surface";
        public const string BLEND = "_Blend";
        public const string ALPHACLIP = "_AlphaClip";
        public const string SRCBLEND = "_SrcBlend";
        public const string DSTBLEND = "_DstBlend";
        public const string ZWRITE = "_ZWrite";
        public const string CULL = "_Cull";
        public const string RECEIVESHADOWS = "_ReceiveShadows";
        public const string TOONLIGHTDIVIDM = "_ToonLightDivid_M";
        public const string TOONLIGHTDIVIDD = "_ToonLightDivid_D";
        public const string TOONDIFFUSEBRIGHT = "_ToonDiffuseBright";
        public const string BOUNDSHARP = "_BoundSharp";
        public const string DARKFACECOLOR = "_DarkFaceColor";
        public const string DEEPDARKCOLOR = "_DeepDarkColor";
        public const string SSSCOLOR = "_SSSColor";
        public const string SSSWEIGHT = "_SSSWeight";
        public const string SSSSIZE = "_SSSSize";
        public const string SSFORWARDATT = "_SSForwardAtt";
        public const string CLEARCOATMASKMAP = "_ClearCoatMaskMap";
        public const string CLEARCOATCOLOR = "_ClearCoatColor";
        public const string CLEARCOATRANGE = "_ClearCoatRange";
        public const string CLEARCOATGLOSS = "_ClearCoatGloss";
        public const string CLEARCOATMULT = "_ClearCoatMult";
        public const string SPECULARMASKMAP = "_SpecularMaskMap";
        public const string SPECULARCOLOR = "_SpecularColor";
        public const string SPECULARRANGE = "_SpecularRange";
        public const string SPECULARMULTI = "_SpecularMulti";
        public const string SPECULARGLOSS = "_SpecularGloss";
        public const string TOONTOPBRMAP = "_ToonToPBRMap";
        public const string TOONTOPBR = "_ToonToPBR";
        public const string OUTLINECOLOR = "_OutLineColor";
        public const string OUTLINETHICKNESS = "_OutLineThickness";
        public MaterialParam<float> parameter__WorkflowMode = new MaterialParam<float>(WORKFLOWMODE, 1.0f);
        public MaterialTextureParam parameter__BaseMap = new MaterialTextureParam(BASEMAP);
        public MaterialParam<Color> parameter__BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
        public MaterialTextureParam parameter__ShadowMap = new MaterialTextureParam(SHADOWMAP);
        public MaterialParam<Color> parameter__ShadowColor = new MaterialParam<Color>(SHADOWCOLOR, Color.white);
        public MaterialParam<float> parameter__TexAddShadowStrength = new MaterialParam<float>(TEXADDSHADOWSTRENGTH, 1.0f);
        public MaterialTextureParam parameter__ToonMaskMap = new MaterialTextureParam(TOONMASKMAP);
        public MaterialParam<float> parameter__Cutoff = new MaterialParam<float>(CUTOFF, 1.0f);
        public MaterialTextureParam parameter__RoughnessMap = new MaterialTextureParam(ROUGHNESSMAP);
        public MaterialParam<float> parameter__Roughness = new MaterialParam<float>(ROUGHNESS, 1.0f);
        public MaterialParam<float> parameter__Metallic = new MaterialParam<float>(METALLIC, 1.0f);
        public MaterialTextureParam parameter__MetallicGlossMap = new MaterialTextureParam(METALLICGLOSSMAP);
        public MaterialParam<Color> parameter__SpecColor = new MaterialParam<Color>(SPECCOLOR, Color.white);
        public MaterialTextureParam parameter__SpecGlossMap = new MaterialTextureParam(SPECGLOSSMAP);
        public MaterialParam<float> parameter__UseToonSpec = new MaterialParam<float>(USETOONSPEC, 1.0f);
        public MaterialTextureParam parameter__ToonSpecMap = new MaterialTextureParam(TOONSPECMAP);
        public MaterialParam<Color> parameter__ToonSpecColor = new MaterialParam<Color>(TOONSPECCOLOR, Color.white);
        public MaterialTextureParam parameter__ToonSpecOptMap = new MaterialTextureParam(TOONSPECOPTMAP);
        public MaterialParam<Vector4> parameter__ToonSpecOptMapST = new MaterialParam<Vector4>(TOONSPECOPTMAPST, Vector4.one);
        public MaterialParam<float> parameter__ToonSpecGloss = new MaterialParam<float>(TOONSPECGLOSS, 1.0f);
        public MaterialParam<float> parameter__ToonSpecFeatherLevel = new MaterialParam<float>(TOONSPECFEATHERLEVEL, 1.0f);
        public MaterialParam<float> parameter__ToonSpecMaskScale = new MaterialParam<float>(TOONSPECMASKSCALE, 1.0f);
        public MaterialParam<float> parameter__ToonSpecMaskOffset = new MaterialParam<float>(TOONSPECMASKOFFSET, 1.0f);
        public MaterialParam<float> parameter__UseToonHairSpec = new MaterialParam<float>(USETOONHAIRSPEC, 1.0f);
        public MaterialParam<float> parameter__ToonSpecAnisoHighLightPower_1st = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTPOWERFIRST, 1.0f);
        public MaterialParam<float> parameter__ToonSpecAnisoHighLightPower_2nd = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTPOWERSECOND, 1.0f);
        public MaterialParam<float> parameter__ToonSpecAnisoHighLightStrength_1st = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTSTRENGTHFIRST, 1.0f);
        public MaterialParam<float> parameter__ToonSpecAnisoHighLightStrength_2nd = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTSTRENGTHSECOND, 1.0f);
        public MaterialParam<float> parameter__ToonSpecShiftTangent_1st = new MaterialParam<float>(TOONSPECSHIFTTANGENTFIRST, 1.0f);
        public MaterialParam<float> parameter__ToonSpecShiftTangent_2nd = new MaterialParam<float>(TOONSPECSHIFTTANGENTSECOND, 1.0f);
        public MaterialParam<float> parameter__BumpScale = new MaterialParam<float>(BUMPSCALE, 1.0f);
        public MaterialTextureParam parameter__BumpMap = new MaterialTextureParam(BUMPMAP);
        public MaterialParam<float> parameter__OcclusionStrength = new MaterialParam<float>(OCCLUSIONSTRENGTH, 1.0f);
        public MaterialTextureParam parameter__OcclusionMap = new MaterialTextureParam(OCCLUSIONMAP);
        public MaterialParam<float> parameter__Surface = new MaterialParam<float>(SURFACE, 1.0f);
        public MaterialParam<float> parameter__Blend = new MaterialParam<float>(BLEND, 1.0f);
        public MaterialParam<float> parameter__AlphaClip = new MaterialParam<float>(ALPHACLIP, 1.0f);
        public MaterialParam<float> parameter__SrcBlend = new MaterialParam<float>(SRCBLEND, 1.0f);
        public MaterialParam<float> parameter__DstBlend = new MaterialParam<float>(DSTBLEND, 1.0f);
        public MaterialParam<float> parameter__ZWrite = new MaterialParam<float>(ZWRITE, 1.0f);
        public MaterialParam<float> parameter__Cull = new MaterialParam<float>(CULL, 1.0f);
        public MaterialParam<float> parameter__ReceiveShadows = new MaterialParam<float>(RECEIVESHADOWS, 1.0f);
        public MaterialParam<float> parameter__ToonLightDivid_M = new MaterialParam<float>(TOONLIGHTDIVIDM, 1.0f);
        public MaterialParam<float> parameter__ToonLightDivid_D = new MaterialParam<float>(TOONLIGHTDIVIDD, 1.0f);
        public MaterialParam<float> parameter__ToonDiffuseBright = new MaterialParam<float>(TOONDIFFUSEBRIGHT, 1.0f);
        public MaterialParam<float> parameter__BoundSharp = new MaterialParam<float>(BOUNDSHARP, 1.0f);
        public MaterialParam<Color> parameter__DarkFaceColor = new MaterialParam<Color>(DARKFACECOLOR, Color.white);
        public MaterialParam<Color> parameter__DeepDarkColor = new MaterialParam<Color>(DEEPDARKCOLOR, Color.white);
        public MaterialParam<Color> parameter__SSSColor = new MaterialParam<Color>(SSSCOLOR, Color.white);
        public MaterialParam<float> parameter__SSSWeight = new MaterialParam<float>(SSSWEIGHT, 1.0f);
        public MaterialParam<float> parameter__SSSSize = new MaterialParam<float>(SSSSIZE, 1.0f);
        public MaterialParam<float> parameter__SSForwardAtt = new MaterialParam<float>(SSFORWARDATT, 1.0f);
        public MaterialTextureParam parameter__ClearCoatMaskMap = new MaterialTextureParam(CLEARCOATMASKMAP);
        public MaterialParam<Color> parameter__ClearCoatColor = new MaterialParam<Color>(CLEARCOATCOLOR, Color.white);
        public MaterialParam<float> parameter__ClearCoatRange = new MaterialParam<float>(CLEARCOATRANGE, 1.0f);
        public MaterialParam<float> parameter__ClearCoatGloss = new MaterialParam<float>(CLEARCOATGLOSS, 1.0f);
        public MaterialParam<float> parameter__ClearCoatMult = new MaterialParam<float>(CLEARCOATMULT, 1.0f);
        public MaterialTextureParam parameter__SpecularMaskMap = new MaterialTextureParam(SPECULARMASKMAP);
        public MaterialParam<Color> parameter__SpecularColor = new MaterialParam<Color>(SPECULARCOLOR, Color.white);
        public MaterialParam<float> parameter__SpecularRange = new MaterialParam<float>(SPECULARRANGE, 1.0f);
        public MaterialParam<float> parameter__SpecularMulti = new MaterialParam<float>(SPECULARMULTI, 1.0f);
        public MaterialParam<float> parameter__SpecularGloss = new MaterialParam<float>(SPECULARGLOSS, 1.0f);
        public MaterialTextureParam parameter__ToonToPBRMap = new MaterialTextureParam(TOONTOPBRMAP);
        public MaterialParam<float> parameter__ToonToPBR = new MaterialParam<float>(TOONTOPBR, 1.0f);
        public MaterialParam<Color> parameter__OutLineColor = new MaterialParam<Color>(OUTLINECOLOR, Color.white);
        public MaterialParam<float> parameter__OutLineThickness = new MaterialParam<float>(OUTLINETHICKNESS, 1.0f);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            parameter__WorkflowMode.Value = material.GetFloat(parameter__WorkflowMode.ParamName);
            var parameter__basemap_temp = material.GetTexture(parameter__BaseMap.ParamName);
            if (parameter__basemap_temp != null) parameter__BaseMap.Value = exportTextureInfo(parameter__basemap_temp);
            parameter__BaseColor.Value = material.GetColor(parameter__BaseColor.ParamName);
            var parameter__shadowmap_temp = material.GetTexture(parameter__ShadowMap.ParamName);
            if (parameter__shadowmap_temp != null) parameter__ShadowMap.Value = exportTextureInfo(parameter__shadowmap_temp);
            parameter__ShadowColor.Value = material.GetColor(parameter__ShadowColor.ParamName);
            parameter__TexAddShadowStrength.Value = material.GetFloat(parameter__TexAddShadowStrength.ParamName);
            var parameter__toonmaskmap_temp = material.GetTexture(parameter__ToonMaskMap.ParamName);
            if (parameter__toonmaskmap_temp != null) parameter__ToonMaskMap.Value = exportTextureInfo(parameter__toonmaskmap_temp);
            parameter__Cutoff.Value = material.GetFloat(parameter__Cutoff.ParamName);
            var parameter__roughnessmap_temp = material.GetTexture(parameter__RoughnessMap.ParamName);
            if (parameter__roughnessmap_temp != null) parameter__RoughnessMap.Value = exportTextureInfo(parameter__roughnessmap_temp);
            parameter__Roughness.Value = material.GetFloat(parameter__Roughness.ParamName);
            parameter__Metallic.Value = material.GetFloat(parameter__Metallic.ParamName);
            var parameter__metallicglossmap_temp = material.GetTexture(parameter__MetallicGlossMap.ParamName);
            if (parameter__metallicglossmap_temp != null) parameter__MetallicGlossMap.Value = exportTextureInfo(parameter__metallicglossmap_temp);
            parameter__SpecColor.Value = material.GetColor(parameter__SpecColor.ParamName);
            var parameter__specglossmap_temp = material.GetTexture(parameter__SpecGlossMap.ParamName);
            if (parameter__specglossmap_temp != null) parameter__SpecGlossMap.Value = exportTextureInfo(parameter__specglossmap_temp);
            parameter__UseToonSpec.Value = material.GetFloat(parameter__UseToonSpec.ParamName);
            var parameter__toonspecmap_temp = material.GetTexture(parameter__ToonSpecMap.ParamName);
            if (parameter__toonspecmap_temp != null) parameter__ToonSpecMap.Value = exportTextureInfo(parameter__toonspecmap_temp);
            parameter__ToonSpecColor.Value = material.GetColor(parameter__ToonSpecColor.ParamName);
            var parameter__toonspecoptmap_temp = material.GetTexture(parameter__ToonSpecOptMap.ParamName);
            if (parameter__toonspecoptmap_temp != null) parameter__ToonSpecOptMap.Value = exportTextureInfo(parameter__toonspecoptmap_temp);
            parameter__ToonSpecOptMapST.Value = material.GetVector(parameter__ToonSpecOptMapST.ParamName);
            parameter__ToonSpecGloss.Value = material.GetFloat(parameter__ToonSpecGloss.ParamName);
            parameter__ToonSpecFeatherLevel.Value = material.GetFloat(parameter__ToonSpecFeatherLevel.ParamName);
            parameter__ToonSpecMaskScale.Value = material.GetFloat(parameter__ToonSpecMaskScale.ParamName);
            parameter__ToonSpecMaskOffset.Value = material.GetFloat(parameter__ToonSpecMaskOffset.ParamName);
            parameter__UseToonHairSpec.Value = material.GetFloat(parameter__UseToonHairSpec.ParamName);
            parameter__ToonSpecAnisoHighLightPower_1st.Value = material.GetFloat(parameter__ToonSpecAnisoHighLightPower_1st.ParamName);
            parameter__ToonSpecAnisoHighLightPower_2nd.Value = material.GetFloat(parameter__ToonSpecAnisoHighLightPower_2nd.ParamName);
            parameter__ToonSpecAnisoHighLightStrength_1st.Value = material.GetFloat(parameter__ToonSpecAnisoHighLightStrength_1st.ParamName);
            parameter__ToonSpecAnisoHighLightStrength_2nd.Value = material.GetFloat(parameter__ToonSpecAnisoHighLightStrength_2nd.ParamName);
            parameter__ToonSpecShiftTangent_1st.Value = material.GetFloat(parameter__ToonSpecShiftTangent_1st.ParamName);
            parameter__ToonSpecShiftTangent_2nd.Value = material.GetFloat(parameter__ToonSpecShiftTangent_2nd.ParamName);
            parameter__BumpScale.Value = material.GetFloat(parameter__BumpScale.ParamName);
            var parameter__bumpmap_temp = material.GetTexture(parameter__BumpMap.ParamName);
            if (parameter__bumpmap_temp != null) parameter__BumpMap.Value = exportNormalTextureInfo(parameter__bumpmap_temp);
            parameter__OcclusionStrength.Value = material.GetFloat(parameter__OcclusionStrength.ParamName);
            var parameter__occlusionmap_temp = material.GetTexture(parameter__OcclusionMap.ParamName);
            if (parameter__occlusionmap_temp != null) parameter__OcclusionMap.Value = exportTextureInfo(parameter__occlusionmap_temp);
            parameter__Surface.Value = material.GetFloat(parameter__Surface.ParamName);
            parameter__Blend.Value = material.GetFloat(parameter__Blend.ParamName);
            parameter__AlphaClip.Value = material.GetFloat(parameter__AlphaClip.ParamName);
            parameter__SrcBlend.Value = material.GetFloat(parameter__SrcBlend.ParamName);
            parameter__DstBlend.Value = material.GetFloat(parameter__DstBlend.ParamName);
            parameter__ZWrite.Value = material.GetFloat(parameter__ZWrite.ParamName);
            parameter__Cull.Value = material.GetFloat(parameter__Cull.ParamName);
            parameter__ReceiveShadows.Value = material.GetFloat(parameter__ReceiveShadows.ParamName);
            parameter__ToonLightDivid_M.Value = material.GetFloat(parameter__ToonLightDivid_M.ParamName);
            parameter__ToonLightDivid_D.Value = material.GetFloat(parameter__ToonLightDivid_D.ParamName);
            parameter__ToonDiffuseBright.Value = material.GetFloat(parameter__ToonDiffuseBright.ParamName);
            parameter__BoundSharp.Value = material.GetFloat(parameter__BoundSharp.ParamName);
            parameter__DarkFaceColor.Value = material.GetColor(parameter__DarkFaceColor.ParamName);
            parameter__DeepDarkColor.Value = material.GetColor(parameter__DeepDarkColor.ParamName);
            parameter__SSSColor.Value = material.GetColor(parameter__SSSColor.ParamName);
            parameter__SSSWeight.Value = material.GetFloat(parameter__SSSWeight.ParamName);
            parameter__SSSSize.Value = material.GetFloat(parameter__SSSSize.ParamName);
            parameter__SSForwardAtt.Value = material.GetFloat(parameter__SSForwardAtt.ParamName);
            var parameter__clearcoatmaskmap_temp = material.GetTexture(parameter__ClearCoatMaskMap.ParamName);
            if (parameter__clearcoatmaskmap_temp != null) parameter__ClearCoatMaskMap.Value = exportTextureInfo(parameter__clearcoatmaskmap_temp);
            parameter__ClearCoatColor.Value = material.GetColor(parameter__ClearCoatColor.ParamName);
            parameter__ClearCoatRange.Value = material.GetFloat(parameter__ClearCoatRange.ParamName);
            parameter__ClearCoatGloss.Value = material.GetFloat(parameter__ClearCoatGloss.ParamName);
            parameter__ClearCoatMult.Value = material.GetFloat(parameter__ClearCoatMult.ParamName);
            var parameter__specularmaskmap_temp = material.GetTexture(parameter__SpecularMaskMap.ParamName);
            if (parameter__specularmaskmap_temp != null) parameter__SpecularMaskMap.Value = exportTextureInfo(parameter__specularmaskmap_temp);
            parameter__SpecularColor.Value = material.GetColor(parameter__SpecularColor.ParamName);
            parameter__SpecularRange.Value = material.GetFloat(parameter__SpecularRange.ParamName);
            parameter__SpecularMulti.Value = material.GetFloat(parameter__SpecularMulti.ParamName);
            parameter__SpecularGloss.Value = material.GetFloat(parameter__SpecularGloss.ParamName);
            var parameter__toontopbrmap_temp = material.GetTexture(parameter__ToonToPBRMap.ParamName);
            if (parameter__toontopbrmap_temp != null) parameter__ToonToPBRMap.Value = exportTextureInfo(parameter__toontopbrmap_temp);
            parameter__ToonToPBR.Value = material.GetFloat(parameter__ToonToPBR.ParamName);
            parameter__OutLineColor.Value = material.GetColor(parameter__OutLineColor.ParamName);
            parameter__OutLineThickness.Value = material.GetFloat(parameter__OutLineThickness.ParamName);
        }
        public async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache, AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case BVA_Material_ToonLit_Extra.WORKFLOWMODE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.WORKFLOWMODE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.BASEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.BASEMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.BASECOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.BASECOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.SHADOWMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.SHADOWMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.SHADOWCOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.SHADOWCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.TEXADDSHADOWSTRENGTH:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TEXADDSHADOWSTRENGTH, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONMASKMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.TOONMASKMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.CUTOFF:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.CUTOFF, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.ROUGHNESSMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.ROUGHNESSMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.ROUGHNESS:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.ROUGHNESS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.METALLIC:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.METALLIC, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.METALLICGLOSSMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.METALLICGLOSSMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.SPECCOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.SPECCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.SPECGLOSSMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.SPECGLOSSMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.USETOONSPEC:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.USETOONSPEC, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.TOONSPECMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECCOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.TOONSPECCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECOPTMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.TOONSPECOPTMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECOPTMAPST:
                            matCache.SetVector(BVA_Material_ToonLit_Extra.TOONSPECOPTMAPST, reader.ReadAsVector4());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECGLOSS:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECGLOSS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECFEATHERLEVEL:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECFEATHERLEVEL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECMASKSCALE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECMASKSCALE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECMASKOFFSET:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECMASKOFFSET, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.USETOONHAIRSPEC:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.USETOONHAIRSPEC, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTPOWERFIRST:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTPOWERFIRST, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTPOWERSECOND:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTPOWERSECOND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTSTRENGTHFIRST:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTSTRENGTHFIRST, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTSTRENGTHSECOND:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECANISOHIGHLIGHTSTRENGTHSECOND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECSHIFTTANGENTFIRST:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECSHIFTTANGENTFIRST, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONSPECSHIFTTANGENTSECOND:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONSPECSHIFTTANGENTSECOND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.BUMPSCALE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.BUMPSCALE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.BUMPMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.BUMPMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.OCCLUSIONSTRENGTH:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.OCCLUSIONSTRENGTH, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.OCCLUSIONMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.OCCLUSIONMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.SURFACE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SURFACE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.BLEND:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.BLEND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.ALPHACLIP:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.ALPHACLIP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.SRCBLEND:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SRCBLEND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.DSTBLEND:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.DSTBLEND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.ZWRITE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.ZWRITE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.CULL:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.CULL, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.RECEIVESHADOWS:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.RECEIVESHADOWS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONLIGHTDIVIDM:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONLIGHTDIVIDM, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONLIGHTDIVIDD:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONLIGHTDIVIDD, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONDIFFUSEBRIGHT:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONDIFFUSEBRIGHT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.BOUNDSHARP:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.BOUNDSHARP, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.DARKFACECOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.DARKFACECOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.DEEPDARKCOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.DEEPDARKCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.SSSCOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.SSSCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.SSSWEIGHT:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SSSWEIGHT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.SSSSIZE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SSSSIZE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.SSFORWARDATT:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SSFORWARDATT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.CLEARCOATMASKMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.CLEARCOATMASKMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.CLEARCOATCOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.CLEARCOATCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.CLEARCOATRANGE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.CLEARCOATRANGE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.CLEARCOATGLOSS:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.CLEARCOATGLOSS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.CLEARCOATMULT:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.CLEARCOATMULT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.SPECULARMASKMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.SPECULARMASKMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.SPECULARCOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.SPECULARCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.SPECULARRANGE:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SPECULARRANGE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.SPECULARMULTI:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SPECULARMULTI, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.SPECULARGLOSS:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.SPECULARGLOSS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.TOONTOPBRMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonLit_Extra.TOONTOPBRMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonLit_Extra.TOONTOPBR:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.TOONTOPBR, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonLit_Extra.OUTLINECOLOR:
                            matCache.SetColor(BVA_Material_ToonLit_Extra.OUTLINECOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ToonLit_Extra.OUTLINETHICKNESS:
                            matCache.SetFloat(BVA_Material_ToonLit_Extra.OUTLINETHICKNESS, reader.ReadAsFloat());
                            break;
                        case nameof(keywords):
                            {
                                var keywords = reader.ReadStringList();
                                foreach (var keyword in keywords)
                                    matCache.EnableKeyword(keyword);
                            }
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(parameter__WorkflowMode.ParamName, parameter__WorkflowMode.Value);
            if (parameter__BaseMap != null && parameter__BaseMap.Value != null) jo.Add(parameter__BaseMap.ParamName, parameter__BaseMap.Serialize());
            jo.Add(parameter__BaseColor.ParamName, parameter__BaseColor.Value.ToJArray());
            if (parameter__ShadowMap != null && parameter__ShadowMap.Value != null) jo.Add(parameter__ShadowMap.ParamName, parameter__ShadowMap.Serialize());
            jo.Add(parameter__ShadowColor.ParamName, parameter__ShadowColor.Value.ToJArray());
            jo.Add(parameter__TexAddShadowStrength.ParamName, parameter__TexAddShadowStrength.Value);
            if (parameter__ToonMaskMap != null && parameter__ToonMaskMap.Value != null) jo.Add(parameter__ToonMaskMap.ParamName, parameter__ToonMaskMap.Serialize());
            jo.Add(parameter__Cutoff.ParamName, parameter__Cutoff.Value);
            if (parameter__RoughnessMap != null && parameter__RoughnessMap.Value != null) jo.Add(parameter__RoughnessMap.ParamName, parameter__RoughnessMap.Serialize());
            jo.Add(parameter__Roughness.ParamName, parameter__Roughness.Value);
            jo.Add(parameter__Metallic.ParamName, parameter__Metallic.Value);
            if (parameter__MetallicGlossMap != null && parameter__MetallicGlossMap.Value != null) jo.Add(parameter__MetallicGlossMap.ParamName, parameter__MetallicGlossMap.Serialize());
            jo.Add(parameter__SpecColor.ParamName, parameter__SpecColor.Value.ToJArray());
            if (parameter__SpecGlossMap != null && parameter__SpecGlossMap.Value != null) jo.Add(parameter__SpecGlossMap.ParamName, parameter__SpecGlossMap.Serialize());
            jo.Add(parameter__UseToonSpec.ParamName, parameter__UseToonSpec.Value);
            if (parameter__ToonSpecMap != null && parameter__ToonSpecMap.Value != null) jo.Add(parameter__ToonSpecMap.ParamName, parameter__ToonSpecMap.Serialize());
            jo.Add(parameter__ToonSpecColor.ParamName, parameter__ToonSpecColor.Value.ToJArray());
            if (parameter__ToonSpecOptMap != null && parameter__ToonSpecOptMap.Value != null) jo.Add(parameter__ToonSpecOptMap.ParamName, parameter__ToonSpecOptMap.Serialize());
            jo.Add(parameter__ToonSpecOptMapST.ParamName, parameter__ToonSpecOptMapST.Value.ToJArray());
            jo.Add(parameter__ToonSpecGloss.ParamName, parameter__ToonSpecGloss.Value);
            jo.Add(parameter__ToonSpecFeatherLevel.ParamName, parameter__ToonSpecFeatherLevel.Value);
            jo.Add(parameter__ToonSpecMaskScale.ParamName, parameter__ToonSpecMaskScale.Value);
            jo.Add(parameter__ToonSpecMaskOffset.ParamName, parameter__ToonSpecMaskOffset.Value);
            jo.Add(parameter__UseToonHairSpec.ParamName, parameter__UseToonHairSpec.Value);
            jo.Add(parameter__ToonSpecAnisoHighLightPower_1st.ParamName, parameter__ToonSpecAnisoHighLightPower_1st.Value);
            jo.Add(parameter__ToonSpecAnisoHighLightPower_2nd.ParamName, parameter__ToonSpecAnisoHighLightPower_2nd.Value);
            jo.Add(parameter__ToonSpecAnisoHighLightStrength_1st.ParamName, parameter__ToonSpecAnisoHighLightStrength_1st.Value);
            jo.Add(parameter__ToonSpecAnisoHighLightStrength_2nd.ParamName, parameter__ToonSpecAnisoHighLightStrength_2nd.Value);
            jo.Add(parameter__ToonSpecShiftTangent_1st.ParamName, parameter__ToonSpecShiftTangent_1st.Value);
            jo.Add(parameter__ToonSpecShiftTangent_2nd.ParamName, parameter__ToonSpecShiftTangent_2nd.Value);
            jo.Add(parameter__BumpScale.ParamName, parameter__BumpScale.Value);
            if (parameter__BumpMap != null && parameter__BumpMap.Value != null) jo.Add(parameter__BumpMap.ParamName, parameter__BumpMap.Serialize());
            jo.Add(parameter__OcclusionStrength.ParamName, parameter__OcclusionStrength.Value);
            if (parameter__OcclusionMap != null && parameter__OcclusionMap.Value != null) jo.Add(parameter__OcclusionMap.ParamName, parameter__OcclusionMap.Serialize());
            jo.Add(parameter__Surface.ParamName, parameter__Surface.Value);
            jo.Add(parameter__Blend.ParamName, parameter__Blend.Value);
            jo.Add(parameter__AlphaClip.ParamName, parameter__AlphaClip.Value);
            jo.Add(parameter__SrcBlend.ParamName, parameter__SrcBlend.Value);
            jo.Add(parameter__DstBlend.ParamName, parameter__DstBlend.Value);
            jo.Add(parameter__ZWrite.ParamName, parameter__ZWrite.Value);
            jo.Add(parameter__Cull.ParamName, parameter__Cull.Value);
            jo.Add(parameter__ReceiveShadows.ParamName, parameter__ReceiveShadows.Value);
            jo.Add(parameter__ToonLightDivid_M.ParamName, parameter__ToonLightDivid_M.Value);
            jo.Add(parameter__ToonLightDivid_D.ParamName, parameter__ToonLightDivid_D.Value);
            jo.Add(parameter__ToonDiffuseBright.ParamName, parameter__ToonDiffuseBright.Value);
            jo.Add(parameter__BoundSharp.ParamName, parameter__BoundSharp.Value);
            jo.Add(parameter__DarkFaceColor.ParamName, parameter__DarkFaceColor.Value.ToJArray());
            jo.Add(parameter__DeepDarkColor.ParamName, parameter__DeepDarkColor.Value.ToJArray());
            jo.Add(parameter__SSSColor.ParamName, parameter__SSSColor.Value.ToJArray());
            jo.Add(parameter__SSSWeight.ParamName, parameter__SSSWeight.Value);
            jo.Add(parameter__SSSSize.ParamName, parameter__SSSSize.Value);
            jo.Add(parameter__SSForwardAtt.ParamName, parameter__SSForwardAtt.Value);
            if (parameter__ClearCoatMaskMap != null && parameter__ClearCoatMaskMap.Value != null) jo.Add(parameter__ClearCoatMaskMap.ParamName, parameter__ClearCoatMaskMap.Serialize());
            jo.Add(parameter__ClearCoatColor.ParamName, parameter__ClearCoatColor.Value.ToJArray());
            jo.Add(parameter__ClearCoatRange.ParamName, parameter__ClearCoatRange.Value);
            jo.Add(parameter__ClearCoatGloss.ParamName, parameter__ClearCoatGloss.Value);
            jo.Add(parameter__ClearCoatMult.ParamName, parameter__ClearCoatMult.Value);
            if (parameter__SpecularMaskMap != null && parameter__SpecularMaskMap.Value != null) jo.Add(parameter__SpecularMaskMap.ParamName, parameter__SpecularMaskMap.Serialize());
            jo.Add(parameter__SpecularColor.ParamName, parameter__SpecularColor.Value.ToJArray());
            jo.Add(parameter__SpecularRange.ParamName, parameter__SpecularRange.Value);
            jo.Add(parameter__SpecularMulti.ParamName, parameter__SpecularMulti.Value);
            jo.Add(parameter__SpecularGloss.ParamName, parameter__SpecularGloss.Value);
            if (parameter__ToonToPBRMap != null && parameter__ToonToPBRMap.Value != null) jo.Add(parameter__ToonToPBRMap.ParamName, parameter__ToonToPBRMap.Serialize());
            jo.Add(parameter__ToonToPBR.ParamName, parameter__ToonToPBR.Value);
            jo.Add(parameter__OutLineColor.ParamName, parameter__OutLineColor.Value.ToJArray());
            jo.Add(parameter__OutLineThickness.ParamName, parameter__OutLineThickness.Value);
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_ToonLit_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_ToonLit_Extra();
        }
    }
}
