using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GLTF.Extensions;
using UnityEngine;

namespace GLTF.Schema.BVA
{
    public enum CubemapImageType
    {
        Row,
        Column,
        Equirect,
        Unknown
    }
    public class BVA_texture_cubemapExtension : IExtension
    {
        public CubemapImageType imageType;
        public bool mipmap;
        public TextureId texture;
        public BVA_texture_cubemapExtension(TextureId textureInfo, CubemapImageType imageType = CubemapImageType.Row, bool mipmap = true)
        {
            this.texture = textureInfo;
            this.imageType = imageType;
            this.mipmap = mipmap;
        }

        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            propObj.Add(nameof(texture), texture.Id);
            propObj.Add(nameof(imageType), imageType.ToString());
            propObj.Add(nameof(mipmap), mipmap);
            JProperty jProperty = new JProperty(BVA_audio_audioClipExtensionFactory.EXTENSION_NAME, propObj);
            return jProperty;
        }

        public static BVA_texture_cubemapExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            int index = -1;
            CubemapImageType imageType = CubemapImageType.Unknown;
            bool mipmap = true;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(texture):
                        index = reader.ReadAsInt32().Value;
                        break;
                    case nameof(imageType):
                        imageType = reader.ReadStringEnum<CubemapImageType>();
                        break;
                    case nameof(mipmap):
                        mipmap = reader.ReadAsBoolean().Value;
                        break;

                }
            }
            if (imageType == CubemapImageType.Unknown || index < 0)
                throw new System.ArgumentNullException();
            return new BVA_texture_cubemapExtension(new TextureId() { Id = index, Root = root }, imageType, mipmap);
        }

        public IExtension Clone(GLTFRoot root)
        {
            throw new System.NotImplementedException();
        }
    }
    public class BVA_texture_cubemapExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "BVA_texture_cubemapExtension";
        public const string EXTENSION_ELEMENT_NAME = "cubemaps";
        public BVA_texture_cubemapExtensionFactory()
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
