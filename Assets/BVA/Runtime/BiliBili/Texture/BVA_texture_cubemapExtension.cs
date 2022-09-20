using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GLTF.Extensions;
using UnityEngine;
using System.Collections.Generic;

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
        public List<TextureId> textures;
        public BVA_texture_cubemapExtension(List<TextureId> textureInfo, CubemapImageType imageType = CubemapImageType.Row, bool mipmap = true)
        {
            this.textures = textureInfo;
            this.imageType = imageType;
            this.mipmap = mipmap;
        }

        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            JArray jTextures = new JArray();
            foreach (var v in textures)
                jTextures.Add(v.Id);
            propObj.Add(nameof(textures), jTextures);
            propObj.Add(nameof(imageType), imageType.ToString());
            propObj.Add(nameof(mipmap), mipmap);
            JProperty jProperty = new JProperty(BVA_audio_audioClipExtensionFactory.EXTENSION_NAME, propObj);
            return jProperty;
        }

        public static BVA_texture_cubemapExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            List<int> textureIds = null;
            CubemapImageType imageType = CubemapImageType.Unknown;
            bool mipmap = true;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(BVA_texture_cubemapExtension.textures):
                        textureIds = reader.ReadInt32List();
                        break;
                    case nameof(BVA_texture_cubemapExtension.imageType):
                        imageType = reader.ReadStringEnum<CubemapImageType>();
                        break;
                    case nameof(BVA_texture_cubemapExtension.mipmap):
                        mipmap = reader.ReadAsBoolean().Value;
                        break;

                }
            }
            if (imageType == CubemapImageType.Unknown || textureIds.Count == 0)
                throw new System.ArgumentNullException();

            List<TextureId> textures = new List<TextureId>();
            foreach (var v in textureIds)
            {
                textures.Add(new TextureId() { Root = root, Id = v });
            }
            return new BVA_texture_cubemapExtension(textures, imageType, mipmap);
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
