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
public class BVA_Material_SkyBox6Sided_Extra : IMaterialExtra
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
public MaterialParam<Color> parameter__Tint = new MaterialParam<Color>(TINT, Color.white);
public MaterialParam<float> parameter__Exposure = new MaterialParam<float>(EXPOSURE, 1.0f);
public MaterialParam<float> parameter__Rotation = new MaterialParam<float>(ROTATION, 1.0f);
public MaterialTextureParam parameter__FrontTex = new MaterialTextureParam(FRONTTEX);
public MaterialTextureParam parameter__BackTex = new MaterialTextureParam(BACKTEX);
public MaterialTextureParam parameter__LeftTex = new MaterialTextureParam(LEFTTEX);
public MaterialTextureParam parameter__RightTex = new MaterialTextureParam(RIGHTTEX);
public MaterialTextureParam parameter__UpTex = new MaterialTextureParam(UPTEX);
public MaterialTextureParam parameter__DownTex = new MaterialTextureParam(DOWNTEX);
public string[] keywords;
public string ShaderName => SHADER_NAME;
public string ExtraName => GetType().Name;
public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
{
keywords = material.shaderKeywords;
parameter__Tint.Value = material.GetColor(parameter__Tint.ParamName);
parameter__Exposure.Value = material.GetFloat(parameter__Exposure.ParamName);
parameter__Rotation.Value = material.GetFloat(parameter__Rotation.ParamName);
var parameter__fronttex_temp = material.GetTexture(parameter__FrontTex.ParamName);
if (parameter__fronttex_temp != null) parameter__FrontTex.Value = exportTextureInfo(parameter__fronttex_temp);
var parameter__backtex_temp = material.GetTexture(parameter__BackTex.ParamName);
if (parameter__backtex_temp != null) parameter__BackTex.Value = exportTextureInfo(parameter__backtex_temp);
var parameter__lefttex_temp = material.GetTexture(parameter__LeftTex.ParamName);
if (parameter__lefttex_temp != null) parameter__LeftTex.Value = exportTextureInfo(parameter__lefttex_temp);
var parameter__righttex_temp = material.GetTexture(parameter__RightTex.ParamName);
if (parameter__righttex_temp != null) parameter__RightTex.Value = exportTextureInfo(parameter__righttex_temp);
var parameter__uptex_temp = material.GetTexture(parameter__UpTex.ParamName);
if (parameter__uptex_temp != null) parameter__UpTex.Value = exportTextureInfo(parameter__uptex_temp);
var parameter__downtex_temp = material.GetTexture(parameter__DownTex.ParamName);
if (parameter__downtex_temp != null) parameter__DownTex.Value = exportTextureInfo(parameter__downtex_temp);
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
jo.Add(parameter__Tint.ParamName, parameter__Tint.Value.ToNumericsColorRaw().ToJArray());
jo.Add(parameter__Exposure.ParamName, parameter__Exposure.Value);
jo.Add(parameter__Rotation.ParamName, parameter__Rotation.Value);
if (parameter__FrontTex != null && parameter__FrontTex.Value != null) jo.Add(parameter__FrontTex.ParamName, parameter__FrontTex.Serialize());
if (parameter__BackTex != null && parameter__BackTex.Value != null) jo.Add(parameter__BackTex.ParamName, parameter__BackTex.Serialize());
if (parameter__LeftTex != null && parameter__LeftTex.Value != null) jo.Add(parameter__LeftTex.ParamName, parameter__LeftTex.Serialize());
if (parameter__RightTex != null && parameter__RightTex.Value != null) jo.Add(parameter__RightTex.ParamName, parameter__RightTex.Serialize());
if (parameter__UpTex != null && parameter__UpTex.Value != null) jo.Add(parameter__UpTex.ParamName, parameter__UpTex.Serialize());
if (parameter__DownTex != null && parameter__DownTex.Value != null) jo.Add(parameter__DownTex.ParamName, parameter__DownTex.Serialize());
if(keywords != null && keywords.Length > 0)
{
JArray jKeywords = new JArray();
foreach (var keyword in jKeywords)
jKeywords.Add(keyword);
jo.Add(nameof(keywords), jKeywords);
}
return new JProperty(BVA_Material_SkyBox6Sided_Extra.SHADER_NAME, jo);
}

        public object Clone()
        {
            return new BVA_Material_SkyBox6Sided_Extra();
        }
    }
}
