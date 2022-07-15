using BVA.Component;
using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GLTF.Schema.BVA
{
    public class BVA_humanoid_accessoryExtension : IExtension
    {
        public List<AccessoryConfig> accessoryConfigs;
        public BVA_humanoid_accessoryExtension(List<AccessoryConfig> accessories)
        {
            accessoryConfigs = accessories;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_humanoid_accessoryExtension(accessoryConfigs);
        }

        public JProperty Serialize()
        {
            JProperty content = new JProperty("accessory");
            JArray jArrayDressConfigs = new JArray();
            foreach (var dressConfig in accessoryConfigs)
            {
                if (dressConfig.node < 0) continue;
                var jDressConfig = new JObject();
                if (!string.IsNullOrWhiteSpace(dressConfig.name)) jDressConfig.Add(nameof(dressConfig.name), dressConfig.name);
                if (!string.IsNullOrWhiteSpace(dressConfig.catagory)) jDressConfig.Add(nameof(dressConfig.catagory), dressConfig.catagory);
                jDressConfig.Add(nameof(dressConfig.node), dressConfig.node);
                jArrayDressConfigs.Add(new JObject(jDressConfig));
            }
            JProperty jProperty = new JProperty(BVA_humanoid_accessoryExtensionFactory.EXTENSION_NAME, jArrayDressConfigs);
            return jProperty;
        }

        public static BVA_humanoid_accessoryExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            List<AccessoryConfig> accessoryConfigs = new List<AccessoryConfig>();
            JArray ja = JArray.Load(reader);
            foreach (var item in ja.Children())
            {
                string name = item[nameof(AccessoryConfig.name)].DeserializeAsString();
                string catagory = item[nameof(AccessoryConfig.catagory)].DeserializeAsString();
                int node = item[nameof(AccessoryConfig.node)].DeserializeAsInt();
                accessoryConfigs.Add(new AccessoryConfig() { name = name, catagory = catagory, node = node });
            }
            return new BVA_humanoid_accessoryExtension(accessoryConfigs);
        }
    }
    public class BVA_humanoid_accessoryExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_humanoid_accessory";
        public const string EXTENSION_ELEMENT_NAME = "accessory";
        public AccessoryId id;
        public BVA_humanoid_accessoryExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_humanoid_accessoryExtensionFactory(AccessoryId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_humanoid_accessoryExtensionFactory() { id = id };
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
                id = new AccessoryId() { Id = _id, Root = root };
            }
            return new BVA_humanoid_accessoryExtensionFactory(id);
        }
    }
}