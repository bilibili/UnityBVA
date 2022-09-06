using UnityEngine;
using GLTF.Extensions;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{
    /// <summary>
    /// glTF extension that defines the specular-glossiness 
    /// material model from Physically-Based Rendering (PBR) methodology.
    /// 
    /// Spec can be found here:
    /// https://github.com/KhronosGroup/glTF/tree/master/extensions/Khronos/KHR_materials_pbrSpecularGlossiness
    /// </summary>
    public class KHR_materials_pbrSpecularGlossinessExtension : IExtension
    {
        public static readonly Vector3 SPEC_FACTOR_DEFAULT = new Vector3(0.2f, 0.2f, 0.2f);
        public static readonly float GLOSS_FACTOR_DEFAULT = 0.5f;

        /// <summary>
        /// The RGBA components of the reflected diffuse color of the material. 
        /// Metals have a diffuse value of [0.0, 0.0, 0.0]. 
        /// The fourth component (A) is the alpha coverage of the material. 
        /// The <see cref="GLTFMaterial.AlphaMode"/> property specifies how alpha is interpreted. 
        /// The values are linear.
        /// </summary>
        public Color DiffuseFactor = Color.white;

        /// <summary>
        /// The diffuse texture. 
        /// This texture contains RGB(A) components of the reflected diffuse color of the material in sRGB color space. 
        /// If the fourth component (A) is present, it represents the alpha coverage of the 
        /// material. Otherwise, an alpha of 1.0 is assumed. 
        /// The <see cref="GLTFMaterial.AlphaMode"/> property specifies how alpha is interpreted. 
        /// The stored texels must not be premultiplied.
        /// </summary>
        public TextureInfo DiffuseTexture;

        /// <summary>
        /// The specular RGB color of the material. This value is linear
        /// </summary>
        public Vector3 SpecularFactor = SPEC_FACTOR_DEFAULT;

        /// <summary>
        /// The glossiness or smoothness of the material. 
        /// A value of 1.0 means the material has full glossiness or is perfectly smooth. 
        /// A value of 0.0 means the material has no glossiness or is completely rough. 
        /// This value is linear.
        /// </summary>
        public float GlossinessFactor = GLOSS_FACTOR_DEFAULT;

        /// <summary>
        /// The specular-glossiness texture is RGBA texture, containing the specular color of the material (RGB components) and its glossiness (A component). 
        /// The values are in sRGB space.
        /// </summary>
        public TextureInfo SpecularGlossinessTexture;

        public KHR_materials_pbrSpecularGlossinessExtension(Color diffuseFactor, TextureInfo diffuseTexture, Vector3 specularFactor, float glossinessFactor, TextureInfo specularGlossinessTexture)
        {
            DiffuseFactor = diffuseFactor;
            DiffuseTexture = diffuseTexture;
            SpecularFactor = specularFactor;
            GlossinessFactor = glossinessFactor;
            SpecularGlossinessTexture = specularGlossinessTexture;
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new KHR_materials_pbrSpecularGlossinessExtension(
                DiffuseFactor,
                new TextureInfo(
                    DiffuseTexture,
                    gltfRoot
                    ),
                SpecularFactor,
                GlossinessFactor,
                new TextureInfo(
                    SpecularGlossinessTexture,
                    gltfRoot
                    )
                );
        }

        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(KHR_materials_pbrSpecularGlossinessExtensionFactory.DIFFUSE_FACTOR, new JArray(DiffuseFactor.r, DiffuseFactor.g, DiffuseFactor.b, DiffuseFactor.a));
            if (DiffuseTexture != null && DiffuseTexture.Index != null && DiffuseTexture.Index.Id >= 0)
                jo.Add(KHR_materials_pbrSpecularGlossinessExtensionFactory.DIFFUSE_TEXTURE, new JObject(new JProperty(TextureInfo.INDEX, DiffuseTexture?.Index.Id)));
            jo.Add(KHR_materials_pbrSpecularGlossinessExtensionFactory.SPECULAR_FACTOR, new JArray(SpecularFactor.x, SpecularFactor.y, SpecularFactor.z));
            jo.Add(KHR_materials_pbrSpecularGlossinessExtensionFactory.GLOSSINESS_FACTOR, GlossinessFactor);
            if (SpecularGlossinessTexture != null && SpecularGlossinessTexture.Index != null && SpecularGlossinessTexture.Index.Id >= 0)
                jo.Add(KHR_materials_pbrSpecularGlossinessExtensionFactory.SPECULAR_GLOSSINESS_TEXTURE, new JObject(new JProperty(TextureInfo.INDEX, SpecularGlossinessTexture?.Index.Id)));
            return new JProperty(KHR_materials_pbrSpecularGlossinessExtensionFactory.EXTENSION_NAME, jo); ;
        }
    }
    public class KHR_materials_pbrSpecularGlossinessExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "KHR_materials_pbrSpecularGlossiness";
        public const string DIFFUSE_FACTOR = "diffuseFactor";
        public const string DIFFUSE_TEXTURE = "diffuseTexture";
        public const string SPECULAR_FACTOR = "specularFactor";
        public const string GLOSSINESS_FACTOR = "glossinessFactor";
        public const string SPECULAR_GLOSSINESS_TEXTURE = "specularGlossinessTexture";

        public KHR_materials_pbrSpecularGlossinessExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            Color diffuseFactor = Color.white;
            TextureInfo diffuseTextureInfo = new TextureInfo();
            Vector3 specularFactor = KHR_materials_pbrSpecularGlossinessExtension.SPEC_FACTOR_DEFAULT;
            float glossinessFactor = KHR_materials_pbrSpecularGlossinessExtension.GLOSS_FACTOR_DEFAULT;
            TextureInfo specularGlossinessTextureInfo = new TextureInfo();

            if (extensionToken != null)
            {
                JToken diffuseFactorToken = extensionToken.Value[DIFFUSE_FACTOR];
                diffuseFactor = diffuseFactorToken != null ? diffuseFactorToken.DeserializeAsColor() : diffuseFactor;
                diffuseTextureInfo = extensionToken.Value[DIFFUSE_TEXTURE]?.DeserializeAsTexture(root);
                JToken specularFactorToken = extensionToken.Value[SPECULAR_FACTOR];
                specularFactor = specularFactorToken != null ? specularFactorToken.DeserializeAsVector3() : specularFactor;
                JToken glossinessFactorToken = extensionToken.Value[GLOSSINESS_FACTOR];
                glossinessFactor = glossinessFactorToken != null ? glossinessFactorToken.DeserializeAsFloat() : glossinessFactor;
                specularGlossinessTextureInfo = extensionToken.Value[SPECULAR_GLOSSINESS_TEXTURE]?.DeserializeAsTexture(root);
            }

            return new KHR_materials_pbrSpecularGlossinessExtension(diffuseFactor, diffuseTextureInfo, specularFactor, glossinessFactor, specularGlossinessTextureInfo);
        }
    }
}
