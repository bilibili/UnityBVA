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
public class BVA_Material_ToonHair_Extra : IMaterialExtra
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
public MaterialParam<Color> parameter__BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialTextureParam parameter__BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialParam<Color> parameter__ShadeColor = new MaterialParam<Color>(SHADECOLOR, Color.white);
public MaterialParam<float> parameter__Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
public MaterialTextureParam parameter__NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialTextureParam parameter__Hightlight = new MaterialTextureParam(HIGHTLIGHT);
public MaterialTextureParam parameter__StripShiftMap = new MaterialTextureParam(STRIPSHIFTMAP);
public MaterialParam<Vector4> parameter__StripShiftTiling = new MaterialParam<Vector4>(STRIPSHIFTTILING, Vector4.one);
public MaterialParam<Color> parameter__FirstLayerHighlightColor = new MaterialParam<Color>(FIRSTLAYERHIGHLIGHTCOLOR, Color.white);
public MaterialParam<Vector4> parameter__FirstLayerExponent = new MaterialParam<Vector4>(FIRSTLAYEREXPONENT, Vector4.one);
public MaterialParam<float> parameter__FirstLayerShiftPower = new MaterialParam<float>(FIRSTLAYERSHIFTPOWER, 1.0f);
public MaterialParam<float> parameter__FirstLayerShift = new MaterialParam<float>(FIRSTLAYERSHIFT, 1.0f);
public MaterialParam<Color> parameter__SecondLayerHighlightColor = new MaterialParam<Color>(SECONDLAYERHIGHLIGHTCOLOR, Color.white);
public MaterialParam<Vector4> parameter__SecondLayerExponent = new MaterialParam<Vector4>(SECONDLAYEREXPONENT, Vector4.one);
public MaterialParam<float> parameter__SecondLayerShiftPower = new MaterialParam<float>(SECONDLAYERSHIFTPOWER, 1.0f);
public MaterialParam<float> parameter__SecondLayerShift = new MaterialParam<float>(SECONDLAYERSHIFT, 1.0f);
public MaterialParam<float> parameter__TonnyLighting = new MaterialParam<float>(TONNYLIGHTING, 1.0f);
public MaterialParam<float> parameter__SPECULARHIGHLIGHTS = new MaterialParam<float>(SPECULARHIGHLIGHTS, 1.0f);
public string[] keywords;
public string ShaderName => SHADER_NAME;
public string ExtraName => GetType().Name;
public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
{
keywords = material.shaderKeywords;
parameter__BaseColor.Value = material.GetColor(parameter__BaseColor.ParamName);
var parameter__basemap_temp = material.GetTexture(parameter__BaseMap.ParamName);
if (parameter__basemap_temp != null) parameter__BaseMap.Value = exportTextureInfo(parameter__basemap_temp);
parameter__ShadeColor.Value = material.GetColor(parameter__ShadeColor.ParamName);
parameter__Smoothness.Value = material.GetFloat(parameter__Smoothness.ParamName);
var parameter__normalmap_temp = material.GetTexture(parameter__NormalMap.ParamName);
if (parameter__normalmap_temp != null) parameter__NormalMap.Value = exportNormalTextureInfo(parameter__normalmap_temp);
var parameter__hightlight_temp = material.GetTexture(parameter__Hightlight.ParamName);
if (parameter__hightlight_temp != null) parameter__Hightlight.Value = exportTextureInfo(parameter__hightlight_temp);
var parameter__stripshiftmap_temp = material.GetTexture(parameter__StripShiftMap.ParamName);
if (parameter__stripshiftmap_temp != null) parameter__StripShiftMap.Value = exportTextureInfo(parameter__stripshiftmap_temp);
parameter__StripShiftTiling.Value = material.GetVector(parameter__StripShiftTiling.ParamName);
parameter__FirstLayerHighlightColor.Value = material.GetColor(parameter__FirstLayerHighlightColor.ParamName);
parameter__FirstLayerExponent.Value = material.GetVector(parameter__FirstLayerExponent.ParamName);
parameter__FirstLayerShiftPower.Value = material.GetFloat(parameter__FirstLayerShiftPower.ParamName);
parameter__FirstLayerShift.Value = material.GetFloat(parameter__FirstLayerShift.ParamName);
parameter__SecondLayerHighlightColor.Value = material.GetColor(parameter__SecondLayerHighlightColor.ParamName);
parameter__SecondLayerExponent.Value = material.GetVector(parameter__SecondLayerExponent.ParamName);
parameter__SecondLayerShiftPower.Value = material.GetFloat(parameter__SecondLayerShiftPower.ParamName);
parameter__SecondLayerShift.Value = material.GetFloat(parameter__SecondLayerShift.ParamName);
parameter__TonnyLighting.Value = material.GetFloat(parameter__TonnyLighting.ParamName);
parameter__SPECULARHIGHLIGHTS.Value = material.GetFloat(parameter__SPECULARHIGHLIGHTS.ParamName);
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
case BVA_Material_ToonHair_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.BASECOLOR, reader.ReadAsRGBAColor());
break;
case BVA_Material_ToonHair_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ToonHair_Extra.BASEMAP, tex);
}
break;
case BVA_Material_ToonHair_Extra.SHADECOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.SHADECOLOR, reader.ReadAsRGBAColor());
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
matCache.SetVector(BVA_Material_ToonHair_Extra.STRIPSHIFTTILING, reader.ReadAsVector4());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYERHIGHLIGHTCOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.FIRSTLAYERHIGHLIGHTCOLOR, reader.ReadAsRGBAColor());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYEREXPONENT:
matCache.SetVector(BVA_Material_ToonHair_Extra.FIRSTLAYEREXPONENT, reader.ReadAsVector4());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFTPOWER:
matCache.SetFloat(BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFTPOWER, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFT:
matCache.SetFloat(BVA_Material_ToonHair_Extra.FIRSTLAYERSHIFT, reader.ReadAsFloat());
break;
case BVA_Material_ToonHair_Extra.SECONDLAYERHIGHLIGHTCOLOR:
matCache.SetColor(BVA_Material_ToonHair_Extra.SECONDLAYERHIGHLIGHTCOLOR, reader.ReadAsRGBAColor());
break;
case BVA_Material_ToonHair_Extra.SECONDLAYEREXPONENT:
matCache.SetVector(BVA_Material_ToonHair_Extra.SECONDLAYEREXPONENT, reader.ReadAsVector4());
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
jo.Add(parameter__ShadeColor.ParamName, parameter__ShadeColor.Value.ToJArray());
jo.Add(parameter__Smoothness.ParamName, parameter__Smoothness.Value);
if (parameter__NormalMap != null && parameter__NormalMap.Value != null) jo.Add(parameter__NormalMap.ParamName, parameter__NormalMap.Serialize());
if (parameter__Hightlight != null && parameter__Hightlight.Value != null) jo.Add(parameter__Hightlight.ParamName, parameter__Hightlight.Serialize());
if (parameter__StripShiftMap != null && parameter__StripShiftMap.Value != null) jo.Add(parameter__StripShiftMap.ParamName, parameter__StripShiftMap.Serialize());
jo.Add(parameter__StripShiftTiling.ParamName, parameter__StripShiftTiling.Value.ToJArray());
jo.Add(parameter__FirstLayerHighlightColor.ParamName, parameter__FirstLayerHighlightColor.Value.ToJArray());
jo.Add(parameter__FirstLayerExponent.ParamName, parameter__FirstLayerExponent.Value.ToJArray());
jo.Add(parameter__FirstLayerShiftPower.ParamName, parameter__FirstLayerShiftPower.Value);
jo.Add(parameter__FirstLayerShift.ParamName, parameter__FirstLayerShift.Value);
jo.Add(parameter__SecondLayerHighlightColor.ParamName, parameter__SecondLayerHighlightColor.Value.ToJArray());
jo.Add(parameter__SecondLayerExponent.ParamName, parameter__SecondLayerExponent.Value.ToJArray());
jo.Add(parameter__SecondLayerShiftPower.ParamName, parameter__SecondLayerShiftPower.Value);
jo.Add(parameter__SecondLayerShift.ParamName, parameter__SecondLayerShift.Value);
jo.Add(parameter__TonnyLighting.ParamName, parameter__TonnyLighting.Value);
jo.Add(parameter__SPECULARHIGHLIGHTS.ParamName, parameter__SPECULARHIGHLIGHTS.Value);
if(keywords != null && keywords.Length > 0)
{
JArray jKeywords = new JArray();
foreach (var keyword in jKeywords)
jKeywords.Add(keyword);
jo.Add(nameof(keywords), jKeywords);
}
return new JProperty(BVA_Material_ToonHair_Extra.SHADER_NAME, jo);
}

        public object Clone()
        {
            return new BVA_Material_ToonHair_Extra();
        }
    }
}
