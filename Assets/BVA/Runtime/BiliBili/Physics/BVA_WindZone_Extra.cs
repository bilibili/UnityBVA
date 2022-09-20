using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;
using BVA.Component;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector4 = UnityEngine.Vector4;

namespace GLTF.Schema.BVA
{
[ComponentExtra]
public class BVA_WindZone_Extra :  IComponentExtra
{
public UnityEngine.WindZoneMode mode;
public float radius;
public float windMain;
public float windTurbulence;
public float windPulseMagnitude;
public float windPulseFrequency;
public string ComponentName => ComponentType.Name;
public string ExtraName => GetType().Name;
public System.Type ComponentType => typeof(WindZone);
public void SetData(Component component)
{
var target = component as WindZone;
this.mode = target.mode;
this.radius = target.radius;
this.windMain = target.windMain;
this.windTurbulence = target.windTurbulence;
this.windPulseMagnitude = target.windPulseMagnitude;
this.windPulseFrequency = target.windPulseFrequency;
}
public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
{
var target = component as WindZone;
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(target.mode):
target.mode = reader.ReadStringEnum<UnityEngine.WindZoneMode>();
break;
case nameof(target.radius):
target.radius =  reader.ReadAsFloat();
break;
case nameof(target.windMain):
target.windMain =  reader.ReadAsFloat();
break;
case nameof(target.windTurbulence):
target.windTurbulence =  reader.ReadAsFloat();
break;
case nameof(target.windPulseMagnitude):
target.windPulseMagnitude =  reader.ReadAsFloat();
break;
case nameof(target.windPulseFrequency):
target.windPulseFrequency =  reader.ReadAsFloat();
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
return new JProperty(ComponentName, jo);
}
public object Clone()
{
return new BVA_WindZone_Extra();
}
}
}
