using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using BVA;

namespace GLTF.Schema.BVA
{
    public class BVA_blendShape_blendShapeMixerExtension : IExtension
    {
        public List<BlendShapeKey> keys;
        private NodeCache _cache;

        public BVA_blendShape_blendShapeMixerExtension(List<BlendShapeKey> keys, NodeCache cache)
        {
            this.keys = keys;
            _cache = cache;
        }
        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new BVA_blendShape_blendShapeMixerExtension(keys, _cache);
        }

        public JProperty Serialize()
        {
            JArray ja = new JArray();
            foreach (var v in keys)
                ja.Add(v.Serialize(_cache));
            JProperty jProperty = new JProperty(BVA_blendShape_blendShapeMixerExtensionFactory.EXTENSION_NAME, new JObject( new JProperty(nameof(keys), ja)));
            return jProperty;
        }

        public static BVA_blendShape_blendShapeMixerExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            var keys = new List<BlendShapeKey>();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(keys):
                        keys = reader.ReadList(() => BlendShapeKey.Deserialize(root, reader));
                        break;
                }
            }

            return new BVA_blendShape_blendShapeMixerExtension(keys, null);
        }
    }

    public class BVA_blendShape_blendShapeMixerExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_blendShape_blendShapeMixer";
        public const string EXTENSION_ELEMENT_NAME = "mixer";
        public BlendshapeMixerId id;
        public BVA_blendShape_blendShapeMixerExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_blendShape_blendShapeMixerExtensionFactory(BlendshapeMixerId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_blendShape_blendShapeMixerExtensionFactory() { id = id };
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
                id = new BlendshapeMixerId() { Id = _id, Root = root };
            }
            return new BVA_blendShape_blendShapeMixerExtensionFactory(id);
        }
    }
}
