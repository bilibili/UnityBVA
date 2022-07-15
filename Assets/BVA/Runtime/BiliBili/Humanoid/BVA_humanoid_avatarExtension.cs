using GLTF.Extensions;
using GLTF.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using BVA.Extensions;
namespace GLTF.Schema.BVA
{
    public class BVA_humanoid_avatarExtension : IExtension
    {
        public GltfAvatar human;
        public static readonly float DEFAULT_ARM_STRETCH = 0.05f;
        public static readonly float DEFAULT_LEG_STRETCH = 0.05f;
        public static readonly float DEFAULT_UPPER_ARM_TWIST = 0.5f;
        public static readonly float DEFAULT_LOWER_ARM_TWIST = 0.5f;
        public static readonly float DEFAULT_UPPER_LEG_TWIST = 0.5f;
        public static readonly float DEFAULT_LOWER_LEG_TWIST = 0.5f;
        public static readonly float DEFAULT_FEET_SPACING = 0f;
        public static readonly bool DEFAULT_HAS_TRANSLATION_DOF = false;

        public static readonly bool DEFAULT_useDefaultValues = true;
        public static readonly Vector3 DEFAULT_min = Vector3.Zero;
        public static readonly Vector3 DEFAULT_max = Vector3.Zero;
        public static readonly Vector3 DEFAULT_center = Vector3.Zero;
        public static readonly float DEFAULT_axisLength = 0.0f;
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_humanoid_avatarExtension(human);
        }
        public BVA_humanoid_avatarExtension(GltfAvatar humanoid) { human = humanoid; }

        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            if (human.armStretch != DEFAULT_ARM_STRETCH) propObj.Add(nameof(human.armStretch), human.armStretch);
            if (human.legStretch != DEFAULT_LEG_STRETCH) propObj.Add(nameof(human.legStretch), human.legStretch);
            if (human.upperArmTwist != DEFAULT_UPPER_ARM_TWIST) propObj.Add(nameof(human.upperArmTwist), human.upperArmTwist);
            if (human.lowerArmTwist != DEFAULT_LOWER_ARM_TWIST) propObj.Add(nameof(human.lowerArmTwist), human.lowerArmTwist);
            if (human.upperLegTwist != DEFAULT_UPPER_LEG_TWIST) propObj.Add(nameof(human.upperLegTwist), human.upperLegTwist);
            if (human.lowerLegTwist != DEFAULT_LOWER_LEG_TWIST) propObj.Add(nameof(human.lowerLegTwist), human.lowerLegTwist);
            if (human.feetSpacing != DEFAULT_FEET_SPACING) propObj.Add(nameof(human.feetSpacing), human.feetSpacing);
            if (human.hasTranslationDoF != DEFAULT_HAS_TRANSLATION_DOF) propObj.Add(nameof(human.hasTranslationDoF), human.hasTranslationDoF);
            JArray bones = new JArray();
            foreach (var v in human.humanBones)
            {
                if (v.node < 0) continue;
                JObject humanBones = new JObject();
                humanBones.Add(nameof(v.bone), v.bone);
                humanBones.Add(nameof(v.node), v.node);
                if (v.useDefaultValues != DEFAULT_useDefaultValues) humanBones.Add(nameof(v.useDefaultValues), v.useDefaultValues);
                if (v.min != DEFAULT_min) humanBones.Add(nameof(v.min), v.min.ToJArray());
                if (v.max != DEFAULT_max) humanBones.Add(nameof(v.max), v.max.ToJArray());
                if (v.center != DEFAULT_center) humanBones.Add(nameof(v.center), v.center.ToJArray());
                if (v.axisLength != DEFAULT_axisLength) humanBones.Add(nameof(v.axisLength), v.axisLength);
                bones.Add(humanBones);
            }
            propObj.Add(nameof(human.humanBones), bones);
            JProperty jProperty = new JProperty(BVA_humanoid_avatarExtensionFactory.EXTENSION_NAME, propObj);
            return jProperty;
        }

        public static GlTFHumanoidBone DeserializeHumanoidBone(GLTFRoot root, JsonReader reader)
        {
            var humanoidBone = new GlTFHumanoidBone() { useDefaultValues = true, min = Vector3.Zero, max = Vector3.Zero, center = Vector3.Zero, axisLength = 0 };

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case "bone":
                        humanoidBone.bone = reader.ReadAsString();
                        break;
                    case "node":
                        humanoidBone.node = reader.ReadAsInt32().Value;
                        break;
                    case "useDefaultValues":
                        humanoidBone.useDefaultValues = reader.ReadAsBoolean().Value;
                        break;
                    case "min":
                        humanoidBone.min = reader.ReadAsVector3();
                        break;
                    case "max":
                        humanoidBone.max = reader.ReadAsVector3();
                        break;
                    case "center":
                        humanoidBone.center = reader.ReadAsVector3();
                        break;
                    case "axisLength":
                        humanoidBone.axisLength = reader.ReadAsFloat();
                        break;
                    default:
                        humanoidBone.DefaultPropertyDeserializer(root, reader);
                        break;
                }
            }

            return humanoidBone;
        }

        public static BVA_humanoid_avatarExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            var human = new GltfAvatar();
            human.armStretch = DEFAULT_ARM_STRETCH;
            human.legStretch = DEFAULT_LEG_STRETCH;
            human.upperArmTwist = DEFAULT_UPPER_ARM_TWIST;
            human.lowerArmTwist = DEFAULT_LOWER_ARM_TWIST;
            human.upperLegTwist = DEFAULT_UPPER_LEG_TWIST;
            human.lowerLegTwist = DEFAULT_LOWER_LEG_TWIST;
            human.feetSpacing = DEFAULT_FEET_SPACING;
            human.hasTranslationDoF = DEFAULT_HAS_TRANSLATION_DOF;
            human.humanBones = new List<GlTFHumanoidBone>();

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case "armStretch":
                        human.armStretch = reader.ReadAsFloat();
                        break;
                    case "legStretch":
                        human.legStretch = reader.ReadAsFloat();
                        break;
                    case "upperArmTwist":
                        human.upperArmTwist = reader.ReadAsFloat();
                        break;
                    case "lowerArmTwist":
                        human.lowerArmTwist = reader.ReadAsFloat();
                        break;
                    case "upperLegTwist":
                        human.upperLegTwist = reader.ReadAsFloat();
                        break;
                    case "lowerLegTwist":
                        human.lowerLegTwist = reader.ReadAsFloat();
                        break;
                    case "feetSpacing":
                        human.feetSpacing = reader.ReadAsFloat();
                        break;
                    case "hasTranslationDoF":
                        human.hasTranslationDoF = reader.ReadAsBoolean().Value;
                        break;
                    case "humanBones":
                        human.humanBones = reader.ReadList(() => DeserializeHumanoidBone(root, reader));
                        break;
                }
            }

            return new BVA_humanoid_avatarExtension(human);
        }
    }
    public class BVA_humanoid_avatarExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_humanoid_avatar";
        public const string EXTENSION_ELEMENT_NAME = "avatars";
        public BVA_humanoid_avatarExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_humanoid_avatarExtensionFactory(AvatarId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public AvatarId id;
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_humanoid_avatarExtensionFactory(id);
        }

        public JProperty Serialize()
        {
            JProperty node = new JProperty(EXTENSION_ELEMENT_NAME, id.Id);
            return new JProperty(EXTENSION_NAME, new JObject(node));
        }
        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            if (extensionToken != null)
            {
                JToken indexToken = extensionToken.Value[EXTENSION_ELEMENT_NAME];
                int _id = indexToken != null ? indexToken.DeserializeAsInt() : -1;
                id = new AvatarId() { Id = _id, Root = root };
            }
            return new BVA_humanoid_avatarExtensionFactory(id);
        }
    }
}