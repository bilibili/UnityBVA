using Newtonsoft.Json.Linq;
using GLTF.Math;
using GLTF.Schema;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;

namespace GLTF.Schema.BVA
{
public class BVA_WindZone_Extra : IExtra
{
public const string PROPERTY = "BVA_WindZone_Extra";
public UnityEngine.WindZoneMode mode;
public float radius;
public float windMain;
public float windTurbulence;
public float windPulseMagnitude;
public float windPulseFrequency;
public BVA_WindZone_Extra(){}

public BVA_WindZone_Extra(UnityEngine.WindZone target){
this.mode = target.mode;
this.radius = target.radius;
this.windMain = target.windMain;
this.windTurbulence = target.windTurbulence;
this.windPulseMagnitude = target.windPulseMagnitude;
this.windPulseFrequency = target.windPulseFrequency;
}
public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.WindZone  target)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(BVA_WindZone_Extra.mode):
target.mode = reader.ReadStringEnum<UnityEngine.WindZoneMode>();
break;
case nameof(BVA_WindZone_Extra.radius):
target.radius= reader.ReadAsFloat();
break;
case nameof(BVA_WindZone_Extra.windMain):
target.windMain= reader.ReadAsFloat();
break;
case nameof(BVA_WindZone_Extra.windTurbulence):
target.windTurbulence= reader.ReadAsFloat();
break;
case nameof(BVA_WindZone_Extra.windPulseMagnitude):
target.windPulseMagnitude= reader.ReadAsFloat();
break;
case nameof(BVA_WindZone_Extra.windPulseFrequency):
target.windPulseFrequency= reader.ReadAsFloat();
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(mode), mode.ToString());
jo.Add(nameof(radius), radius);
jo.Add(nameof(windMain), windMain);
jo.Add(nameof(windTurbulence), windTurbulence);
jo.Add(nameof(windPulseMagnitude), windPulseMagnitude);
jo.Add(nameof(windPulseFrequency), windPulseFrequency);
return new JProperty(BVA_WindZone_Extra.PROPERTY, jo);
}
}
}
