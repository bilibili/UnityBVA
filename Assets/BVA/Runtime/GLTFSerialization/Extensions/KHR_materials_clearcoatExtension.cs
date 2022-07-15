using GLTF.Math;
using GLTF.Extensions;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{
    /// <summary>
    /// glTF extension that defines the specular-glossiness 
    /// material model from Physically-Based Rendering (PBR) methodology.
    /// 
    /// clearcoatFactor number  The clearcoat layer intensity.No, default: 0.0
    /// clearcoatTexture textureInfo The clearcoat layer intensity texture.No
    /// clearcoatRoughnessFactor number The clearcoat layer roughness.No, default: 0.0
    /// clearcoatRoughnessTexture textureInfo The clearcoat layer roughness texture.No
    /// clearcoatNormalTexture normalTextureInfo The clearcoat normal map texture.	No
    /// Spec can be found here:
    /// https://github.com/KhronosGroup/glTF/blob/main/extensions/2.0/Khronos/KHR_materials_clearcoat/README.md
    /// </summary>
    public class KHR_materials_clearcoatExtension : IExtension
    {
        public static readonly float CLEARCOAT_FACTOR_DEFAULT = 0f;
        public static readonly float CLEARCOAT_ROUGHNESS_FACTOR_DEFAULT = 0f;
        public float clearcoatFactor;
        public TextureInfo clearcoatTexture;
        public float clearcoatRoughnessFactor;
        public TextureInfo clearcoatRoughnessTexture;
        public TextureInfo clearcoatNormalTexture;
        public KHR_materials_clearcoatExtension(float clearcoatFactor, TextureInfo clearcoatTexture, float clearcoatRoughnessFactor, TextureInfo clearcoatRoughnessTexture, TextureInfo clearcoatNormalTexture)
        {
            this.clearcoatFactor = clearcoatFactor;
            this.clearcoatTexture = clearcoatTexture;
            this.clearcoatRoughnessFactor = clearcoatRoughnessFactor;
            this.clearcoatRoughnessTexture = clearcoatRoughnessTexture;
            this.clearcoatNormalTexture = clearcoatNormalTexture;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new KHR_materials_clearcoatExtension(clearcoatFactor, clearcoatTexture, clearcoatRoughnessFactor, clearcoatRoughnessTexture, clearcoatNormalTexture);
        }

        public JProperty Serialize()
        {
            var matProperty = new JObject();
            if (clearcoatFactor != CLEARCOAT_FACTOR_DEFAULT)
                matProperty.Add(nameof(clearcoatFactor), clearcoatFactor);
            if (clearcoatTexture != null)
                matProperty.Add(nameof(clearcoatTexture), new JObject(new JProperty(TextureInfo.INDEX, clearcoatTexture?.Index.Id)));
            if (clearcoatRoughnessFactor != CLEARCOAT_ROUGHNESS_FACTOR_DEFAULT)
                matProperty.Add(nameof(clearcoatRoughnessFactor), clearcoatRoughnessFactor);
            if (clearcoatRoughnessTexture != null)
                matProperty.Add(nameof(clearcoatRoughnessTexture), new JObject(new JProperty(TextureInfo.INDEX, clearcoatRoughnessTexture?.Index.Id)));
            if (clearcoatNormalTexture != null)
                matProperty.Add(nameof(clearcoatNormalTexture), new JObject(new JProperty(TextureInfo.INDEX, clearcoatNormalTexture?.Index.Id)));

            return new JProperty(KHR_materials_clearcoatExtensionFactory.EXTENSION_NAME, matProperty);
        }
    }
    public class KHR_materials_clearcoatExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "KHR_materials_clearcoat";
        public KHR_materials_clearcoatExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
        }
        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            float clearcoatFactor = 0;
            TextureInfo clearcoatTexture = new TextureInfo();
            float clearcoatRoughnessFactor = 0;
            TextureInfo clearcoatRoughnessTexture = new TextureInfo();
            TextureInfo clearcoatNormalTexture = new TextureInfo();
            TextureInfo specularGlossinessTextureInfo = new TextureInfo();

            if (extensionToken != null)
            {
                JToken clearcoatFactorToken = extensionToken.Value[nameof(clearcoatFactor)];
                clearcoatFactor = clearcoatFactorToken != null ? clearcoatFactorToken.DeserializeAsFloat() : clearcoatFactor;
                clearcoatTexture = extensionToken.Value[nameof(clearcoatTexture)]?.DeserializeAsTexture(root);
                JToken clearcoatRoughnessFactorToken = extensionToken.Value[nameof(clearcoatRoughnessFactor)];
                clearcoatRoughnessFactor = clearcoatRoughnessFactorToken != null ? clearcoatRoughnessFactorToken.DeserializeAsFloat() : clearcoatRoughnessFactor;
                clearcoatRoughnessTexture = extensionToken.Value[nameof(clearcoatRoughnessTexture)]?.DeserializeAsTexture(root);
                clearcoatNormalTexture = extensionToken.Value[nameof(clearcoatNormalTexture)]?.DeserializeAsTexture(root);
            }

            return new KHR_materials_clearcoatExtension(clearcoatFactor, clearcoatTexture, clearcoatRoughnessFactor, clearcoatRoughnessTexture, clearcoatNormalTexture);

        }
    }
}
