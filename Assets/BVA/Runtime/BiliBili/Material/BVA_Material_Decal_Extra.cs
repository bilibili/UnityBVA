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
public class BVA_Material_Decal_Extra : MaterialDescriptor
{
public const string PROPERTY = "BVA_Material_Decal_Extra";
public const string SHADER_NAME = "Shader Graphs/Decal";
public const string BASEMAP = "Base_Map";
public const string NORMALMAP = "Normal_Map";
public const string NORMALBLEND = "Normal_Blend";
public const string DRAWORDER = "_DrawOrder";
public const string DECALMESHBIASTYPE = "_DecalMeshBiasType";
public const string DECALMESHDEPTHBIAS = "_DecalMeshDepthBias";
public const string DECALMESHVIEWBIAS = "_DecalMeshViewBias";
public const string UNITYLIGHTMAPS = "unity_Lightmaps";
public const string UNITYLIGHTMAPSIND = "unity_LightmapsInd";
public const string UNITYSHADOWMASKS = "unity_ShadowMasks";
public MaterialTextureParam parameter_BaseMap = new MaterialTextureParam(BASEMAP);
public MaterialTextureParam parameter_NormalMap = new MaterialTextureParam(NORMALMAP);
public MaterialParam<float> parameter_NormalBlend = new MaterialParam<float>(NORMALBLEND, 1.0f);
public MaterialParam<float> parameter_DrawOrder = new MaterialParam<float>(DRAWORDER, 1.0f);
public MaterialParam<float> parameter_DecalMeshBiasType = new MaterialParam<float>(DECALMESHBIASTYPE, 1.0f);
public MaterialParam<float> parameter_DecalMeshDepthBias = new MaterialParam<float>(DECALMESHDEPTHBIAS, 1.0f);
public MaterialParam<float> parameter_DecalMeshViewBias = new MaterialParam<float>(DECALMESHVIEWBIAS, 1.0f);
public MaterialTextureParam parameter_unity_Lightmaps = new MaterialTextureParam(UNITYLIGHTMAPS);
public MaterialTextureParam parameter_unity_LightmapsInd = new MaterialTextureParam(UNITYLIGHTMAPSIND);
public MaterialTextureParam parameter_unity_ShadowMasks = new MaterialTextureParam(UNITYSHADOWMASKS);
public BVA_Material_Decal_Extra(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalMapInfo, ExportCubemapInfo exportCubemapInfo)
{
var parameter_basemap_temp = material.GetTexture(parameter_BaseMap.ParamName);
if (parameter_basemap_temp != null) parameter_BaseMap.Value = exportTextureInfo(parameter_basemap_temp);
var parameter_normalmap_temp = material.GetTexture(parameter_NormalMap.ParamName);
if (parameter_normalmap_temp != null) parameter_NormalMap.Value = exportNormalMapInfo(parameter_normalmap_temp);
parameter_NormalBlend.Value = material.GetFloat(parameter_NormalBlend.ParamName);
parameter_DrawOrder.Value = material.GetFloat(parameter_DrawOrder.ParamName);
parameter_DecalMeshBiasType.Value = material.GetFloat(parameter_DecalMeshBiasType.ParamName);
parameter_DecalMeshDepthBias.Value = material.GetFloat(parameter_DecalMeshDepthBias.ParamName);
parameter_DecalMeshViewBias.Value = material.GetFloat(parameter_DecalMeshViewBias.ParamName);
var parameter_unity_lightmaps_temp = material.GetTexture(parameter_unity_Lightmaps.ParamName);
if (parameter_unity_lightmaps_temp != null) parameter_unity_Lightmaps.Value = exportTextureInfo(parameter_unity_lightmaps_temp);
var parameter_unity_lightmapsind_temp = material.GetTexture(parameter_unity_LightmapsInd.ParamName);
if (parameter_unity_lightmapsind_temp != null) parameter_unity_LightmapsInd.Value = exportTextureInfo(parameter_unity_lightmapsind_temp);
var parameter_unity_shadowmasks_temp = material.GetTexture(parameter_unity_ShadowMasks.ParamName);
if (parameter_unity_shadowmasks_temp != null) parameter_unity_ShadowMasks.Value = exportTextureInfo(parameter_unity_shadowmasks_temp);
}
public static async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache, AsyncLoadTexture loadTexture,AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case BVA_Material_Decal_Extra.BASEMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_Decal_Extra.BASEMAP, tex);
}
break;
case BVA_Material_Decal_Extra.NORMALMAP:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadNormalMap(texInfo.Index);
matCache.SetTexture(BVA_Material_Decal_Extra.NORMALMAP, tex);
}
break;
case BVA_Material_Decal_Extra.NORMALBLEND:
matCache.SetFloat(BVA_Material_Decal_Extra.NORMALBLEND, reader.ReadAsFloat());
break;
case BVA_Material_Decal_Extra.DRAWORDER:
matCache.SetFloat(BVA_Material_Decal_Extra.DRAWORDER, reader.ReadAsFloat());
break;
case BVA_Material_Decal_Extra.DECALMESHBIASTYPE:
matCache.SetFloat(BVA_Material_Decal_Extra.DECALMESHBIASTYPE, reader.ReadAsFloat());
break;
case BVA_Material_Decal_Extra.DECALMESHDEPTHBIAS:
matCache.SetFloat(BVA_Material_Decal_Extra.DECALMESHDEPTHBIAS, reader.ReadAsFloat());
break;
case BVA_Material_Decal_Extra.DECALMESHVIEWBIAS:
matCache.SetFloat(BVA_Material_Decal_Extra.DECALMESHVIEWBIAS, reader.ReadAsFloat());
break;
case BVA_Material_Decal_Extra.UNITYLIGHTMAPS:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_Decal_Extra.UNITYLIGHTMAPS, tex);
}
break;
case BVA_Material_Decal_Extra.UNITYLIGHTMAPSIND:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_Decal_Extra.UNITYLIGHTMAPSIND, tex);
}
break;
case BVA_Material_Decal_Extra.UNITYSHADOWMASKS:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_Decal_Extra.UNITYSHADOWMASKS, tex);
}
break;
}
}
}
}
public override JProperty Serialize()
{
JObject jo = new JObject();
if (parameter_BaseMap != null && parameter_BaseMap.Value != null) jo.Add(parameter_BaseMap.ParamName, parameter_BaseMap.Serialize());
if (parameter_NormalMap != null && parameter_NormalMap.Value != null) jo.Add(parameter_NormalMap.ParamName, parameter_NormalMap.Serialize());
jo.Add(parameter_NormalBlend.ParamName, parameter_NormalBlend.Value);
jo.Add(parameter_DrawOrder.ParamName, parameter_DrawOrder.Value);
jo.Add(parameter_DecalMeshBiasType.ParamName, parameter_DecalMeshBiasType.Value);
jo.Add(parameter_DecalMeshDepthBias.ParamName, parameter_DecalMeshDepthBias.Value);
jo.Add(parameter_DecalMeshViewBias.ParamName, parameter_DecalMeshViewBias.Value);
if (parameter_unity_Lightmaps != null && parameter_unity_Lightmaps.Value != null) jo.Add(parameter_unity_Lightmaps.ParamName, parameter_unity_Lightmaps.Serialize());
if (parameter_unity_LightmapsInd != null && parameter_unity_LightmapsInd.Value != null) jo.Add(parameter_unity_LightmapsInd.ParamName, parameter_unity_LightmapsInd.Serialize());
if (parameter_unity_ShadowMasks != null && parameter_unity_ShadowMasks.Value != null) jo.Add(parameter_unity_ShadowMasks.ParamName, parameter_unity_ShadowMasks.Serialize());
return new JProperty(BVA_Material_Decal_Extra.SHADER_NAME, jo);
}
}
}
