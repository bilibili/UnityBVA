using GLTF.Extensions;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{
    /// <summary>
    /// The core glTF 2.0 material model includes an emissiveFactor and an emissiveTexture to control the color and intensity of the light being emitted by the material,
    /// clamped to the range[0.0, 1.0]. However, in PBR environments with high-dynamic range reflections and lighting, stronger emission effects may be desirable.
    /// In this extension, a new emissiveStrength scalar factor is supplied, that governs the upper limit of emissive strength per material.
    /// Implementation Note: This strength can be colored and tempered using the core material's emissiveFactor and emissiveTexture controls
    /// permitting the strength to vary across the surface of the material. Supplying values above 1.0 for emissiveStrength can have an influence on reflections, tonemapping, blooming, and more.
    /// Spec can be found here:
    /// https://github.com/KhronosGroup/glTF/tree/main/extensions/2.0/Khronos/KHR_materials_emissive_strength/README.md
    /// </summary>
    public class KHR_materials_emissive_strengthExtension : IExtension
    {
        public static readonly float EMISSIVE_STRENGTH_DEFAULT = 1f;
        public float emissiveStrength;
        public KHR_materials_emissive_strengthExtension(float emissive)
        {
            emissiveStrength = emissive;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new KHR_materials_emissive_strengthExtension(emissiveStrength);
        }

        public JProperty Serialize()
        {
            var matProperty = new JObject();
            if (emissiveStrength != EMISSIVE_STRENGTH_DEFAULT)
                matProperty.Add(nameof(emissiveStrength), emissiveStrength);

            return new JProperty(KHR_materials_emissive_strengthExtensionFactory.EXTENSION_NAME, matProperty);
        }
    }

    public class KHR_materials_emissive_strengthExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "KHR_materials_emissive_strength";
        public KHR_materials_emissive_strengthExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
        }
        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            float emissiveStrength = KHR_materials_emissive_strengthExtension.EMISSIVE_STRENGTH_DEFAULT;

            if (extensionToken != null)
            {
                JToken clearcoatFactorToken = extensionToken.Value[nameof(emissiveStrength)];
                emissiveStrength = clearcoatFactorToken != null ? clearcoatFactorToken.DeserializeAsFloat() : emissiveStrength;
            }

            return new KHR_materials_emissive_strengthExtension(emissiveStrength);
        }
    }
}
