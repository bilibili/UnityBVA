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
    public class BVA_Material_SkyboxPanoramic_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_SkyboxPanoramic_Extra";
        public const string SHADER_NAME = "Skybox/Panoramic";
        public const string TINT = "_Tint";
        public const string EXPOSURE = "_Exposure";
        public const string ROTATION = "_Rotation";
        public const string MAINTEX = "_MainTex";
        public const string MAPPING = "_Mapping";
        public const string IMAGETYPE = "_ImageType";
        public const string MIRRORONBACK = "_MirrorOnBack";
        public const string LAYOUT = "_Layout";
        public MaterialParam<Color> parameter__Tint = new MaterialParam<Color>(TINT, Color.white);
        public MaterialParam<float> parameter__Exposure = new MaterialParam<float>(EXPOSURE, 1.0f);
        public MaterialParam<float> parameter__Rotation = new MaterialParam<float>(ROTATION, 1.0f);
        public MaterialTextureParam parameter__MainTex = new MaterialTextureParam(MAINTEX);
        public MaterialParam<float> parameter__Mapping = new MaterialParam<float>(MAPPING, 1.0f);
        public MaterialParam<float> parameter__ImageType = new MaterialParam<float>(IMAGETYPE, 1.0f);
        public MaterialParam<float> parameter__MirrorOnBack = new MaterialParam<float>(MIRRORONBACK, 1.0f);
        public MaterialParam<float> parameter__Layout = new MaterialParam<float>(LAYOUT, 1.0f);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            parameter__Tint.Value = material.GetColor(parameter__Tint.ParamName);
            parameter__Exposure.Value = material.GetFloat(parameter__Exposure.ParamName);
            parameter__Rotation.Value = material.GetFloat(parameter__Rotation.ParamName);
            var parameter__maintex_temp = material.GetTexture(parameter__MainTex.ParamName);
            if (parameter__maintex_temp != null) parameter__MainTex.Value = exportTextureInfo(parameter__maintex_temp);
            parameter__Mapping.Value = material.GetFloat(parameter__Mapping.ParamName);
            parameter__ImageType.Value = material.GetFloat(parameter__ImageType.ParamName);
            parameter__MirrorOnBack.Value = material.GetFloat(parameter__MirrorOnBack.ParamName);
            parameter__Layout.Value = material.GetFloat(parameter__Layout.ParamName);
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
                        case BVA_Material_SkyboxPanoramic_Extra.TINT:
                            matCache.SetColor(BVA_Material_SkyboxPanoramic_Extra.TINT, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_SkyboxPanoramic_Extra.EXPOSURE:
                            matCache.SetFloat(BVA_Material_SkyboxPanoramic_Extra.EXPOSURE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxPanoramic_Extra.ROTATION:
                            matCache.SetFloat(BVA_Material_SkyboxPanoramic_Extra.ROTATION, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxPanoramic_Extra.MAINTEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_SkyboxPanoramic_Extra.MAINTEX, tex);
                            }
                            break;
                        case BVA_Material_SkyboxPanoramic_Extra.MAPPING:
                            matCache.SetFloat(BVA_Material_SkyboxPanoramic_Extra.MAPPING, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxPanoramic_Extra.IMAGETYPE:
                            matCache.SetFloat(BVA_Material_SkyboxPanoramic_Extra.IMAGETYPE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxPanoramic_Extra.MIRRORONBACK:
                            matCache.SetFloat(BVA_Material_SkyboxPanoramic_Extra.MIRRORONBACK, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxPanoramic_Extra.LAYOUT:
                            matCache.SetFloat(BVA_Material_SkyboxPanoramic_Extra.LAYOUT, reader.ReadAsFloat());
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
            if (parameter__MainTex != null && parameter__MainTex.Value != null) jo.Add(parameter__MainTex.ParamName, parameter__MainTex.Serialize());
            jo.Add(parameter__Mapping.ParamName, parameter__Mapping.Value);
            jo.Add(parameter__ImageType.ParamName, parameter__ImageType.Value);
            jo.Add(parameter__MirrorOnBack.ParamName, parameter__MirrorOnBack.Value);
            jo.Add(parameter__Layout.ParamName, parameter__Layout.Value);
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_SkyboxPanoramic_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_SkyboxPanoramic_Extra();
        }
    }
}
