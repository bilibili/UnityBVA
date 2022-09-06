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
    public class BVA_Material_ClothLED_Extra : IMaterialExtra
    {
        public const string PROPERTY = "BVA_Material_ClothLED_Extra";
        public const string SHADER_NAME = "Shader Graphs/UnityChanSSU/ClothLED";
        public const string MAINTEX = "_MainTex";
        public const string BASECOLOR = "_BaseColor";
        public const string USEMAINTEXFIRST = "_UseMainTex_1st";
        public const string FIRSTSHADETEX = "_1stShadeTex";
        public const string FIRSTSHADE = "_1stShade";
        public const string FIRSTSHADETHRED = "_1stShadeThred";
        public const string FIRSTSHADESPRED = "_1stShadeSpred";
        public const string USEMAINTEXSECOND = "_UseMainTex_2nd";
        public const string SECONDSHADETEX = "_2ndShadeTex";
        public const string SECONDSHADE = "_2ndShade";
        public const string SECONDSHADETHRED = "_2ndShadeThred";
        public const string SECONDSHADESPRED = "_2ndShadeSpred";
        public const string PATTERNA = "_PatternA";
        public const string PATTERNB = "_PatternB";
        public const string PATTERNC = "_PatternC";
        public const string PATTERND = "_PatternD";
        public const string LEDCOLOR = "_LED_Color";
        public const string SONARA = "_SonarA";
        public const string SONARB = "_SonarB";
        public const string LEDPOWER = "_LED_Power";
        public const string SAMPLETEXTURE2D599099BF4BCA708590264DEFF4B696DBTEXTURE1 = "_SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1";
        public const string SAMPLETEXTURE2D524A088209439C85B55EBCCAF6795670TEXTURE1 = "_SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1";
        public MaterialTextureParam parameter__MainTex = new MaterialTextureParam(MAINTEX);
        public MaterialParam<Color> parameter__BaseColor = new MaterialParam<Color>(BASECOLOR, Color.white);
        public MaterialParam<float> parameter__UseMainTex_1st = new MaterialParam<float>(USEMAINTEXFIRST, 1.0f);
        public MaterialTextureParam parameter__1stShadeTex = new MaterialTextureParam(FIRSTSHADETEX);
        public MaterialParam<Color> parameter__1stShade = new MaterialParam<Color>(FIRSTSHADE, Color.white);
        public MaterialParam<float> parameter__1stShadeThred = new MaterialParam<float>(FIRSTSHADETHRED, 1.0f);
        public MaterialParam<float> parameter__1stShadeSpred = new MaterialParam<float>(FIRSTSHADESPRED, 1.0f);
        public MaterialParam<float> parameter__UseMainTex_2nd = new MaterialParam<float>(USEMAINTEXSECOND, 1.0f);
        public MaterialTextureParam parameter__2ndShadeTex = new MaterialTextureParam(SECONDSHADETEX);
        public MaterialParam<Color> parameter__2ndShade = new MaterialParam<Color>(SECONDSHADE, Color.white);
        public MaterialParam<float> parameter__2ndShadeThred = new MaterialParam<float>(SECONDSHADETHRED, 1.0f);
        public MaterialParam<float> parameter__2ndShadeSpred = new MaterialParam<float>(SECONDSHADESPRED, 1.0f);
        public MaterialTextureParam parameter__PatternA = new MaterialTextureParam(PATTERNA);
        public MaterialTextureParam parameter__PatternB = new MaterialTextureParam(PATTERNB);
        public MaterialTextureParam parameter__PatternC = new MaterialTextureParam(PATTERNC);
        public MaterialTextureParam parameter__PatternD = new MaterialTextureParam(PATTERND);
        public MaterialTextureParam parameter__LED_Color = new MaterialTextureParam(LEDCOLOR);
        public MaterialParam<float> parameter__SonarA = new MaterialParam<float>(SONARA, 1.0f);
        public MaterialParam<float> parameter__SonarB = new MaterialParam<float>(SONARB, 1.0f);
        public MaterialParam<float> parameter__LED_Power = new MaterialParam<float>(LEDPOWER, 1.0f);
        public MaterialTextureParam parameter__SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1 = new MaterialTextureParam(SAMPLETEXTURE2D599099BF4BCA708590264DEFF4B696DBTEXTURE1);
        public MaterialTextureParam parameter__SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1 = new MaterialTextureParam(SAMPLETEXTURE2D524A088209439C85B55EBCCAF6795670TEXTURE1);
        public string[] keywords;
        public string ShaderName => SHADER_NAME;
        public string ExtraName => GetType().Name;
        public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
        {
            keywords = material.shaderKeywords;
            var parameter__maintex_temp = material.GetTexture(parameter__MainTex.ParamName);
            if (parameter__maintex_temp != null) parameter__MainTex.Value = exportTextureInfo(parameter__maintex_temp);
            parameter__BaseColor.Value = material.GetColor(parameter__BaseColor.ParamName);
            parameter__UseMainTex_1st.Value = material.GetFloat(parameter__UseMainTex_1st.ParamName);
            var parameter__1stshadetex_temp = material.GetTexture(parameter__1stShadeTex.ParamName);
            if (parameter__1stshadetex_temp != null) parameter__1stShadeTex.Value = exportTextureInfo(parameter__1stshadetex_temp);
            parameter__1stShade.Value = material.GetColor(parameter__1stShade.ParamName);
            parameter__1stShadeThred.Value = material.GetFloat(parameter__1stShadeThred.ParamName);
            parameter__1stShadeSpred.Value = material.GetFloat(parameter__1stShadeSpred.ParamName);
            parameter__UseMainTex_2nd.Value = material.GetFloat(parameter__UseMainTex_2nd.ParamName);
            var parameter__2ndshadetex_temp = material.GetTexture(parameter__2ndShadeTex.ParamName);
            if (parameter__2ndshadetex_temp != null) parameter__2ndShadeTex.Value = exportTextureInfo(parameter__2ndshadetex_temp);
            parameter__2ndShade.Value = material.GetColor(parameter__2ndShade.ParamName);
            parameter__2ndShadeThred.Value = material.GetFloat(parameter__2ndShadeThred.ParamName);
            parameter__2ndShadeSpred.Value = material.GetFloat(parameter__2ndShadeSpred.ParamName);
            var parameter__patterna_temp = material.GetTexture(parameter__PatternA.ParamName);
            if (parameter__patterna_temp != null) parameter__PatternA.Value = exportTextureInfo(parameter__patterna_temp);
            var parameter__patternb_temp = material.GetTexture(parameter__PatternB.ParamName);
            if (parameter__patternb_temp != null) parameter__PatternB.Value = exportTextureInfo(parameter__patternb_temp);
            var parameter__patternc_temp = material.GetTexture(parameter__PatternC.ParamName);
            if (parameter__patternc_temp != null) parameter__PatternC.Value = exportTextureInfo(parameter__patternc_temp);
            var parameter__patternd_temp = material.GetTexture(parameter__PatternD.ParamName);
            if (parameter__patternd_temp != null) parameter__PatternD.Value = exportTextureInfo(parameter__patternd_temp);
            var parameter__led_color_temp = material.GetTexture(parameter__LED_Color.ParamName);
            if (parameter__led_color_temp != null) parameter__LED_Color.Value = exportTextureInfo(parameter__led_color_temp);
            parameter__SonarA.Value = material.GetFloat(parameter__SonarA.ParamName);
            parameter__SonarB.Value = material.GetFloat(parameter__SonarB.ParamName);
            parameter__LED_Power.Value = material.GetFloat(parameter__LED_Power.ParamName);
            var parameter__sampletexture2d_599099bf4bca708590264deff4b696db_texture_1_temp = material.GetTexture(parameter__SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1.ParamName);
            if (parameter__sampletexture2d_599099bf4bca708590264deff4b696db_texture_1_temp != null) parameter__SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1.Value = exportTextureInfo(parameter__sampletexture2d_599099bf4bca708590264deff4b696db_texture_1_temp);
            var parameter__sampletexture2d_524a088209439c85b55ebccaf6795670_texture_1_temp = material.GetTexture(parameter__SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1.ParamName);
            if (parameter__sampletexture2d_524a088209439c85b55ebccaf6795670_texture_1_temp != null) parameter__SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1.Value = exportTextureInfo(parameter__sampletexture2d_524a088209439c85b55ebccaf6795670_texture_1_temp);
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
                        case BVA_Material_ClothLED_Extra.MAINTEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.MAINTEX, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.BASECOLOR:
                            matCache.SetColor(BVA_Material_ClothLED_Extra.BASECOLOR, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ClothLED_Extra.USEMAINTEXFIRST:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.USEMAINTEXFIRST, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.FIRSTSHADETEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.FIRSTSHADETEX, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.FIRSTSHADE:
                            matCache.SetColor(BVA_Material_ClothLED_Extra.FIRSTSHADE, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ClothLED_Extra.FIRSTSHADETHRED:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.FIRSTSHADETHRED, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.FIRSTSHADESPRED:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.FIRSTSHADESPRED, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.USEMAINTEXSECOND:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.USEMAINTEXSECOND, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.SECONDSHADETEX:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.SECONDSHADETEX, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.SECONDSHADE:
                            matCache.SetColor(BVA_Material_ClothLED_Extra.SECONDSHADE, reader.ReadAsRGBAColor());
                            break;
                        case BVA_Material_ClothLED_Extra.SECONDSHADETHRED:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.SECONDSHADETHRED, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.SECONDSHADESPRED:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.SECONDSHADESPRED, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.PATTERNA:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.PATTERNA, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.PATTERNB:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.PATTERNB, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.PATTERNC:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.PATTERNC, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.PATTERND:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.PATTERND, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.LEDCOLOR:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.LEDCOLOR, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.SONARA:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.SONARA, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.SONARB:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.SONARB, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.LEDPOWER:
                            matCache.SetFloat(BVA_Material_ClothLED_Extra.LEDPOWER, reader.ReadAsFloat());
                            break;
                        case BVA_Material_ClothLED_Extra.SAMPLETEXTURE2D599099BF4BCA708590264DEFF4B696DBTEXTURE1:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.SAMPLETEXTURE2D599099BF4BCA708590264DEFF4B696DBTEXTURE1, tex);
                            }
                            break;
                        case BVA_Material_ClothLED_Extra.SAMPLETEXTURE2D524A088209439C85B55EBCCAF6795670TEXTURE1:
                            {
                                var texInfo = TextureInfo.Deserialize(root, reader);
                                var tex = await loadTexture(texInfo.Index);
                                matCache.SetTexture(BVA_Material_ClothLED_Extra.SAMPLETEXTURE2D524A088209439C85B55EBCCAF6795670TEXTURE1, tex);
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
            if (parameter__MainTex != null && parameter__MainTex.Value != null) jo.Add(parameter__MainTex.ParamName, parameter__MainTex.Serialize());
            jo.Add(parameter__BaseColor.ParamName, parameter__BaseColor.Value.ToJArray());
            jo.Add(parameter__UseMainTex_1st.ParamName, parameter__UseMainTex_1st.Value);
            if (parameter__1stShadeTex != null && parameter__1stShadeTex.Value != null) jo.Add(parameter__1stShadeTex.ParamName, parameter__1stShadeTex.Serialize());
            jo.Add(parameter__1stShade.ParamName, parameter__1stShade.Value.ToJArray());
            jo.Add(parameter__1stShadeThred.ParamName, parameter__1stShadeThred.Value);
            jo.Add(parameter__1stShadeSpred.ParamName, parameter__1stShadeSpred.Value);
            jo.Add(parameter__UseMainTex_2nd.ParamName, parameter__UseMainTex_2nd.Value);
            if (parameter__2ndShadeTex != null && parameter__2ndShadeTex.Value != null) jo.Add(parameter__2ndShadeTex.ParamName, parameter__2ndShadeTex.Serialize());
            jo.Add(parameter__2ndShade.ParamName, parameter__2ndShade.Value.ToJArray());
            jo.Add(parameter__2ndShadeThred.ParamName, parameter__2ndShadeThred.Value);
            jo.Add(parameter__2ndShadeSpred.ParamName, parameter__2ndShadeSpred.Value);
            if (parameter__PatternA != null && parameter__PatternA.Value != null) jo.Add(parameter__PatternA.ParamName, parameter__PatternA.Serialize());
            if (parameter__PatternB != null && parameter__PatternB.Value != null) jo.Add(parameter__PatternB.ParamName, parameter__PatternB.Serialize());
            if (parameter__PatternC != null && parameter__PatternC.Value != null) jo.Add(parameter__PatternC.ParamName, parameter__PatternC.Serialize());
            if (parameter__PatternD != null && parameter__PatternD.Value != null) jo.Add(parameter__PatternD.ParamName, parameter__PatternD.Serialize());
            if (parameter__LED_Color != null && parameter__LED_Color.Value != null) jo.Add(parameter__LED_Color.ParamName, parameter__LED_Color.Serialize());
            jo.Add(parameter__SonarA.ParamName, parameter__SonarA.Value);
            jo.Add(parameter__SonarB.ParamName, parameter__SonarB.Value);
            jo.Add(parameter__LED_Power.ParamName, parameter__LED_Power.Value);
            if (parameter__SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1 != null && parameter__SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1.Value != null) jo.Add(parameter__SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1.ParamName, parameter__SampleTexture2D_599099bf4bca708590264deff4b696db_Texture_1.Serialize());
            if (parameter__SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1 != null && parameter__SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1.Value != null) jo.Add(parameter__SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1.ParamName, parameter__SampleTexture2D_524a088209439c85b55ebccaf6795670_Texture_1.Serialize());
            if (keywords != null && keywords.Length > 0)
            {
                JArray jKeywords = new JArray();
                foreach (var keyword in jKeywords)
                    jKeywords.Add(keyword);
                jo.Add(nameof(keywords), jKeywords);
            }
            return new JProperty(BVA_Material_ClothLED_Extra.SHADER_NAME, jo);
        }
        public object Clone()
        {
            return new BVA_Material_ClothLED_Extra();
        }
    }
}
