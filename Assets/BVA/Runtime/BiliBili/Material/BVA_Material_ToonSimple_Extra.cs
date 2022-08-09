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
public class BVA_Material_ToonSimple_Extra : MaterialExtra
{
public const string PROPERTY = "BVA_Material_ToonSimple_Extra";
public const string SHADER_NAME = "Shader Graphs/Toon (Simple)";
public const string BASECOLOR = "_BaseColor";
public const string BASEMAP = "_BaseMap";
public const string SMOOTHNESS = "_Smoothness";
public const string CURVATURE = "_Curvature";
public const string NORMALMAP = "_NormalMap";
public const string SHADE = "_Shade";
public const string OUTLINEWIDTH = "_OutlineWidth";
public const string SHADETOONY = "_ShadeToony";
public const string TOONYLIGHTING = "_ToonyLighting";
public const string OUTLINEINTENSITY = "_OutlineIntensity";
public MaterialParam<Color> parameter_BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialParam<float> parameter_Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
public MaterialParam<float> parameter_Curvature = new MaterialParam<float>(CURVATURE, 1.0f);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialParam<float> parameter_ShadeShift = new MaterialParam<float>(SHADE, 1.0f);
public MaterialParam<float> parameter_OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
public MaterialParam<float> parameter_ShadeToony = new MaterialParam<float>(SHADETOONY, 1.0f);
public MaterialParam<float> parameter_ToonyLighting = new MaterialParam<float>(TOONYLIGHTING, 1.0f);
public MaterialParam<float> parameter_OutlineIntensity = new MaterialParam<float>(OUTLINEINTENSITY, 1.0f);
public BVA_Material_ToonSimple_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_BaseColor.Value = material.GetColor(parameter_BaseColor.ParamName);
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
parameter_Smoothness.Value = material.GetFloat(parameter_Smoothness.ParamName);
parameter_Curvature.Value = material.GetFloat(parameter_Curvature.ParamName);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportNormalTextureInfo(parameter_normalmap_temp);
parameter_ShadeShift.Value = material.GetFloat(parameter_ShadeShift.ParamName);
parameter_OutlineWidth.Value = material.GetFloat(parameter_OutlineWidth.ParamName);
parameter_ShadeToony.Value = material.GetFloat(parameter_ShadeToony.ParamName);
parameter_ToonyLighting.Value = material.GetFloat(parameter_ToonyLighting.ParamName);
parameter_OutlineIntensity.Value = material.GetFloat(parameter_OutlineIntensity.ParamName);
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
case BVA_Material_ToonSimple_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ToonSimple_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ToonSimple_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonSimple_Extra.BASEMAP, tex);
}
break;
case BVA_Material_ToonSimple_Extra.SMOOTHNESS:
matCache.SetFloat(BVA_Material_ToonSimple_Extra.SMOOTHNESS, reader.ReadAsFloat());
break;
case BVA_Material_ToonSimple_Extra.CURVATURE:
matCache.SetFloat(BVA_Material_ToonSimple_Extra.CURVATURE, reader.ReadAsFloat());
break;
case BVA_Material_ToonSimple_Extra.NORMALMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonSimple_Extra.NORMALMAP, tex);
}
break;
case BVA_Material_ToonSimple_Extra.SHADE:
matCache.SetFloat(BVA_Material_ToonSimple_Extra.SHADE, reader.ReadAsFloat());
break;
case BVA_Material_ToonSimple_Extra.OUTLINEWIDTH:
matCache.SetFloat(BVA_Material_ToonSimple_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
break;
case BVA_Material_ToonSimple_Extra.SHADETOONY:
matCache.SetFloat(BVA_Material_ToonSimple_Extra.SHADETOONY, reader.ReadAsFloat());
break;
case BVA_Material_ToonSimple_Extra.TOONYLIGHTING:
matCache.SetFloat(BVA_Material_ToonSimple_Extra.TOONYLIGHTING, reader.ReadAsFloat());
break;
case BVA_Material_ToonSimple_Extra.OUTLINEINTENSITY:
matCache.SetFloat(BVA_Material_ToonSimple_Extra.OUTLINEINTENSITY, reader.ReadAsFloat());
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
jo.Add(parameter_Smoothness.ParamName, parameter_Smoothness.Value);
jo.Add(parameter_Curvature.ParamName, parameter_Curvature.Value);
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
jo.Add(parameter_ShadeShift.ParamName, parameter_ShadeShift.Value);
jo.Add(parameter_OutlineWidth.ParamName, parameter_OutlineWidth.Value);
jo.Add(parameter_ShadeToony.ParamName, parameter_ShadeToony.Value);
jo.Add(parameter_ToonyLighting.ParamName, parameter_ToonyLighting.Value);
jo.Add(parameter_OutlineIntensity.ParamName, parameter_OutlineIntensity.Value);
return new JProperty(BVA_Material_ToonSimple_Extra.SHADER_NAME, jo);
}
}
}
