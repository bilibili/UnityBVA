using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GLTF.Schema.BVA
{
    public class BVA_UI_Button_Extra : IExtra
    {
        public const string PROPERTY = "BVA_UI_Button_Extra";
        public BVA_UI_Button_Extra() { }

        public BVA_UI_Button_Extra(UnityEngine.UI.Button target)
        {
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, UnityEngine.UI.Button target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();

                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            return new JProperty(BVA_UI_Button_Extra.PROPERTY, jo);
        }
    }
}
