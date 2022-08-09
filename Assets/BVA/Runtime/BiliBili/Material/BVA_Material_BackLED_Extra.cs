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
public class BVA_Material_BackLED_Extra : MaterialExtra
{
public const string PROPERTY = "BVA_Material_BackLED_Extra";
public const string SHADER_NAME = "Shader Graphs/UnityChanSSU/HoodieBackLED";
public const string BACKLEDTEXTURE = "_BackLEDTexture";
public const string POWER = "_Power";
public const string SAMPLETEXTURE2D1A1C7C7C8291CC8B862B9C37E54D7945TEXTURE1 = "_SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1";
public const string SAMPLETEXTURE2D755BF3AC89C16885B70CC1F20EBD00C4TEXTURE1 = "_SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1";
public MaterialTextureParam parameter__BackLEDTexture = new MaterialTextureParam(BACKLEDTEXTURE);
public MaterialParam<float> parameter__Power = new MaterialParam<float>(POWER, 1.0f);
public MaterialTextureParam parameter__SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1 = new MaterialTextureParam(SAMPLETEXTURE2D1A1C7C7C8291CC8B862B9C37E54D7945TEXTURE1);
public MaterialTextureParam parameter__SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1 = new MaterialTextureParam(SAMPLETEXTURE2D755BF3AC89C16885B70CC1F20EBD00C4TEXTURE1);
public string[] keywords;
public BVA_Material_BackLED_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
keywords = material.shaderKeywords;
var parameter__backledtexture_temp = material.GetTexture(parameter__BackLEDTexture.ParamName);
if (parameter__backledtexture_temp != null) parameter__BackLEDTexture.Value = exportTextureInfo(parameter__backledtexture_temp);
parameter__Power.Value = material.GetFloat(parameter__Power.ParamName);
var parameter__sampletexture2d_1a1c7c7c8291cc8b862b9c37e54d7945_texture_1_temp = material.GetTexture(parameter__SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1.ParamName);
if (parameter__sampletexture2d_1a1c7c7c8291cc8b862b9c37e54d7945_texture_1_temp != null) parameter__SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1.Value = exportTextureInfo(parameter__sampletexture2d_1a1c7c7c8291cc8b862b9c37e54d7945_texture_1_temp);
var parameter__sampletexture2d_755bf3ac89c16885b70cc1f20ebd00c4_texture_1_temp = material.GetTexture(parameter__SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1.ParamName);
if (parameter__sampletexture2d_755bf3ac89c16885b70cc1f20ebd00c4_texture_1_temp != null) parameter__SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1.Value = exportTextureInfo(parameter__sampletexture2d_755bf3ac89c16885b70cc1f20ebd00c4_texture_1_temp);
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
case BVA_Material_BackLED_Extra.BACKLEDTEXTURE:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_BackLED_Extra.BACKLEDTEXTURE, tex);
}
break;
case BVA_Material_BackLED_Extra.POWER:
matCache.SetFloat(BVA_Material_BackLED_Extra.POWER, reader.ReadAsFloat());
break;
case BVA_Material_BackLED_Extra.SAMPLETEXTURE2D1A1C7C7C8291CC8B862B9C37E54D7945TEXTURE1:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_BackLED_Extra.SAMPLETEXTURE2D1A1C7C7C8291CC8B862B9C37E54D7945TEXTURE1, tex);
}
break;
case BVA_Material_BackLED_Extra.SAMPLETEXTURE2D755BF3AC89C16885B70CC1F20EBD00C4TEXTURE1:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_BackLED_Extra.SAMPLETEXTURE2D755BF3AC89C16885B70CC1F20EBD00C4TEXTURE1, tex);
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
public override JProperty Serialize()
{
JObject jo = new JObject();
if (parameter__BackLEDTexture != null && parameter__BackLEDTexture.Value != null) jo.Add(parameter__BackLEDTexture.ParamName, parameter__BackLEDTexture.Serialize());
jo.Add(parameter__Power.ParamName, parameter__Power.Value);
if (parameter__SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1 != null && parameter__SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1.Value != null) jo.Add(parameter__SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1.ParamName, parameter__SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1.Serialize());
if (parameter__SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1 != null && parameter__SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1.Value != null) jo.Add(parameter__SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1.ParamName, parameter__SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1.Serialize());
if(keywords != null && keywords.Length > 0)
{
JArray jKeywords = new JArray();
foreach (var keyword in jKeywords)
jKeywords.Add(keyword);
jo.Add(nameof(keywords), jKeywords);
}
return new JProperty(BVA_Material_BackLED_Extra.SHADER_NAME, jo);
}
}
}
