using GLTF.Math;
using GLTF.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GLTF.Schema
{
    /// <summary>
    /// glTF extension that defines the specular-glossiness 
    /// material model from Physically-Based Rendering (PBR) methodology.
    /// 
    /// Spec can be found here:
    /// https://github.com/KhronosGroup/glTF/tree/master/extensions/Khronos/KHR_materials_unlit
    /// </summary>
    public class KHR_materials_unlitExtension : IExtension
    {

        public KHR_materials_unlitExtension()
        {
        }

        public IExtension Clone(GLTFRoot gltfRoot)
        {
            return new KHR_materials_unlitExtension();
        }

        public JProperty Serialize()
        {
            JProperty jProperty = new JProperty(KHR_materials_unlitExtensionFactory.EXTENSION_NAME,new JObject());

            return jProperty;
        }
    }
    public class KHR_materials_unlitExtensionFactory : ExtensionFactory
    {
        public const string EXTENSION_NAME = "KHR_materials_unlit";
        public KHR_materials_unlitExtensionFactory()
        {
            ExtensionName = EXTENSION_NAME;
        }
        public override IExtension Deserialize(GLTFRoot root, JProperty extensionToken)
        {
            return new KHR_materials_unlitExtension();
        }
    }
}
