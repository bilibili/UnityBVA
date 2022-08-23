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
public class BVA_Material_UnlitTransparent_Extra : IMaterialExtra
{
public const string PROPERTY = "BVA_Material_UnlitTransparent_Extra";
public const string SHADER_NAME = "VRM/UnlitTransparent";
public const string MAINTEX = "_MainTex";
public MaterialTextureParam parameter__MainTex = new MaterialTextureParam(MAINTEX);
public string[] keywords;
public string ShaderName => SHADER_NAME;
public string ExtraName => GetType().Name;
public void SetData(Material material, ExportTextureInfo exportTextureInfo, ExportTextureInfo exportNormalTextureInfo, ExportCubemap exportCubemapInfo)
{
keywords = material.shaderKeywords;
var parameter__maintex_temp = material.GetTexture(parameter__MainTex.ParamName);
if (parameter__maintex_temp != null) parameter__MainTex.Value = exportTextureInfo(parameter__maintex_temp);
}
public async Task Deserialize(GLTFRoot root, JsonReader reader, Material matCache,AsyncLoadTexture loadTexture, AsyncLoadTexture loadNormalMap, AsyncLoadCubemap loadCubemap)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case BVA_Material_UnlitTransparent_Extra.MAINTEX:
{
var texInfo = TextureInfo.Deserialize(root, reader);
var tex = await loadTexture(texInfo.Index);
matCache.SetTexture(BVA_Material_UnlitTransparent_Extra.MAINTEX, tex);
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
if(keywords != null && keywords.Length > 0)
{
JArray jKeywords = new JArray();
foreach (var keyword in jKeywords)
jKeywords.Add(keyword);
jo.Add(nameof(keywords), jKeywords);
}
return new JProperty(BVA_Material_UnlitTransparent_Extra.SHADER_NAME, jo);
}

        public object Clone()
        {
            return new BVA_Material_UnlitTransparent_Extra();
        }
    }
}
