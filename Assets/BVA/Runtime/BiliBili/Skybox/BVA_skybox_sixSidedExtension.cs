using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema.BVA
{
    public class BVA_skybox_sixSidedExtension : IExtension
    {
        public MaterialId material;
        public BVA_skybox_sixSidedExtension(MaterialId material)
        {
            this.material = material;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_skybox_sixSidedExtension(material);
        }

        public JProperty Serialize()
        {
            var jo = new JObject(new JProperty(nameof(material), material.Id));
            JProperty jProperty = new JProperty(BVA_skybox_sixSidedExtensionFactory.EXTENSION_NAME, jo);
            return jProperty;
        }
        public static BVA_skybox_sixSidedExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            var id = new MaterialId() { Root = root };
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(material):
                        id.Id = reader.ReadAsInt32().Value;
                        break;
                }
            }
            return new BVA_skybox_sixSidedExtension(id);
        }
    }
    public class BVA_skybox_sixSidedExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "BVA_skybox_sixSidedExtension";
        public const string EXTENSION_ELEMENT_NAME = "mateirals";
        public BVA_skybox_sixSidedExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            return null;
        }
    }
}