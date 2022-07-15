using Newtonsoft.Json.Linq;
using GLTF.Math;
using GLTF.Schema;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;

namespace GLTF.Schema.BVA
{
public class BVA_Material_ZeldaToon_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_ZeldaToon_Extra";
public const string SHADER_NAME = "Shader Graphs/ZeldaToon";
public const string BASECOLOR = "_BaseColor";
public const string AMBIENTCOLOR = "_AmbientColor";
public const string GLOSSINESS = "_Glossiness";
public const string SPECULARCOLOR = "_SpecularColor";
public const string RIMCOLOR = "_RimColor";
public const string RIMAMOUNT = "_RimAmount";
public const string RIMTRESHOLD = "RimTreshold";
public const string BASEMAP = "_BaseMap";
public const string QUEUEOFFSET = "_QueueOffset";
public const string QUEUECONTROL = "_QueueControl";
public MaterialParam<Color> parameter_BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
public MaterialParam<Color> parameter_AmbientColor = new MaterialParam<Color>(AMBIENTCOLOR, Color.white);
public MaterialParam<float> parameter_Glossiness = new MaterialParam<float>(GLOSSINESS, 1.0f);
public MaterialParam<Color> parameter_SpecularColor = new MaterialParam<Color>(SPECULARCOLOR, Color.white);
public MaterialParam<Color> parameter_RimColor = new MaterialParam<Color>(RIMCOLOR, Color.white);
public MaterialParam<float> parameter_RimAmount = new MaterialParam<float>(RIMAMOUNT, 1.0f);
public MaterialParam<float> parameter_RimTreshold = new MaterialParam<float>(RIMTRESHOLD, 1.0f);
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialParam<float> parameter__QueueOffset = new MaterialParam<float>(QUEUEOFFSET, 1.0f);
public MaterialParam<float> parameter__QueueControl = new MaterialParam<float>(QUEUECONTROL, 1.0f);
public BVA_Material_ZeldaToon_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_BaseColor.Value = material.GetColor(parameter_BaseColor.ParamName);
parameter_AmbientColor.Value = material.GetColor(parameter_AmbientColor.ParamName);
parameter_Glossiness.Value = material.GetFloat(parameter_Glossiness.ParamName);
parameter_SpecularColor.Value = material.GetColor(parameter_SpecularColor.ParamName);
parameter_RimColor.Value = material.GetColor(parameter_RimColor.ParamName);
parameter_RimAmount.Value = material.GetFloat(parameter_RimAmount.ParamName);
parameter_RimTreshold.Value = material.GetFloat(parameter_RimTreshold.ParamName);
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
parameter__QueueOffset.Value = material.GetFloat(parameter__QueueOffset.ParamName);
parameter__QueueControl.Value = material.GetFloat(parameter__QueueControl.ParamName);
}
public static async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache,AsyncLoadTexture loadTexture,AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case BVA_Material_ZeldaToon_Extra.BASECOLOR:
matCache.SetColor(BVA_Material_ZeldaToon_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ZeldaToon_Extra.AMBIENTCOLOR:
matCache.SetColor(BVA_Material_ZeldaToon_Extra.AMBIENTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ZeldaToon_Extra.GLOSSINESS:
matCache.SetFloat(BVA_Material_ZeldaToon_Extra.GLOSSINESS, reader.ReadAsFloat());
break;
case BVA_Material_ZeldaToon_Extra.SPECULARCOLOR:
matCache.SetColor(BVA_Material_ZeldaToon_Extra.SPECULARCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ZeldaToon_Extra.RIMCOLOR:
matCache.SetColor(BVA_Material_ZeldaToon_Extra.RIMCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_ZeldaToon_Extra.RIMAMOUNT:
matCache.SetFloat(BVA_Material_ZeldaToon_Extra.RIMAMOUNT, reader.ReadAsFloat());
break;
case BVA_Material_ZeldaToon_Extra.RIMTRESHOLD:
matCache.SetFloat(BVA_Material_ZeldaToon_Extra.RIMTRESHOLD, reader.ReadAsFloat());
break;
case BVA_Material_ZeldaToon_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_ZeldaToon_Extra.BASEMAP, tex);
}
break;
case BVA_Material_ZeldaToon_Extra.QUEUEOFFSET:
matCache.SetFloat(BVA_Material_ZeldaToon_Extra.QUEUEOFFSET, reader.ReadAsFloat());
break;
case BVA_Material_ZeldaToon_Extra.QUEUECONTROL:
matCache.SetFloat(BVA_Material_ZeldaToon_Extra.QUEUECONTROL, reader.ReadAsFloat());
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(parameter_BaseColor.ParamName, parameter_BaseColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_AmbientColor.ParamName, parameter_AmbientColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Glossiness.ParamName, parameter_Glossiness.Value);
jo.Add(parameter_SpecularColor.ParamName, parameter_SpecularColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_RimColor.ParamName, parameter_RimColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_RimAmount.ParamName, parameter_RimAmount.Value);
jo.Add(parameter_RimTreshold.ParamName, parameter_RimTreshold.Value);
if (parameter_BaseMap != null && parameter_BaseMap.Value != null) jo.Add(parameter_BaseMap.ParamName, parameter_BaseMap.Serialize());
jo.Add(parameter__QueueOffset.ParamName, parameter__QueueOffset.Value);
jo.Add(parameter__QueueControl.ParamName, parameter__QueueControl.Value);
return new JProperty(BVA_Material_ZeldaToon_Extra.SHADER_NAME, jo);
}
}
}
