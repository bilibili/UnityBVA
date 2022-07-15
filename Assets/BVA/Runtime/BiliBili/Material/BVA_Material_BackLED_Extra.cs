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
public class BVA_Material_BackLED_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_BackLED_Extra";
public const string SHADER_NAME = "Shader Graphs/UnityChanSSU/HoodieBackLED";
public const string BACKLEDTEXTURE = "_BackLEDTexture";
public const string POWER = "_Power";
public const string SAMPLETEXTURE2D1A1C7C7C8291CC8B862B9C37E54D7945TEXTURE1 = "_SampleTexture2D_1a1c7c7c8291cc8b862b9c37e54d7945_Texture_1";
public const string SAMPLETEXTURE2D755BF3AC89C16885B70CC1F20EBD00C4TEXTURE1 = "_SampleTexture2D_755bf3ac89c16885b70cc1f20ebd00c4_Texture_1";
public MaterialTextureParam parameter_HoodieBackLED = new MaterialTextureParam(BACKLEDTEXTURE);
public MaterialParam<float> parameter_LEDPower = new MaterialParam<float>(POWER, 1.0f);

public BVA_Material_BackLED_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemapInfo exportCubemapInfo)
{
var parameter_hoodiebackled_temp = material.GetTexture(parameter_HoodieBackLED.ParamName);
if (parameter_hoodiebackled_temp != null) parameter_HoodieBackLED.Value = exportTextureInfo(parameter_hoodiebackled_temp);
parameter_LEDPower.Value = material.GetFloat(parameter_LEDPower.ParamName);
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
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
if (parameter_HoodieBackLED != null && parameter_HoodieBackLED.Value != null) jo.Add(parameter_HoodieBackLED.ParamName, parameter_HoodieBackLED.Serialize());
jo.Add(parameter_LEDPower.ParamName, parameter_LEDPower.Value);
return new JProperty(BVA_Material_BackLED_Extra.SHADER_NAME, jo);
}
}
}
