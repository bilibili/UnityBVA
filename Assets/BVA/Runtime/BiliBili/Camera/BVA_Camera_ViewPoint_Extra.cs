using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GLTF.Schema.BVA
{
    public class BVA_Camera_ViewPoint_Extra : IExtra
    {
        public const string PROPERTY = "BVA_CameraViewPoint_Extra";
        public BVA_Camera_ViewPoint_Extra() { }

        public BVA_Camera_ViewPoint_Extra(CameraViewPoint target)
        {
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, CameraViewPoint target)
        {

        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            return new JProperty(BVA_Camera_ViewPoint_Extra.PROPERTY, jo);
        }
    }
}
