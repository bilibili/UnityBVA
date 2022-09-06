using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GLTF.Extensions;
using BVA.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_Rigidbody_Extra : IExtra
    {
        public const string PROPERTY = "BVA_Rigidbody_Extra";
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
        public BVA_Rigidbody_Extra() { }

        public BVA_Rigidbody_Extra(UnityEngine.Rigidbody target)
        {
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
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.Rigidbody target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_Rigidbody_Extra.velocity):
                            target.velocity = reader.ReadAsVector3();
                            break;
                        case nameof(BVA_Rigidbody_Extra.angularVelocity):
                            target.angularVelocity = reader.ReadAsVector3();
                            break;
                        case nameof(BVA_Rigidbody_Extra.drag):
                            target.drag = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_Rigidbody_Extra.angularDrag):
                            target.angularDrag = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_Rigidbody_Extra.mass):
                            target.mass = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_Rigidbody_Extra.useGravity):
                            target.useGravity = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_Rigidbody_Extra.maxDepenetrationVelocity):
                            target.maxDepenetrationVelocity = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_Rigidbody_Extra.isKinematic):
                            target.isKinematic = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_Rigidbody_Extra.freezeRotation):
                            target.freezeRotation = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_Rigidbody_Extra.constraints):
                            target.constraints = reader.ReadStringEnum<UnityEngine.RigidbodyConstraints>();
                            break;
                        case nameof(BVA_Rigidbody_Extra.collisionDetectionMode):
                            target.collisionDetectionMode = reader.ReadStringEnum<UnityEngine.CollisionDetectionMode>();
                            break;
                        case nameof(BVA_Rigidbody_Extra.centerOfMass):
                            target.centerOfMass = reader.ReadAsVector3();
                            break;
                        case nameof(BVA_Rigidbody_Extra.inertiaTensorRotation):
                            target.inertiaTensorRotation = reader.ReadAsQuaternion();
                            break;
                        case nameof(BVA_Rigidbody_Extra.inertiaTensor):
                            target.inertiaTensor = reader.ReadAsVector3();
                            break;
                        case nameof(BVA_Rigidbody_Extra.detectCollisions):
                            target.detectCollisions = reader.ReadAsBoolean().Value;
                            break;
                        case nameof(BVA_Rigidbody_Extra.position):
                            target.position = reader.ReadAsVector3();
                            break;
                        case nameof(BVA_Rigidbody_Extra.rotation):
                            target.rotation = reader.ReadAsQuaternion();
                            break;
                        case nameof(BVA_Rigidbody_Extra.interpolation):
                            target.interpolation = reader.ReadStringEnum<UnityEngine.RigidbodyInterpolation>();
                            break;
                        case nameof(BVA_Rigidbody_Extra.solverIterations):
                            target.solverIterations = reader.ReadAsInt32().Value;
                            break;
                        case nameof(BVA_Rigidbody_Extra.sleepThreshold):
                            target.sleepThreshold = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_Rigidbody_Extra.maxAngularVelocity):
                            target.maxAngularVelocity = reader.ReadAsFloat();
                            break;
                        case nameof(BVA_Rigidbody_Extra.solverVelocityIterations):
                            target.solverVelocityIterations = reader.ReadAsInt32().Value;
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
            return new JProperty(BVA_Rigidbody_Extra.PROPERTY, jo);
        }
    }
}
