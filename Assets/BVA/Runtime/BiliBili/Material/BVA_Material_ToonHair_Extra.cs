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
public class BVA_Material_ToonHair_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_ToonHair_Extra";
public const string SHADER_NAME = "Shader Graphs/Toon (Stylized Hair)";
public const string BASECOLOR = "_BaseColor";
public const string BASEMAP = "_BaseMap";
public const string SHADECOLOR = "_ShadeColor";
public const string SMOOTHNESS = "_Smoothness";
public const string NORMALMAP = "_NormalMap";
public const string HIGHTLIGHT = "_Hightlight";
public const string STRIPSHIFTMAP = "_StripShiftMap";
public const string STRIPSHIFTTILING = "_StripShiftTiling";
public const string FIRSTLAYERHIGHLIGHTCOLOR = "_FirstLayerHighlightColor";
public const string FIRSTLAYEREXPONENT = "_FirstLayerExponent";
public const string FIRSTLAYERSHIFTPOWER = "_FirstLayerShiftPower";
public const string FIRSTLAYERSHIFT = "_FirstLayerShift";
public const string SECONDLAYERHIGHLIGHTCOLOR = "_SecondLayerHighlightColor";
public const string SECONDLAYEREXPONENT = "_SecondLayerExponent";
public const string SECONDLAYERSHIFTPOWER = "_SecondLayerShiftPower";
public const string SECONDLAYERSHIFT = "_SecondLayerShift";
public const string TONNYLIGHTING = "_TonnyLighting";
public const string SPECULARHIGHLIGHTS = "_SPECULARHIGHLIGHTS";
public MaterialParam<Color> parameter_BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialParam<Color> parameter_ShadeColor = new MaterialParam<Color>(SHADECOLOR, Color.white);
public MaterialParam<float> parameter_Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialTextureParam parameter_HighlightColorMap = new MaterialTextureParam(HIGHTLIGHT);
public MaterialTextureParam parameter_StripShiftMap = new MaterialTextureParam(STRIPSHIFTMAP);
public MaterialParam<Vector4> parameter_StripShiftTiling = new MaterialParam<Vector4>(STRIPSHIFTTILING, Vector4.one);
public MaterialParam<Color> parameter_1stLayerHighlightColor = new MaterialParam<Color>(FIRSTLAYERHIGHLIGHTCOLOR, Color.white);
public MaterialParam<Vector4> parameter_1stLayerExponent = new MaterialParam<Vector4>(FIRSTLAYEREXPONENT, Vector4.one);
public MaterialParam<float> parameter_1stLayerShiftPower = new MaterialParam<float>(FIRSTLAYERSHIFTPOWER, 1.0f);
public MaterialParam<float> parameter_1stLayerShift = new MaterialParam<float>(FIRSTLAYERSHIFT, 1.0f);
public MaterialParam<Color> parameter_LayerHighlightColor = new MaterialParam<Color>(SECONDLAYERHIGHLIGHTCOLOR, Color.white);
public MaterialParam<Vector4> parameter_2ndLayerExponent = new MaterialParam<Vector4>(SECONDLAYEREXPONENT, Vector4.one);
public MaterialParam<float> parameter_2ndLayerShiftPower = new MaterialParam<float>(SECONDLAYERSHIFTPOWER, 1.0f);
public MaterialParam<float> parameter_2ndLayerShift = new MaterialParam<float>(SECONDLAYERSHIFT, 1.0f);
public MaterialParam<float> parameter_ToonyLighting = new MaterialParam<float>(TONNYLIGHTING, 1.0f);
public MaterialParam<float> parameter_SpecularHighlights = new MaterialParam<float>(SPECULARHIGHLIGHTS, 1.0f);
public BVA_Material_ToonHair_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_BaseColor.Value = material.GetColor(parameter_BaseColor.ParamName);
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
parameter_ShadeColor.Value = material.GetColor(parameter_ShadeColor.ParamName);
parameter_Smoothness.Value = material.GetFloat(parameter_Smoothness.ParamName);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportNormalTextureInfo(parameter_normalmap_temp);
var parameter_highlightcolormap_temp = material.GetTexture(parameter_HighlightColorMap.ParamName);
if (parameter_highlightcolormap_temp != null) parameter_HighlightColorMap.Value = exportTextureInfo(parameter_highlightcolormap_temp);
var parameter_stripshiftmap_temp = material.GetTexture(parameter_StripShiftMap.ParamName);
if (parameter_stripshiftmap_temp != null) parameter_StripShiftMap.Value = exportTextureInfo(parameter_stripshiftmap_temp);
parameter_StripShiftTiling.Value = material.GetVector(parameter_StripShiftTiling.ParamName);
parameter_1stLayerHighlightColor.Value = material.GetColor(parameter_1stLayerHighlightColor.ParamName);
parameter_1stLayerExponent.Value = material.GetVector(parameter_1stLayerExponent.ParamName);
parameter_1stLayerShiftPower.Value = material.GetFloat(parameter_1stLayerShiftPower.ParamName);
parameter_1stLayerShift.Value = material.GetFloat(parameter_1stLayerShift.ParamName);
parameter_LayerHighlightColor.Value = material.GetColor(parameter_LayerHighlightColor.ParamName);
parameter_2ndLayerExponent.Value = material.GetVector(parameter_2ndLayerExponent.ParamName);
parameter_2ndLayerShiftPower.Value = material.GetFloat(parameter_2ndLayerShiftPower.ParamName);
parameter_2ndLayerShift.Value = material.GetFloat(parameter_2ndLayerShift.ParamName);
parameter_ToonyLighting.Value = material.GetFloat(parameter_ToonyLighting.ParamName);
parameter_SpecularHighlights.Value = material.GetFloat(parameter_SpecularHighlights.ParamName);
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
case BVA_Material_ToonHair_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonHair_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonHair_Extra.BASEMAP, tex);
}
break;
case BVA_Material_ToonHair_Extra.SHADECOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.SHADECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonHair_Extra.SMOOTHNESS:
matCache.SetFloat(BVA_Material_ToonHair_Extra.SMOOTHNESS, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.NORMALMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonHair_Extra.NORMALMAP, tex);
}
break;
case BVA_Material_ToonHair_Extra.HIGHTLIGHT:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonHair_Extra.HIGHTLIGHT, tex);
}
break;
case BVA_Material_ToonHair_Extra.STRIPSHIFTMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonHair_Extra.STRIPSHIFTMAP, tex);
}
break;
case BVA_Material_ToonHair_Extra.STRIPSHIFTTILING:
matCache.SetVector(BVA_Material_ToonHair_Extra.STRIPSHIFTTILING, reader.ReadAsVector4().ToUnityVector4Raw());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYERHIGHLIGHTCOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.FIRSTLAYERHIGHLIGHTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYEREXPONENT:
matCache.SetVector(BVA_Material_ToonHair_Extra.FIRSTLAYEREXPONENT, reader.ReadAsVector4().ToUnityVector4Raw());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFTPOWER:
matCache.SetFloat(BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFTPOWER, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFT:
matCache.SetFloat(BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFT, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.SECONDLAYERHIGHLIGHTCOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.SECONDLAYERHIGHLIGHTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonHair_Extra.SECONDLAYEREXPONENT:
matCache.SetVector(BVA_Material_ToonHair_Extra.SECONDLAYEREXPONENT, reader.ReadAsVector4().ToUnityVector4Raw());
break;
case BVA_Material_ToonHair_Extra.SECONDLAYERSHIFTPOWER:
matCache.SetFloat(BVA_Material_ToonHair_Extra.SECONDLAYERSHIFTPOWER, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.SECONDLAYERSHIFT:
matCache.SetFloat(BVA_Material_ToonHair_Extra.SECONDLAYERSHIFT, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.TONNYLIGHTING:
matCache.SetFloat(BVA_Material_ToonHair_Extra.TONNYLIGHTING, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.SPECULARHIGHLIGHTS:
matCache.SetFloat(BVA_Material_ToonHair_Extra.SPECULARHIGHLIGHTS, reader.ReadAsFloat());
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(parameter_BaseColor.ParamName, parameter_BaseColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_BaseMap != null && parameter_BaseMap.Value != null) jo.Add(parameter_BaseMap.ParamName, parameter_BaseMap.Serialize());
jo.Add(parameter_ShadeColor.ParamName, parameter_ShadeColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Smoothness.ParamName, parameter_Smoothness.Value);
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
if (parameter_HighlightColorMap != null && parameter_HighlightColorMap.Value != null) jo.Add(parameter_HighlightColorMap.ParamName, parameter_HighlightColorMap.Serialize());
if (parameter_StripShiftMap != null && parameter_StripShiftMap.Value != null) jo.Add(parameter_StripShiftMap.ParamName, parameter_StripShiftMap.Serialize());
jo.Add(parameter_StripShiftTiling.ParamName, parameter_StripShiftTiling.Value.ToGltfVector4Raw().ToJArray());
jo.Add(parameter_1stLayerHighlightColor.ParamName, parameter_1stLayerHighlightColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_1stLayerExponent.ParamName, parameter_1stLayerExponent.Value.ToGltfVector4Raw().ToJArray());
jo.Add(parameter_1stLayerShiftPower.ParamName, parameter_1stLayerShiftPower.Value);
jo.Add(parameter_1stLayerShift.ParamName, parameter_1stLayerShift.Value);
jo.Add(parameter_LayerHighlightColor.ParamName, parameter_LayerHighlightColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_2ndLayerExponent.ParamName, parameter_2ndLayerExponent.Value.ToGltfVector4Raw().ToJArray());
jo.Add(parameter_2ndLayerShiftPower.ParamName, parameter_2ndLayerShiftPower.Value);
jo.Add(parameter_2ndLayerShift.ParamName, parameter_2ndLayerShift.Value);
jo.Add(parameter_ToonyLighting.ParamName, parameter_ToonyLighting.Value);
jo.Add(parameter_SpecularHighlights.ParamName, parameter_SpecularHighlights.Value);
return new JProperty(BVA_Material_ToonHair_Extra.SHADER_NAME, jo);
}
}
}
