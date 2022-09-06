using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;
using GLTF.Schema.BVA;

namespace GLTF.Schema.BVA
{
    [MaterialExtra]
    public class BVA_Material_SkyboxCubemap_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_SkyboxCubemap_Extra";
        public const string SHADER_NAME = "Skybox/Cubemap";
        public const string TINT = "_Tint";
        public const string EXPOSURE = "_Exposure";
        public const string ROTATION = "_Rotation";
        public const string TEX = "_Tex";
        public MaterialParam<Color> parameter__Tint = new MaterialParam<Color>(TINT, Color.white);
        public MaterialParam<float> parameter__Exposure = new MaterialParam<float>(EXPOSURE, 1.0f);
        public MaterialParam<float> parameter__Rotation = new MaterialParam<float>(ROTATION, 1.0f);
        public MaterialCubemapParam parameter__Tex = new MaterialCubemapParam(TEX);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            parameter__Tint.Value = material.GetColor(parameter__Tint.ParamName);
            parameter__Exposure.Value = material.GetFloat(parameter__Exposure.ParamName);
            parameter__Rotation.Value = material.GetFloat(parameter__Rotation.ParamName);
            var parameter__tex_temp = material.GetTexture(parameter__Tex.ParamName);
            if (parameter__tex_temp != null) parameter__Tex.Value = exportCubemapInfo(parameter__tex_temp as Cubemap);
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
                        case BVA_Material_SkyboxCubemap_Extra.TINT:
                            matCache.SetColor(BVA_Material_SkyboxCubemap_Extra.TINT, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_SkyboxCubemap_Extra.EXPOSURE:
                            matCache.SetFloat(BVA_Material_SkyboxCubemap_Extra.EXPOSURE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxCubemap_Extra.ROTATION:
                            matCache.SetFloat(BVA_Material_SkyboxCubemap_Extra.ROTATION, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxCubemap_Extra.TEX:
                            {
                                reader.Read(); reader.Read();
                                Cubemap cubemap = await loadCubemap(new CubemapId() { Id = reader.ReadAsInt32().Value, Root = root });
                                matCache.SetTexture(BVA_Material_SkyboxCubemap_Extra.TEX, cubemap);
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
            jo.Add(parameter__Tint.ParamName, parameter__Tint.Value.ToJArray());
            jo.Add(parameter__Exposure.ParamName, parameter__Exposure.Value);
            jo.Add(parameter__Rotation.ParamName, parameter__Rotation.Value);
            if (parameter__Tex != null) jo.Add(parameter__Tex.ParamName, parameter__Tex.Serialize());
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_SkyboxCubemap_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_SkyboxCubemap_Extra();
        }
    }
}
