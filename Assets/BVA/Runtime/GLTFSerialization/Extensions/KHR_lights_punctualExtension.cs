using GLTF.Extensions;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace GLTF.Schema
{
    public enum LightType
    {
        spot = 0,
        directional = 1,
        point = 2
    }
    /// <summary>
    /// This extension defines a set of lights for use with glTF 2.0. Lights define light sources within a scene.
    /// 
    /// Spec can be found here:
    /// https://github.com/KhronosGroup/glTF/tree/master/extensions/Khronos/KHR_lights_punctual
    /// </summary>
    public class KHR_lights_punctualExtension : IExtension
    {
        public static readonly Color DEFAULT_COLOR = Color.white;
        public const float DEFAULT_INTENSITY = 1.0f;

        public string name;
        public Color color;
        public float intensity;
        public LightType type;
        public float range;

        /// <summary>
        /// Spot lights emit light in a cone in the direction of the local -z axis. 
        /// The angle and falloff of the cone is defined using two numbers, the innerConeAngle and outerConeAngle
        /// </summary>
        public float innerConeAngle;
        public float outerConeAngle;
        public KHR_lights_punctualExtension(LightType _type, string _name, Color _color, float _intensity, float _range, float _innerConeAngle, float _outerConeAngle)
        {
            name = _name;
            color = _color;
            intensity = _intensity;
            type = _type;
            range = _range;
            innerConeAngle = _innerConeAngle;
            outerConeAngle = _outerConeAngle;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new KHR_lights_punctualExtension(type, name, color, intensity, range, innerConeAngle, outerConeAngle);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var v = obj as KHR_lights_punctualExtension;
            return color == v.color && intensity == v.intensity && type == v.type && range == v.range
                && innerConeAngle == v.innerConeAngle && outerConeAngle == v.outerConeAngle;
        }
        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            propObj.Add(nameof(type), type.ToString());
            if (name != "") propObj.Add(nameof(name), name);
            if (color != Color.white) propObj.Add(nameof(color), new JArray(color.r, color.g, color.b));
            if (intensity != 1) propObj.Add(nameof(intensity), intensity);
            if (type != LightType.directional) propObj.Add(nameof(range), range);
            if (type == LightType.spot)
            {
                var spotObj = new JObject();
                spotObj.Add(nameof(innerConeAngle), innerConeAngle);
                spotObj.Add(nameof(outerConeAngle), outerConeAngle);
                propObj.Add(LightType.spot.ToString(), spotObj);
            }
            JProperty jProperty = new JProperty(KHR_lights_punctualExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }

        public static void DeserializeSpot(GLTFRoot root, JsonReader reader, ref float innerConeAngle, ref float outerConeAngle)
        {
            reader.Read();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var spotProp = reader.Value.ToString();

                switch (spotProp)
                {
                    case nameof(innerConeAngle):
                        innerConeAngle = reader.ReadAsFloat();
                        break;
                    case nameof(outerConeAngle):
                        outerConeAngle = reader.ReadAsFloat();
                        break;
                }
            }
        }
        public static KHR_lights_punctualExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            LightType type = LightType.directional;
            string name = "";
            float intensity = 1.0f;
            Color color = Color.white;
            float range = 1.0f;
            float innerConeAngle = 0;
            float outerConeAngle = MathF.PI / 4;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(name):
                        name = reader.ReadAsString();
                        break;
                    case nameof(type):
                        type = reader.ReadStringEnum<LightType>();
                        break;
                    case nameof(intensity):
                        intensity = reader.ReadAsFloat();
                        break;
                    case nameof(color):
                        color = reader.ReadAsRGBColor();
                        break;
                    case nameof(range):
                        range = reader.ReadAsFloat();
                        break;
                    case "spot":
                        {
                            DeserializeSpot(root, reader, ref innerConeAngle, ref outerConeAngle);
                            break;
                        }
                }
            }

            return new KHR_lights_punctualExtension(type, name, color, intensity, range, innerConeAngle, outerConeAngle);
        }
    }


    public class KHR_lights_punctualExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "KHR_lights_punctual";
        public const string EXTENSION_ELEMENT_NAME = "lights";
        public KHR_lights_punctualExtensionFactory() { ExtensionName = EXTENSION_NAME; ElementName = EXTENSION_ELEMENT_NAME; }
        public KHR_lights_punctualExtensionFactory(LightId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public LightId id;
        public IExtension Clone(GLTFRoot root)
        {
            return new KHR_lights_punctualExtensionFactory(new LightId() { Root = root });
        }

        public JProperty Serialize()
        {
            return new JProperty(KHR_lights_punctualExtensionFactory.EXTENSION_NAME, new JObject(new JProperty("light", id.Id)));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            int id = 0;
            if (extensionToken != null)
            {
                JToken nameToken = extensionToken.Value["light"];
                id = nameToken != null ? nameToken.DeserializeAsInt() : id;
            }
            LightId li = new LightId { Id = id, Root = root };
            return new KHR_lights_punctualExtensionFactory(li);
        }
    }
}
