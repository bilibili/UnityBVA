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
    public class BVA_Material_Decal_Extra : IMaterialExtra
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
        public MaterialTextureParam parameter_Base_Map = new MaterialTextureParam(BASEMAP);
        public MaterialTextureParam parameter_Normal_Map = new MaterialTextureParam(NORMALMAP);
        public MaterialParam<float> parameter_Normal_Blend = new MaterialParam<float>(NORMALBLEND, 1.0f);
        public MaterialParam<float> parameter__DrawOrder = new MaterialParam<float>(DRAWORDER, 1.0f);
        public MaterialParam<float> parameter__DecalMeshBiasType = new MaterialParam<float>(DECALMESHBIASTYPE, 1.0f);
        public MaterialParam<float> parameter__DecalMeshDepthBias = new MaterialParam<float>(DECALMESHDEPTHBIAS, 1.0f);
        public MaterialParam<float> parameter__DecalMeshViewBias = new MaterialParam<float>(DECALMESHVIEWBIAS, 1.0f);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            var parameter_base_map_temp = material.GetTexture(parameter_Base_Map.ParamName);
            if (parameter_base_map_temp != null) parameter_Base_Map.Value = exportTextureInfo(parameter_base_map_temp);
            var parameter_normal_map_temp = material.GetTexture(parameter_Normal_Map.ParamName);
            if (parameter_normal_map_temp != null) parameter_Normal_Map.Value = exportNormalTextureInfo(parameter_normal_map_temp);
            parameter_Normal_Blend.Value = material.GetFloat(parameter_Normal_Blend.ParamName);
            parameter__DrawOrder.Value = material.GetFloat(parameter__DrawOrder.ParamName);
            parameter__DecalMeshBiasType.Value = material.GetFloat(parameter__DecalMeshBiasType.ParamName);
            parameter__DecalMeshDepthBias.Value = material.GetFloat(parameter__DecalMeshDepthBias.ParamName);
            parameter__DecalMeshViewBias.Value = material.GetFloat(parameter__DecalMeshViewBias.ParamName);
        }
        public async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache, AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
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
                                var tex = await loadTexture(texInfo.Index);
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
            if (parameter_Base_Map != null && parameter_Base_Map.Value != null) jo.Add(parameter_Base_Map.ParamName, parameter_Base_Map.Serialize());
            if (parameter_Normal_Map != null && parameter_Normal_Map.Value != null) jo.Add(parameter_Normal_Map.ParamName, parameter_Normal_Map.Serialize());
            jo.Add(parameter_Normal_Blend.ParamName, parameter_Normal_Blend.Value);
            jo.Add(parameter__DrawOrder.ParamName, parameter__DrawOrder.Value);
            jo.Add(parameter__DecalMeshBiasType.ParamName, parameter__DecalMeshBiasType.Value);
            jo.Add(parameter__DecalMeshDepthBias.ParamName, parameter__DecalMeshDepthBias.Value);
            jo.Add(parameter__DecalMeshViewBias.ParamName, parameter__DecalMeshViewBias.Value);
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_Decal_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_Decal_Extra();
        }
    }
}
