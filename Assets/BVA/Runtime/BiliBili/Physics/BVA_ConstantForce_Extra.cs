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
public class BVA_ConstantForce_Extra :  IComponentExtra
{
public UnityEngine.Vector3 force;
public UnityEngine.Vector3 relativeForce;
public UnityEngine.Vector3 torque;
public UnityEngine.Vector3 relativeTorque;
public string ComponentName => ComponentType.Name;
public string ExtraName => GetType().Name;
public System.Type ComponentType => typeof(ConstantForce);
public void SetData(Component component)
{
var target = component as ConstantForce;
this.force = target.force;
this.relativeForce = target.relativeForce;
this.torque = target.torque;
this.relativeTorque = target.relativeTorque;
}
public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
{
var target = component as ConstantForce;
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(target.force):
target.force =  reader.ReadAsVector3();
break;
case nameof(target.relativeForce):
target.relativeForce =  reader.ReadAsVector3();
break;
case nameof(target.torque):
target.torque =  reader.ReadAsVector3();
break;
case nameof(target.relativeTorque):
target.relativeTorque =  reader.ReadAsVector3();
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(force), force.ToJArray());
jo.Add(nameof(relativeForce), relativeForce.ToJArray());
jo.Add(nameof(torque), torque.ToJArray());
jo.Add(nameof(relativeTorque), relativeTorque.ToJArray());
return new JProperty(ComponentName, jo);
}
public object Clone()
{
return new BVA_ConstantForce_Extra();
}
}
}
