using GLTF.Extensions;
using UnityEngine;
using GLTF.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{
    public struct GpuInstancingAttribute
    {
        public Vector3 TRANSLATION;
        public Quaternion ROTATION;
        public Vector3 SCALE;
    }
    public class EXT_mesh_gpu_instancingExtension : IExtension
    {
        public GpuInstancingAttribute attributes;
        public EXT_mesh_gpu_instancingExtension()
        {
            attributes = new GpuInstancingAttribute();
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new EXT_mesh_gpu_instancingExtension();
        }
        public JProperty Serialize()
        {
            JObject attributeInner = new JObject();
            JProperty propAttribute = new JProperty(nameof(attributes), attributeInner);
            JObject propObj = new JObject(propAttribute);

            attributeInner.Add("TRANSLATION", 0);
            attributeInner.Add("ROTATION", 1);
            attributeInner.Add("SCALE", 2);
            //attributeInner.Add("_ID", 3);
            JProperty jProperty = new JProperty(EXT_mesh_gpu_instancingExtensionFactory.EXTENSION_NAME, propObj);

            return jProperty;
        }

        public static EXT_mesh_gpu_instancingExtension Deserialize(GLTFRoot root, JsonReader reader)
        {
            GpuInstancingAttribute attribute=new GpuInstancingAttribute();
            while (reader.Read() && reader.TokenType == JsonToken.PropertyName)
            {
                var curProp = reader.Value.ToString();

                switch (curProp)
                {
                    case "TRANSLATION":
                        attribute.TRANSLATION = reader.ReadAsVector3();
                        break;
                    case "ROTATION":
                        attribute.ROTATION = reader.ReadAsQuaternion();
                        break;
                    case "SCALE":
                        attribute.SCALE = reader.ReadAsVector3();
                        break;
                }
            }

            return new EXT_mesh_gpu_instancingExtension();
        }
    }


    public class EXT_mesh_gpu_instancingExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "EXT_mesh_gpu_instancing";
        public EXT_mesh_gpu_instancingExtensionFactory() { ExtensionName = EXTENSION_NAME; }


        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            return null;
        }
    }
}
