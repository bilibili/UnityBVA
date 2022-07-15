using Newtonsoft.Json.Linq;
using BVA.Component;
using Newtonsoft.Json;
using GLTF.Extensions;

namespace GLTF.Schema.BVA
{
public class BVA_MirrorPlane_Extra : IExtra
{
public const string PROPERTY = "BVA_MirrorPlane_Extra";
public int TextureResolution;
public float HeightOffset;
public System.String ReflectionTextureName;
public BVA_MirrorPlane_Extra(){}

public BVA_MirrorPlane_Extra(MirrorPlane target){
this.TextureResolution = target.TextureResolution;
this.HeightOffset = target.HeightOffset;
this.ReflectionTextureName = target.ReflectionTextureName;
}
public static void Deserialize(GLTFRoot root, JsonReader reader, MirrorPlane  target)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(BVA_MirrorPlane_Extra.TextureResolution):
target.TextureResolution= reader.ReadAsInt32().Value;
break;
case nameof(BVA_MirrorPlane_Extra.HeightOffset):
target.HeightOffset= reader.ReadAsFloat();
break;
case nameof(BVA_MirrorPlane_Extra.ReflectionTextureName):
target.ReflectionTextureName= reader.ReadAsString();
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(TextureResolution), TextureResolution);
jo.Add(nameof(HeightOffset), HeightOffset);
jo.Add(nameof(ReflectionTextureName), ReflectionTextureName);
return new JProperty(BVA_MirrorPlane_Extra.PROPERTY, jo);
}
}
}
