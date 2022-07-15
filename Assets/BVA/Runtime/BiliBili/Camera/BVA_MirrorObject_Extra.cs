using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BVA.Component;

namespace GLTF.Schema.BVA
{
    public class BVA_MirrorObject_Extra : IExtra
    {
        public const string PROPERTY = "BVA_MirrorObject_Extra";
        public int RenderTextureSize;
        public BVA_MirrorObject_Extra() { }

        public BVA_MirrorObject_Extra(MirrorObject target)
        {
            this.RenderTextureSize = target.RenderTextureSize;
        }
        public static void Deserialize(GLTFRoot root, JsonReader reader, MirrorObject target)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var curProp = reader.Value.ToString();
                    switch (curProp)
                    {
                        case nameof(BVA_MirrorObject_Extra.RenderTextureSize):
                            target.RenderTextureSize = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }
        }
        public JProperty Serialize()
        {
            JObject jo = new JObject();
            jo.Add(nameof(RenderTextureSize), RenderTextureSize);
            return new JProperty(BVA_MirrorObject_Extra.PROPERTY, jo);
        }
    }
}
