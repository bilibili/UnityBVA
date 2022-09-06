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
    public class BVA_Material_SkyboxProcedural_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_SkyboxProcedural_Extra";
        public const string SHADER_NAME = "Skybox/Procedural";
        public const string SUNDISK = "_SunDisk";
        public const string SUNSIZE = "_SunSize";
        public const string SUNSIZECONVERGENCE = "_SunSizeConvergence";
        public const string ATMOSPHERETHICKNESS = "_AtmosphereThickness";
        public const string SKYTINT = "_SkyTint";
        public const string GROUNDCOLOR = "_GroundColor";
        public const string EXPOSURE = "_Exposure";
        public MaterialParam<float> parameter__SunDisk = new MaterialParam<float>(SUNDISK, 1.0f);
        public MaterialParam<float> parameter__SunSize = new MaterialParam<float>(SUNSIZE, 1.0f);
        public MaterialParam<float> parameter__SunSizeConvergence = new MaterialParam<float>(SUNSIZECONVERGENCE, 1.0f);
        public MaterialParam<float> parameter__AtmosphereThickness = new MaterialParam<float>(ATMOSPHERETHICKNESS, 1.0f);
        public MaterialParam<Color> parameter__SkyTint = new MaterialParam<Color>(SKYTINT, Color.white);
        public MaterialParam<Color> parameter__GroundColor = new MaterialParam<Color>(GROUNDCOLOR, Color.white);
        public MaterialParam<float> parameter__Exposure = new MaterialParam<float>(EXPOSURE, 1.0f);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            parameter__SunDisk.Value = material.GetFloat(parameter__SunDisk.ParamName);
            parameter__SunSize.Value = material.GetFloat(parameter__SunSize.ParamName);
            parameter__SunSizeConvergence.Value = material.GetFloat(parameter__SunSizeConvergence.ParamName);
            parameter__AtmosphereThickness.Value = material.GetFloat(parameter__AtmosphereThickness.ParamName);
            parameter__SkyTint.Value = material.GetColor(parameter__SkyTint.ParamName);
            parameter__GroundColor.Value = material.GetColor(parameter__GroundColor.ParamName);
            parameter__Exposure.Value = material.GetFloat(parameter__Exposure.ParamName);
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
                        case BVA_Material_SkyboxProcedural_Extra.SUNDISK:
                            matCache.SetFloat(BVA_Material_SkyboxProcedural_Extra.SUNDISK, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxProcedural_Extra.SUNSIZE:
                            matCache.SetFloat(BVA_Material_SkyboxProcedural_Extra.SUNSIZE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxProcedural_Extra.SUNSIZECONVERGENCE:
                            matCache.SetFloat(BVA_Material_SkyboxProcedural_Extra.SUNSIZECONVERGENCE, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxProcedural_Extra.ATMOSPHERETHICKNESS:
                            matCache.SetFloat(BVA_Material_SkyboxProcedural_Extra.ATMOSPHERETHICKNESS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_SkyboxProcedural_Extra.SKYTINT:
                            matCache.SetColor(BVA_Material_SkyboxProcedural_Extra.SKYTINT, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_SkyboxProcedural_Extra.GROUNDCOLOR:
                            matCache.SetColor(BVA_Material_SkyboxProcedural_Extra.GROUNDCOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_SkyboxProcedural_Extra.EXPOSURE:
                            matCache.SetFloat(BVA_Material_SkyboxProcedural_Extra.EXPOSURE, reader.ReadAsFloat());
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
            jo.Add(parameter__SunDisk.ParamName, parameter__SunDisk.Value);
            jo.Add(parameter__SunSize.ParamName, parameter__SunSize.Value);
            jo.Add(parameter__SunSizeConvergence.ParamName, parameter__SunSizeConvergence.Value);
            jo.Add(parameter__AtmosphereThickness.ParamName, parameter__AtmosphereThickness.Value);
            jo.Add(parameter__SkyTint.ParamName, parameter__SkyTint.Value.ToJArray());
            jo.Add(parameter__GroundColor.ParamName, parameter__GroundColor.Value.ToJArray());
            jo.Add(parameter__Exposure.ParamName, parameter__Exposure.Value);
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_SkyboxProcedural_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_SkyboxProcedural_Extra();
        }
    }
}
