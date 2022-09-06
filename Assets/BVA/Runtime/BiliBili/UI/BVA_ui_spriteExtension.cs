using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using BVA.Extensions;

namespace GLTF.Schema.BVA
{
    public class BVA_ui_spriteExtension : IExtension
    {
        public string name;
        public TextureId texture;
        public Vector4 rect;
        public Vector2 pivot;
        public float pixelsPerUnit;
        public Vector4 border;
        public bool generateFallbackPhysicsShape;
        public BVA_ui_spriteExtension() { }
        public BVA_ui_spriteExtension(Sprite sprite, TextureId textureId)
        {
            name = sprite.name;
            texture = textureId;
            rect = rect = new Vector4(sprite.rect.x, sprite.rect.y, sprite.rect.width, sprite.rect.height);
            pivot = sprite.pivot;
            pixelsPerUnit = sprite.pixelsPerUnit;
            border = sprite.border;
            generateFallbackPhysicsShape = sprite.GetPhysicsShapeCount() > 0;
        }

        public IExtension Clone(GLTFRoot root)
        {
            return this;
        }

        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            if (name != null) propObj.Add(nameof(name), name);
            propObj.Add(nameof(texture), texture.Id);
            propObj.Add(nameof(rect), rect.ToJArray());
            propObj.Add(nameof(pivot), pivot.ToJArray());
            propObj.Add(nameof(pixelsPerUnit), pixelsPerUnit);
            propObj.Add(nameof(border), border.ToJArray());
            propObj.Add(nameof(generateFallbackPhysicsShape), generateFallbackPhysicsShape);

            JProperty jProperty = new JProperty(BVA_ui_spriteExtensionFactory.EXTENSION_NAME, propObj);
            return jProperty;
        }
        public static BVA_ui_spriteExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            var ret = new BVA_ui_spriteExtension();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(name):
                        ret.name = reader.ReadAsString();
                        break;
                    case nameof(texture):
                        ret.texture = new TextureId() { Id = reader.ReadAsInt32().Value, Root = root };
                        break;
                    case nameof(rect):
                        ret.rect = reader.ReadAsVector4();
                        break;
                    case nameof(pivot):
                        ret.pivot = reader.ReadAsVector2();
                        break;
                    case nameof(pixelsPerUnit):
                        ret.pixelsPerUnit = reader.ReadAsInt32().Value;
                        break;
                    case nameof(border):
                        ret.border = reader.ReadAsVector4();
                        break;
                    case nameof(generateFallbackPhysicsShape):
                        ret.generateFallbackPhysicsShape = reader.ReadAsBoolean().Value;
                        break;
                }
            }

            return ret;
        }
    }
    public class BVA_ui_spriteExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_ui_sprite";
        public const string EXTENSION_ELEMENT_NAME = "sprites";
        public BVA_ui_spriteExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_ui_spriteExtensionFactory(SpriteId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public SpriteId id;
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_ui_spriteExtensionFactory(id);
        }

        public JProperty Serialize()
        {
            return new JProperty(EXTENSION_NAME, new JObject(new JProperty(EXTENSION_ELEMENT_NAME, id)));
        }

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            return new BVA_ui_spriteExtensionFactory();
        }
    }
}