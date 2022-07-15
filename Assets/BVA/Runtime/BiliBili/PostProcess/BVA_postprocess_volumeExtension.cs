using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GLTF.Schema.BVA;

namespace GLTF.Schema.BVA
{
    public class BVA_postprocess_volumeExtension : IExtension
    {
        public PostProcessAsset profile;
        public BVA_postprocess_volumeExtension(PostProcessAsset profile)
        {
            this.profile = profile;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_postprocess_volumeExtension(profile);
        }

        public JProperty Serialize()
        {
            JObject propObj = profile.Serialize();

            JProperty jProperty = new JProperty(BVA_postprocess_volumeExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }

        public static BVA_postprocess_volumeExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            PostProcessAsset profile = null;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(profile.postProcesses):
                        profile = PostProcessAsset.Deserialize(root, reader);
                        break;
                }
            }
            return new BVA_postprocess_volumeExtension(profile);
        }
    }


    public class BVA_postprocess_volumeExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_postprocess_volume";
        public const string EXTENSION_ELEMENT_NAME = "profiles";
        public BVA_postprocess_volumeExtensionFactory() { ExtensionName = EXTENSION_NAME; ElementName = EXTENSION_ELEMENT_NAME; }
        public BVA_postprocess_volumeExtensionFactory(PostProcessId _id, bool isGlobal, float weight, float blendDistance, float priority)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
            this.isGlobal = isGlobal;
            this.weight = weight;
            this.blendDistance = blendDistance;
            this.priority = priority;
        }
        public PostProcessId id;
        public bool isGlobal;
        public float weight, blendDistance, priority;
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_postprocess_volumeExtensionFactory(id, isGlobal, weight, blendDistance, priority);
        }

        public JProperty Serialize()
        {
            return new JProperty(EXTENSION_NAME, new JObject(
                new JProperty("postProcess", id.Id),
                new JProperty("isGlobal", isGlobal),
                new JProperty("weight", weight),
                new JProperty("blendDistance", blendDistance),
                new JProperty("priority", priority))
                );
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            int id = 0;
            isGlobal = false;
            weight = 1.0f;
            blendDistance = 0;
            priority = 0;
            if (extensionToken != null)
            {
                JToken nameToken = extensionToken.Value["postProcess"];
                id = nameToken != null ? nameToken.DeserializeAsInt() : id;
                JToken isGlobalToken = extensionToken.Value["isGlobal"];
                isGlobal = isGlobalToken != null ? isGlobalToken.DeserializeAsBool() : isGlobal;
                JToken weightToken = extensionToken.Value["weight"];
                weight = weightToken != null ? weightToken.DeserializeAsFloat() : weight;
                JToken blendDistanceToken = extensionToken.Value["blendDistance"];
                blendDistance = blendDistanceToken != null ? blendDistanceToken.DeserializeAsFloat() : blendDistance;
                JToken priorityToken = extensionToken.Value["priority"];
                priority = priorityToken != null ? priorityToken.DeserializeAsFloat() : priority;
            }
            PostProcessId li = new PostProcessId { Id = id, Root = root };
            return new BVA_postprocess_volumeExtensionFactory(li, isGlobal, weight, blendDistance, priority);
        }
    }
}
