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
public class BVA_Material_ToonGGX_Extra : IMaterialExtra
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
public MaterialParam<Color> parameter__BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter__BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialTextureParam parameter__SSSMap = new MaterialTextureParam(SSSMAP);
public MaterialParam<Color> parameter__EnvironmentColor = new MaterialParam<Color>(ENVIRONMENTCOLOR, Color.white);
public MaterialParam<Color> parameter__HighlightColor = new MaterialParam<Color>(HIGHLIGHTCOLOR, Color.white);
public MaterialTextureParam parameter__NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialTextureParam parameter__IlliminationMap = new MaterialTextureParam(ILLIMINATIONMAP);
public MaterialParam<float> parameter__OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
public MaterialParam<float> parameter__TonnyLighting = new MaterialParam<float>(TONNYLIGHTING, 1.0f);
public MaterialParam<float> parameter__Curvature = new MaterialParam<float>(CURVATURE, 1.0f);
public MaterialParam<float> parameter__WidthScaledMaxDistance = new MaterialParam<float>(WIDTHSCALEDMAXDISTANCE, 1.0f);
public string[] keywords;
public string ShaderName => SHADER_NAME;
public string ExtraName => GetType().Name;
public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
{
keywords = material.shaderKeywords;
parameter__BaseColor.Value = material.GetColor(parameter__BaseColor.ParamName);
var parameter__basemap_temp = material.GetTexture(parameter__BaseMap.ParamName);
if (parameter__basemap_temp != null) parameter__BaseMap.Value = exportTextureInfo(parameter__basemap_temp);
var parameter__sssmap_temp = material.GetTexture(parameter__SSSMap.ParamName);
if (parameter__sssmap_temp != null) parameter__SSSMap.Value = exportTextureInfo(parameter__sssmap_temp);
parameter__EnvironmentColor.Value = material.GetColor(parameter__EnvironmentColor.ParamName);
parameter__HighlightColor.Value = material.GetColor(parameter__HighlightColor.ParamName);
var parameter__normalmap_temp = material.GetTexture(parameter__NormalMap.ParamName);
if (parameter__normalmap_temp != null) parameter__NormalMap.Value = exportNormalTextureInfo(parameter__normalmap_temp);
var parameter__illiminationmap_temp = material.GetTexture(parameter__IlliminationMap.ParamName);
if (parameter__illiminationmap_temp != null) parameter__IlliminationMap.Value = exportTextureInfo(parameter__illiminationmap_temp);
parameter__OutlineWidth.Value = material.GetFloat(parameter__OutlineWidth.ParamName);
parameter__TonnyLighting.Value = material.GetFloat(parameter__TonnyLighting.ParamName);
parameter__Curvature.Value = material.GetFloat(parameter__Curvature.ParamName);
parameter__WidthScaledMaxDistance.Value = material.GetFloat(parameter__WidthScaledMaxDistance.ParamName);
}
public async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache,AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case BVA_Material_ToonGGX_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ToonGGX_Extra.BASECOLOR, reader.ReadAsRGBAColor());
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
matCache.SetColor(BVA_Material_ToonGGX_Extra.ENVIRONMENTCOLOR, reader.ReadAsRGBAColor());
break;
case BVA_Material_ToonGGX_Extra.HIGHLIGHTCOLOR:
matCache.SetColor(BVA_Material_ToonGGX_Extra.HIGHLIGHTCOLOR, reader.ReadAsRGBAColor());
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
jo.Add(parameter__BaseColor.ParamName, parameter__BaseColor.Value.ToJArray());
if (parameter__BaseMap != null && parameter__BaseMap.Value != null) jo.Add(parameter__BaseMap.ParamName, parameter__BaseMap.Serialize());
if (parameter__SSSMap != null && parameter__SSSMap.Value != null) jo.Add(parameter__SSSMap.ParamName, parameter__SSSMap.Serialize());
jo.Add(parameter__EnvironmentColor.ParamName, parameter__EnvironmentColor.Value.ToJArray());
jo.Add(parameter__HighlightColor.ParamName, parameter__HighlightColor.Value.ToJArray());
if (parameter__NormalMap != null && parameter__NormalMap.Value != null) jo.Add(parameter__NormalMap.ParamName, parameter__NormalMap.Serialize());
if (parameter__IlliminationMap != null && parameter__IlliminationMap.Value != null) jo.Add(parameter__IlliminationMap.ParamName, parameter__IlliminationMap.Serialize());
jo.Add(parameter__OutlineWidth.ParamName, parameter__OutlineWidth.Value);
jo.Add(parameter__TonnyLighting.ParamName, parameter__TonnyLighting.Value);
jo.Add(parameter__Curvature.ParamName, parameter__Curvature.Value);
jo.Add(parameter__WidthScaledMaxDistance.ParamName, parameter__WidthScaledMaxDistance.Value);
if(keywords != null && keywords.Length > 0)
{
JArray jKeywords = new JArray();
foreach (var keyword in jKeywords)
jKeywords.Add(keyword);
jo.Add(nameof(keywords), jKeywords);
}
return new JProperty(BVA_Material_ToonGGX_Extra.SHADER_NAME, jo);
}

        public object Clone()
        {
            return new BVA_Material_ToonGGX_Extra();
        }
    }
}
