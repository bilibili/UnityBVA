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
public class BVA_Rigidbody_Extra :  IComponentExtra
{
public UnityEngine.Vector3 velocity;
public UnityEngine.Vector3 angularVelocity;
public float drag;
public float angularDrag;
public float mass;
public bool useGravity;
public float maxDepenetrationVelocity;
public bool isKinematic;
public bool freezeRotation;
public UnityEngine.RigidbodyConstraints constraints;
public UnityEngine.CollisionDetectionMode collisionDetectionMode;
public UnityEngine.Vector3 centerOfMass;
public UnityEngine.Quaternion inertiaTensorRotation;
public UnityEngine.Vector3 inertiaTensor;
public bool detectCollisions;
public UnityEngine.Vector3 position;
public UnityEngine.Quaternion rotation;
public UnityEngine.RigidbodyInterpolation interpolation;
public int solverIterations;
public float sleepThreshold;
public float maxAngularVelocity;
public int solverVelocityIterations;
public string ComponentName => ComponentType.Name;
public string ExtraName => GetType().Name;
public System.Type ComponentType => typeof(Rigidbody);
public void SetData(Component component)
{
var target = component as Rigidbody;
this.velocity = target.velocity;
this.angularVelocity = target.angularVelocity;
this.drag = target.drag;
this.angularDrag = target.angularDrag;
this.mass = target.mass;
this.useGravity = target.useGravity;
this.maxDepenetrationVelocity = target.maxDepenetrationVelocity;
this.isKinematic = target.isKinematic;
this.freezeRotation = target.freezeRotation;
this.constraints = target.constraints;
this.collisionDetectionMode = target.collisionDetectionMode;
this.centerOfMass = target.centerOfMass;
this.inertiaTensorRotation = target.inertiaTensorRotation;
this.inertiaTensor = target.inertiaTensor;
this.detectCollisions = target.detectCollisions;
this.position = target.position;
this.rotation = target.rotation;
this.interpolation = target.interpolation;
this.solverIterations = target.solverIterations;
this.sleepThreshold = target.sleepThreshold;
this.maxAngularVelocity = target.maxAngularVelocity;
this.solverVelocityIterations = target.solverVelocityIterations;
}
public void Deserialize(GLTFRoot root, JsonReader reader, Component component)
{
var target = component as Rigidbody;
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(target.velocity):
target.velocity =  reader.ReadAsVector3();
break;
case nameof(target.angularVelocity):
target.angularVelocity =  reader.ReadAsVector3();
break;
case nameof(target.drag):
target.drag =  reader.ReadAsFloat();
break;
case nameof(target.angularDrag):
target.angularDrag =  reader.ReadAsFloat();
break;
case nameof(target.mass):
target.mass =  reader.ReadAsFloat();
break;
case nameof(target.useGravity):
target.useGravity =  reader.ReadAsBoolean().Value;
break;
case nameof(target.maxDepenetrationVelocity):
target.maxDepenetrationVelocity =  reader.ReadAsFloat();
break;
case nameof(target.isKinematic):
target.isKinematic =  reader.ReadAsBoolean().Value;
break;
case nameof(target.freezeRotation):
target.freezeRotation =  reader.ReadAsBoolean().Value;
break;
case nameof(target.constraints):
target.constraints = reader.ReadStringEnum<UnityEngine.RigidbodyConstraints>();
break;
case nameof(target.collisionDetectionMode):
target.collisionDetectionMode = reader.ReadStringEnum<UnityEngine.CollisionDetectionMode>();
break;
case nameof(target.centerOfMass):
target.centerOfMass =  reader.ReadAsVector3();
break;
case nameof(target.inertiaTensorRotation):
target.inertiaTensorRotation =  reader.ReadAsQuaternion();
break;
case nameof(target.inertiaTensor):
target.inertiaTensor =  reader.ReadAsVector3();
break;
case nameof(target.detectCollisions):
target.detectCollisions =  reader.ReadAsBoolean().Value;
break;
case nameof(target.position):
target.position =  reader.ReadAsVector3();
break;
case nameof(target.rotation):
target.rotation =  reader.ReadAsQuaternion();
break;
case nameof(target.interpolation):
target.interpolation = reader.ReadStringEnum<UnityEngine.RigidbodyInterpolation>();
break;
case nameof(target.solverIterations):
target.solverIterations =  reader.ReadAsInt32().Value;
break;
case nameof(target.sleepThreshold):
target.sleepThreshold =  reader.ReadAsFloat();
break;
case nameof(target.maxAngularVelocity):
target.maxAngularVelocity =  reader.ReadAsFloat();
break;
case nameof(target.solverVelocityIterations):
target.solverVelocityIterations =  reader.ReadAsInt32().Value;
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(velocity), velocity.ToJArray());
jo.Add(nameof(angularVelocity), angularVelocity.ToJArray());
jo.Add(nameof(drag), drag);
jo.Add(nameof(angularDrag), angularDrag);
jo.Add(nameof(mass), mass);
jo.Add(nameof(useGravity), useGravity);
jo.Add(nameof(maxDepenetrationVelocity), maxDepenetrationVelocity);
jo.Add(nameof(isKinematic), isKinematic);
jo.Add(nameof(freezeRotation), freezeRotation);
jo.Add(nameof(constraints), constraints.ToString());
jo.Add(nameof(collisionDetectionMode), collisionDetectionMode.ToString());
jo.Add(nameof(centerOfMass), centerOfMass.ToJArray());
jo.Add(nameof(inertiaTensorRotation), inertiaTensorRotation.ToJArray());
jo.Add(nameof(inertiaTensor), inertiaTensor.ToJArray());
jo.Add(nameof(detectCollisions), detectCollisions);
jo.Add(nameof(position), position.ToJArray());
jo.Add(nameof(rotation), rotation.ToJArray());
jo.Add(nameof(interpolation), interpolation.ToString());
jo.Add(nameof(solverIterations), solverIterations);
jo.Add(nameof(sleepThreshold), sleepThreshold);
jo.Add(nameof(maxAngularVelocity), maxAngularVelocity);
jo.Add(nameof(solverVelocityIterations), solverVelocityIterations);
return new JProperty(ComponentName, jo);
}
public object Clone()
{
return new BVA_Rigidbody_Extra();
}
}
}
