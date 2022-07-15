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
public class BVA_Material_ToonLit_Extra : MaterialDescriptor
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
public MaterialParam<float> parameter_WorkflowMode = new MaterialParam<float>(WORKFLOWMODE, 1.0f);
public MaterialTextureParam parameter_AlbedoMap = new MaterialTextureParam(BASEMAP);
public MaterialParam<Color> parameter_Color = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter_ShadowMap = new MaterialTextureParam(SHADOWMAP);
public MaterialParam<Color> parameter_ShadowColor = new MaterialParam<Color>(SHADOWCOLOR, Color.white);
public MaterialParam<float> parameter_TexAddShadowStrengh = new MaterialParam<float>(TEXADDSHADOWSTRENGTH, 1.0f);
public MaterialTextureParam parameter_ToonMaskMap = new MaterialTextureParam(TOONMASKMAP);
public MaterialParam<float> parameter_AlphaCutoff = new MaterialParam<float>(CUTOFF, 1.0f);
public MaterialTextureParam parameter_RoughnessMap = new MaterialTextureParam(ROUGHNESSMAP);
public MaterialParam<float> parameter_Roughness = new MaterialParam<float>(ROUGHNESS, 1.0f);
public MaterialParam<float> parameter_Metallic = new MaterialParam<float>(METALLIC, 1.0f);
public MaterialTextureParam parameter_MetallicGlossMap = new MaterialTextureParam(METALLICGLOSSMAP);
public MaterialParam<Color> parameter_Specular = new MaterialParam<Color>(SPECCOLOR, Color.white);
public MaterialTextureParam parameter_SpecGlossMap = new MaterialTextureParam(SPECGLOSSMAP);
public MaterialParam<float> parameter_UseToonSpecular = new MaterialParam<float>(USETOONSPEC, 1.0f);
public MaterialTextureParam parameter_ToonSpecMap = new MaterialTextureParam(TOONSPECMAP);
public MaterialParam<Color> parameter_ToonSpecColor = new MaterialParam<Color>(TOONSPECCOLOR, Color.white);
public MaterialTextureParam parameter_ToonSpecOptMap = new MaterialTextureParam(TOONSPECOPTMAP);
public MaterialParam<Vector4> parameter_ToonSpecOptMapST = new MaterialParam<Vector4>(TOONSPECOPTMAPST, Vector4.one);
public MaterialParam<float> parameter_ToonSpecGloss = new MaterialParam<float>(TOONSPECGLOSS, 1.0f);
public MaterialParam<float> parameter_ToonSpecFeatherLevel = new MaterialParam<float>(TOONSPECFEATHERLEVEL, 1.0f);
public MaterialParam<float> parameter_ToonSpecMaskScale = new MaterialParam<float>(TOONSPECMASKSCALE, 1.0f);
public MaterialParam<float> parameter_ToonSpecMaskOffset = new MaterialParam<float>(TOONSPECMASKOFFSET, 1.0f);
public MaterialParam<float> parameter_UseToonHairSpecular = new MaterialParam<float>(USETOONHAIRSPEC, 1.0f);
public MaterialParam<float> parameter_ToonSpecAnisoHighLightPower1stHairSpec = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTPOWERFIRST, 1.0f);
public MaterialParam<float> parameter_ToonSpecAnisoHighLightPower2ndHairSpec = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTPOWERSECOND, 1.0f);
public MaterialParam<float> parameter_ToonSpecAnisoHighLightStrength1stHairSpec = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTSTRENGTHFIRST, 1.0f);
public MaterialParam<float> parameter_ToonSpecAnisoHighLightStrength2ndHairSpec = new MaterialParam<float>(TOONSPECANISOHIGHLIGHTSTRENGTHSECOND, 1.0f);
public MaterialParam<float> parameter_ToonSpecShiftTangent1stHairSpec = new MaterialParam<float>(TOONSPECSHIFTTANGENTFIRST, 1.0f);
public MaterialParam<float> parameter_ToonSpecShiftTangent2ndHairSpec = new MaterialParam<float>(TOONSPECSHIFTTANGENTSECOND, 1.0f);
public MaterialParam<float> parameter_Scale = new MaterialParam<float>(BUMPSCALE, 1.0f);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(BUMPMAP);
public MaterialParam<float> parameter_OcclusionStrength = new MaterialParam<float>(OCCLUSIONSTRENGTH, 1.0f);
public MaterialTextureParam parameter_OcclusionMap = new MaterialTextureParam(OCCLUSIONMAP);
public MaterialParam<float> parameter___surface = new MaterialParam<float>(SURFACE, 1.0f);
public MaterialParam<float> parameter___blend = new MaterialParam<float>(BLEND, 1.0f);
public MaterialParam<float> parameter___clip = new MaterialParam<float>(ALPHACLIP, 1.0f);
public MaterialParam<float> parameter___src = new MaterialParam<float>(SRCBLEND, 1.0f);
public MaterialParam<float> parameter___dst = new MaterialParam<float>(DSTBLEND, 1.0f);
public MaterialParam<float> parameter___zw = new MaterialParam<float>(ZWRITE, 1.0f);
public MaterialParam<float> parameter___cull = new MaterialParam<float>(CULL, 1.0f);
public MaterialParam<float> parameter_ReceiveShadows = new MaterialParam<float>(RECEIVESHADOWS, 1.0f);
public MaterialParam<float> parameter_ToonLightDividM = new MaterialParam<float>(TOONLIGHTDIVIDM, 1.0f);
public MaterialParam<float> parameter_ToonLightDividD = new MaterialParam<float>(TOONLIGHTDIVIDD, 1.0f);
public MaterialParam<float> parameter_ToonDiffuseBrightness = new MaterialParam<float>(TOONDIFFUSEBRIGHT, 1.0f);
public MaterialParam<float> parameter__BoundSharp = new MaterialParam<float>(BOUNDSHARP, 1.0f);
public MaterialParam<Color> parameter_ToonColorofDarkFace = new MaterialParam<Color>(DARKFACECOLOR, Color.white);
public MaterialParam<Color> parameter_ToonColorofDeepDarkFace = new MaterialParam<Color>(DEEPDARKCOLOR, Color.white);
public MaterialParam<Color> parameter_SubsurfaceScatteringColor = new MaterialParam<Color>(SSSCOLOR, Color.white);
public MaterialParam<float> parameter_WeightofSSS = new MaterialParam<float>(SSSWEIGHT, 1.0f);
public MaterialParam<float> parameter_SizeofSSS = new MaterialParam<float>(SSSSIZE, 1.0f);
public MaterialParam<float> parameter_AttenofSSinforwardDir = new MaterialParam<float>(SSFORWARDATT, 1.0f);
public MaterialTextureParam parameter_ClearCoatMaskMap = new MaterialTextureParam(CLEARCOATMASKMAP);
public MaterialParam<Color> parameter_ClearCoatColor = new MaterialParam<Color>(CLEARCOATCOLOR, Color.white);
public MaterialParam<float> parameter_ClearCoatRange = new MaterialParam<float>(CLEARCOATRANGE, 1.0f);
public MaterialParam<float> parameter_ClearCoatGloss = new MaterialParam<float>(CLEARCOATGLOSS, 1.0f);
public MaterialParam<float> parameter_ClearCoatMult = new MaterialParam<float>(CLEARCOATMULT, 1.0f);
public MaterialTextureParam parameter_SpecularMaskMap = new MaterialTextureParam(SPECULARMASKMAP);
public MaterialParam<Color> parameter_SpecularColor = new MaterialParam<Color>(SPECULARCOLOR, Color.white);
public MaterialParam<float> parameter_SpecularRange = new MaterialParam<float>(SPECULARRANGE, 1.0f);
public MaterialParam<float> parameter_SpecularMulti = new MaterialParam<float>(SPECULARMULTI, 1.0f);
public MaterialParam<float> parameter_SprecularGloss = new MaterialParam<float>(SPECULARGLOSS, 1.0f);
public MaterialTextureParam parameter_ToonToPBRMap = new MaterialTextureParam(TOONTOPBRMAP);
public MaterialParam<float> parameter_ToonToPBR = new MaterialParam<float>(TOONTOPBR, 1.0f);
public MaterialParam<Color> parameter_OutLineColor = new MaterialParam<Color>(OUTLINECOLOR, Color.white);
public MaterialParam<float> parameter_OutLineThickness = new MaterialParam<float>(OUTLINETHICKNESS, 1.0f);
public BVA_Material_ToonLit_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_WorkflowMode.Value = material.GetFloat(parameter_WorkflowMode.ParamName);
var parameter_albedomap_temp = material.GetTexture(parameter_AlbedoMap.ParamName);
if (parameter_albedomap_temp != null) parameter_AlbedoMap.Value = exportTextureInfo(parameter_albedomap_temp);
parameter_Color.Value = material.GetColor(parameter_Color.ParamName);
var parameter_shadowmap_temp = material.GetTexture(parameter_ShadowMap.ParamName);
if (parameter_shadowmap_temp != null) parameter_ShadowMap.Value = exportTextureInfo(parameter_shadowmap_temp);
parameter_ShadowColor.Value = material.GetColor(parameter_ShadowColor.ParamName);
parameter_TexAddShadowStrengh.Value = material.GetFloat(parameter_TexAddShadowStrengh.ParamName);
var parameter_toonmaskmap_temp = material.GetTexture(parameter_ToonMaskMap.ParamName);
if (parameter_toonmaskmap_temp != null) parameter_ToonMaskMap.Value = exportTextureInfo(parameter_toonmaskmap_temp);
parameter_AlphaCutoff.Value = material.GetFloat(parameter_AlphaCutoff.ParamName);
var parameter_roughnessmap_temp = material.GetTexture(parameter_RoughnessMap.ParamName);
if (parameter_roughnessmap_temp != null) parameter_RoughnessMap.Value = exportTextureInfo(parameter_roughnessmap_temp);
parameter_Roughness.Value = material.GetFloat(parameter_Roughness.ParamName);
parameter_Metallic.Value = material.GetFloat(parameter_Metallic.ParamName);
var parameter_metallicglossmap_temp = material.GetTexture(parameter_MetallicGlossMap.ParamName);
if (parameter_metallicglossmap_temp != null) parameter_MetallicGlossMap.Value = exportTextureInfo(parameter_metallicglossmap_temp);
parameter_Specular.Value = material.GetColor(parameter_Specular.ParamName);
var parameter_specglossmap_temp = material.GetTexture(parameter_SpecGlossMap.ParamName);
if (parameter_specglossmap_temp != null) parameter_SpecGlossMap.Value = exportTextureInfo(parameter_specglossmap_temp);
parameter_UseToonSpecular.Value = material.GetFloat(parameter_UseToonSpecular.ParamName);
var parameter_toonspecmap_temp = material.GetTexture(parameter_ToonSpecMap.ParamName);
if (parameter_toonspecmap_temp != null) parameter_ToonSpecMap.Value = exportTextureInfo(parameter_toonspecmap_temp);
parameter_ToonSpecColor.Value = material.GetColor(parameter_ToonSpecColor.ParamName);
var parameter_toonspecoptmap_temp = material.GetTexture(parameter_ToonSpecOptMap.ParamName);
if (parameter_toonspecoptmap_temp != null) parameter_ToonSpecOptMap.Value = exportTextureInfo(parameter_toonspecoptmap_temp);
parameter_ToonSpecOptMapST.Value = material.GetVector(parameter_ToonSpecOptMapST.ParamName);
parameter_ToonSpecGloss.Value = material.GetFloat(parameter_ToonSpecGloss.ParamName);
parameter_ToonSpecFeatherLevel.Value = material.GetFloat(parameter_ToonSpecFeatherLevel.ParamName);
parameter_ToonSpecMaskScale.Value = material.GetFloat(parameter_ToonSpecMaskScale.ParamName);
parameter_ToonSpecMaskOffset.Value = material.GetFloat(parameter_ToonSpecMaskOffset.ParamName);
parameter_UseToonHairSpecular.Value = material.GetFloat(parameter_UseToonHairSpecular.ParamName);
parameter_ToonSpecAnisoHighLightPower1stHairSpec.Value = material.GetFloat(parameter_ToonSpecAnisoHighLightPower1stHairSpec.ParamName);
parameter_ToonSpecAnisoHighLightPower2ndHairSpec.Value = material.GetFloat(parameter_ToonSpecAnisoHighLightPower2ndHairSpec.ParamName);
parameter_ToonSpecAnisoHighLightStrength1stHairSpec.Value = material.GetFloat(parameter_ToonSpecAnisoHighLightStrength1stHairSpec.ParamName);
parameter_ToonSpecAnisoHighLightStrength2ndHairSpec.Value = material.GetFloat(parameter_ToonSpecAnisoHighLightStrength2ndHairSpec.ParamName);
parameter_ToonSpecShiftTangent1stHairSpec.Value = material.GetFloat(parameter_ToonSpecShiftTangent1stHairSpec.ParamName);
parameter_ToonSpecShiftTangent2ndHairSpec.Value = material.GetFloat(parameter_ToonSpecShiftTangent2ndHairSpec.ParamName);
parameter_Scale.Value = material.GetFloat(parameter_Scale.ParamName);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportNormalTextureInfo(parameter_normalmap_temp);
parameter_OcclusionStrength.Value = material.GetFloat(parameter_OcclusionStrength.ParamName);
var parameter_occlusionmap_temp = material.GetTexture(parameter_OcclusionMap.ParamName);
if (parameter_occlusionmap_temp != null) parameter_OcclusionMap.Value = exportTextureInfo(parameter_occlusionmap_temp);
parameter___surface.Value = material.GetFloat(parameter___surface.ParamName);
parameter___blend.Value = material.GetFloat(parameter___blend.ParamName);
parameter___clip.Value = material.GetFloat(parameter___clip.ParamName);
parameter___src.Value = material.GetFloat(parameter___src.ParamName);
parameter___dst.Value = material.GetFloat(parameter___dst.ParamName);
parameter___zw.Value = material.GetFloat(parameter___zw.ParamName);
parameter___cull.Value = material.GetFloat(parameter___cull.ParamName);
parameter_ReceiveShadows.Value = material.GetFloat(parameter_ReceiveShadows.ParamName);
parameter_ToonLightDividM.Value = material.GetFloat(parameter_ToonLightDividM.ParamName);
parameter_ToonLightDividD.Value = material.GetFloat(parameter_ToonLightDividD.ParamName);
parameter_ToonDiffuseBrightness.Value = material.GetFloat(parameter_ToonDiffuseBrightness.ParamName);
parameter__BoundSharp.Value = material.GetFloat(parameter__BoundSharp.ParamName);
parameter_ToonColorofDarkFace.Value = material.GetColor(parameter_ToonColorofDarkFace.ParamName);
parameter_ToonColorofDeepDarkFace.Value = material.GetColor(parameter_ToonColorofDeepDarkFace.ParamName);
parameter_SubsurfaceScatteringColor.Value = material.GetColor(parameter_SubsurfaceScatteringColor.ParamName);
parameter_WeightofSSS.Value = material.GetFloat(parameter_WeightofSSS.ParamName);
parameter_SizeofSSS.Value = material.GetFloat(parameter_SizeofSSS.ParamName);
parameter_AttenofSSinforwardDir.Value = material.GetFloat(parameter_AttenofSSinforwardDir.ParamName);
var parameter_clearcoatmaskmap_temp = material.GetTexture(parameter_ClearCoatMaskMap.ParamName);
if (parameter_clearcoatmaskmap_temp != null) parameter_ClearCoatMaskMap.Value = exportTextureInfo(parameter_clearcoatmaskmap_temp);
parameter_ClearCoatColor.Value = material.GetColor(parameter_ClearCoatColor.ParamName);
parameter_ClearCoatRange.Value = material.GetFloat(parameter_ClearCoatRange.ParamName);
parameter_ClearCoatGloss.Value = material.GetFloat(parameter_ClearCoatGloss.ParamName);
parameter_ClearCoatMult.Value = material.GetFloat(parameter_ClearCoatMult.ParamName);
var parameter_specularmaskmap_temp = material.GetTexture(parameter_SpecularMaskMap.ParamName);
if (parameter_specularmaskmap_temp != null) parameter_SpecularMaskMap.Value = exportTextureInfo(parameter_specularmaskmap_temp);
parameter_SpecularColor.Value = material.GetColor(parameter_SpecularColor.ParamName);
parameter_SpecularRange.Value = material.GetFloat(parameter_SpecularRange.ParamName);
parameter_SpecularMulti.Value = material.GetFloat(parameter_SpecularMulti.ParamName);
parameter_SprecularGloss.Value = material.GetFloat(parameter_SprecularGloss.ParamName);
var parameter_toontopbrmap_temp = material.GetTexture(parameter_ToonToPBRMap.ParamName);
if (parameter_toontopbrmap_temp != null) parameter_ToonToPBRMap.Value = exportTextureInfo(parameter_toontopbrmap_temp);
parameter_ToonToPBR.Value = material.GetFloat(parameter_ToonToPBR.ParamName);
parameter_OutLineColor.Value = material.GetColor(parameter_OutLineColor.ParamName);
parameter_OutLineThickness.Value = material.GetFloat(parameter_OutLineThickness.ParamName);
}
public static async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache,AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
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
matCache.SetColor(BVA_Material_ToonLit_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonLit_Extra.SHADOWMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonLit_Extra.SHADOWMAP, tex);
}
break;
case BVA_Material_ToonLit_Extra.SHADOWCOLOR:
matCache.SetColor(BVA_Material_ToonLit_Extra.SHADOWCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
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
matCache.SetColor(BVA_Material_ToonLit_Extra.SPECCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
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
matCache.SetColor(BVA_Material_ToonLit_Extra.TOONSPECCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonLit_Extra.TOONSPECOPTMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonLit_Extra.TOONSPECOPTMAP, tex);
}
break;
case BVA_Material_ToonLit_Extra.TOONSPECOPTMAPST:
matCache.SetVector(BVA_Material_ToonLit_Extra.TOONSPECOPTMAPST, reader.ReadAsVector4().ToUnityVector4Raw());
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
var tex = await loadNormalMap(texInfo.Index);
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
matCache.SetColor(BVA_Material_ToonLit_Extra.DARKFACECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonLit_Extra.DEEPDARKCOLOR:
matCache.SetColor(BVA_Material_ToonLit_Extra.DEEPDARKCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonLit_Extra.SSSCOLOR:
matCache.SetColor(BVA_Material_ToonLit_Extra.SSSCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
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
matCache.SetColor(BVA_Material_ToonLit_Extra.CLEARCOATCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
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
matCache.SetColor(BVA_Material_ToonLit_Extra.SPECULARCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
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
matCache.SetColor(BVA_Material_ToonLit_Extra.OUTLINECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonLit_Extra.OUTLINETHICKNESS:
matCache.SetFloat(BVA_Material_ToonLit_Extra.OUTLINETHICKNESS, reader.ReadAsFloat());
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(parameter_WorkflowMode.ParamName, parameter_WorkflowMode.Value);
if (parameter_AlbedoMap != null && parameter_AlbedoMap.Value != null) jo.Add(parameter_AlbedoMap.ParamName, parameter_AlbedoMap.Serialize());
jo.Add(parameter_Color.ParamName, parameter_Color.Value.ToNumericsColorRaw().ToJArray());
if (parameter_ShadowMap != null && parameter_ShadowMap.Value != null) jo.Add(parameter_ShadowMap.ParamName, parameter_ShadowMap.Serialize());
jo.Add(parameter_ShadowColor.ParamName, parameter_ShadowColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_TexAddShadowStrengh.ParamName, parameter_TexAddShadowStrengh.Value);
if (parameter_ToonMaskMap != null && parameter_ToonMaskMap.Value != null) jo.Add(parameter_ToonMaskMap.ParamName, parameter_ToonMaskMap.Serialize());
jo.Add(parameter_AlphaCutoff.ParamName, parameter_AlphaCutoff.Value);
if (parameter_RoughnessMap != null && parameter_RoughnessMap.Value != null) jo.Add(parameter_RoughnessMap.ParamName, parameter_RoughnessMap.Serialize());
jo.Add(parameter_Roughness.ParamName, parameter_Roughness.Value);
jo.Add(parameter_Metallic.ParamName, parameter_Metallic.Value);
if (parameter_MetallicGlossMap != null && parameter_MetallicGlossMap.Value != null) jo.Add(parameter_MetallicGlossMap.ParamName, parameter_MetallicGlossMap.Serialize());
jo.Add(parameter_Specular.ParamName, parameter_Specular.Value.ToNumericsColorRaw().ToJArray());
if (parameter_SpecGlossMap != null && parameter_SpecGlossMap.Value != null) jo.Add(parameter_SpecGlossMap.ParamName, parameter_SpecGlossMap.Serialize());
jo.Add(parameter_UseToonSpecular.ParamName, parameter_UseToonSpecular.Value);
if (parameter_ToonSpecMap != null && parameter_ToonSpecMap.Value != null) jo.Add(parameter_ToonSpecMap.ParamName, parameter_ToonSpecMap.Serialize());
jo.Add(parameter_ToonSpecColor.ParamName, parameter_ToonSpecColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_ToonSpecOptMap != null && parameter_ToonSpecOptMap.Value != null) jo.Add(parameter_ToonSpecOptMap.ParamName, parameter_ToonSpecOptMap.Serialize());
jo.Add(parameter_ToonSpecOptMapST.ParamName, parameter_ToonSpecOptMapST.Value.ToGltfVector4Raw().ToJArray());
jo.Add(parameter_ToonSpecGloss.ParamName, parameter_ToonSpecGloss.Value);
jo.Add(parameter_ToonSpecFeatherLevel.ParamName, parameter_ToonSpecFeatherLevel.Value);
jo.Add(parameter_ToonSpecMaskScale.ParamName, parameter_ToonSpecMaskScale.Value);
jo.Add(parameter_ToonSpecMaskOffset.ParamName, parameter_ToonSpecMaskOffset.Value);
jo.Add(parameter_UseToonHairSpecular.ParamName, parameter_UseToonHairSpecular.Value);
jo.Add(parameter_ToonSpecAnisoHighLightPower1stHairSpec.ParamName, parameter_ToonSpecAnisoHighLightPower1stHairSpec.Value);
jo.Add(parameter_ToonSpecAnisoHighLightPower2ndHairSpec.ParamName, parameter_ToonSpecAnisoHighLightPower2ndHairSpec.Value);
jo.Add(parameter_ToonSpecAnisoHighLightStrength1stHairSpec.ParamName, parameter_ToonSpecAnisoHighLightStrength1stHairSpec.Value);
jo.Add(parameter_ToonSpecAnisoHighLightStrength2ndHairSpec.ParamName, parameter_ToonSpecAnisoHighLightStrength2ndHairSpec.Value);
jo.Add(parameter_ToonSpecShiftTangent1stHairSpec.ParamName, parameter_ToonSpecShiftTangent1stHairSpec.Value);
jo.Add(parameter_ToonSpecShiftTangent2ndHairSpec.ParamName, parameter_ToonSpecShiftTangent2ndHairSpec.Value);
jo.Add(parameter_Scale.ParamName, parameter_Scale.Value);
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
jo.Add(parameter_OcclusionStrength.ParamName, parameter_OcclusionStrength.Value);
if (parameter_OcclusionMap != null && parameter_OcclusionMap.Value != null) jo.Add(parameter_OcclusionMap.ParamName, parameter_OcclusionMap.Serialize());
jo.Add(parameter___surface.ParamName, parameter___surface.Value);
jo.Add(parameter___blend.ParamName, parameter___blend.Value);
jo.Add(parameter___clip.ParamName, parameter___clip.Value);
jo.Add(parameter___src.ParamName, parameter___src.Value);
jo.Add(parameter___dst.ParamName, parameter___dst.Value);
jo.Add(parameter___zw.ParamName, parameter___zw.Value);
jo.Add(parameter___cull.ParamName, parameter___cull.Value);
jo.Add(parameter_ReceiveShadows.ParamName, parameter_ReceiveShadows.Value);
jo.Add(parameter_ToonLightDividM.ParamName, parameter_ToonLightDividM.Value);
jo.Add(parameter_ToonLightDividD.ParamName, parameter_ToonLightDividD.Value);
jo.Add(parameter_ToonDiffuseBrightness.ParamName, parameter_ToonDiffuseBrightness.Value);
jo.Add(parameter__BoundSharp.ParamName, parameter__BoundSharp.Value);
jo.Add(parameter_ToonColorofDarkFace.ParamName, parameter_ToonColorofDarkFace.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_ToonColorofDeepDarkFace.ParamName, parameter_ToonColorofDeepDarkFace.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_SubsurfaceScatteringColor.ParamName, parameter_SubsurfaceScatteringColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_WeightofSSS.ParamName, parameter_WeightofSSS.Value);
jo.Add(parameter_SizeofSSS.ParamName, parameter_SizeofSSS.Value);
jo.Add(parameter_AttenofSSinforwardDir.ParamName, parameter_AttenofSSinforwardDir.Value);
if (parameter_ClearCoatMaskMap != null && parameter_ClearCoatMaskMap.Value != null) jo.Add(parameter_ClearCoatMaskMap.ParamName, parameter_ClearCoatMaskMap.Serialize());
jo.Add(parameter_ClearCoatColor.ParamName, parameter_ClearCoatColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_ClearCoatRange.ParamName, parameter_ClearCoatRange.Value);
jo.Add(parameter_ClearCoatGloss.ParamName, parameter_ClearCoatGloss.Value);
jo.Add(parameter_ClearCoatMult.ParamName, parameter_ClearCoatMult.Value);
if (parameter_SpecularMaskMap != null && parameter_SpecularMaskMap.Value != null) jo.Add(parameter_SpecularMaskMap.ParamName, parameter_SpecularMaskMap.Serialize());
jo.Add(parameter_SpecularColor.ParamName, parameter_SpecularColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_SpecularRange.ParamName, parameter_SpecularRange.Value);
jo.Add(parameter_SpecularMulti.ParamName, parameter_SpecularMulti.Value);
jo.Add(parameter_SprecularGloss.ParamName, parameter_SprecularGloss.Value);
if (parameter_ToonToPBRMap != null && parameter_ToonToPBRMap.Value != null) jo.Add(parameter_ToonToPBRMap.ParamName, parameter_ToonToPBRMap.Serialize());
jo.Add(parameter_ToonToPBR.ParamName, parameter_ToonToPBR.Value);
jo.Add(parameter_OutLineColor.ParamName, parameter_OutLineColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_OutLineThickness.ParamName, parameter_OutLineThickness.Value);
return new JProperty(BVA_Material_ToonLit_Extra.SHADER_NAME, jo);
}
}
}
