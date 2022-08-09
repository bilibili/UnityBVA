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
public class BVA_Material_ToonRamp_Extra : MaterialExtra
{
public const string PROPERTY = "BVA_Material_ToonRamp_Extra";
public const string SHADER_NAME = "Shader Graphs/Toon (Texture Ramp)";
public const string BASECOLOR = "_BaseColor";
public const string BASEMAP = "_BaseMap";
public const string SSSCOLOR = "_SSSColor";
public const string SSSMAP = "_SSSMap";
public const string SMOOTHNESS = "_Smoothness";
public const string METALIC = "_Metalic";
public const string METALICMAP = "_MetalicMap";
public const string NORMALMAP = "_NormalMap";
public const string SHADESHIFT = "_ShadeShift";
public const string OCCLUSIONMAP = "_OcclusionMap";
public const string EMISSIONMAP = "_EmissionMap";
public const string EMISSIONCOLOR = "_EmissionColor";
public const string OUTLINEWIDTH = "_OutlineWidth";
public const string OUTLINEMAP = "_OutlineMap";
public const string SHADETOONY = "_ShadeToony";
public const string SHADERAMP = "_ShadeRamp";
public const string SHADEENVIRONMENTALCOLOR = "_ShadeEnvironmentalColor";
public const string CURVATURE = "_Curvature";
public const string TOONYLIGHTING = "_ToonyLighting";
public MaterialParam<Color> parameter_BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialParam<Color> parameter_SSSColor = new MaterialParam<Color>(SSSCOLOR, Color.white);
public MaterialTextureParam parameter_SSSMap = new MaterialTextureParam(SSSMAP);
public MaterialParam<float> parameter_Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
public MaterialParam<float> parameter_Metalic = new MaterialParam<float>(METALIC, 1.0f);
public MaterialTextureParam parameter_MetalicMap = new MaterialTextureParam(METALICMAP);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialParam<float> parameter_ShadeShift = new MaterialParam<float>(SHADESHIFT, 1.0f);
public MaterialTextureParam parameter_OcclusionMap = new MaterialTextureParam(OCCLUSIONMAP);
public MaterialTextureParam parameter_EmissionMap = new MaterialTextureParam(EMISSIONMAP);
public MaterialParam<Color> parameter_EmissionColor = new MaterialParam<Color>(EMISSIONCOLOR, Color.white);
public MaterialParam<float> parameter_OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
public MaterialTextureParam parameter_OutlineMap = new MaterialTextureParam(OUTLINEMAP);
public MaterialParam<float> parameter_ShadeToony = new MaterialParam<float>(SHADETOONY, 1.0f);
public MaterialTextureParam parameter_ShadeRamp = new MaterialTextureParam(SHADERAMP);
public MaterialParam<Color> parameter_ShadeEnvironmentalColor = new MaterialParam<Color>(SHADEENVIRONMENTALCOLOR, Color.white);
public MaterialParam<float> parameter_Curvature = new MaterialParam<float>(CURVATURE, 1.0f);
public MaterialParam<float> parameter_ToonyLighting = new MaterialParam<float>(TOONYLIGHTING, 1.0f);
public BVA_Material_ToonRamp_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_BaseColor.Value = material.GetColor(parameter_BaseColor.ParamName);
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
parameter_SSSColor.Value = material.GetColor(parameter_SSSColor.ParamName);
var parameter_sssmap_temp = material.GetTexture(parameter_SSSMap.ParamName);
if (parameter_sssmap_temp != null) parameter_SSSMap.Value = exportTextureInfo(parameter_sssmap_temp);
parameter_Smoothness.Value = material.GetFloat(parameter_Smoothness.ParamName);
parameter_Metalic.Value = material.GetFloat(parameter_Metalic.ParamName);
var parameter_metalicmap_temp = material.GetTexture(parameter_MetalicMap.ParamName);
if (parameter_metalicmap_temp != null) parameter_MetalicMap.Value = exportTextureInfo(parameter_metalicmap_temp);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportNormalTextureInfo(parameter_normalmap_temp);
parameter_ShadeShift.Value = material.GetFloat(parameter_ShadeShift.ParamName);
var parameter_occlusionmap_temp = material.GetTexture(parameter_OcclusionMap.ParamName);
if (parameter_occlusionmap_temp != null) parameter_OcclusionMap.Value = exportTextureInfo(parameter_occlusionmap_temp);
var parameter_emissionmap_temp = material.GetTexture(parameter_EmissionMap.ParamName);
if (parameter_emissionmap_temp != null) parameter_EmissionMap.Value = exportTextureInfo(parameter_emissionmap_temp);
parameter_EmissionColor.Value = material.GetColor(parameter_EmissionColor.ParamName);
parameter_OutlineWidth.Value = material.GetFloat(parameter_OutlineWidth.ParamName);
var parameter_outlinemap_temp = material.GetTexture(parameter_OutlineMap.ParamName);
if (parameter_outlinemap_temp != null) parameter_OutlineMap.Value = exportTextureInfo(parameter_outlinemap_temp);
parameter_ShadeToony.Value = material.GetFloat(parameter_ShadeToony.ParamName);
var parameter_shaderamp_temp = material.GetTexture(parameter_ShadeRamp.ParamName);
if (parameter_shaderamp_temp != null) parameter_ShadeRamp.Value = exportTextureInfo(parameter_shaderamp_temp);
parameter_ShadeEnvironmentalColor.Value = material.GetColor(parameter_ShadeEnvironmentalColor.ParamName);
parameter_Curvature.Value = material.GetFloat(parameter_Curvature.ParamName);
parameter_ToonyLighting.Value = material.GetFloat(parameter_ToonyLighting.ParamName);
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
case BVA_Material_ToonRamp_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ToonRamp_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonRamp_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.BASEMAP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.SSSCOLOR:
matCache.SetColor(BVA_Material_ToonRamp_Extra.SSSCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonRamp_Extra.SSSMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.SSSMAP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.SMOOTHNESS:
matCache.SetFloat(BVA_Material_ToonRamp_Extra.SMOOTHNESS, reader.ReadAsFloat());
break;
case BVA_Material_ToonRamp_Extra.METALIC:
matCache.SetFloat(BVA_Material_ToonRamp_Extra.METALIC, reader.ReadAsFloat());
break;
case BVA_Material_ToonRamp_Extra.METALICMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.METALICMAP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.NORMALMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.NORMALMAP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.SHADESHIFT:
matCache.SetFloat(BVA_Material_ToonRamp_Extra.SHADESHIFT, reader.ReadAsFloat());
break;
case BVA_Material_ToonRamp_Extra.OCCLUSIONMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.OCCLUSIONMAP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.EMISSIONMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.EMISSIONMAP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.EMISSIONCOLOR:
matCache.SetColor(BVA_Material_ToonRamp_Extra.EMISSIONCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonRamp_Extra.OUTLINEWIDTH:
matCache.SetFloat(BVA_Material_ToonRamp_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
break;
case BVA_Material_ToonRamp_Extra.OUTLINEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.OUTLINEMAP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.SHADETOONY:
matCache.SetFloat(BVA_Material_ToonRamp_Extra.SHADETOONY, reader.ReadAsFloat());
break;
case BVA_Material_ToonRamp_Extra.SHADERAMP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonRamp_Extra.SHADERAMP, tex);
}
break;
case BVA_Material_ToonRamp_Extra.SHADEENVIRONMENTALCOLOR:
matCache.SetColor(BVA_Material_ToonRamp_Extra.SHADEENVIRONMENTALCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonRamp_Extra.CURVATURE:
matCache.SetFloat(BVA_Material_ToonRamp_Extra.CURVATURE, reader.ReadAsFloat());
break;
case BVA_Material_ToonRamp_Extra.TOONYLIGHTING:
matCache.SetFloat(BVA_Material_ToonRamp_Extra.TOONYLIGHTING, reader.ReadAsFloat());
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
jo.Add(parameter_SSSColor.ParamName, parameter_SSSColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_SSSMap != null && parameter_SSSMap.Value != null) jo.Add(parameter_SSSMap.ParamName, parameter_SSSMap.Serialize());
jo.Add(parameter_Smoothness.ParamName, parameter_Smoothness.Value);
jo.Add(parameter_Metalic.ParamName, parameter_Metalic.Value);
if (parameter_MetalicMap != null && parameter_MetalicMap.Value != null) jo.Add(parameter_MetalicMap.ParamName, parameter_MetalicMap.Serialize());
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
jo.Add(parameter_ShadeShift.ParamName, parameter_ShadeShift.Value);
if (parameter_OcclusionMap != null && parameter_OcclusionMap.Value != null) jo.Add(parameter_OcclusionMap.ParamName, parameter_OcclusionMap.Serialize());
if (parameter_EmissionMap != null && parameter_EmissionMap.Value != null) jo.Add(parameter_EmissionMap.ParamName, parameter_EmissionMap.Serialize());
jo.Add(parameter_EmissionColor.ParamName, parameter_EmissionColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_OutlineWidth.ParamName, parameter_OutlineWidth.Value);
if (parameter_OutlineMap != null && parameter_OutlineMap.Value != null) jo.Add(parameter_OutlineMap.ParamName, parameter_OutlineMap.Serialize());
jo.Add(parameter_ShadeToony.ParamName, parameter_ShadeToony.Value);
if (parameter_ShadeRamp != null && parameter_ShadeRamp.Value != null) jo.Add(parameter_ShadeRamp.ParamName, parameter_ShadeRamp.Serialize());
jo.Add(parameter_ShadeEnvironmentalColor.ParamName, parameter_ShadeEnvironmentalColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Curvature.ParamName, parameter_Curvature.Value);
jo.Add(parameter_ToonyLighting.ParamName, parameter_ToonyLighting.Value);
return new JProperty(BVA_Material_ToonRamp_Extra.SHADER_NAME, jo);
}
}
}
