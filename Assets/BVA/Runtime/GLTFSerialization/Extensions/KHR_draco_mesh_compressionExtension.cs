using GLTF.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GLTF.Schema
{
    public class KHR_draco_mesh_compressionExtension : IExtension
    {
        public Dictionary<string, AccessorId> attributes;
        public BufferViewId bufferView;

        public KHR_draco_mesh_compressionExtension() { }
        public KHR_draco_mesh_compressionExtension(Dictionary<string, AccessorId> attributes, BufferViewId bufferView)
        {
            this.attributes = attributes;
            this.bufferView = bufferView;
        }
        public IExtension Clone(GLTFRoot root)
        {
            return this;
        }

        public JProperty Serialize()
        {
            var dracoProperty = new JObject();
            dracoProperty.Add(nameof(bufferView), bufferView.Id);

            JObject attributeSerialize = new JObject();

            foreach (var attribute in attributes)
            {
                attributeSerialize.Add(attribute.Key, attribute.Value.Id);
            }
            dracoProperty.Add(nameof(attributes), attributeSerialize);
            return new JProperty(KHR_draco_mesh_compression_ExtensionFactory.EXTENSION_NAME, dracoProperty);
        }
    }

    public class KHR_draco_mesh_compression_ExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "KHR_draco_mesh_compression";

        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            KHR_draco_mesh_compressionExtension extension = null;
            if (extensionToken != null)
            {

                extension = new KHR_draco_mesh_compressionExtension();
                var reader = extensionToken.Value.CreateReader();
                reader.Read();
                while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();

                    switch (curProp)
                    {
                        case nameof(extension.attributes):
                            extension.attributes = reader.ReadAsDictionary(() => new AccessorId 
                            {
                                Id = reader.ReadAsInt32().Value,
                                Root = root
                            });
                            break;
                        case nameof(extension.bufferView):
                            extension.bufferView = BufferViewId.Deserialize(root, reader);
                            break;
                    }
                }
            }
            return extension;
        }

        public JProperty Serialize()
        {
            return new JProperty(EXTENSION_NAME, new JObject());
        }
    }
}