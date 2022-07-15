using System.Linq;
using System.Collections.Generic;
using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema.BVA
{
    public class BVA_variable_collectionExtension : IExtension
    {
        public string type;
        public List<int> collections;
        private BVA_variable_collectionExtension(List<int> collection, string t)
        {
            collections = collection;
            type = t;
        }
        public BVA_variable_collectionExtension(List<MaterialId> materials)
        {
            type = "material";
            collections = (from x in materials where x != null select x.Id).ToList();
        }
        public BVA_variable_collectionExtension(List<TextureId> textures)
        {
            type = "texture";
            collections = (from x in textures where x != null select x.Id).ToList();
        }
        public BVA_variable_collectionExtension(List<CubemapId> textures)
        {
            type = "cubemap";
            collections = (from x in textures where x != null select x.Id).ToList();
        }
        public BVA_variable_collectionExtension(List<AudioId> audios)
        {
            type = "audio";
            collections = (from x in audios where x != null select x.Id).ToList();
        }
        public BVA_variable_collectionExtension(List<MeshId> meshes)
        {
            type = "mesh";
            collections = (from x in meshes where x != null select x.Id).ToList();
        }
        public BVA_variable_collectionExtension(List<LightmapId> lightmaps)
        {
            type = "lightmap";
            collections = (from x in lightmaps where x != null select x.Id).ToList();
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_variable_collectionExtension(collections, type);
        }

        public JProperty Serialize()
        {
            JObject propObj = new JObject();
            propObj.Add(nameof(type), type.ToString());

            JArray ja = new JArray();
            foreach (var v in collections)
                ja.Add(v);

            propObj.Add(nameof(collections), ja);
            JProperty jProperty = new JProperty(BVA_collisions_colliderExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }
        public static BVA_variable_collectionExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            var _collections = new List<int>();
            string _type = null;
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case nameof(collections):
                        _collections = reader.ReadInt32List();
                        break;
                    case nameof(type):
                        _type = reader.ReadAsString();
                        break;
                }
            }

            return new BVA_variable_collectionExtension(_collections, _type);
        }
    }
    public class BVA_variable_collectionExtensionFactory : ExtensionFactory, IExtension
    {
        public const string EXTENSION_NAME = "BVA_variable_collection";
        public const string EXTENSION_ELEMENT_NAME = "variables";
        public VaribleCollectionId id;
        public BVA_variable_collectionExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
        }
        public BVA_variable_collectionExtensionFactory(VaribleCollectionId _id)
        {
            ExtensionName = EXTENSION_NAME;
            ElementName = EXTENSION_ELEMENT_NAME;
            id = _id;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return new BVA_variable_collectionExtensionFactory() { id = id };
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
                id = new VaribleCollectionId() { Id = _id, Root = root };
            }
            return new BVA_variable_collectionExtensionFactory(id);
        }
    }
}