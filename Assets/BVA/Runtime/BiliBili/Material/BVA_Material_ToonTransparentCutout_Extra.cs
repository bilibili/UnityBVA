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
public class BVA_Material_ToonTransparentCutout_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_ToonTransparentCutout_Extra";
public const string SHADER_NAME = "Shader Graphs/Toon (TransparentCutout)";
public const string BASECOLOR = "_BaseColor";
public const string BASEMAP = "_BaseMap";
public const string SHADEENVIRONMENTALCOLOR = "_ShadeEnvironmentalColor";
public const string SHADECOLOR = "_ShadeColor";
public const string SHADEMAP = "_ShadeMap";
public const string SMOOTHNESS = "_Smoothness";
public const string METALIC = "_Metalic";
public const string METALICMAP = "_MetalicMap";
public const string NORMALMAP = "_NormalMap";
public const string OCCLUSIONMAP = "_OcclusionMap";
public const string OUTLINEWIDTH = "_OutlineWidth";
public const string OUTLINEMAP = "_OutlineMap";
public const string SHADETOONY = "_ShadeToony";
public const string TOONYLIGHTING = "_ToonyLighting";
public const string EMISSIONMAP = "_EmissionMap";
public const string EMISSIONCOLOR = "_EmissionColor";
public MaterialParam<Color> parameter_BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialParam<Color> parameter_ShadeEnvironmentalColor = new MaterialParam<Color>(SHADEENVIRONMENTALCOLOR, Color.white);
public MaterialParam<Color> parameter_ShadeColor = new MaterialParam<Color>(SHADECOLOR, Color.white);
public MaterialTextureParam parameter_ShadeMap = new MaterialTextureParam(SHADEMAP);
public MaterialParam<float> parameter_Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
public MaterialParam<float> parameter_Metalic = new MaterialParam<float>(METALIC, 1.0f);
public MaterialTextureParam parameter_MetalicMap = new MaterialTextureParam(METALICMAP);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialTextureParam parameter_ShadowMap = new MaterialTextureParam(OCCLUSIONMAP);
public MaterialParam<float> parameter_OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
public MaterialTextureParam parameter_OutlineMap = new MaterialTextureParam(OUTLINEMAP);
public MaterialParam<float> parameter_ShadeToony = new MaterialParam<float>(SHADETOONY, 1.0f);
public MaterialParam<float> parameter_ToonyLighting = new MaterialParam<float>(TOONYLIGHTING, 1.0f);
public MaterialTextureParam parameter_Emission = new MaterialTextureParam(EMISSIONMAP);
public MaterialParam<Color> parameter_EmissionColor = new MaterialParam<Color>(EMISSIONCOLOR, Color.white);
public BVA_Material_ToonTransparentCutout_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_BaseColor.Value = material.GetColor(parameter_BaseColor.ParamName);
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
parameter_ShadeEnvironmentalColor.Value = material.GetColor(parameter_ShadeEnvironmentalColor.ParamName);
parameter_ShadeColor.Value = material.GetColor(parameter_ShadeColor.ParamName);
var parameter_shademap_temp = material.GetTexture(parameter_ShadeMap.ParamName);
if (parameter_shademap_temp != null) parameter_ShadeMap.Value = exportTextureInfo(parameter_shademap_temp);
parameter_Smoothness.Value = material.GetFloat(parameter_Smoothness.ParamName);
parameter_Metalic.Value = material.GetFloat(parameter_Metalic.ParamName);
var parameter_metalicmap_temp = material.GetTexture(parameter_MetalicMap.ParamName);
if (parameter_metalicmap_temp != null) parameter_MetalicMap.Value = exportTextureInfo(parameter_metalicmap_temp);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportNormalTextureInfo(parameter_normalmap_temp);
var parameter_shadowmap_temp = material.GetTexture(parameter_ShadowMap.ParamName);
if (parameter_shadowmap_temp != null) parameter_ShadowMap.Value = exportTextureInfo(parameter_shadowmap_temp);
parameter_OutlineWidth.Value = material.GetFloat(parameter_OutlineWidth.ParamName);
var parameter_outlinemap_temp = material.GetTexture(parameter_OutlineMap.ParamName);
if (parameter_outlinemap_temp != null) parameter_OutlineMap.Value = exportTextureInfo(parameter_outlinemap_temp);
parameter_ShadeToony.Value = material.GetFloat(parameter_ShadeToony.ParamName);
parameter_ToonyLighting.Value = material.GetFloat(parameter_ToonyLighting.ParamName);
var parameter_emission_temp = material.GetTexture(parameter_Emission.ParamName);
if (parameter_emission_temp != null) parameter_Emission.Value = exportTextureInfo(parameter_emission_temp);
parameter_EmissionColor.Value = material.GetColor(parameter_EmissionColor.ParamName);
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
case BVA_Material_ToonTransparentCutout_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ToonTransparentCutout_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonTransparentCutout_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonTransparentCutout_Extra.BASEMAP, tex);
}
break;
case BVA_Material_ToonTransparentCutout_Extra.SHADEENVIRONMENTALCOLOR:
matCache.SetColor(BVA_Material_ToonTransparentCutout_Extra.SHADEENVIRONMENTALCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonTransparentCutout_Extra.SHADECOLOR:
matCache.SetColor(BVA_Material_ToonTransparentCutout_Extra.SHADECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonTransparentCutout_Extra.SHADEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonTransparentCutout_Extra.SHADEMAP, tex);
}
break;
case BVA_Material_ToonTransparentCutout_Extra.SMOOTHNESS:
matCache.SetFloat(BVA_Material_ToonTransparentCutout_Extra.SMOOTHNESS, reader.ReadAsFloat());
break;
case BVA_Material_ToonTransparentCutout_Extra.METALIC:
matCache.SetFloat(BVA_Material_ToonTransparentCutout_Extra.METALIC, reader.ReadAsFloat());
break;
case BVA_Material_ToonTransparentCutout_Extra.METALICMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonTransparentCutout_Extra.METALICMAP, tex);
}
break;
case BVA_Material_ToonTransparentCutout_Extra.NORMALMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonTransparentCutout_Extra.NORMALMAP, tex);
}
break;
case BVA_Material_ToonTransparentCutout_Extra.OCCLUSIONMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonTransparentCutout_Extra.OCCLUSIONMAP, tex);
}
break;
case BVA_Material_ToonTransparentCutout_Extra.OUTLINEWIDTH:
matCache.SetFloat(BVA_Material_ToonTransparentCutout_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
break;
case BVA_Material_ToonTransparentCutout_Extra.OUTLINEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonTransparentCutout_Extra.OUTLINEMAP, tex);
}
break;
case BVA_Material_ToonTransparentCutout_Extra.SHADETOONY:
matCache.SetFloat(BVA_Material_ToonTransparentCutout_Extra.SHADETOONY, reader.ReadAsFloat());
break;
case BVA_Material_ToonTransparentCutout_Extra.TOONYLIGHTING:
matCache.SetFloat(BVA_Material_ToonTransparentCutout_Extra.TOONYLIGHTING, reader.ReadAsFloat());
break;
case BVA_Material_ToonTransparentCutout_Extra.EMISSIONMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonTransparentCutout_Extra.EMISSIONMAP, tex);
}
break;
case BVA_Material_ToonTransparentCutout_Extra.EMISSIONCOLOR:
matCache.SetColor(BVA_Material_ToonTransparentCutout_Extra.EMISSIONCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
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
jo.Add(parameter_ShadeEnvironmentalColor.ParamName, parameter_ShadeEnvironmentalColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_ShadeColor.ParamName, parameter_ShadeColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_ShadeMap != null && parameter_ShadeMap.Value != null) jo.Add(parameter_ShadeMap.ParamName, parameter_ShadeMap.Serialize());
jo.Add(parameter_Smoothness.ParamName, parameter_Smoothness.Value);
jo.Add(parameter_Metalic.ParamName, parameter_Metalic.Value);
if (parameter_MetalicMap != null && parameter_MetalicMap.Value != null) jo.Add(parameter_MetalicMap.ParamName, parameter_MetalicMap.Serialize());
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
if (parameter_ShadowMap != null && parameter_ShadowMap.Value != null) jo.Add(parameter_ShadowMap.ParamName, parameter_ShadowMap.Serialize());
jo.Add(parameter_OutlineWidth.ParamName, parameter_OutlineWidth.Value);
if (parameter_OutlineMap != null && parameter_OutlineMap.Value != null) jo.Add(parameter_OutlineMap.ParamName, parameter_OutlineMap.Serialize());
jo.Add(parameter_ShadeToony.ParamName, parameter_ShadeToony.Value);
jo.Add(parameter_ToonyLighting.ParamName, parameter_ToonyLighting.Value);
if (parameter_Emission != null && parameter_Emission.Value != null) jo.Add(parameter_Emission.ParamName, parameter_Emission.Serialize());
jo.Add(parameter_EmissionColor.ParamName, parameter_EmissionColor.Value.ToNumericsColorRaw().ToJArray());
return new JProperty(BVA_Material_ToonTransparentCutout_Extra.SHADER_NAME, jo);
}
}
}
