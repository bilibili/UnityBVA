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
public class BVA_CharacterController_Extra :  IComponentExtra
{
public float radius;
public float height;
public UnityEngine.Vector3 center;
public float slopeLimit;
public float stepOffset;
public float skinWidth;
public float minMoveDistance;
public bool detectCollisions;
public bool enableOverlapRecovery;
public string ComponentName => ComponentType.Name;
public string ExtraName => GetType().Name;
public System.Type ComponentType => typeof(CharacterController);
public void SetData(Component component)
{
var target = component as CharacterController;
this.radius = target.radius;
this.height = target.height;
this.center = target.center;
this.slopeLimit = target.slopeLimit;
this.stepOffset = target.stepOffset;
this.skinWidth = target.skinWidth;
this.minMoveDistance = target.minMoveDistance;
this.detectCollisions = target.detectCollisions;
this.enableOverlapRecovery = target.enableOverlapRecovery;
}
public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
{
var target = component as CharacterController;
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(target.radius):
target.radius =  reader.ReadAsFloat();
break;
case nameof(target.height):
target.height =  reader.ReadAsFloat();
break;
case nameof(target.center):
target.center =  reader.ReadAsVector3();
break;
case nameof(target.slopeLimit):
target.slopeLimit =  reader.ReadAsFloat();
break;
case nameof(target.stepOffset):
target.stepOffset =  reader.ReadAsFloat();
break;
case nameof(target.skinWidth):
target.skinWidth =  reader.ReadAsFloat();
break;
case nameof(target.minMoveDistance):
target.minMoveDistance =  reader.ReadAsFloat();
break;
case nameof(target.detectCollisions):
target.detectCollisions =  reader.ReadAsBoolean().Value;
break;
case nameof(target.enableOverlapRecovery):
target.enableOverlapRecovery =  reader.ReadAsBoolean().Value;
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(radius), radius);
jo.Add(nameof(height), height);
jo.Add(nameof(center), center.ToJArray());
jo.Add(nameof(slopeLimit), slopeLimit);
jo.Add(nameof(stepOffset), stepOffset);
jo.Add(nameof(skinWidth), skinWidth);
jo.Add(nameof(minMoveDistance), minMoveDistance);
jo.Add(nameof(detectCollisions), detectCollisions);
jo.Add(nameof(enableOverlapRecovery), enableOverlapRecovery);
return new JProperty(ComponentName, jo);
}
public object Clone()
{
return new BVA_CharacterController_Extra();
}
}
}
