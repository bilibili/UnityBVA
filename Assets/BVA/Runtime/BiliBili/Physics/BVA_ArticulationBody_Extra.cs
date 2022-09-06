using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;

namespace GLTF.Schema.BVA
{
	public class BVA_ArticulationBody_Extra : IExtra
{
public const string PROPERTY = "BVA_ArticulationBody_Extra";
public UnityEngine.ArticulationJointType jointType;
public UnityEngine.Vector3 anchorPosition;
public UnityEngine.Vector3 parentAnchorPosition;
public UnityEngine.Quaternion anchorRotation;
public UnityEngine.Quaternion parentAnchorRotation;
public bool matchAnchors;
public UnityEngine.ArticulationDofLock linearLockX;
public UnityEngine.ArticulationDofLock linearLockY;
public UnityEngine.ArticulationDofLock linearLockZ;
public UnityEngine.ArticulationDofLock swingYLock;
public UnityEngine.ArticulationDofLock swingZLock;
public UnityEngine.ArticulationDofLock twistLock;
public bool immovable;
public bool useGravity;
public float linearDamping;
public float angularDamping;
public float jointFriction;
public UnityEngine.Vector3 velocity;
public UnityEngine.Vector3 angularVelocity;
public float mass;
public UnityEngine.Vector3 centerOfMass;
public UnityEngine.Vector3 inertiaTensor;
public UnityEngine.Quaternion inertiaTensorRotation;
public float sleepThreshold;
public int solverIterations;
public int solverVelocityIterations;
public float maxAngularVelocity;
public float maxLinearVelocity;
public float maxJointVelocity;
public float maxDepenetrationVelocity;
public UnityEngine.CollisionDetectionMode collisionDetectionMode;
public BVA_ArticulationBody_Extra(){}

public BVA_ArticulationBody_Extra(UnityEngine.ArticulationBody target){
this.jointType = target.jointType;
this.anchorPosition = target.anchorPosition;
this.parentAnchorPosition = target.parentAnchorPosition;
this.anchorRotation = target.anchorRotation;
this.parentAnchorRotation = target.parentAnchorRotation;
#if UNITY_2021_1_OR_NEWER
			this.matchAnchors = target.matchAnchors;
#endif
this.linearLockX = target.linearLockX;
this.linearLockY = target.linearLockY;
this.linearLockZ = target.linearLockZ;
this.swingYLock = target.swingYLock;
this.swingZLock = target.swingZLock;
this.twistLock = target.twistLock;
this.immovable = target.immovable;
this.useGravity = target.useGravity;
this.linearDamping = target.linearDamping;
this.angularDamping = target.angularDamping;
this.jointFriction = target.jointFriction;
this.velocity = target.velocity;
this.angularVelocity = target.angularVelocity;
this.mass = target.mass;
this.centerOfMass = target.centerOfMass;
this.inertiaTensor = target.inertiaTensor;
this.inertiaTensorRotation = target.inertiaTensorRotation;
this.sleepThreshold = target.sleepThreshold;
this.solverIterations = target.solverIterations;
this.solverVelocityIterations = target.solverVelocityIterations;
this.maxAngularVelocity = target.maxAngularVelocity;
this.maxLinearVelocity = target.maxLinearVelocity;
this.maxJointVelocity = target.maxJointVelocity;
this.maxDepenetrationVelocity = target.maxDepenetrationVelocity;
this.collisionDetectionMode = target.collisionDetectionMode;
}
public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.ArticulationBody  target)
{
while (reader.Read())
{
if (reader.TokenType == JsonToken.PropertyName)
{
var curProp = reader.Value.ToString();
switch (curProp)
{
case nameof(BVA_ArticulationBody_Extra.jointType):
target.jointType = reader.ReadStringEnum<UnityEngine.ArticulationJointType>();
break;
case nameof(BVA_ArticulationBody_Extra.anchorPosition):
target.anchorPosition= reader.ReadAsVector3();
break;
case nameof(BVA_ArticulationBody_Extra.parentAnchorPosition):
target.parentAnchorPosition= reader.ReadAsVector3();
break;
case nameof(BVA_ArticulationBody_Extra.anchorRotation):
target.anchorRotation= reader.ReadAsQuaternion();
break;
case nameof(BVA_ArticulationBody_Extra.parentAnchorRotation):
target.parentAnchorRotation= reader.ReadAsQuaternion();
break;
#if UNITY_2021_1_OR_NEWER
case nameof(BVA_ArticulationBody_Extra.matchAnchors):
target.matchAnchors= reader.ReadAsBoolean().Value;
break;
#endif
case nameof(BVA_ArticulationBody_Extra.linearLockX):
target.linearLockX = reader.ReadStringEnum<UnityEngine.ArticulationDofLock>();
break;
case nameof(BVA_ArticulationBody_Extra.linearLockY):
target.linearLockY = reader.ReadStringEnum<UnityEngine.ArticulationDofLock>();
break;
case nameof(BVA_ArticulationBody_Extra.linearLockZ):
target.linearLockZ = reader.ReadStringEnum<UnityEngine.ArticulationDofLock>();
break;
case nameof(BVA_ArticulationBody_Extra.swingYLock):
target.swingYLock = reader.ReadStringEnum<UnityEngine.ArticulationDofLock>();
break;
case nameof(BVA_ArticulationBody_Extra.swingZLock):
target.swingZLock = reader.ReadStringEnum<UnityEngine.ArticulationDofLock>();
break;
case nameof(BVA_ArticulationBody_Extra.twistLock):
target.twistLock = reader.ReadStringEnum<UnityEngine.ArticulationDofLock>();
break;
case nameof(BVA_ArticulationBody_Extra.immovable):
target.immovable= reader.ReadAsBoolean().Value;
break;
case nameof(BVA_ArticulationBody_Extra.useGravity):
target.useGravity= reader.ReadAsBoolean().Value;
break;
case nameof(BVA_ArticulationBody_Extra.linearDamping):
target.linearDamping= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.angularDamping):
target.angularDamping= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.jointFriction):
target.jointFriction= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.velocity):
target.velocity= reader.ReadAsVector3();
break;
case nameof(BVA_ArticulationBody_Extra.angularVelocity):
target.angularVelocity= reader.ReadAsVector3();
break;
case nameof(BVA_ArticulationBody_Extra.mass):
target.mass= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.centerOfMass):
target.centerOfMass= reader.ReadAsVector3();
break;
case nameof(BVA_ArticulationBody_Extra.inertiaTensor):
target.inertiaTensor= reader.ReadAsVector3();
break;
case nameof(BVA_ArticulationBody_Extra.inertiaTensorRotation):
target.inertiaTensorRotation= reader.ReadAsQuaternion();
break;
case nameof(BVA_ArticulationBody_Extra.sleepThreshold):
target.sleepThreshold= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.solverIterations):
target.solverIterations= reader.ReadAsInt32().Value;
break;
case nameof(BVA_ArticulationBody_Extra.solverVelocityIterations):
target.solverVelocityIterations= reader.ReadAsInt32().Value;
break;
case nameof(BVA_ArticulationBody_Extra.maxAngularVelocity):
target.maxAngularVelocity= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.maxLinearVelocity):
target.maxLinearVelocity= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.maxJointVelocity):
target.maxJointVelocity= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.maxDepenetrationVelocity):
target.maxDepenetrationVelocity= reader.ReadAsFloat();
break;
case nameof(BVA_ArticulationBody_Extra.collisionDetectionMode):
target.collisionDetectionMode = reader.ReadStringEnum<UnityEngine.CollisionDetectionMode>();
break;
}
}
}
}
public JProperty Serialize()
{
JObject jo = new JObject();
jo.Add(nameof(jointType), jointType.ToString());
jo.Add(nameof(anchorPosition), anchorPosition.ToJArray());
jo.Add(nameof(parentAnchorPosition), parentAnchorPosition.ToJArray());
jo.Add(nameof(anchorRotation), anchorRotation.ToJArray());
jo.Add(nameof(parentAnchorRotation), parentAnchorRotation.ToJArray());
jo.Add(nameof(matchAnchors), matchAnchors);
jo.Add(nameof(linearLockX), linearLockX.ToString());
jo.Add(nameof(linearLockY), linearLockY.ToString());
jo.Add(nameof(linearLockZ), linearLockZ.ToString());
jo.Add(nameof(swingYLock), swingYLock.ToString());
jo.Add(nameof(swingZLock), swingZLock.ToString());
jo.Add(nameof(twistLock), twistLock.ToString());
jo.Add(nameof(immovable), immovable);
jo.Add(nameof(useGravity), useGravity);
jo.Add(nameof(linearDamping), linearDamping);
jo.Add(nameof(angularDamping), angularDamping);
jo.Add(nameof(jointFriction), jointFriction);
jo.Add(nameof(velocity), velocity.ToJArray());
jo.Add(nameof(angularVelocity), angularVelocity.ToJArray());
jo.Add(nameof(mass), mass);
jo.Add(nameof(centerOfMass), centerOfMass.ToJArray());
jo.Add(nameof(inertiaTensor), inertiaTensor.ToJArray());
jo.Add(nameof(inertiaTensorRotation), inertiaTensorRotation.ToJArray());
jo.Add(nameof(sleepThreshold), sleepThreshold);
jo.Add(nameof(solverIterations), solverIterations);
jo.Add(nameof(solverVelocityIterations), solverVelocityIterations);
jo.Add(nameof(maxAngularVelocity), maxAngularVelocity);
jo.Add(nameof(maxLinearVelocity), maxLinearVelocity);
jo.Add(nameof(maxJointVelocity), maxJointVelocity);
jo.Add(nameof(maxDepenetrationVelocity), maxDepenetrationVelocity);
jo.Add(nameof(collisionDetectionMode), collisionDetectionMode.ToString());
return new JProperty(BVA_ArticulationBody_Extra.PROPERTY, jo);
}
}
}
