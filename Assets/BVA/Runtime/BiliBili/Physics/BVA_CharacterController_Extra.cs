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
public class BVA_CharacterController_Extra : IExtra
{
public const string PROPERTY = "BVA_CharacterController_Extra";
public float radius;
public float height;
public UnityEngine.Vector3 center;
public float slopeLimit;
public float stepOffset;
public float skinWidth;
public float minMoveDistance;
public bool detectCollisions;
public bool enableOverlapRecovery;
public BVA_CharacterController_Extra(){}

public BVA_CharacterController_Extra(UnityEngine.CharacterController target){
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
public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.CharacterController  target)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(BVA_CharacterController_Extra.radius):
target.radius= reader.ReadAsFloat();
break;
case nameof(BVA_CharacterController_Extra.height):
target.height= reader.ReadAsFloat();
break;
case nameof(BVA_CharacterController_Extra.center):
target.center= reader.ReadAsVector3().ToUnityVector3Raw();
break;
case nameof(BVA_CharacterController_Extra.slopeLimit):
target.slopeLimit= reader.ReadAsFloat();
break;
case nameof(BVA_CharacterController_Extra.stepOffset):
target.stepOffset= reader.ReadAsFloat();
break;
case nameof(BVA_CharacterController_Extra.skinWidth):
target.skinWidth= reader.ReadAsFloat();
break;
case nameof(BVA_CharacterController_Extra.minMoveDistance):
target.minMoveDistance= reader.ReadAsFloat();
break;
case nameof(BVA_CharacterController_Extra.detectCollisions):
target.detectCollisions= reader.ReadAsBoolean().Value;
break;
case nameof(BVA_CharacterController_Extra.enableOverlapRecovery):
target.enableOverlapRecovery= reader.ReadAsBoolean().Value;
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
jo.Add(nameof(center), center.ToGltfVector3Raw().ToJArray());
jo.Add(nameof(slopeLimit), slopeLimit);
jo.Add(nameof(stepOffset), stepOffset);
jo.Add(nameof(skinWidth), skinWidth);
jo.Add(nameof(minMoveDistance), minMoveDistance);
jo.Add(nameof(detectCollisions), detectCollisions);
jo.Add(nameof(enableOverlapRecovery), enableOverlapRecovery);
return new JProperty(BVA_CharacterController_Extra.PROPERTY, jo);
}
}
}
