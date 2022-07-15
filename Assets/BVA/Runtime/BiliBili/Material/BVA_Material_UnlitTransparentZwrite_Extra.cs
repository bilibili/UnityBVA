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
public class BVA_Material_UnlitTransparentZwrite_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_UnlitTransparentZwrite_Extra";
public const string SHADER_NAME = "VRM/UnlitTransparentZWrite";
public const string MAINTEX = "_MainTex";
public MaterialTextureParam parameter_BaseRGBTransA = new MaterialTextureParam(MAINTEX);
public BVA_Material_UnlitTransparentZwrite_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
var parameter_basergbtransa_temp = material.GetTexture(parameter_BaseRGBTransA.ParamName);
if (parameter_basergbtransa_temp != null) parameter_BaseRGBTransA.Value = exportTextureInfo(parameter_basergbtransa_temp);
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
case BVA_Material_UnlitTransparentZwrite_Extra.MAINTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_UnlitTransparentZwrite_Extra.MAINTEX, tex);
}
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
if (parameter_BaseRGBTransA != null && parameter_BaseRGBTransA.Value != null) jo.Add(parameter_BaseRGBTransA.ParamName, parameter_BaseRGBTransA.Serialize());
return new JProperty(BVA_Material_UnlitTransparentZwrite_Extra.SHADER_NAME, jo);
}
}
}
