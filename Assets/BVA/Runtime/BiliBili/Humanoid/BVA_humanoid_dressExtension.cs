using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema.BVA
{
    public class BVA_humanoid_dressExtension : IExtension
    {
        public GltfDress gltfObject;
        public BVA_humanoid_dressExtension(GltfDress dressUp)
        {
            gltfObject = dressUp;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_humanoid_dressExtension(gltfObject);
        }

        public JProperty Serialize()
        {
            JProperty content = new JProperty("dress");
            JArray jArrayDressConfigs = new JArray();
            foreach (var dressConfig in gltfObject.dressUpConfigs)
            {
                JArray jArrayRendererConfigs = new JArray();
                foreach (var rendererConfig in dressConfig.rendererConfigs)
                {
                    JObject jRendererConfig = new JObject();
                    jRendererConfig.Add(nameof(rendererConfig.node), rendererConfig.node);
                    JArray jArrayMaterials = new JArray();
                    foreach (var material in rendererConfig.materials)
                        jArrayMaterials.Add(material);
                    jRendererConfig.Add(nameof(rendererConfig.materials), jArrayMaterials);
                    jArrayRendererConfigs.Add(jRendererConfig);
                }
                var jDressConfig = new JObject();
                if (!string.IsNullOrEmpty(dressConfig.name)) jDressConfig.Add(new JProperty(nameof(dressConfig.name), dressConfig.name));
                jDressConfig.Add(new JProperty(nameof(dressConfig.rendererConfigs), jArrayRendererConfigs));
                jArrayDressConfigs.Add(new JObject(jDressConfig));
            }
            JProperty jProperty = new JProperty(BVA_humanoid_dressExtensionFactory.EXTENSION_NAME, jArrayDressConfigs);
            return jProperty;
        }

        public static BVA_humanoid_dressExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            GltfDress dressUp = new GltfDress();
            JArray ja = JArray.Load(reader);
            foreach (var item in ja.Children())
            {
                var itemReader = (item as JObject).CreateReader();
                dressUp.dressUpConfigs.Add(GltfDress.DressUpConfig.Deserialize(root, itemReader));
            }
            return new BVA_humanoid_dressExtension(dressUp);
        }
    }
    public class BVA_humanoid_dressExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_humanoid_dress";
        public const string EXTENSION_ELEMENT_NAME = "dress";
        public DressId id;
        public BVA_humanoid_dressExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_humanoid_dressExtensionFactory(DressId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_humanoid_dressExtensionFactory() { id = id };
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
                id = new DressId() { Id = _id, Root = root };
            }
            return new BVA_humanoid_dressExtensionFactory(id);
        }
    }
}