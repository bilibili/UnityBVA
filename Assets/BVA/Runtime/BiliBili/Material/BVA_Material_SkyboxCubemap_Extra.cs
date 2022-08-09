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
public class BVA_Material_SkyboxCubemap_Extra : MaterialExtra
{
public const string PROPERTY = "BVA_Material_SkyboxCubemap_Extra";
public const string SHADER_NAME = "Skybox/Cubemap";
public const string TINT = "_Tint";
public const string EXPOSURE = "_Exposure";
public const string ROTATION = "_Rotation";
public const string TEX = "_Tex";
public MaterialParam<Color> parameter_TintColor = new MaterialParam<Color>(TINT, Color.white);
public MaterialParam<float> parameter_Exposure = new MaterialParam<float>(EXPOSURE, 1.0f);
public MaterialParam<float> parameter_Rotation = new MaterialParam<float>(ROTATION, 1.0f);
public MaterialCubemapParam parameter_CubemapHDR = new MaterialCubemapParam(TEX);
public BVA_Material_SkyboxCubemap_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
parameter_TintColor.Value = material.GetColor(parameter_TintColor.ParamName);
parameter_Exposure.Value = material.GetFloat(parameter_Exposure.ParamName);
parameter_Rotation.Value = material.GetFloat(parameter_Rotation.ParamName);
var parameter_cubemaphdr_temp = material.GetTexture(parameter_CubemapHDR.ParamName);
if (parameter_cubemaphdr_temp != null) parameter_CubemapHDR.Value = exportCubemapInfo(parameter_cubemaphdr_temp as Cubemap);
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
case BVA_Material_SkyboxCubemap_Extra.TINT:
matCache.SetColor(BVA_Material_SkyboxCubemap_Extra.TINT, reader.ReadAsRGBAColor().ToUnityColorRaw());
break;
case BVA_Material_SkyboxCubemap_Extra.EXPOSURE:
matCache.SetFloat(BVA_Material_SkyboxCubemap_Extra.EXPOSURE, reader.ReadAsFloat());
break;
case BVA_Material_SkyboxCubemap_Extra.ROTATION:
matCache.SetFloat(BVA_Material_SkyboxCubemap_Extra.ROTATION, reader.ReadAsFloat());
break;
case BVA_Material_SkyboxCubemap_Extra.TEX:
{
                                reader.Read();reader.Read();
Cubemap cubemap = await loadCubemap(new CubemapId() { Id = reader.ReadAsInt32().Value,Root = root });
matCache.SetTexture(BVA_Material_SkyboxCubemap_Extra.TEX, cubemap);
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
if (parameter_CubemapHDR != null) jo.Add(parameter_CubemapHDR.ParamName, parameter_CubemapHDR.Serialize());
return new JProperty(BVA_Material_SkyboxCubemap_Extra.SHADER_NAME, jo);
}
}
}
