using Newtonsoft.Json.Linq;
using GLTF.Math;
using GLTF.Schema;
using Newtonsoft.Json;
using BVA.Component;

namespace GLTF.Schema.BVA
{
public class BVA_UI_InfoText_Extra : IExtra
{
public const string PROPERTY = "BVA_UI_InfoText_Extra";
public string info;
public BVA_UI_InfoText_Extra(){}

public BVA_UI_InfoText_Extra(InfoText target){
this.info = target.info;
}
public static void Deserialize(GLTFRoot root, JsonReader reader, InfoText  target)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(target.info):
target.info =  reader.ReadAsString();
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(info), info);
return new JProperty(BVA_UI_InfoText_Extra.PROPERTY, jo);
}
}
}
