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
    public class BVA_Material_ZeldaToon_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_ZeldaToon_Extra";
        public const string SHADER_NAME = "Shader Graphs/ZeldaToon";
        public const string BASECOLOR = "_BaseColor";
        public const string AMBIENTCOLOR = "_AmbientColor";
        public const string GLOSSINESS = "_Glossiness";
        public const string SPECULARCOLOR = "_SpecularColor";
        public const string RIMCOLOR = "_RimColor";
        public const string RIMAMOUNT = "_RimAmount";
        public const string RIMTRESHOLD = "RimTreshold";
        public const string BASEMAP = "_BaseMap";
        public MaterialParam<Color> parameter__BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
        public MaterialParam<Color> parameter__AmbientColor = new MaterialParam<Color>(AMBIENTCOLOR, Color.white);
        public MaterialParam<float> parameter__Glossiness = new MaterialParam<float>(GLOSSINESS, 1.0f);
        public MaterialParam<Color> parameter__SpecularColor = new MaterialParam<Color>(SPECULARCOLOR, Color.white);
        public MaterialParam<Color> parameter__RimColor = new MaterialParam<Color>(RIMCOLOR, Color.white);
        public MaterialParam<float> parameter__RimAmount = new MaterialParam<float>(RIMAMOUNT, 1.0f);
        public MaterialParam<float> parameter_RimTreshold = new MaterialParam<float>(RIMTRESHOLD, 1.0f);
        public MaterialTextureParam parameter__BaseMap = new MaterialTextureParam(BASEMAP);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            parameter__BaseColor.Value = material.GetColor(parameter__BaseColor.ParamName);
            parameter__AmbientColor.Value = material.GetColor(parameter__AmbientColor.ParamName);
            parameter__Glossiness.Value = material.GetFloat(parameter__Glossiness.ParamName);
            parameter__SpecularColor.Value = material.GetColor(parameter__SpecularColor.ParamName);
            parameter__RimColor.Value = material.GetColor(parameter__RimColor.ParamName);
            parameter__RimAmount.Value = material.GetFloat(parameter__RimAmount.ParamName);
            parameter_RimTreshold.Value = material.GetFloat(parameter_RimTreshold.ParamName);
            var parameter__basemap_temp = material.GetTexture(parameter__BaseMap.ParamName);
            if (parameter__basemap_temp != null) parameter__BaseMap.Value = exportTextureInfo(parameter__basemap_temp);
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
                        case BVA_Material_ZeldaToon_Extra.BASECOLOR:
                            matCache.SetColor(BVA_Material_ZeldaToon_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_ZeldaToon_Extra.AMBIENTCOLOR:
                            matCache.SetColor(BVA_Material_ZeldaToon_Extra.AMBIENTCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_ZeldaToon_Extra.GLOSSINESS:
                            matCache.SetFloat(BVA_Material_ZeldaToon_Extra.GLOSSINESS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ZeldaToon_Extra.SPECULARCOLOR:
                            matCache.SetColor(BVA_Material_ZeldaToon_Extra.SPECULARCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_ZeldaToon_Extra.RIMCOLOR:
                            matCache.SetColor(BVA_Material_ZeldaToon_Extra.RIMCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_ZeldaToon_Extra.RIMAMOUNT:
                            matCache.SetFloat(BVA_Material_ZeldaToon_Extra.RIMAMOUNT, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ZeldaToon_Extra.RIMTRESHOLD:
                            matCache.SetFloat(BVA_Material_ZeldaToon_Extra.RIMTRESHOLD, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ZeldaToon_Extra.BASEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ZeldaToon_Extra.BASEMAP, tex);
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
            jo.Add(parameter__BaseColor.ParamName, parameter__BaseColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__AmbientColor.ParamName, parameter__AmbientColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__Glossiness.ParamName, parameter__Glossiness.Value);
            jo.Add(parameter__SpecularColor.ParamName, parameter__SpecularColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__RimColor.ParamName, parameter__RimColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__RimAmount.ParamName, parameter__RimAmount.Value);
            jo.Add(parameter_RimTreshold.ParamName, parameter_RimTreshold.Value);
            if (parameter__BaseMap != null && parameter__BaseMap.Value != null) jo.Add(parameter__BaseMap.ParamName, parameter__BaseMap.Serialize());
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_ZeldaToon_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_ZeldaToon_Extra();
        }
    }
}
