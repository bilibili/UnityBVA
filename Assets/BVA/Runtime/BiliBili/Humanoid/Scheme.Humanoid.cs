using GLTF.Math;
using Newtonsoft.Json;
using GLTF.Extensions;
using System.Collections.Generic;

namespace GLTF.Schema.BVA
{
    public enum AvatarBone
    {
        hips,
        leftUpperLeg,
        rightUpperLeg,
        leftLowerLeg,
        rightLowerLeg,
        leftFoot,
        rightFoot,
        spine,
        chest,
        neck,
        head,
        leftShoulder,
        rightShoulder,
        leftUpperArm,
        rightUpperArm,
        leftLowerArm,
        rightLowerArm,
        leftHand,
        rightHand,
        leftToes,
        rightToes,
        leftEye,
        rightEye,
        jaw,
        leftThumbProximal,
        leftThumbIntermediate,
        leftThumbDistal,
        leftIndexProximal,
        leftIndexIntermediate,
        leftIndexDistal,
        leftMiddleProximal,
        leftMiddleIntermediate,
        leftMiddleDistal,
        leftRingProximal,
        leftRingIntermediate,
        leftRingDistal,
        leftLittleProximal,
        leftLittleIntermediate,
        leftLittleDistal,
        rightThumbProximal,
        rightThumbIntermediate,
        rightThumbDistal,
        rightIndexProximal,
        rightIndexIntermediate,
        rightIndexDistal,
        rightMiddleProximal,
        rightMiddleIntermediate,
        rightMiddleDistal,
        rightRingProximal,
        rightRingIntermediate,
        rightRingDistal,
        rightLittleProximal,
        rightLittleIntermediate,
        rightLittleDistal,
        upperChest,

        unknown,
    }
    public class GlTFHumanoidBone : GLTFProperty
    {
        public string bone;

        public int node = -1;

        public bool useDefaultValues = true;

        public Vector3 min;

        public Vector3 max;

        public Vector3 center;

        public float axisLength;
    }

    public class GltfAvatar
    {
        public List<GlTFHumanoidBone> humanBones = new List<GlTFHumanoidBone>();

        public float armStretch = 0.05f;

        public float legStretch = 0.05f;

        public float upperArmTwist = 0.5f;

        public float lowerArmTwist = 0.5f;

        public float upperLegTwist = 0.5f;

        public float lowerLegTwist = 0.5f;

        public float feetSpacing = 0;

        public bool hasTranslationDoF = false;
    }
    public class GltfDress
    {
        public struct GltfRendererMaterialConfig
        {
            public int node;
            public List<int> materials;
            public static GltfRendererMaterialConfig Deserialize(GLTFRoot root, JsonReader reader)
            {
                GltfRendererMaterialConfig config = new GltfRendererMaterialConfig() { materials = new List<int>() };
                while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();

                    switch (curProp)
                    {
                        case nameof(config.node):
                            config.node = reader.ReadAsInt32().Value;
                            break;
                        case nameof(config.materials):
                            config.materials = reader.ReadInt32List();
                            break;
                    }
                }
                return config;
            }
        }
        public class DressUpConfig
        {
            public string name;
            public List<GltfRendererMaterialConfig> rendererConfigs;
            public static DressUpConfig Deserialize(GLTFRoot root, JsonReader reader)
            {
                DressUpConfig config = new DressUpConfig() { rendererConfigs= new List<GltfRendererMaterialConfig>()};
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        var curProp = reader.Value.ToString();

                        switch (curProp)
                        {
                            case nameof(config.name):
                                config.name = reader.ReadAsString();
                                break;
                            case nameof(config.rendererConfigs):
                                config.rendererConfigs = reader.ReadList(() => GltfRendererMaterialConfig.Deserialize(root, reader));
                                break;
                        }
                    }
                }
                return config;
            }
        }
        public List<DressUpConfig> dressUpConfigs;
        public GltfDress()
        {
            dressUpConfigs = new List<DressUpConfig>();
        }
    }
}