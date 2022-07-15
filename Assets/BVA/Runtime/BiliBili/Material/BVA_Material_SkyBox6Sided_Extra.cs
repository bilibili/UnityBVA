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
public class BVA_Material_SkyBox6Sided_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_SkyBox6Sided_Extra";
public const string SHADER_NAME = "Skybox/6 Sided";
public const string TINT = "_Tint";
public const string EXPOSURE = "_Exposure";
public const string ROTATION = "_Rotation";
public const string FRONTTEX = "_FrontTex";
public const string BACKTEX = "_BackTex";
public const string LEFTTEX = "_LeftTex";
public const string RIGHTTEX = "_RightTex";
public const string UPTEX = "_UpTex";
public const string DOWNTEX = "_DownTex";
public MaterialParam<Color> parameter_TintColor = new MaterialParam<Color>(TINT, Color.white);
public MaterialParam<float> parameter_Exposure = new MaterialParam<float>(EXPOSURE, 1.0f);
public MaterialParam<float> parameter_Rotation = new MaterialParam<float>(ROTATION, 1.0f);
public MaterialTextureParam parameter_FrontZHDR = new MaterialTextureParam(FRONTTEX);
public MaterialTextureParam parameter_BackZHDR = new MaterialTextureParam(BACKTEX);
public MaterialTextureParam parameter_LeftXHDR = new MaterialTextureParam(LEFTTEX);
public MaterialTextureParam parameter_RightXHDR = new MaterialTextureParam(RIGHTTEX);
public MaterialTextureParam parameter_UpYHDR = new MaterialTextureParam(UPTEX);
public MaterialTextureParam parameter_DownYHDR = new MaterialTextureParam(DOWNTEX);
public BVA_Material_SkyBox6Sided_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_TintColor.Value = material.GetColor(parameter_TintColor.ParamName);
parameter_Exposure.Value = material.GetFloat(parameter_Exposure.ParamName);
parameter_Rotation.Value = material.GetFloat(parameter_Rotation.ParamName);
var parameter_frontzhdr_temp = material.GetTexture(parameter_FrontZHDR.ParamName);
if (parameter_frontzhdr_temp != null) parameter_FrontZHDR.Value = exportTextureInfo(parameter_frontzhdr_temp);
var parameter_backzhdr_temp = material.GetTexture(parameter_BackZHDR.ParamName);
if (parameter_backzhdr_temp != null) parameter_BackZHDR.Value = exportTextureInfo(parameter_backzhdr_temp);
var parameter_leftxhdr_temp = material.GetTexture(parameter_LeftXHDR.ParamName);
if (parameter_leftxhdr_temp != null) parameter_LeftXHDR.Value = exportTextureInfo(parameter_leftxhdr_temp);
var parameter_rightxhdr_temp = material.GetTexture(parameter_RightXHDR.ParamName);
if (parameter_rightxhdr_temp != null) parameter_RightXHDR.Value = exportTextureInfo(parameter_rightxhdr_temp);
var parameter_upyhdr_temp = material.GetTexture(parameter_UpYHDR.ParamName);
if (parameter_upyhdr_temp != null) parameter_UpYHDR.Value = exportTextureInfo(parameter_upyhdr_temp);
var parameter_downyhdr_temp = material.GetTexture(parameter_DownYHDR.ParamName);
if (parameter_downyhdr_temp != null) parameter_DownYHDR.Value = exportTextureInfo(parameter_downyhdr_temp);
}
public static async Task Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.Material matCache, AsyncLoadTexture loadTexture,AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case BVA_Material_SkyBox6Sided_Extra.TINT:
matCache.SetColor(BVA_Material_SkyBox6Sided_Extra.TINT, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_SkyBox6Sided_Extra.EXPOSURE:
matCache.SetFloat(BVA_Material_SkyBox6Sided_Extra.EXPOSURE, reader.ReadAsFloat());
break;
case BVA_Material_SkyBox6Sided_Extra.ROTATION:
matCache.SetFloat(BVA_Material_SkyBox6Sided_Extra.ROTATION, reader.ReadAsFloat());
break;
case BVA_Material_SkyBox6Sided_Extra.FRONTTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_SkyBox6Sided_Extra.FRONTTEX, tex);
}
break;
case BVA_Material_SkyBox6Sided_Extra.BACKTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_SkyBox6Sided_Extra.BACKTEX, tex);
}
break;
case BVA_Material_SkyBox6Sided_Extra.LEFTTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_SkyBox6Sided_Extra.LEFTTEX, tex);
}
break;
case BVA_Material_SkyBox6Sided_Extra.RIGHTTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_SkyBox6Sided_Extra.RIGHTTEX, tex);
}
break;
case BVA_Material_SkyBox6Sided_Extra.UPTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_SkyBox6Sided_Extra.UPTEX, tex);
}
break;
case BVA_Material_SkyBox6Sided_Extra.DOWNTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_SkyBox6Sided_Extra.DOWNTEX, tex);
}
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(parameter_TintColor.ParamName, parameter_TintColor.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter_Exposure.ParamName, parameter_Exposure.Value);
jo.Add(parameter_Rotation.ParamName, parameter_Rotation.Value);
if (parameter_FrontZHDR != null && parameter_FrontZHDR.Value != null) jo.Add(parameter_FrontZHDR.ParamName, parameter_FrontZHDR.Serialize());
if (parameter_BackZHDR != null && parameter_BackZHDR.Value != null) jo.Add(parameter_BackZHDR.ParamName, parameter_BackZHDR.Serialize());
if (parameter_LeftXHDR != null && parameter_LeftXHDR.Value != null) jo.Add(parameter_LeftXHDR.ParamName, parameter_LeftXHDR.Serialize());
if (parameter_RightXHDR != null && parameter_RightXHDR.Value != null) jo.Add(parameter_RightXHDR.ParamName, parameter_RightXHDR.Serialize());
if (parameter_UpYHDR != null && parameter_UpYHDR.Value != null) jo.Add(parameter_UpYHDR.ParamName, parameter_UpYHDR.Serialize());
if (parameter_DownYHDR != null && parameter_DownYHDR.Value != null) jo.Add(parameter_DownYHDR.ParamName, parameter_DownYHDR.Serialize());
return new JProperty(BVA_Material_SkyBox6Sided_Extra.SHADER_NAME, jo);
}
}
}
