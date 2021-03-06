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
public class BVA_ConstantForce_Extra : IExtra
{
public const string PROPERTY = "BVA_ConstantForce_Extra";
public UnityEngine.Vector3 force;
public UnityEngine.Vector3 relativeForce;
public UnityEngine.Vector3 torque;
public UnityEngine.Vector3 relativeTorque;
public BVA_ConstantForce_Extra(){}

public BVA_ConstantForce_Extra(UnityEngine.ConstantForce target){
this.force = target.force;
this.relativeForce = target.relativeForce;
this.torque = target.torque;
this.relativeTorque = target.relativeTorque;
}
public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.ConstantForce  target)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(BVA_ConstantForce_Extra.force):
target.force= reader.ReadAsVector3().ToUnityVector3Raw();
break;
case nameof(BVA_ConstantForce_Extra.relativeForce):
target.relativeForce= reader.ReadAsVector3().ToUnityVector3Raw();
break;
case nameof(BVA_ConstantForce_Extra.torque):
target.torque= reader.ReadAsVector3().ToUnityVector3Raw();
break;
case nameof(BVA_ConstantForce_Extra.relativeTorque):
target.relativeTorque= reader.ReadAsVector3().ToUnityVector3Raw();
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(force), force.ToGltfVector3Raw().ToJArray());
jo.Add(nameof(relativeForce), relativeForce.ToGltfVector3Raw().ToJArray());
jo.Add(nameof(torque), torque.ToGltfVector3Raw().ToJArray());
jo.Add(nameof(relativeTorque), relativeTorque.ToGltfVector3Raw().ToJArray());
return new JProperty(BVA_ConstantForce_Extra.PROPERTY, jo);
}
}
}
