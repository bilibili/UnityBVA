using GLTF.Extensions;
using GLTF.Math;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{
    /// <summary>
    /// This extension adds the ability to specify textures using KTX v2 images with Basis Universal supercompression.
    /// An implementation of this extension can use such images as an alternative to the PNG or JPEG images available in glTF 2.0 for more efficient asset transmission and reducing GPU memory footprint. 
    /// Furthermore, specifying mip map levels is possible.
    /// When the extension is used, it's allowed to use value image/ktx2 for the mimeType property of images that are referenced by the source property of KHR_texture_basisu texture extension object.
    /// At runtime, engines are expected to transcode a universal texture format into some block-compressed texture format supported by the platform.
    /// </summary>
    public class KHR_texture_basisuExtension : IExtension
    {
        /// <summary>
        /// The image source of the target texture
        /// </summary>
        public int Source = 0;
        public const int SOURCE_DEFAULT = 0;

        public KHR_texture_basisuExtension(int source = SOURCE_DEFAULT)
        {
            source = 0;
        }

        public IExtension Clone(GLTFRoot root)
        {
            return new KHR_texture_basisuExtension(Source);
        }

        public JProperty Serialize()
        {
            JObject ext = new JObject();
            ext.Add(new JProperty(KHR_texture_basisuExtensionFactory.SOURCE, Source));
            return new JProperty(KHR_texture_basisuExtensionFactory.EXTENSION_NAME, ext);
        }
    }
    public class KHR_texture_basisuExtensionFactory : ExtensionFactory
    {
        public const string SOURCE = "source";
        public const string EXTENSION_NAME = "KHR_texture_basisu";
        public KHR_texture_basisuExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            int source = KHR_texture_basisuExtension.SOURCE_DEFAULT;

            if (extensionToken != null)
            {
                JToken texCoordToken = extensionToken.Value[SOURCE];
                source = texCoordToken != null ? texCoordToken.DeserializeAsInt() : source;
            }

            return new KHR_texture_basisuExtension(source);
        }
    }
}
