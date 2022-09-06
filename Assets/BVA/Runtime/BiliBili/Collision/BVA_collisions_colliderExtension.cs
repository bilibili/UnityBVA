using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using BVA.Extensions;
using UnityEngine;
namespace GLTF.Schema.BVA
{
    /// <summary>
    /// This extension defines a set of collisions for use with glTF 2.0.
    /// 
    /// </summary>
    public class BVA_collisions_colliderExtension : IExtension
    {
        public static readonly Vector3 DEFAULT_CENTER = Vector3.zero;
        public static readonly Vector3 DEFAULT_SIZE = Vector3.one;
        public static readonly bool DEFAULT_IS_TRIGGER = false;

        public CollisionType type;
        public bool isTrigger;
        public Vector3 center;

        public Vector3 size;

        public float radius;
        public float height;
        public Direction direction;

        public bool convex;

        public BVA_collisions_colliderExtension(CollisionType type, bool isTrigger, Vector3 center, float radius, float height, Direction direction)
        {
            this.type = type;
            this.isTrigger = isTrigger;
            this.center = center;
            this.radius = radius;
            this.height = height;
            this.direction = direction;
        }

        public BVA_collisions_colliderExtension(CollisionType type, bool isTrigger, bool convex)
        {
            this.type = type;
            this.isTrigger = isTrigger;
            this.convex = convex;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_collisions_colliderExtension(type, isTrigger, center, radius, height, direction);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var v = obj as BVA_collisions_colliderExtension;
            return type == v.type && isTrigger == v.isTrigger && center == v.center && radius == v.radius
                && height == v.height && direction == v.direction;
        }
        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            propObj.Add(nameof(type), type.ToString());
            if (isTrigger) propObj.Add(nameof(isTrigger), isTrigger);
            if (center != DEFAULT_CENTER) propObj.Add(nameof(center), center.ToJArray());

            if (type == CollisionType.Box) propObj.Add(nameof(size), size.ToJArray());
            if (type == CollisionType.Sphere) propObj.Add(nameof(radius), radius);
            if (type == CollisionType.Capsule)
            {
                propObj.Add(nameof(radius), radius);
                propObj.Add(nameof(height), height);
                propObj.Add(nameof(direction), direction.ToString());
            }
            if (type == CollisionType.Mesh) propObj.Add(nameof(convex), convex);
            JProperty jProperty = new JProperty(BVA_collisions_colliderExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }
        public static BVA_collisions_colliderExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            CollisionType type = CollisionType.Box;
            bool isTrigger = DEFAULT_IS_TRIGGER;
            Vector3 center = DEFAULT_CENTER;
            Vector3 size = DEFAULT_SIZE;
            float radius = 1.0f;
            float height = 1.0f;
            bool convex = false;
            Direction direction = Direction.x;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(type):
                        type = reader.ReadStringEnum<CollisionType>();
                        break;
                    case nameof(isTrigger):
                        isTrigger = reader.ReadAsBoolean().Value;
                        break;
                    case nameof(center):
                        center = reader.ReadAsVector3();
                        break;
                    case nameof(size):
                        size = reader.ReadAsVector3();
                        break;
                    case nameof(radius):
                        radius = reader.ReadAsFloat();
                        break;
                    case nameof(height):
                        height = reader.ReadAsFloat();
                        break;
                    case nameof(direction):
                        direction = reader.ReadStringEnum<Direction>();
                        break;
                    case nameof(convex):
                        convex = reader.ReadAsBoolean().Value;
                        break;
                }
            }
            if (type == CollisionType.Mesh) 
                return new BVA_collisions_colliderExtension(type, isTrigger, convex);
            return new BVA_collisions_colliderExtension(type, isTrigger, center, radius, height, direction);
        }
    }

    public class BVA_collisions_colliderExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_collisions_collider";
        public const string EXTENSION_ELEMENT_NAME = "collisions";
        public BVA_collisions_colliderExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_collisions_colliderExtensionFactory(List<CollisionId> _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            ids = _id;
        }
        public List<CollisionId> ids;
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_collisions_colliderExtensionFactory() {ids=ids};
        }

        public JProperty Serialize()
        {
            var array = new JArray();
            foreach(var v in ids)
            {
                array.Add(v.Id);
            }
            return new JProperty(EXTENSION_NAME, new JObject(new JProperty(EXTENSION_ELEMENT_NAME, array)));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            List< int> id = new List<int>();
            if (extensionToken != null)
            {
                JToken indexToken = extensionToken.Value[EXTENSION_ELEMENT_NAME];
                id = indexToken != null ? indexToken.DeserializeAsIntList() : id;
            }
            List<CollisionId> li = new List<CollisionId>();
            foreach(var v in id)
            {
                li.Add(new CollisionId() { Id = v, Root = root });
            }
            return new BVA_collisions_colliderExtensionFactory(li);
        }
    }
}
