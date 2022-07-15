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
public class BVA_Material_MToon_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_MToon_Extra";
public const string SHADER_NAME = "Shader Graphs/MToon";
public const string COLOR = "_Color";
public const string MAINTEX = "_MainTex";
public const string SHADECOLOR = "_ShadeColor";
public const string SHADETEXTURE = "_ShadeTexture";
public const string SHADETOONY = "_ShadeToony";
public const string BUMPMAP = "_BumpMap";
public const string SPHEREADD = "_SphereAdd";
public const string EMISSIONMAP = "_EmissionMap";
public const string EMISSIONCOLOR = "_EmissionColor";
public const string OUTLINEWIDTH = "_OutlineWidth";
public const string OUTLINEWIDTHTEXTURE = "_OutlineWidthTexture";
public const string TOONYLIGHTING = "_ToonyLighting";
public const string SHADESHIFT = "_ShadeShift";
public const string OUTLINECOLOR = "_OutlineColor";
public const string CUTOFF = "_CutOff";
public const string RIMTEXTURE = "_RimTexture";
public const string QUEUEOFFSET = "_QueueOffset";
public const string QUEUECONTROL = "_QueueControl";
public MaterialParam<Color> parameter_Color = new MaterialParam<Color>(COLOR, Color.white);
public MaterialTextureParam parameter_MainTex = new MaterialTextureParam(MAINTEX);
public MaterialParam<Color> parameter_ShadeColor = new MaterialParam<Color>(SHADECOLOR, Color.white);
public MaterialTextureParam parameter_ShadeTexture = new MaterialTextureParam(SHADETEXTURE);
public MaterialParam<float> parameter_ShadeToony = new MaterialParam<float>(SHADETOONY, 1.0f);
public MaterialTextureParam parameter_BumpMap = new MaterialTextureParam(BUMPMAP);
public MaterialTextureParam parameter_SphereAdd = new MaterialTextureParam(SPHEREADD);
public MaterialTextureParam parameter_EmissionMap = new MaterialTextureParam(EMISSIONMAP);
public MaterialParam<Color> parameter_EmissionColor = new MaterialParam<Color>(EMISSIONCOLOR, Color.white);
public MaterialParam<float> parameter_OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
public MaterialTextureParam parameter_OutlineWidthTexture = new MaterialTextureParam(OUTLINEWIDTHTEXTURE);
public MaterialParam<float> parameter_ToonyLighting = new MaterialParam<float>(TOONYLIGHTING, 1.0f);
public MaterialParam<float> parameter_ShadeShift = new MaterialParam<float>(SHADESHIFT, 1.0f);
public MaterialParam<Color> parameter_OutlineColor = new MaterialParam<Color>(OUTLINECOLOR, Color.white);
public MaterialParam<float> parameter_CutOff = new MaterialParam<float>(CUTOFF, 1.0f);
public MaterialTextureParam parameter_RimTexture = new MaterialTextureParam(RIMTEXTURE);
public MaterialParam<float> parameter__QueueOffset = new MaterialParam<float>(QUEUEOFFSET, 1.0f);
public MaterialParam<float> parameter__QueueControl = new MaterialParam<float>(QUEUECONTROL, 1.0f);
public BVA_Material_MToon_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_Color.Value = material.GetColor(parameter_Color.ParamName);
var parameter_maintex_temp = material.GetTexture(parameter_MainTex.ParamName);
if (parameter_maintex_temp != null) parameter_MainTex.Value = exportTextureInfo(parameter_maintex_temp);
parameter_ShadeColor.Value = material.GetColor(parameter_ShadeColor.ParamName);
var parameter_shadetexture_temp = material.GetTexture(parameter_ShadeTexture.ParamName);
if (parameter_shadetexture_temp != null) parameter_ShadeTexture.Value = exportTextureInfo(parameter_shadetexture_temp);
parameter_ShadeToony.Value = material.GetFloat(parameter_ShadeToony.ParamName);
var parameter_bumpmap_temp = material.GetTexture(parameter_BumpMap.ParamName);
if (parameter_bumpmap_temp != null) parameter_BumpMap.Value = exportNormalMapInfo(parameter_bumpmap_temp);
var parameter_sphereadd_temp = material.GetTexture(parameter_SphereAdd.ParamName);
if (parameter_sphereadd_temp != null) parameter_SphereAdd.Value = exportTextureInfo(parameter_sphereadd_temp);
var parameter_emissionmap_temp = material.GetTexture(parameter_EmissionMap.ParamName);
if (parameter_emissionmap_temp != null) parameter_EmissionMap.Value = exportTextureInfo(parameter_emissionmap_temp);
parameter_EmissionColor.Value = material.GetColor(parameter_EmissionColor.ParamName);
parameter_OutlineWidth.Value = material.GetFloat(parameter_OutlineWidth.ParamName);
var parameter_outlinewidthtexture_temp = material.GetTexture(parameter_OutlineWidthTexture.ParamName);
if (parameter_outlinewidthtexture_temp != null) parameter_OutlineWidthTexture.Value = exportTextureInfo(parameter_outlinewidthtexture_temp);
parameter_ToonyLighting.Value = material.GetFloat(parameter_ToonyLighting.ParamName);
parameter_ShadeShift.Value = material.GetFloat(parameter_ShadeShift.ParamName);
parameter_OutlineColor.Value = material.GetColor(parameter_OutlineColor.ParamName);
parameter_CutOff.Value = material.GetFloat(parameter_CutOff.ParamName);
var parameter_rimtexture_temp = material.GetTexture(parameter_RimTexture.ParamName);
if (parameter_rimtexture_temp != null) parameter_RimTexture.Value = exportTextureInfo(parameter_rimtexture_temp);
parameter__QueueOffset.Value = material.GetFloat(parameter__QueueOffset.ParamName);
parameter__QueueControl.Value = material.GetFloat(parameter__QueueControl.ParamName);
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
case BVA_Material_MToon_Extra.COLOR:
matCache.SetColor(BVA_Material_MToon_Extra.COLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_MToon_Extra.MAINTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MToon_Extra.MAINTEX, tex);
}
break;
case BVA_Material_MToon_Extra.SHADECOLOR:
matCache.SetColor(BVA_Material_MToon_Extra.SHADECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_MToon_Extra.SHADETEXTURE:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MToon_Extra.SHADETEXTURE, tex);
}
break;
case BVA_Material_MToon_Extra.SHADETOONY:
matCache.SetFloat(BVA_Material_MToon_Extra.SHADETOONY, reader.ReadAsFloat());
break;
case BVA_Material_MToon_Extra.BUMPMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MToon_Extra.BUMPMAP, tex);
}
break;
case BVA_Material_MToon_Extra.SPHEREADD:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MToon_Extra.SPHEREADD, tex);
}
break;
case BVA_Material_MToon_Extra.EMISSIONMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MToon_Extra.EMISSIONMAP, tex);
}
break;
case BVA_Material_MToon_Extra.EMISSIONCOLOR:
matCache.SetColor(BVA_Material_MToon_Extra.EMISSIONCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_MToon_Extra.OUTLINEWIDTH:
matCache.SetFloat(BVA_Material_MToon_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
break;
case BVA_Material_MToon_Extra.OUTLINEWIDTHTEXTURE:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MToon_Extra.OUTLINEWIDTHTEXTURE, tex);
}
break;
case BVA_Material_MToon_Extra.TOONYLIGHTING:
matCache.SetFloat(BVA_Material_MToon_Extra.TOONYLIGHTING, reader.ReadAsFloat());
break;
case BVA_Material_MToon_Extra.SHADESHIFT:
matCache.SetFloat(BVA_Material_MToon_Extra.SHADESHIFT, reader.ReadAsFloat());
break;
case BVA_Material_MToon_Extra.OUTLINECOLOR:
matCache.SetColor(BVA_Material_MToon_Extra.OUTLINECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_MToon_Extra.CUTOFF:
matCache.SetFloat(BVA_Material_MToon_Extra.CUTOFF, reader.ReadAsFloat());
break;
case BVA_Material_MToon_Extra.RIMTEXTURE:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MToon_Extra.RIMTEXTURE, tex);
}
break;
case BVA_Material_MToon_Extra.QUEUEOFFSET:
matCache.SetFloat(BVA_Material_MToon_Extra.QUEUEOFFSET, reader.ReadAsFloat());
break;
case BVA_Material_MToon_Extra.QUEUECONTROL:
matCache.SetFloat(BVA_Material_MToon_Extra.QUEUECONTROL, reader.ReadAsFloat());
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(parameter_Color.ParamName, parameter_Color.Value.ToNumericsColorRaw().ToJArray());
if (parameter_MainTex != null && parameter_MainTex.Value != null) jo.Add(parameter_MainTex.ParamName, parameter_MainTex.Serialize());
jo.Add(parameter_ShadeColor.ParamName, parameter_ShadeColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_ShadeTexture != null && parameter_ShadeTexture.Value != null) jo.Add(parameter_ShadeTexture.ParamName, parameter_ShadeTexture.Serialize());
jo.Add(parameter_ShadeToony.ParamName, parameter_ShadeToony.Value);
if (parameter_BumpMap != null && parameter_BumpMap.Value != null) jo.Add(parameter_BumpMap.ParamName, parameter_BumpMap.Serialize());
if (parameter_SphereAdd != null && parameter_SphereAdd.Value != null) jo.Add(parameter_SphereAdd.ParamName, parameter_SphereAdd.Serialize());
if (parameter_EmissionMap != null && parameter_EmissionMap.Value != null) jo.Add(parameter_EmissionMap.ParamName, parameter_EmissionMap.Serialize());
jo.Add(parameter_EmissionColor.ParamName, parameter_EmissionColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_OutlineWidth.ParamName, parameter_OutlineWidth.Value);
if (parameter_OutlineWidthTexture != null && parameter_OutlineWidthTexture.Value != null) jo.Add(parameter_OutlineWidthTexture.ParamName, parameter_OutlineWidthTexture.Serialize());
jo.Add(parameter_ToonyLighting.ParamName, parameter_ToonyLighting.Value);
jo.Add(parameter_ShadeShift.ParamName, parameter_ShadeShift.Value);
jo.Add(parameter_OutlineColor.ParamName, parameter_OutlineColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_CutOff.ParamName, parameter_CutOff.Value);
if (parameter_RimTexture != null && parameter_RimTexture.Value != null) jo.Add(parameter_RimTexture.ParamName, parameter_RimTexture.Serialize());
jo.Add(parameter__QueueOffset.ParamName, parameter__QueueOffset.Value);
jo.Add(parameter__QueueControl.ParamName, parameter__QueueControl.Value);
return new JProperty(BVA_Material_MToon_Extra.SHADER_NAME, jo);
}
}
}
