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
public class BVA_Material_MirrorFloor_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_MirrorFloor_Extra";
public const string SHADER_NAME = "MirrorFloor";
public const string ALPHACUTOFF = "_AlphaCutoff";
public const string EMISSIONCOLOR = "_EmissionColor";
public const string REFLECTIONCOLOR = "_ReflectionColor";
public const string REFLECTIONTEXTURE = "_ReflectionTexture";
public const string WAVETEXTURE = "_WaveTexture";
public const string TILE1X = "_Tile1X";
public const string TILE1Y = "_Tile1Y";
public const string TILE2X = "_Tile2X";
public const string TILE2Y = "_Tile2Y";
public const string LIGHTCOLOR = "_LightColor";
public const string LIGHTTEXTURE = "_LightTexture";
public const string TEXCOORD = "_texcoord";
public const string QUEUEOFFSET = "_QueueOffset";
public const string QUEUECONTROL = "_QueueControl";
public MaterialParam<float> parameter_AlphaCutoff = new MaterialParam<float>(ALPHACUTOFF, 1.0f);
public MaterialParam<Color> parameter_EmissionColor = new MaterialParam<Color>(EMISSIONCOLOR, Color.white);
public MaterialParam<Color> parameter_ReflectionColor = new MaterialParam<Color>(REFLECTIONCOLOR, Color.white);
public MaterialTextureParam parameter_ReflectionTexture = new MaterialTextureParam(REFLECTIONTEXTURE);
public MaterialTextureParam parameter_WaveTexture = new MaterialTextureParam(WAVETEXTURE);
public MaterialParam<float> parameter_Tile1X = new MaterialParam<float>(TILE1X, 1.0f);
public MaterialParam<float> parameter_Tile1Y = new MaterialParam<float>(TILE1Y, 1.0f);
public MaterialParam<float> parameter_Tile2X = new MaterialParam<float>(TILE2X, 1.0f);
public MaterialParam<float> parameter_Tile2Y = new MaterialParam<float>(TILE2Y, 1.0f);
public MaterialParam<Color> parameter_LightColor = new MaterialParam<Color>(LIGHTCOLOR, Color.white);
public MaterialTextureParam parameter_LightTexture = new MaterialTextureParam(LIGHTTEXTURE);
public MaterialTextureParam parameter_ = new MaterialTextureParam(TEXCOORD);
public MaterialParam<float> parameter__QueueOffset = new MaterialParam<float>(QUEUEOFFSET, 1.0f);
public MaterialParam<float> parameter__QueueControl = new MaterialParam<float>(QUEUECONTROL, 1.0f);
public BVA_Material_MirrorFloor_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_AlphaCutoff.Value = material.GetFloat(parameter_AlphaCutoff.ParamName);
parameter_EmissionColor.Value = material.GetColor(parameter_EmissionColor.ParamName);
parameter_ReflectionColor.Value = material.GetColor(parameter_ReflectionColor.ParamName);
var parameter_reflectiontexture_temp = material.GetTexture(parameter_ReflectionTexture.ParamName);
if (parameter_reflectiontexture_temp != null) parameter_ReflectionTexture.Value = exportTextureInfo(parameter_reflectiontexture_temp);
var parameter_wavetexture_temp = material.GetTexture(parameter_WaveTexture.ParamName);
if (parameter_wavetexture_temp != null) parameter_WaveTexture.Value = exportTextureInfo(parameter_wavetexture_temp);
parameter_Tile1X.Value = material.GetFloat(parameter_Tile1X.ParamName);
parameter_Tile1Y.Value = material.GetFloat(parameter_Tile1Y.ParamName);
parameter_Tile2X.Value = material.GetFloat(parameter_Tile2X.ParamName);
parameter_Tile2Y.Value = material.GetFloat(parameter_Tile2Y.ParamName);
parameter_LightColor.Value = material.GetColor(parameter_LightColor.ParamName);
var parameter_lighttexture_temp = material.GetTexture(parameter_LightTexture.ParamName);
if (parameter_lighttexture_temp != null) parameter_LightTexture.Value = exportTextureInfo(parameter_lighttexture_temp);
var parameter__temp = material.GetTexture(parameter_.ParamName);
if (parameter__temp != null) parameter_.Value = exportTextureInfo(parameter__temp);
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
case BVA_Material_MirrorFloor_Extra.ALPHACUTOFF:
matCache.SetFloat(BVA_Material_MirrorFloor_Extra.ALPHACUTOFF, reader.ReadAsFloat());
break;
case BVA_Material_MirrorFloor_Extra.EMISSIONCOLOR:
matCache.SetColor(BVA_Material_MirrorFloor_Extra.EMISSIONCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_MirrorFloor_Extra.REFLECTIONCOLOR:
matCache.SetColor(BVA_Material_MirrorFloor_Extra.REFLECTIONCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_MirrorFloor_Extra.REFLECTIONTEXTURE:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MirrorFloor_Extra.REFLECTIONTEXTURE, tex);
}
break;
case BVA_Material_MirrorFloor_Extra.WAVETEXTURE:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MirrorFloor_Extra.WAVETEXTURE, tex);
}
break;
case BVA_Material_MirrorFloor_Extra.TILE1X:
matCache.SetFloat(BVA_Material_MirrorFloor_Extra.TILE1X, reader.ReadAsFloat());
break;
case BVA_Material_MirrorFloor_Extra.TILE1Y:
matCache.SetFloat(BVA_Material_MirrorFloor_Extra.TILE1Y, reader.ReadAsFloat());
break;
case BVA_Material_MirrorFloor_Extra.TILE2X:
matCache.SetFloat(BVA_Material_MirrorFloor_Extra.TILE2X, reader.ReadAsFloat());
break;
case BVA_Material_MirrorFloor_Extra.TILE2Y:
matCache.SetFloat(BVA_Material_MirrorFloor_Extra.TILE2Y, reader.ReadAsFloat());
break;
case BVA_Material_MirrorFloor_Extra.LIGHTCOLOR:
matCache.SetColor(BVA_Material_MirrorFloor_Extra.LIGHTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_MirrorFloor_Extra.LIGHTTEXTURE:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MirrorFloor_Extra.LIGHTTEXTURE, tex);
}
break;
case BVA_Material_MirrorFloor_Extra.TEXCOORD:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_MirrorFloor_Extra.TEXCOORD, tex);
}
break;
case BVA_Material_MirrorFloor_Extra.QUEUEOFFSET:
matCache.SetFloat(BVA_Material_MirrorFloor_Extra.QUEUEOFFSET, reader.ReadAsFloat());
break;
case BVA_Material_MirrorFloor_Extra.QUEUECONTROL:
matCache.SetFloat(BVA_Material_MirrorFloor_Extra.QUEUECONTROL, reader.ReadAsFloat());
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(parameter_AlphaCutoff.ParamName, parameter_AlphaCutoff.Value);
jo.Add(parameter_EmissionColor.ParamName, parameter_EmissionColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_ReflectionColor.ParamName, parameter_ReflectionColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_ReflectionTexture != null && parameter_ReflectionTexture.Value != null) jo.Add(parameter_ReflectionTexture.ParamName, parameter_ReflectionTexture.Serialize());
if (parameter_WaveTexture != null && parameter_WaveTexture.Value != null) jo.Add(parameter_WaveTexture.ParamName, parameter_WaveTexture.Serialize());
jo.Add(parameter_Tile1X.ParamName, parameter_Tile1X.Value);
jo.Add(parameter_Tile1Y.ParamName, parameter_Tile1Y.Value);
jo.Add(parameter_Tile2X.ParamName, parameter_Tile2X.Value);
jo.Add(parameter_Tile2Y.ParamName, parameter_Tile2Y.Value);
jo.Add(parameter_LightColor.ParamName, parameter_LightColor.Value.ToNumericsColorRaw().ToJArray());
if (parameter_LightTexture != null && parameter_LightTexture.Value != null) jo.Add(parameter_LightTexture.ParamName, parameter_LightTexture.Serialize());
if (parameter_ != null && parameter_.Value != null) jo.Add(parameter_.ParamName, parameter_.Serialize());
jo.Add(parameter__QueueOffset.ParamName, parameter__QueueOffset.Value);
jo.Add(parameter__QueueControl.ParamName, parameter__QueueControl.Value);
return new JProperty(BVA_Material_MirrorFloor_Extra.SHADER_NAME, jo);
}
}
}
