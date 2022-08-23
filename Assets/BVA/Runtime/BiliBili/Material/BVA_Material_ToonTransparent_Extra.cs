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
    public class BVA_Material_ToonTransparent_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_ToonTransparent_Extra";
        public const string SHADER_NAME = "Shader Graphs/Toon (Transparent)";
        public const string BASECOLOR = "_BaseColor";
        public const string BASEMAP = "_BaseMap";
        public const string SHADEENVIRONMENTALCOLOR = "_ShadeEnvironmentalColor";
        public const string SHADECOLOR = "_ShadeColor";
        public const string SHADEMAP = "_ShadeMap";
        public const string SMOOTHNESS = "_Smoothness";
        public const string METALIC = "_Metalic";
        public const string METALICMAP = "_MetalicMap";
        public const string NORMALMAP = "_NormalMap";
        public const string OCCLUSIONMAP = "_OcclusionMap";
        public const string OUTLINEWIDTH = "_OutlineWidth";
        public const string OUTLINEMAP = "_OutlineMap";
        public const string SHADETOONY = "_ShadeToony";
        public const string TOONYLIGHTING = "_ToonyLighting";
        public const string EMISSIONMAP = "_EmissionMap";
        public const string EMISSIONCOLOR = "_EmissionColor";
        public MaterialParam<Color> parameter__BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
        public MaterialTextureParam parameter__BaseMap = new MaterialTextureParam(BASEMAP);
        public MaterialParam<Color> parameter__ShadeEnvironmentalColor = new MaterialParam<Color>(SHADEENVIRONMENTALCOLOR, Color.white);
        public MaterialParam<Color> parameter__ShadeColor = new MaterialParam<Color>(SHADECOLOR, Color.white);
        public MaterialTextureParam parameter__ShadeMap = new MaterialTextureParam(SHADEMAP);
        public MaterialParam<float> parameter__Smoothness = new MaterialParam<float>(SMOOTHNESS, 1.0f);
        public MaterialParam<float> parameter__Metalic = new MaterialParam<float>(METALIC, 1.0f);
        public MaterialTextureParam parameter__MetalicMap = new MaterialTextureParam(METALICMAP);
        public MaterialTextureParam parameter__NormalMap = new MaterialTextureParam(NORMALMAP);
        public MaterialTextureParam parameter__OcclusionMap = new MaterialTextureParam(OCCLUSIONMAP);
        public MaterialParam<float> parameter__OutlineWidth = new MaterialParam<float>(OUTLINEWIDTH, 1.0f);
        public MaterialTextureParam parameter__OutlineMap = new MaterialTextureParam(OUTLINEMAP);
        public MaterialParam<float> parameter__ShadeToony = new MaterialParam<float>(SHADETOONY, 1.0f);
        public MaterialParam<float> parameter__ToonyLighting = new MaterialParam<float>(TOONYLIGHTING, 1.0f);
        public MaterialTextureParam parameter__EmissionMap = new MaterialTextureParam(EMISSIONMAP);
        public MaterialParam<Color> parameter__EmissionColor = new MaterialParam<Color>(EMISSIONCOLOR, Color.white);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            parameter__BaseColor.Value = material.GetColor(parameter__BaseColor.ParamName);
            var parameter__basemap_temp = material.GetTexture(parameter__BaseMap.ParamName);
            if (parameter__basemap_temp != null) parameter__BaseMap.Value = exportTextureInfo(parameter__basemap_temp);
            parameter__ShadeEnvironmentalColor.Value = material.GetColor(parameter__ShadeEnvironmentalColor.ParamName);
            parameter__ShadeColor.Value = material.GetColor(parameter__ShadeColor.ParamName);
            var parameter__shademap_temp = material.GetTexture(parameter__ShadeMap.ParamName);
            if (parameter__shademap_temp != null) parameter__ShadeMap.Value = exportTextureInfo(parameter__shademap_temp);
            parameter__Smoothness.Value = material.GetFloat(parameter__Smoothness.ParamName);
            parameter__Metalic.Value = material.GetFloat(parameter__Metalic.ParamName);
            var parameter__metalicmap_temp = material.GetTexture(parameter__MetalicMap.ParamName);
            if (parameter__metalicmap_temp != null) parameter__MetalicMap.Value = exportTextureInfo(parameter__metalicmap_temp);
            var parameter__normalmap_temp = material.GetTexture(parameter__NormalMap.ParamName);
            if (parameter__normalmap_temp != null) parameter__NormalMap.Value = exportNormalTextureInfo(parameter__normalmap_temp);
            var parameter__occlusionmap_temp = material.GetTexture(parameter__OcclusionMap.ParamName);
            if (parameter__occlusionmap_temp != null) parameter__OcclusionMap.Value = exportTextureInfo(parameter__occlusionmap_temp);
            parameter__OutlineWidth.Value = material.GetFloat(parameter__OutlineWidth.ParamName);
            var parameter__outlinemap_temp = material.GetTexture(parameter__OutlineMap.ParamName);
            if (parameter__outlinemap_temp != null) parameter__OutlineMap.Value = exportTextureInfo(parameter__outlinemap_temp);
            parameter__ShadeToony.Value = material.GetFloat(parameter__ShadeToony.ParamName);
            parameter__ToonyLighting.Value = material.GetFloat(parameter__ToonyLighting.ParamName);
            var parameter__emissionmap_temp = material.GetTexture(parameter__EmissionMap.ParamName);
            if (parameter__emissionmap_temp != null) parameter__EmissionMap.Value = exportTextureInfo(parameter__emissionmap_temp);
            parameter__EmissionColor.Value = material.GetColor(parameter__EmissionColor.ParamName);
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
                        case BVA_Material_ToonTransparent_Extra.BASECOLOR:
                            matCache.SetColor(BVA_Material_ToonTransparent_Extra.BASECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_ToonTransparent_Extra.BASEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonTransparent_Extra.BASEMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonTransparent_Extra.SHADEENVIRONMENTALCOLOR:
                            matCache.SetColor(BVA_Material_ToonTransparent_Extra.SHADEENVIRONMENTALCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_ToonTransparent_Extra.SHADECOLOR:
                            matCache.SetColor(BVA_Material_ToonTransparent_Extra.SHADECOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
                            break;
                        case BVA_Material_ToonTransparent_Extra.SHADEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonTransparent_Extra.SHADEMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonTransparent_Extra.SMOOTHNESS:
                            matCache.SetFloat(BVA_Material_ToonTransparent_Extra.SMOOTHNESS, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonTransparent_Extra.METALIC:
                            matCache.SetFloat(BVA_Material_ToonTransparent_Extra.METALIC, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonTransparent_Extra.METALICMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonTransparent_Extra.METALICMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonTransparent_Extra.NORMALMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonTransparent_Extra.NORMALMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonTransparent_Extra.OCCLUSIONMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonTransparent_Extra.OCCLUSIONMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonTransparent_Extra.OUTLINEWIDTH:
                            matCache.SetFloat(BVA_Material_ToonTransparent_Extra.OUTLINEWIDTH, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonTransparent_Extra.OUTLINEMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonTransparent_Extra.OUTLINEMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonTransparent_Extra.SHADETOONY:
                            matCache.SetFloat(BVA_Material_ToonTransparent_Extra.SHADETOONY, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonTransparent_Extra.TOONYLIGHTING:
                            matCache.SetFloat(BVA_Material_ToonTransparent_Extra.TOONYLIGHTING, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ToonTransparent_Extra.EMISSIONMAP:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ToonTransparent_Extra.EMISSIONMAP, tex);
                            }
                            break;
                        case BVA_Material_ToonTransparent_Extra.EMISSIONCOLOR:
                            matCache.SetColor(BVA_Material_ToonTransparent_Extra.EMISSIONCOLOR, reader.ReadAsRGBAColor().ToUnityColorRaw());
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
            if (parameter__BaseMap != null && parameter__BaseMap.Value != null) jo.Add(parameter__BaseMap.ParamName, parameter__BaseMap.Serialize());
            jo.Add(parameter__ShadeEnvironmentalColor.ParamName, parameter__ShadeEnvironmentalColor.Value.ToNumericsColorRaw().ToJArray());
            jo.Add(parameter__ShadeColor.ParamName, parameter__ShadeColor.Value.ToNumericsColorRaw().ToJArray());
            if (parameter__ShadeMap != null && parameter__ShadeMap.Value != null) jo.Add(parameter__ShadeMap.ParamName, parameter__ShadeMap.Serialize());
            jo.Add(parameter__Smoothness.ParamName, parameter__Smoothness.Value);
            jo.Add(parameter__Metalic.ParamName, parameter__Metalic.Value);
            if (parameter__MetalicMap != null && parameter__MetalicMap.Value != null) jo.Add(parameter__MetalicMap.ParamName, parameter__MetalicMap.Serialize());
            if (parameter__NormalMap != null && parameter__NormalMap.Value != null) jo.Add(parameter__NormalMap.ParamName, parameter__NormalMap.Serialize());
            if (parameter__OcclusionMap != null && parameter__OcclusionMap.Value != null) jo.Add(parameter__OcclusionMap.ParamName, parameter__OcclusionMap.Serialize());
            jo.Add(parameter__OutlineWidth.ParamName, parameter__OutlineWidth.Value);
            if (parameter__OutlineMap != null && parameter__OutlineMap.Value != null) jo.Add(parameter__OutlineMap.ParamName, parameter__OutlineMap.Serialize());
            jo.Add(parameter__ShadeToony.ParamName, parameter__ShadeToony.Value);
            jo.Add(parameter__ToonyLighting.ParamName, parameter__ToonyLighting.Value);
            if (parameter__EmissionMap != null && parameter__EmissionMap.Value != null) jo.Add(parameter__EmissionMap.ParamName, parameter__EmissionMap.Serialize());
            jo.Add(parameter__EmissionColor.ParamName, parameter__EmissionColor.Value.ToNumericsColorRaw().ToJArray());
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_ToonTransparent_Extra.SHADER_NAME, jo);
        }

        public object Clone()
        {
            return new BVA_Material_ToonTransparent_Extra();
        }
    }
}
