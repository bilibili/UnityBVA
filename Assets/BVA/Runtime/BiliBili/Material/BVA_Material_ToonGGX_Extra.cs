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
public class BVA_Material_ToonGGX_Extra : MaterialExtra
{
public const string PROPERTY = "BVA_Material_ToonGGX_Extra";
public const string SHADER_NAME = "Shader Graphs/Toon (GGX)";
public const string BASECOLOR = "_BaseColor";
public const string BASEMAP = "_BaseMap";
public const string SSSMAP = "_SSSMap";
public const string ENVIRONMENTCOLOR = "_EnvironmentColor";
public const string HIGHLIGHTCOLOR = "_HighlightColor";
public const string NORMALMAP = "_NormalMap";
public const string ILLIMINATIONMAP = "_IlliminationMap";
public const string OUTLINEWIDTH = "_OutlineWidth";
public const string TONNYLIGHTING = "_TonnyLighting";
public const string CURVATURE = "_Curvature";
public const string WIDTHSCALEDMAXDISTANCE = "_WidthScaledMaxDistance";
public MaterialParam<Color> parameter_BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialTextureParam parameter_SssMap = new MaterialTextureParam(SSSMAP);
public MaterialParam<Color> parameter_EnviromentalColor = new MaterialParam<Color>(ENVIRONMENTCOLOR, Color.white);
public MaterialParam<Color> parameter_HighlightColor = new MaterialParam<Color>(HIGHLIGHTCOLOR, Color.white);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialTextureParam parameter_IlliminationMap = new MaterialTextureParam(ILLIMINATIONMAP);
public MaterialParam<float> parameter_OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
public MaterialParam<float> parameter_ToonyLighting = new MaterialParam<float>(TONNYLIGHTING, 1.0f);
public MaterialParam<float> parameter_Curvature = new MaterialParam<float>(CURVATURE, 1.0f);
public MaterialParam<float> parameter_WidthScaledMaxDistance = new MaterialParam<float>(WIDTHSCALEDMAXDISTANCE, 1.0f);
public BVA_Material_ToonGGX_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_BaseColor.Value = material.GetColor(parameter_BaseColor.ParamName);
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
var parameter_sssmap_temp = material.GetTexture(parameter_SssMap.ParamName);
if (parameter_sssmap_temp != null) parameter_SssMap.Value = exportTextureInfo(parameter_sssmap_temp);
parameter_EnviromentalColor.Value = material.GetColor(parameter_EnviromentalColor.ParamName);
parameter_HighlightColor.Value = material.GetColor(parameter_HighlightColor.ParamName);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportNormalTextureInfo(parameter_normalmap_temp);
var parameter_illiminationmap_temp = material.GetTexture(parameter_IlliminationMap.ParamName);
if (parameter_illiminationmap_temp != null) parameter_IlliminationMap.Value = exportTextureInfo(parameter_illiminationmap_temp);
parameter_OutlineWidth.Value = material.GetFloat(parameter_OutlineWidth.ParamName);
parameter_ToonyLighting.Value = material.GetFloat(parameter_ToonyLighting.ParamName);
parameter_Curvature.Value = material.GetFloat(parameter_Curvature.ParamName);
parameter_WidthScaledMaxDistance.Value = material.GetFloat(parameter_WidthScaledMaxDistance.ParamName);
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
case BVA_Material_ToonGGX_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ToonGGX_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonGGX_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonGGX_Extra.BASEMAP, tex);
}
break;
case BVA_Material_ToonGGX_Extra.SSSMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonGGX_Extra.SSSMAP, tex);
}
break;
case BVA_Material_ToonGGX_Extra.ENVIRONMENTCOLOR:
matCache.SetColor(BVA_Material_ToonGGX_Extra.ENVIRONMENTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonGGX_Extra.HIGHLIGHTCOLOR:
matCache.SetColor(BVA_Material_ToonGGX_Extra.HIGHLIGHTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonGGX_Extra.NORMALMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonGGX_Extra.NORMALMAP, tex);
}
break;
case BVA_Material_ToonGGX_Extra.ILLIMINATIONMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonGGX_Extra.ILLIMINATIONMAP, tex);
}
break;
case BVA_Material_ToonGGX_Extra.OUTLINEWIDTH:
matCache.SetFloat(BVA_Material_ToonGGX_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
break;
case BVA_Material_ToonGGX_Extra.TONNYLIGHTING:
matCache.SetFloat(BVA_Material_ToonGGX_Extra.TONNYLIGHTING, reader.ReadAsFloat());
break;
case BVA_Material_ToonGGX_Extra.CURVATURE:
matCache.SetFloat(BVA_Material_ToonGGX_Extra.CURVATURE, reader.ReadAsFloat());
break;
case BVA_Material_ToonGGX_Extra.WIDTHSCALEDMAXDISTANCE:
matCache.SetFloat(BVA_Material_ToonGGX_Extra.WIDTHSCALEDMAXDISTANCE, reader.ReadAsFloat());
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
if (parameter_SssMap != null && parameter_SssMap.Value != null) jo.Add(parameter_SssMap.ParamName, parameter_SssMap.Serialize());
jo.Add(parameter_EnviromentalColor.ParamName, parameter_EnviromentalColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_HighlightColor.ParamName, parameter_HighlightColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
if (parameter_IlliminationMap != null && parameter_IlliminationMap.Value != null) jo.Add(parameter_IlliminationMap.ParamName, parameter_IlliminationMap.Serialize());
jo.Add(parameter_OutlineWidth.ParamName, parameter_OutlineWidth.Value);
jo.Add(parameter_ToonyLighting.ParamName, parameter_ToonyLighting.Value);
jo.Add(parameter_Curvature.ParamName, parameter_Curvature.Value);
jo.Add(parameter_WidthScaledMaxDistance.ParamName, parameter_WidthScaledMaxDistance.Value);
return new JProperty(BVA_Material_ToonGGX_Extra.SHADER_NAME, jo);
}
}
}
