using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BVA.Component;

namespace GLTF.Schema.BVA
{
    public class BVA_metaExtension : IExtension
    {
        public BVAMetaInfoScriptableObject metaInfo;
        public BVA_metaExtension(BVAMetaInfoScriptableObject asset)
        {
            metaInfo = asset;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_metaExtension(metaInfo);
        }

        public JProperty Serialize()
        {
            JProperty jProperty = new JProperty(BVA_metaExtensionFactory.EXTENSION_NAME, metaInfo.Serialize());
            return jProperty;
        }

        public static BVA_metaExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            BVAMetaInfoScriptableObject meta = BVAMetaInfoScriptableObject.CreateInstance<BVAMetaInfoScriptableObject>();

            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();
                switch (curProp)
                {
                    case nameof(meta.formatVersion):
                        meta.formatVersion = reader.ReadAsString();
                        break;
                    case nameof(meta.title):
                        meta.title = reader.ReadAsString();
                        break;
                    case nameof(meta.version):
                        meta.version = reader.ReadAsString();
                        break;
                    case nameof(meta.author):
                        meta.author = reader.ReadAsString();
                        break;
                    case nameof(meta.contact):
                        meta.contact = reader.ReadAsString();
                        break;
                    case nameof(meta.reference):
                        meta.reference = reader.ReadAsString();
                        break;
                    case nameof(meta.thumbnail):
                        meta.SetTextureId(new TextureId() { Id = reader.ReadAsInt32().Value, Root = root });
                        break;
                    case nameof(meta.legalUser):
                        meta.legalUser = reader.ReadStringEnum<LegalUser>();
                        break;
                    case nameof(meta.contentType):
                        meta. contentType = reader.ReadStringEnum<ContentType>();
                        break;
                    case nameof(meta.violentUsage):
                        meta.violentUsage = reader.ReadStringEnum<UsageLicense>();
                        break;
                    case nameof(meta.sexualUsage):
                        meta.sexualUsage = reader.ReadStringEnum<UsageLicense>();
                        break;
                    case nameof(meta.commercialUsage):
                        meta.commercialUsage = reader.ReadStringEnum<UsageLicense>();
                        break;
                    case nameof(meta.licenseType):
                        meta.licenseType = reader.ReadStringEnum<LicenseType>();
                        break;
                    case nameof(meta.customLicenseUrl):
                        meta.customLicenseUrl = reader.ReadAsString();
                        break;
                }
            }
            return new BVA_metaExtension(meta);
        }
    }

    public class BVA_metaExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_meta";
        public const string EXTENSION_ELEMENT_NAME = "meta";
        public MetaId id;
        public BVA_metaExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_metaExtensionFactory(MetaId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_metaExtensionFactory() { id = id };
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
                id = new MetaId() { Id = _id, Root = root };
            }
            return new BVA_metaExtensionFactory(id);
        }
    }
}